using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using prjShoppingForum.Models.Entity;
using tw.com.essentialoil.News.Models;

namespace tw.com.essentialoil.Controllers
{
    public class NewsController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        private NewsRepository _NewsRepository;

        public NewsController()
        {
            _NewsRepository = new NewsRepository();
        }

        int pagesize = 6;

        //前台消息清單
        public ActionResult NewsList(int page =1)
        {
            int currentPage = pagesize < 1 ? 1 : page;
            //var Newspage = db.tNews.Where(p => p.fNewsDiscontinue != true).OrderBy(p => p.fNewsId).ToList();
            var News = from a in db.tNews
                      where a.fNewsDiscontinue != true
                      orderby a.fNewsId descending
                      select a;
            var Newspage = News.ToList();
            var result = Newspage.ToPagedList(currentPage, pagesize);
            return View(result);
        }

        //前台消息搜索
        [HttpPost]
        public ActionResult NewsList(string searchKey)
        {
            ViewBag.Message = searchKey;
            IPagedList<tNew> NewsList = _NewsRepository.GetNewstitle(searchKey).ToPagedList(1,6);
            return View("NewsList",NewsList);
        }

        //前台文章內容
        public ActionResult Content(int Id)
        {
            ViewBag.Message = "starnum" ;
            var News = db.tNews.FirstOrDefault(p => p.fNewsId == Id);
            News.fNewsTag++;
            db.SaveChanges();
            return View (News);
        }

        // 後臺消息清單
        public ActionResult Index()
        {
            return View(db.tNews.ToList());
        }

        // 後台消息明細
        public ActionResult Details(int Id)
        {
            var News = db.tNews.FirstOrDefault(p => p.fNewsId == Id);
            return View(News);
        }

        // 後臺新增消息
        public ActionResult Create()
        {
            ViewBag.fAddUser = new SelectList(db.tAdminManagers, "ManagerEmail", "ManagerId");
            return View();
        }

        //尚須思考
        [HttpPost]
        public ActionResult Create(tNew tNew)
        {

            if (ModelState.IsValid)
            {
                _NewsRepository.InsertNews(tNew);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tNew);
        }

        // 後臺編輯消息
        public ActionResult Edit(int Id)
        {
            ViewBag.fChangUser = new SelectList(db.tAdminManagers, "ManagerEmail", "ManagerId");
            var News = _NewsRepository.GetNews(Id);
            return View(News);

        }

        //後臺點擊編輯
        [HttpPost]
        public ActionResult Edit(tNew tNew)
        {
            if (ModelState.IsValid)
            {
                _NewsRepository.UpdateNews(tNew);
                return RedirectToAction("Details", new { Id = tNew.fNewsId });
            }
            return View(tNew);
        }

        // 刪除使用不顯示方式
        public ActionResult Delete(int Id)
        {
            _NewsRepository.EditNewsToDiscontinue(Id);
            return RedirectToAction("Index");
        }


        public ActionResult Star(int starnum , int fNewsId) 
        {
            var record = db.tNews.Where(p => p.fNewsId == fNewsId).FirstOrDefault();
            int oldnum, newnum;
            
            if (record!=null)
            {
                oldnum = (record.fGet_No == null) ? 0 : Convert.ToInt32( record.fGet_No);
                newnum = (starnum + oldnum) / 2;
                record.fGet_No = newnum;
                db.SaveChanges();
                return Content(newnum.ToString());
            }

            //todo
            //News.fGet_No = (News.fGet_No.Value + @News.fGet_No.Value) / 2;
            return Content("");
        }
    }
}
