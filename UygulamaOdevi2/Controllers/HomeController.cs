using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UygulamaOdevi2.Models;
using UygulamaOdevi2.Services.Business;
using UygulamaOdevi2.Services.Data;

namespace UygulamaOdevi2.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            RDBMSController db = new RDBMSController("s");
            //  MongoDBController mongoDB = new MongoDBController("s");

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
            else {
                return View("CreateConference");
            }
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
            sub.submittedBy = UserModel.LoggedInUser.Username;
            MongoDBController mongo = new MongoDBController("s");
            /* public void addSubmission(string prevSubmissionID, string submissionID, string title, string ozet, List<string> keywords, List<List<string>> authors, string submittedBy, string correspondingAuthor,
            string pdf_path, string type, DateTime submissionDateTime, int status, int active)*/
            string title = sub.title;
            string ozet = sub.ozet;



            return View("Index");
        }

    }

}