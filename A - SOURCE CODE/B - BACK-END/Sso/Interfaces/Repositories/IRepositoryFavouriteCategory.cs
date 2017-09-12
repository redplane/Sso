using System.Linq;
using Database.Models.Entities;
using Shared.ViewModels.FavoriteCategory;

namespace Sso.Interfaces.Repositories
{
    public interface IRepositoryFavouriteCategory: IParentRepository<FavouriteCategory>
    {
        #region Methods

        /// <summary>
        /// Find favorite categories by using specific conditions.
        /// </summary>
        /// <param name="favoriteCategories"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        IQueryable<FavouriteCategory> Search(IQueryable<FavouriteCategory> favoriteCategories,
            SearchFavoriteCategoryViewModel conditions);

        #endregion
    }
}