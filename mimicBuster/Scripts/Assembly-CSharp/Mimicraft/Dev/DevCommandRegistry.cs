using System;
using System.Collections.Generic;

namespace Mimicraft.Dev
{
	public static class DevCommandRegistry
	{
		private static readonly SortedDictionary<string, DevCommand> commands = new SortedDictionary<string, DevCommand>(StringComparer.OrdinalIgnoreCase);

		public static IEnumerable<DevCommand> All => commands.Values;

		public static int Count => commands.Count;

		public static void Register(string name, string usage, string help, DevCommandHandler run, bool cheat = false)
		{
			if (!string.IsNullOrWhiteSpace(name) && run != null)
			{
				commands[name] = new DevCommand(name, usage, help, run, cheat);
			}
		}

		public static bool TryGet(string name, out DevCommand command)
		{
			return commands.TryGetValue(name ?? "", out command);
		}

		public static List<DevCommand> Match(string prefix, int max)
		{
			List<DevCommand> list = new List<DevCommand>(max);
			if (prefix == null)
			{
				prefix = "";
			}
			foreach (DevCommand value in commands.Values)
			{
				if (list.Count >= max)
				{
					break;
				}
				if (value.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
				{
					list.Add(value);
				}
			}
			return list;
		}
	}
}
