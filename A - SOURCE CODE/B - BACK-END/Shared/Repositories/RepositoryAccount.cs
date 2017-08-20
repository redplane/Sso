using System.Data.Entity;
using Database.Models.Entities;
using Shared.Interfaces.Repositories;

namespace Shared.Repositories
{
    public class RepositoryAccount: ParentRepository<Account>, IRepositoryAccount
    {
        #region Constructor

        /// <summary>
        /// Initiate repository with injectors.
        /// </summary>
        /// <param name="dbContext"></param>
        public RepositoryAccount(DbContext dbContext) : base(dbContext)
        {
        }

        #endregion
    }
}