using System;
using System.Web.Mvc;
using UygulamaOdevi2.Models;
using UygulamaOdevi2.Services.Business;
using UygulamaOdevi2.Services.Data;

<<<<<<< HEAD
namespace UygulamaOdevi2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            RDBMSController db = new RDBMSController("s");
            MongoDBController mongoDB = new MongoDBController("s");
        
            return View();
=======
namespace UygulamaOdevi2.Controllers {
    public class HomeController : Controller {

        public ActionResult Index() {
            new RDBMSController("s");
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
>>>>>>> 0dbfdef5c4f731a10c501f0ce67c43d06305b445
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