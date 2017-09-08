using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Entities
{
    public class Category
    {
        #region Properties

        /// <summary>
        /// Id of category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Category creator email address.
        /// </summary>
        public string CreatorEmail { get; set; }

        /// <summary>
        /// Time when category was created.
        /// </summary>
        public double CreatedTime { get; set; }

        /// <summary>
        /// Time when category was lastly modified.
        /// </summary>
        public double LastModifiedTime { get; set; }

        #endregion

        #region Navigators

        /// <summary>
        /// Category creator navigation property.
        /// </summary>
        [ForeignKey(nameof(CreatorEmail))]
        public virtual Account Creator { get; set; }

        #endregion
    }
}
