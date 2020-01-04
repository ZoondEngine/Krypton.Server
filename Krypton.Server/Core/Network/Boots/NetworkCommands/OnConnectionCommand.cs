using Jareem.Network.Systems.Tcp.Observeable.Providers.TcpConnection;
using Jareem.Support.Implements.Observer.Base;
using System;

namespace Krypton.Server.Core.Network.Boots.NetworkCommands
{
	internal class OnConnectionCommand : IObserver<BaseData>
	{
		public void OnCompleted()
		{
			NetworkComponent.Instance.GetIO().GetPrint().Warning("Network module has been stopped");
		}

		public void OnError(Exception error)
		{
			NetworkComponent.Instance.GetIO().GetPrint().Error($"Internal exception in network module:\n {error}");
		}

		public void OnNext(BaseData value)
		{
			if(value == null)
			{
				NetworkComponent.Instance.GetIO().GetPrint().Trace($"Client has been disconnected with internal error");
				return;
			}

			var conn = value.As<TcpConnection>();
			if(conn.NativeClient.Connected)
			{
				NetworkComponent.Instance.GetIO().GetPrint().Trace($"Client: {conn.Identifier} has been accepted");
			}
			else
			{
				NetworkComponent.Instance.GetIO().GetPrint().Trace($"Client: {conn.Identifier} has been disconnected");
			}
		}
	}
}
