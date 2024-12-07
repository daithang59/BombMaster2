using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Security.Cryptography.X509Certificates;


namespace DangKy_FirebaseDB
{
    public partial class DangKy : Form
    {
        public DangKy()
        {
            InitializeComponent();
        }
        IFirebaseConfig ifc = new FirebaseConfig
        {
            AuthSecret = "ptadAFZjKIegVxEFzWhRrhn5VUj0qbWM0upbVKEa",
            BasePath = "https://bombmaster-14f3a-default-rtdb.asia-southeast1.firebasedatabase.app"
        };
        IFirebaseClient Client;
        public bool checkAccount(string ac)
        {
            return Regex.IsMatch(ac, @"^[a-zA-Z0-9]{6,24}$");
        }
        public bool checkPassword(string pw)
        {
            return Regex.IsMatch(pw, @"^[a-zA-Z0-9!@#$%^&*()_+]{6,24}$");
        }
        public bool checkEmail(string em)
        {
            return Regex.IsMatch(em, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        private async void bt_registry_Click(object sender, EventArgs e)
        {
            string tentk = tb_username.Text;
            string matkhau = tb_password.Text;
            string email = tb_email.Text;

            if (!checkAccount(tentk))
            {
                MessageBox.Show("Tên tài khoản không hợp lệ. Vui lòng nhập tên tài khoản dài 6-24 kí tự, với các kí tự chữ và số, hoa và thường", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!checkPassword(matkhau))
            {
                MessageBox.Show("Mật khẩu không hợp lệ. Vui lòng nhập mật khẩu dài 6-24 kí tự, với các kí tự chữ và số, hoa và thường", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tb_confirmpassword.Text != matkhau)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!checkEmail(email))
            {
                MessageBox.Show("Email không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the username already exists in Firebase
            FirebaseResponse usernameResponse = await Client.GetAsync("Users/" + tentk);
            if (usernameResponse.Body != "null")
            {
                MessageBox.Show("Tên tài khoản này đã tồn tại, vui lòng chọn tên tài khoản khác!", "Thông báo", MessageBoxButtons.RetryCancel);
                return;
            }

            // Check if the email is already registered in Firebase
            FirebaseResponse emailResponse = await Client.GetAsync("Users");
            var users = emailResponse.ResultAs<Dictionary<string, Register>>();
            if (users != null && users.Values.Any(user => user.Email == email))
            {
                MessageBox.Show("Email này đã được đăng kí, vui lòng đăng kí Email khác.", "Thông báo", MessageBoxButtons.RetryCancel);
                return;
            }

            try
            {
                CreateUser();
                MessageBox.Show("Đăng kí thành công, chúc bạn có một trải nghiệm vui vẻ!", "Thông báo", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi trong quá trình đăng kí. Vui lòng thử lại sau.", "Thông báo", MessageBoxButtons.RetryCancel);
            }
        }
        private async void CreateUser()
        {
           var data = new Register
           {
               Username = tb_username.Text,
               Password = tb_password.Text,
               ConfirmPassword = tb_confirmpassword.Text,
               Email = tb_email.Text
           };
            FirebaseResponse firebaseResponse = await Client.SetAsync("Users/" + tb_username.Text, data);
        }
        private void DangKy_Load(object sender, EventArgs e)
        {
            try
            {
                Client = new FireSharp.FirebaseClient(ifc);
                if(Client != null)
                {
                    this.CenterToScreen();
                }
            }
            catch
            {
                MessageBox.Show("Connection Fail.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void bt_hidepw_Click(object sender, EventArgs e)
        {
            if(tb_password.PasswordChar == '\0')
            {
                tb_password.PasswordChar = '*';
                bt_showpw.BringToFront();
        }
        }
        private void bt_showpw_Click(object sender, EventArgs e)
        {
            if (tb_password.PasswordChar == '*')
            {
                tb_password.PasswordChar = '\0';
                bt_hidepw.BringToFront();
            }
        }

        private void bt_hideConfirmpw_Click(object sender, EventArgs e)
        {
            if (tb_confirmpassword.PasswordChar == '\0')
            {
                tb_confirmpassword.PasswordChar = '*';
                bt_showConfirmpw.BringToFront();
            }
        }

        private void bt_showConfirmpw_Click(object sender, EventArgs e)
        {
            if(tb_confirmpassword.PasswordChar == '*')
            {
                tb_confirmpassword.PasswordChar = '\0';
                bt_hideConfirmpw.BringToFront();
            }
        }
    }
}
