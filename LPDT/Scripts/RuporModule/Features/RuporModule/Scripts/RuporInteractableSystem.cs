using System;
using System.Collections.Generic;
using Features.EntitiesSoundOcclusionModule.Scripts;
using Features.GameUpdaterModule;
using Features.GrabModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.SelfMicrophonePlayerModule;
using Features.VoiceControlModule.Scripts;
using Features.VoiceOcclusionModule.Scripts;
using Fusion;
using Zenject;

namespace Features.RuporModule.Scripts
{
	public class RuporInteractableSystem : IInitializable, IDisposable
	{
		private const string RUPOR_PARAMETER_NAME = "Rupor";

		private const float DISTANCE_MULTIPLIER = 0.1f;

		private const float VOICE_OCCLUSION_DISTANCE_MULTIPLIER = 2.5f;

		private readonly RuporModel _ruporModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IVoiceService _voiceService;

		private readonly VoiceDistanceModel _voiceDistanceModel;

		private readonly IFmodMicSelfMonitor _fmodMicSelfMonitor;

		private readonly EntitiesSoundOcclusionModel _entitiesSoundOcclusionModel;

		public RuporInteractableSystem(RuporModel ruporModel, IGameUpdater gameUpdater, MultiplayerModel multiplayerModel, IVoiceService voiceService, VoiceDistanceModel voiceDistanceModel, IFmodMicSelfMonitor fmodMicSelfMonitor, EntitiesSoundOcclusionModel entitiesSoundOcclusionModel)
		{
			_ruporModel = ruporModel;
			_gameUpdater = gameUpdater;
			_multiplayerModel = multiplayerModel;
			_voiceService = voiceService;
			_voiceDistanceModel = voiceDistanceModel;
			_fmodMicSelfMonitor = fmodMicSelfMonitor;
			_entitiesSoundOcclusionModel = entitiesSoundOcclusionModel;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += ProcessRuporInteractions;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= ProcessRuporInteractions;
			_fmodMicSelfMonitor.SetMonitoring(enabled: false);
			ResetVoiceOcclusionMultipliers();
		}

		private void ProcessRuporInteractions()
		{
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			bool monitoring = false;
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				dictionary.Add(activePlayer.PlayerId, value: false);
			}
			foreach (KeyValuePair<IPointGrabable, RuporData> ruporsDatum in _ruporModel.RuporsData)
			{
				if (ruporsDatum.Key.GrabbedByPlayers.Count > 0 && ruporsDatum.Value.IsInInteraction)
				{
					int num = ruporsDatum.Key.GrabbedByPlayers[0];
					dictionary[num] = true;
					if (num == playerId)
					{
						monitoring = true;
					}
				}
			}
			_fmodMicSelfMonitor.SetMonitoring(monitoring);
			foreach (KeyValuePair<int, bool> item in dictionary)
			{
				_entitiesSoundOcclusionModel.SetVoiceDistanceMultiplier(item.Key, item.Value ? 2.5f : 1f);
				if (item.Key != _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
				{
					if (item.Value)
					{
						_voiceService.ProcessEffectForSpeaker(item.Key, 1f, "Rupor");
						_voiceDistanceModel.MaxDistanceMultiplier[item.Key] = 0.1f / _voiceDistanceModel.Distances[item.Key];
					}
					else
					{
						_voiceService.ProcessEffectForSpeaker(item.Key, 0f, "Rupor");
						_voiceDistanceModel.MaxDistanceMultiplier[item.Key] = 1f;
					}
				}
			}
		}

		private void ResetVoiceOcclusionMultipliers()
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return;
			}
			foreach (PlayerRef activePlayer in _multiplayerModel.NetworkRunner.ActivePlayers)
			{
				_entitiesSoundOcclusionModel.SetVoiceDistanceMultiplier(activePlayer.PlayerId, 1f);
			}
		}
	}
}
