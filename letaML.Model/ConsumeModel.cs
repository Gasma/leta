// This file was auto-generated by ML.NET Model Builder. 

using Microsoft.ML;
using System;
using System.IO;

namespace leta.Model
{
    public class ConsumeModel
    {
        private static Lazy<PredictionEngine<RouteTime, ModelOutput>> PredictionEngine = new Lazy<PredictionEngine<RouteTime, ModelOutput>>(CreatePredictionEngine);

        public static string MLNetModelPath = Path.GetFullPath("MLModel.zip");

        // For more info on consuming ML.NET models, visit https://aka.ms/mlnet-consume
        // Method for consuming model in your app
        public static ModelOutput Predict(RouteTime input)
        {
            ModelOutput result = PredictionEngine.Value.Predict(input);
            return result;
        }

        public static PredictionEngine<RouteTime, ModelOutput> CreatePredictionEngine()
        {
            // Create new MLContext
            MLContext mlContext = new MLContext();

            // Load model & create prediction engine
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var modelInputSchema);

            var predEngine = mlContext.Model.CreatePredictionEngine<RouteTime, ModelOutput>(mlModel);

            return predEngine;
        }
    }
}