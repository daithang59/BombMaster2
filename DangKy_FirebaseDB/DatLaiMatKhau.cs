using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp;

namespace DangKy_FirebaseDB
{
    public partial class DatLaiMatKhau : Form
    {
        private IFirebaseClient Client;
        private string userName;

        public DatLaiMatKhau(string email, string username)
        {
            InitializeComponent();
            Client = new FirebaseClient(ifc);
            userName = username;
        }

        IFirebaseConfig ifc = new FirebaseConfig
        {
            AuthSecret = "ptadAFZjKIegVxEFzWhRrhn5VUj0qbWM0upbVKEa",
            BasePath = "https://bombmaster-14f3a-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

        private async void bt_changepw_Click(object sender, EventArgs e)
        {
            string newPassword = tb_password.Text;
            string confirmPassword = tb_confirmpw.Text;

            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await UpdatePasswordInFirebase(userName, newPassword);
            MessageBox.Show("Mật khẩu đã được cập nhật thành công.", "Thông báo", MessageBoxButtons.OK);
            DangNhap dangNhap = new DangNhap();
            dangNhap.Show();
            this.Hide();
        }

        private async Task UpdatePasswordInFirebase(string username, string newPassword)
        {
            var path = $"Users/{username}/Password";
            await Client.SetAsync(path, newPassword);
        }

        private void bt_showpw_Click(object sender, EventArgs e)
        {
            if(tb_password.PasswordChar == '*')
            {
                tb_password.PasswordChar = '\0';
                bt_hidepw.BringToFront();
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

        private void bt_showConfirmpw_Click(object sender, EventArgs e)
        {
            if(tb_confirmpw.PasswordChar == '*')
            {
                tb_confirmpw.PasswordChar = '\0';
                bt_hideConfirmpw.BringToFront();
            }
        }

        private void bt_hideConfirmpw_Click(object sender, EventArgs e)
        {
            if(tb_confirmpw.PasswordChar == '\0')
            {
                tb_confirmpw.PasswordChar = '*';
                bt_showConfirmpw.BringToFront();
            }
        }
    }
}
