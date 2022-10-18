use neArticleJet;

/* 
 * File: article jet mysql.sql
 * Name: jpbear
 * Class: CITC 1317
 * Semester: Fall 2022
 * Project: ArticleJet Project
 */
 
DROP TABLE IF EXISTS Article;
DROP TABLE IF EXISTS JetUser;
DROP TABLE IF EXISTS UserLevel;
DROP TABLE IF EXISTS Rating;

DROP PROCEDURE IF EXISTS spGetAllUserLevels;

DROP PROCEDURE IF EXISTS spGetAUserByGUID;
DROP PROCEDURE IF EXISTS spGetAUserByUserAndPass;
DROP PROCEDURE IF EXISTS spGetAllUsers;
DROP PROCEDURE IF EXISTS spPostNewJetUser;
DROP PROCEDURE IF EXISTS spPutUserActiveState;

DROP PROCEDURE IF EXISTS spGetAllArticles;
DROP PROCEDURE IF EXISTS spGetArticleByArticleID;
DROP PROCEDURE IF EXISTS spGetArticlesAfterDate;
DROP PROCEDURE IF EXISTS spPostANewArticle;
DROP PROCEDURE IF EXISTS spPutAnArticle;
DROP PROCEDURE IF EXISTS spDeleteArticleByArticleID;

DROP PROCEDURE IF EXISTS spGetAllRatings;
DROP PROCEDURE IF EXISTS spGetAllRatingsByAUser;
DROP PROCEDURE IF EXISTS spGetAllRatingsByARating;
DROP PROCEDURE IF EXISTS spGetAllRatingsByArticleID;
DROP PROCEDURE IF EXISTS spPostANewRating;
DROP PROCEDURE IF EXISTS spPutARating;
DROP PROCEDURE IF EXISTS spDeleteRatingByUserIDArticleID;

CREATE TABLE UserLevel(
	LevelId INT PRIMARY KEY AUTO_INCREMENT,
    Level VARCHAR (50) NOT NULL
);

CREATE TABLE JetUser(
	UserGUID varchar(50) NOT NULL,
	Email varchar(50) NOT NULL,
	UserPassword varchar(256) NOT NULL,
	FirstName varchar(50) NOT NULL,
	LastName varchar(50) NOT NULL,
	ActiveUser bit NOT NULL,
	LevelId int NOT NULL,
    primary key(Email, UserPassword),
    UNIQUE(UserGUID)
);

CREATE TABLE Article(
	ArticleID INT PRIMARY KEY AUTO_INCREMENT,
	Title varchar(256) NOT NULL,
	Postdate date NOT NULL,
	Summary text NOT NULL,
	Link varchar(256) NOT NULL,
	OwnerGUID varchar(50) NOT NULL,
    UNIQUE (Title)
);

CREATE TABLE Rating(	
    ArticleID INT NOT NULL,
    UserID varchar(50) NOT NULL,
    Rating float NOT NULL,
    primary key(ArticleID, UserID)
);

ALTER TABLE JetUser
ADD CONSTRAINT FK_JetUser_UserLevel FOREIGN KEY(LevelId)
REFERENCES UserLevel (LevelId);

ALTER TABLE Article  
ADD CONSTRAINT FK_Article_JetUser FOREIGN KEY(OwnerGUID)
REFERENCES JetUser (UserGUID)

ALTER TABLE Rating
ADD CONSTRAINT FK_Rating_Article FOREIGN KEY(ArticleID)
REFERENCES Article (ArticleID)

ALTER TABLE Rating
ADD CONSTRAINT FK_Rating_JetUser FOREIGN KEY(UserID)
REFERENCES JetUser (UserGUID)

/* Create stored procedures */
/* User stored procedures */
DELIMITER //
CREATE PROCEDURE `spGetAUserByUserAndPass`(in userEmail varchar(50), in password varchar(256))
BEGIN
	SELECT UserGUID, Email, FirstName, LastName, ActiveUser, LevelID FROM JetUser WHERE Email = UserEmail AND UserPassword = password; 
END//

CREATE PROCEDURE `spGetAUserByGUID`(in GUID varchar(50))
BEGIN
	SELECT UserGUID, Email, FirstName, LastName, ActiveUser, LevelID FROM JetUser WHERE UserGUID = GUID;
