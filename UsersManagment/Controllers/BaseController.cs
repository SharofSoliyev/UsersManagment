using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersManagment.Businees.Models;

namespace UsersManagment.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController : Controller
    {
        private int _currentUserId;

        protected int CurrentUserId
        {
            get
            {
                if (_currentUserId == 0)
                    _currentUserId = int.Parse(this.User.FindFirst(c => c.Type == "UserId").Value);
                return _currentUserId;
            }
        }


        protected ObjectResult Error(Exception exception)
        {
            return Error(StatusCodes.Status500InternalServerError, exception);
        }

        protected ObjectResult Error(int statusCodes, Exception exception)
        {
            return StatusCode(statusCodes, new ExceptionModel(exception.InnerException != null ? exception.InnerException.Message : exception.Message));
        }
    }
}
