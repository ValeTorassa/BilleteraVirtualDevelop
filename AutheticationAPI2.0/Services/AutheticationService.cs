using AutheticationAPI2._0.Model.Dto;
using AutheticationAPI2._0.Model;
using AutheticationAPI2._0.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AutheticationAPI2._0.Data;
using Microsoft.EntityFrameworkCore;

namespace AutheticationAPI2._0.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthenticationService(UserManager<User> userManager, IConfiguration configuration, AppDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<Result> RegisterAsync(RegisterRequest request)
        {
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            var userByUsername = await _userManager.FindByNameAsync(request.UserName);

            if (userByEmail is not null || userByUsername is not null)
            {
                return new Result { IsSuccess = false, Message = $"El usuario con correo electrónico {request.Email} o nombre de usuario {request.UserName} ya existe" };
            }

            User user = new()
            {
                Email = request.Email,
                UserName = request.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new Result { IsSuccess = false, Message = GetErrorsText(result.Errors) };
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, StaticUserRoles.User);

            if (!addToRoleResult.Succeeded)
            {
                 await _userManager.DeleteAsync(user);

                return new Result { IsSuccess = false, Message = "Error al asignar el rol al usuario" };
            }

            return new Result { IsSuccess = true, Response = "Usuario creado exitosamente" };

        }

        public async Task<Result> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username) ?? await _userManager.FindByEmailAsync(request.Username);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new Result { IsSuccess = false, Message = $"No se puede autenticar al usuario {request.Username}" };
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var token = GetToken(authClaims);
            var refreshToken = GenerateRefreshToken(user);

            return new Result
            {
                IsSuccess = true,
                Response = new TokenResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken.RefreshTokenValue
                }
            };
        }

        public async Task<Result> MakeAdminAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
            {
                return new Result { IsSuccess = false, Message = $"Usuario {username} no encontrado" };
            }

            var resultAddAdmin = await _userManager.AddToRoleAsync(user, StaticUserRoles.Admin);
            var resultRemoveUser = await _userManager.RemoveFromRoleAsync(user, StaticUserRoles.User);

            if (!resultAddAdmin.Succeeded || !resultRemoveUser.Succeeded)
            {
                if (resultAddAdmin.Succeeded) await _userManager.RemoveFromRoleAsync(user, StaticUserRoles.Admin);
                if (resultRemoveUser.Succeeded) await _userManager.AddToRoleAsync(user, StaticUserRoles.User);

                return new Result { IsSuccess = false, Message = "Error al promover al usuario a administrador" };
            }

            return new Result { IsSuccess = true, Message = $"El usuario {username} ahora es administrador" };
        }


        public async Task<Result> LogoutAsync(string refreshToken)
        {
            var existingRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.RefreshTokenValue == refreshToken);

            if (existingRefreshToken != null)
            {
                existingRefreshToken.Used = true;
                await _context.SaveChangesAsync();
            }

            return new Result { IsSuccess = true, Message = "Usuario cerro sesion exitosamente" };
        }


        public async Task<Result> ValidateRefreshTokenAsync(string refreshToken)
        {
            var existingRefreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.RefreshTokenValue == refreshToken && rt.Active && !rt.Used && rt.Expiration > DateTime.UtcNow);

            if (existingRefreshToken == null)
            {
                return new Result { IsSuccess = false, Message = "Refresh Token invalido o expirado" };
            }

            existingRefreshToken.Used = true;
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(existingRefreshToken.UserId);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var newAccessToken = GetToken(authClaims);

            return new Result
            {
                IsSuccess = true,
                Response = new TokenResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                    RefreshToken = existingRefreshToken.RefreshTokenValue
                }
            };
        }



        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        private RefreshToken GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Active = true,
                Expiration = DateTime.UtcNow.AddDays(7),
                RefreshTokenValue = Guid.NewGuid().ToString("N"),
                Used = false,
                UserId = user.Id
            };

            _context.Add(refreshToken);
            _context.SaveChanges();

            return refreshToken;
        }

        private string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }
    }
}
