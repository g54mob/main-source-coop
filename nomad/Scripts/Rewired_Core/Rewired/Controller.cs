using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Internal.Localization;
using Rewired.Utils;
using UnityEngine;

namespace Rewired
{
	public abstract class Controller
	{
		public abstract class Element
		{
			internal abstract class tscibVDDGPsZUjSdSrflLekbJWIv
			{
				public abstract class qovUnvqKzupWVYriIrejjnERcAZjA
				{
					public abstract void AeZTHskpKDEKtDlGzhZniDIlNHXd();
				}

				protected readonly int MGwKVdZqESiwLYNKuhSByNdJdqVBA;

				protected readonly int[] NJfuvXGngFKbErEgrjdMKzrjwjCHA;

				protected qovUnvqKzupWVYriIrejjnERcAZjA[] MbKOWzMIZetTDynPwZmLtYcisFpL;

				public qovUnvqKzupWVYriIrejjnERcAZjA WTGpEIcYIUDuOPUdjlxKVotKgWAs;

				private int sXHNlNJDikaDhSYjWUxdSKBkemqN;

				public int bsPrFCcBdhNSOJSWNBQqsWcxLyMP = -1;

				protected ReadOnlyCollection<qovUnvqKzupWVYriIrejjnERcAZjA> nsYbHaHygwdetXzBGyrTGZfZXXgjA;

				public IList<qovUnvqKzupWVYriIrejjnERcAZjA> agkPIJyvbYFLtBrPOLckOXSeJSrE => nsYbHaHygwdetXzBGyrTGZfZXXgjA;

				public UpdateLoopType jGsfShJhPkFMDimHUUyjynvDZVWIA
				{
					set
					{
						if (bsPrFCcBdhNSOJSWNBQqsWcxLyMP != (int)updateLoopType)
						{
							bsPrFCcBdhNSOJSWNBQqsWcxLyMP = (int)updateLoopType;
							sXHNlNJDikaDhSYjWUxdSKBkemqN = NJfuvXGngFKbErEgrjdMKzrjwjCHA[(int)updateLoopType];
							WTGpEIcYIUDuOPUdjlxKVotKgWAs = MbKOWzMIZetTDynPwZmLtYcisFpL[sXHNlNJDikaDhSYjWUxdSKBkemqN];
						}
					}
				}

				public tscibVDDGPsZUjSdSrflLekbJWIv(UpdateLoopSetting P_0)
				{
					NJfuvXGngFKbErEgrjdMKzrjwjCHA = new int[3];
					MGwKVdZqESiwLYNKuhSByNdJdqVBA = 0;
					using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
					{
						List<UpdateLoopType> list = tList.list;
						EnumConverter.ToUpdateLoopTypes(P_0, list);
						for (int i = 0; i < list.Count; i++)
						{
							NJfuvXGngFKbErEgrjdMKzrjwjCHA[(int)list[i]] = MGwKVdZqESiwLYNKuhSByNdJdqVBA;
							MGwKVdZqESiwLYNKuhSByNdJdqVBA++;
						}
					}
					MbKOWzMIZetTDynPwZmLtYcisFpL = new qovUnvqKzupWVYriIrejjnERcAZjA[MGwKVdZqESiwLYNKuhSByNdJdqVBA];
					nsYbHaHygwdetXzBGyrTGZfZXXgjA = new ReadOnlyCollection<qovUnvqKzupWVYriIrejjnERcAZjA>(MbKOWzMIZetTDynPwZmLtYcisFpL);
				}

				public void MPKIFtFtDrSDEoYRATBwwEBVXpfQ()
				{
					for (int i = 0; i < MGwKVdZqESiwLYNKuhSByNdJdqVBA; i++)
					{
						MbKOWzMIZetTDynPwZmLtYcisFpL[i].AeZTHskpKDEKtDlGzhZniDIlNHXd();
					}
				}

				public qovUnvqKzupWVYriIrejjnERcAZjA LzQVRMRtuvCFGyZrxviHawyAsjQb(UpdateLoopType P_0)
				{
					return MbKOWzMIZetTDynPwZmLtYcisFpL[NJfuvXGngFKbErEgrjdMKzrjwjCHA[(int)P_0]];
				}
			}

			public readonly int id;

			public readonly string name;

			public readonly ControllerElementType type;

			internal tscibVDDGPsZUjSdSrflLekbJWIv dRBUWtmcBTRxhtDGaawyVTtryZJE;

			internal int hpXTymXJxGCOVLSxjNdGVAoLMVBC;

			internal Controller TrpRuvlQuUTvlONxcFCzTxhTsSlf;

			internal readonly int wspMDQsHjWUvnnwRnuvUTkcjyQSk;

			private CompoundElement ZaKjOwCNbIcOdMDWCMzTeiXEIhkd;

			private bool hnrDonQenontZrsigPzHXIoHJRTL;

