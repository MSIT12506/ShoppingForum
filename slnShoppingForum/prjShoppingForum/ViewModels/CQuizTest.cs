using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.ShoppingCart.ViewModels
{
    public class CQuizTest
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
        public string fUserId { get; set; }
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


    }
}