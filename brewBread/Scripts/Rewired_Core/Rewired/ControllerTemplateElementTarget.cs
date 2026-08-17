using System;

namespace Rewired
{
	public struct ControllerTemplateElementTarget
	{
		private IControllerTemplateElement DxtFUupqUGmyUsjBAudqHmDVBidJ;

		private AxisRange dnqcDaKuDzTeZpkjWrulGAfmNtHmA;

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

		public ControllerTemplateElementType elementType
		{
			get
			{
				if (DxtFUupqUGmyUsjBAudqHmDVBidJ == null)
				{
					return ControllerTemplateElementType.Axis;
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
				return DxtFUupqUGmyUsjBAudqHmDVBidJ.type switch
				{
					ControllerTemplateElementType.Axis => ((IControllerTemplateAxis)DxtFUupqUGmyUsjBAudqHmDVBidJ).GetDescriptiveName(dnqcDaKuDzTeZpkjWrulGAfmNtHmA), 
					ControllerTemplateElementType.Button => ((IControllerTemplateButton)DxtFUupqUGmyUsjBAudqHmDVBidJ).descriptiveName, 
					_ => DxtFUupqUGmyUsjBAudqHmDVBidJ.descriptiveName, 
				};
			}
		}

		public IControllerTemplateElement element
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

		public IControllerTemplate template
		{
			get
			{
				if (DxtFUupqUGmyUsjBAudqHmDVBidJ == null)
				{
					return null;
				}
				return (DxtFUupqUGmyUsjBAudqHmDVBidJ as IControllerTemplateElement_Internal).parent;
			}
		}

		public bool hasTarget => DxtFUupqUGmyUsjBAudqHmDVBidJ != null;

		internal ControllerTemplateElementTarget(IControllerTemplateElement P_0, AxisRange P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("element");
			}
			DxtFUupqUGmyUsjBAudqHmDVBidJ = P_0;
			dnqcDaKuDzTeZpkjWrulGAfmNtHmA = P_1;
		}

		public ControllerTemplateElementTarget(ControllerTemplateElementTarget P_0)
		{
			DxtFUupqUGmyUsjBAudqHmDVBidJ = P_0.DxtFUupqUGmyUsjBAudqHmDVBidJ;
			dnqcDaKuDzTeZpkjWrulGAfmNtHmA = P_0.dnqcDaKuDzTeZpkjWrulGAfmNtHmA;
		}
	}
}
