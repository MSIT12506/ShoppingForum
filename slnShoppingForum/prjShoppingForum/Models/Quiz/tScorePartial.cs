using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjShoppingForum.Models.Entity
{
        [MetadataType(typeof(tScoreMetaData))]
        public partial class tScore
        {
            public class tScoreMetaData
            {

            [DisplayName("帳號")]
            public int fId { get; set; }
            [DisplayName("會員總積分")]
            public Nullable<int> fScore { get; set; }
            [DisplayName("活動積分")]
            public Nullable<int> fActiveScore { get; set; }
            [DisplayName("任務積分")]
            public Nullable<int> fQuestionScore { get; set; }
            [DisplayName("積分變化時間")]
            public Nullable<System.DateTime> fScoreDate { get; set; }
            [DisplayName("積分不顯示")]
            public Nullable<bool> fAuthTestFlag { get; set; }

            public virtual tUserProfile tUserProfile { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<tTest> tTests { get; set; }

        }
    }
}