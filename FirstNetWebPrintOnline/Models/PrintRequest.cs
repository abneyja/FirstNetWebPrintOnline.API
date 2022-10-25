namespace FirstNetWebPrintOnline.Models
{
    public class PrintRequest
    {
        public Guid Id { get; set; }
        public string Ordernumber { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Timestamp { get; set; }
        public int Timestale { get; set; }
        public Boolean Printsonar { get; set; }
        public Boolean Printphone { get; set; }
        public Boolean Printorder { get; set; }
        public Boolean Printbarcodes { get; set; }
        public Boolean Printcoam { get; set; }
        public Boolean Automode { get; set; }
        public string Status { get; set; }

    }
}
