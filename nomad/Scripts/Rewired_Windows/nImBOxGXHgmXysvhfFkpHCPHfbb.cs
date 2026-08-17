using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

internal abstract class nImBOxGXHgmXysvhfFkpHCPHfbb<_0001, _0002, _0003> : hlOkjDeqVWQCgnqAjZMKnjtRaEFeA where _0001 : class, global::HWTyxBmMFQveNIpALfzbflNTYGAN<_0002, _0003>, new() where _0002 : struct where _0003 : struct, CQLGJWnqRRTjXzVQgIYtWAvLPRYO
{
	private lqsEFGvGeAeizRYAFkQDyzxyOJRL wygEbfDYFyvIcMymHEMrIyjIWBqcA;

	private readonly Dictionary<string, hWVWhsQejgFPmATIkqbBBZrEFzrcb> ITINXvpSExsUUCBweKaiNjbAGziL = new Dictionary<string, hWVWhsQejgFPmATIkqbBBZrEFzrcb>();

	private static readonly _0003[] XcMkzSEyyndodxXvRKsMzSGjlxKe = new _0003[0];

	protected nImBOxGXHgmXysvhfFkpHCPHfbb(BKiqqPDcNrfyFbgcenapmsdywauK P_0, Guid P_1)
		: base(P_0, P_1)
	{
		lqsEFGvGeAeizRYAFkQDyzxyOJRL lqsEFGvGeAeizRYAFkQDyzxyOJRL2 = RpRJYgIHNFzxkIEYKlQrbGazlMjf();
		ACaFMQQahPtUNreKAmfdEgnRDlAv(lqsEFGvGeAeizRYAFkQDyzxyOJRL2);
	}

	public void CPzpQOSfMBjaUAymHmyAJivJMMCnA(_0001 P_0)
	{
		hbDksYfqzKDrbdMhjndCKiYKCBOEb(ref P_0);
	}

	public _0001 qRHfOwVKbSwGSZpKOGbwsnhTzrsU()
	{
		_0001 result = new _0001();
		hbDksYfqzKDrbdMhjndCKiYKCBOEb(ref result);
		return result;
	}

	public unsafe void hbDksYfqzKDrbdMhjndCKiYKCBOEb(ref _0001 P_0)
	{
		int num = luYaFPaftNInTWGPWfCvgYuDUqDyA.glwGxVPzunhdOUxGIRKtVKPYTQvO<_0002>();
		byte* ptr = stackalloc byte[(int)(uint)(num * 2)];
		EGVClbavWKiTSXSCSriVquOjKqftA(num, (IntPtr)ptr);
		P_0.OQlPvrYnsHapIExslxgKNQUZlorYA((IntPtr)ptr);
	}

