using ControlDeGastos.Models;
using Microsoft.AspNetCore.Mvc;
using ControlDeGastos.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ControlDeGastos.Controllers
{

    [ApiController]
    [Route("Api/[controller]")]
    public class GastosController : ControllerBase
    {
        private readonly ControlGastosContext _context;
        public GastosController(ControlGastosContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostImporte([FromBody] DTO GastoImporte)
        {
            var Categoria = await _context.Categoria.FirstOrDefaultAsync(c => c.Nombre == GastoImporte.NombreCategoria);

            if (Categoria == null)
            {
                Categoria = new CategoriaModel
                {
                    Nombre = GastoImporte.NombreCategoria
                };

                _context.Categoria.Add(Categoria);
                await _context.SaveChangesAsync();
            }
            if (Categoria != null)
            {
                var categoriaId = Categoria.Id;
            }

            var gasto = new GastoModel
            {
                Importe = GastoImporte.Importe,
                Fecha = DateTime.Today,
                Concepto = GastoImporte.Concepto,
                CategoriaId = Categoria.Id


            };

            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();

            return Ok($"Se ha introducido correctamente el importe: {GastoImporte.Importe} Con el concepto de: {GastoImporte.NombreCategoria}");

        }
        [HttpPost("Ingreso")]
        public async Task<IActionResult> PostIngreso([FromBody] DTO Ingreso)
        {
            var CategoriaIngreso = await _context.Categoria.FirstOrDefaultAsync(c => c.Nombre == Ingreso.NombreCategoria);

            if (CategoriaIngreso == null)
            {
                CategoriaIngreso = new CategoriaModel
                {
                    Nombre = Ingreso.NombreCategoria
                };

                _context.Categoria.Add(CategoriaIngreso);
                await _context.SaveChangesAsync();
            }
            if (CategoriaIngreso != null)
            {
                var categoriaId = CategoriaIngreso.Id;
            }
            var ingreso = new GastoModel
            {
                Importe = Ingreso.Importe,
                Fecha = DateTime.Today,
                Concepto = Ingreso.Concepto,
                CategoriaId = CategoriaIngreso.Id,
                TipoMovimiento = Ingreso.TipoMovimiento


            };
            if (ingreso.TipoMovimiento == null)
            {
                Ingreso.TipoMovimiento = 0;
            }
            else { Ingreso.TipoMovimiento = ingreso.TipoMovimiento; }

            _context.Gastos.Add(ingreso);
            await _context.SaveChangesAsync();

            return Ok($"Se ha introducido correctamente el ingreso: {Ingreso.Importe} Con el concepto de: {Ingreso.NombreCategoria}");
        }
        [HttpGet("GastosTotales")]
        public async Task<IActionResult> GetGastos()
        {
            var Gastos = await _context.Categoria.Join(
                                    _context.Gastos,
                                    Category => Category.Id,
                                    Gasto => Gasto.CategoriaId,
                                    (Category, Gasto) => new
                                    {
                                        fecha = Gasto.Fecha,
                                        importe = Gasto.Importe,
                                        Concepto = Category.Nombre,
                                        QueMovimiento = Gasto.TipoMovimiento
                                    }
                                    ).ToListAsync();
            return Ok(Gastos);



        }
        [HttpGet("GastosporCategoria")]
        public async Task<IActionResult> GetGastos(string NombreCategoria)
        {
            var categoria = await _context.Categoria
                .FirstOrDefaultAsync(c => c.Nombre == NombreCategoria);

            if (categoria == null)
            {
                return NotFound("La categoria no existe");
            }

            var total = await _context.Gastos
                .Where(g => g.CategoriaId == categoria.Id)
                .SumAsync(g => g.Importe);

            return Ok(new
            {
                Categoria = categoria.Nombre,
                ImporteTotal = total
            });
        }
        [HttpGet("RestaTotal")]
        public async Task<IActionResult> RestaTotal()
        {
            var ingresos = await _context.Gastos.Where(g => g.TipoMovimiento == TipoMovimiento.Ingreso)
                 .SumAsync(g => g.Importe);

            var resta = await _context.Gastos.Where(m => m.TipoMovimiento == TipoMovimiento.Gasto).SumAsync(g => g.Importe);

            return Ok(resta - ingresos);
        }
        [HttpGet("GastosGlobalesPorFecha")]
        public async Task<IActionResult> GastosGlobalesPorFecha(DateTime inicio, DateTime final)

        {
            var dateInicio = inicio.Date;
            var dateFinal = final.Date.AddDays(1);

            var Gastos = await _context.Gastos
                        .Where(m => m.TipoMovimiento == TipoMovimiento.Gasto
                        &&
                        m.Fecha >= dateInicio
                        &&
                        m.Fecha < dateFinal).ToListAsync();


            return Ok(Gastos);

        }
        [HttpGet("GastosSumadosPorFecha")]
        public async Task<IActionResult> GastosSumadosPorFecha(DateTime inicio, DateTime final)

        {
            var dateInicio = inicio.Date;
            var dateFinal = final.Date.AddDays(1);

            var Gastos = await _context.Gastos
                        .Where(m => m.TipoMovimiento == TipoMovimiento.Gasto
                        &&
                        m.Fecha >= dateInicio
                        &&
                        m.Fecha < dateFinal).SumAsync(g => g.Importe);
            return Ok(Gastos);

        }

        [HttpGet("IngresosSumadosPorFecha")]
        public async Task<IActionResult> IngresosSumadosPorFecha(DateTime inicio, DateTime final)

        {
            var dateInicio = inicio.Date;
            var dateFinal = final.Date.AddDays(1);

            var Ingresos = await _context.Gastos
                        .Where(m => m.TipoMovimiento == TipoMovimiento.Ingreso
                        &&
                        m.Fecha >= dateInicio
                        &&
                        m.Fecha < dateFinal).SumAsync(g => g.Importe);
            return Ok(Ingresos);

        }
        [HttpGet("ControlBalanceMesActual")]
        public async Task<IActionResult> ControlBalanceMesActual()

        {

            DateTime hoy = DateTime.Today;

            DateTime InicioMesActual = new DateTime(hoy.Year, hoy.Month, 1);

            DateTime InicioMesSiguiente = InicioMesActual.AddMonths(1);
            var totalGastos = await _context.Gastos
                              .Where(g => g.TipoMovimiento == TipoMovimiento.Gasto
                              &&
                              g.Fecha >= InicioMesActual
                              &&
                              g.Fecha < InicioMesSiguiente)
                              .SumAsync(g => g.Importe);

            var totalIngresos = await _context.Gastos
                             .Where(g => g.TipoMovimiento == TipoMovimiento.Ingreso
                             &&
                             g.Fecha >= InicioMesActual
                             &&
                             g.Fecha < InicioMesSiguiente)
                             .SumAsync(g => g.Importe);

            var balance = totalIngresos - totalGastos;
            return Ok(new
            {
                Gastos = totalGastos,
                Ingresos = totalIngresos,
                Balance = balance
            });
        }
        [HttpGet("ControlBalanceMesFiltrado")]
        public async Task<IActionResult> ControlBalanceMesFiltrado(DateTime MesInicio, DateTime MesFinal)

        {


            var totalGastos = await _context.Gastos
                              .Where(g => g.TipoMovimiento == TipoMovimiento.Gasto
                              &&
                              g.Fecha >= MesInicio
                              &&
                              g.Fecha < MesFinal)
                              .SumAsync(g => g.Importe);

            var totalIngresos = await _context.Gastos
                             .Where(g => g.TipoMovimiento == TipoMovimiento.Ingreso
                             &&
                             g.Fecha >= MesInicio
                             &&
                             g.Fecha < MesFinal)
                             .SumAsync(g => g.Importe);

            var balance = totalIngresos - totalGastos;
            return Ok(new
            {
                Gastos = totalGastos,
                Ingresos = totalIngresos,
                Balance = balance
            });

        }
    }
}


