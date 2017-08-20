using System.ComponentModel.DataAnnotations;

namespace Shared.ViewModels
{
    public class RegisterAccountViewModel
    {
        #region Properties

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        #endregion
    }
}