using EVF.Data.Repository.Interfaces;

namespace EVF.Tranfer.Service.Data
{
    public class UnitOfWorkControl
    {
        public delegate IUnitOfWork ServiceResolver(string key);
    }
}
