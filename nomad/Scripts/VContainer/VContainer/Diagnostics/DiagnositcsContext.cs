using System;
using System.Collections.Generic;
using System.Linq;

namespace VContainer.Diagnostics
{
	public static class DiagnositcsContext
	{
		private static readonly Dictionary<string, DiagnosticsCollector> collectors = new Dictionary<string, DiagnosticsCollector>();

		public static event Action<IObjectResolver> OnContainerBuilt;

		public static DiagnosticsCollector GetCollector(string name)
		{
			lock (collectors)
			{
				if (!collectors.TryGetValue(name, out var value))
				{
					value = new DiagnosticsCollector(name);
					collectors.Add(name, value);
				}
				return value;
			}
		}

		public static ILookup<string, DiagnosticsInfo> GetGroupedDiagnosticsInfos()
		{
			lock (collectors)
			{
				return (from x in collectors.SelectMany((KeyValuePair<string, DiagnosticsCollector> x) => x.Value.GetDiagnosticsInfos())
					where x.ResolveInfo.MaxDepth <= 1
					select x).ToLookup((DiagnosticsInfo x) => x.ScopeName);
			}
		}

		public static IEnumerable<DiagnosticsInfo> GetDiagnosticsInfos()
		{
			lock (collectors)
			{
				return collectors.SelectMany((KeyValuePair<string, DiagnosticsCollector> x) => x.Value.GetDiagnosticsInfos());
			}
		}

		public static void NotifyContainerBuilt(IObjectResolver container)
		{
			DiagnositcsContext.OnContainerBuilt?.Invoke(container);
		}

		internal static DiagnosticsInfo FindByRegistration(Registration registration)
		{
			return GetDiagnosticsInfos().FirstOrDefault((DiagnosticsInfo x) => x.ResolveInfo.Registration == registration);
		}

		public static void RemoveCollector(string name)
		{
			lock (collectors)
			{
				collectors.Remove(name);
			}
		}
	}
}
