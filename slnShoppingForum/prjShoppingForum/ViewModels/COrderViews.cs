using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.ViewModels
{
    public class COrderViews
    {
        public decimal DiscountContent { get; set; }
        public int DiscountPrice { get; set; }
        public tUserProfile tUserProfiles { get; set; }
        public tProduct tProducts { get; set; }
        public tShoppingCart tShoppingCarts { get; set; }
        public IQueryable<tShoppingCart> ShoppingCart { get; set; }
        public IQueryable<tOrder> Order { get; set; }
        public IQueryable<tOrderDetail> OrderDetail { get; set; }
        public IQueryable<tProduct> Product { get; set; }
        public IQueryable<tUserProfile> UserProfile { get; set; }
        public IQueryable<tUserDiscountList> UserDiscount { get; set; }
        public IQueryable<tDiscount> Discount { get; set; }
    }
}