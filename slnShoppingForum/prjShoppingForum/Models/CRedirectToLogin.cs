using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.Models
{
    public class CRedirectToLogin
    {
        public CRedirectToLogin()
        {
            controller = "FrontUserProfile";
            action = "Login";
        }
        public string controller { get; }
        public string action { get; }
    }
}