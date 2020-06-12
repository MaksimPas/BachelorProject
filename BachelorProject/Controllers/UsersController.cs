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
using PagedList; //pagination: ToPagedList()
using PagedList.EntityFramework; // pagination async: ToPagedListAsync()

namespace BachelorProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db;
        private UserStore<ApplicationUser> us;
        private ApplicationUserManager userManager;
        //public static just in order to make it accessible???
        private RoleManager<IdentityRole> roleManager;

        public UsersController()
        {
            db = new ApplicationDbContext();
            us = new UserStore<ApplicationUser>(db);
            userManager = new ApplicationUserManager(us);
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        }

        // GET: Users
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Index()
        {
            ViewBag.searchParameter = "";

            int pageSize = 10;
            int pageNumber = 1;

            //cannot do it like this due to EF6 sql statements mixing with C# code.
            //linq queries cannot recognize inner c# statements as they are being converted to sql statements
            //var users = userManager.Users.Select(user => UserViewModel.Create(user, roleManager));

            //.ToList() closes the SqlReader !! If not closed then must edit config to allow multiple readers to access the sql table
            //get admin role id
            string adminRoleId = (await roleManager.FindByNameAsync("admin")).Id;
            //exclude admin from the result
            var users = await userManager.Users.Where(user =>user.Roles.FirstOrDefault().RoleId != adminRoleId).ToListAsync();
            var viewModelList = await UserViewModel.CreateFromQueryAsListAsync(users, roleManager);

            return View(viewModelList.OrderBy(record => record.UserId).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateIndex(string searchParameter, int? page, string sortColumn, string sortOrder)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;

            IEnumerable<UserViewModel> viewModelList;
            IEnumerable<ApplicationUser> users;

            //filtering the query
            if (searchParameter != null && searchParameter != "")
            {
                //searching through the email, first name and last name
                ViewBag.searchParameter = searchParameter;
                //get admin role id
                string adminRoleId = (await roleManager.FindByNameAsync("admin")).Id;
                //exclude admin from the result
                users = db.Users.Where(user => user.Roles.FirstOrDefault().RoleId != adminRoleId)
                                .Where(d => d.Email.Contains(searchParameter) || d.FirstName.Contains(searchParameter) || d.LastName.Contains(searchParameter))
                                .ToList();
            }
            else
            {
                //searchParameter is null, i.e. no filter is specified
                //get admin role id
                string adminRoleId = (await roleManager.FindByNameAsync("admin")).Id;
                //exclude admin from the result
                users = db.Users.Where(user => user.Roles.FirstOrDefault().RoleId != adminRoleId).ToList();
            }

            viewModelList = await UserViewModel.CreateFromQueryAsListAsync(users, roleManager);

            //sorting the query
            ViewBag.sortColumn = sortColumn; //this can be null
            ViewBag.sortOrder = sortOrder; //this can be null
            switch (sortColumn)
            {
                case "FirstName":
                    if (sortOrder == "ASC")
                    {
                        viewModelList = viewModelList.OrderBy(d => d.FirstName);
                        ViewBag.newSortOrderForFirstName = "DESC";
                    }
                    else
                    {
                        viewModelList = viewModelList.OrderByDescending(d => d.FirstName);
                        ViewBag.newSortOrderForFirstName = "ASC";
                    }
                    break;
                case "LastName":
                    if (sortOrder == "ASC")
                    {
                        viewModelList = viewModelList.OrderBy(d => d.LastName);
                        ViewBag.newSortOrderForLastName = "DESC";
                    }
                    else
                    {
                        viewModelList = viewModelList.OrderByDescending(d => d.LastName);
                        ViewBag.newSortOrderForLastName = "ASC";
                    }
                    break;
                case "Email":
                    if (sortOrder == "ASC")
                    {
                        viewModelList = viewModelList.OrderBy(d => d.Email);
                        ViewBag.newSortOrderForEmail = "DESC";
                    }
                    else
                    {
                        viewModelList = viewModelList.OrderByDescending(d => d.Email);
                        ViewBag.newSortOrderForEmail = "ASC";
                    }
                    break;
                case "Telefon":
                    if (sortOrder == "ASC")
                    {
                        viewModelList = viewModelList.OrderBy(d => d.Phone);
                        ViewBag.newSortOrderForPhone = "DESC";
                    }
                    else
                    {
                        viewModelList = viewModelList.OrderByDescending(d => d.Phone);
                        ViewBag.newSortOrderForPhone = "ASC";
                    }
                    break;
                case "Role":
                    if (sortOrder == "ASC")
                    {
                        viewModelList = viewModelList.OrderBy(d => d.Role);
                        ViewBag.newSortOrderForRole = "DESC";
                    }
                    else
                    {
                        viewModelList = viewModelList.OrderByDescending(d => d.Role);
                        ViewBag.newSortOrderForRole = "ASC";
                    }
                    break;
                case "DateOfRecord":
                    if (sortOrder == "ASC")
                    {
                        viewModelList = viewModelList.OrderBy(d => d.DateOfRecord);
                        ViewBag.newSortOrderForRole = "DESC";
                    }
                    else
                    {
                        viewModelList = viewModelList.OrderByDescending(d => d.DateOfRecord);
                        ViewBag.newSortOrderForRole = "ASC";
                    }
                    break;
                default:
                    viewModelList = viewModelList.OrderBy(s => s.UserId);
                    break;
            }


            var viewResult = viewModelList.ToPagedList(pageNumber, pageSize);
            return PartialView("UpdateIndex", viewResult as IEnumerable<UserViewModel>);
        }

        // GET: Users/Details/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
            ViewBag.dropdownlist = new SelectList(db.Roles.Where(role=>role.Name!="admin"), "Id", "Name", model.RoleId);
            return View(model);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,RoleId,FirstName,LastName,Email,Phone,DateOfRecord")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                //Do not have all info about user to create a complede user model
                ApplicationUser user = userManager.FindById(userViewModel.UserId);
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.Phone;
                user.UserName = userViewModel.Email;

                ////editing user role
                //removing user from all roles
                var roles = await userManager.GetRolesAsync(user.Id);
                await userManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
                await db.SaveChangesAsync();
                //Assigning user to the specific role
                string roleName = (await roleManager.FindByIdAsync(userViewModel.RoleId)).Name;
                userManager.AddToRole(user.Id, roleName);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.dropdownlist = new SelectList(db.Roles.Where(role => role.Name != "admin"), "Id", "Name", userViewModel.RoleId);
            return View(userViewModel);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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