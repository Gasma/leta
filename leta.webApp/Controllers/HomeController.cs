using leta.Data.Repository;
using leta.Data.UoW;
using leta.webApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            var dados = routeTimeRepository.GetAll().ToList();

            return Json(new { data = dados });
            //return Json(new { draw = 1, recordsTotal = dados.Count(), recordsFiltered = dados.Count(), data = dados });
        }

        [HttpPost]
        public JsonResult SalvaUnicoRegistro(RouteTimeViewModel route)
        {
            routeTimeRepository.Insert(new Data.RouteTime()
            {
                DiaDaSemana = route.DiaDaSemana,
                HoraDoDia = route.HoraDoDia,
                Tempo = route.Tempo
            });
            if (unitOfWork.Commit())
                return Json(new { sucess = true });
            else
                return Json(new { sucess = false });
        }

        [HttpPost]
        public JsonResult SalvaArquivo(IFormFile Arquivo)
        {
            using (var csvreader = new StreamReader(Arquivo.OpenReadStream()))
                while (!csvreader.EndOfStream)
                {
                    var line = csvreader.ReadLine();
                    var values = line.Split(';');
                    if (int.TryParse(values[2], out int tempo) && DateTime.TryParse(values[0], out DateTime data))
                    {
                        routeTimeRepository.Insert(new Data.RouteTime()
                        {
                            HoraDoDia = data,
                            DiaDaSemana = values[1],
                            Tempo = tempo,
                        });
                    }
                }
            if (unitOfWork.Commit())
                return Json(new { sucess = true });
            else
                return Json(new { sucess = false });
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
