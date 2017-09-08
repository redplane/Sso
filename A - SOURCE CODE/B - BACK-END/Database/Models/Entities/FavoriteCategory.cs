using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Entities
{
    public class FavoriteCategory
    {
        #region Properties

        /// <summary>
        /// Id of category.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Email of follower.
        /// </summary>
        public string FollowerEmail { get; set; }

        /// <summary>
        /// Time when follow was created.
        /// </summary>
        public double FollowedTime { get; set; }

        #endregion

        #region Navigators

        /// <summary>
        /// Category which is being followed.
        /// </summary>
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Account which is following category.
        /// </summary>
        [ForeignKey(nameof(FollowerEmail))]
        public virtual Account Follower { get; set; }
        
        #endregion
    }
}