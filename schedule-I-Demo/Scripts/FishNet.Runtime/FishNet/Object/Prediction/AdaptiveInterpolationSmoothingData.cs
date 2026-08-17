using System;
using UnityEngine;

namespace FishNet.Object.Prediction
{
	[Serializable]
	public struct AdaptiveInterpolationSmoothingData
	{
		[NonSerialized]
		[HideInInspector]
		public bool SmoothPosition;

		[NonSerialized]
		[HideInInspector]
		public bool SmoothRotation;

		[NonSerialized]
		[HideInInspector]
		public bool SmoothScale;

		[NonSerialized]
		[HideInInspector]
		public Transform GraphicalObject;

		[NonSerialized]
		[HideInInspector]
		public NetworkObject NetworkObject;

		[NonSerialized]
		[HideInInspector]
		public float TeleportThreshold;

		[Tooltip("Percentage of ping to use as interpolation. Higher values will result in more interpolation.")]
		[Range(0.01f, 5f)]
		public float NormalPercent;

		[Tooltip("Percentage of ping to use as interpolation when colliding with an object local client owns.This is used to speed up local interpolation when predicted objects collide with a player as well keep graphics closer to the objects root while colliding.")]
		[Range(0.01f, 5f)]
		public float CollisionPercent;

		[Tooltip("How much per tick to decrease to collision interpolation when colliding with a local player object. Higher values will set interpolation to collision settings faster.")]
		[Range(0.1f, 10f)]
		public float CollisionStep;

		[Tooltip("How much per tick to increase to normal interpolation when not colliding with a local player object. Higher values will set interpolation to normal settings faster.")]
		[Range(0.1f, 10f)]
		public float NormalStep;

		internal const byte BASE_COLLISION_INTERPOLATION = 0;

		internal const byte BASE_NORMAL_INTERPOLATION = 1;
	}
}
