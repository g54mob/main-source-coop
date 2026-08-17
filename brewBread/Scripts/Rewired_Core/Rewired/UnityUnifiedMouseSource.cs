using System;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using UnityEngine;

namespace Rewired
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class UnityUnifiedMouseSource : IDisposable, IGetSetEnabled, IUnifiedMouseSource
	{
		private class puflpgkLMPMHzuiiJIwEuxMmWYNg
		{
			private float[] OLQIvANwxPxdLvhhJWLJIxgsFjwC;

			private bool[] cMJVUFvfQMWkvBtJpLdwMPYAPQtO;

			public puflpgkLMPMHzuiiJIwEuxMmWYNg(int P_0, int P_1)
			{
				cMJVUFvfQMWkvBtJpLdwMPYAPQtO = new bool[P_0];
				OLQIvANwxPxdLvhhJWLJIxgsFjwC = new float[P_1];
			}

			public void sIVOHhfJwwEIlNaMuLHeQvgSxJje(bool[] P_0, float[] P_1)
			{
				Array.Copy(P_0, cMJVUFvfQMWkvBtJpLdwMPYAPQtO, P_0.Length);
				for (int i = 0; i < OLQIvANwxPxdLvhhJWLJIxgsFjwC.Length; i++)
				{
					OLQIvANwxPxdLvhhJWLJIxgsFjwC[i] += P_1[i];
				}
			}

			public void sqKPSSlsVXhtJbtQVKkAMGtItVvG(ControllerDataUpdater P_0)
			{
				Array.Copy(OLQIvANwxPxdLvhhJWLJIxgsFjwC, P_0.axisValues, OLQIvANwxPxdLvhhJWLJIxgsFjwC.Length);
				Array.Copy(cMJVUFvfQMWkvBtJpLdwMPYAPQtO, P_0.buttonValues, cMJVUFvfQMWkvBtJpLdwMPYAPQtO.Length);
			}

			public void SPGTRPyvIslcMdbPTItsewSLRPxx()
			{
				Array.Clear(OLQIvANwxPxdLvhhJWLJIxgsFjwC, 0, OLQIvANwxPxdLvhhJWLJIxgsFjwC.Length);
				Array.Clear(cMJVUFvfQMWkvBtJpLdwMPYAPQtO, 0, cMJVUFvfQMWkvBtJpLdwMPYAPQtO.Length);
			}

			public void TRdGVpAaqUDZDhNKIlCydndygteTA()
			{
				Array.Clear(OLQIvANwxPxdLvhhJWLJIxgsFjwC, 0, OLQIvANwxPxdLvhhJWLJIxgsFjwC.Length);
			}
		}

		[Serializable]
		private sealed class bISaAuJrMgGmwLJaQqtMKLqAIFQHb
		{
			public static readonly bISaAuJrMgGmwLJaQqtMKLqAIFQHb _003C_003E9 = new bISaAuJrMgGmwLJaQqtMKLqAIFQHb();

			public static Func<puflpgkLMPMHzuiiJIwEuxMmWYNg> _003C_003E9__20_0;

			internal puflpgkLMPMHzuiiJIwEuxMmWYNg KjKfUoBCnmSNuRHUMefqIBVKYwJoB()
			{
				return new puflpgkLMPMHzuiiJIwEuxMmWYNg(7, 4);
			}
		}

		private static HardwareControllerMap_Game NadPxXYRifMljEYjJuhjIbsPVNdr;

		private UpdateLoopDataSet<puflpgkLMPMHzuiiJIwEuxMmWYNg> TIBkwaYFfrlorrgEzKrMTlQMftSC;

		private float[] OLQIvANwxPxdLvhhJWLJIxgsFjwC;

		private bool[] cMJVUFvfQMWkvBtJpLdwMPYAPQtO;

		private bool kKFZZElqKQSUFMZnWdEudRvTJGpo;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public bool enabled
		{
			get
			{
				return kKFZZElqKQSUFMZnWdEudRvTJGpo;
			}
			set
			{
				if (kKFZZElqKQSUFMZnWdEudRvTJGpo != value)
				{
					kKFZZElqKQSUFMZnWdEudRvTJGpo = value;
					Clear();
					ThreadSafeUnityInput.mouse.Monitor(value);
				}
			}
		}

		public InputSource inputSource => InputSource.UnityKeyboardAndMouse;

		public HardwareControllerMap_Game hardwareMap
		{
			get
			{
				if (NadPxXYRifMljEYjJuhjIbsPVNdr == null)
				{
					NadPxXYRifMljEYjJuhjIbsPVNdr = HStMDbOieMaNNxlRQxuGsFEKKaRE();
				}
				return NadPxXYRifMljEYjJuhjIbsPVNdr;
			}
		}

		public int buttonCount => 7;

		public int axisCount => 4;

		public Vector2 mousePosition
		{
			get
			{
				if (!kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					return default(Vector2);
				}
				return ThreadSafeUnityInput.mouse.mousePosition;
			}
		}

		public Controller.Extension controllerExtension => null;

		public UnityUnifiedMouseSource()
		{
			TIBkwaYFfrlorrgEzKrMTlQMftSC = new UpdateLoopDataSet<puflpgkLMPMHzuiiJIwEuxMmWYNg>(ReInput.configVars.updateLoop, bISaAuJrMgGmwLJaQqtMKLqAIFQHb._003C_003E9.KjKfUoBCnmSNuRHUMefqIBVKYwJoB);
			OLQIvANwxPxdLvhhJWLJIxgsFjwC = new float[4];
			cMJVUFvfQMWkvBtJpLdwMPYAPQtO = new bool[7];
			enabled = true;
			ReInput.UpdateEndedEvent += HPfKmnGbWiEcZsNjewIkeEUMGOZFA;
			ReInput.EarlyUpdateEvent += BfPeRRMWMwfjUKkQCFCsSdeTWPdBA;
		}

		public void UpdateInputData(ControllerDataUpdater dataUpdater)
		{
			TIBkwaYFfrlorrgEzKrMTlQMftSC.Get(ReInput.currentUpdateLoop).sqKPSSlsVXhtJbtQVKkAMGtItVvG(dataUpdater);
		}

		public void Clear()
		{
			int count = TIBkwaYFfrlorrgEzKrMTlQMftSC.Count;
			for (int i = 0; i < count; i++)
			{
				TIBkwaYFfrlorrgEzKrMTlQMftSC.Get(i).SPGTRPyvIslcMdbPTItsewSLRPxx();
			}
		}

		private void BfPeRRMWMwfjUKkQCFCsSdeTWPdBA()
		{
			if (kKFZZElqKQSUFMZnWdEudRvTJGpo)
			{
				ThreadSafeUnityInput.mouse.GetAxisRawValues(OLQIvANwxPxdLvhhJWLJIxgsFjwC);
				ThreadSafeUnityInput.mouse.GetButtonValues(cMJVUFvfQMWkvBtJpLdwMPYAPQtO);
				int count = TIBkwaYFfrlorrgEzKrMTlQMftSC.Count;
				for (int i = 0; i < count; i++)
				{
					TIBkwaYFfrlorrgEzKrMTlQMftSC.Get(i).sIVOHhfJwwEIlNaMuLHeQvgSxJje(cMJVUFvfQMWkvBtJpLdwMPYAPQtO, OLQIvANwxPxdLvhhJWLJIxgsFjwC);
				}
			}
		}

		private void HPfKmnGbWiEcZsNjewIkeEUMGOZFA(UpdateLoopType P_0)
		{
			TIBkwaYFfrlorrgEzKrMTlQMftSC.Get(P_0).TRdGVpAaqUDZDhNKIlCydndygteTA();
		}

		private static HardwareControllerMap_Game HStMDbOieMaNNxlRQxuGsFEKKaRE()
		{
			ControllerElementIdentifier[] array = new ControllerElementIdentifier[Consts.unityUnifiedMouseElementIdentifiers.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ControllerElementIdentifier(Consts.unityUnifiedMouseElementIdentifiers[i]);
			}
			int[] array2 = new int[7];
			int[] array3 = new int[4];
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].elementType == ControllerElementType.Axis)
				{
					array3[num2++] = array[j].id;
				}
				else if (array[j].elementType == ControllerElementType.Button)
				{
					array2[num++] = array[j].id;
				}
			}
			AxisCalibrationData[] array4 = new AxisCalibrationData[4];
			AxisRange[] array5 = new AxisRange[4];
			HardwareAxisInfo[] array6 = new HardwareAxisInfo[4];
			HardwareButtonInfo[] array7 = new HardwareButtonInfo[7];
			for (int k = 0; k < 4; k++)
			{
				array4[k] = AxisCalibrationData.Raw;
				array5[k] = AxisRange.Full;
				float num3 = (((uint)k > 1u) ? 2f : 100f);
				array6[k] = new HardwareAxisInfo(AxisCoordinateMode.Relative, false, num3, SpecialAxisType.None);
			}
			for (int l = 0; l < 7; l++)
			{
				array7[l] = new HardwareButtonInfo();
			}
			return new HardwareControllerMap_Game("Mouse", default(HardwareControllerMapIdentifier), array, array2, array3, array4, array5, array6, array7, null);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~UnityUnifiedMouseSource()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				return;
			}
			if (disposing)
			{
				if (kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					ThreadSafeUnityInput.mouse.Monitor(state: false);
				}
				ReInput.UpdateEndedEvent -= HPfKmnGbWiEcZsNjewIkeEUMGOZFA;
				ReInput.EarlyUpdateEvent -= BfPeRRMWMwfjUKkQCFCsSdeTWPdBA;
			}
			AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
		}

		public static ControllerElementType GetHardwareElementType(int elementIdentifierId)
		{
			if (NadPxXYRifMljEYjJuhjIbsPVNdr == null)
			{
				NadPxXYRifMljEYjJuhjIbsPVNdr = HStMDbOieMaNNxlRQxuGsFEKKaRE();
			}
			return NadPxXYRifMljEYjJuhjIbsPVNdr.GetElementType(elementIdentifierId);
		}
	}
}
