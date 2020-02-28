using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Tests.Models
{
    public class TestRepository
    {

        private dbShoppingForumEntities db;

        public TestRepository()
        {
            db = new dbShoppingForumEntities();
        }

        public IEnumerable<tTest> GetTestAccount(string searchKey)
        {
            var TestList = db.tTests.Where(p => p.fId.ToString().Contains(searchKey));
            return TestList;
        }

        public tTest GetTest(int fTestId)
        {
            var TestList = db.tTests.FirstOrDefault(p => p.fTestId == fTestId);
            return TestList;
        }

        public List<SelectListItem> GetSelectListItems()
        {
            var TestQuizSelectListItems = db.tTests.Select(p => new SelectListItem()
            {
                Text = p.tQuestion.fQuestionName,
                Value = p.fQuestionId.ToString()

            }).ToList();
            return TestQuizSelectListItems;
        }


        public void UpdateTests(tTest tTest)
        {
            var tTestFromDb = GetTest(tTest.fTestId);
            //   fTestId,fId,fTestEnd,fTestCost,fQuestionCount,fCorrectCount
            tTestFromDb.fScoreDate = tTest.fScoreDate;
            tTestFromDb.fQuestionId = tTest.fQuestionId;
            tTestFromDb.fQuestionScore = tTest.fQuestionScore;

            db.SaveChanges();
        }

        public void RemoveTest(int fTestId)
        {
            var Test = GetTest(fTestId);
            db.tTests.Remove(Test);
            db.SaveChanges();
        }

        public void EditTest(int fTestId)
        {
            var Test = GetTest(fTestId);
            
            db.SaveChanges();
        }

        public void InsertTest(tTest tTest)
        {
            tTest.fScoreDate = DateTime.Now;
            db.tTests.Add(tTest);
            db.SaveChanges();
        }


    }
}