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
    public class ImagesController : Controller
    {
        private ImagesEntities db = new ImagesEntities();

        // GET: Images
        public ActionResult Index()
        {
            return View(db.Images.ToList());
        }

        public ActionResult Gallery()
        {
            List<Image> all = new List<Image>();
            using (ImagesEntities dc = new ImagesEntities())
            {
                all = dc.Images.ToList();
            }

            return View(all);
        }

        [Authorize]
        //GET :Images
        public ActionResult IndexUserNames()
        {
            string currentUserId = User.Identity.GetUserId();
            List<Image> selected = new List<Image>();
            using (ImagesEntities dc = new ImagesEntities())
            {
                selected = dc.Images.Where(m => m.userName == currentUserId).ToList();

            }

            return View(selected);
        }

        //GET: Images/Create
        public ActionResult CreateIndividual()
        {
            Image image = new Image();
            string currentUserId = User.Identity.GetUserId();
            image.userName = currentUserId;
            return View(image);
        }

        // POST: Artiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult CreateIndividual(Image IG)
        {

            if (IG.File.ContentLength > (2 * 1024 * 1024))
            {
                ModelState.AddModelError("CustomError", "File size must be less than 2 MB");
                return View();
            }
            if (!(IG.File.ContentType == "image/jpeg" || IG.File.ContentType == "image/png"))
            {
                ModelState.AddModelError("CustomError", "File type allowed : jpeg and png");
                return View();
            }

            IG.fileName = IG.File.FileName;
            byte[] data = new byte[IG.File.ContentLength];
            IG.File.InputStream.Read(data, 0, IG.File.ContentLength);
            IG.image1 = data;
            using (ImagesEntities dc = new ImagesEntities())
            {
                dc.Images.Add(IG);
                dc.SaveChanges();
            }
            return RedirectToAction("IndexUserNames");
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(Image IG)
        {
            // Apply Validation Here


            if (IG.File.ContentLength > (2 * 1024 * 1024))
            {
                ModelState.AddModelError("CustomError", "File size must be less than 2 MB");
                return View();
            }
            if (!(IG.File.ContentType == "image/jpeg" || IG.File.ContentType == "image/png"))
            {
                ModelState.AddModelError("CustomError", "File type allowed : jpeg and png");
                return View();
            }

            IG.fileName = IG.File.FileName;
            byte[] data = new byte[IG.File.ContentLength];
            IG.File.InputStream.Read(data, 0, IG.File.ContentLength);
            IG.image1 = data;
            using (ImagesEntities dc = new ImagesEntities())
            {
                dc.Images.Add(IG);
                dc.SaveChanges();
            }
            return RedirectToAction("Gallery");
        }



        // GET: Images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // GET: Images/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "imageID,userName,name,fileName,image1")] Image image)
        {
            if (ModelState.IsValid)
            {
                db.Images.Add(image);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(image);
        }

        // GET: Images/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "imageID,userName,name,fileName,image1")] Image image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(image);
        }

        // GET: Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Image image = db.Images.Find(id);
            db.Images.Remove(image);
            db.SaveChanges();
            return RedirectToAction("Gallery");
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
