using Microsoft.Extensions.Caching.Memory;
using BrowserGameBackend.Dto;
using BrowserGameBackend.Data;
using System.Security.Cryptography;
using BrowserGameBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Runtime.Intrinsics.Arm;
using System;

namespace BrowserGameBackend.Services
{
    public interface ISessionService
    {
        public string CreateSessionId(string email);
        public UserDto? FindBySession(string sessionId);
        public Task<bool> SessionIsStored(string sessionId);

        public Task<string>? CreateOrRefreshSession(string? email = null, string? sessionId = null, bool rememberMe = false);
    }
    public  class SessionService : ISessionService
    {
        private readonly IConfiguration _config;
        private readonly IMemoryCache _memCache;
        private readonly GameContext _context;

        public SessionService(IMemoryCache memoryCache, GameContext context, IConfiguration config)
        {
            _memCache = memoryCache;
            _context = context;
            _config = config;
        }

        public string CreateSessionId(string email)
        {
            string sessionString = email + _config["SessionSecret"] + DateTime.Now.ToString();
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(sessionString));
            string sessionId = Convert.ToHexString(hash);
            return sessionId;
        }

        public UserDto? FindBySession(string sessionId)
        {
            UserDto? userDto = _memCache.Get<UserDto>(sessionId);
            return userDto;
        }

        public async Task<bool> SessionIsStored(string sessionId)
        {
            if (sessionId == null) return false;
            return await _context.Users.Where(usr => usr.SessionId == sessionId).AnyAsync();
        }

        //called on login (still refreshes the sessionId even if 'remember me' is picked) or if user not in memory
        public async Task<string>? CreateOrRefreshSession(string? email = null, string? sessionId = null, bool rememberMe = false)
        {
            if (email == null & sessionId == null ) { return null!; }
            UserDto userDto = await _context.Users
                                        .Where(usr => usr.Email == email || usr.SessionId == sessionId)
                                        .Select(usr => new UserDto(
                                                            usr.Id,
                                                            usr.Name!,
                                                            usr.Email!,
                                                            usr.Faction!,
                                                            usr.Race!,
                                                            usr.CharClass!,
                                                            usr.SessionId!))
                                        .FirstAsync();
            if (userDto != null)
            {
                var options = new MemoryCacheEntryOptions()
                                    .SetPriority(CacheItemPriority.Low)
                                    .SetSize(1)
                                    .SetSlidingExpiration(TimeSpan.FromMinutes(15));
                string newSessionId = CreateSessionId(userDto.Email);
                _memCache.Set(newSessionId, userDto, options);
                if (rememberMe)
                {
                    await _context.Users
                                .Where(usr => usr.Email == email || usr.SessionId == sessionId)
                                .ExecuteUpdateAsync(usr => usr.SetProperty(usr => usr.SessionId, newSessionId));
                }
                return newSessionId;
            }
            return null!;
        }

        public async void ClearStoredSession(string email)
        {

        }
    }
}
