using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test_Demo_2.Models;

namespace Test_Demo_2.Controllers
{
    public class CUAHANGController : Controller
    {
        private GiaoDichEntities db = new GiaoDichEntities();

        // GET: CUAHANG
        public ActionResult Index()
        {
            return View(db.CUA_HANG.ToList());
        }

        // GET: CUAHANG/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CUA_HANG cUA_HANG = db.CUA_HANG.Find(id);
            if (cUA_HANG == null)
            {
                return HttpNotFound();
            }
            return View(cUA_HANG);
        }

        // GET: CUAHANG/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CUAHANG/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MA_CUA_HANG,DIA_CHI,SO_DIEN_THOAI")] CUA_HANG cUA_HANG)
        {
            if (ModelState.IsValid)
            {
                db.CUA_HANG.Add(cUA_HANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cUA_HANG);
        }

        // GET: CUAHANG/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CUA_HANG cUA_HANG = db.CUA_HANG.Find(id);
            if (cUA_HANG == null)
            {
                return HttpNotFound();
            }
            return View(cUA_HANG);
        }

        // POST: CUAHANG/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_CUA_HANG,DIA_CHI,SO_DIEN_THOAI")] CUA_HANG cUA_HANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cUA_HANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cUA_HANG);
        }

        // GET: CUAHANG/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CUA_HANG cUA_HANG = db.CUA_HANG.Find(id);
            if (cUA_HANG == null)
            {
                return HttpNotFound();
            }
            return View(cUA_HANG);
        }

        // POST: CUAHANG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CUA_HANG cUA_HANG = db.CUA_HANG.Find(id);
            db.CUA_HANG.Remove(cUA_HANG);
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
