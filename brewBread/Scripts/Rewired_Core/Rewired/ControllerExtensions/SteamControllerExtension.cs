using System.Collections.Generic;
using Rewired.Interfaces;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.ControllerExtensions
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class SteamControllerExtension : Controller.Extension
	{
		private class zEQRUMftUGefisLckIZUsbYcqoIh : IControllerExtensionSource
		{
			public readonly ISteamControllerInternal oGHFkUAdeULWiqGcBeeHfWxeyoJBb;

			public zEQRUMftUGefisLckIZUsbYcqoIh(ISteamControllerInternal P_0)
			{
				oGHFkUAdeULWiqGcBeeHfWxeyoJBb = P_0;
			}
		}

		private zEQRUMftUGefisLckIZUsbYcqoIh rmMLNGpqXVBSkkjZadentyrvdrgtA;

		private Joystick joystick => GetController<Joystick>();

		internal ISteamControllerInternal internalController => rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb;

		internal SteamControllerExtension(ISteamControllerInternal P_0)
			: base(new zEQRUMftUGefisLckIZUsbYcqoIh(P_0))
		{
			zQQfvDZMmpVqPPLYlLuSJXXpwJcI();
		}

		private SteamControllerExtension(SteamControllerExtension P_0)
			: base(P_0)
		{
			zQQfvDZMmpVqPPLYlLuSJXXpwJcI();
		}

		public ulong GetActionSetHandle(string actionSetName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0uL;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetActionSetHandle(ref actionSetName);
		}

		public ulong GetAnalogActionHandle(string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0uL;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetAnalogActionHandle(ref actionName);
		}

		public ulong GetDigitalActionHandle(string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0uL;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetDigitalActionHandle(ref actionName);
		}

		public string GetActionSetName(ulong actionSetHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return string.Empty;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetActionSetName(actionSetHandle);
		}

		public string GetAnalogActionName(ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return string.Empty;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetAnalogActionName(actionHandle);
		}

		public string GetDigitalActionName(ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return string.Empty;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetDigitalActionName(actionHandle);
		}

		public Vector2 GetAnalogActionValue(string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector2.zero;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetAnalogActionValue(ref actionName);
		}

		public Vector2 GetAnalogActionValue(ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return Vector2.zero;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetAnalogActionValue(actionHandle);
		}

		public bool GetDigitalActionValue(string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetDigitalActionValue(ref actionName);
		}

		public bool GetDigitalActionValue(ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetDigitalActionValue(actionHandle);
		}

		public bool SetActiveActionSet(ulong actionSetHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.SetActiveActionSet(actionSetHandle);
		}

		public bool SetActiveActionSet(string actionSetName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return false;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.SetActiveActionSet(ref actionSetName);
		}

		public ulong GetActiveActionSetHandle()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return 0uL;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetActiveActionSetHandle();
		}

		public string GetActiveActionSetName()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return string.Empty;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetActiveActionSetName();
		}

		public void ShowBindingPanel()
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else
			{
				rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.ShowBindingPanel();
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
				rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.SetHapticPulse(targePad, durationSeconds);
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
				rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.SetHapticPulse(targePad, durationMicroSeconds);
			}
		}

		public IList<SteamControllerActionOrigin> GetDigitalActionOrigins(string actionSetName, string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return EmptyObjects<SteamControllerActionOrigin>.EmptyReadOnlyIListT;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetDigitalActionOrigins(ref actionSetName, ref actionName);
		}

		public IList<SteamControllerActionOrigin> GetDigitalActionOrigins(ulong actionSetHandle, ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return EmptyObjects<SteamControllerActionOrigin>.EmptyReadOnlyIListT;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetDigitalActionOrigins(actionSetHandle, actionHandle);
		}

		public IList<SteamControllerActionOrigin> GetAnalogActionOrigins(string actionSetName, string actionName)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return EmptyObjects<SteamControllerActionOrigin>.EmptyReadOnlyIListT;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetAnalogActionOrigins(ref actionSetName, ref actionName);
		}

		public IList<SteamControllerActionOrigin> GetAnalogActionOrigins(ulong actionSetHandle, ulong actionHandle)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
				return EmptyObjects<SteamControllerActionOrigin>.EmptyReadOnlyIListT;
			}
			return rmMLNGpqXVBSkkjZadentyrvdrgtA.oGHFkUAdeULWiqGcBeeHfWxeyoJBb.GetAnalogActionOrigins(actionSetHandle, actionHandle);
		}

		internal override void UpdateData(UpdateLoopType updateLoop)
		{
		}

		internal override void SourceUpdated(IControllerExtensionSource source)
		{
			rmMLNGpqXVBSkkjZadentyrvdrgtA = source as zEQRUMftUGefisLckIZUsbYcqoIh;
		}

		internal override Controller.Extension Clone()
		{
			return new SteamControllerExtension(this);
		}

		private void zQQfvDZMmpVqPPLYlLuSJXXpwJcI()
		{
		}
	}
}
