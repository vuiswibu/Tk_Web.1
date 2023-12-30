﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreThoiTrang63136126.Areas.Admin.Data
{
    public class LoginAccount
    {
        [Required(ErrorMessage = "Vui lòng nhập Tên đăng nhập")]
        public string username { set; get; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string password { set; get; }
        public bool rememberme { set; get; }
    }
}