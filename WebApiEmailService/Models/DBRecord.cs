using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApiEmailService.Models
{
    /// <summary>
    /// DBRecord entity class.
    /// </summary>
    public class DBRecord
    {
        /// <summary>
        /// ID for database.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Field Subject from email.
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Field Body from email.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Field Resipients from email (array of strings).
        /// Not mapped to database.
        /// </summary>
        [NotMapped]
        public string[] Resipients 
            {
                get
                {
                     var tab = this.ResipientsDb.Split(',');
                     return tab.ToArray();
                }
                set
                {
                     this.ResipientsDb = string.Join(",", value);
                }
            }
        /// <summary>
        /// Field Resipients from email for database as a string.
        /// Not include in JSON result when all records are requested.
        /// </summary>
        [JsonIgnore]
        public string ResipientsDb {get; set; }
        /// <summary>
        /// Moment of time when DBRecord is added to database.
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Result of sending email ("OK" or "Failed").
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// Error message (if none, then "").
        /// </summary>
        public string FailedMessage { get; set; }
    }
}
