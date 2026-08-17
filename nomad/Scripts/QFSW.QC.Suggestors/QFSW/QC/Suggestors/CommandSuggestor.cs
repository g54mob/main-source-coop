using System.Collections.Generic;
using System.Linq;
using QFSW.QC.Utilities;

namespace QFSW.QC.Suggestors
{
	public class CommandSuggestor : BasicCachedQcSuggestor<CollapsedCommand>
	{
		private readonly Dictionary<string, List<CommandData>> _commandGroups = new Dictionary<string, List<CommandData>>();

		private readonly Stack<CollapsedCommand> _commandCollector = new Stack<CollapsedCommand>();

		protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
		{
			return context.Depth == 0;
		}

		protected override IQcSuggestion ItemToSuggestion(CollapsedCommand collapsedCommand)
		{
			return new CommandSuggestion(collapsedCommand.Command, collapsedCommand.NumOptionalParams);
		}

		protected override IEnumerable<CollapsedCommand> GetItems(SuggestionContext context, SuggestorOptions options)
		{
			string incompleteCommandName = context.Prompt.SplitScopedFirst(' ').SplitFirst('<');
			IEnumerable<CommandData> commands = GetCommands(incompleteCommandName, options);
			if (!options.CollapseOverloads)
			{
				return commands.Select((CommandData x) => new CollapsedCommand(x));
			}
			return CollapseCommands(commands);
		}

		public IEnumerable<CommandData> GetCommands(string incompleteCommandName, SuggestorOptions options)
		{
			if (string.IsNullOrWhiteSpace(incompleteCommandName))
			{
				return Enumerable.Empty<CommandData>();
			}
			return from command in QuantumConsoleProcessor.GetAllCommands()
				where SuggestorUtilities.IsCompatible(incompleteCommandName, command.CommandName, options)
				select command;
		}

		protected override bool IsMatch(SuggestionContext context, IQcSuggestion suggestion, SuggestorOptions options)
		{
			return true;
		}

		private IEnumerable<CollapsedCommand> CollapseCommands(IEnumerable<CommandData> commands)
		{
			foreach (List<CommandData> value2 in _commandGroups.Values)
			{
				value2.Clear();
			}
			foreach (CommandData command3 in commands)
			{
				if (!_commandGroups.TryGetValue(command3.CommandName, out var value))
				{
					value = new List<CommandData>();
					_commandGroups[command3.CommandName] = value;
				}
				value.Add(command3);
			}
			foreach (List<CommandData> value3 in _commandGroups.Values)
			{
				value3.InsertionSortBy((CommandData x) => x.ParamCount);
				_commandCollector.Clear();
				foreach (CommandData item2 in value3)
				{
					CollapsedCommand item = new CollapsedCommand(item2);
					if (_commandCollector.Count > 0)
					{
						CollapsedCommand collapsedCommand = _commandCollector.Peek();
						CommandData command = item.Command;
						CommandData command2 = collapsedCommand.Command;
						if (command.ParamCount == command2.ParamCount + 1 && command.ParameterSignature.StartsWith(command2.ParameterSignature))
						{
							_commandCollector.Pop();
							item.NumOptionalParams += 1 + collapsedCommand.NumOptionalParams;
						}
					}
					_commandCollector.Push(item);
				}
				foreach (CollapsedCommand item3 in _commandCollector)
				{
					yield return item3;
				}
			}
		}
	}
}
