using System;
using Rewired.Data.Mapping;
using Rewired.Interfaces;

namespace Rewired
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class UnityUnifiedKeyboardSource : IDisposable, IGetSetEnabled, IUnifiedKeyboardSource
	{
		private const int KSKBmcXcCBdLxyaTWHLWkjHthpEt = 132;

		private static HardwareControllerMap_Game NadPxXYRifMljEYjJuhjIbsPVNdr;

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
					ThreadSafeUnityInput.keyboard.Monitor(value);
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

		public int buttonCount => 132;

		public Controller.Extension controllerExtension => null;

		public UnityUnifiedKeyboardSource()
		{
			enabled = true;
		}

		public void UpdateInputData(ControllerDataUpdater dataUpdater)
		{
			if (kKFZZElqKQSUFMZnWdEudRvTJGpo)
			{
				ThreadSafeUnityInput.keyboard.GetKeyValues(dataUpdater.buttonValues);
			}
		}

		public void Clear()
		{
		}

		private static HardwareControllerMap_Game HStMDbOieMaNNxlRQxuGsFEKKaRE()
		{
			ControllerElementIdentifier[] array = new ControllerElementIdentifier[132];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ControllerElementIdentifier(i, Consts.keyboardKeyNames[i], Consts.keyboardKeyNames[i], string.Empty, ControllerElementType.Button, true);
			}
			int[] array2 = new int[132];
			for (int j = 0; j < 132; j++)
			{
				array2[j] = array[j].id;
			}
			HardwareButtonInfo[] array3 = new HardwareButtonInfo[132];
			for (int k = 0; k < 132; k++)
			{
				array3[k] = new HardwareButtonInfo();
			}
			return new HardwareControllerMap_Game("Keyboard", default(HardwareControllerMapIdentifier), array, array2, new int[0], new AxisCalibrationData[0], new AxisRange[0], new HardwareAxisInfo[0], array3, null);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~UnityUnifiedKeyboardSource()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				if (disposing && kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					ThreadSafeUnityInput.keyboard.Monitor(state: false);
				}
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
			}
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
