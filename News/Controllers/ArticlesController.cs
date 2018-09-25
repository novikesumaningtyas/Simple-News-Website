using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using News.Models;

using Microsoft.AspNet.Identity;

namespace News.Controllers
{
    public class ArticlesController : Controller
    {
        private NewsEntities db = new NewsEntities();

        // GET: Articles
        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.Journalist1);
            return View(db.Articles.OrderByDescending(x => x.date).ToList());
        }

        ////Get: Articles
        public ActionResult IndexUserNames()
        {
            //return View(db.Articles.ToList());
            string currentUserId = User.Identity.GetUserId();
            return View(db.Articles.Where(m => m.authorId == currentUserId).ToList());
        }


        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        [Authorize]
        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.journalist = new SelectList(db.Journalists, "journalistID", "FName");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "articleID,authorId,name,date,topic,content,journalist")] Article article)
        {
            if (string.IsNullOrEmpty(article.name))
            {
                ModelState.AddModelError("name", "Missing data : Title is required");
            }

            if (string.IsNullOrEmpty(article.content))
            {
                ModelState.AddModelError("content", "Missing data : Content of article is required");
            }

            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Success");
            }

            ViewBag.journalist = new SelectList(db.Journalists, "journalistID", "FName", article.journalist);
            return View(article);
        }

        public ActionResult Success()
        {
            return View();
        }

        // GET: Aticles/Create
        public ActionResult CreateIndividual()
        {
            Article article = new Article();
            string currentUserId = User.Identity.GetUserId();
            article.authorId = currentUserId;
            ViewBag.journalist = new SelectList(db.Journalists, "journalistID", "FName");
            return View(article);
        }

        // POST: Artiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIndividual([Bind(Include = "articleID,authorId,name,date,topic,content,journalist")] Article article)
        {

            if (string.IsNullOrEmpty(article.name))
            {
                ModelState.AddModelError("name", "Missing data : Title is required");
            }

            if (string.IsNullOrEmpty(article.content))
            {
                ModelState.AddModelError("content", "Missing data : Content of article is required");
            }


            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.journalist = new SelectList(db.Journalists, "journalistID", "FName", article.journalist);
            return View(article);
        }

        [Authorize]
        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.journalist = new SelectList(db.Journalists, "journalistID", "FName", article.journalist);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "articleID,authorId,name,date,topic,content,journalist")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.journalist = new SelectList(db.Journalists, "journalistID", "FName", article.journalist);
            return View(article);
        }

        [Authorize]
        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        [Authorize]
        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
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
