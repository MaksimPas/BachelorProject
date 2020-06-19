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
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Net.Mail;
using System.Collections.Specialized;
using System.Web.Routing;
using PagedList; //pagination
using PagedList.EntityFramework; // pagination async: ToPagedListAsync()
using Microsoft.AspNet.Identity;
using System.Globalization;

namespace BachelorProject.Controllers
{
    [Authorize]
    public class DepotRecordsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserStore<ApplicationUser> us;
        private readonly ApplicationUserManager userManager;
        public DepotRecordsController()
        {
            db = new ApplicationDbContext();
            us = new UserStore<ApplicationUser>(db);
            userManager = new ApplicationUserManager(us);
        }


        //protected override void Initialize(RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    string cultureInfo = "nb-NO";
        //    System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(cultureInfo);
        //    System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(cultureInfo);
        //    base.Initialize(requestContext);
        //}

        //NB!!
        //Can create a view model and use it --> much more convenient

        // GET: DepotRecords

        [Authorize(Roles = "admin,subAdmin")]
        public async Task<ActionResult> Index()
        {   
            var depotRecords = db.DepotRecords.Include(d => d.Equipment);

            ViewBag.searchParameter = "";

            int pageSize = 10;
            int pageNumber = 1;
            return View(await depotRecords.OrderBy(record => record.Id).ToPagedListAsync(pageNumber,pageSize));
        }

        [Authorize(Roles = "admin,subAdmin")]
        public async Task<ActionResult> UpdateIndex(string searchParameter, int? page, string sortColumn, string sortOrder)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;

            //Either access parameter like this:
            //var par = Request.QueryString["searchParameter"];
            //Or like this:
            //string par;

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

            var result = db.DepotRecords.Include(d => d.Equipment);

