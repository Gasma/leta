using leta.Data.Repository;
using leta.Data.UoW;
using leta.webApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
