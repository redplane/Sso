using System.Collections.Generic;
using Shared.Enumerations.Sortings;
using Shared.Models;

namespace Shared.ViewModels.Photo
{
    public class SearchPhotoViewModel
    {
        #region Properties

        /// <summary>
        /// List of indexes.
        /// </summary>
        public List<string> Ids { get; set; }

        /// <summary>
        /// Photo url list.
        /// </summary>
        public List<string> Urls { get; set; }

        /// <summary>
        /// Photo titles.
        /// </summary>
        public List<string> Titles { get; set; }

        /// <summary>
        /// Photo descriptions.
        /// </summary>
        public List<string> Descriptions { get; set; }

        /// <summary>
        /// Width of photo.
        /// </summary>
        public Range<double?> Width { get; set; }

        /// <summary>
        /// Height of photo.
        /// </summary>
        public Range<double?> Height { get; set; }

        /// <summary>
        /// Time when photo was created.
        /// </summary>
        public Range<double?> CreatedTime { get; set; }

        /// <summary>
        /// Time when photo was lastly modified.
        /// </summary>
        public Range<double?> LastModifiedTime { get; set; }

        /// <summary>
        /// Property and sorting direction.
        /// </summary>
        public Sorting<PhotoPropertySort> Sorting { get; set; }

        /// <summary>
        /// Pagination.
        /// </summary>
        public Pagination Pagination { get; set; }

        #endregion
    }
}