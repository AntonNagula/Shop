using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auto.ModelsApp
{
    public class BodyPost
    {
        public int page { get; set; }
        public string brand { get; set; }
        public int? minPrise { get; set; }
        public int? maxPrice { get; set; }
    }
}