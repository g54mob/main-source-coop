using Features.ItemsModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.CollectingModule.Scripts
{
	public class ItemsConsumer : MonoBehaviour
	{
		private IItemCollectService _itemCollectService;

		private MultiplayerModel _multiplayerModel;

		[Inject]
		private void InjectDependencies(IItemCollectService itemCollectService, MultiplayerModel multiplayerModel)
		{
			_itemCollectService = itemCollectService;
			_multiplayerModel = multiplayerModel;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<IItem>(out var component))
			{
				TryCollectItem(component);
			}
		}

		private void TryCollectItem(IItem item)
		{
			if (item.IsCollectable && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_itemCollectService?.Collect(item);
				item.Consume();
			}
		}
	}
}
