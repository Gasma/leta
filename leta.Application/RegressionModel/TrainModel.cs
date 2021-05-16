using leta.Application.ViewModels;
using leta.Data.Repository;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using System.Linq;

namespace leta.Application
{
    public class TrainModel
    {
        private readonly IRouteTimeRepository routeTimeRepository;
        public TrainModel(IRouteTimeRepository routeTimeRepository)
        {
            this.routeTimeRepository = routeTimeRepository;
        }
        public ModelOutput Treinamento()
        {
            var dataset = routeTimeRepository.GetAll().Select(a => new RouteTimeViewModel() 
            { 
                Id = a.Id,
                DiaDaSemana = a.DiaDaSemana,
                HoraDoDia = a.HoraDoDia,
                Tempo = a.Tempo
            });
            var context = new MLContext();
            var data = context.Data.LoadFromEnumerable(dataset);
            var pipeline = context.Forecasting.ForecastBySsa(
                "TempoTotal", 
                nameof(RouteTimeViewModel.Tempo), 
                windowSize: 5,
                seriesLength: 10,
                trainSize:100,
                horizon:4);
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
