using System;
using Fusion;
using UnityEngine;

namespace Features.CustomSynchronizersModule.Scripts
{
	[NetworkBehaviourWeaved(6)]
	public class QuantizedNetworkTransform : NetworkBehaviour
	{
		[Header("Settings")]
		[SerializeField]
		private float _positionPrecision = 0.01f;

		[SerializeField]
		private float _rotationPrecision = 0.5f;

		[SerializeField]
		private float _sendThreshold = 0.02f;

		[SerializeField]
		private float _rotationThreshold = 1f;

		[SerializeField]
		private float _lerpSpeed = 15f;

		[SerializeField]
		private bool _syncPosition = true;

		[SerializeField]
		private bool _syncRotation = true;

		private Vector3 _targetPosition;

		private Quaternion _targetRotation;

		private Vector3 _lastSentPosition;

		private Vector3 _lastSentEuler;

		private bool _isPositionInitialized;

		private bool _isRotationInitialized;

		[WeaverGenerated]
		[DefaultForProperty("_posQuantized", 0, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3Int __posQuantized;

		[WeaverGenerated]
		[DefaultForProperty("_rotQuantized", 3, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3Int __rotQuantized;

		[Networked]
		[NetworkedWeaved(0, 3)]
		private unsafe Vector3Int _posQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuantizedNetworkTransform._posQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3Int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuantizedNetworkTransform._posQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3Int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 3)]
		private unsafe Vector3Int _rotQuantized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuantizedNetworkTransform._rotQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3Int*)(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing QuantizedNetworkTransform._rotQuantized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3Int*)(Ptr + 3) = value;
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Object.HasStateAuthority)
			{
				UpdateAuthority();
			}
		}

		private void FixedUpdate()
		{
			if (base.Object != null && !base.Object.HasStateAuthority)
			{
				UpdateProxy();
			}
		}

		private void UpdateAuthority()
		{
			Vector3 position = base.transform.position;
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			if (_syncPosition && (!_isPositionInitialized || Vector3.Distance(position, _lastSentPosition) >= _sendThreshold))
			{
				_isPositionInitialized = true;
				_lastSentPosition = position;
				_posQuantized = QuantizeVector3(position, _positionPrecision);
			}
			if (_syncRotation && (!_isRotationInitialized || Quaternion.Angle(Quaternion.Euler(_lastSentEuler), base.transform.rotation) >= _rotationThreshold))
			{
				_isRotationInitialized = true;
				_lastSentEuler = eulerAngles;
				_rotQuantized = QuantizeVector3(eulerAngles, _rotationPrecision);
			}
		}

		private void UpdateProxy()
		{
			if (_syncPosition)
			{
				_targetPosition = DequantizeVector3(_posQuantized, _positionPrecision);
				base.transform.position = Vector3.Lerp(base.transform.position, _targetPosition, Time.fixedDeltaTime * _lerpSpeed);
			}
			if (_syncRotation)
			{
				_targetRotation = Quaternion.Euler(DequantizeVector3(_rotQuantized, _rotationPrecision));
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, _targetRotation, Time.fixedDeltaTime * _lerpSpeed);
			}
		}

		private Vector3Int QuantizeVector3(Vector3 value, float precision)
		{
			return new Vector3Int(Mathf.RoundToInt(value.x / precision), Mathf.RoundToInt(value.y / precision), Mathf.RoundToInt(value.z / precision));
		}

		private Vector3 DequantizeVector3(Vector3Int value, float precision)
		{
			return new Vector3((float)value.x * precision, (float)value.y * precision, (float)value.z * precision);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			_posQuantized = __posQuantized;
			_rotQuantized = __rotQuantized;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			__posQuantized = _posQuantized;
			__rotQuantized = _rotQuantized;
		}
	}
}
