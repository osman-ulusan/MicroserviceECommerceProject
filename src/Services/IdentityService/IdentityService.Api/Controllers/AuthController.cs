﻿using IdentityService.Api.Application.Models;
using IdentityService.Api.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService identityService;

        public AuthController(IIdentityService ıdentityService)
        {
            this.identityService = ıdentityService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {
            var result = await identityService.Login(loginRequestModel);

            return Ok(result);
        }
    }
}
