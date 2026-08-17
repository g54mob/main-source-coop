using System.Reflection;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Powertrain.Wheel;
using UnityEngine;
using UnityEngine.UI;

namespace NWH.VehiclePhysics2.Demo
{
	public class DemoTelemetry : MonoBehaviour
	{
		public Text textUI;

		private string text;

		private VehicleController vc;

		private void LateUpdate()
		{
			if (Time.frameCount % 5 != 0)
			{
				return;
			}
			vc = Vehicle.ActiveVehicle as VehicleController;
			if (vc == null)
			{
				return;
			}
			AddTitle("Vehicle");
			PrintProperties(vc);
			AddTitle("Steering");
			PrintProperties(vc.steering);
			AddTitle("Engine");
			PrintProperties(vc.powertrain.engine);
			AddTitle("Forced Induction", ' ');
			PrintProperties(vc.powertrain.engine.forcedInduction);
			AddSpace();
			AddTitle("Clutch");
			PrintProperties(vc.powertrain.clutch);
			AddSpace();
			AddTitle("Transmission");
			PrintProperties(vc.powertrain.transmission);
			AddSpace();
			AddTitle("Axles");
			int num = 0;
			foreach (WheelGroup wheelGroup in vc.powertrain.wheelGroups)
			{
				AddTitle("Axle " + num);
				PrintProperties(wheelGroup);
				AddTitle("Left Wheel", ' ');
				PrintProperties(wheelGroup.LeftWheel);
				PrintProperties(wheelGroup.LeftWheel.wheelUAPI);
				AddTitle("Right Wheel:", ' ');
				PrintProperties(wheelGroup.RightWheel);
				PrintProperties(wheelGroup.RightWheel.wheelUAPI);
				num++;
			}
			textUI.text = text;
			text = "";
		}

		private void PrintProperties(object obj, string prefix = "")
		{
			FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!fieldInfo.IsDefined(typeof(ShowInTelemetry), inherit: false))
				{
					continue;
				}
				if (fieldInfo.FieldType == typeof(float))
				{
					string value = ((float)fieldInfo.GetValue(obj)).ToString("0.00");
					AddLine(prefix + fieldInfo.Name, value);
					continue;
				}
				try
				{
					AddLine(prefix + fieldInfo.Name, fieldInfo.GetValue(obj).ToString());
				}
				catch
				{
				}
			}
			PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (!propertyInfo.IsDefined(typeof(ShowInTelemetry), inherit: false))
				{
					continue;
				}
				if (propertyInfo.PropertyType == typeof(float))
				{
					string value2 = ((float)propertyInfo.GetValue(obj, null)).ToString("0.00");
					AddLine(propertyInfo.Name, value2);
					continue;
				}
				try
				{
					AddLine(propertyInfo.Name, propertyInfo.GetValue(obj, null).ToString());
				}
				catch
				{
				}
			}
		}

		private void AddLine(string name, string value = "")
		{
			name = Truncate(name, 23);
			text = text + $"{ChangeCase(name),-26}{value,14}" + "\n";
		}

		private void AddLine(string name, float value)
		{
			string value2 = value.ToString("0.0");
			AddLine(name, value2);
		}

		private void AddTitle(string title, char filler = '_')
		{
			text = text + "\n" + CenterString(title, 40, filler);
		}

		private void AddSpace()
		{
			text += "\n";
		}

		private string CenterString(string stringToCenter, int totalLength, char filler)
		{
			return stringToCenter.PadLeft((totalLength - stringToCenter.Length) / 2 + stringToCenter.Length, filler).PadRight(totalLength, filler) + "\n";
		}

		public string Truncate(string value, int maxChars)
		{
			if (value.Length > maxChars)
			{
				return value.Substring(0, maxChars) + "..";
			}
			return value;
		}

		public static string ChangeCase(string str)
		{
			if (str == null)
			{
				return null;
			}
			if (str.Length > 1)
			{
				return char.ToUpper(str[0]) + str.Substring(1);
			}
			return str.ToUpper();
		}
	}
}
