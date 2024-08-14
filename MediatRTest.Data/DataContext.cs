using MediatRTest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MediatRTest.Data;

public class DataContext : DbContext
{
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.BussinsId).IsUnique();
        
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Customer)
            .WithMany(i => i.Invoices)
            .HasForeignKey(i => i.CustomerId).OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        modelBuilder.Entity<Invoice>()
            .HasMany(i => i.Items)
            .WithOne()
            .HasForeignKey(item => item.InvoiceId).OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Invoice>()
            .Property<DateTime>("LastUpdated");

        // Seed data - moved to a separate migration
        // modelBuilder.Entity<Customer>().HasData(
        //     new Customer { CustomerId = 1, Name = "Customer 1", Address = "Address 1" },
        //     new Customer { CustomerId = 2, Name = "Customer 2", Address = "Address 2" }
        // );
        //
        // modelBuilder.Entity<Invoice>().HasData(
        //     new Invoice
        //     {
        //         InvoiceId = 1,
        //         BussinsId = "a9be413b-8ec3-4156-bd5c-07c17f1928ea",
        //         InvoiceNumber = "FV/01/2024",
        //         Amount = 100.00,
        //         InvoiceDate = new DateTime(2024, 08, 01),
        //         DueDate = new DateTime(2024, 08, 01).AddDays(30),
        //         Currency = "USD",
        //         CustomerId = 1,
        //     },
        //     new Invoice
        //     {
        //         InvoiceId = 2,
        //         BussinsId = "f7a8356e-41a7-4801-987e-a35b9fed2e6f",
        //         InvoiceNumber = "FV/02/2024",
        //         Amount = 200.00,
        //         InvoiceDate = DateTime.Now.Date,
        //         DueDate = DateTime.Now.Date.AddDays(30),
        //         Currency = "USD",
        //         CustomerId = 1
        //     },
        //     new Invoice
        //     {
        //         InvoiceId = 3,
        //         BussinsId = "b8662e75-66f4-4cca-96f2-79b33b4db655",
        //         InvoiceNumber = "FV/03/2024",
        //         Amount = 500.50,
        //         InvoiceDate = new DateTime(2024, 08, 10),
        //         DueDate = new DateTime(2024, 09, 01),
        //         Currency = "USD",
        //         CustomerId = 2
        //     }
        // );
        //
        // modelBuilder.Entity<InvoiceItem>().HasData(
        //     new InvoiceItem
        //     {
        //         InvoiceItemId = 1,
        //         InvoiceId = 1,
        //         Description = "Item 1",
        //         Amount = 50.00,
        //         Quantity = 1
        //     },
        //     new InvoiceItem
        //     {
        //         InvoiceItemId = 2,
        //         InvoiceId = 1,
        //         Description = "Item 2",
        //         Amount = 25.00,
        //         Quantity = 2
        //     },
        //     new InvoiceItem
        //     {
        //         InvoiceItemId = 3,
        //         InvoiceId = 2,
        //         Description = "Item 1",
        //         Amount = 200.00,
        //         Quantity = 1
        //     },
        //     new InvoiceItem
        //     {
        //         InvoiceItemId = 4,
        //         InvoiceId = 3,
        //         Description = "Item 1",
        //         Amount = 200.50,
        //         Quantity = 1
        //     },
        //     new InvoiceItem
        //     {
        //         InvoiceItemId = 5,
        //         InvoiceId = 3,
        //         Description = "Item 2",
        //         Amount = 300.00,
        //         Quantity = 1
        //     }
        // );
    }

    public override int SaveChanges()
    {
        UpdateAuditData();
        return base.SaveChanges();
    }

    private void UpdateAuditData()
    {
        foreach (var entry in ChangeTracker.Entries<Invoice>())
        {
            entry.Property("LastUpdated").CurrentValue = DateTime.Now;
        }
    }
}