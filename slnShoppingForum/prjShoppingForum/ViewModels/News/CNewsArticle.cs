using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using tw.com.essentialoil.Services;

namespace tw.com.essentialoil.ShoppingCart.ViewModels
{
    public class CNewsArticle
    {
        [DisplayName("搜尋")]
        public string Search { get; set; }
        //顯示資料陣列
        public List<tNew> DataList { get; set; }
        //顯示分頁內容
        public ForPaging Paging { get; set; }
        //文章列表帳號
        public string Account { get; set; }
    }
}