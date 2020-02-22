using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.ProductFavorite.ViewModels
{
    public class CProductFavorite
    {
        public IQueryable<tUserProductFavorite> ProductFavorite { get; set; }
        public IQueryable<tProduct> Product { get; set; }
    }
}