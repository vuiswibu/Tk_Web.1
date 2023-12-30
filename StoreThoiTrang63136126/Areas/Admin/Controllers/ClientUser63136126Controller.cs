﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using StoreThoiTrang63136126.Models;

namespace StoreThoiTrang63136126.Areas.Admin.Controllers
{
    public class ClientUser63136126Controller : Base63136126Controller  
    {
        private StoreThoiTrang63136126Entities db = new StoreThoiTrang63136126Entities();

        // GET: Admin/ClientUser63136126
        [HttpGet]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            ViewBag.searchString = searchString;
            var taikhoans = db.TaiKhoanNguoiDungs.Select(tk => tk);
            if (!String.IsNullOrEmpty(searchString))
            {
                taikhoans = taikhoans.Where(tk => tk.TenDangNhap.Contains(searchString));
            }
            return View(taikhoans.OrderBy(tk => tk.MaTK).ToPagedList(page, pageSize));
        }

        [HttpPost]
        public JsonResult Index(int id)
        {
            TaiKhoanNguoiDung tk = db.TaiKhoanNguoiDungs.Where(a => a.MaTK.Equals(id)).FirstOrDefault();
            return Json(tk, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(int Matk)
        {
            try
            {
                TaiKhoanNguoiDung update = db.TaiKhoanNguoiDungs.Where(a => a.MaTK.Equals(Matk)).FirstOrDefault();
                update.TrangThai = !update.TrangThai;
                db.Entry(update).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { status = true, message = "Sửa thông tin thành công" });
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "Có lỗi gì đó. Thử lại sau !" });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                TaiKhoanNguoiDung tk = db.TaiKhoanNguoiDungs.Where(a => a.MaTK.Equals(id)).FirstOrDefault();
                db.TaiKhoanNguoiDungs.Remove(tk);
                db.SaveChanges();
                return Json(new { status = true });
            }
            catch (Exception)
            {
                return Json(new { status = false });
            }
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
