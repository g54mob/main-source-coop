using System;
using System.Collections.Generic;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired
{
	public abstract class ControllerTemplateActionElementMap
	{
		private readonly int MzlTsdXUjVBhpWGONVWjFRIvbSxN;

		private readonly ControllerTemplateElementType dmnoGzlAbBxQjrGUIxPjBPcVHLYFA;

		private bool uZCgCpomIXdVfLOmHsTuFGjRuzWL;

		private int ovrsVYfweRLcgPdBlXwJXAaSVvWd;

		private int KxoGnyVfFeBTxsINwkdtYNBnfepEA;

		private static int cdAyiNxJbAXdnkUgEWndQnAoWIkT;

		public int id => MzlTsdXUjVBhpWGONVWjFRIvbSxN;

		public ControllerTemplateElementType elementType => dmnoGzlAbBxQjrGUIxPjBPcVHLYFA;

		public bool enabled
		{
			get
			{
				return uZCgCpomIXdVfLOmHsTuFGjRuzWL;
			}
			set
			{
				uZCgCpomIXdVfLOmHsTuFGjRuzWL = value;
			}
		}

		public int actionId
		{
			get
			{
				return ovrsVYfweRLcgPdBlXwJXAaSVvWd;
			}
			set
			{
				ovrsVYfweRLcgPdBlXwJXAaSVvWd = value;
			}
		}

		public int elementIdentifierId
		{
			get
			{
				return KxoGnyVfFeBTxsINwkdtYNBnfepEA;
			}
			set
			{
				KxoGnyVfFeBTxsINwkdtYNBnfepEA = value;
			}
		}

		internal ControllerTemplateActionElementMap(ControllerTemplateElementType P_0)
		{
			if (!InputTools.IsMappableType(P_0))
			{
				throw new ArgumentException(P_0.ToString() + " is not a supported mappable Controller Template element type.");
			}
			dmnoGzlAbBxQjrGUIxPjBPcVHLYFA = P_0;
			MzlTsdXUjVBhpWGONVWjFRIvbSxN = cdAyiNxJbAXdnkUgEWndQnAoWIkT++;
		}

		internal ControllerTemplateActionElementMap(ControllerTemplateElementType P_0, int P_1, ActionElementMap P_2)
			: this(P_0)
		{
			if (P_2 == null)
			{
				throw new ArgumentNullException("actionElementMap");
			}
			ovrsVYfweRLcgPdBlXwJXAaSVvWd = P_2._actionId;
			KxoGnyVfFeBTxsINwkdtYNBnfepEA = P_1;
			uZCgCpomIXdVfLOmHsTuFGjRuzWL = P_2.uPyFcaFdRzKajesnqkOUtFvpIRKHA;
		}

		internal ControllerTemplateActionElementMap(ControllerTemplateElementType P_0, int P_1, int P_2, bool P_3)
			: this(P_0)
		{
			ovrsVYfweRLcgPdBlXwJXAaSVvWd = P_2;
			KxoGnyVfFeBTxsINwkdtYNBnfepEA = P_1;
			uZCgCpomIXdVfLOmHsTuFGjRuzWL = P_3;
		}

		protected ControllerTemplateActionElementMap(ActionElementMap P_0)
		{
		}

		internal int hvhdmCFTyebrLyYAOFkCWaZMerWF(IControllerTemplate P_0, List<ActionElementMap> P_1, bool P_2)
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
			int num = TZTyfDlDJBQuyssHwjIvJoeWwnTm(P_0, P_1, P_2);
			if (num == 0)
			{
				return 0;
			}
			int num2 = P_1.Count - num;
			for (int i = 0; i < num; i++)
			{
				int index = num2 + i;
				P_1[index].uPyFcaFdRzKajesnqkOUtFvpIRKHA = uZCgCpomIXdVfLOmHsTuFGjRuzWL;
				P_1[index]._actionId = ovrsVYfweRLcgPdBlXwJXAaSVvWd;
			}
			return num;
		}

		internal SerializedObject yKuGOaftbNdiTXgZcKYQyLSECSlk()
		{
			SerializedObject serializedObject = new SerializedObject(GetType(), SerializedObject.ObjectType.Object);
			kKqfEeAXcHvhBUZDJUKIJUUHQJDj(serializedObject);
			return serializedObject;
		}

		internal virtual void kKqfEeAXcHvhBUZDJUKIJUUHQJDj(SerializedObject P_0)
		{
			P_0.Add("elementType", dmnoGzlAbBxQjrGUIxPjBPcVHLYFA);
			P_0.Add("enabled", uZCgCpomIXdVfLOmHsTuFGjRuzWL);
			P_0.Add("elementIdentifierId", KxoGnyVfFeBTxsINwkdtYNBnfepEA);
			P_0.Add("actionId", ovrsVYfweRLcgPdBlXwJXAaSVvWd);
		}

		internal virtual void brPUUTgHWZJnOCkcunQbZcpMlIzm(SerializedObject P_0)
		{
			uNvXvjCnRFGrqgWjquXBFcvytYxn();
			P_0.TryGetDeserializedValueByRef("enabled", ref uZCgCpomIXdVfLOmHsTuFGjRuzWL);
			P_0.TryGetDeserializedValueByRef("elementIdentifierId", ref KxoGnyVfFeBTxsINwkdtYNBnfepEA);
			P_0.TryGetDeserializedValueByRef("actionId", ref ovrsVYfweRLcgPdBlXwJXAaSVvWd);
		}

		internal virtual void uNvXvjCnRFGrqgWjquXBFcvytYxn()
		{
			uZCgCpomIXdVfLOmHsTuFGjRuzWL = true;
			KxoGnyVfFeBTxsINwkdtYNBnfepEA = -1;
			ovrsVYfweRLcgPdBlXwJXAaSVvWd = -1;
		}

		internal abstract int KFGuDGysyfzInGGcCFZmIFyBRYfE(IControllerTemplateElementSource P_0, List<ActionElementMap> P_1, bool P_2);

		private int TZTyfDlDJBQuyssHwjIvJoeWwnTm(IControllerTemplate P_0, List<ActionElementMap> P_1, bool P_2)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("results");
			}
			if (!P_2)
			{
				P_1.Clear();
			}
			IControllerTemplateElement element = P_0.GetElement(KxoGnyVfFeBTxsINwkdtYNBnfepEA);
			if (element == null)
			{
				return 0;
			}
			IControllerTemplateElementSource source = element.source;
			if (source == null)
			{
				return 0;
			}
			return KFGuDGysyfzInGGcCFZmIFyBRYfE(source, P_1, P_2);
		}

		internal static ControllerTemplateActionElementMap VUVvJsQUnMJZkNokZKmPBBVhBMUq(SerializedObject P_0)
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

		internal static ControllerTemplateActionElementMap LgqUdcPPZyvkGAOYnYRJFazMJPQf(ControllerTemplateElementTarget P_0, ActionElementMap P_1)
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

		internal static ControllerTemplateActionElementMap hPZuZdVEdzCRIKSsNUyjiRXEaicP(ActionElementMap P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("actionElementMap");
			}
			ControllerTemplateElementType controllerTemplateElementType = cVDyIiOsEfJNYzVuZSmuEXqylgT.WMwTgbsyrBXDwSyPLcPBVwCFfUPd(P_0._elementType, false);
			if (!InputTools.IsMappableType(controllerTemplateElementType))
			{
				return null;
			}
			return controllerTemplateElementType switch
			{
				ControllerTemplateElementType.Axis => new ControllerTemplateActionAxisMap(P_0._elementIdentifierId, P_0._actionId, P_0._axisRange, P_0._axisContribution, P_0._invert, P_0.uPyFcaFdRzKajesnqkOUtFvpIRKHA), 
				ControllerTemplateElementType.Button => new ControllerTemplateActionButtonMap(P_0._elementIdentifierId, P_0._actionId, P_0._axisContribution, P_0.uPyFcaFdRzKajesnqkOUtFvpIRKHA), 
				_ => throw new NotImplementedException(), 
			};
		}
	}
}
