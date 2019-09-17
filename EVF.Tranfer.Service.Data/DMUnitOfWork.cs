using EVF.Data.Repository.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Tranfer.Service.Data
{
    /// <summary>
    /// DMUnitOfWork class is a unit of work for manipulating about utility data in database via repository.
    /// </summary>
    public class DMUnitOfWork : EfUnitOfWork<DMContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DMUnitOfWork" /> class.
        /// </summary>
        /// <param name="dmDbContext">The DataMart integretion sap database context what inherits from DbContext of EF.</param>
        public DMUnitOfWork(DMContext dmDbContext) : base(dmDbContext)
        { }
    }
}
