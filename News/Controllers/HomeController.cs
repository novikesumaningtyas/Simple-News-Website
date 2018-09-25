using System;
using System.Net;
using System.Net.Mail;

using News.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace News.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Documentation()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Email()
        {

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Email(EmailFormModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
        //        var message = new MailMessage();
        //        message.To.Add(new MailAddress("novi.kesumaningtyas@gmail.com"));
        //        message.From = new MailAddress("novi.kesumaningtyas@outlook.com");
        //        message.Subject = "Your email subject";
        //        message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
        //        message.IsBodyHtml = true;

        //        if (model.Upload != null && model.Upload.ContentLength > 0)
        //        {
        //            message.Attachments.Add(new Attachment(model.Upload.InputStream, System.IO.Path.GetFileName(model.Upload.FileName)));
        //        }


        //        using (var smtp = new SmtpClient())
        //        {
        //            var credential = new NetworkCredential
        //            {
        //                UserName = "novi.kesumaningtyas@outlook.com",
        //                Password = "2Gobaksodor"
        //            };
        //            smtp.Credentials = credential;
        //            smtp.Host = "smtp-mail.outlook.com";
        //            smtp.Port = 587;
        //            smtp.EnableSsl = true;
        //            await smtp.SendMailAsync(message);
        //            return RedirectToAction("Sent");


        //        }
        //    }
        //    return View(model);
        //}

        public ActionResult Sent()
        {
            return View();
        }

        public ActionResult Display()
        {
            return View();
        }
    }
}