using QuanLyQuanCafe.DAL;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.BLL
{
    public class BLL_Account
    {
        private static BLL_Account instance;

        public static BLL_Account Instance
        {
            get { if (instance == null) instance = new BLL_Account(); return instance; }
            private set { instance = value; }
        }

        private BLL_Account() { }

        public bool Login(string userName, string passWord)
        {
            using (MD5 md5 = MD5.Create())
            {
                // Mã hóa mật khẩu trực tiếp từ chuỗi sử dụng MD5
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(passWord));

                // Chuyển đổi mảng byte thành chuỗi hexa
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                string hashedPassword = stringBuilder.ToString();

                // Kiểm tra tên người dùng và mật khẩu đã mã hóa có trong cơ sở dữ liệu hay không
                using (var context = new QuanLyQuanCafeDataContext())
                {
                    var result = context.USP_Login(userName, hashedPassword).ToList();
                    return result.Any();
                }
            }
        }


        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                int result = context.USP_UpdateAccount(userName, displayName, pass, newPass);
                return result > 0;
            }
        }

        public List<Account> GetListAccount()
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                return context.Accounts.ToList();
            }
        }

        public Account GetAccountByUserName(string userName)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                var account = context.Accounts.FirstOrDefault(a => a.UserName == userName);
                return account;
            }
        }

        public bool InsertAccount(string name, string displayName, int type)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                Account newAccount = new Account
                {
                    UserName = name,
                    DisplayName = displayName,
                    Type = type,
                    PassWord = "698d51a19d8a121ce581499d7b701668" // Default password
                };

                context.Accounts.InsertOnSubmit(newAccount);
                context.SubmitChanges();
                return true;
            }
        }

        public bool UpdateAccount(string name, string displayName, int type)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                var accountToUpdate = context.Accounts.FirstOrDefault(acc => acc.UserName == name);
                if (accountToUpdate != null)
                {
                    accountToUpdate.DisplayName = displayName;
                    accountToUpdate.Type = type;
                    context.SubmitChanges();
                    return true;
                }
                return false;
            }
        }

        public bool DeleteAccount(string name)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                var accountToDelete = context.Accounts.FirstOrDefault(acc => acc.UserName == name);
                if (accountToDelete != null)
                {
                    context.Accounts.DeleteOnSubmit(accountToDelete);
                    context.SubmitChanges();
                    return true;
                }
                return false;
            }
        }

        public bool ResetPassword(string name)
        {
            using (var context = new QuanLyQuanCafeDataContext())
            {
                var accountToUpdate = context.Accounts.FirstOrDefault(acc => acc.UserName == name);
                if (accountToUpdate != null)
                {
                    accountToUpdate.PassWord = "698d51a19d8a121ce581499d7b701668"; // Default password
                    context.SubmitChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
