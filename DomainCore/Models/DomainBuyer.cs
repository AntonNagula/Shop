using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainCore.Models
{
    public class DomainBuyer
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public int TelegramID { get; set; }
        public int AuthCode { get; set; }
        public virtual ICollection<DomainBuyCar> BuyCars { get; set; }
        public DomainBuyer()
        {
            BuyCars = new List<DomainBuyCar>();
        }
    }
}
