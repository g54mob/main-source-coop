using System;
using Rewired;
using Rewired.Platforms;
using Rewired.Utils;

internal class ettUIVyZApbfOsMrELHoexHuUXcN
{
	public int DoWnkeloRxSemxvueBCZnmhLIqtS;

	public int KjRormcdGxhvdWbQmSpZaQRujKKN;

	public bool GLrCbizCVIdjJNQlrNzUXdTUyJYs;

	public string uATcQBVLkatxNzIoYaEaljmiwIhB;

	public string MmmKcHslyLeNdcFDErupuZipZFY;

	public Guid uBQxBBQjIUIzUDjNIsbWycLGtKnt;

	public Guid nPASzgADMvDUNpZroJVRAPDvtzpf;

	public int kruIBIEKmDWLUjnMGjJUQFYlSNFx;

	public int QRGPSUGAgdaBSsLOqeCkDiNnMMdGb;

	public int gARdLCOFuGIqRIoDvTCsYmNSZuAM;

	public int KvDEREhlzoVOVCaKPhQeEuSqmmZU;

	public PidVid kFeanmhsYlSYDdkGbPDvpHzfWHqv;

	public Guid VlSDofGrbYLGPQARmijApVYAzBPh;

	public int WtnIVLihDdSbCXtgNiznjyGibXji;

	public int ecdNtblRbpzgaroYaHvzdxqYQgyi;

	public void BIijgChUidFqolBZUkuQKBcLKhQj()
	{
		byte[] value = uBQxBBQjIUIzUDjNIsbWycLGtKnt.ToByteArray();
		int startIndex;
		int startIndex2;
		switch (UnityTools.effectivePlatform)
		{
		case Platform.Windows:
			startIndex = 0;
			startIndex2 = 2;
			break;
		case Platform.OSX:
			startIndex = 0;
			startIndex2 = 8;
			break;
		case Platform.Linux:
			startIndex = 4;
			startIndex2 = 8;
			break;
		default:
			throw new NotImplementedException();
		}
		WtnIVLihDdSbCXtgNiznjyGibXji = BitConverter.ToUInt16(value, startIndex);
		ecdNtblRbpzgaroYaHvzdxqYQgyi = BitConverter.ToUInt16(value, startIndex2);
		kFeanmhsYlSYDdkGbPDvpHzfWHqv = new PidVid((ushort)ecdNtblRbpzgaroYaHvzdxqYQgyi, (ushort)WtnIVLihDdSbCXtgNiznjyGibXji);
		VlSDofGrbYLGPQARmijApVYAzBPh = MiscTools.CreateGuidHashSHA1(uATcQBVLkatxNzIoYaEaljmiwIhB + kFeanmhsYlSYDdkGbPDvpHzfWHqv.ToString() + KjRormcdGxhvdWbQmSpZaQRujKKN);
		if (string.IsNullOrEmpty(MmmKcHslyLeNdcFDErupuZipZFY))
		{
			MmmKcHslyLeNdcFDErupuZipZFY = uATcQBVLkatxNzIoYaEaljmiwIhB;
		}
	}

	public virtual string BtOszQDqpfNSqrDOxgIjFoeHJPtnA()
	{
		string text = string.Concat(string.Concat(string.Concat(string.Concat("" + "joystickIndex = " + DoWnkeloRxSemxvueBCZnmhLIqtS + "\n", "joystickId = ", KjRormcdGxhvdWbQmSpZaQRujKKN.ToString(), "\n"), "isGameController = ", GLrCbizCVIdjJNQlrNzUXdTUyJYs.ToString(), "\n"), "hardwareName = ", uATcQBVLkatxNzIoYaEaljmiwIhB, "\n"), "friendlyName = ", MmmKcHslyLeNdcFDErupuZipZFY, "\n");
		Guid guid = uBQxBBQjIUIzUDjNIsbWycLGtKnt;
		string text2 = text + "sdlJoystickGuid = " + guid.ToString() + "\n";
		guid = nPASzgADMvDUNpZroJVRAPDvtzpf;
		string text3 = string.Concat(string.Concat(string.Concat(string.Concat(text2 + "sdlDeviceGuid = " + guid.ToString() + "\n", "buttonCount = ", kruIBIEKmDWLUjnMGjJUQFYlSNFx.ToString(), "\n"), "axisCount = ", QRGPSUGAgdaBSsLOqeCkDiNnMMdGb.ToString(), "\n"), "hatCount = ", gARdLCOFuGIqRIoDvTCsYmNSZuAM.ToString(), "\n"), "ballCount = ", KvDEREhlzoVOVCaKPhQeEuSqmmZU.ToString(), "\n");
		PidVid pidVid = kFeanmhsYlSYDdkGbPDvpHzfWHqv;
		string text4 = text3 + "pidVid = " + pidVid.ToString() + "\n";
		guid = VlSDofGrbYLGPQARmijApVYAzBPh;
		return string.Concat(string.Concat(text4 + "instanceGuid = " + guid.ToString() + "\n", "vendorId = ", WtnIVLihDdSbCXtgNiznjyGibXji.ToString(), "\n"), "productId = ", ecdNtblRbpzgaroYaHvzdxqYQgyi.ToString(), "\n");
	}
}
