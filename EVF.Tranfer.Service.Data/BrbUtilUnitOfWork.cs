using EVF.Data.Repository.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Tranfer.Service.Data
{
    /// <summary>
    /// BrbUtilUnitOfWork class is a unit of work for manipulating about utility data in database via repository.
    /// </summary>
    public class BrbUtilUnitOfWork : EfUnitOfWork<BrbUtilContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrbUtilUnitOfWork" /> class.
        /// </summary>
        /// <param name="brbUtilDbContext">The brb util integretion sap database context what inherits from DbContext of EF.</param>
        public BrbUtilUnitOfWork(BrbUtilContext brbUtilDbContext) : base(brbUtilDbContext)
        { }
    }
}
