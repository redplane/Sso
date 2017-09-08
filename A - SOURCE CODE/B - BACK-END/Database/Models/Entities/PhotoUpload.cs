using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Entities
{
    public class PhotoUpload
    {
        #region Properties

        /// <summary>
        /// Id of photo upload.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of photo.
        /// </summary>
        public string PhotoId { get; set; }

        /// <summary>
        /// Email of uploader.
        /// </summary>
        public string UploaderEmail { get; set; }

        /// <summary>
        /// Time when photo was uploaded.
        /// </summary>
        public double UploadedTime { get; set; }

        #endregion

        #region Navigators

        /// <summary>
        /// Photo which was uploaded to server.
        /// </summary>
        [ForeignKey(nameof(PhotoId))]
        public virtual Photo Photo { get; set; }

        /// <summary>
        /// Who uploaded photo.
        /// </summary>
        [ForeignKey(nameof(UploaderEmail))]
        public virtual Account Uploader { get; set; }

        #endregion

    }
}