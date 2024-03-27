using QuanLyQuanCafe.BLL;
using QuanLyQuanCafe.DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
       
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        public fTableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbSwitchTable);
        }

        private void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }
        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.InsertTable += f_InsertTable;
            f.DeleteTable += f_DeleteTable;
            f.UpdateTable += f_UpdateTable;
            f.ShowDialog();
        }
        private void LoadCategory()
        {
            List<FoodCategory> listCategory = BLL_Category.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        private void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = BLL_Food.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        private void LoadTable()
        {
            flpTable.Controls.Clear();

            List<Table> tableList = BLL_Table.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = BLL_Table.TableWidth, Height = BLL_Table.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    case "Đã đặt":
                        btn.BackColor = Color.LightPink;
                        break;
                    case "Có người":
                        btn.BackColor = Color.Yellow; // Ví dụ: màu vàng cho trạng thái "Có người"
                        break;
                    default:
                        btn.BackColor = Color.Gray; // Mặc định, có thể là màu xám cho trạng thái không xác định
                        break;
                }

                flpTable.Controls.Add(btn);
            }
        }


        void ShowBill(int id)
        {
            lsvBill.Items.Clear();

            var listBillInfo = BLL_Menu.Instance.GetListMenuByTable(id);

            float totalPrice = 0;

            foreach (var item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName);
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }

            CultureInfo culture = new CultureInfo("vi-VN");
            txbTotalPrice.Text = totalPrice.ToString("c", culture);
        }




        private void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = BLL_Table.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        private void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            int idBill = BLL_Bill.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).id;
            int count = (int)nmFoodCount.Value;

            if (idBill == -1)
            {
                BLL_Bill.Instance.InsertBill(table.ID);
                BLL_BillInfo.Instance.InsertBillInfo(BLL_Bill.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BLL_BillInfo.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);

            LoadTable();
        }



        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BLL_Bill.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDisCount.Value;

            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\nTổng tiền - (Tổng tiền / 100) x Giảm giá\n=> {1} - ({1} / 100) x {2} = {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BLL_Bill.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(table.ID);

                    LoadTable();
                }
            }
        }





        private void btnSwitchTable_Click(object sender, EventArgs e)
        {

            int id1 = (lsvBill.Tag as Table).ID;

            int id2 = (cbSwitchTable.SelectedItem as Table).ID;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                BLL_Table.Instance.SwitchTable(id1, id2);

                LoadTable();
            }
        }



        private void f_UpdateFood(object sender, EventArgs e)
        {
            LoadCategory();
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void f_DeleteFood(object sender, EventArgs e)
        {
            LoadCategory();
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        void f_InsertFood(object sender, EventArgs e)
        {
            if (cbCategory.SelectedItem != null)
            {
                int categoryId = (cbCategory.SelectedItem as FoodCategory).id;

                LoadFoodListByCategoryID(categoryId);

                if (lsvBill.Tag != null && lsvBill.Tag is Table)
                {
                    Table table = lsvBill.Tag as Table;
                    ShowBill(table.ID);
                }
            }
        }
        private void f_UpdateTable(object sender, EventArgs e)
        {
            LoadTable(); // Cập nhật danh sách bàn
        }

        private void f_DeleteTable(object sender, EventArgs e)
        {
            LoadTable(); // Cập nhật danh sách bàn
        }

        private void f_InsertTable(object sender, EventArgs e)
        {
            LoadTable(); // Cập nhật danh sách bàn
        }


        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = (cbCategory.SelectedItem as FoodCategory).id;
            LoadFoodListByCategoryID(id);
        }

        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCheckOut_Click(this, new EventArgs());
        }

        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddFood_Click(this, new EventArgs());
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        private void f_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
    }
}