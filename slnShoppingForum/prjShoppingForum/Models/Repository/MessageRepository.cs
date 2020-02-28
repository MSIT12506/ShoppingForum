using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Message.Models
{
    public class MessageRepository
    {
        private dbShoppingForumEntities db;

        public MessageRepository()
        {
            db = new dbShoppingForumEntities();
        }

        public IEnumerable<tNewsMessage> GetAccountMessage(string searchKey)
        {
            var tMessageList = db.tNewsMessages.Where(p => p.fM_AddUser.Contains(searchKey));
            return tMessageList;
        }

        public tNewsMessage GetMessage(int fMessageId)
        {
            var MessageList = db.tNewsMessages.FirstOrDefault(p => p.fMessageId == fMessageId);
            return MessageList;
        }

        public List<SelectListItem> GetNewsSelectListItems()
        {
            var NewsSelectListItems = db.tNewsMessages.Select(p => new SelectListItem()
            {
                Text = p.tNew.fNewsTitle,
                Value = p.fNewsId.ToString()

            }).ToList();
            return NewsSelectListItems;
        }


        public void UpdateMessage(tNewsMessage tNewsMessage)
        {
            var tMessageFromDb = GetMessage(tNewsMessage.fMessageId);
            //    fMessageId,fNewsId,fMessageTime,fM_AddUser
            tMessageFromDb.fMessageArticle = tNewsMessage.fMessageArticle;

            db.SaveChanges();
        }

        public void RemoveMessage(int fMessageId)
        {
            var Message = GetMessage(fMessageId);
            db.tNewsMessages.Remove(Message);
            db.SaveChanges();
        }

        public void EditMessage(int fMessageId)
        {
            var Message = GetMessage(fMessageId);
            
            db.SaveChanges();
        }

        public void InsertMessage(tNewsMessage tNewsMessage)
        {
            db.tNewsMessages.Add(tNewsMessage);
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