using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using News.Models;

namespace News.Controllers
{
    public class JournalistsController : Controller
    {
        private NewsEntities db = new NewsEntities();

        [Authorize]
        // GET: Journalists
        public ActionResult Index()
        {
            return View(db.Journalists.ToList());
        }

        [Authorize]
        // GET: Journalists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journalist journalist = db.Journalists.Find(id);
            if (journalist == null)
            {
                return HttpNotFound();
            }
            return View(journalist);
        }

        // GET: Journalists/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Journalists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "journalistID,FName,SName,phone,email,joinDate,introduction")] Journalist journalist)
        {
            if (ModelState.IsValid)
            {
                db.Journalists.Add(journalist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(journalist);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Journalists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journalist journalist = db.Journalists.Find(id);
            if (journalist == null)
            {
                return HttpNotFound();
            }
            return View(journalist);
        }

        // POST: Journalists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "journalistID,FName,SName,phone,email,joinDate,introduction")] Journalist journalist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(journalist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(journalist);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Journalists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journalist journalist = db.Journalists.Find(id);
            if (journalist == null)
            {
                return HttpNotFound();
            }
            return View(journalist);
        }

        [Authorize(Roles = "Administrator")]
        // POST: Journalists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Journalist journalist = db.Journalists.Find(id);
            db.Journalists.Remove(journalist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
