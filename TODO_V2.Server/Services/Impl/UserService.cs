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
using TODO_V2.Client.DTO;
using TODO_V2.Server.Models;
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


        public async Task<bool> Add(User user, LoginCredentials credentials)
        {
            if (await GetByUserName(user.UserName) == null)
            {
                UserCredentials userCredentials = CreateUserCredentials(credentials);
                return await UserRepository.Add(user, userCredentials);
            }
            return false;
        }


        public async Task<User> Update(User user, LoginCredentials credentials)
        {
            UserCredentials userCredentials = await GetUserCredentialsByUserName(credentials.Username);
            userCredentials.EncryptedPassword = EncryptionUtil.Encrypt(credentials.Password);
            return await UserRepository.Update(user, userCredentials);
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
                    //user.Password = EncryptionUtil.Decrypt(user.Password);
                    return user;
                    
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<User?> GetByUserName(string username)
        {
            return await UserRepository.GetByUserName(username);           
        }

        public Task<int> Count()
        {
            return UserRepository.Count();      
        }

        public async Task<ActionResult<LoginResponse>> Login(LoginCredentials credentials)
        {
            try
            {
                UserCredentials userCredentials = await UserRepository.GetUserCredentialsByUserName(credentials.Username);

                if (userCredentials == null)
                {
                    return new UnauthorizedResult();
                }

                if (!credentials.Password.Equals(EncryptionUtil.Decrypt(userCredentials.EncryptedPassword)))
                {                    
                    return new UnauthorizedResult();
                }

                User user = await UserRepository.GetByUserName(credentials.Username);

                if (user == null)
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
                await localStorageService.RemoveItemAsync("token");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar cerrar sesión: {ex.Message}");
                return false;
            }
        }

        private async Task<UserCredentials> GetUserCredentialsByUserName(string username)
        {
            return await UserRepository.GetUserCredentialsByUserName(username);
        }

        private UserCredentials CreateUserCredentials(LoginCredentials credentials)
        {
            return new UserCredentials(credentials.Username, EncryptionUtil.Encrypt(credentials.Password));
        }

    }
}
