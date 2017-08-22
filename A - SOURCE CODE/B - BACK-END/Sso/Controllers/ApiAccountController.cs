using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Database.Models.Entities;
using Newtonsoft.Json;
using SharedService.Interfaces;
using SharedService.Models;
using Sso.Interfaces.Repositories;
using Sso.ViewModels;

namespace Sso.Controllers
{
    [RoutePrefix("api/account")]
    public class ApiAccountController : ApiController
    {
        #region Properties

        /// <summary>
        ///     Contains functions & repositories to access into database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Service which handles identity business.
        /// </summary>
        private readonly IIdentityService _identityService;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initiate controller with injectors
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="identityService"></param>
        public ApiAccountController(IUnitOfWork unitOfWork, IIdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _identityService = identityService;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Hello world method.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri] string token)
        {
            var httpResponseMessage = await _identityService.FindGoogleIdentity(token);
            if (!httpResponseMessage.IsSuccessStatusCode)
                return StatusCode(httpResponseMessage.StatusCode);

            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            var identity = JsonConvert.DeserializeObject<IdentityGoogle>(content);
            return Ok(identity);
        }

        /// <summary>
        /// Register account using email & password.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("internal-registration")]
        public async Task<IHttpActionResult> Register([FromBody] InternalRegistrationViewModel info)
        {
            #region Parameter validations

            if (info == null)
            {
                info = new InternalRegistrationViewModel();
                Validate(info);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            #endregion

            #region Account find

            var accounts = _unitOfWork.RepositoryAccount.Search();
            accounts = accounts.Where(x => x.Email.Equals(info.Email, StringComparison.InvariantCultureIgnoreCase));

            // Account exists.
            if (await accounts.AnyAsync())
                return Conflict();

            #endregion

            #region Initiate account
            
            var account = new Account();
            account.Email = info.Email;
            account.Password = info.Password;

            account = _unitOfWork.RepositoryAccount.Insert(account);
            await _unitOfWork.CommitAsync();

            #endregion

            return Ok(account);
        }

        #endregion
    }
}