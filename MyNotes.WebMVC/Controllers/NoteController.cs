using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Models;
using MyNotes.DataAccessLayer;
using MyNotes.EntityLayer;

namespace MyNotes.WebMVC.Controllers
{
    public class NoteController : Controller
    {
        private readonly NoteManager nm = new NoteManager();
        private readonly CategoryManager cm = new CategoryManager();
        private readonly LikedManager lm = new LikedManager();

        // GET: Note
        public ActionResult Index()
        {
            List<Note> notes = nm.QList().Include("Category").Include("Owner")
                .Where(x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(x=>x.ModifiedOn).ToList();

            return View(notes);
        }

        public ActionResult MyLikedNotes()
        {
            var notes = lm.List(s => s.LikedUser.Id == CurrentSession.User.Id).Select(s => s.Note).ToList();

            return View("Index",notes);
        }
        
        // GET: Note/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(s=>s.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Note/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(),"Id","Title");
            return View();
        }

        // POST: Note/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
             
                nm.Insert(note);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title",note.CategoryId);
            return View(note);
        }

        // GET: Note/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(s=>s.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Note/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Text,IsDraft,LikeCount,CreatedOn,ModifiedOn,ModifiedUserName")] Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                Note dbNote = nm.Find(s => s.Id == note.Id);
                dbNote.IsDraft = note.IsDraft;
                dbNote.CategoryId = note.CategoryId;
                dbNote.Text = note.Text;
                dbNote.Title = note.Title;

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // GET: Note/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(s=>s.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = nm.Find(s => s.Id == id);
            nm.Delete(note);
            return RedirectToAction("Index");
        }

    }
}
