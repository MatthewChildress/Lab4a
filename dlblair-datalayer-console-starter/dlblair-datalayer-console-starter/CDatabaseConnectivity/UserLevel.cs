/// File: UserLevel.cs
/// Name: Joe Programmer
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 1
namespace edu.northeaststate.dlblair.cDatabaseConnectivity
{
    /// <summary>
    /// This class matches the database table UserLevel for CRUD operations
    /// </summary>
    internal class UserLevel
    {
        /// <summary>
        /// This is an entity class for the UserLevel table in the database
        /// </summary>
        public int Id { get; set; }
        public string? Level { get; set; }

        /// <summary>
        /// This method provides an easy to output the contents of the object
        /// mostly used for testing
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return " ID: " + Id + " Level: " + Level;
        }
    } // end class
} // end namespace
