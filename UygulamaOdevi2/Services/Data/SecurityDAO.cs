using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UygulamaOdevi2.Models;
using UygulamaOdevi2.Controllers;

namespace UygulamaOdevi2.Services.Data {
    //data access object
    public class SecurityDAO {

        RDBMSController rdbms = new RDBMSController("s");

        internal bool FindByUser(UserModel user) {
            //start by assuming that nothing is found in this query
            bool success = false;
            
            for (int i = 0; i < rdbms.getUsers().Count; i++) {
                String name1 = user.Username;
                String pass1 = user.Password;
                String name2 = rdbms.getUsers()[i].Username;
                String pass2 = rdbms.getUsers()[i].Password;
                success = String.Equals(name1, name2) && String.Equals(pass1, pass2);
            }

            return success;
        }

        internal void CreateNewUser(UserModel user) {
            //rdbms.insertUserInfo();
        }
    }
}