namespace Photon.Voice
{
	internal class DeviceEnumeratorNotSupported : DeviceEnumeratorBase
	{
		private string message;

		public override bool IsSupported => false;

		public override string Error => message;

		public DeviceEnumeratorNotSupported(ILogger logger, string message)
			: base(logger)
		{
			this.message = message;
		}

		public override void Refresh()
		{
			if (base.OnReady != null)
			{
				base.OnReady();
			}
		}

		public override void Dispose()
		{
		}
	}
}