END//

CREATE PROCEDURE `spGetAllUsers`()
BEGIN
	SELECT UserGUID, Email, UserPassword, FirstName, LastName, ActiveUser, LevelID FROM JetUser WHERE ActiveUser = 1;
END//

CREATE PROCEDURE `spPostNewJetUser`(GUID varchar(50), mail varchar(50), pass varchar(256), fname varchar(50), lname varchar(50), isActive bit, userLevel int)
BEGIN
	INSERT INTO JetUser (UserGUID, Email, UserPassword, FirstName, LastName, ActiveUser, LevelID) VALUES (GUID, mail, pass, fname, lname, isActive, userLevel);
END//

CREATE PROCEDURE `spPutUserActiveState`(in GUID varchar(50), in state bit)
BEGIN
	UPDATE JetUser SET ActiveUser = state WHERE UserGuid = GUID;
END//

/* Article stored procedures */

CREATE PROCEDURE `spGetAllArticles`()
BEGIN
	SELECT ArticleID, Title, Postdate, Summary, Link, OwnerGUID FROM article;
END//

CREATE PROCEDURE `spGetArticleByArticleID`(in ID int)
BEGIN
	SELECT Title, Postdate, Summary, Link, OwnerGUID FROM article WHERE ArticleID = ID;
END//

CREATE PROCEDURE `spGetArticlesAfterDate`(in articleDate date)
BEGIN
	SELECT ArticleID, Title, Summary, Link, OwnerGUID FROM article WHERE Postdate >= articleDate;
END//

CREATE PROCEDURE `spPostANewArticle`(articleTitle varchar(256), articleDate date, articleSummary text, articleLink varchar(256), articleOwner varchar(50))
BEGIN
	INSERT INTO Article (Title, Postdate, Summary, Link, OwnerGUID) VALUES (articleTitle, articleDate, articleSummary, articleLink, articleOwner);
END//

CREATE PROCEDURE `spPutAnArticle`(in ID int, articleTitle varchar(256), articleDate date, articleSummary text, articleLink varchar(256), articleOwner varchar(50))
BEGIN
	UPDATE Article SET Title = articleTitle, Postdate = articleDate, Summary = articleSummary, Link = articleLink, OwnerGUID = articleOwner WHERE ArticleID = ID;
END//

CREATE PROCEDURE `spDeleteArticleByArticleID`(in ID int)
BEGIN
	DELETE FROM Article WHERE ArticleID = ID;
END//

/* Rating stored procedures */

CREATE PROCEDURE `spGetAllRatings`()
BEGIN
	SELECT ArticleID, UserID, Rating FROM Rating;
END//

CREATE PROCEDURE `spGetAllRatingsByAUser`(in ID varchar(50))
BEGIN
	SELECT ArticleID, UserID, Rating FROM Rating WHERE UserID = ID;
END//

CREATE PROCEDURE `spGetAllRatingsByARating`(in ratingNum int)
BEGIN
	SELECT ArticleID, UserID, Rating FROM Rating WHERE Rating >= ratingNum;
END//

CREATE PROCEDURE `spGetAllRatingsByArticleID`(in artID int)
BEGIN
	SELECT ArticleID, UserID, Rating FROM Rating WHERE ArticleID = artID;
END//

CREATE PROCEDURE `spPostANewRating`(artID int, uID varchar(50), ratingValue float)
BEGIN
	INSERT INTO Rating (ArticleID, UserID, Rating) VALUES (artID, uID, ratingValue);
END//

CREATE PROCEDURE `spPutARating`(in artID int, uID varchar(50), articleRating float)
BEGIN
	UPDATE Rating SET ArticleID = artID, UserID = uID, Rating = articleRating WHERE ArticleID = artID AND UserID = uID;
END//

CREATE PROCEDURE `spDeleteRatingByUserIDArticleID`(in artID int, uID varchar(50))
BEGIN
	DELETE FROM Rating WHERE ArticleID = artID AND UserID = uID;
END//

/* UserLevel stored procedures */

CREATE PROCEDURE `spGetAllUserLevels`()
BEGIN
	SELECT LevelId, Level FROM userLevel; 
END//

DELIMITER ;
