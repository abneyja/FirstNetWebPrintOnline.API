using FirstNetWebPrintOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstNetWebPrintOnline.Data
{
    public class FirstNetWebPrintOnlineDbContext : DbContext
    {
        public FirstNetWebPrintOnlineDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<PrintRequest> PrintRequests { get; set; }
    }
}
