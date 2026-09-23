using System.Text;
using Mimicraft.Localization;

namespace Mimicraft.Customization
{
	public static class PresetNames
	{
		public const string KeyPrefix = "Preset.";

		public static string Resolve(string nameKey, string authored)
		{
			if (!string.IsNullOrWhiteSpace(nameKey) && Loc.TryGet(nameKey, out var text))
			{
				return text;
			}
			string text2 = KeyFor(authored);
			if (text2 != null && Loc.TryGet(text2, out var text3))
			{
				return text3;
			}
			return authored ?? "";
		}

		public static string KeyFor(string authored)
		{
			if (string.IsNullOrWhiteSpace(authored))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder("Preset.", "Preset.".Length + authored.Length);
			foreach (char c in authored)
			{
				if (char.IsLetterOrDigit(c))
				{
					stringBuilder.Append(c);
				}
			}
			if (stringBuilder.Length <= "Preset.".Length)
			{
				return null;
			}
			return stringBuilder.ToString();
		}
	}
}
