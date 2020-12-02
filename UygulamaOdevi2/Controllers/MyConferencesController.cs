using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UygulamaOdevi2.Models;

namespace UygulamaOdevi2.Controllers {
    public class MyConferencesController : Controller {
        public ActionResult Index() {
            return View("MyConferences");
        }

        public ActionResult MyConferences() {
            RDBMSController rdbms = new RDBMSController("s");
            List<CONFERENCE_ROLES> table = rdbms.getConferenceRoles();
            List<CONFERENCE_ROLES> myConferences = new List<CONFERENCE_ROLES>();
            string username = UserModel.LoggedInUser.Username;

            for (int i = 0; i < table.Count; i++)
                if (String.Equals(username, table[i].userName))
                    myConferences.Add(table[i]);

            return View("MyConferences", myConferences);
        }
    }
}