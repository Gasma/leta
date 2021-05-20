using leta.Application.MLModels;
using leta.Application.ViewModels;

namespace leta.Application.RouteTimeTrainModel
{
    public interface IRouteTimeConsumer
    {
        RouteTimeModelOutput Predict(RouteTimeViewModel input);
    }
}
