using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test_Demo_2.Models;
using OfficeOpenXml;
using System.Text;
//EPPlus



namespace Test_Demo_2.Controllers
{
    public class HOADONController : Controller
    {
        private GiaoDichEntities db = new GiaoDichEntities();

        // GET: HOADON
        public ActionResult Index()
        {
            return View(db.HOA_DON.ToList());
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
        public ActionResult Create(HOA_DON hoadon, THONG_TIN_BAN_HANG tHONG_TIN_BAN_HANG, string selectedValue)
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
                hoadon.MA_HOA_DON = maHoaDon;
                hoadon.NGAY_GIAO_DICH = DateTime.Now;
                db.HOA_DON.Add(hoadon);
                db.SaveChanges();
                TempData["selectedValue"] = selectedValue;
                return RedirectToAction("Create", "THONGTINBANHANG", new { mach = hoadon.MA_CUA_HANG, id = maHoaDon });
            }
            tHONG_TIN_BAN_HANG.MA_HOA_DON = hoadon.MA_HOA_DON;
            ViewBag.MA_CUA_HANG = new SelectList(db.CUA_HANG, "MA_CUA_HANG", "MA_CUA_HANG", hoadon.MA_CUA_HANG);
            return View(hoadon);
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
        public ActionResult Edit(HOA_DON hOA_DON)
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
        public ActionResult ExportToExcel(string id)
        {
            
            var hoaDonList = db.HOA_DON.ToList();
            var thongTinBanHangList = db.THONG_TIN_BAN_HANG.ToList();
            byte[] fileContents;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("HOA_DON");
                // Tiêu đề cột
                worksheet.Cells[1, 1].Value = "Mã hóa đơn";
                worksheet.Cells[1, 2].Value = "Mã cửa hàng";
                worksheet.Cells[1, 3].Value = "Địa chỉ";
                worksheet.Cells[1, 4].Value = "Mã mặt hàng";
                worksheet.Cells[1, 5].Value = "Ngày giao dịch";
                worksheet.Cells[1, 6].Value = "Tổng tiền";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 2].Style.Font.Bold = true;
                worksheet.Cells[1, 3].Style.Font.Bold = true;
                worksheet.Cells[1, 4].Style.Font.Bold = true;
                worksheet.Cells[1, 5].Style.Font.Bold = true;
                worksheet.Cells[1, 6].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, 6].AutoFitColumns();
                // Dữ liệu từ bảng HOA_DON
                int row = 2;
                foreach (var hoaDon in hoaDonList)
                {
                    var tongTien = (from ttbh in db.THONG_TIN_BAN_HANG
                                    join mh in db.MAT_HANG on ttbh.MA_MAT_HANG equals mh.MA_MAT_HANG
                                    where ttbh.MA_HOA_DON == hoaDon.MA_HOA_DON
                                    select ttbh.SO_LUONG_BAN_HANG * mh.DON_GIA).Sum();

                    worksheet.Cells[row, 1].Value = hoaDon.MA_HOA_DON;
                    worksheet.Cells[row, 2].Value = hoaDon.MA_CUA_HANG;
                    worksheet.Cells[row, 3].Value = hoaDon.CUA_HANG.DIA_CHI;
                    double excelDate = hoaDon.NGAY_GIAO_DICH.Value.ToOADate();
                    DateTime date = DateTime.FromOADate(excelDate);
                    worksheet.Cells[row, 5].Value = date.ToString("HH:mm dd/MM/yyyy");
                    worksheet.Cells[row, 6].Value = tongTien;
                    var tenMatHangBuilder = new StringBuilder();
                    foreach (var thongTin in thongTinBanHangList.Where(t => t.MA_HOA_DON == hoaDon.MA_HOA_DON))
                    {
                        string mamathang = thongTin.MA_MAT_HANG;
                        // Kết hợp tên mặt hàng vào chuỗi
                        tenMatHangBuilder.Append(mamathang);
                        tenMatHangBuilder.Append(", ");
                    }
                    string MatHangCombined = tenMatHangBuilder.ToString().TrimEnd(',', ' ');
                    // Thêm giá trị kết hợp vào tệp Excel
                    worksheet.Cells[row, 4].Value = MatHangCombined;
                    row++;
                }
                fileContents = package.GetAsByteArray();
            }
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Bao_cao.xlsx");
        }
    }
}
