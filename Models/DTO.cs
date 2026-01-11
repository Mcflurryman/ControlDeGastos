using System.Diagnostics.Contracts;

namespace ControlDeGastos.Models
{
    public class DTO
    {
        public string Concepto { get; set; }
        public decimal Importe { get; set; }
        public DateTime Fecha { get; set; }

        public TipoMovimiento TipoMovimiento { get; set; } = 0;
        public string NombreCategoria { get; set; }
    }
}
