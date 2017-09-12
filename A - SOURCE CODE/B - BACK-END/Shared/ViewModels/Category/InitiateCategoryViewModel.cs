using System.ComponentModel.DataAnnotations;

namespace Shared.ViewModels.Category
{
    public class InitiateCategoryViewModel
    {
        #region Properties

        /// <summary>
        /// Name of category (this should be unique)
        /// </summary>
        [Required]
        public string Name { get; set; }

        #endregion
    }
}