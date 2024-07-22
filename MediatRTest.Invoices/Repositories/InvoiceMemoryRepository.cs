using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Repositories;

public class InvoiceMemoryRepository(List<Invoice> initialInvoicesData) : IInvoiceRepository
{
    public InvoiceMemoryRepository() : this([])
    {
    }

    public string Create(Invoice invoice)
    {
        initialInvoicesData.Add(invoice);
        return invoice.Id;
    }

    public IEnumerable<Invoice> Get()
        => initialInvoicesData;

    public Invoice? Get(string id)
        => initialInvoicesData.FirstOrDefault(c => c.Id == id);

    public bool Remove(string id)
        => initialInvoicesData.RemoveAll(c => c.Id == id) > 0;
}