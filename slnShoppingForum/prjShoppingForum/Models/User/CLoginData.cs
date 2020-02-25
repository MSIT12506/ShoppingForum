using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.User.Models
{
    public class CLoginData
    {
        [Required]
        public string fUserId { get; set; }
        [Required]
        public string fPassword { get; set; }
        public string fVeriCode { get; set; }
    }
}