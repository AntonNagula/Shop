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
        public ActionResult Index(int i = 1,string brand="Все",string minPrise=null,string maxPrice=null)
        {
            int Total, size = 6;
            int min=minPrise==null ? 0 : int.Parse(minPrise), max=maxPrice==null ? 0 : int.Parse(maxPrice);
            string mail = HttpContext.User.Identity.Name;
            int IdBuyer= unit.GetBuyer(mail).Id;
            List<AppCar> b = unit.Annociment(i, size,brand, out Total,IdBuyer, min, max).Select(x => x.FromDomainCarToRepoCar()).ToList();
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
        public ActionResult Buy(int id)
        {
            string mail=HttpContext.User.Identity.Name;
            int buyerId = unit.GetBuyer(mail).Id;
            AppBuyCar b = new AppBuyCar { BuyerId = buyerId, CarId = id };
            bool result=unit.Buy(b.FromAppCarToDomainBuyCar());
            string message;
            if (result == true)
            {
                message = "Благодарим за покупку";
               // await Task.Run(()=>unit.SendMassege(mail,id));
            }
            else
                message = "Вы уже отметили";
            ViewBag.Message = message;
            return View();
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
        public ActionResult Connect(EmailModel model)
        {
            string type_mail = model.From.Split(new char[] { '@' })[1].Split(new char[] { '.' })[0];
            MailAddress from = new MailAddress(model.From, "АвтоМагазин");
            MailAddress to = new MailAddress(model.To);
            MailMessage m = new MailMessage(from, to);
            m.Subject = model.Subject;
            m.IsBodyHtml = false;
            m.Body = model.Body;
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 25);
            if (type_mail == "gmail")
            {
                smtp = new SmtpClient("smtp." + type_mail + ".com", 587);
            }
            else
            {
                smtp = new SmtpClient("smtp." + type_mail + ".ru", 25);
            }
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(model.From, model.password);
            smtp.EnableSsl = true;
            smtp.Send(m);
            ViewBag.Message = "Сообщение отправлено";
            return View("Buy");
        }

        [HttpPost]
        public ActionResult DeletePurch(int Id)
        {            
            int BuyerId = unit.GetBuyer(User.Identity.Name).Id;
            unit.Delete_Purchase(Id,BuyerId);
            unit.RejectMessage(Id,BuyerId);
            List<AppCar> Cars = unit.GetCarsBuyerId(BuyerId).Select(x => x.FromDomainCarToRepoCar()).ToList();
            return View(Cars);            
        }        

        [HttpGet]
        public ActionResult Speach(int Id, int CarId)
        {
            List<AppMessage> messages = unit.GetMessages(Id, CarId).Select(x => x.FromDomainMessageToAppMessage()).ToList();
            return View(messages);
        }

        [HttpPost]
        public ActionResult Speach(int SpeachId, string message)
        {
            unit.CreateMessage(SpeachId,HttpContext.User.Identity.Name,message);
            List<AppMessage> messages = unit.GetMessages(SpeachId).Select(x => x.FromDomainMessageToAppMessage()).ToList();
            return View(messages);
        }
        
        [HttpGet]
        public ActionResult OwnerSpeach(int OwnerId, int UserId, int CarId)
        {
            List<AppMessage> messages = unit.OwnerGetMessages(OwnerId, UserId, CarId).Select(x => x.FromDomainMessageToAppMessage()).ToList();
            return View(messages);
        }

        [HttpPost]
        public ActionResult OwnerSpeach(int SpeachId, string message)
        {
            unit.CreateMessage(SpeachId, HttpContext.User.Identity.Name, message);
            List<AppMessage> messages = unit.GetMessages(SpeachId).Select(x => x.FromDomainMessageToAppMessage()).ToList();
            return View(messages);
        }
        
        [HttpGet]
        public ActionResult AutoSpeach(int Id)
        {
            List<AppSpeach> speaches=unit.GetAutoSpeach(Id).Select(x=>x.FromDomainSpeachToSpeachSpeach()).ToList();
            return View(speaches);
        }

        [HttpPost]
        public ActionResult DelSpeach(int IdSpeach, int AutoId)
        {
            unit.DelSpeach(IdSpeach);
            List<AppSpeach> speaches = unit.GetAutoSpeach(AutoId).Select(x => x.FromDomainSpeachToSpeachSpeach()).ToList();
            return View(speaches);
        }
    }
}