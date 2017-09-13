using System.Data.Entity;
using System.Linq;
using DbModel.Models.Entities;
using Shared.ViewModels.Category;
using Sso.Interfaces.Repositories;

namespace Sso.Repositories
{
    public class RepositoryCategory : ParentRepository<Category>, IRepositoryCategory
    {
        #region Constructor

        /// <inheritdoc />
        /// <summary>
        /// Initiate repository with injectors.
        /// </summary>
        /// <param name="dbContext"></param>
        public RepositoryCategory(DbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// Search for categories by using specific conditions.
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IQueryable<Category> Search(IQueryable<Category> categories, SearchCategoryViewModel condition)
        {
            // Condition has been defined.
            if (condition == null)
                return categories;

            // Category indexes have been defined.
            if (!(condition.Ids == null || condition.Ids.Count < 1))
            {
                var indexes = condition.Ids.Where(x => x > 0).Distinct().ToList();
                if (!(indexes.Count < 1))
                    categories = categories.Where(x => indexes.Contains(x.Id));
            }

            // Category names have been defined.
            if (!(condition.Names == null || condition.Names.Count < 1))
            {
                var names = condition.Names.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                if (names.Count > 0)
                    categories = categories.Where(x => names.Any(y => x.Name.Contains(y)));
            }

            // Creator emails have been defined.
            if (!(condition.CreatorEmails == null || condition.CreatorEmails.Count < 1))
            {
                var emails = condition.CreatorEmails.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                if (emails.Count > 0)
                    categories = categories.Where(x => emails.Any(y => x.CreatorEmail.Contains(y)));
            }

            // Created time has been defined.
            if (condition.CreatedTime != null)
            {
                var from = condition.CreatedTime.From;
                var to = condition.CreatedTime.To;

                if (from != null)
                    categories = categories.Where(x => x.CreatedTime >= from.Value);
                if (to != null)
                    categories = categories.Where(x => x.CreatedTime <= to.Value);
            }

            // Last modified time has been defined.
            if (condition.LastModifiedTime != null)
            {
                var from = condition.LastModifiedTime.From;
                var to = condition.LastModifiedTime.To;

                if (from != null)
                    categories = categories.Where(x => x.LastModifiedTime >= from.Value);

                if (to != null)
                    categories = categories.Where(x => x.LastModifiedTime <= to.Value);
            }

            return categories;
        }

        #endregion
    }
}