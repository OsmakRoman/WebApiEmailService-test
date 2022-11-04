using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;
using System.Data;
using System.Reflection.Metadata;
using WebApiEmailService.Models;

namespace WebApiEmailService.Controllers
{
    /// <summary>
    /// Main ApiController class.
    /// </summary>
    [ApiController]
    [Route("[controller]/mails")]
    public class ApiController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private DataContext context;
        /// <summary>
        /// Constructor for ApiController class.
        /// </summary>
        /// <param name="emailSender">Inject service for email sending.</param>
        /// <param name="ctx">Inject service with EF Core DataContext.</param>
        public ApiController(IEmailSender emailSender, DataContext ctx)
        {
            _emailSender = emailSender;
            this.context = ctx;
        }

        /// <summary>
        /// Async method returning all records in database (HTTP GET).
        /// </summary>
        /// <returns>All records in database.</returns>
        [HttpGet]
        public IAsyncEnumerable<DBRecord> Get()
        {
            return context.DBRecords.AsAsyncEnumerable(); 
        }

        /// <summary>
        /// Async method that handles incoming JSON data for email (HTTP POST).
        /// </summary>
        /// <param name="pm">Email data from incoming JSON.</param>
        /// <returns>ActionResult with result message.</returns>
        [HttpPost]
        public async Task<IActionResult> Set([FromBody] PostMessage pm)
        {
            if (ModelState.IsValid)
            {
                var message = new Message(pm.Resipients, pm.Subject, pm.Body);
                try
                {
                    await _emailSender.SendEmailAsync(message);
                }
                catch (Exception ex)
                {
                    await SaveDBRecord("Failed", ex.Message);
                    return Ok("!Error!: " + ex.Message);
                }
                await SaveDBRecord("OK", "");
                return Ok("Success: Email is sent");
            }

            string messages = string.Join("; ", ModelState.Values
                                                    .SelectMany(x => x.Errors)
                                                    .Select(x => x.ErrorMessage));
            await SaveDBRecord("Failed", messages);
            return Ok("!Error!: " + messages);

            async Task SaveDBRecord (string result, string errorMessage)
            {
                DBRecord dBRecord = new DBRecord();
                dBRecord.Subject = pm.Subject;
                dBRecord.Body = pm.Body;
                dBRecord.Resipients = pm.Resipients;
                dBRecord.TimeStamp = DateTime.Now;
                dBRecord.Result = result;
                dBRecord.FailedMessage = errorMessage;

                context.Add(dBRecord);
                await context.SaveChangesAsync();
            }
        }
    }
}