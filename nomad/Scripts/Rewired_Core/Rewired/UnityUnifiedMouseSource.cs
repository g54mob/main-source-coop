using System;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using UnityEngine;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class UnityUnifiedMouseSource : IUnifiedMouseSource, IGetSetEnabled, IDisposable
	{
		private class DcNEiwAkjtcgoAfNxIRUZZBYWiIF
		{
			private float[] osMBiuXGbAbBmDuSOHFsMKiZbMdb;

			private bool[] CAzMEBskvLdFPOlKvWIuGBRCzjHD;

			public DcNEiwAkjtcgoAfNxIRUZZBYWiIF(int P_0, int P_1)
			{
				CAzMEBskvLdFPOlKvWIuGBRCzjHD = new bool[P_0];
				osMBiuXGbAbBmDuSOHFsMKiZbMdb = new float[P_1];
			}

			public void pYAjozglrWCsaqhDqGFTGbRykOTD(bool[] P_0, float[] P_1)
			{
				Array.Copy(P_0, CAzMEBskvLdFPOlKvWIuGBRCzjHD, P_0.Length);
				for (int i = 0; i < osMBiuXGbAbBmDuSOHFsMKiZbMdb.Length; i++)
				{
					osMBiuXGbAbBmDuSOHFsMKiZbMdb[i] += P_1[i];
				}
			}

			public void YJKbbNbRLdpdAqDgtQHLGoZKVeMb(ControllerDataUpdater P_0)
			{
				Array.Copy(osMBiuXGbAbBmDuSOHFsMKiZbMdb, P_0.axisValues, osMBiuXGbAbBmDuSOHFsMKiZbMdb.Length);
				Array.Copy(CAzMEBskvLdFPOlKvWIuGBRCzjHD, P_0.buttonValues, CAzMEBskvLdFPOlKvWIuGBRCzjHD.Length);
			}

			public void gBcRCdfaAIzYgOLFmmqObqFZlTYN()
			{
				Array.Clear(osMBiuXGbAbBmDuSOHFsMKiZbMdb, 0, osMBiuXGbAbBmDuSOHFsMKiZbMdb.Length);
				Array.Clear(CAzMEBskvLdFPOlKvWIuGBRCzjHD, 0, CAzMEBskvLdFPOlKvWIuGBRCzjHD.Length);
			}

			public void TEIFPKwfMXSbKXmoxXfvWdtyjXmM()
			{
				Array.Clear(osMBiuXGbAbBmDuSOHFsMKiZbMdb, 0, osMBiuXGbAbBmDuSOHFsMKiZbMdb.Length);
			}
		}

		[Serializable]
		private sealed class XgovmCPnpKXhYhZSqnArexUqDPVB
		{
			public static readonly XgovmCPnpKXhYhZSqnArexUqDPVB _003C_003E9 = new XgovmCPnpKXhYhZSqnArexUqDPVB();

			public static Func<DcNEiwAkjtcgoAfNxIRUZZBYWiIF> _003C_003E9__20_0;

			internal DcNEiwAkjtcgoAfNxIRUZZBYWiIF PeXGSbktKfHsKPTkMcscOqQozlRg()
			{
				return new DcNEiwAkjtcgoAfNxIRUZZBYWiIF(7, 4);
			}
		}

		private static HardwareControllerMap_Game uDqAfdiwziEsZKANGgxEyKqSUAPg;

		private UpdateLoopDataSet<DcNEiwAkjtcgoAfNxIRUZZBYWiIF> OUohdfwdANjlRjLaHbkQBVghyVHM;

		private float[] SlTOrmLTWrnoNhlecZpjDEoDgaSK;

		private bool[] DJCTsMJfnWUEJyvQWjBNlzaIhLos;

		private bool TCrhqMHGIdBeJtVLksPZmyIfJNNF;

		private bool WbvTVilaRCiKjRkiFrpnzDtefBthA;

		bool IGetSetEnabled.enabled
		{
			get
			{
				return TCrhqMHGIdBeJtVLksPZmyIfJNNF;
			}
			set
			{
				if (TCrhqMHGIdBeJtVLksPZmyIfJNNF != value)
				{
					TCrhqMHGIdBeJtVLksPZmyIfJNNF = value;
					Clear();
					ThreadSafeUnityInput.mouse.Monitor(value);
				}
			}
		}

		InputSource IUnifiedMouseSource.inputSource => InputSource.UnityKeyboardAndMouse;

		HardwareControllerMap_Game IUnifiedMouseSource.hardwareMap
		{
			get
			{
				if (uDqAfdiwziEsZKANGgxEyKqSUAPg == null)
				{
					uDqAfdiwziEsZKANGgxEyKqSUAPg = CreateHardwareMap();
				}
				return uDqAfdiwziEsZKANGgxEyKqSUAPg;
			}
		}

		int IUnifiedMouseSource.buttonCount => 7;

		int IUnifiedMouseSource.axisCount => 4;

		Vector2 IUnifiedMouseSource.mousePosition
		{
			get
			{
				if (!TCrhqMHGIdBeJtVLksPZmyIfJNNF)
				{
					return default(Vector2);
				}
				return ThreadSafeUnityInput.mouse.mousePosition;
			}
		}

		Controller.Extension IUnifiedMouseSource.controllerExtension => null;

		public UnityUnifiedMouseSource()
		{
			OUohdfwdANjlRjLaHbkQBVghyVHM = new UpdateLoopDataSet<DcNEiwAkjtcgoAfNxIRUZZBYWiIF>(ReInput.configVars.updateLoop, XgovmCPnpKXhYhZSqnArexUqDPVB._003C_003E9.PeXGSbktKfHsKPTkMcscOqQozlRg);
			SlTOrmLTWrnoNhlecZpjDEoDgaSK = new float[4];
			DJCTsMJfnWUEJyvQWjBNlzaIhLos = new bool[7];
			Rewired_002EInterfaces_002EIGetSetEnabled_002Eenabled = true;
			ReInput.UpdateEndedEvent += KMaqaMVxBrxvMyEWJFWTxKyJwkhK;
			ReInput.EarlyUpdateEvent += xPiczNEloUlQjtTzJcFdHgawTjmjb;
		}

		public void UpdateInputData(ControllerDataUpdater dataUpdater)
		{
			OUohdfwdANjlRjLaHbkQBVghyVHM.Get(ReInput.currentUpdateLoop).YJKbbNbRLdpdAqDgtQHLGoZKVeMb(dataUpdater);
		}

		void IUnifiedMouseSource.UpdateInputData(ControllerDataUpdater dataUpdater)
		{
			//ILSpy generated this explicit interface implementation from .override directive in UpdateInputData
			this.UpdateInputData(dataUpdater);
		}

		public void Clear()
		{
			int count = OUohdfwdANjlRjLaHbkQBVghyVHM.Count;
			for (int i = 0; i < count; i++)
			{
				OUohdfwdANjlRjLaHbkQBVghyVHM.Get(i).gBcRCdfaAIzYgOLFmmqObqFZlTYN();
			}
		}

		void IUnifiedMouseSource.Clear()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Clear
			this.Clear();
		}

		private void xPiczNEloUlQjtTzJcFdHgawTjmjb()
		{
			if (TCrhqMHGIdBeJtVLksPZmyIfJNNF)
			{
				ThreadSafeUnityInput.mouse.GetAxisRawValues(SlTOrmLTWrnoNhlecZpjDEoDgaSK);
				ThreadSafeUnityInput.mouse.GetButtonValues(DJCTsMJfnWUEJyvQWjBNlzaIhLos);
				int count = OUohdfwdANjlRjLaHbkQBVghyVHM.Count;
				for (int i = 0; i < count; i++)
				{
					OUohdfwdANjlRjLaHbkQBVghyVHM.Get(i).pYAjozglrWCsaqhDqGFTGbRykOTD(DJCTsMJfnWUEJyvQWjBNlzaIhLos, SlTOrmLTWrnoNhlecZpjDEoDgaSK);
				}
			}
		}

		private void KMaqaMVxBrxvMyEWJFWTxKyJwkhK(UpdateLoopType P_0)
		{
			OUohdfwdANjlRjLaHbkQBVghyVHM.Get(P_0).TEIFPKwfMXSbKXmoxXfvWdtyjXmM();
		}

		internal static HardwareControllerMap_Game CreateHardwareMap()
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
					array3[num2++] = array[j].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid;
				}
				else if (array[j].elementType == ControllerElementType.Button)
				{
					array2[num++] = array[j].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid;
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

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		~UnityUnifiedMouseSource()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (WbvTVilaRCiKjRkiFrpnzDtefBthA)
			{
				return;
			}
			if (disposing)
			{
				if (TCrhqMHGIdBeJtVLksPZmyIfJNNF)
				{
					ThreadSafeUnityInput.mouse.Monitor(state: false);
				}
				ReInput.UpdateEndedEvent -= KMaqaMVxBrxvMyEWJFWTxKyJwkhK;
				ReInput.EarlyUpdateEvent -= xPiczNEloUlQjtTzJcFdHgawTjmjb;
			}
			WbvTVilaRCiKjRkiFrpnzDtefBthA = true;
		}

		public static ControllerElementType GetHardwareElementType(int elementIdentifierId)
		{
			if (uDqAfdiwziEsZKANGgxEyKqSUAPg == null)
			{
				uDqAfdiwziEsZKANGgxEyKqSUAPg = CreateHardwareMap();
			}
			return uDqAfdiwziEsZKANGgxEyKqSUAPg.GetElementType(elementIdentifierId);
		}
	}
}
