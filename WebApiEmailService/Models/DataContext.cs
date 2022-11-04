using Microsoft.EntityFrameworkCore;

namespace WebApiEmailService.Models
{
    /// <summary>
    /// EF Core context class.
    /// </summary>
    public class DataContext: DbContext
    {
        /// <summary>
        /// Constructor for DataContext class.
        /// </summary>
        /// <param name="opts">Options passed to the base class.</param>
        public DataContext(DbContextOptions<DataContext> opts)
            : base(opts) { }
        /// <summary>
        /// DbSet property for DBRecords.
        /// </summary>
        public DbSet<DBRecord> DBRecords => Set<DBRecord>();
    }
}
