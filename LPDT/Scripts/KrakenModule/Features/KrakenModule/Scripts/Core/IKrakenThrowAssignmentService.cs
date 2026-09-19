using Features.GrabModule.Scripts;
using Features.KrakenModule.Scripts.Data;
using UnityEngine;

namespace Features.KrakenModule.Scripts.Core
{
	public interface IKrakenThrowAssignmentService
	{
		bool IsGrabbableValid(IPointGrabable pointGrabable);

		bool IsItemAlreadyAssigned(TentacleController[] tentacles, IPointGrabable pointGrabable);

		bool TryFindTentacle(TentacleController[] tentacles, IPointGrabable item, Vector3 fromPosition, KrakenTentacleAssignmentStrategy strategy, out TentacleController tentacle);
	}
}
