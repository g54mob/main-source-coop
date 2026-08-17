using System;
using System.Collections.Generic;
using Rewired.Utils.Classes.Data;

namespace Rewired
{
	public sealed class ControllerTemplateActionAxisMap : ControllerTemplateActionElementMap
	{
		private AxisRange dVkWrKZuAyUNBIpiwSNbDGxBSfMD;

		private Pole QHdBEYYOxbFbzdPnNksOhvFWoDJO;

		private bool tkuffMyUioHdsTxyTcKVeiXSBuZcA;

		public AxisRange axisRange => dVkWrKZuAyUNBIpiwSNbDGxBSfMD;

		public Pole axisContribution => QHdBEYYOxbFbzdPnNksOhvFWoDJO;

		public bool invert => tkuffMyUioHdsTxyTcKVeiXSBuZcA;

		internal ControllerTemplateActionAxisMap(SerializedObject P_0)
			: base(ControllerTemplateElementType.Axis)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("serializedObject");
			}
			brPUUTgHWZJnOCkcunQbZcpMlIzm(P_0);
		}

		internal ControllerTemplateActionAxisMap(int P_0, AxisRange P_1, ActionElementMap P_2)
			: base(ControllerTemplateElementType.Axis, P_0, P_2)
		{
			dVkWrKZuAyUNBIpiwSNbDGxBSfMD = P_1;
			QHdBEYYOxbFbzdPnNksOhvFWoDJO = P_2.axisContribution;
			tkuffMyUioHdsTxyTcKVeiXSBuZcA = P_2._invert;
		}

		internal ControllerTemplateActionAxisMap(int P_0, int P_1, AxisRange P_2, Pole P_3, bool P_4, bool P_5)
			: base(ControllerTemplateElementType.Axis, P_0, P_1, P_5)
		{
			dVkWrKZuAyUNBIpiwSNbDGxBSfMD = P_2;
			QHdBEYYOxbFbzdPnNksOhvFWoDJO = P_3;
			tkuffMyUioHdsTxyTcKVeiXSBuZcA = P_4;
		}

		internal void jLgVqZbdLWdWdzwWqHyEHDQsswfS(SerializedObject P_0)
		{
			base.kKqfEeAXcHvhBUZDJUKIJUUHQJDj(P_0);
			P_0.Add("axisContribution", QHdBEYYOxbFbzdPnNksOhvFWoDJO);
			P_0.Add("axisRange", dVkWrKZuAyUNBIpiwSNbDGxBSfMD);
			P_0.Add("invert", tkuffMyUioHdsTxyTcKVeiXSBuZcA);
		}

		internal void jQqnyrHQGROZOsuDHLatSTLOrDxN(SerializedObject P_0)
		{
			base.brPUUTgHWZJnOCkcunQbZcpMlIzm(P_0);
			P_0.TryGetDeserializedValueByRef("axisContribution", ref QHdBEYYOxbFbzdPnNksOhvFWoDJO);
			P_0.TryGetDeserializedValueByRef("axisRange", ref dVkWrKZuAyUNBIpiwSNbDGxBSfMD);
			P_0.TryGetDeserializedValueByRef("invert", ref tkuffMyUioHdsTxyTcKVeiXSBuZcA);
		}

		internal void fQpmiEGHMzUbywrOaWqczVPPNDtr()
		{
			dVkWrKZuAyUNBIpiwSNbDGxBSfMD = AxisRange.Full;
			QHdBEYYOxbFbzdPnNksOhvFWoDJO = Pole.Positive;
			tkuffMyUioHdsTxyTcKVeiXSBuZcA = false;
		}

		internal int FDTNLPRubQRTSWUHmEiCqemzMRkQ(IControllerTemplateElementSource P_0, List<ActionElementMap> P_1, bool P_2)
		{
			if (!(P_0 is IControllerTemplateAxisSource controllerTemplateAxisSource))
			{
				return 0;
			}
			int num = 0;
			if (dVkWrKZuAyUNBIpiwSNbDGxBSfMD == AxisRange.Full)
			{
				if (controllerTemplateAxisSource.splitAxis)
				{
					ActionElementMap actionElementMap = KhQxMsVrYNBVcQajiCgfhHKHhaocb(controllerTemplateAxisSource.positiveTarget, (!tkuffMyUioHdsTxyTcKVeiXSBuZcA) ? AxisRange.Positive : AxisRange.Negative, QHdBEYYOxbFbzdPnNksOhvFWoDJO);
					if (actionElementMap != null)
					{
						P_1.Add(actionElementMap);
						num++;
					}
					actionElementMap = KhQxMsVrYNBVcQajiCgfhHKHhaocb(controllerTemplateAxisSource.negativeTarget, tkuffMyUioHdsTxyTcKVeiXSBuZcA ? AxisRange.Positive : AxisRange.Negative, QHdBEYYOxbFbzdPnNksOhvFWoDJO);
					if (actionElementMap != null)
					{
						P_1.Add(actionElementMap);
						num++;
					}
				}
				else
				{
					ActionElementMap actionElementMap = KhQxMsVrYNBVcQajiCgfhHKHhaocb(controllerTemplateAxisSource.fullTarget, AxisRange.Full, QHdBEYYOxbFbzdPnNksOhvFWoDJO);
					if (actionElementMap != null)
					{
						P_1.Add(actionElementMap);
						num++;
					}
				}
			}
			else if (controllerTemplateAxisSource.splitAxis)
			{
				if (dVkWrKZuAyUNBIpiwSNbDGxBSfMD == AxisRange.Positive)
				{
					ActionElementMap actionElementMap = PWNklTxYDBrNQjrBxQpoBnbbSVnP(controllerTemplateAxisSource.positiveTarget, Pole.Positive, QHdBEYYOxbFbzdPnNksOhvFWoDJO);
					if (actionElementMap != null)
					{
						P_1.Add(actionElementMap);
						num++;
					}
				}
				else
				{
					ActionElementMap actionElementMap = PWNklTxYDBrNQjrBxQpoBnbbSVnP(controllerTemplateAxisSource.negativeTarget, Pole.Negative, QHdBEYYOxbFbzdPnNksOhvFWoDJO);
					if (actionElementMap != null)
					{
						P_1.Add(actionElementMap);
						num++;
					}
				}
			}
			else
			{
				ActionElementMap actionElementMap = PWNklTxYDBrNQjrBxQpoBnbbSVnP(controllerTemplateAxisSource.fullTarget, (dVkWrKZuAyUNBIpiwSNbDGxBSfMD == AxisRange.Negative) ? Pole.Negative : Pole.Positive, QHdBEYYOxbFbzdPnNksOhvFWoDJO);
				if (actionElementMap != null)
				{
					P_1.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		private ActionElementMap KhQxMsVrYNBVcQajiCgfhHKHhaocb(IControllerElementTarget P_0, AxisRange P_1, Pole P_2)
		{
			if (P_0 == null || P_0.element == null)
			{
				return null;
			}
			ControllerElementType controllerElementType = P_0.elementType;
			AxisRange axisRange = P_0.axisRange;
			try
			{
				ControllerMap.RAmMePHwhbbjmrfLAYKtBaJPbccQ();
				ActionElementMap actionElementMap = new ActionElementMap();
				actionElementMap._elementIdentifierId = P_0.elementIdentifierId;
				actionElementMap._elementType = controllerElementType;
				actionElementMap._axisRange = axisRange;
				if (axisRange == AxisRange.Full)
				{
					switch (controllerElementType)
					{
					case ControllerElementType.Axis:
						actionElementMap._invert = tkuffMyUioHdsTxyTcKVeiXSBuZcA;
						break;
					case ControllerElementType.Button:
						actionElementMap._axisContribution = P_2;
						break;
					}
				}
				else if (controllerElementType == ControllerElementType.Axis || controllerElementType == ControllerElementType.Button)
				{
					Pole pole = ((P_1 == AxisRange.Negative) ? Pole.Negative : Pole.Positive);
					actionElementMap._axisContribution = pole;
				}
				return actionElementMap;
			}
			finally
			{
				ControllerMap.oeOZZgeXJicFbaxfdmvQlNMqgCjfA();
			}
		}

		private ActionElementMap PWNklTxYDBrNQjrBxQpoBnbbSVnP(IControllerElementTarget P_0, Pole P_1, Pole P_2)
		{
			if (P_0 == null || P_0.element == null)
			{
				return null;
			}
			ControllerElementType controllerElementType = P_0.elementType;
			AxisRange axisRange = P_0.axisRange;
			try
			{
				ControllerMap.RAmMePHwhbbjmrfLAYKtBaJPbccQ();
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
			finally
			{
				ControllerMap.oeOZZgeXJicFbaxfdmvQlNMqgCjfA();
			}
		}
	}
}
