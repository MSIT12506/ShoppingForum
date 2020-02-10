using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tProductImageMetadata))]

    public partial class tProductImage
    {
        public class tProductImageMetadata
        {
            [DisplayName("商品編號")]
            public int fProductID { get; set; }

            [DisplayName("商品圖片編號")]
            public string fProductImageId { get; set; }

            [DisplayName("商品圖片路徑")]
            public string fProductImagePath { get; set; }


        }
    }
}