using leta.Application.MLModels;

namespace leta.Application.RouteTimeModel
{
    public interface IRouteTimeModel
    {
        RouteTimeModelOutput Predict(RouteTimeModelInput input);
    }
}
