using AutheticationAPI2._0.Data;
using AutheticationAPI2._0.Model;
using AutheticationAPI2._0.Model.Dto;
using AutheticationAPI2._0.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace AutheticationAPI2._0.Services
{
    public class CuentaService : ICuentaService
    {
       
        private readonly AppDbContext _context;
        public CuentaService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result> CreateCuentaAsync(int userId)
        {
            try
            {
                var cuenta = new Cuenta
                {
                    UserId = userId,
                    NumeroCuenta = Guid.NewGuid().ToString(),
                    Saldo = 0
                };
                await _context.Cuentas.AddAsync(cuenta);
                await _context.SaveChangesAsync();
                return new Result { IsSuccess = true, Message = "Cuenta creada con éxito" };
            }
            catch (Exception e)
            {
                return new Result { IsSuccess = false, Message = e.Message };
            }
        }
        public async Task<Result> DeleteCuentaAsync(int userId)
        {
            try
            {
                var cuenta = await GetCuentaByUserIdAsync(userId);
                _context.Cuentas.Remove(cuenta);
                await _context.SaveChangesAsync();
                return new Result { IsSuccess = true, Message = "Cuenta eliminada con éxito" };
                
            }
            catch (Exception e)
            {
               return new Result { IsSuccess = false, Message = e.Message };
            }
        }
        public async Task<Cuenta> GetCuentaByNumeroCuentaAsync(string numeroCuenta)
        {
            try
            {
                return await _context.Cuentas.FirstOrDefaultAsync(u => u.NumeroCuenta == numeroCuenta);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Cuenta> GetCuentaByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Cuentas.FirstOrDefaultAsync(u => u.UserId == userId);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Result> UpdateCuentaAsync(Cuenta cuenta)
        {
            try
            {
                _context.Cuentas.Update(cuenta);
                await _context.SaveChangesAsync();
                return new Result { IsSuccess = true, Message = "Cuenta actualizada con éxito" };
            }
            catch (Exception e)
            {
                return new Result { IsSuccess = false, Message = e.Message };
            }
        }


    }
}
