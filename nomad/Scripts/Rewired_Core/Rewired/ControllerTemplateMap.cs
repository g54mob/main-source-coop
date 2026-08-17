using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired
{
	public class ControllerTemplateMap
	{
		private readonly int GSvCoCFfARioDiVjeAErYnCJWRndb;

		private readonly int wFQxUKGOcQtWhHAGAjHxFQqpRSHfA;

		private readonly Guid sNaDItuGqqzJReckcPYEpRAQNcry;

		private readonly List<ControllerTemplateActionElementMap> dJnGDZxuicaPmyeZAdRaxfiCQkXu;

		private readonly ReadOnlyCollection<ControllerTemplateActionElementMap> gTqfqSskMMEQCEhzIyuEOMvZBRqQA;

		private bool SDuyRChzuLKXcqUCzdqGrAYMfRej;

		private int YNnVWhUkEnicqFPzPspNNldMeiyL;

		private int DevIRtVqIEGeUKgwgMztieMyBOaNA;

		private int ScDxjOyOEXaWeAHJklrCYUTuQjiIA = -1;

		private static int yMjtMDSJzIfmbcUOHdxxCtypehk;

		public int id
		{
			get
			{
				if (ReInput._id != GSvCoCFfARioDiVjeAErYnCJWRndb)
				{
					ReInput.CheckInitialized(GSvCoCFfARioDiVjeAErYnCJWRndb);
					return -1;
				}
				return wFQxUKGOcQtWhHAGAjHxFQqpRSHfA;
			}
		}

		public Guid templateTypeGuid
		{
			get
			{
				if (ReInput._id != GSvCoCFfARioDiVjeAErYnCJWRndb)
				{
					ReInput.CheckInitialized(GSvCoCFfARioDiVjeAErYnCJWRndb);
					return Guid.Empty;
				}
				return sNaDItuGqqzJReckcPYEpRAQNcry;
			}
		}

		public bool enabled
		{
			get
			{
				if (ReInput._id != GSvCoCFfARioDiVjeAErYnCJWRndb)
				{
					ReInput.CheckInitialized(GSvCoCFfARioDiVjeAErYnCJWRndb);
					return false;
				}
				return SDuyRChzuLKXcqUCzdqGrAYMfRej;
			}
			set
			{
				SDuyRChzuLKXcqUCzdqGrAYMfRej = value;
			}
		}

		public int categoryId
		{
			get
			{
				if (ReInput._id != GSvCoCFfARioDiVjeAErYnCJWRndb)
				{
					ReInput.CheckInitialized(GSvCoCFfARioDiVjeAErYnCJWRndb);
					return -1;
				}
				return YNnVWhUkEnicqFPzPspNNldMeiyL;
			}
			internal set
			{
				YNnVWhUkEnicqFPzPspNNldMeiyL = yNnVWhUkEnicqFPzPspNNldMeiyL;
			}
		}

		public int layoutId
		{
			get
			{
				if (ReInput._id != GSvCoCFfARioDiVjeAErYnCJWRndb)
				{
					ReInput.CheckInitialized(GSvCoCFfARioDiVjeAErYnCJWRndb);
					return -1;
				}
				return DevIRtVqIEGeUKgwgMztieMyBOaNA;
			}
			internal set
			{
				DevIRtVqIEGeUKgwgMztieMyBOaNA = devIRtVqIEGeUKgwgMztieMyBOaNA;
			}
		}

		public IList<ControllerTemplateActionElementMap> ElementMaps
		{
			get
			{
				if (ReInput._id != GSvCoCFfARioDiVjeAErYnCJWRndb)
				{
					ReInput.CheckInitialized(GSvCoCFfARioDiVjeAErYnCJWRndb);
					return EmptyObjects<ControllerTemplateActionElementMap>.EmptyReadOnlyIListT;
				}
				return gTqfqSskMMEQCEhzIyuEOMvZBRqQA;
			}
		}

		internal ControllerTemplateMap(Guid P_0)
		{
			wFQxUKGOcQtWhHAGAjHxFQqpRSHfA = yMjtMDSJzIfmbcUOHdxxCtypehk++;
			GSvCoCFfARioDiVjeAErYnCJWRndb = ReInput._id;
			sNaDItuGqqzJReckcPYEpRAQNcry = P_0;
			dJnGDZxuicaPmyeZAdRaxfiCQkXu = new List<ControllerTemplateActionElementMap>();
			gTqfqSskMMEQCEhzIyuEOMvZBRqQA = new ReadOnlyCollection<ControllerTemplateActionElementMap>(dJnGDZxuicaPmyeZAdRaxfiCQkXu);
			SDuyRChzuLKXcqUCzdqGrAYMfRej = true;
		}

		internal ControllerTemplateMap(Guid P_0, int P_1, int P_2, int P_3)
			: this(P_0)
		{
			YNnVWhUkEnicqFPzPspNNldMeiyL = P_1;
			DevIRtVqIEGeUKgwgMztieMyBOaNA = P_2;
			ScDxjOyOEXaWeAHJklrCYUTuQjiIA = P_3;
		}

		public string ToXmlString()
		{
			if (ReInput._id != GSvCoCFfARioDiVjeAErYnCJWRndb)
			{
				ReInput.CheckInitialized(GSvCoCFfARioDiVjeAErYnCJWRndb);
				return string.Empty;
			}
			try
			{
				return rTxHjorLFIzrjKAAjkixXHzrGoodA().ToXmlString(writeDocumentTag: true);
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to XML. " + ex.Message);
				return string.Empty;
			}
		}

		public string ToJsonString()
		{
			if (ReInput._id != GSvCoCFfARioDiVjeAErYnCJWRndb)
			{
				ReInput.CheckInitialized(GSvCoCFfARioDiVjeAErYnCJWRndb);
				return string.Empty;
			}
			try
			{
				return rTxHjorLFIzrjKAAjkixXHzrGoodA().ToJsonString();
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to JSON. " + ex.Message);
				return string.Empty;
			}
		}

		public ControllerMap ToControllerMap(Controller controller)
		{
			if (ReInput._id != GSvCoCFfARioDiVjeAErYnCJWRndb)
			{
				ReInput.CheckInitialized(GSvCoCFfARioDiVjeAErYnCJWRndb);
				return null;
			}
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			IControllerTemplate template = controller.GetTemplate(sNaDItuGqqzJReckcPYEpRAQNcry);
			if (template == null)
			{
				Logger.LogError("The Controller does not implement the expected Controller Template.");
				return null;
			}
			ControllerMap controllerMap = ControllerMap.tTAynXDpgjHudMCdDyGjoeUtdBDX(controller.type);
			controllerMap.categoryId = YNnVWhUkEnicqFPzPspNNldMeiyL;
			controllerMap.layoutId = DevIRtVqIEGeUKgwgMztieMyBOaNA;
			if (ScDxjOyOEXaWeAHJklrCYUTuQjiIA >= 0)
			{
				controllerMap.sourceMapId = ScDxjOyOEXaWeAHJklrCYUTuQjiIA;
			}
			controllerMap.controllerId = controller.id;
			controllerMap.enabled = SDuyRChzuLKXcqUCzdqGrAYMfRej;
			controllerMap.hardwareGuid = controller.zyYehdPaDXciYCtKVPxEsznJTyqP;
			using TempListPool.TList<ActionElementMap> tList = TempListPool.GetTList<ActionElementMap>();
			List<ActionElementMap> list = tList.list;
			for (int i = 0; i < dJnGDZxuicaPmyeZAdRaxfiCQkXu.Count; i++)
			{
				dJnGDZxuicaPmyeZAdRaxfiCQkXu[i].hvhdmCFTyebrLyYAOFkCWaZMerWF(template, list, false);
				for (int j = 0; j < list.Count; j++)
				{
					controllerMap.MJooKrlGDJFRhfXMOcJbjtQgiaYJA(list[j]);
				}
			}
			return controllerMap;
		}

		internal virtual void VzoLDMRkeYYgfpaQggNafbnhJHGWA(SerializedObject P_0)
		{
			if (P_0.xmlInfo == null)
			{
				P_0.xmlInfo = new SerializedObject.XmlInfo();
			}
			P_0.Add("dataVersion", 1, SerializedObject.FieldOptions.ExculdeFromXml);
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "dataVersion",
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = 1.ToString()
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "templateTypeGuid",
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = sNaDItuGqqzJReckcPYEpRAQNcry.ToString()
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				XCTmYYtBCCzTuZmlJPszaypSgJdS = "xmlns",
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "xsi",
				GpDRMyFZBJdjlvWjrvVYAzZnhbYW = null,
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = "http://www.w3.org/2001/XMLSchema-instance"
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				XCTmYYtBCCzTuZmlJPszaypSgJdS = "xsi",
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "schemaLocation",
				GpDRMyFZBJdjlvWjrvVYAzZnhbYW = null,
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = string.Format("{0} {1}{2}{3}{4}{5}", "http://guavaman.com/rewired", "http://guavaman.com/schemas/rewired/", "1.0", "/", GetType().Name, ".xsd")
			});
			P_0.Add("templateTypeGuid", sNaDItuGqqzJReckcPYEpRAQNcry);
			P_0.Add("enabled", SDuyRChzuLKXcqUCzdqGrAYMfRej);
			P_0.Add("categoryId", YNnVWhUkEnicqFPzPspNNldMeiyL);
			P_0.Add("layoutId", DevIRtVqIEGeUKgwgMztieMyBOaNA);
			P_0.Add("sourceMapId", ScDxjOyOEXaWeAHJklrCYUTuQjiIA);
			int count = dJnGDZxuicaPmyeZAdRaxfiCQkXu.Count;
			List<object> list = new List<object>();
			P_0.Add("elementMaps", list);
			for (int i = 0; i < count; i++)
			{
				if (dJnGDZxuicaPmyeZAdRaxfiCQkXu[i] != null)
				{
					list.Add(dJnGDZxuicaPmyeZAdRaxfiCQkXu[i].yKuGOaftbNdiTXgZcKYQyLSECSlk());
				}
			}
		}

		internal virtual void JqICgAMnPdCKXOfVRyDBWCFmxpbW(SerializedObject P_0)
		{
			vBfFlUVlaHvjTPfJKBObAcfZdvnZ();
			P_0.TryGetDeserializedValueByRef("enabled", ref SDuyRChzuLKXcqUCzdqGrAYMfRej);
			P_0.TryGetDeserializedValueByRef("categoryId", ref YNnVWhUkEnicqFPzPspNNldMeiyL);
			P_0.TryGetDeserializedValueByRef("layoutId", ref DevIRtVqIEGeUKgwgMztieMyBOaNA);
			P_0.TryGetDeserializedValueByRef("sourceMapId", ref ScDxjOyOEXaWeAHJklrCYUTuQjiIA);
			SerializedObject value = null;
			if (!P_0.TryGetDeserializedValueByRef("elementMaps", ref value) || value == null)
			{
				return;
			}
			for (int i = 0; i < value.count; i++)
			{
				if (value.TryGetDeserializedValue<SerializedObject>(i, out var value2) || value2 == null)
				{
					ControllerTemplateActionElementMap controllerTemplateActionElementMap = ControllerTemplateActionElementMap.VUVvJsQUnMJZkNokZKmPBBVhBMUq(value2);
					if (controllerTemplateActionElementMap != null)
					{
						obMkAgIOWyiPnfwJwgFVtoLAQyTe(controllerTemplateActionElementMap);
					}
				}
			}
		}

		private void vBfFlUVlaHvjTPfJKBObAcfZdvnZ()
		{
			SDuyRChzuLKXcqUCzdqGrAYMfRej = true;
			YNnVWhUkEnicqFPzPspNNldMeiyL = -1;
			DevIRtVqIEGeUKgwgMztieMyBOaNA = -1;
			ScDxjOyOEXaWeAHJklrCYUTuQjiIA = -1;
			dJnGDZxuicaPmyeZAdRaxfiCQkXu.Clear();
		}

		private SerializedObject rTxHjorLFIzrjKAAjkixXHzrGoodA()
		{
			SerializedObject serializedObject = new SerializedObject(GetType(), SerializedObject.ObjectType.Object);
			VzoLDMRkeYYgfpaQggNafbnhJHGWA(serializedObject);
			return serializedObject;
		}

		internal void obMkAgIOWyiPnfwJwgFVtoLAQyTe(ControllerTemplateActionElementMap P_0)
		{
			if (P_0 != null)
			{
				dJnGDZxuicaPmyeZAdRaxfiCQkXu.Add(P_0);
			}
		}

		internal static ControllerTemplateMap etXjiJxFMJGdZePDIixBOnvxkoedb(IControllerTemplate P_0, ControllerMap P_1)
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
			controllerTemplateMap.SDuyRChzuLKXcqUCzdqGrAYMfRej = P_1.enabled;
			controllerTemplateMap.YNnVWhUkEnicqFPzPspNNldMeiyL = P_1.categoryId;
			controllerTemplateMap.DevIRtVqIEGeUKgwgMztieMyBOaNA = P_1.layoutId;
			controllerTemplateMap.ScDxjOyOEXaWeAHJklrCYUTuQjiIA = P_1.sourceMapId;
			using TempListPool.TList<ControllerTemplateElementTarget> tList = TempListPool.GetTList<ControllerTemplateElementTarget>();
			List<ControllerTemplateElementTarget> list = tList.list;
			foreach (ActionElementMap allMap in P_1.AllMaps)
			{
				if (P_0.GetElementTargets(allMap, list) > 0)
				{
					for (int i = 0; i < list.Count; i++)
					{
						controllerTemplateMap.obMkAgIOWyiPnfwJwgFVtoLAQyTe(ControllerTemplateActionElementMap.LgqUdcPPZyvkGAOYnYRJFazMJPQf(list[i], allMap));
					}
				}
			}
			return controllerTemplateMap;
		}

		public static ControllerTemplateMap FromXml(string xmlString)
		{
			try
			{
				return FFzYFMuiGiAaHmGNJXHgKjONhEdW(SerializedObject.FromXml(typeof(ControllerTemplateMap), xmlString));
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
				return FFzYFMuiGiAaHmGNJXHgKjONhEdW(SerializedObject.FromJson(typeof(ControllerTemplateMap), jsonString));
			}
			catch (Exception ex)
			{
				Logger.LogError("Error creating ControllerTemplateMap from JSON! " + ex.Message);
				return null;
			}
		}

		private static ControllerTemplateMap FFzYFMuiGiAaHmGNJXHgKjONhEdW(SerializedObject P_0)
		{
			if (!P_0.TryGetDeserializedValue<Guid>("templateTypeGuid", out var value))
			{
				throw new Exception();
			}
			ControllerTemplateMap controllerTemplateMap = new ControllerTemplateMap(value);
			controllerTemplateMap.JqICgAMnPdCKXOfVRyDBWCFmxpbW(P_0);
			return controllerTemplateMap;
		}
	}
}
