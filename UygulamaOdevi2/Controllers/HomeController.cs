using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using UygulamaOdevi2.Models;
using UygulamaOdevi2.Services.Business;
using UygulamaOdevi2.Services.Data;

namespace UygulamaOdevi2.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KONFERANS"].ConnectionString);
            RDBMSController db;
       //     MongoDBController mongoDB = new MongoDBController("s");
            bool exists;
            con.Open();
            var cmd = new SqlCommand();
            string sql = "select count (*) from INFORMATION_SCHEMA.TABLES where table_name = 'USERS'";
            cmd.Connection = con;
            var asd = new SqlCommand(sql, con);
            cmd.CommandText = sql;
            exists = (int)cmd.ExecuteScalar() == 1;
            con.Close();

            if (exists)
            {
                db = new RDBMSController("1");
            }
            else
            {
                db = new RDBMSController();
                db = new RDBMSController("1");
            }

            //search users table for an admin, if there is none, create an admin user
            List<USERS> list = db.getUsers();
            bool admin_exists = false;
            for (int i = 0; i < list.Count; i++) {
                if (String.Equals(list[i].Username, "Admin")) {
                    admin_exists = true;
                    break;
                }
            }
            if (!admin_exists)
                db.insertUser("Admin", "123");

            return View("Index");
        }

        public ActionResult UserRequests() {
            UserModel user = UserModel.LoggedInUser;
            if (user == null) //if user is not logged in
                return View("NotLoggedIn");
            else {
                if (String.Equals(user.Username, "Admin")) //if user is logged in and is an admin
                    return View("UserRequests", SecurityDAO.user_request);
                else //if user is logged in but is not an admin
                    return View("NotAnAdmin");
            }
        }

        public ActionResult LogOut() {
            UserModel.LoggedInUser = null;
            return View("Index");
        }

        public ActionResult SignUp() {
            return View("SignUp");
        }

        public ActionResult AcceptUser(string username) {
            UserModel user = null;
            for (int i = 0; i < SecurityDAO.user_request.Count; i++) {
                if (String.Equals(SecurityDAO.user_request[i].Username, username)) {
                    user = SecurityDAO.user_request[i];
                    SecurityDAO.user_request.RemoveAt(i);
                    break;
                }
            }

            SecurityService ss = new SecurityService();
            ss.addUser(user);

            return View("UserRequests", SecurityDAO.user_request);
        }

        public ActionResult RejectUser(string username) {
            for (int i = 0; i < SecurityDAO.user_request.Count; i++) {
                if (String.Equals(SecurityDAO.user_request[i].Username, username)) {
                    SecurityDAO.user_request.RemoveAt(i);
                    break;
                }
            }
            return View("UserRequests", SecurityDAO.user_request);
        }

        public ActionResult ConferenceRequests() {
            UserModel user = UserModel.LoggedInUser;
            if (user == null) //if user is not logged in
                return View("NotLoggedIn");
            else {
                if (String.Equals(user.Username, "Admin")) //if user is logged in and is an admin
                    return View("ConferenceRequests", SecurityDAO.conference_request);
                else //if user is logged in but is not an admin
                    return View("NotAnAdmin");
            }
        }

        public ActionResult CreateConference() {
            if (UserModel.LoggedInUser == null) //if user is not logged in
                return View("NotLoggedIn");
            else 
                return View("CreateConference");
        }

        public ActionResult AcceptConference(string confID) {
            ConferenceModel conf = null;
            for (int i = 0; i < SecurityDAO.conference_request.Count; i++) {
                if (String.Equals(SecurityDAO.conference_request[i].ConfID, confID)) {
                    conf = SecurityDAO.conference_request[i];
                    SecurityDAO.conference_request.RemoveAt(i);
                    break;
                }
            }
            SecurityService ss = new SecurityService();
            RDBMSController rdbms = new RDBMSController("s");
            ss.addConference(conf);
            ss.addUserToConferenceRoles(conf);
            return View("ConferenceRequests", SecurityDAO.conference_request);
        }

        public ActionResult RejectConference(string confID) {
            for (int i = 0; i < SecurityDAO.conference_request.Count; i++) {
                if (String.Equals(SecurityDAO.conference_request[i].ConfID, confID)) {
                    SecurityDAO.conference_request.RemoveAt(i);
                    break;
                }
            }
            return View("ConferenceRequests", SecurityDAO.conference_request);
        }

        public ActionResult AllConferences() {
            RDBMSController rdbms = new RDBMSController("s");
            if (UserModel.LoggedInUser == null)
                return View("NotLoggedIn");
            return View("AllConferences", rdbms.getConferences());
        }

        public ActionResult JoinConference(string confName) {
            RDBMSController rdbms = new RDBMSController("s");
            rdbms.insertConferenceRoles(confName, 1, UserModel.LoggedInUser.Username);
            return View("AllConferences", rdbms.getConferences());
        }

        public ActionResult MyConferences() {
            RDBMSController rdbms = new RDBMSController("s");
            List<CONFERENCE_ROLES> table = rdbms.getConferenceRoles();
            List<CONFERENCE_ROLES> myConferences = new List<CONFERENCE_ROLES>();

            if (UserModel.LoggedInUser == null)
                return View("NotLoggedIn");

            string username = UserModel.LoggedInUser.Username;

            for (int i = 0; i < table.Count; i++)
                if (String.Equals(username, table[i].userName))
                    myConferences.Add(table[i]);

            return View("MyConferences", myConferences);
        }

        public ActionResult ShowParticipants(string confName) {
            RDBMSController rdbms = new RDBMSController("s");
            List<CONFERENCE_ROLES> roles = rdbms.getConferenceRoles();
            List<CONFERENCE_ROLES> findConf = new List<CONFERENCE_ROLES>();
            //get all rows from conference_roles table which have the name confName and put them in findConf list
            for (int i = 0; i < roles.Count; i++)
                if (String.Equals(roles[i].ConfName, confName))
                    findConf.Add(roles[i]);

            //search findConf to see if current user is a chair in that conference
            string username = UserModel.LoggedInUser.Username;
            bool chair = false;
            for (int i = 0; i < findConf.Count; i++) {
                if (String.Equals(username, findConf[i].userName)) {
                    if (findConf[i].ConfID_ROLE == 0) { //if the user is chair
                        chair = true;
                        break;
                    }
                }
            }

            if (chair)
                return View("ShowParticipants", findConf);

            else
                return View("NotChair");
        }

        public ActionResult AssignRole(string username, string confName, int role) {
            RDBMSController rdbms = new RDBMSController("s");
            List<CONFERENCE_ROLES> roles = rdbms.getConferenceRoles();
            List<CONFERENCE_ROLES> findConf = new List<CONFERENCE_ROLES>();
            //get all rows from conference_roles table which have the name confName and put them in findConf list
            for (int i = 0; i < roles.Count; i++)
                if (String.Equals(roles[i].ConfName, confName))
                    findConf.Add(roles[i]);

            //find user in the list and change their role
            for (int i = 0; i < findConf.Count; i++) {
                if (String.Equals(username, findConf[i].userName)) {
                    rdbms.updateConferenceRoles(findConf[i].ConfName, role, username);
                    break;
                }
            }

            //create a new list that is updated
            List<CONFERENCE_ROLES> new_list = new List<CONFERENCE_ROLES>();
            for (int i = 0; i < roles.Count; i++)
                if (String.Equals(roles[i].ConfName, confName))
                    new_list.Add(roles[i]);

            return View("ShowParticipants", new_list);
        }

        public ActionResult UserProfile() {
            if (UserModel.LoggedInUser == null)
                return View("NotLoggedIn");

            USER_INFO user = null;
            RDBMSController rdbms = new RDBMSController("s");
            List<USER_INFO> info = rdbms.getUserInfos();
            List<USERS> users = rdbms.getUsers();

            int id = -1;
            for (int i = 0; i < users.Count; i++) {
                if (String.Equals(UserModel.LoggedInUser.Username, users[i].Username)) {
                    id = users[i].AuthenticationID;
                }
            }

            for (int i = 0; i < info.Count; i++) {
                if (id == info[i].AuthenticationID) {
                    user = info[i];
                    break;
                }
            }

            return View("UserProfile", user);
        }

        public ActionResult SaveProfile(USER_INFO user) {
            RDBMSController rdbms = new RDBMSController("s");
            List<USERS> users = rdbms.getUsers();
            string username = UserModel.LoggedInUser.Username;
            int id = -1;
            for (int i = 0; i < users.Count; i++) {
                if (String.Equals(users[i].Username, username)) {
                    id = users[i].AuthenticationID;
                    break;
                }
            }

            string salutation = user.Salutation;
            string name = user.Name;
            string lname = user.LastName;
            int affiliation = (int)user.Affiliation;
            string pemail = user.primary_email;
            string semail = user.secondary_email;
            string pass = user.password;
            string phone = user.phone;
            string fax = user.fax;
            string url = user.URL;
            string address = user.Address;
            string city = user.City;
            string country = user.Country;
            DateTime date = Convert.ToDateTime(user.Record_Creation_Date);

            rdbms.insertLog(salutation, id, name, lname, affiliation, pemail, semail, pass, phone, fax, url, address, city, country, date);
            rdbms.updateUser(id, user.password);
            rdbms.updateUserInfo(salutation, id, name, lname, affiliation, pemail, semail, pass, phone, fax, url, address, city, country, date);

            return View("ChangesSaved");
        }

        public ActionResult Submissions() {
            if (UserModel.LoggedInUser == null)
                return View("NotLoggedIn");
            return View("Submissions");
        }

        public ActionResult CreateSubmission() {
            if (UserModel.LoggedInUser == null)
                return View("NotLoggedIn");
            return View("CreateSubmission");
        }

        public ActionResult CreateNewSubmission(MongoSubmission sub) {
            if (UserModel.LoggedInUser == null)
                return View("NotLoggedIn");
            MongoDBController mongo = new MongoDBController("s");

            sub.submittedBy = UserModel.LoggedInUser.Username;
            string submittedBy = sub.submittedBy;
            string submissionID = sub.submissionID;
            string title = sub.title;
            string ozet = sub.ozet;
            string pdf = sub.pdf_path;
            string type = sub.type;
            sub.submissionDateTime = DateTime.Today + "";
            int status = (int) sub.status;
            int active = (int) sub.active;
            string corrAut = sub.correspondingAuthor;
            string kw = sub.keywords;
            char[] separator = { ' ' };
            Int32 count = 5;
            List<string> keywords = new List<string>();
            String[] tag_list = kw.Split(separator, count, StringSplitOptions.None);
            for (int i = 0; i < tag_list.Length; i++)
                keywords.Add(tag_list[i]);

            List<List<string>> authors = new List<List<string>>();
            List<string> list = new List<string>();

            Int32 count2 = 5;
            String[] list2 = sub.authors.Split(separator, count2, StringSplitOptions.None);
            for (int i = 0; i < list2.Length; i++)
                list.Add(list2[i]);
            authors.Add(list);

            mongo.addSubmission(submissionID, title, ozet, keywords, authors, submittedBy, corrAut, pdf, type, DateTime.Today, status, active);

            return View("SubmissionCreated");
        }

        public ActionResult MySubmissions() {
            if (UserModel.LoggedInUser == null)
                return View("NotLoggedIn");

            MongoDBController mongo = new MongoDBController("s");
            List<MongoSubmission> list = mongo.getSubmissions();
            string username = UserModel.LoggedInUser.Username;
            List<MongoSubmission> mySub = new List<MongoSubmission>();
            for (int i = 0; i < list.Count; i++) {
                if (String.Equals(username, list[i].submittedBy)) {
                    mySub.Add(list[i]);
                }
            }
            return View("MySubmissions", mySub);
        }

    }

}