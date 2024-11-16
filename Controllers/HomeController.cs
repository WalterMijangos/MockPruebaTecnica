using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockPruebaTecnica.Data;
using Newtonsoft.Json;
using MockPruebaTecnica.Models;
using System.Diagnostics;

namespace MockPruebaTecnica.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
		{
			_logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var ventasQuery = _context.Ventas.Include(venta => venta.Producto).AsQueryable();


            var totalVentas = ventasQuery.Sum(venta => venta.TotalVenta);
            var totalUnidades = ventasQuery.Sum(venta => venta.Cantidad);

            //ventas por mes
            var ventasPorMes = _context.Ventas
                .GroupBy(venta => venta.Fecha.Month)
                .Select(grupo => new
                {
                    Mes = grupo.Key,
                    TotalVentas = grupo.Sum(venta => venta.TotalVenta)
                })
                .OrderBy(venta => venta.Mes)
                .ToList();
            ViewBag.VentasPorMes = ventasPorMes;

            //ventas por categoría
            var ventasPorCategoria = _context.Ventas
                .Join(_context.Productos, venta => venta.ProductoId, producto => producto.Id, (venta, producto) => new { venta, producto })
                .GroupBy(data => data.producto.Categoria)
                .Select(grupo => new
                {
                    Categoria = grupo.Key,
                    TotalVentas = grupo.Sum(data => data.venta.TotalVenta)
                })
                .OrderByDescending(venta => venta.TotalVentas)
                .ToList();
            ViewBag.VentasPorCategoria = ventasPorCategoria;

            //top de 10 productos más vendidos
            var top10Productos = _context.Ventas
                .Join(_context.Productos, venta => venta.ProductoId, producto => producto.Id, (venta, producto) => new { venta, producto })
                .GroupBy(data => data.producto.NombreProducto)
                .Select(grupo => new
                {
                    Producto = grupo.Key,
                    UnidadesVendidas = grupo.Sum(data => data.venta.Cantidad)
                })
                .OrderByDescending(venta => venta.UnidadesVendidas)
                .Take(10)
                .ToList();
            ViewBag.Top10Productos = top10Productos;

            ViewData["TotalVentas"] = totalVentas;
            ViewData["TotalUnidades"] = totalUnidades;
            return View();
        }

		public IActionResult CargaArchivo()
		{ 
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

	}
}
