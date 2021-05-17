using leta.Application.Helper;
using leta.Application.MLModels;
using leta.Application.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using System;
using System.IO;

namespace leta.Application.RouteTimeModel
{
    public class RouteTimeModel : IRouteTimeModel
    {
        private Lazy<PredictionEngine<RouteTimeModelInput, RouteTimeModelOutput>> PredictionEngine;

        public string MLNetModelPath;
        public RouteTimeModel(IOptions<AppSettings> appSettings)
        {
            this.MLNetModelPath = Path.GetFullPath(appSettings.Value.TrainedModelPath);
        }

        // For more info on consuming ML.NET models, visit https://aka.ms/mlnet-consume
        // Method for consuming model in your app
        public RouteTimeModelOutput Predict(RouteTimeViewModel viewModel)
        {
            var input = new RouteTimeModelInput() {
                Ano = int.TryParse(viewModel.HoraDoDia.ToString("yy"), out int ano) ? ano : 0,
                DiaSemana = viewModel.DiaDaSemana,
                Hora = viewModel.HoraDoDia.Hour,
                Mes = int.TryParse(viewModel.HoraDoDia.ToString("MM"), out int mes) ? mes : 0,
                Tempo = viewModel.Tempo
            };
            PredictionEngine = new Lazy<PredictionEngine<RouteTimeModelInput, RouteTimeModelOutput>>(CreatePredictionEngine);
            RouteTimeModelOutput result = PredictionEngine.Value.Predict(input);
            return result;
        }

        public PredictionEngine<RouteTimeModelInput, RouteTimeModelOutput> CreatePredictionEngine()
        {
            // Create new MLContext
            MLContext mlContext = new MLContext();

            // Load model & create prediction engine
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<RouteTimeModelInput, RouteTimeModelOutput>(mlModel);

            return predEngine;
        }
    }
}
