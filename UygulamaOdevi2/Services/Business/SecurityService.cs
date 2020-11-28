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

        public void CreateNewUser(UserModel user) {
            daoService.CreateNewUser(user);
        }
    }
}