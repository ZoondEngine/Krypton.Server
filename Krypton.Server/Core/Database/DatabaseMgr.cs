using Krypton.Server.Core.Database.Configuration;
using Krypton.Server.Core.IO;
using Krypton.Support;
using Krypton.Support.CodeAnalyzer;

namespace Krypton.Server.Core.Database
{
	public class DatabaseMgr : KryptonComponent<DatabaseMgr>
	{
		private IOMgr IO;
		private DatabaseConfiguration m_config;

		public DatabaseMgr()
		{
			m_config = Ini.IniComponent.Instance.GetByName("database-native-configuration").As<DatabaseConfiguration>();
		}

		public void OnServerInitialized(IOMgr io)
		{
			using (Analyze.Watch("Database modules initialization"))
			{
				IO = io;
				var cmds = Commands.CommandsMap.DatabaseCommands;

				IO.GetPrint().Trace("Database module successfully intended.");
				IO.GetPrint().Trace($"Database added '{cmds.Count}' implemented commands");
				IO.GetPrint().Trace("User '.db help' for getting database commands list");
			}
		}

		public DatabaseConfiguration GetConfig()
			=> m_config;

		public Contexts.KeysContext GetKeysContext()
				=> new Contexts.KeysContext();

		public Contexts.JournalContext GetJournal()
			=> new Contexts.JournalContext();
	}
}
