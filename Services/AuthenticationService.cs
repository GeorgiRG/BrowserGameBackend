using BrowserGameBackend.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions.Generated;

namespace BrowserGameBackend.Services
{

    public interface IAuthenticationService
    {
        public Task<bool> UsernameExists(string username);
        public Task<bool> EmailExists(string email);
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

        public async Task<bool> UsernameExists (string username)
        {
            return await _context.Users.FirstOrDefaultAsync(usr => usr.Name == username) != null;
        }

        public async Task<bool> EmailExists (string email)
        {
            return await _context.Users.FirstOrDefaultAsync(usr => usr.Name == email) != null;
        }
    }
}
