using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Entities
{
    public class PhotoThumbnail
    {
        #region Properties

        /// <summary>
        /// Id of photo thumbnail.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Photo index.
        /// </summary>
        public string PhotoId { get; set; }

        /// <summary>
        /// Url of thumbnail.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Width of thumbnail.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Height of thumbnail.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// When thumbnail was created.
        /// </summary>
        public double CreatedTime { get; set; }

        #endregion

        #region Navigators

        /// <summary>
        /// Photo which owns this thumbnail.
        /// </summary>
        [ForeignKey(nameof(PhotoId))]
        public virtual Photo Photo { get; set; }

        #endregion
    }
}