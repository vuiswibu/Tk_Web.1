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
    public class AdminUser63136126Controller : Base63136126Controller
    {
        private StoreThoiTrang63136126Entities db = new StoreThoiTrang63136126Entities();

        // GET: Admin/AdminUser63136126
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            TaiKhoanQuanTri ses = (TaiKhoanQuanTri)Session[StoreThoiTrang63136126.Session.ConstaintUser.ADMIN_SESSION];
            if (ses.LoaiTaiKhoan == false)
            {
                return RedirectToAction("Index", "Home63136126");
            }
            ViewBag.searchString = searchString;
            var taikhoans = db.TaiKhoanQuanTris.Select(tk => tk);
            if (!String.IsNullOrEmpty(searchString))
            {
                taikhoans = taikhoans.Where(tk => tk.TenDangNhap.Contains(searchString));
            }
            return View(taikhoans.OrderBy(tk => tk.ID).ToPagedList(page, pageSize));
        }

        [HttpPost]
        public JsonResult Create(TaiKhoanQuanTri tk)
        {
            try
            {
                TaiKhoanQuanTri check = db.TaiKhoanQuanTris.Where(a => a.TenDangNhap.Equals(tk.TenDangNhap)).FirstOrDefault();
                if (check != null)
                {
                    return Json(new { status = false, message = "Tên đăng nhập đã tồn tại!" });
                }
                else
                {
                    tk.TrangThai = true;
                    db.TaiKhoanQuanTris.Add(tk);
                    db.SaveChanges();
                    return Json(new { status = true, message = "Thêm thành công!" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "Thêm không thành công!" });
            }
        }

        [HttpPost]
        public JsonResult Index(int id)
        {
            TaiKhoanQuanTri tk = db.TaiKhoanQuanTris.Where(a => a.ID.Equals(id)).FirstOrDefault();
            return Json(tk, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(TaiKhoanQuanTri tk)
        {
            TaiKhoanQuanTri login = (TaiKhoanQuanTri)Session[StoreThoiTrang63136126.Session.ConstaintUser.ADMIN_SESSION];
            try
            {
                TaiKhoanQuanTri update = db.TaiKhoanQuanTris.Where(a => a.ID.Equals(tk.ID)).FirstOrDefault();
                if (update.TenDangNhap.Equals(login.TenDangNhap))
                {
                    return Json(new { status = false, message = "Bạn không thể sửa tài khoản này !" });
                }
                update.LoaiTaiKhoan = tk.LoaiTaiKhoan;
                update.TrangThai = tk.TrangThai;
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
            TaiKhoanQuanTri login = (TaiKhoanQuanTri)Session[StoreThoiTrang63136126.Session.ConstaintUser.ADMIN_SESSION];
            try
            {
                TaiKhoanQuanTri tk = db.TaiKhoanQuanTris.Where(a => a.ID.Equals(id)).FirstOrDefault();
                if (tk.TenDangNhap.Equals(login.TenDangNhap))
                {
                    return Json(new { status = false, message = "Bạn không thể xóa tài khoản này !" });
                }
                db.TaiKhoanQuanTris.Remove(tk);
                db.SaveChanges();
                return Json(new { status = true });
            }
            catch (Exception)
            {
                return Json(new { status = false });
            }
        }
    }
}
