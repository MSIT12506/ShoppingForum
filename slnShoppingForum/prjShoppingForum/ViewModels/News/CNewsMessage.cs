using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tw.com.essentialoil.Services;

namespace tw.com.essentialoil.ShoppingCart.ViewModels
{
    public class CNewsMessage
    {
        //顯示資料陣列
        public List<tNewsMessage> DataList { get; set; }
        //分頁內容
        public ForPaging Paging { get; set;}
        //消息編號
        public int fNewsId { get; set; }
    }
}