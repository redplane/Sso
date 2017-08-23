using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Database.Models.Entities;
using Newtonsoft.Json;
using SharedService.Interfaces;
using SharedService.Models;
using Sso.Interfaces.Repositories;
using Sso.Interfaces.Services;
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

        /// <summary>
        /// Service which is for encrypting information.
        /// </summary>
        private readonly IEncryptionService _encryptionService;

        /// <summary>
        /// System time service.
        /// </summary>
        private readonly ISystemTimeService _systemTimeService;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initiate controller with injectors
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="identityService"></param>
        /// <param name="encryptionService"></param>
        /// <param name="systemTimeService"></param>
        public ApiAccountController(IUnitOfWork unitOfWork, 
            IIdentityService identityService,
            IEncryptionService encryptionService, 
            ISystemTimeService systemTimeService)
        {
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _encryptionService = encryptionService;
            _systemTimeService = systemTimeService;
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
           
            // Find account duplication.
            var accounts = _unitOfWork.RepositoryAccount.Search();
            accounts = accounts.Where(x => x.Email.Equals(info.Email, StringComparison.InvariantCultureIgnoreCase));

            // Account exists.
            if (await accounts.AnyAsync())
                return Conflict();

            #endregion

            #region Initiate account
            
            var account = new Account();
            account.Email = info.Email;
            account.Password = _encryptionService.InitMd5(info.Password);

            account = _unitOfWork.RepositoryAccount.Insert(account);
            await _unitOfWork.CommitAsync();

            #endregion

            return Ok(account);
        }

        [HttpPost]
        [Route("internal-login")]
        public async Task<IHttpActionResult> InternalLogin([FromBody] InternalLoginViewModel info)
        {
            #region Parameter validation

            if (info == null)
            {
                info = new InternalLoginViewModel();
                Validate(info);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            #endregion

            #region Find account

            // Hash the password first.
            var hashedPassword = _encryptionService.InitMd5(info.Password).ToLower();

            // Find list of accounts with specific conditions.
            var accounts = _unitOfWork.RepositoryAccount.Search();
            accounts = accounts.Where(x => x.Email.Equals(info.Email) && x.Password.ToLower() == hashedPassword);

            // Find account availability.
            var account = await accounts.FirstOrDefaultAsync();
            if (account == null)
                return NotFound();

            #endregion

            #region Token initialization

            // When token should be expired.
            var expiration = DateTime.Now.AddSeconds(3600);
            var iExpiration = _systemTimeService.UtcToEpoch(expiration);

            var claims = new Dictionary<string, string>();
            claims.Add(ClaimTypes.Email, account.Email);
            claims.Add(ClaimTypes.Role, "User");
            claims.Add(ClaimTypes.Expiration, $"{iExpiration}");

            var token = new TokenViewModel();
            token.Code = _identityService.EncodeJwt(claims, "hello-world");
            token.Expiration = iExpiration;
            token.Type = "Bearer";

            #endregion

            return Ok(token);
        }
        #endregion
    }
}