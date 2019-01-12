using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using e_auction.Models;

namespace e_auction.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        
        //
        // GET: /Home/
        [HttpGet]
        public ActionResult Index()
        {
            if(Session["clientid"]==null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Live=db.get_live_auction(Convert.ToInt32( Session["clientid"]), "Live");
            ViewBag.Raised = db.get_live_auction(Convert.ToInt32(Session["clientid"]), "Raised");
            ViewBag.Completed = db.get_live_auction(Convert.ToInt32(Session["clientid"]), "Completed");
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return View();
        }
        public ActionResult Logout()
        {
            Session["clientid"] = null;
            return RedirectToAction("Login", "Account");
        }
        public ActionResult  ChangePassword()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ChangePassword(string currentpass,string newpass,string repeatpass)
        {
            try
            {
                if (Session["clientid"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var valid = db.checklogin( Convert.ToInt32( Session["Clientid"]), currentpass).ToList();
                if (valid[0] == 1)
                {
                    if (newpass != repeatpass)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Password mismatch');</script>");

                    }
                    else
                    {
                        var check = db.ChangePassword(Convert.ToInt32(Session["Clientid"]), newpass, 0);
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Wrong current Password');</script>");
                }
            }
            catch(Exception ex)
            {
                return View();
            }
        }
	}
}