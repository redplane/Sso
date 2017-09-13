using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shared.Enumerations;
using Shared.Models;

namespace Sso.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        #region Properties

        /// <summary>
        /// Repository which provides functions to access account database.
        /// </summary>
        IRepositoryAccount RepositoryAccounts { get; }

        /// <summary>
        /// Repository which provides functions to access category database.
        /// </summary>
        IRepositoryCategory RepositoryCategories { get; }

        /// <summary>
        /// Repository which provides functions to access favourite category database.
        /// </summary>
        IRepositoryFavouriteCategory RepositoryFavouriteCategories { get; }

        /// <summary>
        /// Repository which provides functions to access photo database.
        /// </summary>
        IRepositoryPhoto RepositoryPhotos { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Save change to database synchronously.
        /// </summary>
        /// <returns></returns>
        int Commit();

        /// <summary>
        /// Save change to database asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<int> CommitAsync();

        /// <summary>
        ///     Do pagination on a specific list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        IQueryable<T> Paginate<T>(IQueryable<T> list, Pagination pagination);

        /// <summary>
        ///     Do pagination on a specific list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="page"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        IQueryable<T> Paginate<T>(IQueryable<T> list, int page, int records);

        /// <summary>
        ///     Sort a list by using specific property enumeration.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="list"></param>
        /// <param name="sortDirection"></param>
        /// <param name="sortProperty"></param>
        /// <returns></returns>
        IQueryable<TEntity> Sort<TEntity>(IQueryable<TEntity> list, SortDirection sortDirection,
            Enum sortProperty);

        #endregion
    }
}