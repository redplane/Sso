using System.Linq;
using DbModel.Models.Entities;
using Shared.ViewModels.Photo;

namespace Sso.Interfaces.Repositories
{
    public interface IRepositoryPhoto: IParentRepository<Photo>
    {
        #region Methods

        /// <summary>
        /// Search photos by using specific conditions.
        /// </summary>
        /// <param name="photos"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        IQueryable<Photo> Search(IQueryable<Photo> photos, SearchPhotoViewModel conditions);

        #endregion
    }
}