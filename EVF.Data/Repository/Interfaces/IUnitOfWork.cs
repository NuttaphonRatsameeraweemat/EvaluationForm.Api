using System;
using System.Threading.Tasks;
using System.Transactions;

namespace EVF.Data.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TPocoEntity> GetRepository<TPocoEntity>() where TPocoEntity : class;
        int Complete(TransactionScope scope = null);
        Task<int> CompleteAsync(TransactionScope scope = null);
    }
}
