using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UygulamaOdevi2.Models;
using UygulamaOdevi2.Services.Business;
using UygulamaOdevi2.Services.Data;

namespace UygulamaOdevi2.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            RDBMSController db = new RDBMSController();
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

    }
}