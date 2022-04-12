using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Models;
using MyNotes.BusinessLayer.ValueObject;

namespace MyNotes.WebMVC.Controllers
{
    public class CategoryController : Controller
    {
        readonly CategoryManager cm = new CategoryManager();
        // GET: Category
        public ActionResult Index()
        {
            var cat = cm.IndexCat();
            return View(cat);
            //return View(cm.List());
        }

        public ActionResult Details(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CategoryViewModel cvm = cm.FindCat(id);
            if (cvm == null)
            {
                return HttpNotFound();
            }
            //CategoryViewModel cvm = new CategoryViewModel();
            //cvm.Id = cat.Id;
            //cvm.Title = cat.Title;
            //cvm.Description = cat.Description;
            //cvm.CreatedOn = cat.CreatedOn;
            //cat.ModifiedOn = cat.ModifiedOn;
            //cat.ModifiedUserName = cat.ModifiedUserName;
            return View(cvm);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CategoryViewModel cvm)
        {
            if (ModelState.IsValid)
            {
                cm.InsertCat(cvm);
                CacheHelper.RemoveCategoriesCache();
                return RedirectToAction("Index");
            }
            return View(cvm);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryViewModel cvm = cm.FindCat(id);
            if (cvm == null)
            {
                return HttpNotFound();
            }
            cm.DeleteCat(id);
            CacheHelper.RemoveCategoriesCache();
            return RedirectToAction("Index");
        }
    }
}