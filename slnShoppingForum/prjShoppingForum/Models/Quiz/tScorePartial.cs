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
            [DisplayName("分數異動編號")]
            public int fScoreId { get; set; }
            [DisplayName("帳號")]
            public int fId { get; set; }
            [DisplayName("會員總積分")]
            public int fScore { get; set; }
            [DisplayName("活動積分")]
            public int fActiveScore { get; set; }
            [DisplayName("任務積分")]
            public int fQuestionScore { get; set; }
            [DisplayName("積分變化時間")]
            public System.DateTime fScoreDate { get; set; }
            [DisplayName("積分不顯示")]
            public bool fScoreDiscontinue { get; set; }

            public virtual tUserProfile tUserProfile { get; set; }

        }
    }
}