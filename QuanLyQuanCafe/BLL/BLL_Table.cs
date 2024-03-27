using QuanLyQuanCafe.DAL;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyQuanCafe.BLL
{
    public class Table
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }

    public class BLL_Table
    {
        private static BLL_Table instance;

        public static BLL_Table Instance
        {
            get { if (instance == null) instance = new BLL_Table(); return BLL_Table.instance; }
            private set { BLL_Table.instance = value; }
        }

        public static int TableWidth = 90;
        public static int TableHeight = 90;

        private BLL_Table() { }

        public void SwitchTable(int id1, int id2)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                var table1 = context.TableFoods.FirstOrDefault(t => t.id == id1);
                var table2 = context.TableFoods.FirstOrDefault(t => t.id == id2);

                if (table1 != null && table2 != null)
                {
                    // Chuyển đổi status từ kiểu int sang string trước khi gán
                    string tempStatus = table1.status.ToString();
                    table1.status = table2.status.ToString();
                    table2.status = tempStatus;

                    context.SubmitChanges();
                }
            }
        }

        public List<Table> LoadTableList()
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                var tableList = (from t in context.TableFoods
                                 select new Table
                                 {
                                     ID = t.id,
                                     Name = t.name,
                                     Status = t.status
                                 }).ToList();
                return tableList;
            }
        }
        public bool InsertTableFood(string name, string status)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                try
                {
                    TableFood tableFood = new TableFood();
                    tableFood.name = name;
                    tableFood.status = status;

                    context.TableFoods.InsertOnSubmit(tableFood);
                    context.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool UpdateTableFood(int idTable, string name, string status)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                try
                {
                    var tableFood = context.TableFoods.FirstOrDefault(t => t.id == idTable);
                    if (tableFood != null)
                    {
                        tableFood.name = name;
                        tableFood.status = status;
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

        public bool DeleteTableFood(int idTable)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                try
                {
                    var tableFood = context.TableFoods.FirstOrDefault(t => t.id == idTable);
                    if (tableFood != null)
                    {
                        context.TableFoods.DeleteOnSubmit(tableFood);
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
        public List<Table> GetListTable()
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                // Lấy danh sách các bàn từ database và chuyển đổi chúng thành danh sách các đối tượng Table
                var tables = context.TableFoods.Select(t => new Table
                {
                    ID = t.id,
                    Name = t.name,
                    Status = t.status
                }).ToList();

                return tables;
            }
        }


    }
}
