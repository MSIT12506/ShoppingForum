using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tQuestionMetaData))]
    public partial class tQuestion
    {
        public class tQuestionMetaData
        {
            [DisplayName("編號")]
            public int fQuestionId { get; set; }
            [DisplayName("題目")]
            public string fQuestionName { get; set; }
            [DisplayName("問題內容")]
            public string fQuestion { get; set; }
            [DisplayName("正解")]
            public string fAnswer { get; set; }
            [DisplayName("A選項")]
            public string fItemA { get; set; }
            [DisplayName("B選項")]
            public string fItemB { get; set; }
            [DisplayName("C選項")]
            public string fItemC { get; set; }
            [DisplayName("D選項")]
            public string fItemD { get; set; }
            [DisplayName("E選項")]
            public string fItemE { get; set; }

        }
    }
}