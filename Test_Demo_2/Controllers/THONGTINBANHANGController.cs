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
    public class THONGTINBANHANGController : Controller
    {
        private GiaoDichEntities db = new GiaoDichEntities();
        public ActionResult Index(string id)
        {
            var tHONG_TIN_BAN_HANG = db.THONG_TIN_BAN_HANG
            .Include(t => t.HOA_DON)
            .Include(t => t.MAT_HANG)
            .Where(t => t.HOA_DON.MA_HOA_DON == id);
            return View(tHONG_TIN_BAN_HANG.ToList());
        }
        public ActionResult Details(string mamh, string mahd)
        {
            if (mahd == null && mamh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONG_TIN_BAN_HANG tHONG_TIN_BAN_HANG = db.THONG_TIN_BAN_HANG.Find(mamh, mahd);
            if (tHONG_TIN_BAN_HANG == null)
            {
                return HttpNotFound();
            }
            return View(tHONG_TIN_BAN_HANG);
        }

        public ActionResult Create(string mach)
        {
            var items = (from lt in db.LUU_TRU
                         join mh in db.MAT_HANG on lt.MA_MAT_HANG equals mh.MA_MAT_HANG
                         where lt.MA_CUA_HANG == mach
                         select new
                         {
                             lt.MA_MAT_HANG,
                             mh.TEN_MAT_HANG
                         }).ToList();
            items.Insert(0, new { MA_MAT_HANG = "", TEN_MAT_HANG = "Chọn mặt hàng" });
            ViewBag.MA_MAT_HANG = new SelectList(items, "MA_MAT_HANG", "TEN_MAT_HANG");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(THONG_TIN_BAN_HANG tHONG_TIN_BAN_HANG, string id, string mach, LUU_TRU Luu, string selectedValue)
        {
            var soLuong = db.LUU_TRU.FirstOrDefault(l => l.MA_CUA_HANG == mach && l.MA_MAT_HANG == selectedValue)?.SO_LUONG_MAT_HANG;
            var items = (from lt in db.LUU_TRU
                         join mh in db.MAT_HANG on lt.MA_MAT_HANG equals mh.MA_MAT_HANG
                         where lt.MA_CUA_HANG == mach
                         select new
                         {
                             lt.MA_MAT_HANG,
                             mh.TEN_MAT_HANG
                         }).ToList();
            items.Insert(0, new { MA_MAT_HANG = "", TEN_MAT_HANG = "Chọn mặt hàng" });
            if (ModelState.IsValid)
            {
                if (soLuong >= tHONG_TIN_BAN_HANG.SO_LUONG_BAN_HANG)
                {
                    tHONG_TIN_BAN_HANG.MA_HOA_DON = id;
                    db.THONG_TIN_BAN_HANG.Add(tHONG_TIN_BAN_HANG);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = id });
                }
                else
                {
                    ModelState.AddModelError("", "Không đủ số lượng trong kho");
                }
            }

            ViewBag.MA_HOA_DON = new SelectList(db.HOA_DON, "MA_HOA_DON", "MA_CUA_HANG", tHONG_TIN_BAN_HANG.MA_HOA_DON);
            ViewBag.MA_MAT_HANG = new SelectList(items, "MA_MAT_HANG", "TEN_MAT_HANG", tHONG_TIN_BAN_HANG.MA_MAT_HANG);
            return View(tHONG_TIN_BAN_HANG);
        }


        public ActionResult Edit(string mamh, string mahd, string mach)
        {
            if (mahd == null && mamh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONG_TIN_BAN_HANG tHONG_TIN_BAN_HANG = db.THONG_TIN_BAN_HANG.Find(mamh, mahd);
            if (tHONG_TIN_BAN_HANG == null)
            {
                return HttpNotFound();
            }
            var items = (from lt in db.LUU_TRU
                         join mh in db.MAT_HANG on lt.MA_MAT_HANG equals mh.MA_MAT_HANG
                         where lt.MA_CUA_HANG == mach
                         select new
                         {
                             lt.MA_MAT_HANG,
                             mh.TEN_MAT_HANG
                         }).ToList();
            items.Insert(0, new { MA_MAT_HANG = "", TEN_MAT_HANG = "Chọn mặt hàng" });
            ViewBag.MA_MAT_HANG = new SelectList(items, "MA_MAT_HANG", "TEN_MAT_HANG", tHONG_TIN_BAN_HANG.MA_MAT_HANG);
            ViewBag.MA_HOA_DON = new SelectList(db.HOA_DON, "MA_HOA_DON", "MA_CUA_HANG", tHONG_TIN_BAN_HANG.MA_HOA_DON);
            return View(tHONG_TIN_BAN_HANG);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_MAT_HANG,MA_HOA_DON,SO_LUONG_BAN_HANG")] THONG_TIN_BAN_HANG tHONG_TIN_BAN_HANG, string id, string mach, string selectedValue)
        {
            var items = (from lt in db.LUU_TRU
                         join mh in db.MAT_HANG on lt.MA_MAT_HANG equals mh.MA_MAT_HANG
                         where lt.MA_CUA_HANG == mach
                         select new
                         {
                             lt.MA_MAT_HANG,
                             mh.TEN_MAT_HANG
                         }).ToList();
            items.Insert(0, new { MA_MAT_HANG = "", TEN_MAT_HANG = "Chọn mặt hàng" });
            var soLuong = db.LUU_TRU.FirstOrDefault(l => l.MA_CUA_HANG == mach && l.MA_MAT_HANG == selectedValue)?.SO_LUONG_MAT_HANG;
            if (ModelState.IsValid)
            {
                if (soLuong >= tHONG_TIN_BAN_HANG.SO_LUONG_BAN_HANG)
                {
                    db.Entry(tHONG_TIN_BAN_HANG).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = id });
                }
                else
                {
                    ModelState.AddModelError("", "Không đủ số lượng trong kho");
                }
            }
            ViewBag.MA_HOA_DON = new SelectList(db.HOA_DON, "MA_HOA_DON", "MA_CUA_HANG", tHONG_TIN_BAN_HANG.MA_HOA_DON);
            ViewBag.MA_MAT_HANG = new SelectList(items, "MA_MAT_HANG", "TEN_MAT_HANG", tHONG_TIN_BAN_HANG.MA_MAT_HANG);
            return View(tHONG_TIN_BAN_HANG);
        }

        public ActionResult Delete(string mamh, string mahd)
        {
            if (mamh == null && mahd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONG_TIN_BAN_HANG tHONG_TIN_BAN_HANG = db.THONG_TIN_BAN_HANG.Find(mamh, mahd);
            if (tHONG_TIN_BAN_HANG == null)
            {
                return HttpNotFound();
            }
            return View(tHONG_TIN_BAN_HANG);
        }

        // POST: THONGTINBANHANG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string mamh, string mahd, string id)
        {
            THONG_TIN_BAN_HANG tHONG_TIN_BAN_HANG = db.THONG_TIN_BAN_HANG.Find(mamh, mahd);
            db.THONG_TIN_BAN_HANG.Remove(tHONG_TIN_BAN_HANG);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = id });
        }

    }
}
