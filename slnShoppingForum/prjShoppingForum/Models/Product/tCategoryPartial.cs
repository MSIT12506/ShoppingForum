using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tCategoryMetadata))]
    public partial class tCategory
    {
        public class tCategoryMetadata
        {
          
            [DisplayName("商品分類編號")]
            public int fCategoryID { get; set; }

            [DisplayName("商品分類")]
            public string fCategoryName { get; set; }
        }

    }
}