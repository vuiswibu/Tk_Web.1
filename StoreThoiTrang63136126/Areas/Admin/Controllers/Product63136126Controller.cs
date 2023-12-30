﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.Script.Serialization;
using StoreThoiTrang63136126.Models;

namespace StoreThoiTrang63136126.Areas.Admin.Controllers
{
    public class Product63136126Controller : Base63136126Controller
    {
        private StoreThoiTrang63136126Entities db = new StoreThoiTrang63136126Entities();

        // GET: Admin/Product63136126
        [HttpGet]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            ViewBag.Category = db.DanhMucs.Select(d => d);
            ViewBag.Size = db.KichCoes.Select(c => c);
            ViewBag.searchString = searchString;
            var sanphams = db.SanPhams.Include("DanhMuc").Select(p => p);
            if (!String.IsNullOrEmpty(searchString))
            {
                sanphams = sanphams.Where(sp => sp.TenSP.Contains(searchString));
            }
            return View(sanphams.OrderBy(sp => sp.MaSP).ToPagedList(page, pageSize));
        }

        [HttpPost]
        public JsonResult Create(string sanpham, string chiTiets, HttpPostedFileBase hinhanh)
        {
            try
            {
                JavaScriptSerializer convert = new JavaScriptSerializer();
                SanPham sp = convert.Deserialize<SanPham>(sanpham);
                List<SanPhamChiTiet> sanPhamChiTiets = convert.Deserialize<List<SanPhamChiTiet>>(chiTiets);
                TaiKhoanQuanTri tk = (TaiKhoanQuanTri)Session[StoreThoiTrang63136126.Session.ConstaintUser.ADMIN_SESSION];
                var f = hinhanh;
                if (f != null && f.ContentLength > 0)
                {
                    string fileName = new Random().Next() + System.IO.Path.GetFileName(f.FileName);
                    string uploadPath = Server.MapPath("~/UserImage/images/" + fileName);
                    f.SaveAs(uploadPath);
                    sp.HinhAnh = "/UserImage/images/" + fileName;
                }
                else
                {
                    sp.HinhAnh = "/UserImage/images/noimage.jpg";
                }
                sp.NgayTao = DateTime.Now;
                sp.NguoiTao = tk.HoTen;
                sp.NgaySua = DateTime.Now;
                sp.NguoiSua = tk.HoTen;
                db.SanPhams.Add(sp);
                db.SaveChanges();
                int masp = sp.MaSP;
                foreach (SanPhamChiTiet spct in sanPhamChiTiets)
                {
                    spct.MaSP = masp;
                    db.SanPhamChiTiets.Add(spct);
                    db.SaveChanges();
                }
                return Json(new { status = true, message = "Thêm thành công!" });
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "Thêm không thành công!" });
            }
        }

        [HttpPost]
        public JsonResult Update(string sanpham, string chiTiets, HttpPostedFileBase hinhanh)
        {
            try
            {
                JavaScriptSerializer convert = new JavaScriptSerializer();
                SanPham sp = convert.Deserialize<SanPham>(sanpham);
                List<SanPhamChiTiet> sanPhamChiTiets = convert.Deserialize<List<SanPhamChiTiet>>(chiTiets);
                TaiKhoanQuanTri tk = (TaiKhoanQuanTri)Session[StoreThoiTrang63136126.Session.ConstaintUser.ADMIN_SESSION];
                SanPham update = db.SanPhams.Where(u => u.MaSP.Equals(sp.MaSP)).FirstOrDefault();
                var f = hinhanh;
                if (f != null && f.ContentLength > 0)
                {
                    string fileName = new Random().Next() + System.IO.Path.GetFileName(f.FileName);
                    string uploadPath = Server.MapPath("~/UserImage/images/" + fileName);
                    f.SaveAs(uploadPath);
                    update.HinhAnh = "/UserImage/images/" + fileName;
                }
                update.MaDM = sp.MaDM;
                update.TenSP = sp.TenSP;
                update.Gia = sp.Gia;
                update.MoTa = sp.MoTa;
                update.MaMau = sp.MaMau;
                update.ChatLieu = sp.ChatLieu;
                update.HuongDan = sp.HuongDan;
                update.NgaySua = DateTime.Now;
                update.NguoiSua = tk.HoTen;
                db.Entry(update).State = EntityState.Modified;
                db.SaveChanges();
                foreach (SanPhamChiTiet spct in sanPhamChiTiets)
                {
                    SanPhamChiTiet updatee = db.SanPhamChiTiets.Where(u => u.IDCTSP.Equals(spct.IDCTSP)).FirstOrDefault();
                    updatee.SoLuong = spct.SoLuong;
                    db.Entry(updatee).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json(new { status = true, message = "Sửa thông tin thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Sửa thông tin không thành công!" + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                SanPham sp = db.SanPhams.Where(a => a.MaSP.Equals(id)).FirstOrDefault();
                db.SanPhams.Remove(sp);
                db.SaveChanges();
                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { status = false });
            }
        }

        [HttpPost]
        public JsonResult Index(int id)
        {
            SanPham sp = db.SanPhams.Include("SanPhamChiTiets").Include("Danhmuc").Where(s => s.MaSP.Equals(id)).FirstOrDefault();
            return Json(sp, JsonRequestBehavior.AllowGet);
        }
    }
}
