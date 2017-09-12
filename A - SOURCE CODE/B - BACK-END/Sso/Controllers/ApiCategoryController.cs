using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Database.Models.Entities;
using Shared.Enumerations;
using Shared.Enumerations.Sortings;
using Shared.Models;
using Shared.Resources;
using Shared.ViewModels.Category;
using Sso.Attributes;
using Sso.Interfaces.Repositories;
using Sso.Interfaces.Services;

namespace Sso.Controllers
{
    [RoutePrefix("api/category")]
    [ApiAuthorize]
    public class ApiCategoryController : ApiController
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initiate controller with injectors.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="identityService"></param>
        /// <param name="systemTimeService"></param>
        public ApiCategoryController(IUnitOfWork unitOfWork,
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
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> InitiateCategory([FromBody] InitiateCategoryViewModel info)
        {
            #region Parameter validation

            if (info == null)
            {
                info = new InitiateCategoryViewModel();
                Validate(info);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            #endregion

            #region Find requester

            // Find claim attached into the current request.
            var account = _identityService.FindRequestIdentity(Request);
            if (account == null)
                return Unauthorized();

            #endregion


            #region Duplicate check

            // Find category by using the submitted name.
            var categories = _unitOfWork.RepositoryCategories.Search();

            var conditions = new SearchCategoryViewModel();
            conditions.Names = new List<string> { info.Name };
            categories = _unitOfWork.RepositoryCategories.Search(categories, conditions);

            // Category exists.
            if (await categories.AnyAsync())
                return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.Conflict,
                    HttpMessages.CategoryDuplicated));

            #endregion

            #region Category initialization

            var category = new Category();
            category.Name = info.Name;
            category.CreatorEmail = account.Email;
            category.CreatedTime = _systemTimeService.UtcToEpoch(DateTime.UtcNow);

            // Initiate account into repository and submit to database.
            _unitOfWork.RepositoryCategories.Insert(category);
            await _unitOfWork.CommitAsync();

            #endregion

            return Ok(category);
        }

        /// <summary>
        /// Start following a category.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [Route("follow")]
        public async Task<IHttpActionResult> FollowCategory([FromUri] int categoryId)
        {
            #region Find request identity

            // Find account from request.
            var account = _identityService.FindRequestIdentity(Request);
            if (account == null)
                return Unauthorized();

            #endregion

            #region Find category

            // Find categories.
            var categories = _unitOfWork.RepositoryCategories.Search();
            categories = categories.Where(x => x.Id == categoryId);
            
            // Category is not found.
            var category = await categories.FirstOrDefaultAsync();
            if (category == null)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    HttpMessages.CategoryNotFound));

            #endregion

            #region Check following category
            

            #endregion
        }

        /// <summary>
        /// Edit category information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        public async Task<IHttpActionResult> EditCategory([FromUri] int id,
            [FromBody] EditCategoryViewModel information)
        {
            #region Parameters validation

            if (information == null)
            {
                information = new EditCategoryViewModel();
                Validate(information);
            }

            if (ModelState.IsValid)
                return BadRequest(ModelState);

            #endregion

            #region Find category
            
            var categories = _unitOfWork.RepositoryCategories.Search();
            var target = await categories.Where(x => x.Id == id).FirstOrDefaultAsync();
            
            // Target category is not found.
            if (target == null)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    HttpMessages.CategoryNotFound));

            // Find duplicate.
            var bDuplicate = await categories.Where(x =>
                x.Name.Equals(information.Name, StringComparison.InvariantCultureIgnoreCase)).AnyAsync();

            if (bDuplicate)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict,
                    HttpMessages.CategoryDuplicated));

            #endregion

            #region Update category information

            target.Name = information.Name;
            target.LastModifiedTime = _systemTimeService.UtcToEpoch(DateTime.UtcNow);

            // Save information into database.
            await _unitOfWork.CommitAsync();

            #endregion

            return Ok(target);
        }

        /// <summary>
        ///     Search categories by using specific conditions.
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchCategories([FromBody] SearchCategoryViewModel conditions)
        {
            #region Parameters validation

            if (conditions == null)
            {
                conditions = new SearchCategoryViewModel();
                Validate(conditions);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            #endregion

            #region Search for categories

            var categories = _unitOfWork.RepositoryCategories.Search();
            categories = _unitOfWork.RepositoryCategories.Search(categories, conditions);

            // Do sorting.
            var sorting = conditions.Sorting;
            if (sorting != null)
                categories = _unitOfWork.Sort(categories, sorting.Direction, sorting.Property);
            else
                categories = _unitOfWork.Sort(categories, SortDirection.Descending,
                    CategoryPropertySort.LastModifiedTime);

            // Paginate.
            var result = new SearchResult<IList<Category>>();
            result.Total = await categories.CountAsync();
            result.Records = await _unitOfWork.Paginate(categories, conditions.Pagination).ToListAsync();

            #endregion

            return Ok(result);
        }
        
        #endregion
    }
}