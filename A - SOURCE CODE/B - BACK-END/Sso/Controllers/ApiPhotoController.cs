using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using DbModel.Models.Entities;
using Shared.Enumerations;
using Shared.Enumerations.Sortings;
using Shared.Models;
using Shared.ViewModels.Photo;
using Sso.Attributes;
using Sso.Interfaces.Repositories;
using Sso.Interfaces.Services;

namespace Sso.Controllers
{
    [RoutePrefix("api/photo")]
    [ApiAuthorize]
    public class ApiPhotoController : ApiController
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initiate controller with injectors.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="identityService"></param>
        /// <param name="systemTimeService"></param>
        public ApiPhotoController(IUnitOfWork unitOfWork,
            IIdentityService identityService,
            ISystemTimeService systemTimeService)
        {
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _systemTimeService = systemTimeService;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Search for favourite categories using specific conditions.
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        public async Task<IHttpActionResult> Search([FromBody] SearchPhotoViewModel conditions)
        {
            #region Parameters validation

            if (conditions == null)
            {
                conditions = new SearchPhotoViewModel();
                Validate(conditions);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            #endregion

            #region Photos search

            var photos = _unitOfWork.RepositoryPhotos.Search();
            photos = _unitOfWork.RepositoryPhotos.Search(photos, conditions);

            // Do sorting.
            var sorting = conditions.Sorting;
            if (sorting == null)
                photos = _unitOfWork.Sort(photos, SortDirection.Descending, PhotoPropertySort.CreatedTime);
            else
                photos = _unitOfWork.Sort(photos, sorting.Direction, sorting.Property);

            // Do pagination.
            var result = new SearchResult<IList<Photo>>();
            result.Total = await photos.CountAsync();
            result.Records = await _unitOfWork.Paginate(photos, conditions.Pagination).ToListAsync();

            #endregion

            return Ok(result);
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
    }
}