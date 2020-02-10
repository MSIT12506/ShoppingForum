using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tProductUnilateralMetadata))]
    public partial class tProductUnilateral
    {
        public class tProductUnilateralMetadata
        {
            [DisplayName("商品編號")]
            public int fProductID { get; set; }

            [DisplayName("萃取部位編號")]
            public Nullable<int> fPartID { get; set; }

            [DisplayName("香調編號")]
            public Nullable<int> fNoteID { get; set; }

            [DisplayName("產地")]
            public string fOrigin { get; set; }

            [DisplayName("萃取方式")]
            public string fextraction { get; set; }

        }

    }
}