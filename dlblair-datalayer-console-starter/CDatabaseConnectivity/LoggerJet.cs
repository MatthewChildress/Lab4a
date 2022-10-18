/// File: DataLayer.cs
/// Name: Joe Programmer
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 1
namespace edu.northeaststate.dlblair.cDatabaseConnectivity
{

    /// <summary>
    /// This class can be used to create a log of error messages.
    /// By default, it logs to the same directory where the executable 
    /// is running. That directory can be changed by using the 
    /// parameterized constructor.
    /// </summary>
    internal class LoggerJet
    {
        /// <summary>
        /// Default constructor sets the log directory to where ever
        /// the executable is running.
        /// </summary>
        private readonly string? filePath;
        public LoggerJet()
        {
            filePath = "./logger.txt";
        }

        /// <summary>
        /// Parameterized constructor allow the user to set the
        /// log director and file name to another location.
        /// The parameter can not be null.
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="Exception"></exception>
        public LoggerJet(string? filePath)
        {
            if (filePath == null)
            {
                throw new Exception("File path can not be null");
            }
            this.filePath = filePath;
        }

        /// <summary>
        /// Writes a message to the designated log file
        /// </summary>
        /// <param name="msg"></param>
        /// <exception cref="Exception"></exception>
        public async void WriteLog(string? msg)
        {
            if (filePath == null)
            {
                throw new Exception("File path can not be null.");
            }
            //using guarantees stream resources are released 
            //also not the new way an object can be instantiated
            using StreamWriter writer = new(filePath, true);
            await writer.WriteAsync(DateTime.Now + " :: " + msg + "\n");

        }

        /// <summary>
        /// Reads the entire log from the designated log location
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string>? ReadLog()
        {
            if (filePath == null)
            {
                throw new Exception("File path can not be null.");
            }
            //using guarantees stream resources are released
            //also not the new way an object can be instantiated
            using StreamReader reader = new(this.filePath, true);
            string contents = await reader.ReadToEndAsync();
            return contents;
        }

        /// <summary>
        /// Reads the entire log from the designated log location
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string>? ReadLog(string? filePath)
        {
            if (filePath == null)
            {
                throw new Exception("File path can not be null.");
            }
            //using guarantees stream resources are released
            //also not the new way an object can be instantiated
            using StreamReader reader = new(filePath, true);
            string contents = await reader.ReadToEndAsync();
            return contents;
        }

    } // end class
} // end namespace
