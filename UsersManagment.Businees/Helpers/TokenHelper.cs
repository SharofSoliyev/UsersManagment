using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UsersManagment.Businees.Models;
using UsersManagment.Businees.Settings;

namespace UsersManagment.Businees.Helpers
{
    public class TokenHelper
    {
        private readonly TokenSetting _tokenSettings;

        public TokenHelper(TokenSetting tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }
        public LoginTokenModel CreateToken(UserModel userModel)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", userModel.Id.ToString())
                }),
                //Claims = new Dictionary<string, object>() {
                //    { "Permissions", userModel.Role.Permissions.Select(e => EncryptionHelper.AES.EncryptData(e)) }
                //},
                Expires = DateTime.Now.AddDays(_tokenSettings.ExpiresInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginTokenModel model = new LoginTokenModel()
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = token.ValidTo,
                IsActive = userModel.IsActive
            };

            return model;
        }
    }

}
