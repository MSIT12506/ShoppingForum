using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tTestMetaData))]
    public partial class tTest
    {
        public class tTestMetaData
        {
            [DisplayName("測驗編號")]
            public int fTestId { get; set; }
            [DisplayName("測試者")]
            public string fId { get; set; }
            [DisplayName("題目編號")]
            public int fQuestionId { get; set; }
            [DisplayName("開始時間")]
            public Nullable<System.DateTime> fTestStar { get; set; }
            [DisplayName("結束時間")]
            public Nullable<System.DateTime> fTestEnd { get; set; }
            [DisplayName("花費時間")]
            public Nullable<System.DateTime> fTestCost { get; set; }
            [DisplayName("作答數")]
            public Nullable<int> fQuestionCount { get; set; }
            [DisplayName("正確數")]
            public Nullable<int> fCorrectCount { get; set; }
            [DisplayName("積分日期")]
            public Nullable<System.DateTime> fScoreDate { get; set; }
            [DisplayName("測驗積分")]
            public Nullable<int> fQuestionScore { get; set; }

            [DisplayName("題目表")]
            public virtual tQuestion tQuestion { get; set; }
            [DisplayName("帳戶積分表")]
            public virtual tScore tScore { get; set; }

        }
    }
}