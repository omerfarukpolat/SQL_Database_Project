﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UygulamaOdevi2.Models;
using UygulamaOdevi2.Services.Business;

namespace UygulamaOdevi2.Controllers {
    public class LoginController : Controller {

        public ActionResult Index() {
            return View("Login");
        }

        public ActionResult SignUp() {
            return View("SignUp");
        }

        public ActionResult Login(UserModel userModel) {
            SecurityService securityService = new SecurityService();
            Boolean success = securityService.Authenticate(userModel);

            if (success) {
                RDBMSController rdbms = new RDBMSController("s");
                List<USERS> list = rdbms.getUsers();
                for (int i = 0; i < list.Count; i++) {
                    if (String.Equals(userModel.Username, list[i].Username)) {
                        userModel.AuthenticationID = list[i].AuthenticationID;
                        break;
                    }
                }
                UserModel.LoggedInUser = userModel;
                return View("LoginSuccess", userModel);
            }
            else 
                return View("LoginFailure", userModel);
        }
    }
}