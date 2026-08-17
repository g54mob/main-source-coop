using System;

namespace Rewired
{
	public struct ControllerIdentifier
	{
		private int usaXwVIQhBMHnPurTBZmObSRPPSx;

		private ControllerType npWCvFYEdEwRovPxqSFLnKQqETtC;

		private Guid HOlIzJYjREJtUjiQxTiQKiJXIOgv;

		private string AjCbmudgoeYyrhzehrPcUTPdollLA;

		private Guid KfyBmYLPpABcAIlWFYrMGpTfbTgdA;

		public int controllerId
		{
			get
			{
				return usaXwVIQhBMHnPurTBZmObSRPPSx;
			}
			set
			{
				usaXwVIQhBMHnPurTBZmObSRPPSx = value;
			}
		}

		public ControllerType controllerType
		{
			get
			{
				return npWCvFYEdEwRovPxqSFLnKQqETtC;
			}
			set
			{
				npWCvFYEdEwRovPxqSFLnKQqETtC = value;
			}
		}

		public Guid hardwareTypeGuid
		{
			get
			{
				return HOlIzJYjREJtUjiQxTiQKiJXIOgv;
			}
			set
			{
				HOlIzJYjREJtUjiQxTiQKiJXIOgv = value;
			}
		}

		public string hardwareIdentifier
		{
			get
			{
				return AjCbmudgoeYyrhzehrPcUTPdollLA;
			}
			set
			{
				AjCbmudgoeYyrhzehrPcUTPdollLA = value;
			}
		}

		public Guid deviceInstanceGuid
		{
			get
			{
				return KfyBmYLPpABcAIlWFYrMGpTfbTgdA;
			}
			set
			{
				KfyBmYLPpABcAIlWFYrMGpTfbTgdA = value;
			}
		}

		public static ControllerIdentifier Blank => new ControllerIdentifier
		{
			usaXwVIQhBMHnPurTBZmObSRPPSx = -1
		};

		internal ControllerIdentifier(Controller P_0)
		{
			usaXwVIQhBMHnPurTBZmObSRPPSx = P_0.id;
			npWCvFYEdEwRovPxqSFLnKQqETtC = P_0.type;
			HOlIzJYjREJtUjiQxTiQKiJXIOgv = P_0.zyYehdPaDXciYCtKVPxEsznJTyqP;
			AjCbmudgoeYyrhzehrPcUTPdollLA = P_0.hardwareIdentifier;
			KfyBmYLPpABcAIlWFYrMGpTfbTgdA = P_0.deviceInstanceGuid;
		}
	}
}
