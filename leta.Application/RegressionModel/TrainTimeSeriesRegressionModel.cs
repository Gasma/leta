using leta.Application.ViewModels;
using leta.Data.Repository;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using System.Linq;

namespace leta.Application.RegressionModel
{
    public class TrainTimeSeriesRegressionModel : ITrainTimeSeriesRegressionModel
    {
        private readonly IRouteTimeRepository routeTimeRepository;
        public TrainTimeSeriesRegressionModel(IRouteTimeRepository routeTimeRepository)
        {
            this.routeTimeRepository = routeTimeRepository;
        }
        public ModelOutput Treinamento()
        {
            var dados = routeTimeRepository.GetAll();
            var dataset = routeTimeRepository.GetAll().Select(a => new RouteTimeViewModel() 
            { 
                DiaDaSemana = a.DiaDaSemana,
                HoraDoDia = a.HoraDoDia,
                Tempo = a.Tempo
            });
            var context = new MLContext();
            var data = context.Data.LoadFromEnumerable(dataset);
            var pipeline = context.Forecasting.ForecastBySsa(
                "MediaDeTempo", 
                nameof(RouteTimeViewModel.Tempo), 
                windowSize: 2,
                seriesLength: 7,
                trainSize: dataset.Count(),
                horizon:1);
            var model = pipeline.Fit(data);
            var forecastingTempo = model.CreateTimeSeriesEngine<RouteTimeViewModel, ModelOutput>(context);
            var tempos = forecastingTempo.Predict();

            return tempos;
            //foreach (var item in tempos.MediaDeTempo)
            //{

            //}
        }
    }
}
