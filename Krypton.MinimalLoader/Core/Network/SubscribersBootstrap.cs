using Jareem.Network.Systems.Tcp.Observeable.Providers;
using Jareem.Support.Implements.Observer.Base;
using Krypton.MinimalLoader.Core.Network.Subscribers;
using System;
using System.Collections.Generic;

namespace Krypton.MinimalLoader.Core.Network
{
	public class SubscribersBootstrap
	{
		private List<Action<NetworkComponent>> Subscribers = new List<Action<NetworkComponent>>()
		{
			(client) => client.GetService().Subscribe<TcpConnectionProvider, BaseData>(new OnConnectHandler()),
		};

		public void MountUp(NetworkComponent comp)
		{
			foreach(var sub in Subscribers)
			{
				sub(comp);
			}
		}
	}
}
