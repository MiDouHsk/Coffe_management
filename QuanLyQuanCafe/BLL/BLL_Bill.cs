using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using QuanLyQuanCafe.DAL;

namespace QuanLyQuanCafe.BLL
{
    public class BLL_Bill
    {
        private static BLL_Bill instance;

        public static BLL_Bill Instance
        {
            get { if (instance == null) instance = new BLL_Bill(); return instance; }
            private set { instance = value; }
        }

        private BLL_Bill() { }

        public int GetUncheckBillIDByTableID(int id)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                var uncheckBill = context.Bills.FirstOrDefault(b => b.idTable == id && b.status == 0);
                return uncheckBill != null ? uncheckBill.id : -1;
            }
        }

        public void CheckOut(int id, int discount, float totalPrice)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                var bill = context.Bills.FirstOrDefault(b => b.id == id);
                if (bill != null)
                {
                    bill.DateCheckOut = DateTime.Now;
                    bill.status = 1;
                    bill.discount = discount;
                    bill.totalPrice = totalPrice;
                    context.SubmitChanges();
                }
            }
        }

        public void InsertBill(int id)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                context.USP_InsertBill(id);
            }
        }

        public List<USP_GetListBillByDateResult> GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                return context.USP_GetListBillByDate(checkIn, checkOut).ToList();
            }
        }

        public List<USP_GetListBillByDateAndPageResult> GetBillListByDateAndPage(DateTime checkIn, DateTime checkOut, int pageNum)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                return context.USP_GetListBillByDateAndPage(checkIn, checkOut, pageNum).ToList();
            }
        }

        public int GetNumBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                var result = context.USP_GetNumBillByDate(checkIn, checkOut).FirstOrDefault();
                return result != null ? result.Column1 ?? 0 : 0;
            }
        }



        public int GetMaxIDBill()
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                return context.Bills.Max(b => b.id);
            }
        }
    }
}
