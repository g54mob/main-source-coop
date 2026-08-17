using System.Collections.Generic;
using System.Linq;
using QFSW.QC.Suggestors.Tags;
using QFSW.QC.Utilities;
using UnityEngine.SceneManagement;

namespace QFSW.QC.Suggestors
{
	public class SceneNameSuggestor : BasicCachedQcSuggestor<string>
	{
		protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
		{
			return context.HasTag<SceneNameTag>();
		}

		protected override IQcSuggestion ItemToSuggestion(string sceneName)
		{
			return new RawSuggestion(sceneName, singleLiteral: true);
		}

		protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
		{
			if (context.GetTag<SceneNameTag>().LoadedOnly)
			{
				return from x in SceneUtilities.GetLoadedScenes()
					select x.name;
			}
			return SceneUtilities.GetAllSceneNames();
		}
	}
}
