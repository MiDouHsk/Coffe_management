using QuanLyQuanCafe.BLL;
using QuanLyQuanCafe.DAL;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        

        public Account loginAccount;

        public fAdmin()
        {
            InitializeComponent();
            LoadData();
        }

        #region methods

        void LoadData()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
           

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadAccount();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddFoodBinding();
            AddAccountBinding();
            dtgvTable.DataSource = BLL_Table.Instance.GetListTable();
            dtgvCategory.DataSource = BLL_Category.Instance.GetListCategory();

            AddTableBinding();
            AddCategoryBiding();
        }

        
        void LoadAccount()
        {
            accountList.DataSource = BLL_Account.Instance.GetListAccount();
        }
        void AddFoodBinding()
        {
            // Xóa bỏ liên kết dữ liệu hiện tại trước khi thêm liên kết mới
            txbFoodName.DataBindings.Clear();
            txbFoodID.DataBindings.Clear();
            nmFoodPrice.DataBindings.Clear();

            // Thêm liên kết dữ liệu mới
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        void AddTableBinding()
        {
            // Xóa các binding cũ để tránh xung đột
            txbTableName.DataBindings.Clear();
            cbTableStatus.DataBindings.Clear();
            textBox3.DataBindings.Clear();


            // Thiết lập các binding mới từ DataSource của DataGridView
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            textBox3.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));

            cbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Status", true, DataSourceUpdateMode.Never));
        }
        void AddCategoryBiding()
        {
            // Xóa các binding cũ để tránh xung đột
            textBox2.DataBindings.Clear();
            txbCategoryID.DataBindings.Clear();



            // Thiết lập các binding mới từ DataSource của DataGridView
            textBox2.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
        }

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Clear();
            txbDisplayName.DataBindings.Clear();
            numericUpDown1.DataBindings.Clear();

            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            numericUpDown1.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }



        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BLL_Bill.Instance.GetBillListByDate(checkIn, checkOut);
        }

        

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = BLL_Category.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void LoadListFood()
        {
            foodList.DataSource = BLL_Food.Instance.GetListFood();
        }
        void LoadListTable()
        {
            dtgvTable.DataSource = BLL_Table.Instance.GetListTable();
        }
        void LoadCategory()
        {
            dtgvCategory.DataSource = BLL_Category.Instance.GetListCategory();
        }


        void AddAccount(string userName, string displayName, int type)
        {
            if (BLL_Account.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (BLL_Account.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn chứ");
                return;
            }
            if (BLL_Account.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (BLL_Account.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }
        #endregion

        #region events
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)numericUpDown1.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)numericUpDown1.Value;

            EditAccount(userName, displayName, type);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            ResetPass(userName);
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = BLL_Food.Instance.SearchFoodByName(txbSearchFoodName.Text);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as FoodCategory).id;
            float price = (float)nmFoodPrice.Value;

            if (BLL_Food.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);
            string name = txbFoodName.Text;

            // Chuyển đổi giá trị được chọn từ ComboBox sang đối tượng FoodCategory
            FoodCategory selectedCategory = cbFoodCategory.SelectedItem as FoodCategory;

            // Kiểm tra xem có đối tượng được chọn không
            if (selectedCategory != null)
            {
                // Lấy ID từ đối tượng FoodCategory đã chọn
                int categoryID = selectedCategory.id;

                float price = (float)nmFoodPrice.Value;

                if (BLL_Food.Instance.UpdateFood(id, name, categoryID, price))
                {
                    MessageBox.Show("Sửa món thành công");
                    LoadListFood();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi sửa thức ăn");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một danh mục thức ăn");
            }
        }



        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (BLL_Food.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }


        private void btnShow_Click(object sender, EventArgs e)
        {
            // Đây là sự kiện xem chung, bạn cần kiểm tra xem nút "Xem" nào được nhấn bằng cách kiểm tra ID hoặc Text của nút.
            Button btn = sender as Button;
            if (btn != null)
            {
                // Kiểm tra ID của nút để xác định nó là nút "Xem Tài Khoản" hay "Xem Món".
                if (btn.Name == "btnShowAccount")
                {
                    // Nếu nút là "Xem Tài Khoản", gọi phương thức LoadAccount().
                    LoadAccount();
                }
                else if (btn.Name == "btnShowFood")
                {
                    // Nếu nút là "Xem Món", gọi phương thức LoadListFood().
                    LoadListFood();
                }
            }
        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BLL_Bill.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            int lastPage = sumRecord / 10;
            if (sumRecord % 10 != 0)
                lastPage++;

            txbPageBill.Text = lastPage.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BLL_Bill.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            if (page > 1)
                page--;

            txbPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BLL_Bill.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            int lastPage = sumRecord / 10;
            if (sumRecord % 10 != 0)
                lastPage++;

            if (page < lastPage)
                page++;

            txbPageBill.Text = page.ToString();
        }
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }


        private void fAdmin_Load(object sender, EventArgs e)
        {
            LoadData();
            this.USP_GetListBillByDateForReportTableAdapter.Fill(this.QuanLyQuanCafeDataSet2.USP_GetListBillByDateForReport, dtpkFromDate.Value, dtpkToDate.Value);
            this.rpViewer.RefreshReport();
        }

        #endregion

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            string status = cbTableStatus.Text;

            if (BLL_Table.Instance.InsertTableFood(name, status))
            {
                MessageBox.Show("Thêm món bàn thành công");
                LoadListTable();
            }
            else
            {
                MessageBox.Show("Thêm bàn thất bại");
            }
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các điều khiển trên giao diện
            int idTable = Convert.ToInt32(textBox3.Text);
            string name = txbTableName.Text;
            string status = cbTableStatus.Text;

            // Gọi hàm cập nhật thông tin bàn từ BLL_Table
            if (BLL_Table.Instance.UpdateTableFood(idTable, name, status))
            {
                MessageBox.Show("Cập nhật thông tin bàn thành công");
                LoadListTable(); // Tải lại danh sách bàn sau khi cập nhật
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin bàn thất bại");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            // Lấy ID của bàn từ điều khiển trên giao diện
            int idTable = Convert.ToInt32(textBox3.Text);

            // Gọi hàm xóa bàn từ BLL_Table
            if (BLL_Table.Instance.DeleteTableFood(idTable))
            {
                MessageBox.Show("Xóa bàn thành công");
                LoadListTable(); // Tải lại danh sách bàn sau khi xóa
            }
            else
            {
                MessageBox.Show("Xóa bàn thất bại");
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string categoryName = textBox2.Text; // Lấy tên loại thức ăn từ TextBox hoặc điều chỉnh tương ứng

            // Kiểm tra xem đã nhập đủ thông tin loại thức ăn hay chưa
            if (!string.IsNullOrEmpty(categoryName))
            {
                // Thực hiện việc thêm loại thức ăn
                if (BLL_Category.Instance.InsertCategory(categoryName))
                {
                    MessageBox.Show("Thêm loại thức ăn thành công");
                    // Load lại danh sách loại thức ăn sau khi thêm
                    LoadCategory(); // Gọi lại phương thức để tải lại danh sách loại thức ăn
                }
                else
                {
                    MessageBox.Show("Thêm loại thức ăn thất bại");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin cho loại thức ăn");
            }
        }
    }

}