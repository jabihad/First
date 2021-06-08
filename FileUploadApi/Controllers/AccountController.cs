﻿using AutoMapper;
using Entities.DTO;
using Entities.Models;
using FileUploadApi.JwtFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FileUploadApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        //private readonly JwtHandler _jwtHandler;
        public AccountController(UserManager<User> userManager, IMapper mapper/*, JwtHandler jwtHandler*/, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            //_jwtHandler = jwtHandler;
            _signInManager = signInManager;
        }

        [HttpPost("Registration")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration == null || !ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<User>(userForRegistration);

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            return StatusCode(201);
        }
        //[HttpPost("Login")]
        //public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        //{
        //    var user = await _userManager.FindByNameAsync(userForAuthentication.Email);

        //    if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
        //        return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

        //    var signingCredentials = _jwtHandler.GetSigningCredentials();
        //    var claims = _jwtHandler.GetClaims(user);
        //    var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
        //    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        //    return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        //}
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(userForAuthentication.Email, userForAuthentication.Password, false, false);

                if (result.Succeeded)
                {
                    Log.Information("Login success {@model}", userForAuthentication);
                    return StatusCode(200);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    Log.Warning("Invalid Login attempt {@model}", userForAuthentication);
                }
            }
            return StatusCode(401);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return StatusCode(200);
        }

    }
}
