using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UygulamaOdevi2.Models;
using UygulamaOdevi2.Services.Business;
using UygulamaOdevi2.Services.Data;

namespace UygulamaOdevi2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() {
            RDBMSController db = new RDBMSController("s");
            return View("Index");
        }

        public ActionResult UserRequests() {
            return View("UserRequests", SecurityDAO.user_request);
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";
            return View("About");
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";
            return View("Contact");
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