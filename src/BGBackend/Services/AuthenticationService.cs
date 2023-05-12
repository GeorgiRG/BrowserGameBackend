using BrowserGameBackend.Data;
using BrowserGameBackend.Tools;
using BrowserGameBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions.Generated;

namespace BrowserGameBackend.Services
{

    public interface IAuthenticationService
    {
        public Task<bool> UsernameExists(string username);
        public Task<bool> EmailExists(string email);
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

        public async Task<bool> UsernameExists(string username)
        {
            if (!UserInputTools.ValidUsername(username)) return false;
            return await _context.Users.Where(usr => usr.Name == username).AnyAsync();
        }

        public async Task<bool> EmailExists(string email)
        {
            if (!UserInputTools.ValidUsername(email)) return false;
            return await _context.Users.Where(usr => usr.Email == email).AnyAsync();
        }

        public async Task<string> Login(string email, string password)
        {
            if (!UserInputTools.ValidUsername(email) || !UserInputTools.ValidPassword(password))
            {
                return "Invalid input";
            }
            string? userPass = await _context.Users.Where(user => user.Email == email).Select(user => user.Password).FirstAsync();
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
