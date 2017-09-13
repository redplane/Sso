using System.Data.Entity;
using System.Linq;
using DbModel.Models.Entities;
using Shared.ViewModels.Photo;
using Sso.Interfaces.Repositories;

namespace Sso.Repositories
{
    public class RepositoryPhoto: ParentRepository<Photo>, IRepositoryPhoto
    {
        #region Methods

        /// <summary>
        /// Initiate repository with injectors.
        /// </summary>
        /// <param name="dbContext"></param>
        public RepositoryPhoto(DbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// Search for photos by using specific conditions.
        /// </summary>
        /// <param name="photos"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IQueryable<Photo> Search(IQueryable<Photo> photos, SearchPhotoViewModel conditions)
        {
            // Condition hasn't been initialized.
            if (conditions == null)
                return photos;

            // Indexes have been specified.
            if (conditions.Ids != null)
            {
                var indexes = conditions.Ids.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                if (indexes.Count > 0)
                    photos = photos.Where(x => indexes.Any(y => x.Id.Contains(y)));
            }

            // Urls have been specified.
            if (conditions.Urls != null)
            {
                var urls = conditions.Urls.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                if (urls.Count > 0)
                    photos = photos.Where(x => urls.Any(y => x.Url.Contains(y)));
            }

            // Titles have been specified.
            if (conditions.Titles != null)
            {
                var titles = conditions.Titles.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                if (titles.Count > 0)
                    photos = photos.Where(x => titles.Any(y => x.Title.Contains(y)));
            }

            // Descriptions have been specified.
            if (conditions.Descriptions != null)
            {
                var descriptions = conditions.Descriptions.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                if (descriptions.Count > 0)
                    photos = photos.Where(x => descriptions.Any(y => x.Description.Contains(y)));
            }

            // Width has been specified.
            if (conditions.Width != null)
            {
                var width = conditions.Width;
                if (width.From != null)
                    photos = photos.Where(x => x.Width >= width.From);
                if (width.To != null)
                    photos = photos.Where(x => x.Width <= width.To);
            }

            // Height has been specified.
            if (conditions.Height != null)
            {
                var height = conditions.Height;
                if (height.From != null)
                    photos = photos.Where(x => x.Height >= height.From);
                if (height.To != null)
                    photos = photos.Where(x => x.Height <= height.To);
            }

            // Created time has been specified.
            if (conditions.CreatedTime != null)
            {
                var createdTime = conditions.CreatedTime;
                if (createdTime.From != null)
                    photos = photos.Where(x => x.CreatedTime >= createdTime.From);
                if (createdTime.To != null)
                    photos = photos.Where(x => x.CreatedTime <= createdTime.To);
            }
            
            // Last modified time has been specified.
            if (conditions.LastModifiedTime != null)
            {
                var lastModifiedTime = conditions.LastModifiedTime;
                if (lastModifiedTime.From != null)
                    photos = photos.Where(x => x.LastModifiedTime >= lastModifiedTime.From);
                if (lastModifiedTime.To != null)
                    photos = photos.Where(x => x.LastModifiedTime <= lastModifiedTime.To);
            }

            return photos;
        }

        #endregion
    }
}