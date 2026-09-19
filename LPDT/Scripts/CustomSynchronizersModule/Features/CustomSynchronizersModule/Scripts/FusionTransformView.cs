using System;
using Fusion;
using UnityEngine;

namespace Features.CustomSynchronizersModule.Scripts
{
	[NetworkBehaviourWeaved(11)]
	public class FusionTransformView : NetworkBehaviour
	{
		private const float THRESHOLD = 100000f;

		[SerializeField]
		private float _interpolationSpeed = 5f;

		[SerializeField]
		private float _maxLerpDistance = 2f;

		[Header("Position")]
		[SerializeField]
		private bool _syncPositionX = true;

		[SerializeField]
		private bool _syncPositionY = true;

		[SerializeField]
		private bool _syncPositionZ = true;

		[Header("Rotation")]
		[SerializeField]
		private bool _syncRotation = true;

		[Header("Scale")]
		[SerializeField]
		private bool _syncScaleX;

		[SerializeField]
		private bool _syncScaleY;

		[SerializeField]
		private bool _syncScaleZ;

		[WeaverGenerated]
		[DefaultForProperty("PositionX", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ushort _PositionX;

		[WeaverGenerated]
		[DefaultForProperty("PositionY", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ushort _PositionY;

		[WeaverGenerated]
		[DefaultForProperty("PositionZ", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ushort _PositionZ;

		[WeaverGenerated]
		[DefaultForProperty("NetRotation", 3, 4)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Quaternion _NetRotation;

		[WeaverGenerated]
		[DefaultForProperty("ScaleX", 7, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ushort _ScaleX;

		[WeaverGenerated]
		[DefaultForProperty("ScaleY", 8, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ushort _ScaleY;

		[WeaverGenerated]
		[DefaultForProperty("ScaleZ", 9, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ushort _ScaleZ;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("StopSync", 10, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _StopSync;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe ushort PositionX
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.PositionX. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(ushort*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.PositionX. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(ushort*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe ushort PositionY
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.PositionY. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((ushort*)Ptr)[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.PositionY. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[2] = (short)value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe ushort PositionZ
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.PositionZ. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((ushort*)Ptr)[4];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.PositionZ. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[4] = (short)value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 4)]
		private unsafe Quaternion NetRotation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.NetRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Quaternion*)(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.NetRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Quaternion*)(Ptr + 3) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(7, 1)]
		private unsafe ushort ScaleX
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.ScaleX. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((ushort*)Ptr)[14];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.ScaleX. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[14] = (short)value;
			}
		}

		[Networked]
		[NetworkedWeaved(8, 1)]
		private unsafe ushort ScaleY
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.ScaleY. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((ushort*)Ptr)[16];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.ScaleY. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[16] = (short)value;
			}
		}

		[Networked]
		[NetworkedWeaved(9, 1)]
		private unsafe ushort ScaleZ
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.ScaleZ. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((ushort*)Ptr)[18];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.ScaleZ. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[18] = (short)value;
			}
		}

		[Networked]
		[NetworkedWeaved(10, 1)]
		public unsafe bool StopSync
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.StopSync. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 10);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FusionTransformView.StopSync. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 10) = new NetworkBool(value);
			}
		}

		private static float Float16ToFloat32(ushort value)
		{
			int num = (value & 0x8000) << 16;
			uint num2 = (uint)(value & 0x7C00) >> 10;
			uint num3 = (uint)(value & 0x3FF);
			return BitConverter.ToSingle(BitConverter.GetBytes((uint)(num | (num2 switch
			{
				0u => 0, 
				31u => 255, 
				_ => (int)(num2 - 15 + 127), 
			} << 23)) | (num3 << 13)), 0);
		}

		public override void FixedUpdateNetwork()
		{
			if (!StopSync)
			{
				SynchronizeTransform();
			}
		}

		public override void Render()
		{
			if (!StopSync)
			{
				ApplySynchronizedTransform();
			}
		}

		private void SynchronizeTransform()
		{
			if (base.HasStateAuthority)
			{
				SynchronizePosition();
				SynchronizeRotation();
				SynchronizeScale();
			}
		}

		private void SynchronizePosition()
		{
			Vector3 position = base.transform.position;
			if (_syncPositionX)
			{
				PositionX = Float32ToFloat16(position.x);
			}
			if (_syncPositionY)
			{
				PositionY = Float32ToFloat16(position.y);
			}
			if (_syncPositionZ)
			{
				PositionZ = Float32ToFloat16(position.z);
			}
		}

		private void SynchronizeRotation()
		{
			if (_syncRotation)
			{
				Quaternion rotation = base.transform.rotation;
				rotation.Normalize();
				NetRotation = rotation;
			}
		}

		private void SynchronizeScale()
		{
			Vector3 localScale = base.transform.localScale;
			if (_syncScaleX)
			{
				ScaleX = Float32ToFloat16(localScale.x);
			}
			if (_syncScaleY)
			{
				ScaleY = Float32ToFloat16(localScale.y);
			}
			if (_syncScaleZ)
			{
				ScaleZ = Float32ToFloat16(localScale.z);
			}
		}

		private void ApplySynchronizedTransform()
		{
			if (!base.HasStateAuthority)
			{
				ApplySynchronizedPosition();
				ApplySynchronizedRotation();
				ApplySynchronizedScale();
			}
		}

		private void ApplySynchronizedPosition()
		{
			Vector3 position = base.transform.position;
			if (_syncPositionX)
			{
				position.x = Float16ToFloat32(PositionX);
			}
			if (_syncPositionY)
			{
				position.y = Float16ToFloat32(PositionY);
			}
			if (_syncPositionZ)
			{
				position.z = Float16ToFloat32(PositionZ);
			}
			if (!(position.x > 100000f) && !(position.y > 100000f) && !(position.z > 100000f) && !(position.x < -100000f) && !(position.y < -100000f) && !(position.z < -100000f))
			{
				if (Vector3.Distance(base.transform.position, position) >= _maxLerpDistance)
				{
					base.transform.position = position;
				}
				else
				{
					base.transform.position = Vector3.Lerp(base.transform.position, position, base.Runner.DeltaTime * _interpolationSpeed);
				}
			}
		}

		private void ApplySynchronizedRotation()
		{
			if (_syncRotation)
			{
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, NetRotation, base.Runner.DeltaTime * _interpolationSpeed);
			}
		}

		private void ApplySynchronizedScale()
		{
			Vector3 localScale = base.transform.localScale;
			if (_syncScaleX)
			{
				localScale.x = Float16ToFloat32(ScaleX);
			}
			if (_syncScaleY)
			{
				localScale.y = Float16ToFloat32(ScaleY);
			}
			if (_syncScaleZ)
			{
				localScale.z = Float16ToFloat32(ScaleZ);
			}
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, localScale, base.Runner.DeltaTime * _interpolationSpeed);
		}

		private ushort Float32ToFloat16(float value)
		{
			uint num = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
			uint num2 = (num >> 16) & 0x8000;
			uint num3 = ((num >> 23) & 0xFF) - 112;
			uint num4 = num & 0x7FFFFF;
			switch (num3)
			{
			case 0u:
				return (ushort)num2;
			default:
				return (ushort)(num2 | 0x7C00);
			case 1u:
			case 2u:
			case 3u:
			case 4u:
			case 5u:
			case 6u:
			case 7u:
			case 8u:
			case 9u:
			case 10u:
			case 11u:
			case 12u:
			case 13u:
			case 14u:
			case 15u:
			case 16u:
			case 17u:
			case 18u:
			case 19u:
			case 20u:
			case 21u:
			case 22u:
			case 23u:
			case 24u:
			case 25u:
			case 26u:
			case 27u:
			case 28u:
			case 29u:
			case 30u:
				return (ushort)(num2 | (num3 << 10) | (num4 >> 13));
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			PositionX = _PositionX;
			PositionY = _PositionY;
			PositionZ = _PositionZ;
			NetRotation = _NetRotation;
			ScaleX = _ScaleX;
			ScaleY = _ScaleY;
			ScaleZ = _ScaleZ;
			StopSync = _StopSync;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_PositionX = PositionX;
			_PositionY = PositionY;
			_PositionZ = PositionZ;
			_NetRotation = NetRotation;
			_ScaleX = ScaleX;
			_ScaleY = ScaleY;
			_ScaleZ = ScaleZ;
			_StopSync = StopSync;
		}
	}
}
