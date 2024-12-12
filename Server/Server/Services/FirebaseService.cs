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

        public async Task<bool> ResetPassword(string email)
        {
            var emailResponse = await _client.GetAsync("Users");
            var users = emailResponse.ResultAs<Dictionary<string, Register>>();
            if (users == null || !users.Values.Any(u => u.Email == email))
            {
                return false; // Email không được đăng ký
            }

            // Thực hiện logic đặt lại mật khẩu ở đây (ví dụ: gửi liên kết đặt lại mật khẩu đến email)
            return true;
        }

        public async Task<RegisterResult> UpdatePasswordInFirebase(string username, string newPassword)
        {
            var path = $"Users/{username}/Password";
            var setResponse = await _client.SetAsync(path, newPassword);
            if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new RegisterResult { Success = true, Message = "Mật khẩu đã được cập nhật thành công." };
            }
            else
            {
                return new RegisterResult { Success = false, Message = "Không thể cập nhật mật khẩu." };
            }
        }
    }
    public class RegisterResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
