using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bil372_Odev2.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public string Welcome(string Name, int numTimes = 1) {
            return HttpUtility.HtmlEncode("Hello, " + Name + " Number of times is " + numTimes);
        }

        public string Welcome2(string Name, int ID = 1) {
            return HttpUtility.HtmlEncode("Hello, " + Name + " ID is " + ID);
        }

        public ActionResult TestView() {
            //return a 'testviev.cshtml' view maps to the action method name
            return View();
        }

        public string PrintMessage() {
            return "<h1>Welcome</h1><p>This is the first control page of your application</p>";
        }
    }
}