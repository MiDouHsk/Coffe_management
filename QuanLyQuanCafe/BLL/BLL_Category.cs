using QuanLyQuanCafe.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyQuanCafe.BLL
{
    public class Category
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
    public class BLL_Category
    {
        

        private static BLL_Category instance;

        public static BLL_Category Instance
        {
            get { if (instance == null) instance = new BLL_Category(); return instance; }
            private set { instance = value; }
        }

        private BLL_Category() { }
        public bool InsertCategory(string name)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                try
                {
                    FoodCategory category = new FoodCategory();
                    category.name = name;

                    context.FoodCategories.InsertOnSubmit(category);
                    context.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool UpdateCategory(int id, string name)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                try
                {
                    var category = context.FoodCategories.FirstOrDefault(c => c.id == id);
                    if (category != null)
                    {
                        category.name = name;
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

        public bool DeleteCategory(int id)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                try
                {
                    var category = context.FoodCategories.FirstOrDefault(c => c.id == id);
                    if (category != null)
                    {
                        context.FoodCategories.DeleteOnSubmit(category);
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

        public List<FoodCategory> GetListCategory()
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                return context.FoodCategories.ToList();
            }
        }
       

    

    public FoodCategory GetCategoryByID(int id)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                return context.FoodCategories.FirstOrDefault(c => c.id == id);
            }
        }
    }
}
