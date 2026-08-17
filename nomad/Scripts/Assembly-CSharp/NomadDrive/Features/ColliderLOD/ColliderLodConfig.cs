using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.ColliderLOD
{
	[CreateAssetMenu(menuName = "NomadDrive/Performance/Collider LOD Config", fileName = "ColliderLodConfig")]
	public class ColliderLodConfig : ScriptableObject
	{
		[Serializable]
		public struct TypeRadiusOverride
		{
			[Tooltip("Component type name (e.g. \"GasStation\"). Matched against the target's runtime type.")]
			public string componentTypeName;

			public float activationRadius;
		}

		[Header("Master")]
		[Tooltip("Global on/off. When false, all colliders are forced active (LOD axis = true).")]
		public bool masterEnabled = true;

		[Header("Radii")]
		[Tooltip("Distance from any player within which a target's colliders are enabled. Small base radius (loot/props live within a few metres of the player); fast movers are covered by the velocity look-ahead below, not by a large base.")]
		public float activationRadius = 5f;

		[Tooltip("Added to activationRadius for the disable test. Prevents flicker at the boundary.")]
		public float hysteresisBuffer = 1.5f;

		[Tooltip("Seconds of player movement to look ahead. effectiveRadius += playerSpeed * this. Absorbs eval latency for fast movers so they never tunnel into not-yet-enabled colliders.")]
		public float velocityLookaheadSeconds = 0.5f;

		[Header("Evaluation")]
		[Tooltip("Seconds between full evaluation cycles.")]
		public float evaluationInterval = 0.2f;

		[Tooltip("Max entries processed per frame while a cycle is in progress (time-slicing).")]
		public int entriesPerSlice = 200;

		[Tooltip("Max collider DISABLE operations applied per frame. Enables are never budgeted (deferring an enable risks tunneling). Spreads chunk-boundary disable spikes.")]
		public int maxDisableTogglesPerFrame = 50;

		[Header("Spatial Grid")]
		[Tooltip("Uniform XZ spatial-hash cell size. Sized for the largest effective query radius (base + velocity look-ahead for fast vehicles), not the small base radius.")]
		public float gridCellSize = 12f;

		[Tooltip("World distance a moving target must travel before it is re-bucketed in the grid.")]
		public float rebucketThreshold = 2f;

		[Header("Behavior")]
		[Tooltip("Also LOD trigger colliders. Default off - triggers drive equip/snapping detection and are far fewer.")]
		public bool includeTriggerColliders;

		[Tooltip("When no player positions are known (early load / disconnect), keep all colliders active.")]
		public bool failSafeKeepActive = true;

		[Header("Debug")]
		[Tooltip("Draw player activation/deactivation spheres and target states in the editor.")]
		public bool drawGizmos;

		[Tooltip("Max entries to draw gizmos for (avoids tanking the editor with thousands of objects).")]
		public int maxGizmoEntries = 500;

		[Tooltip("Per-component-type activation radius overrides (e.g. large POIs). Empty = use activationRadius for all.")]
		public List<TypeRadiusOverride> typeOverrides = new List<TypeRadiusOverride>();

		private const float MaxInteractionDistance = 3f;

		private const float SafetyMargin = 1.5f;

		public float DeactivationRadius => activationRadius + hysteresisBuffer;

		private void OnValidate()
		{
			float num = 4.5f;
			if (activationRadius < num)
			{
				activationRadius = num;
			}
			if (hysteresisBuffer < 0f)
			{
				hysteresisBuffer = 0f;
			}
			if (evaluationInterval < 0.01f)
			{
				evaluationInterval = 0.01f;
			}
			if (entriesPerSlice < 1)
			{
				entriesPerSlice = 1;
			}
			if (maxDisableTogglesPerFrame < 1)
			{
				maxDisableTogglesPerFrame = 1;
			}
			if (gridCellSize < 1f)
			{
				gridCellSize = 1f;
			}
			if (velocityLookaheadSeconds < 0f)
			{
				velocityLookaheadSeconds = 0f;
			}
		}
	}
}
