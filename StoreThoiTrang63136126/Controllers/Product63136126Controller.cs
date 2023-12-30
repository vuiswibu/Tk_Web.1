using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using StoreThoiTrang63136126.Models;

namespace StoreThoiTrang63136126.Controllers
{
    public class Product63136126Controller : Controller
    {
        private StoreThoiTrang63136126Entities db = new StoreThoiTrang63136126Entities();

        // GET: Product63136126
        public ActionResult Shop(string searchString, int? madm, int page = 1, int pageSize = 9)
        {
            ViewBag.searchString = searchString;
            ViewBag.madm = madm;
            var sanphams = db.SanPhams.Select(p => p);
            if (!String.IsNullOrEmpty(searchString))
            {
                sanphams = sanphams.Where(sp => sp.TenSP.Contains(searchString));
            }
            if (madm != null && madm != 0)
            {
                sanphams = sanphams.Where(s => s.MaDM == madm);
                ViewBag.DanhMuc = db.DanhMucs.Where(d => d.MaDM == madm).FirstOrDefault();
            }
            return View(sanphams.OrderBy(sp => sp.MaSP).ToPagedList(page, pageSize));
        }

        public ActionResult ProductDetail(int id)
        {
            SanPham sp = db.SanPhams.Include("DanhMuc").Where(s => s.MaSP.Equals(id)).FirstOrDefault();
            List<SanPhamChiTiet> list = db.SanPhamChiTiets.Include("KichCo").Where(s => s.MaSP.Equals(id)).ToList();
            ViewBag.SPCT = list;
            ViewBag.Exitst = list[0];
            return View(sp);
        }

        [HttpPost]
        public JsonResult Index(int id)
        {
            Trace.WriteLine("" + id);
            SanPham sp = db.SanPhams.Include("DanhMuc").Include("SanPhamChiTiets").Where(s => s.MaSP.Equals(id)).FirstOrDefault();
            return Json(sp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Detail(int id)
        {
            SanPhamChiTiet spct = db.SanPhamChiTiets.Where(sp => sp.IDCTSP == id).FirstOrDefault();
            return Json(spct, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
