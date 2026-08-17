using System;
using Rewired.Config;
using Rewired.Drivers.Interfaces;
using Rewired.Platforms;
using Rewired.Utils;

namespace Rewired.HID.Drivers
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal abstract class HIDDeviceDriver : IDisposable, IControllerDriver
	{
		[CustomObfuscation(rename = false)]
		public enum DriverType
		{
			None = 0,
			DualShock4 = 1,
			DualSense = 2,
			RailDriver = 3
		}

		[CustomObfuscation(rename = false)]
		internal delegate byte[] GetHidFeatureData(byte reportId);

		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		internal class InitArgs
		{
			public UpdateLoopSetting updateLoopSetting;

			public DeviceConnectionType connectionType;

			public int minAxisValue;

			public int maxAxisValue;

			public int hatZeroValue;

			public int hatSpan;

			public int inputReportLength;

			public int outputReportLength;

			public Func<OutputReport, bool> synchronousWriteOutputReportDelegate;

			public Action<OutputReport> asynchronousWriteOutputReportDelegate;

			public GetHidFeatureData getFeatureReportDelegate;

			public InitArgs(UpdateLoopSetting P_0, DeviceConnectionType P_1, int P_2, int P_3, int P_4, int P_5, int P_6, int P_7, Func<OutputReport, bool> P_8, Action<OutputReport> P_9, GetHidFeatureData P_10)
			{
				updateLoopSetting = P_0;
				connectionType = P_1;
				minAxisValue = P_2;
				maxAxisValue = P_3;
				hatZeroValue = P_4;
				hatSpan = P_5;
				inputReportLength = P_6;
				outputReportLength = P_7;
				synchronousWriteOutputReportDelegate = P_8;
				asynchronousWriteOutputReportDelegate = P_9;
				getFeatureReportDelegate = P_10;
			}
		}

		public HIDAxis[] axes;

		public HIDButton[] buttons;

		public HIDHat[] hats;

		public HIDAccelerometer[] accelerometers;

		public HIDGyroscope[] gyroscopes;

		public HIDTouchpad[] touchpads;

		public HIDVibrationMotor[] vibrationMotors;

		public HIDLight[] lights;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public int AxisCount
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

		public int ButtonCount
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

		public int HatCount
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

		public int AccelerometerCount
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

		public int GyroscopeCount
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

		public int TouchpadCount
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

		public int LightCount
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

		public int VibrationMotorCount
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

		protected bool disposed => AeWaeWamxRrERkciQkpFDWbRfZMkA;

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
				if (UnityTools.effectivePlatform == Platform.OSX && hidDriverInitArgs.connectionType == DeviceConnectionType.Bluetooth)
				{
					return null;
				}
				return new DualShock4Driver(hidDriverInitArgs);
			case DriverType.DualSense:
				return new DualSenseDriver(hidDriverInitArgs);
			case DriverType.RailDriver:
				return new RailDriverDriver(hidDriverInitArgs);
			default:
				return null;
			}
		}

		public static DriverType FindDriverId(int vendorId, int productId)
		{
			if (DualShock4Driver.Matches(vendorId, productId))
			{
				return DriverType.DualShock4;
			}
			if (DualSenseDriver.Matches(vendorId, productId))
			{
				return DriverType.DualSense;
			}
			if (RailDriverDriver.Matches(vendorId, productId))
			{
				return DriverType.RailDriver;
			}
			return DriverType.None;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~HIDDeviceDriver()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
			}
		}
	}
}
