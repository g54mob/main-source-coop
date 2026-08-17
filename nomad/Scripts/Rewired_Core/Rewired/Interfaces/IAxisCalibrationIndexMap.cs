namespace Rewired.Interfaces
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal interface IAxisCalibrationIndexMap
	{
		int GetMappedAxisIndex(int axisIndex);
	}
}
