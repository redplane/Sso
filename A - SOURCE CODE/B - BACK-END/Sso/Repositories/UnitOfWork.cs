using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shared.Enumerations;
using Shared.Models;
using Sso.Interfaces.Repositories;

namespace Sso.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Constructor

        /// <summary>
        ///     Initiate unit of work with database context.
        /// </summary>
        /// <param name="dbContext"></param>
        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Context which is used for accessing into database.
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        ///     Repository which provides function to access into account database.
        /// </summary>
        private IRepositoryAccount _repositoryAccount;

        /// <summary>
        /// Repository which provides function to access into category database.
        /// </summary>
        private IRepositoryCategory _repositoryCategory;

        /// <summary>
        /// Repository which provides function to access into favorite categories database.
        /// </summary>
        private IRepositoryFavouriteCategory _repositoryFavouriteCategory;

        /// <inheritdoc />
        /// <summary>
        ///     Repository which provides function to access into account database.
        /// </summary>
        public IRepositoryAccount RepositoryAccounts
            => _repositoryAccount ?? (_repositoryAccount = new RepositoryAccount(_dbContext));

        /// <inheritdoc />
        /// <summary>
        /// Repository which provides functions to access into category database.
        /// </summary>
        public IRepositoryCategory RepositoryCategories =>
            _repositoryCategory ?? (_repositoryCategory = new RepositoryCategory(_dbContext));

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public IRepositoryFavouriteCategory RepositoryFavouriteCategories =>
            _repositoryFavouriteCategory ??
            (_repositoryFavouriteCategory = new RepositoryFavouriteCategory(_dbContext));

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// Commit changes to database.
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        /// <inheritdoc />
        /// <summary>
        /// Commit changes to database asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Do pagination on a specific list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public IQueryable<T> Paginate<T>(IQueryable<T> list, Pagination pagination)
        {
            if (pagination == null)
                return list;

            // Calculate page index.
            var index = pagination.Page - 1;
            if (index < 0)
                index = 0;

            return list.Skip(index * pagination.Records).Take(pagination.Records);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Do pagination on a specific list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="page"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        public IQueryable<T> Paginate<T>(IQueryable<T> list, int page, int records)
        {
            // Calculate page index.
            var index = page - 1;
            if (index < 0)
                index = 0;

            return list.Skip(index * records).Take(records);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Sort a list by using specific property enumeration.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="list"></param>
        /// <param name="sortDirection"></param>
        /// <param name="sortProperty"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Sort<TEntity>(IQueryable<TEntity> list, SortDirection sortDirection,
            Enum sortProperty)
        {
            string sortMethod;
            if (sortDirection == SortDirection.Ascending)
                sortMethod = "OrderBy";
            else
                sortMethod = "OrderByDescending";

            // Search parameter expression.
            var parameterExpression = Expression.Parameter(list.ElementType, "p");

            // Search name of property which should be used for sorting.
            var sortPropertyName = Enum.GetName(sortProperty.GetType(), sortProperty);
            if (string.IsNullOrEmpty(sortPropertyName))
                return list;

            // Search member expression.
            var memberExpression = Expression.Property(parameterExpression, sortPropertyName);

            var lamdaExpression = Expression.Lambda(memberExpression, parameterExpression);

            var methodCallExpression = Expression.Call(
                typeof(Queryable),
                sortMethod,
                new[] { list.ElementType, memberExpression.Type },
                list.Expression,
                Expression.Quote(lamdaExpression));

            return list.Provider.CreateQuery<TEntity>(methodCallExpression);
        }
        #endregion
    }
}