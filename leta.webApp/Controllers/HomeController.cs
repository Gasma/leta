using leta.Application.Helper;
using leta.Application.RouteTimeTrainModel;
using leta.Application.ViewModels;
using leta.Data.Repository;
using leta.Data.UoW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IRouteTimeConsumer routeTimeModel;
        private readonly IRouteTimeModelBuilder consumeModelBuilder;
        private readonly IInfoModeloRepository infoModeloRepository;
        private string pathModeloTreinado;
        private int limiteMinimoTreino;

        public HomeController(ILogger<HomeController> logger,
            IRouteTimeRepository routeTimeRepository,
            IUnitOfWork unitOfWork,
            IRouteTimeConsumer routeTimeModel,
            IRouteTimeModelBuilder consumeModelBuilder,
            IInfoModeloRepository infoModeloRepository,
            IOptions<AppSettings> appSettings)
        {
            this.logger = logger;
            this.routeTimeRepository = routeTimeRepository;
            this.infoModeloRepository = infoModeloRepository;
            this.unitOfWork = unitOfWork;
            this.routeTimeModel = routeTimeModel;
            this.consumeModelBuilder = consumeModelBuilder;
            pathModeloTreinado = Path.GetFullPath(appSettings.Value.TrainedModelPath);
            limiteMinimoTreino = appSettings.Value.MinimalTrainLimit;
        }

        #region Adiciona Dados
        public IActionResult AddData()
        {
            return View();
        }

        public JsonResult PopulaTabela()
        {
            var dados = routeTimeRepository
                .GetAll()
                .Select(a => new { a.Id, HoraDoDia = a.TimeOfDay.ToString("dd/MM/yyyy HH:mm"), DiaDaSemana = ((DiaSemana)a.TimeOfDay.DayOfWeek).ToDescription(), Tempo = a.Time });

            return Json(new { data = dados });
        }

        [HttpPost]
        public JsonResult SalvaUnicoRegistro(RouteTimeViewModel route)
        {
            if (route.Id <= 0)
            {
                routeTimeRepository.Insert(new Data.RouteTime()
                {
                    TimeOfDay = route.HoraDoDia,
                    Time = route.Tempo
                });
            }
            else
            {
                routeTimeRepository.Update(new Data.RouteTime()
                {
                    Id = route.Id,
                    TimeOfDay = route.HoraDoDia,
                    Time = route.Tempo
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
                            TimeOfDay = data,
                            Time = tempo,
                        });
                    }
                }
            if (unitOfWork.Commit())
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        #endregion

        #region Predição
        public IActionResult Predicao()
        {
            var qtdAuxiliar = routeTimeRepository.GetAll().Count();
            ViewBag.Qtd = (qtdAuxiliar >= limiteMinimoTreino) ? qtdAuxiliar : 0;
            ViewBag.LimiteMinimoTreino = limiteMinimoTreino;
            ViewBag.ModelExists = System.IO.File.Exists(pathModeloTreinado);
            var model = infoModeloRepository.GetAll().FirstOrDefault();
            if (model != null)
            {
                ViewBag.DataSet = model.QuantData;
                ViewBag.TreinarModel = model.Message;
            }
            else
            {
                ViewBag.DataSet = 0;
                ViewBag.TreinarModel = string.Empty;
            }
            return View();
        }        
        
        public JsonResult ApagarModel()
        {
            System.IO.File.Delete(pathModeloTreinado);
            var model = infoModeloRepository.GetAll().FirstOrDefault();
            infoModeloRepository.Delete(model);
            if (unitOfWork.Commit())
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult TreinarModel()
        {
            var message = consumeModelBuilder.CreateModel();
            var qtd = routeTimeRepository.GetAll().Count();
            var novoModel = new Data.Entities.InfoModel()
            {
                Message = message,
                QuantData = qtd,
                LastTraining = DateTime.Now
            };
            var model = infoModeloRepository.GetAll().FirstOrDefault();
            if (model != null)
            {
                model.Update(novoModel);
                infoModeloRepository.Update(model);
            }
            else
            {
                infoModeloRepository.Insert(novoModel);
            }

            if (unitOfWork.Commit() && !string.IsNullOrEmpty(message))
                return Json(new { success = true, message = message, qtd = qtd });
            else
                return Json(new { success = false, message = "Ocorreu algum problema durante a tentativa de Treinar o Modelo." });
        }

        [HttpPost]
        public JsonResult ConsumirModel(RouteTimeViewModel route)
        {
            return Json(new { success = true, message = Math.Truncate(routeTimeModel.Predict(route).Score) });
        }
        #endregion
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
