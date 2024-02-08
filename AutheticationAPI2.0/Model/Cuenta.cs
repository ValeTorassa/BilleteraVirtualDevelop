using System.ComponentModel.DataAnnotations;

namespace AutheticationAPI2._0.Model
{
    public class Cuenta
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string NumeroCuenta { get; set; }

        [Required]
        public string Moneda { get; set; }

        [Required]
        public EstadoCuenta Estado { get; set; }

        [Required]
        public double Saldo { get; set; }

        [Required]
        public DateTime FechaApertura { get; set; }
        // public List<Transaccion> Transacciones { get; set; }

        public enum EstadoCuenta
        {
            Activa,
            Inactiva,
            Congelada
        }

    }
}
