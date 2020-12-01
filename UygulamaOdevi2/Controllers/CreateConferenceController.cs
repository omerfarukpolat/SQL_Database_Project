using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UygulamaOdevi2.Models;
using UygulamaOdevi2.Services.Business;

namespace UygulamaOdevi2.Controllers
{
    public class CreateConferenceController : Controller
    {
        // GET: CreateConference
        public ActionResult Index()
        {
            return View("CreateConference");
        }

        public ActionResult CreateConference(ConferenceModel conf) {
            SecurityService securityService = new SecurityService();
            if (securityService.CreateConference(conf))
                return View("HomePage");
            else
                return View("ConferenceRequestSent");
            
        }
    }
}