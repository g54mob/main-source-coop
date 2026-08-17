using System.Collections.Generic;
using System.Globalization;

namespace Febucci.UI.Core
{
	public static class FormatUtils
	{
		public static bool TryGetFloat(List<string> attributes, int index, float defValue, out float result)
		{
			if (index >= attributes.Count || index < 0)
			{
				result = defValue;
				return false;
			}
			return TryGetFloat(attributes[index], defValue, out result);
		}

		public static bool TryGetFloat(string attribute, float defValue, out float result)
		{
			if (ParseFloat(attribute, out result))
			{
				return true;
			}
			result = defValue;
			return false;
		}

		public static bool ParseFloat(string value, out float result)
		{
			return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
		}
	}
}
