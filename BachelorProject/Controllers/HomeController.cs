using BachelorProject.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BachelorProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserStore<ApplicationUser> us;
        private readonly ApplicationUserManager userManager;
        public HomeController()
        {
            db = new ApplicationDbContext();
            us = new UserStore<ApplicationUser>(db);
            userManager = new ApplicationUserManager(us);
        }

        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated)
            {
                return await returnUserView();
            }
            else 
            {
                return View();
            }
            
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "admin,worker,new_user")]
        public async Task<ActionResult> returnUserView()
        {
            string userEmail = HttpContext.User.Identity.Name;
            string userId = (await userManager.FindByEmailAsync(userEmail)).Id;
            string userRole = (await userManager.GetRolesAsync(userId)).First();
            switch (userRole)
            {
                case "new_user":
                    return View("New_user");
                case "worker":
                    return View("Worker");
                case "admin":
                    return View("Admin");
                default:
                    throw new NotImplementedException(string.Format("User has unidentified role {0}", userRole));
            }
        }
    }
}