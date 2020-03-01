using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.Discount.ViewModels
{
    public class CDiscountCreate
    {
        public string discountName { get; set; }
        public string discountCategory { get; set; }
        public string moneyLimit { get; set; }
        public string discountContent { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string count { get; set; }
    }
}