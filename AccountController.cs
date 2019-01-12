using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using e_auction.Models;
using System.Data.Sql;
using System.IO;

namespace e_auction.Controllers
{
    public class AccountController : Controller
    {
        
        private Entities db = new Entities();
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Account/Register
        [HttpGet]
        public ActionResult Register()
        {
            
            ViewBag.CategoryListbag = new SelectList(db.CategoryMasters.ToList(), "CategoryID", "CategoryName");
            ViewBag.StateListbag = new SelectList(db.StateMasters.ToList(), "stateId", "StateName");
            return View();
        }
        

        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(RegisterMaster registermaster, string address)
        {
            if(ModelState.IsValid)
            { 
            // Code for validating the CAPTCHA  
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                db.Register(registermaster.CategoryId, "1", registermaster.CompanyName, registermaster.RegAddress, registermaster.City, registermaster.StateId, registermaster.pincode, registermaster.PanNo, registermaster.ContactPerson, registermaster.Email, registermaster.Mobile, registermaster.Telephone, registermaster.Tin);
                
                return RedirectToAction("Login");
            }
            else 
            { 
            ViewBag.ErrMessage = "Error: captcha is not valid.";
            return RedirectToAction("Register");
            }
            
            }
            ViewBag.CategoryListbag = new SelectList(db.CategoryMasters.ToList(), "CategoryID", "CategoryName");
            ViewBag.StateListbag = new SelectList(db.StateMasters.ToList(), "stateId", "StateName");
            return View();
        }

        [HttpGet]
        public ActionResult login()
        {
            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginMaster lc,string clientpass)
        {
         if(ModelState.IsValid)
         {
             var client = Convert.ToInt32(lc.ClientId);
             
             var valid = db.checklogin(client, clientpass).ToList();

             if (valid[0] == 1)
             {
                 var defaultcheck = db.checkdefault(client).ToList();
                 Session["clientid"] = lc.ClientId;
                 if(defaultcheck[0]==1)
                 {
                     return RedirectToAction("ChangePassword", "Home");
                 }
                 var role = db.get_role1(Convert.ToInt32(Session["clientid"])).ToList().FirstOrDefault().rolename;
                 if(role=="Admin")
                 {
                     var pendingcount = db.get_live_auction(Convert.ToInt32(Session["clientid"]), "NotApproved").ToList().Count;
                     if(pendingcount>0)
                     {
                         return RedirectToAction("PendingApproval", "Tender");
                     }
                 }
                 return RedirectToAction("Index", "Home");
             }
             else
                 ModelState.AddModelError("","Invalid Client ID or Password");
                 return RedirectToAction("Login","Account"); 

         }
            return View();
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        
        
        public JsonResult getemail(string clientid)
        {
            var email = db.getemail(Convert.ToInt32(clientid));
            return Json(email, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ForgetPassword(string clientid)
        {
            try
            {
                var check=db.ChangePassword(Convert.ToInt32(clientid),null,1);

                return RedirectToAction("Login", "Account");
            }
            catch(Exception ex)
            {
                return View();
            }
        }
	}

}