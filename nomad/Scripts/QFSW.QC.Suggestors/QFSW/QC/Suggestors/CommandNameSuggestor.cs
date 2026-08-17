using System.Collections.Generic;
using System.Linq;
using QFSW.QC.Suggestors.Tags;

namespace QFSW.QC.Suggestors
{
	public class CommandNameSuggestor : BasicCachedQcSuggestor<string>
	{
		protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
		{
			if (context.HasTag<CommandNameTag>())
			{
				return !string.IsNullOrWhiteSpace(context.Prompt);
			}
			return false;
		}

		protected override IQcSuggestion ItemToSuggestion(string commandName)
		{
			return new RawSuggestion(commandName);
		}

		protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
		{
			context.Prompt.SplitScopedFirst(' ').SplitFirst('<');
			return from command in QuantumConsoleProcessor.GetUniqueCommands()
				select command.CommandName;
		}
	}
}
