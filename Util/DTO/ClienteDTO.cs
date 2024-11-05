using System.ComponentModel.DataAnnotations;

namespace Util.DTO {
    public class ClienteDTO {

        public ClienteDTO() { }
        public int ClienteId { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        [RegularExpression("^[MF]$", ErrorMessage = "El campo género solo puede contener los valores 'M' o 'F'.")]
        public string Genero { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El número de teléfono debe tener 10 dígitos.")]
        public string Telefono { get; set; }
        public string Contrasenia { get; set; }
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "La identifiación debe tener 10 dígitos.")]
        public string Identificacion { get; set; }
        public int Edad { get; set; }
        public bool Estado { get; set; } = true;
    }
}
