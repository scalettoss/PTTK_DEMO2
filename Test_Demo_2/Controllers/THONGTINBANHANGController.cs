using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IdentityModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Windows.Forms;
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
            ViewBag.MA_MAT_HANG = new SelectList(items, "MA_MAT_HANG", "TEN_MAT_HANG");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(THONG_TIN_BAN_HANG tHONG_TIN_BAN_HANG, System.Web.Mvc.FormCollection form , string id, int count, List<int> SoLuong)
        {
            var selectedValue = TempData["selectedValue"]?.ToString();
            var mach = selectedValue;
            if (selectedValue != null)
            {
                var maMatHangs = (from lt in db.LUU_TRU
                                  where lt.MA_CUA_HANG == mach
                                  select lt.MA_MAT_HANG).ToList();
                //select tat ca ma mat hàng của cửa hàng

                if (ModelState.IsValid)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < maMatHangs.Count && i < SoLuong.Count; i++)
                            {
                                var ttbh = new THONG_TIN_BAN_HANG();
                                ttbh.MA_MAT_HANG = maMatHangs[i];
                                ttbh.MA_HOA_DON = id;
                                ttbh.SO_LUONG_BAN_HANG = SoLuong[i];
                                var slKho = (from lt in db.LUU_TRU
                                             where lt.MA_CUA_HANG == mach && lt.MA_MAT_HANG == ttbh.MA_MAT_HANG
                                             select lt.SO_LUONG_MAT_HANG).FirstOrDefault();

                                if (slKho.HasValue && ttbh.SO_LUONG_BAN_HANG > slKho.Value)
                                {
                                    TempData["mess"] = $"KHÔNG ĐỦ SỐ LƯỢNG TRONG KHO - {ttbh.MA_MAT_HANG}";
                                    transaction.Rollback(); // Rollback transaction to undo changes
                                    return RedirectToAction("Create", "THONGTINBANHANG", new { mach = mach, id = id });
                                }
                                else
                                {
                                    db.THONG_TIN_BAN_HANG.Add(ttbh);
                                    db.SaveChanges();
                                }
                            }
                            transaction.Commit(); // Commit transaction to save changes
                            return RedirectToAction("Index", new { id = id });
                        }
                        catch (Exception)
                        {
                            transaction.Rollback(); // Rollback transaction in case of any exception
                            throw;
                        }
                    }
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            //var soLuong = db.LUU_TRU.FirstOrDefault(l => l.MA_CUA_HANG == mach && l.MA_MAT_HANG == selectedValue)?.SO_LUONG_MAT_HANG;
            ViewBag.MA_HOA_DON = new SelectList(db.HOA_DON, "MA_HOA_DON", "MA_CUA_HANG", tHONG_TIN_BAN_HANG.MA_HOA_DON);
            //ViewBag.MA_MAT_HANG = new SelectList(, "MA_MAT_HANG", "TEN_MAT_HANG", tHONG_TIN_BAN_HANG.MA_MAT_HANG);
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
            var soLuongBanHang = tHONG_TIN_BAN_HANG.SO_LUONG_BAN_HANG;
            var luuTru = db.LUU_TRU.FirstOrDefault(lt => lt.MA_MAT_HANG == mamh);
            luuTru.SO_LUONG_MAT_HANG += soLuongBanHang;
            db.THONG_TIN_BAN_HANG.Remove(tHONG_TIN_BAN_HANG);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = id });
        }





    }
}
