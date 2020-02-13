using Jareem.Network.Systems.Tcp.Observeable.Providers;
using Jareem.Network.Systems.Tcp.Server;
using Jareem.Support.Implements.Observer.Base;
using Krypton.Server.Core.Network.Boots.NetworkCommands;
using System;
using System.Collections.Generic;

namespace Krypton.Server.Core.Network.Boots.NetworkHandlers
{
	public class PacketsBootHandler : Contracts.IServerNetworkModule
	{
		private List<Action<TcpServer>> Commands = new List<Action<TcpServer>>()
		{
			(service) => service.Subscribe<TcpConnectionProvider, BaseData>(new OnConnectionCommand()),
			(service) => service.Subscribe<TcpReceivingProvider, BaseData>(new OnSetGuidCommand()),
			(service) => service.Subscribe<TcpReceivingProvider, BaseData>(new OnSetKeyCommand()),
			(service) => service.Subscribe<TcpReceivingProvider, BaseData>(new OnSetUpdateCommand()),
		};

		private int AddSubscribers(NetworkComponent network)
		{
			foreach(var module in Commands)
			{
				module(network.GetService());
			}

			return Commands.Count;
		}

		public void MountUp(NetworkComponent network)
		{
			var print = network.GetIO().GetPrint();
			print.Trace($"Registering network handlers: {AddSubscribers(network)}");
		}

		public void MountDown(NetworkComponent network)
		{
			var print = network.GetIO().GetPrint();
			print.Trace($"Destroying network handlers ...");
		}
	}
}
