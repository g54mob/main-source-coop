using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Observing;
using UnityEngine;

namespace FishNet.Component.Observing
{
	[CreateAssetMenu(menuName = "FishNet/Observers/Grid Condition", fileName = "New Grid Condition")]
	public class GridCondition : ObserverCondition
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool ConditionMet(NetworkConnection connection, bool currentlyAdded, out bool notProcessed)
		{
			notProcessed = false;
			return connection.HashGridEntry.NearbyEntries.Contains(NetworkObject.HashGridEntry);
		}

		public override ObserverConditionType GetConditionType()
		{
			return ObserverConditionType.Timed;
		}

		public override ObserverCondition Clone()
		{
			return ScriptableObject.CreateInstance<GridCondition>();
		}
	}
}
