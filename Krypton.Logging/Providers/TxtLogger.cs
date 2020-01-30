using System;
using System.IO;

namespace Krypton.Logging.Providers
{
	public class TxtLogger : Contracts.IKryptonLogger
	{
		private string m_folder {get; set;}
		public void Build(string folder)
		{
			if(Directory.Exists(folder))
			{
				Directory.Delete(folder, true);
			}

			Directory.CreateDirectory(folder);
			m_folder = folder;
		}

		public void Debug(string text, string filename = "debug.txt")
			=> ToFile($"[{DateTime.Now}][DEBUG] >> {text}", filename);

		public void Error(string text, string filename = "errors.txt")
			=> ToFile($"[{DateTime.Now}][ERROR] >> {text}", filename);

		public void Success(string text, string filename = "success.txt")
			=> ToFile($"[{DateTime.Now}][TRACE] >> {text}", filename);

		public void Warning(string text, string filename = "warnings.txt")
			=> ToFile($"[{DateTime.Now}][WARNING] >> {text}", filename);

		private void ToFile(string ready, string filename)
		{
			if (!Directory.Exists(m_folder))
			{
				Directory.CreateDirectory(m_folder);
			}

			File.AppendAllText(m_folder + "\\" + filename, ready);
		}
	}
}
