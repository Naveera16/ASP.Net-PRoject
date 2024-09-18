using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace online_sms_site.Models
{
    public class mainmodel
    {
        public user_required model1 { get; set; }
        public user_personal model2 { get; set; }
        public user_professional model3 { get; set; }
        public List<services_> services_ { get; set; }
        public List<combinedentity> combined_table { get; set; }
        public List<combinedentity2> combined_table2 { get; set; }
        //public List<user_personal> model5 { get; set; }
        public List<Messages> model5 { get; set; }
        public List<friend_list> model6 { get; set; }
        public List<Contact> contactlist { get; set; }
        public List<texting> texts { get; set; }
        public Messages setdata { get; set; }
        public texting sendtext { get; set; }
        public Contact addcontact { get; set; }
        public mainmodel()
        {
            model1 = new user_required();
            model2 = new user_personal();
            model3 = new user_professional();
            //model4 = new List<user_required>();
            //model5 = new List<user_personal>();
            model5 = new List<Messages>();
            model6 = new List<friend_list>();
            sendtext = new texting();
            addcontact = new Contact();
        }
    }
    public class combinedentity
    {
        public user_personal person { get; set; }
        public user_required person_required { get; set; }
    }
    public class combinedentity2
    {
        public user_required person { get; set; }
        public friend_list friendlist { get; set; }
    }
    public class services_
    {
        public credit_card card { get; set; }
        public Services service { get; set; }
    }
}