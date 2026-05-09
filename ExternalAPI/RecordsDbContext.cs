using ExternalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExternalAPI
{
    public class RecordsDbContext(DbContextOptions<RecordsDbContext> options) : DbContext(options)
    {
        public DbSet<Record> Records => Set<Record>();

    }
}
