using System;
using System.Collections.Generic;
using System.Linq;

namespace QFSW.QC.Suggestors
{
	public class EnumSuggestor : BasicCachedQcSuggestor<string>
	{
		private readonly Dictionary<Type, string[]> _enumCaseCache = new Dictionary<Type, string[]>();

		protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
		{
			Type targetType = context.TargetType;
			if (targetType != null)
			{
				return targetType.IsEnum;
			}
			return false;
		}

		protected override IQcSuggestion ItemToSuggestion(string item)
		{
			return new RawSuggestion(item);
		}

		protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
		{
			return GetEnumCases(context.TargetType);
		}

		private string[] GetEnumCases(Type enumType)
		{
			if (_enumCaseCache.TryGetValue(enumType, out var value))
			{
				return value;
			}
			string[] array = (from x in enumType.GetEnumNames()
				select x.ToString()).ToArray();
			return _enumCaseCache[enumType] = array;
		}
	}
}
