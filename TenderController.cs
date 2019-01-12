using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using e_auction.Models;
using System.IO;
using System.Data;
using System.Text;


namespace e_auction.Controllers
{
    public class TenderController : Controller
    {
        private Entities db = new Entities();
        
        
        // GET: /Tender/
        [HttpGet]
        public ActionResult Index()
        {
            
            return View();
        }
        // GET: /Tender/AddTender
        [HttpGet]
        public ActionResult AddTender()
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var client =Convert.ToInt32(Session["Clientid"].ToString());
            ViewBag.MinListbag = new SelectList(db.MinPeriods.ToList(), "id", "Timeperiod");
            ViewBag.CompanyListbag = new SelectList(db.companies.Where(d=>d.ClientId==client).ToList(),"CompanyId", "companyName");
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return View();
        }
        [HttpPost]
        public ActionResult AddTender(Tender td,string tenderid,HttpPostedFileBase Attachment)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if(ModelState.IsValid)
            {
                int tid=0;
                if(tenderid!=null)
                {
                    tid=Convert.ToInt32(securityUtil.Decode(tenderid));
                }
                SelectList tenderlist;
                int client = Convert.ToInt32(Session["Clientid"].ToString());
                tenderlist=new SelectList(db.save_tender(tid, client, td.TenderDesc, null, td.StartDate, td.StartTime,Convert.ToInt32(td.TimePeriod.ToString()), Convert.ToInt32(td.Company.ToString()),7).ToList());
                if (Attachment != null && Attachment.ContentLength > 0)
                try
                {
                    if (Attachment.ContentLength > 5000000)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('File Size greater Than 5 MB ');</script>");
                        
                    }
                    string path = Path.Combine(Server.MapPath("~/upload"), Path.GetFileName(Convert.ToString(tenderlist.ToList().Single().Text) + Path.GetExtension(Attachment.FileName)));
                    Attachment.SaveAs(path);
                    String Filename = Path.Combine(Path.GetFileName(Convert.ToString(tenderlist.ToList().Single().Text) + Path.GetExtension(Attachment.FileName)));
                    db.file_upload(Filename, Convert.ToInt32(tenderlist.ToList().Single().Text));
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            ViewBag.MinListbag = new SelectList(db.MinPeriods.ToList(), "id", "Timeperiod");
            ViewBag.CompanyListbag = new SelectList(db.companies.Where(d => d.ClientId == client).ToList(), "CompanyId", "companyName");
            return RedirectToAction("Terms", "Tender", new { @tenderid = ( securityUtil.Encode(tenderlist.ToList().Single().Text)) });
            }
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return View();
        }

        // GET: /Tender/Approvetender
        [HttpGet]   
        public ActionResult EditTender(String tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var client = Convert.ToInt32(Session["Clientid"].ToString());
            ViewBag.MinList = new SelectList(db.MinPeriods.ToList(), "id", "Timeperiod");
            ViewBag.CompanyList = new SelectList(db.companies.Where(d => d.ClientId == client).ToList(), "CompanyId", "companyName");
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            var id = int.Parse(securityUtil.Decode(tenderid));
            Tender tender = db.Tenders.Single(m => m.TenderId == id);
            return View(tender);
        }
        [HttpPost]
        public ActionResult EditTender(Tender td, string tenderid, HttpPostedFileBase Attachment)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if(tenderid!=null)
            {
                td.TenderId = Convert.ToInt32(securityUtil.Decode(tenderid));
                SelectList tenderlist;
                int client = Convert.ToInt32(Session["Clientid"].ToString());

                tenderlist = new SelectList(db.save_tender(Convert.ToInt32(securityUtil.Decode(tenderid)), client, td.TenderDesc, td.Attachment, td.StartDate, td.StartTime, Convert.ToInt32(td.TimePeriod.ToString()), Convert.ToInt32(td.Company.ToString()), 3).ToList());
                if (Attachment != null && Attachment.ContentLength > 0)
                    try
                    {
                        if (Attachment.ContentLength > 600000)
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('File Size greater Than 5 MB ');</script>");
                        }
                        string path = Path.Combine(Server.MapPath("~/upload"), Path.GetFileName(Convert.ToString(tenderlist.ToList().Single().Text) + Path.GetExtension(Attachment.FileName)));
                        Attachment.SaveAs(path);
                        String Filename = Path.Combine(Path.GetFileName(Convert.ToString(tenderlist.ToList().Single().Text) + Path.GetExtension(Attachment.FileName)));

                        db.file_upload(Filename, Convert.ToInt32(tenderlist.ToList().Single().Text));
                        ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }
                ViewBag.MinList = new SelectList(db.MinPeriods.ToList(), "id", "Timeperiod");
                ViewBag.CompanyList = new SelectList(db.companies.Where(d => d.ClientId == client).ToList(), "CompanyId", "companyName");
                return RedirectToAction("Additems", "TenderItem", new { @tenderid = tenderid });
            }
            
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return RedirectToAction("AddTender","Tender");
        }
        [HttpGet]
        public ActionResult ApproveTender(string tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            ViewBag.PendingApproval=db.get_live_auction(Convert.ToInt32(Session["clientid"]),"NotApproved");
            ViewBag.BidsDetail = db.get_Bid_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.ItemDetail = db.get_item_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.TenderDetail = db.get_tender_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.AddedSupplier = db.Get_AddedSupplier(Convert.ToInt32(securityUtil.Decode(tenderid)));
            ViewBag.Tracking = db.get_Tracking(Convert.ToInt32(securityUtil.Decode(tenderid)));
            return View();
        }

        [HttpPost]
        public ActionResult ApproveTender(string tenderid, string Approval, string RemarksText)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            db.update_tenderStatus(Convert.ToInt32(securityUtil.Decode(tenderid)), Approval, Convert.ToInt32(Session["clientid"]), RemarksText);
            try
            {   if(Approval=="Approved")
                {
                    int client = Convert.ToInt32(Session["clientid"]);
                    string Tenderdesc=string.Empty;
                    string tenderdate=string.Empty;
                    var buyermail = (from a in db.RegisterMasters join b in db.LoginMasters on a.RegisterId equals b.RegisterId where b.ClientId == client select a.Email).Single();
                        
                    var items=db.get_tender_detail(Convert.ToInt32(securityUtil.Decode(tenderid)),Convert.ToInt32(Session["Clientid"]));
                    foreach(var tender in items)
                    {
                        Tenderdesc=tender.Tenderdesc;
                        tenderdate=tender.StartDate+" "+tender.StartTime;
                    }
                    var suplist = db.get_supplierlist(Convert.ToInt32(securityUtil.Decode(tenderid))).ToList();
                    foreach (var item in suplist)
                    {
                        SendEmail(item.ContactPerson.ToString(), item.ClientId.ToString(), Tenderdesc, tenderdate, item.Email,buyermail);
                    }
                }
                
                return RedirectToAction("PendingApproval", "Tender");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Purchaser(string tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.BidsDetail = db.get_Bid_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.ItemDetail = db.get_item_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.ItemDetails = db.get_item_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.TenderDetail = db.get_tender_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
           return View();
        }
        [HttpPost]
        public ActionResult Purchaser(string tenderid,string minutes)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if(minutes!=null)
            {
                string ipaddress = Request.UserHostAddress;
            db.add_min(Convert.ToInt32(securityUtil.Decode(@tenderid)), Convert.ToInt32(minutes),Convert.ToInt32(Session["clientid"]),ipaddress);
            return RedirectToAction("Purchaser","Tender", new { @tenderid=tenderid});
                }
            return View();
        }
        [HttpGet]
        public ActionResult PendingApproval()
        {
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            ViewBag.PendingApproval = db.get_live_auction(Convert.ToInt32(Session["clientid"]), "NotApproved");
            return View();
            
        }

        [HttpGet]
        public ActionResult Seller(string tenderid,string Eguid)
        {
            if(Eguid!=null)
            {
                var data = db.ValidateGUID(Eguid).ToList();
                int count = data.FirstOrDefault().counts.Value;
                if(count==1)
                {

                    Session["clientid"] = data.FirstOrDefault().clientid.Value;
                    tenderid = securityUtil.Encode(Convert.ToString( data.FirstOrDefault().Tenderid.Value));
                    return RedirectToAction("Seller", "Tender", new { @tenderid = tenderid });
                }
                
            }
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var checkaccept = db.check_SupplierAcceptance(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"])).ToList().Single().Value;
            if(checkaccept==0)
            {
                return RedirectToAction("Disclaimer", "Tender", new { @tenderid = tenderid });
            }

            ViewBag.BidsDetail = db.get_Bid_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.ItemDetail = db.get_item_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.ItemDetails = db.get_item_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.TenderDetail = db.get_tender_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
           
            return View();            
        }

        public ActionResult gettenderdetail(string tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var tenderdetail = db.get_tender_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["clientid"]));
            return Json(tenderdetail, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult Seller(string tenderid, String itemid, int? Price, int? PriceExpected,string RemarksText,int Unit)
        {
            
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string ipaddress = Request.UserHostAddress;
            var checktime = db.check_tendertime(Convert.ToInt32(securityUtil.Decode(itemid)), Convert.ToInt32(Session["clientid"])).ToList().Single().Value;
            if (checktime == 0)
            {
                db.update_tenderStatus(Convert.ToInt32(securityUtil.Decode(tenderid)), "Completed", Convert.ToInt32(Session["clientid"]), RemarksText);
                return Content("<script language='javascript' type='text/javascript'>alert('Time Close');window.location = '../Home/Index';</script>");
            }
            if (Price==null)
            {
                var checkflag = db.Add_Bid(Convert.ToInt32(securityUtil.Decode(itemid)), Convert.ToInt32(Session["clientid"]), PriceExpected,ipaddress,Unit).ToList().Single().Value;

                if(checkflag==1)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Successfully Bid Added');window.location = '../Tender/Seller?tenderid="+tenderid+"';</script>");
                    
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Current Bid Amount already taken by other. Please try for next bid');window.location = '../Tender/Seller?tenderid=" + tenderid + "';</script>");
                }
            }
            else
            {
                db.Add_Bid(Convert.ToInt32(securityUtil.Decode(itemid)), Convert.ToInt32(Session["Clientid"]), Price,ipaddress,Unit);
            }
            return RedirectToAction("Seller", "Tender", new { @tenderid = tenderid });
            ViewBag.BidsDetail = db.get_Bid_detail(Convert.ToInt32(securityUtil.Decode( tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.ItemDetail = db.get_item_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.ItemDetails = db.get_item_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.TenderDetail = db.get_tender_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"]));
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return View();
        }
        [HttpGet]
        public ActionResult Download(string tenderid)
        {
            try
            {
                string files = db.get_tender_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["clientid"])).ToList().FirstOrDefault().Attachment;
                var filepath = System.IO.Path.Combine(Server.MapPath("~/upload/"), files);
                return File(filepath, MimeMapping.GetMimeMapping(files), files);
            }
            catch(Exception ex)
            {
                return View();
            }
            
        }

        [HttpGet]
        public ActionResult Disclaimer(string tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.termsconditions = db.GetTerms(Convert.ToInt32(securityUtil.Decode(tenderid)));
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return View();
        }
        [HttpPost]
        public ActionResult Disclaimer(string tenderid,string Acceptance)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            var checkaccept = db.check_SupplierAcceptance(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["Clientid"])).ToList().Single().Value;
            if(checkaccept==0)
            { 
            
            db.update_SupplierAcceptance(Convert.ToInt32(securityUtil.Decode(tenderid)), Acceptance, Convert.ToInt32(Session["Clientid"]));
            return RedirectToAction("Seller", "Tender", new { @tenderid = tenderid });
            }
            return View();
        }

        public JsonResult getbiddata(string itemid,string tenderid,int price,int unit)
        {
            
            var bids = db.get_ReduceBidPrice(Convert.ToInt32(securityUtil.Decode( itemid)),price,unit);
            
            return Json(bids,JsonRequestBehavior.AllowGet);
        }

        public ActionResult getitems(string tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var itemlist = db.get_item_detail(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["clientid"]));
                return Json(itemlist, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return View();
            }
        }
        
        [HttpGet]
        public ActionResult Edititem(string itemid)
        {
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return View();
        }

        [HttpPost]
        public ActionResult Edititem()
        {
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return View();
        }
        
        public JsonResult getnotify(string tenderid)
        {

            var notify = db.get_notify(Convert.ToInt32(securityUtil.Decode(tenderid)), Convert.ToInt32(Session["clientid"]));
            return Json(notify, JsonRequestBehavior.AllowGet);
        }

        public ActionResult updateTender(string tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            db.update_tenderStatus(Convert.ToInt32(securityUtil.Decode(tenderid)), "Completed", Convert.ToInt32(Session["clientid"]), "Autoclose");
            return Content("<script language='javascript' type='text/javascript'>alert('Time Close');window.location = '../Home/Index';</script>");
        }
        [HttpGet]
        public ActionResult Report(string tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.reportitem=db.tenderitemreport_print(Convert.ToInt32(securityUtil.Decode(tenderid))).ToList();
            return View();
        }
        [HttpGet]
        
        public ActionResult Terms(string tenderid)
        {
            if (Session["clientid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Terms(string tenderid,string ck)
        {
            db.AddTerms(Convert.ToInt32(securityUtil.Decode( tenderid)), ck);
            return RedirectToAction("Additems", "TenderItem", new { @tenderid = tenderid });
            ViewBag.Menu = db.get_role(Convert.ToInt32(Session["clientid"]));
            return View();
        }

        public void SendEmail(string ContactPerson,string Clientid,string TenderDesc,string Datetime,string Email,string ccmail)
        {
            string subject = string.Empty;
            string mailbody = string.Empty;

            StringBuilder Body = new StringBuilder("<table width='600' border='0' cellspacing='0' cellpadding='0' align='center'>");
            Body.Append("<tr>");
            Body.Append("<td style='border: solid 1px #d2d2d2'>");
            Body.Append("<table width='600' border='0' cellspacing='0' cellpadding='0'>");
            Body.Append("<tr><td style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #fff; font-size: 18px;width:100%' bgcolor='#2b576e'>EBEX : Ebid</td></tr>");

            Body.Append("<tr><td>&nbsp;</td></tr>");
            Body.Append("<tr><td style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px'>Dear "+ContactPerson+", </td></tr>");
            Body.Append("<tr><td>&nbsp;</td></tr>");
            Body.Append("<tr><td style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px' colspan='2'>You have been Select for below Auction:</td></tr>");
            Body.Append("<tr><td valign='top' align='left'><table width='600' border='0' cellspacing='0' cellpadding='0'>");

            if (Clientid != "")
            {
                Body.Append("<tr>");
                Body.Append("<td width='100' style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px'><strong>Client ID</strong>: </td>");
                Body.Append("<td width='500' style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px'>" + Clientid+ "</td>");
                Body.Append("</tr>");
            }
            if (TenderDesc != "")
            {
                Body.Append("<tr>");
                Body.Append("<td  style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px'><strong>Tender Description</strong>: </td>");
                Body.Append("<td style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px'>" + TenderDesc + "</td>");
                Body.Append("</tr>");
            }
            if (Datetime != "")
            {
                Body.Append("<tr>");
                Body.Append("<td  style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px'><strong>Date/Time</strong>: </td>");
                Body.Append("<td  style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px'>" + Datetime + "</td>");
                Body.Append("</tr>");
            }
            string Website = "http://www.zeeintranet.com/ebid/";
            
                Body.Append("<tr>");
                Body.Append("<td  style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px'><strong>Website</strong>: </td>");
                Body.Append("<td  style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px'>"+Website+"</td>");
                Body.Append("</tr>");
            

            Body.Append("<tr><td style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px' colspan='2'>Please be on Time and read all Terms & Condition,BOQ before accepting.</td></tr>");
            Body.Append("<tr><td colspan='2'>&nbsp;</td></tr>");
            Body.Append("<tr><td style='padding: 7px 15px; font-family: Arial, Helvetica, sans-serif; color: #000; font-size: 15px' colspan='2'>Regards,<br/>Team EBEX<br/></td></tr>");
            Body.Append("<tr><td colspan='2'>&nbsp;</td></tr>");
            Body.Append("</table></td></tr>");

            Body.Append("</table>");
            Body.Append("</td>");
            Body.Append("</tr>");
            Body.Append("</table>");

            subject = "Ebex Ebid";
            clsCommon.SendMail(Email, Body.ToString(), subject,ccmail);
        }
        
	}
}