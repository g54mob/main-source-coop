using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HeathenEngineering
{
	[CreateAssetMenu(menuName = "System Core/Application/Keyed String List")]
	public class KeyedStringListVariable : ScriptableObject
	{
		public string OpenTag = "@[";

		public string CloseTag = "];";

		public int MaxDepth = 10;

		public List<KeyedReference> Values;

		public void SetValue(List<KeyedReference> value)
		{
			Values.Clear();
			Values.AddRange(value);
		}

		public void SetValue(KeyedStringListVariable value)
		{
			Values.Clear();
			Values.AddRange(value.Values);
		}

		public string GetValue(string key)
		{
			if (Values.Any((KeyedReference p) => p.Key == key))
			{
				return SwapKeys(Values.First((KeyedReference p) => p.Key == key).Value);
			}
			return string.Empty;
		}

		public string SwapKeys(string source)
		{
			int num = 0;
			string text = source;
			while (text.Contains(OpenTag) && num < MaxDepth)
			{
				num++;
				StringBuilder stringBuilder = new StringBuilder(text);
				foreach (KeyedReference value in Values)
				{
					stringBuilder = stringBuilder.Replace(OpenTag + value.Key + CloseTag, value.Value.Value);
				}
				text = stringBuilder.ToString();
			}
			return text;
		}
	}
}
