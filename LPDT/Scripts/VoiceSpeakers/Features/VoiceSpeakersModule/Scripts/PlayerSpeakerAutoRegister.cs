using Features.VoiceSpeakersModule.Scripts.Data;
using Fusion;
using NetworkServices.NetworkEvents;
using Photon.Voice.Unity.FMOD;
using UnityEngine;
using Zenject;

namespace Features.VoiceSpeakersModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerSpeakerAutoRegister : NetworkBehaviour
	{
		[SerializeField]
		private NetworkObject _networkObject;

		[SerializeField]
		private SpeakerFMOD _speaker;

		[SerializeField]
		private bool _isNon3dSpeaker;

		private SpawnedVoiceModel _spawnedVoiceModel;

		private ActiveSpeakerModel _activeSpeakerModel;

		private int _cashedPlayerRef;

		private NetworkRunnerEventBus _networkRunnerEventBus;

		private bool _isWaitingForStateAuthority;

		[Inject]
		public void InjectDependencies(SpawnedVoiceModel spawnedVoiceModel, ActiveSpeakerModel activeSpeakerModel, NetworkRunnerEventBus networkRunnerEventBus)
		{
			_spawnedVoiceModel = spawnedVoiceModel;
			_activeSpeakerModel = activeSpeakerModel;
			_networkRunnerEventBus = networkRunnerEventBus;
		}

		private void Start()
		{
			if (_networkObject.StateAuthority == PlayerRef.None)
			{
				_isWaitingForStateAuthority = true;
			}
			else
			{
				Initialize();
			}
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_speaker.OnCreateAudioOut -= RegisterSpeaker;
		}

		private void Update()
		{
			if (_networkObject.StateAuthority == PlayerRef.None)
			{
				_isWaitingForStateAuthority = true;
			}
			if (_isWaitingForStateAuthority && _networkObject.StateAuthority != PlayerRef.None)
			{
				Initialize();
				_isWaitingForStateAuthority = false;
			}
		}

		private void Initialize()
		{
			if (_activeSpeakerModel != null && _networkRunnerEventBus != null)
			{
				_cashedPlayerRef = _networkObject.StateAuthority.PlayerId;
				_activeSpeakerModel.RegisterPlayer(_cashedPlayerRef);
				if (_speaker.Initialized)
				{
					RegisterSpeakerInModel();
				}
				else
				{
					_speaker.OnCreateAudioOut += RegisterSpeaker;
				}
				_networkRunnerEventBus.Subscribe<OnPlayerLeftEvent>(CleanUp);
			}
		}

		private void RegisterSpeakerInModel()
		{
			if (_isNon3dSpeaker)
			{
				_spawnedVoiceModel.RegisterNon3dSpeaker(_cashedPlayerRef, _speaker);
			}
			else
			{
				_spawnedVoiceModel.RegisterSpeaker(_cashedPlayerRef, _speaker);
			}
		}

		private void RegisterSpeaker()
		{
			_speaker.OnCreateAudioOut -= RegisterSpeaker;
			RegisterSpeakerInModel();
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			_networkRunnerEventBus.Unsubscribe<OnPlayerLeftEvent>(CleanUp);
			CleanUp();
		}

		private void CleanUp(OnPlayerLeftEvent _)
		{
			if (_networkObject.StateAuthority == PlayerRef.None)
			{
				CleanUp();
			}
		}

		private void CleanUp()
		{
			if (_isNon3dSpeaker)
			{
				_spawnedVoiceModel.UnregisterSpeakerNon3d(_cashedPlayerRef, _speaker);
			}
			else
			{
				_spawnedVoiceModel.UnregisterSpeaker(_cashedPlayerRef, _speaker);
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
