using leta.Application.Helper;
using leta.Application.MLModels;
using leta.Application.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using System;
using System.IO;

namespace leta.Application.RouteTimeTrainModel
{
    public class RouteTimeConsumer : IRouteTimeConsumer
    {
        private Lazy<PredictionEngine<RouteTimeModelInput, RouteTimeModelOutput>> PredictionEngine;

        public string MLNetModelPath;
        public RouteTimeConsumer(IOptions<AppSettings> appSettings)
        {
            this.MLNetModelPath = Path.GetFullPath(appSettings.Value.TrainedModelPath);
        }

        // For more info on consuming ML.NET models, visit https://aka.ms/mlnet-consume
        // Method for consuming model in your app
        public RouteTimeModelOutput Predict(RouteTimeViewModel viewModel)
        {
            var input = new RouteTimeModelInput()
            {
                Year = int.TryParse(viewModel.HoraDoDia.ToString("yy"), out int ano) ? ano : 0,
                WeekDay = (int)viewModel.HoraDoDia.DayOfWeek,
                Hour = viewModel.HoraDoDia.Hour,
                Month = int.TryParse(viewModel.HoraDoDia.ToString("MM"), out int mes) ? mes : 0,
                Time = viewModel.Tempo
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
