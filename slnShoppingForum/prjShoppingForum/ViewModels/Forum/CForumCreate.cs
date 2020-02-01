using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Forum.ViewModels
{
    public class CForumCreate
    {
        public string postTitle { get; set; }

        [AllowHtml]
        public string tmpContent { get; set; }
    }
}