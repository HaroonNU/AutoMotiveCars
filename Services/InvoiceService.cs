using AutoMotiveProject.cs.Models;

namespace AutoMotiveProject.cs.Services
{
    public class InvoiceService
    {
        private readonly GoogleSheetsDbService _sheetsDb;

        public InvoiceService(GoogleSheetsDbService sheetsDb)
        {
            _sheetsDb = sheetsDb;
        }

        public async Task<List<Invoice>> GetAllInvoicesAsync() => await _sheetsDb.GetAllInvoicesAsync();

        public List<Invoice> GetAllInvoices() 
        {
            var task = _sheetsDb.GetAllInvoicesAsync();
            task.Wait();
            return task.Result;
        }

        public async void AddInvoice(Invoice invoice)
        {
            // Generate timestamp-based invoice number: YYYYMMDDHHMMSS
            invoice.InvoiceNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
            invoice.BookingDate = invoice.BookingDate == default ? DateTime.Now : invoice.BookingDate;
            invoice.EntryDate = invoice.EntryDate == default ? DateTime.Now : invoice.EntryDate;
            await _sheetsDb.AddInvoiceAsync(invoice, "current_user");
        }

        public async Task<bool> UpdateInvoice(Invoice invoice)
        {
            var invoices = await GetAllInvoicesAsync();
            var existingInvoice = invoices.FirstOrDefault(i => i.Id == invoice.Id);
            if (existingInvoice != null)
            {
                invoice.InvoiceNumber = existingInvoice.InvoiceNumber;
                var index = invoices.IndexOf(existingInvoice);
                return await _sheetsDb.UpdateInvoiceAsync(invoice, index, "current_user", existingInvoice.LastModified);
            }
            return false;
        }

        public async void DeleteInvoice(int id)
        {
            var invoices = await GetAllInvoicesAsync();
            var invoice = invoices.FirstOrDefault(i => i.Id == id);
            if (invoice != null)
            {
                var index = invoices.IndexOf(invoice);
                await _sheetsDb.DeleteInvoiceAsync(index);
            }
        }

        public string GetSheetsUrl() => _sheetsDb.GetSheetUrl();
    }
}