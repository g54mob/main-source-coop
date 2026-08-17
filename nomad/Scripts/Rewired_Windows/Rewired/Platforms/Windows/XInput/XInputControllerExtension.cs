using Rewired.Interfaces;

namespace Rewired.Platforms.Windows.XInput
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	public sealed class XInputControllerExtension : Controller.Extension
	{
		private class RQYfguEYCpVlYVLUIDpXODUhETBtA : IControllerExtensionSource
		{
			private TVocCDfxMinGIOCkmldqehsxNAxhb.AlKsZxctHXLfYYkzfFYPZTTBpoGg rdzWQYwoosTQQnDRtarszTrsydFv;

			public RQYfguEYCpVlYVLUIDpXODUhETBtA(TVocCDfxMinGIOCkmldqehsxNAxhb.AlKsZxctHXLfYYkzfFYPZTTBpoGg P_0)
			{
				rdzWQYwoosTQQnDRtarszTrsydFv = P_0;
			}
		}

		private RQYfguEYCpVlYVLUIDpXODUhETBtA UBJTiRzKsUmUTOrsDFNJnJhTGDjk;

		private bool eWQDTTfjNzlVwBXWplQkvTLvsSHH;

		internal XInputControllerExtension(TVocCDfxMinGIOCkmldqehsxNAxhb.AlKsZxctHXLfYYkzfFYPZTTBpoGg P_0)
			: base(new RQYfguEYCpVlYVLUIDpXODUhETBtA(P_0))
		{
		}

		private XInputControllerExtension(XInputControllerExtension P_0)
			: base(P_0)
		{
		}

		internal override void UpdateData(UpdateLoopType updateLoop)
		{
			if (eWQDTTfjNzlVwBXWplQkvTLvsSHH)
			{
				_ = base.enabled;
			}
		}

		internal override void SourceUpdated(IControllerExtensionSource source)
		{
			UBJTiRzKsUmUTOrsDFNJnJhTGDjk = source as RQYfguEYCpVlYVLUIDpXODUhETBtA;
			eWQDTTfjNzlVwBXWplQkvTLvsSHH = UBJTiRzKsUmUTOrsDFNJnJhTGDjk != null;
		}

		internal override Controller.Extension Clone()
		{
			return new XInputControllerExtension(this);
		}
	}
}
