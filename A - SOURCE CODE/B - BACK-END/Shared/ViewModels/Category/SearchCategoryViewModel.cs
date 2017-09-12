using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Enumerations;
using Shared.Enumerations.Sortings;
using Shared.Models;

namespace Shared.ViewModels.Category
{
    public class SearchCategoryViewModel
    {
        #region Properties

        /// <summary>
        /// List of category indexes.
        /// </summary>
        public List<int> Ids { get; set; }

        /// <summary>
        /// List of category names.
        /// </summary>
        public List<string> Names { get; set; }

        /// <summary>
        /// List of creator email addresses.
        /// </summary>
        public List<string> CreatorEmails { get; set; }

        /// <summary>
        /// Time when category had been created.
        /// </summary>
        public Range<double?> CreatedTime { get; set; }

        /// <summary>
        /// Time when category was lastly modified.
        /// </summary>
        public Range<double?> LastModifiedTime { get; set; }
        
        /// <summary>
        /// Record sortings.
        /// </summary>
        public Sorting<CategoryPropertySort> Sorting { get; set; }

        /// <summary>
        /// Pagination information.
        /// </summary>
        [Required]
        public Pagination Pagination { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SearchCategoryViewModel()
        {
            Sorting = new Sorting<CategoryPropertySort>(CategoryPropertySort.LastModifiedTime, SortDirection.Descending);
        }

        #endregion
    }
}