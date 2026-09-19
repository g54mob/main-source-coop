using System;
using Fusion;
using UnityEngine;

namespace Features.CustomSynchronizersModule.Scripts
{
	[NetworkBehaviourWeaved(7)]
	public class SpawnSynchronizer : NetworkBehaviour
	{
		[WeaverGenerated]
		[DefaultForProperty("Position", 0, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3Compressed _Position;

		[WeaverGenerated]
		[DefaultForProperty("Rotation", 3, 4)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private QuaternionCompressed _Rotation;

		[Networked]
		[NetworkedWeaved(0, 3)]
		private unsafe Vector3Compressed Position
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SpawnSynchronizer.Position. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3Compressed*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SpawnSynchronizer.Position. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3Compressed*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 4)]
		private unsafe QuaternionCompressed Rotation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SpawnSynchronizer.Rotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(QuaternionCompressed*)(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SpawnSynchronizer.Rotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(QuaternionCompressed*)(Ptr + 3) = value;
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.HasStateAuthority)
			{
				Position = base.transform.position;
				Rotation = base.transform.rotation;
			}
			else
			{
				base.transform.position = Position;
				base.transform.rotation = ((Quaternion)Rotation).normalized;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Position = _Position;
			Rotation = _Rotation;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Position = Position;
			_Rotation = Rotation;
		}
	}
}
