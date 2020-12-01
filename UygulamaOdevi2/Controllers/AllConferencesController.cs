using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UygulamaOdevi2.Models;

namespace UygulamaOdevi2.Controllers {
    public class AllConferencesController : Controller {
        public ActionResult Index() {
            return View("AllConferences");
        }

        public ActionResult JoinConference(string confName) {
            RDBMSController rdbms = new RDBMSController("s");
            rdbms.insertConferenceRoles(confName, 1, UserModel.LoggedInUser.Username);
            return View("AllConferences", rdbms.getConferences());
        }
    }
}