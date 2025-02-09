﻿using Microsoft.EntityFrameworkCore;
using MockPruebaTecnica.Models;

namespace MockPruebaTecnica.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }


    }

}
