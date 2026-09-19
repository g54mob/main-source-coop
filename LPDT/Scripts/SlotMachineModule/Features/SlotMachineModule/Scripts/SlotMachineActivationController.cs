using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.SlotMachineModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class SlotMachineActivationController : NetworkBehaviour
	{
		[SerializeField]
		private SlotMachineBehaviour _slotMachineBehaviour;

		[SerializeField]
		private SimplePointGrabable _leverGrabbable;

		[SerializeField]
		private SlotMachineInitializeController _slotMachineInitializeController;

		[SerializeField]
		private float _activationRotationThreshold = 60f;

		private Quaternion _initialLeverRotation;

		private bool _isArmed;

		private bool _isInitialized;

		public override void Spawned()
		{
			base.Spawned();
			if (_slotMachineInitializeController.IsSlotMachineInitialized)
			{
				InitializeSlotMachineActivation();
			}
			else
			{
				_slotMachineInitializeController.OnSlotMachineInitialized += InitializeSlotMachineActivation;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_slotMachineInitializeController.OnSlotMachineInitialized -= InitializeSlotMachineActivation;
		}

		private void InitializeSlotMachineActivation()
		{
			_slotMachineInitializeController.OnSlotMachineInitialized -= InitializeSlotMachineActivation;
			_initialLeverRotation = GetLeverRotationRelativeToMachine();
			_isArmed = false;
			_isInitialized = true;
		}

		public override void FixedUpdateNetwork()
		{
			if (_isInitialized && !(_slotMachineBehaviour == null) && (bool)_slotMachineBehaviour.Object && _slotMachineBehaviour.Object.IsValid && _slotMachineBehaviour.Object.HasStateAuthority && !_slotMachineBehaviour.IsSpinning && !_slotMachineBehaviour.IsPayingOut && (bool)_slotMachineBehaviour.IsSpinReady)
			{
				if (Quaternion.Angle(_initialLeverRotation, GetLeverRotationRelativeToMachine()) < _activationRotationThreshold)
				{
					_isArmed = true;
				}
				else if (_isArmed)
				{
					_isArmed = false;
					_slotMachineBehaviour.RequestSpin();
				}
			}
		}

		private Quaternion GetLeverRotationRelativeToMachine()
		{
			return Quaternion.Inverse(_slotMachineBehaviour.transform.rotation) * _leverGrabbable.Rigidbody.rotation;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
