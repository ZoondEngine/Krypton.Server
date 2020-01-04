using System;

namespace Krypton.Network.Packetize
{
	public enum Packets
	{
		AuthRequest = 1,
		AuthResponse = 2,
	}

	public class BaseNetworkable
	{
		public int Identifier { get; set; }

		public string ToNetwork()
			=> Newtonsoft.Json.JsonConvert.SerializeObject(this);

		public T Convert<T>() where T : BaseNetworkable
				=> (T)this;
	}
}
