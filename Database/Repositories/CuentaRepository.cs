using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories {
    public class CuentaRepository : Repository<Cuenta>, ICuentaRepository
    {
        public CuentaRepository(BankDbContext context) : base(context) {
        }

        public async Task<Cuenta?> GetByNumeroCuenta(string numeroCuenta) {
            return await _context.Cuentas.FirstOrDefaultAsync(cuenta => cuenta.NumeroCuenta == numeroCuenta);
        }
    }
}
