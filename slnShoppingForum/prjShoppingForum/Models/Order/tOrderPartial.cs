using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tOrderMetaData))]
    public partial class tOrder
    {
        public class tOrderMetaData
        {
            [DisplayName("訂單編號")]
            public long fOrderId { get; set; }
            [DisplayName("客戶編號")]
            public int fId { get; set; }
            [Display(Name = "訂單建立日期")]
            //[Range(DateTime.Now.Day, 999.99)]
            [DataType(DataType.Date, ErrorMessage = "請輸入正確的日期格式")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            [Required(ErrorMessage = "請輸入正確的日期格式")]
            public System.DateTime fOrderDate { get; set; }
            [DisplayName("訂單出貨日期")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public Nullable<System.DateTime> fShippedDate { get; set; }
            [DisplayName("希望送達日")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public Nullable<System.DateTime> fRequiredDate { get; set; }
            [DisplayName("使用積分")]
            public Nullable<int> fScore { get; set; }
            [DisplayName("使用優惠券")]
            public string fDiscountCode { get; set; }
            [DisplayName("收貨者姓名")]
            public string fConsigneeName { get; set; }
            [DisplayName("收貨者市話")]
            public string fConsigneeTelephone { get; set; }
            [DisplayName("收貨者手機號碼")]
            public string fConsigneeCellPhone { get; set; }
            [DisplayName("收貨者地址")]
            public string fConsigneeAddress { get; set; }
            [DisplayName("發票公司抬頭")]
            public string fOrderCompanyTitle { get; set; }
            [DisplayName("統一編號")]
            public Nullable<int> fOrderTaxIdDNumber { get; set; }
            [DisplayName("訂單備註")]
            public string fOrderPostScript { get; set; }
            [DisplayName("支付方式")]
            public string fPayment { get; set; }
        }
    }
}
