using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.ViewModels
{
    public class COrderView
    {
        public tUserProfile tUserProfiles { get; set; }
        public IQueryable<tOrder> Order { get; set; }
    }
}