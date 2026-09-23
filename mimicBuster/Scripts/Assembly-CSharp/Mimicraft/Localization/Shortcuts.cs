using System;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Settings;
using UnityEngine;

namespace Mimicraft.Localization
{
	public static class Shortcuts
	{
		private const string Open = "[key:";

		private const char Close = ']';

		private static readonly HashSet<string> reported = new HashSet<string>();

		public static bool Present(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				return text.Contains("[key:");
			}
			return false;
		}

		public static string Fill(string text)
		{
			if (!Present(text))
			{
				return text;
			}
			if (!Application.isPlaying && !GameInput.IsBuilt)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			int num = 0;
			while (num < text.Length)
			{
				int num2 = text.IndexOf("[key:", num, StringComparison.Ordinal);
				if (num2 < 0)
				{
					break;
				}
				int num3 = text.IndexOf(']', num2 + "[key:".Length);
				if (num3 < 0)
				{
					break;
				}
				string id = text.Substring(num2 + "[key:".Length, num3 - num2 - "[key:".Length).Trim();
				stringBuilder.Append(text, num, num2 - num);
				stringBuilder.Append(Display(id));
				num = num3 + 1;
			}
			stringBuilder.Append(text, num, text.Length - num);
			return stringBuilder.ToString();
		}

		private static string Display(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return "";
			}
			string text = GameInput.DisplayFor(id);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (reported.Add(id))
			{
				Debug.LogWarning("[Loc] '" + id + "' diye bir komut yok - metindeki [key:" + id + "] cozulemedi. GameInput.BuildCommands icindeki kimliklere bak.");
			}
			int num = id.LastIndexOf('/');
			if (num < 0 || num >= id.Length - 1)
			{
				return id;
			}
			return id.Substring(num + 1);
		}
	}
}
