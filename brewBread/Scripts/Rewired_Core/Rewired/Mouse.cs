using System;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

namespace Rewired
{
	public sealed class Mouse : ControllerWithAxes
	{
		private TimerAbs fLCCypuZqCpPtgbCnJCjRCIOLhzo;

		private float[] RppfAfhTSdIchQLnZNhktEnYcfuq;

		private Vector2 JHlQdvIDknwhbpDkONoyRzgqysDj;

		private Vector2 NNeHrvYGutiOIXhlhFaJfwMWTlqw;

		private int bwNXkWpZUjObQdsLBTnYRgEGhqar;

		private readonly IUnifiedMouseSource eyyVWBZaxsCJCQMmwhCWcagfYfzWA;

		private static Guid prVaMvCMokqAbWLeJcbpooSHlNBE;

		public Vector2 screenPosition
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Vector2.zero;
				}
				return JHlQdvIDknwhbpDkONoyRzgqysDj;
			}
		}

		public Vector2 screenPositionPrev
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Vector2.zero;
				}
				return NNeHrvYGutiOIXhlhFaJfwMWTlqw;
			}
		}

		public Vector2 screenPositionDelta
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Vector2.zero;
				}
				return JHlQdvIDknwhbpDkONoyRzgqysDj - NNeHrvYGutiOIXhlhFaJfwMWTlqw;
			}
		}

		public override Guid deviceInstanceGuid
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Guid.Empty;
				}
				return prVaMvCMokqAbWLeJcbpooSHlNBE;
			}
		}

		internal Mouse(string P_0, IUnifiedMouseSource P_1)
			: this(0, P_1.inputSource, P_0, InputTools.FormatHardwareIdentifierString(P_0), P_1.axisCount, P_1.buttonCount, P_1.hardwareMap, P_1?.controllerExtension, new ControllerDataUpdater(P_1.inputSource, P_1.axisCount, P_1.buttonCount, null))
		{
			eyyVWBZaxsCJCQMmwhCWcagfYfzWA = P_1;
			prVaMvCMokqAbWLeJcbpooSHlNBE = MiscTools.CreateGuidHashSHA1("[Universal Mouse]");
			VFZCTlETAOxSLbCyseetbWbCRTGUb();
		}

		private Mouse(int P_0, InputSource P_1, string P_2, string P_3, int P_4, int P_5, HardwareControllerMap_Game P_6, Extension P_7, ControllerDataUpdater P_8)
			: base(P_0, P_1, P_2, P_2, P_3, ControllerType.Mouse, Consts.hardwareTypeGuid_universalMouse, P_4, P_5, null, P_6, P_7, P_8)
		{
		}

		internal override void PwSVyxiNOmkyuuXUHWyuymUyabog(UpdateLoopType P_0)
		{
			eyyVWBZaxsCJCQMmwhCWcagfYfzWA.UpdateInputData(BkAqQtJmzNvLobJflzLPUhNYRphu);
			base.PwSVyxiNOmkyuuXUHWyuymUyabog(P_0);
			iKTboufbDeOsBaqAaWXIhLIsMqxu();
		}

		protected override bool IsPolledAxisActive(int index, out Pole pole, out int elementIdentifierId)
		{
			pole = Pole.Positive;
			elementIdentifierId = -1;
			if (RppfAfhTSdIchQLnZNhktEnYcfuq == null)
			{
				RppfAfhTSdIchQLnZNhktEnYcfuq = new float[_axisCount];
			}
			if (fLCCypuZqCpPtgbCnJCjRCIOLhzo == null)
			{
				fLCCypuZqCpPtgbCnJCjRCIOLhzo = new TimerAbs(1.0);
			}
			if (fLCCypuZqCpPtgbCnJCjRCIOLhzo.Update() || !fLCCypuZqCpPtgbCnJCjRCIOLhzo.running)
			{
				fLCCypuZqCpPtgbCnJCjRCIOLhzo.Start();
				Array.Clear(RppfAfhTSdIchQLnZNhktEnYcfuq, 0, RppfAfhTSdIchQLnZNhktEnYcfuq.Length);
			}
			if (ReInput.currentUpdateLoop == UpdateLoopType.OnGUI && !ReInput.configVars.GetPlatformVar_useNativeMouse())
			{
				RppfAfhTSdIchQLnZNhktEnYcfuq[index] += axes[index].valueRaw * 0.5f;
			}
			else
			{
				RppfAfhTSdIchQLnZNhktEnYcfuq[index] += axes[index].valueRaw;
			}
			float num = RppfAfhTSdIchQLnZNhktEnYcfuq[index];
			if (MathTools.Abs(num) <= axes[index].rtcgmrWiaobuHyScaIvcDhJnPmXHA)
			{
				return false;
			}
			pole = ((!(num >= 0f)) ? Pole.Negative : Pole.Positive);
			elementIdentifierId = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.axisElementIdentifierIds[index];
			if (elementIdentifierId < 0)
			{
				return false;
			}
			fLCCypuZqCpPtgbCnJCjRCIOLhzo.running = false;
			return true;
		}

		internal override void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			base.SPGTRPyvIslcMdbPTItsewSLRPxx();
			if (fLCCypuZqCpPtgbCnJCjRCIOLhzo != null)
			{
				fLCCypuZqCpPtgbCnJCjRCIOLhzo.Clear();
			}
			JHlQdvIDknwhbpDkONoyRzgqysDj = Vector2.zero;
			NNeHrvYGutiOIXhlhFaJfwMWTlqw = Vector2.zero;
		}

		internal override bool RQRRqZiAdoXIQMUtzTIWsndPGdwX(bool P_0)
		{
			if (!base.RQRRqZiAdoXIQMUtzTIWsndPGdwX(P_0))
			{
				return false;
			}
			if (eyyVWBZaxsCJCQMmwhCWcagfYfzWA is IGetSetEnabled)
			{
				(eyyVWBZaxsCJCQMmwhCWcagfYfzWA as IGetSetEnabled).enabled = P_0;
			}
			if (P_0)
			{
				iKTboufbDeOsBaqAaWXIhLIsMqxu();
				NNeHrvYGutiOIXhlhFaJfwMWTlqw = screenPosition;
			}
			return true;
		}

		private void iKTboufbDeOsBaqAaWXIhLIsMqxu()
		{
			int currentUnityFrame = ReInput.currentUnityFrame;
			if (currentUnityFrame != bwNXkWpZUjObQdsLBTnYRgEGhqar)
			{
				NNeHrvYGutiOIXhlhFaJfwMWTlqw = JHlQdvIDknwhbpDkONoyRzgqysDj;
				JHlQdvIDknwhbpDkONoyRzgqysDj = eyyVWBZaxsCJCQMmwhCWcagfYfzWA.mousePosition;
				bwNXkWpZUjObQdsLBTnYRgEGhqar = currentUnityFrame;
			}
		}
	}
}
