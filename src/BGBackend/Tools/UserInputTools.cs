
using BrowserGameBackend.Models;
using Quartz.Util;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Text.RegularExpressions;

namespace BrowserGameBackend.Tools
{
    public static partial class UserInputTools
    {
        public static bool BadNames(string input)
        {
            string[] badWords = { "nothing yet" };
            foreach (string badWord in badWords)
            {
                if (input.Contains(badWord)) { return true; }
            }
            return false;
        }

        [GeneratedRegex(@"[A-Za-z-_']{3,25}")]
        private static partial Regex UsernameRegex();
        public static bool ValidUsername(string username)
        {
            if (username == null) return false;

            Regex rgx = UsernameRegex();
            if (username.IsNullOrWhiteSpace() || 
                BadNames(username) ||
                !rgx.IsMatch(username)) 
            {
                return false;
            }
            else { return true; }
        }

        [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$")]
        private static partial Regex PasswordRegex();
        public static bool ValidPassword(string password) 
        {
            if (password == null) return false;

            return PasswordRegex().IsMatch(password);
        }

        [GeneratedRegex(@"^[\w-\.]+@([\w -]+\.)+[\w-]{2,4}$")]
        private static partial Regex EmailRegex();
        public static bool ValidEmail (string email)
        {
            string[] emailParts = email.Split('@');
            if (email == null) return false;
            return (EmailRegex().IsMatch(email) 
                && email.Length < 320
                && emailParts[0].Length < 63
                && emailParts[1].Length < 253);
        }


    }
}
