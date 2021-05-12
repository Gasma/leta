using leta.Data.Repository;
using leta.Data.UoW;
using leta.webApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace leta.webApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IRouteTimeRepository routeTimeRepository;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger,
            IRouteTimeRepository routeTimeRepository,
            IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.routeTimeRepository = routeTimeRepository;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult AddData()
        {
            return View();
        }

        public JsonResult PopulaTabela()
        {
            var dados = routeTimeRepository
                .GetAll()
                .Select(a => new { a.Id, HoraDoDia = a.HoraDoDia.ToString("dd/MM/yyyy HH:mm"), DiaDaSemana = ((DiaSemana)a.DiaDaSemana).ToDescription(), a.Tempo });

            return Json(new { data = dados });
        }

        [HttpPost]
        public JsonResult SalvaUnicoRegistro(RouteTimeViewModel route)
        {
            if (route.Id <= 0)
            {
                routeTimeRepository.Insert(new Data.RouteTime()
                {
                    DiaDaSemana = (int)route.DiaDaSemana.ParseToEnumDiaSemana(),
                    HoraDoDia = route.HoraDoDia,
                    Tempo = route.Tempo
                });
            }
            else
            {
                routeTimeRepository.Update(new Data.RouteTime()
                {
                    Id = route.Id,
                    DiaDaSemana = (int)route.DiaDaSemana.ParseToEnumDiaSemana(),
                    HoraDoDia = route.HoraDoDia,
                    Tempo = route.Tempo
                });
            }

            if (unitOfWork.Commit())
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult ApagarRegistro(int id)
        {
            routeTimeRepository.Delete(id);
            if (unitOfWork.Commit())
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }        
        
        [HttpPost]
        public JsonResult ApagarTodosRegistro()
        {
            var dados = routeTimeRepository.GetAll();
            foreach (var item in dados)
            {
                routeTimeRepository.Delete(item.Id);
            }
            if (unitOfWork.Commit())
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult SalvaArquivo(IFormFile Arquivo)
        {
            using (var csvreader = new StreamReader(Arquivo.OpenReadStream()))
                while (!csvreader.EndOfStream)
                {
                    var linha = csvreader.ReadLine();
                    var values = linha.Split(';');
                    if (values.Length <= 1)
                    {
                        values = linha.Split(',');
                    }
                    try
                    {
                        values[0] = JsonConvert.DeserializeObject<string>(values[0]);
                        values[1] = JsonConvert.DeserializeObject<string>(values[1]);
                        values[2] = JsonConvert.DeserializeObject<string>(values[2]);
                    }
                    catch { }

                    if (int.TryParse(values[2], out int tempo) && DateTime.TryParse(values[0], out DateTime data))
                    {
                        routeTimeRepository.Insert(new Data.RouteTime()
                        {
                            HoraDoDia = data,
                            DiaDaSemana = (int)values[1].ParseToEnumDiaSemana(),
                            Tempo = tempo,
                        });
                    }
                }
            if (unitOfWork.Commit())
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }

        public IActionResult Predicao()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
