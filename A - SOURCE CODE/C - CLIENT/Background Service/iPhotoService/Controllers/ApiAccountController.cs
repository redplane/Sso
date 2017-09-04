using System.Web.Http;

namespace iPhotoService.Controllers
{
    [RoutePrefix("api/account")]
    public class ApiAccountController : ApiController
    {
        #region Methods

        /// <summary>
        ///     Find account information.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var message = new
            {
                Message = "Hello world"
            };
            return Ok(message);
        }

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion
    }
}