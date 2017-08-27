using Sso.Enumerations;

namespace Sso.ViewModels.Accounts
{
    public class ExternalRegistrationViewModel
    {
        #region Properties

        /// <summary>
        /// Token provided by external authentication system.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Open authentication provider.
        /// </summary>
        public TokenProvider Provider { get; set; }

        #endregion
    }
}
