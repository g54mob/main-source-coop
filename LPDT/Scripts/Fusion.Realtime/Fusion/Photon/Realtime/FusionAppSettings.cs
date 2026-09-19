using System;
using System.Text;
using Photon.Realtime;

namespace Fusion.Photon.Realtime
{
	[Serializable]
	public class FusionAppSettings : AppSettings
	{
		[InlineHelp]
		public EncryptionMode encryptionMode;

		[InlineHelp]
		public int emptyRoomTtl;

		public FusionAppSettings GetCopy()
		{
			FusionAppSettings fusionAppSettings = new FusionAppSettings();
			CopyTo(fusionAppSettings);
			return fusionAppSettings;
		}

		public FusionAppSettings CopyTo(FusionAppSettings target)
		{
			CopyTo((AppSettings)target);
			target.encryptionMode = encryptionMode;
			target.emptyRoomTtl = emptyRoomTtl;
			return target;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(ToStringFull());
			stringBuilder.Append($", Encryption Mode: {encryptionMode}");
			stringBuilder.Append($", Empty Room TTL: {emptyRoomTtl}");
			return stringBuilder.ToString();
		}
	}
}
