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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BachelorProject.Controllers
{
    public class DepotRecordsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //NB!!
        //Create a view model and use it --> much more convenient

        // GET: DepotRecords
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult> Index()
        {   
            var depotRecords = db.DepotRecords.Include(d => d.Equipment);
            return View(await depotRecords.ToListAsync());
        }

        // GET: DepotRecords/Details/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepotRecord depotRecord = await db.DepotRecords.FindAsync(id);
            if (depotRecord == null)
            {
                return HttpNotFound();
            }
            return View(depotRecord);
        }

        // GET: DepotRecords/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.selectList = new SelectList(db.Equipments, "Id", "NameAndType");
            return View();
        }

        // POST: DepotRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EquipmentCodeId,ExpirationDate,QuantityOriginal")] DepotRecord depotRecord)
        {
            depotRecord.QuantityLeft = depotRecord.QuantityOriginal;
            if (ModelState.IsValid)
            {
                db.DepotRecords.Add(depotRecord);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        // GET: DepotRecords/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepotRecord depotRecord = await db.DepotRecords.FindAsync(id);
            if (depotRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.dropDownList = new SelectList(db.Equipments, "Id", "NameAndType", depotRecord.EquipmentCodeId);
            return View(depotRecord);
        }

        // POST: DepotRecords/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EquipmentCodeId,ExpirationDate,QuantityOriginal,QuantityLeft")] DepotRecord depotRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depotRecord).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.dropDownList = new SelectList(db.Equipments, "Id", "NameAndType", depotRecord.EquipmentCodeId);
            return View(depotRecord);
        }

        // GET: DepotRecords/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepotRecord depotRecord = await db.DepotRecords.FindAsync(id);
            if (depotRecord == null)
            {
                return HttpNotFound();
            }
            return View(depotRecord);
        }

        // POST: DepotRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DepotRecord depotRecord = await db.DepotRecords.FindAsync(id);
            db.DepotRecords.Remove(depotRecord);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "admin,worker")]
        public ActionResult ReduceQuantity()
        {
            ViewBag.dropDownList = new SelectList(db.Equipments, "Id", "NameAndType");
            return PartialView();
        }

        [Authorize(Roles = "admin,worker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReduceQuantity(ReduceAmountViewModel formCollection)
        {
            int quantityToReduce = formCollection.ReduceQuantity;
            if (formCollection != null)
            {
                var depotRecords = db.DepotRecords.Where(record => record.EquipmentCodeId == formCollection.EquipmentId);
                foreach (var record in depotRecords)
                {
                    if (record.QuantityLeft >= quantityToReduce)
                    {
                        record.QuantityLeft -= quantityToReduce;
                        break;
                    }
                    quantityToReduce -= record.QuantityLeft;
                    record.QuantityLeft = 0;
                }
                await db.SaveChangesAsync();
                return RedirectToAction("../");
            }
            return RedirectToAction("../");
        }

        //[Authorize(Roles = "admin")]
        public async Task<ActionResult> Search()
        {
            var par = Request.QueryString["searchParameter"];

            //anonymous object could be passed as json:
            //var result = from e in db.DepotRecords
            //             where e.Equipment.NameAndType.Contains(par)
            //             select new
            //             {
            //                 e.Equipment.NameAndType,
            //                 e.ExpirationDate,
            //                 e.QuantityOriginal,
            //                 e.QuantityLeft

            //             };

            var result = await db.DepotRecords
                                 .Include(d => d.Equipment)
                                 .Where(record => record.Equipment.NameAndType.Contains(par))
                                 .ToListAsync();
                            

            return PartialView("SearchResult",result);
        }



    }
}
