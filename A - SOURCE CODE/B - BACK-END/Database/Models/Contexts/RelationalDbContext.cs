using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Database.Models.Entities;

namespace Database.Models.Contexts
{
    public class RelationalDbContext : DbContext
    {
        #region Properties

        /// <summary>
        /// List of account.
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// List of categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// List of category initialization.
        /// </summary>
        public DbSet<CategoryInitialization> CategoryInitializations { get; set; }

        /// <summary>
        /// List of favourite categories.
        /// </summary>
        public DbSet<FavoriteCategory> FavoriteCategories { get; set; }

        /// <summary>
        /// List of photos.
        /// </summary>
        public DbSet<Photo> Photos { get; set; }

        /// <summary>
        /// List of photo categorization.
        /// </summary>
        public DbSet<PhotoCategorization> PhotoCategorizations { get; set; }

        /// <summary>
        /// List of photo thumbnails.
        /// </summary>
        public DbSet<PhotoThumbnail> PhotoThumbnails { get; set; }

        /// <summary>
        /// List of photo uploads.
        /// </summary>
        public DbSet<PhotoUpload> PhotoUploads { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initiate database context with connection obtained from App.config.
        /// </summary>
        public RelationalDbContext() : base("iDistribution")
        {
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Override method which is called when model is being create.d
        /// </summary>
        /// <param name="dbModelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        {
            // Remove pluralizing naming convention.
            dbModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Initialize tables.
            InitAccount(dbModelBuilder);
            InitCategory(dbModelBuilder);
            InitCategoryInitialization(dbModelBuilder);
            InitFavouriteCategory(dbModelBuilder);
            InitPhoto(dbModelBuilder);
            InitPhotoUpload(dbModelBuilder);
            InitPhotoCategorization(dbModelBuilder);
            InitPhotoThumbnail(dbModelBuilder);

            base.OnModelCreating(dbModelBuilder);
        }

        /// <summary>
        /// Initiate account table.
        /// </summary>
        private void InitAccount(DbModelBuilder dbModelBuilder)
        {
            // Find entity.
            var entity = dbModelBuilder.Entity<Account>();

            // Email.
            entity.HasKey(x => x.Email);
            entity.Property(x => x.Email).IsRequired();
        }

        /// <summary>
        /// Initiate category table.
        /// </summary>
        /// <param name="dbModelBuilder"></param>
        private void InitCategory(DbModelBuilder dbModelBuilder)
        {
            // Find entity.
            var entity = dbModelBuilder.Entity<Category>();
            
            // Category index.
            entity.HasKey(x => x.Id);

            // Table name mapping.
            entity.ToTable(nameof(Category));
        }

        /// <summary>
        /// Initiate category initialization.
        /// </summary>
        /// <param name="dbModelBuilder"></param>
        private void InitCategoryInitialization(DbModelBuilder dbModelBuilder)
        {
            // Find entity.
            var entity = dbModelBuilder.Entity<CategoryInitialization>();
            
            // Primary key setup.
            entity.HasKey(x => new {x.CategoryId, x.CreatorEmail});
        }

        /// <summary>
        /// Initiate favourite category.
        /// </summary>
        /// <param name="dbModelBuilder"></param>
        private void InitFavouriteCategory(DbModelBuilder dbModelBuilder)
        {
            // Find entity.
            var entity = dbModelBuilder.Entity<FavoriteCategory>();

            // Primary key setup.
            entity.HasKey(x => new {x.CategoryId, x.FollowerEmail});
        }

        /// <summary>
        /// Initiate photo instance.
        /// </summary>
        /// <param name="dbModelBuilder"></param>
        private void InitPhoto(DbModelBuilder dbModelBuilder)
        {
            // Find entity.
            var entity = dbModelBuilder.Entity<Photo>();

            entity.HasKey(x => x.Id);
        }

        /// <summary>
        /// Initiate photo upload.
        /// </summary>
        /// <param name="dbModelBuilder"></param>
        private void InitPhotoUpload(DbModelBuilder dbModelBuilder)
        {
            // Find entity
            var entity = dbModelBuilder.Entity<PhotoUpload>();

            // Primary key setup.
            entity.HasKey(x => new {x.Id});
        }

        /// <summary>
        /// Initiate photo categorization.
        /// </summary>
        /// <param name="dbModelBuilder"></param>
        private void InitPhotoCategorization(DbModelBuilder dbModelBuilder)
        {
            // Find entity.
            var entity = dbModelBuilder.Entity<PhotoCategorization>();

            // Primary key setup.
            entity.HasKey(x => new {x.PhotoId, x.CategoryId});
        }

        /// <summary>
        /// Initiate photo thumbnail entity.
        /// </summary>
        /// <param name="dbModelBuilder"></param>
        private void InitPhotoThumbnail(DbModelBuilder dbModelBuilder)
        {
            // Find entity.
            var entity = dbModelBuilder.Entity<PhotoThumbnail>();

            // Primary key setup.
            entity.HasKey(x => new {x.Id});
        }

        #endregion
    }
}