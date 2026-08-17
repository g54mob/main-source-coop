using System;
using NWH.Common.Input;
using UnityEngine;

namespace NWH.VehiclePhysics2.Input
{
	public class InputManagerVehicleInputProvider : VehicleInputProviderBase
	{
		[Tooltip("    Should mouse be used for input?")]
		public bool mouseInput;

		[NonSerialized]
		[Tooltip("Names of input bindings for each individual gears. If you need to add more gears modify this and the corresponding\r\niterator in the\r\nShiftInto() function.")]
		public string[] shiftInputNames = new string[11]
		{
			"ShiftIntoR1", "ShiftInto0", "ShiftInto1", "ShiftInto2", "ShiftInto3", "ShiftInto4", "ShiftInto5", "ShiftInto6", "ShiftInto7", "ShiftInto8",
			"ShiftInto9"
		};

		private string _tmpStr;

		public override float Steering()
		{
			if (!mouseInput)
			{
				return InputUtils.TryGetAxisRaw("Steering");
			}
			return Mathf.Clamp(GetMouseHorizontal(), -1f, 1f);
		}

		public override float Throttle()
		{
			if (!mouseInput)
			{
				return Mathf.Clamp01(InputUtils.TryGetAxisRaw("Throttle"));
			}
			return Mathf.Clamp(GetMouseVertical(), 0f, 1f);
		}

		public override float Brakes()
		{
			if (!mouseInput)
			{
				return Mathf.Clamp01(InputUtils.TryGetAxisRaw("Brakes"));
			}
			return 0f - Mathf.Clamp(GetMouseVertical(), -1f, 0f);
		}

		public override float Clutch()
		{
			return Mathf.Clamp01(InputUtils.TryGetAxis("Clutch"));
		}

		public override float Handbrake()
		{
			return Mathf.Clamp01(InputUtils.TryGetAxis("Handbrake"));
		}

		public override bool EngineStartStop()
		{
			return InputUtils.TryGetButtonDown("EngineStartStop", KeyCode.E);
		}

		public override bool ExtraLights()
		{
			return InputUtils.TryGetButtonDown("ExtraLights", KeyCode.Semicolon);
		}

		public override bool HighBeamLights()
		{
			return InputUtils.TryGetButtonDown("HighBeamLights", KeyCode.K);
		}

		public override bool HazardLights()
		{
			return InputUtils.TryGetButtonDown("HazardLights", KeyCode.J);
		}

		public override bool Horn()
		{
			return InputUtils.TryGetButton("Horn", KeyCode.H);
		}

		public override bool LeftBlinker()
		{
			return InputUtils.TryGetButtonDown("LeftBlinker", KeyCode.Z);
		}

		public override bool LowBeamLights()
		{
			return InputUtils.TryGetButtonDown("LowBeamLights", KeyCode.L);
		}

		public override bool RightBlinker()
		{
			return InputUtils.TryGetButtonDown("RightBlinker", KeyCode.X);
		}

		public override bool ShiftDown()
		{
			return InputUtils.TryGetButtonDown("ShiftDown", KeyCode.F);
		}

		public override int ShiftInto()
		{
			for (int i = -1; i < 9; i++)
			{
				if (InputUtils.TryGetButton(shiftInputNames[i + 1], KeyCode.Alpha0, showWarning: false))
				{
					return i;
				}
			}
			return -999;
		}

		public override bool ShiftUp()
		{
			return InputUtils.TryGetButtonDown("ShiftUp", KeyCode.R);
		}

		public override bool TrailerAttachDetach()
		{
			return InputUtils.TryGetButtonDown("TrailerAttachDetach", KeyCode.T);
		}

		public override bool FlipOver()
		{
			return InputUtils.TryGetButtonDown("FlipOver", KeyCode.M);
		}

		public override bool Boost()
		{
			return InputUtils.TryGetButton("Boost", KeyCode.LeftShift);
		}

		public override bool CruiseControl()
		{
			return InputUtils.TryGetButtonDown("CruiseControl", KeyCode.N);
		}

		private float GetMouseHorizontal()
		{
			float num = Mathf.Clamp(UnityEngine.Input.mousePosition.x / (float)Screen.width, -1f, 1f);
			if (num < 0.5f)
			{
				return (0f - (0.5f - num)) * 2f;
			}
			return (num - 0.5f) * 2f;
		}

		private float GetMouseVertical()
		{
			float num = Mathf.Clamp(UnityEngine.Input.mousePosition.y / (float)Screen.height, -1f, 1f);
			if (num < 0.5f)
			{
				return (0f - (0.5f - num)) * 2f;
			}
			return (num - 0.5f) * 2f;
		}
	}
}
