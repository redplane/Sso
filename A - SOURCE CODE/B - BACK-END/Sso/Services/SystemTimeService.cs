using System;
using Sso.Interfaces.Services;

namespace Sso.Services
{
    public class SystemTimeService : ISystemTimeService
    {
        #region Properties

        /// <summary>
        /// Origin time.
        /// </summary>
        private readonly DateTime _origin;

        #endregion

        #region Constructors

        /// <summary>
        /// Initiate service with default settings.
        /// </summary>
        public SystemTimeService()
        {
            _origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert utc time to epoch time.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public double UtcToEpoch(DateTime dateTime)
        {
            return (dateTime - _origin).TotalMilliseconds;
        }

        /// <summary>
        /// Convert epoch time to utc.
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public DateTime EpochToUtc(double unixTime)
        {
            return _origin.AddMilliseconds(unixTime);
        }

        #endregion
    }
}