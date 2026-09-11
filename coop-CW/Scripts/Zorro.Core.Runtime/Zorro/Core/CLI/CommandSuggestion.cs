using System.Reflection;

namespace Zorro.Core.CLI
{
	public class CommandSuggestion : Suggestion
	{
		public string Domain;

		public string Command;

		public ParameterInfo[] ParameterInfos;

		private int selectedParameterIndex;

		public string FullCommand => Domain + "." + Command;

		public CommandSuggestion(string domain, string command, ParameterInfo[] parameterInfos)
		{
			Domain = domain;
			Command = command;
			ParameterInfos = parameterInfos;
			selectedParameterIndex = -1;
		}

		public override string GetInputValue()
		{
			return FullCommand;
		}

		public override bool CanBeSelected()
		{
			return selectedParameterIndex == -1;
		}

		public void HighlightParameter(int parameterIndex)
		{
			selectedParameterIndex = parameterIndex;
		}

		public override string ToString()
		{
			return GetDisplayTextWithMaxParameter(int.MaxValue, color: true);
		}

		public string GetDisplayTextWithMaxParameter(int maxParameterIndex, bool color)
		{
			string text = "";
			for (int i = 0; i < ParameterInfos.Length && i < maxParameterIndex; i++)
			{
				if (color)
				{
					text = ((selectedParameterIndex != i) ? (text + "<color=#cccaca>") : (text + "<color=#ffffff>"));
				}
				text = text + " " + ParameterInfos[i].Name + " (" + ParameterInfos[i].ParameterType.Name + ") ";
			}
			if (!color)
			{
				return FullCommand + text;
			}
			return "<color=#cccaca>" + FullCommand + text;
		}
	}
}
