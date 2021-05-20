using leta.Application.Helper;
using leta.Application.MLModels;
using leta.Data.Repository;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace leta.Application.RouteTimeTrainModel
{
    public class RouteTimeModelBuilder : IRouteTimeModelBuilder
    {
        private readonly IRouteTimeRepository routeTimeRepository;
        private readonly string MODEL_FILE;
        private MLContext mlContext;
        private StringBuilder message;

        public RouteTimeModelBuilder(IRouteTimeRepository routeTimeRepository, IOptions<AppSettings> appSettings)
        {
            mlContext = new MLContext(seed: 1);
            MODEL_FILE = Path.GetFullPath(appSettings.Value.TrainedModelPath);
            this.routeTimeRepository = routeTimeRepository;
            message = new StringBuilder();
        }
        public string CreateModel()
        {
            message.Clear();
            var dados = routeTimeRepository.GetAll();
            var dataset = routeTimeRepository.GetAll().Select(a => new RouteTimeModelInput()
            {
                Ano = int.TryParse(a.HoraDoDia.ToString("yy"), out int ano) ? ano : 0,
                DiaSemana = (int)a.HoraDoDia.DayOfWeek,
                Hora = a.HoraDoDia.Hour,
                Mes = int.TryParse(a.HoraDoDia.ToString("MM"), out int mes) ? mes : 0,
                Tempo = a.Tempo
            });
            // Load Data
            IDataView trainingDataView = mlContext.Data.LoadFromEnumerable(dataset);

            // Build training pipeline
            IEstimator<ITransformer> trainingPipeline = BuildTrainingPipeline(mlContext);

            // Train Model
            ITransformer mlModel = TrainModel(mlContext, trainingDataView, trainingPipeline);

            // Evaluate quality of Model
            Evaluate(mlContext, trainingDataView, trainingPipeline);

            // Save model
            SaveModel(mlContext, mlModel, MODEL_FILE, trainingDataView.Schema);

            return message.ToString();
        }

        public IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations 
            var dataProcessPipeline = mlContext.Transforms.Concatenate("Features", new[] { "Ano", "DiaSemana", "Hora", "Mes" });
            // Set the training algorithm 
            var trainer = mlContext.Regression.Trainers.FastTreeTweedie(labelColumnName: @"Tempo", featureColumnName: "Features");

            return dataProcessPipeline.Append(trainer);
        }

        public ITransformer TrainModel(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> trainingPipeline)
        {
            return trainingPipeline.Fit(trainingDataView);
        }

        private void Evaluate(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> trainingPipeline)
        {
            // Cross-Validate with single dataset (since we don't have two datasets, one for training and for evaluate)
            // in order to evaluate and get the model's accuracy metrics
            var crossValidationResults = mlContext.Regression.CrossValidate(trainingDataView, trainingPipeline, numberOfFolds: 5, labelColumnName: "Tempo");
            PrintRegressionFoldsAverageMetrics(crossValidationResults);
        }

        private void SaveModel(MLContext mlContext, ITransformer mlModel, string modelRelativePath, DataViewSchema modelInputSchema)
        {
            // Save/persist the trained model to a .ZIP file
            mlContext.Model.Save(mlModel, modelInputSchema, GetAbsolutePath(modelRelativePath));
        }

        public string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(RouteTimeModelBuilder).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }

        public void PrintRegressionMetrics(RegressionMetrics metrics)
        {
            message.Append($"*************************************************#");
            message.Append($"*       Metrics for Regression model      #");
            message.Append($"*------------------------------------------------#");
            message.Append($"*       LossFn:        {metrics.LossFunction:0.##}#");
            message.Append($"*       R2 Score:      {metrics.RSquared:0.##}#");
            message.Append($"*       Absolute loss: {metrics.MeanAbsoluteError:#.##}#");
            message.Append($"*       Squared loss:  {metrics.MeanSquaredError:#.##}#");
            message.Append($"*       RMS loss:      {metrics.RootMeanSquaredError:#.##}#");
            message.Append($"*************************************************#");
        }

        public void PrintRegressionFoldsAverageMetrics(IEnumerable<TrainCatalogBase.CrossValidationResult<RegressionMetrics>> crossValidationResults)
        {
            var L1 = crossValidationResults.Select(r => r.Metrics.MeanAbsoluteError);
            var L2 = crossValidationResults.Select(r => r.Metrics.MeanSquaredError);
            var RMS = crossValidationResults.Select(r => r.Metrics.RootMeanSquaredError);
            var lossFunction = crossValidationResults.Select(r => r.Metrics.LossFunction);
            var R2 = crossValidationResults.Select(r => r.Metrics.RSquared);

            message.Append($"*       Metrics for Regression model      #");
            message.Append($"*       Average L1 Loss:       {L1.Average():0.###} #");
            message.Append($"*       Average L2 Loss:       {L2.Average():0.###}  #");
            message.Append($"*       Average RMS:           {RMS.Average():0.###}  #");
            message.Append($"*       Average Loss Function: {lossFunction.Average():0.###}  #");
            message.Append($"*       Average R-squared:     {R2.Average():0.###}  #");
        }
    }
}
