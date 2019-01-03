using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.ViewModels.Auth;

namespace Web.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {

        String uName = "janedoe@example.com";
        String pw = "5ESTdYB5cyYwA2dKhJqyjPYnKUc&45Ydw^gz^jy&FCV3gxpmDPdaDmxpMkhpp&9TRadU%wQ2TUge!TsYXsh77Qmauan3PEG8!6EP";


        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel vm)
        {
            // Validate the requests
            if (!ModelState.IsValid)
            {
                return BadRequest(); // TODO: Return error description
            }

            var result = await _signInManager.PasswordSignInAsync(
                userName: vm.Username,
                password: vm.Password,
                isPersistent: true, // TODO: Get this from the viewmodel
                lockoutOnFailure: true
            );

            if (result.RequiresTwoFactor)
            {
                return StatusCode(StatusCodes.Status501NotImplemented);
            }
            if (result.IsLockedOut)
            {
                return StatusCode(StatusCodes.Status423Locked);
            }
            if (result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
        }

        // POST: /api/auth/logout
        [Authorize, HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}