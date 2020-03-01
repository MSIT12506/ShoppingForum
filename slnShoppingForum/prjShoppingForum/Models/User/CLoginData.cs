using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.User.Models
{
    public class CLoginData
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "請輸入Email作為後續您登入的帳號")]
        [StringLength(200, ErrorMessage = "Email長度最多200字元")]
        [EmailAddress(ErrorMessage = "這不是Email格式")]
        public string fUserId { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string fPassword { get; set; }
        public string fVeriCode { get; set; }
    }
}