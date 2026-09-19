using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.DamageableTrackModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class EnemyDamageDetector : MonoBehaviour
	{
		[SerializeField]
		private SimpleEnemyDamageable _simpleEnemyDamageable;

		[SerializeField]
		private LayerMask _damageableLayers;

		private bool _isDisabled;

		private void OnTriggerEnter(Collider other)
		{
			if (IsInLayerMask(other.gameObject.layer, _damageableLayers) && !_isDisabled)
			{
				NetworkObject component = other.GetComponent<NetworkObject>();
				if (!(component == null))
				{
					_simpleEnemyDamageable.Damage(new DamageData
					{
						DamageDealerPlayerID = component.StateAuthority.PlayerId
					});
				}
			}
		}

		private bool IsInLayerMask(int layer, LayerMask layerMask)
		{
			return (layerMask.value & (1 << layer)) != 0;
		}

		public void DisableDetection()
		{
			_isDisabled = true;
		}

		public void EnableDetection()
		{
			_isDisabled = false;
		}
	}
}
