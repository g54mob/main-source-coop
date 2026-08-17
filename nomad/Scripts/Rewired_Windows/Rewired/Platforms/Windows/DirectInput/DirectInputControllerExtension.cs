using Rewired.ControllerExtensions;
using Rewired.Interfaces;

namespace Rewired.Platforms.Windows.DirectInput
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	public sealed class DirectInputControllerExtension : Controller.Extension, IHIDControllerExtension
	{
		private class hldkpLJbJhunQMROVLJAzcfoAowA : IControllerExtensionSource
		{
			private AZmRVMBCLWjdSJdmldWMNaRORDDE RzmjLocLhyrFZyqNXBgNzfYaMgYX;

			private hlOkjDeqVWQCgnqAjZMKnjtRaEFeA sYQImuwCPUuQQiOdRUIwKNFxEpwO;

			public AZmRVMBCLWjdSJdmldWMNaRORDDE dueXJEryHhxpNBjuzDnHoYoPhcJT => RzmjLocLhyrFZyqNXBgNzfYaMgYX;

			public hlOkjDeqVWQCgnqAjZMKnjtRaEFeA euvWAABsjtIZReUtYVgjkZbWMLdAA => sYQImuwCPUuQQiOdRUIwKNFxEpwO;

			public hldkpLJbJhunQMROVLJAzcfoAowA(AZmRVMBCLWjdSJdmldWMNaRORDDE P_0, hlOkjDeqVWQCgnqAjZMKnjtRaEFeA P_1)
			{
				RzmjLocLhyrFZyqNXBgNzfYaMgYX = P_0;
				sYQImuwCPUuQQiOdRUIwKNFxEpwO = P_1;
			}
		}

		private hldkpLJbJhunQMROVLJAzcfoAowA JtmrtBEPjdpoaglPfHMeSdEcixjq;

		private bool pfbVDJNgModbtsUAulDpLUMhqLuN;

		string IHIDControllerExtension.productName
		{
			get
			{
				if (ReInput._id != _reInputId)
				{
					ReInput.CheckInitialized(_reInputId);
					return string.Empty;
				}
				if (!pfbVDJNgModbtsUAulDpLUMhqLuN || !base.enabled)
				{
					return string.Empty;
				}
				return JtmrtBEPjdpoaglPfHMeSdEcixjq.euvWAABsjtIZReUtYVgjkZbWMLdAA.JANJnqDdtgdZeQdaCCMdLNmlQEuP.jylQTjZiUCZzLSJBZIvTnKIRkKcv;
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
				if (!pfbVDJNgModbtsUAulDpLUMhqLuN || !base.enabled)
				{
					return 0;
				}
				if (JtmrtBEPjdpoaglPfHMeSdEcixjq.euvWAABsjtIZReUtYVgjkZbWMLdAA == null)
				{
					return 0;
				}
				return JtmrtBEPjdpoaglPfHMeSdEcixjq.dueXJEryHhxpNBjuzDnHoYoPhcJT.AZDSATNUswZqkwVvzNYRJeLWxVJG;
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
				if (!pfbVDJNgModbtsUAulDpLUMhqLuN || !base.enabled)
				{
					return 0;
				}
				if (JtmrtBEPjdpoaglPfHMeSdEcixjq.euvWAABsjtIZReUtYVgjkZbWMLdAA == null)
				{
					return 0;
				}
				return JtmrtBEPjdpoaglPfHMeSdEcixjq.dueXJEryHhxpNBjuzDnHoYoPhcJT.svoXuyQtjqEufBoeaMucAjnxlHmg;
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
				if (!pfbVDJNgModbtsUAulDpLUMhqLuN || !base.enabled)
				{
					return 0;
				}
				return (ushort)JtmrtBEPjdpoaglPfHMeSdEcixjq.euvWAABsjtIZReUtYVgjkZbWMLdAA.JANJnqDdtgdZeQdaCCMdLNmlQEuP.hUdFIigItAsRWpFvbtCrpHQlLcXo;
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
				if (!pfbVDJNgModbtsUAulDpLUMhqLuN || !base.enabled)
				{
					return 0;
				}
				return (ushort)JtmrtBEPjdpoaglPfHMeSdEcixjq.euvWAABsjtIZReUtYVgjkZbWMLdAA.JANJnqDdtgdZeQdaCCMdLNmlQEuP.cfOdAfTfPQHiScPSUPDbGfIAyhqlB;
			}
		}

		string IHIDControllerExtension.manufacturer => string.Empty;

		internal DirectInputControllerExtension(AZmRVMBCLWjdSJdmldWMNaRORDDE P_0, hlOkjDeqVWQCgnqAjZMKnjtRaEFeA P_1)
			: base(new hldkpLJbJhunQMROVLJAzcfoAowA(P_0, P_1))
		{
		}

		private DirectInputControllerExtension(DirectInputControllerExtension P_0)
			: base(P_0)
		{
		}

		internal override void UpdateData(UpdateLoopType updateLoop)
		{
			if (pfbVDJNgModbtsUAulDpLUMhqLuN)
			{
				_ = base.enabled;
			}
		}

		internal override void SourceUpdated(IControllerExtensionSource source)
		{
			JtmrtBEPjdpoaglPfHMeSdEcixjq = source as hldkpLJbJhunQMROVLJAzcfoAowA;
			pfbVDJNgModbtsUAulDpLUMhqLuN = JtmrtBEPjdpoaglPfHMeSdEcixjq != null;
		}

		internal override Controller.Extension Clone()
		{
			return new DirectInputControllerExtension(this);
		}
	}
}
