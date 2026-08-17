using System;
using System.Collections.Generic;
using Rewired.Config;
using Rewired.Platforms;
using Rewired.Utils;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal abstract class HIDDeviceDriver : IControllerDriver, IDisposable
	{
		[CustomObfuscation(rename = false)]
		public enum DriverType
		{
			None = 0,
			DualShock4 = 1,
			DualSense = 2,
			RailDriver = 3,
			SwitchJoyConLeft = 4,
			SwitchJoyConRight = 5,
			SwitchProController = 6
		}

		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		internal struct HIDProperties
		{
			public ushort vendorId;

			public ushort productId;

			public string productName;

			public string manufacturer;

			public ushort usagePage;

			public ushort usage;

			public int maxInputReportLength;

			public int maxOutputReportLength;

			public int maxFeatureReportLength;

			public HIDProperties(ushort P_0, ushort P_1, string P_2, string P_3, ushort P_4, ushort P_5, int P_6, int P_7, int P_8)
			{
				vendorId = P_0;
				productId = P_1;
				productName = P_2;
				manufacturer = P_3;
				usagePage = P_4;
				usage = P_5;
				maxInputReportLength = P_6;
				maxOutputReportLength = P_7;
				maxFeatureReportLength = P_8;
			}
		}

		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		internal interface IHIDDevice
		{
			HIDProperties properties { get; }

			bool WriteSync(aMZqdyjJERTAUbjSZWzzHWVxTEnF outputReport, int timeoutMs);

			void WriteAsync(aMZqdyjJERTAUbjSZWzzHWVxTEnF outputReport, int timeoutMs);

			bool ReadSync(IntPtr buffer, int bytesToRead, int timeoutMs);

			byte[] GetHidFeatureData(byte reportId, int reportLength, int timeoutMs, int retryCount);
		}

		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		internal class InitArgs
		{
			public readonly UpdateLoopSetting updateLoopSetting;

			public readonly YDvFqJokstcNyQQOYydcruGncmeb connectionType;

			public readonly int minAxisValue;

			public readonly int maxAxisValue;

			public readonly int hatZeroValue;

			public readonly int hatSpan;

			public readonly IHIDDevice hidDevice;

			public InitArgs(UpdateLoopSetting P_0, YDvFqJokstcNyQQOYydcruGncmeb P_1, int P_2, int P_3, int P_4, int P_5, IHIDDevice P_6)
			{
				updateLoopSetting = P_0;
				connectionType = P_1;
				minAxisValue = P_2;
				maxAxisValue = P_3;
				hatZeroValue = P_4;
				hatSpan = P_5;
				hidDevice = P_6;
			}
		}

		public eTBgDLAnVcEreaYiOpvDFMeVVuExA[] axes;

		public RyDagBEfRFfkQlRDvQAHmQXROhrtA[] buttons;

		public AlQQSkDXAKgzPiahlYVsHmMBdhGkA[] hats;

		public fcgInupHfYVLlnSfBDoHscyUgTsEA[] accelerometers;

		public zeduVYzSnJpVQGxDoGRFMdphEaCi[] gyroscopes;

		public JeEihaxNGDZUEopEZTyRorKoTSAm[] touchpads;

		public iwnZquMFWHwhZjzckYkHRPdcqkIc[] vibrationMotors;

		public dRxYZKovikdvFiOlZLmFiKpaWUdu[] lights;

		private bool ZznETxPPavrVhtnVPnKDoHtNiXyR;

		int IControllerDriver.AxisCount
		{
			get
			{
				if (axes == null)
				{
					return 0;
				}
				return axes.Length;
			}
		}

		int IControllerDriver.ButtonCount
		{
			get
			{
				if (buttons == null)
				{
					return 0;
				}
				return buttons.Length;
			}
		}

		int IControllerDriver.HatCount
		{
			get
			{
				if (hats == null)
				{
					return 0;
				}
				return hats.Length;
			}
		}

		int IControllerDriver.AccelerometerCount
		{
			get
			{
				if (accelerometers == null)
				{
					return 0;
				}
				return accelerometers.Length;
			}
		}

		int IControllerDriver.GyroscopeCount
		{
			get
			{
				if (gyroscopes == null)
				{
					return 0;
				}
				return gyroscopes.Length;
			}
		}

		int IControllerDriver.TouchpadCount
		{
			get
			{
				if (touchpads == null)
				{
					return 0;
				}
				return touchpads.Length;
			}
		}

		int IControllerDriver.LightCount
		{
			get
			{
				if (lights == null)
				{
					return 0;
				}
				return lights.Length;
			}
		}

		int IControllerDriver.VibrationMotorCount
		{
			get
			{
				if (vibrationMotors == null)
				{
					return 0;
				}
				return vibrationMotors.Length;
			}
		}

		protected bool disposed => ZznETxPPavrVhtnVPnKDoHtNiXyR;

		public HIDDeviceDriver()
		{
		}

		public abstract void Update(UpdateLoopType updateLoop);

		public abstract bool ParseInputReport(IntPtr inputReportPtr, int inputReportLength, double timestamp);

		public abstract Controller.Extension CreateControllerExtension();

		public static HIDDeviceDriver GetDriver(DriverType driverId, InitArgs hidDriverInitArgs)
		{
			if (hidDriverInitArgs == null)
			{
				return null;
			}
			switch (driverId)
			{
			case DriverType.DualShock4:
				if (UnityTools.effectivePlatform == Platform.Linux)
				{
					return null;
				}
				return new DualShock4Driver(hidDriverInitArgs);
			case DriverType.DualSense:
				return new DualSenseDriver(hidDriverInitArgs);
			case DriverType.RailDriver:
				return new RailDriverDriver(hidDriverInitArgs);
			case DriverType.SwitchJoyConLeft:
				return new NintendoSwitchJoyConLeftDriver(hidDriverInitArgs);
			case DriverType.SwitchJoyConRight:
				return new SwitchJoyConRightDriver(hidDriverInitArgs);
			case DriverType.SwitchProController:
				if (hidDriverInitArgs.connectionType != YDvFqJokstcNyQQOYydcruGncmeb.Bluetooth)
				{
					return null;
				}
				return new NintendoSwitchProControllerDriver(hidDriverInitArgs);
			default:
				return null;
			}
		}

		public static DriverType FindDriverId(int vendorId, int productId, IList<EnhancedDeviceSupportDeviceType> exclusions)
		{
			if (DualShock4Driver.Matches(vendorId, productId))
			{
				if (exclusions != null && exclusions.Contains(EnhancedDeviceSupportDeviceType.SonyDualShock4))
				{
					return DriverType.None;
				}
				return DriverType.DualShock4;
			}
			if (DualSenseDriver.Matches(vendorId, productId))
			{
				if (exclusions != null && exclusions.Contains(EnhancedDeviceSupportDeviceType.SonyDualSense))
				{
					return DriverType.None;
				}
				return DriverType.DualSense;
			}
			if (RailDriverDriver.Matches(vendorId, productId))
			{
				if (exclusions != null && exclusions.Contains(EnhancedDeviceSupportDeviceType.PIEngineeringRailDriver))
				{
					return DriverType.None;
				}
				return DriverType.RailDriver;
			}
			if (NintendoSwitchJoyConLeftDriver.Matches(vendorId, productId))
			{
				if (exclusions != null && exclusions.Contains(EnhancedDeviceSupportDeviceType.NintendoSwitchJoyConLeft))
				{
					return DriverType.None;
				}
				return DriverType.SwitchJoyConLeft;
			}
			if (SwitchJoyConRightDriver.Matches(vendorId, productId))
			{
				if (exclusions != null && exclusions.Contains(EnhancedDeviceSupportDeviceType.NintendoSwitchJoyConRight))
				{
					return DriverType.None;
				}
				return DriverType.SwitchJoyConRight;
			}
			if (NintendoSwitchProControllerDriver.Matches(vendorId, productId))
			{
				if (exclusions != null && exclusions.Contains(EnhancedDeviceSupportDeviceType.NintendoSwitchProController))
				{
					return DriverType.None;
				}
				return DriverType.SwitchProController;
			}
			return DriverType.None;
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

		~HIDDeviceDriver()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!ZznETxPPavrVhtnVPnKDoHtNiXyR)
			{
				ZznETxPPavrVhtnVPnKDoHtNiXyR = true;
			}
		}
	}
}
