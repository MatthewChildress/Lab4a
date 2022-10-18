
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
/// File: Driver.cs
/// Name: Joe Programmer
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project Name
namespace edu.northeaststate.dlblair.cDatabaseConnectivity
{
    /// <summary>
    /// This class is the driver class to test the DatabaseLayer class.
    /// It will test a subset of CRUD operations for the ArticleJet database.
    /// Specifically, it will test retrieving all active articles, and
    /// retrieving a single article by article ID.
    /// </summary>
    internal class DatabaseDriver
    {
        /// <summary>
        /// The Main method is the execution entry point for this C# program.
        /// You should refresh the database each time you run a test.
        /// </summary>
        /// The args string array allows the user to pass in string arguments into the Main method
        /// args string array parameter.
        /// <param name="args"></param>
        static Task Main(string[] args)
        {

            var expectedOutcome = 0;
            var unExpectedOutcome = 0;
            var appVersionIfo = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            Console.WriteLine();
            Console.WriteLine("Testing: " + appVersionIfo.Name + " " + appVersionIfo.Version);
            Console.WriteLine("======================================\n\n");

            try
            {
                DataLayer dl = new();

                #region "JetUser table tests"
                
                // good GUID
                Task<User>? user1 = dl.GetAUserByGUIDAsync("22541e71-ceba-42c9-8908-eee3792543d1");
                if (user1.Result != null)
                {
                    Console.WriteLine("GetAUserByGUIDAsync passed");
                    expectedOutcome++;
                }
                else
                {
                    Console.WriteLine("GetAUserByGUIDAsync failed");
                    unExpectedOutcome++;
                }

                // bad GUID
                Task<User>? user2 = dl.GetAUserByGUIDAsync("22541e71-ceba-42c9-8908");
                if (user2.Result != null)
                {
                    Console.WriteLine("GetAUserByGUIDAsync failed");
                    unExpectedOutcome++;
                }
                else
                {
                    Console.WriteLine("GetAUserByGUIDAsync passed");
                    expectedOutcome++;
                }

                // null argument
                Task<User>? userNull = dl.GetAUserByGUIDAsync(null);
                if (userNull.Result != null)
                {
                    Console.WriteLine("GetAUserByGUIDAsync failed");
                    unExpectedOutcome++;
                }
                else
                {
                    Console.WriteLine("GetAUserByGUIDAsync passed");
                    expectedOutcome++;
                }

                
                SHA256 sha256Hash = SHA256.Create();

                Task<User>? user3;
                //correct user info
                user3 = dl.GetAUserByUserPassAsync("rbhayes@northeaststate.edu", GetHash(sha256Hash, "Password4"));
                if (user3 != null)
                {
                    Console.WriteLine("GetAUserByUserPassAsync passed");
                    expectedOutcome++;
                }
                else
                {
                    Console.WriteLine("GetAUserByUserPassAsync failed");
                    unExpectedOutcome++;
                }

                Task<User>? user4;
                //incorrect user info
                user4 = dl.GetAUserByUserPassAsync("rbhayes@northeaststate.edu", GetHash(sha256Hash, "WrongPassword"));
                if (user4.Result != null)
                {
                    Console.WriteLine("GetAUserByUserPassAsync failed");
                    unExpectedOutcome++;
                }
                else
                {
                    Console.WriteLine("GetAUserByUserPassAsync passed");
                    expectedOutcome++;
                }

                Task<User>? user5;
                //null arguments
                user5 = dl.GetAUserByUserPassAsync(null, null);
                if (user5.Result != null)
                {
                    Console.WriteLine("GetAUserByUserPassAsync failed");
                    unExpectedOutcome++;
                }
                else
                {
                    Console.WriteLine("GetAUserByUserPassAsync passed");
                    expectedOutcome++;
                }

                // no arguments - good test
                Task<List<User>> users = dl.GetAllUsersAsync();
                if (users.Result.Count != 0)
                {
                    Console.WriteLine("GetAllUsersAsync passed");
                    expectedOutcome++;
                }
                else
                {
                    Console.WriteLine("GetAllUsersAsync failed");
                    unExpectedOutcome++;
                }

                // good GUID set to active
                Task<int> userUpdate1 = dl.PutAUsersStateAsync("22541e71-ceba-42c9-8908-eee3792543d1", true);
                if (userUpdate1.Result == 1)
                {
                    Console.WriteLine("PutAUsersStateAsync passed");
                    expectedOutcome++;
                }
                else
                {
                    Console.WriteLine("PutAUsersStateAsync failed");
                    unExpectedOutcome++;
                }

                // good GUID set to inactive
                Task<int> userUpdate2 = dl.PutAUsersStateAsync("22541e71-ceba-42c9-8908-eee3792543d1", false);
                if (userUpdate2.Result == 1)
                {
                    Console.WriteLine("PutAUsersStateAsync passed");
                    expectedOutcome++;
                }
                else
                {
                    Console.WriteLine("PutAUsersStateAsync failed");
                    unExpectedOutcome++;
                }

                // bad GUID
                Task<int> userUpdate3 = dl.PutAUsersStateAsync("22541e71-ceba-42c9-8908", false);
                if (userUpdate3.Result == 0)
                {
                    Console.WriteLine("PutAUsersStateAsync passed");
                    expectedOutcome++;
                }
                else
                {
                    Console.WriteLine("PutAUsersStateAsync failed");
                    unExpectedOutcome++;
                }

                // null arguments
                Task<int> userUpdate4 = dl.PutAUsersStateAsync(null, null);
                if (userUpdate4.Result == 0)
                {
                    Console.WriteLine("PutAUsersStateAsync passed");
                    expectedOutcome++;
                }
                else
                {
                    Console.WriteLine("PutAUsersStateAsync failed");
                    unExpectedOutcome++;
                }

                // create test user                
                User newUser = new User();
                var guid = Guid.NewGuid().ToString();
                newUser.Guid = guid;
                newUser.Email = "jpen@northeaststate.edu";
                newUser.Password = GetHash(sha256Hash, "Password1");
                newUser.FirstName = "Joe";
                newUser.LastName = "Pen";
                newUser.IsActive = true;
                newUser.LevelID = 1;

                // add new user
                Task<int>? res = dl.PostANewUserAsync(newUser);
                int numRowsInserted;
                if (res.Result == 0)
                {
                    numRowsInserted = res.Result;
                    Console.WriteLine("PostANewUserAsync number inserted new user " + numRowsInserted + " failed");
                    unExpectedOutcome++;
                }
                else
                {
                    numRowsInserted = res.Result;
                    Console.WriteLine("PostANewUserAsync number inserted new user " + numRowsInserted + " passed");
                    expectedOutcome++;
                }

                // add duplicate user
                Task<int>? res2 = dl.PostANewUserAsync(newUser);
                if (res2.Result != 0)
                {
                    numRowsInserted = res2.Result;
                    Console.WriteLine("PostANewUserAsync number inserted duplicate user " + numRowsInserted + " failed");
                    unExpectedOutcome++;
                }
                else
                {
                    numRowsInserted = res2.Result;
                    Console.WriteLine("PostANewUserAsync number inserted  duplicate user " + numRowsInserted + " passed");
                    expectedOutcome++;
                }

                // add null user
                Task<int>? res3 = dl.PostANewUserAsync(null);
                if (res3.Result != 0)
                {
                    numRowsInserted = res3.Result;
                    Console.WriteLine("PostANewUserAsync number inserted  null user " + numRowsInserted + " failed");
                    unExpectedOutcome++;
                }
                else
                {
                    numRowsInserted = res3.Result;
                    Console.WriteLine("PostANewUserAsync number inserted null user " + numRowsInserted + " passed");
                    expectedOutcome++;
                }

                #endregion

                #region "Article table tests"

                // not argument - good test                
                Task<List<Article>>? articles = dl.GetAllArticlesAsync();
                if (articles != null)
                {
                    if (articles.Result.Count > 0)
                    {
                        Console.WriteLine("GetAllArticlesAsync retrieved " + articles.Result.Count + " records passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetAllArticlesAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetAllArticlesAsync failed");
                    unExpectedOutcome++;
                }

                // good argument
                Task<Article>? article = dl.GetArticlesByArticleIDAsync(1);
                if (article != null)
                {
                    if (article.Result != null)
                    {
                        Console.WriteLine("GetArticlesByArticleIDAsync retrieved a record passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetArticlesByArticleIDAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetArticlesByArticleIDAsync failed");
                    unExpectedOutcome++;
                }

                // bad argument
                Task<Article>? articleBad = dl.GetArticlesByArticleIDAsync(0);
                if (articleBad != null)
                {
                    if (articleBad.Result != null)
                    {
                        Console.WriteLine("GetArticlesByArticleIDAsync failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetArticlesByArticleIDAsync passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetArticlesByArticleIDAsync failed");
                    unExpectedOutcome++;
                }

                // null argument
                Task<Article>? articleBad2 = dl.GetArticlesByArticleIDAsync(null);
                if (articleBad != null)
                {
                    if (articleBad.Result == null)
                    {
                        Console.WriteLine("GetArticlesByArticleIDAsync passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetArticlesByArticleIDAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetArticlesByArticleIDAsync failed");
                    unExpectedOutcome++;
                }
                
                // good argument
                Task<List<Article>>? articles2 = dl.GetArticlesAfterDateAsync(new DateTime(2022, 08, 01));
                if (articles2 != null)
                {
                    if (articles2.Result.Count > 0)
                    {
                        Console.WriteLine("GetArticlesAfterDateAsync passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetArticlesAfterDateAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetArticlesAfterDateAsync failed");
                    unExpectedOutcome++;
                }

                // null argument
                Task<List<Article>>? articles3 = dl.GetArticlesAfterDateAsync(null);
                if (articles3 != null)
                {
                    if (articles3.Result.Count != 0)
                    {
                        Console.WriteLine("GetArticlesAfterDateAsync failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetArticlesAfterDateAsync passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetArticlesAfterDateAsync failed");
                    unExpectedOutcome++;
                }

                // create test article
                Article articleNew = new()
                {
                    Title = "Testing 1",
                    PostDate = DateTime.Now,
                    Summary = "Summary for new Article",
                    Link = "https://www.newlink.org/",
                    OwnerGuid = "22541e71-ceba-42c9-8908-eee3792543d1"
                };

                // good argument
                Task<int>? results = dl.PostANewArticleAsync(articleNew);
                if (results != null)
                {
                    if (results.Result > 0)
                    {
                        Console.WriteLine("PostANewArticleAsync Inserted new Article passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("PostANewArticleAsync Inserted new Article failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("PostANewArticleAsync Inserted new Article failed");
                    unExpectedOutcome++;
                }

                // try bad (duplicate) article
                Task<int>? results2 = dl.PostANewArticleAsync(articleNew);
                if (results2 != null)
                {
                    if (results2.Result > 0)
                    {
                        Console.WriteLine("PostANewArticleAsync Inserted duplicate Article failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("PostANewArticleAsync Inserted duplicate Article passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("PostANewArticleAsync Inserted duplicate Article failed");
                    unExpectedOutcome++;
                }

                // try null article
                Task<int>? results3 = dl.PostANewArticleAsync(null);
                if (results3 != null)
                {
                    if (results3.Result > 0)
                    {
                        Console.WriteLine("PostANewArticleAsync Inserted new null failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("PostANewArticleAsync Inserted new null passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("PostANewArticleAsync Inserted new null failed");
                    unExpectedOutcome++;
                }

                // create another test article
                Article article2 = new()
                {
                    ArticleID = 1,
                    Title = "Testing ALSO 2",
                    PostDate = DateTime.Now,
                    Summary = "Summary for UPDATED ALSO article",
                    Link = "https://www.changeURL.org/TEST",
                    OwnerGuid = "22541e71-ceba-42c9-8908-eee3792543d1"
                };

                // try to update existing article
                Task<int>? result2 = dl.PutAnArticleAsync(article2);
                if (result2 != null)
                {
                    if (result2.Result == 1)
                    {
                        Console.WriteLine("PutAnArticleAsync updated passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("PutAnArticleAsync updated failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("PutAnArticleAsync updated failed");
                    unExpectedOutcome++;
                }

                // changed ArticleID to something invalid
                article2.ArticleID = 0;

                // try to update article that doesn't exist
                Task<int>? result3 = dl.PutAnArticleAsync(article2);
                if (result3 != null)
                {
                    if (result3.Result == 1)
                    {
                        Console.WriteLine("PutAnArticleAsync updated failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("PutAnArticleAsync updated passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("PutAnArticleAsync updated failed");
                    unExpectedOutcome++;
                }

                // null argument
                Task<int>? result4 = dl.PutAnArticleAsync(null);
                if (result4 != null)
                {
                    if (result4.Result == 1)
                    {
                        Console.WriteLine("PutAnArticleAsync updated failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("PutAnArticleAsync updated passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("PutAnArticleAsync updated failed");
                    unExpectedOutcome++;
                }

                // changed ArticleID back to what was posted
                article2.ArticleID = 1;
                // good argument
                Task<int>? result5 = dl.DeleteAnArticleAsync(article2);
                if (result5 != null)
                {
                    if (result5.Result != 1)
                    {
                        Console.WriteLine("DeleteAnArticleAsync failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("DeleteAnArticleAsync passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("DeleteAnArticleAsync failed");
                    unExpectedOutcome++;
                }

                // record already deleted
                result5 = dl.DeleteAnArticleAsync(article2);
                if (result5 != null)
                {
                    if (result5.Result != 1)
                    {
                        Console.WriteLine("DeleteAnArticleAsync passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("DeleteAnArticleAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("DeleteAnArticleAsync failed");
                    unExpectedOutcome++;
                }

                // null argument
                result5 = dl.DeleteAnArticleAsync(null);
                if (result5 != null)
                {
                    if (result5.Result != 1)
                    {
                        Console.WriteLine("DeleteAnArticleAsync passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("DeleteAnArticleAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("DeleteAnArticleAsync failed");
                    unExpectedOutcome++;
                }

                #endregion

                #region "Rating table tests"
                // get all ratings
                Task<List<Rating>>? ratings = dl.GetAllRatingsAsync();
                if (ratings != null)
                {
                    if (ratings.Result.Count > 0)
                    {
                        Console.WriteLine("GetAllRatingsAsync passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetAllRatingsAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetAllRatingsAsync failed");
                    unExpectedOutcome++;
                }

                // good argument
                Task<List<Rating>>? ratings2 = dl.GetAllRatingsByAUserAsync("f2161cae-5fe4-49d6-b61a-73203d94bdf7");
                if (ratings2 != null)
                {
                    if (ratings2.Result.Count > 0)
                    {
                        Console.WriteLine("GetAllRatingsByAUserAsync passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetAllRatingsByAUserAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetAllRatingsByAUserAsync failed");
                    unExpectedOutcome++;
                }

                // bad argument
                Task<List<Rating>>? ratings3 = dl.GetAllRatingsByAUserAsync("badkey");
                if (ratings3 != null)
                {
                    if (ratings3.Result.Count > 0)
                    {
                        Console.WriteLine("GetAllRatingsByAUserAsync failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetAllRatingsByAUserAsync passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetAllRatingsByAUserAsync failed");
                    unExpectedOutcome++;
                }

                // null argument
                Task<List<Rating>>? ratings4 = dl.GetAllRatingsByAUserAsync(null);
                if (ratings4 != null)
                {
                    if (ratings4.Result.Count > 0)
                    {
                        Console.WriteLine("GetAllRatingsByAUserAsync failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetAllRatingsByAUserAsync passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetAllRatingsByAUserAsync failed");
                    unExpectedOutcome++;
                }

                // good argument
                Task<List<Rating>>? ratings5 = dl.GetAllRatingsByARatingAsync(4);
                if (ratings5 != null)
                {
                    if (ratings5.Result.Count > 0)
                    {
                        Console.WriteLine("GetAllRatingsByARatingAsync passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetAllRatingsByARatingAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetAllRatingsByARatingAsync failed");
                    unExpectedOutcome++;
                }

                // bad argument
                Task<List<Rating>>? ratings6 = dl.GetAllRatingsByARatingAsync(10);
                if (ratings6 != null)
                {
                    if (ratings6.Result.Count > 0)
                    {
                        Console.WriteLine("GetAllRatingsByARatingAsync failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetAllRatingsByARatingAsync passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetAllRatingsByARatingAsync failed");
                    unExpectedOutcome++;
                }

                // null argument
                Task<List<Rating>>? ratings7 = dl.GetAllRatingsByARatingAsync(null);
                if (ratings7 != null)
                {
                    if (ratings7.Result.Count > 0)
                    {
                        Console.WriteLine("GetAllRatingsByARatingAsync failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetAllRatingsByARatingAsync passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetAllRatingsByAsync failed");
                    unExpectedOutcome++;
                }

                // good argument
                Task<List<Rating>>? articleID = dl.GetAllRatingsByArticleIDAsync(2);
                if (articleID != null)
                {
                    if (articleID.Result.Count > 0)
                    {
                        Console.WriteLine("GetAllRatingsByArticleIDAsync passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetAllRatingsByArticleIDAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetAllRatingsByArticleIDAsync failed");
                    unExpectedOutcome++;
                }


                // bad argument
                Task<List<Rating>>? articleID1 = dl.GetAllRatingsByArticleIDAsync(0);
                if (articleID1 != null)
                {
                    if (articleID1.Result.Count > 0)
                    {
                        Console.WriteLine("GetAllRatingsByArticleIDAsync failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetAllRatingsByArticleIDAsync passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetAllRatingsByArticleIDAsync failed");
                    unExpectedOutcome++;
                }

                // null argument
                Task<List<Rating>>? articleID2 = dl.GetAllRatingsByArticleIDAsync(null);
                if (articleID2 != null)
                {
                    if (articleID2.Result.Count > 0)
                    {
                        Console.WriteLine("GetAllRatingsByArticleIDAsync failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("GetAllRatingsByArticleIDAsync passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("GetAllRatingsByArticleIDAsync failed");
                    unExpectedOutcome++;
                }

                Rating myRating = new()
                {
                    ArticleID = 1,
                    UserID = "a9fe9735-8aa3-4d5b-80c5-6dae8db89b28",
                    UserRating = 3.5F
                };

                // good argument
                Task<int>? res4 = dl.PostANewRatingAsync(myRating);
                if (res4.Result == 0)
                {
                    Console.WriteLine("PostANewRatingAsync failed");
                    unExpectedOutcome++;
                }
                else
                {
                    Console.WriteLine("PostANewRatingAsync passed");
                    expectedOutcome++;
                }

                // bad argument - insert same rating object again
                Task<int>? res5 = dl.PostANewRatingAsync(myRating);
                if (res5.Result != 0)
                {
                    Console.WriteLine("PostANewRatingAsync failed");
                    unExpectedOutcome++;
                }
                else
                {
                    Console.WriteLine("PostANewRatingAsync passed");
                    expectedOutcome++;
                }

                // bad argument - null
                Task<int>? res6 = dl.PostANewRatingAsync(null);
                if (res6.Result != 0)
                {
                    Console.WriteLine("PostANewRatingAsync failed");
                    unExpectedOutcome++;
                }
                else
                {
                    Console.WriteLine("PostANewRatingAsync passed");
                    expectedOutcome++;
                }

                // good argument                
                Task<int>? result6 = dl.PutARatingAsync(myRating);
                if (result6 != null)
                {
                    if (result6.Result == 1)
                    {
                        Console.WriteLine("PutARatingAsync updated passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("PutARatingAsync updated failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("PutARatingAsync updated failed");
                    unExpectedOutcome++;
                }

                // bad argument                
                myRating.ArticleID = 0;

                Task<int>? result7 = dl.PutARatingAsync(myRating);
                if (result7 != null)
                {
                    if (result7.Result == 1)
                    {
                        Console.WriteLine("PutARatingAsync updated failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("PutARatingAsync updated passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("PutARatingAsync updated failed");
                    unExpectedOutcome++;
                }

                // bad argument - null
                Task<int>? result8 = dl.PutARatingAsync(null);
                if (result8 != null)
                {
                    if (result8.Result == 1)
                    {
                        Console.WriteLine("PutARatingAsync updated failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("PutARatingAsync updated passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("PutARatingAsync updated failed");
                    unExpectedOutcome++;
                }

                myRating.ArticleID = 1;
                // good argument
                Task<int>? result9 = dl.DeleteARatingAsync(myRating);
                if (result9 != null)
                {
                    if (result9.Result != 1)
                    {
                        Console.WriteLine("DeleteARatingAsync failed");
                        unExpectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("DeleteARatingAsync passed");
                        expectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("DeleteARatingAsync failed");
                    unExpectedOutcome++;
                }

                // bad argument -delete same rating that was delete
                Task<int>? result10 = dl.DeleteARatingAsync(myRating);
                if (result5 != null)
                {
                    if (result10.Result != 1)
                    {
                        Console.WriteLine("DeleteARatingAsync passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("DeleteARatingAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("DeleteARatingAsync failed");
                    unExpectedOutcome++;
                }


                // bad argument - delete same rating that was delete
                Task<int>? result11 = dl.DeleteARatingAsync(null);
                if (result11 != null)
                {
                    if (result11.Result != 1)
                    {
                        Console.WriteLine("DeleteARatingAsync passed");
                        expectedOutcome++;
                    }
                    else
                    {
                        Console.WriteLine("DeleteARatingAsync failed");
                        unExpectedOutcome++;
                    }
                }
                else
                {
                    Console.WriteLine("DeleteARatingAsync failed");
                    unExpectedOutcome++;
                }


                #endregion

                Console.WriteLine("\nTest Completed\n");
                Console.WriteLine("Results: ");
                Console.WriteLine("Expected: " + expectedOutcome + " Unexpected: " + unExpectedOutcome);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (MySqlException ex)
            {
                // probably should log this exception
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                // probably should log this exception
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // no resource to clean up
            }

            return Task.CompletedTask;

        } // end Main

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        } // end GetHash

    } // end class Driver
} // end namespace