using System.Collections.Generic;

namespace Features.GameCycle.Scripts.SessionCleanup
{
	public class ContextDependentCleanupModel
	{
		private readonly List<ContextDependentCleanupBase> _contextCleanups = new List<ContextDependentCleanupBase>();

		public IReadOnlyList<ContextDependentCleanupBase> ContextCleanups => _contextCleanups;

		public void AddContextCleanup(ContextDependentCleanupBase contextCleanup)
		{
			_contextCleanups.Add(contextCleanup);
		}

		public void RemoveContextCleanup(ContextDependentCleanupBase contextCleanup)
		{
			_contextCleanups.Remove(contextCleanup);
		}
	}
}
