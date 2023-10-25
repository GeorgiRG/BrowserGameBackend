using BrowserGameBackend.Controllers;
using BrowserGameBackend.Data;
using BrowserGameBackend.Models;
using Microsoft.AspNetCore.Mvc;
using BrowserGameBackend.Tools;
using BrowserGameBackend.Tools.GameTools;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using BrowserGameBackend.Dto;

namespace BrowserGameBackend.Services
{
    public interface IUserRegistrationService
    {
        public Task<string> CreateUser(UserRegistrationDto user);
        public Task<string> SendConfirmationEmail(User user);
        public Task<string> ConfirmEmail(string confirmCode);
    }
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly GameContext _context;
        private readonly IEmailService _emailService;
        private readonly IAuthenticationService _authenticationService;
        public UserRegistrationService(
            GameContext context,
            IEmailService emailService,
            IAuthenticationService authenticationService)
        {
            _context = context;
            _emailService = emailService;
            _authenticationService = authenticationService;
        }

        public async Task<string> CreateUser(UserRegistrationDto userData)
        {
            try { 
                //check inputs
                if (userData == null)
                {
                    return "No user data provided";
                }
                else
                {
                    if (!UserInputTools.ValidUsername(userData.Name!)) return "Invalid username";
                    if (!UserInputTools.ValidEmail(userData.Email!)) return "Invalid email";
                    if (!UserInputTools.ValidPassword(userData.Password!)) return "Invalid password";
                }
                //"Exists" and validity check
                if (!await _authenticationService.UsernameValidAndOriginal(userData.Name!)) return "Username taken";
                if (!await _authenticationService.EmailValidAndOriginal(userData.Email!)) return "Email taken";

                //hash password, create user
                User user = new()
                {
                    Name = userData.Name,
                    Email = userData.Email,
                    Password = PasswordTools.Hash(userData.Password!),
                    LastLogin = DateTime.UtcNow
                };
                UserSkills userSkills = new()
                {
                    User = user,
                    UserId = user.Id,
                };
                user.UserSkills = userSkills;

                //send confirmation email
                string emailResult = await SendConfirmationEmail(user);
                if (emailResult != "Ok") return emailResult;

                _context.Users.Add(user);
                _context.UserSkills.Add(userSkills);
                await _context.SaveChangesAsync();
                return "Ok";
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return "Server error";
            }
        }

        public async Task<string> SendConfirmationEmail(User user)
        {
            string confirmationCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            string body = String.Format("Hello {0},\nYour confirmation code is {1}.\n--BrowserGame", user.Name, confirmationCode);
            string emailResult = await _emailService.Send(user.Email!, "Confirming your email", body);
            user.EmailConfirmation = confirmationCode;
            await _context.SaveChangesAsync();
            return emailResult;
        }

        public async Task<string> ConfirmEmail(string confirmCode)
        {
            //validate
            if (confirmCode == null
                || confirmCode.Length != 6
                || !confirmCode.All(char.IsDigit))
            {
                return "Invalid code";
            }
            //update user
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(usr => usr.EmailConfirmation == confirmCode);
                if (user == null) return "Wrong code"; 
                user.EmailConfirmation = "Ok";
                await _context.SaveChangesAsync();
                return "Ok";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "Server error";
            }   
        }
    }
}
