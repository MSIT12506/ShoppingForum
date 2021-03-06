﻿using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.ViewModels
{
    public class COrderViews
    {
        public IQueryable<tOrder> Order { get; set; }
        public IQueryable<tOrderDetail> OrderDetail { get; set; }
        public IQueryable<tProduct> Product { get; set; }
        public IQueryable<tUserProfile> UserProfile { get; set; }
    }
}