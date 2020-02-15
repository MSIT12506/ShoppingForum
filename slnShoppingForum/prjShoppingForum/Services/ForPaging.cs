using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.Services
{
    public class ForPaging
    {
        //當前頁數
        public int NowPage { get; set; }
        //最大頁數
        public int MaxPage { get; set; }
        //分頁項目個數為唯讀
        //以後修改頁數來此即可
        public int ItemNum
        {
            get
            {
                return 5;
            }
        }
        public ForPaging()
        {
            //預設頁數為1
            this.NowPage = 1;
        }
        //包含傳入頁數
        public ForPaging(int Page)
        {
            //設定頁數
            this.NowPage = Page;
        }
        //設定正確頁數的方法,避免傳入值不正確
        public void SetRightPage()
        {
            //判斷是否小於1
            if (this.NowPage < 1)
            {
                this.NowPage = 1;
            }
            else if (this.NowPage > this.MaxPage)
            {
                this.NowPage = this.MaxPage;
            }
            //無資料時重設回1
            if (this.MaxPage.Equals(0))
            {
                this.NowPage = 1;
            }
        }
    }
}