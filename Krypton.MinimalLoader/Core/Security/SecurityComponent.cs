using Krypton.MinimalLoader.Core.Security.Checks;
using Krypton.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Krypton.MinimalLoader.Core.Security
{
	public class SecurityComponent : KryptonComponent<SecurityComponent>
	{
		private List<ISecurityCheck> Checkers;

		public SecurityComponent()
		{
			Checkers = GetCheckers();
		}

		public bool Check(out string message)
		{
			for(int i = 0; i < Checkers.Count; i++)
			{
				if(!Checkers[i].Check(out message))
				{
					return false;
				}
			}

			message = "";
			return true;
		}

		private List<ISecurityCheck> GetCheckers()
		{
			return Assembly
					.GetExecutingAssembly()
					.GetTypes()
					.Where(m => m.GetInterfaces().Contains(typeof(ISecurityCheck)))
					.Select(m => m.GetConstructor(Type.EmptyTypes).Invoke(null) as ISecurityCheck)
					.ToList();
		}
	}
}
