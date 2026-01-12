using AutoMotiveProject.cs.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace AutoMotiveProject.cs.Services
{
    public class GoogleSheetsDbService
    {
        private readonly SheetsService _sheetsService;
        private readonly string _spreadsheetId = "1h-I3z7Z5YihbwNkZFH2IAbC3tLVzt8D8gwkdyVb_h_Y";
        private readonly string _apiKey = "763fb843d4ef6effeb64f4252d371e2470566b2c";

        public GoogleSheetsDbService()
        {
            var credential = GoogleCredential.FromFile("service-account.json")
                .CreateScoped(SheetsService.Scope.Spreadsheets);

            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "AutoMotiveProject"
            });
        }

        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            try
            {
                var range = "Sheet1!A2:N";
                var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);
                var response = await request.ExecuteAsync();

                var invoices = new List<Invoice>();
                if (response.Values != null)
                {
                    int id = 1;
                    foreach (var row in response.Values)
                    {
                        if (row.Count >= 12)
                        {
                            invoices.Add(new Invoice
                            {
                                Id = id++,
                                InvoiceNumber = row[0]?.ToString() ?? "",
                                BookingDate = DateTime.TryParse(row[1]?.ToString(), out var bookingDate) ? bookingDate : DateTime.Now,
                                EntryDate = DateTime.TryParse(row[2]?.ToString(), out var entryDate) ? entryDate : DateTime.Now,
                                CustomerName = row[3]?.ToString() ?? "",
                                CustomerEmail = row[4]?.ToString() ?? "",
                                Phone = row[5]?.ToString() ?? "",
                                VehicleYear = row[6]?.ToString() ?? "",
                                VehicleMake = row[7]?.ToString() ?? "",
                                VehicleModel = row[8]?.ToString() ?? "",
                                RegNo = row[9]?.ToString() ?? "",
                                WorkDone = row[10]?.ToString() ?? "",
                                Recommendation = row[11]?.ToString() ?? "",
                                BranchAddress = row.Count > 12 ? row[12]?.ToString() ?? "" : "",
                                LastModified = row.Count > 13 && DateTime.TryParse(row[13]?.ToString(), out var lastMod) ? lastMod : DateTime.UtcNow,
                                ModifiedBy = row.Count > 14 ? row[14]?.ToString() ?? "" : ""
                            });
                        }
                    }
                }
                return invoices;
            }
            catch
            {
                return new List<Invoice>();
            }
        }

        public async Task AddInvoiceAsync(Invoice invoice, string currentUser)
        {
            var range = "Sheet1!A:O";
            var valueRange = new ValueRange();

            var objectList = new List<object>()
            {
                invoice.InvoiceNumber,
                invoice.BookingDate.ToString("yyyy-MM-dd"),
                invoice.EntryDate.ToString("yyyy-MM-dd"),
                invoice.CustomerName,
                invoice.CustomerEmail,
                invoice.Phone,
                invoice.VehicleYear,
                invoice.VehicleMake,
                invoice.VehicleModel,
                invoice.RegNo,
                invoice.WorkDone,
                invoice.Recommendation,
                invoice.BranchAddress,
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                currentUser
            };

            valueRange.Values = new List<IList<object>> { objectList };

            var appendRequest = _sheetsService.Spreadsheets.Values.Append(valueRange, _spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            await appendRequest.ExecuteAsync();
        }

        public async Task<bool> UpdateInvoiceAsync(Invoice invoice, int rowIndex, string currentUser, DateTime originalTimestamp)
        {
            try
            {
                // Check for conflicts first
                var currentData = await GetAllInvoicesAsync();
                var currentInvoice = currentData.ElementAtOrDefault(rowIndex);
                
                if (currentInvoice != null && currentInvoice.LastModified > originalTimestamp)
                {
                    return false; // Conflict detected
                }
                
                var range = $"Sheet1!A{rowIndex + 2}:O{rowIndex + 2}";
                var valueRange = new ValueRange();
                
                var objectList = new List<object>()
                {
                    invoice.InvoiceNumber,
                    invoice.BookingDate.ToString("yyyy-MM-dd"),
                    invoice.EntryDate.ToString("yyyy-MM-dd"),
                    invoice.CustomerName,
                    invoice.CustomerEmail,
                    invoice.Phone,
                    invoice.VehicleYear,
                    invoice.VehicleMake,
                    invoice.VehicleModel,
                    invoice.RegNo,
                    invoice.WorkDone,
                    invoice.Recommendation,
                    invoice.BranchAddress,
                    DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                    currentUser
                };
                
                valueRange.Values = new List<IList<object>> { objectList };
                
                var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, _spreadsheetId, range);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                await updateRequest.ExecuteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task DeleteInvoiceAsync(int rowIndex)
        {
            try
            {
                var range = $"Sheet1!A{rowIndex + 2}:N{rowIndex + 2}";
                var requestBody = new ClearValuesRequest();
                var deleteRequest = _sheetsService.Spreadsheets.Values.Clear(requestBody, _spreadsheetId, range);
                await deleteRequest.ExecuteAsync();
            }
            catch
            {
                // Handle error silently
            }
        }

        public string GetSheetUrl() => $"https://docs.google.com/spreadsheets/d/{_spreadsheetId}/edit";
    }
}