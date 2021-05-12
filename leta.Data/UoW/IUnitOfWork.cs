using System;
using System.Threading.Tasks;

namespace leta.Data.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
        Task<bool> CommitAsync();
    }
}
