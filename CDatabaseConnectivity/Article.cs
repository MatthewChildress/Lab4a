/// File: Article.cs
/// Name: Joe Programmer
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 1
namespace edu.northeaststate.dlblair.cDatabaseConnectivity
{
    /// <summary>
    /// This class matches the database table Article for CRUD operations
    /// </summary>
    internal class Article
    {
        /// <summary>
        /// This is an entity class for the Article table in the database
        /// </summary>
        public int ArticleID { get; set; }
        public string? Title { get; set; }
        public DateTime PostDate { get; set; }
        public string? Summary { get; set; }
        public string? Link { get; set; }
        public string? OwnerGuid { get; set; }

        /// <summary>
        /// This method provides an easy to output the contents of the object
        /// mostly used for testing
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return "Article ID: " + ArticleID + " Title: " + "" + Title + " Post Date: " + PostDate + " Summary: " + Summary + " Link: " + Link + " OwnerGUID: " + OwnerGuid;
        }

    } // end class

} // end namespace
