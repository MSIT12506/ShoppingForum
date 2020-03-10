using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjShoppingForum.Models.Entity
{
    public class DropDownList
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        //下拉選單--商品分類
        public IEnumerable<SelectListItem> GetCategoryDropList()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                  Text = "請選擇",
                  Value = null
                }
            };

            list.AddRange(db.tCategories.Select(p => new SelectListItem
            {
                Text = p.fCategoryName,
                Value = p.fCategoryID.ToString()
            }).ToList());

            return list;
        }

        //下拉選單--單方精油萃取部位
        public IEnumerable<SelectListItem> GetPartDropDownList()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                  Text = "無",
                  Value = null
                }
            };

            list.AddRange(db.tParts.Select(p => new SelectListItem
            {
                Value = p.fPartID.ToString(),
                Text = p.fPartName
            }).ToList());

            return list;
        }

        //下拉選單--單方精油香調
        public IEnumerable<SelectListItem> GetNoteDropList()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="無",
                    Value=null
                }
            };

            list.AddRange(db.tNotes.Select(p => new SelectListItem
            {
                Value = p.fNoteID.ToString(),
                Text = p.fNoteName,
            }).ToList());

            return list;
        }

        //下拉選單--單方&副方精油功效
        public IEnumerable<SelectListItem> GetEfficacyDropLise()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="無",
                    Value=null
                }
            };

            list.AddRange(db.tEfficacies.Select(p => new SelectListItem
            {
                Value = p.fEfficacyID.ToString(),
                Text = p.fEfficacyName
            }).ToList());

            return list;
        }

        //下拉選單--純露/植物油特性
        public IEnumerable<SelectListItem> GetfeatureDropList()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="無",
                    Value=null
                }
            };

            list.AddRange(db.tfeatures.Select(p => new SelectListItem
            {
                Value = p.ffeatureID.ToString(),
                Text = p.ffeatureName
            }).ToList());

            return list;
        }

        //下拉選單--商品功效
        public IEnumerable<SelectListItem> GetEfficacyDropList()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="無",
                    Value=null
                }
            };

            list.AddRange(db.tEfficacies.Select(p => new SelectListItem
            {
                Value = p.fEfficacyID.ToString(),
                Text = p.fEfficacyName
            }).ToList());

            return list;
        }
    }
}