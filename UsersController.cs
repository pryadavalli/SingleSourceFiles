using AuthServiceToken.Entities;
using AuthServiceToken.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
using AuthServiceToken.Helpers;

namespace AuthServiceToken.Controllers
{
    [AuthorizationFilter]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserModel _loginuserobj;
        public UsersController(IUserModel loginuserobj)
        {
            _loginuserobj = loginuserobj;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]User userParam)
        {
            User user = await _loginuserobj.AuthenticateUser(userParam);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpGet]
        [Route("TestPrivateMethod")]
        public async Task<IActionResult> TestPrivateMethod()
        {
            
            return Ok("test authorization");
        }

        
    }
}
