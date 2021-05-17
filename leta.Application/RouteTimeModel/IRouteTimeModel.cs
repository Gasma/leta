using leta.Application.MLModels;
using leta.Application.ViewModels;

namespace leta.Application.RouteTimeModel
{
    public interface IRouteTimeModel
    {
        RouteTimeModelOutput Predict(RouteTimeViewModel input);
    }
}
