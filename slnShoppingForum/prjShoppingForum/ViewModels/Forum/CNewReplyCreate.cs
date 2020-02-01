using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Forum.ViewModels
{
    public class CNewReplyCreate
    {
        public int tmpPostId { get; set; }
        public string tmpReplyType { get; set; }
        public string tmpTargetId { get; set; }
        
        [AllowHtml]
        public string tmpContent { get; set; }
    }
}