using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagment.Businees.Helpers;
using UsersManagment.Businees.Models;
using UsersManagment.Businees.Settings;
using UsersManagment.Core.Entities;
using UsersManagment.Infostructure.Repository;

namespace UsersManagment.Businees.Services
{
    public interface IUserService
   {
        Task<LoginTokenModel> Login(UserLoginModel userLoginModel);
        Task<UserModel> Register(UserModel userModel);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
    
        private readonly TokenSetting _tokenSettings;
        private readonly IMapper _mapper;
        private readonly TokenHelper _tokenHelper;
        public UserService(IUserRepository userRepository,
                           IMapper mapper,
                           TokenHelper tokenHelper,
                           IOptions<TokenSetting> tokenSettings
            )
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            this._tokenHelper = tokenHelper;
            this._tokenSettings = tokenSettings.Value;
            ////////////////////////
        }
        public async Task<LoginTokenModel> Login(UserLoginModel userLoginModel)
        {
            var user = await _userRepository.GetUser(userLoginModel.UserName, userLoginModel.Password);

            if (user != null)
            {
                var mapped = _mapper.Map<UserModel>(user);
                if (mapped == null)
                    throw new Exception($"Entity could not be mapped.");

                LoginTokenModel loginTokenModel = _tokenHelper.CreateToken(mapped);
                return loginTokenModel;
            }
            return null;
        }

        public async Task<UserModel> Register(UserModel userModel)
        {
            var userExists = await _userRepository.GetUser(userModel.UserName, userModel.Password);

            if (userExists != null)
                throw new ApplicationException("User already exists!");

            var user = _mapper.Map<User>(userModel);
            if (user == null)
                throw new Exception($"Entity could not be mapped.");

            var result = await _userRepository.CreateUser(user);
            if (result == null)
                throw new ApplicationException("User creation failed! Please check user details and try again.");

            return _mapper.Map<UserModel>(user);
        }
    }
}
