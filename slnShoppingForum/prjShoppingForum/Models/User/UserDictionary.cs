using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.User.Models
{
    public class UserDictionary
    {
        public static readonly string S_CURRENT_LOGINED_USER = "S_CURRENT_LOGINED_USER";
        public static readonly string S_AUTHENTICATED_CODE = "S_AUTHENTICATED_CODE";
        public static readonly string S_CURRENT_LOGINED_USERFID = "S_CURRENT_LOGINED_USERFID";
        public static readonly string S_CURRENT_LOGINED_USERSHOPCART = "S_CURRENT_LOGINED_USERSHOPCART";
    }

    public class UserLoginInfo
    {
        public int user_fid { get; set; }
        public string user_userid { get; set; }
        public string user_name { get; set; }
    }
}