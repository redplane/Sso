using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Enumerations;
using Shared.Enumerations.Sortings;
using Shared.Models;

namespace Shared.ViewModels.FavoriteCategory
{
    public class SearchFavoriteCategoryViewModel
    {
        #region Properties

        /// <summary>
        /// List of category indexes.
        /// </summary>
        public List<int> CategoryIds { get; set; }

        /// <summary>
        /// List of follower emails.
        /// </summary>
        public List<string> FollowerEmails { get; set; }

        /// <summary>
        /// Sorting property and direction.
        /// </summary>
        public Sorting<FavouriteCategoryPropertySort> Sorting { get; set; }

        /// <summary>
        /// Pagination information.
        /// </summary>
        [Required]
        public Pagination Pagination { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initiate search conditoin with default settings.
        /// </summary>
        public SearchFavoriteCategoryViewModel()
        {
            Sorting = new Sorting<FavouriteCategoryPropertySort>(FavouriteCategoryPropertySort.FollowedTime, SortDirection.Descending);
        }

        #endregion
    }
}