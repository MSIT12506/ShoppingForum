using PagedList;
using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.Product.Models
{
    public class ProductRepository
    {
        object lockObject = new object();

        dbShoppingForumEntities db = new dbShoppingForumEntities();
        int pagesize = 10;

        //商品換頁方法
        public IPagedList<tProduct> ProdPageList(int page)
        {
            int currentPage = page < 1 ? 1 : page;

            var products = db.tProducts.ToList();
            var pageresult = products.ToPagedList(currentPage, pagesize);
            return pageresult;
        }

        //搜尋商品方法
        public IQueryable<tProduct> SearchProducts(string searchprod, int? categoryId, int? efficacyId, int? noteId, int? partId, int? featureId, bool? fDiscontinued)
        {
            var products = db.tProducts.AsQueryable();

            //搜尋商品名稱方法(糢糊搜尋)
            if (searchprod != null)
            {
                products = products.Where(p => p.fProductChName.Contains(searchprod));
            }
            //找全部商品類別方法
            if (categoryId != null && categoryId != 0)
            {
                products = products.Where(p => p.fCategoryID == categoryId);
            }
            //找全部商品功效方法
            if (efficacyId != null && efficacyId != 0)
            {
                products = products.Where(p => p.tEfficacies.Any(q => q.fEfficacyID == efficacyId));
            }
            //找單方精油香調方法
            if (noteId != null && noteId != 0)
            {
                products = products.Where(p => p.tProductUnilateral != null &&
                p.tProductUnilateral.fNoteID == noteId);
            }
            //找單方精油萃取部位方法
            if (partId != null && partId != 0)
            {
                products = products.Where(p => p.tProductUnilateral != null &&
                p.tProductUnilateral.fPartID == partId);
            }
            //找植物油&純露特性方法
            if (featureId != null && featureId != 0)
            {
                products = products.Where(p => p.tProductVegetableoil != null &&
                  p.tProductVegetableoil.ffeatureID == featureId);
            }

            var productsResult = products.Where(p => p.fDiscontinued == false);

            return productsResult;
        }

        //刪除商品方法
        public void deleteProd(int prodId)
        {
            var prod = db.tProducts.Where(m => m.fProductID == prodId).FirstOrDefault();
            var prodU = db.tProductUnilaterals.Where(m => m.fProductID == prodId).FirstOrDefault();
            var prodV = db.tProductVegetableoils.Where(m => m.fProductID == prodId).FirstOrDefault();

            if (prodU != null)
            {
                db.tProductUnilaterals.Remove(prodU);
            }

            if (prodV != null)
            {
                db.tProductVegetableoils.Remove(prodV);
            }

            db.tProducts.Remove(prod);
            db.SaveChanges();
        }

        //ProductID重複解法
        public int SetProductId(int prodId)
        {
            prodId = prodId == 0 ?
                db.tProducts.Max(p => p.fProductID) + 1 : prodId + 1;

            if (db.tProducts.Any(p => p.fProductID == prodId))
            {
                SetProductId(prodId);
            }

            return prodId;
        }

        //更新商品方法
        public void UpdateProduct
            (tProduct prod, HttpPostedFileBase prodImg, HttpServerUtilityBase server)
        {
            string fileName = "";
            if (prodImg != null)
            {
                if (prodImg.ContentLength > 0)
                {
                    fileName = prod.fProductID + Path.GetExtension(prodImg.FileName); //ID+取得副檔名
                    var path = Path.Combine(server.MapPath("~/Images/Product"), fileName); //合成(取得存檔路徑+名稱)
                    prodImg.SaveAs(path); //存檔上傳照片 至path
                }
            }

            var product = db.tProducts.FirstOrDefault(p => p.fProductID == prod.fProductID);
            var unil = db.tProductUnilaterals.FirstOrDefault(p => p.fProductID == prod.fProductID);
            var vegetable = db.tProductVegetableoils.FirstOrDefault(p => p.fProductID == prod.fProductID);

            try
            {
                product.fProductChName = prod.fProductChName;
                product.fProductDesc = prod.fProductDesc;
                product.fUnitPrice = prod.fUnitPrice;
                product.fQuantityPerUnit = prod.fQuantityPerUnit;
                product.fCategoryID = prod.fCategoryID;
                product.fDiscontinued = prod.fDiscontinued;
                product.fUnitsInStock = prod.fUnitsInStock;

                if (vegetable != null)
                {
                    vegetable.ffeatureID = prod.tProductVegetableoil.ffeatureID;
                }

                if (unil != null)
                {
                    unil.fPartID = prod.tProductUnilateral.fPartID;
                    unil.fNoteID = prod.tProductUnilateral.fNoteID;
                    unil.fOrigin = prod.tProductUnilateral.fOrigin;
                    unil.fextraction = prod.tProductUnilateral.fextraction;
                }

                db.SaveChanges();
            }
            catch (Exception ee)
            {
                throw;
            }
        }

        //新增商品方法
        public void InsertProduct(tProduct prod, HttpPostedFileBase prodImg, HttpServerUtilityBase server)
        {
            string fileName = "";
            

            //限定同時只有一位操作者能增加ProdcutID
            lock (lockObject)
            {
                int prodId = SetProductId(0);
                prod.fProductID = prodId;

                if (prodImg != null)
                {
                    if (prodImg.ContentLength > 0)
                    {
                        fileName = prod.fProductID + Path.GetExtension(prodImg.FileName); //ID+取得副檔名
                        var path = Path.Combine(server.MapPath("~\\Images\\Product"), fileName); //合成(取得存檔路徑+名稱)
                        prodImg.SaveAs(path); //存檔上傳照片 至path
                    }
                }

                var product = new tProduct()
                {
                    fProductID = prod.fProductID,
                    fProductChName = prod.fProductChName,
                    fDiscontinued = prod.fDiscontinued,
                    fProductDesc = prod.fProductDesc,
                    fQuantityPerUnit = prod.fQuantityPerUnit,
                    fUnitPrice = prod.fUnitPrice,
                    fUnitsInStock = prod.fUnitsInStock,
                    fCategoryID = prod.fCategoryID
              
                };

                var productU = new tProductUnilateral()
                {
                    fProductID = prod.fProductID,
                    fextraction = prod.tProductUnilateral.fextraction,
                    fNoteID = prod.tProductUnilateral.fNoteID,
                    fOrigin = prod.tProductUnilateral.fOrigin,
                    fPartID = prod.tProductUnilateral.fNoteID
                };

                var productV = new tProductVegetableoil()
                {
                    fProductID = prod.fProductID,
                    ffeatureID = prod.tProductVegetableoil.ffeatureID,
                };

                db.tProductUnilaterals.Add(productU);
                db.tProductVegetableoils.Add(productV);
                db.tProducts.Add(product);
                db.SaveChanges();
            }

        }
    }
}
