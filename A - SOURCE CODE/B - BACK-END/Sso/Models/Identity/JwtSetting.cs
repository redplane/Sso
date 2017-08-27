namespace Sso.Models.Identity
{
    public class JwtSetting
    {
        #region Properties

        /// <summary>
        /// Key which is for protecting token.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Name of identity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Life time of token.
        /// </summary>
        public int LifeTime { get; set; }

        /// <summary>
        /// Type of token.
        /// </summary>
        public string Type { get; set; }

        #endregion
    }
}