using QuanLyQuanCafe.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyQuanCafe.BLL
{
    public class BLL_BillInfo
    {
        private static BLL_BillInfo instance;

        public static BLL_BillInfo Instance
        {
            get { if (instance == null) instance = new BLL_BillInfo(); return BLL_BillInfo.instance; }
            private set { BLL_BillInfo.instance = value; }
        }

        private BLL_BillInfo() { }

        public void DeleteBillInfoByFoodID(int id)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                var billInfoToDelete = context.BillInfos.Where(bi => bi.idFood == id).ToList();
                context.BillInfos.DeleteAllOnSubmit(billInfoToDelete);
                context.SubmitChanges();
            }
        }

        public List<BillInfo> GetListBillInfo(int id)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                return context.BillInfos.Where(bi => bi.idBill == id).ToList();
            }
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                BillInfo billInfo = new BillInfo();
                billInfo.idBill = idBill;
                billInfo.idFood = idFood;
                billInfo.count = count;

                context.BillInfos.InsertOnSubmit(billInfo);
                context.SubmitChanges();
            }
        }
    }
}
