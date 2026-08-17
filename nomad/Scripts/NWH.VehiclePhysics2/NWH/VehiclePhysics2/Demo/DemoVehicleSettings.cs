using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NWH.Common.SceneManagement;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Powertrain.Wheel;
using UnityEngine;
using UnityEngine.UI;

namespace NWH.VehiclePhysics2.Demo
{
	public class DemoVehicleSettings : MonoBehaviour
	{
		public class Setting
		{
			public FieldInfo field;

			public object obj;

			public Text nameField;

			public Text valueField;

			public Button leftButton;

			public Button rightButton;

			public GameObject settingObject;

			public float min;

			public float max;

			public float step;
		}

		public List<Setting> settingList = new List<Setting>();

		public GameObject settingPrefab;

		private Font font;

		private void Start()
		{
			font = Resources.Load<Font>("NWH/Fonts/Inconsolata/Inconsolata-Regular");
			if (VehicleChanger.Instance == null)
			{
				Debug.LogError("DemoVehicleSettings requires VehicleChanger in the scene to work.");
				return;
			}
			VehicleChanger.Instance.onVehicleChanged.AddListener(HandleVehicleChange);
			Redraw();
		}

		private void HandleVehicleChange()
		{
			Clear();
			if (!(VehicleChanger.Instance == null) && !(Vehicle.ActiveVehicle == null))
			{
				Redraw();
			}
		}

		private void Update()
		{
			if (VehicleChanger.Instance == null || Vehicle.ActiveVehicle == null)
			{
				return;
			}
			foreach (Setting setting in settingList)
			{
				if (setting.valueField != null)
				{
					setting.valueField.text = setting.field.GetValue(setting.obj).ToString();
				}
			}
		}

		private void Redraw()
		{
			VehicleController vehicleController = Vehicle.ActiveVehicle as VehicleController;
			if (vehicleController == null)
			{
				return;
			}
			AddTitle("==== CONTROL ====");
			AddTitle("Steering");
			AddSettings(vehicleController.steering);
			AddTitle("Input");
			AddSettings(vehicleController.input);
			AddTitle("Brakes");
			AddSettings(vehicleController.brakes);
			AddTitle("==== POWERTRAIN ====");
			AddTitle("Engine");
			AddSettings(vehicleController.powertrain.engine);
			AddTitle("Engine.ForcedInduction");
			AddSettings(vehicleController.powertrain.engine.forcedInduction);
			AddTitle("Clutch");
			AddSettings(vehicleController.powertrain.clutch);
			AddTitle("Transmission");
			AddSettings(vehicleController.powertrain.transmission);
			AddTitle("Differentials");
			for (int i = 0; i < vehicleController.powertrain.differentials.Count; i++)
			{
				AddSettings(vehicleController.powertrain.differentials[i]);
			}
			for (int j = 0; j < vehicleController.powertrain.wheelGroups.Count; j++)
			{
				WheelGroup wheelGroup = vehicleController.powertrain.wheelGroups[j];
				AddTitle("Axle " + j);
				AddSettings(wheelGroup);
				if (wheelGroup.Wheels.Count == 1)
				{
					AddTitle("Wheel, Axle " + j, subtitle: true);
					WheelUAPI wheelUAPI = wheelGroup.Wheels[0].wheelUAPI;
					AddSettings(wheelUAPI);
				}
				else if (wheelGroup.Wheels.Count == 2)
				{
					AddTitle("Left Wheel, Axle " + j, subtitle: true);
					WheelUAPI wheelUAPI2 = wheelGroup.LeftWheel.wheelUAPI;
					AddSettings(wheelUAPI2);
					AddTitle("Right Wheel, Axle " + j, subtitle: true);
					WheelUAPI wheelUAPI3 = wheelGroup.RightWheel.wheelUAPI;
					AddSettings(wheelUAPI3);
				}
			}
			AddTitle("Steering");
			AddSettings(vehicleController.steering);
			AddTitle("Brakes");
			AddSettings(vehicleController.brakes);
			AddTitle("Modules:");
			for (int k = 0; k < vehicleController.moduleManager.Components.Count; k++)
			{
				AddTitle(vehicleController.moduleManager.Components[k].GetType().Name);
				AddSettings(vehicleController.moduleManager.Components[k]);
			}
		}

		public void Clear()
		{
			foreach (Setting setting in settingList)
			{
				UnityEngine.Object.Destroy(setting.settingObject);
			}
			settingList.Clear();
		}

