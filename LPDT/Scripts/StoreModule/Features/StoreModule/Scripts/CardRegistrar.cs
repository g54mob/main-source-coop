using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class CardRegistrar : NetworkBehaviour
	{
		[SerializeField]
		private StoreCardBehaviour _storeCardBehaviour;

		private CardsOnTableModel _cardsOnTableModel;

		[Inject]
		private void InjectDependencies(CardsOnTableModel cardsOnTableModel)
		{
			_cardsOnTableModel = cardsOnTableModel;
		}

		public override void Spawned()
		{
			_cardsOnTableModel.RegisterCard(_storeCardBehaviour);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_cardsOnTableModel.UnregisterCard(_storeCardBehaviour);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
