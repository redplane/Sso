using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sso.Interfaces.Services
{
   public  interface IEncryptionService
    {
        #region Methods

        /// <summary>
        /// Initiate md5 from text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string InitMd5(string text);

        #endregion
    }
}
