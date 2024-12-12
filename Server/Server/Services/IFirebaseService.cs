using Server.Models;

namespace Server.Services
{
    public interface IFirebaseService
    {
        Task<RegisterResult> RegisterUser(Register user);
        Task<LoginResult> LoginUser(string username, string password); // Change return type to match implementation
        Task<bool> ResetPassword(string email);
        Task<RegisterResult> UpdatePasswordInFirebase(string username, string newPassword);
    }
    public class UpdatePasswordResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
