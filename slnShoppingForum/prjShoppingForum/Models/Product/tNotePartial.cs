using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tNoteMetadata))]
    public partial class tNote
    {
        public class tNoteMetadata
        {
            [DisplayName("香調編號")]
            public int fNoteID { get; set; }

            [DisplayName("香調")]
            public string fNoteName { get; set; }
        }
    }
}