
using MySql.Data.MySqlClient;
using System.Data;

/// File: DataLayer.cs
/// Name: Joe Programmer
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 1
namespace edu.northeaststate.dlblair.cDatabaseConnectivity
{
    /// <summary>
    /// The data DataLayer class is used to hide implementation details of
    /// connecting to the database doing standard CRUD operations.
    /// 
    /// IMPORTANT NOTES:
    /// On the serverside, any input-output operations should be done asynchronously. This includes
    /// file and database operations. In doing so, the thread is freed up for the entire time a request
    /// is in flight. When a request executes the await code, the request thread is returned back to the
    /// thread pool. When the request is satisfied, the thread is taken from the thread pool and continues.
    /// This is all built into the .NET Core Framework making it very easy to implement into our code.
    /// 
    /// When throwing an exception from an ASYNC function the exception is never thrown. This makes sense
    /// because the function could possibly block and cause strange and unexpected behavior. Instead,
    /// we will LOG the exception.
    /// </summary>
    internal class DataLayer
    {

        #region "Properties"

        /// <summary>
        /// This variable holds the connection details
        /// such as name of database server, database name, username, and password.
        /// The ? makes the property nullable
        /// </summary>
        private string? connectionString = null;

        #endregion

        #region "Constructors"

        /// <summary>
        /// This is the default constructor and has the default 
        /// connection string specified 
        /// </summary>
        public DataLayer()
        {
            //preprocessor directives can help by using a debug build database environment for testing
            // while using a production database environment for production build.
#if (DEBUG)
            connectionString = @"server=localhost;uid=citc1317;pwd=Password1;database=neArticleJet";
#else
            connectionString = @"server=192.168.79.131;uid=citc1317;pwd=Password1;database=neArticleJet";
#endif
        }

        /// <summary>
        /// Parameterized Constructor passing in a connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public DataLayer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion

        #region "User Database Operations"

