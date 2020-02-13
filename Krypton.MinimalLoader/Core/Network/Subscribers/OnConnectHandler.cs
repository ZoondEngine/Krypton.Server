using Jareem.Network.Systems.Tcp.Observeable.Providers.TcpConnection;
using Jareem.Support.Implements.Observer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.MinimalLoader.Core.Network.Subscribers
{
	public class OnConnectHandler : IObserver<BaseData>
	{
		public void OnCompleted()
		{

		}

		public void OnError(Exception error)
		{

		}

		public void OnNext(BaseData value)
		{
			var conn = value.As<TcpConnection>();

			if (conn.IsConnected)
			{
				//Console.WriteLine($"Connected: {conn.Identifier}");
			}
			else
			{
				//Console.WriteLine($"Disconnected: {conn.Identifier}");
			}
		}
	}
}
