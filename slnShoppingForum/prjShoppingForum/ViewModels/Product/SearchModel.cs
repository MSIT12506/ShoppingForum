using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.Product.ViewModels
{
    public class SearchModel
    {
        public string searchprod { get; set; }
        public int? categoryId { get; set; }
        public int? efficacyId { get; set; }
        public int? noteId { get; set; }
        public int? partId { get; set; }
        public int? featureId { get; set; }
        public bool? fDiscontinued { get; set; }
    }
}