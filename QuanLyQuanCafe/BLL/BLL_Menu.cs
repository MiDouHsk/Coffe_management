using QuanLyQuanCafe.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QuanLyQuanCafe.BLL
{
    public class Menu
    {
        // Constructor mặc định không có tham số
        public Menu()
        {
        }

        public Menu(string foodName, int count, float price, float totalPrice = 0)
        {
            this.FoodName = foodName;
            this.Count = count;
            this.Price = price;
            this.TotalPrice = totalPrice;
        }

        public Menu(DataRow row)
        {
            this.FoodName = row["Name"].ToString();
            this.Count = (int)row["count"];
            this.Price = Convert.ToSingle(row["price"]); // Convert.ToDouble thành Convert.ToSingle
            this.TotalPrice = Convert.ToSingle(row["totalPrice"]); // Convert.ToDouble thành Convert.ToSingle
        }

        private float totalPrice;

        public float TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }

        private float price;

        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        private int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        private string foodName;

        public string FoodName
        {
            get { return foodName; }
            set { foodName = value; }
        }
    }

    public class BLL_Menu
    {
        private static BLL_Menu instance;

        public static BLL_Menu Instance
        {
            get { if (instance == null) instance = new BLL_Menu(); return BLL_Menu.instance; }
            private set { BLL_Menu.instance = value; }
        }

        private BLL_Menu() { }

        public List<Menu> GetListMenuByTable(int id)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                var listMenu = (from bi in context.BillInfos
                                join b in context.Bills on bi.idBill equals b.id
                                join f in context.Foods on bi.idFood equals f.id
                                where b.status == 0 && b.idTable == id
                                select new Menu
                                {
                                    FoodName = f.name,
                                    Count = bi.count,
                                    Price = Convert.ToSingle(f.price), // Convert.ToDouble thành Convert.ToSingle
                                    TotalPrice = Convert.ToSingle(f.price * bi.count) // Convert.ToDouble thành Convert.ToSingle
                                }).ToList();
                return listMenu;
            }
        }
    }
}
