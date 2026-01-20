
namespace ControlDeGastos.Models
{
    public class CategoriaModel
    {
        
            public int Id { get; set; }
            public string Nombre { get; set; }
            public int Orden { get; set; }
            public int UserId { get; set; }
            public User? User { get; set; }


    }
}
