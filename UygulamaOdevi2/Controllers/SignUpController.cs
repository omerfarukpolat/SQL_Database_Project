using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UygulamaOdevi2.Models;
using UygulamaOdevi2.Services.Business;

namespace UygulamaOdevi2.Controllers {
    public class SignUpController : Controller {
        // GET: SignUp
        public ActionResult Index() {
            return View("SignUp");
        }

        public ActionResult SignUp(UserModel userModel) {
            SecurityService securityService = new SecurityService();
            if (securityService.CreateNewUser(userModel))
                return View("HomePage", userModel);
            else
                return View("UserRequestSent", userModel);
        }
    }
}