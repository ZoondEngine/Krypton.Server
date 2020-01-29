using Microsoft.EntityFrameworkCore;

namespace Krypton.Server.Core.Database.Contexts
{
	public class JournalContext : DbContext
	{
		public DbSet<Models.Journal> Logs { get; set; }

		public JournalContext()
		{ }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseMySql("server=localhost;UserId=root;Password=12589635Ff;database=evilcorp;");
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
