using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace iPhotoService.Installers
{
    [RunInstaller(true)]
    public class ApiHostingServiceInstaller : Installer
    {
        #region Properties

        /// <summary>
        /// Name of service process.
        /// </summary>
        private readonly string _serviceName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initiate installer with services.
        /// </summary>
        public ApiHostingServiceInstaller()
        {
            _serviceName = "Desktop Photo";

            var serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem; // Or whatever account you want

            var serviceInstaller = new ServiceInstaller()
            {
                DisplayName = "iPhoto Service",
                StartType = ServiceStartMode.Automatic, // Or whatever startup type you want
                Description = "A small service which is for obtaining image and set it as background.",
                ServiceName = _serviceName
            };

            Installers.Add(serviceInstaller);
            Installers.Add(serviceProcessInstaller);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Callback which is fired when service is committed.
        /// </summary>
        /// <param name="savedState"></param>
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            // This will automatically start your service upon completion of the installation.
            try
            {
                var serviceController = new ServiceController(_serviceName);
                serviceController.Start();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        #endregion
    }
}