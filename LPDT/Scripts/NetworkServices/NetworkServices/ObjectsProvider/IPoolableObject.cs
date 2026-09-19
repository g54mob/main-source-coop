using Fusion;

namespace NetworkServices.ObjectsProvider
{
	public interface IPoolableObject
	{
		void OnSpawned();

		void OnActivated();

		void OnDeactivated();

		void OnDespawned();

		void PerformReturnToPool(NetworkObject instance)
		{
			instance.gameObject.SetActive(value: false);
		}

		void PerformTakeFromPool(NetworkObject result)
		{
			result.gameObject.SetActive(value: true);
		}
	}
}