	private lqsEFGvGeAeizRYAFkQDyzxyOJRL RpRJYgIHNFzxkIEYKlQrbGazlMjf()
	{
		if (wygEbfDYFyvIcMymHEMrIyjIWBqcA == null)
		{
			if (typeof(psUUzNeLrjgenCTnfoFwdojFrHH).IsAssignableFrom(typeof(_0002)))
			{
				psUUzNeLrjgenCTnfoFwdojFrHH psUUzNeLrjgenCTnfoFwdojFrHH2 = (psUUzNeLrjgenCTnfoFwdojFrHH)(object)new _0002();
				wygEbfDYFyvIcMymHEMrIyjIWBqcA = new lqsEFGvGeAeizRYAFkQDyzxyOJRL(psUUzNeLrjgenCTnfoFwdojFrHH2.KIuQyhnpOztKDHksFSuESlcKoQUq)
				{
					tDwvaHEmpftYXgYzYZSczXYySoeg = luYaFPaftNInTWGPWfCvgYuDUqDyA.glwGxVPzunhdOUxGIRKtVKPYTQvO<_0002>(),
					msHlyRwUUJbfBhpzcEZccYNuVKIUA = psUUzNeLrjgenCTnfoFwdojFrHH2.rYOcHgIbNlBBHJnrOljYAqazDfFs
				};
			}
			else
			{
				object[] customAttributes = typeof(_0002).GetCustomAttributes(typeof(fVRcDUKECDpEiyKWWhAWuPmcoROo), inherit: false);
				if (customAttributes.Length != 1)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The structure [{0}] must be marked with DataFormatAttribute or provide a IDataFormatProvider", typeof(_0002).FullName));
				}
				wygEbfDYFyvIcMymHEMrIyjIWBqcA = new lqsEFGvGeAeizRYAFkQDyzxyOJRL(((fVRcDUKECDpEiyKWWhAWuPmcoROo)customAttributes[0]).YJxjSOGKobWsKNDGmvifWjsseEax)
				{
					tDwvaHEmpftYXgYzYZSczXYySoeg = luYaFPaftNInTWGPWfCvgYuDUqDyA.glwGxVPzunhdOUxGIRKtVKPYTQvO<_0002>()
				};
				List<hWVWhsQejgFPmATIkqbBBZrEFzrcb> list = new List<hWVWhsQejgFPmATIkqbBBZrEFzrcb>();
				FieldInfo[] fields = typeof(_0002).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (FieldInfo fieldInfo in fields)
				{
					object[] customAttributes2 = fieldInfo.GetCustomAttributes(typeof(mBPBcxXsWephbbwGEvfsGMORiFiE), inherit: false);
					if (customAttributes2.Length == 0)
					{
						continue;
					}
					int num = Marshal.OffsetOf(typeof(_0002), fieldInfo.Name).ToInt32();
					int num2 = Marshal.SizeOf(fieldInfo.FieldType);
					int num3 = num;
					int num4 = 0;
					for (int j = 0; j < customAttributes2.Length; j++)
					{
						mBPBcxXsWephbbwGEvfsGMORiFiE mBPBcxXsWephbbwGEvfsGMORiFiE2 = (mBPBcxXsWephbbwGEvfsGMORiFiE)customAttributes2[j];
						num4 += ((mBPBcxXsWephbbwGEvfsGMORiFiE2.qOTaLciaGHCkCPfryiuhWINGMXDkA == 0) ? 1 : mBPBcxXsWephbbwGEvfsGMORiFiE2.qOTaLciaGHCkCPfryiuhWINGMXDkA);
					}
					int num5 = num2 / num4;
					if (num5 * num4 != num2)
					{
						throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Field [{0}] has incompatible size [{1}] and number of DataObjectAttributes [{2}]", fieldInfo.Name, (double)num2 / (double)num4, num4));
					}
					int num6 = 0;
					for (int k = 0; k < customAttributes2.Length; k++)
					{
						mBPBcxXsWephbbwGEvfsGMORiFiE mBPBcxXsWephbbwGEvfsGMORiFiE3 = (mBPBcxXsWephbbwGEvfsGMORiFiE)customAttributes2[k];
						num4 = ((mBPBcxXsWephbbwGEvfsGMORiFiE3.qOTaLciaGHCkCPfryiuhWINGMXDkA == 0) ? 1 : mBPBcxXsWephbbwGEvfsGMORiFiE3.qOTaLciaGHCkCPfryiuhWINGMXDkA);
						for (int l = 0; l < num4; l++)
						{
							hWVWhsQejgFPmATIkqbBBZrEFzrcb hWVWhsQejgFPmATIkqbBBZrEFzrcb2 = new hWVWhsQejgFPmATIkqbBBZrEFzrcb(string.IsNullOrEmpty(mBPBcxXsWephbbwGEvfsGMORiFiE3.KdsnEUZJmQuaTvzpHgjVaBPueVLH) ? Guid.Empty : new Guid(mBPBcxXsWephbbwGEvfsGMORiFiE3.KdsnEUZJmQuaTvzpHgjVaBPueVLH), num3, mBPBcxXsWephbbwGEvfsGMORiFiE3.JLVbKbjsNzgyBlMRaCwGDbjJbKymc, mBPBcxXsWephbbwGEvfsGMORiFiE3.AhXFEsumBrUVdvvhZpluyHrLUbHm, mBPBcxXsWephbbwGEvfsGMORiFiE3.RpzwNqQGWaGoynwnkjNhXnuhUvnf);
							string text = (string.IsNullOrEmpty(mBPBcxXsWephbbwGEvfsGMORiFiE3.uBElPjbOaIOwlYENuWQblnhGOYaT) ? fieldInfo.Name : mBPBcxXsWephbbwGEvfsGMORiFiE3.uBElPjbOaIOwlYENuWQblnhGOYaT);
							text = ((num4 == 1) ? text : (text + num6));
							hWVWhsQejgFPmATIkqbBBZrEFzrcb2.BpxWAnijsxTTRWWQZjjdqgtJhgYA = text;
							list.Add(hWVWhsQejgFPmATIkqbBBZrEFzrcb2);
							num3 += num5;
							num6++;
						}
					}
				}
				wygEbfDYFyvIcMymHEMrIyjIWBqcA.msHlyRwUUJbfBhpzcEZccYNuVKIUA = list.ToArray();
			}
			for (int m = 0; m < wygEbfDYFyvIcMymHEMrIyjIWBqcA.msHlyRwUUJbfBhpzcEZccYNuVKIUA.Length; m++)
			{
				hWVWhsQejgFPmATIkqbBBZrEFzrcb hWVWhsQejgFPmATIkqbBBZrEFzrcb3 = wygEbfDYFyvIcMymHEMrIyjIWBqcA.msHlyRwUUJbfBhpzcEZccYNuVKIUA[m];
				if (ITINXvpSExsUUCBweKaiNjbAGziL.ContainsKey(hWVWhsQejgFPmATIkqbBBZrEFzrcb3.BpxWAnijsxTTRWWQZjjdqgtJhgYA))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Incorrect field name [{0}]. Field name must be unique", hWVWhsQejgFPmATIkqbBBZrEFzrcb3.BpxWAnijsxTTRWWQZjjdqgtJhgYA));
				}
				ITINXvpSExsUUCBweKaiNjbAGziL.Add(hWVWhsQejgFPmATIkqbBBZrEFzrcb3.BpxWAnijsxTTRWWQZjjdqgtJhgYA, hWVWhsQejgFPmATIkqbBBZrEFzrcb3);
			}
		}
		return wygEbfDYFyvIcMymHEMrIyjIWBqcA;
	}
}
