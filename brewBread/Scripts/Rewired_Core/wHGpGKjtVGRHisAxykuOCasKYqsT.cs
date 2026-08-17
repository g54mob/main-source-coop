using System;
using Rewired;
using Rewired.Platforms;
using Rewired.Utils;

internal class wHGpGKjtVGRHisAxykuOCasKYqsT
{
	public int HNrdETOGKUabWLqKrsrJjjjvQQqu;

	public int bUXXVdXGkUesYGUEMAGNgTFtAiMsA;

	public bool YpBtwmHHGBDqMXwfPbSITfkICZop;

	public string TyBbfACkfBSDMBUYEBOmeDAdwaFFc;

	public string noDyXDUdlPKBMSkRvbqYasAXqAFaA;

	public Guid FeHpFNUpCcTBGOhdkWReGnguZnrV;

	public Guid BkXUMuYBKghOBIdJYiPftcAEYsXQA;

	public int tpoonPbJmajQyAErVLnAOoXhqZqEb;

	public int yLLlShtixVkbYMaIfuUMijmLCboKA;

	public int VlOBcOivgRYNVAIyWkzlGxAaDIWac;

	public int ZotJgQDIqTjrsClzSattVdeemzgR;

	public PidVid uHkXbPenQtCEXhGoWMoJMQqwsSNMA;

	public Guid rWMIAFcgxIbsJaFNJhzhhxuHtcTX;

	public int kUHHTIgTMDyMEXufsUvSHmaExSkL;

	public int IghITlQqAHYzeQDzcXCztBBmmcAl;

	public void XrAbLZWFEwcAyuirOieMPGMErUhj()
	{
		byte[] value = FeHpFNUpCcTBGOhdkWReGnguZnrV.ToByteArray();
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
		kUHHTIgTMDyMEXufsUvSHmaExSkL = BitConverter.ToUInt16(value, startIndex);
		IghITlQqAHYzeQDzcXCztBBmmcAl = BitConverter.ToUInt16(value, startIndex2);
		uHkXbPenQtCEXhGoWMoJMQqwsSNMA = new PidVid((ushort)IghITlQqAHYzeQDzcXCztBBmmcAl, (ushort)kUHHTIgTMDyMEXufsUvSHmaExSkL);
		rWMIAFcgxIbsJaFNJhzhhxuHtcTX = MiscTools.CreateGuidHashSHA1(TyBbfACkfBSDMBUYEBOmeDAdwaFFc + uHkXbPenQtCEXhGoWMoJMQqwsSNMA.ToString() + bUXXVdXGkUesYGUEMAGNgTFtAiMsA);
		if (string.IsNullOrEmpty(noDyXDUdlPKBMSkRvbqYasAXqAFaA))
		{
			noDyXDUdlPKBMSkRvbqYasAXqAFaA = TyBbfACkfBSDMBUYEBOmeDAdwaFFc;
		}
	}

	public virtual string RsAkLkYigfQxKWvdxVBUwNqsOGbJ()
	{
		string text = string.Concat(string.Concat(string.Concat(string.Concat("" + "joystickIndex = " + HNrdETOGKUabWLqKrsrJjjjvQQqu + "\n", "joystickId = ", bUXXVdXGkUesYGUEMAGNgTFtAiMsA.ToString(), "\n"), "isGameController = ", YpBtwmHHGBDqMXwfPbSITfkICZop.ToString(), "\n"), "hardwareName = ", TyBbfACkfBSDMBUYEBOmeDAdwaFFc, "\n"), "friendlyName = ", noDyXDUdlPKBMSkRvbqYasAXqAFaA, "\n");
		Guid feHpFNUpCcTBGOhdkWReGnguZnrV = FeHpFNUpCcTBGOhdkWReGnguZnrV;
		string text2 = text + "sdlJoystickGuid = " + feHpFNUpCcTBGOhdkWReGnguZnrV.ToString() + "\n";
		feHpFNUpCcTBGOhdkWReGnguZnrV = BkXUMuYBKghOBIdJYiPftcAEYsXQA;
		string text3 = string.Concat(string.Concat(string.Concat(string.Concat(text2 + "sdlDeviceGuid = " + feHpFNUpCcTBGOhdkWReGnguZnrV.ToString() + "\n", "buttonCount = ", tpoonPbJmajQyAErVLnAOoXhqZqEb.ToString(), "\n"), "axisCount = ", yLLlShtixVkbYMaIfuUMijmLCboKA.ToString(), "\n"), "hatCount = ", VlOBcOivgRYNVAIyWkzlGxAaDIWac.ToString(), "\n"), "ballCount = ", ZotJgQDIqTjrsClzSattVdeemzgR.ToString(), "\n");
		PidVid pidVid = uHkXbPenQtCEXhGoWMoJMQqwsSNMA;
		string text4 = text3 + "pidVid = " + pidVid.ToString() + "\n";
		feHpFNUpCcTBGOhdkWReGnguZnrV = rWMIAFcgxIbsJaFNJhzhhxuHtcTX;
		return string.Concat(string.Concat(text4 + "instanceGuid = " + feHpFNUpCcTBGOhdkWReGnguZnrV.ToString() + "\n", "vendorId = ", kUHHTIgTMDyMEXufsUvSHmaExSkL.ToString(), "\n"), "productId = ", IghITlQqAHYzeQDzcXCztBBmmcAl.ToString(), "\n");
	}
}
