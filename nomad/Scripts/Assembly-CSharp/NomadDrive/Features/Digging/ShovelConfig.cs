using Ami.BroAudio;
using EvilCore.Particles;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Digging
{
	[CreateAssetMenu(menuName = "NomadDrive/Tools/Shovel", fileName = "ShovelConfig")]
	public class ShovelConfig : ScriptableObject
	{
		[Header("Surface Aim")]
		[Tooltip("Max distance the camera ray reaches to find the terrain surface to dig.")]
		[Range(1f, 10f)]
		public float maxRayDistance = 3f;

		[Header("Treasure Detection (Capsule)")]
		[Tooltip("Layer of buried-treasure detection colliders. Cast straight down from the dig point.")]
		public LayerMask treasureDetectionLayer;

		[Range(0.05f, 2f)]
		public float detectRadius = 0.3f;

		[Tooltip("How deep below the surface point the capsule probes for a buried treasure.")]
		[Range(5f, 20f)]
		public float detectDepth = 15f;

		[Header("Timing")]
		[Range(0.05f, 2f)]
		public float cooldown = 0.4f;

		[Header("Ground Hover-Snap")]
		[Tooltip("Shovel orientation while planted, applied on top of the player's FACING YAW (fixed relative to the player; does NOT tilt with the look angle). Tune so the blade points down/forward into the ground.")]
		public Vector3 digRotationEuler = new Vector3(80f, 0f, 0f);

		[Tooltip("Offset in the dig-rotation frame so the BLADE TIP (not the root) reaches the aim point.")]
		public Vector3 digLocalPositionOffset = Vector3.zero;

		[Tooltip("Lifts the planted pose along the surface normal (clearance above the ground).")]
		public float digSurfaceClearance;

		[Tooltip("Time to PLANT the shovel into the ground on hover (also Phase 1 of the dig scoop: the downward plunge).")]
		[Range(0.02f, 1f)]
		public float digDownDuration = 0.14f;

		public Ease digDownEase = Ease.OutCubic;

		[Header("Dig Scoop — vertical (plunge → lift → settle) + rotation IN PARALLEL (simultaneous, not after)")]
		[Tooltip("Plunge depth (down along the surface normal) as the blade drives into the ground.")]
		public float digPlungeDepth = 0.12f;

		[Tooltip("Lift height (up along the surface normal) as the blade scoops.")]
		public float digScoopLift = 0.15f;

		[Tooltip("Scoop rotation (planted local frame) blended in WHILE the blade descends + lifts (parallel), released on settle — the 'toss'.")]
		public Vector3 digScoopRotationEuler = new Vector3(-35f, 0f, 0f);

		[Tooltip("Lift duration (the rotation ramps over the plunge + lift, in parallel with the vertical).")]
		[Range(0.02f, 1f)]
		public float digScoopDuration = 0.18f;

		public Ease digScoopEase = Ease.OutCubic;

		[Tooltip("Phase 3 duration: settle back to the rest pose (remotes also fly the shovel back to the hand). Hover UN-snap stays instant.")]
		[Range(0.02f, 1f)]
		public float digUpDuration = 0.22f;

		public Ease digUpEase = Ease.InOutCubic;

		[Header("Audio")]
		[Tooltip("Special strike sound — plays only on the FIRST detection of a buried treasure.")]
		public SoundID strikeSound;

		[Tooltip("Normal dig sound — empty digs and continued excavation after the first detection.")]
		public SoundID digSound;

		[Header("Particle")]
		[Tooltip("Particle played at the dig point on each dig (via IParticlesManager). Leave unset for none.")]
		public ParticleKey digParticle;
	}
}
