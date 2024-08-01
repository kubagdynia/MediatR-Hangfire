namespace MediatRTest.Api.Invoices.Contracts.V1;

public record InvoiceResponse : Invoice
{
    /// <summary>
    /// Invoice id
    /// </summary>
    public string Id { get; set; } = string.Empty;
}