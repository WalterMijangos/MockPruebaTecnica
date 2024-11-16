using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MockPruebaTecnica.Data;
using MockPruebaTecnica.Models;

namespace MockPruebaTecnica.Controllers
{
    public class VentasController : Controller
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ventas.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .FirstOrDefaultAsync(m => m.VentaId == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VentaId,Fecha,ClienteId,ProductoId,Cantidad,TotalVenta")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venta);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VentaId,Fecha,ClienteId,ProductoId,Cantidad,TotalVenta")] Venta venta)
        {
            if (id != venta.VentaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.VentaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .FirstOrDefaultAsync(m => m.VentaId == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.VentaId == id);
        }

        [HttpPost]
        public async Task<IActionResult> CargarArchivo(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Por favor, seleccione un archivo de Excel");
            }

            if (!file.FileName.EndsWith(".xlsx"))
            {
                return BadRequest("Por favor, seleccione un archivo de Excel");
            }

            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataTable = reader.AsDataSet().Tables[0];

                    for (int fila = 1; fila < dataTable.Rows.Count; fila++)
                    {
                        
                        DateTime fecha = DateTime.Parse(dataTable.Rows[fila][0].ToString());
                        string nombreCliente = dataTable.Rows[fila][1].ToString();
                        string apellidoCliente = dataTable.Rows[fila][2].ToString();
                        string correoCliente = dataTable.Rows[fila][3].ToString();
                        string codigoProducto = dataTable.Rows[fila][4].ToString();
                        string nombreProducto = dataTable.Rows[fila][5].ToString();
                        string descripcionProducto = dataTable.Rows[fila][6].ToString();
                        string categoriaProducto = dataTable.Rows[fila][7].ToString();
                        int cantidad = int.Parse(dataTable.Rows[fila][8].ToString());
                        decimal precio = decimal.Parse(dataTable.Rows[fila][9].ToString());
                        decimal totalVenta = decimal.Parse(dataTable.Rows[fila][10].ToString());

                        Cliente? cliente = await _context.Clientes
                            .FirstOrDefaultAsync(c => c.Correo == correoCliente);

                        if (cliente == null)
                        {
                            cliente = new Cliente
                            {
                                Nombre = nombreCliente,
                                Apellido = apellidoCliente,
                                Correo = correoCliente
                            };
                            _context.Clientes.Add(cliente);
                            await _context.SaveChangesAsync();
                        }

                        Producto? producto = await _context.Productos
                            .FirstOrDefaultAsync(p => p.CodigoBarras == codigoProducto);

                        if (producto == null)
                        {
                            producto = new Producto
                            {
                                CodigoBarras = codigoProducto,
                                NombreProducto = nombreProducto,
                                Descripcion = descripcionProducto,
                                Categoria = categoriaProducto,
                                Precio = precio
                            };
                            _context.Productos.Add(producto);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            producto.NombreProducto = nombreProducto;
                            producto.Descripcion = descripcionProducto;
                            producto.Categoria = categoriaProducto;
                            producto.Precio = precio;
                            _context.Productos.Update(producto);
                        }

                        Venta venta = new Venta
                        {
                            Fecha = fecha,
                            ClienteId = cliente != null ? cliente.ClienteID : -1,
                            ProductoId = producto != null ? producto.Id : -1,
                            Cantidad = cantidad,
                            TotalVenta = totalVenta
                        };
                        _context.Ventas.Add(venta);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
