using Krypton.Support;

namespace Krypton.Server.Core.Updating
{
	public class UpdatingComponent : KryptonComponent<UpdatingComponent>
	{
		public bool IsDeclineDownloadHack { get; set; }

		public UpdatingComponent()
		{
			IsDeclineDownloadHack = false;
		}
	}
}
