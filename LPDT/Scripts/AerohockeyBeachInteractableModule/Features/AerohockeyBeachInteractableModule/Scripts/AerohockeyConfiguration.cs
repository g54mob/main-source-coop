using FMODUnity;
using UnityEngine;

namespace Features.AerohockeyBeachInteractableModule.Scripts
{
	[CreateAssetMenu(fileName = "AerohockeyConfiguration_Default", menuName = "Configurations/AerohockeyBeachInteractableModule/AerohockeyConfiguration")]
	public class AerohockeyConfiguration : ScriptableObject
	{
		[Header("Paddle")]
		[field: SerializeField]
		[field: Min(0.1f)]
		public float PaddleMoveSpeed { get; private set; } = 4f;

		[Header("Puck speed")]
		[field: SerializeField]
		[field: Min(0.1f)]
		public float PuckMaxSpeed { get; private set; } = 4f;

		[Header("Puck feel (drag + bounce)")]
		[field: SerializeField]
		[field: Min(0f)]
		public float PuckLinearDamping { get; private set; } = 1.1f;

		[field: SerializeField]
		[field: Min(0f)]
		public float PuckAngularDamping { get; private set; } = 1.2f;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float WallRestitution { get; private set; } = 0.72f;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float WallTangentRetain { get; private set; } = 0.85f;

		[field: SerializeField]
		[field: Range(0.05f, 1f)]
		public float PaddleHitVelocityScale { get; private set; } = 0.85f;

		[field: SerializeField]
		[field: Min(0f)]
		public int ScoreToWin { get; private set; }

		[Header("After goal")]
		[field: SerializeField]
		[field: Min(0f)]
		public float PuckRespawnDelaySeconds { get; private set; } = 1.5f;

		[Header("Sounds (FMOD)")]
		[field: SerializeField]
		public EventReference PaddleHitSound { get; private set; }

		[field: SerializeField]
		public EventReference WallHitSound { get; private set; }

		[field: SerializeField]
		public EventReference GoalSound { get; private set; }
	}
}
