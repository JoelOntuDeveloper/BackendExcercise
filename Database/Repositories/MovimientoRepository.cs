using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories {
    public class MovimientoRepository : Repository<Movimiento>, IMovimientoRepository
    {
        public MovimientoRepository(BankDbContext context) : base(context) {
        }

        public async Task<IEnumerable<Movimiento>> GetAllAsync() {
            return await _context.Movimientos
                .Include(c => c.Cuenta)
                .ToListAsync();
        }

        public async Task<Movimiento?> GetLastMovimientoByCuentaId(int cuentaId) {
            return await _context.Movimientos
                .Where(movimiento => movimiento.CuentaId == cuentaId)
                .OrderByDescending(movimiento => movimiento.Fecha)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Movimiento>> GetMovimientosByFechasAndClienteId(DateTime fechaInicio, DateTime fechaFin, int clienteId) {
            return await _context.Movimientos
                .Include(mov => mov.Cuenta)
                .Where(mov => mov.Cuenta.ClienteId == clienteId && mov.Fecha >= fechaInicio && mov.Fecha <= fechaFin)
                .OrderBy(mov => mov.CuentaId)
                .ThenByDescending(mov => mov.Fecha).ToListAsync();
        }
    }
}
