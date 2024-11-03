namespace Core.Entities {
    public class Cliente {
        public Cliente() {
            Cuenta = new HashSet<Cuenta>();
        }

        public int ClienteId { get; set; }
        public string Contrasenia { get; set; }
        public bool Estado { get; set; }

        public virtual Persona Persona { get; set; }
        public virtual ICollection<Cuenta> Cuenta { get; set; }
    }
}
