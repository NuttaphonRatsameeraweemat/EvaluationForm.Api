using System;
using System.Threading.Tasks;

namespace EVF.Data.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TPocoEntity> GetRepository<TPocoEntity>() where TPocoEntity : class;
        int Complete();
        Task<int> CompleteAsync();
    }
}
