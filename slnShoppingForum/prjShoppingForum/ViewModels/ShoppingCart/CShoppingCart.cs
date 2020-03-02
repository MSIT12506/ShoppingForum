﻿using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.ShoppingCart.ViewModels
{
    public class CShoppingCart
    {
        public IQueryable<tShoppingCart> ShoppingCart { get; set; }
        public IQueryable<tProduct> Product { get; set; }
        public IQueryable<tUserBrowseHistory> BrowseHistory { get; set; }
        public IQueryable<tUserProfile> UserProfiles { get; set; }
        public IQueryable<tUserDiscountList> UserDiscountLists { get; set; }
    }
}