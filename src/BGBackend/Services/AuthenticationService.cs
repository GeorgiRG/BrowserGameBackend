using BrowserGameBackend.Data;
using BrowserGameBackend.Tools;
using BrowserGameBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions.Generated;

namespace BrowserGameBackend.Services
{

    public interface IAuthenticationService
    {
        public Task<bool> UsernameValidAndOriginal(string username);
        public Task<bool> EmailValidAndOriginal(string email);
        public Task<string> Login(string email, string password);

    }
    /// <summary>
    /// Various user authentication, e.g Exists()
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly GameContext _context;
        public AuthenticationService(GameContext context) 
        {
            _context = context;
        }

        public async Task<bool> UsernameValidAndOriginal(string username)
        {
            return UserInputTools.ValidUsername(username) &&
                    !await _context.Users.Where(usr => usr.Name == username).AnyAsync();
        }

        public async Task<bool> EmailValidAndOriginal(string email)
        {
            return UserInputTools.ValidEmail(email) &&
                    !await _context.Users.Where(usr => usr.Email == email).AnyAsync();
        }

        public async Task<string> Login(string email, string password)
        {
            if (!UserInputTools.ValidEmail(email) || !UserInputTools.ValidPassword(password))
            {
                return "Invalid input";
            }
            string? userPass = await _context.Users.Where(user => user.Email == email).Select(user => user.Password).FirstOrDefaultAsync();
            if (userPass != null)
            {
                if (PasswordTools.Verify(password, userPass))
                {
                    return "Ok";
                }
                else return "Wrong password";
            }
            else return "Wrong email";
        }

    }
}
