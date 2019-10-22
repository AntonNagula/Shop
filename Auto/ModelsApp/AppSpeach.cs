using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auto.ModelsApp
{
    public class AppSpeach
    {
        public int Id { get; set; }
        public int IdCar { get; set; }
        public int IdOwner { get; set; }
        public string Name { get; set; }
        public int IdUser { get; set; }
        public string LastMes { get; set; }
        public bool readed { get; set; }
        public bool IsDeleted { get; set; }
    }
}