using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using SuperTank.WindowsForms;


namespace DangKy_FirebaseDB
{
    public partial class DangNhap : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public DangNhap()
        {
            InitializeComponent();
        }

        private async void bt_login_Click(object sender, EventArgs e)
        {
            string tentk = tb_username.Text;
            string matkhau = tb_password.Text;
            if (string.IsNullOrEmpty(tentk) || string.IsNullOrEmpty(matkhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var loginData = new { Username = tentk, Password = matkhau };
            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("https://localhost:7029/api/account/login", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var loginResult = JsonConvert.DeserializeObject<LoginResult>(responseBody);

                if (loginResult.Success)
                {
                    MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Chuyển sang form khác
                    // Tạo và mở form frmMenu từ project SuperTank
                    frmMenu menuForm = new frmMenu();
                    menuForm.Show();

                    // Ẩn form đăng nhập hiện tại
                    this.Hide();
                }
                else
                {
                    MessageBox.Show(loginResult.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Không thể kết nối đến server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonReaderException ex)
            {
                MessageBox.Show("Lỗi phân tích cú pháp JSON: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llb_registry_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DangKy dk = new DangKy();
            dk.Show();
        }

        private void llb_forgetedpw_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            QuenMatKhau qmk = new QuenMatKhau();
            qmk.Show();
        }

        private void bt_hide_Click(object sender, EventArgs e)
        {
            if (tb_password.PasswordChar == '\0')
            {
                tb_password.PasswordChar = '*';
                bt_show.BringToFront();
            }
        }

        private void bt_show_Click(object sender, EventArgs e)
        {
            if (tb_password.PasswordChar == '*')
            {
                tb_password.PasswordChar = '\0';
                bt_hide.BringToFront();
            }
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {

        }
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
