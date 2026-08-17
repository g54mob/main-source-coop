using System;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

namespace Rewired
{
	public sealed class Mouse : ControllerWithAxes
	{
		private TimerAbs FyRwofZXROkkjBXpfPAFaCYzyLIl;

		private float[] mGRSYLSKNeqoAZYByVhIwCcRrABj;

		private Vector2 wOitJmZBTCrKClApXQGplRYLlbXq;

		private Vector2 ieGidtslHeiVdBvoGXtFgLARCJIUA;

		private int EYBDxRBZXjIzKmlrpabbLHcXsDLIA;

		private readonly IUnifiedMouseSource NqruDerxeNWbowSYjlRIINrcrJku;

		private static Guid oHaKRgsuZsvpTLMRpqdtbBZbxHok;

		public Vector2 screenPosition
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return Vector2.zero;
				}
				return wOitJmZBTCrKClApXQGplRYLlbXq;
			}
		}

		public Vector2 screenPositionPrev
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return Vector2.zero;
				}
				return ieGidtslHeiVdBvoGXtFgLARCJIUA;
			}
		}

		public Vector2 screenPositionDelta
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return Vector2.zero;
				}
				return wOitJmZBTCrKClApXQGplRYLlbXq - ieGidtslHeiVdBvoGXtFgLARCJIUA;
			}
		}

		Guid Controller.deviceInstanceGuid
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return Guid.Empty;
				}
				return oHaKRgsuZsvpTLMRpqdtbBZbxHok;
			}
		}

		internal Mouse(string P_0, IUnifiedMouseSource P_1)
			: this(0, P_1.inputSource, P_0, InputTools.FormatHardwareIdentifierString(P_0), P_1.axisCount, P_1.buttonCount, P_1.hardwareMap, P_1?.controllerExtension, new ControllerDataUpdater(P_1.inputSource, P_1.axisCount, P_1.buttonCount, null))
		{
			NqruDerxeNWbowSYjlRIINrcrJku = P_1;
			oHaKRgsuZsvpTLMRpqdtbBZbxHok = MiscTools.CreateGuidHashSHA1("[Universal Mouse]");
			sXPBxAVgVVidzfPmKZUCZYhRwaIf();
		}

		private Mouse(int P_0, InputSource P_1, string P_2, string P_3, int P_4, int P_5, HardwareControllerMap_Game P_6, Extension P_7, ControllerDataUpdater P_8)
			: base(P_0, P_1, P_2, P_2, P_3, ControllerType.Mouse, Consts.hardwareTypeGuid_universalMouse, P_4, P_5, null, P_6, P_7, P_8)
		{
		}

		internal void iMnfMLkKvbmnYgWKbtvUSCHaOtXrA(UpdateLoopType P_0)
		{
			NqruDerxeNWbowSYjlRIINrcrJku.UpdateInputData(yZwGORAVRJPjNCmxxWIIoQgNomuqA);
			ZHABOdGNpKMPRYvZWajChRBuWCjCA(P_0);
			tPWmPLkJUxphBjwUZQuDmEUzJJrv();
		}

		protected override bool IsPolledAxisActive(int index, out Pole pole, out int elementIdentifierId)
		{
			pole = Pole.Positive;
			elementIdentifierId = -1;
			if (mGRSYLSKNeqoAZYByVhIwCcRrABj == null)
			{
				mGRSYLSKNeqoAZYByVhIwCcRrABj = new float[_axisCount];
			}
			if (FyRwofZXROkkjBXpfPAFaCYzyLIl == null)
			{
				FyRwofZXROkkjBXpfPAFaCYzyLIl = new TimerAbs(1.0);
			}
			if (FyRwofZXROkkjBXpfPAFaCYzyLIl.Update() || !FyRwofZXROkkjBXpfPAFaCYzyLIl.running)
			{
				FyRwofZXROkkjBXpfPAFaCYzyLIl.Start();
				Array.Clear(mGRSYLSKNeqoAZYByVhIwCcRrABj, 0, mGRSYLSKNeqoAZYByVhIwCcRrABj.Length);
			}
			if (ReInput.currentUpdateLoop == UpdateLoopType.OnGUI && !ReInput.configVars.GetPlatformVar_useNativeMouse())
			{
				mGRSYLSKNeqoAZYByVhIwCcRrABj[index] += axes[index].valueRaw * 0.5f;
			}
			else
			{
				mGRSYLSKNeqoAZYByVhIwCcRrABj[index] += axes[index].valueRaw;
			}
			float num = mGRSYLSKNeqoAZYByVhIwCcRrABj[index];
			if (MathTools.Abs(num) <= axes[index].ZmUbemxBLMMLeVyysMXgQzhrblwr)
			{
				return false;
			}
			pole = ((!(num >= 0f)) ? Pole.Negative : Pole.Positive);
			elementIdentifierId = UNRIOyvPojfCPrjRsEYcHBwwkZqS.axisElementIdentifierIds[index];
			if (elementIdentifierId < 0)
			{
				return false;
			}
			FyRwofZXROkkjBXpfPAFaCYzyLIl.running = false;
			return true;
		}

		internal void ZRAAJMgcnxWyuoDtYxVgelJQOFAA()
		{
			hlKcVTBoQXLWkqiUgXEameLeIdsf();
			if (FyRwofZXROkkjBXpfPAFaCYzyLIl != null)
			{
				FyRwofZXROkkjBXpfPAFaCYzyLIl.Clear();
			}
			wOitJmZBTCrKClApXQGplRYLlbXq = Vector2.zero;
			ieGidtslHeiVdBvoGXtFgLARCJIUA = Vector2.zero;
		}

		internal bool vsalBsIeHUgzWUzqImJKjoPNmZfg(bool P_0)
		{
			if (!base.YMYaXjiCPrJkpmbpKNcXpVseIAcFA(P_0))
			{
				return false;
			}
			if (NqruDerxeNWbowSYjlRIINrcrJku is IGetSetEnabled)
			{
				(NqruDerxeNWbowSYjlRIINrcrJku as IGetSetEnabled).enabled = P_0;
			}
			if (P_0)
			{
				tPWmPLkJUxphBjwUZQuDmEUzJJrv();
				ieGidtslHeiVdBvoGXtFgLARCJIUA = screenPosition;
			}
			return true;
		}

		private void tPWmPLkJUxphBjwUZQuDmEUzJJrv()
		{
			int currentUnityFrame = ReInput.currentUnityFrame;
			if (currentUnityFrame != EYBDxRBZXjIzKmlrpabbLHcXsDLIA)
			{
				ieGidtslHeiVdBvoGXtFgLARCJIUA = wOitJmZBTCrKClApXQGplRYLlbXq;
				wOitJmZBTCrKClApXQGplRYLlbXq = NqruDerxeNWbowSYjlRIINrcrJku.mousePosition;
				EYBDxRBZXjIzKmlrpabbLHcXsDLIA = currentUnityFrame;
			}
		}
	}
}
