using Features.GrabModule.Scripts;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	public class AnchorHitNotifier : MonoBehaviour
	{
		[SerializeField]
		private AnchorHookController _controller;

		[SerializeField]
		private LayerMask _playerLayerMask;

		[SerializeField]
		private LayerMask _obstacleLayerMask;

		private void OnTriggerEnter(Collider other)
		{
			int layer = other.gameObject.layer;
			if (IsInLayerMask(layer, _playerLayerMask) && other.TryGetComponent<SimplePointGrabable>(out var component))
			{
				_controller.ReportPlayerHit(component);
			}
			else if (IsInLayerMask(layer, _obstacleLayerMask))
			{
				_controller.ReportObstacle();
			}
		}

		private static bool IsInLayerMask(int layer, LayerMask mask)
		{
			return (mask.value & (1 << layer)) != 0;
		}
	}
}
