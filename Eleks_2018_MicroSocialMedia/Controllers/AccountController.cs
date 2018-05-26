using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eleks_2018_MicroSocialMedia.Helpers;
using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.Services.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Eleks_2018_MicroSocialMedia.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _userService;
        private readonly ILogger<AccountController> _logger;
        
        public AccountController(IAccountService userService, ILogger<AccountController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterDto userDto)
        {
            if (ModelState.IsValid)
            {
                var created = await _userService.Create(userDto, ModelState);

                if (created)
                {
                    return Ok();
                }
                return BadRequest(ModelState);

            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogCritical("Before getting user");
            var user = await _userService.GetUserAsync(loginDto);

            if (user == null)
            {
                _logger.LogCritical("User is null");
                ModelState.AddModelError(string.Empty, "Invalid Email and (or) password.");
                return BadRequest(ModelState);
            }

            var jwt = await _userService.GenerateJwt(user, loginDto.Email);

            return Ok(jwt);
        }

        [HttpPost]
        public async Task Logout()
        {
            await _userService.Logout();
        }
    }
}