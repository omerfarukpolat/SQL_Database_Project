using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;

namespace UygulamaOdevi2.Controllers
{
    public class RDBMSController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KONFERANS"].ConnectionString);
        public RDBMSController()
        {
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
                                 ConfID_ROLE INTEGER,
                                 AuthenticationID INTEGER,
                                 FOREIGN KEY(AuthenticationID) REFERENCES USERS(AuthenticationID) ON DELETE CASCADE
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
            cmd.CommandText = @"CREATE TABLE USER_LOG(
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
        public RDBMSController(String s)
        {

        }

    }
}