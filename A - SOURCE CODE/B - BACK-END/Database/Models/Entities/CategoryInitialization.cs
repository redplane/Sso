using System.ComponentModel.DataAnnotations.Schema;

namespace DbModel.Models.Entities
{
    public class CategoryInitialization
    {
        #region Properties

        /// <summary>
        /// Email of category creator.
        /// </summary>
        public string CreatorEmail { get; set; }

        /// <summary>
        /// Id of category which has been created.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Time when category initialization was logged.
        /// </summary>
        public double CreatedTime { get; set; }

        #endregion

        #region Navigators

        /// <summary>
        /// Who created the category.
        /// </summary>
        [ForeignKey(nameof(CreatorEmail))]
        public virtual Account Creator { get; set; }

        /// <summary>
        /// Information of category.
        /// </summary>
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        #endregion

    }
}