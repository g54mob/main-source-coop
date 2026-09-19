using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyPorterSettings_Default", menuName = "Configurations/AIModuleStateMachine/MonkeyPorter/MonkeyPorterSettings")]
	public class MonkeyPorterSettings : ScriptableObject
	{
		[Header("Movement")]
		[field: SerializeField]
		public float MoveSpeed { get; private set; } = 5f;

		[field: SerializeField]
		public float CarrySpeed { get; private set; } = 4.5f;

		[field: SerializeField]
		public float StoppingDistance { get; private set; } = 1f;

		[Header("Steering")]
		[Tooltip("Degrees per second of turn per degree of heading error.")]
		[field: SerializeField]
		public float RotationSpeed { get; private set; } = 5f;

		[field: SerializeField]
		public float MaxTurnSpeed { get; private set; } = 160f;

		[Tooltip("Turn-rate ramp, deg/s². Low = heavy cart.")]
		[field: SerializeField]
		public float TurnAcceleration { get; private set; } = 480f;

		[Tooltip("Lowest fraction of full speed in a hard turn.")]
		[field: SerializeField]
		public float MinTurnSpeedScale { get; private set; } = 0.15f;

		[Tooltip("Distance at which she starts aiming at the next path corner. 0 disables.")]
		[field: SerializeField]
		public float LookAheadDistance { get; private set; } = 1.5f;

		[Tooltip("Low-pass on steering direction. Lower = smoother but lazier.")]
		[field: SerializeField]
		public float SteeringSmoothing { get; private set; } = 12f;

		[field: SerializeField]
		public float SpeedAnimationLerpSpeed { get; private set; } = 8f;

		[Header("Behaviour")]
		[field: SerializeField]
		public float WaitRadius { get; private set; } = 3f;

		[field: SerializeField]
		public float LeashRadius { get; private set; } = 8f;

		[field: SerializeField]
		public float FollowSwitchMargin { get; private set; } = 2f;

		[field: SerializeField]
		public float RepathInterval { get; private set; } = 0.5f;

		[field: SerializeField]
		public float BoatArriveThreshold { get; private set; } = 2f;

		[field: SerializeField]
		public float NavMeshSampleRadius { get; private set; } = 4f;

		[field: SerializeField]
		public float TakeIntoCartDuration { get; private set; } = 0.8f;

		[Header("Delivery")]
		[Tooltip("Where she stops before throwing. Bigger = throws from farther.")]
		[field: SerializeField]
		public float BoatStopDistance { get; private set; } = 4f;

		[Tooltip("Used instead of BoatStopDistance when the boat has an authored approach point.")]
		[field: SerializeField]
		public float BoatApproachStopDistance { get; private set; } = 0.75f;

		[Tooltip("Height of the throw arc above the cart-to-boat line.")]
		[field: SerializeField]
		public float DeliverApexHeight { get; private set; } = 1.4f;

		[field: SerializeField]
		public float DeliverThrowDuration { get; private set; } = 0.6f;

		[field: SerializeField]
		public float DeliverHoldAfterThrow { get; private set; } = 1f;

		[Tooltip("Heading error within which she counts as facing the boat.")]
		[field: SerializeField]
		public float DeliverAimTolerance { get; private set; } = 15f;

		[Tooltip("How long the throw waits for her to face the boat before throwing anyway.")]
		[field: SerializeField]
		public float DeliverAimTimeout { get; private set; } = 2f;

		[field: SerializeField]
		public float ThrowFlightDuration { get; private set; } = 1f;

		[Header("Roam limits")]
		[Tooltip("Leash from the spawn point. 0 = no horizontal limit, only the height one.")]
		[field: SerializeField]
		public float RoamRadius { get; private set; }

		[Tooltip("Above a staircase inside one room, below a floor-to-floor climb.")]
		[field: SerializeField]
		public float RoamMaxHeightDelta { get; private set; } = 3f;

		[Tooltip("How close to the border she stops instead of nudging at it every tick.")]
		[field: SerializeField]
		public float RoamBoundaryStopDistance { get; private set; } = 1.5f;

		[Header("Blocked path")]
		[Tooltip("How long she must be pressed against something, while still wanting to move.")]
		[field: SerializeField]
		public float BlockedDetectDelay { get; private set; } = 1.5f;

		[Tooltip("Path re-check period. Each re-check allocates a NavMeshPath.")]
		[field: SerializeField]
		public float BlockedRecheckInterval { get; private set; } = 1.5f;

		[field: SerializeField]
		public float ComplainCooldown { get; private set; } = 5f;

		[field: SerializeField]
		public float AngryFaceLerpSpeed { get; private set; } = 3f;

		[Tooltip("How far ahead she probes for a player body. Players do not carve the navmesh.")]
		[field: SerializeField]
		public float PlayerBlockCheckDistance { get; private set; } = 1.2f;

		[Header("Threat")]
		[Tooltip("Keep close to enemy chase speed so she can be caught.")]
		[field: SerializeField]
		public float FleeSpeed { get; private set; } = 3.5f;

		[Tooltip("Detection radius. Line of sight is required on top of it.")]
		[field: SerializeField]
		public float ThreatDetectRadius { get; private set; } = 10f;

		[Tooltip("Sticky radius of an already spotted enemy. Larger than detection so it does not flicker.")]
		[field: SerializeField]
		public float ThreatForgetRadius { get; private set; } = 12f;

		[Tooltip("This close counts as standing on top of her and restarts the escape.")]
		[field: SerializeField]
		public float ThreatOverrideRadius { get; private set; } = 4f;

		[Tooltip("Every enemy must be this far away before she leaves cover.")]
		[field: SerializeField]
		public float ThreatClearRadius { get; private set; } = 10f;

		[Tooltip("Cap on a single run to a cover spot.")]
		[field: SerializeField]
		public float MaxRunToCoverTime { get; private set; } = 20f;

		[Tooltip("Cap on sitting in cover, so a wandering enemy cannot park her there for the level.")]
		[field: SerializeField]
		public float MaxHideTime { get; private set; } = 45f;

		[Tooltip("Cap on the whole escape across all phases. Last-resort watchdog.")]
		[field: SerializeField]
		public float MaxFleeTime { get; private set; } = 90f;

		[Tooltip("Quiet time after an escape before she may panic again.")]
		[field: SerializeField]
		public float FleeCooldown { get; private set; } = 4f;

		[Tooltip("Scan period. Each scan does one line-of-sight raycast.")]
		[field: SerializeField]
		public float ThreatScanInterval { get; private set; } = 0.3f;

		[Tooltip("How long she squeals in place before running.")]
		[field: SerializeField]
		public float SignalDuration { get; private set; } = 0.4f;

		[Tooltip("How long she stays in cover after the last threat is gone.")]
		[field: SerializeField]
		public float HideHoldTime { get; private set; } = 5f;

		[field: SerializeField]
		public float CoverSearchRadius { get; private set; } = 12f;

		[Tooltip("Highest ceiling over a candidate point that still counts as cover. The agent is 0.6 high.")]
		[field: SerializeField]
		public float CoverCeilingHeight { get; private set; } = 1.2f;

		[Tooltip("Lowest gap she can still crawl under. Below it the object is solid, not cover.")]
		[field: SerializeField]
		public float CoverMinClearance { get; private set; } = 0.5f;

		[Tooltip("Smallest footprint counting as cover in the fallback search. Authored PlayerSafeZones ignore it.")]
		[field: SerializeField]
		public float CoverMinWidth { get; private set; } = 1f;

		[Tooltip("Cover re-search period while running. Each run allocates NavMeshPaths.")]
		[field: SerializeField]
		public float CoverRetryInterval { get; private set; } = 1f;

		[Tooltip("How far she runs when there is no cover around at all.")]
		[field: SerializeField]
		public float FleeFallbackRadius { get; private set; } = 20f;

		[Tooltip("No progress for this long while escaping = drop that spot and pick another.")]
		[field: SerializeField]
		public float FleeStuckTime { get; private set; } = 1.5f;

		[Header("Noise")]
		[Tooltip("Hearing radius while walking empty. 0 disables walking noise.")]
		[field: SerializeField]
		public float MoveNoiseRadius { get; private set; } = 5f;

		[Tooltip("Hearing radius while the cart carries loot.")]
		[field: SerializeField]
		public float CarryNoiseRadius { get; private set; } = 8f;

		[field: SerializeField]
		public float MoveNoiseInterval { get; private set; } = 1.2f;

		[Tooltip("Hearing radius of the one-shot panic squeal.")]
		[field: SerializeField]
		public float PanicNoiseRadius { get; private set; } = 12f;

		[Tooltip("Below this agent speed she counts as standing still and makes no noise.")]
		[field: SerializeField]
		public float NoiseMoveSpeedThreshold { get; private set; } = 0.5f;

		[field: SerializeField]
		public float PanicSoundCooldown { get; private set; } = 4f;

		[Header("Mortality")]
		[field: SerializeField]
		public float MaxHealth { get; private set; } = 30f;

		[Tooltip("Grabbable cart left behind on death. Empty = just despawn the cart.")]
		[field: SerializeField]
		public NetworkObject DeadCartPrefab { get; private set; }
	}
}
