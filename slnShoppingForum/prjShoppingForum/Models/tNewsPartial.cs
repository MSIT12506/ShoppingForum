using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tNewsMetaData))]
    public partial class tNew
    {
        public class tNewsMetaData
        {
            [DisplayName("消息編號")]
            public int fNewsId { get; set; }
            //思考是否要結合活動
            [DisplayName("開始時間")]
            public System.DateTime fNewsStart { get; set; }
            [DisplayName("結束時間")]
            public System.DateTime fNewsEnd { get; set; }
            [DisplayName("分類")]
            public string fClass { get; set; }
            [DisplayName("標題")]
            [Required(ErrorMessage = "請輸入標題")]
            [StringLength(50, ErrorMessage = "標題長度最多為50字元")]
            public string fNewsTitle { get; set; }
            [DisplayName("概要")]
            public string fNewsDesc { get; set; }
            [DisplayName("內文")]
            [Required(ErrorMessage = "請輸入內容")]
            [AllowHtml]
            public string fNewsArticle { get; set; }
            [DisplayName("點閱率")]
            public Nullable<int> fNewsTag { get; set; }
            [DisplayName("評分")]
            public Nullable<int> fGet_No { get; set; }
            [DisplayName("公告者")]
            public string fAddUser { get; set; }
            [DisplayName("編輯者")]
            public string fChangUser { get; set; }
            [DisplayName("刪除者")]
            public string fDeleteUser { get; set; }
            [DisplayName("置頂Y/N")]
            public string fApproved { get; set; }
        }
    }
}
