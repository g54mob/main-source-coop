using EvilCore;
using UnityEngine;

namespace NomadDrive.Features.Player
{
	public class PlayerRespawnService : IPlayerRespawnService
	{
		private const float RespawnCooldownSeconds = 2f;

		private readonly IPlayerService _playerService;

		private float _nextAllowedRespawnTime;

		public bool CanRespawn
		{
			get
			{
				if (_playerService.IsPlayerSpawned && _playerService.LocalPlayer != null)
				{
					return !_playerService.LocalPlayer.IsPlayerSitting();
				}
				return false;
			}
		}

		public PlayerRespawnService(IPlayerService playerService)
		{
			_playerService = playerService;
		}

		public void RequestRespawn()
		{
			if (CanRespawn && !(Time.unscaledTime < _nextAllowedRespawnTime))
			{
				_nextAllowedRespawnTime = Time.unscaledTime + 2f;
				_playerService.LocalPlayer.RespawnUnstuck();
			}
		}
	}
}
