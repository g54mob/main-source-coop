using System;
using Features.HingeModule.Scripts;
using UnityEngine;

namespace Features.SlotMachineModule.Scripts
{
	public class SlotMachineInitializeController : MonoBehaviour
	{
		[SerializeField]
		private HingeSetupHandler _hingeSetupHandler;

		private bool _isSlotMachineInitialized;

		public bool IsSlotMachineInitialized
		{
			get
			{
				return _isSlotMachineInitialized;
			}
			private set
			{
				_isSlotMachineInitialized = value;
				if (_isSlotMachineInitialized)
				{
					this.OnSlotMachineInitialized?.Invoke();
				}
			}
		}

		public event Action OnSlotMachineInitialized;

		private void OnEnable()
		{
			if (_hingeSetupHandler != null && _hingeSetupHandler.IsJointInitialized)
			{
				IsSlotMachineInitialized = true;
			}
			else if (_hingeSetupHandler != null)
			{
				_hingeSetupHandler.OnJointInitialized += OnJointInitialized;
			}
		}

		private void OnDisable()
		{
			if (_hingeSetupHandler != null)
			{
				_hingeSetupHandler.OnJointInitialized -= OnJointInitialized;
			}
		}

		private void OnJointInitialized()
		{
			IsSlotMachineInitialized = true;
		}
	}
}
