using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UygulamaOdevi2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            RDBMSController db = new RDBMSController();
            MongoDBController mongo = new MongoDBController("KONFERANS");
        
            return View();
        }

        public ActionResult SignUp() {
            return View("SignUp");
        }

    }
}