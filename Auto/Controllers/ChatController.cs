using Auto.Mappers;
using Auto.ModelsApp;
using DomainCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auto.Controllers
{
    public class ChatController : Controller
    {
        IService unit;
        public ChatController(IService Unit)
        {
            unit = Unit;
        }
        // GET: Chat
        public ActionResult Index(int? OwnerId, int UserId, int CarId)
        {
            if (OwnerId == null)
            {
                ViewBag.Message = "Данный диалог более не поддерживается, так как заявка удалена";
                return View("Buy");
            }
            List<AppMessage> messages = unit.OwnerGetMessages((int)OwnerId, UserId, CarId).Select(x => x.FromDomainMessageToAppMessage()).ToList();
            FormForDialoge dialoge = new FormForDialoge();
            dialoge.Id = messages[0].SpeachId;
            dialoge.Name = User.Identity.Name;
            dialoge.messages = messages;
            return View(dialoge);
        }

        [HttpPost]
        public JsonResult sendmsg(string message,string name, int SpeachId)
        {
            unit.CreateMessage(SpeachId, name, message);
            return Json(null);
        }
    }
}