using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities {
    public class Movimiento {
        public int MovimientoId { get; set; }
        public int CuentaId { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; } = null!;
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }

        public virtual Cuenta Cuenta { get; set; } = null!;
    }
}
