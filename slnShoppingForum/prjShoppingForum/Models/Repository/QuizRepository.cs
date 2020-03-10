using prjShoppingForum.Models.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Questions.Models
{
    public class QuizRepository
    {
        private dbShoppingForumEntities db;

        public QuizRepository()
        {
            db = new dbShoppingForumEntities();
        }

        public IEnumerable<tQuestion> GetQuizName(string searchKey)
        {
            var QuizList = db.tQuestions.Where(p => p.fQuestionName.Contains(searchKey)).OrderBy(p => p.fQuestionId);
            return QuizList;
        }

        public tQuestion GetQuiz(int fQuestionId)
        {
            var Quiz = db.tQuestions.FirstOrDefault(p => p.fQuestionId == fQuestionId);
            return Quiz;
        }

        //不知正不正確?
        public List<SelectListItem> Items(string fAnswer)
        {
            var AnswerSelectItem = db.tQuestions.Select(p => new SelectListItem()
            {
                Text = "A" + "B" + "C" + "D" + "E",
                Value = "A" + "B" + "C" + "D" + "E",
            }).ToList();
            return AnswerSelectItem;
        }

        public List<SelectListItem> GetQuizSelectListItems()
        {
            var QuizSelectListItems = db.tQuestions.Select(p => new SelectListItem()
            {
                Text = p.fQuestionName,
                Value = p.fQuestionId.ToString(),
            }).ToList();
            return QuizSelectListItems;
        }


        public void UpdateQuiz(tQuestion tQuestion)
        {
            var tQuestionFromDb = GetQuiz(tQuestion.fQuestionId);
            //         QuestionId,fQuestionName,fQuestion,fAnswer,fItemA,fItemB,fItemC,fItemD,fItemE

            tQuestionFromDb.fQuestionName = tQuestion.fQuestionName;
            tQuestionFromDb.fQuestion = tQuestion.fQuestion;
            tQuestionFromDb.fAnswer = tQuestion.fAnswer;
            tQuestionFromDb.fItemA = tQuestion.fItemA;
            tQuestionFromDb.fItemB = tQuestion.fItemB;
            tQuestionFromDb.fItemC = tQuestion.fItemC;
            tQuestionFromDb.fItemD = tQuestion.fItemD;
            tQuestionFromDb.fItemE = tQuestion.fItemE;

            db.SaveChanges();
        }

        public void RemoveQuiz(int fQuestionId)
        {
            var Quiz = GetQuiz(fQuestionId);
            db.tQuestions.Remove(Quiz);
            db.SaveChanges();
        }

        public void EditQuiz(int fQuestionId)
        {
            var Quiz = GetQuiz(fQuestionId);
            db.SaveChanges();
        }

        public void InsertNews(tQuestion tQuestion)
        {
            db.tQuestions.Add(tQuestion);
            db.SaveChanges();
        }

        public void GetQuiz(object sender, EventArgs e)
        {
            ArrayList al = new ArrayList();

            if (al != null)
            {
                var quiz = from q in db.tQuestions.AsEnumerable()
                           select new
                           {
                               q.fQuestionName,
                               q.fQuestion,
                               q.fItemA,
                               q.fItemB,
                               q.fItemC,
                               q.fItemD,
                               q.fItemE
                           };
                var getquiz = quiz.ToList();
                for (int i = 0; i < getquiz.Count(); i++)
                {
                    al.Add(getquiz[i]);
                }
                var rnd = new Random();
                var index = rnd.Next(0, al.Count);
                al.Remove(index);

            }

        }

        //public void button1_Click(object sender, EventArgs e)
        //{
        //    ArrayList al = new ArrayList();

        //    if (al != null)//如果集合不是空值從頭跑
        //    {
        //        var q = from g in this.haDB.Gifts.AsEnumerable()
        //                select new { 抽到 = g.GiftName, 獎項 = g.Descgift };
        //        var q1 = q.ToList();
        //        for (int i = 0; i < q1.Count(); i++)
        //        {
        //            al.Add(q1[i]);
        //        }
        //        var rnd = new Random();
        //        var index = rnd.Next(0, al.Count);
        //        MessageBox.Show(al[index].ToString());
        //        al.Remove(index);
        //    }
        //    else
        //    {
        //    }
        //    //var list = new List<T>();
        //    //fillList(list);
        //    //var randomizedList = new List<T>();
        //    //var rnd = new Random();
        //    //while (list.Count != 0)
        //    //{
        //    //    var index = rnd.Next(0, list.Count);
        //    //    randomizedList.Add(list[index]);
        //    //    list.RemoveAt(index);
        //    //}
        //    //var list = new List<T>();
        //    //var randomizedList = new List<T>();
        //}
    }
}