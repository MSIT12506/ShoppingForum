using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tPartMetadata))]
    public partial class tPart
    {
        public class tPartMetadata
        {
            [DisplayName("萃取部位編號")]
            public int fPartID { get; set; }

            [DisplayName("萃取部位")]
            public string fPartName { get; set; }

        }
    }
}