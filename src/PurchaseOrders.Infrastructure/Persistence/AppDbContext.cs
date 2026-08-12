using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PurchaseOrders.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<StatusHistory> StatusHistory => Set<StatusHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- User: auto-referencia Supervisor ---
            modelBuilder.Entity<User>()
                .HasOne(u => u.Supervisor)
                .WithMany(u => u.Subordinates)
                .HasForeignKey(u => u.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict); // evita ciclos de cascada

            // --- Product -> Supplier ---
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- PurchaseOrder -> Employee (User) ---
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(o => o.Employee)
                .WithMany(u => u.PurchaseOrders)
                .HasForeignKey(o => o.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- PurchaseOrder -> Supplier ---
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(o => o.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(o => o.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- OrderItem -> PurchaseOrder (sí cascadea: si borro la orden, se borran sus items) ---
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.PurchaseOrder)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- OrderItem -> Product ---
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Invoice -> PurchaseOrder (cascada) ---
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.PurchaseOrder)
                .WithMany(o => o.Invoices)
                .HasForeignKey(i => i.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- StatusHistory -> PurchaseOrder (cascada) ---
            modelBuilder.Entity<StatusHistory>()
                .HasOne(sh => sh.PurchaseOrder)
                .WithMany(o => o.StatusHistory)
                .HasForeignKey(sh => sh.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- StatusHistory -> User (quién hizo el cambio) ---
            modelBuilder.Entity<StatusHistory>()
                .HasOne(sh => sh.User)
                .WithMany()
                .HasForeignKey(sh => sh.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Precisión de campos monetarios (evita el warning de EF Core) ---
            modelBuilder.Entity<Product>().Property(p => p.ReferencePrice).HasPrecision(18, 2);
            modelBuilder.Entity<PurchaseOrder>().Property(o => o.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.Amount).HasPrecision(18, 2);

            // --- Índice único: no puede haber dos órdenes con el mismo número ---
            modelBuilder.Entity<PurchaseOrder>()
                .HasIndex(o => o.Number)
                .IsUnique();

            // --- Índice único: no puede haber dos usuarios con el mismo email ---
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
