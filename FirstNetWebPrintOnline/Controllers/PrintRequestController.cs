using FirstNetWebPrintOnline.Models;
using FirstNetWebPrintOnline.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Diagnostics;

namespace FirstNetWebPrintOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrintRequestsController : Controller
    {
        private readonly FirstNetWebPrintOnlineDbContext _firstNetWebPrintOnlineDbContext;

        public PrintRequestsController(FirstNetWebPrintOnlineDbContext firstNetWebPrintOnlineDbContext)
        {
            _firstNetWebPrintOnlineDbContext = firstNetWebPrintOnlineDbContext;
        }

        [HttpGet]
        [Authorize("read:printrequests")]
        public async Task<IActionResult> GetAllPrintRequests()
        {
            var qString = "SELECT * FROM PrintRequests ORDER by CAST(Timestamp as datetime) DESC";

            var printRequests = await _firstNetWebPrintOnlineDbContext.PrintRequests.FromSqlRaw(qString).ToListAsync();
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

            foreach (var printRequest in printRequests)
            {
                var myDateArray = printRequest.Timestamp.Split(' ');
                DateTime myDate = DateTime.ParseExact(myDateArray[0], "MM/dd/yy",
                                       Thread.CurrentThread.CurrentCulture);

                DateTime myDateNow = DateTime.Now.Date;

                if(printRequest.Timestale != (myDateNow - myDate).Days) 
                {
                    printRequest.Timestale = (myDateNow - myDate).Days;

                    _firstNetWebPrintOnlineDbContext.PrintRequests.Update(printRequest);
                    await _firstNetWebPrintOnlineDbContext.SaveChangesAsync();
                }
            }

            return Ok(printRequests);
        }

        [HttpPost]
        [Authorize("write:printrequest")]
        public async Task<IActionResult> AddPrintRequest([FromBody] PrintRequest printRequest)
        {
            printRequest.Id = Guid.NewGuid();

            var addr = new MailAddress(printRequest.Username);            

            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            printRequest.Username = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(addr.User.Replace('.', ' ').ToLower());

            printRequest.Timestamp = TimeZoneInfo.ConvertTime(DateTime.Now,
                 TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time")).ToString("MM/dd/yy hh:mm:ss tt");

            await _firstNetWebPrintOnlineDbContext.PrintRequests.AddAsync(printRequest);
            await _firstNetWebPrintOnlineDbContext.SaveChangesAsync();
            return Ok(printRequest);
        }

        [HttpDelete]
        [Authorize("delete:printrequest")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeletePrintRequest([FromRoute] Guid id)
        {
            var printRequest = await _firstNetWebPrintOnlineDbContext.PrintRequests.FindAsync(id);

            if (printRequest == null)
            {
                return NotFound();
            }

            _firstNetWebPrintOnlineDbContext.PrintRequests.Remove(printRequest);

            await _firstNetWebPrintOnlineDbContext.SaveChangesAsync();

            return Ok(printRequest);
        }
    }
}
