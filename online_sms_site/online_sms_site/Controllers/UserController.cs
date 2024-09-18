using online_sms_site.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace online_sms_site.Controllers
{
    public class UserController : Controller 
    { 
    
        SmsSite_dbEntities db_obj = new SmsSite_dbEntities();

        // GET: User 
        //Messanger page
        public ActionResult Index()  //view
        {
                var myid = Convert.ToInt32(Session["user_id"]);
           
                
                    var messages = db_obj.Messages.Where(a => a.To == myid || a.From == myid).ToList();
                    var friendlist = db_obj.friend_list.Where(a => a.receiver_id == myid && a.status == "Friend" || a.sender_id == myid && a.status == "Friend").ToList();
                    //var friend_data = new List<user_personal>(); 
                    //var users = db_obj.user_required.ToList();
                    //var users_data = db_obj.user_personal.ToList();
                    mainmodel mainmodel = new mainmodel()
                    {
                        //model4 = users,
                        model6 = friendlist,
                        model5 = messages,

                    };
                    mainmodel.combined_table = (from a in db_obj.user_personal
                                                join b in db_obj.user_required
                                                on a.user_id equals b.Id
                                                select new combinedentity
                                                {
                                                    person = a,
                                                    person_required = b
                                                }).ToList();
                    return View(mainmodel);


                }
               
        //Friend List
        public ActionResult friend_list() //view
        {
            var myid = Convert.ToInt32(Session["user_id"]);
            //Calling friends ids from table friendlist where either reeceiver id is equals to myid or the sender id  is equals to myid
            var freind_list = db_obj.friend_list.Where(a => a.receiver_id == myid && a.status == "Friend").Select(a => a.sender_id).ToList();
            var freind_list2 = db_obj.friend_list.Where(a => a.sender_id == myid && a.status == "Friend").Select(a => a.receiver_id).ToList();
            int?[] ids = freind_list.ToArray();
            int?[] ids2 = freind_list2.ToArray();
            ViewBag.friend_ids = ids;
            ViewBag.friend_ids2 = ids2;
            return View(db_obj.user_personal.ToList());
        }
        //Contact List
        public ActionResult contact_List() //view
        {
            var myid = Convert.ToInt32(Session["user_id"]);
            var contact_list = db_obj.Contact.Where(a => a.user == myid).ToList();
            //using main model
            mainmodel mainmodel = new mainmodel();  
            mainmodel.contactlist= contact_list;
            //Combining two tables in one property
            mainmodel.combined_table2 = (from b in db_obj.user_required
                                        join a in db_obj.friend_list.Where(a=> a.sender_id == myid)
                                        on b.Id equals a.receiver_id 
                                        join c in db_obj.friend_list.Where(a=> a.receiver_id == myid)
                                        on b.Id equals c.sender_id
                                         select new combinedentity2
                                        {
                                            friendlist = a,
                                            person = b
                                        }).ToList();
            return View(mainmodel);
        }
        //Friend Request list
        public ActionResult friend_request() //view
        { 
            var myid = Convert.ToInt32(Session["user_id"]);
            var myrequestlist = db_obj.Friend_Request.Where(a => a.receiver_id == myid && a.status == "Pending").Select(a=>a.sender_id).ToList();
            int?[] ids=myrequestlist.ToArray();
            ViewBag.requestids = ids;
            return View(db_obj.user_personal.ToList());
        }
        //Sugesstion List "List of  register peoples who are neither include in friendlist of myid nor they havent send the request any request to myid"
        public ActionResult suggestion() //view
        {
            var myid = Convert.ToInt32(Session["user_id"]);
            var request = db_obj.Friend_Request.Where(a => a.sender_id == myid).Select(a => new { ReceiverId = a.receiver_id.Value, Status = a.status }).ToArray();
            var request2 = db_obj.Friend_Request.Where(a => a.receiver_id == myid).Select(a => new { SenderId = a.sender_id.Value, Status = a.status }).ToArray();
            ViewBag.req1 = request;
            ViewBag.req2 = request2;
            return View(db_obj.user_personal.ToList());
        }
        // add friend  "The action is for sending the friend request to other registered users"
        public ActionResult addfriend(int Id) //no view
        {
            var my_id = Convert.ToInt32(Session["user_id"]);
            var checking = db_obj.Friend_Request.Where(a => a.sender_id == my_id && a.receiver_id == Id).FirstOrDefault();
            if (checking == null) //  checking if myid has send any request to Id in past 
            {
                //setting data in table friend request
                var Friend_Request = new Friend_Request
                {
                    sender_id = Convert.ToInt32(Session["user_id"]),
                    receiver_id = Id,
                    status = "Pending", //setting status "Pending"
                    unfriended_by = "NoOne",
                    send_date = (DateTime.Now).ToString(),
                    receive_date = null,

                };
                db_obj.Friend_Request.Add(Friend_Request);
                db_obj.SaveChanges();
                return RedirectToAction("suggestion", "User");
            }
            else
            {
                checking.status = "Pending";//setting status "Pending"
                db_obj.Entry(checking).State = EntityState.Modified;
                db_obj.SaveChanges();
                return RedirectToAction("suggestion", "User");
            }
        }
        //Cancel Request "the action if for : if myid send any request and then it wants to cancel it"
        public ActionResult cancel_req(int Id) //no view
        {
            var my_id = Convert.ToInt32(Session["user_id"]);
            var cancel_id = db_obj.Friend_Request.Where(a => a.sender_id == my_id && a.receiver_id == Id ).FirstOrDefault();
            cancel_id.status = "Cancel"; //updating status from Pending to Cancel
            db_obj.Entry(cancel_id).State = EntityState.Modified;
            db_obj.SaveChanges();
            return RedirectToAction("suggestion", "User");
        }
        //Remove Request "The action is deleting any friends requests"
        public ActionResult remove_req(int Id)  //no view                                        
        {
            var my_id = Convert.ToInt32(Session["user_id"]);
            var remove = db_obj.Friend_Request.Where(a => a.sender_id == Id && a.receiver_id == my_id).FirstOrDefault();
            remove.status = "Removed"; 
            db_obj.Entry(remove).State = EntityState.Modified;
            db_obj.SaveChanges();
            return RedirectToAction("friend_request", "User");
        }
        // Accept Request "The action if for accepting the request"
        public ActionResult accept_req(int Id) //no view 
        {
            var my_id = Convert.ToInt32(Session["user_id"]);
            var checking = db_obj.friend_list.Where(a=>a.sender_id == Id && a.receiver_id == my_id || a.receiver_id == Id && a.sender_id == my_id).FirstOrDefault();
            if (checking == null)
            {
                var accepting = db_obj.Friend_Request.Where(a => a.sender_id == Id & a.receiver_id == my_id).FirstOrDefault();
                accepting.status = "Accepted";
                accepting.receive_date = (DateTime.Now).ToString();
                db_obj.Entry(accepting).State = EntityState.Modified;
                var freind_add = new friend_list
                {
                    sender_id = accepting.sender_id,
                    receiver_id = my_id,
                    date_time = (DateTime.Now).ToString(),
                    status = "Friend",
                    request_id = accepting.Id
                };
                db_obj.friend_list.Add(freind_add);
                db_obj.SaveChanges();
                return RedirectToAction("friend_request", "User");
            }
            else
            {
               var accepting = db_obj.Friend_Request.Where(a => a.sender_id == Id & a.receiver_id == my_id).FirstOrDefault();
                accepting.status = "Accepted";
                accepting.receive_date = (DateTime.Now).ToString();
                db_obj.Entry(accepting).State = EntityState.Modified;
                checking.date_time = (DateTime.Now).ToString();
                checking.status = "Friend";
                db_obj.Entry(checking).State = EntityState.Modified;
                db_obj.SaveChanges();

                return RedirectToAction("friend_request", "User");

            }

        }
        //Unfriend "The action is for unfriend any friend"
        public ActionResult unfriend(int Id) //no view
        {
            var my_id = Convert.ToInt32(Session["user_id"]);
            var remove = db_obj.Friend_Request.Where(a => a.sender_id == Id && a.receiver_id == my_id || a.receiver_id ==  Id && a.sender_id == my_id).FirstOrDefault();
            remove.status = "Unfriend";
            remove.unfriended_by = my_id.ToString();
            var unfriend = db_obj.friend_list.Where(a => a.sender_id == Id && a.receiver_id == my_id || a.receiver_id == my_id && a.sender_id == Id).FirstOrDefault();
            unfriend.status = "Unfriend";

            db_obj.Entry(remove).State = EntityState.Modified;
            db_obj.Entry(unfriend).State = EntityState.Modified;
            db_obj.SaveChanges();
            return RedirectToAction("friend_list", "User");
        }
        //Other profile "the action is to visit other profiles"
        public ActionResult otherprofile(int Id,string Status) //no view
        {
            ViewBag.status=Status;
            
            var req = db_obj.user_required.Where(a => a.Id == Id).FirstOrDefault();
            var per = db_obj.user_personal.Where(a => a.user_id == Id).FirstOrDefault();
            var pro = db_obj.user_professional.Where(a => a.Id == Id).FirstOrDefault();
            mainmodel dbase = new mainmodel();

            dbase.model1 = req;
            dbase.model2 = per;
            dbase.model3 = pro;

            return View(dbase);
        }

        //Send Message "Action for sending the msg"
        //: data for message is coming through ajax and in return its taking the same msg for showinf in view without reloading the page
        public ActionResult send_msg(int? to, int? from, long? no_of_receiver, long? no_of_sender, string message) //view
        {
            if (to != null && from != null && no_of_receiver != null && no_of_sender != null && message != null)
            {
                var messages = new Messages
                {
                    To = to,
                    From = from,
                    no_of_receiver = no_of_receiver,
                    no_of_sender = no_of_sender,
                    messages = message,
                    date_time = (DateTime.Now).ToString(),
                    status = true
                };
                db_obj.Messages.Add(messages); db_obj.SaveChanges();
                ViewBag.not1 = "yesss!";

                return View(messages);
            }
            else
            {
                return View();
            }
          }
        public ActionResult deletemsg(int Id)
        {
            var msg = db_obj.Messages.Where(a => a.Id == Id).FirstOrDefault();
            msg.status = false;
            db_obj.Entry(msg).State = EntityState.Modified;
            db_obj.SaveChanges();
            return RedirectToAction("Index", "User");
        }
        //mychat "the action is for calling the chat " 
        //taking the chat from this to the page  Index(messanger) through ajax
        public ActionResult mychat(int id) // view
        {           
            var myid = Convert.ToInt32(Session["user_id"]);
            var chatdata = db_obj.Messages.Where(a => a.To == id && a.From == myid || a.To == myid && a.From == id).ToList();
            mainmodel chattingmodel = new mainmodel()
            {
                model5 = chatdata
            };
            return View(chattingmodel);
        }
        //Texting "the action action is for showing and sending the text messages "
        public ActionResult texting() //view
        {
            var myid = Convert.ToInt32(Session["user_id"]);
            mainmodel mainmodel = new mainmodel();

            mainmodel.contactlist = db_obj.Contact.Where(a => a.user == myid).ToList();
            mainmodel.texts = db_obj.texting.Where(a => a.from == myid).ToList();
          var dropdown=db_obj.Contact.Where(a=> a.user == myid).Select(a => a.contact_number).ToList();
            ViewBag.dropdown = dropdown;
            return View(mainmodel);
        }
        //Add contact "action is used for adding any unregistered number in contacts"
        public ActionResult addcontact(mainmodel data)
        {
            
            var myid = Convert.ToInt32(Session["user_id"]);
            if (ModelState.IsValid)
            {
                data.addcontact.status = true;
                data.addcontact.user = myid;
                db_obj.Contact.Add(data.addcontact);
                db_obj.SaveChanges();
                return RedirectToAction("texting", "User");
            }
            else
            {
                return RedirectToAction("texting", "User");

            }
        }
        //Editing contacts from contact list
        public ActionResult editcontact(mainmodel data,int id,string page)
        {
            var myid = Convert.ToInt32(Session["user_id"]);
            var contact = db_obj.Contact.Where(a => a.Id == id).FirstOrDefault();
            contact.status = true;
            contact.user = myid;
            contact.name = data.addcontact.name;
            contact.contact_number= data.addcontact.contact_number;
            db_obj.Entry(contact).State = EntityState.Modified;
            db_obj.SaveChanges();
            if (page == "contact_list") {
                return RedirectToAction("contact_list", "User");
            }
            else if (page == "texting") {
                return RedirectToAction("texting", "User");
            }
            return null;

        }
        
        //delete contacts from contact list
        public ActionResult Deletecontact(int id)
        {

          var contactData = db_obj.Contact.Where(a => a.Id == id).FirstOrDefault();
            if(contactData != null)
            {
                db_obj.Entry(contactData).State = EntityState.Deleted;
                db_obj.SaveChanges();
                Session["contactdeleted"] = "<script>alert('Contact deleted successfully!')</script>";
                return RedirectToAction("contact_List","User");
            }
            return null;

        }
       
    }

}