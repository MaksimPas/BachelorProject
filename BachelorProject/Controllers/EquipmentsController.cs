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

namespace BachelorProject.Controllers
{
    [Authorize]
    public class EquipmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Equipments
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Index()
        {
            var equipment = db.Equipments;

            ViewBag.searchParameter = "";
            int pageSize = 10;
            int pageNumber = 1;
            return View(await equipment.OrderBy(record => record.Id).ToPagedListAsync(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateIndex(string searchParameter, int? page, string sortColumn, string sortOrder)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;

            IQueryable<Equipment> result;

            //filtering the query
            if (searchParameter != null && searchParameter != "")
            {
                ViewBag.searchParameter = searchParameter;
                result = db.Equipments
                            //filter
                           .Where(d => d.NameAndType.Contains(searchParameter));
            }
            else
            {
                //searchParameter is null, i.e. no filter is specified
                result = db.Equipments;
            }

            //sorting the query
            ViewBag.sortColumn = sortColumn; //this can be null
            ViewBag.sortOrder = sortOrder; //this can be null
            switch (sortColumn)
            {
                case "Id":
                    if (sortOrder == "ASC")
                    {
                        result = result.OrderBy(d => d.Id);
                        ViewBag.newSortOrderForId = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.Id);
                        ViewBag.newSortOrderForId = "ASC";
                    }
                    break;
                case "NameAndType":
                    if (sortOrder == "ASC")
                    {
                        result = result.OrderBy(d => d.NameAndType);
                        ViewBag.newSortOrderForNameAndType = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.NameAndType);
                        ViewBag.newSortOrderForNameAndType = "ASC";
                    }
                    break;
                default:
                    result = result.OrderBy(s => s.Id);
                    break;
            }


            var viewResult = await result.ToPagedListAsync(pageNumber, pageSize);
            return PartialView("UpdateIndex", viewResult as IEnumerable<Equipment>);
        }

        // GET: Equipments/Details/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = await db.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // GET: Equipments/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,NameAndType")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Equipments.Add(equipment);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Noe gikk galt. Vennligst prøv igjen.");
                    ModelState.AddModelError("", "Tips: utstyret kan ikke ha duplisert navn.");
                    return View(equipment);
                }
            }   
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = await db.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,NameAndType")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = await db.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Equipment equipment = await db.Equipments.FindAsync(id);
                db.Equipments.Remove(equipment);
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
