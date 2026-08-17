using NomadDrive.Features.ObjectPlacement;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace NomadDrive.Features.Interaction
{
	[CreateAssetMenu(menuName = "NomadDrive/Interaction/World Object Config", fileName = "WorldObjectConfig")]
	public class HeldItemConfig : SerializedScriptableObject
	{
		[Tooltip("Default rotation when entering placing mode")]
		public Vector3 enterPlacingModeStartRotation;

		[Tooltip("Which axis can be rotated while in placing mode")]
		public RotationAxis placingModeRotationAxis;

		[FormerlySerializedAs("virtualScaleForPlacingMode")]
		[Tooltip("Additional local-space offset applied on top of auto-calculated bounds center during placement")]
		public Vector3 positionOffsetForPlacement = Vector3.zero;

		public float placementShaderScale = 1f;

		[Tooltip("Ghost preview style: ModelClone shows full mesh, FootprintPlane shows a scaled cube footprint")]
		public GhostPreviewMode ghostPreviewMode;

		[Tooltip("Scale of the footprint ghost cube (used when ghostPreviewMode is FootprintPlane)")]
		public Vector3 ghostPreviewScale = new Vector3(1f, 0.1f, 1f);

		[Tooltip("Multiplier for the dynamically calculated placement grid radius")]
		public float placementRadiusMultiplier = 1f;

		[Tooltip("Default distance from camera when entering placement mode")]
		public float defaultPlacementDistance = 0.75f;

		[Tooltip("Minimum distance from camera during placement")]
		public float minPlacementDistance = 0.75f;

		[Tooltip("Maximum distance from camera during placement")]
		public float maxPlacementDistance = 2f;

		[Tooltip("Position offset when held in player's hand")]
		public Vector3 positionOnHand = new Vector3(0.25f, 1.4f, 0.5f);

		[Tooltip("Rotation when held in player's hand")]
		public Vector3 rotationOnHand = Vector3.zero;

		[Tooltip("Material category for drop/impact sound selection. Generic falls back to surface-only sound.")]
		public ItemMaterial itemMaterial;

		[Tooltip("Override max retraction distance for this item (0 = use manager default)")]
		public float collisionClippingMaxRetraction;

		[Tooltip("Disable collision clipping for this specific item")]
		public bool disableCollisionClipping;

		[Tooltip("Forward raycast length from camera to detect wall poke-through (0 = use manager default)")]
		public float clippingRayLength;

		[Tooltip("Local-space offset for the proximity probe relative to grip point (e.g., (0,0,0.3) moves probe toward item tip)")]
		public Vector3 clippingProbeOffset = Vector3.zero;

		[Tooltip("Override probe radius for this item (0 = use manager default)")]
		public float clippingProbeRadius;
	}
}
