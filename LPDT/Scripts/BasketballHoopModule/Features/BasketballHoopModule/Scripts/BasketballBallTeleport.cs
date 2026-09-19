using System;
using Features.TeleportModule.Scripts.TeleportCommon;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.BasketballHoopModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public sealed class BasketballBallTeleport : NetworkBehaviour, ITeleportable
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		private IBasketballService _basketballService;

		private BasketballHoopSpawnLocationsModel _spawnLocations;

		private bool _registered;

		public NetworkObject NetworkObject => base.Object;

		public event Action<NetworkObject> OnDespawned;

		[Inject]
		private void InjectDependencies(IBasketballService basketballService, BasketballHoopSpawnLocationsModel spawnLocations)
		{
			_basketballService = basketballService;
			_spawnLocations = spawnLocations;
		}

		public override void Spawned()
		{
			_registered = false;
			TryRegisterWithService();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_registered = false;
			this.OnDespawned?.Invoke(base.Object);
		}

		public void Teleport(Vector3 position)
		{
			if (_rigidbody != null)
			{
				_rigidbody.position = position;
				_rigidbody.linearVelocity = Vector3.zero;
				_rigidbody.angularVelocity = Vector3.zero;
			}
			base.transform.position = position;
		}

		private void TryRegisterWithService()
		{
			if (!_registered && _basketballService != null && _spawnLocations != null && _spawnLocations.IsConfigured && !(base.Object == null) && base.Object.IsValid)
			{
				_basketballService.RegisterBall(base.Object, _spawnLocations.SpawnPosition, _spawnLocations.DistanceTrackPosition, this);
				_registered = true;
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
