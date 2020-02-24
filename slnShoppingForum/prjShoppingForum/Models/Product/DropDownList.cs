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
            var list= db.tCategories.Select(p => new SelectListItem
            {
                Text = p.fCategoryName,
                Value = p.fCategoryID.ToString()
            }).ToList();

            list.Add(new SelectListItem() {
                Text = "無",
                Value = null
            });

            return list;
        }

        //下拉選單--單方精油萃取部位
        public IEnumerable<SelectListItem> GetPartDropDownList()
        {
            var list= db.tParts.Select(p => new SelectListItem
            {
                Value = p.fPartID.ToString(),
                Text = p.fPartName
            }).ToList();

            list.Add(new SelectListItem()
            {
                Text = "無",
                Value = null
            });

            return list;
        }

        //下拉選單--單方精油香調
        public IEnumerable<SelectListItem> GetNoteDropList()
        {
            var list = db.tNotes.Select(p => new SelectListItem
            {
                Value = p.fNoteID.ToString(),
                Text = p.fNoteName,
            }).ToList();

            list.Add(new SelectListItem()
            {
                Text = "無",
                Value = null
            });

            return list;
        }

        //下拉選單--單方&副方精油功效
        public IEnumerable<SelectListItem> GetEfficacyDropLise()
        {
            var list= db.tEfficacies.Select(p => new SelectListItem
            {
                Value = p.fEfficacyID.ToString(),
                Text=p.fEfficacyName
            }).ToList();

            list.Add(new SelectListItem()
            {
                Text = "無",
                Value = null
            });

            return list;

        }

        //下拉選單--純露/植物油特性
        public IEnumerable<SelectListItem> GetfeatureDropList()
        {
            var list = db.tfeatures.Select(p => new SelectListItem
            {
                Value = p.ffeatureID.ToString(),
                Text = p.ffeatureName
            }).ToList();

            list.Add(new SelectListItem()
            {
                Text = "無",
                Value = null
            });

            return list;
        }

        //下拉選單--商品功效
        public IEnumerable<SelectListItem> GetEfficacyDropList()
        {
            var list = db.tEfficacies.Select(p => new SelectListItem
            {
                Value = p.fEfficacyID.ToString(),
                Text = p.fEfficacyName
            }).ToList();

            list.Add(new SelectListItem()
            {
                Text = "無",
                Value = null
            });

            return list;
        }



    }
}