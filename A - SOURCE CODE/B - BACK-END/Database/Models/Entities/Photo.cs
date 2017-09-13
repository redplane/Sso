namespace DbModel.Models.Entities
{
    public class Photo
    {
        #region Properties

        /// <summary>
        /// Id of photo.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Url where photo is stored.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Title of photo.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Photo description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Photo width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Photo height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Time when photo was uploaded.
        /// </summary>
        public double CreatedTime { get; set; }

        /// <summary>
        /// Last time when photo was modified.
        /// </summary>
        public double? LastModifiedTime { get; set; }

        #endregion
    }
}