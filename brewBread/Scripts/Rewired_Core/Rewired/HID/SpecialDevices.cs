namespace Rewired.HID
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal static class SpecialDevices
	{
		private class tAHDWRkJoziOmKcYBZsvcvfLAengb
		{
			public readonly ushort kUHHTIgTMDyMEXufsUvSHmaExSkL;

			public readonly ushort IghITlQqAHYzeQDzcXCztBBmmcAl;

			public readonly string jxKZpZUINkwFwLQZujwACzNlaNSm;

			public readonly bool vjpNeYUtHjCRXTGXvWPCBxswfqNM;

			public readonly int IlkvwxakuwfhTumITTFTfyzRuigt;

			public readonly int DoNfIgKqVAFoMBwXVKAqTkbpBYZMA;

			public readonly int PwZsgYmImOWQkpTqhTVXCIsXytmG;

			public readonly float TpSdduBsPZggpIRDCvyWVHUSRFxyB;

			public tAHDWRkJoziOmKcYBZsvcvfLAengb(ushort P_0, ushort P_1, string P_2, bool P_3, int P_4, int P_5, int P_6, float P_7)
			{
				kUHHTIgTMDyMEXufsUvSHmaExSkL = P_0;
				IghITlQqAHYzeQDzcXCztBBmmcAl = P_1;
				if (string.IsNullOrEmpty(P_2))
				{
					P_2 = string.Empty;
				}
				jxKZpZUINkwFwLQZujwACzNlaNSm = P_2;
				vjpNeYUtHjCRXTGXvWPCBxswfqNM = P_3;
				IlkvwxakuwfhTumITTFTfyzRuigt = P_4;
				DoNfIgKqVAFoMBwXVKAqTkbpBYZMA = P_5;
				PwZsgYmImOWQkpTqhTVXCIsXytmG = P_6;
				TpSdduBsPZggpIRDCvyWVHUSRFxyB = P_7;
			}

			public bool szFcrGQfdusgxVHjXepjIOCnhMzoA(ushort P_0, ushort P_1)
			{
				if (kUHHTIgTMDyMEXufsUvSHmaExSkL == P_0)
				{
					return IghITlQqAHYzeQDzcXCztBBmmcAl == P_1;
				}
				return false;
			}

			public bool szFcrGQfdusgxVHjXepjIOCnhMzoA(ushort P_0, ushort P_1, string P_2)
			{
				if (kUHHTIgTMDyMEXufsUvSHmaExSkL != P_0 || IghITlQqAHYzeQDzcXCztBBmmcAl != P_1)
				{
					if (!string.IsNullOrEmpty(P_2))
					{
						return jxKZpZUINkwFwLQZujwACzNlaNSm == P_2;
					}
					return false;
				}
				return true;
			}

			public bool szFcrGQfdusgxVHjXepjIOCnhMzoA(string P_0)
			{
				if (!string.IsNullOrEmpty(P_0))
				{
					return jxKZpZUINkwFwLQZujwACzNlaNSm == P_0;
				}
				return false;
			}
		}

		private const float pDHqLaNEVuUDXiOCTKReKHiUajLS = 0.034f;

		private static tAHDWRkJoziOmKcYBZsvcvfLAengb[] WVsTOQOJDzIvotDPBqfeGdkiBxkn = new tAHDWRkJoziOmKcYBZsvcvfLAengb[3]
		{
			new tAHDWRkJoziOmKcYBZsvcvfLAengb(1133, 50726, "SpaceNavigator", true, -350, 350, 0, 0.034f),
			new tAHDWRkJoziOmKcYBZsvcvfLAengb(1133, 50728, "SpaceNavigator for Notebooks", true, -350, 350, 0, 0.034f),
			new tAHDWRkJoziOmKcYBZsvcvfLAengb(1133, 50727, "Space Explorer", true, -350, 350, 0, 0.034f)
		};

		public static bool RequiresRelativeToAbsoluteAxisConversion(ushort vendorId, ushort productId, string productName = null)
		{
			return mONALEVwKkoZxyESqdEvshAuAVnA(vendorId, productId, productName)?.vjpNeYUtHjCRXTGXvWPCBxswfqNM ?? false;
		}

		public static float GetRelativeToAbsoluteAxisEventTimeout(ushort vendorId, ushort productId, string productName = null)
		{
			return mONALEVwKkoZxyESqdEvshAuAVnA(vendorId, productId, productName)?.TpSdduBsPZggpIRDCvyWVHUSRFxyB ?? 0f;
		}

		public static bool GetRelativeAxisRanges(ushort vendorId, ushort productId, out int min, out int max, out int zero)
		{
			return GetRelativeAxisRanges(vendorId, productId, null, out min, out max, out zero);
		}

		public static bool GetRelativeAxisRanges(ushort vendorId, ushort productId, string productName, out int min, out int max, out int zero)
		{
			for (int i = 0; i < WVsTOQOJDzIvotDPBqfeGdkiBxkn.Length; i++)
			{
				if (WVsTOQOJDzIvotDPBqfeGdkiBxkn[i].szFcrGQfdusgxVHjXepjIOCnhMzoA(vendorId, productId) && WVsTOQOJDzIvotDPBqfeGdkiBxkn[i].vjpNeYUtHjCRXTGXvWPCBxswfqNM)
				{
					min = WVsTOQOJDzIvotDPBqfeGdkiBxkn[i].IlkvwxakuwfhTumITTFTfyzRuigt;
					max = WVsTOQOJDzIvotDPBqfeGdkiBxkn[i].DoNfIgKqVAFoMBwXVKAqTkbpBYZMA;
					zero = WVsTOQOJDzIvotDPBqfeGdkiBxkn[i].PwZsgYmImOWQkpTqhTVXCIsXytmG;
					return true;
				}
			}
			min = 0;
			max = 0;
			zero = 0;
			return false;
		}

		public static bool IsSupportedSpecialDevice(ushort vendorId, ushort productId, string productName = null)
		{
			return aUknxRCuChPVzdoSMlJITQkscDTt(vendorId, productId, productName);
		}

		private static bool aUknxRCuChPVzdoSMlJITQkscDTt(ushort P_0, ushort P_1, string P_2 = null)
		{
			for (int i = 0; i < WVsTOQOJDzIvotDPBqfeGdkiBxkn.Length; i++)
			{
				if (WVsTOQOJDzIvotDPBqfeGdkiBxkn[i].szFcrGQfdusgxVHjXepjIOCnhMzoA(P_0, P_1, P_2))
				{
					return true;
				}
			}
			return false;
		}

		private static tAHDWRkJoziOmKcYBZsvcvfLAengb mONALEVwKkoZxyESqdEvshAuAVnA(ushort P_0, ushort P_1, string P_2 = null)
		{
			for (int i = 0; i < WVsTOQOJDzIvotDPBqfeGdkiBxkn.Length; i++)
			{
				if (WVsTOQOJDzIvotDPBqfeGdkiBxkn[i].szFcrGQfdusgxVHjXepjIOCnhMzoA(P_0, P_1, P_2))
				{
					return WVsTOQOJDzIvotDPBqfeGdkiBxkn[i];
				}
			}
			return null;
		}
	}
}
