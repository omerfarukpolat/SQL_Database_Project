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
        //idyi profilde goster
        internal void CreateNewUser(UserModel user) {
            string salutation = user.Salutation;
            int id = user.AuthenticationID;
            string name = user.Name;
            string lname = user.LastName;
            int affiliation = user.Affiliation;
            string pemail = user.PrimaryEmail;
            string semail = user.SecondaryEmail;
            string pass = user.Password;
            string phone = user.Phone;
            string fax = user.Fax;
            string url = user.URL;
            string address = user.Address;
            string city = user.City;
            string country = user.Country;
            //DateTime date = user.RecordCreationDate;
            DateTime date = DateTime.Today;
            string username = user.Username;
            rdbms.insertUser(username, pass);

            List<USERS> list = rdbms.getUsers();
            int id2 = 1;
            for (int i = 0; i < list.Count; i++) {
                if (String.Equals(username, list[i].Username)) {
                    id2 = list[i].AuthenticationID;
                    break;
                }
            }

            rdbms.insertUserInfo(salutation, id2, name, lname, affiliation, pemail, semail, pass, phone, fax, url, address, city, country, date);
        }
    }
}