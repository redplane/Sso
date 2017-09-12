using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Database.Enumerations;
using Database.Models.Entities;
using Shared.Enumerations;
using Shared.Enumerations.Sortings;
using Shared.Models;
using Shared.Resources;
using Shared.ViewModels.FavoriteCategory;
using Sso.Attributes;
using Sso.Interfaces.Repositories;
using Sso.Interfaces.Services;

namespace Sso.Controllers
{
    [RoutePrefix("api/favourite-category")]
    [ApiAuthorize]
    public class ApiFavouriteCategoryController : ApiController
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initiate controller with injectors.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="identityService"></param>
        /// <param name="systemTimeService"></param>
        public ApiFavouriteCategoryController(IUnitOfWork unitOfWork,
            IIdentityService identityService,
            ISystemTimeService systemTimeService)
        {
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _systemTimeService = systemTimeService;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Contains functions & repositories to access into database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        ///     Service which handles identity business.
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     System time service.
        /// </summary>
        private readonly ISystemTimeService _systemTimeService;

        #endregion

        #region Methods

        /// <summary>
        ///     Initiate a category into database.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [Route("follow")]
        [HttpGet]
        public async Task<IHttpActionResult> Follow([FromUri] int categoryId)
        {
            #region Find requester

            // Find claim attached into the current request.
            var account = _identityService.FindRequestIdentity(Request);
            if (account == null)
                return Unauthorized();

            #endregion

            #region Duplicate check

            // Find favourite category by using the submitted name.
            var favoriteCategories = _unitOfWork.RepositoryFavouriteCategories.Search();
            favoriteCategories = favoriteCategories.Where(x =>
                x.CategoryId == categoryId &&
                x.FollowerEmail.Equals(account.Email, StringComparison.InvariantCultureIgnoreCase));

            // Already liked category.
            if (await favoriteCategories.AnyAsync())
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.Conflict,
                    HttpMessages.AlreadyLikedCategory));

            #endregion

            #region Category search

            var categories = _unitOfWork.RepositoryCategories.Search();
            categories = categories.Where(x => x.Id == categoryId);
            if (!await categories.AnyAsync())
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    HttpMessages.CategoryNotFound));

            #endregion

            #region Favourite category initialization

            var favouriteCategory = new FavouriteCategory();
            favouriteCategory.CategoryId = categoryId;
            favouriteCategory.FollowerEmail = account.Email;
            favouriteCategory.FollowedTime = _systemTimeService.UtcToEpoch(DateTime.UtcNow);

            // Initiate account into repository and submit to database.
            _unitOfWork.RepositoryFavouriteCategories.Insert(favouriteCategory);
            await _unitOfWork.CommitAsync();

            #endregion

            return Ok(favouriteCategory);
        }

        /// <summary>
        ///     Initiate a category into database.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [Route("unfollow")]
        [HttpDelete]
        public async Task<IHttpActionResult> Unfollow([FromUri] int categoryId)
        {
            #region Find requester

            // Find claim attached into the current request.
            var account = _identityService.FindRequestIdentity(Request);
            if (account == null)
                return Unauthorized();

            #endregion

            #region Delete favourite category

            // Find favourite category by using the submitted name.
            var favoriteCategories = _unitOfWork.RepositoryFavouriteCategories.Search();
            favoriteCategories = favoriteCategories.Where(x =>
                x.CategoryId == categoryId &&
                x.FollowerEmail.Equals(account.Email, StringComparison.InvariantCultureIgnoreCase));

            // Unfollow specific categories.
            _unitOfWork.RepositoryFavouriteCategories.Remove(favoriteCategories);

            await _unitOfWork.CommitAsync();

            #endregion

            return Ok();
        }

        /// <summary>
        /// Search for favourite categories using specific conditions.
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Search([FromBody] SearchFavoriteCategoryViewModel conditions)
        {
            #region Parameters validation

            if (conditions == null)
            {
                conditions = new SearchFavoriteCategoryViewModel();
                Validate(conditions);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            #endregion

            #region Find request identity

            // Find request identity.
            var account = _identityService.FindRequestIdentity(Request);
            if (account == null)
                return Unauthorized();

            #endregion

            #region Favourite categories search

            // Client only sees his/her favourite categories.
            if (account.Role != Role.Administrator)
                conditions.FollowerEmails = new List<string> { account.Email };

            var favouriteCategories = _unitOfWork.RepositoryFavouriteCategories.Search();
            favouriteCategories = _unitOfWork.RepositoryFavouriteCategories.Search(favouriteCategories, conditions);

            // Do sorting.
            var sorting = conditions.Sorting;
            if (sorting == null)
                favouriteCategories = _unitOfWork.Sort(favouriteCategories, SortDirection.Descending,
                    FavouriteCategoryPropertySort.FollowedTime);
            else
                favouriteCategories = _unitOfWork.Sort(favouriteCategories, sorting.Direction, sorting.Property);

            // Do pagination.
            var result = new SearchResult<IList<FavouriteCategory>>();
            result.Total = await favouriteCategories.CountAsync();
            result.Records = await _unitOfWork.Paginate(favouriteCategories, conditions.Pagination).ToListAsync();

            #endregion

            return Ok(result);
        }
        
        #endregion
    }
}