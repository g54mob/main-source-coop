using NomadDrive.Features.Player;
using NomadDrive.Features.Vehicle.Collision;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleCollisionOccupantDamageModule : VehicleModule
	{
		[SerializeField]
		private VehicleCollisionDamageConfig config;

		[Inject]
		private IPlayerService _playerService;

		protected override void SubscribeEvents()
		{
			base.EventBus.OnCollisionAllClients += OnCollisionAllClients;
		}

		protected override void UnsubscribeEvents()
		{
			base.EventBus.OnCollisionAllClients -= OnCollisionAllClients;
		}

		private void OnCollisionAllClients(VehicleCollisionData data)
		{
			if (config == null || (int)data.Severity < (int)config.MinSeverity)
			{
				return;
			}
			NomadDrive.Features.Player.Player player = _playerService?.LocalPlayer;
			if (!(player == null) && player.IsRidingVehicle(base.VehicleManager) && _playerService.TryGetStatsManager(out var manager))
			{
				float t = Mathf.InverseLerp(config.MinDamageForce, config.MaxDamageForce, data.Force);
				float num = Mathf.Lerp(config.MinDamage, config.MaxDamage, t);
				if (!(num <= 0f))
				{
					manager.ApplyDirectDamage(num);
				}
			}
		}
	}
}
