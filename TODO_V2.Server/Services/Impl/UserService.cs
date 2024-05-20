using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Server.Utils;
using TODO_V2.Shared;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Models.Enum;
using TODO_V2.Shared.Utils;

namespace TODO_V2.Server.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository UserRepository;
        private readonly EncryptionUtil EncryptionUtil;
        private readonly IConfiguration configuration;
        private readonly ILocalStorageService localStorageService;

        public UserService(IUserRepository userRepository, EncryptionUtil encryptionUtil, IConfiguration configuration, ILocalStorageService localStorageService)
        {
            UserRepository = userRepository;
            EncryptionUtil = encryptionUtil;
            this.configuration = configuration;
            this.localStorageService = localStorageService;
        }


        public async Task<bool> Add(User user)
        {
            if (await GetByUserName(user.UserName) == null)
            {
                user.Password = EncryptionUtil.Encrypt(user.Password);
                return await UserRepository.Add(user);
            }
            return false;
        }

        public async Task<User> Update(User user)
        {
            user.Password = EncryptionUtil.Encrypt(user.Password);
            return await UserRepository.Update(user);
        }

        public void Delete(int userId)
        {
            UserRepository.Delete(userId);
        }

        public void LogicDelete(int userId)
        {
            UserRepository.LogicDelete(userId);
        }

        public Task<IEnumerable<User>> GetAll(GetRequest<User>? request)
        {
            return UserRepository.GetAll(request);
        }

        public Task<IEnumerable<User>> GetAllLogic(GetRequest<User>? request)
        {
            return UserRepository.GetAllLogic(request);
        }

        public async Task<User?> GetById(int userId)
        {
            try
            {
                User? user = await UserRepository.GetById(userId);

                if (user != null)
                {
                    user.Password = EncryptionUtil.Decrypt(user.Password);
                }

                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<User?> GetByUserName(string username)
        {
            try
            {
                User? user = await UserRepository.GetByUserName(username);

                if (user != null)
                {
                    user.Password = EncryptionUtil.Decrypt(user.Password);
                }

                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<int> Count()
        {
            return UserRepository.Count();      
        }

        public async Task<ActionResult<LoginResponse>> Login(string username, string password)
        {
            try
            {
                User user = await UserRepository.GetByUserName(username);

                if (user == null)
                {
                    return new UnauthorizedResult();
                }

                string decryptedPassword = EncryptionUtil.Decrypt(user.Password);
                if (password != decryptedPassword)
                {
                    return new UnauthorizedResult();
                }

                string tokenString = await EncryptionUtil.BuildToken(user, configuration);

                if (tokenString == null)
                {
                    return new StatusCodeResult(500);
                }

                var response = new LoginResponse
                {
                    User = user,
                    Token = tokenString
                };

                return response;            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar iniciar sesión: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        public async Task<bool> Logout()
        {
            try
            {
                await localStorageService.ClearAsync();
                await localStorageService.RemoveItemAsync("Token");
                await localStorageService.RemoveItemAsync("token");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar cerrar sesión: {ex.Message}");
                return false;
            }
        }

    }
}
