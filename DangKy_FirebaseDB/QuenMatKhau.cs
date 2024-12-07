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
using System.Net.Mail;
using System.Net;



namespace DangKy_FirebaseDB
{
    public partial class QuenMatKhau : Form
    {
        private FirebaseClient Client;
        string username;

        public QuenMatKhau()
        {
            InitializeComponent();
            Client = new FirebaseClient(ifc);
        }

        IFirebaseConfig ifc = new FirebaseConfig
        {
            AuthSecret = "ptadAFZjKIegVxEFzWhRrhn5VUj0qbWM0upbVKEa",
            BasePath = "https://bombmaster-14f3a-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

        private async void bt_getVeriCode_Click(object sender, EventArgs e)
        {
            string email = tb_email.Text;

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Email không hợp lệ. Vui lòng nhập đúng định dạng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!await IsEmailExists(email))
            {
                MessageBox.Show("Email không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string verificationCode = GenerateVerificationCode();
            await SendVerificationCode(email, verificationCode);
            await SaveVerificationCodeToFirebase(email, verificationCode);
            MessageBox.Show("Mã xác nhận đã được gửi đến email của bạn.", "Thông báo", MessageBoxButtons.OK);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                // Check if the domain part has at least two characters
                var domainPart = addr.Host.Split('.');
                return addr.Address == email && domainPart.Length > 1 && domainPart[domainPart.Length - 1].Length > 1;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> IsEmailExists(string email)
        {
            FirebaseResponse emailResponse = await Client.GetAsync("Users");
            var users = emailResponse.ResultAs<Dictionary<string, Register>>();
            foreach (var user in users)
            {
                if (user.Value.Email == email)
                {
                    username = user.Key;
                    return true;
                }
            }
            return false;
        }

        private async void bt_confirm_Click(object sender, EventArgs e)
        {
            try
            {

                string email = tb_email.Text;
                string enteredCode = tb_veriCode.Text;
                string savedCode = await GetVerificationCodeFromFirebase(email);

                if (enteredCode == savedCode)
                {
                    MessageBox.Show("Xác nhận thành công. Vui lòng đặt lại mật khẩu!", "Thông báo", MessageBoxButtons.OK);
                    DatLaiMatKhau dlmk = new DatLaiMatKhau(email, username);
                    dlmk.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Mã xác nhận không chính xác.", "Thông báo", MessageBoxButtons.RetryCancel);
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập mã xác nhận!", "Thông báo", MessageBoxButtons.RetryCancel);
            }
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        // ...

        private async Task SendVerificationCode(string email, string verificationCode)
        {
            var message = new MailMessage();
            message.From = new MailAddress("noreplybombmaster@gmail.com", "BombMaster");
            message.To.Add(new MailAddress(email));
            message.Subject = "BombMaster: Mã xác nhận thay đổi mật khẩu.";
            message.Body = $"Mã xác nhận của bạn là: {verificationCode}, mã này là duy nhất xin đừng chia sẻ cho bất kì ai.";
            message.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential("noreplybombmaster@gmail.com", "sgka twxe wyce smfj");
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(message);
            }
        }

        private async Task SaveVerificationCodeToFirebase(string email, string verificationCode)
        {
            var path = $"VerificationCodes/{email.Replace(".", ",")}";
            await Client.SetAsync(path, verificationCode);
        }

        private async Task<string> GetVerificationCodeFromFirebase(string email)
        {
            try
            {

                var path = $"VerificationCodes/{email.Replace(".", ",")}";
                var response = await Client.GetAsync(path);
                return response.ResultAs<string>();
            }
            catch
            {
                return null;
            }
        }

        private void QuenMatKhau_Load(object sender, EventArgs e)
        {

        }
    }
}
