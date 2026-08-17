using System;
using System.Collections.Generic;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired
{
	public abstract class ControllerTemplateActionElementMap
	{
		private readonly int YVGDeWQAhUWKAOSPoxXuizkXaiTI;

		private readonly ControllerTemplateElementType RUyyQSEmqRidLNphokXShBrzehNy;

		private bool kKFZZElqKQSUFMZnWdEudRvTJGpo;

		private int JwOchbdvhsvHVdSyYjtqEJdTZzYG;

		private int NPuaUGjYHfmjnFZazhpMoRbThWzG;

		private static int xhbaffnTlrHEJlYqvJfRvkDhUTlO;

		public int id => YVGDeWQAhUWKAOSPoxXuizkXaiTI;

		public ControllerTemplateElementType elementType => RUyyQSEmqRidLNphokXShBrzehNy;

		public bool enabled
		{
			get
			{
				return kKFZZElqKQSUFMZnWdEudRvTJGpo;
			}
			set
			{
				kKFZZElqKQSUFMZnWdEudRvTJGpo = value;
			}
		}

		public int actionId
		{
			get
			{
				return JwOchbdvhsvHVdSyYjtqEJdTZzYG;
			}
			set
			{
				JwOchbdvhsvHVdSyYjtqEJdTZzYG = value;
			}
		}

		public int elementIdentifierId
		{
			get
			{
				return NPuaUGjYHfmjnFZazhpMoRbThWzG;
			}
			set
			{
				NPuaUGjYHfmjnFZazhpMoRbThWzG = value;
			}
		}

		internal ControllerTemplateActionElementMap(ControllerTemplateElementType P_0)
		{
			if (!InputTools.IsMappableType(P_0))
			{
				throw new ArgumentException(P_0.ToString() + " is not a supported mappable Controller Template element type.");
			}
			RUyyQSEmqRidLNphokXShBrzehNy = P_0;
			YVGDeWQAhUWKAOSPoxXuizkXaiTI = xhbaffnTlrHEJlYqvJfRvkDhUTlO++;
		}

		internal ControllerTemplateActionElementMap(ControllerTemplateElementType P_0, int P_1, ActionElementMap P_2)
			: this(P_0)
		{
			if (P_2 == null)
			{
				throw new ArgumentNullException("actionElementMap");
			}
			JwOchbdvhsvHVdSyYjtqEJdTZzYG = P_2._actionId;
			NPuaUGjYHfmjnFZazhpMoRbThWzG = P_1;
			kKFZZElqKQSUFMZnWdEudRvTJGpo = P_2.kKFZZElqKQSUFMZnWdEudRvTJGpo;
		}

		internal ControllerTemplateActionElementMap(ControllerTemplateElementType P_0, int P_1, int P_2, bool P_3)
			: this(P_0)
		{
			JwOchbdvhsvHVdSyYjtqEJdTZzYG = P_2;
			NPuaUGjYHfmjnFZazhpMoRbThWzG = P_1;
			kKFZZElqKQSUFMZnWdEudRvTJGpo = P_3;
		}

		protected ControllerTemplateActionElementMap(ActionElementMap P_0)
		{
		}

		internal int NFPuyCFqJBLAlixqfiJDFASGMotR(IControllerTemplate P_0, List<ActionElementMap> P_1, bool P_2)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("controllerTemplate");
			}
			if (P_1 == null)
			{
				throw new ArgumentNullException("results");
			}
			if (!P_2)
			{
				P_1.Clear();
			}
			int num = MjWorShMnyIKQrloWOUGnSSkuSAc(P_0, P_1, P_2);
			if (num == 0)
			{
				return 0;
			}
			int num2 = P_1.Count - num;
			for (int i = 0; i < num; i++)
			{
				int index = num2 + i;
				P_1[index].kKFZZElqKQSUFMZnWdEudRvTJGpo = kKFZZElqKQSUFMZnWdEudRvTJGpo;
				P_1[index]._actionId = JwOchbdvhsvHVdSyYjtqEJdTZzYG;
			}
			return num;
		}

		internal SerializedObject JxowpgGeLUBIhbyiSIIfFkkGllDuA()
		{
			SerializedObject serializedObject = new SerializedObject(GetType(), SerializedObject.ObjectType.Object);
			JxowpgGeLUBIhbyiSIIfFkkGllDuA(serializedObject);
			return serializedObject;
		}

		internal virtual void JxowpgGeLUBIhbyiSIIfFkkGllDuA(SerializedObject P_0)
		{
			P_0.Add("elementType", RUyyQSEmqRidLNphokXShBrzehNy);
			P_0.Add("enabled", kKFZZElqKQSUFMZnWdEudRvTJGpo);
			P_0.Add("elementIdentifierId", NPuaUGjYHfmjnFZazhpMoRbThWzG);
			P_0.Add("actionId", JwOchbdvhsvHVdSyYjtqEJdTZzYG);
		}

		internal virtual void ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject P_0)
		{
			SPGTRPyvIslcMdbPTItsewSLRPxx();
			P_0.TryGetDeserializedValueByRef("enabled", ref kKFZZElqKQSUFMZnWdEudRvTJGpo);
			P_0.TryGetDeserializedValueByRef("elementIdentifierId", ref NPuaUGjYHfmjnFZazhpMoRbThWzG);
			P_0.TryGetDeserializedValueByRef("actionId", ref JwOchbdvhsvHVdSyYjtqEJdTZzYG);
		}

		internal virtual void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			kKFZZElqKQSUFMZnWdEudRvTJGpo = true;
			NPuaUGjYHfmjnFZazhpMoRbThWzG = -1;
			JwOchbdvhsvHVdSyYjtqEJdTZzYG = -1;
		}

		internal abstract int kYHECBYfnrBTBCoSAFHALwaMjook(IControllerTemplateElementSource P_0, List<ActionElementMap> P_1, bool P_2);

		private int MjWorShMnyIKQrloWOUGnSSkuSAc(IControllerTemplate P_0, List<ActionElementMap> P_1, bool P_2)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("results");
			}
			if (!P_2)
			{
				P_1.Clear();
			}
			IControllerTemplateElement element = P_0.GetElement(NPuaUGjYHfmjnFZazhpMoRbThWzG);
			if (element == null)
			{
				return 0;
			}
			IControllerTemplateElementSource source = element.source;
			if (source == null)
			{
				return 0;
			}
			return kYHECBYfnrBTBCoSAFHALwaMjook(source, P_1, P_2);
		}

		internal static ControllerTemplateActionElementMap lOzmglNwrCkddSvkHTjSvLufiURJ(SerializedObject P_0)
		{
			if (P_0 == null)
			{
				return null;
			}
			if (!P_0.TryGetDeserializedValue<ControllerTemplateElementType>("elementType", out var value))
			{
				return null;
			}
			return value switch
			{
				ControllerTemplateElementType.Axis => new ControllerTemplateActionAxisMap(P_0), 
				ControllerTemplateElementType.Button => new ControllerTemplateActionButtonMap(P_0), 
				_ => throw new NotImplementedException(), 
			};
		}

		internal static ControllerTemplateActionElementMap lOzmglNwrCkddSvkHTjSvLufiURJ(ControllerTemplateElementTarget P_0, ActionElementMap P_1)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("actionElementMap");
			}
			if (P_0.elementType == ControllerTemplateElementType.Axis)
			{
				return new ControllerTemplateActionAxisMap(P_0.element.id, P_0.axisRange, P_1);
			}
			if (P_0.elementType == ControllerTemplateElementType.Button)
			{
				return new ControllerTemplateActionButtonMap(P_0.element.id, P_1);
			}
			throw new NotImplementedException();
		}

		internal static ControllerTemplateActionElementMap lOzmglNwrCkddSvkHTjSvLufiURJ(ActionElementMap P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("actionElementMap");
			}
			ControllerTemplateElementType controllerTemplateElementType = OzpbBOHoTkskKeBaOXWaTXQCmjjBA.yYbzPczOHmktyhazvMykohFtarNh(P_0._elementType, false);
			if (!InputTools.IsMappableType(controllerTemplateElementType))
			{
				return null;
			}
			return controllerTemplateElementType switch
			{
				ControllerTemplateElementType.Axis => new ControllerTemplateActionAxisMap(P_0._elementIdentifierId, P_0._actionId, P_0._axisRange, P_0._axisContribution, P_0._invert, P_0.kKFZZElqKQSUFMZnWdEudRvTJGpo), 
				ControllerTemplateElementType.Button => new ControllerTemplateActionButtonMap(P_0._elementIdentifierId, P_0._actionId, P_0._axisContribution, P_0.kKFZZElqKQSUFMZnWdEudRvTJGpo), 
				_ => throw new NotImplementedException(), 
			};
		}
	}
}
