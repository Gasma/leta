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
                Year = int.TryParse(a.TimeOfDay.ToString("yy"), out int ano) ? ano : 0,
                WeekDay = (int)a.TimeOfDay.DayOfWeek,
                Hour = a.TimeOfDay.Hour,
                Month = int.TryParse(a.TimeOfDay.ToString("MM"), out int mes) ? mes : 0,
                Time = a.Time
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
            var dataProcessPipeline = mlContext.Transforms.Categorical.OneHotHashEncoding(new[] { new InputOutputColumnPair("Month", "Month"), new InputOutputColumnPair("WeekDay", "WeekDay"), new InputOutputColumnPair("Year", "Year") })
                          .Append(mlContext.Transforms.Concatenate("Features", new[] { "Month", "WeekDay", "Year", "Hour" }));
            // Set the training algorithm 
            var trainer = mlContext.Regression.Trainers.FastTreeTweedie(labelColumnName: @"Time", featureColumnName: "Features");

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
            var crossValidationResults = mlContext.Regression.CrossValidate(trainingDataView, trainingPipeline, numberOfFolds: 5, labelColumnName: "Time");
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

            message.Append($"*       Erro médio absoluto:   {L1.Average():0.###} (ele é a magnitude media dos erros e mede a precisão para variáveis continuas)#");
            message.Append($"*       Erro quadrático médio: {L2.Average():0.###} (se refere a média da diferença quadrática entre Y (parâmetro predito) e as variáveis X (parâmetros observados).) #");
            message.Append($"*       Erro quadrático médio: {RMS.Average():0.###} (é o desvio padrão dos resíduos (erros de previsão).Resíduos são uma medida de quão longe os pontos de dados da linha de regressão estão. ) #");
            message.Append($"*       Funções de perda:      {lossFunction.Average():0.###} (é uma medida de quão bom é um modelo de previsão em termos de ser capaz de prever o resultado esperado) #");
            message.Append($"*       R-quadrado:            {R2.Average():0.###}  (é uma medida estatística de ajuste que indica quanta variação de uma variável dependente é explicada pelas variáveis independentes)#");
        }
    }
}
