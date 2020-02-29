using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.Quiz.ViewModels
{
    public class CQuiz
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

        [DisplayName("測驗編號")]
        public int fTestId { get; set; }
        [DisplayName("測試者")]
        public string fId { get; set; }
        [DisplayName("積分日期")]
        public System.DateTime fScoreDate { get; set; }
        [DisplayName("測驗積分")]
        public Nullable<int> fQuestionScore { get; set; }
        [DisplayName("測驗不顯示")]
        public bool fTestDiscontinue { get; set; }


        [DisplayName("分數異動編號")]
        public int fScoreId { get; set; }
        [DisplayName("會員總積分")]
        public int fScore { get; set; }
        [DisplayName("活動積分")]
        public int fActiveScore { get; set; }
        [DisplayName("積分不顯示")]
        public bool fScoreDiscontinue { get; set; }




    }
}