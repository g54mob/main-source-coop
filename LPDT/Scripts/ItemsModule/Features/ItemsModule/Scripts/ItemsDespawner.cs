using Features.MultiplayerSessionServices.Scripts;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using Zenject;

namespace Features.ItemsModule.Scripts
{
	public class ItemsDespawner : MonoBehaviour
	{
		private MultiplayerModel _multiplayerModel;

		[Inject]
		public void InjectDependencies(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		private void OnCollisionEnter(Collision other)
		{
			if (other.transform.TryGetComponent<IItem>(out var component) && !(component.NetworkObject == null) && component.NetworkObject.HasStateAuthority)
			{
				component.NetworkObject.DespawnHierarchy();
			}
		}
	}
}
