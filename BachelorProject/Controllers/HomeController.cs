using BachelorProject.Models;
using Microsoft.AspNet.Identity;
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
                return await returnUserViewAsync();
            }
            else 
            {
                return View();
            }
            
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Om";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontantinformasjon";

            return View();
        }

        [Authorize(Roles = "admin,worker,new_user")]
        public async Task<ActionResult> returnUserViewAsync()
        {
            string userId = User.Identity.GetUserId();
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