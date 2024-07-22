using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Repositories;

public interface IInvoiceRepository
{
    IEnumerable<Invoice> Get();

    Invoice? Get(string id);

    string Create(Invoice invoice);

    bool Remove(string id);
}