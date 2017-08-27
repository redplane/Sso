namespace Sso.ViewModels.Accounts
{
    public class TokenViewModel
    {
        #region Properties

        /// <summary>
        /// Token code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// When token should be expired.
        /// </summary>
        public double Expiration { get; set; }

        /// <summary>
        /// Type of token.
        /// </summary>
        public string Type { get; set; }

        #endregion
    }
}