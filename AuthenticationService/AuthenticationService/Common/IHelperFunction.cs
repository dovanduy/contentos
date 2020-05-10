using AuthenticationService.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Common
{
    public interface IHelperFunction
    {
        object GenerateJwtToken(string email, Accounts user, string Role);
        string GenerateRandomPassword(PasswordOptions opts = null);
    }
}
