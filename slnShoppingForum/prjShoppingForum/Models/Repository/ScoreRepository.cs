using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Score.Models
{
    public class ScoreRepository
    {
        private dbShoppingForumEntities db;

        public ScoreRepository()
        {
            db = new dbShoppingForumEntities();
        }

        public IEnumerable<tScore> GetScoreAccount(string searchKey)
        {
            var ScoreList = db.tScores.Where(p => p.tUserProfile.fName.Contains(searchKey));
            return ScoreList;
        }

        public tScore GetScore(int fId)
        {
            var Score = db.tScores.FirstOrDefault(p => p.fId == fId);
            return Score;
        }

        public List<SelectListItem> GetScoreAccountSelectListItems()
        {
            var ScorefIdSelectListItems = db.tScores.Select(p => new SelectListItem()
            {
                Text = p.tUserProfile.fName,
                Value = p.fId.ToString()

            }).ToList();
            return ScorefIdSelectListItems;
        }


        public void UpdateScore(tScore tScore)
        {
            var tScoreFromDb = GetScore(tScore.fId);
            //fId
            tScoreFromDb.fScore = tScore.fScore;
            tScoreFromDb.fActiveScore = tScore.fActiveScore;
            tScoreFromDb.fQuestionScore = tScore.fQuestionScore;
            tScoreFromDb.fScoreDate = tScore.fScoreDate;
            db.SaveChanges();
        }

        public void RemoveScore(int fId)
        {
            var Score = GetScore(fId);
            db.tScores.Remove(Score);
            db.SaveChanges();
        }

        public void EditScoreToAuth(int fId)
        {
            var score = GetScore(fId);
            score.fAuthTestFlag = true;
            db.SaveChanges();
        }

        public void InsertScore(tScore tScore)
        {
            tScore.fAuthTestFlag = false;
            db.tScores.Add(tScore);
            db.SaveChanges();
        }

        //點閱 tag
        //public ActionResult Details(int productId)
        //{
        //    var product = _productRepository.GetProduct(productId);
        //    //_productRepository.AddPrice(productId, 1, TODO);

        //    return View(product);
        //}
        //public void AddPrice(int productId, int price, string account)
        //{
        //    var products = GetProduct(productId);

        //    if (!_northWindEntities.ProductLog.Any(p => p.Account == account && p.ProductId == productId))
        //    {
        //        var productLog = new ProductLog() { Account = account, ProductId = productId };
        //        products.UnitPrice = products.UnitPrice + price;
        //        _northWindEntities.ProductLog.Add(productLog);
        //        _northWindEntities.SaveChanges();
        //    }
        //}


    }
}