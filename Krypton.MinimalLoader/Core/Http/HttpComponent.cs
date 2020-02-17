using Krypton.Support;
using System;
using System.IO;
using System.Net;
using System.Threading;

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

		public bool Download(string addr, out string file_path, bool hidden = true)
		{
			Reset();

			var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\temporary";

			try
			{
				if (!Directory.Exists(directory))
				{
					var dir = Directory.CreateDirectory(directory);
					dir.Attributes = FileAttributes.Hidden | FileAttributes.Directory | FileAttributes.System;
				}

				file_path = Path.GetFullPath(directory + "\\shell_host.dll");

				if (File.Exists(file_path))
				{
					File.Delete(file_path);
				}

				using (var completedEvent = new ManualResetEventSlim(false))
				{
					Web.DownloadFileCompleted += (sender, args) =>
					{
						completedEvent.Set();
						//Console.WriteLine(" DONE");
					};
					Web.DownloadProgressChanged += (sender, args) =>
					{
						if (!hidden)
						{
							drawTextProgressBar(args.ProgressPercentage, 100);
						}
					};
					Web.DownloadFileAsync(new Uri(addr), file_path);
					completedEvent.Wait();
				}

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

		public static void drawTextProgressBar(int progress, int total)
		{
			Console.Write($"\r{progress}% of {total}%"); //blanks at the end remove any excess
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
