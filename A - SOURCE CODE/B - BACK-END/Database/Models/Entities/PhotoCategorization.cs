using System.ComponentModel.DataAnnotations.Schema;

namespace DbModel.Models.Entities
{
    public class PhotoCategorization
    {
        #region Properties

        /// <summary>
        /// Id of photo.
        /// </summary>
        public string PhotoId { get; set; }

        /// <summary>
        /// Category index.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Time when photo was categorized.
        /// </summary>
        public double CategorizedTime { get; set; }

        /// <summary>
        /// Time when photo categorization was lastly modified.
        /// </summary>
        public double? LastModifiedTime { get; set; }

        #endregion

        #region Navigators

        /// <summary>
        /// Photo which was categorized.
        /// </summary>
        [ForeignKey(nameof(PhotoId))]
        public Photo Photo { get; set; }

        /// <summary>
        /// Category which photo belongs to.
        /// </summary>
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        #endregion

    }
}