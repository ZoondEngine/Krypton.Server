using Krypton.Server.Core.IO.Contracts;
using System;
using System.Collections.Generic;

namespace Krypton.Server.Core.Database.Commands
{
	public class KeysClearOldCommand : ICommandElement
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Level { get; set; }

		public KeysClearOldCommand()
		{
			Name = "Keys clearing helper";
			Description = "Command for clearing old keys and packets. Required Administrator level\nAttention! You can delete all keys from your database";
			Level = 2;
		}

		public string GetHelp()
			=> ".db keys clear | .db keys clear old";

		public bool IsHandlable(string line)
			=> line.ToLower().Contains(".db keys clear");

		public bool Run(string line)
		{
			var log = Log.LogComponent.Instance;

			if (line.ToLower().Contains(".db keys clear old"))
			{

				var keys_context = DatabaseMgr.Instance.GetKeysContext();
				var keys = keys_context.Keys;
				List<Models.Key> removable_keys = new List<Models.Key>();

				foreach(var key in keys)
				{
					if(key.EndAt < DateTime.Now)
					{
						removable_keys.Add(key);
					}
				}

				int count = removable_keys.Count;
				keys.RemoveRange(removable_keys);
				keys_context.SaveChanges();

				IO.IOMgr.Instance.GetPrint().Success($"Removed {count} old keys");
				log.Warning($"Server has been delete old keys. Count: {count}");

				return true;
			}
			else if(line.ToLower().Contains(".db keys clear"))
			{
				var print = IO.IOMgr.Instance.GetPrint();
				print.Warning("Are you sure wanna delete all keys? [y/n]");
				string user_answer = Console.ReadLine();
				if (user_answer.Length == 1 & user_answer.ToLower().Contains("y"))
				{
					var keys_context = DatabaseMgr.Instance.GetKeysContext();
					var keys = keys_context.Keys;
					List<Models.Key> removable_keys = new List<Models.Key>();

					foreach (var key in keys)
					{
						removable_keys.Add(key);
					}

					var packets = keys_context.KeyPackets;
					List<Models.KeyPacket> removable_packets = new List<Models.KeyPacket>();
					foreach (var packet in packets)
					{
						removable_packets.Add(packet);
					}

					keys.RemoveRange(removable_keys);
					packets.RemoveRange(removable_packets);
					keys_context.SaveChanges();

					IO.IOMgr.Instance.GetPrint().Success($"Removed all keys and key packets");
					log.Error($"SERVER HAS BEEN DELETE ALL KEYS");

					return true;
				}
				else
				{
					print.Success("Deleting keys and key packets cancelled");
					return true;
				}
			}
			else
			{
				return false;
			}
		}
	}
}