        /// <summary>
        /// Get a user by using the user GUID (key)
        /// returns a single User object or a null User
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<User>? GetAUserByGUIDAsync(string? GUID)
        {
           
            User? user = null;

            try
            {

                //test for guid to be null and throw and exception back to the caller
                if (GUID == null)
                {
                    throw new ArgumentNullException("GUID can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAUserByGUID", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("GUID", GUID));

                // execute the command which returns a data reader object
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // if the reader contains a data set, load a local user object
                if (rdr.Read())
                {
                    user = new User();
                    user.Guid = (string?)rdr.GetValue(0);
                    user.Email = (string?)rdr.GetValue(1);
                    user.FirstName = (string?)rdr.GetValue(2);
                    user.LastName = (string?)rdr.GetValue(3);
                    UInt64 test = (UInt64)rdr.GetValue(4);
                    if (test == 0)
                        user.IsActive = true;
                    else
                        user.IsActive = false;
                    user.LevelID = (int)rdr.GetValue(5);
                }
            }
            catch(ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }                       
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return user;

        } // end GetAUserByGUID

        /// <summary>
        /// Get a user by Username and Password (key)
        /// returns a single User object or a null User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<User>? GetAUserByUserPassAsync(string? username, string? password)
        {            

            User? user = null; // new syntax can replace User? user = new User()

            try
            {
                if (username == null || password == null)
                {
                    throw new ArgumentNullException("Username or Password can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAUserByUserAndPass", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("userEmail", username));
                cmd.Parameters.Add(new MySqlParameter("password", password));

                // execute the command which returns a data reader object
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // if the reader contains a data set, load a local user object
                if (rdr.Read())
                {
                    user = new User();
                    user.Guid = (string?)rdr.GetValue(0);
                    user.Email = (string?)rdr.GetValue(1);

                    user.FirstName = (string?)rdr.GetValue(2);
                    user.LastName = (string?)rdr.GetValue(3);
                    UInt64 test = (UInt64)rdr.GetValue(4);
                    if (test == 1)
                        user.IsActive = true;
                    else
                        user.IsActive = false;
                    user.LevelID = (int)rdr.GetValue(5);
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return user;

        } // end GetAUserByUserPassAsync

        /// <summary>
        /// Get all the active users in the database and return as a List of Users.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>>? GetAllUsersAsync()
        {

            // instantiate a new empty List of Users
            List<User> users = new List<User>();

            try
            {
                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllUsers", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // execute the command which returns a data reader object
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results adding a new user to a generic List of users
                while (rdr.Read())
                {
                    User user = new User();
                    user.Guid = (string?)rdr.GetValue(0);
                    user.Email = (string?)rdr.GetValue(1);
                    user.Password = (string?)rdr.GetValue(2);
                    user.FirstName = (string?)rdr.GetValue(3);
                    user.LastName = (string?)rdr.GetValue(4);
                    UInt64 test = (UInt64)rdr.GetValue(5);
                    if (test == 1)
                        user.IsActive = true;
                    else
                        user.IsActive = false;
                    user.LevelID = (int)rdr.GetValue(6);

                    users.Add(user);
                }
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return users;

        } // end GetAllUsersAsync

        /// <summary>
        /// Update a User by their GUID key to an active state of 0 or 1.
        /// Return 0 if no row in the database is modified.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> PutAUsersStateAsync(string? guid, bool? state)
        {
            // integer value that shows if a row is updated in the database
            int results = 0;           

            try
            {
                // check for null parameters being passed in
                if (guid == null || state == null)
                {
                    throw new ArgumentNullException("GUID and State can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spPutUserActiveState", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("GUID", guid));
                cmd.Parameters.Add(new MySqlParameter("state", state));

                // execute the none query command that returns an integer for number of rows changed
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;

        } // end PutAUsersStateAsync

        /// <summary>
        /// Insert a new User into the database.
        /// Return 0 if no row is modified.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? PostANewUserAsync(User? user)
        {
            // local variable to return the row count altered
            int results = 0;
          
            try
            {
                // check for User to be null
                if (user == null)
                {
                    throw new ArgumentNullException("New user can not be null.");
                }

                // using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spPostNewJetUser", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("GUID", user.Guid));
                cmd.Parameters.Add(new MySqlParameter("mail", user.Email));
                cmd.Parameters.Add(new MySqlParameter("pass", user.Password));
                cmd.Parameters.Add(new MySqlParameter("fname", user.FirstName));
                cmd.Parameters.Add(new MySqlParameter("lname", user.LastName));
                cmd.Parameters.Add(new MySqlParameter("isActive", user.IsActive));
                cmd.Parameters.Add(new MySqlParameter("userLevel", user.LevelID));

                // execute the command
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;

        } // end PostANewUserAsync

        #endregion

        #region "Article Database Operations"

        /// <summary>
        /// Get all the Articles in the database and return as a List of Articles.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Article>>? GetAllArticlesAsync()
        {

            // Instantiate new List of Articles
            List<Article> articles = new(); // new way to instantiate, old way: List<Article> articles = new List<Article>()

            try
            {
                // using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllArticles", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // execute the command
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results adding a new Article to the List to return
                while (rdr.Read())
                {
                    Article article = new Article();
                    article.ArticleID = (int)rdr.GetValue("ArticleID");
                    article.Title = (string?)rdr.GetValue("Title");
                    article.PostDate = (DateTime)rdr.GetValue("Postdate");
                    article.Summary = (string?)rdr.GetValue("Summary");
                    article.Link = (string?)rdr.GetValue("Link");
                    article.OwnerGuid = (string?)rdr.GetValue("OwnerGUID");

                    articles.Add(article);
                }
            }            
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return articles;

        } // end GetAllArticlesAsync

        /// <summary>
        /// Get a single Article in the database by the ArticleID (auto increment int).
        /// If none are found, return a null object.
        /// </summary>
        /// <param name="ArticleID"></param>
        /// <returns></returns>
        public async Task<Article>? GetArticlesByArticleIDAsync(int? ArticleID)
        {
            // instantiate new Article object
            Article? article = null;

            try
            {
                if(ArticleID == null)
                {
                    throw new ArgumentNullException("Article ID can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetArticleByArticleID", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("ID", ArticleID));

                // execute the command and return a data reader
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // Load a new article and return to caller
                if (rdr.Read())
                {
                    article = new Article();
                    article.Title = (string?)rdr.GetValue("Title");
                    article.PostDate = (DateTime)rdr.GetValue("Postdate");
                    article.Summary = (string?)rdr.GetValue("Summary");
                    article.Link = (string?)rdr.GetValue("Link");
                    article.OwnerGuid = (string?)rdr.GetValue("OwnerGUID");
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return article;

        } // end GetArticlesByArticleIDAsync

        /// <summary>
        /// Get all Articles that are equal to or greater than the date passed in.
        /// If none are found, return list with count 0.
        /// </summary>
        /// <param name="articleDate"></param>
        /// <returns></returns>
        public async Task<List<Article>>? GetArticlesAfterDateAsync(DateTime? articleDate)
        {            

            // Instantiate new List of Articles
            List<Article> articles = new();

            try
            {
                // check for null date
                if (articleDate == null)
                {
                    throw new ArgumentNullException("The Article data can not be null");
                }
                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetArticlesAfterDate", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("articleDate", articleDate));

                // execute the command that returns a data reader
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results loading the Article List<>
                while (rdr.Read())
                {
                    Article article = new Article();

                    article.ArticleID = (int)rdr.GetValue("ArticleID");
                    article.Title = (string?)rdr.GetValue("Title");
                    article.Summary = (string?)rdr.GetValue("Summary");
                    article.Link = (string?)rdr.GetValue("Link");
                    article.OwnerGuid = (string?)rdr.GetValue("OwnerGUID");

                    articles.Add(article);
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return articles;

        } // end GetArticlesAfterDateAsync

        /// <summary>
        /// Adds a new Article to the database.
        /// Returns an integer for the number of rows affected.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? PostANewArticleAsync(Article? article)
        {

            // local variable for returning row count
            int results = 0;            

            try
            {
                // check for null article being passed in
                if (article == null)
                {
                    throw new ArgumentNullException("New article can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // Open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spPostANewArticle", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // load all parameters to pass to stored procedure
                cmd.Parameters.Add(new MySqlParameter("articleTitle", article.Title));
                cmd.Parameters.Add(new MySqlParameter("articleDate", article.PostDate));
                cmd.Parameters.Add(new MySqlParameter("articleSummary", article.Summary));
                cmd.Parameters.Add(new MySqlParameter("articleLink", article.Link));
                cmd.Parameters.Add(new MySqlParameter("articleOwner", article.OwnerGuid));

                // execute command and get number of rows affected
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;

        } // end PostANewArticleAsync

        /// <summary>
        /// Update an existing Article changing everything except the ArticleID (key).
        /// Returns an integer showing how many rows in the database was affected.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? PutAnArticleAsync(Article? article)
        {

            // local variable to return the number of rows affected in the database
            int results = 0;

            try
            {

                // check for null parameter
                if (article == null)
                {
                    throw new ArgumentNullException("Article can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spPutAnArticle", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure                
                cmd.Parameters.Add(new MySqlParameter("ID", article.ArticleID));
                cmd.Parameters.Add(new MySqlParameter("articleTitle", article.Title));
                cmd.Parameters.Add(new MySqlParameter("articleDate", article.PostDate));
                cmd.Parameters.Add(new MySqlParameter("articleSummary", article.Summary));
                cmd.Parameters.Add(new MySqlParameter("articleLink", article.Link));
                cmd.Parameters.Add(new MySqlParameter("articleOwner", article.OwnerGuid));

                // execute the command and get number of rows affected
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;

        } // end PutAnArticleAsync

        /// <summary>
        /// Delete a single Article.
        /// Returns an integer showing how many rows in the database was affected.
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? DeleteAnArticleAsync(int? articleID)
        {
            // local variable for returning total rows affected in the database
            int results = 0;            

            try
            {
                // check for null id being passed in
                if (articleID == null)
                {
                    throw new ArgumentNullException("New article can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spDeleteArticleByArticleID", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure                
                cmd.Parameters.Add(new MySqlParameter("ID", articleID));

                // execute the command that returns the number of rows affected                
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;
        } // end DeleteAnArticleAsync

        #endregion

        #region "Rating Database Operations"

        /// <summary>
        /// Get all the Ratings in the database and return as a List.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Rating>>? GetAllRatingsAsync()
        {
            // Instantiate Ratings List
            List<Rating> ratings = new();

            try
            {
                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllRatings", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // execute the command
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results loading Rating List
                while (rdr.Read())
                {
                    Rating rating = new();

                    //rating.RatingID = (int)rdr.GetValue("RatingID");
                    rating.ArticleID = (int)rdr.GetValue("ArticleID");
                    rating.UserID = (string?)rdr.GetValue("UserID");
                    rating.UserRating = (float)rdr.GetValue("Rating");

                    ratings.Add(rating);
                }
            }
            catch (MySqlException)
            {
                // preserves the stack trace
                // consider logging error message
                throw;
            }
            catch (Exception)
            {
                // preserves the stack trace
                // consider logging error message
                throw;
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return ratings;

        } // end GetAllRatingsAsync

        /// <summary>
        /// Get all the Ratings in the database by the User that submitted the rating and return as a List.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public async Task<List<Rating>>? GetAllRatingsByAUserAsync(string? GUID)
        {
            
            // Instantiate new List of Ratings
            List<Rating> ratings = new();

            try
            {
                // check parameter for null
                if (GUID == null)
                {
                    throw new ArgumentNullException("GUID can not be null");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection 
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllRatingsByAUser", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("ID", GUID));

                // execute the command
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results loading the Ratings List
                while (rdr.Read())
                {
                    Rating rating = new();
                    
                    rating.ArticleID = (int)rdr.GetValue("ArticleID");
                    rating.UserID = (string?)rdr.GetValue("UserID");
                    rating.UserRating = (float)rdr.GetValue("Rating");

                    ratings.Add(rating);
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return ratings;

        } // end GetAllRatingsByAUserAsync

        /// <summary>
        /// Get all the Ratings in the database by the Rating (ex: 4) and return as a List.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <param name="ratingNum"></param>
        /// <returns></returns>
        public async Task<List<Rating>>? GetAllRatingsByARatingAsync(int? ratingNum)
        {
                        
            // Instantiage ratings List
            List<Rating> ratings = new();

            // TODO - add code for this method

            return ratings;

        }

        /// <summary>
        /// Get all the Ratings in the database for an Article and return as a List.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <param name="ArticleID"></param>
        /// <returns></returns>
        public async Task<List<Rating>>? GetAllRatingsByArticleIDAsync(int? ArticleID)
        {
            
            // Instantiate Ratings List
            List<Rating> ratings = new();

            // TODO - add code for this method

            return ratings;

        } // end GetAllRatingsByArticleIDAsync

        /// <summary>
        /// Submit a new Rating for an Article.
        /// Return an integer for how many rows were affected in the database.
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? PostANewRatingAsync(Rating? rating)
        {
            
            // returns the affected row count from the database
            int results = 0;

            // TODO - add code for this method

            return results;

        } // end PostANewRatingAsync

        /// <summary>
        /// Update a new Rating for an Article.
        /// Return an integer for how many rows were affected in the database.
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? PutARatingAsync(Rating? rating)
        {
            
            // returns the affected row count from the database
            int results = 0;

            // TODO - add code for this method

            return results;

        } // end PutARatingAsync

        /// <summary>
        /// Delete a new Rating for an Article.
        /// Return an integer for how many rows were affected in the database.
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? DeleteARatingAsync(Rating? rating)
        {

            // returns the affected row count from the database
            int results = 0;

            // TODO - add code for this method

            return results;
        } // end DeleteARatingAsync

        #endregion

        #region "UserLevel Database Operations"

        /// <summary>
        /// Get all UserLevels and return a List of all UserLevels or an empty List (Count == 0)
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserLevel>>? GetAllUserLevelsAsync()
        {
            
            // Instantiate UserLevel List
            List<UserLevel> userLevels = new List<UserLevel>();

            try
            {
                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllUserLevels", conn);
                
                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // execute the command returning a data reader
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results
                while (rdr.Read())
                {
                    UserLevel ul = new UserLevel();
                    ul.Id = (int)rdr.GetValue(0);
                    ul.Level = (string?)rdr.GetValue(1);

                    userLevels.Add(ul);
                }
            }
            catch (MySqlException)
            {
                // preserves the stack trace
                // consider logging error message
                throw;
            }
            catch (Exception)
            {
                // preserves the stack trace
                // consider logging error message
                throw;
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return userLevels;

        } // end GetAllUserLevelsAsync

        #endregion
    } // end class DataLayer

} // end namespace