		private void AddSettings(object obj)
		{
			FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.IsDefined(typeof(ShowInSettings), inherit: false))
				{
					AddSetting(fieldInfo, obj);
				}
			}
		}

		public void AddTitle(string text, bool subtitle = false)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = text;
			Text text2 = gameObject.AddComponent<Text>();
			text2.font = font;
			text2.text = text;
			text2.fontSize = 15;
			text2.alignment = TextAnchor.MiddleLeft;
			if (!subtitle)
			{
				text2.fontStyle = FontStyle.Bold;
			}
			gameObject.transform.SetParent(base.gameObject.transform, worldPositionStays: false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(260f, 25f);
			component.anchorMin = new Vector2(0f, 1f);
			component.anchorMax = new Vector2(0f, 1f);
			settingList.Add(new Setting
			{
				settingObject = gameObject
			});
		}

		public void AddSetting(FieldInfo field, object obj)
		{
			Setting setting = new Setting();
			setting.field = field;
			setting.obj = obj;
			setting.settingObject = UnityEngine.Object.Instantiate(settingPrefab, base.gameObject.transform, worldPositionStays: false);
			setting.settingObject.name = field.Name + "Setting";
			setting.nameField = setting.settingObject.transform.GetChild(1).GetComponent<Text>();
			setting.valueField = setting.settingObject.transform.GetChild(2).GetComponent<Text>();
			setting.leftButton = setting.settingObject.transform.GetChild(3).GetComponent<Button>();
			setting.rightButton = setting.settingObject.transform.GetChild(4).GetComponent<Button>();
			setting.nameField.text = field.Name;
			ShowInSettings showInSettings = field.GetCustomAttributes(typeof(ShowInSettings), inherit: false).Cast<ShowInSettings>().FirstOrDefault();
			if (showInSettings == null)
			{
				return;
			}
			if (showInSettings.name != null)
			{
				setting.nameField.text = showInSettings.name;
			}
			if (field.FieldType == typeof(float))
			{
				setting.valueField.text = ((float)field.GetValue(obj)).ToString("0.00");
				setting.min = showInSettings.min;
				setting.max = showInSettings.max;
				setting.step = showInSettings.step;
				setting.leftButton.onClick.AddListener(delegate
				{
					IncrementFloat(setting, increment: false);
				});
				setting.rightButton.onClick.AddListener(delegate
				{
					IncrementFloat(setting, increment: true);
				});
			}
			else if (field.FieldType == typeof(int))
			{
				setting.valueField.text = field.GetValue(obj).ToString();
				setting.min = (int)showInSettings.min;
				setting.max = (int)showInSettings.max;
				setting.step = (int)showInSettings.step;
				setting.leftButton.onClick.AddListener(delegate
				{
					IncrementInt(setting, increment: false);
				});
				setting.rightButton.onClick.AddListener(delegate
				{
					IncrementInt(setting, increment: true);
				});
			}
			else if (field.FieldType == typeof(bool))
			{
				setting.valueField.text = field.GetValue(obj).ToString();
				setting.leftButton.onClick.AddListener(delegate
				{
					ToggleBool(setting);
				});
				setting.rightButton.onClick.AddListener(delegate
				{
					ToggleBool(setting);
				});
			}
			else if (field.FieldType.IsEnum)
			{
				Type fieldType = field.FieldType;
				setting.min = 0f;
				setting.max = fieldType.GetFields(BindingFlags.Static | BindingFlags.Public).Length - 1;
				setting.step = 1f;
				setting.leftButton.onClick.AddListener(delegate
				{
					IncrementEnum(setting, increment: false);
				});
				setting.rightButton.onClick.AddListener(delegate
				{
					IncrementEnum(setting, increment: true);
				});
			}
			settingList.Add(setting);
		}

		public void IncrementEnum(Setting setting, bool increment)
		{
			float num = (int)setting.field.GetValue(setting.obj);
			num += (float)((int)setting.step * (increment ? 1 : (-1)));
			if (num < 0f)
			{
				num = (int)setting.max;
			}
			else if (num >= setting.max)
			{
				num = 0f;
			}
			object obj = num;
			obj = Enum.Parse(setting.field.FieldType, obj.ToString());
			setting.field.SetValue(setting.obj, obj);
		}

		public void ToggleBool(Setting setting)
		{
			bool flag = (bool)setting.field.GetValue(setting.obj);
			flag = !flag;
			setting.field.SetValue(setting.obj, flag);
		}

		public void IncrementFloat(Setting setting, bool increment)
		{
			float num = (float)setting.field.GetValue(setting.obj);
			num = Mathf.Clamp(num + setting.step * (increment ? 1f : (-1f)), setting.min, setting.max);
			setting.field.SetValue(setting.obj, num);
		}

		public void IncrementInt(Setting setting, bool increment)
		{
			float num = (int)setting.field.GetValue(setting.obj);
			num = Mathf.Clamp(num + (float)((int)setting.step * (increment ? 1 : (-1))), (int)setting.min, (int)setting.max);
			setting.field.SetValue(setting.obj, num);
		}
	}
}
