using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Repositories;

public class InvoiceMemoryRepository(List<DbInvoice> initialInvoicesData) : IInvoiceRepository
{
    public InvoiceMemoryRepository() : this([])
    {
    }

    public string Create(DbInvoice invoice)
    {
        initialInvoicesData.Add(invoice);
        return invoice.Id;
    }

    public IEnumerable<DbInvoice> Get()
        => initialInvoicesData;

    public DbInvoice? Get(string id)
        => initialInvoicesData.FirstOrDefault(c => c.Id == id);

    public bool Remove(string id)
        => initialInvoicesData.RemoveAll(c => c.Id == id) > 0;
}