            //filtering the query
            if (searchParameter != null && searchParameter != "")
            {
                ViewBag.searchParameter = searchParameter;
                result = result.Where(d => d.Equipment.NameAndType.Contains(searchParameter));
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
                        result = result.OrderBy(d => d.Equipment.NameAndType);
                        ViewBag.newSortOrderForNameAndType = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.Equipment.NameAndType);
                        ViewBag.newSortOrderForNameAndType = "ASC";
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
                case "ExpirationDate":
                    if (sortOrder == "ASC")
                    {
                        result = result.OrderBy(d => d.ExpirationDate);
                        ViewBag.newSortOrderForExpirationDate = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.ExpirationDate);
                        ViewBag.newSortOrderForExpirationDate = "ASC";
                    }
                    break;
                case "QuantityOriginal":
                    if (sortOrder == "ASC")
                    {
                        result = result.OrderBy(d => d.QuantityOriginal);
                        ViewBag.newSortOrderForQuantityOriginal = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.QuantityOriginal);
                        ViewBag.newSortOrderForQuantityOriginal = "ASC";
                    }
                    break;
                case "QuantityLeft":
                    if (sortOrder == "ASC")
                    {
                        result = result.OrderBy(d => d.QuantityLeft);
                        ViewBag.newSortOrderForQuantityLeft = "DESC";
                    }
                    else
                    {
                        result = result.OrderByDescending(d => d.QuantityLeft);
                        ViewBag.newSortOrderForQuantityLeft = "ASC";
                    }
                    break;
                default:
                    result = result.OrderBy(s => s.Id);
                    break;
            }


            var viewResult = await result.ToPagedListAsync(pageNumber, pageSize);
            return PartialView("UpdateIndex", viewResult as IEnumerable<DepotRecord>);
        }

        // GET: DepotRecords/Details/5
        [Authorize(Roles = "admin,subAdmin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var depotRecord = (await db.DepotRecords.Include(d => d.Equipment).ToListAsync())
                              .Find(record => record.Id == id);
            if (depotRecord == null)
            {
                return HttpNotFound();
            }
            return View(depotRecord);
        }

        // GET: DepotRecords/Create
        [Authorize(Roles = "admin,subAdmin")]
        public ActionResult Create()
        {
            ViewBag.selectList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType");
            return View();
        }

        // POST: DepotRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin,subAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EquipmentCodeId,ExpirationDate,QuantityOriginal,Information")] DepotRecord depotRecord)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    depotRecord.QuantityLeft = depotRecord.QuantityOriginal;
                    depotRecord.DateOfRecord = DateTime.Now;
                    depotRecord = db.DepotRecords.Add(depotRecord);
                    await db.SaveChangesAsync();

                    var controller = DependencyResolver.Current.GetService<LogRecordsController>();
                    controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
                    controller.Create(
                        new LogRecord
                        {
                            UserId = this.User.Identity.GetUserId(),
                            DateOfRecord = DateTime.Now,
                            Action = LogAction.NYTT_UTSTYR,
                            InfoMessage = string.Format(
                                                        "NYTT UTSTYR. Utstyr-ID: {0}; Type-ID: {1}; Utløpsdato: {2}; Antall opprinnelig: {3}",
                                                        depotRecord.Id,
                                                        depotRecord.EquipmentCodeId,
                                                        (depotRecord.ExpirationDate != null) ? depotRecord.ExpirationDate.Value.ToShortDateString() : "Ingen",
                                                        depotRecord.QuantityOriginal
                                                        )
                        });
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Noe gikk galt. Vennligst prøv igjen.");
                    ViewBag.selectList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType");
                    return View(depotRecord);
                }
            }
            ViewBag.selectList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType");
            return View(depotRecord);
        }

        // GET: DepotRecords/Edit/5
        [Authorize(Roles = "admin,subAdmin")]
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
            ViewBag.dropDownList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType", depotRecord.EquipmentCodeId);
            return View(depotRecord);
        }

        // POST: DepotRecords/Edit/5
        [Authorize(Roles = "admin,subAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EquipmentCodeId,ExpirationDate,DateOfRecord,QuantityOriginal,QuantityLeft,Information")] DepotRecord depotRecord)
        {
            try
            {
                if (depotRecord.QuantityLeft > depotRecord.QuantityOriginal)
                {
                    ModelState.AddModelError(string.Empty, "Gjenværende antallet kan ikke være større enn det opprinnelige antallet!");
                    ViewBag.dropDownList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType", depotRecord.EquipmentCodeId);
                    return View(depotRecord);
                }
                if (ModelState.IsValid)
                {
                    db.Entry(depotRecord).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    var controller = DependencyResolver.Current.GetService<LogRecordsController>();
                    controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
                    controller.Create(
                        new LogRecord
                        {
                            UserId = this.User.Identity.GetUserId(),
                            DateOfRecord = DateTime.Now,
                            Action = LogAction.ENDRING,
                            InfoMessage = string.Format(
                                                        "ENDRING. NY INFO: Utstyr-ID: {0}; Type-ID: {1}; Utløpsdato: {2}; Antall opprinnelig: {3}; Antall igjen: {4}",
                                                        depotRecord.Id,
                                                        depotRecord.EquipmentCodeId,
                                                        (depotRecord.ExpirationDate != null) ? depotRecord.ExpirationDate.Value.ToShortDateString() : "Ingen",
                                                        depotRecord.QuantityOriginal
                                                        ,depotRecord.QuantityLeft
                                                        )
                        });
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                ViewBag.dropDownList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType", depotRecord.EquipmentCodeId);
                return View(depotRecord);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Noe gikk galt. Vennligst prøv igjen.");
                ViewBag.dropDownList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType", depotRecord.EquipmentCodeId);
                return View(depotRecord);
            }
        }

        // GET: DepotRecords/Delete/5
        [Authorize(Roles = "admin,subAdmin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var depotRecord = (await db.DepotRecords.Include(d => d.Equipment).ToListAsync())
                              .Find(record => record.Id == id);
            if (depotRecord == null)
            {
                return HttpNotFound();
            }
            return View(depotRecord);
        }

        // POST: DepotRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,subAdmin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                DepotRecord depotRecord = await db.DepotRecords.FindAsync(id);
                db.DepotRecords.Remove(depotRecord);
                await db.SaveChangesAsync();

                var controller = DependencyResolver.Current.GetService<LogRecordsController>();
                controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
                controller.Create(
                    new LogRecord
                    {
                        UserId = this.User.Identity.GetUserId(),
                        DateOfRecord = DateTime.Now,
                        Action = LogAction.SLETTING,
                        InfoMessage = string.Format(
                                                    "SLETTING. INFO: Utstyr-ID: {0}; Type-ID: {1}; Utløpsdato: {2}; Antall opprinnelig: {3}; Antall igjen: {4}",
                                                    depotRecord.Id,
                                                    depotRecord.EquipmentCodeId,
                                                    (depotRecord.ExpirationDate != null) ? depotRecord.ExpirationDate.Value.ToShortDateString() : "Ingen",
                                                    depotRecord.QuantityOriginal
                                                    , depotRecord.QuantityLeft
                                                    )
                    });
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

        [Authorize(Roles = "worker,subAdmin")]
        public ActionResult ReduceQuantity()
        {
            if (TempData["StatusMessage"] != null)
            {
                ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
            }
            ViewBag.dropDownList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType");
            return View();
        }

        [Authorize(Roles = "worker,subAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReduceQuantity([Bind(Include = "EquipmentCodeId,ReduceQuantity")] ReduceAmountViewModel formCollection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int quantityToReduce = formCollection.ReduceQuantity;

                    string nameAndType = db.Equipments
                                           .Where(record => record.Id == formCollection.EquipmentCodeId)
                                           .FirstOrDefault()
                                           .NameAndType;
                    if (formCollection != null)
                    {
                        var depotRecords = db.DepotRecords
                                             .Where(record => record.EquipmentCodeId == formCollection.EquipmentCodeId)
                                             .ToList()
                                             // workers can ONLY use equipment with valid expiration date. Discard expired equipment:
                                             .Where(record => record.ExpirationDate >= DateTime.Today || record.ExpirationDate == null)
                                             .OrderBy(record => record.ExpirationDate);

                        if (depotRecords.Count() == 0)
                        {
                            ModelState.AddModelError(string.Empty, string.Format("Kan ikke redusere {0} med {1} enheter fordi {0} med gyldig utløpsdato ikke finnes på lageret",
                                                                    nameAndType,
                                                                    quantityToReduce
                                                                    ));
                            ViewBag.dropDownList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType");
                            return View(formCollection);
                        }
                        int totalQuantity = 0;
                        foreach (var record in depotRecords)
                        {
                            totalQuantity += record.QuantityLeft;
                        }
                        if (totalQuantity < quantityToReduce)
                        {
                            ModelState.AddModelError(string.Empty, string.Format("Kan ikke redusere {0} med {1} enheter fordi det er kun {2} enheter med gyldig utløpsdato på lageret",
                                                                    nameAndType,
                                                                    quantityToReduce,
                                                                    totalQuantity));
                            ViewBag.dropDownList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType");
                            return View(formCollection);
                        }

                        int totalQuantityLeft = totalQuantity - quantityToReduce;

                        //could also use DepotRecord/Edit here... 
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
                        string userId = User.Identity.GetUserId();
                        //could do it like this:
                        //(new LogRecordsController())
                        //But then newly created controller would miss the essential controller info in MVC like User, HttpContext, Request , Server etc:
                        ////https://stackoverflow.com/questions/16870413/how-to-call-another-controller-action-from-a-controller-in-mvc

                        //if you want to invoke method from another controller, this is the way:
                        var controller = DependencyResolver.Current.GetService<LogRecordsController>();
                        controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
                        controller.Create(
                            new LogRecord
                            {
                                UserId = userId,
                                DateOfRecord = DateTime.Now,
                                Action = LogAction.FORBRUK,
                                InfoMessage = string.Format(
                                                            "{0} ble redusert med {1} enheter. Antall igjen: {2}",
                                                            nameAndType,
                                                            formCollection.ReduceQuantity,
                                                            totalQuantityLeft
                                                            )
                            });
                        await db.SaveChangesAsync();
                        //ViewBag is not contained when calling to new action. Must use TempData to transfer data between controllers
                        //see https://stackoverflow.com/questions/14497711/set-viewbag-before-redirect
                        TempData["StatusMessage"] = string.Format(
                                                            "Suksess! {0} ble redusert med {1} enheter. Antall igjen: {2}",
                                                            nameAndType,
                                                            formCollection.ReduceQuantity,
                                                            totalQuantityLeft
                                                            );
                        return RedirectToAction("ReduceQuantity");
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Noe gikk galt. Vennligst prøv igjen.");
                    ViewBag.dropDownList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType");
                    return View(formCollection);
                }
            }
            ViewBag.dropDownList = new SelectList(db.Equipments.OrderBy(r => r.NameAndType), "Id", "NameAndType");
            return View(formCollection);
        }
        
    }

}


