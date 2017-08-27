namespace Sso.ViewModels.Accounts
{
    public class InternalLoginViewModel
    {
        #region Properties

        /// <summary>
        /// Email for logging into system.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password of account.
        /// </summary>
        public string Password { get; set; }

        #endregion
    }
}