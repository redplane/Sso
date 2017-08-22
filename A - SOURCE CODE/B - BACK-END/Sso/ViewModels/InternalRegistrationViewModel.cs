using System.ComponentModel.DataAnnotations;

namespace Sso.ViewModels
{
    public class InternalRegistrationViewModel
    {
        #region Properties

        /// <summary>
        /// Email address.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Password of account.
        /// </summary>
        [Required]
        public string Password { get; set; }

        #endregion
    }
}
