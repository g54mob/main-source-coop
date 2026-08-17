using System.Runtime.CompilerServices;
using FishNet.Object;
using FishNet.Utility.Extension;
using UnityEngine;

namespace FishNet.Component.Prediction
{
	internal class PredictedObjectOwnerSmoother
	{
		private Transform _graphicalObject;

		private NetworkBehaviour _networkBehaviour;

		private float _teleportThreshold = 1f;

		private byte _interpolation = 1;

		private Vector3 _graphicalStartPosition;

		private Quaternion _graphicalStartRotation;

		private Vector3 _graphicalInstantiatedOffsetPosition;

		private float _positionMoveRate = -2f;

		private Quaternion _graphicalInstantiatedOffsetRotation;

		private float _rotationMoveRate = -2f;

		private bool _preTickReceived;

		private bool _smoothPosition;

		private bool _smoothRotation;

		public void SetGraphicalObject(Transform value)
		{
			_graphicalObject = value;
			_networkBehaviour.transform.SetTransformOffsets(value, ref _graphicalInstantiatedOffsetPosition, ref _graphicalInstantiatedOffsetRotation);
		}

		public void SetInterpolation(byte value)
		{
			_interpolation = value;
		}

		public void Initialize(NetworkBehaviour nb, Vector3 instantiatedOffsetPosition, Quaternion instantiatedOffsetRotation, Transform graphicalObject, bool smoothPosition, bool smoothRotation, byte interpolation, float teleportThreshold)
		{
			_networkBehaviour = nb;
			_graphicalInstantiatedOffsetPosition = instantiatedOffsetPosition;
			_graphicalInstantiatedOffsetRotation = instantiatedOffsetRotation;
			_graphicalObject = graphicalObject;
			_smoothPosition = smoothPosition;
			_smoothRotation = smoothRotation;
			_interpolation = interpolation;
			_teleportThreshold = teleportThreshold;
		}

		public void ManualUpdate()
		{
			MoveToTarget();
		}

		public void OnPreTick()
		{
			if (CanSmooth())
			{
				_preTickReceived = true;
				if (_interpolation == 1)
				{
					ResetGraphicalToInstantiatedProperties(position: true, rotation: true);
				}
				SetGraphicalPreviousProperties();
			}
		}

		public void OnPostTick()
		{
			if (CanSmooth() && _preTickReceived)
			{
				_preTickReceived = false;
				ResetGraphicalToPreviousProperties();
				SetGraphicalMoveRates();
			}
		}

		private bool CanSmooth()
		{
			if (_interpolation == 0)
			{
				return false;
			}
			if (!_networkBehaviour.IsOwner && !_networkBehaviour.IsHost)
			{
				return false;
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void MoveToTarget()
		{
			if (_positionMoveRate == -2f && _rotationMoveRate == -2f)
			{
				return;
			}
			Vector3 graphicalGoalPosition = GetGraphicalGoalPosition();
			Quaternion graphicalGoalRotation = GetGraphicalGoalRotation();
			Transform graphicalObject = _graphicalObject;
			float deltaTime = Time.deltaTime;
			if (SmoothPosition())
			{
				if (_positionMoveRate == -1f)
				{
					ResetGraphicalToInstantiatedProperties(position: true, rotation: false);
				}
				else if (_positionMoveRate > 0f)
				{
					graphicalObject.position = Vector3.MoveTowards(graphicalObject.position, graphicalGoalPosition, _positionMoveRate * deltaTime);
				}
			}
			if (SmoothRotation())
			{
				if (_rotationMoveRate == -1f)
				{
					ResetGraphicalToInstantiatedProperties(position: false, rotation: true);
				}
				else if (_rotationMoveRate > 0f)
				{
					graphicalObject.rotation = Quaternion.RotateTowards(graphicalObject.rotation, graphicalGoalRotation, _rotationMoveRate * deltaTime);
				}
			}
			if (GraphicalObjectMatches(graphicalGoalPosition, graphicalGoalRotation))
			{
				_positionMoveRate = -2f;
				_rotationMoveRate = -2f;
			}
		}

		private bool GraphicalObjectMatches(Vector3 position, Quaternion rotation)
		{
			bool num = !_smoothPosition || _graphicalObject.position == position;
			bool flag = !_smoothRotation || _graphicalObject.rotation == rotation;
			return num && flag;
		}

		private bool SmoothPosition()
		{
			if (_smoothPosition)
			{
				if (!_networkBehaviour.IsOwner)
				{
					return _networkBehaviour.IsHost;
				}
				return true;
			}
			return false;
		}

		private bool SmoothRotation()
		{
			if (_smoothRotation)
			{
				if (!_networkBehaviour.IsOwner)
				{
					return _networkBehaviour.IsHost;
				}
				return true;
			}
			return false;
		}

		private void SetGraphicalMoveRates()
		{
			float num = (float)_networkBehaviour.TimeManager.TickDelta * (float)(int)_interpolation;
			float num2 = Vector3.Distance(_graphicalObject.position, GetGraphicalGoalPosition());
			if (_teleportThreshold != -1f && num2 >= _teleportThreshold)
			{
				_positionMoveRate = -1f;
				_rotationMoveRate = -1f;
				return;
			}
			_positionMoveRate = num2 / num;
			num2 = Quaternion.Angle(_graphicalObject.rotation, GetGraphicalGoalRotation());
			if (num2 > 0f)
			{
				_rotationMoveRate = num2 / num;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Vector3 GetGraphicalGoalPosition()
		{
			if (SmoothPosition())
			{
				return _networkBehaviour.transform.position + _graphicalInstantiatedOffsetPosition;
			}
			return _graphicalObject.position;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Quaternion GetGraphicalGoalRotation()
		{
			if (SmoothRotation())
			{
				return _graphicalInstantiatedOffsetRotation * _networkBehaviour.transform.rotation;
			}
			return _graphicalObject.rotation;
		}

		private void SetGraphicalPreviousProperties()
		{
			_graphicalStartPosition = _graphicalObject.position;
			_graphicalStartRotation = _graphicalObject.rotation;
		}

		private void ResetGraphicalToPreviousProperties()
		{
			_graphicalObject.SetPositionAndRotation(_graphicalStartPosition, _graphicalStartRotation);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ResetGraphicalToInstantiatedProperties(bool position, bool rotation)
		{
			if (position)
			{
				_graphicalObject.position = GetGraphicalGoalPosition();
			}
			if (rotation)
			{
				_graphicalObject.rotation = GetGraphicalGoalRotation();
			}
		}
	}
}
