using AutheticationAPI2._0.Model.Dto;
using System.IdentityModel.Tokens.Jwt;


namespace AutheticationAPI2._0.Services.IServices
{
    public interface IAuthenticationService
    {
        Task<Result> RegisterAsync(RegisterRequest request);
        Task<Result> LoginAsync(LoginRequest request);
        Task<Result> LogoutAsync(string refreshToken);
        Task<Result> MakeAdminAsync(string username);
        Task<Result> ValidateRefreshTokenAsync(string refreshToken);
    }
}
