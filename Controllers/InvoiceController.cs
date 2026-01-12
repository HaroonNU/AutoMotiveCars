using AutoMotiveProject.cs.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoMotiveProject.cs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;

        public InvoiceController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GeneratePdf(int id)
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            var invoice = invoices.FirstOrDefault(i => i.Id == id);
            
            if (invoice == null)
                return NotFound();

            var pdfBytes = InvoicePdfGenerator.GenerateInvoicePdf(invoice);
            
            return File(pdfBytes, "application/pdf", $"Invoice_{invoice.InvoiceNumber}.pdf");
        }
    }
}