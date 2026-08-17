using EvilCore;
using NomadDrive.Features.Interaction;
using UnityEngine.Events;

namespace NomadDrive.Features.Furnitures
{
	public class Sink : Interactable, IInitialize
	{
		private BasicInteraction _basicInteraction;

		private bool _isHandleOn;

		public UnityEvent OnSinkOn { get; } = new UnityEvent();

		public UnityEvent OnSinkOff { get; } = new UnityEvent();

		public bool IsOn { get; set; }

		public bool IsInitialized { get; set; }

		public void Init()
		{
			_basicInteraction = GetComponent<BasicInteraction>();
			IsOn = false;
			IsInitialized = true;
		}

		public void CheckState()
		{
			if (_isHandleOn)
			{
				On();
			}
			else
			{
				Off();
			}
		}

		public void OnHandle()
		{
			_isHandleOn = true;
			CheckState();
			SetBasicInteraction(OffHandle, "@interaction.off");
		}

		private void OffHandle()
		{
			_isHandleOn = false;
			CheckState();
			SetBasicInteraction(OnHandle, "@interaction.on");
		}

		private void On()
		{
			if (!IsOn)
			{
				OnSinkOn.Invoke();
			}
		}

		public void Off()
		{
			if (IsOn)
			{
				IsOn = false;
				OnSinkOff.Invoke();
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
