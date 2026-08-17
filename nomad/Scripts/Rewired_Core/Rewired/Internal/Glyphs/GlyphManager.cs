using System;
using System.Text;
using Rewired.Interfaces;
using Rewired.Internal.Localization;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Interfaces;

namespace Rewired.Internal.Glyphs
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal static class GlyphManager
	{
		private sealed class UZSCgxtboQkWQlgCSqPmIFGKXJYO
		{
			[Serializable]
			private sealed class sUUkccLUJFbFYDSPLaZWDouQbyUb
			{
				public static readonly sUUkccLUJFbFYDSPLaZWDouQbyUb _003C_003E9 = new sUUkccLUJFbFYDSPLaZWDouQbyUb();

				public static Action<IPrefetch> _003C_003E9__7_0;

				internal void qCWFbagRDIsSPukWJFzWZYNWzWkqA(IPrefetch P_0)
				{
					P_0.Prefetch();
				}
			}

			private const float damCVXGygIglSTZWuPmgKdSdAbDZA = 60f;

			private readonly global::JBObjUemFLdkzOFbxqzCefghedsSc<IPrefetch> omAmnbNjPwTXlVRkPRUqTSwlHkzn;

			public bool yOjorhuSgvHEaCwyQvFtszgdRWTi;

			public IGlyphProvider RLcVklofcyvvxzdRefISUwyYfliy;

			public uint vCVIHBKmPzXGzIywOWmNmyYgzqqdA;

			private Action<IPrefetch> UintoOKaSVgrsEpjPkhAzhlCdIHz = sUUkccLUJFbFYDSPLaZWDouQbyUb._003C_003E9.qCWFbagRDIsSPukWJFzWZYNWzWkqA;

			private Id OdfkuOsXgaxHLOURcBXxEJQzNfGA;

			public UZSCgxtboQkWQlgCSqPmIFGKXJYO()
			{
				omAmnbNjPwTXlVRkPRUqTSwlHkzn = new global::JBObjUemFLdkzOFbxqzCefghedsSc<IPrefetch>(60f);
				vCVIHBKmPzXGzIywOWmNmyYgzqqdA = 0u;
				OdfkuOsXgaxHLOURcBXxEJQzNfGA = 1u;
			}

			public void kkRoZsNlHSHMJxKrvUiqbrwkWbxb(IGlyphProvider P_0)
			{
				RLcVklofcyvvxzdRefISUwyYfliy = P_0;
				if (P_0 != null)
				{
					vCVIHBKmPzXGzIywOWmNmyYgzqqdA = OdfkuOsXgaxHLOURcBXxEJQzNfGA.id;
					OdfkuOsXgaxHLOURcBXxEJQzNfGA.Increment();
				}
				else
				{
					vCVIHBKmPzXGzIywOWmNmyYgzqqdA = 0u;
				}
				fUJdkiphTIeZzUgRnEMNMAUafVum();
			}

			public void NYPyzfGAJnpTrvzzpsITqOnUgTxw(bool P_0)
			{
				if (P_0 != yOjorhuSgvHEaCwyQvFtszgdRWTi)
				{
					yOjorhuSgvHEaCwyQvFtszgdRWTi = P_0;
					if (P_0)
					{
						dQKDwKpucVAZXQAnxTaKXHAVIEmv();
					}
				}
			}

			public void dQKDwKpucVAZXQAnxTaKXHAVIEmv()
			{
				if (RLcVklofcyvvxzdRefISUwyYfliy != null)
				{
					omAmnbNjPwTXlVRkPRUqTSwlHkzn.zitygvTOkbnKqqpSWWQDpLAXkwTP(UintoOKaSVgrsEpjPkhAzhlCdIHz);
				}
			}

			public void yczbZLBKeeVYZHWBkbJQUjHXGlQUA()
			{
				if (RLcVklofcyvvxzdRefISUwyYfliy != null)
				{
					vCVIHBKmPzXGzIywOWmNmyYgzqqdA = OdfkuOsXgaxHLOURcBXxEJQzNfGA.id;
					OdfkuOsXgaxHLOURcBXxEJQzNfGA.Increment();
					if (yOjorhuSgvHEaCwyQvFtszgdRWTi)
					{
						dQKDwKpucVAZXQAnxTaKXHAVIEmv();
					}
					else
					{
						fUJdkiphTIeZzUgRnEMNMAUafVum();
					}
				}
			}

			public uint lMNllWAwFLMDFCqyhfxOfvzHXWuuA(IPrefetch P_0)
			{
				return omAmnbNjPwTXlVRkPRUqTSwlHkzn.sJQDtVtbrpkDDcVlnWqHJwMgozbN(P_0);
			}

			public bool furWbrrhFtILwHUIDDxtCGFqaoWlA(uint P_0)
			{
				return omAmnbNjPwTXlVRkPRUqTSwlHkzn.hexsubMGcGqoTRCNpeAuAKJQpCdz(P_0);
			}

			public void fUJdkiphTIeZzUgRnEMNMAUafVum()
			{
				omAmnbNjPwTXlVRkPRUqTSwlHkzn.PecQYXbrNbodqHbeNaKQJKgkJILn();
			}
		}

		[CustomObfuscation(rename = false)]
		public enum GetAndUpdateGlyphResultFlags
		{
			None = 0,
			Failed = 1,
			IsCachedValue = 2,
			Changed = 4,
			JustGot = 8
		}

		private static UZSCgxtboQkWQlgCSqPmIFGKXJYO sDLdMDITeRZiWERhUbJUjetgMVjFb;

		private static StringBuilder rtDvprKyVufAKiMgVQITekHNdHZbA;

		public static bool isEnabled
		{
			get
			{
				if (sDLdMDITeRZiWERhUbJUjetgMVjFb == null)
				{
					return false;
				}
				return sDLdMDITeRZiWERhUbJUjetgMVjFb.RLcVklofcyvvxzdRefISUwyYfliy != null;
			}
		}

		public static uint version
		{
			get
			{
				qDbnGFBsoZjlRvTHmjaFAdmMrbjLA();
				return sDLdMDITeRZiWERhUbJUjetgMVjFb.vCVIHBKmPzXGzIywOWmNmyYgzqqdA;
			}
		}

		public static IGlyphProvider glyphProvider
		{
			get
			{
				qDbnGFBsoZjlRvTHmjaFAdmMrbjLA();
				return sDLdMDITeRZiWERhUbJUjetgMVjFb.RLcVklofcyvvxzdRefISUwyYfliy;
			}
			set
			{
				qDbnGFBsoZjlRvTHmjaFAdmMrbjLA();
				sDLdMDITeRZiWERhUbJUjetgMVjFb.kkRoZsNlHSHMJxKrvUiqbrwkWbxb(value);
			}
		}

		public static bool autoPrefetch
		{
			get
			{
				qDbnGFBsoZjlRvTHmjaFAdmMrbjLA();
				return sDLdMDITeRZiWERhUbJUjetgMVjFb.yOjorhuSgvHEaCwyQvFtszgdRWTi;
			}
			set
			{
				qDbnGFBsoZjlRvTHmjaFAdmMrbjLA();
				sDLdMDITeRZiWERhUbJUjetgMVjFb.NYPyzfGAJnpTrvzzpsITqOnUgTxw(value);
			}
		}

		public static void Initialize()
		{
			if (sDLdMDITeRZiWERhUbJUjetgMVjFb != null)
			{
				throw new Exception("Already initialized");
			}
			sDLdMDITeRZiWERhUbJUjetgMVjFb = new UZSCgxtboQkWQlgCSqPmIFGKXJYO();
		}

		public static void Deinitialize()
		{
			if (sDLdMDITeRZiWERhUbJUjetgMVjFb != null)
			{
				sDLdMDITeRZiWERhUbJUjetgMVjFb = null;
			}
		}

		public static void Add(IPrefetch obj, ref Id id)
		{
			qDbnGFBsoZjlRvTHmjaFAdmMrbjLA();
			id = sDLdMDITeRZiWERhUbJUjetgMVjFb.lMNllWAwFLMDFCqyhfxOfvzHXWuuA(obj);
		}

		public static bool Remove(ref Id id)
		{
			qDbnGFBsoZjlRvTHmjaFAdmMrbjLA();
			bool result = sDLdMDITeRZiWERhUbJUjetgMVjFb.furWbrrhFtILwHUIDDxtCGFqaoWlA(id);
			id = 0u;
			return result;
		}

		public static void Prefetch()
		{
			qDbnGFBsoZjlRvTHmjaFAdmMrbjLA();
			sDLdMDITeRZiWERhUbJUjetgMVjFb.dQKDwKpucVAZXQAnxTaKXHAVIEmv();
		}

		public static void Reload()
		{
			qDbnGFBsoZjlRvTHmjaFAdmMrbjLA();
			sDLdMDITeRZiWERhUbJUjetgMVjFb.yczbZLBKeeVYZHWBkbJQUjHXGlQUA();
		}

		private static void qDbnGFBsoZjlRvTHmjaFAdmMrbjLA()
		{
			if (sDLdMDITeRZiWERhUbJUjetgMVjFb == null)
			{
				throw new Exception(typeof(GlyphManager).Name + " is not initialized.");
			}
		}

		public static bool TryGetCachedGlyph(KeyedGlyph keyedGlyph, uint glyphProviderVersion, uint dependenciesVersion, out bool glyphProviderVersionChanged, out object result)
		{
			if (sDLdMDITeRZiWERhUbJUjetgMVjFb.RLcVklofcyvvxzdRefISUwyYfliy != null)
			{
				return keyedGlyph.TryGetValue(null, sDLdMDITeRZiWERhUbJUjetgMVjFb.RLcVklofcyvvxzdRefISUwyYfliy, sDLdMDITeRZiWERhUbJUjetgMVjFb.vCVIHBKmPzXGzIywOWmNmyYgzqqdA, dependenciesVersion, out glyphProviderVersionChanged, out result);
			}
			result = null;
			glyphProviderVersionChanged = false;
			return false;
		}

		public static bool TryGetGlyph(KeyedGlyph keyedGlyph, string key, uint glyphProviderVersion, uint dependenciesVersion, out object result)
		{
			if (sDLdMDITeRZiWERhUbJUjetgMVjFb.RLcVklofcyvvxzdRefISUwyYfliy != null)
			{
				keyedGlyph.Clear();
				bool versionChanged;
				return keyedGlyph.TryGetValue(key, sDLdMDITeRZiWERhUbJUjetgMVjFb.RLcVklofcyvvxzdRefISUwyYfliy, sDLdMDITeRZiWERhUbJUjetgMVjFb.vCVIHBKmPzXGzIywOWmNmyYgzqqdA, dependenciesVersion, out versionChanged, out result);
			}
			result = null;
			return false;
		}

		public static GetAndUpdateGlyphResultFlags GetAndUpdateGlyph(KeyedGlyph keyedGlyph, IReadOnlyList<string> parentKeys, string keyCategory, out object result)
		{
			if (sDLdMDITeRZiWERhUbJUjetgMVjFb.RLcVklofcyvvxzdRefISUwyYfliy == null)
			{
				result = null;
				return GetAndUpdateGlyphResultFlags.Failed;
			}
			GetAndUpdateGlyphResultFlags getAndUpdateGlyphResultFlags = ((!TryGetCachedGlyph(keyedGlyph, sDLdMDITeRZiWERhUbJUjetgMVjFb.vCVIHBKmPzXGzIywOWmNmyYgzqqdA, 0u, out var glyphProviderVersionChanged, out result)) ? GetAndUpdateGlyphResultFlags.Failed : GetAndUpdateGlyphResultFlags.IsCachedValue);
			if (!keyedGlyph.hasCachedValue || glyphProviderVersionChanged)
			{
				getAndUpdateGlyphResultFlags |= GetAndUpdateGlyphResultFlags.Changed;
				if (ZGpGUOEEjiFndUXXMDnaAdVbKvUb(keyedGlyph, parentKeys, keyCategory, out result))
				{
					getAndUpdateGlyphResultFlags |= GetAndUpdateGlyphResultFlags.JustGot;
					getAndUpdateGlyphResultFlags &= (GetAndUpdateGlyphResultFlags)(-2);
				}
				else
				{
					getAndUpdateGlyphResultFlags |= GetAndUpdateGlyphResultFlags.Failed;
				}
			}
			return getAndUpdateGlyphResultFlags;
		}

		public static GetAndUpdateGlyphResultFlags GetAndUpdateGlyph(KeyedGlyph keyedGlyph, string key, string keyCategory, IReadOnlyList<string> parentKeys, out object result)
		{
			if (sDLdMDITeRZiWERhUbJUjetgMVjFb.RLcVklofcyvvxzdRefISUwyYfliy == null)
			{
				result = null;
				return GetAndUpdateGlyphResultFlags.Failed;
			}
			GetAndUpdateGlyphResultFlags getAndUpdateGlyphResultFlags = ((!TryGetCachedGlyph(keyedGlyph, sDLdMDITeRZiWERhUbJUjetgMVjFb.vCVIHBKmPzXGzIywOWmNmyYgzqqdA, 0u, out var glyphProviderVersionChanged, out result)) ? GetAndUpdateGlyphResultFlags.Failed : GetAndUpdateGlyphResultFlags.IsCachedValue);
			if (!keyedGlyph.hasCachedValue || glyphProviderVersionChanged)
			{
				getAndUpdateGlyphResultFlags |= GetAndUpdateGlyphResultFlags.Changed;
				if (xVlFtsYevTBnKBhnDbRpWxFENikKA(keyedGlyph, key, keyCategory, parentKeys, out result))
				{
					getAndUpdateGlyphResultFlags |= GetAndUpdateGlyphResultFlags.JustGot;
					getAndUpdateGlyphResultFlags &= (GetAndUpdateGlyphResultFlags)(-2);
				}
				else
				{
					getAndUpdateGlyphResultFlags |= GetAndUpdateGlyphResultFlags.Failed;
				}
			}
			return getAndUpdateGlyphResultFlags;
		}

		private static bool ZGpGUOEEjiFndUXXMDnaAdVbKvUb(KeyedGlyph P_0, IReadOnlyList<string> P_1, string P_2, out object P_3)
		{
			if (P_1 == null)
			{
				P_3 = null;
				return false;
			}
			bool result = false;
			bool flag = !string.IsNullOrEmpty(P_2);
			StringBuilder sharedStringBuilder = GetSharedStringBuilder();
			int num = 0;
			while (true)
			{
				if (num < P_1.Count)
				{
					if (!string.IsNullOrEmpty(P_1[num]))
					{
						sharedStringBuilder.Length = 0;
						if (flag)
						{
							sharedStringBuilder.Append(P_2);
						}
						LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, P_1[num]);
						if (TryGetGlyph(P_0, sharedStringBuilder.ToString(), sDLdMDITeRZiWERhUbJUjetgMVjFb.vCVIHBKmPzXGzIywOWmNmyYgzqqdA, 0u, out P_3))
						{
							result = true;
							break;
						}
					}
					num++;
					continue;
				}
				P_3 = null;
				break;
			}
			P_0.cachedValue = P_3;
			return result;
		}

		private static bool xVlFtsYevTBnKBhnDbRpWxFENikKA(KeyedGlyph P_0, string P_1, string P_2, IReadOnlyList<string> P_3, out object P_4)
		{
			if (string.IsNullOrEmpty(P_1))
			{
				P_4 = null;
				return false;
			}
			bool result = false;
			uint dependenciesVersion = 0u;
			bool flag = !string.IsNullOrEmpty(P_2);
			StringBuilder sharedStringBuilder = GetSharedStringBuilder();
			if (P_3 != null)
			{
				for (int i = 0; i < P_3.Count; i++)
				{
					if (string.IsNullOrEmpty(P_3[i]))
					{
						continue;
					}
					sharedStringBuilder.Length = 0;
					if (flag)
					{
						sharedStringBuilder.Append(P_2);
					}
					LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, P_3[i]);
					LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, P_1);
					if (!TryGetGlyph(P_0, sharedStringBuilder.ToString(), sDLdMDITeRZiWERhUbJUjetgMVjFb.vCVIHBKmPzXGzIywOWmNmyYgzqqdA, dependenciesVersion, out P_4))
					{
						continue;
					}
					goto IL_007d;
				}
			}
			if (P_3 == null || P_3.Count == 0)
			{
				sharedStringBuilder.Length = 0;
				if (flag)
				{
					sharedStringBuilder.Append(P_2);
				}
				LocalizationManager.AppendToKeyAsPath(sharedStringBuilder, P_1);
				if (TryGetGlyph(P_0, sharedStringBuilder.ToString(), sDLdMDITeRZiWERhUbJUjetgMVjFb.vCVIHBKmPzXGzIywOWmNmyYgzqqdA, dependenciesVersion, out P_4))
				{
					result = true;
					goto IL_00d9;
				}
			}
			P_4 = null;
			goto IL_00d9;
			IL_00d9:
			P_0.cachedValue = P_4;
			return result;
			IL_007d:
			result = true;
			goto IL_00d9;
		}

		[CustomObfuscation(rename = false)]
		public static StringBuilder GetSharedStringBuilder()
		{
			if (rtDvprKyVufAKiMgVQITekHNdHZbA != null)
			{
				rtDvprKyVufAKiMgVQITekHNdHZbA.Length = 0;
				return rtDvprKyVufAKiMgVQITekHNdHZbA;
			}
			return rtDvprKyVufAKiMgVQITekHNdHZbA = new StringBuilder();
		}
	}
}
