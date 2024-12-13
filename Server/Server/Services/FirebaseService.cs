using FireSharp;    
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.Extensions.Configuration;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireSharp.Response;

namespace Server.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly IFirebaseClient _client;

        public FirebaseService(IConfiguration configuration)
        {
            var config = new FirebaseConfig
            {
                AuthSecret = "ptadAFZjKIegVxEFzWhRrhn5VUj0qbWM0upbVKEa",
                BasePath = "https://bombmaster-14f3a-default-rtdb.asia-southeast1.firebasedatabase.app"
            };
            _client = new FireSharp.FirebaseClient(config);
        }

        public async Task<RegisterResult> RegisterUser(Register user)
        {
            var response = await _client.GetAsync("Users/" + user.Username);
            if (response.Body != "null")
            {
                return new RegisterResult { Success = false, Message = "Tên tài khoản đã tồn tại. Vui lòng đặt tên khác!" };
            }

            var emailResponse = await _client.GetAsync("Users");
            var users = emailResponse.ResultAs<Dictionary<string, Register>>();
            if (users != null && users.Values.Any(u => u.Email == user.Email))
            {
                return new RegisterResult { Success = false, Message = "Email này đã được đăng ký!" };
            }

            var setResponse = await _client.SetAsync("Users/" + user.Username, user);
            if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new RegisterResult { Success = true, Message = "Tạo tài khoản thành công. Chúc bạn có trải nghiệm game vui vẻ!" };
            }
            else
            {
                return new RegisterResult { Success = false, Message = "Không thể tạo tài khoản." };
            }
        }

        public async Task<LoginResult> LoginUser(string username, string password)
        {
            var response = await _client.GetAsync("Users/" + username);
            if (response.Body == "null")
            {
                return new LoginResult { Success = false, Message = "Tài khoản không tồn tại" };
            }

            var user = response.ResultAs<Register>();
            if (user.Password == password)
            {
                return new LoginResult { Success = true, Message = "Đăng nhập thành công" };
            }
            else
            {
                return new LoginResult { Success = false, Message = "Sai mật khẩu" };
            }
        }

        public async Task<UpdatePasswordResult> UpdatePasswordInFirebase(string username, string newPassword)
        {
            // Check if user exists
            var response = await _client.GetAsync("Users/" + username);
            if (response.Body == "null")
            {
                return new UpdatePasswordResult { Success = false, Message = "Tài khoản không tồn tại." };
            }

            // Update the password
            var path = $"Users/{username}/Password";
            var setResponse = await _client.SetAsync(path, newPassword);
            if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new UpdatePasswordResult { Success = true, Message = "Mật khẩu đã được cập nhật thành công." };
            }
            else
            {
                return new UpdatePasswordResult { Success = false, Message = "Không thể cập nhật mật khẩu." };
            }
        }

        public async Task<bool> IsEmailExists(string email)
        {
            var emailResponse = await _client.GetAsync("Users");
            var users = emailResponse.ResultAs<Dictionary<string, Register>>();
            return users != null && users.Values.Any(u => u.Email == email);
        }

        public async Task SaveVerificationCodeToFirebase(string email, string verificationCode)
        {
            var path = $"VerificationCodes/{email.Replace(".", ",")}";
            await _client.SetAsync(path, verificationCode);
        }

        public async Task<VerificationCodeResult> GetVerificationCodeFromFirebase(string email)
        {
            // Retrieve all users
            var emailResponse = await _client.GetAsync("Users");
            var users = emailResponse.ResultAs<Dictionary<string, Register>>();

            if (users == null || users.Count == 0)
            {
                return new VerificationCodeResult { Success = false, Message = "Không thể truy cập dữ liệu người dùng." };
            }

            // Find the user with the matching email
            var userEntry = users.FirstOrDefault(u => u.Value.Email == email);

            if (string.IsNullOrEmpty(userEntry.Key))
            {
                return new VerificationCodeResult { Success = false, Message = "Email không tồn tại." };
            }

            var username = userEntry.Key;

            // Get the verification code
            var path = $"VerificationCodes/{email.Replace(".", ",")}";
            var response = await _client.GetAsync(path);
            var verificationCode = response.ResultAs<string>();

            if (string.IsNullOrEmpty(verificationCode))
            {
                return new VerificationCodeResult { Success = false, Message = "Không tìm thấy mã xác nhận.", Username = username };
            }

            return new VerificationCodeResult { Success = true, VerificationCode = verificationCode, Username = username };
        }
    }

}
