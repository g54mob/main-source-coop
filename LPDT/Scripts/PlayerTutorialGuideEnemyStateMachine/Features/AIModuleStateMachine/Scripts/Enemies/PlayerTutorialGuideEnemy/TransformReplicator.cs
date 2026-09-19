using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy
{
	public class TransformReplicator : MonoBehaviour
	{
		private Transform _target;

		public void StartReplication(Transform target)
		{
			_target = target;
		}

		public void StopReplication()
		{
			_target = null;
		}

		private void LateUpdate()
		{
			if (!(_target == null))
			{
				base.transform.position = _target.position;
				base.transform.rotation = _target.rotation;
			}
		}
	}
}
