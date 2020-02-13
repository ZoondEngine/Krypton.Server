using Krypton.Server.Core.MMF.Details;
using Krypton.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.MMF
{
	public class MMFComponent : KryptonComponent<MMFComponent>
	{
		private List<MappedFile> m_mapped_files;

		public MMFComponent()
		{
			m_mapped_files = new List<MappedFile>();
		}

		public MappedFile Map(string path)
		{
			var print = IO.IOMgr.Instance.GetPrint();

			try
			{
				var mapped = new MappedFile();
				if(mapped.Create(path))
				{
					if(!m_mapped_files.Contains(mapped))
					{
						m_mapped_files.Add(mapped);

						return mapped;
					}
					else
					{
						print.Error($"File: '{path}' already mapped in system memory");
					}
				}
			}
			catch(FileNotFoundException fnfe)
			{
				print.Error($"File: '{path}' not founded on hardrive for mapping in system memory");
				print.Error($"Exception: {fnfe}");
			}

			return null;
		}
		public MappedFile OpenExisting(string path)
			=> m_mapped_files.FirstOrDefault((x) => x.GetComparableString().ToLower() == path.ToLower());
	}
}
