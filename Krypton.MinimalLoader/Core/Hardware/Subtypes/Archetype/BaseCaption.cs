﻿using System.Management;

namespace Krypton.MinimalLoader.Core.Hardware.Subtypes
{
	public abstract class BaseCaption
	{
		protected ManagementObjectSearcher SearcherInstance;

		public BaseCaption(ManagementObjectSearcher obj)
		{
			SearcherInstance = obj;
		}

		public T Get<T>(string key)
		{
			foreach(var base_object in GetSearcher().Get())
			{
				var management_object = (ManagementObject)base_object;
				if(management_object != null)
				{
					return (T)management_object[key];
				}
			}


			return (T)(object)null;
		}

		public ManagementObjectSearcher GetSearcher()
			=> SearcherInstance;
	}
}
