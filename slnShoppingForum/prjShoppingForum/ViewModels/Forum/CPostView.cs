using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.Forum.ViewModels
{
    public class CPostView
    {
        public tForum forum { get; set; }
        public List<List<tForumReply>> reply { get; set; }
    }
}