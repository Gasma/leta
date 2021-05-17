using leta.Application.MLModels;
using Microsoft.ML;
using System;
using System.IO;

namespace leta.Application.RouteTimeModel
{
    public class RouteTimeModel : IRouteTimeModel
    {
        private Lazy<PredictionEngine<RouteTimeModelInput, RouteTimeModelOutput>> PredictionEngine;

        public string MLNetModelPath;
        public RouteTimeModel()
        {
            this.MLNetModelPath = Path.GetFullPath("MLModel.zip");
        }

        // For more info on consuming ML.NET models, visit https://aka.ms/mlnet-consume
        // Method for consuming model in your app
        public RouteTimeModelOutput Predict(RouteTimeModelInput input)
        {
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
