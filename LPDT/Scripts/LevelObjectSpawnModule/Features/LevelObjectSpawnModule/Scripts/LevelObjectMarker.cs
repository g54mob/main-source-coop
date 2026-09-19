using System;
using Fusion;
using UnityEngine;

namespace Features.LevelObjectSpawnModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class LevelObjectMarker : NetworkBehaviour
	{
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Type", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private LevelObjectType _Type;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe LevelObjectType Type
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelObjectMarker.Type. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((LevelObjectType*)Ptr)[0];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelObjectMarker.Type. Networked properties can only be accessed when Spawned() has been called.");
				}
				((LevelObjectType*)Ptr)[0] = value;
			}
		}

		public void SetType(LevelObjectType type)
		{
			if (base.HasStateAuthority)
			{
				Type = type;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Type = _Type;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Type = Type;
		}
	}
}
