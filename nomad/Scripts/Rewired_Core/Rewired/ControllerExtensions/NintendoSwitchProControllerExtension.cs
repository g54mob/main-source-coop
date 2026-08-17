using Rewired.HID.Drivers;
using Rewired.Interfaces;

namespace Rewired.ControllerExtensions
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	public sealed class NintendoSwitchProControllerExtension : NintendoSwitchGamepadExtension, IControllerVibrator, IHIDControllerExtension
	{
		private class RMrBVMaeJWbxePMxIbUidrbiEtoKB : ExtSource_Base
		{
			public IDriver_NintendoSwitchProController YqZwkwslsGCiBJtVhCzOWNZtAvugb => base.driver as IDriver_NintendoSwitchProController;

			public RMrBVMaeJWbxePMxIbUidrbiEtoKB(IDriver_NintendoSwitchProController P_0)
				: base(P_0)
			{
			}
		}

		public int motorIndexLeft;

		public int motorIndexRight = 1;

		private new RMrBVMaeJWbxePMxIbUidrbiEtoKB source => base.source as RMrBVMaeJWbxePMxIbUidrbiEtoKB;

		internal NintendoSwitchProControllerExtension(IDriver_NintendoSwitchProController P_0)
			: base(new RMrBVMaeJWbxePMxIbUidrbiEtoKB(P_0))
		{
		}

		private NintendoSwitchProControllerExtension(NintendoSwitchProControllerExtension P_0)
			: base(P_0)
		{
		}

		internal override Controller.Extension Clone()
		{
			return new NintendoSwitchProControllerExtension(this);
		}
	}
}
