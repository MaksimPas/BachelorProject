using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BachelorProject.Models;
using PagedList; //pagination: ToPagedList()
using PagedList.EntityFramework; // pagination async: ToPagedListAsync()
using Microsoft.AspNet.Identity;

namespace BachelorProject.Controllers
{
    [Authorize]
    public class LogRecordsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LogRecords
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Index()
        {
            var logRecords = db.LogRecords.Include(r => r.ApplicationUser);

            ViewBag.searchParameter = "";

            int pageSize = 10;
            int pageNumber = 1;
            return View(await logRecords.OrderByDescending(record => record.Id).ToPagedListAsync(pageNumber, pageSize));
        }

        [Authorize(Roles = "worker,subAdmin")]
        public async Task<ActionResult> IndexByUserId()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    throw new NotImplementedException();
                }

                string userId = User.Identity.GetUserId();

                int pageNumber = 1;
                int pageSize = 10;

                var result = db.LogRecords.Include(r => r.ApplicationUser)
                                           .Where(d => d.ApplicationUser.Id == userId);

                var viewResult = await result.OrderByDescending(r => r.Id)
                                             .ToPagedListAsync(pageNumber, pageSize);
                return View("IndexByUserId", viewResult as IEnumerable<LogRecord>);
            }
            catch (Exception e)
            {
                //var msg = e.Message;
                throw new NotImplementedException();
            }
        }

        [Authorize(Roles = "worker,subAdmin")]
        public async Task<ActionResult> UpdateIndexByUserId(string searchParameter, int? page, string sortColumn, string sortOrder)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    throw new NotImplementedException();
                }

                string userId = User.Identity.GetUserId();

                int pageNumber = (page ?? 1);
                int pageSize = 10;

                var result = db.LogRecords.Include(r => r.ApplicationUser)
                                           .Where(d => d.ApplicationUser.Id == userId);

                //filtering the query
                if (searchParameter != null && searchParameter != "")
                {
                    ViewBag.searchParameter = searchParameter;
                    result = result.Where(d => d.InfoMessage.Contains(searchParameter));
                }

                //sorting the query
                ViewBag.sortColumn = sortColumn; //this can be null
                ViewBag.sortOrder = sortOrder; //this can be null
                switch (sortColumn)
                {
                    case "DateOfRecord":
                        if (sortOrder == "ASC")
                        {
                            result = result.OrderBy(d => d.DateOfRecord);
                            ViewBag.newSortOrderForDateOfRecord = "DESC";
                        }
                        else
                        {
                            result = result.OrderByDescending(d => d.DateOfRecord);
                            ViewBag.newSortOrderForDateOfRecord = "ASC";
                        }
                        break;
                    case "Action":
                        if (sortOrder == "ASC")
                        {
                            result = result.OrderBy(d => d.Action);
                            ViewBag.newSortOrderForAction = "DESC";
                        }
                        else
                        {
                            result = result.OrderByDescending(d => d.Action);
                            ViewBag.newSortOrderForAction = "ASC";
                        }
                        break;
                    case "InfoMessage":
                        if (sortOrder == "ASC")
                        {
                            result = result.OrderBy(d => d.InfoMessage);
                            ViewBag.newSortOrderForInfoMessage = "DESC";
                        }
                        else
                        {
                            result = result.OrderByDescending(d => d.InfoMessage);
                            ViewBag.newSortOrderForInfoMessage = "ASC";
                        }
                        break;
                    default:
                        result = result.OrderBy(s => s.Id);
                        break;
                }


                var viewResult = await result.ToPagedListAsync(pageNumber, pageSize);
                return PartialView("UpdateIndexByUserId", viewResult as IEnumerable<LogRecord>);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateIndex(string searchParameter, int? page, string sortColumn, string sortOrder)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;

            IQueryable<LogRecord> result;

            //filtering the query
            if (searchParameter != null && searchParameter != "")
            {
                ViewBag.searchParameter = searchParameter;
                result = db.LogRecords
                           .Include(r => r.ApplicationUser)
                            //filter
                            .Where(d => d.ApplicationUser.Email.Contains(searchParameter) ||
                                            d.ApplicationUser.FirstName.Contains(searchParameter) ||
                                            d.ApplicationUser.LastName.Contains(searchParameter));
            }
            else
            {
                //searchParameter is null, i.e. no filter is specified
                result = db.LogRecords.Include(r => r.ApplicationUser);
            }

            //sorting the query
            ViewBag.sortColumn = sortColumn; //this can be null
            ViewBag.sortOrder = sortOrder; //this can be null
            switch (sortColumn)
            {
                case "FirstName":
                    if (sortOrder == "ASC")
                    {
                        result = result.OrderBy(d => d.ApplicationUser.FirstName);
                        ViewBag.newSortOrderForFirstName = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.ApplicationUser.FirstName);
                        ViewBag.newSortOrderForFirstName = "ASC";
                    }
                    break;
                case "LastName":
                    if (sortOrder == "ASC")
                    {
                        result = result.OrderBy(d => d.ApplicationUser.LastName);
                        ViewBag.newSortOrderForLastName = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.ApplicationUser.LastName);
                        ViewBag.newSortOrderForLastName = "ASC";
                    }
                    break;
                case "Email":
                    if (sortOrder == "ASC")
                    {
                        result = result.OrderBy(d => d.ApplicationUser.Email);
                        ViewBag.newSortOrderForEmail = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.ApplicationUser.Email);
                        ViewBag.newSortOrderForEmail = "ASC";
                    }
                    break;
                case "DateOfRecord":
                    if (sortOrder == "ASC")
                    {
                        result = result.OrderBy(d => d.DateOfRecord);
                        ViewBag.newSortOrderForDateOfRecord = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.DateOfRecord);
                        ViewBag.newSortOrderForDateOfRecord = "ASC";
                    }
                    break;
                case "Action":
                    if (sortOrder == "ASC")
                    {
                        result = result.OrderBy(d => d.Action);
                        ViewBag.newSortOrderForAction = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.Action);
                        ViewBag.newSortOrderForAction = "ASC";
                    }
                    break;
                case "InfoMessage":
                    if (sortOrder == "ASC")
                    {
                        result = result.OrderBy(d => d.InfoMessage);
                        ViewBag.newSortOrderForInfoMessage = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.InfoMessage);
                        ViewBag.newSortOrderForInfoMessage = "ASC";
                    }
                    break;
                default:
                    result = result.OrderBy(s => s.Id);
                    break;
            }


            var viewResult = await result.ToPagedListAsync(pageNumber, pageSize);
            return PartialView("UpdateIndex", viewResult as IEnumerable<LogRecord>);
        }

        // GET: LogRecords/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    LogRecord logRecord = await db.LogRecords.FindAsync(id);
        //    if (logRecord == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(logRecord);
        //}

        // GET: LogRecords/Create
        //public ActionResult Create()
        //{
        //    ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Email");
        //    return View();
        //}

        // POST: LogRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,UserId,DateOfRecord,Action,InfoMessage")] LogRecord logRecord)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.LogRecords.Add(logRecord);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(logRecord);
        //}

        [Authorize(Roles = "worker,subAdmin")]
        public void Create([Bind(Include = "UserId,Action,InfoMessage")] LogRecord logRecord)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    logRecord.DateOfRecord = DateTime.Now;
                    db.LogRecords.Add(logRecord);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    //if something went wrong here, don't do anything
                }
            }

        }


        // POST: LogRecords/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                LogRecord logRecord = await db.LogRecords.FindAsync(id);
                db.LogRecords.Remove(logRecord);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Error");
            }
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
