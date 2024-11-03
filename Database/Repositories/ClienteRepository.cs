using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories {
    public class ClienteRepository : Repository<Cliente>, IClienteRepository {
        public ClienteRepository(BankDbContext context) : base(context) {
        }

        public async Task<Cliente?> GetByIdAsync(int id) {

            return await _context.Clientes
                .Include(c => c.Persona)
                .FirstOrDefaultAsync(c => c.ClienteId == id);
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync() {
            return await _context.Clientes
                .Include(c => c.Persona)
                .ToListAsync();
        }

        public async Task<Cliente?> GetByIdentificacionAsync(string identificacion) {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Persona.Identificacion == identificacion);
        }
    }
}
