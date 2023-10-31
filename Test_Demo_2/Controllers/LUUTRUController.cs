using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Test_Demo_2.Models;

namespace Test_Demo_2.Controllers
{
    public class LUUTRUController : Controller
    {
        private GiaoDichEntities db = new GiaoDichEntities();

        // GET: LUUTRU
        public ActionResult Index()
        {
            var lUU_TRU = db.LUU_TRU.Include(l => l.CUA_HANG).Include(l => l.MAT_HANG);
            return View(lUU_TRU.ToList());
        }

        // GET: LUUTRU/Details/5
        public ActionResult Details(string mach, string mamh)
        {
            if (mach == null || mamh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LUU_TRU lUU_TRU = db.LUU_TRU.Find(mach, mamh);
            if (lUU_TRU == null)
            {
                return HttpNotFound();
            }
            return View(lUU_TRU);
        }

        // GET: LUUTRU/Create
        public ActionResult Create()
        {
            ViewBag.MA_CUA_HANG = new SelectList(db.CUA_HANG, "MA_CUA_HANG", "MA_CUA_HANG");
            ViewBag.MA_MAT_HANG = new SelectList(db.MAT_HANG, "MA_MAT_HANG", "MA_MAT_HANG");
            return View();
        }

        // POST: LUUTRU/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MA_CUA_HANG,MA_MAT_HANG,SO_LUONG_MAT_HANG")] LUU_TRU lUU_TRU)
        {
            if (ModelState.IsValid)
            {
                db.LUU_TRU.Add(lUU_TRU);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MA_CUA_HANG = new SelectList(db.CUA_HANG, "MA_CUA_HANG", "MA_CUA_HANG", lUU_TRU.MA_CUA_HANG);
            ViewBag.MA_MAT_HANG = new SelectList(db.MAT_HANG, "MA_MAT_HANG", "MA_MAT_HANG", lUU_TRU.MA_MAT_HANG);
            return View(lUU_TRU);
        }

        // GET: LUUTRU/Edit/5
        public ActionResult Edit(string mach, string mamh)
        {
            if (mach == null || mamh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LUU_TRU lUU_TRU = db.LUU_TRU.Find(mach, mamh);
            if (lUU_TRU == null)
            {
                return HttpNotFound();
            }
            ViewBag.MA_CUA_HANG = new SelectList(db.CUA_HANG, "MA_CUA_HANG", "MA_CUA_HANG", lUU_TRU.MA_CUA_HANG);
            ViewBag.MA_MAT_HANG = new SelectList(db.MAT_HANG, "MA_MAT_HANG", "MA_MAT_HANG", lUU_TRU.MA_MAT_HANG);
            return View(lUU_TRU);
        }

        // POST: LUUTRU/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_CUA_HANG,MA_MAT_HANG,SO_LUONG_MAT_HANG")] LUU_TRU lUU_TRU)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lUU_TRU).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MA_CUA_HANG = new SelectList(db.CUA_HANG, "MA_CUA_HANG", "DIA_CHI", lUU_TRU.MA_CUA_HANG);
            ViewBag.MA_MAT_HANG = new SelectList(db.MAT_HANG, "MA_MAT_HANG", "TEN_MAT_HANG", lUU_TRU.MA_MAT_HANG);
            return View(lUU_TRU);
        }

        // GET: LUUTRU/Delete/5
        public ActionResult Delete(string mach, string mamh)
        {
            if (mach == null || mamh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LUU_TRU lUU_TRU = db.LUU_TRU.Find(mach, mamh);
            if (lUU_TRU == null)
            {
                return HttpNotFound();
            }
            return View(lUU_TRU);
        }

        // POST: LUUTRU/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string mach, string mamh)
        {
            LUU_TRU lUU_TRU = db.LUU_TRU.Find(mach, mamh);
            db.LUU_TRU.Remove(lUU_TRU);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}