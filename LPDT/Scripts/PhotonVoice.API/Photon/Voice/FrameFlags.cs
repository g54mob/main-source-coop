namespace Photon.Voice
{
	public enum FrameFlags : byte
	{
		Config = 1,
		KeyFrame = 2,
		PartNotBeg = 4,
		EndOfStream = 8,
		FragNotBeg = 16,
		FragNotEnd = 32,
		FEC = 64,
		PartNotEnd = 128,
		MaskFrag = 48,
		MaskPart = 132
	}
}
