using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using e_auction.Models;

namespace e_auction.Controllers
{
    public class TenderItemController : Controller
    {
        private Entities db = new Entities();

        
        // GET: /TenderItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TenderItem tenderitem = db.TenderItems.Find(id);
            if (tenderitem == null)
            {
                return HttpNotFound();
            }
            return View(tenderitem);
        }

        // GET: /TenderItem/Create
        public ActionResult Additems(string tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ModelState.Clear();
            if (tenderid == null)
            {
               return RedirectToAction("AddTender", "Tender");
            }
            ViewBag.BidMethodbag = new SelectList(db.BidMethodMasters, "Methodid", "MethodName");
            ViewBag.ItemCategorybag = new SelectList(db.CategoryMasters, "CategoryID", "CategoryName");
            ViewBag.Currencybag = new SelectList(db.CurrencyMasters, "currencyid", "currency");
            ViewBag.ItemTypebag = new SelectList(db.ItemTypeMasters, "Typeid", "typename");
            ViewBag.Tenderidbag = new SelectList(db.Tenders, "TenderId", "Attachment");
            ViewBag.UOMbag = new SelectList(db.UOMMasters, "UOMid", "UOM");
            ViewBag.itemlistbag = db.getitemslist(Convert.ToInt32(securityUtil.Decode(tenderid))).ToList();
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return View();
        }
        [HttpGet]
        public ActionResult Next(string tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            int id=Convert.ToInt32(securityUtil.Decode(tenderid));
            return RedirectToAction("Supplier", "AddSupplier", new { @tenderid = tenderid });
        }
        
        // POST: /TenderItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        public ActionResult Delete(string itemid,string tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try 
            { 
            db.Delete_Item(Convert.ToInt32(securityUtil.Decode(itemid)));
            return RedirectToAction("Additems", "TenderItem", new { @tenderid = tenderid});
            }
            catch
            {
                return RedirectToAction("Additems", "TenderItem", new { @tenderid = tenderid });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItems(TenderItem tenderitem,string tenderid, string name)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
           
                try 
                { 
                db.save_item(null,Convert.ToInt32(securityUtil.Decode(tenderid)), tenderitem.ItemType, tenderitem.ItemName, tenderitem.ItemDesc, tenderitem.ItemCategory, tenderitem.Location, tenderitem.Quantity, tenderitem.UOM, tenderitem.Price, tenderitem.Currency, tenderitem.BidMethod, tenderitem.ReducePerBid, tenderitem.Remarks);
                }
                catch
                {

                }
           
            ViewBag.BidMethodbag = new SelectList(db.BidMethodMasters, "Methodid", "MethodName", tenderitem.BidMethod);
            ViewBag.ItemCategorybag = new SelectList(db.CategoryMasters, "CategoryID", "CategoryName", tenderitem.ItemCategory);
            ViewBag.Currencybag = new SelectList(db.CurrencyMasters, "currencyid", "currency", tenderitem.Currency);
            ViewBag.ItemTypebag = new SelectList(db.ItemTypeMasters, "Typeid", "typename", tenderitem.ItemType);
            ViewBag.Tenderidbag = new SelectList(db.Tenders, "TenderId", "Attachment", tenderitem.Tenderid);
            ViewBag.UOMbag = new SelectList(db.UOMMasters, "UOMid", "UOM", tenderitem.UOM);
            ViewBag.itemlistbag = db.getitemslist(Convert.ToInt32(securityUtil.Decode(tenderid))).ToList();
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return RedirectToAction("AddItems", "TenderItem", new { @tenderid=tenderid });
        }
        [HttpGet]
        public ActionResult Edit(TenderItem tenderitem,string itemid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int tenderid = db.TenderItems.ToList().Where(m => m.itemid == Convert.ToInt32(securityUtil.Decode(itemid))).FirstOrDefault().Tenderid.Value;
            var item = int.Parse(securityUtil.Decode(itemid));
            tenderitem = db.TenderItems.Single(m => m.itemid == item);
            ViewBag.itemid = itemid;
            ViewBag.BidMethodbag = new SelectList(db.BidMethodMasters.ToList(), "Methodid", "MethodName", tenderitem.BidMethod);
            ViewBag.ItemCategorybag = new SelectList(db.CategoryMasters.ToList(), "CategoryID", "CategoryName", tenderitem.ItemCategory);
            ViewBag.Currencybag = new SelectList(db.CurrencyMasters.ToList(), "currencyid", "currency", tenderitem.Currency);
            ViewBag.ItemTypebag = new SelectList(db.ItemTypeMasters.ToList(), "Typeid", "typename", tenderitem.ItemType);
            ViewBag.Tenderidbag = new SelectList(db.Tenders.ToList(), "TenderId", "Attachment", tenderitem.Tenderid);
            ViewBag.UOMbag = new SelectList(db.UOMMasters.ToList(), "UOMid", "UOM", tenderitem.UOM);
            
            ViewBag.itemlist = db.getitemslist(Convert.ToInt32(tenderid)).ToList();
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            
            return View(tenderitem);
        }

        [HttpPost]
        public ActionResult Edit( string itemid, string tenderid,TenderItem tenderitem)
        {
            itemid = Request.QueryString["itemid"];
            tenderid = Request.QueryString["tenderid"];
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
                try
                {

                    db.save_item(Convert.ToInt32(securityUtil.Decode(itemid)), Convert.ToInt32(securityUtil.Decode(tenderid)), tenderitem.ItemType, tenderitem.ItemName, tenderitem.ItemDesc, tenderitem.ItemCategory, tenderitem.Location, tenderitem.Quantity, tenderitem.UOM, tenderitem.Price, tenderitem.Currency, tenderitem.BidMethod, tenderitem.ReducePerBid, tenderitem.Remarks);
                    return RedirectToAction("Additems", "TenderItem", new { @tenderid =tenderid });
                    
                }
                catch
                {

                }

            
            ViewBag.BidMethodbag = new SelectList(db.BidMethodMasters, "Methodid", "MethodName", tenderitem.BidMethod);
            ViewBag.ItemCategorybag = new SelectList(db.CategoryMasters, "CategoryID", "CategoryName", tenderitem.ItemCategory);
            ViewBag.Currencybag = new SelectList(db.CurrencyMasters, "currencyid", "currency", tenderitem.Currency);
            ViewBag.ItemTypebag = new SelectList(db.ItemTypeMasters, "Typeid", "typename", tenderitem.ItemType);
            ViewBag.Tenderidbag = new SelectList(db.Tenders, "TenderId", "Attachment", tenderitem.Tenderid);
            ViewBag.UOMbag = new SelectList(db.UOMMasters, "UOMid", "UOM", tenderitem.UOM);
            ViewBag.itemlistbag = db.getitemslist(Convert.ToInt32(securityUtil.Decode(tenderid))).ToList();
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return View();
        }
        
    }
}
