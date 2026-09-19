using System;

namespace Features.AIModule.Scripts
{
	public interface IEnemyDeadStateProvider
	{
		bool IsDead { get; }

		event Action<bool> OnIsDeadChanged;
	}
}
