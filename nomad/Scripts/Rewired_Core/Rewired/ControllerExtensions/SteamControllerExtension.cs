using System.Collections.Generic;
using Rewired.Interfaces;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal class SteamControllerExtension : Controller.Extension
	{
		private class HsgQoMBcxcqXjEfXUIASJsRWNcLp : IControllerExtensionSource
		{
			public readonly ISteamControllerInternal FpoPBKjkmeKGVxLBPMSAWibWWjuy;

			public HsgQoMBcxcqXjEfXUIASJsRWNcLp(ISteamControllerInternal P_0)
			{
				FpoPBKjkmeKGVxLBPMSAWibWWjuy = P_0;
			}
		}

		private HsgQoMBcxcqXjEfXUIASJsRWNcLp bDZIAqDAjYJiiEdQJeXpFXWhIugWA;

		private Joystick joystick => GetController<Joystick>();

		internal ISteamControllerInternal internalController => bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy;

		internal SteamControllerExtension(ISteamControllerInternal P_0)
			: base(new HsgQoMBcxcqXjEfXUIASJsRWNcLp(P_0))
		{
			WPbhNzDVXpbaaHhveuMCSlaMSRGV();
		}

		private SteamControllerExtension(SteamControllerExtension P_0)
			: base(P_0)
		{
			WPbhNzDVXpbaaHhveuMCSlaMSRGV();
		}

		public ulong GetActionSetHandle(string actionSetName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0uL;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetActionSetHandle(ref actionSetName);
		}

		public ulong GetAnalogActionHandle(string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0uL;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetAnalogActionHandle(ref actionName);
		}

		public ulong GetDigitalActionHandle(string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0uL;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetDigitalActionHandle(ref actionName);
		}

		public string GetActionSetName(ulong actionSetHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return string.Empty;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetActionSetName(actionSetHandle);
		}

		public string GetAnalogActionName(ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return string.Empty;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetAnalogActionName(actionHandle);
		}

		public string GetDigitalActionName(ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return string.Empty;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetDigitalActionName(actionHandle);
		}

		public Vector2 GetAnalogActionValue(string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector2.zero;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetAnalogActionValue(ref actionName);
		}

		public Vector2 GetAnalogActionValue(ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector2.zero;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetAnalogActionValue(actionHandle);
		}

		public bool GetDigitalActionValue(string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetDigitalActionValue(ref actionName);
		}

		public bool GetDigitalActionValue(ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetDigitalActionValue(actionHandle);
		}

		public bool SetActiveActionSet(ulong actionSetHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.SetActiveActionSet(actionSetHandle);
		}

		public bool SetActiveActionSet(string actionSetName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.SetActiveActionSet(ref actionSetName);
		}

		public ulong GetActiveActionSetHandle()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0uL;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetActiveActionSetHandle();
		}

		public string GetActiveActionSetName()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return string.Empty;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetActiveActionSetName();
		}

		public void ShowBindingPanel()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else
			{
				bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.ShowBindingPanel();
			}
		}

		public void SetHapticPulse(SteamControllerPadType targePad, float durationSeconds)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else
			{
				bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.SetHapticPulse(targePad, durationSeconds);
			}
		}

		public void SetHapticPulse(SteamControllerPadType targePad, ushort durationMicroSeconds)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else
			{
				bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.SetHapticPulse(targePad, durationMicroSeconds);
			}
		}

		public IList<SteamControllerActionOrigin> GetDigitalActionOrigins(string actionSetName, string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return EmptyObjects<SteamControllerActionOrigin>.EmptyReadOnlyIListT;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetDigitalActionOrigins(ref actionSetName, ref actionName);
		}

		public IList<SteamControllerActionOrigin> GetDigitalActionOrigins(ulong actionSetHandle, ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return EmptyObjects<SteamControllerActionOrigin>.EmptyReadOnlyIListT;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetDigitalActionOrigins(actionSetHandle, actionHandle);
		}

		public IList<SteamControllerActionOrigin> GetAnalogActionOrigins(string actionSetName, string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return EmptyObjects<SteamControllerActionOrigin>.EmptyReadOnlyIListT;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetAnalogActionOrigins(ref actionSetName, ref actionName);
		}

		public IList<SteamControllerActionOrigin> GetAnalogActionOrigins(ulong actionSetHandle, ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return EmptyObjects<SteamControllerActionOrigin>.EmptyReadOnlyIListT;
			}
			return bDZIAqDAjYJiiEdQJeXpFXWhIugWA.FpoPBKjkmeKGVxLBPMSAWibWWjuy.GetAnalogActionOrigins(actionSetHandle, actionHandle);
		}

		internal override void UpdateData(UpdateLoopType updateLoop)
		{
		}

		internal override void SourceUpdated(IControllerExtensionSource source)
		{
			bDZIAqDAjYJiiEdQJeXpFXWhIugWA = source as HsgQoMBcxcqXjEfXUIASJsRWNcLp;
		}

		internal override Controller.Extension Clone()
		{
			return new SteamControllerExtension(this);
		}

		private void WPbhNzDVXpbaaHhveuMCSlaMSRGV()
		{
		}
	}
}
