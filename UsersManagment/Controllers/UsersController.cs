using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersManagment.Businees.Models;
using UsersManagment.Businees.Services;

namespace UsersManagment.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        //[MapToApiVersion("2")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginModel userLoginModel)
        {
            try
            {
                var user = await _userService.Login(userLoginModel);
                if (user == null)
                {
                    return Ok(new ResponseModel(ResponseStatusCode.BadRequest, "Invalid username or password"));
                }

                if (!user.IsActive)
                {
                    return Ok(new ResponseModel(ResponseStatusCode.BadRequest, "You are not active user"));
                }

                return Ok(new ResponseModel(user));
            }
            catch (Exception ex)
            {
                return Error(StatusCodes.Status500InternalServerError, ex);
            }
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserModel userModel)
        {
            try
            {
                var result = await _userService.Register(userModel);
                return Ok(new ResponseModel(result));
            }
            catch (Exception ex)
            {
                return Error(StatusCodes.Status500InternalServerError, ex);
            }
        }

    }
}
