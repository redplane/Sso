using DbModel.Enumerations;

namespace DbModel.Models.Entities
{
    public class Account
    {
        #region Properties
        
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
        public Role Role { get; set; }

        /// <summary>
        /// Time when account had been created.
        /// </summary>
        public double JoinedTime { get; set; }

        /// <summary>
        /// Time when account information was lastly modified.
        /// </summary>
        public double? LastModifiedTime { get; set; }

        #endregion

    }
}