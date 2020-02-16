using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Models
{
    public class CDictionary
    {
        public static readonly string UserLoginInfo = "UserLoginInfo";
        public static readonly string LoginPageInfo = "LoginPageInfo";

        //-------------------------------------------------------------------------------
        public static readonly string S_CURRENT_LOGINED_USER = "S_CURRENT_LOGINED_USER";
        public static readonly string S_AUTHENTICATED_CODE = "S_AUTHENTICATED_CODE";
    }

    public class LoginPageInfo
    {
        public string controllerName { get; set; }
        public string actionName { get; set; }
    }

    public class CStaticMethod
    {
        public static bool isLogin(HttpSessionStateBase Session, string controllerName, string actionName)
        {
            if (Session[CDictionary.UserLoginInfo] == null)
            {
                Session[CDictionary.LoginPageInfo] = new LoginPageInfo() { controllerName = controllerName, actionName = actionName };
                return false;
            }

            return true;
        }
    }
    
}