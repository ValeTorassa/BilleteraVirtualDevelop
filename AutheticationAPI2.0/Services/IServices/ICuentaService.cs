using AutheticationAPI2._0.Model;
using AutheticationAPI2._0.Model.Dto;

namespace AutheticationAPI2._0.Services.IServices
{
    public interface ICuentaService
    {
        Task<Result> CreateCuentaAsync(int userId);
        Task<Result> UpdateCuentaAsync(Cuenta cuenta);
        Task<Result> DeleteCuentaAsync(int userId);
        Task<Cuenta> GetCuentaByUserIdAsync(int userId);
        Task<Cuenta> GetCuentaByNumeroCuentaAsync(string numeroCuenta);
       
    }
}
