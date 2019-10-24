using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auto.ModelsApp
{
    public class FormForDialoge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AppMessage> messages { get; set; }
    }
}