using System;
using Rewired.Utils;

namespace Rewired
{
	public sealed class CustomController : ControllerWithAxes
	{
		private int yoavhWwtGtfjBpHJvgqhDqXhOEerA;

		private Func<int, float> zcSXAyMHqAsNagYRzwLujGrKPxZK;

		private Func<int, bool> yuBSkafLoamnrrYjlqakRTHtEmIw;

		private bool tqlumXaHXlBZvgjKpgVlCSTAbJlS;

		private Guid uTTNPBOMmCxwEwLNggfbtfwTtFvg;

		public int sourceControllerId => yoavhWwtGtfjBpHJvgqhDqXhOEerA;

		Guid Controller.deviceInstanceGuid
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return Guid.Empty;
				}
				return uTTNPBOMmCxwEwLNggfbtfwTtFvg;
			}
		}

		internal CustomController(fwgKdBUKTlRPdLgAbAbZZfczAgRhA P_0)
			: this(P_0.xeXxvGYvjFirovAVsEbKHSBiTfze, P_0.drwTiMlBCJtrHnnBOjUoUmEsSCIc, P_0.SLSuArTGSHNkNzkmWeYRkDMdTXBk, P_0.mqFrAXIklmBKtLvNbaeTzicAlAZS, P_0.DWKTTUsUiQIxbXclGunmDxAsRdJn, P_0.vBFQqEGJmQCmxRemiFRJGbhmcLjX, P_0.lZKPTLcOrnPBoHHSBfJsQQAfWYeV, P_0.huYUOMQqeSPrNGNymoPdGbxrIQqq, P_0.SrHkRIbnCJqDjhkAMIJyjEDwzLCtA, P_0.WuHueMVJjpDMMEfXPbHTeCKBXytE, null, new ControllerDataUpdater(P_0.mqFrAXIklmBKtLvNbaeTzicAlAZS, P_0.huYUOMQqeSPrNGNymoPdGbxrIQqq, P_0.SrHkRIbnCJqDjhkAMIJyjEDwzLCtA, null))
		{
		}

		private CustomController(int P_0, int P_1, Guid P_2, InputSource P_3, string P_4, string P_5, string P_6, int P_7, int P_8, HardwareControllerMap_Game P_9, Extension P_10, ControllerDataUpdater P_11)
			: base(P_0, P_3, P_4, P_5, P_6, ControllerType.Custom, P_2, P_7, P_8, null, P_9, P_10, P_11)
		{
			yoavhWwtGtfjBpHJvgqhDqXhOEerA = P_1;
			uTTNPBOMmCxwEwLNggfbtfwTtFvg = MiscTools.CreateGuidHashSHA1("CustomController device instance GUID: sourceId = " + yoavhWwtGtfjBpHJvgqhDqXhOEerA + ", controllerId = " + P_0);
			sXPBxAVgVVidzfPmKZUCZYhRwaIf();
		}

		internal void mDaReUmdzWmtGgOUJOzXLErqrwKK()
		{
			if (!tqlumXaHXlBZvgjKpgVlCSTAbJlS)
			{
				return;
			}
			if (zcSXAyMHqAsNagYRzwLujGrKPxZK != null)
			{
				for (int i = 0; i < _axisCount; i++)
				{
					yZwGORAVRJPjNCmxxWIIoQgNomuqA.axisValues[i] = zcSXAyMHqAsNagYRzwLujGrKPxZK(i);
				}
			}
			if (yuBSkafLoamnrrYjlqakRTHtEmIw != null)
			{
				for (int j = 0; j < _buttonCount; j++)
				{
					yZwGORAVRJPjNCmxxWIIoQgNomuqA.buttonValues[j] = yuBSkafLoamnrrYjlqakRTHtEmIw(j);
				}
			}
		}

		public void SetAxisValue(int index, float value)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				if (index < 0 || index >= _axisCount)
				{
					Logger.LogWarning(index + " is not a valid Axis index.");
				}
				else
				{
					yZwGORAVRJPjNCmxxWIIoQgNomuqA.axisValues[index] = value;
				}
			}
		}

		public void SetAxisValue(string elementName, float value)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementName);
				if (axisIndex < 0 || axisIndex >= _axisCount)
				{
					Logger.LogWarning("\"" + axisIndex + "\" is not a valid Axis name.");
				}
				else
				{
					yZwGORAVRJPjNCmxxWIIoQgNomuqA.axisValues[axisIndex] = value;
				}
			}
		}

		public void SetAxisValueById(int elementId, float value)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementId);
				if (axisIndex < 0 || axisIndex >= _axisCount)
				{
					Logger.LogWarning(elementId + " is not a valid Axis id.");
				}
				else
				{
					yZwGORAVRJPjNCmxxWIIoQgNomuqA.axisValues[axisIndex] = value;
				}
			}
		}

		public void SetButtonValue(int index, bool value)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				if (index < 0 || index >= _buttonCount)
				{
					Logger.LogWarning(index + " is not a valid Button index.");
				}
				else
				{
					yZwGORAVRJPjNCmxxWIIoQgNomuqA.buttonValues[index] = value;
				}
			}
		}

		public void SetButtonValue(string elementName, bool value)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementName);
				if (buttonIndex < 0 || buttonIndex >= _buttonCount)
				{
					Logger.LogWarning("\"" + buttonIndex + "\" is not a valid Button name.");
				}
				else
				{
					yZwGORAVRJPjNCmxxWIIoQgNomuqA.buttonValues[buttonIndex] = value;
				}
			}
		}

		public void SetButtonValueById(int elementId, bool value)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementId);
				if (buttonIndex < 0 || buttonIndex >= _buttonCount)
				{
					Logger.LogWarning(elementId + " is not a valid Button id.");
				}
				else
				{
					yZwGORAVRJPjNCmxxWIIoQgNomuqA.buttonValues[buttonIndex] = value;
				}
			}
		}

		public void SetAxisUpdateCallback(Func<int, float> callback)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return;
			}
			zcSXAyMHqAsNagYRzwLujGrKPxZK = callback;
			if (!tqlumXaHXlBZvgjKpgVlCSTAbJlS)
			{
				tqlumXaHXlBZvgjKpgVlCSTAbJlS = true;
			}
		}

		public void SetButtonUpdateCallback(Func<int, bool> callback)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return;
			}
			yuBSkafLoamnrrYjlqakRTHtEmIw = callback;
			if (!tqlumXaHXlBZvgjKpgVlCSTAbJlS)
			{
				tqlumXaHXlBZvgjKpgVlCSTAbJlS = true;
			}
		}

		public void ClearAxisValue(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				if (index < 0 || index >= _axisCount)
				{
					Logger.LogWarning(index + " is not a valid Axis index.");
					return;
				}
				float num = ((_calibrationMap != null) ? _calibrationMap.GetAxis(index).calibratedZero : 0f);
				yZwGORAVRJPjNCmxxWIIoQgNomuqA.axisValues[index] = num;
			}
		}

		public void ClearAxisValue(string elementName)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementName);
				if (axisIndex < 0 || axisIndex >= _axisCount)
				{
					Logger.LogWarning("\"" + axisIndex + "\" is not a valid Axis name.");
				}
				else
				{
					ClearAxisValue(axisIndex);
				}
			}
		}

		public void ClearAxisValueById(int elementId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementId);
				if (axisIndex < 0 || axisIndex >= _axisCount)
				{
					Logger.LogWarning(elementId + " is not a valid Axis id.");
				}
				else
				{
					ClearAxisValue(axisIndex);
				}
			}
		}

		public void ClearButtonValue(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				if (index < 0 || index >= _buttonCount)
				{
					Logger.LogWarning(index + " is not a valid Button index.");
					return;
				}
				yZwGORAVRJPjNCmxxWIIoQgNomuqA.buttonValues[index] = false;
				yZwGORAVRJPjNCmxxWIIoQgNomuqA.buttonPressureValues[index] = 0f;
			}
		}

		public void ClearButtonValue(string elementName)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementName);
				if (buttonIndex < 0 || buttonIndex >= _buttonCount)
				{
					Logger.LogWarning("\"" + buttonIndex + "\" is not a valid Button name.");
				}
				else
				{
					ClearButtonValue(buttonIndex);
				}
			}
		}

		public void ClearButtonValueById(int elementId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
			}
			else if (base.enabled)
			{
				int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementId);
				if (buttonIndex < 0 || buttonIndex >= _buttonCount)
				{
					Logger.LogWarning(elementId + " is not a valid Button id.");
				}
				else
				{
					ClearButtonValue(buttonIndex);
				}
			}
		}
	}
}
