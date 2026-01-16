using ControlDeGastos.Models;
using Microsoft.AspNetCore.Mvc;
using ControlDeGastos.Data;
using Microsoft.EntityFrameworkCore;

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
                                        QueMovimiento= Gasto.TipoMovimiento
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




    }
    }


