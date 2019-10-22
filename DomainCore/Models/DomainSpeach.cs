using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainCore.Models
{
    public class DomainSpeach
    {
        public int Id { get; set; }
        public int IdCar { get; set; }
        public int IdOwner { get; set; }
        public string Name { get; set; }
        public int IdUser { get; set; }
        public string LastMes { get; set; }
        public bool Readed { get; set; }
        public bool IsDeleted { get; set; }
    }
}