			public ControllerElementIdentifier elementIdentifier
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return null;
					}
					ControllerElementIdentifier elementIdentifierById = TrpRuvlQuUTvlONxcFCzTxhTsSlf.GetElementIdentifierById(id);
					if (elementIdentifierById == null)
					{
						return ControllerElementIdentifier.BlankReadOnly;
					}
					return elementIdentifierById;
				}
			}

			public virtual bool excludeFromPolling
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return false;
					}
					return hnrDonQenontZrsigPzHXIoHJRTL;
				}
				set
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
					}
					else
					{
						hnrDonQenontZrsigPzHXIoHJRTL = value;
					}
				}
			}

			public bool isMemberElement
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return false;
					}
					return hpXTymXJxGCOVLSxjNdGVAoLMVBC > 0;
				}
			}

			public CompoundElement compoundElement => ZaKjOwCNbIcOdMDWCMzTeiXEIhkd;

			internal Element(Controller P_0, int P_1, string P_2, ControllerElementType P_3)
			{
				TrpRuvlQuUTvlONxcFCzTxhTsSlf = P_0;
				id = P_1;
				name = P_2;
				type = P_3;
				wspMDQsHjWUvnnwRnuvUTkcjyQSk = ReInput.id;
			}

			public void Reset()
			{
				if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
				{
					ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
				}
				else if (dRBUWtmcBTRxhtDGaawyVTtryZJE != null)
				{
					dRBUWtmcBTRxhtDGaawyVTtryZJE.MPKIFtFtDrSDEoYRATBwwEBVXpfQ();
				}
			}

			internal void eYdeHNHRuZZwOGULngUhlKsbfuOC(CompoundElement P_0)
			{
				if (hpXTymXJxGCOVLSxjNdGVAoLMVBC > 0)
				{
					Logger.LogWarning("This element is already a member of a compound element! This is not supported. Resulting values may be unpredictable.");
				}
				hpXTymXJxGCOVLSxjNdGVAoLMVBC++;
				if (ZaKjOwCNbIcOdMDWCMzTeiXEIhkd == null)
				{
					ZaKjOwCNbIcOdMDWCMzTeiXEIhkd = P_0;
				}
			}

			internal void jsYrwpVWSHLmJfVYDDeWuibuixRq(CompoundElement P_0)
			{
				if (hpXTymXJxGCOVLSxjNdGVAoLMVBC == 0)
				{
					Logger.LogWarning("This element is not a member of a compound element!");
					hpXTymXJxGCOVLSxjNdGVAoLMVBC = 0;
					return;
				}
				hpXTymXJxGCOVLSxjNdGVAoLMVBC--;
				if (ZaKjOwCNbIcOdMDWCMzTeiXEIhkd == P_0)
				{
					ZaKjOwCNbIcOdMDWCMzTeiXEIhkd = null;
				}
			}
		}

		public sealed class Axis : Element
		{
			internal class gzanTKNReNUHQliYcDMYQgATaPrL : tscibVDDGPsZUjSdSrflLekbJWIv
			{
				public class zXqcgEYBMtAHkgIazGtLmapljpub : qovUnvqKzupWVYriIrejjnERcAZjA
				{
					private const float lnzrOYlkSyybkAIeJETGBtEiRJxtA = 0.001f;

					public float daIrFBeFZxLevlDRXcAfGpfXXiIF;

					public float HgmFrNyaRCKBABHEcKjiSMKvcEvg;

					public float aKPxkWLqIBrcDsgrLhXxwnyczeic;

					public float PxtfTVHfmDZZgKciCQxSOeIhgqMT;

					public float KZIPkrvAznjLVpbvzUAXFELgaMcdA;

					public float viJXzoTxFpsDOIWHGChQnbjaLPiq;

					public double RVsClIVWIOqjrTOOjUNBdnUzeazK;

					public double zZPAUrfsFJVOijknRPrZNmDHRFLP;

					public double eDjQGwfByGBwleePhdrNBcJtYjtlA;

					public double WcxdOhaFBwpkLfaLBoBgHSZyhqZO;

					public double UZsBfDTbeIDTeguCldAuEUCTsJtAA;

					public double rinAYAaDUGhStUOzsuNHtNrLclskA;

					public double OgbInWCamSafoCoynVwQACIfyshv
					{
						get
						{
							if ((double)daIrFBeFZxLevlDRXcAfGpfXXiIF == 0.0)
							{
								return 0.0;
							}
							return ReInput.unscaledTime - eDjQGwfByGBwleePhdrNBcJtYjtlA;
						}
					}

					public double FAdDBsIgdzBVCbxnaNgwIHSkjkePb
					{
						get
						{
							if ((double)aKPxkWLqIBrcDsgrLhXxwnyczeic == 0.0)
							{
								return 0.0;
							}
							return ReInput.unscaledTime - WcxdOhaFBwpkLfaLBoBgHSZyhqZO;
						}
					}

					public double qwuDiMtnKYCsGoAHkIsLKaqvFecG
					{
						get
						{
							if (daIrFBeFZxLevlDRXcAfGpfXXiIF != 0f)
							{
								return 0.0;
							}
							return ReInput.unscaledTime - RVsClIVWIOqjrTOOjUNBdnUzeazK;
						}
					}

					public double SjiRSuHYaKRoeBrzKWGlWfwSjiot
					{
						get
						{
							if ((double)aKPxkWLqIBrcDsgrLhXxwnyczeic != 0.0)
							{
								return 0.0;
							}
							return ReInput.unscaledTime - zZPAUrfsFJVOijknRPrZNmDHRFLP;
						}
					}

					public void KhtxyKVHXxtTinUwbucUecgGFAFH(bool P_0)
					{
						double unscaledTime = ReInput.unscaledTime;
						if (P_0)
						{
							if (!MathTools.Approximately(KZIPkrvAznjLVpbvzUAXFELgaMcdA, 0f))
							{
								RVsClIVWIOqjrTOOjUNBdnUzeazK = unscaledTime;
							}
							else
							{
								eDjQGwfByGBwleePhdrNBcJtYjtlA = unscaledTime;
							}
							if (!MathTools.IsNear(KZIPkrvAznjLVpbvzUAXFELgaMcdA, viJXzoTxFpsDOIWHGChQnbjaLPiq, 0.001f))
							{
								UZsBfDTbeIDTeguCldAuEUCTsJtAA = unscaledTime;
							}
						}
						else
						{
							if (!MathTools.Approximately(daIrFBeFZxLevlDRXcAfGpfXXiIF, 0f))
							{
								RVsClIVWIOqjrTOOjUNBdnUzeazK = unscaledTime;
							}
							else
							{
								eDjQGwfByGBwleePhdrNBcJtYjtlA = unscaledTime;
							}
							if (!MathTools.IsNear(daIrFBeFZxLevlDRXcAfGpfXXiIF, HgmFrNyaRCKBABHEcKjiSMKvcEvg, 0.001f))
							{
								UZsBfDTbeIDTeguCldAuEUCTsJtAA = unscaledTime;
							}
						}
						if (!MathTools.Approximately(aKPxkWLqIBrcDsgrLhXxwnyczeic, 0f))
						{
							zZPAUrfsFJVOijknRPrZNmDHRFLP = unscaledTime;
						}
						else
						{
							WcxdOhaFBwpkLfaLBoBgHSZyhqZO = unscaledTime;
						}
						if (!MathTools.IsNear(aKPxkWLqIBrcDsgrLhXxwnyczeic, PxtfTVHfmDZZgKciCQxSOeIhgqMT, 0.001f))
						{
							rinAYAaDUGhStUOzsuNHtNrLclskA = unscaledTime;
						}
					}

					public void wdvvvLIXRYueTDhEAtBgojaoQjKx(float P_0)
					{
						if (PxtfTVHfmDZZgKciCQxSOeIhgqMT != aKPxkWLqIBrcDsgrLhXxwnyczeic)
						{
							PxtfTVHfmDZZgKciCQxSOeIhgqMT = aKPxkWLqIBrcDsgrLhXxwnyczeic;
						}
						if (aKPxkWLqIBrcDsgrLhXxwnyczeic != P_0)
						{
							aKPxkWLqIBrcDsgrLhXxwnyczeic = P_0;
						}
					}

					public virtual void fErzGKqVHHayLbKasJpAQrMFCTAX()
					{
						daIrFBeFZxLevlDRXcAfGpfXXiIF = 0f;
						HgmFrNyaRCKBABHEcKjiSMKvcEvg = 0f;
						aKPxkWLqIBrcDsgrLhXxwnyczeic = 0f;
						PxtfTVHfmDZZgKciCQxSOeIhgqMT = 0f;
						RVsClIVWIOqjrTOOjUNBdnUzeazK = 0.0;
						zZPAUrfsFJVOijknRPrZNmDHRFLP = 0.0;
						eDjQGwfByGBwleePhdrNBcJtYjtlA = 0.0;
						WcxdOhaFBwpkLfaLBoBgHSZyhqZO = 0.0;
						UZsBfDTbeIDTeguCldAuEUCTsJtAA = 0.0;
						rinAYAaDUGhStUOzsuNHtNrLclskA = 0.0;
					}
				}

				public gzanTKNReNUHQliYcDMYQgATaPrL(UpdateLoopSetting P_0)
					: base(P_0)
				{
					for (int i = 0; i < MGwKVdZqESiwLYNKuhSByNdJdqVBA; i++)
					{
						MbKOWzMIZetTDynPwZmLtYcisFpL[i] = new zXqcgEYBMtAHkgIazGtLmapljpub();
					}
					WTGpEIcYIUDuOPUdjlxKVotKgWAs = MbKOWzMIZetTDynPwZmLtYcisFpL[0];
				}
			}

			internal readonly AxisRange kMuYzvLFErRkHZnwoBLLTOVCGSOfA;

			internal readonly HardwareAxisInfo dyBDSFNRWhEclaVHduPpTilGwQgN;

			public float value
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0f;
					}
					if (base.isMemberElement)
					{
						return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).KZIPkrvAznjLVpbvzUAXFELgaMcdA;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).daIrFBeFZxLevlDRXcAfGpfXXiIF;
				}
			}

			public float valuePrev
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0f;
					}
					if (base.isMemberElement)
					{
						return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).viJXzoTxFpsDOIWHGChQnbjaLPiq;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).HgmFrNyaRCKBABHEcKjiSMKvcEvg;
				}
			}

			public float valueRaw
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0f;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).aKPxkWLqIBrcDsgrLhXxwnyczeic;
				}
				internal set
				{
					((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).wdvvvLIXRYueTDhEAtBgojaoQjKx(num);
				}
			}

			public float valueRawPrev
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0f;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).PxtfTVHfmDZZgKciCQxSOeIhgqMT;
				}
			}

			public float valueDelta
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0f;
					}
					return value - valuePrev;
				}
			}

			public float valueDeltaRaw
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0f;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).aKPxkWLqIBrcDsgrLhXxwnyczeic - ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).PxtfTVHfmDZZgKciCQxSOeIhgqMT;
				}
			}

			public double lastTimeActive
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).RVsClIVWIOqjrTOOjUNBdnUzeazK;
				}
			}

			public double lastTimeActiveRaw
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).zZPAUrfsFJVOijknRPrZNmDHRFLP;
				}
			}

			public double lastTimeInactive
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).eDjQGwfByGBwleePhdrNBcJtYjtlA;
				}
			}

			public double lastTimeInactiveRaw
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).WcxdOhaFBwpkLfaLBoBgHSZyhqZO;
				}
			}

			public double lastTimeValueChanged
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).UZsBfDTbeIDTeguCldAuEUCTsJtAA;
				}
			}

			public double lastTimeValueChangedRaw
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).rinAYAaDUGhStUOzsuNHtNrLclskA;
				}
			}

			public double timeActive
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).OgbInWCamSafoCoynVwQACIfyshv;
				}
			}

			public double timeActiveRaw
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).OgbInWCamSafoCoynVwQACIfyshv;
				}
			}

			public double timeInactive
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).qwuDiMtnKYCsGoAHkIsLKaqvFecG;
				}
			}

			public double timeInactiveRaw
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).SjiRSuHYaKRoeBrzKWGlWfwSjiot;
				}
			}

			public float pollingDeadZone
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0f;
					}
					if (dyBDSFNRWhEclaVHduPpTilGwQgN == null)
					{
						return -1f;
					}
					return dyBDSFNRWhEclaVHduPpTilGwQgN._pollingDeadZone;
				}
				set
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return;
					}
					if (value < 0f)
					{
						value = -1f;
					}
					if (dyBDSFNRWhEclaVHduPpTilGwQgN != null)
					{
						dyBDSFNRWhEclaVHduPpTilGwQgN._pollingDeadZone = value;
					}
				}
			}

			public AxisCoordinateMode axisCoordinateMode
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return AxisCoordinateMode.Absolute;
					}
					if (dyBDSFNRWhEclaVHduPpTilGwQgN == null)
					{
						return AxisCoordinateMode.Absolute;
					}
					return dyBDSFNRWhEclaVHduPpTilGwQgN.dataFormat;
				}
			}

			bool Element.excludeFromPolling
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return false;
					}
					if (dyBDSFNRWhEclaVHduPpTilGwQgN == null)
					{
						return base.excludeFromPolling;
					}
					return dyBDSFNRWhEclaVHduPpTilGwQgN._excludeFromPolling;
				}
				set
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return;
					}
					if (dyBDSFNRWhEclaVHduPpTilGwQgN != null)
					{
						dyBDSFNRWhEclaVHduPpTilGwQgN._excludeFromPolling = value;
					}
					base.excludeFromPolling = value;
				}
			}

			internal float LDgBrgpInKitDyYzRBGCUsxXGoSZ => ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).daIrFBeFZxLevlDRXcAfGpfXXiIF;

			internal float IVuhfgyqAblISHwARZTZmVcLDZSe => ((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).HgmFrNyaRCKBABHEcKjiSMKvcEvg;

			internal float ZmUbemxBLMMLeVyysMXgQzhrblwr
			{
				get
				{
					if (dyBDSFNRWhEclaVHduPpTilGwQgN == null)
					{
						return ReInput.configuration.defaultAbsoluteAxisPollingDeadZone;
					}
					if (dyBDSFNRWhEclaVHduPpTilGwQgN._pollingDeadZone >= 0f)
					{
						return dyBDSFNRWhEclaVHduPpTilGwQgN._pollingDeadZone;
					}
					return dyBDSFNRWhEclaVHduPpTilGwQgN._dataFormat switch
					{
						AxisCoordinateMode.Absolute => ReInput.configuration.defaultAbsoluteAxisPollingDeadZone, 
						AxisCoordinateMode.Relative => ReInput.configuration.defaultRelativeAxisPollingDeadZone, 
						_ => throw new NotImplementedException(), 
					};
				}
			}

			internal void KLCbduAnFkodzKqNHiMBAiSsnUmU(float P_0)
			{
				gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub obj = (gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs;
				obj.viJXzoTxFpsDOIWHGChQnbjaLPiq = obj.KZIPkrvAznjLVpbvzUAXFELgaMcdA;
				obj.KZIPkrvAznjLVpbvzUAXFELgaMcdA = P_0;
			}

			internal Axis(Controller P_0, int P_1, string P_2, AxisRange P_3, HardwareAxisInfo P_4)
				: base(P_0, P_1, P_2, ControllerElementType.Axis)
			{
				dRBUWtmcBTRxhtDGaawyVTtryZJE = new gzanTKNReNUHQliYcDMYQgATaPrL(ReInput.configVars.updateLoop);
				kMuYzvLFErRkHZnwoBLLTOVCGSOfA = P_3;
				dyBDSFNRWhEclaVHduPpTilGwQgN = P_4;
				if (P_4 != null)
				{
					base.excludeFromPolling = P_4._excludeFromPolling;
				}
			}

			internal void djDAGcFGahfpCIQyOMeGtBhgCdRjb(UpdateLoopType P_0)
			{
				if (dRBUWtmcBTRxhtDGaawyVTtryZJE != null && dRBUWtmcBTRxhtDGaawyVTtryZJE.bsPrFCcBdhNSOJSWNBQqsWcxLyMP != (int)P_0)
				{
					dRBUWtmcBTRxhtDGaawyVTtryZJE.jGsfShJhPkFMDimHUUyjynvDZVWIA = P_0;
				}
			}

			internal void VaFdUwCfBYWoHlFEnHpkRJWicfYP(AxisCalibration P_0)
			{
				gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub zXqcgEYBMtAHkgIazGtLmapljpub = (gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs;
				zXqcgEYBMtAHkgIazGtLmapljpub.HgmFrNyaRCKBABHEcKjiSMKvcEvg = zXqcgEYBMtAHkgIazGtLmapljpub.daIrFBeFZxLevlDRXcAfGpfXXiIF;
				float daIrFBeFZxLevlDRXcAfGpfXXiIF = P_0.GetCalibratedValue(zXqcgEYBMtAHkgIazGtLmapljpub.aKPxkWLqIBrcDsgrLhXxwnyczeic, kMuYzvLFErRkHZnwoBLLTOVCGSOfA);
				if (P_0.applyRangeCalibration)
				{
					daIrFBeFZxLevlDRXcAfGpfXXiIF = MathTools.Clamp(daIrFBeFZxLevlDRXcAfGpfXXiIF, -1f, 1f);
				}
				zXqcgEYBMtAHkgIazGtLmapljpub.daIrFBeFZxLevlDRXcAfGpfXXiIF = daIrFBeFZxLevlDRXcAfGpfXXiIF;
			}

			internal void CWlYieOkidEnFODOKxOpjHXYAYnbA()
			{
				gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub obj = (gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs;
				obj.HgmFrNyaRCKBABHEcKjiSMKvcEvg = obj.daIrFBeFZxLevlDRXcAfGpfXXiIF;
				obj.daIrFBeFZxLevlDRXcAfGpfXXiIF = obj.aKPxkWLqIBrcDsgrLhXxwnyczeic;
			}

			internal void WdjBmJghRNEhxNnKSipACAdKSKGfA()
			{
				gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub obj = (gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs;
				obj.HgmFrNyaRCKBABHEcKjiSMKvcEvg = obj.daIrFBeFZxLevlDRXcAfGpfXXiIF;
				obj.daIrFBeFZxLevlDRXcAfGpfXXiIF = 0f;
			}

			internal void htPLiNlTbUoEUTdlgcwYKZkEMZTn()
			{
				((gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).KhtxyKVHXxtTinUwbucUecgGFAFH(base.isMemberElement);
			}

			internal void CEOburxWsZBCCBjVNOZKCFBJGLOo(float P_0)
			{
				for (int i = 0; i < dRBUWtmcBTRxhtDGaawyVTtryZJE.agkPIJyvbYFLtBrPOLckOXSeJSrE.Count; i++)
				{
					if (dRBUWtmcBTRxhtDGaawyVTtryZJE.agkPIJyvbYFLtBrPOLckOXSeJSrE[i] is gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub zXqcgEYBMtAHkgIazGtLmapljpub)
					{
						zXqcgEYBMtAHkgIazGtLmapljpub.wdvvvLIXRYueTDhEAtBgojaoQjKx(P_0);
						zXqcgEYBMtAHkgIazGtLmapljpub.HgmFrNyaRCKBABHEcKjiSMKvcEvg = zXqcgEYBMtAHkgIazGtLmapljpub.daIrFBeFZxLevlDRXcAfGpfXXiIF;
						zXqcgEYBMtAHkgIazGtLmapljpub.daIrFBeFZxLevlDRXcAfGpfXXiIF = 0f;
						zXqcgEYBMtAHkgIazGtLmapljpub.KhtxyKVHXxtTinUwbucUecgGFAFH(base.isMemberElement);
					}
				}
			}

			internal float MjUcmHSsSWDyqbnOlMgFROvslKJHA(UpdateLoopType P_0, AxisCalibration P_1)
			{
				gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub zXqcgEYBMtAHkgIazGtLmapljpub = (gzanTKNReNUHQliYcDMYQgATaPrL.zXqcgEYBMtAHkgIazGtLmapljpub)dRBUWtmcBTRxhtDGaawyVTtryZJE.LzQVRMRtuvCFGyZrxviHawyAsjQb(P_0);
				float result = P_1.GetCalibratedValue(zXqcgEYBMtAHkgIazGtLmapljpub.aKPxkWLqIBrcDsgrLhXxwnyczeic, kMuYzvLFErRkHZnwoBLLTOVCGSOfA, P_1.deadZone, applySensitivity: false, applyInversion: true);
				if (P_1.applyRangeCalibration)
				{
					result = MathTools.Clamp(result, -1f, 1f);
				}
				return result;
			}
		}

		public sealed class Button : Element
		{
			internal class fZhemGfKZHSUxJeAQNWwIARWcQzy : tscibVDDGPsZUjSdSrflLekbJWIv
			{
				public class RANZvfYTXfNfVoozjYrFcIaBQXlt : qovUnvqKzupWVYriIrejjnERcAZjA
				{
					public bool apvQtTpjjTIgYHMMkCxnFzCvvwNgb;

					public bool pOtyHIVPaIKuhSodOdBnnmQNROBL;

					public ButtonStateRecorder OnpkHrYsUUPSoVPjmPMpzZEQCoN;

					public uSRMPRDlTuSsQoNOCyxXzHNDAaTv buxlUxcIAYgFekHQKPoqlusfQczoA;

					public RANZvfYTXfNfVoozjYrFcIaBQXlt()
					{
						OnpkHrYsUUPSoVPjmPMpzZEQCoN = new ButtonStateRecorder();
						buxlUxcIAYgFekHQKPoqlusfQczoA = new uSRMPRDlTuSsQoNOCyxXzHNDAaTv(0.3f);
					}

					public void ZOMnpiqWuQICoKgKJqytiIQkJNRu(bool P_0)
					{
						if (pOtyHIVPaIKuhSodOdBnnmQNROBL != apvQtTpjjTIgYHMMkCxnFzCvvwNgb)
						{
							pOtyHIVPaIKuhSodOdBnnmQNROBL = apvQtTpjjTIgYHMMkCxnFzCvvwNgb;
						}
						if (apvQtTpjjTIgYHMMkCxnFzCvvwNgb != P_0)
						{
							apvQtTpjjTIgYHMMkCxnFzCvvwNgb = P_0;
						}
						OnpkHrYsUUPSoVPjmPMpzZEQCoN.OwofcwFjGiirRKyKTFEGyYhcTuFw(P_0 && !pOtyHIVPaIKuhSodOdBnnmQNROBL, P_0, ReInput.unscaledTime);
						buxlUxcIAYgFekHQKPoqlusfQczoA.oXTUuAXhsMaBKQYqVlzuLxgKopvQ(0.3f, P_0 && !pOtyHIVPaIKuhSodOdBnnmQNROBL, P_0);
					}

					public virtual void SbebbxJapMIFuZqrfPWhEjyealCfb()
					{
						apvQtTpjjTIgYHMMkCxnFzCvvwNgb = false;
						pOtyHIVPaIKuhSodOdBnnmQNROBL = false;
						OnpkHrYsUUPSoVPjmPMpzZEQCoN.ymIPXtpzXCiamHBhXeIwNypZXQfF();
						buxlUxcIAYgFekHQKPoqlusfQczoA.JLNdcprfgggQLGgUwOOffAfDicRIA();
					}
				}

				public class GHIcuPiWABlBAvsWKjxsgxEcAQINb : RANZvfYTXfNfVoozjYrFcIaBQXlt
				{
					public float QdDZKMLkCwSHWESQCqpdmDUKzblP;

					public float cqYvABMxbQQOKuXrcEUFMbHdZRu;

					public void YCyHKaWfisaFjDvjPOVyuynzIjLP(float P_0)
					{
						if (cqYvABMxbQQOKuXrcEUFMbHdZRu != QdDZKMLkCwSHWESQCqpdmDUKzblP)
						{
							cqYvABMxbQQOKuXrcEUFMbHdZRu = QdDZKMLkCwSHWESQCqpdmDUKzblP;
						}
						if (QdDZKMLkCwSHWESQCqpdmDUKzblP != P_0)
						{
							QdDZKMLkCwSHWESQCqpdmDUKzblP = ((P_0 > 0.001f) ? P_0 : 0f);
						}
						ZOMnpiqWuQICoKgKJqytiIQkJNRu(QdDZKMLkCwSHWESQCqpdmDUKzblP > 0f);
					}

					public virtual void MIAHLMUuEfUAFoebFeEhdXmaANrf()
					{
						SbebbxJapMIFuZqrfPWhEjyealCfb();
						QdDZKMLkCwSHWESQCqpdmDUKzblP = 0f;
						cqYvABMxbQQOKuXrcEUFMbHdZRu = 0f;
					}
				}

				public fZhemGfKZHSUxJeAQNWwIARWcQzy(UpdateLoopSetting P_0, bool P_1)
					: base(P_0)
				{
					for (int i = 0; i < MGwKVdZqESiwLYNKuhSByNdJdqVBA; i++)
					{
						if (P_1)
						{
							MbKOWzMIZetTDynPwZmLtYcisFpL[i] = new GHIcuPiWABlBAvsWKjxsgxEcAQINb();
						}
						else
						{
							MbKOWzMIZetTDynPwZmLtYcisFpL[i] = new RANZvfYTXfNfVoozjYrFcIaBQXlt();
						}
					}
					WTGpEIcYIUDuOPUdjlxKVotKgWAs = MbKOWzMIZetTDynPwZmLtYcisFpL[0];
				}

				public void HCQcCTWhDVcahFjdqFUzwFytWzZiA(float P_0)
				{
					for (int i = 0; i < MbKOWzMIZetTDynPwZmLtYcisFpL.Length; i++)
					{
						((RANZvfYTXfNfVoozjYrFcIaBQXlt)MbKOWzMIZetTDynPwZmLtYcisFpL[i]).buxlUxcIAYgFekHQKPoqlusfQczoA.KvnqCaqvimJhesMKIYfqxezWTIpe(P_0);
					}
				}

				public void kzaPioyRSjCphWcpaIhlpcjZJYrf()
				{
					for (int i = 0; i < MbKOWzMIZetTDynPwZmLtYcisFpL.Length; i++)
					{
						((RANZvfYTXfNfVoozjYrFcIaBQXlt)MbKOWzMIZetTDynPwZmLtYcisFpL[i]).buxlUxcIAYgFekHQKPoqlusfQczoA.KvnqCaqvimJhesMKIYfqxezWTIpe(0.3f);
					}
				}
			}

			internal readonly bool eNSFGVUwhFxlboZfoPTFrrWgpWCl;

			internal readonly HardwareButtonInfo jPvtwJYFXYwDJlmlygLGlTdEfLMc;

			public bool valuePrev
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return false;
					}
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).pOtyHIVPaIKuhSodOdBnnmQNROBL;
				}
			}

			public bool value
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return false;
					}
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).apvQtTpjjTIgYHMMkCxnFzCvvwNgb;
				}
			}

			public float pressure
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0f;
					}
					if (!eNSFGVUwhFxlboZfoPTFrrWgpWCl)
					{
						if (!((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).apvQtTpjjTIgYHMMkCxnFzCvvwNgb)
						{
							return 0f;
						}
						return 1f;
					}
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.GHIcuPiWABlBAvsWKjxsgxEcAQINb)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).QdDZKMLkCwSHWESQCqpdmDUKzblP;
				}
			}

			public float pressurePrev
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0f;
					}
					if (!eNSFGVUwhFxlboZfoPTFrrWgpWCl)
					{
						if (!((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).pOtyHIVPaIKuhSodOdBnnmQNROBL)
						{
							return 0f;
						}
						return 1f;
					}
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.GHIcuPiWABlBAvsWKjxsgxEcAQINb)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).cqYvABMxbQQOKuXrcEUFMbHdZRu;
				}
			}

			public bool isPressureSensitive
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return false;
					}
					return eNSFGVUwhFxlboZfoPTFrrWgpWCl;
				}
			}

			public bool justPressed
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return false;
					}
					if (!((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).pOtyHIVPaIKuhSodOdBnnmQNROBL && ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).apvQtTpjjTIgYHMMkCxnFzCvvwNgb)
					{
						return true;
					}
					return false;
				}
			}

			public bool justReleased
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return false;
					}
					if (((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).pOtyHIVPaIKuhSodOdBnnmQNROBL && !((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).apvQtTpjjTIgYHMMkCxnFzCvvwNgb)
					{
						return true;
					}
					return false;
				}
			}

			public bool justChangedState
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return false;
					}
					if (((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).pOtyHIVPaIKuhSodOdBnnmQNROBL != ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).apvQtTpjjTIgYHMMkCxnFzCvvwNgb)
					{
						return true;
					}
					return false;
				}
			}

			public bool doublePressedAndHeld
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return false;
					}
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).buxlUxcIAYgFekHQKPoqlusfQczoA.ppmmfFcSDhxyPrCAEnkHbdEODykk;
				}
			}

			public bool justDoublePressed
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return false;
					}
					if (!justPressed)
					{
						return false;
					}
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).buxlUxcIAYgFekHQKPoqlusfQczoA.ppmmfFcSDhxyPrCAEnkHbdEODykk;
				}
			}

			public double timePressed
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).OnpkHrYsUUPSoVPjmPMpzZEQCoN.cTahTQgfsptofvxpQBxxUulebBaf;
				}
			}

			public double timeUnpressed
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).OnpkHrYsUUPSoVPjmPMpzZEQCoN.SjocHlgjNdoneNbqtHVvYGtbLTVCb;
				}
			}

			public double lastTimePressed
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).OnpkHrYsUUPSoVPjmPMpzZEQCoN.SiMAzADNHMHJVSDUvcYniBVCqcDo;
				}
			}

			public double lastTimeUnpressed
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).OnpkHrYsUUPSoVPjmPMpzZEQCoN.KGJgGsipDXStkfULpEGonaViXCGhA;
				}
			}

			public double lastTimeStateChanged
			{
				get
				{
					if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
					{
						ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
						return 0.0;
					}
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).OnpkHrYsUUPSoVPjmPMpzZEQCoN.EJWywQSxNgzLvgCsHdvAzzbYRmyD;
				}
			}

			internal ButtonStateFlags NIWDsoHmSxaJvEDDdXiYnrzlQbeTB
			{
				get
				{
					fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt rANZvfYTXfNfVoozjYrFcIaBQXlt = (fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs;
					ButtonStateFlags buttonStateFlags = ButtonStateFlags.Off;
					if (rANZvfYTXfNfVoozjYrFcIaBQXlt.apvQtTpjjTIgYHMMkCxnFzCvvwNgb)
					{
						buttonStateFlags |= ButtonStateFlags.On;
						if (!rANZvfYTXfNfVoozjYrFcIaBQXlt.pOtyHIVPaIKuhSodOdBnnmQNROBL)
						{
							buttonStateFlags |= ButtonStateFlags.Down;
						}
					}
					else if (rANZvfYTXfNfVoozjYrFcIaBQXlt.pOtyHIVPaIKuhSodOdBnnmQNROBL)
					{
						buttonStateFlags |= ButtonStateFlags.Up;
					}
					return buttonStateFlags;
				}
			}

			internal Button(Controller P_0, int P_1, string P_2, HardwareButtonInfo P_3)
				: base(P_0, P_1, P_2, ControllerElementType.Button)
			{
				jPvtwJYFXYwDJlmlygLGlTdEfLMc = P_3;
				dRBUWtmcBTRxhtDGaawyVTtryZJE = new fZhemGfKZHSUxJeAQNWwIARWcQzy(ReInput.configVars.updateLoop, false);
			}

			internal Button(Controller P_0, int P_1, string P_2, bool P_3, HardwareButtonInfo P_4)
				: base(P_0, P_1, P_2, ControllerElementType.Button)
			{
				jPvtwJYFXYwDJlmlygLGlTdEfLMc = P_4;
				eNSFGVUwhFxlboZfoPTFrrWgpWCl = P_3;
				dRBUWtmcBTRxhtDGaawyVTtryZJE = new fZhemGfKZHSUxJeAQNWwIARWcQzy(ReInput.configVars.updateLoop, P_3);
			}

			public bool DoublePressedAndHeld(float speed)
			{
				if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
				{
					ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
					return false;
				}
				if (speed <= 0f)
				{
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).buxlUxcIAYgFekHQKPoqlusfQczoA.ppmmfFcSDhxyPrCAEnkHbdEODykk;
				}
				return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).OnpkHrYsUUPSoVPjmPMpzZEQCoN.rIomjiGhhTHLXAPYnIXAvYvGDZbo(speed);
			}

			public bool JustDoublePressed(float speed)
			{
				if (ReInput._id != wspMDQsHjWUvnnwRnuvUTkcjyQSk)
				{
					ReInput.CheckInitialized(wspMDQsHjWUvnnwRnuvUTkcjyQSk);
					return false;
				}
				if (!justPressed)
				{
					return false;
				}
				if (speed <= 0f)
				{
					return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).buxlUxcIAYgFekHQKPoqlusfQczoA.ppmmfFcSDhxyPrCAEnkHbdEODykk;
				}
				return ((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).OnpkHrYsUUPSoVPjmPMpzZEQCoN.rIomjiGhhTHLXAPYnIXAvYvGDZbo(speed);
			}

			internal void afeJkBinlRdwvHJUmFnGIJdBjiWVb(UpdateLoopType P_0, int P_1, ControllerDataUpdater P_2)
			{
				if (dRBUWtmcBTRxhtDGaawyVTtryZJE != null && dRBUWtmcBTRxhtDGaawyVTtryZJE.bsPrFCcBdhNSOJSWNBQqsWcxLyMP != (int)P_0)
				{
					dRBUWtmcBTRxhtDGaawyVTtryZJE.jGsfShJhPkFMDimHUUyjynvDZVWIA = P_0;
				}
				if (eNSFGVUwhFxlboZfoPTFrrWgpWCl)
				{
					((fZhemGfKZHSUxJeAQNWwIARWcQzy.GHIcuPiWABlBAvsWKjxsgxEcAQINb)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).YCyHKaWfisaFjDvjPOVyuynzIjLP(P_2.buttonPressureValues[P_1]);
				}
				else
				{
					((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).ZOMnpiqWuQICoKgKJqytiIQkJNRu(P_2.buttonValues[P_1]);
				}
			}

			internal void YXaEnheWDXkoMvMdXoifLBXucVbW(UpdateLoopType P_0)
			{
				if (dRBUWtmcBTRxhtDGaawyVTtryZJE != null && dRBUWtmcBTRxhtDGaawyVTtryZJE.bsPrFCcBdhNSOJSWNBQqsWcxLyMP != (int)P_0)
				{
					dRBUWtmcBTRxhtDGaawyVTtryZJE.jGsfShJhPkFMDimHUUyjynvDZVWIA = P_0;
				}
				if (eNSFGVUwhFxlboZfoPTFrrWgpWCl)
				{
					((fZhemGfKZHSUxJeAQNWwIARWcQzy.GHIcuPiWABlBAvsWKjxsgxEcAQINb)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).YCyHKaWfisaFjDvjPOVyuynzIjLP(0f);
				}
				else
				{
					((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)dRBUWtmcBTRxhtDGaawyVTtryZJE.WTGpEIcYIUDuOPUdjlxKVotKgWAs).ZOMnpiqWuQICoKgKJqytiIQkJNRu(false);
				}
			}

			internal void AGfbYckJxrUAssVVtOJUfGCodhRq()
			{
				for (int i = 0; i < dRBUWtmcBTRxhtDGaawyVTtryZJE.agkPIJyvbYFLtBrPOLckOXSeJSrE.Count; i++)
				{
					tscibVDDGPsZUjSdSrflLekbJWIv.qovUnvqKzupWVYriIrejjnERcAZjA qovUnvqKzupWVYriIrejjnERcAZjA = dRBUWtmcBTRxhtDGaawyVTtryZJE.agkPIJyvbYFLtBrPOLckOXSeJSrE[i];
					if (qovUnvqKzupWVYriIrejjnERcAZjA != null)
					{
						if (eNSFGVUwhFxlboZfoPTFrrWgpWCl)
						{
							((fZhemGfKZHSUxJeAQNWwIARWcQzy.GHIcuPiWABlBAvsWKjxsgxEcAQINb)qovUnvqKzupWVYriIrejjnERcAZjA).YCyHKaWfisaFjDvjPOVyuynzIjLP(0f);
						}
						else
						{
							((fZhemGfKZHSUxJeAQNWwIARWcQzy.RANZvfYTXfNfVoozjYrFcIaBQXlt)qovUnvqKzupWVYriIrejjnERcAZjA).ZOMnpiqWuQICoKgKJqytiIQkJNRu(false);
						}
					}
				}
			}
		}

		public abstract class CompoundElement
		{
			private class mvIkVEoHGkozsoyxgTTdDgKfHojh
			{
				public readonly Element supGZAYoVxscEbsLJSwucuQARBbk;

				public readonly int RHTUNxeLJOatLluqQdOYwMeuXLww;

				public mvIkVEoHGkozsoyxgTTdDgKfHojh(Element P_0, int P_1)
				{
					supGZAYoVxscEbsLJSwucuQARBbk = P_0;
					RHTUNxeLJOatLluqQdOYwMeuXLww = P_1;
				}
			}

			private int iHtVjbfXHffcLpLJouZbyseAvQfD;

			private string uGkDhqtDWsZlHIVcOQDMjySyImlw;

			private CompoundControllerElementType wNPliVQYAnsQxvXYAuxJnxViMBAe;

			private int zkHmhveHOAFtrIIecNCeGOazRHUrA;

			private mvIkVEoHGkozsoyxgTTdDgKfHojh[] TJrVpDeNawhlrSfwSbxBtjyfdCnfA;

			private Controller DTpFlfbsvpnfWfMtPZeWFbBlfjZNA;

			internal readonly int xvpPtUrtXouTLVkUdkpCJFZoygnp;

			public int id
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return -1;
					}
					return iHtVjbfXHffcLpLJouZbyseAvQfD;
				}
			}

			public string name
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return string.Empty;
					}
					return uGkDhqtDWsZlHIVcOQDMjySyImlw;
				}
			}

			public CompoundControllerElementType type
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return CompoundControllerElementType.Axis2D;
					}
					return wNPliVQYAnsQxvXYAuxJnxViMBAe;
				}
			}

			public bool hasElements
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return false;
					}
					return zkHmhveHOAFtrIIecNCeGOazRHUrA > 0;
				}
			}

			public int elementCount
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return 0;
					}
					return zkHmhveHOAFtrIIecNCeGOazRHUrA;
				}
			}

			public abstract int elementCapacity { get; }

			public ControllerElementIdentifier elementIdentifier
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					ControllerElementIdentifier elementIdentifierById = DTpFlfbsvpnfWfMtPZeWFbBlfjZNA.GetElementIdentifierById(iHtVjbfXHffcLpLJouZbyseAvQfD);
					if (elementIdentifierById == null)
					{
						return ControllerElementIdentifier.BlankReadOnly;
					}
					return elementIdentifierById;
				}
			}

			internal CompoundElement(Controller P_0, int P_1, string P_2, CompoundControllerElementType P_3)
			{
				DTpFlfbsvpnfWfMtPZeWFbBlfjZNA = P_0;
				iHtVjbfXHffcLpLJouZbyseAvQfD = P_1;
				uGkDhqtDWsZlHIVcOQDMjySyImlw = P_2;
				wNPliVQYAnsQxvXYAuxJnxViMBAe = P_3;
				TJrVpDeNawhlrSfwSbxBtjyfdCnfA = new mvIkVEoHGkozsoyxgTTdDgKfHojh[elementCapacity];
				xvpPtUrtXouTLVkUdkpCJFZoygnp = ReInput.id;
			}

			internal Element nhsafaLnsqmOczvkBehAHMZcBAmDA(int P_0)
			{
				if (P_0 < 0 || P_0 >= TJrVpDeNawhlrSfwSbxBtjyfdCnfA.Length)
				{
					return null;
				}
				if (TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_0] == null)
				{
					return null;
				}
				return TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_0].supGZAYoVxscEbsLJSwucuQARBbk;
			}

			internal _0001 nhsafaLnsqmOczvkBehAHMZcBAmDA<_0001>(int P_0) where _0001 : Element
			{
				if (P_0 < 0 || P_0 >= TJrVpDeNawhlrSfwSbxBtjyfdCnfA.Length)
				{
					return null;
				}
				if (TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_0] == null)
				{
					return null;
				}
				return TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_0].supGZAYoVxscEbsLJSwucuQARBbk as _0001;
			}

			internal _0001 qfZNPYtpwHxVNbjiAosWKOyQEYgt<_0001>(int P_0, out int P_1) where _0001 : Element
			{
				P_1 = -1;
				if (P_0 < 0 || P_0 >= TJrVpDeNawhlrSfwSbxBtjyfdCnfA.Length)
				{
					return null;
				}
				if (TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_0] == null)
				{
					return null;
				}
				P_1 = TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_0].RHTUNxeLJOatLluqQdOYwMeuXLww;
				return TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_0].supGZAYoVxscEbsLJSwucuQARBbk as _0001;
			}

			internal bool QAuMxPUGOaJdEMNWDIDVYTOmOLcD(Element P_0, int P_1)
			{
				if (P_0 == null)
				{
					return false;
				}
				if (zkHmhveHOAFtrIIecNCeGOazRHUrA >= elementCapacity)
				{
					Logger.LogWarning("Cannot add element! This Compound Element already contains the maximum number of elements.");
					return false;
				}
				if (P_0.isMemberElement)
				{
					Logger.LogWarning("Cannot add element! The element you are trying to add is already a member of another compound element.");
					return false;
				}
				if (sMMFpLXxczKjRdlklFVqHJsFxwjD(P_0) >= 0)
				{
					Logger.LogWarning("Cannot add element! This Compound Element already contains the element you are trying to add.");
					return false;
				}
				int num = OfzkPIowNBchTynWEgjZVqSZYmXp();
				if (num < 0)
				{
					Logger.LogWarning("Cannot add element! This Compound Element already contains the maximum number of elements.");
					return false;
				}
				return vDUIRqFOBHDEXTByGqmaAntCBChwB(P_0, P_1, num);
			}

			internal bool SCTeSpkTLUqjtGAbkupfOhBlGeLEA(Element P_0)
			{
				if (P_0 == null)
				{
					return false;
				}
				if (zkHmhveHOAFtrIIecNCeGOazRHUrA == 0)
				{
					Logger.LogWarning("Cannot remove element! This Compound Element has no elements.");
					return false;
				}
				int num = sMMFpLXxczKjRdlklFVqHJsFxwjD(P_0);
				if (num < 0)
				{
					Logger.LogWarning("Cannot remove element! This Compound Element does not contain the element you are trying to remove.");
					return false;
				}
				return PaCRpWFOfgTYsqAOENsehrYtRhUo(num);
			}

			internal void dtahSyEUJaqxceudBMYIbiqEwhimc()
			{
				for (int i = 0; i < TJrVpDeNawhlrSfwSbxBtjyfdCnfA.Length; i++)
				{
					PaCRpWFOfgTYsqAOENsehrYtRhUo(i);
				}
				zkHmhveHOAFtrIIecNCeGOazRHUrA = 0;
			}

			private int sMMFpLXxczKjRdlklFVqHJsFxwjD(Element P_0)
			{
				if (P_0 == null)
				{
					return -1;
				}
				for (int i = 0; i < TJrVpDeNawhlrSfwSbxBtjyfdCnfA.Length; i++)
				{
					if (TJrVpDeNawhlrSfwSbxBtjyfdCnfA[i] != null && TJrVpDeNawhlrSfwSbxBtjyfdCnfA[i].supGZAYoVxscEbsLJSwucuQARBbk == P_0)
					{
						return i;
					}
				}
				return -1;
			}

			private bool vDUIRqFOBHDEXTByGqmaAntCBChwB(Element P_0, int P_1, int P_2)
			{
				if (P_2 < 0 || P_2 >= TJrVpDeNawhlrSfwSbxBtjyfdCnfA.Length)
				{
					return false;
				}
				if (TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_2] != null)
				{
					return false;
				}
				TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_2] = new mvIkVEoHGkozsoyxgTTdDgKfHojh(P_0, P_1);
				P_0.eYdeHNHRuZZwOGULngUhlKsbfuOC(this);
				zkHmhveHOAFtrIIecNCeGOazRHUrA++;
				return true;
			}

			private bool PaCRpWFOfgTYsqAOENsehrYtRhUo(int P_0)
			{
				if (P_0 < 0 || P_0 >= TJrVpDeNawhlrSfwSbxBtjyfdCnfA.Length)
				{
					return false;
				}
				if (TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_0] == null)
				{
					return false;
				}
				if (TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_0].supGZAYoVxscEbsLJSwucuQARBbk != null)
				{
					TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_0].supGZAYoVxscEbsLJSwucuQARBbk.jsYrwpVWSHLmJfVYDDeWuibuixRq(this);
				}
				TJrVpDeNawhlrSfwSbxBtjyfdCnfA[P_0] = null;
				zkHmhveHOAFtrIIecNCeGOazRHUrA--;
				return true;
			}

			private int OfzkPIowNBchTynWEgjZVqSZYmXp()
			{
				for (int i = 0; i < TJrVpDeNawhlrSfwSbxBtjyfdCnfA.Length; i++)
				{
					if (TJrVpDeNawhlrSfwSbxBtjyfdCnfA[i] == null)
					{
						return i;
					}
				}
				return -1;
			}
		}

		public sealed class Axis2D : CompoundElement
		{
			private const int zVHxBtYqDlRKFhFKXxOOMyqGKfSK = 2;

			private CalibrationMap qiJqZvEpYsmCQwwMdxQzIysfLqpj;

			int CompoundElement.elementCapacity => 2;

			public Axis xAxis
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Axis>(0);
				}
			}

			public Axis yAxis
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Axis>(1);
				}
			}

			public Vector2 value
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return Vector2.zero;
					}
					return izoDfHUtKUXNCHONhrFqvpnrdadgA();
				}
			}

			public Vector2 valuePrev
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return Vector2.zero;
					}
					return DTGGdJaUGSEaEmAJbllVIZSpbTTC();
				}
			}

			public Vector2 valueRaw
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return Vector2.zero;
					}
					return new Vector2((xAxis != null) ? xAxis.valueRaw : 0f, (yAxis != null) ? yAxis.valueRaw : 0f);
				}
			}

			public Vector2 valueRawPrev
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return Vector2.zero;
					}
					return new Vector2((xAxis != null) ? xAxis.valueRawPrev : 0f, (yAxis != null) ? yAxis.valueRawPrev : 0f);
				}
			}

			internal Axis2D(Controller P_0, int P_1, string P_2, Axis P_3, Axis P_4, int P_5, int P_6, CalibrationMap P_7)
				: base(P_0, P_1, P_2, CompoundControllerElementType.Axis2D)
			{
				QAuMxPUGOaJdEMNWDIDVYTOmOLcD(P_3, P_5);
				QAuMxPUGOaJdEMNWDIDVYTOmOLcD(P_4, P_6);
				qiJqZvEpYsmCQwwMdxQzIysfLqpj = P_7;
			}

			internal void fabXstqoGnFPFRfQUDZCKnMUIfwB()
			{
				Vector2 vector = value;
				if (xAxis != null)
				{
					xAxis.KLCbduAnFkodzKqNHiMBAiSsnUmU(vector.x);
				}
				if (yAxis != null)
				{
					yAxis.KLCbduAnFkodzKqNHiMBAiSsnUmU(vector.y);
				}
			}

			private Vector2 izoDfHUtKUXNCHONhrFqvpnrdadgA()
			{
				if (qiJqZvEpYsmCQwwMdxQzIysfLqpj == null)
				{
					return default(Vector2);
				}
				int xAxisIndex;
				Axis axis = qfZNPYtpwHxVNbjiAosWKOyQEYgt<Axis>(0, out xAxisIndex);
				int yAxisIndex;
				Axis axis2 = qfZNPYtpwHxVNbjiAosWKOyQEYgt<Axis>(1, out yAxisIndex);
				DeadZone2DType defaultJoystickAxis2DDeadZoneType = ReInput.configVars.defaultJoystickAxis2DDeadZoneType;
				AxisSensitivity2DType defaultJoystickAxis2DSensitivityType = ReInput.configVars.defaultJoystickAxis2DSensitivityType;
				float valueRawX = axis?.valueRaw ?? 0f;
				float valueRawY = axis2?.valueRaw ?? 0f;
				return qiJqZvEpYsmCQwwMdxQzIysfLqpj.GetCalibrated2DValue(xAxisIndex, yAxisIndex, valueRawX, valueRawY, defaultJoystickAxis2DDeadZoneType, defaultJoystickAxis2DSensitivityType);
			}

			private Vector2 DTGGdJaUGSEaEmAJbllVIZSpbTTC()
			{
				if (qiJqZvEpYsmCQwwMdxQzIysfLqpj == null)
				{
					return default(Vector2);
				}
				int xAxisIndex;
				Axis axis = qfZNPYtpwHxVNbjiAosWKOyQEYgt<Axis>(0, out xAxisIndex);
				int yAxisIndex;
				Axis axis2 = qfZNPYtpwHxVNbjiAosWKOyQEYgt<Axis>(1, out yAxisIndex);
				DeadZone2DType defaultJoystickAxis2DDeadZoneType = ReInput.configVars.defaultJoystickAxis2DDeadZoneType;
				AxisSensitivity2DType defaultJoystickAxis2DSensitivityType = ReInput.configVars.defaultJoystickAxis2DSensitivityType;
				float valueRawX = axis?.valueRawPrev ?? 0f;
				float valueRawY = axis2?.valueRawPrev ?? 0f;
				return qiJqZvEpYsmCQwwMdxQzIysfLqpj.GetCalibrated2DValue(xAxisIndex, yAxisIndex, valueRawX, valueRawY, defaultJoystickAxis2DDeadZoneType, defaultJoystickAxis2DSensitivityType);
			}
		}

		public sealed class Hat : CompoundElement
		{
			private const int EUWfHsGFkqjLqEDNguitdPoEDuIOb = 8;

			private const int TxTTyBXSQCOISmpiPHljcuUDeLnX = 0;

			private const int sygpculLPXkLFLGaaDcIzQMRfFcHA = 1;

			private const int CEeOdnyUppdRGDljLhNvxHokINrG = 2;

			private const int isynKqwISgEUWACqKLjbjMIMMjYD = 3;

			private const int XGImbgsgISlXjZQRZusMTNcyBAfCA = 4;

			private const int ZmSuPNuJCCDOwvqRItBdzFMlEKSl = 5;

			private const int QfhRaDaZewaciCBHkjWQaoPADRfBb = 6;

			private const int uLHlpYtlmHeEThMbefZHYbrYQNToA = 7;

			private readonly int RLdOlgMDalvcBJGMQebYJromCuzpA;

			private readonly Button[] lpTXKbQJmZsGMlUAsBruBKRrNLDn;

			private readonly ReadOnlyCollection<Button> AyAXTnTzUZXpcGoINSjDjewkNptA;

			private readonly int[] YQxixGDQsgGqsRQYzKqceFNQrqVp;

			private bool OWxqeBZlQnBXzipiXxngZaphbgdDA;

			int CompoundElement.elementCapacity => 8;

			public bool force4Way
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return false;
					}
					return OWxqeBZlQnBXzipiXxngZaphbgdDA;
				}
				set
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
					}
					else
					{
						OWxqeBZlQnBXzipiXxngZaphbgdDA = value;
					}
				}
			}

			public int directionCount
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return 0;
					}
					return RLdOlgMDalvcBJGMQebYJromCuzpA;
				}
			}

			public IList<Button> Buttons
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return EmptyObjects<Button>.EmptyReadOnlyIListT;
					}
					return AyAXTnTzUZXpcGoINSjDjewkNptA;
				}
			}

			public Button buttonUp
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(0);
				}
			}

			public Button buttonRight
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(2);
				}
			}

			public Button buttonDown
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(4);
				}
			}

			public Button buttonLeft
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(6);
				}
			}

			public Button buttonUpRight
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(1);
				}
			}

			public Button buttonDownRight
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(3);
				}
			}

			public Button buttonDownLeft
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(5);
				}
			}

			public Button buttonUpLeft
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(7);
				}
			}

			internal Hat(Controller P_0, int P_1, string P_2, Button[] P_3, int[] P_4)
				: base(P_0, P_1, P_2, CompoundControllerElementType.Hat)
			{
				int num = ((P_3 != null) ? P_3.Length : 0);
				if (num != ((P_4 != null) ? P_4.Length : 0))
				{
					throw new ArgumentException("buttons.Length must equal buttonIndices.Length!");
				}
				if (num != 0 && num != 4 && num != 8)
				{
					throw new ArgumentException("buttons.Length must be 0, 4, or 8! Length: " + num);
				}
				for (int i = 0; i < num; i++)
				{
					QAuMxPUGOaJdEMNWDIDVYTOmOLcD(P_3[i], P_4[i]);
				}
				lpTXKbQJmZsGMlUAsBruBKRrNLDn = P_3;
				YQxixGDQsgGqsRQYzKqceFNQrqVp = P_4;
				RLdOlgMDalvcBJGMQebYJromCuzpA = num;
				AyAXTnTzUZXpcGoINSjDjewkNptA = new ReadOnlyCollection<Button>(P_3);
			}

			internal void DzCfshFMjzYYSClLiqDxyGpzkzkQA(UpdateLoopType P_0, ControllerDataUpdater P_1)
			{
				if (RLdOlgMDalvcBJGMQebYJromCuzpA == 0)
				{
					return;
				}
				if (RLdOlgMDalvcBJGMQebYJromCuzpA == 8 && (OWxqeBZlQnBXzipiXxngZaphbgdDA || ReInput.configVars.force4WayHats))
				{
					HPIvgSjcBjiCLEwgvUFqkTKwgUShA(lpTXKbQJmZsGMlUAsBruBKRrNLDn[0], YQxixGDQsgGqsRQYzKqceFNQrqVp[0], YQxixGDQsgGqsRQYzKqceFNQrqVp[7], YQxixGDQsgGqsRQYzKqceFNQrqVp[1], P_0, P_1);
					HPIvgSjcBjiCLEwgvUFqkTKwgUShA(lpTXKbQJmZsGMlUAsBruBKRrNLDn[2], YQxixGDQsgGqsRQYzKqceFNQrqVp[2], YQxixGDQsgGqsRQYzKqceFNQrqVp[1], YQxixGDQsgGqsRQYzKqceFNQrqVp[3], P_0, P_1);
					HPIvgSjcBjiCLEwgvUFqkTKwgUShA(lpTXKbQJmZsGMlUAsBruBKRrNLDn[4], YQxixGDQsgGqsRQYzKqceFNQrqVp[4], YQxixGDQsgGqsRQYzKqceFNQrqVp[5], YQxixGDQsgGqsRQYzKqceFNQrqVp[3], P_0, P_1);
					HPIvgSjcBjiCLEwgvUFqkTKwgUShA(lpTXKbQJmZsGMlUAsBruBKRrNLDn[6], YQxixGDQsgGqsRQYzKqceFNQrqVp[6], YQxixGDQsgGqsRQYzKqceFNQrqVp[5], YQxixGDQsgGqsRQYzKqceFNQrqVp[7], P_0, P_1);
					aLJqLCeLKXYzQKtFtXODZpswXilE(lpTXKbQJmZsGMlUAsBruBKRrNLDn[1], YQxixGDQsgGqsRQYzKqceFNQrqVp[1], P_0, P_1);
					aLJqLCeLKXYzQKtFtXODZpswXilE(lpTXKbQJmZsGMlUAsBruBKRrNLDn[3], YQxixGDQsgGqsRQYzKqceFNQrqVp[3], P_0, P_1);
					aLJqLCeLKXYzQKtFtXODZpswXilE(lpTXKbQJmZsGMlUAsBruBKRrNLDn[5], YQxixGDQsgGqsRQYzKqceFNQrqVp[5], P_0, P_1);
					aLJqLCeLKXYzQKtFtXODZpswXilE(lpTXKbQJmZsGMlUAsBruBKRrNLDn[7], YQxixGDQsgGqsRQYzKqceFNQrqVp[7], P_0, P_1);
					return;
				}
				for (int i = 0; i < lpTXKbQJmZsGMlUAsBruBKRrNLDn.Length; i++)
				{
					if (lpTXKbQJmZsGMlUAsBruBKRrNLDn[i] != null)
					{
						lpTXKbQJmZsGMlUAsBruBKRrNLDn[i].afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_0, YQxixGDQsgGqsRQYzKqceFNQrqVp[i], P_1);
					}
				}
			}

			private void HPIvgSjcBjiCLEwgvUFqkTKwgUShA(Button P_0, int P_1, int P_2, int P_3, UpdateLoopType P_4, ControllerDataUpdater P_5)
			{
				if (P_0 == null || P_1 < 0 || P_1 >= P_5.buttonCount)
				{
					return;
				}
				if (!P_0.isPressureSensitive)
				{
					if (P_2 >= 0 && P_2 < P_5.buttonCount)
					{
						ref bool reference = ref P_5.buttonValues[P_1];
						reference |= P_5.buttonValues[P_2];
					}
					if (P_3 >= 0 && P_3 < P_5.buttonCount)
					{
						ref bool reference2 = ref P_5.buttonValues[P_1];
						reference2 |= P_5.buttonValues[P_3];
					}
				}
				else
				{
					P_5.buttonPressureValues[P_1] = MathTools.MaxMagnitude(P_5.buttonPressureValues[P_1], MathTools.MaxMagnitude((P_2 >= 0 && P_2 < P_5.buttonCount) ? P_5.buttonPressureValues[P_2] : 0f, (P_3 >= 0 && P_3 < P_5.buttonCount) ? P_5.buttonPressureValues[P_3] : 0f));
				}
				P_0.afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_4, P_1, P_5);
			}

			private void aLJqLCeLKXYzQKtFtXODZpswXilE(Button P_0, int P_1, UpdateLoopType P_2, ControllerDataUpdater P_3)
			{
				if (P_0 != null && P_1 >= 0 && P_1 < P_3.buttonCount)
				{
					if (!P_0.isPressureSensitive)
					{
						P_3.buttonValues[P_1] = false;
					}
					else
					{
						P_3.buttonPressureValues[P_1] = 0f;
					}
					P_0.afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_2, P_1, P_3);
				}
			}
		}

		public sealed class DirectionalPad : CompoundElement
		{
			private const int hfoRJzVhbbqfzYOpdtOFhyHUQjvb = 4;

			private const int TaFdFljrBpsccYLfyhDKYFVLyvHA = 0;

			private const int EwisIfIwufmAYNoRRGfAJlkaTpWj = 1;

			private const int oNKYYMxDmQpFVfDmyOyuRmXLXLBe = 2;

			private const int NEfqDWHIutBnSeDUstXJCwuVrxdm = 3;

			private readonly int dGemgsxGAzhxeMxpRLGetsDYCIKFA;

			private readonly Button[] PgHJYprMAZWgxIlwVcOifayKwvkg;

			private readonly ReadOnlyCollection<Button> fOwOYgWMJkkCCswRucXmKzuCpEXA;

			private readonly int[] OcQlQPtmcDCPlMLjYfAJHHPHgPdL;

			int CompoundElement.elementCapacity => 4;

			public IList<Button> Buttons
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return EmptyObjects<Button>.EmptyReadOnlyIListT;
					}
					return fOwOYgWMJkkCCswRucXmKzuCpEXA;
				}
			}

			public Button buttonUp
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(0);
				}
			}

			public Button buttonRight
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(1);
				}
			}

			public Button buttonDown
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(2);
				}
			}

			public Button buttonLeft
			{
				get
				{
					if (ReInput._id != xvpPtUrtXouTLVkUdkpCJFZoygnp)
					{
						ReInput.CheckInitialized(xvpPtUrtXouTLVkUdkpCJFZoygnp);
						return null;
					}
					return nhsafaLnsqmOczvkBehAHMZcBAmDA<Button>(3);
				}
			}

			internal DirectionalPad(Controller P_0, int P_1, string P_2, Button[] P_3, int[] P_4)
				: base(P_0, P_1, P_2, CompoundControllerElementType.DPad)
			{
				int num = ((P_3 != null) ? P_3.Length : 0);
				if (num != ((P_4 != null) ? P_4.Length : 0))
				{
					throw new ArgumentException("buttons.Length must equal buttonIndices.Length!");
				}
				if (num != 0 && num != 4)
				{
					throw new ArgumentException("buttons.Length must be 0 or 4! Length: " + num);
				}
				for (int i = 0; i < num; i++)
				{
					QAuMxPUGOaJdEMNWDIDVYTOmOLcD(P_3[i], P_4[i]);
				}
				PgHJYprMAZWgxIlwVcOifayKwvkg = P_3;
				OcQlQPtmcDCPlMLjYfAJHHPHgPdL = P_4;
				dGemgsxGAzhxeMxpRLGetsDYCIKFA = num;
				fOwOYgWMJkkCCswRucXmKzuCpEXA = new ReadOnlyCollection<Button>(P_3);
			}

			internal void vVeVuMQDOLyQhDuvXFZdaVjthqIP(UpdateLoopType P_0, ControllerDataUpdater P_1)
			{
				if (dGemgsxGAzhxeMxpRLGetsDYCIKFA == 0)
				{
					return;
				}
				for (int i = 0; i < PgHJYprMAZWgxIlwVcOifayKwvkg.Length; i++)
				{
					if (PgHJYprMAZWgxIlwVcOifayKwvkg[i] != null)
					{
						PgHJYprMAZWgxIlwVcOifayKwvkg[i].afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_0, OcQlQPtmcDCPlMLjYfAJHHPHgPdL[i], P_1);
					}
				}
			}
		}

		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
		public abstract class Extension
		{
			private Controller wFsExcFCXNacNiJudcsgEMtDDlrX;

			private IControllerExtensionSource cgNWCQShoQOVDFqXUgAgXUmTToVF;

			internal readonly int _reInputId;

			internal bool isJoystickConnected
			{
				get
				{
					if (wFsExcFCXNacNiJudcsgEMtDDlrX == null)
					{
						return false;
					}
					return wFsExcFCXNacNiJudcsgEMtDDlrX._isConnected;
				}
			}

			internal bool enabled
			{
				get
				{
					if (wFsExcFCXNacNiJudcsgEMtDDlrX == null)
					{
						return false;
					}
					return wFsExcFCXNacNiJudcsgEMtDDlrX.enabled;
				}
			}

			public Controller controller => wFsExcFCXNacNiJudcsgEMtDDlrX;

			internal Extension(IControllerExtensionSource P_0)
			{
				_reInputId = ReInput.id;
				AtWLjkoBbCfDlCIecLaAeMuxUeTvA(P_0);
			}

			internal Extension(Extension P_0)
				: this(P_0.cgNWCQShoQOVDFqXUgAgXUmTToVF)
			{
				wFsExcFCXNacNiJudcsgEMtDDlrX = P_0.wFsExcFCXNacNiJudcsgEMtDDlrX;
			}

			internal T GetController<T>() where T : Controller
			{
				if (wFsExcFCXNacNiJudcsgEMtDDlrX == null)
				{
					return null;
				}
				return wFsExcFCXNacNiJudcsgEMtDDlrX as T;
			}

			internal void SetController(Controller controller)
			{
				wFsExcFCXNacNiJudcsgEMtDDlrX = controller;
			}

			[CustomObfuscation(rename = false)]
			internal IControllerExtensionSource GetSource()
			{
				return cgNWCQShoQOVDFqXUgAgXUmTToVF;
			}

			internal void SetSource(Extension extension)
			{
				if (extension == null)
				{
					AtWLjkoBbCfDlCIecLaAeMuxUeTvA(null);
				}
				else
				{
					AtWLjkoBbCfDlCIecLaAeMuxUeTvA(extension.cgNWCQShoQOVDFqXUgAgXUmTToVF);
				}
			}

			private void AtWLjkoBbCfDlCIecLaAeMuxUeTvA(IControllerExtensionSource P_0)
			{
				cgNWCQShoQOVDFqXUgAgXUmTToVF = P_0;
				SourceUpdated(cgNWCQShoQOVDFqXUgAgXUmTToVF);
			}

			internal virtual void Clear()
			{
			}

			internal abstract void SourceUpdated(IControllerExtensionSource source);

			internal abstract void UpdateData(UpdateLoopType updateLoop);

			internal abstract Extension Clone();
		}

		[Serializable]
		private sealed class RoDsUjoauloFSRpHoPXPTaJrjcTT
		{
			public static readonly RoDsUjoauloFSRpHoPXPTaJrjcTT _003C_003E9 = new RoDsUjoauloFSRpHoPXPTaJrjcTT();

			public static Func<Controller, Guid, bool> _003C_003E9__166_0;

			public static Func<Controller, Type, bool> _003C_003E9__169_0;

			internal bool KnlIxDVdBwJdxZfxSFMABtnYWTfG(Controller P_0, Guid P_1)
			{
				return P_0.ImplementsTemplate(P_1);
			}

			internal bool nTCjITVUGbUCFBmnvNklIeWHZJFh(Controller P_0, Type P_1)
			{
				return P_0.ImplementsTemplate(P_1);
			}
		}

		private sealed class SluOYyYHnFxnZahKdaGDXbGlagXaA : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
		{
			private int VFZZhxEeJJrCdWAdgNPWDQsHwZPQ;

			private ControllerPollingInfo ZPuecMIwUPuwNkcCOzoZsLENaGHA;

			private int HcpDkwYhUhzzFopJPDAxezqLVneb;

			public Controller NHaKAvbsMcQhjMjcvFtHXovdLrnF;

			private int vCPioqcmhTytoCXSIkhcLvUHYGHL;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return ZPuecMIwUPuwNkcCOzoZsLENaGHA;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return ZPuecMIwUPuwNkcCOzoZsLENaGHA;
				}
			}

			[DebuggerHidden]
			public SluOYyYHnFxnZahKdaGDXbGlagXaA(int P_0)
			{
				VFZZhxEeJJrCdWAdgNPWDQsHwZPQ = P_0;
				HcpDkwYhUhzzFopJPDAxezqLVneb = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				VFZZhxEeJJrCdWAdgNPWDQsHwZPQ = -2;
			}

			private bool MoveNext()
			{
				int vFZZhxEeJJrCdWAdgNPWDQsHwZPQ = VFZZhxEeJJrCdWAdgNPWDQsHwZPQ;
				Controller nHaKAvbsMcQhjMjcvFtHXovdLrnF = NHaKAvbsMcQhjMjcvFtHXovdLrnF;
				if (vFZZhxEeJJrCdWAdgNPWDQsHwZPQ != 0)
				{
					if (vFZZhxEeJJrCdWAdgNPWDQsHwZPQ != 1)
					{
						return false;
					}
					VFZZhxEeJJrCdWAdgNPWDQsHwZPQ = -1;
					goto IL_00a0;
				}
				VFZZhxEeJJrCdWAdgNPWDQsHwZPQ = -1;
				if (ReInput._id != nHaKAvbsMcQhjMjcvFtHXovdLrnF.SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(nHaKAvbsMcQhjMjcvFtHXovdLrnF.SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return false;
				}
				nHaKAvbsMcQhjMjcvFtHXovdLrnF.UpdatePollingFrameTracking();
				vCPioqcmhTytoCXSIkhcLvUHYGHL = 0;
				goto IL_00b0;
				IL_00b0:
				if (vCPioqcmhTytoCXSIkhcLvUHYGHL < nHaKAvbsMcQhjMjcvFtHXovdLrnF._buttonCount)
				{
					if (nHaKAvbsMcQhjMjcvFtHXovdLrnF.fwGkXfUBUPCGOjirDDhztKOlWHnvA(vCPioqcmhTytoCXSIkhcLvUHYGHL, out var num))
					{
						ZPuecMIwUPuwNkcCOzoZsLENaGHA = new ControllerPollingInfo(true, -1, nHaKAvbsMcQhjMjcvFtHXovdLrnF.id, nHaKAvbsMcQhjMjcvFtHXovdLrnF._name, nHaKAvbsMcQhjMjcvFtHXovdLrnF._type, ControllerElementType.Button, vCPioqcmhTytoCXSIkhcLvUHYGHL, Pole.Positive, nHaKAvbsMcQhjMjcvFtHXovdLrnF.UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetElementIdentifierName(num), num, KeyCode.None);
						VFZZhxEeJJrCdWAdgNPWDQsHwZPQ = 1;
						return true;
					}
					goto IL_00a0;
				}
				return false;
				IL_00a0:
				vCPioqcmhTytoCXSIkhcLvUHYGHL++;
				goto IL_00b0;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
			{
				SluOYyYHnFxnZahKdaGDXbGlagXaA sluOYyYHnFxnZahKdaGDXbGlagXaA;
				if (VFZZhxEeJJrCdWAdgNPWDQsHwZPQ == -2 && HcpDkwYhUhzzFopJPDAxezqLVneb == Environment.CurrentManagedThreadId)
				{
					VFZZhxEeJJrCdWAdgNPWDQsHwZPQ = 0;
					sluOYyYHnFxnZahKdaGDXbGlagXaA = this;
				}
				else
				{
					sluOYyYHnFxnZahKdaGDXbGlagXaA = new SluOYyYHnFxnZahKdaGDXbGlagXaA(0);
					sluOYyYHnFxnZahKdaGDXbGlagXaA.NHaKAvbsMcQhjMjcvFtHXovdLrnF = NHaKAvbsMcQhjMjcvFtHXovdLrnF;
				}
				return sluOYyYHnFxnZahKdaGDXbGlagXaA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		private sealed class WTNLOHUbyebUtdJRrBvqMhPJAtinA : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
		{
			private int RJjaXoQLyWdvusTTddDTXjfZZDIQ;

			private ControllerPollingInfo QnARobrytxJedQZjgLsrngpbSkPN;

			private int MOLfhOAjqgFAFbtnHUvOnvngZyKM;

			public Controller ICYSzDevaAhcRInTPYrOOjdCpqEm;

			private int oEFlYDuaBSVFhzZTKbSOtVvvZwSj;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return QnARobrytxJedQZjgLsrngpbSkPN;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return QnARobrytxJedQZjgLsrngpbSkPN;
				}
			}

			[DebuggerHidden]
			public WTNLOHUbyebUtdJRrBvqMhPJAtinA(int P_0)
			{
				RJjaXoQLyWdvusTTddDTXjfZZDIQ = P_0;
				MOLfhOAjqgFAFbtnHUvOnvngZyKM = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				RJjaXoQLyWdvusTTddDTXjfZZDIQ = -2;
			}

			private bool MoveNext()
			{
				int rJjaXoQLyWdvusTTddDTXjfZZDIQ = RJjaXoQLyWdvusTTddDTXjfZZDIQ;
				Controller iCYSzDevaAhcRInTPYrOOjdCpqEm = ICYSzDevaAhcRInTPYrOOjdCpqEm;
				if (rJjaXoQLyWdvusTTddDTXjfZZDIQ != 0)
				{
					if (rJjaXoQLyWdvusTTddDTXjfZZDIQ != 1)
					{
						return false;
					}
					RJjaXoQLyWdvusTTddDTXjfZZDIQ = -1;
					goto IL_00a0;
				}
				RJjaXoQLyWdvusTTddDTXjfZZDIQ = -1;
				if (ReInput._id != iCYSzDevaAhcRInTPYrOOjdCpqEm.SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(iCYSzDevaAhcRInTPYrOOjdCpqEm.SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return false;
				}
				iCYSzDevaAhcRInTPYrOOjdCpqEm.UpdatePollingFrameTracking();
				oEFlYDuaBSVFhzZTKbSOtVvvZwSj = 0;
				goto IL_00b0;
				IL_00b0:
				if (oEFlYDuaBSVFhzZTKbSOtVvvZwSj < iCYSzDevaAhcRInTPYrOOjdCpqEm._buttonCount)
				{
					if (iCYSzDevaAhcRInTPYrOOjdCpqEm.HxzdPFIWBlSeHbAToIIjDicMonQOA(oEFlYDuaBSVFhzZTKbSOtVvvZwSj, out var num))
					{
						QnARobrytxJedQZjgLsrngpbSkPN = new ControllerPollingInfo(true, -1, iCYSzDevaAhcRInTPYrOOjdCpqEm.id, iCYSzDevaAhcRInTPYrOOjdCpqEm._name, iCYSzDevaAhcRInTPYrOOjdCpqEm._type, ControllerElementType.Button, oEFlYDuaBSVFhzZTKbSOtVvvZwSj, Pole.Positive, iCYSzDevaAhcRInTPYrOOjdCpqEm.UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetElementIdentifierName(num), num, KeyCode.None);
						RJjaXoQLyWdvusTTddDTXjfZZDIQ = 1;
						return true;
					}
					goto IL_00a0;
				}
				return false;
				IL_00a0:
				oEFlYDuaBSVFhzZTKbSOtVvvZwSj++;
				goto IL_00b0;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
			{
				WTNLOHUbyebUtdJRrBvqMhPJAtinA wTNLOHUbyebUtdJRrBvqMhPJAtinA;
				if (RJjaXoQLyWdvusTTddDTXjfZZDIQ == -2 && MOLfhOAjqgFAFbtnHUvOnvngZyKM == Environment.CurrentManagedThreadId)
				{
					RJjaXoQLyWdvusTTddDTXjfZZDIQ = 0;
					wTNLOHUbyebUtdJRrBvqMhPJAtinA = this;
				}
				else
				{
					wTNLOHUbyebUtdJRrBvqMhPJAtinA = new WTNLOHUbyebUtdJRrBvqMhPJAtinA(0);
					wTNLOHUbyebUtdJRrBvqMhPJAtinA.ICYSzDevaAhcRInTPYrOOjdCpqEm = ICYSzDevaAhcRInTPYrOOjdCpqEm;
				}
				return wTNLOHUbyebUtdJRrBvqMhPJAtinA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		public readonly int id;

		protected string _tag;

		protected string _name;

		private readonly DeviceLocalizationInfo qdxXDUaEvAZcguhlOqGXDjzaezso;

		protected string _hardwareName;

		protected readonly ControllerType _type;

		internal readonly Guid zyYehdPaDXciYCtKVPxEsznJTyqP;

		protected string _hardwareIdentifier;

		protected bool _isConnected;

		private Extension oWesMrDicAgNlfkQztlQHWhhZzeN;

		private bool CztGOPfiApvLbqMFHlgkutBryvyM;

		private ControllerIdentifier iVNQZGAeKWtWktzmWFejnFrgrxad;

		internal int SnkHeIsGgHerWcblJwOwvoQCoGCVA;

		protected readonly int _buttonCount;

		protected readonly Button[] buttons;

		protected readonly ReadOnlyCollection<Button> buttons_readOnly;

		private readonly IList<Element> yDQOEyRiVyztMPRBpUjDYPCMmCVw;

		private readonly ReadOnlyCollection<Element> emRmXCIbhIcAuXzVucpfvqRnaqZJ;

		private readonly IList<CompoundElement> ZwKFNbuVrQZmzYJKPDplbFoMExDrA;

		private readonly ReadOnlyCollection<CompoundElement> jtdSCyBzdxSDqHCXCKMbTjjiwSAt;

		[CustomObfuscation(rename = false)]
		internal readonly InputSource inputSource;

		internal readonly ControllerDataUpdater yZwGORAVRJPjNCmxxWIIoQgNomuqA;

		internal readonly HardwareControllerMap_Game UNRIOyvPojfCPrjRsEYcHBwwkZqS;

		internal uint WFyUDqcKJyQDAXEmOKARWhCbapCI;

		private uint hGoaMTwcbYQGiUFjdFrocBgUSUvZ;

		private uint FHnjaWyRwHBlVCykKbtAVyfILdWH;

		private ITryGetLocalizedName YaxdCrRpSKZcBfqrDWnTmUBKUPTH;

		private readonly LocalizedString yarfKzOwgQdAFQnPzcyaDSVjJARC;

		private readonly zilgekDXvTkMzThzckfvYmnJXPaEA YWXWusFBElAEnYxsHWysaQxEPDIJ;

		private Action<bool> KnBeifENscqPCtLBgjtDkZUwAQVbA;

		private IControllerTemplate[] VqOdvgztHVSAzrjsfaieWlSYRpuo;

		private ReadOnlyCollection<IControllerTemplate> xBeGOLJxtRkdDRCNsZrIfiqgOIIBb;

		private static Func<Controller, Guid, bool> SACyVqbayIGLOJqOvWRupAwqZFhGA;

		private static Func<Controller, Type, bool> sNNHqiWSxFvMOADFguDTnRODaqYj;

		internal bool TFbsbLxzEUTIermMKApCIIPMYFEsA => hGoaMTwcbYQGiUFjdFrocBgUSUvZ == ReInput.previousFrame;

		public bool enabled
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return false;
				}
				return CztGOPfiApvLbqMFHlgkutBryvyM;
			}
			set
			{
				YMYaXjiCPrJkpmbpKNcXpVseIAcFA(value);
			}
		}

		public string name
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return string.Empty;
				}
				if (!LocalizationManager.isEnabled)
				{
					return _name;
				}
				if (CnhzsmaaLAXWKxkRoAdJnqWWsbef != null && CnhzsmaaLAXWKxkRoAdJnqWWsbef.TryGetLocalizedName(out var value))
				{
					return value;
				}
				if (_type == ControllerType.Joystick && zyYehdPaDXciYCtKVPxEsznJTyqP == Consts.joystickGuid_unknownController)
				{
					return _name;
				}
				if (qdxXDUaEvAZcguhlOqGXDjzaezso == null || qdxXDUaEvAZcguhlOqGXDjzaezso.parentKeys == null)
				{
					return _name;
				}
				LocalizationManager.GetAndUpdateLocalizedString(yarfKzOwgQdAFQnPzcyaDSVjJARC, (qdxXDUaEvAZcguhlOqGXDjzaezso != null) ? qdxXDUaEvAZcguhlOqGXDjzaezso.parentKeys : null, fNDBBZXbOAvGiTXVzfEmFadoOOjj.ZjyEVyERnmGwvaLVfGpAagVLJQHN(_type), _name, out value);
				return value;
			}
			internal set
			{
				_name = text;
			}
		}

		public string tag
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return string.Empty;
				}
				return _tag;
			}
			set
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				}
				else
				{
					_tag = value;
				}
			}
		}

		public string hardwareName
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return string.Empty;
				}
				return _hardwareName;
			}
		}

		public ControllerType type
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return ControllerType.Keyboard;
				}
				return _type;
			}
		}

		public Guid hardwareTypeGuid
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return Guid.Empty;
				}
				return zyYehdPaDXciYCtKVPxEsznJTyqP;
			}
		}

		public abstract Guid deviceInstanceGuid { get; }

		public ControllerIdentifier identifier => iVNQZGAeKWtWktzmWFejnFrgrxad;

		public bool isConnected
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return false;
				}
				return _isConnected;
			}
			internal set
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				}
				else if (!flag)
				{
					Disconnected();
				}
				else
				{
					Connected();
				}
			}
		}

		public string hardwareIdentifier
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return string.Empty;
				}
				return _hardwareIdentifier;
			}
		}

		public string mapTypeString => _type.ToString() + "Map";

		public int elementCount
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return 0;
				}
				return yDQOEyRiVyztMPRBpUjDYPCMmCVw.Count;
			}
		}

		public int buttonCount
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return 0;
				}
				return _buttonCount;
			}
		}

		public IList<Element> Elements
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<Element>.EmptyReadOnlyIListT;
				}
				return emRmXCIbhIcAuXzVucpfvqRnaqZJ;
			}
		}

		public IList<CompoundElement> CompoundElements
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<CompoundElement>.EmptyReadOnlyIListT;
				}
				return jtdSCyBzdxSDqHCXCKMbTjjiwSAt;
			}
		}

		public IList<Button> Buttons
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<Button>.EmptyReadOnlyIListT;
				}
				return buttons_readOnly;
			}
		}

		public Extension extension
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return null;
				}
				return oWesMrDicAgNlfkQztlQHWhhZzeN;
			}
		}

		public IList<ControllerElementIdentifier> ElementIdentifiers
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<ControllerElementIdentifier>.EmptyReadOnlyIListT;
				}
				return UNRIOyvPojfCPrjRsEYcHBwwkZqS.elementIdentifiers_readOnly;
			}
		}

		public IList<ControllerElementIdentifier> ButtonElementIdentifiers
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<ControllerElementIdentifier>.EmptyReadOnlyIListT;
				}
				return UNRIOyvPojfCPrjRsEYcHBwwkZqS.buttonElementIdentifiers_readOnly;
			}
		}

		internal ITryGetLocalizedName CnhzsmaaLAXWKxkRoAdJnqWWsbef
		{
			get
			{
				return YaxdCrRpSKZcBfqrDWnTmUBKUPTH;
			}
			set
			{
				YaxdCrRpSKZcBfqrDWnTmUBKUPTH = yaxdCrRpSKZcBfqrDWnTmUBKUPTH;
			}
		}

		public IList<IControllerTemplate> Templates
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<IControllerTemplate>.EmptyReadOnlyIListT;
				}
				return xBeGOLJxtRkdDRCNsZrIfiqgOIIBb;
			}
		}

		public int templateCount
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return 0;
				}
				return VqOdvgztHVSAzrjsfaieWlSYRpuo.Length;
			}
		}

		internal static Func<Controller, Guid, bool> LGCbTsfsKjXUihgBjSRSfMnFskaqB => RoDsUjoauloFSRpHoPXPTaJrjcTT._003C_003E9.KnlIxDVdBwJdxZfxSFMABtnYWTfG;

		internal static Func<Controller, Type, bool> uZGcBlGhyPRcywtCOzvauDzbiJPSA => RoDsUjoauloFSRpHoPXPTaJrjcTT._003C_003E9.nTCjITVUGbUCFBmnvNklIeWHZJFh;

		internal event Action<bool> CTMeOjKJARUNRwcCVPoKffFMNnHY
		{
			add
			{
				KnBeifENscqPCtLBgjtDkZUwAQVbA = (Action<bool>)Delegate.Combine(KnBeifENscqPCtLBgjtDkZUwAQVbA, b);
			}
			remove
			{
				KnBeifENscqPCtLBgjtDkZUwAQVbA = (Action<bool>)Delegate.Remove(KnBeifENscqPCtLBgjtDkZUwAQVbA, value2);
			}
		}

		internal Controller(int P_0, InputSource P_1, string P_2, string P_3, string P_4, ControllerType P_5, Guid P_6, int P_7, bool[] P_8, HardwareButtonInfo[] P_9, HardwareControllerMap_Game P_10, Extension P_11, ControllerDataUpdater P_12)
		{
			id = P_0;
			inputSource = P_1;
			_type = P_5;
			zyYehdPaDXciYCtKVPxEsznJTyqP = P_6;
			_buttonCount = P_7;
			_name = P_2;
			_hardwareName = P_3;
			_hardwareIdentifier = P_4;
			yZwGORAVRJPjNCmxxWIIoQgNomuqA = P_12;
			UNRIOyvPojfCPrjRsEYcHBwwkZqS = P_10;
			qdxXDUaEvAZcguhlOqGXDjzaezso = P_10.deviceLocalizationInfo;
			CztGOPfiApvLbqMFHlgkutBryvyM = true;
			SnkHeIsGgHerWcblJwOwvoQCoGCVA = ReInput.id;
			yarfKzOwgQdAFQnPzcyaDSVjJARC = new LocalizedString();
			YWXWusFBElAEnYxsHWysaQxEPDIJ = new zilgekDXvTkMzThzckfvYmnJXPaEA(delegate
			{
				_ = name;
			});
			MFRDThVvTPdToHImVrXolFRUxuPCA(P_11);
			yDQOEyRiVyztMPRBpUjDYPCMmCVw = new List<Element>(P_7);
			emRmXCIbhIcAuXzVucpfvqRnaqZJ = new ReadOnlyCollection<Element>(yDQOEyRiVyztMPRBpUjDYPCMmCVw);
			ZwKFNbuVrQZmzYJKPDplbFoMExDrA = new List<CompoundElement>();
			jtdSCyBzdxSDqHCXCKMbTjjiwSAt = new ReadOnlyCollection<CompoundElement>(ZwKFNbuVrQZmzYJKPDplbFoMExDrA);
			buttons = new Button[P_7];
			if (P_8 == null || P_8.Length < P_7)
			{
				for (int num = 0; num < P_7; num++)
				{
					buttons[num] = new Button(this, P_10.buttonElementIdentifierIds[num], "Button " + num, false, (P_9 != null) ? P_9[num] : new HardwareButtonInfo());
					VSJEcXGbudCogDmpJpnIeQxXMTbBB(buttons[num]);
				}
			}
			else
			{
				for (int num2 = 0; num2 < P_7; num2++)
				{
					buttons[num2] = new Button(this, P_10.buttonElementIdentifierIds[num2], "Button " + num2, P_8[num2], (P_9 != null) ? P_9[num2] : new HardwareButtonInfo());
					VSJEcXGbudCogDmpJpnIeQxXMTbBB(buttons[num2]);
				}
			}
			buttons_readOnly = new ReadOnlyCollection<Button>(buttons);
			VqOdvgztHVSAzrjsfaieWlSYRpuo = EmptyObjects<IControllerTemplate>.array;
			xBeGOLJxtRkdDRCNsZrIfiqgOIIBb = new ReadOnlyCollection<IControllerTemplate>(VqOdvgztHVSAzrjsfaieWlSYRpuo);
			if (LocalizationManager.isEnabled && LocalizationManager.autoPrefetch)
			{
				((cAwfhgIDGfMqIqwFGxVCNiWfViqT)YWXWusFBElAEnYxsHWysaQxEPDIJ).Localize();
			}
			Connected();
		}

		internal virtual void sXPBxAVgVVidzfPmKZUCZYhRwaIf()
		{
			iVNQZGAeKWtWktzmWFejnFrgrxad = new ControllerIdentifier(this);
		}

		public virtual Element GetElementById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return null;
			}
			if (UNRIOyvPojfCPrjRsEYcHBwwkZqS == null)
			{
				return null;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0)
			{
				return null;
			}
			return buttons[buttonIndex];
		}

		public virtual CompoundElement GetCompoundElementById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return null;
			}
			int count = ZwKFNbuVrQZmzYJKPDplbFoMExDrA.Count;
			for (int i = 0; i < count; i++)
			{
				if (ZwKFNbuVrQZmzYJKPDplbFoMExDrA[i] != null && ZwKFNbuVrQZmzYJKPDplbFoMExDrA[i].id == elementIdentifierId)
				{
					return ZwKFNbuVrQZmzYJKPDplbFoMExDrA[i];
				}
			}
			return null;
		}

		[Obsolete("This method is deprecated. Use GetCompoundElementById instead.", false)]
		public virtual CompoundElement GetCompundElementById(int elementIdentifierId)
		{
			return GetCompoundElementById(elementIdentifierId);
		}

		public int GetButtonIndexById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return -1;
			}
			return UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
		}

		public ControllerElementIdentifier GetElementIdentifierById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return null;
			}
			return UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetElementIdentifierById(elementIdentifierId);
		}

		public virtual bool GetButton(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].value;
		}

		public virtual bool GetButtonDown(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].justPressed;
		}

		public virtual bool GetButtonUp(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].justReleased;
		}

		public virtual bool GetButtonChanged(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].value != buttons[index].valuePrev;
		}

		public virtual bool GetButtonPrev(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].valuePrev;
		}

		public virtual bool GetButtonDoublePressHold(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			return GetButtonDoublePressHold(index, 0f);
		}

		public virtual bool GetButtonDoublePressHold(int index, float speed)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].DoublePressedAndHeld(speed);
		}

		public virtual bool GetButtonDoublePressDown(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			return GetButtonDoublePressDown(index, 0f);
		}

		public virtual bool GetButtonDoublePressDown(int index, float speed)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return false;
			}
			return buttons[index].JustDoublePressed(speed);
		}

		public virtual double GetButtonTimePressed(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[index].timePressed;
		}

		public virtual double GetButtonTimeUnpressed(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[index].timeUnpressed;
		}

		public virtual double GetButtonLastTimePressed(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[index].lastTimePressed;
		}

		public virtual double GetButtonLastTimeUnpressed(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			if (index < 0 || index >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[index].lastTimeUnpressed;
		}

		public virtual bool GetAnyButton()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i].value)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetAnyButtonDown()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i].justPressed)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetAnyButtonUp()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i].justReleased)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetAnyButtonPrev()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i].valuePrev)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetAnyButtonChanged()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i].justChangedState)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetButtonById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].value;
		}

		public virtual bool GetButtonDownById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].justPressed;
		}

		public virtual bool GetButtonUpById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].justReleased;
		}

		public virtual bool GetButtonDoublePressHoldById(int elementIdentifierId, float speed)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].DoublePressedAndHeld(speed);
		}

		public virtual bool GetButtonDoublePressDownById(int elementIdentifierId, float speed)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].JustDoublePressed(speed);
		}

		public virtual bool GetButtonDoublePressHoldById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			return GetButtonDoublePressHold(buttonIndex, 0f);
		}

		public virtual bool GetButtonDoublePressDownById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			return GetButtonDoublePressDown(buttonIndex, 0f);
		}

		public virtual bool GetButtonPrevById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return false;
			}
			return buttons[buttonIndex].valuePrev;
		}

		public virtual double GetButtonTimePressedById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[buttonIndex].timePressed;
		}

		public virtual double GetButtonTimeUnpressedById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[buttonIndex].timeUnpressed;
		}

		public virtual double GetButtonLastTimePressedById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[buttonIndex].lastTimePressed;
		}

		public virtual double GetButtonLastTimeUnpressedById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int buttonIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetButtonIndex(elementIdentifierId);
			if (buttonIndex < 0 || buttonIndex >= _buttonCount)
			{
				return 0.0;
			}
			return buttons[buttonIndex].lastTimeUnpressed;
		}

		public virtual ControllerPollingInfo PollForFirstElement()
		{
			return PollForFirstButton();
		}

		public virtual ControllerPollingInfo PollForFirstElementDown()
		{
			return PollForFirstButtonDown();
		}

		public virtual ControllerPollingInfo PollForFirstButton()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
			}
			UpdatePollingFrameTracking();
			for (int i = 0; i < _buttonCount; i++)
			{
				if (fwGkXfUBUPCGOjirDDhztKOlWHnvA(i, out var num))
				{
					return new ControllerPollingInfo(true, -1, id, _name, _type, ControllerElementType.Button, i, Pole.Positive, UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetElementIdentifierName(num), num, KeyCode.None);
				}
			}
			return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
		}

		public virtual ControllerPollingInfo PollForFirstButtonDown()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
			}
			UpdatePollingFrameTracking();
			for (int i = 0; i < _buttonCount; i++)
			{
				if (HxzdPFIWBlSeHbAToIIjDicMonQOA(i, out var num))
				{
					return new ControllerPollingInfo(true, -1, id, _name, _type, ControllerElementType.Button, i, Pole.Positive, UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetElementIdentifierName(num), num, KeyCode.None);
				}
			}
			return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
		}

		public virtual IEnumerable<ControllerPollingInfo> PollForAllElements()
		{
			return PollForAllButtons();
		}

		public virtual IEnumerable<ControllerPollingInfo> PollForAllElementsDown()
		{
			return PollForAllButtonsDown();
		}

		[IteratorStateMachine(typeof(SluOYyYHnFxnZahKdaGDXbGlagXaA))]
		public virtual IEnumerable<ControllerPollingInfo> PollForAllButtons()
		{
			return new SluOYyYHnFxnZahKdaGDXbGlagXaA(-2)
			{
				NHaKAvbsMcQhjMjcvFtHXovdLrnF = this
			};
		}

		[IteratorStateMachine(typeof(WTNLOHUbyebUtdJRrBvqMhPJAtinA))]
		public virtual IEnumerable<ControllerPollingInfo> PollForAllButtonsDown()
		{
			return new WTNLOHUbyebUtdJRrBvqMhPJAtinA(-2)
			{
				ICYSzDevaAhcRInTPYrOOjdCpqEm = this
			};
		}

		private bool fwGkXfUBUPCGOjirDDhztKOlWHnvA(int P_0, out int P_1)
		{
			P_1 = -1;
			if (!buttons[P_0].value || buttons[P_0].jPvtwJYFXYwDJlmlygLGlTdEfLMc._excludeFromPolling)
			{
				return false;
			}
			P_1 = UNRIOyvPojfCPrjRsEYcHBwwkZqS.buttonElementIdentifierIds[P_0];
			if (P_1 < 0)
			{
				return false;
			}
			return true;
		}

		private bool HxzdPFIWBlSeHbAToIIjDicMonQOA(int P_0, out int P_1)
		{
			P_1 = -1;
			if (!buttons[P_0].justPressed || buttons[P_0].jPvtwJYFXYwDJlmlygLGlTdEfLMc._excludeFromPolling)
			{
				return false;
			}
			P_1 = UNRIOyvPojfCPrjRsEYcHBwwkZqS.buttonElementIdentifierIds[P_0];
			if (P_1 < 0)
			{
				return false;
			}
			return true;
		}

		protected void UpdatePollingFrameTracking()
		{
			if (FHnjaWyRwHBlVCykKbtAVyfILdWH == ReInput.currentFrame)
			{
				return;
			}
			hGoaMTwcbYQGiUFjdFrocBgUSUvZ = FHnjaWyRwHBlVCykKbtAVyfILdWH;
			FHnjaWyRwHBlVCykKbtAVyfILdWH = ReInput.currentFrame;
			if (!TFbsbLxzEUTIermMKApCIIPMYFEsA)
			{
				if (WFyUDqcKJyQDAXEmOKARWhCbapCI == 4294967295u)
				{
					WFyUDqcKJyQDAXEmOKARWhCbapCI = 0u;
				}
				else
				{
					WFyUDqcKJyQDAXEmOKARWhCbapCI++;
				}
			}
		}

		public virtual double GetLastTimeActive()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			return GetLastTimeActive(useRawValues: false);
		}

		public virtual double GetLastTimeActive(bool useRawValues)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			return GetLastTimeAnyButtonPressed();
		}

		public virtual double GetLastTimeAnyElementChanged()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			return GetLastTimeAnyElementChanged(useRawValues: false);
		}

		public virtual double GetLastTimeAnyElementChanged(bool useRawValues)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			return GetLastTimeAnyButtonChanged();
		}

		public double GetLastTimeAnyButtonPressed()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			if (buttons == null)
			{
				return 0.0;
			}
			double num = 0.0;
			for (int i = 0; i < buttons.Length; i++)
			{
				double lastTimePressed = buttons[i].lastTimePressed;
				if (lastTimePressed > num)
				{
					num = lastTimePressed;
				}
			}
			return num;
		}

		public double GetLastTimeAnyButtonChanged()
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			if (buttons == null)
			{
				return 0.0;
			}
			double num = 0.0;
			for (int i = 0; i < buttons.Length; i++)
			{
				double lastTimeStateChanged = buttons[i].lastTimeStateChanged;
				if (lastTimeStateChanged > num)
				{
					num = lastTimeStateChanged;
				}
			}
			return num;
		}

		public T GetExtension<T>() where T : class
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return null;
			}
			return oWesMrDicAgNlfkQztlQHWhhZzeN as T;
		}

		public IControllerTemplate GetTemplate(Guid typeGuid)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return null;
			}
			for (int i = 0; i < VqOdvgztHVSAzrjsfaieWlSYRpuo.Length; i++)
			{
				if (VqOdvgztHVSAzrjsfaieWlSYRpuo[i].typeGuid == typeGuid)
				{
					return VqOdvgztHVSAzrjsfaieWlSYRpuo[i];
				}
			}
			return null;
		}

		public IControllerTemplate GetTemplate(Type type)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return null;
			}
			for (int i = 0; i < VqOdvgztHVSAzrjsfaieWlSYRpuo.Length; i++)
			{
				if (ReflectionTools.DoesTypeImplement(VqOdvgztHVSAzrjsfaieWlSYRpuo[i].GetType(), type))
				{
					return VqOdvgztHVSAzrjsfaieWlSYRpuo[i];
				}
			}
			return null;
		}

		public T GetTemplate<T>() where T : class
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return null;
			}
			for (int i = 0; i < VqOdvgztHVSAzrjsfaieWlSYRpuo.Length; i++)
			{
				if (VqOdvgztHVSAzrjsfaieWlSYRpuo[i] as T != null)
				{
					return VqOdvgztHVSAzrjsfaieWlSYRpuo[i] as T;
				}
			}
			return null;
		}

		public bool ImplementsTemplate(Guid typeGuid)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			for (int i = 0; i < VqOdvgztHVSAzrjsfaieWlSYRpuo.Length; i++)
			{
				if (VqOdvgztHVSAzrjsfaieWlSYRpuo[i].typeGuid == typeGuid)
				{
					return true;
				}
			}
			return false;
		}

		public bool ImplementsTemplate(Type type)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			for (int i = 0; i < VqOdvgztHVSAzrjsfaieWlSYRpuo.Length; i++)
			{
				if (ReflectionTools.DoesTypeImplement(VqOdvgztHVSAzrjsfaieWlSYRpuo[i].GetType(), type))
				{
					return true;
				}
			}
			return false;
		}

		public bool ImplementsTemplate<T>() where T : class
		{
			return ImplementsTemplate(typeof(T));
		}

		internal void KbegYtcCiIutDJXwQSVkDlTUgErR(IControllerTemplate[] P_0)
		{
			if (P_0 != null)
			{
				VqOdvgztHVSAzrjsfaieWlSYRpuo = P_0;
				xBeGOLJxtRkdDRCNsZrIfiqgOIIBb = new ReadOnlyCollection<IControllerTemplate>(VqOdvgztHVSAzrjsfaieWlSYRpuo);
			}
		}

		internal virtual void PTpLZPTdIGBCXbVzlMCHCqylApVQA(UpdateLoopType P_0)
		{
			bool num = ReInput.IsInputAllowed(_type);
			int num2 = _buttonCount;
			if (num)
			{
				for (int i = 0; i < num2; i++)
				{
					if (buttons[i].hpXTymXJxGCOVLSxjNdGVAoLMVBC <= 0)
					{
						buttons[i].afeJkBinlRdwvHJUmFnGIJdBjiWVb(P_0, i, yZwGORAVRJPjNCmxxWIIoQgNomuqA);
					}
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					if (buttons[j].hpXTymXJxGCOVLSxjNdGVAoLMVBC <= 0)
					{
						buttons[j].YXaEnheWDXkoMvMdXoifLBXucVbW(P_0);
					}
				}
			}
			if (oWesMrDicAgNlfkQztlQHWhhZzeN != null)
			{
				oWesMrDicAgNlfkQztlQHWhhZzeN.UpdateData(P_0);
			}
		}

		internal virtual ButtonStateFlags faGHnbatZKmYeuwMysYwtGBKNqobb(int P_0)
		{
			if (P_0 < 0 || P_0 >= _buttonCount)
			{
				return ButtonStateFlags.Off;
			}
			return buttons[P_0].NIWDsoHmSxaJvEDDdXiYnrzlQbeTB;
		}

		internal void MFRDThVvTPdToHImVrXolFRUxuPCA(Extension P_0)
		{
			if (P_0 == null)
			{
				oWesMrDicAgNlfkQztlQHWhhZzeN = null;
				return;
			}
			if (oWesMrDicAgNlfkQztlQHWhhZzeN != null)
			{
				KSZIZVWOlOLbvBwrSsoUdeHPfSXb(P_0);
				return;
			}
			P_0.SetController(this);
			oWesMrDicAgNlfkQztlQHWhhZzeN = P_0.Clone();
		}

		internal void KSZIZVWOlOLbvBwrSsoUdeHPfSXb(Extension P_0)
		{
			if (oWesMrDicAgNlfkQztlQHWhhZzeN != null)
			{
				oWesMrDicAgNlfkQztlQHWhhZzeN.SetSource(P_0);
				oWesMrDicAgNlfkQztlQHWhhZzeN.SetController(this);
				P_0?.SetController(this);
			}
			else
			{
				MFRDThVvTPdToHImVrXolFRUxuPCA(P_0);
			}
		}

		internal virtual void bglRweWaaTFfEiIQjwyzpBARhNXC()
		{
			for (int i = 0; i < _buttonCount; i++)
			{
				if (buttons[i] != null)
				{
					buttons[i].Reset();
				}
			}
			if (yZwGORAVRJPjNCmxxWIIoQgNomuqA != null)
			{
				yZwGORAVRJPjNCmxxWIIoQgNomuqA.ClearData();
			}
			if (oWesMrDicAgNlfkQztlQHWhhZzeN != null)
			{
				oWesMrDicAgNlfkQztlQHWhhZzeN.Clear();
			}
		}

		internal virtual bool YMYaXjiCPrJkpmbpKNcXpVseIAcFA(bool P_0)
		{
			if (CztGOPfiApvLbqMFHlgkutBryvyM == P_0)
			{
				return false;
			}
			if (!P_0)
			{
				bglRweWaaTFfEiIQjwyzpBARhNXC();
			}
			CztGOPfiApvLbqMFHlgkutBryvyM = P_0;
			if (KnBeifENscqPCtLBgjtDkZUwAQVbA != null)
			{
				KnBeifENscqPCtLBgjtDkZUwAQVbA(P_0);
			}
			return true;
		}

		internal virtual void FGOWYFxDhAlAiuIOIJEWlURoAViy(ControllerMap P_0)
		{
			if (P_0 == null)
			{
				return;
			}
			try
			{
				ControllerMap.RAmMePHwhbbjmrfLAYKtBaJPbccQ();
				P_0.controllerId = id;
				IList<ActionElementMap> buttonMaps = P_0.ButtonMaps;
				for (int i = 0; i < buttonMaps.Count; i++)
				{
					VnQORoKBKYcDfQniJOyRPalZgtMZ(P_0, buttonMaps[i]);
				}
				for (int num = buttonMaps.Count - 1; num >= 0; num--)
				{
					if (buttonMaps[num].elementIndex < 0)
					{
						P_0.DeleteElementMap(buttonMaps[num].nJilCjIhFvMUTsTBcUWuYpormNsu);
					}
				}
			}
			finally
			{
				ControllerMap.oeOZZgeXJicFbaxfdmvQlNMqgCjfA();
			}
		}

		internal virtual void VnQORoKBKYcDfQniJOyRPalZgtMZ(ControllerMap P_0, ActionElementMap P_1)
		{
			if (P_1 != null && P_1._elementType == ControllerElementType.Button)
			{
				P_1.CeDmmMmdwtjVdcVPRFXshDHqgijv(P_0);
			}
		}

		internal bool BLbebkNyKNYgVESpoLfsIwKqgdIj(ActionElementMap P_0, int P_1, out float P_2, out bool P_3)
		{
			P_3 = false;
			P_2 = 0f;
			if (P_1 != P_0._actionId)
			{
				return false;
			}
			int uxemeTqImFAncCLpTkOkfOWaWKUK = P_0.uxemeTqImFAncCLpTkOkfOWaWKUK;
			if (uxemeTqImFAncCLpTkOkfOWaWKUK < 0 || uxemeTqImFAncCLpTkOkfOWaWKUK >= _buttonCount)
			{
				return false;
			}
			P_3 = buttons[uxemeTqImFAncCLpTkOkfOWaWKUK].eNSFGVUwhFxlboZfoPTFrrWgpWCl;
			float num = ((!P_3) ? (buttons[uxemeTqImFAncCLpTkOkfOWaWKUK].value ? 1f : 0f) : buttons[uxemeTqImFAncCLpTkOkfOWaWKUK].pressure);
			if (num > 0f)
			{
				if (P_0._elementType == ControllerElementType.Button)
				{
					if (P_0._axisContribution == Pole.Negative)
					{
						num *= -1f;
					}
				}
				else if (P_0._elementType == ControllerElementType.Axis)
				{
					if (P_0._axisRange == AxisRange.Full)
					{
						if (P_0._invert)
						{
							num *= -1f;
						}
					}
					else if (P_0._axisContribution == Pole.Negative)
					{
						num *= -1f;
					}
				}
			}
			P_2 = num;
			return true;
		}

		internal bool zIVcpTEOiCkgBkmaYtBIcWKMePct(ActionElementMap P_0, int P_1, bool P_2, out float P_3)
		{
			P_3 = 0f;
			if (P_1 != P_0._actionId)
			{
				return false;
			}
			float num = (P_2 ? 1f : 0f);
			if (num > 0f)
			{
				if (P_0._elementType == ControllerElementType.Button)
				{
					if (P_0._axisContribution == Pole.Negative)
					{
						num *= -1f;
					}
				}
				else if (P_0._elementType == ControllerElementType.Axis)
				{
					if (P_0._axisRange == AxisRange.Full)
					{
						if (P_0._invert)
						{
							num *= -1f;
						}
					}
					else if (P_0._axisContribution == Pole.Negative)
					{
						num *= -1f;
					}
				}
			}
			P_3 = num;
			return true;
		}

		internal void VSJEcXGbudCogDmpJpnIeQxXMTbBB(Element P_0)
		{
			if (P_0 != null)
			{
				ListTools.AddIfUnique(yDQOEyRiVyztMPRBpUjDYPCMmCVw, P_0);
			}
		}

		internal void eTsBnmdKizZCCukLuZJWklbrvzdOA(CompoundElement P_0)
		{
			if (P_0 != null)
			{
				ListTools.AddIfUnique(ZwKFNbuVrQZmzYJKPDplbFoMExDrA, P_0);
			}
		}

		internal virtual Guid qdiLdUnLondkwIKYAiKdPYAHCJvz()
		{
			return Guid.Empty;
		}

		internal virtual void rlUNcxrpXspwUOOiDFKtvkpmClqcA(bool P_0)
		{
			if (!P_0 && !ReInput.IsInputAllowed(_type) && oWesMrDicAgNlfkQztlQHWhhZzeN != null)
			{
				oWesMrDicAgNlfkQztlQHWhhZzeN.Clear();
			}
		}

		protected virtual void Connected()
		{
			_isConnected = true;
		}

		protected virtual void Disconnected()
		{
			_isConnected = false;
			if (yZwGORAVRJPjNCmxxWIIoQgNomuqA != null)
			{
				yZwGORAVRJPjNCmxxWIIoQgNomuqA.ClearData();
			}
		}

		[CompilerGenerated]
		private void DFTBjlFQoJUnSrbCxbSwQZWCadnIb()
		{
			_ = name;
		}
	}
}
