using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Services
{
	public interface IEnemyTrackable
	{
		Transform Transform { get; }

		bool IsTrackable { get; }
	}
}
