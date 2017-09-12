using System.Linq;
using Database.Models.Entities;
using Shared.ViewModels.Category;

namespace Sso.Interfaces.Repositories
{
    public interface IRepositoryCategory : IParentRepository<Category>
    {
        #region Methods

        /// <summary>
        /// Search categories base on specific conditions.
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        IQueryable<Category> Search(IQueryable<Category> categories, SearchCategoryViewModel condition);

        #endregion
    }
}