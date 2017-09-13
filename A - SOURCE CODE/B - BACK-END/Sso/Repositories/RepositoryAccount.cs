using System.Data.Entity;
using DbModel.Models.Entities;
using Sso.Interfaces.Repositories;

namespace Sso.Repositories
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