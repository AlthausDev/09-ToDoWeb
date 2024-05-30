/**
 * Esta clase representa el servicio responsable de las operaciones relacionadas con usuarios.
 * @author Althaus_Dev
 */
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
    /// <summary>
    /// Clase de servicio para operaciones relacionadas con usuarios.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository UserRepository;
        private readonly EncryptionUtil EncryptionUtil;
        private readonly IConfiguration configuration;
        private readonly ILocalStorageService localStorageService;

        /// <summary>
        /// Constructor de la clase UserService.
        /// </summary>
        /// <param name="userRepository">El repositorio de usuarios.</param>
        /// <param name="encryptionUtil">La utilidad de encriptación.</param>
        /// <param name="configuration">La configuración.</param>
        /// <param name="localStorageService">El servicio de almacenamiento local.</param>
        public UserService(IUserRepository userRepository, EncryptionUtil encryptionUtil, IConfiguration configuration, ILocalStorageService localStorageService)
        {
            UserRepository = userRepository;
            EncryptionUtil = encryptionUtil;
            this.configuration = configuration;
            this.localStorageService = localStorageService;
        }

        /// <summary>
        /// Agrega un nuevo usuario con las credenciales proporcionadas.
        /// </summary>
        /// <param name="user">El usuario a agregar.</param>
        /// <param name="credentials">Las credenciales de inicio de sesión del usuario.</param>
        /// <returns>Devuelve true si el usuario se agrega correctamente, de lo contrario devuelve false.</returns>
        public async Task<bool> Add(User user, LoginCredentials credentials)
        {
            if (await GetByUserName(user.UserName) == null)
            {
                UserCredentials userCredentials = CreateUserCredentials(credentials);
                return await UserRepository.Add(user, userCredentials);
            }
            return false;
        }

        /// <summary>
        /// Actualiza un usuario existente con las credenciales proporcionadas.
        /// </summary>
        /// <param name="user">El usuario a actualizar.</param>
        /// <param name="credentials">Las credenciales de inicio de sesión del usuario.</param>
        /// <returns>Devuelve el usuario actualizado.</returns>
        public async Task<User> Update(User user, LoginCredentials credentials)
        {
       
            UserCredentials userCredentials = await UserRepository.GetUserCredentialsById(user.Id); 
            userCredentials.UserName = credentials.Username;

            return await UserRepository.Update(user, userCredentials);
        }

        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        /// <param name="userId">El ID del usuario a eliminar.</param>
        public void Delete(int userId)
        {
            UserRepository.Delete(userId);
        }

        /// <summary>
        /// Realiza una eliminación lógica de un usuario por su ID.
        /// </summary>
        /// <param name="userId">El ID del usuario a eliminar lógicamente.</param>
        public void LogicDelete(int userId)
        {
            UserRepository.LogicDelete(userId);
        }

        /// <summary>
        /// Obtiene todos los usuarios.
        /// </summary>
        /// <param name="request">La solicitud para obtener los usuarios.</param>
        /// <returns>Devuelve una lista de usuarios.</returns>
        public Task<IEnumerable<User>> GetAll(GetRequest<User>? request)
        {
            return UserRepository.GetAll(request);
        }

        /// <summary>
        /// Obtiene todos los usuarios lógicamente eliminados.
        /// </summary>
        /// <param name="request">La solicitud para obtener los usuarios lógicamente eliminados.</param>
        /// <returns>Devuelve una lista de usuarios lógicamente eliminados.</returns>
        public Task<IEnumerable<User>> GetAllLogic(GetRequest<User>? request)
        {
            return UserRepository.GetAllLogic(request);
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="userId">El ID del usuario a obtener.</param>
        /// <returns>Devuelve el usuario si se encuentra, de lo contrario devuelve null.</returns>
        public async Task<User?> GetById(int userId)
        {
            try
            {
                User? user = await UserRepository.GetById(userId);

                if (user != null)
                {
                    // user.Password = EncryptionUtil.Decrypt(user.Password);
                    return user;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario.
        /// </summary>
        /// <param name="username">El nombre de usuario.</param>
        /// <returns>Devuelve el usuario si se encuentra, de lo contrario devuelve null.</returns>
        public async Task<User?> GetByUserName(string username)
        {
            return await UserRepository.GetByUserName(username.ToUpper());
        }

        /// <summary>
        /// Cuenta el número de usuarios.
        /// </summary>
        /// <returns>Devuelve el número de usuarios.</returns>
        public Task<int> Count()
        {
            return UserRepository.Count();
        }

        /// <summary>
        /// Inicia sesión con las credenciales proporcionadas.
        /// </summary>
        /// <param name="credentials">Las credenciales de inicio de sesión.</param>
        /// <returns>Devuelve un resultado de acción con la respuesta de inicio de sesión.</returns>
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

        /// <summary>
        /// Cierra la sesión del usuario.
        /// </summary>
        /// <returns>Devuelve true si la sesión se cierra correctamente, de lo contrario devuelve false.</returns>
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

        /// <summary>
        /// Obtiene las credenciales de usuario por su nombre de usuario.
        /// </summary>
        /// <param name="username">El nombre de usuario.</param>
        /// <returns>Devuelve las credenciales de usuario.</returns>
        private async Task<UserCredentials> GetUserCredentialsByUserName(string username)
        {
            return await UserRepository.GetUserCredentialsByUserName(username.ToUpper());
        }

        /// <summary>
        /// Obtiene las credenciales de inicio de sesión por el ID del usuario.
        /// </summary>
        /// <param name="userId">El ID del usuario.</param>
        /// <returns>Devuelve las credenciales de inicio de sesión.</returns>
        public async Task<LoginCredentials> GetCredentialsByUserId(int userId)
        {
            UserCredentials userCredentials = await UserRepository.GetUserCredentialsById(userId);
            LoginCredentials loginCredentials = new(userCredentials.UserName, EncryptionUtil.Decrypt(userCredentials.EncryptedPassword));
            return loginCredentials;
        }

        /// <summary>
        /// Crea credenciales de usuario a partir de las credenciales de inicio de sesión.
        /// </summary>
        /// <param name="credentials">Las credenciales de inicio de sesión.</param>
        /// <returns>Devuelve las credenciales de usuario.</returns>
        private UserCredentials CreateUserCredentials(LoginCredentials credentials)
        {
            return new UserCredentials(credentials.Username, EncryptionUtil.Encrypt(credentials.Password));
        }
    }
}
