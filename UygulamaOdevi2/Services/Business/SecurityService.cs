﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UygulamaOdevi2.Models;
using UygulamaOdevi2.Services.Data;

namespace UygulamaOdevi2.Services.Business {
    public class SecurityService {

        SecurityDAO daoService = new SecurityDAO();

        public bool Authenticate(UserModel user) {
            return daoService.FindByUser(user);
        }

        public bool CreateNewUser(UserModel user) {
            return daoService.CreateNewUser(user);
        }

        public void addUser(UserModel user) {
            daoService.addUser(user);
        }

        public void addConference(ConferenceModel conf) {
            daoService.addConference(conf);
        }

        public bool CreateConference(ConferenceModel conf) {
            return daoService.CreateConference(conf);
        }

        public void addUserToConferenceRoles(ConferenceModel conf) {
            daoService.addUserToConferenceRoles(conf);
        }
    }
}