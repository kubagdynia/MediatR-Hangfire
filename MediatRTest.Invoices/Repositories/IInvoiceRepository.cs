using MediatRTest.Invoices.Models;

namespace MediatRTest.Invoices.Repositories;

public interface IInvoiceRepository
{
    IEnumerable<DbInvoice> Get();

    DbInvoice? Get(string id);

    string Create(DbInvoice invoice);

    bool Remove(string id);
}