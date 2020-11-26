using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UygulamaOdevi2.Models;

namespace UygulamaOdevi2.Services.Data {
    public class SecurityDAO {
        internal bool FindByUser(UserModel user) {
            return user.Username == "a" && user.Password == "a";
        }
    }
}