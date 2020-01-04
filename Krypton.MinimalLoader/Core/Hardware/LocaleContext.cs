using System.Globalization;
using System.Threading;

namespace Krypton.MinimalLoader.Core.Hardware
{
	public class LocaleContext
	{
		private CultureInfo Culture;

		public LocaleContext()
		{
			Culture = Thread.CurrentThread.CurrentCulture;
		}

		public CultureInfo GetCulture()
			=> Culture;

		public string GetShortLocale()
			=> GetCulture().TwoLetterISOLanguageName;

		public int GetLocaleCode()
			=> GetCulture().LCID;
	}
}
