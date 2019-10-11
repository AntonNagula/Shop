using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auto.ModelsApp
{
    public class AppMessage
    {
        public int Id { get; set; }
        public int SpeachId { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
}