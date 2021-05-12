using leta.Data.Repository;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leta.RegressionModule
{
    public class TrainModel
    {
        private readonly IRouteTimeRepository routeTimeRepository;
        public TrainModel(IRouteTimeRepository routeTimeRepository)
        {
            this.routeTimeRepository = routeTimeRepository;
        }
        public void Treinamento()
        {
            MLContext mlContext = new MLContext();
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<RouteTime>();
            IDataView dataView = loader.Load(dbSource);
        }
    }
}
