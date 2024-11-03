namespace Core.Entities {
    public class Cuenta {

        public Cuenta() {
            Movimiento = new HashSet<Movimiento>();
        }

        public int CuentaId { get; set; }
        public int ClienteId { get; set; }
        public string NumeroCuenta { get; set; } = null!;
        public string TipoCuenta { get; set; } = null!;
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }

        public virtual Cliente Cliente { get; set; } = null!;
        public virtual ICollection<Movimiento> Movimiento { get; set; }
    }
}
