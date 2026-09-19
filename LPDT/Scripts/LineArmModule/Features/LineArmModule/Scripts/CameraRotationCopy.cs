using System;
using Features.CameraModelModule;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LineArmModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class CameraRotationCopy : NetworkBehaviour
	{
		[Header("References")]
		public Transform target;

		[SerializeField]
		public bool _ignoreYRotation;

		[SerializeField]
		private float _armEndMoveSpeed = 5f;

		private PlayerMovableModel _playerMovableModel;

		private CameraModel _cameraModel;

		private bool _isInitialized;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("DisableRotation", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _DisableRotation;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe bool DisableRotation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CameraRotationCopy.DisableRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CameraRotationCopy.DisableRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public float RotationSpeed
		{
			get
			{
				return _armEndMoveSpeed;
			}
			set
			{
				_armEndMoveSpeed = value;
			}
		}

		[Inject]
		private void InjectDependencies(CameraModel cameraModel, PlayerMovableModel playerMovableModel)
		{
			_cameraModel = cameraModel;
			_playerMovableModel = playerMovableModel;
		}

		public override void Spawned()
		{
			_isInitialized = true;
		}

		private void LateUpdate()
		{
			if (!_isInitialized || DisableRotation || _playerMovableModel.RotationMode != PlayerRotationMode.HeadAndBodyRotation)
			{
				return;
			}
			if (base.Object.InputAuthority != base.Runner.LocalPlayer)
			{
				if (_playerMovableModel.AllCharacterMovables.TryGetValue(base.Object.InputAuthority, out var value))
				{
					Vector3 forward = value.RotatoblePart.forward;
					if (_ignoreYRotation)
					{
						forward.y = 0f;
						forward.Normalize();
					}
					target.forward = Vector3.Lerp(target.forward, forward, Time.deltaTime * _armEndMoveSpeed);
				}
			}
			else if (!(_cameraModel.CameraObject == null) && !(target == null))
			{
				Vector3 forward2 = _cameraModel.AimTransform.forward;
				if (_ignoreYRotation)
				{
					forward2.y = 0f;
					forward2.Normalize();
				}
				target.forward = Vector3.Lerp(target.forward, forward2, Time.deltaTime * _armEndMoveSpeed);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			DisableRotation = _DisableRotation;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_DisableRotation = DisableRotation;
		}
	}
}
