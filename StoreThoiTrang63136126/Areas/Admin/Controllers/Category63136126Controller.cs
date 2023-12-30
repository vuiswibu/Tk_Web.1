using System;
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
    public class Category63136126Controller : Base63136126Controller
    {
        private StoreThoiTrang63136126Entities db = new StoreThoiTrang63136126Entities();

        // GET: Admin/Category63136126
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            ViewBag.searchString = searchString;
            var danhmucs = db.DanhMucs.Select(dm => dm);
            if (!String.IsNullOrEmpty(searchString))
            {
                danhmucs = danhmucs.Where(dm => dm.TenDanhMuc.Contains(searchString));
            }
            return View(danhmucs.OrderBy(dm => dm.MaDM).ToPagedList(page, pageSize));
        }

        [HttpPost]
        public JsonResult Index(int id)
        {
            DanhMuc dm = db.DanhMucs.Where(a => a.MaDM.Equals(id)).FirstOrDefault();
            return Json(dm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(DanhMuc dm)
        {
            TaiKhoanQuanTri tk = (TaiKhoanQuanTri)Session[StoreThoiTrang63136126.Session.ConstaintUser.ADMIN_SESSION];
            try
            {
                dm.NgayTao = DateTime.Now;
                dm.NguoiTao = tk.HoTen;
                dm.NgaySua = DateTime.Now;
                dm.NguoiSua = tk.HoTen;
                db.DanhMucs.Add(dm);
                db.SaveChanges();
                return Json(new { status = true, message = "Thêm thành công" });
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "Tên danh mục đã tồn tại" });
            }
        }

        [HttpPost]
        public JsonResult Update(DanhMuc dm)
        {
            TaiKhoanQuanTri tk = (TaiKhoanQuanTri)Session[StoreThoiTrang63136126.Session.ConstaintUser.ADMIN_SESSION];
            try
            {
                DanhMuc update = db.DanhMucs.Where(a => a.MaDM.Equals(dm.MaDM)).FirstOrDefault();
                update.TenDanhMuc = dm.TenDanhMuc;
                update.NgaySua = DateTime.Now;
                update.NguoiSua = tk.HoTen;
                db.Entry(update).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { status = true, message = "Sửa thông tin thành công" });
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "Tên danh mục đã tồn tại" });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                DanhMuc dm = db.DanhMucs.Where(a => a.MaDM.Equals(id)).FirstOrDefault();
                db.DanhMucs.Remove(dm);
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
