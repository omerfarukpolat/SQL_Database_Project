﻿using System;
using System.Collections.Generic;
using UygulamaOdevi2.Controllers;
using UygulamaOdevi2.Models;

namespace UygulamaOdevi2.Services.Data {
    //data access object
    public class SecurityDAO {

        RDBMSController rdbms = new RDBMSController("s");

        public static List<UserModel> user_request = new List<UserModel>();
        public static List<CONFERENCE> conference_request = new List<CONFERENCE>();

        internal bool FindByUser(UserModel user) {
            //start by assuming that nothing is found in this query
            bool success = false;

            for (int i = 0; i < rdbms.getUsers().Count; i++) {
                String name1 = user.Username;
                String pass1 = user.Password;
                String name2 = rdbms.getUsers()[i].Username;
                String pass2 = rdbms.getUsers()[i].Password;
                success = String.Equals(name1, name2) && String.Equals(pass1, pass2);
                if (success) break;
            }

            return success;
        }

        internal void addUser(UserModel user) {
            string salutation = user.Salutation;
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
            int id2 = -1;
            for (int i = 0; i < list.Count; i++) {
                if (String.Equals(username, list[i].Username)) {
                    id2 = list[i].AuthenticationID;
                    break;
                }
            }

            rdbms.insertUserInfo(salutation, id2, name, lname, affiliation, pemail, semail, pass, phone, fax, url, address, city, country, date);
        }

        internal bool CreateNewUser(UserModel user) {
            string salutation = user.Salutation;
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

            List<USERS> list = rdbms.getUsers();
            int id2 = -1;
            for (int i = 0; i < list.Count; i++) {
                if (String.Equals(username, list[i].Username)) {
                    id2 = list[i].AuthenticationID;
                    break;
                }
            }

            if (String.Equals(username, "Admin")) {
                rdbms.insertUser(username, pass);
                rdbms.insertUserInfo(salutation, id2, name, lname, affiliation, pemail, semail, pass, phone, fax, url, address, city, country, date);
                return true;
            }
            else {
                user_request.Add(user);
                return false;
            }
        }
    }
}