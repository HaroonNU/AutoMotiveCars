using AutoMotiveProject.cs.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Borders;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace AutoMotiveProject.cs.Services
{
    public class InvoicePdfGenerator
    {
        private static readonly Color PRIMARY_COLOR = new DeviceRgb(70, 130, 180); // Steel Blue
        
        public static byte[] GenerateInvoicePdf(Invoice invoice)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new PdfWriter(stream))
                using (var pdf = new PdfDocument(writer))
                using (var document = new Document(pdf))
                {
                    document.SetMargins(20, 20, 20, 20);

                    // Header
                    document.Add(new Paragraph("Mama Autos")
                        .SetBold()
                        .SetFontSize(18)
                        .SetFontColor(PRIMARY_COLOR)
                        .SetTextAlignment(TextAlignment.CENTER));

                    document.Add(new Paragraph(
                        "174 Clarence Street Howrah, 7018 | 289-291 Main Road Glenorchy, 7010")
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER));

                    document.Add(new Paragraph("\n\n"));

                    document.Add(new Paragraph($"Invoice #: {invoice.InvoiceNumber}")
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.RIGHT));

                    document.Add(new Paragraph($"Booking Date: {invoice.BookingDate:yyyy-MM-dd}")
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.RIGHT));

                    document.Add(new Paragraph($"Entry Date: {invoice.EntryDate:yyyy-MM-dd}")
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.RIGHT));

                    document.Add(new Paragraph("\n\n"));

                    // Customer Details Table
                    Table customerTable = new Table(2).UseAllAvailableWidth();

                    AddCell(customerTable, "Customer Name", true);
                    AddCell(customerTable, invoice.CustomerName);

                    AddCell(customerTable, "Email", true);
                    AddCell(customerTable, invoice.CustomerEmail);

                    AddCell(customerTable, "Phone", true);
                    AddCell(customerTable, invoice.Phone);

                    document.Add(customerTable);
                    document.Add(new Paragraph("\n\n"));

                    // Vehicle Details Table
                    Table vehicleTable = new Table(4).UseAllAvailableWidth();

                    AddCell(vehicleTable, "Year", true);
                    AddCell(vehicleTable, invoice.VehicleYear);

                    AddCell(vehicleTable, "Make", true);
                    AddCell(vehicleTable, invoice.VehicleMake);

                    AddCell(vehicleTable, "Model", true);
                    AddCell(vehicleTable, invoice.VehicleModel);

                    AddCell(vehicleTable, "Reg No", true);
                    AddCell(vehicleTable, invoice.RegNo);

                    document.Add(vehicleTable);
                    document.Add(new Paragraph("\n\n"));

                    // Work Description
                    Table workTable = new Table(new float[] { 1, 9 }).UseAllAvailableWidth();

                    AddCell(workTable, "#", true);
                    AddCell(workTable, "Work Description", true);

                    AddCell(workTable, "1");
                    AddCell(workTable, invoice.WorkDone);

                    document.Add(workTable);
                    document.Add(new Paragraph("\n\n"));

                    // Technician Recommendation
                    document.Add(new Paragraph("Technician Recommendation")
                        .SetBold()
                        .SetFontColor(PRIMARY_COLOR)
                        .SetFontSize(12));

                    Table techTable = new Table(1).UseAllAvailableWidth();
                    techTable.AddCell(new Cell()
                        .Add(new Paragraph(invoice.Recommendation))
                        .SetMinHeight(40)
                        .SetPadding(10)
                        .SetBorder(new SolidBorder(PRIMARY_COLOR, 1)));

                    document.Add(techTable);
                }
                return stream.ToArray();
            }
        }

        private static void AddCell(Table table, string text, bool isHeader = false)
        {
            var cell = new Cell().Add(new Paragraph(text)).SetFontSize(10).SetBorder(new SolidBorder(1)).SetPadding(8);
            if (isHeader)
            {
                cell.SetBold().SetBackgroundColor(new DeviceRgb(70, 130, 180)).SetFontColor(ColorConstants.WHITE);
            }
            table.AddCell(cell);
        }
    }
}