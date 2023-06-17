
using BrowserGameBackend.Models;
using Quartz.Util;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Security.Policy;
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
        //Username has to be 3-25 charecters long, start with letters and not have special symbols(except _ or -)
        [GeneratedRegex(@"^[^<>[\]{\}|\\\/^~')(`=@!¤€.%# :;,$%?\0-\cZ](?=.{1}[A-Za-z])[\dA-Za-z_-]{1,25}$")]
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
        //Password must be between 8-64 characters long, have at least one number, one uppercase and one lowercase letter!
        [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d\w\W]{8,64}$")]
        private static partial Regex PasswordRegex();
        public static bool ValidPassword(string password) 
        {
            if (password == null)  return false;

            return PasswordRegex().IsMatch(password);
        }

        [GeneratedRegex(@"^[\w-\.]+@([\w -]+\.)+[\w-]{2,4}$")]
        private static partial Regex EmailRegex();
        public static bool ValidEmail (string email)
        {
            if (email == null) return false;
            string[] emailParts = email.Split('@');
            return (EmailRegex().IsMatch(email) 
                && email.Length < 320
                && emailParts[0].Length < 63
                && emailParts[1].Length < 253);
        }

        public static bool ValidFaction(string faction)
        {
           string[] validFactions = { "Vega", "Solar", "Azure", "Unaffiliated" };
           return validFactions.Contains(faction);
        }

        public static bool ValidSpecies(string species)
        {
            string[] validSpecies = { "Aquatics", "Humans", "Insects", "Liths", "Robots", "Parasites" };
            return validSpecies.Contains(species);
        }

    }
}
