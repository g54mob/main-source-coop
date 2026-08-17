using System;

namespace Rewired
{
	public struct ControllerElementTarget
	{
		private Controller.Element DxtFUupqUGmyUsjBAudqHmDVBidJ;

		private AxisRange dnqcDaKuDzTeZpkjWrulGAfmNtHmA;

		public int elementIdentifierId
		{
			get
			{
				if (DxtFUupqUGmyUsjBAudqHmDVBidJ == null)
				{
					return -1;
				}
				return DxtFUupqUGmyUsjBAudqHmDVBidJ.id;
			}
		}

		public AxisRange axisRange
		{
			get
			{
				return dnqcDaKuDzTeZpkjWrulGAfmNtHmA;
			}
			set
			{
				dnqcDaKuDzTeZpkjWrulGAfmNtHmA = value;
			}
		}

		public bool hasTarget => DxtFUupqUGmyUsjBAudqHmDVBidJ != null;

		public ControllerElementType elementType
		{
			get
			{
				if (DxtFUupqUGmyUsjBAudqHmDVBidJ == null)
				{
					return ControllerElementType.Axis;
				}
				return DxtFUupqUGmyUsjBAudqHmDVBidJ.type;
			}
		}

		public string descriptiveName
		{
			get
			{
				if (DxtFUupqUGmyUsjBAudqHmDVBidJ == null)
				{
					return string.Empty;
				}
				ControllerElementIdentifier elementIdentifier = DxtFUupqUGmyUsjBAudqHmDVBidJ.elementIdentifier;
				if (elementIdentifier == null)
				{
					return string.Empty;
				}
				return elementIdentifier.GetDisplayName(DxtFUupqUGmyUsjBAudqHmDVBidJ.type, dnqcDaKuDzTeZpkjWrulGAfmNtHmA);
			}
		}

		public Controller controller
		{
			get
			{
				if (DxtFUupqUGmyUsjBAudqHmDVBidJ == null)
				{
					return null;
				}
				return DxtFUupqUGmyUsjBAudqHmDVBidJ.oBPMdftKtJLWDwKjOKgLuimxBfUKA;
			}
		}

		public Controller.Element element
		{
			get
			{
				return DxtFUupqUGmyUsjBAudqHmDVBidJ;
			}
			set
			{
				DxtFUupqUGmyUsjBAudqHmDVBidJ = value;
			}
		}

		public ControllerElementTarget(ActionElementMap P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("actionElementMap");
			}
			if (P_0.mtCzlvnEPTtCfVpotNRemSaRaiHy != null)
			{
				Controller controller = ReInput.VmqcbbbvPImXBEBcMHWjUAXVfrUSA.IzoAZhLSJPdQcJxvWLgBrvgaJNMKA(P_0.mtCzlvnEPTtCfVpotNRemSaRaiHy.controllerType, P_0.mtCzlvnEPTtCfVpotNRemSaRaiHy.controllerId);
				DxtFUupqUGmyUsjBAudqHmDVBidJ = controller.GetElementById(P_0._elementIdentifierId);
			}
			else
			{
				DxtFUupqUGmyUsjBAudqHmDVBidJ = null;
			}
			dnqcDaKuDzTeZpkjWrulGAfmNtHmA = P_0._axisRange;
		}

		public ControllerElementTarget(ControllerElementTarget P_0)
		{
			DxtFUupqUGmyUsjBAudqHmDVBidJ = P_0.DxtFUupqUGmyUsjBAudqHmDVBidJ;
			dnqcDaKuDzTeZpkjWrulGAfmNtHmA = P_0.dnqcDaKuDzTeZpkjWrulGAfmNtHmA;
		}

		public ControllerElementTarget(IControllerElementTarget P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("other");
			}
			DxtFUupqUGmyUsjBAudqHmDVBidJ = P_0.element;
			dnqcDaKuDzTeZpkjWrulGAfmNtHmA = P_0.axisRange;
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
