using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.Forum.ViewModels
{
    public class CForumList
    {
        public int postId { get; set; }
        public int userFid { get; set; }
        public string postTitle { get; set; }
        public string likeOrHate { get; set; }
    }
}