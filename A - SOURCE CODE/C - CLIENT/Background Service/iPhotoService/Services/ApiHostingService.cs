using System.ServiceProcess;
using Microsoft.Owin.Hosting;

namespace iPhotoService.Services
{
    public class ApiHostingService : ServiceBase
    {
        #region Methods

        /// <summary>
        /// Callback which is fired when service starts.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            WebApp.Start<Startup>("http://localhost:9000");
        }

        #endregion
    }
}