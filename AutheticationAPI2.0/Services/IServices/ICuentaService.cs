using AutheticationAPI2._0.Model;

namespace AutheticationAPI2._0.Services.IServices
{
    public interface ICuentaService
    {
        Task CreateCuentaAsync(int userId);
        Task UpdateCuentaAsync(Cuenta cuenta);
        Task DeleteCuentaAsync(int userId);
        Task<Cuenta> GetCuentaByUserIdAsync(int userId);
        Task<Cuenta> GetCuentaByNumeroCuentaAsync(string numeroCuenta);
       
    }
}
