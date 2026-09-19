using Fusion;
using Zenject;

namespace Features.VoiceControlModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class VoiceSpawner : NetworkBehaviour
	{
		private IVoiceService _voiceService;

		[Inject]
		private void InjectDependencies(IVoiceService voiceService)
		{
			_voiceService = voiceService;
		}

		public override void Spawned()
		{
			if (base.HasStateAuthority)
			{
				_voiceService.SpawnVoiceSpeaker(base.Object.InputAuthority.PlayerId);
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
