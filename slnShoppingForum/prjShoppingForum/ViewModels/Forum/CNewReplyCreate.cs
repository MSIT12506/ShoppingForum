using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Forum.ViewModels
{
    public class CNewReplyCreate
    {
        public int postId { get; set; }
        public string targetType { get; set; }
        public string targetId { get; set; }
        public string content { get; set; }
    }
}