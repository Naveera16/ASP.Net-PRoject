using online_sms_site.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace online_sms_site.Controllers
{
    public class HomeController : Controller
    {
        SmsSite_dbEntities db_obj = new SmsSite_dbEntities();
        // Methods of Verification Code for signup
        private static readonly Random random = new Random();
        public string GenerateRandomOTP(int length)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        // End Method of Verification
        
        // GET: Home 
        public ActionResult Index()
        {
            int otpLength = 8;
            string randomOTP = GenerateRandomOTP(otpLength);

            Session["verification_code"] = randomOTP;
            return View();
        }
        //POST Home : Signup
        [HttpPost]
        public ActionResult Index(user_required require_data,string vf_code,string name)
        {
            if (ModelState.IsValid && name != "" ) // Validatiioon for  input field 
            { 
                   var checking_username = db_obj.user_required.Where(a => a.username == require_data.username).FirstOrDefault();
                    var checking_phone = db_obj.user_required.Where(a => a.phone_no == require_data.phone_no).FirstOrDefault();
                    if (checking_username == null) //Checking if the username is extist or not
                { 
                    if (checking_phone == null)  //Checking if the Phone number is extist or not
                    {

                                if (vf_code != "") //checking if the verification code field is null or not
                                {
                                    if (vf_code == Session["verification_code"].ToString())  //checking if the verification code is correct or not
                                     {         
                                        Session.Remove("verification_code");
                                        //setting the data in the table
                                        require_data.account_status = true;
                                        require_data.time = (DateTime.Now).ToString();
                                        db_obj.user_required.Add(require_data);
                                        var personalData = new user_personal
                                        {
                                            user_id = require_data.Id,
                                            name = name,
                                            photo = "~/assets/images/user-icon.png" //setting the image by default on signup of user
                                        };
                                        db_obj.user_personal.Add(personalData);
                                        var professional = new user_professional
                                        {
                                            Id = require_data.Id, 
                                        };
                                        db_obj.user_professional.Add(professional);
                                        db_obj.SaveChanges();

                                        ModelState.Clear();
                                        Session["signup"] = "<script>alert('Sign Up Commplete! Now Login to your account.')</script>";
                                        return RedirectToAction("Index", "Home");
                                    }
                                    else
                                    {
                                        Session.Remove("verification_code");
                                        Session["verifywrong"] = "<script>alert('Verification code you enter was wrong')</script>";
                                        return RedirectToAction("Index", "Home");
                                    }
                                }
                                else
                                {
                                    Session.Remove("verification_code");
                                    Session["verifywrong"] = "<script>alert('Enter Verification Code')</script>";
                                    return RedirectToAction("Index", "Home");
                                }
                      }
                        else
                        {
                            Session["phone_no"] = "<script>alert('THIS MOBILE NUMER had been registered already')</script>";
                        }
                    }
                    else
                    {
                    
                        Session["Username"] = "<script>alert('Username Already Exist!')</script>";
                    }
                return View();
            }
            else{
            Session["name.."] = name; //setiing name in session for the logic in view of manual validation

                return View(); }
        }
        // Home : Login
        public ActionResult Login(user_required login_data)
        {

            var data_check = db_obj.user_required.Where(a => a.username == login_data.username && a.password == login_data.password).FirstOrDefault();
            if (data_check != null) // checking the data user enter for login is exist in tables of not
            {
                data_check.active_status = true; //changiing the activite status when the user login 
                db_obj.Entry(data_check).State = EntityState.Modified;
                db_obj.SaveChanges();
                Session["user_id"] = data_check.Id;
                Session["user_name"] = data_check.username;
                Session["login_complete"] = "<script>alert('Login Successfully')</script>";
                return RedirectToAction("Profile", "Home");

            }
            else
            {
                Session["Login_uncomplete"] = "<script>alert('Invalid data')</script>";

            }

            return RedirectToAction("Index", "Home");
        }
       //Logout
       public ActionResult Logout(int Id)
        {
            var activestatus = db_obj.user_required.Where(a => a.Id == Id).FirstOrDefault();
            activestatus.active_status = false; //changiing the activite status when the user Logout 
            activestatus.lastseen = (DateTime.Now).ToString(); //setting the last seen of the user 
            db_obj.Entry(activestatus).State = EntityState.Modified;
            db_obj.SaveChanges();
            Session.Remove("user_id");
            Session.Remove("user_name");
            return RedirectToAction("Index", "Home");
        }
        //Profile 
        public ActionResult Profile()
        {
            if (Session["user_id"] == null) //logic which will prevent to on this page directly without login
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int user_id = Convert.ToInt32(Session["user_id"]);
                //Calling the user's data and saving it in new model named : mainmodel
                var req = db_obj.user_required.Where(a => a.Id == user_id).FirstOrDefault();
                var per = db_obj.user_personal.Where(a => a.user_id == user_id).FirstOrDefault();
                var pro = db_obj.user_professional.Where(a => a.Id == user_id).FirstOrDefault();
                Session["myphoto"] = per.photo;
                mainmodel dbase = new mainmodel();
                dbase.model1 = req;
                dbase.model2 = per;
                dbase.model3 = pro;
                dbase.services_ = (from a in db_obj.credit_card
                                   join b in db_obj.Services
                                   on a.activated_service equals b.Id
                                   select new services_
                                   {
                                       card = a,
                                       service = b
                                   }).ToList();
                ViewBag.norecord = "No Data";//this is set for the logic that if user didnt enter in there personal or professional detail
                return View(dbase);

            }
        }
        // GET : Edit page
        public ActionResult EditData(int id)
        {
            //Calling data from database of user
            var req = db_obj.user_required.Where(a => a.Id == id).FirstOrDefault();
            var per = db_obj.user_personal.Where(a => a.user_id == id).FirstOrDefault();
            var pro = db_obj.user_professional.Where(a => a.Id == id).FirstOrDefault();
            //Settting data in mainmodel
            mainmodel dbase = new mainmodel();
            dbase.model1 = req;
            dbase.model2 = per;
            dbase.model3 = pro;
            //setting image path in session
            Session["imgPath"] = per.photo;
            return View(dbase);
        }
        // POST : Edit page
        [HttpPost]
        public ActionResult EditData(HttpPostedFileBase file, mainmodel data)
        {

            if (ModelState.IsValid) // Validatiioon for  input field
            {
                if (file != null) //checking if the user set an image 
                {
                    string filename = Path.GetFileNameWithoutExtension(file.FileName);
                    string fileextension = Path.GetExtension(file.FileName);
                    filename = filename + fileextension;
                    if (fileextension.ToLower() == ".jpg" || fileextension.ToLower() == ".jpeg" || fileextension.ToLower() == ".png")
                    {
                        data.model2.photo = "~/profile_pictures/" + filename;
                        filename = Path.Combine(Server.MapPath("~/profile_pictures/") + filename);
                        file.SaveAs(filename);
                        Session["myphoto"] = data.model2.photo; //changing the photo set in session
                        // Update each model through main model
                        db_obj.Entry(data.model1).State = EntityState.Modified;
                        db_obj.Entry(data.model2).State = EntityState.Modified;
                        db_obj.Entry(data.model3).State = EntityState.Modified;
                        db_obj.SaveChanges();

                        Session["updated"] = "<script> alert('Data Updated') </script>";
                        return RedirectToAction("Profile", "Home");



                    }
                    else
                    {
                        Session["img_msg"] = "<script>alert('Inavlid File Type') </script>";
                        RedirectToAction("Profile", "Home");
                    }
                }

                else
                {
                    //setting the old image of user 
                    data.model2.photo = (Session["imgPath"]).ToString();
                    // Update each model through main model
                    db_obj.Entry(data.model1).State = EntityState.Modified;
                    db_obj.Entry(data.model2).State = EntityState.Modified;
                    db_obj.Entry(data.model3).State = EntityState.Modified;
                    db_obj.SaveChanges();
                    Session["updated"] = "<script> alert('Data Updated') </script>";
                    return RedirectToAction("Profile", "Home");
                }
            }
            foreach (var modelStateEntry in ModelState.Values)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    // Log or debug the error messages
                    Debug.WriteLine($"Error: {error.ErrorMessage}");
                }
            }
            return View(data);

        }
       //Services 
        public ActionResult Services()
        {
            return View(db_obj.Services.ToList());
        }
        //GET : Payment 
        public ActionResult payment(int id)
        {
            var service = db_obj.Services.Where(a => a.Id == id).FirstOrDefault();
            return View(service);
        }
        //POST : Payment  "the action is use for setting the data of credit card user enter in payment view"
        [HttpPost]
        public ActionResult creditcard(int? service_id,int service_amount, string email , string name , int? card_no ,int? cvv ,string ex_date) //data is coming through ajax
        {
            if(email != "" && name != "" && card_no != null && cvv != null && ex_date != "" && service_id != null && service_amount != null)
            {
                int my_id = Convert.ToInt32(Session["user_id"]);

                var creditcard = new credit_card
                {
                    holder_name = name,
                    card_no = card_no,
                    expiry_date = ex_date,
                    CVV = cvv,
                    amount = service_amount,
                    user_Id = my_id,
                    email = email,
                    activated_service = service_id


                };
                db_obj.credit_card.Add(creditcard);
                var serviceactivate = db_obj.user_required.Where(a=> a.Id ==  my_id).FirstOrDefault();
                serviceactivate.service_activate = service_id.ToString();
                db_obj.Entry(serviceactivate).State = EntityState.Modified;
                db_obj.SaveChanges();
                Session["service_activate"] = "<script>alert('Congratulations ! Your Service will be activate in 3 days!!')</script>";
                return RedirectToAction("Services", "Home");
            }
            return View();
        }
    }
}