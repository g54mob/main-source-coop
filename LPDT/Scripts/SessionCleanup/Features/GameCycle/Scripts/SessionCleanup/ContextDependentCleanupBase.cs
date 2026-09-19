using System;
using Zenject;

namespace Features.GameCycle.Scripts.SessionCleanup
{
	public abstract class ContextDependentCleanupBase : IInitializable, IDisposable
	{
		private ContextDependentCleanupModel _contextDependentCleanupModel;

		[Inject]
		public void InjectDependencies(ContextDependentCleanupModel contextDependentCleanupModel)
		{
			_contextDependentCleanupModel = contextDependentCleanupModel;
		}

		public virtual void Initialize()
		{
			_contextDependentCleanupModel.AddContextCleanup(this);
		}

		public virtual void Dispose()
		{
			_contextDependentCleanupModel.RemoveContextCleanup(this);
		}

		public abstract void Cleanup();
	}
}
