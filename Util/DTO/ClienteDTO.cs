namespace Util.DTO {
    public class ClienteDTO {

        public ClienteDTO() { }
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        public string Contrasenia { get; set; }
        public string Identificacion { get; set; }
        public int Edad { get; set; }
        public bool Estado { get; set; } = true;
    }
}
