using System.Collections.Generic;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.VoiceControlModule.Scripts
{
	public class EffectZoneComponent : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _layerMask;

		[SerializeField]
		private EffectType _parameterName;

		[SerializeField]
		[Range(-1f, 1f)]
		private float _targetValueOnEnter = 1f;

		[SerializeField]
		private float _fadeDuration = 1f;

		private IVoiceService _voiceService;

		private PlayerMovableModel _playerMovableModel;

		private MultiplayerModel _multiplayerModel;

		[Inject]
		public void InjectDependencies(IVoiceService voiceService, PlayerMovableModel playerMovableModel, MultiplayerModel multiplayerModel)
		{
			_voiceService = voiceService;
			_playerMovableModel = playerMovableModel;
			_multiplayerModel = multiplayerModel;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return;
			}
			int layer = other.gameObject.layer;
			if ((_layerMask.value & (1 << layer)) == 0)
			{
				return;
			}
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				if (!(allCharacterMovable.Key == _multiplayerModel.NetworkRunner.LocalPlayer) && !(allCharacterMovable.Value == null) && !(allCharacterMovable.Value.gameObject != other.gameObject))
				{
					_voiceService.ProcessFadeEffectForSpeaker(allCharacterMovable.Key.PlayerId, _targetValueOnEnter, _fadeDuration, _parameterName.ToString());
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return;
			}
			int layer = other.gameObject.layer;
			if ((_layerMask.value & (1 << layer)) == 0)
			{
				return;
			}
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				if (!(allCharacterMovable.Value == null) && !(allCharacterMovable.Key == _multiplayerModel.NetworkRunner.LocalPlayer) && !(allCharacterMovable.Value.gameObject != other.gameObject))
				{
					_voiceService.ProcessFadeEffectForSpeaker(allCharacterMovable.Key.PlayerId, 0f, _fadeDuration, _parameterName.ToString());
				}
			}
		}
	}
}
