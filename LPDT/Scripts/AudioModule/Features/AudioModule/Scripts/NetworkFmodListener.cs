using FMODUnity;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using Zenject;

namespace Features.AudioModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class NetworkFmodListener : NetworkBehaviour
	{
		private MultiplayerModel _multiplayerModel;

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public override void Spawned()
		{
			if (!(base.Object.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer))
			{
				base.gameObject.AddComponent<StudioListener>().AttenuationObject = base.gameObject;
			}
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
