using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities {
    public class Cliente {
        public Cliente() {
            Cuenta = new HashSet<Cuenta>();
        }

        public int ClienteId { get; set; }
        public string Contrasenia { get; set; } = null!;
        public bool Estado { get; set; }

        public virtual Persona ClienteNavigation { get; set; } = null!;
        public virtual ICollection<Cuenta> Cuenta { get; set; }
    }
}
