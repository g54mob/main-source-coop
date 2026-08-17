using System;
using System.Collections.Generic;
using QFSW.QC.Utilities;
using TMPro;
using UnityEngine;

namespace QFSW.QC
{
	[CreateAssetMenu(fileName = "Untitled Theme", menuName = "Quantum Console/Theme")]
	public class QuantumTheme : ScriptableObject
	{
		[SerializeField]
		public TMP_FontAsset Font;

		[SerializeField]
		public Material PanelMaterial;

		[SerializeField]
		public Color PanelColor = Color.white;

		[SerializeField]
		public Color CommandLogColor = new Color(0f, 1f, 1f);

		[SerializeField]
		public Color SelectedSuggestionColor = new Color(1f, 1f, 0.55f);

		[SerializeField]
		public Color SuggestionColor = Color.gray;

		[SerializeField]
		public Color ErrorColor = Color.red;

		[SerializeField]
		public Color WarningColor = new Color(1f, 0.5f, 0f);

		[SerializeField]
		public Color SuccessColor = Color.green;

		[SerializeField]
		public string TimestampFormat = "[{0}:{1}:{2}]";

		[SerializeField]
		public string CommandLogFormat = "> {0}";

		[SerializeField]
		public Color DefaultReturnValueColor = Color.white;

		[SerializeField]
		public List<TypeColorFormatter> TypeFormatters = new List<TypeColorFormatter>(0);

		[SerializeField]
		public List<CollectionFormatter> CollectionFormatters = new List<CollectionFormatter>(0);

		private T FindTypeFormatter<T>(List<T> formatters, Type type) where T : TypeFormatter
		{
			foreach (T formatter in formatters)
			{
				if (type == formatter.Type || type.IsGenericTypeOf(formatter.Type))
				{
					return formatter;
				}
			}
			foreach (T formatter2 in formatters)
			{
				if (formatter2.Type.IsAssignableFrom(type))
				{
					return formatter2;
				}
			}
			return null;
		}

		public string ColorizeReturn(string data, Type type)
		{
			TypeColorFormatter typeColorFormatter = FindTypeFormatter(TypeFormatters, type);
			if (typeColorFormatter == null)
			{
				return data.ColorText(DefaultReturnValueColor);
			}
			return data.ColorText(typeColorFormatter.Color);
		}

		public void GetCollectionFormatting(Type type, out string leftScoper, out string seperator, out string rightScoper)
		{
			CollectionFormatter collectionFormatter = FindTypeFormatter(CollectionFormatters, type);
			if (collectionFormatter == null)
			{
				leftScoper = "[";
				seperator = ",";
				rightScoper = "]";
			}
			else
			{
				leftScoper = collectionFormatter.LeftScoper.Replace("\\n", "\n");
				seperator = collectionFormatter.SeperatorString.Replace("\\n", "\n");
				rightScoper = collectionFormatter.RightScoper.Replace("\\n", "\n");
			}
		}
	}
}
