﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UygulamaOdevi2.Models;
using UygulamaOdevi2.Services.Business;

namespace UygulamaOdevi2.Controllers {
    public class LoginController : Controller {
        // GET: Login
        public ActionResult Index() {
            return View("Login");
        }

        public ActionResult Login(UserModel userModel) {
            SecurityService securityService = new SecurityService();
            Boolean success = securityService.Authenticate(userModel);

            if (success) {
                return View("LoginSuccess", userModel);
            }
            else {
                return View("LoginFailure", userModel);
            }
        }
    }
}