using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired
{
	public class ControllerTemplateMap
	{
		private readonly int QajDFFaomlkLHzaostfWYpGUioys;

		private readonly int YVGDeWQAhUWKAOSPoxXuizkXaiTI;

		private readonly Guid wEGmrXqbEACLGKEEkvmJAEUkVrGjA;

		private readonly List<ControllerTemplateActionElementMap> dkUinCqcoUYCbdDIAstIzqPJHxBCA;

		private readonly ReadOnlyCollection<ControllerTemplateActionElementMap> SLwYyfvTOiFhXLptREPwBVhqamLkA;

		private bool kKFZZElqKQSUFMZnWdEudRvTJGpo;

		private int edeLxTfmbpxhIDuJltQHKkBuPZPj;

		private int ZiLaIhElokROILCGuHdQJwwpRZEL;

		private int oTTxavRtAZqKBBNpXAgzWVacZtXS = -1;

		private static int xhbaffnTlrHEJlYqvJfRvkDhUTlO;

		public int id
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1;
				}
				return YVGDeWQAhUWKAOSPoxXuizkXaiTI;
			}
		}

		public Guid templateTypeGuid
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Guid.Empty;
				}
				return wEGmrXqbEACLGKEEkvmJAEUkVrGjA;
			}
		}

		public bool enabled
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				return kKFZZElqKQSUFMZnWdEudRvTJGpo;
			}
			set
			{
				kKFZZElqKQSUFMZnWdEudRvTJGpo = value;
			}
		}

		public int categoryId
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1;
				}
				return edeLxTfmbpxhIDuJltQHKkBuPZPj;
			}
			internal set
			{
				edeLxTfmbpxhIDuJltQHKkBuPZPj = num;
			}
		}

		public int layoutId
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1;
				}
				return ZiLaIhElokROILCGuHdQJwwpRZEL;
			}
			internal set
			{
				ZiLaIhElokROILCGuHdQJwwpRZEL = ziLaIhElokROILCGuHdQJwwpRZEL;
			}
		}

		public IList<ControllerTemplateActionElementMap> ElementMaps
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<ControllerTemplateActionElementMap>.EmptyReadOnlyIListT;
				}
				return SLwYyfvTOiFhXLptREPwBVhqamLkA;
			}
		}

		internal ControllerTemplateMap(Guid P_0)
		{
			YVGDeWQAhUWKAOSPoxXuizkXaiTI = xhbaffnTlrHEJlYqvJfRvkDhUTlO++;
			QajDFFaomlkLHzaostfWYpGUioys = ReInput._id;
			wEGmrXqbEACLGKEEkvmJAEUkVrGjA = P_0;
			dkUinCqcoUYCbdDIAstIzqPJHxBCA = new List<ControllerTemplateActionElementMap>();
			SLwYyfvTOiFhXLptREPwBVhqamLkA = new ReadOnlyCollection<ControllerTemplateActionElementMap>(dkUinCqcoUYCbdDIAstIzqPJHxBCA);
			kKFZZElqKQSUFMZnWdEudRvTJGpo = true;
		}

		internal ControllerTemplateMap(Guid P_0, int P_1, int P_2, int P_3)
			: this(P_0)
		{
			edeLxTfmbpxhIDuJltQHKkBuPZPj = P_1;
			ZiLaIhElokROILCGuHdQJwwpRZEL = P_2;
			oTTxavRtAZqKBBNpXAgzWVacZtXS = P_3;
		}

		public string ToXmlString()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return string.Empty;
			}
			try
			{
				return JxowpgGeLUBIhbyiSIIfFkkGllDuA().ToXmlString(writeDocumentTag: true);
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to XML. " + ex.Message);
				return string.Empty;
			}
		}

		public string ToJsonString()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return string.Empty;
			}
			try
			{
				return JxowpgGeLUBIhbyiSIIfFkkGllDuA().ToJsonString();
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to JSON. " + ex.Message);
				return string.Empty;
			}
		}

		public ControllerMap ToControllerMap(Controller controller)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			IControllerTemplate template = controller.GetTemplate(wEGmrXqbEACLGKEEkvmJAEUkVrGjA);
			if (template == null)
			{
				Logger.LogError("The Controller does not implement the expected Controller Template.");
				return null;
			}
			ControllerMap controllerMap = ControllerMap.lOzmglNwrCkddSvkHTjSvLufiURJ(controller.type);
			controllerMap.categoryId = edeLxTfmbpxhIDuJltQHKkBuPZPj;
			controllerMap.layoutId = ZiLaIhElokROILCGuHdQJwwpRZEL;
			if (oTTxavRtAZqKBBNpXAgzWVacZtXS >= 0)
			{
				controllerMap.sourceMapId = oTTxavRtAZqKBBNpXAgzWVacZtXS;
			}
			controllerMap.controllerId = controller.id;
			controllerMap.enabled = kKFZZElqKQSUFMZnWdEudRvTJGpo;
			controllerMap.hardwareGuid = controller.fMxZVPLmyEupjctdQIaGgbDJdlvHA;
			using TempListPool.TList<ActionElementMap> tList = TempListPool.GetTList<ActionElementMap>();
			List<ActionElementMap> list = tList.list;
			for (int i = 0; i < dkUinCqcoUYCbdDIAstIzqPJHxBCA.Count; i++)
			{
				dkUinCqcoUYCbdDIAstIzqPJHxBCA[i].NFPuyCFqJBLAlixqfiJDFASGMotR(template, list, false);
				for (int j = 0; j < list.Count; j++)
				{
					controllerMap.lqUvlJMImEtcvtFlVYWmonZgLfZC(list[j]);
				}
			}
			return controllerMap;
		}

		internal virtual void eqxcMNQhZrhfftsCuDbhlTUMXCQB(SerializedObject P_0)
		{
			if (P_0.xmlInfo == null)
			{
				P_0.xmlInfo = new SerializedObject.XmlInfo();
			}
			P_0.Add("dataVersion", 1, SerializedObject.FieldOptions.ExculdeFromXml);
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "dataVersion",
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = 1.ToString()
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "templateTypeGuid",
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = wEGmrXqbEACLGKEEkvmJAEUkVrGjA.ToString()
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				yCwJMdebpVsoAZwRHxyMbDxHYvvT = "xmlns",
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "xsi",
				ZVXfpRlSPjKWRYaEYlEnqADXJiAs = null,
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = "http://www.w3.org/2001/XMLSchema-instance"
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				yCwJMdebpVsoAZwRHxyMbDxHYvvT = "xsi",
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "schemaLocation",
				ZVXfpRlSPjKWRYaEYlEnqADXJiAs = null,
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = string.Format("{0} {1}{2}{3}{4}{5}", "http://guavaman.com/rewired", "http://guavaman.com/schemas/rewired/", "1.0", "/", GetType().Name, ".xsd")
			});
			P_0.Add("templateTypeGuid", wEGmrXqbEACLGKEEkvmJAEUkVrGjA);
			P_0.Add("enabled", kKFZZElqKQSUFMZnWdEudRvTJGpo);
			P_0.Add("categoryId", edeLxTfmbpxhIDuJltQHKkBuPZPj);
			P_0.Add("layoutId", ZiLaIhElokROILCGuHdQJwwpRZEL);
			P_0.Add("sourceMapId", oTTxavRtAZqKBBNpXAgzWVacZtXS);
			int count = dkUinCqcoUYCbdDIAstIzqPJHxBCA.Count;
			List<object> list = new List<object>();
			P_0.Add("elementMaps", list);
			for (int i = 0; i < count; i++)
			{
				if (dkUinCqcoUYCbdDIAstIzqPJHxBCA[i] != null)
				{
					list.Add(dkUinCqcoUYCbdDIAstIzqPJHxBCA[i].JxowpgGeLUBIhbyiSIIfFkkGllDuA());
				}
			}
		}

		internal virtual void ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject P_0)
		{
			SPGTRPyvIslcMdbPTItsewSLRPxx();
			P_0.TryGetDeserializedValueByRef("enabled", ref kKFZZElqKQSUFMZnWdEudRvTJGpo);
			P_0.TryGetDeserializedValueByRef("categoryId", ref edeLxTfmbpxhIDuJltQHKkBuPZPj);
			P_0.TryGetDeserializedValueByRef("layoutId", ref ZiLaIhElokROILCGuHdQJwwpRZEL);
			P_0.TryGetDeserializedValueByRef("sourceMapId", ref oTTxavRtAZqKBBNpXAgzWVacZtXS);
			SerializedObject value = null;
			if (!P_0.TryGetDeserializedValueByRef("elementMaps", ref value) || value == null)
			{
				return;
			}
			for (int i = 0; i < value.count; i++)
			{
				if (value.TryGetDeserializedValue<SerializedObject>(i, out var value2) || value2 == null)
				{
					ControllerTemplateActionElementMap controllerTemplateActionElementMap = ControllerTemplateActionElementMap.lOzmglNwrCkddSvkHTjSvLufiURJ(value2);
					if (controllerTemplateActionElementMap != null)
					{
						ZxwtLymUXBfmZpaPpFNVkvIPfZZDA(controllerTemplateActionElementMap);
					}
				}
			}
		}

		private void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			kKFZZElqKQSUFMZnWdEudRvTJGpo = true;
			edeLxTfmbpxhIDuJltQHKkBuPZPj = -1;
			ZiLaIhElokROILCGuHdQJwwpRZEL = -1;
			oTTxavRtAZqKBBNpXAgzWVacZtXS = -1;
			dkUinCqcoUYCbdDIAstIzqPJHxBCA.Clear();
		}

		private SerializedObject JxowpgGeLUBIhbyiSIIfFkkGllDuA()
		{
			SerializedObject serializedObject = new SerializedObject(GetType(), SerializedObject.ObjectType.Object);
			eqxcMNQhZrhfftsCuDbhlTUMXCQB(serializedObject);
			return serializedObject;
		}

		internal void ZxwtLymUXBfmZpaPpFNVkvIPfZZDA(ControllerTemplateActionElementMap P_0)
		{
			if (P_0 != null)
			{
				dkUinCqcoUYCbdDIAstIzqPJHxBCA.Add(P_0);
			}
		}

		internal static ControllerTemplateMap FSIRgQmHZEfgkCSfmcoeDxGPUtlh(IControllerTemplate P_0, ControllerMap P_1)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("controllerMap");
			}
			if (P_0 == null)
			{
				throw new ArgumentNullException("controllerTemplate");
			}
			if (!ReInput.isReady)
			{
				throw new Exception("Rewired is not initialized.");
			}
			Controller controller = ReInput.controllers.GetController(P_1.controllerType, P_1.controllerId);
			if (controller == null)
			{
				Logger.LogError("The Controller Map is not associated with a Controller. This method can only be used with a Controller Map that is associated with a Controller.", requiredThreadSafety: true);
				return null;
			}
			if (!controller.ImplementsTemplate(P_0.typeGuid))
			{
				Logger.LogError("The Controller does not implement the Controller Template.", requiredThreadSafety: true);
				return null;
			}
			ControllerTemplateMap controllerTemplateMap = new ControllerTemplateMap(P_0.typeGuid);
			controllerTemplateMap.kKFZZElqKQSUFMZnWdEudRvTJGpo = P_1.enabled;
			controllerTemplateMap.edeLxTfmbpxhIDuJltQHKkBuPZPj = P_1.categoryId;
			controllerTemplateMap.ZiLaIhElokROILCGuHdQJwwpRZEL = P_1.layoutId;
			controllerTemplateMap.oTTxavRtAZqKBBNpXAgzWVacZtXS = P_1.sourceMapId;
			using TempListPool.TList<ControllerTemplateElementTarget> tList = TempListPool.GetTList<ControllerTemplateElementTarget>();
			List<ControllerTemplateElementTarget> list = tList.list;
			foreach (ActionElementMap allMap in P_1.AllMaps)
			{
				if (P_0.GetElementTargets(allMap, list) > 0)
				{
					for (int i = 0; i < list.Count; i++)
					{
						controllerTemplateMap.ZxwtLymUXBfmZpaPpFNVkvIPfZZDA(ControllerTemplateActionElementMap.lOzmglNwrCkddSvkHTjSvLufiURJ(list[i], allMap));
					}
				}
			}
			return controllerTemplateMap;
		}

		public static ControllerTemplateMap FromXml(string xmlString)
		{
			try
			{
				return EDhKFXVvqenOpomJGzojRIfOAHPDA(SerializedObject.FromXml(typeof(ControllerTemplateMap), xmlString));
			}
			catch (Exception ex)
			{
				Logger.LogError("Error creating ControllerTemplateMap from XML! " + ex.Message);
				return null;
			}
		}

		public static ControllerTemplateMap FromJson(string jsonString)
		{
			try
			{
				return EDhKFXVvqenOpomJGzojRIfOAHPDA(SerializedObject.FromJson(typeof(ControllerTemplateMap), jsonString));
			}
			catch (Exception ex)
			{
				Logger.LogError("Error creating ControllerTemplateMap from JSON! " + ex.Message);
				return null;
			}
		}

		private static ControllerTemplateMap EDhKFXVvqenOpomJGzojRIfOAHPDA(SerializedObject P_0)
		{
			if (!P_0.TryGetDeserializedValue<Guid>("templateTypeGuid", out var value))
			{
				throw new Exception();
			}
			ControllerTemplateMap controllerTemplateMap = new ControllerTemplateMap(value);
			controllerTemplateMap.ckPgJNiFQjrLdKfwClUsdsySXFyIb(P_0);
			return controllerTemplateMap;
		}
	}
}
