using System;
using System.Collections.Generic;
using System.Linq;
using QFSW.QC.Utilities;
using UnityEngine;

namespace QFSW.QC.Suggestors
{
	public class ComponentSuggestor : BasicCachedQcSuggestor<string>
	{
		protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
		{
			Type targetType = context.TargetType;
			if (targetType != null && targetType.IsDerivedTypeOf(typeof(Component)))
			{
				return !targetType.IsGenericParameter;
			}
			return false;
		}

		protected override IQcSuggestion ItemToSuggestion(string name)
		{
			return new RawSuggestion(name, singleLiteral: true);
		}

		protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
		{
			return from cmp in UnityEngine.Object.FindObjectsByType(context.TargetType, FindObjectsSortMode.None)
				select (Component)cmp into cmp
				select cmp.gameObject.name;
		}
	}
}
