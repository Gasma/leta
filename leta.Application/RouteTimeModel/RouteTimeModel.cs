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
            var input = new RouteTimeModelInput()
            {
                Ano = int.TryParse(viewModel.HoraDoDia.ToString("yy"), out int ano) ? ano : 0,
                DiaSemana = (int)viewModel.HoraDoDia.DayOfWeek,
                Hora = viewModel.HoraDoDia.Hour,
                Mes = int.TryParse(viewModel.HoraDoDia.ToString("MM"), out int mes) ? mes : 0,
                Tempo = viewModel.Tempo
            };
            RouteTimeModelOutput result;
            try
            {
                PredictionEngine = new Lazy<PredictionEngine<RouteTimeModelInput, RouteTimeModelOutput>>(CreatePredictionEngine);
                result = PredictionEngine.Value.Predict(input);
            }
            catch (NullReferenceException)
            {
                result = new RouteTimeModelOutput() { Score = 0 };
            }
            return result;
        }

        public PredictionEngine<RouteTimeModelInput, RouteTimeModelOutput> CreatePredictionEngine()
        {
            // Create new MLContext
            MLContext mlContext = new MLContext();
            PredictionEngine<RouteTimeModelInput, RouteTimeModelOutput> predEngine = null;
            try
            {
                // Load model & create prediction engine
                ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var modelInputSchema);
                predEngine = mlContext.Model.CreatePredictionEngine<RouteTimeModelInput, RouteTimeModelOutput>(mlModel);

            }
            catch { }
            return predEngine;
        }
    }
}
