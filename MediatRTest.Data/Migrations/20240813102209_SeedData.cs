using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediatRTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed data - moved from DataContext to a separate migration.
            // Using Database.EnsureCreatedAsync in tests will not trigger this migration and the database
            // will be without redundant data which will facilitate testing.
            
            // Seed data for Customers
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Name", "Address" },
                values: new object[,]
                {
                    { 1, "Customer 1", "Address 1" },
                    { 2, "Customer 2", "Address 2" }
                });

            // Seed data for Invoices
            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[]
                {
                    "InvoiceId", "BussinsId", "InvoiceNumber", "Amount", "InvoiceDate", "DueDate", "Currency",
                    "CustomerId"
                },
                values: new object[,]
                {
                    {
                        1, "a9be413b-8ec3-4156-bd5c-07c17f1928ea", "FV/01/2024", 100.00, new DateTime(2024, 08, 01),
                        new DateTime(2024, 08, 01).AddDays(30), "USD", 1
                    },
                    {
                        2, "f7a8356e-41a7-4801-987e-a35b9fed2e6f", "FV/02/2024", 200.00, DateTime.Now.Date,
                        DateTime.Now.Date.AddDays(30), "USD", 1
                    },
                    {
                        3, "b8662e75-66f4-4cca-96f2-79b33b4db655", "FV/03/2024", 500.50, new DateTime(2024, 08, 10),
                        new DateTime(2024, 09, 01), "USD", 2
                    }
                });
            
            migrationBuilder.InsertData(
                table: "InvoiceItems",
                columns: new[] { "InvoiceItemId", "InvoiceId", "Description", "Amount", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, "Item 1", 50.00, 1 },
                    { 2, 1, "Item 2", 25.00, 2 },
                    { 3, 2, "Item 1", 200.00, 1 },
                    { 4, 3, "Item 1", 200.50, 1 },
                    { 5, 3, "Item 2", 300.00, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove seed data for Invoices
            migrationBuilder.DeleteData("Invoices", "InvoiceId", 1);
            migrationBuilder.DeleteData("Invoices", "InvoiceId", 2);
            migrationBuilder.DeleteData("Invoices", "InvoiceId", 3);
            
            // Remove seed data for Customers
            migrationBuilder.DeleteData("Customers", "CustomerId", 1);
            migrationBuilder.DeleteData("Customers", "CustomerId", 2);
        }
    }
}
