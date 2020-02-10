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
        public IQueryable<SelectListItem> GetCategoryDropList()
        {
            return db.tCategories.Select(p => new SelectListItem
            {
                Text = p.fCategoryName,
                Value = p.fCategoryID.ToString()
            });
        }

        //下拉選單--單方精油萃取部位
        public IQueryable<SelectListItem> GetPartDropDownList()
        {
            return db.tParts.Select(p => new SelectListItem
            {
                Value = p.fPartID.ToString(),
                Text = p.fPartName
            });
        }

        //下拉選單--單方精油香調
        public IQueryable<SelectListItem> GetNoteDropList()
        {
            return db.tNotes.Select(p => new SelectListItem
            {
                Value = p.fNoteID.ToString(),
                Text = p.fNoteName,
            });
        }

        //下拉選單--單方&副方精油功效
        public IQueryable<SelectListItem> GetEfficacyDropLise()
        {
            return db.tEfficacies.Select(p => new SelectListItem
            {
                Value = p.fEfficacyID.ToString(),
                Text=p.fEfficacyName
            });
        }

        //下拉選單--純露/植物油特性
        public IQueryable<SelectListItem> GetfeatureDropList()
        {
            return db.tfeatures.Select(p => new SelectListItem
            {
                Value = p.ffeatureID.ToString(),
                Text = p.ffeatureName
            });
        }

    }
}