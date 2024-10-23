using System;
using System.Collections.Generic;

namespace Database.Enities
{
    public partial class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Genero { get; set; } = null!;
        public int Edad { get; set; }
        public string Identificacion { get; set; } = null!;
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }

        public virtual Cliente? Cliente { get; set; }
    }
}
