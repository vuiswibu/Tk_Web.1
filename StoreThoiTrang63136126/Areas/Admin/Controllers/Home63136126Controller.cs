using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreThoiTrang63136126.Models;

namespace StoreThoiTrang63136126.Areas.Admin.Controllers
{
    public class Home63136126Controller : Base63136126Controller
    {
        private StoreThoiTrang63136126Entities db = new StoreThoiTrang63136126Entities();

        // GET: Admin/Home63136126
        public ActionResult Index()
        {
            return View();
        }
    }
}
