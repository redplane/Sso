using System.ComponentModel.DataAnnotations;

namespace Sso.ViewModels.Accounts
{
    public class InternalLoginViewModel
    {
        #region Properties

        /// <summary>
        /// Email for logging into system.
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