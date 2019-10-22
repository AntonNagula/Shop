using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auto.ModelsApp
{
    public class AppBuyer
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public int TelegramID { get; set; }
        public int AuthCode { get; set; }
        public virtual ICollection<AppBuyCar> BuyCars { get; set; }
        public AppBuyer()
        {
            BuyCars = new List<AppBuyCar>();
        }
    }
}