using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Create(RegisterDto user, ModelStateDictionary modelState);
        Task<bool> Delete(RegisterDto user);
        Task<AppUser> GetUserAsync(LoginDto model);
        Task<object> GenerateJwt(AppUser user, string email);
        Task Logout();
    }
}
