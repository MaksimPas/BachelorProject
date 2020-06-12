using BachelorProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.IO;
using Quartz;
using System.Threading.Tasks;
using Quartz.Impl;
using System.Collections.Specialized;
using System.Web.Routing;
using System.Web.UI;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace BachelorProject.Controllers
{
    public class SendEmailController : Controller
    {
        private readonly ApplicationDbContext db;
        public SendEmailController()
        {
            db = new ApplicationDbContext();
        }

        //public void Send()
        //{

        //    //select all record with specific expidation dates
        //    var result = db.DepotRecords
        //                         .Include(d => d.Equipment)
        //                         .Where(record => record.ExpirationDate < new DateTime(2020, 10, 1))
        //                         .ToList();
        //    try
        //    {
        //        var senderEmail = new MailAddress("maksympas111@gmail.com", "Maksym");
        //        var receiverEmail = new MailAddress("maksympas111@gmail.com", "Receiver");
        //        var password = "Elvis27059393++";
        //        var subject = "subject";
        //        var body = RenderRazorViewToString("~/Views/EmailService/EmailToSend.cshtml", result);
        //        var smtp = new SmtpClient
        //        {
        //            Host = "smtp.gmail.com",
        //            Port = 587,
        //            EnableSsl = true,
        //            DeliveryMethod = SmtpDeliveryMethod.Network,
        //            UseDefaultCredentials = false,
        //            Credentials = new NetworkCredential(senderEmail.Address, password)
        //        };
        //        using (var message = new MailMessage(senderEmail, receiverEmail) { IsBodyHtml = true, Subject = subject, Body = body })
        //        {
        //            smtp.Send(message);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        var message = e.Message;
        //    }
        //}




        ////cannot split view from controller. A view must always have a controller context
        //public string RenderRazorViewToString(string viewName, object model)
        //{
        //    ViewData.Model = model;
        //    using (var sw = new StringWriter())
        //    {
        //        var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
        //                                                                 viewName);
        //        var viewContext = new ViewContext(ControllerContext, viewResult.View,
        //                                     ViewData, TempData, sw);
        //        viewResult.View.Render(viewContext, sw);
        //        viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
        //        return sw.GetStringBuilder().ToString();
        //    }
        //}

        
    }

    class SendMailJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(SendMail);
        }

        public void SendMail() 
        {
            var db = new ApplicationDbContext();
            UserStore<ApplicationUser> us = new UserStore<ApplicationUser>(db);
            ApplicationUserManager userManager = new ApplicationUserManager(us);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            //select all record with specific expidation dates
            var depotRecords = db.DepotRecords.Include(d => d.Equipment).ToList();
            
            //customers wants to be notified 1 month in advance
            //LINQ doesnt understand C# code. must retrieve the entire collection and filer it here in this method
            var toBeExpired = depotRecords.Where(record => (record.ExpirationDate != null) && (record.ExpirationDate >= DateTime.Today && record.ExpirationDate <= DateTime.Today.AddMonths(1)) )
                                           //consider only those records which have not been depleted completely:
                                          .Where(record => record.QuantityLeft > 0);
            var expired = depotRecords.Where(record => (record.ExpirationDate != null) && (record.ExpirationDate < DateTime.Today))
                                      //consider only those records which have not been depleted:
                                      .Where(record => record.QuantityLeft > 0);
            //if both queries returned 0 elements then dont send email. 
            if (toBeExpired.Count()==0 && expired.Count()==0)
            {
                return;
            }

            //get admin role id
            string adminRoleId = roleManager.FindByName("admin").Id;
            //exclude admin from the result
            var admins = userManager.Users.Where(user => user.Roles.FirstOrDefault().RoleId == adminRoleId).ToList();
            string receiver = admins.First().Email;
            string firstName = admins.First().FirstName;
            string lastName = admins.First().LastName;

            //creating email body
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<h1>"+string.Format("Hei, {0} {1}!",firstName,lastName)+"</h1>"); //make this contain first and last names 
            string borderStyle = "style='border: 1px solid black;border-collapse: collapse;'";
            if (toBeExpired.Count() > 0)
            {
                //equipment with near expiration date:
                sb.AppendLine("<h3> Følgende utstyr har mindre enn 1 måned før det går ut på dato:</h3>");
                sb.AppendLine("<table style='border: 1px solid black;border-collapse: collapse; width:100%;text-align:center;'>");
                sb.AppendFormat("<tr><th {4}>{0}</th><th {4}>{1}</th><th {4}>{2}</th><th {4}>{3}</th></tr> ",
                                "Navn og Type", "Utløpsdato", "Antall opprinnelig", "Antall igjen",borderStyle);
                foreach (var item in toBeExpired)
                {
                    //sb.AppendFormat("<tr><td" + borderStyle + ">{0}</td><td" + borderStyle + ">{1}</td><td" + borderStyle + ">{2}</td><td" + borderStyle + ">{3}</td></tr>",
                    sb.AppendFormat("<tr><td {4}>{0}</td><td {4}>{1}</td><td {4}>{2}</td><td {4}>{3}</td></tr> ",
                                    item.Equipment.NameAndType,
                                    item.ExpirationDate.Value.ToShortDateString(),
                                    item.QuantityOriginal,
                                    item.QuantityLeft,
                                    borderStyle);
                }
                sb.AppendLine("</table>");
                sb.AppendLine("<br>");
                sb.AppendLine("<hr/>");
            }
            if (expired.Count() > 0)
            {
                //Equipment which has already expired:
                sb.AppendLine("<h3> Følgende utstyr har allerede gått ut på dato:</h3>");
                sb.AppendLine("<table style='border: 1px solid black;border-collapse: collapse; width:100%;text-align:center;'>");
                //sb.AppendFormat("<tr><th" + borderStyle + ">{0}</th><th" + borderStyle + ">{1}</th><th" + borderStyle + ">{2}</th><th" + borderStyle + ">{3}</th></tr> ",
                sb.AppendFormat("<tr><th {4}>{0}</th><th {4}>{1}</th><th {4}>{2}</th><th {4}>{3}</th></tr> ",
                                "Navn og Type", "Utløpsdato", "Antall opprinnelig", "Antall igjen",borderStyle);
                foreach (var item in expired)
                {
                    sb.AppendFormat("<tr><td {4}>{0}</td><td {4}>{1}</td><td {4}>{2}</td><td {4}>{3}</td></tr> ",
                                    item.Equipment.NameAndType,
                                    item.ExpirationDate.Value.ToShortDateString(),
                                    item.QuantityOriginal,
                                    item.QuantityLeft,
                                    borderStyle);
                }
                sb.AppendLine("</table>");
            }

            var body = sb.ToString();

            try
            {
                var senderEmail = new MailAddress("maksympas111@gmail.com", "Trondheim Røde Kors");
                var receiverEmail = new MailAddress("maksympas111@gmail.com", "Receiver");
                //remember to change password
                var password = "Elvis27059393++";
                var subject = "Påminning om utstyr som går ut på dato";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var message = new MailMessage(senderEmail, receiverEmail) { IsBodyHtml = true, Subject = subject, Body = body })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {
                var message = e.Message;
            }
        }
    }

    class QuartzSendMailJob
    {
        public static async void Execute()
        {
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            IScheduler sched = await factory.GetScheduler();
            await sched.Start();
            IJobDetail sendMailJob = JobBuilder.Create<SendMailJob>()
                                        .WithIdentity("SendMailJob", "group1")
                                        .Build();
            var trigger = TriggerBuilder.Create()
                                .WithIdentity("trigger1", "group1")
                                //just to test:
                                .WithSchedule(SimpleScheduleBuilder.Create().WithIntervalInSeconds(5))
                                //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(7, 0))
                                .StartNow()
                                .Build();
            await sched.ScheduleJob(sendMailJob, trigger);
        }
    }

}
