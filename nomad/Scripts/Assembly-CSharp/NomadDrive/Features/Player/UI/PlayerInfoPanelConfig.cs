using UnityEngine;
using UnityEngine.Serialization;

namespace NomadDrive.Features.Player.UI
{
	[CreateAssetMenu(fileName = "PlayerInfoPanelConfig", menuName = "NomadDrive/Player/PlayerInfoPanelConfig")]
	public class PlayerInfoPanelConfig : ScriptableObject
	{
		[Header("Distance")]
		[Tooltip("Below this distance the panel renders at full scale.")]
		public float fullSizeDistance = 5f;

		[Tooltip("Beyond this distance the panel is hidden entirely.")]
		public float maxVisibleDistance = 25f;

		[Tooltip("Scale multiplier applied at maxVisibleDistance (before fade-out).")]
		[Range(0.1f, 1f)]
		public float minScale = 0.45f;

		[Tooltip("Fraction of the fade range used for the alpha falloff (1 = fades across the whole band).")]
		[Range(0.2f, 1f)]
		public float fadeRangeRatio = 0.7f;

		[Header("Speaking Indicator")]
		[Tooltip("How long the speaking icon stays lit after the last 'speaking=true' event from EOS. Smooths flicker.")]
		public float speakingHoldSeconds = 0.2f;

		[Tooltip("Mic icon fade-in/out duration (alpha 0 ↔ 1).")]
		public float speakingFadeDuration = 0.15f;

		[Tooltip("Speaking pulse: how much the mic icon scales up.")]
		[Range(1f, 1.5f)]
		public float speakingPulseScale = 1.18f;

		[Tooltip("Speaking pulse: duration of one half-cycle.")]
		public float speakingPulseDuration = 0.35f;

		[Header("Occlusion")]
		[Tooltip("If true, a raycast from camera to head fades the panel when blocked.")]
		public bool occlusionCheckEnabled = true;

		[Tooltip("Layers that count as occluders (walls, vehicles, etc).")]
		public LayerMask occluderLayerMask = -1;

		[Tooltip("Alpha multiplier when the panel is occluded.")]
		[Range(0.05f, 1f)]
		public float occludedAlphaMultiplier = 0.3f;

		[Header("Layout")]
		[Tooltip("STANDING offset from the player root, in PLAYER-LOCAL space. Rotates with player yaw so the offset stays consistent from any camera angle.\n  X = player's right (positive) / left (negative) — tune to visually center label\n  Y = world up (player has no pitch/roll); ~1.75 = just above head for 1.6m capsule\n  Z = player's forward (positive) / behind (negative)\nLive-editable at runtime: change in PlayMode and panel eases to the new offset.")]
		[FormerlySerializedAs("anchorOffset")]
		public Vector3 standingOffset = new Vector3(0f, 1.75f, 0f);

		[Tooltip("Offset used while the player is crouching (lower than standing).")]
		public Vector3 crouchedOffset = new Vector3(0f, 1.2f, 0f);

		[Tooltip("Offset used while the player is sitting (e.g. in a vehicle seat).")]
		public Vector3 sittingOffset = new Vector3(0f, 1.3f, 0f);

		[Tooltip("Offset used while the player is downed. Measured from the chicken transform, so keep Y small/near-ground.")]
		public Vector3 downedOffset = new Vector3(0f, 0.5f, 0f);

		[Tooltip("SmoothDamp time (seconds) for easing between state offsets. 0 = snap instantly.")]
		public float offsetSmoothTime = 0.15f;

		[Tooltip("World-space canvas scale (small — TMP characters are huge by default).")]
		public float baseCanvasScale = 0.005f;
	}
}
