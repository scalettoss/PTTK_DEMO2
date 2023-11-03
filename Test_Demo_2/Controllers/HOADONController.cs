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
    public class HOADONController : Controller
    {
        private GiaoDichEntities db = new GiaoDichEntities();

        // GET: HOADON
        public ActionResult Index()
        {
            var hOA_DON = db.HOA_DON.Include(h => h.CUA_HANG);
            return View(hOA_DON.ToList());
        }
        // GET: HOADON
        // /Details/5



        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOA_DON hOA_DON = db.HOA_DON.Find(id);
            if (hOA_DON == null)
            {
                return HttpNotFound();
            }
            var tenMatHangs = (from ttbh in db.THONG_TIN_BAN_HANG
                               join mh in db.MAT_HANG on ttbh.MA_MAT_HANG equals mh.MA_MAT_HANG
                               where ttbh.MA_HOA_DON == id
                               select mh.TEN_MAT_HANG).ToList();
            ViewBag.TenMatHangs = tenMatHangs;

            var soLuong = (from ttbh in db.THONG_TIN_BAN_HANG
                           where ttbh.MA_HOA_DON == id
                           select ttbh.SO_LUONG_BAN_HANG).ToList();
            ViewBag.SoLuongs = soLuong;

            var donGia = (from ttbh in db.THONG_TIN_BAN_HANG
                          join mh in db.MAT_HANG on ttbh.MA_MAT_HANG equals mh.MA_MAT_HANG
                          where ttbh.MA_HOA_DON == id
                          select mh.DON_GIA).ToList();
            ViewBag.dongia = donGia;

            var tongTien = (from ttbh in db.THONG_TIN_BAN_HANG
                            join mh in db.MAT_HANG on ttbh.MA_MAT_HANG equals mh.MA_MAT_HANG
                            where ttbh.MA_HOA_DON == id
                            select ttbh.SO_LUONG_BAN_HANG * mh.DON_GIA).Sum();
            ViewBag.tthd = tongTien;
            TempData["tongtien"] = tongTien;
            var hoaDon = db.HOA_DON.FirstOrDefault(hd => hd.MA_HOA_DON == id);
            if (hoaDon != null)
            {
                hoaDon.TONG_TIEN = tongTien;
                db.SaveChanges();
            }
            return View(hOA_DON);
        }





        // GET: HOADON/Create
        public ActionResult Create()
        {
            ViewBag.MA_CUA_HANG = new SelectList(db.CUA_HANG, "MA_CUA_HANG", "MA_CUA_HANG");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HOA_DON hOA_DON, THONG_TIN_BAN_HANG tHONG_TIN_BAN_HANG, string selectedValue)
        {
            if (ModelState.IsValid)
            {
                string maHoaDon = string.Empty;
                int currentNumber = db.HOA_DON.Count() + 1;
                do
                {
                    maHoaDon = string.Format("HD{0:00}", currentNumber);
                    currentNumber++;
                } while (db.HOA_DON.Any(hd => hd.MA_HOA_DON == maHoaDon));
                hOA_DON.MA_HOA_DON = maHoaDon;
                hOA_DON.NGAY_GIAO_DICH = DateTime.Now;
                db.HOA_DON.Add(hOA_DON);
                db.SaveChanges();
                TempData["selectedValue"] = selectedValue;
                return RedirectToAction("Create", "THONGTINBANHANG", new { mach = hOA_DON.MA_CUA_HANG, id = maHoaDon });
            }
            tHONG_TIN_BAN_HANG.MA_HOA_DON = hOA_DON.MA_HOA_DON;
            ViewBag.MA_CUA_HANG = new SelectList(db.CUA_HANG, "MA_CUA_HANG", "MA_CUA_HANG", hOA_DON.MA_CUA_HANG);
            return View(hOA_DON);
        }

        // GET: HOADON/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOA_DON hOA_DON = db.HOA_DON.Find(id);
            if (hOA_DON == null)
            {
                return HttpNotFound();
            }
            ViewBag.MA_CUA_HANG = new SelectList(db.CUA_HANG, "MA_CUA_HANG", "DIA_CHI", hOA_DON.MA_CUA_HANG);
            return View(hOA_DON);
        }

        // POST: HOADON/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MA_HOA_DON,MA_CUA_HANG,TONG_TIEN,NGAY_GIAO_DICH")] HOA_DON hOA_DON)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hOA_DON).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MA_CUA_HANG = new SelectList(db.CUA_HANG, "MA_CUA_HANG", "DIA_CHI", hOA_DON.MA_CUA_HANG);
            return View(hOA_DON);
        }

        // GET: HOADON/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOA_DON hOA_DON = db.HOA_DON.Find(id);
            if (hOA_DON == null)
            {
                return HttpNotFound();
            }
            return View(hOA_DON);
        }

        // POST: HOADON/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HOA_DON hOA_DON = db.HOA_DON.Find(id);
            db.HOA_DON.Remove(hOA_DON);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
