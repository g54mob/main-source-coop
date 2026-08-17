using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Observing;
using UnityEngine;

namespace FishNet.Component.Observing
{
	[CreateAssetMenu(menuName = "FishNet/Observers/Distance Condition", fileName = "New Distance Condition")]
	public class DistanceCondition : ObserverCondition
	{
		[Tooltip("Maximum distance a client must be within this object to see it.")]
		[SerializeField]
		private float _maximumDistance = 100f;

		[Tooltip("Additional percent of distance client must be until this object is hidden. For example, if distance was 100f and percent was 0.5f the client must be 150f units away before this object is hidden again. This can be useful for keeping objects from regularly appearing and disappearing.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float _hideDistancePercent = 0.1f;

		[Obsolete("UpdateFrequency is no longer used.")]
		[HideInInspector]
		public float UpdateFrequency;

		private Dictionary<NetworkConnection, float> _timedUpdates = new Dictionary<NetworkConnection, float>();

		public float MaximumDistance
		{
			get
			{
				return _maximumDistance;
			}
			set
			{
				_maximumDistance = value;
			}
		}

		public void ConditionConstructor(float maximumDistance)
		{
			MaximumDistance = maximumDistance;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool ConditionMet(NetworkConnection connection, bool currentlyAdded, out bool notProcessed)
		{
			notProcessed = false;
			float num2;
			if (currentlyAdded)
			{
				float num = MaximumDistance * (1f + _hideDistancePercent);
				num2 = num * num;
			}
			else
			{
				num2 = MaximumDistance * MaximumDistance;
			}
			Vector3 position = NetworkObject.transform.position;
			foreach (NetworkObject @object in connection.Objects)
			{
				if (Vector3.SqrMagnitude(@object.transform.position - position) <= num2)
				{
					return true;
				}
			}
			return false;
		}

		public override ObserverConditionType GetConditionType()
		{
			return ObserverConditionType.Timed;
		}

		public override ObserverCondition Clone()
		{
			DistanceCondition distanceCondition = ScriptableObject.CreateInstance<DistanceCondition>();
			distanceCondition.ConditionConstructor(MaximumDistance);
			return distanceCondition;
		}
	}
}
