using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OpenMind.Models;
using OpenMind.Services.Validators.Interfaces;

namespace OpenMind.Services.Validators
{
    public class PasswordValidator : IPasswordValidator
    {
        public async Task<int> ValidateAsync(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                return 457;
            }
            string pattern = "^^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$";
 
            if (!Regex.IsMatch(password, pattern))
            {
                return 457;
            }
            return 200;
        }
    }
}