using online_sms_site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace online_sms_site.Controllers
{
    public class textingController : Controller
    {
        // GET: texting
        SmsSite_dbEntities db_obj = new SmsSite_dbEntities();


        [HttpPost]
        //Index : action for sending text message
        public ActionResult Index(mainmodel text)
        { var myid = Convert.ToInt32(Session["user_id"]);

            //API key
            string MyApiKey = "923127341950-c02e862c-284f-4b50-b8db-b792841e9da1"; //Your API Key At Sendpk.com
            string toNumber = "92"+(text.sendtext.to).ToString(); //Recepient cell phone number with country code
            string Masking = "Online SMS Site"; //Your Company Brand Name
            string MessageText = text.sendtext.text;
            string jsonResponse = "yes";
            //string jsonResponse = SendSMS(Masking, toNumber, MessageText, "923127341950", "bilal2002");
            if (jsonResponse == "7 : Invalid Recepient Mobile Numbers Please Type Correct Mobile Number")
            {
                Session["error"] = "<script>alert('Invalid Phone Number')</script>";
                return RedirectToAction("Texting", "User");

            }
            else 
            {
                Session["error"] = "<script>alert('The Message is successfully delivered!')</script>";
                text.sendtext.from = myid;
                text.sendtext.time = (DateTime.Now).ToString();
                text.sendtext.status = "Free";
                db_obj.texting.Add(text.sendtext);
                var checking = db_obj.Contact.Where(a => a.contact_number == text.sendtext.to && a.user == myid).FirstOrDefault();
                if (checking == null)
                {
                    var contact = new Contact
                    {
                        user = myid,
                        contact_number = text.sendtext.to,
                        name = text.sendtext.name_of_receiver,
                        status = false
                    };
                    db_obj.Contact.Add(contact);
                }
                db_obj.SaveChanges();
                TempData["text_number"] = text.sendtext.to;
                TempData["text_"] = text.sendtext.text;
                return RedirectToAction("Texting", "User");
            }
        }
        //Method : for sending message through API
        public static string SendSMS(string Masking, string toNumber, string MessageText, string MyUsername, string MyPassword)
        {
            string MyApiKey = "923127341950-c02e862c-284f-4b50-b8db-b792841e9da1"; //Your API Key At Sendpk.com
            String URI = "http://Sendpk.com" +
            "/api/sms.php?" +
            "api_key=" + MyApiKey +
            "&sender=" + Masking +
            "&mobile=" + toNumber +
             "//&message=" + System.Net.WebUtility.UrlEncode(MessageText);// Visual Studio 12
            try
            {
                WebRequest req = WebRequest.Create(URI);
                WebResponse resp = req.GetResponse();
                var sr = new System.IO.StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (WebException ex)
            {
                var httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    switch (httpWebResponse.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            return "404:URL not found :" + URI;
                            break;
                        case HttpStatusCode.BadRequest:
                            return "400:Bad Request";
                            break;
                        default:
                            return httpWebResponse.StatusCode.ToString();
                    }
                }
            }
            return null;
        }

    }
}