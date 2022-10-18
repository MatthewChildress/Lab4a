/// File: User.cs
/// Name: Joe Programmer
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 1
namespace edu.northeaststate.dlblair.cDatabaseConnectivity
{
    /// <summary>
    /// This class matches the database table JetUser for CRUD operations
    /// </summary>
    internal class User
    {
        /// <summary>
        /// This is an entity class for the JetUser table in the database
        /// </summary>
        public string? Guid { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }    
        public Boolean IsActive { get; set; }
        public int LevelID { get; set; }

        /// <summary>
        /// This method provides an easy to output the contents of the object
        /// mostly used for testing
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return base.ToString();
        }
    } // end class
} // end namespace
