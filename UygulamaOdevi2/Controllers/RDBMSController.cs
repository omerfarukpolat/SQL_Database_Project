using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Management;
using System.Web.Mvc;
using UygulamaOdevi2.Models;
using MongoDB.Driver;

namespace UygulamaOdevi2.Controllers {
    /*
     * Yaptığım Değişiklikler : 
     * 1- Conference_Roles tablosuna confID eklendi.
     * 
     * 2- Get komutunda konferans adı, konferansID, UserID, USERNAME ve rolü geliyor.
     * 
     * 3- Insert komutunda konferansın ADINI, kullanıcı ADINI ve kullanıcının rolünü (INT) olarak alıyor ve
     * düzgün biçimde insert ediyor.
     * 
     * 4- update komutunda değişiklik yapılacak kullanıcının USERNAME'i ve konferansın adı, değiştirilen rol (INT) alınıyor,
     * 1(chair) iken 2(reviewer) gibi,sonrasında update ediliyor.
     * 
     * 5- delete komutunda konferans adı ve o konferanstan silinecek olan kullanıcın USERNAME'i alınıyor ve o konferanstan
     * o kullanıcı siliniyor.
     * 
     * 
     * 
     * 
     * 
     * 
     * */

    public class RDBMSController : Controller {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KONFERANS"].ConnectionString);
        public RDBMSController() {
            con.Open();
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE USERS(
                                   AuthenticationID  INTEGER identity(1,1) PRIMARY KEY,
                                   Username VARCHAR(20),
                                   Password VARCHAR(50)
                         )";
            cmd.ExecuteNonQuery();


            cmd.CommandText = @"CREATE TABLE CONFERENCE(
                                    ConfID VARCHAR(20) PRIMARY KEY,
                                    CreationDateTime DATETIME,
                                    Name varchar(100),
                                    ShortName varchar(19),
                                    Year Integer,
                                    StartDate Date,
                                    EndDate Date,
                                    Submission_Deadline Date,
                                    CreatorUser Integer,
                                    WebSite Varchar(100),
                                    FOREIGN KEY(CreatorUser) REFERENCES USERS(AuthenticationID) ON DELETE NO ACTION
                         )";
            cmd.ExecuteNonQuery();


            cmd.CommandText = @"CREATE TABLE CONFERENCE_TAGS(
                                   ConfID VARCHAR(20),
                                   Tag   VARCHAR(100) PRIMARY KEY,
                                   FOREIGN KEY(ConfID) REFERENCES CONFERENCE(ConfID) ON DELETE CASCADE,
                         )";
            cmd.ExecuteNonQuery();


