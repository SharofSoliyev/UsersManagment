using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagment.Businees.Helpers;
using UsersManagment.Businees.Models;
using UsersManagment.Businees.Settings;
using UsersManagment.Core.Entities;
using UsersManagment.Core.Repository;

namespace UsersManagment.Businees.Services
{
    public interface IUserService
   {
        Task<LoginTokenModel> Login(UserLoginModel userLoginModel);
        Task<UserModel> Register(UserModel userModel);
    }
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
    
        private readonly TokenSetting _tokenSettings;
        private readonly IMapper _mapper;
        private readonly TokenHelper _tokenHelper;
        public UserService(IRepository<User> userRepository,
                           IMapper mapper,
                           TokenHelper tokenHelper
            )
        {
            this._userRepository = userRepository;
            ////////////////////////
        }
        public Task<LoginTokenModel> Login(UserLoginModel userLoginModel)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> Register(UserModel userModel)
        {
            throw new NotImplementedException();
        }
    }
}
