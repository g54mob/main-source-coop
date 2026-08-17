using TMPro;

namespace EvilCore.Localization
{
	internal class LocaleFontProvider
	{
		private readonly LocaleFontConfig _config;

		public LocaleFontProvider(LocaleFontConfig config)
		{
			_config = config;
		}

		public TMP_FontAsset GetFont(string localeCode, string style = null)
		{
			if (_config == null)
			{
				return null;
			}
			return _config.GetFont(localeCode, style);
		}
	}
}
