using System;
using UnityEngine;

namespace Features.RumModule.Scripts
{
	public class RumCollisionHandler : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _layerMask;

		[SerializeField]
		private LayerMask _enemyLayerMask;

		[SerializeField]
		private LayerMask _collectableLayerMask;

		[SerializeField]
		private LayerMask _ignoreCollisionMask;

		public event Action<RumHitData> OnPlayerHited;

		public event Action<RumHitData> OnEnemyHited;

		public event Action<RumHitData> OnCollectableHited;

		public event Action<RumHitData> OnHited;

		private void OnCollisionEnter(Collision other)
		{
			if (IsInLayerMask(other.gameObject, _collectableLayerMask))
			{
				this.OnCollectableHited?.Invoke(new RumHitData(other.gameObject, other.relativeVelocity));
			}
			if (IsInLayerMask(other.gameObject, _layerMask))
			{
				this.OnPlayerHited?.Invoke(new RumHitData(other.gameObject, other.relativeVelocity));
			}
			if (IsInLayerMask(other.gameObject, _enemyLayerMask))
			{
				this.OnEnemyHited?.Invoke(new RumHitData(other.gameObject, other.relativeVelocity));
			}
			int layer = other.gameObject.layer;
			if ((_ignoreCollisionMask.value & (1 << layer)) == 0)
			{
				this.OnHited?.Invoke(new RumHitData(other.gameObject, other.relativeVelocity));
			}
		}

		private bool IsInLayerMask(GameObject gameObject, LayerMask layerMask)
		{
			return (layerMask.value & (1 << gameObject.layer)) != 0;
		}
	}
}
