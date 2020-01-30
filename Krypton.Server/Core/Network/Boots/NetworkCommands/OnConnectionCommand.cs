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
			NetworkComponent.Instance.GetLog()?.Error($"Internal network core error: {error.ToString()}");
			NetworkComponent.Instance.GetIO().GetPrint().Error($"Internal exception in network module:\n {error}");
		}

		public void OnNext(BaseData value)
		{
			var log = NetworkComponent.Instance.GetLog();
			if (value == null)
			{
				log?.Error("Unknown client(value == null) has been disconnected with internal error");
				NetworkComponent.Instance.GetIO().GetPrint().Trace($"Client has been disconnected with internal error");
				return;
			}

			var conn = value.As<TcpConnection>();
			if(conn.NativeClient.Connected)
			{
				log?.Debug($"Client: {conn.Identifier} has been accepted. EndPoint: {conn.NativeClient.Client.RemoteEndPoint}");
				NetworkComponent.Instance.GetIO().GetPrint().Trace($"Client: {conn.Identifier} has been accepted");
			}
			else
			{
				log?.Debug($"Client: {conn.Identifier} has been disconnected");
				NetworkComponent.Instance.GetIO().GetPrint().Trace($"Client: {conn.Identifier} has been disconnected");
			}
		}
	}
}
