﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjShoppingForum.Models.User
{
    public class CAdminData
    {
        public string ManagerId { get; set; }
        public string ManagerPassword { get; set; }

        public static readonly string S_CURRENT_LOGINED_ADMIN = "S_CURRENT_LOGINED_ADMIN";


    }
}