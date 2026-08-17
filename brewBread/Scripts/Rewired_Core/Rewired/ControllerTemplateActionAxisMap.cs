using System;
using System.Collections.Generic;
using Rewired.Utils.Classes.Data;

namespace Rewired
{
	public sealed class ControllerTemplateActionAxisMap : ControllerTemplateActionElementMap
	{
		private AxisRange dnqcDaKuDzTeZpkjWrulGAfmNtHmA;

		private Pole ysANvLtALhiLvQPHMrzVONBSGlf;

		private bool GYjNfJzKOfaDDnMNUsNgyurjLVIT;

		public AxisRange axisRange => dnqcDaKuDzTeZpkjWrulGAfmNtHmA;

		public Pole axisContribution => ysANvLtALhiLvQPHMrzVONBSGlf;

		public bool invert => GYjNfJzKOfaDDnMNUsNgyurjLVIT;

		internal ControllerTemplateActionAxisMap(SerializedObject P_0)
			: base(ControllerTemplateElementType.Axis)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("serializedObject");
			}
			ckPgJNiFQjrLdKfwClUsdsySXFyIb(P_0);
		}

		internal ControllerTemplateActionAxisMap(int P_0, AxisRange P_1, ActionElementMap P_2)
			: base(ControllerTemplateElementType.Axis, P_0, P_2)
		{
			dnqcDaKuDzTeZpkjWrulGAfmNtHmA = P_1;
			ysANvLtALhiLvQPHMrzVONBSGlf = P_2.axisContribution;
			GYjNfJzKOfaDDnMNUsNgyurjLVIT = P_2._invert;
		}

		internal ControllerTemplateActionAxisMap(int P_0, int P_1, AxisRange P_2, Pole P_3, bool P_4, bool P_5)
			: base(ControllerTemplateElementType.Axis, P_0, P_1, P_5)
		{
			dnqcDaKuDzTeZpkjWrulGAfmNtHmA = P_2;
			ysANvLtALhiLvQPHMrzVONBSGlf = P_3;
			GYjNfJzKOfaDDnMNUsNgyurjLVIT = P_4;
		}

		internal override void JxowpgGeLUBIhbyiSIIfFkkGllDuA(SerializedObject P_0)
		{
			base.JxowpgGeLUBIhbyiSIIfFkkGllDuA(P_0);
			P_0.Add("axisContribution", ysANvLtALhiLvQPHMrzVONBSGlf);
			P_0.Add("axisRange", dnqcDaKuDzTeZpkjWrulGAfmNtHmA);
			P_0.Add("invert", GYjNfJzKOfaDDnMNUsNgyurjLVIT);
		}

		internal override void ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject P_0)
		{
			base.ckPgJNiFQjrLdKfwClUsdsySXFyIb(P_0);
			P_0.TryGetDeserializedValueByRef("axisContribution", ref ysANvLtALhiLvQPHMrzVONBSGlf);
			P_0.TryGetDeserializedValueByRef("axisRange", ref dnqcDaKuDzTeZpkjWrulGAfmNtHmA);
			P_0.TryGetDeserializedValueByRef("invert", ref GYjNfJzKOfaDDnMNUsNgyurjLVIT);
		}

		internal override void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			dnqcDaKuDzTeZpkjWrulGAfmNtHmA = AxisRange.Full;
			ysANvLtALhiLvQPHMrzVONBSGlf = Pole.Positive;
			GYjNfJzKOfaDDnMNUsNgyurjLVIT = false;
		}

		internal override int kYHECBYfnrBTBCoSAFHALwaMjook(IControllerTemplateElementSource P_0, List<ActionElementMap> P_1, bool P_2)
		{
			if (!(P_0 is IControllerTemplateAxisSource controllerTemplateAxisSource))
			{
				return 0;
			}
			int num = 0;
			if (dnqcDaKuDzTeZpkjWrulGAfmNtHmA == AxisRange.Full)
			{
				if (controllerTemplateAxisSource.splitAxis)
				{
					ActionElementMap actionElementMap = XKfnTwUxMqrNRcKZcAsOtKWYkvgs(controllerTemplateAxisSource.positiveTarget, (!GYjNfJzKOfaDDnMNUsNgyurjLVIT) ? AxisRange.Positive : AxisRange.Negative);
					if (actionElementMap != null)
					{
						P_1.Add(actionElementMap);
						num++;
					}
					actionElementMap = XKfnTwUxMqrNRcKZcAsOtKWYkvgs(controllerTemplateAxisSource.negativeTarget, GYjNfJzKOfaDDnMNUsNgyurjLVIT ? AxisRange.Positive : AxisRange.Negative);
					if (actionElementMap != null)
					{
						P_1.Add(actionElementMap);
						num++;
					}
				}
				else
				{
					ActionElementMap actionElementMap = XKfnTwUxMqrNRcKZcAsOtKWYkvgs(controllerTemplateAxisSource.fullTarget, AxisRange.Full);
					if (actionElementMap != null)
					{
						P_1.Add(actionElementMap);
						num++;
					}
				}
			}
			else if (controllerTemplateAxisSource.splitAxis)
			{
				if (dnqcDaKuDzTeZpkjWrulGAfmNtHmA == AxisRange.Positive)
				{
					ActionElementMap actionElementMap = ctPZvrcCarVHYPSvKJOhegYxXMvd(controllerTemplateAxisSource.positiveTarget, Pole.Positive, ysANvLtALhiLvQPHMrzVONBSGlf);
					if (actionElementMap != null)
					{
						P_1.Add(actionElementMap);
						num++;
					}
				}
				else
				{
					ActionElementMap actionElementMap = ctPZvrcCarVHYPSvKJOhegYxXMvd(controllerTemplateAxisSource.negativeTarget, Pole.Negative, ysANvLtALhiLvQPHMrzVONBSGlf);
					if (actionElementMap != null)
					{
						P_1.Add(actionElementMap);
						num++;
					}
				}
			}
			else
			{
				ActionElementMap actionElementMap = ctPZvrcCarVHYPSvKJOhegYxXMvd(controllerTemplateAxisSource.fullTarget, (dnqcDaKuDzTeZpkjWrulGAfmNtHmA == AxisRange.Negative) ? Pole.Negative : Pole.Positive, ysANvLtALhiLvQPHMrzVONBSGlf);
				if (actionElementMap != null)
				{
					P_1.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		private ActionElementMap XKfnTwUxMqrNRcKZcAsOtKWYkvgs(IControllerElementTarget P_0, AxisRange P_1)
		{
			if (P_0 == null || P_0.element == null)
			{
				return null;
			}
			ControllerElementType controllerElementType = P_0.elementType;
			AxisRange axisRange = P_0.axisRange;
			ActionElementMap actionElementMap = new ActionElementMap();
			actionElementMap._elementIdentifierId = P_0.elementIdentifierId;
			actionElementMap._elementType = controllerElementType;
			actionElementMap._axisRange = axisRange;
			if (controllerElementType == ControllerElementType.Axis && axisRange == AxisRange.Full)
			{
				actionElementMap._invert = GYjNfJzKOfaDDnMNUsNgyurjLVIT;
			}
			else if (controllerElementType == ControllerElementType.Axis || controllerElementType == ControllerElementType.Button)
			{
				Pole pole = ((P_1 == AxisRange.Negative) ? Pole.Negative : Pole.Positive);
				actionElementMap._axisContribution = pole;
			}
			return actionElementMap;
		}

		private ActionElementMap ctPZvrcCarVHYPSvKJOhegYxXMvd(IControllerElementTarget P_0, Pole P_1, Pole P_2)
		{
			if (P_0 == null || P_0.element == null)
			{
				return null;
			}
			ControllerElementType controllerElementType = P_0.elementType;
			AxisRange axisRange = P_0.axisRange;
			ActionElementMap actionElementMap = new ActionElementMap();
			actionElementMap._elementIdentifierId = P_0.elementIdentifierId;
			actionElementMap._elementType = controllerElementType;
			actionElementMap._axisRange = axisRange;
			if (controllerElementType == ControllerElementType.Axis && axisRange == AxisRange.Full)
			{
				actionElementMap._axisRange = ((P_1 == Pole.Positive) ? AxisRange.Positive : AxisRange.Negative);
				actionElementMap._axisContribution = P_2;
			}
			else if (controllerElementType == ControllerElementType.Axis || controllerElementType == ControllerElementType.Button)
			{
				actionElementMap._axisContribution = P_2;
			}
			return actionElementMap;
		}
	}
}
