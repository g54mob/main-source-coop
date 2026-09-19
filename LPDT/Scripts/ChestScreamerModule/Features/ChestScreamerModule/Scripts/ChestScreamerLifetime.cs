using UnityEngine;

namespace Features.ChestScreamerModule.Scripts
{
	public class ChestScreamerLifetime : MonoBehaviour
	{
		[SerializeField]
		private bool _isSpawnVfxOnDestroy;

		[SerializeField]
		private ParticleSystem _destroyVfxPrefab;

		public void Configure(float lifetimeSeconds, bool destroyAfterLifetime)
		{
			if (destroyAfterLifetime)
			{
				Object.Destroy(base.gameObject, lifetimeSeconds);
			}
		}

		private void OnDestroy()
		{
			if (_isSpawnVfxOnDestroy)
			{
				Object.Instantiate(_destroyVfxPrefab.gameObject, base.transform.position, base.transform.rotation);
			}
		}
	}
}
