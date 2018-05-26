using AutoMapper;
using Eleks_2018_MicroSocialMedia.AuthHelpers;
using Eleks_2018_MicroSocialMedia.AuthHelpers.interfaces;
using Eleks_2018_MicroSocialMedia.Helpers;
using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.Services.Interfaces;
using Eleks_2018_MicroSocialMedia.UoW.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Services
{
    public class AccountService
        : IAccountService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtFactory _jwtFactory;

        public AccountService(
            IUserRepository userRepo,
            IUnitOfWork unitOfWork,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IJwtFactory jwtFactory)
        {
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _jwtFactory = jwtFactory;
        }

        public async Task<bool> Create(RegisterDto userDto, ModelStateDictionary modelState)
        {
            var user = new AppUser {
                Email = userDto.Email,
                UserName = userDto.Email,
                Profile = new WriteModels.Profile()
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    DateOfBirth = userDto.DateOfBirth,
                    Geolocation = new Geolocation(),
                    LastGeolocation = new LastGeolocation(),
                }
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    modelState.AddModelError(string.Empty, error.Description);
                }
            }

            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> Delete(RegisterDto user)
        {
            // TODO User Delete

            return await Task.FromResult(true);
        }

        public async Task<object> GenerateJwt(AppUser user, string email)
        {
            return await JWT.GenerateJwt(user, _jwtFactory, email, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        public async Task<AppUser> GetUserAsync(LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                return _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
            }

            return null;
        }
    }
}
