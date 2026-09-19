using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.KrakenModule.Scripts.Data
{
	public readonly struct KrakenThrowContext
	{
		public KrakenThrowMode Mode { get; }

		public IPointGrabable Item { get; }

		public PlayerRef TargetPlayer { get; }

		public Vector3 TargetPosition { get; }

		public KrakenThrowDamageProfile DamageProfile { get; }

		public KrakenTentacleAssignmentStrategy AssignmentStrategy { get; }

		public bool HasExplicitTargetPosition => TargetPosition != default(Vector3);

		public KrakenThrowContext(KrakenThrowMode mode, IPointGrabable item, PlayerRef targetPlayer, Vector3 targetPosition, KrakenThrowDamageProfile damageProfile, KrakenTentacleAssignmentStrategy assignmentStrategy)
		{
			Mode = mode;
			Item = item;
			TargetPlayer = targetPlayer;
			TargetPosition = targetPosition;
			DamageProfile = damageProfile;
			AssignmentStrategy = assignmentStrategy;
		}

		public static KrakenThrowContext ThrowBack(IPointGrabable item)
		{
			return new KrakenThrowContext(KrakenThrowMode.ThrowBack, item, (item?.NetworkObject != null) ? item.NetworkObject.StateAuthority : PlayerRef.None, default(Vector3), KrakenThrowDamageProfile.None, KrakenTentacleAssignmentStrategy.NearestFree);
		}
	}
}
