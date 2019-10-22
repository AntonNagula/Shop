using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrustructureData.DataModels
{
    public class Speach
    {
        public int Id { get; set; }
        public int IdCar { get; set; }
        public int IdUser { get; set; }
        public int IdOwner { get; set; }
        public string LastMes { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Message> Messages { get; set; }
        public Speach()
        {
            Messages = new List<Message>();
        }
    }
}
