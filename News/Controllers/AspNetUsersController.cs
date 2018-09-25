using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using News.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace News.Controllers
{
    [Authorize]
    public class AspNetUsersController : Controller
    {
        private NewsEntities db = new NewsEntities();

        // GET: AspNetUsers
        public ActionResult Index()
        {
            return View(db.AspNetUsers.ToList());
        }

        public ActionResult AdminEmail()
        {
            AspNetUserModel user = new AspNetUserModel();
            user.AspNetUsers = db.AspNetUsers.Where(u => u.Email != "mvctest74@gmail.com").OrderBy(u => u.UserName).ToList<AspNetUser>();
            return View(user);
        }

        [HttpPost]
        public ActionResult AdminEmail(AspNetUserModel model)
        {
            var selectedUser = model.AspNetUsers.Where(x => x.isChecked == true).ToList<AspNetUser>();
            TempData.Add("User", string.Join(",", selectedUser.Select(x => x.Email)));
            return RedirectToAction("AdminEmail2");
        }

        public async Task<ActionResult> AdminEmail2(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                string recipients = TempData["User"].ToString();
                string FromName = "ADMIN";
                string FromEmail = "mvctest74@gmail.com";
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.From = new MailAddress("mvctest74@gmail.com");
                message.Subject = model.Subject;
                message.Body = string.Format(body, FromName, FromEmail, model.Message);
                message.IsBodyHtml = true;

                if (model.Upload != null && model.Upload.ContentLength > 0)
                {
                    message.Attachments.Add(new Attachment(model.Upload.InputStream, System.IO.Path.GetFileName(model.Upload.FileName)));
                }
                foreach (var email in recipients.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(new MailAddress(email));
                }

                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }

            }
            return View(model);
        }


        public ActionResult Sent()
        {
            return View();
        }


        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        [Authorize(Roles = "Administrator")]
        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        [Authorize(Roles = "Administrator")]
        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
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