            cmd.CommandText = @"CREATE TABLE CONFERENCE_ROLES(
                                 ConfID VARCHAR(20),
                                 ConfID_ROLE INTEGER,
                                 AuthenticationID INTEGER,
                                 FOREIGN KEY(AuthenticationID) REFERENCES USERS(AuthenticationID) ON DELETE CASCADE,
                                 FOREIGN KEY(ConfID) REFERENCES CONFERENCE(ConfID) ON DELETE CASCADE
                         )";
            cmd.ExecuteNonQuery();


            cmd.CommandText = @"CREATE TABLE COUNTRY(
                          Country_Code  CHAR(3) PRIMARY KEY NOT NULL,
                          Country_Name  VARCHAR(50)
                         )";

            cmd.ExecuteNonQuery();


            cmd.CommandText = @"CREATE TABLE COUNTRY_CITY(
                            Country_Code CHAR(3),
                            CityID INT identity(1,1) NOT NULL PRIMARY KEY,
                            City_Name VARCHAR(100),
                            FOREIGN KEY(Country_Code) REFERENCES COUNTRY(Country_Code) ON DELETE CASCADE
                         )";

            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY(Country_Code , Country_Name) " +
                           "VALUES('DEU','Almanya')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY(Country_Code , Country_Name) " +
                              "VALUES('USA','Amerika')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY(Country_Code , Country_Name) " +
                              "VALUES('DNK','Danimarka')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY(Country_Code , Country_Name) " +
                              "VALUES('FRA','Fransa')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY(Country_Code , Country_Name) " +
                               "VALUES('TUR','Türkiye')";
            cmd.ExecuteNonQuery();

            //Ýnsert CountryCity
            cmd.CommandText = "INSERT INTO COUNTRY_CITY(Country_Code , City_Name) " +
                               "VALUES('DEU','Berlin')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY_CITY(Country_Code , City_Name) " +
                               "VALUES('DEU','Münih')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY_CITY(Country_Code , City_Name) " +
                               "VALUES('USA','New York')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY_CITY(Country_Code , City_Name) " +
                               "VALUES('USA','Boston')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY_CITY(Country_Code , City_Name) " +
                               "VALUES('DNK','Kopenhag')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY_CITY(Country_Code , City_Name) " +
                               "VALUES('DNK','Odense')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY_CITY(Country_Code , City_Name) " +
                               "VALUES('FRA','Paris')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY_CITY(Country_Code , City_Name) " +
                               "VALUES('FRA','Nice')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO COUNTRY_CITY(Country_Code , City_Name) " +
                               "VALUES('TUR','Ankara')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO COUNTRY_CITY(Country_Code , City_Name) " +
                       "VALUES('TUR','Ýstanbul')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE USER_INFO(
                                InfoID INTEGER identity(1,1) PRIMARY KEY,
                                Salutation VARCHAR(50),
                                AuthenticationID INTEGER,
                                Name VARCHAR(50),
                                LastName VARCHAR(50),
                                Affiliation INTEGER,
                                primary_email VARCHAR(50),
                                secondary_email VARCHAR(50),
                                password VARCHAR(50), 
                                phone VARCHAR(12),
                                fax VARCHAR(50),
                                URL VARCHAR(50),
                                Address VARCHAR(50),
                                City INTEGER,
                                Country CHAR(3),
                                Record_Creation_Date DATETIME,
                                FOREIGN KEY(Country) REFERENCES COUNTRY(Country_Code),
                                FOREIGN KEY(AuthenticationID) REFERENCES USERS(AuthenticationID) ON DELETE CASCADE,
                                FOREIGN KEY(City) REFERENCES COUNTRY_CITY(CityID)
                                                         )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE USER_LOG(
                                LogID INTEGER identity(1,1) PRIMARY KEY,
                                Salutation VARCHAR(50),
                                AuthenticationID INTEGER,
                                Name VARCHAR(50),
                                LastName VARCHAR(50),
                                Affiliation INTEGER,
                                primary_email VARCHAR(50),
                                secondary_email VARCHAR(50),
                                password VARCHAR(50), 
                                phone VARCHAR(12),
                                fax VARCHAR(50),
                                URL VARCHAR(50),
                                Address VARCHAR(50),
                                City INTEGER,
                                Country CHAR(3),
                                Record_Creation_Date DATETIME,
                                FOREIGN KEY(Country) REFERENCES COUNTRY(Country_Code),
                                FOREIGN KEY(AuthenticationID) REFERENCES USERS(AuthenticationID) ON DELETE CASCADE,
                                FOREIGN KEY(City) REFERENCES COUNTRY_CITY(CityID),
                                                         )";
            cmd.ExecuteNonQuery();


            cmd.CommandText = @"CREATE TABLE SUBMISSIONS(
                            AuthenticationID INTEGER,
                            ConfID VARCHAR(20),
                            SubmissionID INT identity(1,1) PRIMARY KEY,
                            prevSubmissionID INT,
                            FOREIGN KEY(prevSubmissionID) REFERENCES SUBMISSIONS(SubmissionID),
                            FOREIGN KEY(AuthenticationID) REFERENCES USERS(AuthenticationID) ON DELETE CASCADE,
                            FOREIGN KEY(ConfID) REFERENCES CONFERENCE(ConfID) ON DELETE CASCADE
                         )";

            cmd.ExecuteNonQuery();

        }
        public RDBMSController(String s) {
            con.Open();
        }

        public List<USERS> getUsers() {
            List<USERS> list = new List<USERS>();
            string sql = "SELECT * FROM USERS";
            var asd = new SqlCommand(sql, con);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                USERS item = new USERS();
                item.AuthenticationID = rdr.GetInt32(0);
                item.Username = rdr.GetString(1);
                item.Password = rdr.GetString(2);
                list.Add(item);
            }
            return list;
        }
        public void insertUser(string username, string password) {
            string s = "INSERT INTO USERS(Username, Password) " +
                        "VALUES (@username,@password)";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public void updateUser(int AuthenticationID, string password) {
            string s = "UPDATE USERS SET password = @password  " +
                        "WHERE AuthenticationID = @AuthenticationID";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@AuthenticationID", AuthenticationID);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public List<USER_INFO> getUserInfos() {
            List<USER_INFO> list = new List<USER_INFO>();
            string sql = "SELECT InfoID,Salutation,AuthenticationID,Name,LastName,Affiliation,primary_email,secondary_email,password" +
            ",phone,fax,URL,Address,City_Name,Country_Name,Record_Creation_Date FROM USER_INFO,COUNTRY, COUNTRY_CITY" +
            " WHERE USER_INFO.Country = COUNTRY.Country_Code AND City = COUNTRY_CITY.CityID";
            var asd = new SqlCommand(sql, con);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                USER_INFO item = new USER_INFO();
                item.InfoID = rdr.GetInt32(0);
                item.Salutation = rdr.GetString(1);
                item.AuthenticationID = rdr.GetInt32(2);
                item.Name = rdr.GetString(3);
                item.LastName = rdr.GetString(4);
                item.Affiliation = rdr.GetInt32(5);
                item.primary_email = rdr.GetString(6);
                item.secondary_email = rdr.GetString(7);
                item.password = rdr.GetString(8);
                item.phone = rdr.GetString(9);
                item.fax = rdr.GetString(10);
                item.URL = rdr.GetString(11);
                item.Address = rdr.GetString(12);
                item.City = rdr.GetString(13);
                item.Country = rdr.GetString(14);
                item.Record_Creation_Date = rdr.GetDateTime(15);
                list.Add(item);

            }
            return list;
        }
        public void updateUserInfo(string salutation, int AuthenticationID, string name, string lname, int affiliation,
                                      string primary_email, string secondary_email, string password, string phone, string fax, string URL,
                                      string address, string city, string country, DateTime recordCreationDate) {
            int cityID = 0;
            string countryCode = "";
            string findcity = "SELECT CityID,Country_Code FROM COUNTRY_CITY WHERE City_Name = @city";
            var asd = new SqlCommand(findcity, con);
            asd.Parameters.AddWithValue("@city", city);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                cityID = rdr.GetInt32(0);
                countryCode = rdr.GetString(1);
            }
            rdr.Close();
            string s = "UPDATE USER_INFO SET Salutation = @salutation," +
                "Name = @name,LastName = @lname, Affiliation = @affiliation,primary_email = @primary_email, secondary_email = @secondary_email," +
                "password = @password, phone = @phone, fax = @fax, URL = @URL," +
                "Address = @address,City = @cityID ,Country = @countryCode ,Record_Creation_Date = @recordCreationDate" +
                " WHERE AuthenticationID= @AuthenticationID";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@salutation", salutation);
            cmd.Parameters.AddWithValue("@AuthenticationID", AuthenticationID);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@lname", lname);
            cmd.Parameters.AddWithValue("@affiliation", affiliation);
            cmd.Parameters.AddWithValue("@primary_email", primary_email);
            cmd.Parameters.AddWithValue("@secondary_email", secondary_email);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@fax", fax);
            cmd.Parameters.AddWithValue("@URL", URL);
            cmd.Parameters.AddWithValue("@address", address);
            cmd.Parameters.AddWithValue("@cityID", cityID);
            cmd.Parameters.AddWithValue("@countryCode", countryCode);
            cmd.Parameters.AddWithValue("@recordCreationDate", recordCreationDate);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public void insertUserInfo(string salutation, int AuthenticationID, string name, string lname, int affiliation,
                                      string primary_email, string secondary_email, string password, string phone, string fax, string URL,
                                      string address, string city, string country, DateTime recordCreationDate) {
            int cityID = 0;
            string countryCode = "";
            string findcity = "SELECT CityID,Country_Code FROM COUNTRY_CITY WHERE City_Name = @city";
            var asd = new SqlCommand(findcity, con);
            asd.Parameters.AddWithValue("@city", city);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                cityID = rdr.GetInt32(0);
                countryCode = rdr.GetString(1);
            }
            rdr.Close();
            var cmd = new SqlCommand();

            string s = "INSERT INTO USER_INFO " +
                        "VALUES (@salutation,@AuthenticationID,@name,@lname,@affiliation,@primary_email,@secondary_email," +
                        "@password,@phone,@fax,@URL,@address," +
                        "@cityID,@countryCode,@recordCreationDate)";
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@salutation", salutation);
            cmd.Parameters.AddWithValue("@AuthenticationID", AuthenticationID);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@lname", lname);
            cmd.Parameters.AddWithValue("@affiliation", affiliation);
            cmd.Parameters.AddWithValue("@primary_email", primary_email);
            cmd.Parameters.AddWithValue("@secondary_email", secondary_email);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@fax", fax);
            cmd.Parameters.AddWithValue("@URL", URL);
            cmd.Parameters.AddWithValue("@address", address);
            cmd.Parameters.AddWithValue("@cityID", cityID);
            cmd.Parameters.AddWithValue("@countryCode", countryCode);
            cmd.Parameters.AddWithValue("@recordCreationDate", recordCreationDate);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }

        public List<USER_LOG> getUserLog() {
            List<USER_LOG> list = new List<USER_LOG>();
            string sql = "SELECT InfoID,Salutation,AuthenticationID,Name,LastName,Affiliation,primary_email,secondary_email,password" +
                ",phone,fax,URL,Address,City_Name,Country_Name,Record_Creation_Date FROM USER_LOG,COUNTRY, COUNTRY_CITY" +
                "WHERE Country = Country_Code AND City = CityID";
            var asd = new SqlCommand(sql, con);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                USER_LOG item = new USER_LOG();
                item.Salutation = rdr.GetString(1);
                item.AuthenticationID = rdr.GetInt32(2);
                item.Name = rdr.GetString(3);
                item.LastName = rdr.GetString(4);
                item.Affiliation = rdr.GetInt32(5);
                item.primary_email = rdr.GetString(6);
                item.secondary_email = rdr.GetString(7);
                item.password = rdr.GetString(8);
                item.phone = rdr.GetString(9);
                item.fax = rdr.GetString(10);
                item.URL = rdr.GetString(11);
                item.Address = rdr.GetString(12);
                item.City = rdr.GetString(13);
                item.Country = rdr.GetString(14);
                item.Record_Creation_Date = rdr.GetDateTime(15);
                list.Add(item);

            }
            return list;
        }
        public void insertLog(string salutation, int AuthenticationID, string name, string lname, int affiliation,
                                    string primary_email, string secondary_email, string password, string phone, string fax, string URL,
                                    string address, string city, string country, DateTime recordCreationDate) {
            int cityID = 0;
            string countryCode = "";
            string findcity = "SELECT CityID,Country_Code FROM COUNTRY_CITY WHERE City_Name = @city";
            var asd = new SqlCommand(findcity, con);
            asd.Parameters.AddWithValue("@city", city);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                cityID = rdr.GetInt32(0);
                countryCode = rdr.GetString(1);
            }
            rdr.Close();
            var cmd = new SqlCommand();

            string s = "INSERT INTO USER_LOG " +
                        "VALUES (@salutation,@AuthenticationID,@name,@lname,@affiliation,@primary_email,@secondary_email," +
                        "@password,@phone,@fax,@URL,@address," +
                        "@cityID,@countryCode,@recordCreationDate)";
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@salutation", salutation);
            cmd.Parameters.AddWithValue("@AuthenticationID", AuthenticationID);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@lname", lname);
            cmd.Parameters.AddWithValue("@affiliation", affiliation);
            cmd.Parameters.AddWithValue("@primary_email", primary_email);
            cmd.Parameters.AddWithValue("@secondary_email", secondary_email);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@fax", fax);
            cmd.Parameters.AddWithValue("@URL", URL);
            cmd.Parameters.AddWithValue("@address", address);
            cmd.Parameters.AddWithValue("@cityID", cityID);
            cmd.Parameters.AddWithValue("@countryCode", countryCode);
            cmd.Parameters.AddWithValue("@recordCreationDate", recordCreationDate);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public List<CONFERENCE> getConferences() {

            List<CONFERENCE> list = new List<CONFERENCE>();
            string sql = "SELECT ConfID,CreationDateTime,Name,ShortName," +
                "Year,StartDate,EndDate,Submission_Deadline,Username,WebSite FROM CONFERENCE,USERS " +
                "WHERE USERS.AuthenticationID = CONFERENCE.CreatorUser";
            var asd = new SqlCommand(sql, con);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                CONFERENCE item = new CONFERENCE();
                item.ConfID = rdr.GetString(0);
                item.CreationDateTime = rdr.GetDateTime(1);
                item.Name = rdr.GetString(2);
                item.ShortName = rdr.GetString(3);
                item.Year = rdr.GetInt32(4);
                item.StartDate = rdr.GetDateTime(5);
                item.EndDate = rdr.GetDateTime(6);
                item.Submission_Deadline = rdr.GetDateTime(7);
                item.CreatorUser = rdr.GetString(8);
                item.WebSite = rdr.GetString(9);
                list.Add(item);

            }
            return list;
        }

        public void insertConference(string ConfID, DateTime CreationDateTime, string name, string ShortName, int Year,
                                  DateTime StartDate, DateTime EndDate, DateTime Submission_Deadline, string CreatorUser, string WebSite) {
            int userID = 0;
            string findUser = "SELECT AuthenticationID FROM USERS WHERE Username = @CreatorUser";
            var asd = new SqlCommand(findUser, con);
            asd.Parameters.AddWithValue("@CreatorUser", CreatorUser);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                userID = rdr.GetInt32(0);
            }
            rdr.Close();
            var cmd = new SqlCommand();
            string s = "INSERT INTO CONFERENCE " +
                        "VALUES (@ConfID,@CreationDateTime,@name,@ShortName,@Year,@StartDate,@EndDate," +
                        "@Submission_Deadline,@userID,@WebSite)";
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@ConfID", ConfID);
            cmd.Parameters.AddWithValue("@CreationDateTime", CreationDateTime);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@ShortName", ShortName);
            cmd.Parameters.AddWithValue("@Year", Year);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@Submission_Deadline", Submission_Deadline);
            cmd.Parameters.AddWithValue("@WebSite", WebSite);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public void updateConference(string ConfID, DateTime CreationDateTime, string name, string ShortName, int Year,
                            DateTime StartDate, DateTime EndDate, DateTime Submission_Deadline, string CreatorUser, string WebSite) {
            int userID = 0;
            string findUser = "SELECT AuthenticationID FROM USERS WHERE Username = @CreatorUser";
            var asd = new SqlCommand(findUser, con);
            asd.Parameters.AddWithValue("@CreatorUser", CreatorUser);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                userID = rdr.GetInt32(0);
            }
            rdr.Close();
            string s = "UPDATE CONFERENCE SET CreationDateTime = @CreationDateTime," +
                "Name = @name,ShortName = @ShortName,Year = @Year,StartDate = @StartDate, EndDate = @EndDate," +
                "Submission_Deadline = @Submission_Deadline,CreatorUser = @userID" +
                " WHERE ConfID= @ConfID";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@ConfID", ConfID);
            cmd.Parameters.AddWithValue("@CreationDateTime", CreationDateTime);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@ShortName", ShortName);
            cmd.Parameters.AddWithValue("@Year", Year);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@Submission_Deadline", Submission_Deadline);
            cmd.Parameters.AddWithValue("@WebSite", WebSite);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public void deleteConference(string name) {

            string s = "DELETE FROM CONFERENCE WHERE Name =@name";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@name", name);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }

        public List<CONFERENCE_TAGS> getConferenceTags() {
            List<CONFERENCE_TAGS> list = new List<CONFERENCE_TAGS>();
            string sql = "SELECT ConfID,Name,Tag FROM CONFERENCE_TAGS,CONFERENCE WHERE CONFERENCE_TAGS.ConfID = CONFERENCE.ConfID ";
            var asd = new SqlCommand(sql, con);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                CONFERENCE_TAGS item = new CONFERENCE_TAGS();
                item.ConfID = rdr.GetString(0);
                item.ConferenceName = rdr.GetString(1);
                item.Tag = rdr.GetString(2);
                list.Add(item);
            }
            return list;
        }
        public void updateConferenceTags(string ConferenceName, string Tag) {
            string confID = "";
            string findConfID = "SELECT ConfID FROM CONFERENCE WHERE Name = @ConferenceName";
            var asd = new SqlCommand(findConfID, con);
            asd.Parameters.AddWithValue("@ConferenceName", ConferenceName);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                confID = rdr.GetString(0);
            }
            rdr.Close();
            string s = "UPDATE CONFERENCE_TAGS SET Tag = @Tag" +
                " WHERE ConfID= @ConfID";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@ConfID", confID);
            cmd.Parameters.AddWithValue("@Tag", Tag);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public void insertConferenceTags(string conferenceName, string Tag) {
            string confID = "";
            string findConference = "SELECT ConfID FROM CONFERENCE WHERE Name = @conferenceName";
            var asd = new SqlCommand(findConference, con);
            asd.Parameters.AddWithValue("@conferenceName", conferenceName);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                confID = rdr.GetString(0);
            }
            rdr.Close();
            string s = "INSERT INTO CONFERENCE_TAGS(ConfID, Tag) " +
                        "VALUES (@confID,@Tag)";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@confID", confID);
            cmd.Parameters.AddWithValue("@Tag", Tag);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public void deleteConferenceTags(string conferenceName) {
            string confID = "";
            string findConference = "SELECT ConfID FROM CONFERENCE WHERE Name = @conferenceName";
            var asd = new SqlCommand(findConference, con);
            asd.Parameters.AddWithValue("@conferenceName", conferenceName);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                confID = rdr.GetString(0);
            }
            string s = "DELETE FROM CONFERENCE_TAGS WHERE ConfID =@confID";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@confID", confID);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public List<CONFERENCE_ROLES> getConferenceRoles() {
            List<CONFERENCE_ROLES> list = new List<CONFERENCE_ROLES>();
            string sql = "SELECT Name, ConfID_ROLE,AuthenticationID, Username FROM CONFERENCE_ROLES,USERS,CONFERENCE " +
                "WHERE CONFERENCE_ROLES.AuthenticationID = USERS.AuthenticationID AND CONFERENCE.ConfID = CONFERENCE_ROLES.ConfID";
            var asd = new SqlCommand(sql, con);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                CONFERENCE_ROLES item = new CONFERENCE_ROLES();
                item.ConfName = rdr.GetString(0);
                item.ConfID_ROLE = rdr.GetInt32(1);
                item.AuthenticationID = rdr.GetInt32(2);
                item.userName = rdr.GetString(3);
                list.Add(item);
            }
            return list;
        }
        public void insertConferenceRoles(string confName, int ConfID_ROLE, string username) {
            int userID = 0;
            string confID = "";
            string findUser = "SELECT AuthenticationID FROM USERS WHERE Username = @username";
            var asd = new SqlCommand(findUser, con);
            asd.Parameters.AddWithValue("@username", username);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                userID = rdr.GetInt32(0);
            }
            rdr.Close();
            string findConference = "SELECT ConfID FROM CONFERENCE WHERE Name = @confName";
            var asd2 = new SqlCommand(findConference, con);
            asd2.Parameters.AddWithValue("@confName", confName);
            SqlDataReader rdr2 = asd2.ExecuteReader();
            while (rdr2.Read()) {
                confID = rdr2.GetString(0);
            }
            rdr2.Close();
            string s = "INSERT INTO CONFERENCE_ROLES(ConfID,ConfID_ROLE, AuthenticationID) " +
                        "VALUES (@confID,@ConfID_ROLE,@userID)";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@ConfID_ROLE", ConfID_ROLE);
            cmd.Parameters.AddWithValue("@confID", confID);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public void updateConferenceRoles(string confName, int ConfID_ROLE, string username) {
            int userID = 0;
            string confID = "";
            string findUser = "SELECT AuthenticationID FROM USERS WHERE Username = @username";
            var asd = new SqlCommand(findUser, con);
            asd.Parameters.AddWithValue("@username", username);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {

                userID = rdr.GetInt32(0);
            }
            rdr.Close();
            string findConference = "SELECT ConfID FROM CONFERENCE WHERE Name = @confName";
            var asd2 = new SqlCommand(findConference, con);
            asd2.Parameters.AddWithValue("@confName", confName);
            SqlDataReader rdr2 = asd2.ExecuteReader();
            while (rdr2.Read()) {
                confID = rdr2.GetString(0);
            }
            string s = "UPDATE CONFERENCE_ROLES SET ConfID_ROLE = @ConfID_ROLE " +
                        "WHERE AuthenticationID = @userID AND ConfID = @confID";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@ConfID_ROLE", ConfID_ROLE);
            cmd.Parameters.AddWithValue("@confID", confID);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public void deleteConferenceRole(string confName, string username) {
            int userID = 0;
            string confID = "";
            string findUser = "SELECT AuthenticationID FROM USERS WHERE Username = @username";
            var asd = new SqlCommand(findUser, con);
            asd.Parameters.AddWithValue("@username", username);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                userID = rdr.GetInt32(0);
            }
            rdr.Close();
            string findConference = "SELECT ConfID FROM CONFERENCE WHERE Name = @confName";
            var asd2 = new SqlCommand(findConference, con);
            asd2.Parameters.AddWithValue("@confName", confName);
            SqlDataReader rdr2 = asd2.ExecuteReader();
            while (rdr2.Read()) {
                confID = rdr2.GetString(0);
            }
            rdr2.Close();
            string s = "DELETE FROM CONFERENCE_ROLES WHERE AuthenticationID =@userID AND ConfID = @confID";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@confID", confID);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public List<COUNTRY> getCountry() {
            List<COUNTRY> countries = new List<COUNTRY>();
            string sql = "SELECT * FROM COUNTRY";
            var asd = new SqlCommand(sql, con);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                COUNTRY c = new COUNTRY();
                c.Country_Code = rdr.GetString(0);
                c.Country_Name = rdr.GetString(1);
                countries.Add(c);
            }
            return countries;
        }
        public List<COUNTRY_CITY> getCountryCity() {
            List<COUNTRY_CITY> cities = new List<COUNTRY_CITY>();
            string sql = "SELECT * FROM COUNTRY_CITY";
            var asd = new SqlCommand(sql, con);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                COUNTRY_CITY cc = new COUNTRY_CITY();
                cc.Country_Code = rdr.GetString(0);
                cc.CityID = rdr.GetInt32(1);
                cities.Add(cc);
            }
            return cities;
        }
        public List<SUBMISSIONS> getSubmissions() {
            List<SUBMISSIONS> list = new List<SUBMISSIONS>();
            string sql = "SELECT * FROM SUBMISSIONS";
            var asd = new SqlCommand(sql, con);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                SUBMISSIONS item = new SUBMISSIONS();
                item.AuthenticationID = rdr.GetInt32(0);
                item.ConfID = rdr.GetString(1);
                item.SubmissionID = rdr.GetInt32(2);
                item.prevSubmissionID = rdr.GetInt32(3);
                list.Add(item);
            }
            return list;
        }
        public void insertSubmission(int AuthenticationID, string ConfID, int prevSubmissionID) {
            string s = "INSERT INTO SUBMISSIONS(AuthenticationID, ConfID,prevSubmissionID) " +
                        "VALUES (@AuthenticationID,@ConfID,@prevSubmissionID)";
            var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@AuthenticationID", AuthenticationID);
            cmd.Parameters.AddWithValue("@ConfID", ConfID);
            cmd.Parameters.AddWithValue("@prevSubmissionID", prevSubmissionID);
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        public List<SEARCH_TAGS> searchWithTag(string tag) {
            List<SEARCH_TAGS> list = new List<SEARCH_TAGS>();
            string findConference = "SELECT ConfID,Name,Tag FROM CONFERENCE,CONFERENCE_TAGS" +
                " WHERE CONFERENCE_TAGS.Tag LIKE " + "'%" + tag + "%' AND CONFERENCE.ConfID = CONFERENCE_TAGS.ConfID";
            var asd = new SqlCommand(findConference, con);
            SqlDataReader rdr = asd.ExecuteReader();
            while (rdr.Read()) {
                SEARCH_TAGS item = new SEARCH_TAGS();
                item.confID = rdr.GetString(0);
                item.confName = rdr.GetString(1);
                item.tags = rdr.GetString(2);
                list.Add(item);
            }
            rdr.Close();
            return list;
        }
    }
}