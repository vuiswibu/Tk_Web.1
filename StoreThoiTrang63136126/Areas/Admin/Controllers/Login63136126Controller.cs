using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreThoiTrang63136126.Areas.Admin.Data;
using StoreThoiTrang63136126.Models;
using StoreThoiTrang63136126.Session;

namespace StoreThoiTrang63136126.Areas.Admin.Controllers
{
    public class Login63136126Controller : Controller
    {
        private StoreThoiTrang63136126Entities db = new StoreThoiTrang63136126Entities();

        // GET: Admin/Login63136126
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginAccount loginAccount)
        {
            if (ModelState.IsValid)
            {
                TaiKhoanQuanTri tk = db.TaiKhoanQuanTris.Where(a => a.TenDangNhap.Equals(loginAccount.username)
                && a.MatKhau.Equals(loginAccount.password)).SingleOrDefault();
                if (tk != null)
                {
                    if (tk.TrangThai == false)
                    {
                        ModelState.AddModelError("ErrorLogin", "Tài khoản đã bị vô hiệu hóa! Liên hệ quản trị viên!");
                    }
                    else
                    {
                        Session.Add(ConstaintUser.ADMIN_SESSION, tk);
                        return RedirectToAction("Index", "Home63136126");
                    }
                }
                else
                {
                    ModelState.AddModelError("ErrorLogin", "Tài khoản hoặc mật khẩu không đúng!");
                }
            }
            return View(loginAccount);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove(ConstaintUser.ADMIN_SESSION);
            return RedirectToAction("Index");
        }
    }
}
