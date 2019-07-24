using EVF.Data.Repository.EF;

namespace EVF.Data
{
    /// <summary>
    /// EVFUnitOfWork class is a unit of work for manipulating about utility data in database via repository.
    /// </summary>
    public class EVFUnitOfWork : EfUnitOfWork<EVFContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EVFUnitOfWork" /> class.
        /// </summary>
        /// <param name="evfDbContext">The Evaluation Form database context what inherits from DbContext of EF.</param>
        public EVFUnitOfWork(EVFContext evfDbContext) : base(evfDbContext)
        { }
    }
}
