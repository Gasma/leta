using System.Threading.Tasks;

namespace leta.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LetaAppDbContext context;

        public UnitOfWork(LetaAppDbContext context)
        {
            this.context = context;
        }

        public bool Commit()
        {
            return context.SaveChanges() > 0;
        }

        public Task<bool> CommitAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                return Commit();
            });
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
