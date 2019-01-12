using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using e_auction.Models;

namespace e_auction.Controllers
{
    public class AddSupplierController : Controller
    {
        private Entities db = new Entities();
        SecurityAES.AES crypt = new SecurityAES.AES();
        // GET: /AddSupplier/Supplier
        public ActionResult Supplier(String tenderid,string Search)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int a = Convert.ToInt32(securityUtil.Decode( tenderid));
            if (tenderid == null)
            {
                return RedirectToAction("AddTender", "Tender");
            }
            if(Search!=null)
            { 
            ViewBag.SearchText = Search.ToString();
            }
            if(Search==null)
            {
                ViewBag.SearchText = "";
            }
            ViewBag.supplier = db.get_supplierlist(a).ToList();
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            ViewBag.companylist = db.get_company(Search, Convert.ToInt32(securityUtil.Decode(tenderid))).ToList();
            return View();
        }
        
        
        //
        // POST: /AddSupplier/Create
        [HttpGet]
        public ActionResult Add(int companyid,String tenderid,string Search)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
         try
         {
             ViewBag.SearchText = Search;
             Guid Eguid = Guid.NewGuid();
             db.save_supplier(companyid,Convert.ToInt32(securityUtil.Decode( tenderid)),Convert.ToString( Eguid));
             return RedirectToAction("Supplier", new { @tenderid=tenderid,@Search=Search});
         }
         catch
         {
             return View();
         }
        }
        [HttpGet]
        public ActionResult Delete(int companyid, String tenderid,String Search)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            try
            {
                ViewBag.SearchText = Search;
                ViewBag.supplier = db.get_supplierlist(Convert.ToInt32(securityUtil.Decode(tenderid))).ToList();
                db.del_supplier(companyid, Convert.ToInt32(securityUtil.Decode(tenderid)));
                return RedirectToAction("Supplier", new { @tenderid = tenderid,@Search=Search });
            }
            catch
            {
                
                return View();
            }
        }
        [HttpPost]
        public ActionResult Supplier(Supplier list, string Search,string tenderid,string submit,string RemarksText)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            ViewBag.supplier = db.get_supplierlist(Convert.ToInt32(securityUtil.Decode(tenderid))).ToList();
            try
            {
                ViewBag.SearchText = Search;
                    ViewBag.companylist = db.get_company(Search,Convert.ToInt32(securityUtil.Decode(tenderid))).ToList();
                    
                
                if(submit=="Submit")
                {

                    db.update_tenderStatus(Convert.ToInt32(securityUtil.Decode(tenderid)), "Raised", Convert.ToInt32(Session["clientid"]), RemarksText);
                    return Content("<script language='javascript' type='text/javascript'>alert('Submited Successfully');window.location = '../Home/Index';</script>");
                }
                return View();  
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Search(string search,string tenderid)
        {
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            ViewBag.companylist= db.get_company(search,Convert.ToInt32(securityUtil.Decode(tenderid))).ToList();
            return View();
        }
    }
}
