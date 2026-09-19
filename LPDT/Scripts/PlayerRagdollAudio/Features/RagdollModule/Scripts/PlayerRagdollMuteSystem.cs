using System;
using Features.CoroutineUtils.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.VoiceControlModule.Scripts;
using Features.VoiceSpeakersModule.Scripts.Data;
using Zenject;

namespace Features.RagdollModule.Scripts
{
	public class PlayerRagdollMuteSystem : IInitializable, IDisposable
	{
		private const string PITCH_PARAMETER = "Pitch";

		private readonly PlayersRagdollModel _playersRagdollModel;

		private readonly SpawnedVoiceModel _spawnedVoiceModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IPlayerStateService _playerStateService;

		private readonly IVoiceService _voiceService;

		public PlayerRagdollMuteSystem(PlayersRagdollModel playersRagdollModel, SpawnedVoiceModel spawnedVoiceModel, ICoroutineRunner coroutineRunner, MultiplayerModel multiplayerModel, IPlayerStateService playerStateService, IVoiceService voiceService)
		{
			_playersRagdollModel = playersRagdollModel;
			_spawnedVoiceModel = spawnedVoiceModel;
			_coroutineRunner = coroutineRunner;
			_multiplayerModel = multiplayerModel;
			_playerStateService = playerStateService;
			_voiceService = voiceService;
		}

		public void Initialize()
		{
			_playersRagdollModel.OnPlayerRagdollAdded += SubscribeToRagdoll;
		}

		public void Dispose()
		{
			_playersRagdollModel.OnPlayerRagdollAdded -= SubscribeToRagdoll;
			foreach (PlayerRagdollEntity value in _playersRagdollModel.PlayersRagdoll.Values)
			{
				value.OnSimulationStarted -= MutePlayer;
				value.OnSimulationStopped -= UnMutePlayer;
			}
		}

		private void SubscribeToRagdoll(int playerId, PlayerRagdollEntity playerRagdollEntity)
		{
			if (playerId != _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				playerRagdollEntity.OnSimulationStarted += MutePlayer;
				playerRagdollEntity.OnSimulationStopped += UnMutePlayer;
			}
		}

		private void MutePlayer(IRagdollEntity ragdollEntity)
		{
			PlayerRagdollEntity playerRagdollEntity = ragdollEntity as PlayerRagdollEntity;
			_voiceService.ProcessFadeEffectForSpeaker(playerRagdollEntity.PlayerId, -1f, playerRagdollEntity.VoiceChangeDuration, "Pitch");
			_voiceService.ProcessFadeVolumeEffectForSpeaker(playerRagdollEntity.PlayerId, 0f, playerRagdollEntity.VoiceChangeDuration);
		}

		private void UnMutePlayer(IRagdollEntity ragdollEntity)
		{
			PlayerRagdollEntity playerRagdollEntity = ragdollEntity as PlayerRagdollEntity;
			_voiceService.ProcessFadeEffectForSpeaker(playerRagdollEntity.PlayerId, 0f, playerRagdollEntity.VoiceChangeDuration, "Pitch");
			_voiceService.ProcessFadeVolumeEffectForSpeaker(playerRagdollEntity.PlayerId, 1f, playerRagdollEntity.VoiceChangeDuration);
		}
	}
}
