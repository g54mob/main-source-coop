using System;
using Rewired.Utils;

namespace Rewired
{
	public sealed class CustomController : ControllerWithAxes
	{
		private int iaffKOkGQHfQnQXgmjxXCjrvpuxu;

		private Func<int, float> LPxLRwuenJKjnaUGDRRTfKoDurIn;

		private Func<int, bool> rnFmbfHEOUgFMmBQOWoxCxIWMJZD;

		private bool yzcKsJMDeIRHgPciVraqzlyRLviv;

		private Guid guGxXkQdSCnhDedSVaPzhraGphmHA;

		public int sourceControllerId => iaffKOkGQHfQnQXgmjxXCjrvpuxu;

		public override Guid deviceInstanceGuid
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Guid.Empty;
				}
				return guGxXkQdSCnhDedSVaPzhraGphmHA;
			}
		}

		internal CustomController(LDGblTyVuNxcqdmvXxXJqAlXAiYR P_0)
			: this(P_0.vLaDHvfmijzLwCtDfpDRfeHfsWbqc, P_0.IkfUAiUlOToIXPDSQCjvytoLhPaV, P_0.AdvgSiYWsKyfjHPRtouNwdStlsJq, P_0.rmMLNGpqXVBSkkjZadentyrvdrgtA, P_0.rCJyseVRiKOjaaVRmjlxeXUUOXNJ, P_0.LIPsbVuyBvODEWtXBDzVIGmLLUAS, P_0.WncgGowlsDGmmJANqyvwLIbwHAtqA, P_0.yLLlShtixVkbYMaIfuUMijmLCboKA, P_0.tpoonPbJmajQyAErVLnAOoXhqZqEb, P_0.uAnjbTFgypgjVDFVAgoVQDncZkGd, null, new ControllerDataUpdater(P_0.rmMLNGpqXVBSkkjZadentyrvdrgtA, P_0.yLLlShtixVkbYMaIfuUMijmLCboKA, P_0.tpoonPbJmajQyAErVLnAOoXhqZqEb, null))
		{
		}

		private CustomController(int P_0, int P_1, Guid P_2, InputSource P_3, string P_4, string P_5, string P_6, int P_7, int P_8, HardwareControllerMap_Game P_9, Extension P_10, ControllerDataUpdater P_11)
			: base(P_0, P_3, P_4, P_5, P_6, ControllerType.Custom, P_2, P_7, P_8, null, P_9, P_10, P_11)
		{
			iaffKOkGQHfQnQXgmjxXCjrvpuxu = P_1;
			guGxXkQdSCnhDedSVaPzhraGphmHA = MiscTools.CreateGuidHashSHA1("CustomController device instance GUID: sourceId = " + iaffKOkGQHfQnQXgmjxXCjrvpuxu + ", controllerId = " + P_0);
			VFZCTlETAOxSLbCyseetbWbCRTGUb();
		}

		internal void pPRJisySYxBqxxleQgjLYfheiyTx()
		{
			if (!yzcKsJMDeIRHgPciVraqzlyRLviv)
			{
				return;
			}
			if (LPxLRwuenJKjnaUGDRRTfKoDurIn != null)
			{
				for (int i = 0; i < _axisCount; i++)
				{
					BkAqQtJmzNvLobJflzLPUhNYRphu.axisValues[i] = LPxLRwuenJKjnaUGDRRTfKoDurIn(i);
				}
			}
			if (rnFmbfHEOUgFMmBQOWoxCxIWMJZD != null)
			{
				for (int j = 0; j < _buttonCount; j++)
				{
					BkAqQtJmzNvLobJflzLPUhNYRphu.buttonValues[j] = rnFmbfHEOUgFMmBQOWoxCxIWMJZD(j);
				}
			}
		}

		public void SetAxisValue(int index, float value)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				if (index < 0 || index >= _axisCount)
				{
					Logger.LogWarning(index + " is not a valid Axis index.");
				}
				else
				{
					BkAqQtJmzNvLobJflzLPUhNYRphu.axisValues[index] = value;
				}
			}
		}

		public void SetAxisValue(string elementName, float value)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementName);
				if (axisIndex < 0 || axisIndex >= _axisCount)
				{
					Logger.LogWarning("\"" + axisIndex + "\" is not a valid Axis name.");
				}
				else
				{
					BkAqQtJmzNvLobJflzLPUhNYRphu.axisValues[axisIndex] = value;
				}
			}
		}

		public void SetAxisValueById(int elementId, float value)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementId);
				if (axisIndex < 0 || axisIndex >= _axisCount)
				{
					Logger.LogWarning(elementId + " is not a valid Axis id.");
				}
				else
				{
					BkAqQtJmzNvLobJflzLPUhNYRphu.axisValues[axisIndex] = value;
				}
			}
		}

		public void SetButtonValue(int index, bool value)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				if (index < 0 || index >= _buttonCount)
				{
					Logger.LogWarning(index + " is not a valid Button index.");
				}
				else
				{
					BkAqQtJmzNvLobJflzLPUhNYRphu.buttonValues[index] = value;
				}
			}
		}

		public void SetButtonValue(string elementName, bool value)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementName);
				if (buttonIndex < 0 || buttonIndex >= _buttonCount)
				{
					Logger.LogWarning("\"" + buttonIndex + "\" is not a valid Button name.");
				}
				else
				{
					BkAqQtJmzNvLobJflzLPUhNYRphu.buttonValues[buttonIndex] = value;
				}
			}
		}

		public void SetButtonValueById(int elementId, bool value)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementId);
				if (buttonIndex < 0 || buttonIndex >= _buttonCount)
				{
					Logger.LogWarning(elementId + " is not a valid Button id.");
				}
				else
				{
					BkAqQtJmzNvLobJflzLPUhNYRphu.buttonValues[buttonIndex] = value;
				}
			}
		}

		public void SetAxisUpdateCallback(Func<int, float> callback)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return;
			}
			LPxLRwuenJKjnaUGDRRTfKoDurIn = callback;
			if (!yzcKsJMDeIRHgPciVraqzlyRLviv)
			{
				yzcKsJMDeIRHgPciVraqzlyRLviv = true;
			}
		}

		public void SetButtonUpdateCallback(Func<int, bool> callback)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return;
			}
			rnFmbfHEOUgFMmBQOWoxCxIWMJZD = callback;
			if (!yzcKsJMDeIRHgPciVraqzlyRLviv)
			{
				yzcKsJMDeIRHgPciVraqzlyRLviv = true;
			}
		}

		public void ClearAxisValue(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				if (index < 0 || index >= _axisCount)
				{
					Logger.LogWarning(index + " is not a valid Axis index.");
					return;
				}
				float num = ((_calibrationMap != null) ? _calibrationMap.GetAxis(index).calibratedZero : 0f);
				BkAqQtJmzNvLobJflzLPUhNYRphu.axisValues[index] = num;
			}
		}

		public void ClearAxisValue(string elementName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementName);
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementId);
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				if (index < 0 || index >= _buttonCount)
				{
					Logger.LogWarning(index + " is not a valid Button index.");
					return;
				}
				BkAqQtJmzNvLobJflzLPUhNYRphu.buttonValues[index] = false;
				BkAqQtJmzNvLobJflzLPUhNYRphu.buttonPressureValues[index] = 0f;
			}
		}

		public void ClearButtonValue(string elementName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementName);
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
			}
			else if (base.enabled)
			{
				int buttonIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetButtonIndex(elementId);
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
