using SignalRDbUpdates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using SignalRDbUpdates.Hubs;


namespace SignalRDbUpdates.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.msgReturnCreate = TempData["msgReturnCreate"];
            ViewBag.msgReturnUpdate = TempData["msgReturnUpdate"];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetMessages()
        {
            MessagesRepository _messageRepository = new MessagesRepository();
            return PartialView("_MessagesList", _messageRepository.GetAllMessages());
        }

        [HttpGet]
        public ActionResult Monitor()
        {
            return View();
        }

        public ActionResult MonitorGetAll()
        {
            MessagesRepository _messageRepository = new MessagesRepository();
            return PartialView("_Monitor", _messageRepository.GetAllMessages());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(string CampaignName, DateTime MessageDate, int Clicks, int Conversions, int Impressions, string AffiliateName)
        {
            MessagesRepository _messageRepository = new MessagesRepository();
            Messages objMsg = new Messages();
            objMsg.CampaignName = CampaignName;
            objMsg.MessageDate = MessageDate;
            objMsg.Clicks = Clicks;
            objMsg.Conversions = Conversions;
            objMsg.Impressions = Impressions;
            objMsg.AffiliateName = AffiliateName;

            //string msgReturn = _messageRepository.InsertMessages(objMsg.CampaignName, MessageDate, Clicks, Conversions, Impressions, AffiliateName);
            string msgReturn = _messageRepository.InsertMessages(objMsg);
            //if (msgReturn == "Success")
            //{
            //    TempData["msgReturnCreate"] = "Record has been Created successfully.";
            //}
            //else
            //{
            //    TempData["msgReturnCreate"] = "Error Gets Generated for Create Record";
            //}
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DeleteUser(int GridId)
        {
            MessagesRepository _messageRepository = new MessagesRepository();
            string msgReturn = _messageRepository.DeleteMessages(GridId);
            string msgName = "deleted Record";
            MessagesHub.UpdateStatus(GridId, msgName);
            var data = msgReturn;
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Update(int id)
        {
            Messages model = new Messages();
            MessagesRepository _messageRepository = new MessagesRepository();
            model = _messageRepository.GetAllMessagesByID(id);
            return View(model);
        
        }

        [HttpPost]
        public ActionResult Update(int Id,DateTime MessageDate,string CampaignName, int Clicks, int Conversions, int Impressions, string AffiliateName)
        {
            MessagesRepository _messageRepository = new MessagesRepository();
            Messages objMsg = new Messages();
            objMsg.CampaignName = CampaignName;
            objMsg.MessageDate = MessageDate;
            objMsg.Clicks = Clicks;
            objMsg.Conversions = Conversions;
            objMsg.Impressions = Impressions;
            objMsg.AffiliateName = AffiliateName;
            objMsg.ID = Id;
            //string msgReturn = _messageRepository.UpdateMessages(Id, MessageDate, CampaignName, Clicks, Conversions, Impressions, AffiliateName);
            string msgReturn = _messageRepository.UpdateMessages(objMsg);
            
            //if (msgReturn == "Success")
            //{
            //    TempData["msgReturnUpdate"] = "Record has been Updated successfully.";
            //}
            //else
            //{
            //    TempData["msgReturnUpdate"] = "Error Gets Generated for Update Record";
            //}
            //return Json(new { data }, JsonRequestBehavior.AllowGet);
            return View("Index");
        }
    }
}