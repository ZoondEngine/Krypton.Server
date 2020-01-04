using Krypton.Server.Core.Network.Boots.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Krypton.Server.Core.Network
{
	public class BootstrapHelper
	{
		private List<IServerNetworkModule> _cachedModules;
		private NetworkComponent _serviceInstance;

		public bool IsBooted;

		public BootstrapHelper(NetworkComponent instance)
		{
			_serviceInstance = instance;
			IsBooted = false;
		}

		public void MountAll()
		{
			if (_cachedModules == null)
				_cachedModules = LoadAllModules();

			foreach (var module in _cachedModules)
				module.MountUp(_serviceInstance);

			IsBooted = true;
		}

		public void Mount<T>()
		{
			if (_cachedModules == null)
				_cachedModules = LoadAllModules();

			foreach (var module in _cachedModules)
				if (module.GetType() == typeof(T))
					module.MountUp(_serviceInstance);

			IsBooted = true;
		}

		public void Unmount()
		{
			if (_cachedModules != null)
			{
				foreach (var module in _cachedModules)
					module.MountDown(_serviceInstance);

				IsBooted = false;
			}
		}

		private List<IServerNetworkModule> LoadAllModules()
		{
			return Assembly
					.GetExecutingAssembly()
					.GetTypes()
					.Where(m => m.GetInterfaces().Contains(typeof(IServerNetworkModule)))
					.Select(m => m.GetConstructor(Type.EmptyTypes).Invoke(null) as IServerNetworkModule)
					.ToList();
		}
	}
}
