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
			var cfg = DatabaseMgr.Instance.GetConfig();
			var host = cfg.Read<string>("host");
			var user = cfg.Read<string>("user");
			var password = cfg.Read<string>("password");
			var db = cfg.Read<string>("database");

			options.UseMySql($"server={host};UserId={user};Password={password};database={db};");
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
