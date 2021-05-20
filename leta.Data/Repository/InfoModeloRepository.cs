using leta.Data.Entities;

namespace leta.Data.Repository
{
    public class InfoModeloRepository : BaseRepository<InfoModel, int>, IInfoModeloRepository
    {
        public InfoModeloRepository(LetaAppDbContext context) : base(context)
        {
        }
    }
}
