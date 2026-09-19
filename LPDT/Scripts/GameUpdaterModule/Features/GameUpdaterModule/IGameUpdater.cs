using System;

namespace Features.GameUpdaterModule
{
	public interface IGameUpdater
	{
		event Action OnUpdate;

		event Action OnFixedUpdate;

		event Action OnLateUpdate;

		event Action OnOnDrawGizmos;

		void ClearEvents();
	}
}
