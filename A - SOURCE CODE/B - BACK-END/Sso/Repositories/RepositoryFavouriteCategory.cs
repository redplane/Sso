using System.Data.Entity;
using System.Linq;
using Database.Models.Entities;
using Shared.ViewModels.Category;
using Shared.ViewModels.FavoriteCategory;
using Sso.Interfaces.Repositories;

namespace Sso.Repositories
{
    public class RepositoryFavouriteCategory: ParentRepository<FavouriteCategory>, IRepositoryFavouriteCategory
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initiate repository with injectors.
        /// </summary>
        /// <param name="dbContext"></param>
        public RepositoryFavouriteCategory(DbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// Search for favourite categories by using specific conditions.
        /// </summary>
        /// <param name="favoriteCategories"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IQueryable<FavouriteCategory> Search(IQueryable<FavouriteCategory> favoriteCategories, SearchFavoriteCategoryViewModel conditions)
        {
            if (conditions == null)
                return favoriteCategories;

            // Category indexes search.
            if (conditions.CategoryIds != null && conditions.CategoryIds.Count > 0)
            {
                var indexes = conditions.CategoryIds.Where(x => x > 0).Distinct().ToList();
                if (indexes.Count > 0)
                    favoriteCategories = favoriteCategories.Where(x => indexes.Contains(x.CategoryId));
            }

            // Creator emails search.
            if (conditions.FollowerEmails != null && conditions.FollowerEmails.Count > 0)
            {
                var emails = conditions.FollowerEmails.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                if (emails.Count > 0)
                    favoriteCategories = favoriteCategories.Where(x => emails.Any(y => x.FollowerEmail.Contains(y)));
            }

            return favoriteCategories;
        }

        #endregion
    }
}