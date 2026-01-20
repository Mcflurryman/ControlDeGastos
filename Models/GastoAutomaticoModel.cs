
namespace ControlDeGastos.Models
{
    public class GastoAutomaticoModel
    {
        public int Id { get; set; }

        public decimal Importe { get; set; }
        public string Concepto { get; set; }

        public int CategoriaId { get; set; }
        public CategoriaModel Categoria { get; set; }

        public TipoMovimiento TipoMovimiento { get; set; }

        public int DiaDelMes { get; set; } 

        public bool Activo { get; set; } = true;

        public string UltimoMesGenerado { get; set; }
        public int UserId { get; set; }

        public User? User { get; set; }

    }

}
