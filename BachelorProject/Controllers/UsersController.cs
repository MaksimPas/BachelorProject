using BachelorProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BachelorProject.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db;
        private UserStore<ApplicationUser> us;
        private ApplicationUserManager userManager;
        //public static just in order to make it accessible
        public static RoleManager<IdentityRole> roleManager;

        public UsersController()
        {
            db = new ApplicationDbContext();
            us = new UserStore<ApplicationUser>(db);
            userManager = new ApplicationUserManager(us);
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        }
        
        // GET: Users
        public async Task<ActionResult> Index()
        {
            var result = new List<UserViewModel>();
            var users = await userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                result.Add(await UserViewModel.CreateAsync(user, roleManager));
            }

            return View(result);
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserViewModel userViewModel = await UserViewModel.CreateAsync(user, roleManager);
            return View(userViewModel); 
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserViewModel model = await UserViewModel.CreateAsync(user, roleManager);
            ViewBag.dropdownlist = new SelectList(db.Roles, "Id", "Name", model.RoleId);
            return View(model);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,RoleId,FirstName,LastName,Email")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                //Do not have all info about user to create a complede user model
                ApplicationUser user = userManager.FindById(userViewModel.UserId);
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.Email = userViewModel.Email;
                user.UserName = userViewModel.Email;

                ////editing user role
                //removing user from all roles
                var roles = await userManager.GetRolesAsync(user.Id);
                await userManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
                //Assigning user to the specific role
                string roleName = (await roleManager.FindByIdAsync(userViewModel.RoleId)).Name;
                userManager.AddToRole(user.Id, roleName);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userViewModel);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserViewModel userViewModel = await UserViewModel.CreateAsync(user, roleManager);
            return View(userViewModel);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
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