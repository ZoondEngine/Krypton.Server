using Krypton.Server.Core.Ini;
using Krypton.Server.Core.IO;
using Krypton.Support;
using Krypton.Support.CodeAnalyzer;
using Krypton.Support.ComponentModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Krypton.Server
{
	public class ServerBootstrap : KryptonComponent<ServerBootstrap>
	{
		public ServerBootstrap()
		{
			var io = Core.IO.IOMgr.Instance;
			var ini = Core.Ini.IniComponent.Instance;
			var network = Core.Network.NetworkComponent.Instance;
			var database = Core.Database.DatabaseMgr.Instance;
			var log = Core.Log.LogComponent.Instance;
			var updaing = Core.Updating.UpdatingComponent.Instance;

			network.OnServerInitialized(io);
			database.OnServerInitialized(io);

			io.RunPrompt();
		}

		public void Build()
		{
			//m_components = Assembly
			//	.GetExecutingAssembly()
			//	.GetTypes()
			//	.Where(m => m.GetInterfaces().Contains(typeof(IKryptonComponent)))
			//	.Select(m => m.GetConstructor(Type.EmptyTypes).Invoke(null) as IKryptonComponent)
			//	.ToList();
		}

		public void Boot()
		{
			CallAll("OnServerInitialized");
		}

		public void Stop()
		{
			CallAll("OnServerShutdown");
		}

		private void CallAll(string hook_name, bool injecting = true)
		{
			//using (Analyze.Watch("Server initialization"))
			//{
			//	var components = m_components;

			//	for (var i = 0; i < components.Count; i++)
			//	{
			//		var method = components[i].GetType().GetMethod(hook_name);
			//		if (injecting & method != null)
			//		{
			//			var parameters = method.GetParameters();
			//			var required = new object[parameters.Length];

			//			for (var j = 0; j < parameters.Length; j++)
			//			{
			//				for (var k = 0; k < components.Count; k++)
			//				{
			//					if (components[k].GetType() == parameters[j].ParameterType)
			//					{
			//						required[j] = components[k];
			//					}
			//				}
			//			}

			//			method?.Invoke(components[i], required);
			//		}
			//		else
			//		{
			//			method?.Invoke(components[i], null);
			//		}
			//	}
			//}

			//Must be after all components
			//GetComponent<IOMgr>().RunPrompt();
		}
	}
}
