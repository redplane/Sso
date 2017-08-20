using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Database.Models.Entities;

namespace Shared.Models.Contexts
{
    public class RelationalDbContext : DbContext
    {
        #region Properties

        /// <summary>
        /// List of account.
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

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

            base.OnModelCreating(dbModelBuilder);
        }

        /// <summary>
        /// Initiate account table.
        /// </summary>
        private void InitAccount(DbModelBuilder dbModelBuilder)
        {
            // Find entity.
            var entity = dbModelBuilder.Entity<Account>();

            // Id.
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
        
        #endregion
    }
}