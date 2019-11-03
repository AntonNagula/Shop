using Auto.Mappers;
using Auto.Models;
using Auto.ModelsApp;
using DomainCore.Interfaces;
using DomainCore.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Auto.Controllers
{
    [Authorize(Roles ="user")]
    public class BuyerController : Controller
    {
        IService unit;
        public BuyerController(IService Unit)
        {
            unit=Unit;
        }

        public ActionResult New()
        {
            return View();
        }
        public ActionResult Index(int i = 1,string brand="Все")
        {
            int Total, size = 6;
            
            string mail = HttpContext.User.Identity.Name;
            int IdBuyer= unit.GetBuyer(mail).Id;
            List<AppCar> b = unit.Annociment(i, size,brand, out Total,IdBuyer).Select(x => x.FromDomainCarToRepoCar()).ToList();
            PageInfo page = new PageInfo { PageSize = size, PageNumber = i, TotalItems = Total };
            ModelsApp.IndexViewModel index = new ModelsApp.IndexViewModel { PageInfo = page, Cars = b };
            List<string> CarBrands = unit.GetAllBrands().Select(x => x.FromAppBrandToBrand()).ToList();
            CarBrands.Insert(0,"Все");
            BrandsListViewModel brands = new BrandsListViewModel { PageInfo = page, Cars = b, Brands = new SelectList(CarBrands) };
            return View(brands);
        }

        public ActionResult ShowCar(int id)
        {
            AppCar car = unit.GetCar(id).FromDomainCarToRepoCar();
            return View(car);
        }
        [HttpPost]
        public JsonResult Buy(int id)
        {
            string mail=HttpContext.User.Identity.Name;
            int buyerId = unit.GetBuyer(mail).Id;
            AppBuyCar b = new AppBuyCar { BuyerId = buyerId, CarId = id };
            bool result=unit.Buy(b.FromAppCarToDomainBuyCar());
            string message;            
            if (result == true)
            {
                message = "Благодарим за покупку";
                return Json(new { value = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                message = "Вы уже отметили";
                return Json(new { value = message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            string mail = HttpContext.User.Identity.Name;
            int buyerId = unit.GetBuyer(mail).Id;
            ViewBag.BuyerId = buyerId;
            List<AppCarBrand> brands = unit.GetAllBrands().Select(x => x.FromDomainBrandsToAppCarBrand()).ToList();
            brands.Add(new AppCarBrand { Id = 0, BrandName = "Свой бренд" });
            DropList ViewBrands = new DropList { brands = new SelectList(brands, "Id", "BrandName") };
            return View(ViewBrands);
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase upload, AppCar ViewCar)
        {
            if (upload != null)
            {
                if(ViewCar.Id==0 && ViewCar.CarBrand=="")
                {
                    ViewBag.Message = "Вы не указали бренд";
                    return View("~/Views/Buyer/Buy.cshtml");
                }
                string fileName = Path.GetFileName(upload.FileName);
                string ext = Path.GetExtension(fileName);
                string ExtencionName = ViewCar.Name + ext;
                upload.SaveAs(Server.MapPath("~/Files/" + ViewCar.Name + ext));
                byte[] avatar = new byte[upload.ContentLength];
                upload.InputStream.Read(avatar, 0, upload.ContentLength);
                DomainCar car = new DomainCar(ViewCar.Name, ViewCar.CarBrand,ViewCar.BrandId, ViewCar.Price, ExtencionName, ViewCar.Info,avatar);
                string mail = HttpContext.User.Identity.Name;
                car.OwnerId = unit.GetBuyer(mail).Id;
                car.Status = "Продается";
                unit.Create_Car(car);
            }
            ViewBag.Message = "Авто занесено в базу данных";
            return View("~/Views/Buyer/Buy.cshtml");
        }

        public ActionResult MyAnnouncement()
        {
            string mail = HttpContext.User.Identity.Name;
            int id = unit.GetBuyer(mail).Id;
            List<AppCar> Cars = unit.GetCarsByOwnerId(id).Select(x=>x.FromDomainCarToRepoCar()).ToList();
            return View(Cars);
        }

        public ActionResult Purchases()
        {
            string mail = HttpContext.User.Identity.Name;
            int id = unit.GetBuyer(mail).Id;
            ViewBag.Id = id;
            List<AppCar> Cars = unit.GetCarsBuyerId(id).Select(x => x.FromDomainCarToRepoCar()).ToList();
            
            return View(Cars);
        }

        public ActionResult BuyersForCar(int Id)
        {
            List<AppBuyer> buyers=unit.GetBuyersByCarId(Id).Select(x => x.FromDomainBuyerToRepoBuyer()).ToList();
            ViewBag.CarId = Id;
            ViewBag.OwnerId = unit.GetBuyer(HttpContext.User.Identity.Name).Id;
            return View(buyers);
        }

        [HttpGet]
        public ActionResult UpdateCar(int Id)
        {
            List<string> CarBrands = unit.GetAllBrands().Select(x => x.FromAppBrandToBrand()).ToList();
            CarBrands.Insert(0, "Все");
            AppCar car = unit.GetCar(Id).FromDomainCarToRepoCar();
            UpdateCar updateCar = new UpdateCar { Brands=new SelectList(CarBrands), Car=car  };
            return View(updateCar);
        }

        [HttpPost]
        public ActionResult UpdateCar(HttpPostedFileBase upload, AppCar ViewCar)
        {
            AppCar car = unit.GetCar(ViewCar.Id).FromDomainCarToRepoCar();
            if (upload != null)
            {
                string fileName = Path.GetFileName(upload.FileName);
                string ext = Path.GetExtension(fileName);
                string ExtencionName = ViewCar.Name + ext;
                upload.SaveAs(Server.MapPath("~/Files/" + ViewCar.Name + ext));
                byte[] avatar = new byte[upload.ContentLength];
                upload.InputStream.Read(avatar, 0, upload.ContentLength);
                car.ExtencionName = ExtencionName;
            }
            car.Price = ViewCar.Price;
            car.Name = ViewCar.Name;
            car.Info = ViewCar.Info;
            car.BrandId = ViewCar.BrandId;
            car.CarBrand = ViewCar.CarBrand;
            unit.Update_Car(car.FromRepoCarToDomainCar());
            ViewBag.Message = "Обновление сохранено в базе данных";
            return View("~/Views/Buyer/Buy.cshtml");
        }

        [HttpGet]
        public ActionResult DeleteCar(int Id)
        {            
            unit.Delete_Car(Id);
            ViewBag.Message = "Удаление прошло успешно";
            return View("~/Views/Buyer/Buy.cshtml");
        }

        [HttpGet]
        public ActionResult Connect(int Id)
        {
            AppBuyer appBuyer = unit.GetBuyer(Id).FromDomainBuyerToRepoBuyer();
            return View(appBuyer);
        }

        [HttpPost]
        public ActionResult DeletePurch(int Id)
        {            
            int BuyerId = unit.GetBuyer(User.Identity.Name).Id;
            unit.Delete_Purchase(Id,BuyerId);            
            List<AppCar> Cars = unit.GetCarsBuyerId(BuyerId).Select(x => x.FromDomainCarToRepoCar()).ToList();
            return View(Cars);            
        }

        // Вызывается когда продавец хочет поговорить с покупателем
        [HttpGet]
        public ActionResult Speach(int IdUser, int CarId)
        {
            List<AppMessage> messages = unit.GetMessages(IdUser, CarId).Select(x => x.FromDomainMessageToAppMessage()).ToList();
            return View(messages);
        }

        [HttpPost]
        public ActionResult Speach(int SpeachId, string message)
        {
            unit.CreateMessage(SpeachId, HttpContext.User.Identity.Name, message);
            List<AppMessage> messages = unit.GetMessages(SpeachId).Select(x => x.FromDomainMessageToAppMessage()).ToList();
            return View(messages);
        }

        // Вызывается когда покупатель хочет поговорить с продавцом
        [HttpGet]
        public ActionResult SpeachWithOwner(int? OwnerId, int UserId, int CarId)
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
        public ActionResult SpeachWithOwner(int SpeachId, string message)
        {
            unit.CreateMessage(SpeachId, HttpContext.User.Identity.Name, message);
            List<AppMessage> messages = unit.GetMessages(SpeachId).Select(x => x.FromDomainMessageToAppMessage()).ToList();
            return View(messages);
        }
        
        [HttpGet]
        public ActionResult AutoSpeach(int Id)
        {
            List<AppSpeach> speaches=unit.GetAutoSpeach(Id,HttpContext.User.Identity.Name).Select(x=>x.FromDomainSpeachToSpeachSpeach()).ToList();
            return View(speaches);
        }

        [HttpPost]
        public ActionResult DelSpeach(int IdSpeach, int AutoId)
        {
            unit.DelSpeach(IdSpeach);
            List<AppSpeach> speaches = unit.GetAutoSpeach(AutoId,HttpContext.User.Identity.Name).Select(x => x.FromDomainSpeachToSpeachSpeach()).ToList();
            return View(speaches);
        }

        // методы для отправка и принятия сообщений с помощью ajax
        [HttpPost]
        public JsonResult sendmsg(string message, int SpeachId)
        {
            unit.CreateMessage(SpeachId, HttpContext.User.Identity.Name, message);
            return Json(null);
        }

        // Два метода для привязки телеграмма к аккаунту
        [HttpGet]
        public ActionResult ConnectTelegramm()
        {
            string mail = User.Identity.Name;
            AppBuyer buyer = unit.GetBuyer(mail).FromDomainBuyerToRepoBuyer();
            return View(buyer);
        }

        [HttpPost]
        public JsonResult ConnectTelegramm(int Id,string telephone)
        {
            int val=unit.Create_Code_Telegramm(Id,telephone);            
            return Json(new { value = val }, JsonRequestBehavior.AllowGet);
        }        
    }
}