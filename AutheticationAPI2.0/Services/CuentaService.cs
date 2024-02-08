using AutheticationAPI2._0.Data;
using AutheticationAPI2._0.Model;
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
        public async Task CreateCuentaAsync(int userId)
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
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task DeleteCuentaAsync(int userId)
        {
            try
            {
                var cuenta = await GetCuentaByUserIdAsync(userId);
                _context.Cuentas.Remove(cuenta);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
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
                throw e;
            }
        }

        public async Task UpdateCuentaAsync(Cuenta cuenta)
        {
            try
            {
                _context.Cuentas.Update(cuenta);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
