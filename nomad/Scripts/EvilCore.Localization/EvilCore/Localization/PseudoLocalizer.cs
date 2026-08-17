using System.Collections.Generic;
using System.Text;

namespace EvilCore.Localization
{
	public class PseudoLocalizer
	{
		private static readonly Dictionary<char, char> AccentMap = new Dictionary<char, char>
		{
			{ 'a', 'à' },
			{ 'e', 'è' },
			{ 'i', 'î' },
			{ 'o', 'ö' },
			{ 'u', 'ü' },
			{ 'A', 'À' },
			{ 'E', 'È' },
			{ 'I', 'Î' },
			{ 'O', 'Ö' },
			{ 'U', 'Ü' },
			{ 'c', 'ç' },
			{ 'n', 'ñ' },
			{ 's', 'š' },
			{ 'y', 'ý' },
			{ 'C', 'Ç' },
			{ 'N', 'Ñ' },
			{ 'S', 'Š' },
			{ 'Y', 'Ý' }
		};

		public string Apply(string text, PseudoLocalizationMode mode)
		{
			if (!string.IsNullOrEmpty(text))
			{
				switch (mode)
				{
				case PseudoLocalizationMode.None:
					break;
				case PseudoLocalizationMode.Accented:
					return ApplyAccented(text);
				case PseudoLocalizationMode.Expanded:
					return ApplyExpanded(text);
				case PseudoLocalizationMode.Bracketed:
					return "[" + text + "]";
				default:
					return text;
				}
			}
			return text;
		}

		private string ApplyAccented(string text)
		{
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			bool flag = false;
			foreach (char c in text)
			{
				if (c == '<')
				{
					flag = true;
				}
				if (flag)
				{
					stringBuilder.Append(c);
					if (c == '>')
					{
						flag = false;
					}
				}
				else if (c == '{' || c == '}')
				{
					stringBuilder.Append(c);
				}
				else
				{
					stringBuilder.Append(AccentMap.TryGetValue(c, out var value) ? value : c);
				}
			}
			return stringBuilder.ToString();
		}

		private string ApplyExpanded(string text)
		{
			StringBuilder stringBuilder = new StringBuilder((int)((float)text.Length * 1.4f) + 2);
			stringBuilder.Append('[');
			bool flag = false;
			foreach (char c in text)
			{
				if (c == '<')
				{
					flag = true;
				}
				if (flag)
				{
					stringBuilder.Append(c);
					if (c == '>')
					{
						flag = false;
					}
					continue;
				}
				if (c == '{' || c == '}')
				{
					stringBuilder.Append(c);
					continue;
				}
				char value2;
				char value = (AccentMap.TryGetValue(c, out value2) ? value2 : c);
				stringBuilder.Append(value);
				if (char.IsLetter(c))
				{
					stringBuilder.Append(value);
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}
	}
}
