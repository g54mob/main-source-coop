using System;

namespace Rewired
{
	public struct ControllerElementTarget
	{
		private Controller.Element JcmDRAQpJIvGKOukAFGDAdtLkYDo;

		private AxisRange LdwbpRKvNUERXlyjLYTiJWgXIPFo;

		public int elementIdentifierId
		{
			get
			{
				if (JcmDRAQpJIvGKOukAFGDAdtLkYDo == null)
				{
					return -1;
				}
				return JcmDRAQpJIvGKOukAFGDAdtLkYDo.id;
			}
		}

		public AxisRange axisRange
		{
			get
			{
				return LdwbpRKvNUERXlyjLYTiJWgXIPFo;
			}
			set
			{
				LdwbpRKvNUERXlyjLYTiJWgXIPFo = value;
			}
		}

		public bool hasTarget => JcmDRAQpJIvGKOukAFGDAdtLkYDo != null;

		public ControllerElementType elementType
		{
			get
			{
				if (JcmDRAQpJIvGKOukAFGDAdtLkYDo == null)
				{
					return ControllerElementType.Axis;
				}
				return JcmDRAQpJIvGKOukAFGDAdtLkYDo.type;
			}
		}

		public string descriptiveName
		{
			get
			{
				if (JcmDRAQpJIvGKOukAFGDAdtLkYDo == null)
				{
					return string.Empty;
				}
				ControllerElementIdentifier elementIdentifier = JcmDRAQpJIvGKOukAFGDAdtLkYDo.elementIdentifier;
				if (elementIdentifier == null)
				{
					return string.Empty;
				}
				return elementIdentifier.GetDisplayName(JcmDRAQpJIvGKOukAFGDAdtLkYDo.type, LdwbpRKvNUERXlyjLYTiJWgXIPFo);
			}
		}

		public Controller controller
		{
			get
			{
				if (JcmDRAQpJIvGKOukAFGDAdtLkYDo == null)
				{
					return null;
				}
				return JcmDRAQpJIvGKOukAFGDAdtLkYDo.TrpRuvlQuUTvlONxcFCzTxhTsSlf;
			}
		}

		public Controller.Element element
		{
			get
			{
				return JcmDRAQpJIvGKOukAFGDAdtLkYDo;
			}
			set
			{
				JcmDRAQpJIvGKOukAFGDAdtLkYDo = value;
			}
		}

		public ControllerElementTarget(ActionElementMap P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("actionElementMap");
			}
			if (P_0.LPxrnOpGfQRCZcQMNZvgstOaWZoi != null)
			{
				Controller controller = ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn.QaXmawDzDviOFGGVAiudAlfjSkMM(P_0.LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerType, P_0.LPxrnOpGfQRCZcQMNZvgstOaWZoi.controllerId);
				JcmDRAQpJIvGKOukAFGDAdtLkYDo = controller.GetElementById(P_0._elementIdentifierId);
			}
			else
			{
				JcmDRAQpJIvGKOukAFGDAdtLkYDo = null;
			}
			LdwbpRKvNUERXlyjLYTiJWgXIPFo = P_0._axisRange;
		}

		public ControllerElementTarget(ControllerElementTarget P_0)
		{
			JcmDRAQpJIvGKOukAFGDAdtLkYDo = P_0.JcmDRAQpJIvGKOukAFGDAdtLkYDo;
			LdwbpRKvNUERXlyjLYTiJWgXIPFo = P_0.LdwbpRKvNUERXlyjLYTiJWgXIPFo;
		}

		public ControllerElementTarget(IControllerElementTarget P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("other");
			}
			JcmDRAQpJIvGKOukAFGDAdtLkYDo = P_0.element;
			LdwbpRKvNUERXlyjLYTiJWgXIPFo = P_0.axisRange;
		}

		public static implicit operator ControllerElementTarget(ActionElementMap actionElementMap)
		{
			if (actionElementMap == null)
			{
				return default(ControllerElementTarget);
			}
			return new ControllerElementTarget(actionElementMap);
		}
	}
}
