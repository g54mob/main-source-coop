using System;
using Fusion;
using UnityEngine;

namespace Features.SynchronizationGateModule
{
	[NetworkBehaviourWeaved(35)]
	public class SynchronizationGateArrival : NetworkBehaviour
	{
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Lane", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Lane;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Visit", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Visit;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("OwnerId", 2, 33)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkString<_32> _OwnerId;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe int Lane
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SynchronizationGateArrival.Lane. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SynchronizationGateArrival.Lane. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe int Visit
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SynchronizationGateArrival.Visit. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SynchronizationGateArrival.Visit. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 33)]
		public unsafe NetworkString<_32> OwnerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SynchronizationGateArrival.OwnerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkString<_32>*)(Ptr + 2);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SynchronizationGateArrival.OwnerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkString<_32>*)(Ptr + 2) = value;
			}
		}

		public string OwnerIdValue => OwnerId.ToString();

		public bool TryInitialize(string ownerId, int lane, int visit)
		{
			if (!base.HasStateAuthority || string.IsNullOrEmpty(ownerId))
			{
				return false;
			}
			OwnerId = ownerId;
			Lane = lane;
			Visit = visit;
			return true;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Lane = _Lane;
			Visit = _Visit;
			OwnerId = _OwnerId;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Lane = Lane;
			_Visit = Visit;
			_OwnerId = OwnerId;
		}
	}
}
