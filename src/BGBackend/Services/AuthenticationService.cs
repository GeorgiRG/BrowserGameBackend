using BrowserGameBackend.Data;
using BrowserGameBackend.Tools;
using BrowserGameBackend.Models;
using BrowserGameBackend.Dto;

using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions.Generated;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Identity;

namespace BrowserGameBackend.Services
{

    public interface IAuthenticationService
    {
        public Task<bool> UsernameValidAndOriginal(string username);
        public Task<bool> EmailValidAndOriginal(string email);
        public Task<string> CheckLoginCredentials(string email, string password);
        public Task<bool> SessionIsValid(string session);

        public bool AdminCheck(string password);

    }
    /// <summary>
    /// Various user authentication, e.g Exists()
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly GameContext _context;
        private readonly IMemoryCache _memoryCache;
        public AuthenticationService(GameContext context, IMemoryCache memoryCache) 
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<bool> SessionIsValid(string session)
        {
            if (session != null && _memoryCache.Get(session) == null)
            {
                return await _context.Users.Where(user => user.SessionId == session)
                                            .AnyAsync();
            }
            else return true;
        }

        public async Task<bool> UsernameValidAndOriginal(string username)
        {
            Console.WriteLine(username);
            return UserInputTools.ValidUsername(username) &&
                    !await _context.Users.Where(usr => usr.Name == username)
                                         .AnyAsync();
        }

        public async Task<bool> EmailValidAndOriginal(string email)
        {
            return UserInputTools.ValidEmail(email) &&
                    !await _context.Users.Where(usr => usr.Email == email)
                                         .AnyAsync();
        }

        public async Task<string> CheckLoginCredentials(string email, string password)
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

        public bool AdminCheck(string password)
        {
            //high security
            return password == "123";
        }
    }
}
