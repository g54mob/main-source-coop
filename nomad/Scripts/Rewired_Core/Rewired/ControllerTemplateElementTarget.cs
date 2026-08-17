using System;

namespace Rewired
{
	public struct ControllerTemplateElementTarget
	{
		private IControllerTemplateElement YysslmvaBKaropbsdClRKATXiyLJ;

		private AxisRange KwSgWRXcJNVGJHLeNPuckFBayTNk;

		public AxisRange axisRange
		{
			get
			{
				return KwSgWRXcJNVGJHLeNPuckFBayTNk;
			}
			set
			{
				KwSgWRXcJNVGJHLeNPuckFBayTNk = value;
			}
		}

		public ControllerTemplateElementType elementType
		{
			get
			{
				if (YysslmvaBKaropbsdClRKATXiyLJ == null)
				{
					return ControllerTemplateElementType.Axis;
				}
				return YysslmvaBKaropbsdClRKATXiyLJ.type;
			}
		}

		public string descriptiveName
		{
			get
			{
				if (YysslmvaBKaropbsdClRKATXiyLJ == null)
				{
					return string.Empty;
				}
				return YysslmvaBKaropbsdClRKATXiyLJ.type switch
				{
					ControllerTemplateElementType.Axis => ((IControllerTemplateAxis)YysslmvaBKaropbsdClRKATXiyLJ).GetDescriptiveName(KwSgWRXcJNVGJHLeNPuckFBayTNk), 
					ControllerTemplateElementType.Button => ((IControllerTemplateButton)YysslmvaBKaropbsdClRKATXiyLJ).descriptiveName, 
					_ => YysslmvaBKaropbsdClRKATXiyLJ.descriptiveName, 
				};
			}
		}

		public IControllerTemplateElement element
		{
			get
			{
				return YysslmvaBKaropbsdClRKATXiyLJ;
			}
			set
			{
				YysslmvaBKaropbsdClRKATXiyLJ = value;
			}
		}

		public IControllerTemplate template
		{
			get
			{
				if (YysslmvaBKaropbsdClRKATXiyLJ == null)
				{
					return null;
				}
				return (YysslmvaBKaropbsdClRKATXiyLJ as IControllerTemplateElement_Internal).parent;
			}
		}

		public bool hasTarget => YysslmvaBKaropbsdClRKATXiyLJ != null;

		internal ControllerTemplateElementTarget(IControllerTemplateElement P_0, AxisRange P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("element");
			}
			YysslmvaBKaropbsdClRKATXiyLJ = P_0;
			KwSgWRXcJNVGJHLeNPuckFBayTNk = P_1;
		}

		public ControllerTemplateElementTarget(ControllerTemplateElementTarget P_0)
		{
			YysslmvaBKaropbsdClRKATXiyLJ = P_0.YysslmvaBKaropbsdClRKATXiyLJ;
			KwSgWRXcJNVGJHLeNPuckFBayTNk = P_0.KwSgWRXcJNVGJHLeNPuckFBayTNk;
		}
	}
}
