using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrustructureData.DataModels
{
    public class Message
    {
        public int Id { get; set; }
        public int? SpeachId { get; set; }
        public int IdUser { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public Speach Speach { get; set; }
    }
}
