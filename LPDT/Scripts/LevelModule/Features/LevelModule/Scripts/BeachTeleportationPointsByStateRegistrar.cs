using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts
{
	public class BeachTeleportationPointsByStateRegistrar : MonoBehaviour
	{
		[SerializeField]
		private List<Transform> _teleportationPoints;

		private List<TeleportationPoint> _convertedTeleportationPoints;

		private TeleportationPointsEventClass _teleportationPointsEventClass;

		[Inject]
		public void InjectDependencies(TeleportationPointsEventClass teleportationPointsEventClass)
		{
			_teleportationPointsEventClass = teleportationPointsEventClass;
		}

		private void OnEnable()
		{
			_convertedTeleportationPoints = _teleportationPoints.Select((Transform x) => new TeleportationPoint(x.position, x.rotation)).ToList();
			_teleportationPointsEventClass.RegisterBeachPoints(_convertedTeleportationPoints);
		}

		private void OnDisable()
		{
			_teleportationPointsEventClass.UnregisterBeachPoints(_convertedTeleportationPoints);
		}
	}
}
