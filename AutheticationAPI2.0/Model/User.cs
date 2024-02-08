using Microsoft.AspNetCore.Identity;

namespace AutheticationAPI2._0.Model
{
    public class User : IdentityUser
    {
        public string NombrePila { get; set; }
        public string Apellido { get; set; }
        public string Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }
       
        public Cuenta CuentaBancaria { get; set; }

    }
}
