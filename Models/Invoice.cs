namespace AutoMotiveProject.cs.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = "";
        public DateTime BookingDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public string Phone { get; set; } = "";
        public string VehicleYear { get; set; } = "";
        public string VehicleMake { get; set; } = "";
        public string VehicleModel { get; set; } = "";
        public string RegNo { get; set; } = "";
        public string WorkDone { get; set; } = "";
        public string Recommendation { get; set; } = "";
        public string BranchAddress { get; set; } = "";
        public DateTime LastModified { get; set; }
        public string ModifiedBy { get; set; } = "";
    }
}