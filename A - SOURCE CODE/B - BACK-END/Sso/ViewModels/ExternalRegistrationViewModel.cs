using Sso.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sso.ViewModels
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
