using System;
using System.Collections.Generic;

namespace Database.Enities
{
    public partial class Cliente
    {
        public Cliente()
        {
            Cuenta = new HashSet<Cuenta>();
        }

        public int ClienteId { get; set; }
        public string Contrasenia { get; set; } = null!;
        public bool Estado { get; set; }

        public virtual Persona ClienteNavigation { get; set; } = null!;
        public virtual ICollection<Cuenta> Cuenta { get; set; }
    }
}
