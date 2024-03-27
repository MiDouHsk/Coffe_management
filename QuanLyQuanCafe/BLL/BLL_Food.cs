using QuanLyQuanCafe.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyQuanCafe.BLL
{
    public class BLL_Food
    {
        private static BLL_Food instance;

        public static BLL_Food Instance
        {
            get { if (instance == null) instance = new BLL_Food(); return instance; }
            private set { instance = value; }
        }

        private BLL_Food() { }

        public List<Food> GetFoodByCategoryID(int id)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                return context.Foods.Where(f => f.idCategory == id).ToList();
            }
        }

        public List<Food> GetListFood()
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                return context.Foods.ToList();
            }
        }

        public List<Food> SearchFoodByName(string name)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                return context.Foods.Where(f => f.name.ToLower().Contains(name.ToLower())).ToList();
            }
        }

        public bool InsertFood(string name, int id, float price)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                try
                {
                    Food food = new Food();
                    food.name = name;
                    food.idCategory = id;
                    food.price = price;

                    context.Foods.InsertOnSubmit(food);
                    context.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool UpdateFood(int idFood, string name, int id, float price)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                try
                {
                    var food = context.Foods.FirstOrDefault(f => f.id == idFood);
                    if (food != null)
                    {
                        food.name = name;
                        food.idCategory = id;
                        food.price = price;
                        context.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool DeleteFood(int idFood)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                try
                {
                    var food = context.Foods.FirstOrDefault(f => f.id == idFood);
                    if (food != null)
                    {
                        context.Foods.DeleteOnSubmit(food);
                        context.SubmitChanges();
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
