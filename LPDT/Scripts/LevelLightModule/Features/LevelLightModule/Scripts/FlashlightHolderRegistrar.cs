using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LevelLightModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class FlashlightHolderRegistrar : NetworkBehaviour
	{
		[SerializeField]
		private FlashlightPositionType _flashlightPositionType;

		[SerializeField]
		private Transform _flashlightHolder;

		private FlashlightPositionsModel _flashlightPositionsModel;

		[Inject]
		public void InjectDependencies(FlashlightPositionsModel flashlightPositionsModel)
		{
			_flashlightPositionsModel = flashlightPositionsModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_flashlightPositionsModel.AddFlashlightHolder(base.Object.InputAuthority.PlayerId, _flashlightPositionType, _flashlightHolder);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_flashlightPositionsModel.RemoveFlashlightHolder(base.Object.InputAuthority.PlayerId, _flashlightPositionType, _flashlightHolder);
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
