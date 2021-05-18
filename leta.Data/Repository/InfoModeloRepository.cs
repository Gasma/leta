using leta.Data.Entities;

namespace leta.Data.Repository
{
    public class InfoModeloRepository : BaseRepository<InfoModelo, int>, IInfoModeloRepository
    {
        public InfoModeloRepository(LetaAppDbContext context) : base(context)
        {
        }
    }
}
