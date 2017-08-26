using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Database.Models.Entities;
using Newtonsoft.Json;
using Sso.Attributes;
using Sso.Enumerations;
using Sso.Interfaces.Repositories;
using Sso.Interfaces.Services;
using Sso.ViewModels.Accounts;
using Sso.Models.Identity;

namespace Sso.Controllers
{
    [RoutePrefix("api/account")]
    [ApiAuthorize]
    public class ApiAccountController : ApiController
    {
        #region Constructors

        /// <summary>
        ///     Initiate controller with injectors
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="identityService"></param>
        /// <param name="encryptionService"></param>
        /// <param name="systemTimeService"></param>
        /// <param name="jwtSetting"></param>
        public ApiAccountController(IUnitOfWork unitOfWork,
            IIdentityService identityService,
            IEncryptionService encryptionService,
            ISystemTimeService systemTimeService,
            JwtSetting jwtSetting)
        {
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _encryptionService = encryptionService;
            _systemTimeService = systemTimeService;
            _jwtSetting = jwtSetting;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Jwt setting.
        /// </summary>
        private readonly JwtSetting _jwtSetting;

        /// <summary>
        ///     Contains functions & repositories to access into database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        ///     Service which handles identity business.
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     Service which is for encrypting information.
        /// </summary>
        private readonly IEncryptionService _encryptionService;

        /// <summary>
        ///     System time service.
        /// </summary>
        private readonly ISystemTimeService _systemTimeService;

        #endregion

        #region Methods

        /// <summary>
        ///     Hello world method.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok();
        }

        /// <summary>
        ///     Register account using email & password.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("internal-registration")]
        [AllowAnonymous]
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
            var accounts = _unitOfWork.RepositoryAccounts.Search();
            accounts = accounts.Where(x => x.Email.Equals(info.Email, StringComparison.InvariantCultureIgnoreCase));

            // Account exists.
            if (await accounts.AnyAsync())
                return Conflict();

            #endregion

            #region Initiate account

            var account = new Account();
            account.Email = info.Email;
            account.Password = _encryptionService.InitMd5(info.Password);

            account = _unitOfWork.RepositoryAccounts.Insert(account);
            await _unitOfWork.CommitAsync();

            #endregion

            // Clear the password first.
            account.Password = "";

            return Ok(account);
        }

        /// <summary>
        ///     This api is for register account by using email & password.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("internal-login")]
        [AllowAnonymous]
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
            var accounts = _unitOfWork.RepositoryAccounts.Search();
            accounts = accounts.Where(x => x.Email.Equals(info.Email) && x.Password.ToLower() == hashedPassword);

            // Find account availability.
            var account = await accounts.FirstOrDefaultAsync();
            if (account == null)
                return NotFound();

            #endregion

            #region Token initialization

            // When token should be expired.
            var expiration = DateTime.Now.AddSeconds(_jwtSetting.LifeTime);
            var iExpiration = _systemTimeService.UtcToEpoch(expiration);
            
            // Initiate claim.
            var generic = new Generic(account);
            generic.ExpirationTime = iExpiration;

            var token = new TokenViewModel();
            token.Code = _identityService.EncodeJwt(generic.ToClaims(), _jwtSetting.Key);
            token.Expiration = iExpiration;
            token.Type = "Bearer";

            #endregion

            return Ok(token);
        }

        /// <summary>
        ///     Use token provided by 3rd party provider to sign into system.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("external-login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ExternalLogin([FromBody] ExternalLoginViewModel info)
        {
            #region Parameters validation

            if (info == null)
            {
                info = new ExternalLoginViewModel();
                Validate(info);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            #endregion

            #region Identity generation

            // Initiate list of claims.
            var claims = new Dictionary<string, string>();

            // Account instance.
            Account account;
            var accounts = _unitOfWork.RepositoryAccounts.Search();
            
            switch (info.Provider)
            {
                case TokenProvider.Google:

                    var httpResponseMessage = await _identityService.FindGoogleIdentity(info.Token);
                    var szContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    var identityGoogle = JsonConvert.DeserializeObject<Google>(szContent);

                    // Account is not in the system. Create one.
                    account = await accounts.Where(x => x.Email.Equals(identityGoogle.Email,
                        StringComparison.InvariantCultureIgnoreCase)).FirstOrDefaultAsync();

                    if (account == null)
                    {
                        account = new Account();
                        account.Email = identityGoogle.Email;
                        account.PhotoUrl = identityGoogle.Photo;
                        
                        _unitOfWork.RepositoryAccounts.Insert(account);
                       await _unitOfWork.CommitAsync();
                    }

                    break;
                default:
                    return NotFound();
            }
            
            #endregion

            #region Token generation

            // Initiate expiration
            var expiration = DateTime.Now.AddSeconds(_jwtSetting.LifeTime);
            var iExpiration = _systemTimeService.UtcToEpoch(expiration);

            // Initiate generic token.
            var generic = new Generic(account);
            generic.ExpirationTime = iExpiration;

            // Payload initialization.
            var payload = _identityService.EncodeJwt(generic.ToClaims(), _jwtSetting.Key);

            // Initiate token view model.
            var jwt = new TokenViewModel();
            jwt.Code = payload;
            jwt.Type = _jwtSetting.Type;
            jwt.Expiration = iExpiration;

            #endregion

            return Ok(jwt);
        }

        #endregion
    }
}