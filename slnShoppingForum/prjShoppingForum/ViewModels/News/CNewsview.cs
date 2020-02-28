using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.ShoppingCart.ViewModels
{
    public class CNewsview
    {
        //消息本體
        public tNew article { get; set; }
        //顯示留言
        public List<tNewsMessage> DataList { get; set; }

    }
}