
using ControlDeGastos.Models;

namespace ControlDeGastos.Models
{
    public class GastoModel
    {
        
            public int Id { get; set; }
            public string Concepto { get; set; }
            public decimal Importe { get; set; }
            public DateTime Fecha { get; set; }

            public TipoMovimiento TipoMovimiento { get; set; }

            public int CategoriaId { get; set; }
                

    }
}
