using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace online_sms_site.Models
{
    public class chatting
    {
        public List<Messages> mychat { get; set; }
        public List<Messages> frchat { get; set; }
        public chatting() { 
        mychat = new List<Messages>();
           frchat = new List<Messages>();
        }


    }
}