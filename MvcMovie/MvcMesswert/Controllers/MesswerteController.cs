using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcMesswert.Models;

namespace MvcMesswert.Controllers
{
    public class MesswerteController : Controller
    {
        private MesswertDBContext db = new MesswertDBContext();

        // GET: /Messwerte/
        public ActionResult Index(string hourFilter, string template, string orderBy, string groupBy)
        {

            var TempLst = new List<string>();

            var TempQry = from d in db.Messwerte
                           select d.Template;

            TempLst.AddRange(TempQry.Distinct());
            ViewBag.Template = new SelectList(TempLst);
            ViewBag.orderBy = new[]{
                            new SelectListItem() { Text = "Time", Value = "Time" },
                            new SelectListItem() { Text = "Template", Value = "Template" },
                            new SelectListItem() { Text = "Value", Value = "Value" }};
            ViewBag.groupBy = new[]{
                            new SelectListItem() { Text = "Time", Value = "Time" },
                            new SelectListItem() { Text = "Template", Value = "Template" },
                            new SelectListItem() { Text = "Value", Value = "Value" }};



            var messwerte = from m in db.Messwerte
                         select m;

            if (!String.IsNullOrEmpty(hourFilter))
            {
                int hour = int.Parse(hourFilter);
                messwerte = messwerte.Where(h => h.Time.Hour == hour);
            }

            if (!string.IsNullOrEmpty(template))
            {
                messwerte  = messwerte.Where(x => x.Template == template);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                if (orderBy == "Template")
                {
                    messwerte = messwerte.OrderBy(o => o.Template);
                }
                else if (orderBy == "Time")
                {
                    messwerte = messwerte.OrderBy(o => o.Time);
                }
                else if (orderBy == "Value")
                {
                    messwerte = messwerte.OrderBy(o => o.Value);
                }
                
            }


            MyViewModel mvm = new MyViewModel();
            if (!string.IsNullOrEmpty(groupBy))
            {
                if (groupBy == "Template")
                {
                    // Group the messuare by the template
                   //var  gm = messwerte.GroupBy(m => m.Template).Select(m => new Group<Messwert, string>{ Key = m.Key, Values = m });
                   var gm = from m in messwerte
                            group m by m.Template into t
                            select new Group<Messwert, string> { Key = t.Key, Values = t };
                   mvm.TypeString = gm;
                    return View(mvm);
                }

                else if (orderBy == "Time")
                {
                    // Group the messuare by the Time
                    //var gm = messwerte.GroupBy(m => m.Time).Select(m => new Group<Messwert, DateTime>{ Key => m.Key, Values => m });
                    var gm = from m in messwerte
                             group m by m.Time into t
                             select new Group<Messwert, DateTime> { Key = t.Key, Values = t };
                    mvm.TypeDatetime = gm;
                    return View(mvm);
                }
                else if (groupBy == "Value")
                {
                  
                    var gm = from m in messwerte
                                group m by m.Value into t
                                select new Group<Messwert, decimal> { Key = t.Key, Values = t };
                    mvm.TypeDecimal = gm;
                    return View(mvm);
                    //messwerte = messwerte.GroupBy(m => m.Value, m).Select(m => new Group<Messwert, string> { Key = m.Key, Values = m });
                }
                
            }
           
           
            mvm.TypeMesswert = messwerte;
            return View(mvm); 
            //return View(db.Messwerte.ToList());
        }

        // GET: /Messwerte/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messwert messwert = db.Messwerte.Find(id);
            if (messwert == null)
            {
                return HttpNotFound();
            }
            return View(messwert);
        }

        // GET: /Messwerte/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Messwerte/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Time,Template,Value")] Messwert messwert)
        {
            if (ModelState.IsValid)
            {
                db.Messwerte.Add(messwert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(messwert);
        }

        // GET: /Messwerte/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messwert messwert = db.Messwerte.Find(id);
            if (messwert == null)
            {
                return HttpNotFound();
            }
            return View(messwert);
        }

        // POST: /Messwerte/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Time,Template,Value")] Messwert messwert)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messwert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(messwert);
        }

        // GET: /Messwerte/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messwert messwert = db.Messwerte.Find(id);
            if (messwert == null)
            {
                return HttpNotFound();
            }
            return View(messwert);
        }

        // POST: /Messwerte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Messwert messwert = db.Messwerte.Find(id);
            db.Messwerte.Remove(messwert);
            db.SaveChanges();
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
