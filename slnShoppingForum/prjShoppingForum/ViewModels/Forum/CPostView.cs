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
        public List<List<CPostReplyInfo>> reply { get; set; }
    }

    public class CPostReplyInfo
    {
        public string replyId { get; set; }
        public int replySeqNo { get; set; }
        public string replyContent { get; set; }
        public string userName { get; set; }
        public DateTime replyTime { get; set; }
        public string isLikeOrHate { get; set; }
    }
}