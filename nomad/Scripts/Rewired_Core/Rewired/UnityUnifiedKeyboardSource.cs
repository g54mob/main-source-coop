using System;
using Rewired.Data.Mapping;
using Rewired.Interfaces;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class UnityUnifiedKeyboardSource : IUnifiedKeyboardSource, IGetSetEnabled, IDisposable
	{
		private const int OHzbZDHJXODXOMjxXFypgCWBADnrc = 132;

		private static HardwareControllerMap_Game NkPGJUwdVTCYgbLAfOBekusXXkVH;

		private bool yptnYBqnKTqzSZWYwLzontKULgNv;

		private bool ioBJBypLuVBaSFKWpfFXiENWfNrGb;

		bool IGetSetEnabled.enabled
		{
			get
			{
				return yptnYBqnKTqzSZWYwLzontKULgNv;
			}
			set
			{
				if (yptnYBqnKTqzSZWYwLzontKULgNv != value)
				{
					yptnYBqnKTqzSZWYwLzontKULgNv = value;
					Clear();
					ThreadSafeUnityInput.keyboard.Monitor(value);
				}
			}
		}

		InputSource IUnifiedKeyboardSource.inputSource => InputSource.UnityKeyboardAndMouse;

		HardwareControllerMap_Game IUnifiedKeyboardSource.hardwareMap
		{
			get
			{
				if (NkPGJUwdVTCYgbLAfOBekusXXkVH == null)
				{
					NkPGJUwdVTCYgbLAfOBekusXXkVH = CreateHardwareMap();
				}
				return NkPGJUwdVTCYgbLAfOBekusXXkVH;
			}
		}

		int IUnifiedKeyboardSource.buttonCount => 132;

		Controller.Extension IUnifiedKeyboardSource.controllerExtension => null;

		public UnityUnifiedKeyboardSource()
		{
			Rewired_002EInterfaces_002EIGetSetEnabled_002Eenabled = true;
		}

		public void UpdateInputData(ControllerDataUpdater dataUpdater)
		{
			if (yptnYBqnKTqzSZWYwLzontKULgNv)
			{
				ThreadSafeUnityInput.keyboard.GetKeyValues(dataUpdater.buttonValues);
			}
		}

		void IUnifiedKeyboardSource.UpdateInputData(ControllerDataUpdater dataUpdater)
		{
			//ILSpy generated this explicit interface implementation from .override directive in UpdateInputData
			this.UpdateInputData(dataUpdater);
		}

		public void Clear()
		{
		}

		void IUnifiedKeyboardSource.Clear()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Clear
			this.Clear();
		}

		internal static HardwareControllerMap_Game CreateHardwareMap()
		{
			ControllerElementIdentifier[] array = new ControllerElementIdentifier[132];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ControllerElementIdentifier(i, Consts.keyboardKeyNames[i], Consts.keyboardKeyNames[i], string.Empty, ControllerElementType.Button, true);
			}
			int[] array2 = new int[132];
			for (int j = 0; j < 132; j++)
			{
				array2[j] = array[j].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid;
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

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		~UnityUnifiedKeyboardSource()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!ioBJBypLuVBaSFKWpfFXiENWfNrGb)
			{
				if (disposing && yptnYBqnKTqzSZWYwLzontKULgNv)
				{
					ThreadSafeUnityInput.keyboard.Monitor(state: false);
				}
				ioBJBypLuVBaSFKWpfFXiENWfNrGb = true;
			}
		}

		public static ControllerElementType GetHardwareElementType(int elementIdentifierId)
		{
			if (NkPGJUwdVTCYgbLAfOBekusXXkVH == null)
			{
				NkPGJUwdVTCYgbLAfOBekusXXkVH = CreateHardwareMap();
			}
			return NkPGJUwdVTCYgbLAfOBekusXXkVH.GetElementType(elementIdentifierId);
		}
	}
}
