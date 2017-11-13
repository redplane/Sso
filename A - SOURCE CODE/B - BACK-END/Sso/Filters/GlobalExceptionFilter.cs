using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using log4net;

namespace Sso.Filters
{
    /// <inheritdoc />
    /// <summary>
    /// Catch action execution for global filter handling.
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        #region Properties
        
        /// <inheritdoc />
        /// <summary>
        /// Allow multi filters to run.
        /// </summary>
        public bool AllowMultiple => true;
        
        /// <summary>
        /// Instance for logging.
        /// </summary>
        public ILog Log { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initiate filter without parameters (parameterless constructor)
        /// </summary>
        public GlobalExceptionFilter()
        {
            Log = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetType());
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// Execute exception filter asynchronously.
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            // Context is invalid.
            if (actionExecutedContext == null)
                return Task.FromResult(0);

            // Exception is not valid.
            var exception = actionExecutedContext.Exception;
            if (exception == null)
                return Task.FromResult(0);

            Log.Error(exception.Message, exception);
            return Task.FromResult(0);
        }

        #endregion
    }
}