using BrowserGameBackend.Controllers;
using BrowserGameBackend.Data;
using BrowserGameBackend.Models;
using Microsoft.AspNetCore.Mvc;
using BrowserGameBackend.Tools;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace BrowserGameBackend.Services
{
    public interface IUserRegistrationService
    {
        public Task<string> CreateUser(User user);
        public Task<string> ConfirmEmail(string confirmCode);
        public Task<bool> Exists(string username, string email);
        /*
        public Task<ActionResult> VerifyUser();
        public Task<ActionResult> FindUser();
        public Task<ActionResult> DeleteUser();
        public Task<ActionResult> EditUser();
        */
    }
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly GameContext _context;
        private readonly IEmailService _emailService;
        private readonly IAuthenticationService _authenticationService;
        public UserRegistrationService(GameContext context, IEmailService emailService, IAuthenticationService authenticationService)
        {
            _context = context;
            _emailService = emailService;
            _authenticationService = authenticationService;
        }

        public async Task<string> CreateUser(User user)
        {
            try { 
                //check inputs
                if (user == null)
                {
                    return "No user provided";
                }
                else
                {
                    if (!UserInputTools.ValidUsername(user.Name!)) return "Invalid username";
                    if (!UserInputTools.ValidPassword(user.Password!)) return "Invalid password";
                    if (!UserInputTools.ValidEmail(user.Email!)) return "Invalid email";
                }
                //"Exists" check
                if (await _authenticationService.UsernameExists(user.Name!)) return "Username taken";
                if (await _authenticationService.EmailExists(user.Email!)) return "Email taken";

                //send confirmation email
                string confirmationCode = RandomNumberGenerator.GetInt32(111111, 999999).ToString();
                string body = String.Format("Hello {0},\nYour confirmation code is {1}.\n--BrowserGame", user.Name, confirmationCode);
                string emailResult = "";
                for (int retryTimes = 0; retryTimes < 2; retryTimes++)
                {
                    emailResult = _emailService.Send(user.Email!, "Confirming your email", body);
                    if (emailResult == "Ok") break;
                }
                if (emailResult != "Ok") return emailResult;

                //hash password, create user
                user.Password = PasswordTools.Hash(user.Password!);
                user.EmailConfirmation = confirmationCode;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return "Server error";
            }

            return "Ok";

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
                User user = await _context.Users.FirstAsync(usr => usr.EmailConfirmation == confirmCode);
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

        public async Task<bool> Exists(string username, string email)
        {
            User user = await _context.Users.FirstAsync(usr => usr.Name == username);

            return false;
        }


        /*
        public Task<ActionResult> VerifyUser();
        public Task<ActionResult> FindUser();
        public Task<ActionResult> DeleteUser();
        public Task<ActionResult> EditUser();
        */
    }
}
