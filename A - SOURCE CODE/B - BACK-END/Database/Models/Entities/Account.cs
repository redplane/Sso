namespace Database.Models.Entities
{
    public class Account
    {
        #region Properties

        /// <summary>
        /// Id of account.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Email which is used for registering email.
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Account password (hash)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Url of photo.
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Account role.
        /// </summary>
        public int Role { get; set; }

        #endregion
    }
}