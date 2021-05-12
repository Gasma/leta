namespace leta.Data.Repository
{
    public class RouteTimeRepository : BaseRepository<RouteTime, int>, IRouteTimeRepository
    {
        public RouteTimeRepository(LetaAppDbContext context) : base(context)
        {
        }
    }
}
