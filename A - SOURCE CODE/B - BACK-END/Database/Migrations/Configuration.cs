using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using DbModel.Enumerations;
using DbModel.Models.Contexts;
using DbModel.Models.Entities;

namespace DbModel.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<RelationalDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RelationalDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var accounts = SeedAccounts(context);
            var categories = SeedCategories(accounts, context);
            SeedCategoryInitializations(categories, context);
            SeedFavouriteCategories(accounts, categories, context);
        }

        /// <summary>
        ///     Initiate accounts lít.
        /// </summary>
        private List<Account> SeedAccounts(RelationalDbContext context)
        {
            var index = 0;
            var accounts = new List<Account>();
            for (;index < 10; index++)
            {
                var account = new Account();
                account.Email = $"email{index}@gmail.com";
                account.Password = "200ceb26807d6bf99fd6f4f0d1ca54d4";
                account.JoinedTime = 1505224176595;
                account.Role = Role.Administrator;
                account.PhotoUrl = "http://http://via.placeholder.com/512x512";

                accounts.Add(account);
            }

            for (; index < 20; index++)
            {
                var account = new Account();
                account.Email = $"email{index}@gmail.com";
                account.Password = "200ceb26807d6bf99fd6f4f0d1ca54d4";
                account.JoinedTime = 1505224176595;
                account.Role = Role.Client;
                account.PhotoUrl = "http://http://via.placeholder.com/512x512";

                accounts.Add(account);
            }

            context.Accounts.AddRange(accounts);
            return accounts;
        }

        /// <summary>
        /// Initiate list of categories.
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<Category> SeedCategories(List<Account> accounts, RelationalDbContext context)
        {
            var randomizer = new Random();
            var categories = new List<Category>();
            for (var index = 0; index < 10; index++)
            {
                var accountIndex = randomizer.Next(2);
                var category = new Category();
                category.Id = index + 1;
                category.Name = $"Category {index}";
                category.CreatorEmail = accounts[accountIndex].Email;
                category.CreatedTime = 1505224176595;

                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            return categories;
        }

        /// <summary>
        /// Initiate list of category initializations.
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<CategoryInitialization> SeedCategoryInitializations(List<Category> categories,
            RelationalDbContext context)
        {
            var categoryInitializations = new List<CategoryInitialization>();
            foreach (var category in categories)
            {
                var categoryInitialization = new CategoryInitialization();
                categoryInitialization.CategoryId = category.Id;
                categoryInitialization.CreatorEmail = category.CreatorEmail;
                categoryInitialization.CreatedTime = categoryInitialization.CreatedTime;

                categoryInitializations.Add(categoryInitialization);
            }

            context.CategoryInitializations.AddRange(categoryInitializations);
            return categoryInitializations;
        }

        /// <summary>
        /// Initialize list of favourite categories.
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="categories"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<FavouriteCategory> SeedFavouriteCategories(List<Account> accounts, List<Category> categories, RelationalDbContext context)
        {
            var favouriteCategories = new List<FavouriteCategory>();
            foreach (var account in accounts)
            {
                foreach (var category in categories)
                {
                    var favouriteCategory = new FavouriteCategory();
                    favouriteCategory.CategoryId = category.Id;
                    favouriteCategory.FollowerEmail = account.Email;
                    favouriteCategory.FollowedTime = category.CreatedTime;

                    favouriteCategories.Add(favouriteCategory);
                }
            }
            
            context.FavoriteCategories.AddRange(favouriteCategories);
            return favouriteCategories;
        }
        

    }
}