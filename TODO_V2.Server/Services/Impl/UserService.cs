using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared;
using System.Data.Common;
using TODO_V2.Server.Utils;
using TODO_V2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using BlazorBootstrap;
using TODO_V2.Shared.Models.Enum;

namespace TODO_V2.Server.Services.Impl
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        private readonly EncryptionUtil encryptionUtil;

        public UserService(IUserRepository userRepository, EncryptionUtil encryptionUtil)
        {
            this.userRepository = userRepository;
            this.encryptionUtil = encryptionUtil;
        }

        public Task<bool> Add(User user)
        {      
            if (GetByUserName(user.UserName) == null)
            {
                user.Password = encryptionUtil.Encrypt(user.Password);
                return userRepository.Add(user);               
            }
            return new Task<bool>(() => false);
        }

        public Task<User> Update(User user)
        {
            user.Password = encryptionUtil.Encrypt(user.Password);
            return userRepository.Update(user);
        }

        public void Delete(int userId)
        {
            userRepository.Delete(userId);
        }

        public void LogicDelete(int userId)
        {
            userRepository.LogicDelete(userId);
        }

        public Task<IEnumerable<User>> GetAll(GetRequest<User>? request)
        {
            return userRepository.GetAll(request);
        }

        public Task<IEnumerable<User>> GetAllLogic(GetRequest<User>? request)
        {
            return userRepository.GetAllLogic(request);
        }

        public User GetById(int userId)
        {
            try
            {
                User user = userRepository.GetById(userId).Result;

                user.Password = encryptionUtil.Decrypt(user.Password);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public User GetByUserName(string Username)
        {
            try
            {
                User user = userRepository.GetByUserName(Username).Result;

                user.Password = encryptionUtil.Decrypt(user.Password);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult<int> Count()
        {
            return userRepository.Count();
        }

        public ActionResult<User> Login(string username, string password)
        {
            throw new NotImplementedException();
        }


    }
}
