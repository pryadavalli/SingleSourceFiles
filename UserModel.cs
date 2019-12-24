using AuthServiceToken.Entities;
using AuthServiceToken.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceToken.Helpers
{
    public interface IUserModel
    {
        Task<User> AuthenticateUser(User login);
    }
   
    public class UserModel : IUserModel
    {
        private IConfiguration _config;
        private IUserService _userService;

        public UserModel(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        public async Task<User> AuthenticateUser(User login)
        {
            var user = await _userService.Authenticate(login.Username, login.Password);

            if (user != null)
                {
                    string token = await Task.Run(() =>
                        TokenManager.GenerateToken(login.Username)
                    );
                    login.Token = token;
                    login.Password = "";
                    return login;
                }
                else
                {
                    return null;
                }
        }

      
      
    }
}
