using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auto.ModelsApp
{
    public class UpdateCar
    {
        public AppCar Car { get; set; }
        public SelectList Brands { get; set; }
    }
}