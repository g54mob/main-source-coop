using System.Collections.Generic;
using System.Linq;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class TeleportationPointsByStateRegistrar : NetworkBehaviour
	{
		[SerializeField]
		private PlayerState _playerState;

		[SerializeField]
		private List<Transform> _teleportationPoints;

		private List<TeleportationPoint> _convertedTeleportationPoints;

		private TeleportationPointsEventClass _teleportationPointsEventClass;

		[Inject]
		public void InjectDependencies(TeleportationPointsEventClass teleportationPointsEventClass)
		{
			_teleportationPointsEventClass = teleportationPointsEventClass;
		}

		public override void Spawned()
		{
			base.Spawned();
			_convertedTeleportationPoints = _teleportationPoints.Select((Transform x) => new TeleportationPoint(x.position, x.rotation)).ToList();
			_teleportationPointsEventClass.RegisterPoint(_playerState, _convertedTeleportationPoints);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_teleportationPointsEventClass.UnRegisterPoint(_playerState, _convertedTeleportationPoints);
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
