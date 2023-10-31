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
    public class MATHANGController : Controller
    {
        private GiaoDichEntities db = new GiaoDichEntities();

        // GET: MATHANG
        public ActionResult Index()
        {
            return View(db.MAT_HANG.ToList());
        }

        // GET: MATHANG/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MAT_HANG mAT_HANG = db.MAT_HANG.Find(id);
            if (mAT_HANG == null)
            {
                return HttpNotFound();
            }
            return View(mAT_HANG);
        }

        // GET: MATHANG/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MATHANG/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MA_MAT_HANG,TEN_MAT_HANG,DON_GIA")] MAT_HANG mAT_HANG)
        {
            if (ModelState.IsValid)
            {
                db.MAT_HANG.Add(mAT_HANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mAT_HANG);
        }

        // GET: MATHANG/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MAT_HANG mAT_HANG = db.MAT_HANG.Find(id);
            if (mAT_HANG == null)
            {
                return HttpNotFound();
            }
            return View(mAT_HANG);
        }

        // POST: MATHANG/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_MAT_HANG,TEN_MAT_HANG,DON_GIA")] MAT_HANG mAT_HANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mAT_HANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mAT_HANG);
        }

        // GET: MATHANG/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MAT_HANG mAT_HANG = db.MAT_HANG.Find(id);
            if (mAT_HANG == null)
            {
                return HttpNotFound();
            }
            return View(mAT_HANG);
        }

        // POST: MATHANG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MAT_HANG mAT_HANG = db.MAT_HANG.Find(id);
            db.MAT_HANG.Remove(mAT_HANG);
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
