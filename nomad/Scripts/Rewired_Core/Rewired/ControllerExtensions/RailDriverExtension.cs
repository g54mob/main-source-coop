using Rewired.HID.Drivers;
using Rewired.Interfaces;

namespace Rewired.ControllerExtensions
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	public sealed class RailDriverExtension : Controller.Extension, IHIDControllerExtension
	{
		private class XoDpmllmzIpAHgnCwKJBebfkZLrD : IControllerExtensionSource
		{
			public readonly IDriver_RailDriver qKdFTSQYnqcyrunAMjlsJjYeDXFFA;

			public XoDpmllmzIpAHgnCwKJBebfkZLrD(IDriver_RailDriver P_0)
			{
				qKdFTSQYnqcyrunAMjlsJjYeDXFFA = P_0;
			}
		}

		private XoDpmllmzIpAHgnCwKJBebfkZLrD CWrfYRdckvUrGEGpcdmymUbEGgcvA;

		private Joystick joystick => GetController<Joystick>();

		public bool speakerEnabled
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return false;
				}
				if (CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA == null)
				{
					return false;
				}
				return CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA.SpeakerEnabled;
			}
			set
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
				}
				else if (CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA != null)
				{
					CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA.SpeakerEnabled = value;
				}
			}
		}

		ushort IHIDControllerExtension.vendorId
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				return CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA.vendorId;
			}
		}

		ushort IHIDControllerExtension.productId
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				return CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA.productId;
			}
		}

		string IHIDControllerExtension.productName
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return string.Empty;
				}
				return CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA.productName;
			}
		}

		string IHIDControllerExtension.manufacturer
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return string.Empty;
				}
				return CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA.manufacturer;
			}
		}

		ushort IHIDControllerExtension.usagePage
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				return CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA.usagePage;
			}
		}

		ushort IHIDControllerExtension.usage
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return 0;
				}
				return CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA.usage;
			}
		}

		internal RailDriverExtension(IDriver_RailDriver P_0)
			: base(new XoDpmllmzIpAHgnCwKJBebfkZLrD(P_0))
		{
		}

		private RailDriverExtension(RailDriverExtension P_0)
			: base(P_0)
		{
		}

		public void SetLEDDisplay(int digitIndex, byte digitBitValues)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA != null && base.enabled)
			{
				CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA.SetLEDDisplay(digitIndex, digitBitValues);
			}
		}

		public void SetLEDDisplay(byte digit1BitValues, byte digit2BitValues, byte digit3BitValues)
		{
			if (ReInput._id != _reInputId)
			{
				ReInput.CheckInitialized(_reInputId);
			}
			else if (CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA != null && base.enabled)
			{
				CWrfYRdckvUrGEGpcdmymUbEGgcvA.qKdFTSQYnqcyrunAMjlsJjYeDXFFA.SetLEDDisplay(digit1BitValues, digit2BitValues, digit3BitValues);
			}
		}

		internal override void UpdateData(UpdateLoopType updateLoop)
		{
		}

		internal override void SourceUpdated(IControllerExtensionSource source)
		{
			CWrfYRdckvUrGEGpcdmymUbEGgcvA = source as XoDpmllmzIpAHgnCwKJBebfkZLrD;
		}

		internal override Controller.Extension Clone()
		{
			return new RailDriverExtension(this);
		}
	}
}
