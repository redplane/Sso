using Sso.Enumerations;

namespace Sso.ViewModels.Accounts
{
    public class ExternalLoginViewModel
    {
        #region Properties

        /// <summary>
        /// Token of 3rd provider.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Provider type.
        /// </summary>
        public TokenProvider Provider { get; set; }

        #endregion
    }
}