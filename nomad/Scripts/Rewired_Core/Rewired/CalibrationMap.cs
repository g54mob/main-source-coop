using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	public sealed class CalibrationMap
	{
		private AxisCalibration[] HXKrpesdiwufFXJMpINADFYSNfUk;

		private MappedArray<AxisCalibration> vkLdpDoCchINZgDfohmJwjwZxHpRA;

		private IList<AxisCalibration> nzgHExHsAuqroGHOoaDDGmBMLzfwA;

		private readonly int IQTQeVqkCFnZnTkQnEauufWvtQrh;

		public IList<AxisCalibration> Axes => nzgHExHsAuqroGHOoaDDGmBMLzfwA;

		public int axisCount
		{
			get
			{
				if (HXKrpesdiwufFXJMpINADFYSNfUk == null)
				{
					return 0;
				}
				return HXKrpesdiwufFXJMpINADFYSNfUk.Length;
			}
		}

		private CalibrationMap()
		{
			IQTQeVqkCFnZnTkQnEauufWvtQrh = ReInput.id;
		}

		internal CalibrationMap(AxisCalibrationData[] P_0, Func<int, int> P_1)
			: this()
		{
			int num = ((P_0 != null) ? P_0.Length : 0);
			HXKrpesdiwufFXJMpINADFYSNfUk = new AxisCalibration[num];
			vkLdpDoCchINZgDfohmJwjwZxHpRA = new MappedArray<AxisCalibration>(HXKrpesdiwufFXJMpINADFYSNfUk, P_1);
			for (int i = 0; i < num; i++)
			{
				HXKrpesdiwufFXJMpINADFYSNfUk[i] = new AxisCalibration(P_0[i]);
			}
			nzgHExHsAuqroGHOoaDDGmBMLzfwA = new ReadOnlyCollection<AxisCalibration>(vkLdpDoCchINZgDfohmJwjwZxHpRA);
		}

		public CalibrationMap(AxisCalibration[] P_0)
			: this()
		{
			HXKrpesdiwufFXJMpINADFYSNfUk = P_0;
			vkLdpDoCchINZgDfohmJwjwZxHpRA = new MappedArray<AxisCalibration>(HXKrpesdiwufFXJMpINADFYSNfUk, null);
			nzgHExHsAuqroGHOoaDDGmBMLzfwA = new ReadOnlyCollection<AxisCalibration>(vkLdpDoCchINZgDfohmJwjwZxHpRA);
		}

		public void Reset()
		{
			if (ReInput._id != IQTQeVqkCFnZnTkQnEauufWvtQrh)
			{
				ReInput.CheckInitialized(IQTQeVqkCFnZnTkQnEauufWvtQrh);
				return;
			}
			for (int i = 0; i < HXKrpesdiwufFXJMpINADFYSNfUk.Length; i++)
			{
				HXKrpesdiwufFXJMpINADFYSNfUk[i].Reset();
			}
		}

		public AxisCalibration GetAxis(int index)
		{
			if (ReInput._id != IQTQeVqkCFnZnTkQnEauufWvtQrh)
			{
				ReInput.CheckInitialized(IQTQeVqkCFnZnTkQnEauufWvtQrh);
				return null;
			}
			if (index < 0 || index >= HXKrpesdiwufFXJMpINADFYSNfUk.Length)
			{
				return null;
			}
			return vkLdpDoCchINZgDfohmJwjwZxHpRA[index];
		}

		public float GetCalibratedValue(int axisIndex, float value)
		{
			if (ReInput._id != IQTQeVqkCFnZnTkQnEauufWvtQrh)
			{
				ReInput.CheckInitialized(IQTQeVqkCFnZnTkQnEauufWvtQrh);
				return 0f;
			}
			if (axisIndex < 0 || axisIndex >= HXKrpesdiwufFXJMpINADFYSNfUk.Length)
			{
				return value;
			}
			return vkLdpDoCchINZgDfohmJwjwZxHpRA[axisIndex].GetCalibratedValue(value);
		}

		public bool SetAxisData(int index, AxisCalibrationData data)
		{
			if (ReInput._id != IQTQeVqkCFnZnTkQnEauufWvtQrh)
			{
				ReInput.CheckInitialized(IQTQeVqkCFnZnTkQnEauufWvtQrh);
				return false;
			}
			if (index < 0 || index >= HXKrpesdiwufFXJMpINADFYSNfUk.Length)
			{
				return false;
			}
			vkLdpDoCchINZgDfohmJwjwZxHpRA[index].SetData(data);
			return true;
		}

		public AxisCalibrationData GetAxisData(int index)
		{
			if (ReInput._id != IQTQeVqkCFnZnTkQnEauufWvtQrh)
			{
				ReInput.CheckInitialized(IQTQeVqkCFnZnTkQnEauufWvtQrh);
				return default(AxisCalibrationData);
			}
			if (index < 0 || index >= HXKrpesdiwufFXJMpINADFYSNfUk.Length)
			{
				return default(AxisCalibrationData);
			}
			return vkLdpDoCchINZgDfohmJwjwZxHpRA[index].GetData();
		}

		internal void CopyFrom(CalibrationMap map, bool copyHardwareDeadzone)
		{
			if (map == null)
			{
				return;
			}
			if (map.HXKrpesdiwufFXJMpINADFYSNfUk.Length != HXKrpesdiwufFXJMpINADFYSNfUk.Length)
			{
				Logger.LogError("Calibration map data does not match the number of elements in the hardware!");
				return;
			}
			for (int i = 0; i < HXKrpesdiwufFXJMpINADFYSNfUk.Length; i++)
			{
				HXKrpesdiwufFXJMpINADFYSNfUk[i].CopyFrom(map.HXKrpesdiwufFXJMpINADFYSNfUk[i], copyHardwareDeadzone);
			}
		}

		public string ToXmlString()
		{
			if (ReInput._id != IQTQeVqkCFnZnTkQnEauufWvtQrh)
			{
				ReInput.CheckInitialized(IQTQeVqkCFnZnTkQnEauufWvtQrh);
				return string.Empty;
			}
			string empty = string.Empty;
			try
			{
				return ItIKUftLxZlGRwRHOTKvJbbsTXmJ().ToXmlString(writeDocumentTag: true);
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing CalibrationMap to XML! " + ex.Message);
				return empty;
			}
		}

		public string ToJsonString()
		{
			if (ReInput._id != IQTQeVqkCFnZnTkQnEauufWvtQrh)
			{
				ReInput.CheckInitialized(IQTQeVqkCFnZnTkQnEauufWvtQrh);
				return string.Empty;
			}
			try
			{
				return ItIKUftLxZlGRwRHOTKvJbbsTXmJ().ToJsonString();
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing CalibrationMap to JSON! " + ex.Message);
			}
			return string.Empty;
		}

		public bool ImportXmlString(string xmlString)
		{
			if (ReInput._id != IQTQeVqkCFnZnTkQnEauufWvtQrh)
			{
				ReInput.CheckInitialized(IQTQeVqkCFnZnTkQnEauufWvtQrh);
				return false;
			}
			if (string.IsNullOrEmpty(xmlString))
			{
				return false;
			}
			try
			{
				YpUrfBeSDaaGsjEkunxMdyRmAmKMA(SerializedObject.FromXml(GetType(), xmlString));
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error creating CalibrationMap from XML! " + ex.Message);
			}
			return false;
		}

		public bool ImportJsonString(string jsonString)
		{
			if (ReInput._id != IQTQeVqkCFnZnTkQnEauufWvtQrh)
			{
				ReInput.CheckInitialized(IQTQeVqkCFnZnTkQnEauufWvtQrh);
				return false;
			}
			if (string.IsNullOrEmpty(jsonString))
			{
				return false;
			}
			try
			{
				YpUrfBeSDaaGsjEkunxMdyRmAmKMA(SerializedObject.FromJson(GetType(), jsonString));
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error creating CalibrationMap from JSON! " + ex.Message);
			}
			return false;
		}

		private SerializedObject ItIKUftLxZlGRwRHOTKvJbbsTXmJ()
		{
			SerializedObject serializedObject = new SerializedObject(GetType(), SerializedObject.ObjectType.Object);
			serializedObject.Add("dataVersion", 4, SerializedObject.FieldOptions.ExculdeFromXml);
			serializedObject.xmlInfo = new SerializedObject.XmlInfo();
			serializedObject.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "dataVersion",
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = 4.ToString()
			});
			serializedObject.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				XCTmYYtBCCzTuZmlJPszaypSgJdS = "xmlns",
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "xsi",
				GpDRMyFZBJdjlvWjrvVYAzZnhbYW = null,
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = "http://www.w3.org/2001/XMLSchema-instance"
			});
			serializedObject.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				XCTmYYtBCCzTuZmlJPszaypSgJdS = "xsi",
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "schemaLocation",
				GpDRMyFZBJdjlvWjrvVYAzZnhbYW = null,
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = string.Format("{0} {1}{2}{3}{4}{5}", "http://guavaman.com/rewired", "http://guavaman.com/schemas/rewired/", "1.3", "/", GetType().Name, ".xsd")
			});
			List<object> list = new List<object>();
			serializedObject.Add("axes", list);
			int num = ((HXKrpesdiwufFXJMpINADFYSNfUk != null) ? HXKrpesdiwufFXJMpINADFYSNfUk.Length : 0);
			for (int i = 0; i < num; i++)
			{
				if (HXKrpesdiwufFXJMpINADFYSNfUk[i] != null)
				{
					list.Add(HXKrpesdiwufFXJMpINADFYSNfUk[i].ExportData());
				}
			}
			return serializedObject;
		}

		private void YpUrfBeSDaaGsjEkunxMdyRmAmKMA(SerializedObject P_0)
		{
			SerializedObject value = null;
			if (!P_0.TryGetDeserializedValueByRef("axes", ref value))
			{
				return;
			}
			int num = MathTools.Min(value.count, HXKrpesdiwufFXJMpINADFYSNfUk.Length);
			for (int i = 0; i < num; i++)
			{
				if (value[i].value is SerializedObject && HXKrpesdiwufFXJMpINADFYSNfUk[i] != null)
				{
					HXKrpesdiwufFXJMpINADFYSNfUk[i].Import((SerializedObject)value[i].value);
				}
			}
		}

		internal Vector2 GetCalibrated2DValue(int xAxisIndex, int yAxisIndex, float valueRawX, float valueRawY, DeadZone2DType deadZoneType, AxisSensitivity2DType sensitivityType)
		{
			return Axis2DCalibration.GetCalibrated2DValue(valueRawX, valueRawY, GetAxis(xAxisIndex), GetAxis(yAxisIndex), deadZoneType, sensitivityType);
		}
	}
}
