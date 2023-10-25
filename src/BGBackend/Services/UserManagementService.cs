/*
 * Handles sessions
 * Handles user data for and from the memory cache and database
 */
using Microsoft.Extensions.Caching.Memory;
using BrowserGameBackend.Dto;
using BrowserGameBackend.Data;
using BrowserGameBackend.Tools;
using System.Security.Cryptography;
using BrowserGameBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Runtime.Intrinsics.Arm;
using System.Reflection.PortableExecutable;
using BrowserGameBackend.Tools.GameTools;
using BrowserGameBackend.Enums;

namespace BrowserGameBackend.Services
{
    public interface IUserManagementService
    {
        public string CreateSessionId(string username);
        public Task<UserDto>? GetUserDto(string? sessionId = null, string? email = null);
        public Task<UserDto>? LoginUser(bool rememberMe, string? email = null, string? sessionId = null);
        public Task<UserDto>? CharacterCreation(string sessionId, string faction, string species);
        public Task<UserSkills>? GetUserSkills(UserDto userDto);
        public Task<UserSkills>? UpdateUserSkills(string sessionId, LevelUpSkillsDto userSkills);

    }
    public class UserManagementService : IUserManagementService
    {
        private readonly IConfiguration _config;
        private readonly IMemoryCache _memoryCache;
        private readonly GameContext _context;
        private readonly Services.IAuthenticationService _authenticationService;
        private readonly MemoryCacheEntryOptions _userOptions = new MemoryCacheEntryOptions()
                                                                   .SetPriority(CacheItemPriority.Low)
                                                                   .SetSize(1)
                                                                   .SetSlidingExpiration(TimeSpan.FromMinutes(15));
        public UserManagementService(
            IMemoryCache memoryCache,
            GameContext context,
            IConfiguration config,
            IAuthenticationService authenticationService)
        {
            _memoryCache = memoryCache;
            _context = context;
            _authenticationService = authenticationService;
            _config = config;
            _authenticationService = authenticationService;
        }

        public string CreateSessionId(string username)
        {
            string sessionString = username + _config["SessionSecret"] + DateTime.Now.ToString();
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(sessionString));
            string sessionId = Convert.ToHexString(hash);
            return sessionId;
        }

        public async Task<UserDto>? GetUserDto(string? sessionId = null, string? email = null)
        {
            if (email == null & sessionId == null) { return null!; }
            UserDto? userDto = null;
            if (sessionId != null)
            {
                userDto = _memoryCache.Get<UserDto>(sessionId);
            }
            if (userDto != null) return userDto;
            userDto = 
                await _context.Users.Where(user => user.SessionId == sessionId || user.Email == email)
                                    .Select(user => new UserDto(
                                                        user.Id,
                                                        user.Name!,
                                                        user.Faction!,
                                                        user.Species!,
                                                        user.SessionId!))
                                    .FirstOrDefaultAsync();
            return userDto!;
        }

        public async Task<UserDto>? LoginUser(bool rememberMe, string? email = null, string? sessionId = null)
        {
            UserDto? userDto = await GetUserDto(sessionId, email)!;
            if (userDto != null)
            {
                string newSessionId = CreateSessionId(userDto.Name);
                //still refreshes the sessionId even if 'remember me' is picked
                userDto.SessionId = newSessionId;
                _memoryCache.Set(newSessionId, userDto, _userOptions);
                if (rememberMe)
                {
                    await _context.Users.Where(usr => usr.Email == email || usr.SessionId == sessionId)
                                        .ExecuteUpdateAsync(usr => usr.SetProperty(usr => usr.SessionId, newSessionId));
                }
                return userDto;
            }
            return null!;
        }
        
        public async Task<UserDto>? CharacterCreation(string sessionId, string faction, string speciesData)
        {
            UserDto? userDto = await GetUserDto(sessionId: sessionId)!;
            if (userDto == null) return null!;

            Factions factions = new();
            SpeciesEnum species = new();
            
            if (factions.IsValidValue(faction!))
            {
                userDto.Faction = faction!;
                await _context.Users.Where(user => user.Id == userDto.Id)
                                    .ExecuteUpdateAsync(user => user.SetProperty(user => user.Faction, faction));
            }
            if (species.IsValidValue(speciesData))
            {
                userDto.Species = speciesData!;
                await _context.Users.Where(user => user.Id == userDto.Id)
                                    .ExecuteUpdateAsync(user => user.SetProperty(user => user.Species, speciesData));
            }
            
            _memoryCache.Set(sessionId, userDto, _userOptions);
            return userDto;
        }
        public async Task<UserSkills>? GetUserSkills(UserDto userDto)
        {
            UserSkills? userSkills = await _context.UserSkills.Where(userSkills => userSkills.UserId == userDto.Id)
                                                              .FirstOrDefaultAsync();
            return userSkills;
        }
        public async Task<UserSkills>? UpdateUserSkills(string sessionId, LevelUpSkillsDto userSkills)
        {
            UserDto? userDto = await GetUserDto(sessionId: sessionId)!;
            UserSkills oldUserSkills = await GetUserSkills(userDto)!;
            if (oldUserSkills == null) return null!;
            oldUserSkills = CharacterTools.LevelUp(oldUserSkills, userSkills)!;
            if (oldUserSkills == null) return null!;

            await _context.SaveChangesAsync();
            return oldUserSkills;
        }
    }
}
