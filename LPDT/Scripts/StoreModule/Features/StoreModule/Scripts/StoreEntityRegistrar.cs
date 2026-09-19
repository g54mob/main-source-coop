using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreEntityRegistrar : MonoBehaviour
	{
		[SerializeField]
		private StoreEntity _storeEntity;

		private ActiveStoreModel _activeStoreModel;

		[Inject]
		public void InjectDependencies(ActiveStoreModel activeStoreModel)
		{
			_activeStoreModel = activeStoreModel;
		}

		private void Start()
		{
			_activeStoreModel.ActiveStoreEntity = _storeEntity;
		}
	}
}
