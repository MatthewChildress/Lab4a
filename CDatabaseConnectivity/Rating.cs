/// File: User.cs
/// Name: Joe Programmer
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 1
namespace edu.northeaststate.dlblair.cDatabaseConnectivity
{
    /// <summary>
    /// Entity object to match database table Rating
    /// </summary>
    internal class Rating
    {
        /// <summary>
        /// Class Properties, entity to match database table Rating
        /// </summary>
        public int ArticleID { get; set; }
        public string? UserID { get; set; }
        public float UserRating { get; set; }

        /// <summary>
        /// Output properties for testing, mostly
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return "Artile ID: " + ArticleID + " User ID: " + UserID + " User Rating: " + UserRating;
        }

    } // end class

} // end namespapce
