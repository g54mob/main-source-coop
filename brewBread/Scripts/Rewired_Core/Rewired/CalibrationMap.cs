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
		private AxisCalibration[] gmjcdmbtDXKatrtMKrVjuasYTyOh;

		private IList<AxisCalibration> WuBQpAtPZoiJZXzObfljBSXmjRMg;

		private readonly int QajDFFaomlkLHzaostfWYpGUioys;

		public IList<AxisCalibration> Axes => WuBQpAtPZoiJZXzObfljBSXmjRMg;

		public int axisCount
		{
			get
			{
				if (gmjcdmbtDXKatrtMKrVjuasYTyOh == null)
				{
					return 0;
				}
				return gmjcdmbtDXKatrtMKrVjuasYTyOh.Length;
			}
		}

		private CalibrationMap()
		{
			QajDFFaomlkLHzaostfWYpGUioys = ReInput.id;
		}

		internal CalibrationMap(AxisCalibrationData[] P_0)
			: this()
		{
			int num = ((P_0 != null) ? P_0.Length : 0);
			gmjcdmbtDXKatrtMKrVjuasYTyOh = new AxisCalibration[num];
			for (int i = 0; i < num; i++)
			{
				gmjcdmbtDXKatrtMKrVjuasYTyOh[i] = new AxisCalibration(P_0[i]);
			}
			WuBQpAtPZoiJZXzObfljBSXmjRMg = new ReadOnlyCollection<AxisCalibration>(gmjcdmbtDXKatrtMKrVjuasYTyOh);
		}

		public CalibrationMap(AxisCalibration[] P_0)
			: this()
		{
			gmjcdmbtDXKatrtMKrVjuasYTyOh = P_0;
			WuBQpAtPZoiJZXzObfljBSXmjRMg = new ReadOnlyCollection<AxisCalibration>(gmjcdmbtDXKatrtMKrVjuasYTyOh);
		}

		public void Reset()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return;
			}
			for (int i = 0; i < gmjcdmbtDXKatrtMKrVjuasYTyOh.Length; i++)
			{
				gmjcdmbtDXKatrtMKrVjuasYTyOh[i].Reset();
			}
		}

		public AxisCalibration GetAxis(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (index < 0 || index >= gmjcdmbtDXKatrtMKrVjuasYTyOh.Length)
			{
				return null;
			}
			return gmjcdmbtDXKatrtMKrVjuasYTyOh[index];
		}

		public float GetCalibratedValue(int axisIndex, float value)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			if (axisIndex < 0 || axisIndex >= gmjcdmbtDXKatrtMKrVjuasYTyOh.Length)
			{
				return value;
			}
			return gmjcdmbtDXKatrtMKrVjuasYTyOh[axisIndex].GetCalibratedValue(value);
		}

		public bool SetAxisData(int index, AxisCalibrationData data)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (index < 0 || index >= gmjcdmbtDXKatrtMKrVjuasYTyOh.Length)
			{
				return false;
			}
			gmjcdmbtDXKatrtMKrVjuasYTyOh[index].SetData(data);
			return true;
		}

		public AxisCalibrationData GetAxisData(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return default(AxisCalibrationData);
			}
			if (index < 0 || index >= gmjcdmbtDXKatrtMKrVjuasYTyOh.Length)
			{
				return default(AxisCalibrationData);
			}
			return gmjcdmbtDXKatrtMKrVjuasYTyOh[index].GetData();
		}

		internal void CopyFrom(CalibrationMap map, bool copyHardwareDeadzone)
		{
			if (map == null)
			{
				return;
			}
			if (map.gmjcdmbtDXKatrtMKrVjuasYTyOh.Length != gmjcdmbtDXKatrtMKrVjuasYTyOh.Length)
			{
				Logger.LogError("Calibration map data does not match the number of elements in the hardware!");
				return;
			}
			for (int i = 0; i < gmjcdmbtDXKatrtMKrVjuasYTyOh.Length; i++)
			{
				gmjcdmbtDXKatrtMKrVjuasYTyOh[i].CopyFrom(map.gmjcdmbtDXKatrtMKrVjuasYTyOh[i], copyHardwareDeadzone);
			}
		}

		public string ToXmlString()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return string.Empty;
			}
			string empty = string.Empty;
			try
			{
				return JxowpgGeLUBIhbyiSIIfFkkGllDuA().ToXmlString(writeDocumentTag: true);
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing CalibrationMap to XML! " + ex.Message);
				return empty;
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
				Logger.LogWarning("Error writing CalibrationMap to JSON! " + ex.Message);
			}
			return string.Empty;
		}

		public bool ImportXmlString(string xmlString)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (string.IsNullOrEmpty(xmlString))
			{
				return false;
			}
			try
			{
				ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject.FromXml(GetType(), xmlString));
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (string.IsNullOrEmpty(jsonString))
			{
				return false;
			}
			try
			{
				ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject.FromJson(GetType(), jsonString));
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error creating CalibrationMap from JSON! " + ex.Message);
			}
			return false;
		}

		private SerializedObject JxowpgGeLUBIhbyiSIIfFkkGllDuA()
		{
			SerializedObject serializedObject = new SerializedObject(GetType(), SerializedObject.ObjectType.Object);
			serializedObject.Add("dataVersion", 4, SerializedObject.FieldOptions.ExculdeFromXml);
			serializedObject.xmlInfo = new SerializedObject.XmlInfo();
			serializedObject.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "dataVersion",
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = 4.ToString()
			});
			serializedObject.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				yCwJMdebpVsoAZwRHxyMbDxHYvvT = "xmlns",
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "xsi",
				ZVXfpRlSPjKWRYaEYlEnqADXJiAs = null,
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = "http://www.w3.org/2001/XMLSchema-instance"
			});
			serializedObject.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				yCwJMdebpVsoAZwRHxyMbDxHYvvT = "xsi",
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "schemaLocation",
				ZVXfpRlSPjKWRYaEYlEnqADXJiAs = null,
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = string.Format("{0} {1}{2}{3}{4}{5}", "http://guavaman.com/rewired", "http://guavaman.com/schemas/rewired/", "1.3", "/", GetType().Name, ".xsd")
			});
			List<object> list = new List<object>();
			serializedObject.Add("axes", list);
			int num = ((gmjcdmbtDXKatrtMKrVjuasYTyOh != null) ? gmjcdmbtDXKatrtMKrVjuasYTyOh.Length : 0);
			for (int i = 0; i < num; i++)
			{
				if (gmjcdmbtDXKatrtMKrVjuasYTyOh[i] != null)
				{
					list.Add(gmjcdmbtDXKatrtMKrVjuasYTyOh[i].ExportData());
				}
			}
			return serializedObject;
		}

		private void ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject P_0)
		{
			SerializedObject value = null;
			if (!P_0.TryGetDeserializedValueByRef("axes", ref value))
			{
				return;
			}
			int num = MathTools.Min(value.count, gmjcdmbtDXKatrtMKrVjuasYTyOh.Length);
			for (int i = 0; i < num; i++)
			{
				if (value[i].value is SerializedObject && gmjcdmbtDXKatrtMKrVjuasYTyOh[i] != null)
				{
					gmjcdmbtDXKatrtMKrVjuasYTyOh[i].Import((SerializedObject)value[i].value);
				}
			}
		}

		internal Vector2 GetCalibrated2DValue(int xAxisIndex, int yAxisIndex, float valueRawX, float valueRawY, DeadZone2DType deadZoneType, AxisSensitivity2DType sensitivityType)
		{
			return Axis2DCalibration.GetCalibrated2DValue(valueRawX, valueRawY, GetAxis(xAxisIndex), GetAxis(yAxisIndex), deadZoneType, sensitivityType);
		}
	}
}
