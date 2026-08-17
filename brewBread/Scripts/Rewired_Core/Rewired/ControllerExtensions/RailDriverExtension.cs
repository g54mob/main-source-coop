using Rewired.Drivers.Interfaces;
using Rewired.Interfaces;

namespace Rewired.ControllerExtensions
{
	public sealed class RailDriverExtension : Controller.Extension
	{
		private class zCpUUtBgVgeEECUIEuENGJyICVsCb : IControllerExtensionSource
		{
			public readonly IDriver_RailDriver UVnyPZOPqXebrkDwGURsMwQhOdVT;

			public zCpUUtBgVgeEECUIEuENGJyICVsCb(IDriver_RailDriver P_0)
			{
				UVnyPZOPqXebrkDwGURsMwQhOdVT = P_0;
			}
		}

		private zCpUUtBgVgeEECUIEuENGJyICVsCb rmMLNGpqXVBSkkjZadentyrvdrgtA;

		private Joystick chcOLBMXAtWUArFXncgBETOGbAnFb => GetController<Joystick>();

		public bool speakerEnabled
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return false;
				}
				if (rmMLNGpqXVBSkkjZadentyrvdrgtA.UVnyPZOPqXebrkDwGURsMwQhOdVT == null)
				{
					return false;
				}
				return rmMLNGpqXVBSkkjZadentyrvdrgtA.UVnyPZOPqXebrkDwGURsMwQhOdVT.SpeakerEnabled;
			}
			set
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
				}
				else if (rmMLNGpqXVBSkkjZadentyrvdrgtA.UVnyPZOPqXebrkDwGURsMwQhOdVT != null)
				{
					rmMLNGpqXVBSkkjZadentyrvdrgtA.UVnyPZOPqXebrkDwGURsMwQhOdVT.SpeakerEnabled = value;
				}
			}
		}

		internal RailDriverExtension(IDriver_RailDriver P_0)
			: base(new zCpUUtBgVgeEECUIEuENGJyICVsCb(P_0))
		{
		}

		private RailDriverExtension(RailDriverExtension P_0)
			: base(P_0)
		{
		}

		public void SetLEDDisplay(int digitIndex, byte digitBitValues)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (rmMLNGpqXVBSkkjZadentyrvdrgtA.UVnyPZOPqXebrkDwGURsMwQhOdVT != null && base.enabled)
			{
				rmMLNGpqXVBSkkjZadentyrvdrgtA.UVnyPZOPqXebrkDwGURsMwQhOdVT.SetLEDDisplay(digitIndex, digitBitValues);
			}
		}

		public void SetLEDDisplay(byte digit1BitValues, byte digit2BitValues, byte digit3BitValues)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (rmMLNGpqXVBSkkjZadentyrvdrgtA.UVnyPZOPqXebrkDwGURsMwQhOdVT != null && base.enabled)
			{
				rmMLNGpqXVBSkkjZadentyrvdrgtA.UVnyPZOPqXebrkDwGURsMwQhOdVT.SetLEDDisplay(digit1BitValues, digit2BitValues, digit3BitValues);
			}
		}

		internal void PwSVyxiNOmkyuuXUHWyuymUyabog(UpdateLoopType P_0)
		{
		}

		internal void EJfWkXMMYTRfEHNEnCEaDzBnFICoA(IControllerExtensionSource P_0)
		{
			rmMLNGpqXVBSkkjZadentyrvdrgtA = P_0 as zCpUUtBgVgeEECUIEuENGJyICVsCb;
		}

		internal Controller.Extension vpHGhDFXfrFPnKJBKRFCgbudEJvBB()
		{
			return new RailDriverExtension(this);
		}
	}
}
