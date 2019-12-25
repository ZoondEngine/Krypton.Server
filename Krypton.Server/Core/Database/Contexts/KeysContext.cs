using Krypton.Server.Core.Database.Logging;
using Krypton.Server.Core.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Krypton.Server.Core.Database.Contexts
{
    public class KeysContext : DbContext
    {
        public DbSet<KeyPacket> KeyPackets { get; set; }
        public DbSet<Key> Keys { get; set; }

        public KeysContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql("server=localhost;UserId=root;Password=;database=evilcorp;");
            options.UseLoggerFactory(DatabaseLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public static readonly ILoggerFactory DatabaseLoggerFactory
            = LoggerFactory.Create(builder => builder.AddProvider(new KeysContextLogger()));
    }
}
