using Features.AIModule.Scripts;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial
{
	public class PirateVisibilityTargetDetector : MonoBehaviour
	{
		private const int OVERLAP_BUFFER_SIZE = 16;

		[SerializeField]
		private float _detectionRadius = 20f;

		[SerializeField]
		private LayerMask _enemyLayerMask;

		private readonly Collider[] _overlapBuffer = new Collider[16];

		private IEnemyBehaviour _self;

		private void Awake()
		{
			_self = GetComponentInParent<IEnemyBehaviour>();
		}

		public bool TryGetNearestEnemy(out IEnemyBehaviour target)
		{
			target = null;
			float num = float.PositiveInfinity;
			Vector3 position = base.transform.position;
			int num2 = Physics.OverlapSphereNonAlloc(position, _detectionRadius, _overlapBuffer, _enemyLayerMask, QueryTriggerInteraction.Collide);
			for (int i = 0; i < num2; i++)
			{
				IEnemyBehaviour componentInParent = _overlapBuffer[i].GetComponentInParent<IEnemyBehaviour>();
				if (componentInParent != null && componentInParent != _self && !(componentInParent.NetworkObject == null))
				{
					float sqrMagnitude = (componentInParent.NetworkObject.transform.position - position).sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						target = componentInParent;
					}
				}
			}
			return target != null;
		}
	}
}
