using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rewired.Config;
using Rewired.Internal.Localization;
using Rewired.Utils;
using Rewired.Utils.Classes;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired
{
	public sealed class Player : bguKJVtsagJfXPpJQeurpzlOLIYd
	{
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class ControllerHelper
		{
			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public sealed class ConflictCheckingHelper : CodeHelper
			{
				private sealed class sFUjBWoRWvBCcJjkbyFuAQIZnYFgA : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int XGbWyobTBLQMCcDUaENWiVnMKhRS;

					private ElementAssignmentConflictInfo iNPGMOldVkOvgpsGZdHpjEDUIGhEA;

					private int hqAdVjenImuaPfzGVijKelnqbkwfA;

					private int TkRjePlNVjJcLQNhwAvZYCXUeTmKA;

					public int GJjqLbnirVNYoWWmhxFRVUsaPtdm;

					private CustomControllerMap DakcqafVdLbfxbnrkwaWSkouLPuvB;

					public CustomControllerMap zsZdtbAoOLUrofelwniZsnnVLkUdA;

					public ConflictCheckingHelper MzLBcnFDGvAgDBoFGsOhKFGgxvgce;

					private bool vXmcgXgSPSMZDrCEKlyQGCsDwOAN;

					public bool ZFiFBXmiyuaQWvraERuzwFxZdqZfA;

					private bool nfCrWGtzPqtjsmyGmznkcwHHeEsF;

					public bool QVgGVsfwjyJSdQNCIdyMEAQjsVip;

					private int myJIzSCaHurDQjgWVdLUDlOWfWdBA;

					private IEnumerator<ElementAssignmentConflictInfo> igVRURdtOdfHxgInEQHUObDQAxfdA;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return iNPGMOldVkOvgpsGZdHpjEDUIGhEA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return iNPGMOldVkOvgpsGZdHpjEDUIGhEA;
						}
					}

					[DebuggerHidden]
					public sFUjBWoRWvBCcJjkbyFuAQIZnYFgA(int P_0)
					{
						XGbWyobTBLQMCcDUaENWiVnMKhRS = P_0;
						hqAdVjenImuaPfzGVijKelnqbkwfA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int xGbWyobTBLQMCcDUaENWiVnMKhRS = XGbWyobTBLQMCcDUaENWiVnMKhRS;
						if (xGbWyobTBLQMCcDUaENWiVnMKhRS == -3 || xGbWyobTBLQMCcDUaENWiVnMKhRS == 1)
						{
							try
							{
							}
							finally
							{
								XwcLaYvcIpaDyiDmelAGaVjtKXW();
							}
						}
						igVRURdtOdfHxgInEQHUObDQAxfdA = null;
						XGbWyobTBLQMCcDUaENWiVnMKhRS = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int xGbWyobTBLQMCcDUaENWiVnMKhRS = XGbWyobTBLQMCcDUaENWiVnMKhRS;
							ConflictCheckingHelper mzLBcnFDGvAgDBoFGsOhKFGgxvgce = MzLBcnFDGvAgDBoFGsOhKFGgxvgce;
							if (xGbWyobTBLQMCcDUaENWiVnMKhRS != 0)
							{
								if (xGbWyobTBLQMCcDUaENWiVnMKhRS != 1)
								{
									return false;
								}
								XGbWyobTBLQMCcDUaENWiVnMKhRS = -3;
								goto IL_00eb;
							}
							XGbWyobTBLQMCcDUaENWiVnMKhRS = -1;
							if (TkRjePlNVjJcLQNhwAvZYCXUeTmKA < 0 || DakcqafVdLbfxbnrkwaWSkouLPuvB == null)
							{
								return false;
							}
							myJIzSCaHurDQjgWVdLUDlOWfWdBA = 0;
							goto IL_0117;
							IL_00eb:
							if (igVRURdtOdfHxgInEQHUObDQAxfdA.MoveNext())
							{
								ElementAssignmentConflictInfo current = igVRURdtOdfHxgInEQHUObDQAxfdA.Current;
								iNPGMOldVkOvgpsGZdHpjEDUIGhEA = current;
								XGbWyobTBLQMCcDUaENWiVnMKhRS = 1;
								return true;
							}
							XwcLaYvcIpaDyiDmelAGaVjtKXW();
							igVRURdtOdfHxgInEQHUObDQAxfdA = null;
							goto IL_0105;
							IL_0117:
							if (myJIzSCaHurDQjgWVdLUDlOWfWdBA < mzLBcnFDGvAgDBoFGsOhKFGgxvgce.YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA())
							{
								if (mzLBcnFDGvAgDBoFGsOhKFGgxvgce.YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(myJIzSCaHurDQjgWVdLUDlOWfWdBA).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == TkRjePlNVjJcLQNhwAvZYCXUeTmKA)
								{
									igVRURdtOdfHxgInEQHUObDQAxfdA = mzLBcnFDGvAgDBoFGsOhKFGgxvgce.nBsAqDGgxwAxzRoUafYXAbtBPhVXb(ControllerType.Custom, TkRjePlNVjJcLQNhwAvZYCXUeTmKA, DakcqafVdLbfxbnrkwaWSkouLPuvB, vXmcgXgSPSMZDrCEKlyQGCsDwOAN, nfCrWGtzPqtjsmyGmznkcwHHeEsF, mzLBcnFDGvAgDBoFGsOhKFGgxvgce.YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(myJIzSCaHurDQjgWVdLUDlOWfWdBA).GgWdqFddlhBZqeFBwCaEiHGCVWTqA).GetEnumerator();
									XGbWyobTBLQMCcDUaENWiVnMKhRS = -3;
									goto IL_00eb;
								}
								goto IL_0105;
							}
							return false;
							IL_0105:
							myJIzSCaHurDQjgWVdLUDlOWfWdBA++;
							goto IL_0117;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void XwcLaYvcIpaDyiDmelAGaVjtKXW()
					{
						XGbWyobTBLQMCcDUaENWiVnMKhRS = -1;
						if (igVRURdtOdfHxgInEQHUObDQAxfdA != null)
						{
							igVRURdtOdfHxgInEQHUObDQAxfdA.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						sFUjBWoRWvBCcJjkbyFuAQIZnYFgA sFUjBWoRWvBCcJjkbyFuAQIZnYFgA2;
						if (XGbWyobTBLQMCcDUaENWiVnMKhRS == -2 && hqAdVjenImuaPfzGVijKelnqbkwfA == Environment.CurrentManagedThreadId)
						{
							XGbWyobTBLQMCcDUaENWiVnMKhRS = 0;
							sFUjBWoRWvBCcJjkbyFuAQIZnYFgA2 = this;
						}
						else
						{
							sFUjBWoRWvBCcJjkbyFuAQIZnYFgA2 = new sFUjBWoRWvBCcJjkbyFuAQIZnYFgA(0);
							sFUjBWoRWvBCcJjkbyFuAQIZnYFgA2.MzLBcnFDGvAgDBoFGsOhKFGgxvgce = MzLBcnFDGvAgDBoFGsOhKFGgxvgce;
						}
						sFUjBWoRWvBCcJjkbyFuAQIZnYFgA2.TkRjePlNVjJcLQNhwAvZYCXUeTmKA = GJjqLbnirVNYoWWmhxFRVUsaPtdm;
						sFUjBWoRWvBCcJjkbyFuAQIZnYFgA2.DakcqafVdLbfxbnrkwaWSkouLPuvB = zsZdtbAoOLUrofelwniZsnnVLkUdA;
						sFUjBWoRWvBCcJjkbyFuAQIZnYFgA2.vXmcgXgSPSMZDrCEKlyQGCsDwOAN = ZFiFBXmiyuaQWvraERuzwFxZdqZfA;
						sFUjBWoRWvBCcJjkbyFuAQIZnYFgA2.nfCrWGtzPqtjsmyGmznkcwHHeEsF = QVgGVsfwjyJSdQNCIdyMEAQjsVip;
						return sFUjBWoRWvBCcJjkbyFuAQIZnYFgA2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class uCdAUSaynPXuKZvRtDCOwHkZHkAp : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int zBubkFTEJJkLFnZoclXETogEBOZL;

					private ElementAssignmentConflictInfo mSPFECiNUlHvROFceUXTRTuKPSYrA;

					private int lurFRvFVBVGVwXTsvjpfmqttOSGyA;

					private int iSZpdXukwEeWJSGmwuAUdhQxcQDo;

					public int vuBdOxGPZRGnwDUKXkpJYdyjVZCRA;

					private ActionElementMap fRuBWoyfNkAljDmnlIdgFooGJFLfb;

					public ActionElementMap GOJcFXZUQozAnrWpfTrRUdDxKjsV;

					public ConflictCheckingHelper fFruoJrfLbhinChIhjnqGZKrwtfqA;

					private CustomControllerMap GfyfeXDjhTxBeSBoeVqtBUWClggCA;

					public CustomControllerMap YIajnaHfAFeRBHxKtyuZefvqJdbq;

					private bool PWMqbPqAXbgMyaSWYlRMfOnFafQFA;

					public bool IsjEGtoMzUKuVVFPhDbsekmmpoqeA;

					private bool wSYcJOMkYhPRkKZctudgEdbQfULAA;

					public bool PRRkvFWEQqykLgdtHFGEFZdqwhMw;

					private int intdfjMdKJieajHdUFMQWyijBmJQ;

					private IEnumerator<ElementAssignmentConflictInfo> kcNLPxyuMoGKHqLSuqsYtBTSkTSQ;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return mSPFECiNUlHvROFceUXTRTuKPSYrA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return mSPFECiNUlHvROFceUXTRTuKPSYrA;
						}
					}

					[DebuggerHidden]
					public uCdAUSaynPXuKZvRtDCOwHkZHkAp(int P_0)
					{
						zBubkFTEJJkLFnZoclXETogEBOZL = P_0;
						lurFRvFVBVGVwXTsvjpfmqttOSGyA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = zBubkFTEJJkLFnZoclXETogEBOZL;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								GzfqSEnJOWAdxVFfhxruVwAOfQpI();
							}
						}
						kcNLPxyuMoGKHqLSuqsYtBTSkTSQ = null;
						zBubkFTEJJkLFnZoclXETogEBOZL = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = zBubkFTEJJkLFnZoclXETogEBOZL;
							ConflictCheckingHelper conflictCheckingHelper = fFruoJrfLbhinChIhjnqGZKrwtfqA;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								zBubkFTEJJkLFnZoclXETogEBOZL = -3;
								goto IL_00f1;
							}
							zBubkFTEJJkLFnZoclXETogEBOZL = -1;
							if (iSZpdXukwEeWJSGmwuAUdhQxcQDo < 0 || fRuBWoyfNkAljDmnlIdgFooGJFLfb == null)
							{
								return false;
							}
							intdfjMdKJieajHdUFMQWyijBmJQ = 0;
							goto IL_011d;
							IL_00f1:
							if (kcNLPxyuMoGKHqLSuqsYtBTSkTSQ.MoveNext())
							{
								ElementAssignmentConflictInfo current = kcNLPxyuMoGKHqLSuqsYtBTSkTSQ.Current;
								mSPFECiNUlHvROFceUXTRTuKPSYrA = current;
								zBubkFTEJJkLFnZoclXETogEBOZL = 1;
								return true;
							}
							GzfqSEnJOWAdxVFfhxruVwAOfQpI();
							kcNLPxyuMoGKHqLSuqsYtBTSkTSQ = null;
							goto IL_010b;
							IL_011d:
							if (intdfjMdKJieajHdUFMQWyijBmJQ < conflictCheckingHelper.YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA())
							{
								if (conflictCheckingHelper.YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(intdfjMdKJieajHdUFMQWyijBmJQ).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == iSZpdXukwEeWJSGmwuAUdhQxcQDo)
								{
									kcNLPxyuMoGKHqLSuqsYtBTSkTSQ = conflictCheckingHelper.VWzCqXLGGBbVBDfGyrTEnCVZcAZfA(ControllerType.Custom, iSZpdXukwEeWJSGmwuAUdhQxcQDo, GfyfeXDjhTxBeSBoeVqtBUWClggCA, fRuBWoyfNkAljDmnlIdgFooGJFLfb, PWMqbPqAXbgMyaSWYlRMfOnFafQFA, wSYcJOMkYhPRkKZctudgEdbQfULAA, conflictCheckingHelper.YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(intdfjMdKJieajHdUFMQWyijBmJQ).GgWdqFddlhBZqeFBwCaEiHGCVWTqA).GetEnumerator();
									zBubkFTEJJkLFnZoclXETogEBOZL = -3;
									goto IL_00f1;
								}
								goto IL_010b;
							}
							return false;
							IL_010b:
							intdfjMdKJieajHdUFMQWyijBmJQ++;
							goto IL_011d;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void GzfqSEnJOWAdxVFfhxruVwAOfQpI()
					{
						zBubkFTEJJkLFnZoclXETogEBOZL = -1;
						if (kcNLPxyuMoGKHqLSuqsYtBTSkTSQ != null)
						{
							kcNLPxyuMoGKHqLSuqsYtBTSkTSQ.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						uCdAUSaynPXuKZvRtDCOwHkZHkAp uCdAUSaynPXuKZvRtDCOwHkZHkAp2;
						if (zBubkFTEJJkLFnZoclXETogEBOZL == -2 && lurFRvFVBVGVwXTsvjpfmqttOSGyA == Environment.CurrentManagedThreadId)
						{
							zBubkFTEJJkLFnZoclXETogEBOZL = 0;
							uCdAUSaynPXuKZvRtDCOwHkZHkAp2 = this;
						}
						else
						{
							uCdAUSaynPXuKZvRtDCOwHkZHkAp2 = new uCdAUSaynPXuKZvRtDCOwHkZHkAp(0);
							uCdAUSaynPXuKZvRtDCOwHkZHkAp2.fFruoJrfLbhinChIhjnqGZKrwtfqA = fFruoJrfLbhinChIhjnqGZKrwtfqA;
						}
						uCdAUSaynPXuKZvRtDCOwHkZHkAp2.iSZpdXukwEeWJSGmwuAUdhQxcQDo = vuBdOxGPZRGnwDUKXkpJYdyjVZCRA;
						uCdAUSaynPXuKZvRtDCOwHkZHkAp2.GfyfeXDjhTxBeSBoeVqtBUWClggCA = YIajnaHfAFeRBHxKtyuZefvqJdbq;
						uCdAUSaynPXuKZvRtDCOwHkZHkAp2.fRuBWoyfNkAljDmnlIdgFooGJFLfb = GOJcFXZUQozAnrWpfTrRUdDxKjsV;
						uCdAUSaynPXuKZvRtDCOwHkZHkAp2.PWMqbPqAXbgMyaSWYlRMfOnFafQFA = IsjEGtoMzUKuVVFPhDbsekmmpoqeA;
						uCdAUSaynPXuKZvRtDCOwHkZHkAp2.wSYcJOMkYhPRkKZctudgEdbQfULAA = PRRkvFWEQqykLgdtHFGEFZdqwhMw;
						return uCdAUSaynPXuKZvRtDCOwHkZHkAp2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class wHIjoWuYAVrzmfgIVsiySJSsMxul : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int FYVmNopAtvxgLmngjxfqWoqmTqHH;

					private ElementAssignmentConflictInfo obBNHmUqPlvPEmiRCoNbglYMTtqg;

					private int iNjOrNjUVCwuZeneNvPtwyHbhUPJ;

					private ElementAssignmentConflictCheck VAJoKsUaLIcbabOoKmKsKbeQmRAk;

					public ElementAssignmentConflictCheck HaHcsSCPTayPVuLjlbliOjoDQLWz;

					public ConflictCheckingHelper ecPwMxpSVgeSzTOlYxFFGSvZaWSU;

					private bool aTtcFpjFLvYgHOBdVSqaPHNPTUBG;

					public bool FQdneaQKnfFnPHaYIJqiAcGhcqCpc;

					private bool LKDbIbdAiXxDFejscGVqkVLZDcfcA;

					public bool AxnRDvwkNUxDAhJiEuCsssAfkBqL;

					private int JJQXiHOYaugxBioGofdEtohvnyoV;

					private IEnumerator<ElementAssignmentConflictInfo> LUVGYOtDfwKVXIotYChKLvvZjqqR;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return obBNHmUqPlvPEmiRCoNbglYMTtqg;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return obBNHmUqPlvPEmiRCoNbglYMTtqg;
						}
					}

					[DebuggerHidden]
					public wHIjoWuYAVrzmfgIVsiySJSsMxul(int P_0)
					{
						FYVmNopAtvxgLmngjxfqWoqmTqHH = P_0;
						iNjOrNjUVCwuZeneNvPtwyHbhUPJ = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int fYVmNopAtvxgLmngjxfqWoqmTqHH = FYVmNopAtvxgLmngjxfqWoqmTqHH;
						if (fYVmNopAtvxgLmngjxfqWoqmTqHH == -3 || fYVmNopAtvxgLmngjxfqWoqmTqHH == 1)
						{
							try
							{
							}
							finally
							{
								BdxQsgVToEbNtSajXToIRUBeDWGc();
							}
						}
						LUVGYOtDfwKVXIotYChKLvvZjqqR = null;
						FYVmNopAtvxgLmngjxfqWoqmTqHH = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int fYVmNopAtvxgLmngjxfqWoqmTqHH = FYVmNopAtvxgLmngjxfqWoqmTqHH;
							ConflictCheckingHelper conflictCheckingHelper = ecPwMxpSVgeSzTOlYxFFGSvZaWSU;
							if (fYVmNopAtvxgLmngjxfqWoqmTqHH != 0)
							{
								if (fYVmNopAtvxgLmngjxfqWoqmTqHH != 1)
								{
									return false;
								}
								FYVmNopAtvxgLmngjxfqWoqmTqHH = -3;
								goto IL_00f3;
							}
							FYVmNopAtvxgLmngjxfqWoqmTqHH = -1;
							if (VAJoKsUaLIcbabOoKmKsKbeQmRAk.controllerId < 0 || VAJoKsUaLIcbabOoKmKsKbeQmRAk.elementAssignmentType == ElementAssignmentType.KeyboardKey)
							{
								return false;
							}
							JJQXiHOYaugxBioGofdEtohvnyoV = 0;
							goto IL_011f;
							IL_00f3:
							if (LUVGYOtDfwKVXIotYChKLvvZjqqR.MoveNext())
							{
								ElementAssignmentConflictInfo current = LUVGYOtDfwKVXIotYChKLvvZjqqR.Current;
								obBNHmUqPlvPEmiRCoNbglYMTtqg = current;
								FYVmNopAtvxgLmngjxfqWoqmTqHH = 1;
								return true;
							}
							BdxQsgVToEbNtSajXToIRUBeDWGc();
							LUVGYOtDfwKVXIotYChKLvvZjqqR = null;
							goto IL_010d;
							IL_011f:
							if (JJQXiHOYaugxBioGofdEtohvnyoV < conflictCheckingHelper.YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA())
							{
								if (conflictCheckingHelper.YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(JJQXiHOYaugxBioGofdEtohvnyoV).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == VAJoKsUaLIcbabOoKmKsKbeQmRAk.controllerId)
								{
									LUVGYOtDfwKVXIotYChKLvvZjqqR = conflictCheckingHelper.yPHbsWdMNegyAelNKdHhyecMMPrcB(VAJoKsUaLIcbabOoKmKsKbeQmRAk, aTtcFpjFLvYgHOBdVSqaPHNPTUBG, LKDbIbdAiXxDFejscGVqkVLZDcfcA, conflictCheckingHelper.YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(JJQXiHOYaugxBioGofdEtohvnyoV).GgWdqFddlhBZqeFBwCaEiHGCVWTqA).GetEnumerator();
									FYVmNopAtvxgLmngjxfqWoqmTqHH = -3;
									goto IL_00f3;
								}
								goto IL_010d;
							}
							return false;
							IL_010d:
							JJQXiHOYaugxBioGofdEtohvnyoV++;
							goto IL_011f;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void BdxQsgVToEbNtSajXToIRUBeDWGc()
					{
						FYVmNopAtvxgLmngjxfqWoqmTqHH = -1;
						if (LUVGYOtDfwKVXIotYChKLvvZjqqR != null)
						{
							LUVGYOtDfwKVXIotYChKLvvZjqqR.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						wHIjoWuYAVrzmfgIVsiySJSsMxul wHIjoWuYAVrzmfgIVsiySJSsMxul2;
						if (FYVmNopAtvxgLmngjxfqWoqmTqHH == -2 && iNjOrNjUVCwuZeneNvPtwyHbhUPJ == Environment.CurrentManagedThreadId)
						{
							FYVmNopAtvxgLmngjxfqWoqmTqHH = 0;
							wHIjoWuYAVrzmfgIVsiySJSsMxul2 = this;
						}
						else
						{
							wHIjoWuYAVrzmfgIVsiySJSsMxul2 = new wHIjoWuYAVrzmfgIVsiySJSsMxul(0);
							wHIjoWuYAVrzmfgIVsiySJSsMxul2.ecPwMxpSVgeSzTOlYxFFGSvZaWSU = ecPwMxpSVgeSzTOlYxFFGSvZaWSU;
						}
						wHIjoWuYAVrzmfgIVsiySJSsMxul2.VAJoKsUaLIcbabOoKmKsKbeQmRAk = HaHcsSCPTayPVuLjlbliOjoDQLWz;
						wHIjoWuYAVrzmfgIVsiySJSsMxul2.aTtcFpjFLvYgHOBdVSqaPHNPTUBG = FQdneaQKnfFnPHaYIJqiAcGhcqCpc;
						wHIjoWuYAVrzmfgIVsiySJSsMxul2.LKDbIbdAiXxDFejscGVqkVLZDcfcA = AxnRDvwkNUxDAhJiEuCsssAfkBqL;
						return wHIjoWuYAVrzmfgIVsiySJSsMxul2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class NmmQkrvndwFaCWOWSFbvRtCwTvxy<_0001> : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable where _0001 : ControllerMap
				{
					private int yjiChBHaXAwrXBctXSOwxVNYvRyL;

					private ElementAssignmentConflictInfo pcxFxxsOmUCCuDnbkcCkBbBbyjTS;

					private int TRAPXNjcxBCUGAKSfCdaYsBRcnJoA;

					private global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> rXtHaEhDcBjbFhRSrZrOditZqbEk;

					public global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> SYKoyllvoODsmpzDGiCEcpMhHxrL;

					private _0001 IykUnNHbcwAngftjGffOJlDGtZTK;

					public _0001 zNiXWOXAJkqGbTekVSKxialzRvEn;

					private bool iZikCBJCnMWnKswoDZOoorfJJOrd;

					public bool USOpVgbMepkmcUkurHggUFhLkUpD;

					private bool fIEyEOjzPaRBtiHAbNaGlktKamlI;

					public bool BJvyopErriSwUGBXeNNfoOydYcDb;

					public ConflictCheckingHelper fPvcNbBOtFCsdkVqTTDZgpsFIkFCA;

					private ControllerType KkUhvxyaklaVvkmFmkSuKQuIBQMI;

					public ControllerType XUcmNIrFMSvGMbLcIrNpaQsAsncj;

					private int nASKBnAfJUgSVnEUzDCIEHVzjfFeb;

					public int qfdVgkXxrmjFpGocqEcIEwhkWEViA;

					private InputMapCategory nnEnMaEyfojkPkntmQXSYkYOlIoR;

					private int JkRjUzQffKnqAiodTwTGfInkZWOL;

					private IEnumerator<ElementAssignmentConflictInfo> ieDwBubTmmFNJxgjZCGQyxBNnfun;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return pcxFxxsOmUCCuDnbkcCkBbBbyjTS;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return pcxFxxsOmUCCuDnbkcCkBbBbyjTS;
						}
					}

					[DebuggerHidden]
					public NmmQkrvndwFaCWOWSFbvRtCwTvxy(int P_0)
					{
						yjiChBHaXAwrXBctXSOwxVNYvRyL = P_0;
						TRAPXNjcxBCUGAKSfCdaYsBRcnJoA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = yjiChBHaXAwrXBctXSOwxVNYvRyL;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								YNIgBDWgoHLLytXIKmVHBZsWpyqt();
							}
						}
						nnEnMaEyfojkPkntmQXSYkYOlIoR = null;
						ieDwBubTmmFNJxgjZCGQyxBNnfun = null;
						yjiChBHaXAwrXBctXSOwxVNYvRyL = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = yjiChBHaXAwrXBctXSOwxVNYvRyL;
							ConflictCheckingHelper conflictCheckingHelper = fPvcNbBOtFCsdkVqTTDZgpsFIkFCA;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								yjiChBHaXAwrXBctXSOwxVNYvRyL = -3;
								goto IL_014a;
							}
							yjiChBHaXAwrXBctXSOwxVNYvRyL = -1;
							if (rXtHaEhDcBjbFhRSrZrOditZqbEk == null || IykUnNHbcwAngftjGffOJlDGtZTK == null)
							{
								return false;
							}
							nnEnMaEyfojkPkntmQXSYkYOlIoR = ReInput.mapping.GetMapCategory(IykUnNHbcwAngftjGffOJlDGtZTK.categoryId);
							if (nnEnMaEyfojkPkntmQXSYkYOlIoR == null)
							{
								return false;
							}
							JkRjUzQffKnqAiodTwTGfInkZWOL = 0;
							goto IL_0176;
							IL_0176:
							if (JkRjUzQffKnqAiodTwTGfInkZWOL < rXtHaEhDcBjbFhRSrZrOditZqbEk.GeYULXtFpYgsaXqXZIDwegpjOMgW())
							{
								ControllerMap controllerMap = rXtHaEhDcBjbFhRSrZrOditZqbEk.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(JkRjUzQffKnqAiodTwTGfInkZWOL);
								if ((!iZikCBJCnMWnKswoDZOoorfJJOrd || controllerMap.enabled) && (fIEyEOjzPaRBtiHAbNaGlktKamlI || !conflictCheckingHelper.ZMSnrXwatOJKXFUOINkdsMMJbHqEA(nnEnMaEyfojkPkntmQXSYkYOlIoR, controllerMap)))
								{
									ieDwBubTmmFNJxgjZCGQyxBNnfun = controllerMap.ElementAssignmentConflicts(IykUnNHbcwAngftjGffOJlDGtZTK, iZikCBJCnMWnKswoDZOoorfJJOrd).GetEnumerator();
									yjiChBHaXAwrXBctXSOwxVNYvRyL = -3;
									goto IL_014a;
								}
								goto IL_0164;
							}
							return false;
							IL_014a:
							if (ieDwBubTmmFNJxgjZCGQyxBNnfun.MoveNext())
							{
								ElementAssignmentConflictInfo current = ieDwBubTmmFNJxgjZCGQyxBNnfun.Current;
								ElementAssignmentConflictInfo elementAssignmentConflictInfo = new ElementAssignmentConflictInfo(current);
								elementAssignmentConflictInfo.playerId = conflictCheckingHelper.fjigivZpzAPHFCyeaiNCiGwNAbWf.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								elementAssignmentConflictInfo.controllerType = KkUhvxyaklaVvkmFmkSuKQuIBQMI;
								elementAssignmentConflictInfo.controllerId = nASKBnAfJUgSVnEUzDCIEHVzjfFeb;
								pcxFxxsOmUCCuDnbkcCkBbBbyjTS = elementAssignmentConflictInfo;
								yjiChBHaXAwrXBctXSOwxVNYvRyL = 1;
								return true;
							}
							YNIgBDWgoHLLytXIKmVHBZsWpyqt();
							ieDwBubTmmFNJxgjZCGQyxBNnfun = null;
							goto IL_0164;
							IL_0164:
							JkRjUzQffKnqAiodTwTGfInkZWOL++;
							goto IL_0176;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void YNIgBDWgoHLLytXIKmVHBZsWpyqt()
					{
						yjiChBHaXAwrXBctXSOwxVNYvRyL = -1;
						if (ieDwBubTmmFNJxgjZCGQyxBNnfun != null)
						{
							ieDwBubTmmFNJxgjZCGQyxBNnfun.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						NmmQkrvndwFaCWOWSFbvRtCwTvxy<_0001> nmmQkrvndwFaCWOWSFbvRtCwTvxy;
						if (yjiChBHaXAwrXBctXSOwxVNYvRyL == -2 && TRAPXNjcxBCUGAKSfCdaYsBRcnJoA == Environment.CurrentManagedThreadId)
						{
							yjiChBHaXAwrXBctXSOwxVNYvRyL = 0;
							nmmQkrvndwFaCWOWSFbvRtCwTvxy = this;
						}
						else
						{
							nmmQkrvndwFaCWOWSFbvRtCwTvxy = new NmmQkrvndwFaCWOWSFbvRtCwTvxy<_0001>(0);
							nmmQkrvndwFaCWOWSFbvRtCwTvxy.fPvcNbBOtFCsdkVqTTDZgpsFIkFCA = fPvcNbBOtFCsdkVqTTDZgpsFIkFCA;
						}
						nmmQkrvndwFaCWOWSFbvRtCwTvxy.KkUhvxyaklaVvkmFmkSuKQuIBQMI = XUcmNIrFMSvGMbLcIrNpaQsAsncj;
						nmmQkrvndwFaCWOWSFbvRtCwTvxy.nASKBnAfJUgSVnEUzDCIEHVzjfFeb = qfdVgkXxrmjFpGocqEcIEwhkWEViA;
						nmmQkrvndwFaCWOWSFbvRtCwTvxy.IykUnNHbcwAngftjGffOJlDGtZTK = zNiXWOXAJkqGbTekVSKxialzRvEn;
						nmmQkrvndwFaCWOWSFbvRtCwTvxy.iZikCBJCnMWnKswoDZOoorfJJOrd = USOpVgbMepkmcUkurHggUFhLkUpD;
						nmmQkrvndwFaCWOWSFbvRtCwTvxy.fIEyEOjzPaRBtiHAbNaGlktKamlI = BJvyopErriSwUGBXeNNfoOydYcDb;
						nmmQkrvndwFaCWOWSFbvRtCwTvxy.rXtHaEhDcBjbFhRSrZrOditZqbEk = SYKoyllvoODsmpzDGiCEcpMhHxrL;
						return nmmQkrvndwFaCWOWSFbvRtCwTvxy;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class eNcbsNnBTGFEnRBcPHCKoVHkJJBj<_0001> : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable where _0001 : ControllerMap
				{
					private int FBVioGmeMKeHRSawwHoMFsmduvIG;

					private ElementAssignmentConflictInfo YxCkAhmKidpWoOimcaFtidRKPxmO;

					private int FlAtIlHkkdOseFFxQdcjTINxqtRN;

					private global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> fMSOaPyBxqkZvuHlbEGphHCjRdPP;

					public global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> yvvLzbNnZSDVFoUbbjcWnkGGcOfr;

					private ActionElementMap HIhreEzKDeABdWReQzydugysrLmv;

					public ActionElementMap xJaimwJZzuxDRrCrcJpSjbGmLIDb;

					private _0001 rINdOshVOfKdVpXVlHCFOSlYhtbC;

					public _0001 jpPRjCibEOOtODojLnBSwOsbFIcf;

					private bool qRMgmNKJJMeDZOfaLNGEstqQgifP;

					public bool FYtTEUfKpIuzDgbvAofbbKquuaSx;

					private bool oORHkNwkGwbNJLtzNiMqQiQyYVgu;

					public bool wpwPAxoFImDEYlvZsJQbcayXgdMV;

					public ConflictCheckingHelper KkKonfOqGacAAVFSmFOAAvyhEBNEb;

					private ControllerType iGjbSYkwNitJBFhHmfMYpqRDMOGGA;

					public ControllerType EzZpsMbcpCkajvDLAeaFtGZLCFbN;

					private int XtOFWEFIvAjXbQCDllsDOxOpuuKk;

					public int WIodLvGMnfnTAUdLeCYBdIWitlylB;

					private InputMapCategory paZAyvCPHflJpKeFLtxVOpOBlHdN;

					private int coaWFTfXbvrcsZhlRjgRBEDWtnDi;

					private IEnumerator<ElementAssignmentConflictInfo> cjeDTTiGTlAtxUYfRAgseLiDnIcDc;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return YxCkAhmKidpWoOimcaFtidRKPxmO;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return YxCkAhmKidpWoOimcaFtidRKPxmO;
						}
					}

					[DebuggerHidden]
					public eNcbsNnBTGFEnRBcPHCKoVHkJJBj(int P_0)
					{
						FBVioGmeMKeHRSawwHoMFsmduvIG = P_0;
						FlAtIlHkkdOseFFxQdcjTINxqtRN = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int fBVioGmeMKeHRSawwHoMFsmduvIG = FBVioGmeMKeHRSawwHoMFsmduvIG;
						if (fBVioGmeMKeHRSawwHoMFsmduvIG == -3 || fBVioGmeMKeHRSawwHoMFsmduvIG == 1)
						{
							try
							{
							}
							finally
							{
								oVJUnkwJXnEtwMqyUVFrpNoBNDFh();
							}
						}
						paZAyvCPHflJpKeFLtxVOpOBlHdN = null;
						cjeDTTiGTlAtxUYfRAgseLiDnIcDc = null;
						FBVioGmeMKeHRSawwHoMFsmduvIG = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int fBVioGmeMKeHRSawwHoMFsmduvIG = FBVioGmeMKeHRSawwHoMFsmduvIG;
							ConflictCheckingHelper kkKonfOqGacAAVFSmFOAAvyhEBNEb = KkKonfOqGacAAVFSmFOAAvyhEBNEb;
							if (fBVioGmeMKeHRSawwHoMFsmduvIG != 0)
							{
								if (fBVioGmeMKeHRSawwHoMFsmduvIG != 1)
								{
									return false;
								}
								FBVioGmeMKeHRSawwHoMFsmduvIG = -3;
								goto IL_0141;
							}
							FBVioGmeMKeHRSawwHoMFsmduvIG = -1;
							if (fMSOaPyBxqkZvuHlbEGphHCjRdPP == null || HIhreEzKDeABdWReQzydugysrLmv == null)
							{
								return false;
							}
							paZAyvCPHflJpKeFLtxVOpOBlHdN = ((rINdOshVOfKdVpXVlHCFOSlYhtbC != null) ? ReInput.mapping.GetMapCategory(rINdOshVOfKdVpXVlHCFOSlYhtbC.categoryId) : null);
							coaWFTfXbvrcsZhlRjgRBEDWtnDi = 0;
							goto IL_016d;
							IL_016d:
							if (coaWFTfXbvrcsZhlRjgRBEDWtnDi < fMSOaPyBxqkZvuHlbEGphHCjRdPP.GeYULXtFpYgsaXqXZIDwegpjOMgW())
							{
								ControllerMap controllerMap = fMSOaPyBxqkZvuHlbEGphHCjRdPP.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(coaWFTfXbvrcsZhlRjgRBEDWtnDi);
								if ((!qRMgmNKJJMeDZOfaLNGEstqQgifP || controllerMap.enabled) && (oORHkNwkGwbNJLtzNiMqQiQyYVgu || !kkKonfOqGacAAVFSmFOAAvyhEBNEb.ZMSnrXwatOJKXFUOINkdsMMJbHqEA(paZAyvCPHflJpKeFLtxVOpOBlHdN, controllerMap)))
								{
									cjeDTTiGTlAtxUYfRAgseLiDnIcDc = controllerMap.ElementAssignmentConflicts(HIhreEzKDeABdWReQzydugysrLmv, qRMgmNKJJMeDZOfaLNGEstqQgifP).GetEnumerator();
									FBVioGmeMKeHRSawwHoMFsmduvIG = -3;
									goto IL_0141;
								}
								goto IL_015b;
							}
							return false;
							IL_015b:
							coaWFTfXbvrcsZhlRjgRBEDWtnDi++;
							goto IL_016d;
							IL_0141:
							if (cjeDTTiGTlAtxUYfRAgseLiDnIcDc.MoveNext())
							{
								ElementAssignmentConflictInfo current = cjeDTTiGTlAtxUYfRAgseLiDnIcDc.Current;
								ElementAssignmentConflictInfo yxCkAhmKidpWoOimcaFtidRKPxmO = new ElementAssignmentConflictInfo(current);
								yxCkAhmKidpWoOimcaFtidRKPxmO.playerId = kkKonfOqGacAAVFSmFOAAvyhEBNEb.fjigivZpzAPHFCyeaiNCiGwNAbWf.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								yxCkAhmKidpWoOimcaFtidRKPxmO.controllerType = iGjbSYkwNitJBFhHmfMYpqRDMOGGA;
								yxCkAhmKidpWoOimcaFtidRKPxmO.controllerId = XtOFWEFIvAjXbQCDllsDOxOpuuKk;
								YxCkAhmKidpWoOimcaFtidRKPxmO = yxCkAhmKidpWoOimcaFtidRKPxmO;
								FBVioGmeMKeHRSawwHoMFsmduvIG = 1;
								return true;
							}
							oVJUnkwJXnEtwMqyUVFrpNoBNDFh();
							cjeDTTiGTlAtxUYfRAgseLiDnIcDc = null;
							goto IL_015b;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void oVJUnkwJXnEtwMqyUVFrpNoBNDFh()
					{
						FBVioGmeMKeHRSawwHoMFsmduvIG = -1;
						if (cjeDTTiGTlAtxUYfRAgseLiDnIcDc != null)
						{
							cjeDTTiGTlAtxUYfRAgseLiDnIcDc.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						eNcbsNnBTGFEnRBcPHCKoVHkJJBj<_0001> eNcbsNnBTGFEnRBcPHCKoVHkJJBj2;
						if (FBVioGmeMKeHRSawwHoMFsmduvIG == -2 && FlAtIlHkkdOseFFxQdcjTINxqtRN == Environment.CurrentManagedThreadId)
						{
							FBVioGmeMKeHRSawwHoMFsmduvIG = 0;
							eNcbsNnBTGFEnRBcPHCKoVHkJJBj2 = this;
						}
						else
						{
							eNcbsNnBTGFEnRBcPHCKoVHkJJBj2 = new eNcbsNnBTGFEnRBcPHCKoVHkJJBj<_0001>(0);
							eNcbsNnBTGFEnRBcPHCKoVHkJJBj2.KkKonfOqGacAAVFSmFOAAvyhEBNEb = KkKonfOqGacAAVFSmFOAAvyhEBNEb;
						}
						eNcbsNnBTGFEnRBcPHCKoVHkJJBj2.iGjbSYkwNitJBFhHmfMYpqRDMOGGA = EzZpsMbcpCkajvDLAeaFtGZLCFbN;
						eNcbsNnBTGFEnRBcPHCKoVHkJJBj2.XtOFWEFIvAjXbQCDllsDOxOpuuKk = WIodLvGMnfnTAUdLeCYBdIWitlylB;
						eNcbsNnBTGFEnRBcPHCKoVHkJJBj2.rINdOshVOfKdVpXVlHCFOSlYhtbC = jpPRjCibEOOtODojLnBSwOsbFIcf;
						eNcbsNnBTGFEnRBcPHCKoVHkJJBj2.HIhreEzKDeABdWReQzydugysrLmv = xJaimwJZzuxDRrCrcJpSjbGmLIDb;
						eNcbsNnBTGFEnRBcPHCKoVHkJJBj2.qRMgmNKJJMeDZOfaLNGEstqQgifP = FYtTEUfKpIuzDgbvAofbbKquuaSx;
						eNcbsNnBTGFEnRBcPHCKoVHkJJBj2.oORHkNwkGwbNJLtzNiMqQiQyYVgu = wpwPAxoFImDEYlvZsJQbcayXgdMV;
						eNcbsNnBTGFEnRBcPHCKoVHkJJBj2.fMSOaPyBxqkZvuHlbEGphHCjRdPP = yvvLzbNnZSDVFoUbbjcWnkGGcOfr;
						return eNcbsNnBTGFEnRBcPHCKoVHkJJBj2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class NemQLvypQhFYwBgQHTUVNtjdBwplA<_0001> : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable where _0001 : ControllerMap
				{
					private int pbAtMrTXpmnzzVIyQqzyJDkjosBe;

					private ElementAssignmentConflictInfo EEWUbbJLFnUrKjgXJvtEIKDqaivg;

					private int OHkdsrLShOmATwPXXzyrAUZHoFyG;

					private global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> JRKYdjrRpSRVqTnIfXwcuCvbgbAs;

					public global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> oFVqfXAuQGieVuKlqjzNTiyWlsKL;

					private ElementAssignmentConflictCheck gLpDDuWzQfEIMwCOKDjkRWOYjMOeA;

					public ElementAssignmentConflictCheck ceGcwTfoYBvucLAOveyTVJPPpXpfA;

					private bool WtOThiRJjfRkXDhLDFHLKQHrbwTAb;

					public bool DLZWSEYRaSssVxNTUVzowLfpmdrA;

					private bool VaBkAhJrRyFqOafBRfgRWeyqXgKj;

					public bool tNcZiIgOEpGukzYCUUDONhqaxtBA;

					public ConflictCheckingHelper FutDbwQzlqSknJxczTXcGeqWAZnw;

					private InputMapCategory emMbUOeZrfPzXuXUZKnbZQdIAFNJ;

					private int qiiCkMcTNyGAPBAYkEzBDWWFfltxB;

					private IEnumerator<ElementAssignmentConflictInfo> tKfqKuvAXAkKAAXHcwGNGzgYCPLm;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return EEWUbbJLFnUrKjgXJvtEIKDqaivg;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return EEWUbbJLFnUrKjgXJvtEIKDqaivg;
						}
					}

					[DebuggerHidden]
					public NemQLvypQhFYwBgQHTUVNtjdBwplA(int P_0)
					{
						pbAtMrTXpmnzzVIyQqzyJDkjosBe = P_0;
						OHkdsrLShOmATwPXXzyrAUZHoFyG = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = pbAtMrTXpmnzzVIyQqzyJDkjosBe;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								ChCDzUKcPjibHKJLtDnpBFvugUxbA();
							}
						}
						emMbUOeZrfPzXuXUZKnbZQdIAFNJ = null;
						tKfqKuvAXAkKAAXHcwGNGzgYCPLm = null;
						pbAtMrTXpmnzzVIyQqzyJDkjosBe = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = pbAtMrTXpmnzzVIyQqzyJDkjosBe;
							ConflictCheckingHelper futDbwQzlqSknJxczTXcGeqWAZnw = FutDbwQzlqSknJxczTXcGeqWAZnw;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								pbAtMrTXpmnzzVIyQqzyJDkjosBe = -3;
								goto IL_01ab;
							}
							pbAtMrTXpmnzzVIyQqzyJDkjosBe = -1;
							if (JRKYdjrRpSRVqTnIfXwcuCvbgbAs == null)
							{
								return false;
							}
							Player player = ReInput.players.GetPlayer(gLpDDuWzQfEIMwCOKDjkRWOYjMOeA.playerId);
							if (player == null)
							{
								return false;
							}
							ControllerMap map = player.controllers.maps.GetMap(gLpDDuWzQfEIMwCOKDjkRWOYjMOeA.controllerType, gLpDDuWzQfEIMwCOKDjkRWOYjMOeA.controllerId, gLpDDuWzQfEIMwCOKDjkRWOYjMOeA.controllerMapId);
							emMbUOeZrfPzXuXUZKnbZQdIAFNJ = ((map != null) ? ReInput.mapping.GetMapCategory(map.categoryId) : ReInput.mapping.GetMapCategory(gLpDDuWzQfEIMwCOKDjkRWOYjMOeA.controllerMapCategoryId));
							if (emMbUOeZrfPzXuXUZKnbZQdIAFNJ == null)
							{
								return false;
							}
							qiiCkMcTNyGAPBAYkEzBDWWFfltxB = 0;
							goto IL_01d7;
							IL_01ab:
							if (tKfqKuvAXAkKAAXHcwGNGzgYCPLm.MoveNext())
							{
								ElementAssignmentConflictInfo current = tKfqKuvAXAkKAAXHcwGNGzgYCPLm.Current;
								ElementAssignmentConflictInfo eEWUbbJLFnUrKjgXJvtEIKDqaivg = new ElementAssignmentConflictInfo(current);
								eEWUbbJLFnUrKjgXJvtEIKDqaivg.playerId = futDbwQzlqSknJxczTXcGeqWAZnw.fjigivZpzAPHFCyeaiNCiGwNAbWf.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								eEWUbbJLFnUrKjgXJvtEIKDqaivg.controllerType = gLpDDuWzQfEIMwCOKDjkRWOYjMOeA.controllerType;
								eEWUbbJLFnUrKjgXJvtEIKDqaivg.controllerId = gLpDDuWzQfEIMwCOKDjkRWOYjMOeA.controllerId;
								EEWUbbJLFnUrKjgXJvtEIKDqaivg = eEWUbbJLFnUrKjgXJvtEIKDqaivg;
								pbAtMrTXpmnzzVIyQqzyJDkjosBe = 1;
								return true;
							}
							ChCDzUKcPjibHKJLtDnpBFvugUxbA();
							tKfqKuvAXAkKAAXHcwGNGzgYCPLm = null;
							goto IL_01c5;
							IL_01d7:
							if (qiiCkMcTNyGAPBAYkEzBDWWFfltxB < JRKYdjrRpSRVqTnIfXwcuCvbgbAs.GeYULXtFpYgsaXqXZIDwegpjOMgW())
							{
								ControllerMap controllerMap = JRKYdjrRpSRVqTnIfXwcuCvbgbAs.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(qiiCkMcTNyGAPBAYkEzBDWWFfltxB);
								if ((!WtOThiRJjfRkXDhLDFHLKQHrbwTAb || controllerMap.enabled) && (VaBkAhJrRyFqOafBRfgRWeyqXgKj || !futDbwQzlqSknJxczTXcGeqWAZnw.ZMSnrXwatOJKXFUOINkdsMMJbHqEA(emMbUOeZrfPzXuXUZKnbZQdIAFNJ, controllerMap)))
								{
									tKfqKuvAXAkKAAXHcwGNGzgYCPLm = controllerMap.ElementAssignmentConflicts(gLpDDuWzQfEIMwCOKDjkRWOYjMOeA, WtOThiRJjfRkXDhLDFHLKQHrbwTAb).GetEnumerator();
									pbAtMrTXpmnzzVIyQqzyJDkjosBe = -3;
									goto IL_01ab;
								}
								goto IL_01c5;
							}
							return false;
							IL_01c5:
							qiiCkMcTNyGAPBAYkEzBDWWFfltxB++;
							goto IL_01d7;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void ChCDzUKcPjibHKJLtDnpBFvugUxbA()
					{
						pbAtMrTXpmnzzVIyQqzyJDkjosBe = -1;
						if (tKfqKuvAXAkKAAXHcwGNGzgYCPLm != null)
						{
							tKfqKuvAXAkKAAXHcwGNGzgYCPLm.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						NemQLvypQhFYwBgQHTUVNtjdBwplA<_0001> nemQLvypQhFYwBgQHTUVNtjdBwplA;
						if (pbAtMrTXpmnzzVIyQqzyJDkjosBe == -2 && OHkdsrLShOmATwPXXzyrAUZHoFyG == Environment.CurrentManagedThreadId)
						{
							pbAtMrTXpmnzzVIyQqzyJDkjosBe = 0;
							nemQLvypQhFYwBgQHTUVNtjdBwplA = this;
						}
						else
						{
							nemQLvypQhFYwBgQHTUVNtjdBwplA = new NemQLvypQhFYwBgQHTUVNtjdBwplA<_0001>(0);
							nemQLvypQhFYwBgQHTUVNtjdBwplA.FutDbwQzlqSknJxczTXcGeqWAZnw = FutDbwQzlqSknJxczTXcGeqWAZnw;
						}
						nemQLvypQhFYwBgQHTUVNtjdBwplA.gLpDDuWzQfEIMwCOKDjkRWOYjMOeA = ceGcwTfoYBvucLAOveyTVJPPpXpfA;
						nemQLvypQhFYwBgQHTUVNtjdBwplA.WtOThiRJjfRkXDhLDFHLKQHrbwTAb = DLZWSEYRaSssVxNTUVzowLfpmdrA;
						nemQLvypQhFYwBgQHTUVNtjdBwplA.VaBkAhJrRyFqOafBRfgRWeyqXgKj = tNcZiIgOEpGukzYCUUDONhqaxtBA;
						nemQLvypQhFYwBgQHTUVNtjdBwplA.JRKYdjrRpSRVqTnIfXwcuCvbgbAs = oFVqfXAuQGieVuKlqjzNTiyWlsKL;
						return nemQLvypQhFYwBgQHTUVNtjdBwplA;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class MJBaQSYvTGbhuECmtagrXXxTClySA : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int CJgGPnjCsqLysEfSGIhJAmQLZFgN;

					private ElementAssignmentConflictInfo qIYWUfnhMcTEkInstdwptHsQshTc;

					private int EaUJoJjQfMIUGYbxnBYBVLKrNUGe;

					private int ucAipoSeCGADpAOQoTdhTcGLBwLLA;

					public int nqqfmgGvKzZNqquAOEHXnLrKwVVAA;

					private JoystickMap WyPWARyXecDteBELsHGyEEtgqjAv;

					public JoystickMap qLMGbZeGZKqmpLYcNtWRgvHVqgDi;

					public ConflictCheckingHelper HzMfMHhcWvwmirWnQqfFVaTSzJtl;

					private bool zebAmshFaOcsBtnGdOCiGpsgMkxJb;

					public bool RDiknBGmYeGldoSXhRKRAvTTtKfx;

					private bool eKIdnlJFZJgFtvfmraQGCNRKkRYd;

					public bool UpuAqRENXkPyejhAzuxNfBZUDydb;

					private int eHPplrjsCGUCejBBEZqcseMUgbsx;

					private IEnumerator<ElementAssignmentConflictInfo> hgPHrKCAvvgCKSKXrnGrymtgzaRgA;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return qIYWUfnhMcTEkInstdwptHsQshTc;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return qIYWUfnhMcTEkInstdwptHsQshTc;
						}
					}

					[DebuggerHidden]
					public MJBaQSYvTGbhuECmtagrXXxTClySA(int P_0)
					{
						CJgGPnjCsqLysEfSGIhJAmQLZFgN = P_0;
						EaUJoJjQfMIUGYbxnBYBVLKrNUGe = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int cJgGPnjCsqLysEfSGIhJAmQLZFgN = CJgGPnjCsqLysEfSGIhJAmQLZFgN;
						if (cJgGPnjCsqLysEfSGIhJAmQLZFgN == -3 || cJgGPnjCsqLysEfSGIhJAmQLZFgN == 1)
						{
							try
							{
							}
							finally
							{
								JZYHOxQDTyXKMEAbJHwsFQBHhofwA();
							}
						}
						hgPHrKCAvvgCKSKXrnGrymtgzaRgA = null;
						CJgGPnjCsqLysEfSGIhJAmQLZFgN = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int cJgGPnjCsqLysEfSGIhJAmQLZFgN = CJgGPnjCsqLysEfSGIhJAmQLZFgN;
							ConflictCheckingHelper hzMfMHhcWvwmirWnQqfFVaTSzJtl = HzMfMHhcWvwmirWnQqfFVaTSzJtl;
							if (cJgGPnjCsqLysEfSGIhJAmQLZFgN != 0)
							{
								if (cJgGPnjCsqLysEfSGIhJAmQLZFgN != 1)
								{
									return false;
								}
								CJgGPnjCsqLysEfSGIhJAmQLZFgN = -3;
								goto IL_00ea;
							}
							CJgGPnjCsqLysEfSGIhJAmQLZFgN = -1;
							if (ucAipoSeCGADpAOQoTdhTcGLBwLLA < 0 || WyPWARyXecDteBELsHGyEEtgqjAv == null)
							{
								return false;
							}
							eHPplrjsCGUCejBBEZqcseMUgbsx = 0;
							goto IL_0116;
							IL_00ea:
							if (hgPHrKCAvvgCKSKXrnGrymtgzaRgA.MoveNext())
							{
								ElementAssignmentConflictInfo current = hgPHrKCAvvgCKSKXrnGrymtgzaRgA.Current;
								qIYWUfnhMcTEkInstdwptHsQshTc = current;
								CJgGPnjCsqLysEfSGIhJAmQLZFgN = 1;
								return true;
							}
							JZYHOxQDTyXKMEAbJHwsFQBHhofwA();
							hgPHrKCAvvgCKSKXrnGrymtgzaRgA = null;
							goto IL_0104;
							IL_0116:
							if (eHPplrjsCGUCejBBEZqcseMUgbsx < hzMfMHhcWvwmirWnQqfFVaTSzJtl.YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA())
							{
								if (hzMfMHhcWvwmirWnQqfFVaTSzJtl.YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(eHPplrjsCGUCejBBEZqcseMUgbsx).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == ucAipoSeCGADpAOQoTdhTcGLBwLLA)
								{
									hgPHrKCAvvgCKSKXrnGrymtgzaRgA = hzMfMHhcWvwmirWnQqfFVaTSzJtl.nBsAqDGgxwAxzRoUafYXAbtBPhVXb(ControllerType.Joystick, ucAipoSeCGADpAOQoTdhTcGLBwLLA, WyPWARyXecDteBELsHGyEEtgqjAv, zebAmshFaOcsBtnGdOCiGpsgMkxJb, eKIdnlJFZJgFtvfmraQGCNRKkRYd, hzMfMHhcWvwmirWnQqfFVaTSzJtl.YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(eHPplrjsCGUCejBBEZqcseMUgbsx).GgWdqFddlhBZqeFBwCaEiHGCVWTqA).GetEnumerator();
									CJgGPnjCsqLysEfSGIhJAmQLZFgN = -3;
									goto IL_00ea;
								}
								goto IL_0104;
							}
							return false;
							IL_0104:
							eHPplrjsCGUCejBBEZqcseMUgbsx++;
							goto IL_0116;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void JZYHOxQDTyXKMEAbJHwsFQBHhofwA()
					{
						CJgGPnjCsqLysEfSGIhJAmQLZFgN = -1;
						if (hgPHrKCAvvgCKSKXrnGrymtgzaRgA != null)
						{
							hgPHrKCAvvgCKSKXrnGrymtgzaRgA.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						MJBaQSYvTGbhuECmtagrXXxTClySA mJBaQSYvTGbhuECmtagrXXxTClySA;
						if (CJgGPnjCsqLysEfSGIhJAmQLZFgN == -2 && EaUJoJjQfMIUGYbxnBYBVLKrNUGe == Environment.CurrentManagedThreadId)
						{
							CJgGPnjCsqLysEfSGIhJAmQLZFgN = 0;
							mJBaQSYvTGbhuECmtagrXXxTClySA = this;
						}
						else
						{
							mJBaQSYvTGbhuECmtagrXXxTClySA = new MJBaQSYvTGbhuECmtagrXXxTClySA(0);
							mJBaQSYvTGbhuECmtagrXXxTClySA.HzMfMHhcWvwmirWnQqfFVaTSzJtl = HzMfMHhcWvwmirWnQqfFVaTSzJtl;
						}
						mJBaQSYvTGbhuECmtagrXXxTClySA.ucAipoSeCGADpAOQoTdhTcGLBwLLA = nqqfmgGvKzZNqquAOEHXnLrKwVVAA;
						mJBaQSYvTGbhuECmtagrXXxTClySA.WyPWARyXecDteBELsHGyEEtgqjAv = qLMGbZeGZKqmpLYcNtWRgvHVqgDi;
						mJBaQSYvTGbhuECmtagrXXxTClySA.zebAmshFaOcsBtnGdOCiGpsgMkxJb = RDiknBGmYeGldoSXhRKRAvTTtKfx;
						mJBaQSYvTGbhuECmtagrXXxTClySA.eKIdnlJFZJgFtvfmraQGCNRKkRYd = UpuAqRENXkPyejhAzuxNfBZUDydb;
						return mJBaQSYvTGbhuECmtagrXXxTClySA;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class UorMzFDbyqvUNDMPgtQYMurXPYoo : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int kpMHWlPoFatjovMRYyfhbbXeojaF;

					private ElementAssignmentConflictInfo tmczvgykEBiLelpSVSFFoaYyCiBn;

					private int PXgLRmXGbUYuKnMCIGSNzSsLYcdE;

					private int rgpXeONcqgEFBdLOiSREFvIkFpdZ;

					public int TAaGvHdPMxaYMZXCjKIcWOtIZmct;

					private ActionElementMap FVptsTNzNNKAvILbUbWUChXzDduGA;

					public ActionElementMap xexZGzZpVFXYgkdNJVSpaMPIaBcaA;

					public ConflictCheckingHelper iDZldIPjbVrDcOATlGqYJiBOLQcDb;

					private JoystickMap RzoeVeavuapggwiKGqvXrHwnWakqA;

					public JoystickMap nZDHJdhhZmXBtMCCayCoBmuxKNod;

					private bool OzZwDawcWZVgTtXmtQqdrcKfWcEw;

					public bool bcSDMKgatftGmHWyunfXffzOfQmE;

					private bool eSCCuADRgcLZjLeVfekCEsLxaSlIA;

					public bool uyireGBduMfjOozwKtwoVZhWhZKd;

					private int PlVQzGFIIbodczGzafsFiHYHCexu;

					private IEnumerator<ElementAssignmentConflictInfo> TJSFXungeSEiUvkAWdhCvdYdAxnC;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return tmczvgykEBiLelpSVSFFoaYyCiBn;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return tmczvgykEBiLelpSVSFFoaYyCiBn;
						}
					}

					[DebuggerHidden]
					public UorMzFDbyqvUNDMPgtQYMurXPYoo(int P_0)
					{
						kpMHWlPoFatjovMRYyfhbbXeojaF = P_0;
						PXgLRmXGbUYuKnMCIGSNzSsLYcdE = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = kpMHWlPoFatjovMRYyfhbbXeojaF;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								VdKeyoYsJOKGZmkTsMFDoQuiSijc();
							}
						}
						TJSFXungeSEiUvkAWdhCvdYdAxnC = null;
						kpMHWlPoFatjovMRYyfhbbXeojaF = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = kpMHWlPoFatjovMRYyfhbbXeojaF;
							ConflictCheckingHelper conflictCheckingHelper = iDZldIPjbVrDcOATlGqYJiBOLQcDb;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								kpMHWlPoFatjovMRYyfhbbXeojaF = -3;
								goto IL_00f0;
							}
							kpMHWlPoFatjovMRYyfhbbXeojaF = -1;
							if (rgpXeONcqgEFBdLOiSREFvIkFpdZ < 0 || FVptsTNzNNKAvILbUbWUChXzDduGA == null)
							{
								return false;
							}
							PlVQzGFIIbodczGzafsFiHYHCexu = 0;
							goto IL_011c;
							IL_00f0:
							if (TJSFXungeSEiUvkAWdhCvdYdAxnC.MoveNext())
							{
								ElementAssignmentConflictInfo current = TJSFXungeSEiUvkAWdhCvdYdAxnC.Current;
								tmczvgykEBiLelpSVSFFoaYyCiBn = current;
								kpMHWlPoFatjovMRYyfhbbXeojaF = 1;
								return true;
							}
							VdKeyoYsJOKGZmkTsMFDoQuiSijc();
							TJSFXungeSEiUvkAWdhCvdYdAxnC = null;
							goto IL_010a;
							IL_011c:
							if (PlVQzGFIIbodczGzafsFiHYHCexu < conflictCheckingHelper.YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA())
							{
								if (conflictCheckingHelper.YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(PlVQzGFIIbodczGzafsFiHYHCexu).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == rgpXeONcqgEFBdLOiSREFvIkFpdZ)
								{
									TJSFXungeSEiUvkAWdhCvdYdAxnC = conflictCheckingHelper.VWzCqXLGGBbVBDfGyrTEnCVZcAZfA(ControllerType.Joystick, rgpXeONcqgEFBdLOiSREFvIkFpdZ, RzoeVeavuapggwiKGqvXrHwnWakqA, FVptsTNzNNKAvILbUbWUChXzDduGA, OzZwDawcWZVgTtXmtQqdrcKfWcEw, eSCCuADRgcLZjLeVfekCEsLxaSlIA, conflictCheckingHelper.YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(PlVQzGFIIbodczGzafsFiHYHCexu).GgWdqFddlhBZqeFBwCaEiHGCVWTqA).GetEnumerator();
									kpMHWlPoFatjovMRYyfhbbXeojaF = -3;
									goto IL_00f0;
								}
								goto IL_010a;
							}
							return false;
							IL_010a:
							PlVQzGFIIbodczGzafsFiHYHCexu++;
							goto IL_011c;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void VdKeyoYsJOKGZmkTsMFDoQuiSijc()
					{
						kpMHWlPoFatjovMRYyfhbbXeojaF = -1;
						if (TJSFXungeSEiUvkAWdhCvdYdAxnC != null)
						{
							TJSFXungeSEiUvkAWdhCvdYdAxnC.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						UorMzFDbyqvUNDMPgtQYMurXPYoo uorMzFDbyqvUNDMPgtQYMurXPYoo;
						if (kpMHWlPoFatjovMRYyfhbbXeojaF == -2 && PXgLRmXGbUYuKnMCIGSNzSsLYcdE == Environment.CurrentManagedThreadId)
						{
							kpMHWlPoFatjovMRYyfhbbXeojaF = 0;
							uorMzFDbyqvUNDMPgtQYMurXPYoo = this;
						}
						else
						{
							uorMzFDbyqvUNDMPgtQYMurXPYoo = new UorMzFDbyqvUNDMPgtQYMurXPYoo(0);
							uorMzFDbyqvUNDMPgtQYMurXPYoo.iDZldIPjbVrDcOATlGqYJiBOLQcDb = iDZldIPjbVrDcOATlGqYJiBOLQcDb;
						}
						uorMzFDbyqvUNDMPgtQYMurXPYoo.rgpXeONcqgEFBdLOiSREFvIkFpdZ = TAaGvHdPMxaYMZXCjKIcWOtIZmct;
						uorMzFDbyqvUNDMPgtQYMurXPYoo.RzoeVeavuapggwiKGqvXrHwnWakqA = nZDHJdhhZmXBtMCCayCoBmuxKNod;
						uorMzFDbyqvUNDMPgtQYMurXPYoo.FVptsTNzNNKAvILbUbWUChXzDduGA = xexZGzZpVFXYgkdNJVSpaMPIaBcaA;
						uorMzFDbyqvUNDMPgtQYMurXPYoo.OzZwDawcWZVgTtXmtQqdrcKfWcEw = bcSDMKgatftGmHWyunfXffzOfQmE;
						uorMzFDbyqvUNDMPgtQYMurXPYoo.eSCCuADRgcLZjLeVfekCEsLxaSlIA = uyireGBduMfjOozwKtwoVZhWhZKd;
						return uorMzFDbyqvUNDMPgtQYMurXPYoo;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private sealed class XTXFWDPtEhvQKKTibORnCxwRrOkr : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
				{
					private int lEVjCveoOMeTKvTrUYVSpmcsOuyL;

					private ElementAssignmentConflictInfo sxlcHNloOLNroMxFevzvgOBAcBpK;

					private int VOLvRjalOwUWIFjSBKfQXhDIdJrjA;

					private ElementAssignmentConflictCheck WpffKsgeMSqXgCCpuFMzQtGkblHUA;

					public ElementAssignmentConflictCheck CyqXzCnJfZTHlBKEwZsjdJyDdgOI;

					public ConflictCheckingHelper HDWlxiZtEAOIqiVTPJJCHKUcmQKX;

					private bool kAJLTzjEDUqUUHCGpxhkcOKGrpZJ;

					public bool BEmzDvaAWgKvSmYQPqMXdNhfjWXDA;

					private bool VfYNplCPDBFxtOCHGAqhaSxSmhoe;

					public bool CVPoKPACwUcOLtDYIzYYXTVhHEEj;

					private int PcROcGFBjFdZLiFCCygnSihmcKhLA;

					private IEnumerator<ElementAssignmentConflictInfo> AERybscEpOVQPGuDnjyspHtYGPcp;

					ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return sxlcHNloOLNroMxFevzvgOBAcBpK;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return sxlcHNloOLNroMxFevzvgOBAcBpK;
						}
					}

					[DebuggerHidden]
					public XTXFWDPtEhvQKKTibORnCxwRrOkr(int P_0)
					{
						lEVjCveoOMeTKvTrUYVSpmcsOuyL = P_0;
						VOLvRjalOwUWIFjSBKfQXhDIdJrjA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = lEVjCveoOMeTKvTrUYVSpmcsOuyL;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								nESAuWHWrnlIciOdKzibWeXXLnuiA();
							}
						}
						AERybscEpOVQPGuDnjyspHtYGPcp = null;
						lEVjCveoOMeTKvTrUYVSpmcsOuyL = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = lEVjCveoOMeTKvTrUYVSpmcsOuyL;
							ConflictCheckingHelper hDWlxiZtEAOIqiVTPJJCHKUcmQKX = HDWlxiZtEAOIqiVTPJJCHKUcmQKX;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								lEVjCveoOMeTKvTrUYVSpmcsOuyL = -3;
								goto IL_00f3;
							}
							lEVjCveoOMeTKvTrUYVSpmcsOuyL = -1;
							if (WpffKsgeMSqXgCCpuFMzQtGkblHUA.controllerId < 0 || WpffKsgeMSqXgCCpuFMzQtGkblHUA.elementAssignmentType == ElementAssignmentType.KeyboardKey)
							{
								return false;
							}
							PcROcGFBjFdZLiFCCygnSihmcKhLA = 0;
							goto IL_011f;
							IL_00f3:
							if (AERybscEpOVQPGuDnjyspHtYGPcp.MoveNext())
							{
								ElementAssignmentConflictInfo current = AERybscEpOVQPGuDnjyspHtYGPcp.Current;
								sxlcHNloOLNroMxFevzvgOBAcBpK = current;
								lEVjCveoOMeTKvTrUYVSpmcsOuyL = 1;
								return true;
							}
							nESAuWHWrnlIciOdKzibWeXXLnuiA();
							AERybscEpOVQPGuDnjyspHtYGPcp = null;
							goto IL_010d;
							IL_011f:
							if (PcROcGFBjFdZLiFCCygnSihmcKhLA < hDWlxiZtEAOIqiVTPJJCHKUcmQKX.YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA())
							{
								if (hDWlxiZtEAOIqiVTPJJCHKUcmQKX.YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(PcROcGFBjFdZLiFCCygnSihmcKhLA).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == WpffKsgeMSqXgCCpuFMzQtGkblHUA.controllerId)
								{
									AERybscEpOVQPGuDnjyspHtYGPcp = hDWlxiZtEAOIqiVTPJJCHKUcmQKX.yPHbsWdMNegyAelNKdHhyecMMPrcB(WpffKsgeMSqXgCCpuFMzQtGkblHUA, kAJLTzjEDUqUUHCGpxhkcOKGrpZJ, VfYNplCPDBFxtOCHGAqhaSxSmhoe, hDWlxiZtEAOIqiVTPJJCHKUcmQKX.YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(PcROcGFBjFdZLiFCCygnSihmcKhLA).GgWdqFddlhBZqeFBwCaEiHGCVWTqA).GetEnumerator();
									lEVjCveoOMeTKvTrUYVSpmcsOuyL = -3;
									goto IL_00f3;
								}
								goto IL_010d;
							}
							return false;
							IL_010d:
							PcROcGFBjFdZLiFCCygnSihmcKhLA++;
							goto IL_011f;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void nESAuWHWrnlIciOdKzibWeXXLnuiA()
					{
						lEVjCveoOMeTKvTrUYVSpmcsOuyL = -1;
						if (AERybscEpOVQPGuDnjyspHtYGPcp != null)
						{
							AERybscEpOVQPGuDnjyspHtYGPcp.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
					{
						XTXFWDPtEhvQKKTibORnCxwRrOkr xTXFWDPtEhvQKKTibORnCxwRrOkr;
						if (lEVjCveoOMeTKvTrUYVSpmcsOuyL == -2 && VOLvRjalOwUWIFjSBKfQXhDIdJrjA == Environment.CurrentManagedThreadId)
						{
							lEVjCveoOMeTKvTrUYVSpmcsOuyL = 0;
							xTXFWDPtEhvQKKTibORnCxwRrOkr = this;
						}
						else
						{
							xTXFWDPtEhvQKKTibORnCxwRrOkr = new XTXFWDPtEhvQKKTibORnCxwRrOkr(0);
							xTXFWDPtEhvQKKTibORnCxwRrOkr.HDWlxiZtEAOIqiVTPJJCHKUcmQKX = HDWlxiZtEAOIqiVTPJJCHKUcmQKX;
						}
						xTXFWDPtEhvQKKTibORnCxwRrOkr.WpffKsgeMSqXgCCpuFMzQtGkblHUA = CyqXzCnJfZTHlBKEwZsjdJyDdgOI;
						xTXFWDPtEhvQKKTibORnCxwRrOkr.kAJLTzjEDUqUUHCGpxhkcOKGrpZJ = BEmzDvaAWgKvSmYQPqMXdNhfjWXDA;
						xTXFWDPtEhvQKKTibORnCxwRrOkr.VfYNplCPDBFxtOCHGAqhaSxSmhoe = CVPoKPACwUcOLtDYIzYYXTVhHEEj;
						return xTXFWDPtEhvQKKTibORnCxwRrOkr;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
					}
				}

				private readonly Player fjigivZpzAPHFCyeaiNCiGwNAbWf;

				private readonly ControllerHelper YGgiLrmkKvnCQihxbvaZObfoyApM;

				private readonly int rYAOvivyJUiuYdOHWnGFizplkvnR;

				internal ConflictCheckingHelper(Player P_0, ControllerHelper P_1)
				{
					rYAOvivyJUiuYdOHWnGFizplkvnR = ReInput.id;
					fjigivZpzAPHFCyeaiNCiGwNAbWf = P_0;
					YGgiLrmkKvnCQihxbvaZObfoyApM = P_1;
				}

				public bool DoesElementAssignmentConflict(ControllerType controllerType, int controllerId, ControllerMap controllerMap)
				{
					return DoesElementAssignmentConflict(controllerType, controllerId, controllerMap, skipDisabledMaps: false, forceCheckAllCategories: false);
				}

				public bool DoesElementAssignmentConflict(ControllerType controllerType, int controllerId, ControllerMap controllerMap, bool skipDisabledMaps)
				{
					return DoesElementAssignmentConflict(controllerType, controllerId, controllerMap, skipDisabledMaps, forceCheckAllCategories: false);
				}

				public bool DoesElementAssignmentConflict(ControllerType controllerType, int controllerId, ControllerMap controllerMap, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return false;
					}
					if (controllerMap == null)
					{
						return false;
					}
					return controllerType switch
					{
						ControllerType.Joystick => kMCEwHfNVJjQFZKpNouacVmBRtYrB(controllerId, controllerMap as JoystickMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Keyboard => GMQuidVXxvieCSqBslQkdsXJcxuV(controllerMap as KeyboardMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Mouse => QovZinlMwcEwfwwUDdWGagsPtrok(controllerMap as MouseMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Custom => lkivVSHvVqJMkACpSebgahdCIiQD(controllerId, controllerMap as CustomControllerMap, skipDisabledMaps, forceCheckAllCategories), 
						_ => throw new NotImplementedException(), 
					};
				}

				public bool DoesElementAssignmentConflict(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap)
				{
					return DoesElementAssignmentConflict(controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps: false, forceCheckAllCategories: false);
				}

				public bool DoesElementAssignmentConflict(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps)
				{
					return DoesElementAssignmentConflict(controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories: false);
				}

				public bool DoesElementAssignmentConflict(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return false;
					}
					if (controllerMap == null || elementMap == null)
					{
						return false;
					}
					return controllerType switch
					{
						ControllerType.Joystick => leRldIMWpBLUxSQhRrLoJZXfyJjC(controllerId, controllerMap as JoystickMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Keyboard => sZVlAJLgpsQauKICYjoDcIeyudBaA(controllerMap as KeyboardMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Mouse => HqgVNMpXBKUWUPKatLXzqKDzNKkX(controllerMap as MouseMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Custom => NalVIWPORyGyJbeiGWvQJPwohFfz(controllerId, controllerMap as CustomControllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						_ => throw new NotImplementedException(), 
					};
				}

				public bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck)
				{
					return DoesElementAssignmentConflict(conflictCheck, skipDisabledMaps: false, forceCheckAllCategories: false);
				}

				public bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
				{
					return DoesElementAssignmentConflict(conflictCheck, skipDisabledMaps, forceCheckAllCategories: false);
				}

				public bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return false;
					}
					if (conflictCheck.controllerType == ControllerType.Joystick)
					{
						return ULEoptGcZPgwDieidgUGMKPefRNhA(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return emPffmlSPqdDnbTBRlGtAoolURyCb(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return OnIIiMOZOCjmUQihtKaawgnJOpY(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return yuRgSVDtnqdPCqoXSocDZCqKtqWQA(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					throw new NotImplementedException();
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap)
				{
					return ElementAssignmentConflicts(controllerType, controllerId, controllerMap, skipDisabledMaps: false, forceCheckAllCategories: false);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, bool skipDisabledMaps)
				{
					return ElementAssignmentConflicts(controllerType, controllerId, controllerMap, skipDisabledMaps, forceCheckAllCategories: false);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return EmptyObjects<ElementAssignmentConflictInfo>.EmptyReadOnlyIListT;
					}
					if (controllerMap == null)
					{
						return new List<ElementAssignmentConflictInfo>();
					}
					return controllerType switch
					{
						ControllerType.Joystick => vyTnRzMwqYmfHHeiOOOtHoWhmrDr(controllerId, controllerMap as JoystickMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Keyboard => dJrEcrzTOpbYFdhsAQrCbDMKqDeib(controllerMap as KeyboardMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Mouse => xOmOsIlFGGANDRFBQtNhUVJVwzit(controllerMap as MouseMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Custom => yvacKkQjJFRBEXKVxPOJzHtHucKp(controllerId, controllerMap as CustomControllerMap, skipDisabledMaps, forceCheckAllCategories), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap)
				{
					return ElementAssignmentConflicts(controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps: false, forceCheckAllCategories: false);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps)
				{
					return ElementAssignmentConflicts(controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories: false);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return EmptyObjects<ElementAssignmentConflictInfo>.EmptyReadOnlyIListT;
					}
					if (controllerMap == null || elementMap == null)
					{
						return new List<ElementAssignmentConflictInfo>();
					}
					return controllerType switch
					{
						ControllerType.Joystick => IFYDdUfvQXgFkTAzQBxpeMBXFebbA(controllerId, controllerMap as JoystickMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Keyboard => xlINAzrepBNKnPJCKzqxTBhDFwrm(controllerMap as KeyboardMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Mouse => UJjcYBzojKdMbtKeXdJOQpNUSctM(controllerMap as MouseMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Custom => FUFqHTvCYMNHTXXXWmRretxhHLEK(controllerId, controllerMap as CustomControllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck)
				{
					return ElementAssignmentConflicts(conflictCheck, skipDisabledMaps: false, forceCheckAllCategories: false);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
				{
					return ElementAssignmentConflicts(conflictCheck, skipDisabledMaps, forceCheckAllCategories: false);
				}

				public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return EmptyObjects<ElementAssignmentConflictInfo>.EmptyReadOnlyIListT;
					}
					if (conflictCheck.controllerType == ControllerType.Joystick)
					{
						return NtFTxbTReXPllFERTgMJMRtkjjbk(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return JhszRScfQtGlSKDeWEkDkwDXnBKl(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return kAsjcAQosNPEopwSNnrUnpkQRUdE(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return AZBedMJlTnzusafJSIWNmNHBdYaeA(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					throw new NotImplementedException();
				}

				public int RemoveElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap)
				{
					return RemoveElementAssignmentConflicts(controllerType, controllerId, controllerMap, skipRemovedMaps: false, forceCheckAllCategories: false);
				}

				public int RemoveElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, bool skipRemovedMaps)
				{
					return RemoveElementAssignmentConflicts(controllerType, controllerId, controllerMap, skipRemovedMaps, forceCheckAllCategories: false);
				}

				public int RemoveElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, bool skipRemovedMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return 0;
					}
					if (controllerMap == null)
					{
						return 0;
					}
					return controllerType switch
					{
						ControllerType.Joystick => YslApImfxKGvimZCqrZPtTwXxFti(controllerId, controllerMap as JoystickMap, skipRemovedMaps, forceCheckAllCategories), 
						ControllerType.Keyboard => EQACapKhXQbJNKncCLyuyiTuLFAnA(controllerMap as KeyboardMap, skipRemovedMaps, forceCheckAllCategories), 
						ControllerType.Mouse => EYPpTgGeodCCxzzeYZvOWaABITdA(controllerMap as MouseMap, skipRemovedMaps, forceCheckAllCategories), 
						ControllerType.Custom => tDCaLfGaFIsqlcPvIbyxoYTAQCHFA(controllerId, controllerMap as CustomControllerMap, skipRemovedMaps, forceCheckAllCategories), 
						_ => throw new NotImplementedException(), 
					};
				}

				public int RemoveElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap)
				{
					return RemoveElementAssignmentConflicts(controllerType, controllerId, controllerMap, elementMap, skipRemovedMaps: false, forceCheckAllCategories: false);
				}

				public int RemoveElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipRemovedMaps)
				{
					return RemoveElementAssignmentConflicts(controllerType, controllerId, controllerMap, elementMap, skipRemovedMaps, forceCheckAllCategories: false);
				}

				public int RemoveElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipRemovedMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return 0;
					}
					if (controllerMap == null || elementMap == null)
					{
						return 0;
					}
					return controllerType switch
					{
						ControllerType.Joystick => OxGvfxDZKBrHgPXDgNVeYMIeoIaA(controllerId, controllerMap as JoystickMap, elementMap, skipRemovedMaps, forceCheckAllCategories), 
						ControllerType.Keyboard => zKbofRSzJmfycNHsvhHMeEfXxAjf(controllerMap as KeyboardMap, elementMap, skipRemovedMaps, forceCheckAllCategories), 
						ControllerType.Mouse => rwTcIPgNdynYJpmauOGUrgngYMid(controllerMap as MouseMap, elementMap, skipRemovedMaps, forceCheckAllCategories), 
						ControllerType.Custom => EBpdVGkxgMqOtBLJeAGZpfOdZABQ(controllerId, controllerMap as CustomControllerMap, elementMap, skipRemovedMaps, forceCheckAllCategories), 
						_ => throw new NotImplementedException(), 
					};
				}

				public int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck)
				{
					return RemoveElementAssignmentConflicts(conflictCheck, skipRemovedMaps: false, forceCheckAllCategories: false);
				}

				public int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipRemovedMaps)
				{
					return RemoveElementAssignmentConflicts(conflictCheck, skipRemovedMaps, forceCheckAllCategories: false);
				}

				public int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipRemovedMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return 0;
					}
					if (conflictCheck.controllerType == ControllerType.Joystick)
					{
						return dqwAGWYayZBluQbgyosZqBWoIgee(conflictCheck, skipRemovedMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return nWAndEWKazfBllukVxUggzZPfaPo(conflictCheck, skipRemovedMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return qoLLQthXTHryAPhDJDaynKbPjbEj(conflictCheck, skipRemovedMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return AFPzhsOsAPeXZgZVrCxKydLbTaUh(conflictCheck, skipRemovedMaps, forceCheckAllCategories);
					}
					throw new NotImplementedException();
				}

				public int DisableElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap)
				{
					return DisableElementAssignmentConflicts(controllerType, controllerId, controllerMap, skipDisabledMaps: false, forceCheckAllCategories: false);
				}

				public int DisableElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, bool skipDisabledMaps)
				{
					return DisableElementAssignmentConflicts(controllerType, controllerId, controllerMap, skipDisabledMaps, forceCheckAllCategories: false);
				}

				public int DisableElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return 0;
					}
					if (controllerMap == null)
					{
						return 0;
					}
					return controllerType switch
					{
						ControllerType.Joystick => cQNTHsvMGLemHbuXbEYWEJufRxdnc(controllerId, controllerMap as JoystickMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Keyboard => muxHjeAuPZfQlQtSDwANChPxmCTG(controllerMap as KeyboardMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Mouse => jZaAFXHvQIDikPvuoATOHCURzjqiA(controllerMap as MouseMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Custom => WcyQbIaVqDmFkleIgffoaAjTlpgvA(controllerId, controllerMap as CustomControllerMap, skipDisabledMaps, forceCheckAllCategories), 
						_ => throw new NotImplementedException(), 
					};
				}

				public int DisableElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap)
				{
					return DisableElementAssignmentConflicts(controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps: false, forceCheckAllCategories: false);
				}

				public int DisableElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps)
				{
					return DisableElementAssignmentConflicts(controllerType, controllerId, controllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories: false);
				}

				public int DisableElementAssignmentConflicts(ControllerType controllerType, int controllerId, ControllerMap controllerMap, ActionElementMap elementMap, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return 0;
					}
					if (controllerMap == null || elementMap == null)
					{
						return 0;
					}
					return controllerType switch
					{
						ControllerType.Joystick => ebsGUymBTpVYNOVKoBMBKdyQhgtQ(controllerId, controllerMap as JoystickMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Keyboard => hgERfktSEemIPqEdapZDdgGGEygt(controllerMap as KeyboardMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Mouse => HVyTPnRNRTRdOEANFmeTEyWvjUcu(controllerMap as MouseMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						ControllerType.Custom => bFUJeNFxDwEZDxYxubVpquzoHQHL(controllerId, controllerMap as CustomControllerMap, elementMap, skipDisabledMaps, forceCheckAllCategories), 
						_ => throw new NotImplementedException(), 
					};
				}

				public int DisableElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck)
				{
					return DisableElementAssignmentConflicts(conflictCheck, skipDisabledMaps: false, forceCheckAllCategories: false);
				}

				public int DisableElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
				{
					return DisableElementAssignmentConflicts(conflictCheck, skipDisabledMaps, forceCheckAllCategories: false);
				}

				public int DisableElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps, bool forceCheckAllCategories)
				{
					if (ReInput._id != rYAOvivyJUiuYdOHWnGFizplkvnR)
					{
						ReInput.CheckInitialized(rYAOvivyJUiuYdOHWnGFizplkvnR);
						return 0;
					}
					if (conflictCheck.controllerType == ControllerType.Joystick)
					{
						return CMyNaAbDlLQLxuZIiBtMypTkmrzl(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Keyboard)
					{
						return xJUkugJIjNiQaadRfcOtrJBPKUmNA(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Mouse)
					{
						return XJJseptvcEqwrZpmZlfrQmvvWAEE(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					if (conflictCheck.controllerType == ControllerType.Custom)
					{
						return FcYiXiBfPfcEHQKtjMlJqKvetzsJ(conflictCheck, skipDisabledMaps, forceCheckAllCategories);
					}
					throw new NotImplementedException();
				}

				private bool kMCEwHfNVJjQFZKpNouacVmBRtYrB(int P_0, JoystickMap P_1, bool P_2 = false, bool P_3 = false)
				{
					if (P_0 < 0 || P_1 == null)
					{
						return false;
					}
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0 && AGBcjxXbdKpmMEoMoUsQipCkCrFKA(ControllerType.Joystick, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA))
						{
							return true;
						}
					}
					return false;
				}

				private bool leRldIMWpBLUxSQhRrLoJZXfyJjC(int P_0, JoystickMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return false;
					}
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0 && KKGAQlibiwiNzcNKGTvnMshxkFgnB(ControllerType.Joystick, P_0, P_1, P_2, P_3, P_4, YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA))
						{
							return true;
						}
					}
					return false;
				}

				private bool ULEoptGcZPgwDieidgUGMKPefRNhA(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					if (P_0.controllerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0.controllerId && haoRagDWpeniondPeGOlXoiVHjcJA(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA))
						{
							return true;
						}
					}
					return false;
				}

				private bool GMQuidVXxvieCSqBslQkdsXJcxuV(KeyboardMap P_0, bool P_1 = false, bool P_2 = false)
				{
					return AGBcjxXbdKpmMEoMoUsQipCkCrFKA(ControllerType.Keyboard, 0, P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA);
				}

				private bool sZVlAJLgpsQauKICYjoDcIeyudBaA(KeyboardMap P_0, ActionElementMap P_1, bool P_2 = false, bool P_3 = false)
				{
					return KKGAQlibiwiNzcNKGTvnMshxkFgnB(ControllerType.Keyboard, 0, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA);
				}

				private bool emPffmlSPqdDnbTBRlGtAoolURyCb(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					if (P_0.elementAssignmentType != ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					return haoRagDWpeniondPeGOlXoiVHjcJA(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA);
				}

				private bool QovZinlMwcEwfwwUDdWGagsPtrok(MouseMap P_0, bool P_1 = false, bool P_2 = false)
				{
					return AGBcjxXbdKpmMEoMoUsQipCkCrFKA(ControllerType.Mouse, 0, P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA);
				}

				private bool HqgVNMpXBKUWUPKatLXzqKDzNKkX(MouseMap P_0, ActionElementMap P_1, bool P_2 = false, bool P_3 = false)
				{
					return KKGAQlibiwiNzcNKGTvnMshxkFgnB(ControllerType.Mouse, 0, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA);
				}

				private bool OnIIiMOZOCjmUQihtKaawgnJOpY(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					if (P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					return haoRagDWpeniondPeGOlXoiVHjcJA(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA);
				}

				private bool lkivVSHvVqJMkACpSebgahdCIiQD(int P_0, CustomControllerMap P_1, bool P_2 = false, bool P_3 = false)
				{
					if (P_0 < 0 || P_1 == null)
					{
						return false;
					}
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0 && AGBcjxXbdKpmMEoMoUsQipCkCrFKA(ControllerType.Custom, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA))
						{
							return true;
						}
					}
					return false;
				}

				private bool NalVIWPORyGyJbeiGWvQJPwohFfz(int P_0, CustomControllerMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return false;
					}
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0 && KKGAQlibiwiNzcNKGTvnMshxkFgnB(ControllerType.Custom, P_0, P_1, P_2, P_3, P_4, YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA))
						{
							return true;
						}
					}
					return false;
				}

				private bool yuRgSVDtnqdPCqoXSocDZCqKtqWQA(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					if (P_0.controllerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return false;
					}
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0.controllerId && haoRagDWpeniondPeGOlXoiVHjcJA(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA))
						{
							return true;
						}
					}
					return false;
				}

				[IteratorStateMachine(typeof(MJBaQSYvTGbhuECmtagrXXxTClySA))]
				private IEnumerable<ElementAssignmentConflictInfo> vyTnRzMwqYmfHHeiOOOtHoWhmrDr(int P_0, JoystickMap P_1, bool P_2 = false, bool P_3 = false)
				{
					return new MJBaQSYvTGbhuECmtagrXXxTClySA(-2)
					{
						HzMfMHhcWvwmirWnQqfFVaTSzJtl = this,
						nqqfmgGvKzZNqquAOEHXnLrKwVVAA = P_0,
						qLMGbZeGZKqmpLYcNtWRgvHVqgDi = P_1,
						RDiknBGmYeGldoSXhRKRAvTTtKfx = P_2,
						UpuAqRENXkPyejhAzuxNfBZUDydb = P_3
					};
				}

				[IteratorStateMachine(typeof(UorMzFDbyqvUNDMPgtQYMurXPYoo))]
				private IEnumerable<ElementAssignmentConflictInfo> IFYDdUfvQXgFkTAzQBxpeMBXFebbA(int P_0, JoystickMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false)
				{
					return new UorMzFDbyqvUNDMPgtQYMurXPYoo(-2)
					{
						iDZldIPjbVrDcOATlGqYJiBOLQcDb = this,
						TAaGvHdPMxaYMZXCjKIcWOtIZmct = P_0,
						nZDHJdhhZmXBtMCCayCoBmuxKNod = P_1,
						xexZGzZpVFXYgkdNJVSpaMPIaBcaA = P_2,
						bcSDMKgatftGmHWyunfXffzOfQmE = P_3,
						uyireGBduMfjOozwKtwoVZhWhZKd = P_4
					};
				}

				[IteratorStateMachine(typeof(XTXFWDPtEhvQKKTibORnCxwRrOkr))]
				private IEnumerable<ElementAssignmentConflictInfo> NtFTxbTReXPllFERTgMJMRtkjjbk(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					return new XTXFWDPtEhvQKKTibORnCxwRrOkr(-2)
					{
						HDWlxiZtEAOIqiVTPJJCHKUcmQKX = this,
						CyqXzCnJfZTHlBKEwZsjdJyDdgOI = P_0,
						BEmzDvaAWgKvSmYQPqMXdNhfjWXDA = P_1,
						CVPoKPACwUcOLtDYIzYYXTVhHEEj = P_2
					};
				}

				private IEnumerable<ElementAssignmentConflictInfo> dJrEcrzTOpbYFdhsAQrCbDMKqDeib(KeyboardMap P_0, bool P_1 = false, bool P_2 = false)
				{
					return nBsAqDGgxwAxzRoUafYXAbtBPhVXb(ControllerType.Keyboard, 0, P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA);
				}

				private IEnumerable<ElementAssignmentConflictInfo> xlINAzrepBNKnPJCKzqxTBhDFwrm(KeyboardMap P_0, ActionElementMap P_1, bool P_2 = false, bool P_3 = false)
				{
					return VWzCqXLGGBbVBDfGyrTEnCVZcAZfA(ControllerType.Keyboard, 0, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA);
				}

				private IEnumerable<ElementAssignmentConflictInfo> JhszRScfQtGlSKDeWEkDkwDXnBKl(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					if (P_0.elementAssignmentType != ElementAssignmentType.KeyboardKey)
					{
						return new List<ElementAssignmentConflictInfo>();
					}
					return yPHbsWdMNegyAelNKdHhyecMMPrcB(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA);
				}

				private IEnumerable<ElementAssignmentConflictInfo> xOmOsIlFGGANDRFBQtNhUVJVwzit(MouseMap P_0, bool P_1 = false, bool P_2 = false)
				{
					return nBsAqDGgxwAxzRoUafYXAbtBPhVXb(ControllerType.Mouse, 0, P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA);
				}

				private IEnumerable<ElementAssignmentConflictInfo> UJjcYBzojKdMbtKeXdJOQpNUSctM(MouseMap P_0, ActionElementMap P_1, bool P_2 = false, bool P_3 = false)
				{
					return VWzCqXLGGBbVBDfGyrTEnCVZcAZfA(ControllerType.Mouse, 0, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA);
				}

				private IEnumerable<ElementAssignmentConflictInfo> kAsjcAQosNPEopwSNnrUnpkQRUdE(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					if (P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return new List<ElementAssignmentConflictInfo>();
					}
					return yPHbsWdMNegyAelNKdHhyecMMPrcB(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA);
				}

				[IteratorStateMachine(typeof(sFUjBWoRWvBCcJjkbyFuAQIZnYFgA))]
				private IEnumerable<ElementAssignmentConflictInfo> yvacKkQjJFRBEXKVxPOJzHtHucKp(int P_0, CustomControllerMap P_1, bool P_2 = false, bool P_3 = false)
				{
					return new sFUjBWoRWvBCcJjkbyFuAQIZnYFgA(-2)
					{
						MzLBcnFDGvAgDBoFGsOhKFGgxvgce = this,
						GJjqLbnirVNYoWWmhxFRVUsaPtdm = P_0,
						zsZdtbAoOLUrofelwniZsnnVLkUdA = P_1,
						ZFiFBXmiyuaQWvraERuzwFxZdqZfA = P_2,
						QVgGVsfwjyJSdQNCIdyMEAQjsVip = P_3
					};
				}

				[IteratorStateMachine(typeof(uCdAUSaynPXuKZvRtDCOwHkZHkAp))]
				private IEnumerable<ElementAssignmentConflictInfo> FUFqHTvCYMNHTXXXWmRretxhHLEK(int P_0, CustomControllerMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false)
				{
					return new uCdAUSaynPXuKZvRtDCOwHkZHkAp(-2)
					{
						fFruoJrfLbhinChIhjnqGZKrwtfqA = this,
						vuBdOxGPZRGnwDUKXkpJYdyjVZCRA = P_0,
						YIajnaHfAFeRBHxKtyuZefvqJdbq = P_1,
						GOJcFXZUQozAnrWpfTrRUdDxKjsV = P_2,
						IsjEGtoMzUKuVVFPhDbsekmmpoqeA = P_3,
						PRRkvFWEQqykLgdtHFGEFZdqwhMw = P_4
					};
				}

				[IteratorStateMachine(typeof(wHIjoWuYAVrzmfgIVsiySJSsMxul))]
				private IEnumerable<ElementAssignmentConflictInfo> AZBedMJlTnzusafJSIWNmNHBdYaeA(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					return new wHIjoWuYAVrzmfgIVsiySJSsMxul(-2)
					{
						ecPwMxpSVgeSzTOlYxFFGSvZaWSU = this,
						HaHcsSCPTayPVuLjlbliOjoDQLWz = P_0,
						FQdneaQKnfFnPHaYIJqiAcGhcqCpc = P_1,
						AxnRDvwkNUxDAhJiEuCsssAfkBqL = P_2
					};
				}

				private int YslApImfxKGvimZCqrZPtTwXxFti(int P_0, JoystickMap P_1, bool P_2 = false, bool P_3 = false)
				{
					if (P_0 < 0 || P_1 == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0)
						{
							num += AqzMgmbXkOorlYtXogcoSLJdBBsP(ControllerType.Joystick, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA);
						}
					}
					return num;
				}

				private int OxGvfxDZKBrHgPXDgNVeYMIeoIaA(int P_0, JoystickMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0)
						{
							num += CENpKLHfqVZXqBZiZMlmXTusghFl(ControllerType.Joystick, P_0, P_1, P_2, P_3, P_4, YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA);
						}
					}
					return num;
				}

				private int dqwAGWYayZBluQbgyosZqBWoIgee(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					if (P_0.controllerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0.controllerId)
						{
							num += tSqdKfLKqgEMByULJoaOvsARNJuV(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA);
						}
					}
					return num;
				}

				private int EQACapKhXQbJNKncCLyuyiTuLFAnA(KeyboardMap P_0, bool P_1 = false, bool P_2 = false)
				{
					return AqzMgmbXkOorlYtXogcoSLJdBBsP(ControllerType.Keyboard, 0, P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA);
				}

				private int zKbofRSzJmfycNHsvhHMeEfXxAjf(KeyboardMap P_0, ActionElementMap P_1, bool P_2 = false, bool P_3 = false)
				{
					return CENpKLHfqVZXqBZiZMlmXTusghFl(ControllerType.Keyboard, 0, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA);
				}

				private int nWAndEWKazfBllukVxUggzZPfaPo(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					if (P_0.elementAssignmentType != ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					return tSqdKfLKqgEMByULJoaOvsARNJuV(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA);
				}

				private int EYPpTgGeodCCxzzeYZvOWaABITdA(MouseMap P_0, bool P_1 = false, bool P_2 = false)
				{
					return AqzMgmbXkOorlYtXogcoSLJdBBsP(ControllerType.Mouse, 0, P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA);
				}

				private int rwTcIPgNdynYJpmauOGUrgngYMid(MouseMap P_0, ActionElementMap P_1, bool P_2 = false, bool P_3 = false)
				{
					return CENpKLHfqVZXqBZiZMlmXTusghFl(ControllerType.Mouse, 0, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA);
				}

				private int qoLLQthXTHryAPhDJDaynKbPjbEj(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					if (P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					return tSqdKfLKqgEMByULJoaOvsARNJuV(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA);
				}

				private int tDCaLfGaFIsqlcPvIbyxoYTAQCHFA(int P_0, CustomControllerMap P_1, bool P_2 = false, bool P_3 = false)
				{
					if (P_0 < 0 || P_1 == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0)
						{
							num += AqzMgmbXkOorlYtXogcoSLJdBBsP(ControllerType.Custom, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA);
						}
					}
					return num;
				}

				private int EBpdVGkxgMqOtBLJeAGZpfOdZABQ(int P_0, CustomControllerMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0)
						{
							num += CENpKLHfqVZXqBZiZMlmXTusghFl(ControllerType.Custom, P_0, P_1, P_2, P_3, P_4, YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA);
						}
					}
					return num;
				}

				private int AFPzhsOsAPeXZgZVrCxKydLbTaUh(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false)
				{
					if (P_0.controllerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0.controllerId)
						{
							num += tSqdKfLKqgEMByULJoaOvsARNJuV(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA);
						}
					}
					return num;
				}

				private int cQNTHsvMGLemHbuXbEYWEJufRxdnc(int P_0, JoystickMap P_1, bool P_2 = false, bool P_3 = false, List<ActionElementMap> P_4 = null)
				{
					if (P_0 < 0 || P_1 == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0)
						{
							num += mvIPhySWhlevMKCFbaOoRtJudXSL(ControllerType.Joystick, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA, P_4);
						}
					}
					return num;
				}

				private int ebsGUymBTpVYNOVKoBMBKdyQhgtQ(int P_0, JoystickMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, List<ActionElementMap> P_5 = null)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0)
						{
							num += hMKDPdgPFObyCAfYaLmZuIVLSswAc(ControllerType.Joystick, P_0, P_1, P_2, P_3, P_4, YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA, P_5);
						}
					}
					return num;
				}

				private int CMyNaAbDlLQLxuZIiBtMypTkmrzl(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, List<ActionElementMap> P_3 = null)
				{
					if (P_0.controllerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0.controllerId)
						{
							num += BZUMjSlOYBSMJIepsirSGJbxazQg(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA, P_3);
						}
					}
					return num;
				}

				private int muxHjeAuPZfQlQtSDwANChPxmCTG(KeyboardMap P_0, bool P_1 = false, bool P_2 = false, List<ActionElementMap> P_3 = null)
				{
					return mvIPhySWhlevMKCFbaOoRtJudXSL(ControllerType.Keyboard, 0, P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA, P_3);
				}

				private int hgERfktSEemIPqEdapZDdgGGEygt(KeyboardMap P_0, ActionElementMap P_1, bool P_2 = false, bool P_3 = false, List<ActionElementMap> P_4 = null)
				{
					return hMKDPdgPFObyCAfYaLmZuIVLSswAc(ControllerType.Keyboard, 0, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA, P_4);
				}

				private int xJUkugJIjNiQaadRfcOtrJBPKUmNA(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, List<ActionElementMap> P_3 = null)
				{
					if (P_0.elementAssignmentType != ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					return BZUMjSlOYBSMJIepsirSGJbxazQg(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.YjmFIPDNzImOLouWSUVvaLZLKYPIA, P_3);
				}

				private int jZaAFXHvQIDikPvuoATOHCURzjqiA(MouseMap P_0, bool P_1 = false, bool P_2 = false, List<ActionElementMap> P_3 = null)
				{
					return mvIPhySWhlevMKCFbaOoRtJudXSL(ControllerType.Mouse, 0, P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA, P_3);
				}

				private int HVyTPnRNRTRdOEANFmeTEyWvjUcu(MouseMap P_0, ActionElementMap P_1, bool P_2 = false, bool P_3 = false, List<ActionElementMap> P_4 = null)
				{
					return hMKDPdgPFObyCAfYaLmZuIVLSswAc(ControllerType.Mouse, 0, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA, P_4);
				}

				private int XJJseptvcEqwrZpmZlfrQmvvWAEE(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, List<ActionElementMap> P_3 = null)
				{
					if (P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					return BZUMjSlOYBSMJIepsirSGJbxazQg(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.zUjTgtTczFEofMiBtuFpiPLDHbokA, P_3);
				}

				private int WcyQbIaVqDmFkleIgffoaAjTlpgvA(int P_0, CustomControllerMap P_1, bool P_2 = false, bool P_3 = false, List<ActionElementMap> P_4 = null)
				{
					if (P_0 < 0 || P_1 == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0)
						{
							num += mvIPhySWhlevMKCFbaOoRtJudXSL(ControllerType.Custom, P_0, P_1, P_2, P_3, YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA, P_4);
						}
					}
					return num;
				}

				private int bFUJeNFxDwEZDxYxubVpquzoHQHL(int P_0, CustomControllerMap P_1, ActionElementMap P_2, bool P_3 = false, bool P_4 = false, List<ActionElementMap> P_5 = null)
				{
					if (P_0 < 0 || P_2 == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0)
						{
							num += hMKDPdgPFObyCAfYaLmZuIVLSswAc(ControllerType.Custom, P_0, P_1, P_2, P_3, P_4, YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA, P_5);
						}
					}
					return num;
				}

				private int FcYiXiBfPfcEHQKtjMlJqKvetzsJ(ElementAssignmentConflictCheck P_0, bool P_1 = false, bool P_2 = false, List<ActionElementMap> P_3 = null)
				{
					if (P_0.controllerId < 0 || P_0.elementAssignmentType == ElementAssignmentType.KeyboardKey)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.RArIgKvfketfmjSIMPRkYKyyEMJJA(); i++)
					{
						if (YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0.controllerId)
						{
							num += BZUMjSlOYBSMJIepsirSGJbxazQg(P_0, P_1, P_2, YGgiLrmkKvnCQihxbvaZObfoyApM.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i).GgWdqFddlhBZqeFBwCaEiHGCVWTqA, P_3);
						}
					}
					return num;
				}

				private bool AGBcjxXbdKpmMEoMoUsQipCkCrFKA<_0001>(ControllerType P_0, int P_1, _0001 P_2, bool P_3, bool P_4, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_5) where _0001 : ControllerMap
				{
					if (P_5 == null || P_2 == null)
					{
						return false;
					}
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(P_2.categoryId);
					if (mapCategory == null)
					{
						return false;
					}
					for (int i = 0; i < P_5.GeYULXtFpYgsaXqXZIDwegpjOMgW(); i++)
					{
						ControllerMap controllerMap = P_5.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(i);
						if ((!P_3 || controllerMap.enabled) && (P_4 || !ZMSnrXwatOJKXFUOINkdsMMJbHqEA(mapCategory, controllerMap)) && controllerMap.DoesElementAssignmentConflict(P_2, P_3))
						{
							return true;
						}
					}
					return false;
				}

				private bool KKGAQlibiwiNzcNKGTvnMshxkFgnB<_0001>(ControllerType P_0, int P_1, _0001 P_2, ActionElementMap P_3, bool P_4, bool P_5, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_6) where _0001 : ControllerMap
				{
					if (P_6 == null || P_3 == null)
					{
						return false;
					}
					InputMapCategory inputMapCategory = ((P_2 != null) ? ReInput.mapping.GetMapCategory(P_2.categoryId) : null);
					for (int i = 0; i < P_6.GeYULXtFpYgsaXqXZIDwegpjOMgW(); i++)
					{
						ControllerMap controllerMap = P_6.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(i);
						if ((!P_4 || controllerMap.enabled) && (P_5 || !ZMSnrXwatOJKXFUOINkdsMMJbHqEA(inputMapCategory, controllerMap)) && controllerMap.DoesElementAssignmentConflict(P_3, P_4))
						{
							return true;
						}
					}
					return false;
				}

				private bool haoRagDWpeniondPeGOlXoiVHjcJA<_0001>(ElementAssignmentConflictCheck P_0, bool P_1, bool P_2, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_3) where _0001 : ControllerMap
				{
					if (P_3 == null)
					{
						return false;
					}
					Player player = ReInput.players.GetPlayer(P_0.playerId);
					if (player == null)
					{
						return false;
					}
					ControllerMap map = player.controllers.maps.GetMap(P_0.controllerType, P_0.controllerId, P_0.controllerMapId);
					InputMapCategory inputMapCategory = ((map != null) ? ReInput.mapping.GetMapCategory(map.categoryId) : ReInput.mapping.GetMapCategory(P_0.controllerMapCategoryId));
					if (inputMapCategory == null)
					{
						return false;
					}
					for (int i = 0; i < P_3.GeYULXtFpYgsaXqXZIDwegpjOMgW(); i++)
					{
						ControllerMap controllerMap = P_3.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(i);
						if ((!P_1 || controllerMap.enabled) && (P_2 || !ZMSnrXwatOJKXFUOINkdsMMJbHqEA(inputMapCategory, controllerMap)) && controllerMap.DoesElementAssignmentConflict(P_0, P_1))
						{
							return true;
						}
					}
					return false;
				}

				[IteratorStateMachine(typeof(NmmQkrvndwFaCWOWSFbvRtCwTvxy))]
				private IEnumerable<ElementAssignmentConflictInfo> nBsAqDGgxwAxzRoUafYXAbtBPhVXb<_0001>(ControllerType P_0, int P_1, _0001 P_2, bool P_3, bool P_4, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_5) where _0001 : ControllerMap
				{
					return new NmmQkrvndwFaCWOWSFbvRtCwTvxy<_0001>(-2)
					{
						fPvcNbBOtFCsdkVqTTDZgpsFIkFCA = this,
						XUcmNIrFMSvGMbLcIrNpaQsAsncj = P_0,
						qfdVgkXxrmjFpGocqEcIEwhkWEViA = P_1,
						zNiXWOXAJkqGbTekVSKxialzRvEn = P_2,
						USOpVgbMepkmcUkurHggUFhLkUpD = P_3,
						BJvyopErriSwUGBXeNNfoOydYcDb = P_4,
						SYKoyllvoODsmpzDGiCEcpMhHxrL = P_5
					};
				}

				[IteratorStateMachine(typeof(eNcbsNnBTGFEnRBcPHCKoVHkJJBj))]
				private IEnumerable<ElementAssignmentConflictInfo> VWzCqXLGGBbVBDfGyrTEnCVZcAZfA<_0001>(ControllerType P_0, int P_1, _0001 P_2, ActionElementMap P_3, bool P_4, bool P_5, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_6) where _0001 : ControllerMap
				{
					return new eNcbsNnBTGFEnRBcPHCKoVHkJJBj<_0001>(-2)
					{
						KkKonfOqGacAAVFSmFOAAvyhEBNEb = this,
						EzZpsMbcpCkajvDLAeaFtGZLCFbN = P_0,
						WIodLvGMnfnTAUdLeCYBdIWitlylB = P_1,
						jpPRjCibEOOtODojLnBSwOsbFIcf = P_2,
						xJaimwJZzuxDRrCrcJpSjbGmLIDb = P_3,
						FYtTEUfKpIuzDgbvAofbbKquuaSx = P_4,
						wpwPAxoFImDEYlvZsJQbcayXgdMV = P_5,
						yvvLzbNnZSDVFoUbbjcWnkGGcOfr = P_6
					};
				}

				[IteratorStateMachine(typeof(NemQLvypQhFYwBgQHTUVNtjdBwplA))]
				private IEnumerable<ElementAssignmentConflictInfo> yPHbsWdMNegyAelNKdHhyecMMPrcB<_0001>(ElementAssignmentConflictCheck P_0, bool P_1, bool P_2, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_3) where _0001 : ControllerMap
				{
					return new NemQLvypQhFYwBgQHTUVNtjdBwplA<_0001>(-2)
					{
						FutDbwQzlqSknJxczTXcGeqWAZnw = this,
						ceGcwTfoYBvucLAOveyTVJPPpXpfA = P_0,
						DLZWSEYRaSssVxNTUVzowLfpmdrA = P_1,
						tNcZiIgOEpGukzYCUUDONhqaxtBA = P_2,
						oFVqfXAuQGieVuKlqjzNTiyWlsKL = P_3
					};
				}

				private int AqzMgmbXkOorlYtXogcoSLJdBBsP<_0001>(ControllerType P_0, int P_1, _0001 P_2, bool P_3, bool P_4, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_5) where _0001 : ControllerMap
				{
					if (P_5 == null || P_2 == null)
					{
						return 0;
					}
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(P_2.categoryId);
					if (mapCategory == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < P_5.GeYULXtFpYgsaXqXZIDwegpjOMgW(); i++)
					{
						ControllerMap controllerMap = P_5.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(i);
						if ((!P_3 || controllerMap.enabled) && (P_4 || !ZMSnrXwatOJKXFUOINkdsMMJbHqEA(mapCategory, controllerMap)))
						{
							num += controllerMap.RemoveElementAssignmentConflicts(P_2, P_3);
						}
					}
					return num;
				}

				private int CENpKLHfqVZXqBZiZMlmXTusghFl<_0001>(ControllerType P_0, int P_1, _0001 P_2, ActionElementMap P_3, bool P_4, bool P_5, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_6) where _0001 : ControllerMap
				{
					if (P_6 == null || P_3 == null)
					{
						return 0;
					}
					InputMapCategory inputMapCategory = ((P_2 != null) ? ReInput.mapping.GetMapCategory(P_2.categoryId) : null);
					int num = 0;
					for (int i = 0; i < P_6.GeYULXtFpYgsaXqXZIDwegpjOMgW(); i++)
					{
						ControllerMap controllerMap = P_6.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(i);
						if ((!P_4 || controllerMap.enabled) && (P_5 || !ZMSnrXwatOJKXFUOINkdsMMJbHqEA(inputMapCategory, controllerMap)))
						{
							num += controllerMap.RemoveElementAssignmentConflicts(P_3, P_4);
						}
					}
					return num;
				}

				private int tSqdKfLKqgEMByULJoaOvsARNJuV<_0001>(ElementAssignmentConflictCheck P_0, bool P_1, bool P_2, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_3) where _0001 : ControllerMap
				{
					if (P_3 == null)
					{
						return 0;
					}
					Player player = ReInput.players.GetPlayer(P_0.playerId);
					if (player == null)
					{
						return 0;
					}
					ControllerMap map = player.controllers.maps.GetMap(P_0.controllerType, P_0.controllerId, P_0.controllerMapId);
					InputMapCategory inputMapCategory = ((map != null) ? ReInput.mapping.GetMapCategory(map.categoryId) : ReInput.mapping.GetMapCategory(P_0.controllerMapCategoryId));
					if (inputMapCategory == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < P_3.GeYULXtFpYgsaXqXZIDwegpjOMgW(); i++)
					{
						ControllerMap controllerMap = P_3.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(i);
						if ((!P_1 || controllerMap.enabled) && (P_2 || !ZMSnrXwatOJKXFUOINkdsMMJbHqEA(inputMapCategory, controllerMap)))
						{
							num += controllerMap.RemoveElementAssignmentConflicts(P_0, P_1);
						}
					}
					return num;
				}

				private int mvIPhySWhlevMKCFbaOoRtJudXSL<_0001>(ControllerType P_0, int P_1, _0001 P_2, bool P_3, bool P_4, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_5, List<ActionElementMap> P_6 = null) where _0001 : ControllerMap
				{
					P_6?.Clear();
					if (P_5 == null || P_2 == null)
					{
						return 0;
					}
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(P_2.categoryId);
					if (mapCategory == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < P_5.GeYULXtFpYgsaXqXZIDwegpjOMgW(); i++)
					{
						ControllerMap controllerMap = P_5.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(i);
						if ((!P_3 || controllerMap.enabled) && (P_4 || !ZMSnrXwatOJKXFUOINkdsMMJbHqEA(mapCategory, controllerMap)))
						{
							num += controllerMap.uDLYQOQEpEkgIfOMsAsuIdaYXpcq(P_2, P_3, P_6, true);
						}
					}
					return num;
				}

				private int hMKDPdgPFObyCAfYaLmZuIVLSswAc<_0001>(ControllerType P_0, int P_1, _0001 P_2, ActionElementMap P_3, bool P_4, bool P_5, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_6, List<ActionElementMap> P_7 = null) where _0001 : ControllerMap
				{
					P_7?.Clear();
					if (P_6 == null || P_3 == null)
					{
						return 0;
					}
					InputMapCategory inputMapCategory = ((P_2 != null) ? ReInput.mapping.GetMapCategory(P_2.categoryId) : null);
					int num = 0;
					for (int i = 0; i < P_6.GeYULXtFpYgsaXqXZIDwegpjOMgW(); i++)
					{
						ControllerMap controllerMap = P_6.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(i);
						if ((!P_4 || controllerMap.enabled) && (P_5 || !ZMSnrXwatOJKXFUOINkdsMMJbHqEA(inputMapCategory, controllerMap)))
						{
							num += controllerMap.cTyUpZEcqTXlnXWCJGCCWDVCdMkt(P_3, P_4, P_7, true);
						}
					}
					return num;
				}

				private int BZUMjSlOYBSMJIepsirSGJbxazQg<_0001>(ElementAssignmentConflictCheck P_0, bool P_1, bool P_2, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_3, List<ActionElementMap> P_4 = null) where _0001 : ControllerMap
				{
					P_4?.Clear();
					if (P_3 == null)
					{
						return 0;
					}
					Player player = ReInput.players.GetPlayer(P_0.playerId);
					if (player == null)
					{
						return 0;
					}
					ControllerMap map = player.controllers.maps.GetMap(P_0.controllerType, P_0.controllerId, P_0.controllerMapId);
					InputMapCategory inputMapCategory = ((map != null) ? ReInput.mapping.GetMapCategory(map.categoryId) : ReInput.mapping.GetMapCategory(P_0.controllerMapCategoryId));
					if (inputMapCategory == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < P_3.GeYULXtFpYgsaXqXZIDwegpjOMgW(); i++)
					{
						ControllerMap controllerMap = P_3.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(i);
						if ((!P_1 || controllerMap.enabled) && (P_2 || !ZMSnrXwatOJKXFUOINkdsMMJbHqEA(inputMapCategory, controllerMap)))
						{
							num += controllerMap.UlMygnKDTVqoNMDLtBFpFSHRKVIw(P_0, P_1, P_4, true);
						}
					}
					return num;
				}

				private bool ZMSnrXwatOJKXFUOINkdsMMJbHqEA(InputMapCategory P_0, ControllerMap P_1)
				{
					if (P_0 == null || P_1 == null)
					{
						return false;
					}
					if (P_0.checkConflictsWithAllCategories)
					{
						return false;
					}
					IList<int> checkConflictsCategoryIds = P_0.checkConflictsCategoryIds;
					if (checkConflictsCategoryIds == null)
					{
						return true;
					}
					for (int i = 0; i < checkConflictsCategoryIds.Count; i++)
					{
						if (checkConflictsCategoryIds[i] == P_1.categoryId)
						{
							return false;
						}
					}
					return true;
				}
			}

			[DefaultMember("Item")]
			internal interface rJRHfxObWEyZQOmmYgoxgmGnxuol
			{
				azzemdvQpsbVWyBpuJprUkaWgEfEA sDAWdtZDsakGIJIaYihvJuWJnJN { get; }

				ControllerType CmnNBXSIdbihnrUVSoDPbcGWAEaJA { get; }

				int hdHAWgwPJNkiyeCCaiyVDbScMIAib { get; }

				bool ChUTkqYEvlzRZUQQaIOilOHpukeX(Controller P_0);

				bool YPWLlgPgmfXEmjcHdghesHfKasTy(int P_0);

				void tLuHYEZoYWwWCNCBqjbBnZSeRHNd(int P_0);

				void sucPQMFPlLWJuHhAtLtBWyzWZtRE(Controller P_0);

				void OyWpEnvgZWsPMJPfaFYuTBQZiZpn(int P_0);

				Controller tRzYsIDkUnycgWhiaBoHxEoQFNVe(int P_0);

				Controller akiTudPECybTenNzjQJPXgKQxnjf(string P_0);

				int EaFhdJXPCpQaFgRFmoouXHNWdXkU(Controller P_0);

				int faRlVmfoiqiQrJTzbjHrcoaesFpg(int P_0);

				int JJLOtnZmgNTSeMMhVpBDNOnBJmWC(string P_0);

				void qDXuYglJNcJClRMmUAumTMMTvwVf();

				azzemdvQpsbVWyBpuJprUkaWgEfEA eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(int P_0);

				azzemdvQpsbVWyBpuJprUkaWgEfEA XRkBbJBsJBaziaGmeTwVOAtSsQbXb(Controller P_0);

				void OectVWAHFVqDzTrMmfXfIKlAoVeY(azzemdvQpsbVWyBpuJprUkaWgEfEA P_0);
			}

			internal interface azzemdvQpsbVWyBpuJprUkaWgEfEA
			{
				vHszkbCJdDAIcILHhpVCxcZlIBxlA UEDbvZCpORwBphRIdGLFIEwJLiiEb { get; }

				Controller HwlnGjcjkEjHQFCbiyeLWdkAyzlm { get; }

				double OvEOwhMasvJnaipypySCYOXubbwy { get; }
			}

			[DefaultMember("Item")]
			internal sealed class fLcZuTpMOwYPWmGZCMQAZMEBzxNc<_0001, _0002> : rJRHfxObWEyZQOmmYgoxgmGnxuol where _0001 : Controller where _0002 : ControllerMap
			{
				public class afpFMKpOFhEzEBdDrNcunaaeluicA : azzemdvQpsbVWyBpuJprUkaWgEfEA
				{
					public _0001 TFffYazGHMOVnLpOXAoTjAaUfvCi;

					public global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0002> GgWdqFddlhBZqeFBwCaEiHGCVWTqA;

					public double VMVGtFSxnlRICEbhWfEXJOzjDuKP;

					Controller azzemdvQpsbVWyBpuJprUkaWgEfEA.PRTGqMcuZQlTjTPZSLCBoIoXctsJ => TFffYazGHMOVnLpOXAoTjAaUfvCi;

					vHszkbCJdDAIcILHhpVCxcZlIBxlA azzemdvQpsbVWyBpuJprUkaWgEfEA.SWGxajomXWVKDPavQcQkjpYcckNEb => GgWdqFddlhBZqeFBwCaEiHGCVWTqA;

					double azzemdvQpsbVWyBpuJprUkaWgEfEA.EPkNFPqrJdOHsMeHdGrGGveRwzGHA => VMVGtFSxnlRICEbhWfEXJOzjDuKP;

					public afpFMKpOFhEzEBdDrNcunaaeluicA(_0001 P_0, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0002> P_1)
					{
						TFffYazGHMOVnLpOXAoTjAaUfvCi = P_0;
						GgWdqFddlhBZqeFBwCaEiHGCVWTqA = P_1;
					}

					public void eEiBJvFIRTCjCLeGfjCifhpeLHnhA()
					{
						VMVGtFSxnlRICEbhWfEXJOzjDuKP = ReInput.unscaledTime;
					}
				}

				private List<afpFMKpOFhEzEBdDrNcunaaeluicA> wSwZSfXvKbgIdSmEfCKnBybDhzfGA;

				private List<_0001> GuqIICgDfejdjMhboHwIDpBkxJbfA;

				private ReadOnlyCollection<_0001> YYBALXefFWKWEAhcmSXkfRJgfVQKc;

				private readonly ControllerType zCjKRlGOcZWtzrMTHByOyyiApysr;

				int rJRHfxObWEyZQOmmYgoxgmGnxuol.hdHAWgwPJNkiyeCCaiyVDbScMIAib => wSwZSfXvKbgIdSmEfCKnBybDhzfGA.Count;

				public IList<_0001> ZDrrszgPTGReCxYGDqgUGfMuezBB => YYBALXefFWKWEAhcmSXkfRJgfVQKc;

				public afpFMKpOFhEzEBdDrNcunaaeluicA dSYKQkbgntZnAQkRTKvyOruncsBr => wSwZSfXvKbgIdSmEfCKnBybDhzfGA[P_0];

				ControllerType rJRHfxObWEyZQOmmYgoxgmGnxuol.CmnNBXSIdbihnrUVSoDPbcGWAEaJA => zCjKRlGOcZWtzrMTHByOyyiApysr;

				azzemdvQpsbVWyBpuJprUkaWgEfEA rJRHfxObWEyZQOmmYgoxgmGnxuol.fvhgcCcDoEEdwvOAsNZxGaWYIOBnA => wSwZSfXvKbgIdSmEfCKnBybDhzfGA[index];

				public fLcZuTpMOwYPWmGZCMQAZMEBzxNc()
				{
					if ((object)cVDyIiOsEfJNYzVuZSmuEXqylgT.WgiGQObGClzyTmnYauahFKxEARVE<_0001>() != typeof(_0002))
					{
						throw new Exception(typeof(_0001).Name + " cannot be used with a map of type " + typeof(_0002).Name);
					}
					zCjKRlGOcZWtzrMTHByOyyiApysr = cVDyIiOsEfJNYzVuZSmuEXqylgT.BSwQqTYVRDGoCrMsVyJwioaaDRlKA(typeof(_0001));
					wSwZSfXvKbgIdSmEfCKnBybDhzfGA = new List<afpFMKpOFhEzEBdDrNcunaaeluicA>();
					GuqIICgDfejdjMhboHwIDpBkxJbfA = new List<_0001>();
					YYBALXefFWKWEAhcmSXkfRJgfVQKc = new ReadOnlyCollection<_0001>(GuqIICgDfejdjMhboHwIDpBkxJbfA);
				}

				public afpFMKpOFhEzEBdDrNcunaaeluicA pkjlWGvDwUGmTlVSrBQXPvLtnxSf(int P_0)
				{
					if (zCjKRlGOcZWtzrMTHByOyyiApysr == ControllerType.Keyboard || zCjKRlGOcZWtzrMTHByOyyiApysr == ControllerType.Mouse)
					{
						P_0 = 0;
					}
					int num = tBpYkJcehaMgvrfSgoFzUIsiUoRb(P_0);
					if (num < 0)
					{
						return null;
					}
					return wSwZSfXvKbgIdSmEfCKnBybDhzfGA[num];
				}

				public afpFMKpOFhEzEBdDrNcunaaeluicA HXpgYBEgWKiBmOzgXMPbeuwzCWiyA(_0001 P_0)
				{
					if (P_0 == null)
					{
						return null;
					}
					return pkjlWGvDwUGmTlVSrBQXPvLtnxSf(P_0.id);
				}

				public void RcHyVTjtIQMIakNthRUsDLEHAPIk(afpFMKpOFhEzEBdDrNcunaaeluicA P_0)
				{
					if (P_0 != null)
					{
						wSwZSfXvKbgIdSmEfCKnBybDhzfGA.Add(P_0);
						GuqIICgDfejdjMhboHwIDpBkxJbfA.Add(P_0.TFffYazGHMOVnLpOXAoTjAaUfvCi);
					}
				}

				public void DIsnnCqrmpwTUiqlECTmAqhduariA(int P_0)
				{
					if (zCjKRlGOcZWtzrMTHByOyyiApysr == ControllerType.Keyboard || zCjKRlGOcZWtzrMTHByOyyiApysr == ControllerType.Mouse)
					{
						P_0 = 0;
					}
					if (tBpYkJcehaMgvrfSgoFzUIsiUoRb(P_0) < 0)
					{
						return;
					}
					for (int i = 0; i < wSwZSfXvKbgIdSmEfCKnBybDhzfGA.Count; i++)
					{
						if (wSwZSfXvKbgIdSmEfCKnBybDhzfGA[i].TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0)
						{
							srKYEDluyYQKAiHneSvRJFgYXFQh(i);
							break;
						}
					}
				}

				void rJRHfxObWEyZQOmmYgoxgmGnxuol.tLuHYEZoYWwWCNCBqjbBnZSeRHNd(int P_0)
				{
					//ILSpy generated this explicit interface implementation from .override directive in DIsnnCqrmpwTUiqlECTmAqhduariA
					this.DIsnnCqrmpwTUiqlECTmAqhduariA(P_0);
				}

				public void YDZrIvUUPvfAsegfbJPUgutaLAfFb(_0001 P_0)
				{
					if (P_0 != null && P_0.type == zCjKRlGOcZWtzrMTHByOyyiApysr)
					{
						DIsnnCqrmpwTUiqlECTmAqhduariA(P_0.id);
					}
				}

				public void srKYEDluyYQKAiHneSvRJFgYXFQh(int P_0)
				{
					if (P_0 >= 0 && P_0 < wSwZSfXvKbgIdSmEfCKnBybDhzfGA.Count)
					{
						wSwZSfXvKbgIdSmEfCKnBybDhzfGA.RemoveAt(P_0);
						GuqIICgDfejdjMhboHwIDpBkxJbfA.RemoveAt(P_0);
					}
				}

				void rJRHfxObWEyZQOmmYgoxgmGnxuol.OyWpEnvgZWsPMJPfaFYuTBQZiZpn(int P_0)
				{
					//ILSpy generated this explicit interface implementation from .override directive in srKYEDluyYQKAiHneSvRJFgYXFQh
					this.srKYEDluyYQKAiHneSvRJFgYXFQh(P_0);
				}

				public _0001 mwluYWpfujxinFejkWlvebBLHySV(int P_0)
				{
					if (zCjKRlGOcZWtzrMTHByOyyiApysr == ControllerType.Keyboard || zCjKRlGOcZWtzrMTHByOyyiApysr == ControllerType.Mouse)
					{
						P_0 = 0;
					}
					int num = tBpYkJcehaMgvrfSgoFzUIsiUoRb(P_0);
					if (num < 0)
					{
						return null;
					}
					return wSwZSfXvKbgIdSmEfCKnBybDhzfGA[num].TFffYazGHMOVnLpOXAoTjAaUfvCi;
				}

				public bool ZGKLCWtyDAoNLljKbxTLYFZxzmwo(int P_0)
				{
					if (zCjKRlGOcZWtzrMTHByOyyiApysr == ControllerType.Keyboard || zCjKRlGOcZWtzrMTHByOyyiApysr == ControllerType.Mouse)
					{
						P_0 = 0;
					}
					if (P_0 < 0)
					{
						return false;
					}
					for (int i = 0; i < wSwZSfXvKbgIdSmEfCKnBybDhzfGA.Count; i++)
					{
						if (wSwZSfXvKbgIdSmEfCKnBybDhzfGA[i].TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0)
						{
							return true;
						}
					}
					return false;
				}

				bool rJRHfxObWEyZQOmmYgoxgmGnxuol.YPWLlgPgmfXEmjcHdghesHfKasTy(int P_0)
				{
					//ILSpy generated this explicit interface implementation from .override directive in ZGKLCWtyDAoNLljKbxTLYFZxzmwo
					return this.ZGKLCWtyDAoNLljKbxTLYFZxzmwo(P_0);
				}

				public bool bRhEzXwkmaklzoHwQCaBwbzwcWzO(_0001 P_0)
				{
					if (P_0 == null)
					{
						return false;
					}
					if (P_0.type != zCjKRlGOcZWtzrMTHByOyyiApysr)
					{
						return false;
					}
					return ZGKLCWtyDAoNLljKbxTLYFZxzmwo(P_0.id);
				}

				public int tBpYkJcehaMgvrfSgoFzUIsiUoRb(int P_0)
				{
					if (zCjKRlGOcZWtzrMTHByOyyiApysr == ControllerType.Keyboard || zCjKRlGOcZWtzrMTHByOyyiApysr == ControllerType.Mouse)
					{
						P_0 = 0;
					}
					if (P_0 < 0)
					{
						return -1;
					}
					for (int i = 0; i < wSwZSfXvKbgIdSmEfCKnBybDhzfGA.Count; i++)
					{
						if (wSwZSfXvKbgIdSmEfCKnBybDhzfGA[i].TFffYazGHMOVnLpOXAoTjAaUfvCi.id == P_0)
						{
							return i;
						}
					}
					return -1;
				}

				int rJRHfxObWEyZQOmmYgoxgmGnxuol.faRlVmfoiqiQrJTzbjHrcoaesFpg(int P_0)
				{
					//ILSpy generated this explicit interface implementation from .override directive in tBpYkJcehaMgvrfSgoFzUIsiUoRb
					return this.tBpYkJcehaMgvrfSgoFzUIsiUoRb(P_0);
				}

				public int BeSYkoplBVvaRPuXfsItNLoMFguv(_0001 P_0)
				{
					if (P_0 == null)
					{
						return -1;
					}
					if (P_0.type != zCjKRlGOcZWtzrMTHByOyyiApysr)
					{
						return -1;
					}
					return tBpYkJcehaMgvrfSgoFzUIsiUoRb(P_0.id);
				}

				public int AYuDtPJRztTJjJWlUcXNWGwSuaIU(string P_0)
				{
					if (P_0 == null || P_0 == string.Empty)
					{
						return -1;
					}
					for (int i = 0; i < wSwZSfXvKbgIdSmEfCKnBybDhzfGA.Count; i++)
					{
						if (wSwZSfXvKbgIdSmEfCKnBybDhzfGA[i].TFffYazGHMOVnLpOXAoTjAaUfvCi.tag.Equals(P_0, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				int rJRHfxObWEyZQOmmYgoxgmGnxuol.JJLOtnZmgNTSeMMhVpBDNOnBJmWC(string P_0)
				{
					//ILSpy generated this explicit interface implementation from .override directive in AYuDtPJRztTJjJWlUcXNWGwSuaIU
					return this.AYuDtPJRztTJjJWlUcXNWGwSuaIU(P_0);
				}

				public void DKVqwmQVnvDIPMBoxetgfzXgJneqA()
				{
					wSwZSfXvKbgIdSmEfCKnBybDhzfGA.Clear();
					GuqIICgDfejdjMhboHwIDpBkxJbfA.Clear();
				}

				void rJRHfxObWEyZQOmmYgoxgmGnxuol.qDXuYglJNcJClRMmUAumTMMTvwVf()
				{
					//ILSpy generated this explicit interface implementation from .override directive in DKVqwmQVnvDIPMBoxetgfzXgJneqA
					this.DKVqwmQVnvDIPMBoxetgfzXgJneqA();
				}

				azzemdvQpsbVWyBpuJprUkaWgEfEA rJRHfxObWEyZQOmmYgoxgmGnxuol.GetEntry(int controllerId)
				{
					return pkjlWGvDwUGmTlVSrBQXPvLtnxSf(controllerId);
				}

				azzemdvQpsbVWyBpuJprUkaWgEfEA rJRHfxObWEyZQOmmYgoxgmGnxuol.GetEntry(Controller controller)
				{
					if (controller as _0001 == null)
					{
						return null;
					}
					return HXpgYBEgWKiBmOzgXMPbeuwzCWiyA(controller as _0001);
				}

				void rJRHfxObWEyZQOmmYgoxgmGnxuol.AddEntry(azzemdvQpsbVWyBpuJprUkaWgEfEA entry)
				{
					RcHyVTjtIQMIakNthRUsDLEHAPIk((afpFMKpOFhEzEBdDrNcunaaeluicA)entry);
				}

				void rJRHfxObWEyZQOmmYgoxgmGnxuol.RemoveController(Controller controller)
				{
					YDZrIvUUPvfAsegfbJPUgutaLAfFb(controller as _0001);
				}

				Controller rJRHfxObWEyZQOmmYgoxgmGnxuol.GetController(int controllerId)
				{
					return mwluYWpfujxinFejkWlvebBLHySV(controllerId);
				}

				bool rJRHfxObWEyZQOmmYgoxgmGnxuol.Contains(Controller controller)
				{
					return bRhEzXwkmaklzoHwQCaBwbzwcWzO(controller as _0001);
				}

				int rJRHfxObWEyZQOmmYgoxgmGnxuol.IndexOf(Controller controller)
				{
					return BeSYkoplBVvaRPuXfsItNLoMFguv(controller as _0001);
				}

				Controller rJRHfxObWEyZQOmmYgoxgmGnxuol.GetControllerWithTag(string tag)
				{
					int num = AYuDtPJRztTJjJWlUcXNWGwSuaIU(tag);
					if (num < 0)
					{
						return null;
					}
					return wSwZSfXvKbgIdSmEfCKnBybDhzfGA[num].TFffYazGHMOVnLpOXAoTjAaUfvCi;
				}
			}

			internal class fYKvcKBvQvLCetjdAAIphvYYuqYMA
			{
				public readonly int qmWCUQsQKLAGFDPAlkhDMwpKjAPm;

				private ControllerType[] WYYGpmkPGICfEHoXnwBUoAQgwCjlA;

				private rJRHfxObWEyZQOmmYgoxgmGnxuol[] RAhuSbifUFqgwhwEUSQHHOwOeVCS;

				public rJRHfxObWEyZQOmmYgoxgmGnxuol kImmYmzaCiBQvBGIMTsyajaFoeGI(int P_0)
				{
					return RAhuSbifUFqgwhwEUSQHHOwOeVCS[P_0];
				}

				public ControllerType vdLtqhDGdbvtgdsibpfWVEbufiSz(int P_0)
				{
					return WYYGpmkPGICfEHoXnwBUoAQgwCjlA[P_0];
				}

				public fYKvcKBvQvLCetjdAAIphvYYuqYMA(int P_0)
				{
					qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MathTools.Max(0, P_0);
					WYYGpmkPGICfEHoXnwBUoAQgwCjlA = new ControllerType[P_0];
					RAhuSbifUFqgwhwEUSQHHOwOeVCS = new rJRHfxObWEyZQOmmYgoxgmGnxuol[P_0];
				}

				public rJRHfxObWEyZQOmmYgoxgmGnxuol kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType P_0)
				{
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						if (P_0 == WYYGpmkPGICfEHoXnwBUoAQgwCjlA[i])
						{
							return RAhuSbifUFqgwhwEUSQHHOwOeVCS[i];
						}
					}
					throw new Exception("Value is not in the set.");
				}

				public void WSAzvniOAzEKalJWPqwJODoejxSCA(int P_0, ControllerType P_1, rJRHfxObWEyZQOmmYgoxgmGnxuol P_2)
				{
					WYYGpmkPGICfEHoXnwBUoAQgwCjlA[P_0] = P_1;
					RAhuSbifUFqgwhwEUSQHHOwOeVCS[P_0] = P_2;
				}
			}

			private class ioztArpRhJBBpDKdzHOLwnTeIxai
			{
				public class AWuExMeXcRcFvejhKnEIzzotTfRNA
				{
					public int wzLIvPcnBdaoAUwimrDfrvStkckb;

					public global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<JoystickMap> NixxSePeJrbvXWAFUfJZYdxHrNWF;

					public double cdTQrUFQrmxXmTDABSVQNGgmgVzV;

					public AWuExMeXcRcFvejhKnEIzzotTfRNA(int P_0, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<JoystickMap> P_1, double P_2)
					{
						wzLIvPcnBdaoAUwimrDfrvStkckb = P_0;
						NixxSePeJrbvXWAFUfJZYdxHrNWF = P_1;
						cdTQrUFQrmxXmTDABSVQNGgmgVzV = P_2;
					}
				}

				private readonly List<AWuExMeXcRcFvejhKnEIzzotTfRNA> lWssfaZajKQsnLaUtJPaRMLRoIsA;

				private readonly Player OSoZYYljkKibiLKOvcoSDsMwCGsm;

				public ioztArpRhJBBpDKdzHOLwnTeIxai(Player P_0)
				{
					OSoZYYljkKibiLKOvcoSDsMwCGsm = P_0;
					lWssfaZajKQsnLaUtJPaRMLRoIsA = new List<AWuExMeXcRcFvejhKnEIzzotTfRNA>();
				}

				public void UXThxtVoYROgWzmqhhBkXfgNiSDM(Joystick P_0, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<JoystickMap> P_1)
				{
					for (int i = 0; i < lWssfaZajKQsnLaUtJPaRMLRoIsA.Count; i++)
					{
						AWuExMeXcRcFvejhKnEIzzotTfRNA aWuExMeXcRcFvejhKnEIzzotTfRNA = lWssfaZajKQsnLaUtJPaRMLRoIsA[i];
						if (aWuExMeXcRcFvejhKnEIzzotTfRNA.wzLIvPcnBdaoAUwimrDfrvStkckb == P_0.id)
						{
							aWuExMeXcRcFvejhKnEIzzotTfRNA.NixxSePeJrbvXWAFUfJZYdxHrNWF = P_1;
							aWuExMeXcRcFvejhKnEIzzotTfRNA.cdTQrUFQrmxXmTDABSVQNGgmgVzV = ReInput.realTime;
							return;
						}
					}
					AWuExMeXcRcFvejhKnEIzzotTfRNA item = new AWuExMeXcRcFvejhKnEIzzotTfRNA(P_0.id, P_1, ReInput.realTime);
					lWssfaZajKQsnLaUtJPaRMLRoIsA.Add(item);
				}

				public void GrkffHPRitRYlQBnXbCOZrtaeRHj(fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap>.afpFMKpOFhEzEBdDrNcunaaeluicA P_0)
				{
					UXThxtVoYROgWzmqhhBkXfgNiSDM(P_0.TFffYazGHMOVnLpOXAoTjAaUfvCi, P_0.GgWdqFddlhBZqeFBwCaEiHGCVWTqA);
				}

				public void GLaWLnEHInVwyAWnURcXgeouPfWv()
				{
					for (int i = 0; i < lWssfaZajKQsnLaUtJPaRMLRoIsA.Count; i++)
					{
						if (!OSoZYYljkKibiLKOvcoSDsMwCGsm.controllers.ContainsController(ControllerType.Joystick, lWssfaZajKQsnLaUtJPaRMLRoIsA[i].wzLIvPcnBdaoAUwimrDfrvStkckb))
						{
							lWssfaZajKQsnLaUtJPaRMLRoIsA[i].NixxSePeJrbvXWAFUfJZYdxHrNWF = null;
						}
					}
				}

				public AWuExMeXcRcFvejhKnEIzzotTfRNA ZUUEyZycxAvvEXwRwslIAfpPWAmv(int P_0)
				{
					int num = QEMBgrJwCIhxcpSYOjWXDQXUFVFBA(P_0);
					if (num < 0)
					{
						return null;
					}
					return lWssfaZajKQsnLaUtJPaRMLRoIsA[num];
				}

				public bool pTpdRwepTcgkzpqCNNarvhepPaLzA(int P_0)
				{
					for (int i = 0; i < lWssfaZajKQsnLaUtJPaRMLRoIsA.Count; i++)
					{
						if (lWssfaZajKQsnLaUtJPaRMLRoIsA[i].wzLIvPcnBdaoAUwimrDfrvStkckb == P_0)
						{
							return true;
						}
					}
					return false;
				}

				public int QEMBgrJwCIhxcpSYOjWXDQXUFVFBA(int P_0)
				{
					for (int i = 0; i < lWssfaZajKQsnLaUtJPaRMLRoIsA.Count; i++)
					{
						if (lWssfaZajKQsnLaUtJPaRMLRoIsA[i].wzLIvPcnBdaoAUwimrDfrvStkckb == P_0)
						{
							return i;
						}
					}
					return -1;
				}

				public void CzSpYeUOHMbjQobFyCDEdLrecRHNA()
				{
					lWssfaZajKQsnLaUtJPaRMLRoIsA.Clear();
				}
			}

			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public sealed class MapHelper : CodeHelper
			{
				private sealed class ZGCZROVqXgqRpPRQPVRwGoxetJzx : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
				{
					private int yOvXYMevWFiExglJIHPaOdYgmoRP;

					private ActionElementMap UHjXnAzSGDCCgCnKQFGKmFqOkScp;

					private int VcZsfBNxRbPmLoPSnqMYeLOPkyrb;

					public MapHelper JTaMnchNBVBgBBoHrSsuKHTeBiRH;

					private int BrLdNnmkfgFUqPQFaSbNwuadFpkI;

					public int rVGnozjdgIPMXJNxRGegaEQqEHYh;

					private bool hSxYsOVOKNTPNJPwUHsaTgtOAVIDA;

					public bool NZfirYICFAjEfUHeInKsSZTFVeZL;

					private int bqHBaMbCSxrcpCMLYLTPLDZkBMGyA;

					private int aDtDEcFFjgmfDhARfMpoHeCCxjTRc;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol KPEgKZHkynzxfMcfWQbPcbvggpckc;

					private int BXlEnckVuliqFcCQwsjuhqeIIbUTb;

					private int xUPfOzcWgdKBOAPrZhUUhMSQjqyz;

					private vHszkbCJdDAIcILHhpVCxcZlIBxlA dTQHgdLHsPjmwXMbZkboOePxATPiA;

					private int VUrTcdJZCrAJPtsKjJoHwkJKwfqR;

					private int JhTMuBoNLWaEFeiGfxiFTUQrFMjPA;

					private IEnumerator<ActionElementMap> ABUjVjeOSVxWGJgdckNltUQBJdwUA;

					ActionElementMap IEnumerator<ActionElementMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return UHjXnAzSGDCCgCnKQFGKmFqOkScp;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return UHjXnAzSGDCCgCnKQFGKmFqOkScp;
						}
					}

					[DebuggerHidden]
					public ZGCZROVqXgqRpPRQPVRwGoxetJzx(int P_0)
					{
						yOvXYMevWFiExglJIHPaOdYgmoRP = P_0;
						VcZsfBNxRbPmLoPSnqMYeLOPkyrb = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = yOvXYMevWFiExglJIHPaOdYgmoRP;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								nxjHUJFGXkfvTmRKKUTSsAVHUbEM();
							}
						}
						KPEgKZHkynzxfMcfWQbPcbvggpckc = null;
						dTQHgdLHsPjmwXMbZkboOePxATPiA = null;
						ABUjVjeOSVxWGJgdckNltUQBJdwUA = null;
						yOvXYMevWFiExglJIHPaOdYgmoRP = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = yOvXYMevWFiExglJIHPaOdYgmoRP;
							MapHelper jTaMnchNBVBgBBoHrSsuKHTeBiRH = JTaMnchNBVBgBBoHrSsuKHTeBiRH;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								yOvXYMevWFiExglJIHPaOdYgmoRP = -3;
								goto IL_0177;
							}
							yOvXYMevWFiExglJIHPaOdYgmoRP = -1;
							if (ReInput._id != jTaMnchNBVBgBBoHrSsuKHTeBiRH.MUfpEiqygGgMiNQDuSWClpiuBuic)
							{
								ReInput.CheckInitialized(jTaMnchNBVBgBBoHrSsuKHTeBiRH.MUfpEiqygGgMiNQDuSWClpiuBuic);
								return false;
							}
							if (BrLdNnmkfgFUqPQFaSbNwuadFpkI < 0)
							{
								return false;
							}
							bqHBaMbCSxrcpCMLYLTPLDZkBMGyA = jTaMnchNBVBgBBoHrSsuKHTeBiRH.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
							aDtDEcFFjgmfDhARfMpoHeCCxjTRc = 0;
							goto IL_01f7;
							IL_0177:
							if (ABUjVjeOSVxWGJgdckNltUQBJdwUA.MoveNext())
							{
								ActionElementMap current = ABUjVjeOSVxWGJgdckNltUQBJdwUA.Current;
								UHjXnAzSGDCCgCnKQFGKmFqOkScp = current;
								yOvXYMevWFiExglJIHPaOdYgmoRP = 1;
								return true;
							}
							nxjHUJFGXkfvTmRKKUTSsAVHUbEM();
							ABUjVjeOSVxWGJgdckNltUQBJdwUA = null;
							goto IL_0191;
							IL_0191:
							JhTMuBoNLWaEFeiGfxiFTUQrFMjPA++;
							goto IL_01a3;
							IL_01cd:
							if (xUPfOzcWgdKBOAPrZhUUhMSQjqyz < BXlEnckVuliqFcCQwsjuhqeIIbUTb)
							{
								dTQHgdLHsPjmwXMbZkboOePxATPiA = KPEgKZHkynzxfMcfWQbPcbvggpckc.EfgDlYQxDuklXErnyCYTFBkkEVmX(xUPfOzcWgdKBOAPrZhUUhMSQjqyz).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
								VUrTcdJZCrAJPtsKjJoHwkJKwfqR = dTQHgdLHsPjmwXMbZkboOePxATPiA.spuxbZMpjzXXEeAzzgWchppYZEErA;
								JhTMuBoNLWaEFeiGfxiFTUQrFMjPA = 0;
								goto IL_01a3;
							}
							KPEgKZHkynzxfMcfWQbPcbvggpckc = null;
							aDtDEcFFjgmfDhARfMpoHeCCxjTRc++;
							goto IL_01f7;
							IL_01a3:
							if (JhTMuBoNLWaEFeiGfxiFTUQrFMjPA < VUrTcdJZCrAJPtsKjJoHwkJKwfqR)
							{
								if (dTQHgdLHsPjmwXMbZkboOePxATPiA.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(JhTMuBoNLWaEFeiGfxiFTUQrFMjPA) is ControllerMapWithAxes controllerMapWithAxes && (!hSxYsOVOKNTPNJPwUHsaTgtOAVIDA || controllerMapWithAxes.enabled) && controllerMapWithAxes.ContainsAction(BrLdNnmkfgFUqPQFaSbNwuadFpkI))
								{
									ABUjVjeOSVxWGJgdckNltUQBJdwUA = controllerMapWithAxes.AxisMapsWithAction(BrLdNnmkfgFUqPQFaSbNwuadFpkI, hSxYsOVOKNTPNJPwUHsaTgtOAVIDA).GetEnumerator();
									yOvXYMevWFiExglJIHPaOdYgmoRP = -3;
									goto IL_0177;
								}
								goto IL_0191;
							}
							dTQHgdLHsPjmwXMbZkboOePxATPiA = null;
							xUPfOzcWgdKBOAPrZhUUhMSQjqyz++;
							goto IL_01cd;
							IL_01f7:
							if (aDtDEcFFjgmfDhARfMpoHeCCxjTRc < bqHBaMbCSxrcpCMLYLTPLDZkBMGyA)
							{
								KPEgKZHkynzxfMcfWQbPcbvggpckc = jTaMnchNBVBgBBoHrSsuKHTeBiRH.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(aDtDEcFFjgmfDhARfMpoHeCCxjTRc);
								BXlEnckVuliqFcCQwsjuhqeIIbUTb = KPEgKZHkynzxfMcfWQbPcbvggpckc.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
								xUPfOzcWgdKBOAPrZhUUhMSQjqyz = 0;
								goto IL_01cd;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void nxjHUJFGXkfvTmRKKUTSsAVHUbEM()
					{
						yOvXYMevWFiExglJIHPaOdYgmoRP = -1;
						if (ABUjVjeOSVxWGJgdckNltUQBJdwUA != null)
						{
							ABUjVjeOSVxWGJgdckNltUQBJdwUA.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
					{
						ZGCZROVqXgqRpPRQPVRwGoxetJzx zGCZROVqXgqRpPRQPVRwGoxetJzx;
						if (yOvXYMevWFiExglJIHPaOdYgmoRP == -2 && VcZsfBNxRbPmLoPSnqMYeLOPkyrb == Environment.CurrentManagedThreadId)
						{
							yOvXYMevWFiExglJIHPaOdYgmoRP = 0;
							zGCZROVqXgqRpPRQPVRwGoxetJzx = this;
						}
						else
						{
							zGCZROVqXgqRpPRQPVRwGoxetJzx = new ZGCZROVqXgqRpPRQPVRwGoxetJzx(0);
							zGCZROVqXgqRpPRQPVRwGoxetJzx.JTaMnchNBVBgBBoHrSsuKHTeBiRH = JTaMnchNBVBgBBoHrSsuKHTeBiRH;
						}
						zGCZROVqXgqRpPRQPVRwGoxetJzx.BrLdNnmkfgFUqPQFaSbNwuadFpkI = rVGnozjdgIPMXJNxRGegaEQqEHYh;
						zGCZROVqXgqRpPRQPVRwGoxetJzx.hSxYsOVOKNTPNJPwUHsaTgtOAVIDA = NZfirYICFAjEfUHeInKsSZTFVeZL;
						return zGCZROVqXgqRpPRQPVRwGoxetJzx;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
					}
				}

				private sealed class WveEXWkrtuRurBcnNjfjwuimhjPIA : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
				{
					private int nVsklDoIxfoTTWrkgdXwblAiWdDS;

					private ActionElementMap ADkhXffXOMcFZhNBbGtxjnHJPELOA;

					private int qKrcnXhdPqNHVYWDiTCbFAFIenfZA;

					public MapHelper PwAwMPagJLLSzcRnOxdywAzGBKyL;

					private int LYvBOsbKCveqaXUjOQvrqzzrLOcrA;

					public int LfMPccdFQitFrXNTMezSeKmGEjQmA;

					private bool RYzzsxffllOaPxovdXfKFDQzbOse;

					public bool fOtBzkmZLTRfYwXgoPunxeSdsUOL;

					private int owslnIAhLBDcqzoLzbzbtWfQaBOhA;

					private int fuTaQbaOooDYrSSyvMVLhQnbZuKfA;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol yGMRSajhlpMNRshoNlYtxjEkGpUR;

					private int bAaojUMZkwMEPrUiqGRAekyUWQif;

					private int BufxfEMnEQOBWQozrSFyCzkvldlr;

					private vHszkbCJdDAIcILHhpVCxcZlIBxlA NKjitKyRzfiGwotmJqvuNCUFFnliA;

					private int QYNDbDbxzZsNyhexfLEZxYUIcgEb;

					private int byiPSffLiiWFOMHbcfnQinjjitcjA;

					private IEnumerator<ActionElementMap> HqQdFqDubhOfNeCAOnuzdwTHzJRgA;

					ActionElementMap IEnumerator<ActionElementMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return ADkhXffXOMcFZhNBbGtxjnHJPELOA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return ADkhXffXOMcFZhNBbGtxjnHJPELOA;
						}
					}

					[DebuggerHidden]
					public WveEXWkrtuRurBcnNjfjwuimhjPIA(int P_0)
					{
						nVsklDoIxfoTTWrkgdXwblAiWdDS = P_0;
						qKrcnXhdPqNHVYWDiTCbFAFIenfZA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = nVsklDoIxfoTTWrkgdXwblAiWdDS;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								IYRLmpOvHAUPspkUvqaxVrBpQUKW();
							}
						}
						yGMRSajhlpMNRshoNlYtxjEkGpUR = null;
						NKjitKyRzfiGwotmJqvuNCUFFnliA = null;
						HqQdFqDubhOfNeCAOnuzdwTHzJRgA = null;
						nVsklDoIxfoTTWrkgdXwblAiWdDS = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = nVsklDoIxfoTTWrkgdXwblAiWdDS;
							MapHelper pwAwMPagJLLSzcRnOxdywAzGBKyL = PwAwMPagJLLSzcRnOxdywAzGBKyL;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								nVsklDoIxfoTTWrkgdXwblAiWdDS = -3;
								goto IL_016c;
							}
							nVsklDoIxfoTTWrkgdXwblAiWdDS = -1;
							if (ReInput._id != pwAwMPagJLLSzcRnOxdywAzGBKyL.MUfpEiqygGgMiNQDuSWClpiuBuic)
							{
								ReInput.CheckInitialized(pwAwMPagJLLSzcRnOxdywAzGBKyL.MUfpEiqygGgMiNQDuSWClpiuBuic);
								return false;
							}
							if (LYvBOsbKCveqaXUjOQvrqzzrLOcrA < 0)
							{
								return false;
							}
							owslnIAhLBDcqzoLzbzbtWfQaBOhA = pwAwMPagJLLSzcRnOxdywAzGBKyL.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
							fuTaQbaOooDYrSSyvMVLhQnbZuKfA = 0;
							goto IL_01ec;
							IL_016c:
							if (HqQdFqDubhOfNeCAOnuzdwTHzJRgA.MoveNext())
							{
								ActionElementMap current = HqQdFqDubhOfNeCAOnuzdwTHzJRgA.Current;
								ADkhXffXOMcFZhNBbGtxjnHJPELOA = current;
								nVsklDoIxfoTTWrkgdXwblAiWdDS = 1;
								return true;
							}
							IYRLmpOvHAUPspkUvqaxVrBpQUKW();
							HqQdFqDubhOfNeCAOnuzdwTHzJRgA = null;
							goto IL_0186;
							IL_0186:
							byiPSffLiiWFOMHbcfnQinjjitcjA++;
							goto IL_0198;
							IL_01c2:
							if (BufxfEMnEQOBWQozrSFyCzkvldlr < bAaojUMZkwMEPrUiqGRAekyUWQif)
							{
								NKjitKyRzfiGwotmJqvuNCUFFnliA = yGMRSajhlpMNRshoNlYtxjEkGpUR.EfgDlYQxDuklXErnyCYTFBkkEVmX(BufxfEMnEQOBWQozrSFyCzkvldlr).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
								QYNDbDbxzZsNyhexfLEZxYUIcgEb = NKjitKyRzfiGwotmJqvuNCUFFnliA.spuxbZMpjzXXEeAzzgWchppYZEErA;
								byiPSffLiiWFOMHbcfnQinjjitcjA = 0;
								goto IL_0198;
							}
							yGMRSajhlpMNRshoNlYtxjEkGpUR = null;
							fuTaQbaOooDYrSSyvMVLhQnbZuKfA++;
							goto IL_01ec;
							IL_0198:
							if (byiPSffLiiWFOMHbcfnQinjjitcjA < QYNDbDbxzZsNyhexfLEZxYUIcgEb)
							{
								ControllerMap controllerMap = NKjitKyRzfiGwotmJqvuNCUFFnliA.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(byiPSffLiiWFOMHbcfnQinjjitcjA);
								if ((!RYzzsxffllOaPxovdXfKFDQzbOse || controllerMap.enabled) && controllerMap.ContainsAction(LYvBOsbKCveqaXUjOQvrqzzrLOcrA))
								{
									HqQdFqDubhOfNeCAOnuzdwTHzJRgA = controllerMap.ButtonMapsWithAction(LYvBOsbKCveqaXUjOQvrqzzrLOcrA, RYzzsxffllOaPxovdXfKFDQzbOse).GetEnumerator();
									nVsklDoIxfoTTWrkgdXwblAiWdDS = -3;
									goto IL_016c;
								}
								goto IL_0186;
							}
							NKjitKyRzfiGwotmJqvuNCUFFnliA = null;
							BufxfEMnEQOBWQozrSFyCzkvldlr++;
							goto IL_01c2;
							IL_01ec:
							if (fuTaQbaOooDYrSSyvMVLhQnbZuKfA < owslnIAhLBDcqzoLzbzbtWfQaBOhA)
							{
								yGMRSajhlpMNRshoNlYtxjEkGpUR = pwAwMPagJLLSzcRnOxdywAzGBKyL.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(fuTaQbaOooDYrSSyvMVLhQnbZuKfA);
								bAaojUMZkwMEPrUiqGRAekyUWQif = yGMRSajhlpMNRshoNlYtxjEkGpUR.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
								BufxfEMnEQOBWQozrSFyCzkvldlr = 0;
								goto IL_01c2;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void IYRLmpOvHAUPspkUvqaxVrBpQUKW()
					{
						nVsklDoIxfoTTWrkgdXwblAiWdDS = -1;
						if (HqQdFqDubhOfNeCAOnuzdwTHzJRgA != null)
						{
							HqQdFqDubhOfNeCAOnuzdwTHzJRgA.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
					{
						WveEXWkrtuRurBcnNjfjwuimhjPIA wveEXWkrtuRurBcnNjfjwuimhjPIA;
						if (nVsklDoIxfoTTWrkgdXwblAiWdDS == -2 && qKrcnXhdPqNHVYWDiTCbFAFIenfZA == Environment.CurrentManagedThreadId)
						{
							nVsklDoIxfoTTWrkgdXwblAiWdDS = 0;
							wveEXWkrtuRurBcnNjfjwuimhjPIA = this;
						}
						else
						{
							wveEXWkrtuRurBcnNjfjwuimhjPIA = new WveEXWkrtuRurBcnNjfjwuimhjPIA(0);
							wveEXWkrtuRurBcnNjfjwuimhjPIA.PwAwMPagJLLSzcRnOxdywAzGBKyL = PwAwMPagJLLSzcRnOxdywAzGBKyL;
						}
						wveEXWkrtuRurBcnNjfjwuimhjPIA.LYvBOsbKCveqaXUjOQvrqzzrLOcrA = LfMPccdFQitFrXNTMezSeKmGEjQmA;
						wveEXWkrtuRurBcnNjfjwuimhjPIA.RYzzsxffllOaPxovdXfKFDQzbOse = fOtBzkmZLTRfYwXgoPunxeSdsUOL;
						return wveEXWkrtuRurBcnNjfjwuimhjPIA;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
					}
				}

				private sealed class JldUPVCDlMTiKCYQUUvYxvRSwomI : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
				{
					private int eVSPImBKXeumTHdOrwpoiZeFhnqP;

					private ActionElementMap ttGFDnFRgUGLDAwJHqzMxxBrmARqc;

					private int UhLbGCpmzhWejSLDIRdedWHdFLin;

					private int buWssDtlfXPplivTrLwGAOPTAknQ;

					public int vqZvdSXeZrOcPXXNTckmlevfRnRW;

					public MapHelper YwfPjnPQVSQeWXAJrfpaMPbtGWjU;

					private ControllerType jsvYvbieBvQByXsJMLczIePjxuoX;

					public ControllerType gpUJaCNkvsLZTXmVbeIrmgSaGtQz;

					private bool EriJbxwWoglfHKBpiOoTBdgDAiXF;

					public bool RIvfwGrWINsWpfqRCxxVyaHsCsZBA;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol TZtsijAyWtpNUcmJSDyDWWBMVQWE;

					private int qumsyrMcBgIiqcOwFZjTTvxrMmUD;

					private IList<ControllerMap> hBHDUHlwyNaRoJonpWoaClJzlCyf;

					private int MZjOoMdbHiyRqFmUXwXLXToJSikq;

					private IEnumerator<ActionElementMap> NsMNsCWPqhGuQJrfxTZSXIYCVmUK;

					ActionElementMap IEnumerator<ActionElementMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return ttGFDnFRgUGLDAwJHqzMxxBrmARqc;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return ttGFDnFRgUGLDAwJHqzMxxBrmARqc;
						}
					}

					[DebuggerHidden]
					public JldUPVCDlMTiKCYQUUvYxvRSwomI(int P_0)
					{
						eVSPImBKXeumTHdOrwpoiZeFhnqP = P_0;
						UhLbGCpmzhWejSLDIRdedWHdFLin = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = eVSPImBKXeumTHdOrwpoiZeFhnqP;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								pflUpVLnYkAFPiSeqdAIZhGTBGHW();
							}
						}
						TZtsijAyWtpNUcmJSDyDWWBMVQWE = null;
						hBHDUHlwyNaRoJonpWoaClJzlCyf = null;
						NsMNsCWPqhGuQJrfxTZSXIYCVmUK = null;
						eVSPImBKXeumTHdOrwpoiZeFhnqP = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = eVSPImBKXeumTHdOrwpoiZeFhnqP;
							MapHelper ywfPjnPQVSQeWXAJrfpaMPbtGWjU = YwfPjnPQVSQeWXAJrfpaMPbtGWjU;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								eVSPImBKXeumTHdOrwpoiZeFhnqP = -3;
								goto IL_0150;
							}
							eVSPImBKXeumTHdOrwpoiZeFhnqP = -1;
							if (buWssDtlfXPplivTrLwGAOPTAknQ < 0)
							{
								return false;
							}
							TZtsijAyWtpNUcmJSDyDWWBMVQWE = ywfPjnPQVSQeWXAJrfpaMPbtGWjU.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(jsvYvbieBvQByXsJMLczIePjxuoX);
							qumsyrMcBgIiqcOwFZjTTvxrMmUD = 0;
							goto IL_01ab;
							IL_0150:
							if (NsMNsCWPqhGuQJrfxTZSXIYCVmUK.MoveNext())
							{
								ActionElementMap current = NsMNsCWPqhGuQJrfxTZSXIYCVmUK.Current;
								ttGFDnFRgUGLDAwJHqzMxxBrmARqc = current;
								eVSPImBKXeumTHdOrwpoiZeFhnqP = 1;
								return true;
							}
							pflUpVLnYkAFPiSeqdAIZhGTBGHW();
							NsMNsCWPqhGuQJrfxTZSXIYCVmUK = null;
							goto IL_016a;
							IL_017c:
							if (MZjOoMdbHiyRqFmUXwXLXToJSikq < hBHDUHlwyNaRoJonpWoaClJzlCyf.Count)
							{
								if (!(hBHDUHlwyNaRoJonpWoaClJzlCyf[MZjOoMdbHiyRqFmUXwXLXToJSikq] is ControllerMapWithAxes))
								{
									return false;
								}
								if ((!EriJbxwWoglfHKBpiOoTBdgDAiXF || hBHDUHlwyNaRoJonpWoaClJzlCyf[MZjOoMdbHiyRqFmUXwXLXToJSikq].enabled) && hBHDUHlwyNaRoJonpWoaClJzlCyf[MZjOoMdbHiyRqFmUXwXLXToJSikq].ContainsAction(buWssDtlfXPplivTrLwGAOPTAknQ))
								{
									NsMNsCWPqhGuQJrfxTZSXIYCVmUK = (hBHDUHlwyNaRoJonpWoaClJzlCyf[MZjOoMdbHiyRqFmUXwXLXToJSikq] as ControllerMapWithAxes).AxisMapsWithAction(buWssDtlfXPplivTrLwGAOPTAknQ, EriJbxwWoglfHKBpiOoTBdgDAiXF).GetEnumerator();
									eVSPImBKXeumTHdOrwpoiZeFhnqP = -3;
									goto IL_0150;
								}
								goto IL_016a;
							}
							hBHDUHlwyNaRoJonpWoaClJzlCyf = null;
							qumsyrMcBgIiqcOwFZjTTvxrMmUD++;
							goto IL_01ab;
							IL_016a:
							MZjOoMdbHiyRqFmUXwXLXToJSikq++;
							goto IL_017c;
							IL_01ab:
							if (qumsyrMcBgIiqcOwFZjTTvxrMmUD < TZtsijAyWtpNUcmJSDyDWWBMVQWE.hdHAWgwPJNkiyeCCaiyVDbScMIAib)
							{
								hBHDUHlwyNaRoJonpWoaClJzlCyf = TZtsijAyWtpNUcmJSDyDWWBMVQWE.EfgDlYQxDuklXErnyCYTFBkkEVmX(qumsyrMcBgIiqcOwFZjTTvxrMmUD).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
								MZjOoMdbHiyRqFmUXwXLXToJSikq = 0;
								goto IL_017c;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void pflUpVLnYkAFPiSeqdAIZhGTBGHW()
					{
						eVSPImBKXeumTHdOrwpoiZeFhnqP = -1;
						if (NsMNsCWPqhGuQJrfxTZSXIYCVmUK != null)
						{
							NsMNsCWPqhGuQJrfxTZSXIYCVmUK.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
					{
						JldUPVCDlMTiKCYQUUvYxvRSwomI jldUPVCDlMTiKCYQUUvYxvRSwomI;
						if (eVSPImBKXeumTHdOrwpoiZeFhnqP == -2 && UhLbGCpmzhWejSLDIRdedWHdFLin == Environment.CurrentManagedThreadId)
						{
							eVSPImBKXeumTHdOrwpoiZeFhnqP = 0;
							jldUPVCDlMTiKCYQUUvYxvRSwomI = this;
						}
						else
						{
							jldUPVCDlMTiKCYQUUvYxvRSwomI = new JldUPVCDlMTiKCYQUUvYxvRSwomI(0);
							jldUPVCDlMTiKCYQUUvYxvRSwomI.YwfPjnPQVSQeWXAJrfpaMPbtGWjU = YwfPjnPQVSQeWXAJrfpaMPbtGWjU;
						}
						jldUPVCDlMTiKCYQUUvYxvRSwomI.jsvYvbieBvQByXsJMLczIePjxuoX = gpUJaCNkvsLZTXmVbeIrmgSaGtQz;
						jldUPVCDlMTiKCYQUUvYxvRSwomI.buWssDtlfXPplivTrLwGAOPTAknQ = vqZvdSXeZrOcPXXNTckmlevfRnRW;
						jldUPVCDlMTiKCYQUUvYxvRSwomI.EriJbxwWoglfHKBpiOoTBdgDAiXF = RIvfwGrWINsWpfqRCxxVyaHsCsZBA;
						return jldUPVCDlMTiKCYQUUvYxvRSwomI;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
					}
				}

				private sealed class BLKXTkwDeHSSxKfvFemOBIooEvqUA : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
				{
					private int zbGzoNLgYIALpEodFeNhcNhRoobdA;

					private ActionElementMap WxjlVOSqrswtecmiyUzHiOiGbnkl;

					private int osuHFywafvfXUnTZDZBvBhESylyK;

					private int QJGjAEYulTqPGOAdLTNcKfhPfZBP;

					public int TrbalfsvoLxkTCeaZlQDDLdAelJU;

					public MapHelper iejmNJcBCmEZCeXhgnlQPJlWBHJmA;

					private ControllerType zzUEDVusOQMlMjpnFBvHygsHVcfn;

					public ControllerType WwmPFmZgsWHdeqrEbwDTshIHBiMB;

					private int gxqJCjkUNIIDwbAVzIJHfKzkMLhl;

					public int xmZwZBhQTJnZSJivDenYOcXkGhWd;

					private bool rZzLFerIVLXoTLUlpwnEoBGlUesu;

					public bool OqZODjaQPScYKpbuXdJNWHnWRbln;

					private IList<ControllerMap> lWseeVMUVMowKEZmPgIAYayQfdfW;

					private int nNkEGMELMRFELNCukfYmQKFVuYxY;

					private IEnumerator<ActionElementMap> ruhNVYfTrHCexEAcXinJQZQWdnPv;

					ActionElementMap IEnumerator<ActionElementMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return WxjlVOSqrswtecmiyUzHiOiGbnkl;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return WxjlVOSqrswtecmiyUzHiOiGbnkl;
						}
					}

					[DebuggerHidden]
					public BLKXTkwDeHSSxKfvFemOBIooEvqUA(int P_0)
					{
						zbGzoNLgYIALpEodFeNhcNhRoobdA = P_0;
						osuHFywafvfXUnTZDZBvBhESylyK = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = zbGzoNLgYIALpEodFeNhcNhRoobdA;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								gFxvnpbEeBhijtrCmWQOHhfYUikR();
							}
						}
						lWseeVMUVMowKEZmPgIAYayQfdfW = null;
						ruhNVYfTrHCexEAcXinJQZQWdnPv = null;
						zbGzoNLgYIALpEodFeNhcNhRoobdA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = zbGzoNLgYIALpEodFeNhcNhRoobdA;
							MapHelper mapHelper = iejmNJcBCmEZCeXhgnlQPJlWBHJmA;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								zbGzoNLgYIALpEodFeNhcNhRoobdA = -3;
								goto IL_014f;
							}
							zbGzoNLgYIALpEodFeNhcNhRoobdA = -1;
							if (QJGjAEYulTqPGOAdLTNcKfhPfZBP < 0)
							{
								return false;
							}
							rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(zzUEDVusOQMlMjpnFBvHygsHVcfn);
							int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(gxqJCjkUNIIDwbAVzIJHfKzkMLhl);
							if (num2 < 0)
							{
								return false;
							}
							lWseeVMUVMowKEZmPgIAYayQfdfW = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num2).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
							nNkEGMELMRFELNCukfYmQKFVuYxY = 0;
							goto IL_017b;
							IL_014f:
							if (ruhNVYfTrHCexEAcXinJQZQWdnPv.MoveNext())
							{
								ActionElementMap current = ruhNVYfTrHCexEAcXinJQZQWdnPv.Current;
								WxjlVOSqrswtecmiyUzHiOiGbnkl = current;
								zbGzoNLgYIALpEodFeNhcNhRoobdA = 1;
								return true;
							}
							gFxvnpbEeBhijtrCmWQOHhfYUikR();
							ruhNVYfTrHCexEAcXinJQZQWdnPv = null;
							goto IL_0169;
							IL_017b:
							if (nNkEGMELMRFELNCukfYmQKFVuYxY < lWseeVMUVMowKEZmPgIAYayQfdfW.Count)
							{
								if (!(lWseeVMUVMowKEZmPgIAYayQfdfW[nNkEGMELMRFELNCukfYmQKFVuYxY] is ControllerMapWithAxes))
								{
									return false;
								}
								if ((!rZzLFerIVLXoTLUlpwnEoBGlUesu || lWseeVMUVMowKEZmPgIAYayQfdfW[nNkEGMELMRFELNCukfYmQKFVuYxY].enabled) && lWseeVMUVMowKEZmPgIAYayQfdfW[nNkEGMELMRFELNCukfYmQKFVuYxY].ContainsAction(QJGjAEYulTqPGOAdLTNcKfhPfZBP))
								{
									ruhNVYfTrHCexEAcXinJQZQWdnPv = (lWseeVMUVMowKEZmPgIAYayQfdfW[nNkEGMELMRFELNCukfYmQKFVuYxY] as ControllerMapWithAxes).AxisMapsWithAction(QJGjAEYulTqPGOAdLTNcKfhPfZBP, rZzLFerIVLXoTLUlpwnEoBGlUesu).GetEnumerator();
									zbGzoNLgYIALpEodFeNhcNhRoobdA = -3;
									goto IL_014f;
								}
								goto IL_0169;
							}
							return false;
							IL_0169:
							nNkEGMELMRFELNCukfYmQKFVuYxY++;
							goto IL_017b;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void gFxvnpbEeBhijtrCmWQOHhfYUikR()
					{
						zbGzoNLgYIALpEodFeNhcNhRoobdA = -1;
						if (ruhNVYfTrHCexEAcXinJQZQWdnPv != null)
						{
							ruhNVYfTrHCexEAcXinJQZQWdnPv.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
					{
						BLKXTkwDeHSSxKfvFemOBIooEvqUA bLKXTkwDeHSSxKfvFemOBIooEvqUA;
						if (zbGzoNLgYIALpEodFeNhcNhRoobdA == -2 && osuHFywafvfXUnTZDZBvBhESylyK == Environment.CurrentManagedThreadId)
						{
							zbGzoNLgYIALpEodFeNhcNhRoobdA = 0;
							bLKXTkwDeHSSxKfvFemOBIooEvqUA = this;
						}
						else
						{
							bLKXTkwDeHSSxKfvFemOBIooEvqUA = new BLKXTkwDeHSSxKfvFemOBIooEvqUA(0);
							bLKXTkwDeHSSxKfvFemOBIooEvqUA.iejmNJcBCmEZCeXhgnlQPJlWBHJmA = iejmNJcBCmEZCeXhgnlQPJlWBHJmA;
						}
						bLKXTkwDeHSSxKfvFemOBIooEvqUA.zzUEDVusOQMlMjpnFBvHygsHVcfn = WwmPFmZgsWHdeqrEbwDTshIHBiMB;
						bLKXTkwDeHSSxKfvFemOBIooEvqUA.gxqJCjkUNIIDwbAVzIJHfKzkMLhl = xmZwZBhQTJnZSJivDenYOcXkGhWd;
						bLKXTkwDeHSSxKfvFemOBIooEvqUA.QJGjAEYulTqPGOAdLTNcKfhPfZBP = TrbalfsvoLxkTCeaZlQDDLdAelJU;
						bLKXTkwDeHSSxKfvFemOBIooEvqUA.rZzLFerIVLXoTLUlpwnEoBGlUesu = OqZODjaQPScYKpbuXdJNWHnWRbln;
						return bLKXTkwDeHSSxKfvFemOBIooEvqUA;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
					}
				}

				private sealed class SkFpOgciCsVWqHnXmXTnAlBRGqhL : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
				{
					private int sywNKztGIBLBSwSiTiluhFerzPfq;

					private ActionElementMap SQTmUWAcvWPMImMMdQMUuKTsfxZP;

					private int oSgOhVPJptLDWwaScHplxhWsGPUg;

					private int zxdwamKtCKFDLsZdIuWjytnlgyQg;

					public int BaydhJTiRuuDNLmlANptKMwLUmeS;

					public MapHelper nTmTcZMXlTpCNhdQyeEjCrqErfhU;

					private ControllerType ViPHeVwKzOdPMeMRakSCvMBLZkvaA;

					public ControllerType yMZgMLEpiVfQLovhAHmzpfvlzLTb;

					private bool zdPAUBIjBHCqRdsUIkAPIhVhJRcgB;

					public bool rGJFpbfUOhuPhWdiqLfurpqkfhzP;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol kMXzNTBmAICOvcdGYgCVkXkCrfRO;

					private int jyoAYrEveCbPUpMtQplzbXGflxkGb;

					private IList<ControllerMap> hPyWqfaBUPzefQlsmmNcwBurfzXfA;

					private int uNIuSAcTpggZWWSaYEhGDrtfWkKF;

					private IEnumerator<ActionElementMap> KlPOPhsrZivJTsJpxbHJwdYlBPto;

					ActionElementMap IEnumerator<ActionElementMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return SQTmUWAcvWPMImMMdQMUuKTsfxZP;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return SQTmUWAcvWPMImMMdQMUuKTsfxZP;
						}
					}

					[DebuggerHidden]
					public SkFpOgciCsVWqHnXmXTnAlBRGqhL(int P_0)
					{
						sywNKztGIBLBSwSiTiluhFerzPfq = P_0;
						oSgOhVPJptLDWwaScHplxhWsGPUg = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = sywNKztGIBLBSwSiTiluhFerzPfq;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								KddkLoOtbbgqBLbhDbPnXwPdpVzU();
							}
						}
						kMXzNTBmAICOvcdGYgCVkXkCrfRO = null;
						hPyWqfaBUPzefQlsmmNcwBurfzXfA = null;
						KlPOPhsrZivJTsJpxbHJwdYlBPto = null;
						sywNKztGIBLBSwSiTiluhFerzPfq = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = sywNKztGIBLBSwSiTiluhFerzPfq;
							MapHelper mapHelper = nTmTcZMXlTpCNhdQyeEjCrqErfhU;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								sywNKztGIBLBSwSiTiluhFerzPfq = -3;
								goto IL_012c;
							}
							sywNKztGIBLBSwSiTiluhFerzPfq = -1;
							if (zxdwamKtCKFDLsZdIuWjytnlgyQg < 0)
							{
								return false;
							}
							kMXzNTBmAICOvcdGYgCVkXkCrfRO = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ViPHeVwKzOdPMeMRakSCvMBLZkvaA);
							jyoAYrEveCbPUpMtQplzbXGflxkGb = 0;
							goto IL_0187;
							IL_012c:
							if (KlPOPhsrZivJTsJpxbHJwdYlBPto.MoveNext())
							{
								ActionElementMap current = KlPOPhsrZivJTsJpxbHJwdYlBPto.Current;
								SQTmUWAcvWPMImMMdQMUuKTsfxZP = current;
								sywNKztGIBLBSwSiTiluhFerzPfq = 1;
								return true;
							}
							KddkLoOtbbgqBLbhDbPnXwPdpVzU();
							KlPOPhsrZivJTsJpxbHJwdYlBPto = null;
							goto IL_0146;
							IL_0158:
							if (uNIuSAcTpggZWWSaYEhGDrtfWkKF < hPyWqfaBUPzefQlsmmNcwBurfzXfA.Count)
							{
								if ((!zdPAUBIjBHCqRdsUIkAPIhVhJRcgB || hPyWqfaBUPzefQlsmmNcwBurfzXfA[uNIuSAcTpggZWWSaYEhGDrtfWkKF].enabled) && hPyWqfaBUPzefQlsmmNcwBurfzXfA[uNIuSAcTpggZWWSaYEhGDrtfWkKF].ContainsAction(zxdwamKtCKFDLsZdIuWjytnlgyQg))
								{
									KlPOPhsrZivJTsJpxbHJwdYlBPto = hPyWqfaBUPzefQlsmmNcwBurfzXfA[uNIuSAcTpggZWWSaYEhGDrtfWkKF].ButtonMapsWithAction(zxdwamKtCKFDLsZdIuWjytnlgyQg, zdPAUBIjBHCqRdsUIkAPIhVhJRcgB).GetEnumerator();
									sywNKztGIBLBSwSiTiluhFerzPfq = -3;
									goto IL_012c;
								}
								goto IL_0146;
							}
							hPyWqfaBUPzefQlsmmNcwBurfzXfA = null;
							jyoAYrEveCbPUpMtQplzbXGflxkGb++;
							goto IL_0187;
							IL_0146:
							uNIuSAcTpggZWWSaYEhGDrtfWkKF++;
							goto IL_0158;
							IL_0187:
							if (jyoAYrEveCbPUpMtQplzbXGflxkGb < kMXzNTBmAICOvcdGYgCVkXkCrfRO.hdHAWgwPJNkiyeCCaiyVDbScMIAib)
							{
								hPyWqfaBUPzefQlsmmNcwBurfzXfA = kMXzNTBmAICOvcdGYgCVkXkCrfRO.EfgDlYQxDuklXErnyCYTFBkkEVmX(jyoAYrEveCbPUpMtQplzbXGflxkGb).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
								uNIuSAcTpggZWWSaYEhGDrtfWkKF = 0;
								goto IL_0158;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void KddkLoOtbbgqBLbhDbPnXwPdpVzU()
					{
						sywNKztGIBLBSwSiTiluhFerzPfq = -1;
						if (KlPOPhsrZivJTsJpxbHJwdYlBPto != null)
						{
							KlPOPhsrZivJTsJpxbHJwdYlBPto.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
					{
						SkFpOgciCsVWqHnXmXTnAlBRGqhL skFpOgciCsVWqHnXmXTnAlBRGqhL;
						if (sywNKztGIBLBSwSiTiluhFerzPfq == -2 && oSgOhVPJptLDWwaScHplxhWsGPUg == Environment.CurrentManagedThreadId)
						{
							sywNKztGIBLBSwSiTiluhFerzPfq = 0;
							skFpOgciCsVWqHnXmXTnAlBRGqhL = this;
						}
						else
						{
							skFpOgciCsVWqHnXmXTnAlBRGqhL = new SkFpOgciCsVWqHnXmXTnAlBRGqhL(0);
							skFpOgciCsVWqHnXmXTnAlBRGqhL.nTmTcZMXlTpCNhdQyeEjCrqErfhU = nTmTcZMXlTpCNhdQyeEjCrqErfhU;
						}
						skFpOgciCsVWqHnXmXTnAlBRGqhL.ViPHeVwKzOdPMeMRakSCvMBLZkvaA = yMZgMLEpiVfQLovhAHmzpfvlzLTb;
						skFpOgciCsVWqHnXmXTnAlBRGqhL.zxdwamKtCKFDLsZdIuWjytnlgyQg = BaydhJTiRuuDNLmlANptKMwLUmeS;
						skFpOgciCsVWqHnXmXTnAlBRGqhL.zdPAUBIjBHCqRdsUIkAPIhVhJRcgB = rGJFpbfUOhuPhWdiqLfurpqkfhzP;
						return skFpOgciCsVWqHnXmXTnAlBRGqhL;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
					}
				}

				private sealed class zkEKkSbmgisgTJXFlpIZQOAjWTyB : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
				{
					private int zJYOKqdTHphCruVBVocXhnpWibLx;

					private ActionElementMap omZgnbKQElPVCicrHjGTJuYwBayEb;

					private int HucgFSAZHtlXzCsPTyKWhbXONPjwA;

					private int YYcHAuHkzkAcKDbxWhpuMznkStrB;

					public int RTxfqScQuvMSbASoolhqwhpdDdXx;

					public MapHelper kwjlrkdFDqDomvpeFROBdKwUzvcV;

					private ControllerType wjiHvEmbbLYMeoaHNcyDuQpIYbhG;

					public ControllerType rAcRQDAbzBltJGCbvYGAZSfqBQFl;

					private int OdxiIovdybIPiGLzDstAgfLnCrDO;

					public int ijusRhQqZvsIPVlPfYjZVOEGAVFHA;

					private bool khkHHIpXbHJsQgMAKztIODtwjTsr;

					public bool fEjjfzXJHwvPpsILNPGijhekNdgx;

					private IList<ControllerMap> oSmXPrKIYdRYmbPHjfiVweonscPy;

					private int rUfGFHdMctuzDcdxqwPykSxiAowDA;

					private IEnumerator<ActionElementMap> qeobDmhTTJCHDWUtXmolwiSUBpoDA;

					ActionElementMap IEnumerator<ActionElementMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return omZgnbKQElPVCicrHjGTJuYwBayEb;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return omZgnbKQElPVCicrHjGTJuYwBayEb;
						}
					}

					[DebuggerHidden]
					public zkEKkSbmgisgTJXFlpIZQOAjWTyB(int P_0)
					{
						zJYOKqdTHphCruVBVocXhnpWibLx = P_0;
						HucgFSAZHtlXzCsPTyKWhbXONPjwA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = zJYOKqdTHphCruVBVocXhnpWibLx;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								XZfgqlfFdgKVTpefiHapxjRPNeNiA();
							}
						}
						oSmXPrKIYdRYmbPHjfiVweonscPy = null;
						qeobDmhTTJCHDWUtXmolwiSUBpoDA = null;
						zJYOKqdTHphCruVBVocXhnpWibLx = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = zJYOKqdTHphCruVBVocXhnpWibLx;
							MapHelper mapHelper = kwjlrkdFDqDomvpeFROBdKwUzvcV;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								zJYOKqdTHphCruVBVocXhnpWibLx = -3;
								goto IL_012b;
							}
							zJYOKqdTHphCruVBVocXhnpWibLx = -1;
							if (YYcHAuHkzkAcKDbxWhpuMznkStrB < 0)
							{
								return false;
							}
							rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(wjiHvEmbbLYMeoaHNcyDuQpIYbhG);
							int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(OdxiIovdybIPiGLzDstAgfLnCrDO);
							if (num2 < 0)
							{
								return false;
							}
							oSmXPrKIYdRYmbPHjfiVweonscPy = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num2).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
							rUfGFHdMctuzDcdxqwPykSxiAowDA = 0;
							goto IL_0157;
							IL_012b:
							if (qeobDmhTTJCHDWUtXmolwiSUBpoDA.MoveNext())
							{
								ActionElementMap current = qeobDmhTTJCHDWUtXmolwiSUBpoDA.Current;
								omZgnbKQElPVCicrHjGTJuYwBayEb = current;
								zJYOKqdTHphCruVBVocXhnpWibLx = 1;
								return true;
							}
							XZfgqlfFdgKVTpefiHapxjRPNeNiA();
							qeobDmhTTJCHDWUtXmolwiSUBpoDA = null;
							goto IL_0145;
							IL_0157:
							if (rUfGFHdMctuzDcdxqwPykSxiAowDA < oSmXPrKIYdRYmbPHjfiVweonscPy.Count)
							{
								if ((!khkHHIpXbHJsQgMAKztIODtwjTsr || oSmXPrKIYdRYmbPHjfiVweonscPy[rUfGFHdMctuzDcdxqwPykSxiAowDA].enabled) && oSmXPrKIYdRYmbPHjfiVweonscPy[rUfGFHdMctuzDcdxqwPykSxiAowDA].ContainsAction(YYcHAuHkzkAcKDbxWhpuMznkStrB))
								{
									qeobDmhTTJCHDWUtXmolwiSUBpoDA = oSmXPrKIYdRYmbPHjfiVweonscPy[rUfGFHdMctuzDcdxqwPykSxiAowDA].ButtonMapsWithAction(YYcHAuHkzkAcKDbxWhpuMznkStrB, khkHHIpXbHJsQgMAKztIODtwjTsr).GetEnumerator();
									zJYOKqdTHphCruVBVocXhnpWibLx = -3;
									goto IL_012b;
								}
								goto IL_0145;
							}
							return false;
							IL_0145:
							rUfGFHdMctuzDcdxqwPykSxiAowDA++;
							goto IL_0157;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void XZfgqlfFdgKVTpefiHapxjRPNeNiA()
					{
						zJYOKqdTHphCruVBVocXhnpWibLx = -1;
						if (qeobDmhTTJCHDWUtXmolwiSUBpoDA != null)
						{
							qeobDmhTTJCHDWUtXmolwiSUBpoDA.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
					{
						zkEKkSbmgisgTJXFlpIZQOAjWTyB zkEKkSbmgisgTJXFlpIZQOAjWTyB2;
						if (zJYOKqdTHphCruVBVocXhnpWibLx == -2 && HucgFSAZHtlXzCsPTyKWhbXONPjwA == Environment.CurrentManagedThreadId)
						{
							zJYOKqdTHphCruVBVocXhnpWibLx = 0;
							zkEKkSbmgisgTJXFlpIZQOAjWTyB2 = this;
						}
						else
						{
							zkEKkSbmgisgTJXFlpIZQOAjWTyB2 = new zkEKkSbmgisgTJXFlpIZQOAjWTyB(0);
							zkEKkSbmgisgTJXFlpIZQOAjWTyB2.kwjlrkdFDqDomvpeFROBdKwUzvcV = kwjlrkdFDqDomvpeFROBdKwUzvcV;
						}
						zkEKkSbmgisgTJXFlpIZQOAjWTyB2.wjiHvEmbbLYMeoaHNcyDuQpIYbhG = rAcRQDAbzBltJGCbvYGAZSfqBQFl;
						zkEKkSbmgisgTJXFlpIZQOAjWTyB2.OdxiIovdybIPiGLzDstAgfLnCrDO = ijusRhQqZvsIPVlPfYjZVOEGAVFHA;
						zkEKkSbmgisgTJXFlpIZQOAjWTyB2.YYcHAuHkzkAcKDbxWhpuMznkStrB = RTxfqScQuvMSbASoolhqwhpdDdXx;
						zkEKkSbmgisgTJXFlpIZQOAjWTyB2.khkHHIpXbHJsQgMAKztIODtwjTsr = fEjjfzXJHwvPpsILNPGijhekNdgx;
						return zkEKkSbmgisgTJXFlpIZQOAjWTyB2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
					}
				}

				private sealed class dgXTotYzdjpgFKeKxZpeeZuXRKUF : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
				{
					private int eQfeGHVBcELkXECGLgRqKJirzIPQA;

					private ActionElementMap AEiUNRuyvYCmiOGfdBBXDPbEeqAyA;

					private int IJprmpcIzrwTzAVOKCjwHqpWeRlhA;

					private int HBZAliJjldUTHSYaJzsfOThFDPDy;

					public int srSVfBYYtXWggITDxfkhzIYpOzdb;

					public MapHelper jUcbobiPbeJSdGSHbwCNGnupGvyy;

					private ControllerType HyTQGOmFCmrHIoOfIDvywHaNQUaE;

					public ControllerType HBBwQjBwOvhwnqhsoSKUasjqGnoW;

					private bool ByqrWvtCXUfNbougniYdEKdefvLpA;

					public bool mnaJSFrpkieQGDaIMqZjbpAcJLhIB;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol QVGlaoTIEKOEcPHPHIkbJujVjlFG;

					private int QDLOuRajcUVpYoMiDdzTafaHIPnD;

					private IList<ControllerMap> OcnHdnKPZPFvCABOmPefBlMUjPTSA;

					private int vgOgGbELfKJxalsihezSgxfgMXyy;

					private IEnumerator<ActionElementMap> iXbwqFkKCCxcBugerxuGnHZAoZNO;

					ActionElementMap IEnumerator<ActionElementMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return AEiUNRuyvYCmiOGfdBBXDPbEeqAyA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return AEiUNRuyvYCmiOGfdBBXDPbEeqAyA;
						}
					}

					[DebuggerHidden]
					public dgXTotYzdjpgFKeKxZpeeZuXRKUF(int P_0)
					{
						eQfeGHVBcELkXECGLgRqKJirzIPQA = P_0;
						IJprmpcIzrwTzAVOKCjwHqpWeRlhA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = eQfeGHVBcELkXECGLgRqKJirzIPQA;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								dmyIrJaCpaZymqcgeZJzvBnJUXfk();
							}
						}
						QVGlaoTIEKOEcPHPHIkbJujVjlFG = null;
						OcnHdnKPZPFvCABOmPefBlMUjPTSA = null;
						iXbwqFkKCCxcBugerxuGnHZAoZNO = null;
						eQfeGHVBcELkXECGLgRqKJirzIPQA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = eQfeGHVBcELkXECGLgRqKJirzIPQA;
							MapHelper mapHelper = jUcbobiPbeJSdGSHbwCNGnupGvyy;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								eQfeGHVBcELkXECGLgRqKJirzIPQA = -3;
								goto IL_012c;
							}
							eQfeGHVBcELkXECGLgRqKJirzIPQA = -1;
							if (HBZAliJjldUTHSYaJzsfOThFDPDy < 0)
							{
								return false;
							}
							QVGlaoTIEKOEcPHPHIkbJujVjlFG = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(HyTQGOmFCmrHIoOfIDvywHaNQUaE);
							QDLOuRajcUVpYoMiDdzTafaHIPnD = 0;
							goto IL_0187;
							IL_012c:
							if (iXbwqFkKCCxcBugerxuGnHZAoZNO.MoveNext())
							{
								ActionElementMap current = iXbwqFkKCCxcBugerxuGnHZAoZNO.Current;
								AEiUNRuyvYCmiOGfdBBXDPbEeqAyA = current;
								eQfeGHVBcELkXECGLgRqKJirzIPQA = 1;
								return true;
							}
							dmyIrJaCpaZymqcgeZJzvBnJUXfk();
							iXbwqFkKCCxcBugerxuGnHZAoZNO = null;
							goto IL_0146;
							IL_0158:
							if (vgOgGbELfKJxalsihezSgxfgMXyy < OcnHdnKPZPFvCABOmPefBlMUjPTSA.Count)
							{
								if ((!ByqrWvtCXUfNbougniYdEKdefvLpA || OcnHdnKPZPFvCABOmPefBlMUjPTSA[vgOgGbELfKJxalsihezSgxfgMXyy].enabled) && OcnHdnKPZPFvCABOmPefBlMUjPTSA[vgOgGbELfKJxalsihezSgxfgMXyy].ContainsAction(HBZAliJjldUTHSYaJzsfOThFDPDy))
								{
									iXbwqFkKCCxcBugerxuGnHZAoZNO = OcnHdnKPZPFvCABOmPefBlMUjPTSA[vgOgGbELfKJxalsihezSgxfgMXyy].ElementMapsWithAction(HBZAliJjldUTHSYaJzsfOThFDPDy, ByqrWvtCXUfNbougniYdEKdefvLpA).GetEnumerator();
									eQfeGHVBcELkXECGLgRqKJirzIPQA = -3;
									goto IL_012c;
								}
								goto IL_0146;
							}
							OcnHdnKPZPFvCABOmPefBlMUjPTSA = null;
							QDLOuRajcUVpYoMiDdzTafaHIPnD++;
							goto IL_0187;
							IL_0146:
							vgOgGbELfKJxalsihezSgxfgMXyy++;
							goto IL_0158;
							IL_0187:
							if (QDLOuRajcUVpYoMiDdzTafaHIPnD < QVGlaoTIEKOEcPHPHIkbJujVjlFG.hdHAWgwPJNkiyeCCaiyVDbScMIAib)
							{
								OcnHdnKPZPFvCABOmPefBlMUjPTSA = QVGlaoTIEKOEcPHPHIkbJujVjlFG.EfgDlYQxDuklXErnyCYTFBkkEVmX(QDLOuRajcUVpYoMiDdzTafaHIPnD).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
								vgOgGbELfKJxalsihezSgxfgMXyy = 0;
								goto IL_0158;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void dmyIrJaCpaZymqcgeZJzvBnJUXfk()
					{
						eQfeGHVBcELkXECGLgRqKJirzIPQA = -1;
						if (iXbwqFkKCCxcBugerxuGnHZAoZNO != null)
						{
							iXbwqFkKCCxcBugerxuGnHZAoZNO.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
					{
						dgXTotYzdjpgFKeKxZpeeZuXRKUF dgXTotYzdjpgFKeKxZpeeZuXRKUF2;
						if (eQfeGHVBcELkXECGLgRqKJirzIPQA == -2 && IJprmpcIzrwTzAVOKCjwHqpWeRlhA == Environment.CurrentManagedThreadId)
						{
							eQfeGHVBcELkXECGLgRqKJirzIPQA = 0;
							dgXTotYzdjpgFKeKxZpeeZuXRKUF2 = this;
						}
						else
						{
							dgXTotYzdjpgFKeKxZpeeZuXRKUF2 = new dgXTotYzdjpgFKeKxZpeeZuXRKUF(0);
							dgXTotYzdjpgFKeKxZpeeZuXRKUF2.jUcbobiPbeJSdGSHbwCNGnupGvyy = jUcbobiPbeJSdGSHbwCNGnupGvyy;
						}
						dgXTotYzdjpgFKeKxZpeeZuXRKUF2.HyTQGOmFCmrHIoOfIDvywHaNQUaE = HBBwQjBwOvhwnqhsoSKUasjqGnoW;
						dgXTotYzdjpgFKeKxZpeeZuXRKUF2.HBZAliJjldUTHSYaJzsfOThFDPDy = srSVfBYYtXWggITDxfkhzIYpOzdb;
						dgXTotYzdjpgFKeKxZpeeZuXRKUF2.ByqrWvtCXUfNbougniYdEKdefvLpA = mnaJSFrpkieQGDaIMqZjbpAcJLhIB;
						return dgXTotYzdjpgFKeKxZpeeZuXRKUF2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
					}
				}

				private sealed class OGCeldLwcwvXcFXOhXYIHpXJQDaj : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
				{
					private int RZrgYqfLVDiCGLYIUuNYYNyTqfFI;

					private ActionElementMap fheaVucpNTClmJZijNFkeGVdSPojA;

					private int JqRdaPsgWOIIvHdKoFfjGEeloRNS;

					private int VKyxAFPHTmYkQUPccsgEmtZFGfOo;

					public int NBGKhrMrjmBXBtuEcPuyseySctgb;

					public MapHelper rgzqAfLUTwfVDDqYlSzucuJOakqg;

					private ControllerType dCUJzIdddaBpFRdxnzPnewMtiGIu;

					public ControllerType FGwMoBPueckqLRPzPbMzhTKuSjER;

					private int vVShDdMXxptcOmdDpcwlaibHdXwhA;

					public int kslDCAgzYYTdwoPWlBUlvncSwWjX;

					private bool DJgYoRCCPNxOPrIetfxXAOiiqGBhA;

					public bool gVXQISOmJseLkfxptiHechnnNMYp;

					private IList<ControllerMap> vjMLKUHJZaAXXkuVRTVSHOuSDpxk;

					private int srbhVBYqnlIdTeKcQbbGPTruEtDBb;

					private IEnumerator<ActionElementMap> ZhIDCrDprMAagoVuNqDCkjAcMNKGb;

					ActionElementMap IEnumerator<ActionElementMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return fheaVucpNTClmJZijNFkeGVdSPojA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return fheaVucpNTClmJZijNFkeGVdSPojA;
						}
					}

					[DebuggerHidden]
					public OGCeldLwcwvXcFXOhXYIHpXJQDaj(int P_0)
					{
						RZrgYqfLVDiCGLYIUuNYYNyTqfFI = P_0;
						JqRdaPsgWOIIvHdKoFfjGEeloRNS = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rZrgYqfLVDiCGLYIUuNYYNyTqfFI = RZrgYqfLVDiCGLYIUuNYYNyTqfFI;
						if (rZrgYqfLVDiCGLYIUuNYYNyTqfFI == -3 || rZrgYqfLVDiCGLYIUuNYYNyTqfFI == 1)
						{
							try
							{
							}
							finally
							{
								yIpPlPRJyLANSUXxaecljxojmsSF();
							}
						}
						vjMLKUHJZaAXXkuVRTVSHOuSDpxk = null;
						ZhIDCrDprMAagoVuNqDCkjAcMNKGb = null;
						RZrgYqfLVDiCGLYIUuNYYNyTqfFI = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int rZrgYqfLVDiCGLYIUuNYYNyTqfFI = RZrgYqfLVDiCGLYIUuNYYNyTqfFI;
							MapHelper mapHelper = rgzqAfLUTwfVDDqYlSzucuJOakqg;
							if (rZrgYqfLVDiCGLYIUuNYYNyTqfFI != 0)
							{
								if (rZrgYqfLVDiCGLYIUuNYYNyTqfFI != 1)
								{
									return false;
								}
								RZrgYqfLVDiCGLYIUuNYYNyTqfFI = -3;
								goto IL_012b;
							}
							RZrgYqfLVDiCGLYIUuNYYNyTqfFI = -1;
							if (VKyxAFPHTmYkQUPccsgEmtZFGfOo < 0)
							{
								return false;
							}
							rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(dCUJzIdddaBpFRdxnzPnewMtiGIu);
							int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(vVShDdMXxptcOmdDpcwlaibHdXwhA);
							if (num < 0)
							{
								return false;
							}
							vjMLKUHJZaAXXkuVRTVSHOuSDpxk = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
							srbhVBYqnlIdTeKcQbbGPTruEtDBb = 0;
							goto IL_0157;
							IL_012b:
							if (ZhIDCrDprMAagoVuNqDCkjAcMNKGb.MoveNext())
							{
								ActionElementMap current = ZhIDCrDprMAagoVuNqDCkjAcMNKGb.Current;
								fheaVucpNTClmJZijNFkeGVdSPojA = current;
								RZrgYqfLVDiCGLYIUuNYYNyTqfFI = 1;
								return true;
							}
							yIpPlPRJyLANSUXxaecljxojmsSF();
							ZhIDCrDprMAagoVuNqDCkjAcMNKGb = null;
							goto IL_0145;
							IL_0157:
							if (srbhVBYqnlIdTeKcQbbGPTruEtDBb < vjMLKUHJZaAXXkuVRTVSHOuSDpxk.Count)
							{
								if ((!DJgYoRCCPNxOPrIetfxXAOiiqGBhA || vjMLKUHJZaAXXkuVRTVSHOuSDpxk[srbhVBYqnlIdTeKcQbbGPTruEtDBb].enabled) && vjMLKUHJZaAXXkuVRTVSHOuSDpxk[srbhVBYqnlIdTeKcQbbGPTruEtDBb].ContainsAction(VKyxAFPHTmYkQUPccsgEmtZFGfOo))
								{
									ZhIDCrDprMAagoVuNqDCkjAcMNKGb = vjMLKUHJZaAXXkuVRTVSHOuSDpxk[srbhVBYqnlIdTeKcQbbGPTruEtDBb].ElementMapsWithAction(VKyxAFPHTmYkQUPccsgEmtZFGfOo, DJgYoRCCPNxOPrIetfxXAOiiqGBhA).GetEnumerator();
									RZrgYqfLVDiCGLYIUuNYYNyTqfFI = -3;
									goto IL_012b;
								}
								goto IL_0145;
							}
							return false;
							IL_0145:
							srbhVBYqnlIdTeKcQbbGPTruEtDBb++;
							goto IL_0157;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void yIpPlPRJyLANSUXxaecljxojmsSF()
					{
						RZrgYqfLVDiCGLYIUuNYYNyTqfFI = -1;
						if (ZhIDCrDprMAagoVuNqDCkjAcMNKGb != null)
						{
							ZhIDCrDprMAagoVuNqDCkjAcMNKGb.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
					{
						OGCeldLwcwvXcFXOhXYIHpXJQDaj oGCeldLwcwvXcFXOhXYIHpXJQDaj;
						if (RZrgYqfLVDiCGLYIUuNYYNyTqfFI == -2 && JqRdaPsgWOIIvHdKoFfjGEeloRNS == Environment.CurrentManagedThreadId)
						{
							RZrgYqfLVDiCGLYIUuNYYNyTqfFI = 0;
							oGCeldLwcwvXcFXOhXYIHpXJQDaj = this;
						}
						else
						{
							oGCeldLwcwvXcFXOhXYIHpXJQDaj = new OGCeldLwcwvXcFXOhXYIHpXJQDaj(0);
							oGCeldLwcwvXcFXOhXYIHpXJQDaj.rgzqAfLUTwfVDDqYlSzucuJOakqg = rgzqAfLUTwfVDDqYlSzucuJOakqg;
						}
						oGCeldLwcwvXcFXOhXYIHpXJQDaj.dCUJzIdddaBpFRdxnzPnewMtiGIu = FGwMoBPueckqLRPzPbMzhTKuSjER;
						oGCeldLwcwvXcFXOhXYIHpXJQDaj.vVShDdMXxptcOmdDpcwlaibHdXwhA = kslDCAgzYYTdwoPWlBUlvncSwWjX;
						oGCeldLwcwvXcFXOhXYIHpXJQDaj.VKyxAFPHTmYkQUPccsgEmtZFGfOo = NBGKhrMrjmBXBtuEcPuyseySctgb;
						oGCeldLwcwvXcFXOhXYIHpXJQDaj.DJgYoRCCPNxOPrIetfxXAOiiqGBhA = gVXQISOmJseLkfxptiHechnnNMYp;
						return oGCeldLwcwvXcFXOhXYIHpXJQDaj;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
					}
				}

				private sealed class NXqgpGhehClIXdrGgInUICKZbVMtA : IEnumerable<ControllerMap>, IEnumerable, IEnumerator<ControllerMap>, IEnumerator, IDisposable
				{
					private int swKxojCKoWoBeUAwsmUGYtBhhcse;

					private ControllerMap BNbWrPoSkCkgjClKSvBZTMPoTfhj;

					private int HbCRVHCXCsnALhoCUUjUmiFjrNpc;

					public MapHelper GmmdVkQGZVGlwzrUoIbmEiqRcqgfA;

					private ControllerType ueKDyVxmEOuGBJRjVCYhhrIejMfR;

					public ControllerType idzFzvVrZSrNMVCGcwRxMoCdWgXG;

					private int pOjbZcMUXsPOFQDBrQVzEoYIveNd;

					public int eePGklArUlluWtozTEdyouDufCwlA;

					private int NXfKSluUbpwxtrIuofodzCHxTMtV;

					public int cgJZhJswBclBeuevcPiQgcLyBRxHA;

					private IList<ControllerMap> lWJYmvPrJjRrIKhxqZLdiZTEftRK;

					private int kBhTYKwqFOLyUGFvnBSgRvruLuSU;

					ControllerMap IEnumerator<ControllerMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return BNbWrPoSkCkgjClKSvBZTMPoTfhj;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return BNbWrPoSkCkgjClKSvBZTMPoTfhj;
						}
					}

					[DebuggerHidden]
					public NXqgpGhehClIXdrGgInUICKZbVMtA(int P_0)
					{
						swKxojCKoWoBeUAwsmUGYtBhhcse = P_0;
						HbCRVHCXCsnALhoCUUjUmiFjrNpc = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						lWJYmvPrJjRrIKhxqZLdiZTEftRK = null;
						swKxojCKoWoBeUAwsmUGYtBhhcse = -2;
					}

					private bool MoveNext()
					{
						int num = swKxojCKoWoBeUAwsmUGYtBhhcse;
						MapHelper gmmdVkQGZVGlwzrUoIbmEiqRcqgfA = GmmdVkQGZVGlwzrUoIbmEiqRcqgfA;
						if (num != 0)
						{
							if (num != 1)
							{
								return false;
							}
							swKxojCKoWoBeUAwsmUGYtBhhcse = -1;
							goto IL_00b0;
						}
						swKxojCKoWoBeUAwsmUGYtBhhcse = -1;
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = gmmdVkQGZVGlwzrUoIbmEiqRcqgfA.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ueKDyVxmEOuGBJRjVCYhhrIejMfR);
						int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(pOjbZcMUXsPOFQDBrQVzEoYIveNd);
						if (num2 < 0)
						{
							return false;
						}
						lWJYmvPrJjRrIKhxqZLdiZTEftRK = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num2).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
						kBhTYKwqFOLyUGFvnBSgRvruLuSU = 0;
						goto IL_00c2;
						IL_00c2:
						if (kBhTYKwqFOLyUGFvnBSgRvruLuSU < lWJYmvPrJjRrIKhxqZLdiZTEftRK.Count)
						{
							if (lWJYmvPrJjRrIKhxqZLdiZTEftRK[kBhTYKwqFOLyUGFvnBSgRvruLuSU].categoryId == NXfKSluUbpwxtrIuofodzCHxTMtV)
							{
								BNbWrPoSkCkgjClKSvBZTMPoTfhj = lWJYmvPrJjRrIKhxqZLdiZTEftRK[kBhTYKwqFOLyUGFvnBSgRvruLuSU];
								swKxojCKoWoBeUAwsmUGYtBhhcse = 1;
								return true;
							}
							goto IL_00b0;
						}
						return false;
						IL_00b0:
						kBhTYKwqFOLyUGFvnBSgRvruLuSU++;
						goto IL_00c2;
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
					IEnumerator<ControllerMap> IEnumerable<ControllerMap>.GetEnumerator()
					{
						NXqgpGhehClIXdrGgInUICKZbVMtA nXqgpGhehClIXdrGgInUICKZbVMtA;
						if (swKxojCKoWoBeUAwsmUGYtBhhcse == -2 && HbCRVHCXCsnALhoCUUjUmiFjrNpc == Environment.CurrentManagedThreadId)
						{
							swKxojCKoWoBeUAwsmUGYtBhhcse = 0;
							nXqgpGhehClIXdrGgInUICKZbVMtA = this;
						}
						else
						{
							nXqgpGhehClIXdrGgInUICKZbVMtA = new NXqgpGhehClIXdrGgInUICKZbVMtA(0);
							nXqgpGhehClIXdrGgInUICKZbVMtA.GmmdVkQGZVGlwzrUoIbmEiqRcqgfA = GmmdVkQGZVGlwzrUoIbmEiqRcqgfA;
						}
						nXqgpGhehClIXdrGgInUICKZbVMtA.ueKDyVxmEOuGBJRjVCYhhrIejMfR = idzFzvVrZSrNMVCGcwRxMoCdWgXG;
						nXqgpGhehClIXdrGgInUICKZbVMtA.pOjbZcMUXsPOFQDBrQVzEoYIveNd = eePGklArUlluWtozTEdyouDufCwlA;
						nXqgpGhehClIXdrGgInUICKZbVMtA.NXfKSluUbpwxtrIuofodzCHxTMtV = cgJZhJswBclBeuevcPiQgcLyBRxHA;
						return nXqgpGhehClIXdrGgInUICKZbVMtA;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerMap>)this).GetEnumerator();
					}
				}

				private sealed class cgOEWFBxxXyUDtUbVEMLCdlpJkszA<_0001> : IEnumerable<_0001>, IEnumerable, IEnumerator<_0001>, IEnumerator, IDisposable where _0001 : ControllerMap
				{
					private int EIymqcbjrATKrciHyGwPlfvstjnA;

					private _0001 ucjLlosgIZyMWBuGOwpdycIkfjnY;

					private int nPUvNIAGJGIQpgMGeshibIBsNudL;

					public MapHelper fUglqonmBZHvsErINdRrTqawGKoX;

					private int RHdaBzuOkbVAUTdZgciyIjMEGPpV;

					public int CmjQnJyQJlISMZdqdEdSZVMyPbCQ;

					private int BCJmGtwBdKiEeiOrztQRWSJnqMJCb;

					public int FfVcoMOJYPbColKSMsnRJznSMTFr;

					private IList<_0001> JdWPCzCFEkxrwomWSHDTsAQlSag;

					private int xaAfLEjWolNSiTpvUthDnwVKJucGA;

					_0001 IEnumerator<_0001>.Current
					{
						[DebuggerHidden]
						get
						{
							return ucjLlosgIZyMWBuGOwpdycIkfjnY;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return ucjLlosgIZyMWBuGOwpdycIkfjnY;
						}
					}

					[DebuggerHidden]
					public cgOEWFBxxXyUDtUbVEMLCdlpJkszA(int P_0)
					{
						EIymqcbjrATKrciHyGwPlfvstjnA = P_0;
						nPUvNIAGJGIQpgMGeshibIBsNudL = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						JdWPCzCFEkxrwomWSHDTsAQlSag = null;
						EIymqcbjrATKrciHyGwPlfvstjnA = -2;
					}

					private bool MoveNext()
					{
						int eIymqcbjrATKrciHyGwPlfvstjnA = EIymqcbjrATKrciHyGwPlfvstjnA;
						MapHelper mapHelper = fUglqonmBZHvsErINdRrTqawGKoX;
						if (eIymqcbjrATKrciHyGwPlfvstjnA != 0)
						{
							if (eIymqcbjrATKrciHyGwPlfvstjnA != 1)
							{
								return false;
							}
							EIymqcbjrATKrciHyGwPlfvstjnA = -1;
							goto IL_00b9;
						}
						EIymqcbjrATKrciHyGwPlfvstjnA = -1;
						ControllerType controllerType = cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<_0001>();
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
						int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(RHdaBzuOkbVAUTdZgciyIjMEGPpV);
						if (num < 0)
						{
							return false;
						}
						JdWPCzCFEkxrwomWSHDTsAQlSag = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.HlfkqnKSqjcPLaWjYfBdVuecIjhS<_0001>();
						xaAfLEjWolNSiTpvUthDnwVKJucGA = 0;
						goto IL_00cb;
						IL_00cb:
						if (xaAfLEjWolNSiTpvUthDnwVKJucGA < JdWPCzCFEkxrwomWSHDTsAQlSag.Count)
						{
							if (JdWPCzCFEkxrwomWSHDTsAQlSag[xaAfLEjWolNSiTpvUthDnwVKJucGA].categoryId == BCJmGtwBdKiEeiOrztQRWSJnqMJCb)
							{
								ucjLlosgIZyMWBuGOwpdycIkfjnY = JdWPCzCFEkxrwomWSHDTsAQlSag[xaAfLEjWolNSiTpvUthDnwVKJucGA];
								EIymqcbjrATKrciHyGwPlfvstjnA = 1;
								return true;
							}
							goto IL_00b9;
						}
						return false;
						IL_00b9:
						xaAfLEjWolNSiTpvUthDnwVKJucGA++;
						goto IL_00cb;
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
					IEnumerator<_0001> IEnumerable<_0001>.GetEnumerator()
					{
						cgOEWFBxxXyUDtUbVEMLCdlpJkszA<_0001> cgOEWFBxxXyUDtUbVEMLCdlpJkszA2;
						if (EIymqcbjrATKrciHyGwPlfvstjnA == -2 && nPUvNIAGJGIQpgMGeshibIBsNudL == Environment.CurrentManagedThreadId)
						{
							EIymqcbjrATKrciHyGwPlfvstjnA = 0;
							cgOEWFBxxXyUDtUbVEMLCdlpJkszA2 = this;
						}
						else
						{
							cgOEWFBxxXyUDtUbVEMLCdlpJkszA2 = new cgOEWFBxxXyUDtUbVEMLCdlpJkszA<_0001>(0);
							cgOEWFBxxXyUDtUbVEMLCdlpJkszA2.fUglqonmBZHvsErINdRrTqawGKoX = fUglqonmBZHvsErINdRrTqawGKoX;
						}
						cgOEWFBxxXyUDtUbVEMLCdlpJkszA2.RHdaBzuOkbVAUTdZgciyIjMEGPpV = CmjQnJyQJlISMZdqdEdSZVMyPbCQ;
						cgOEWFBxxXyUDtUbVEMLCdlpJkszA2.BCJmGtwBdKiEeiOrztQRWSJnqMJCb = FfVcoMOJYPbColKSMsnRJznSMTFr;
						return cgOEWFBxxXyUDtUbVEMLCdlpJkszA2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<_0001>)this).GetEnumerator();
					}
				}

				private sealed class okayzLlwCRoeiyylHCDzvnMpCCJO : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
				{
					private int qRLtLvOxuqjhDODKFBjPlGfdBrJBA;

					private ActionElementMap QtKggTWAtVBrHFGEekxPTbaxaIHjb;

					private int UcDcBqKkGJzcRgeYbnRZIPjoegZIb;

					public MapHelper qsjAhQEEzvLHzufNmbHrqCYfNsiP;

					private int uVyMzCOgSvYpnMjjIgsXnYfspVko;

					public int PEvbVdApKPJfMIhyHoqpmMxkUDnkA;

					private bool TmXsLKyMYbdGvatcQNagOTaTMUAyA;

					public bool WBJmqArDNBXSvaecFKCxupngEhCS;

					private int JwiqfXpKUcdaZiAMaOnTYuMUKEliA;

					private int ZRWMMGTVqoIDjIrAWgLpfPFejFDc;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol oOXCHXAlwdeBUGAAIoSZNKLKzrcqb;

					private int pMtLqcVnpYgpoMXzOjiZBCozDqvy;

					private int TuKpVMXIgmhMtkXyFOvJmUVbKPocA;

					private vHszkbCJdDAIcILHhpVCxcZlIBxlA kaPkSBUszZmbbBlNtQVXENsikDFP;

					private int nRoThpfGYBkmSnEgOxzMmOTyOnjL;

					private int GxfaKwbjakiVyGGaHIqjSVlUYFLE;

					private IEnumerator<ActionElementMap> OYeSGmVVDWTeTueZsrkSpuTBbchW;

					ActionElementMap IEnumerator<ActionElementMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return QtKggTWAtVBrHFGEekxPTbaxaIHjb;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return QtKggTWAtVBrHFGEekxPTbaxaIHjb;
						}
					}

					[DebuggerHidden]
					public okayzLlwCRoeiyylHCDzvnMpCCJO(int P_0)
					{
						qRLtLvOxuqjhDODKFBjPlGfdBrJBA = P_0;
						UcDcBqKkGJzcRgeYbnRZIPjoegZIb = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = qRLtLvOxuqjhDODKFBjPlGfdBrJBA;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								aslkzmjXPIUKbwwMJLbkjMVssTnR();
							}
						}
						oOXCHXAlwdeBUGAAIoSZNKLKzrcqb = null;
						kaPkSBUszZmbbBlNtQVXENsikDFP = null;
						OYeSGmVVDWTeTueZsrkSpuTBbchW = null;
						qRLtLvOxuqjhDODKFBjPlGfdBrJBA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = qRLtLvOxuqjhDODKFBjPlGfdBrJBA;
							MapHelper mapHelper = qsjAhQEEzvLHzufNmbHrqCYfNsiP;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								qRLtLvOxuqjhDODKFBjPlGfdBrJBA = -3;
								goto IL_016c;
							}
							qRLtLvOxuqjhDODKFBjPlGfdBrJBA = -1;
							if (ReInput._id != mapHelper.MUfpEiqygGgMiNQDuSWClpiuBuic)
							{
								ReInput.CheckInitialized(mapHelper.MUfpEiqygGgMiNQDuSWClpiuBuic);
								return false;
							}
							if (uVyMzCOgSvYpnMjjIgsXnYfspVko < 0)
							{
								return false;
							}
							JwiqfXpKUcdaZiAMaOnTYuMUKEliA = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
							ZRWMMGTVqoIDjIrAWgLpfPFejFDc = 0;
							goto IL_01ec;
							IL_016c:
							if (OYeSGmVVDWTeTueZsrkSpuTBbchW.MoveNext())
							{
								ActionElementMap current = OYeSGmVVDWTeTueZsrkSpuTBbchW.Current;
								QtKggTWAtVBrHFGEekxPTbaxaIHjb = current;
								qRLtLvOxuqjhDODKFBjPlGfdBrJBA = 1;
								return true;
							}
							aslkzmjXPIUKbwwMJLbkjMVssTnR();
							OYeSGmVVDWTeTueZsrkSpuTBbchW = null;
							goto IL_0186;
							IL_0186:
							GxfaKwbjakiVyGGaHIqjSVlUYFLE++;
							goto IL_0198;
							IL_01c2:
							if (TuKpVMXIgmhMtkXyFOvJmUVbKPocA < pMtLqcVnpYgpoMXzOjiZBCozDqvy)
							{
								kaPkSBUszZmbbBlNtQVXENsikDFP = oOXCHXAlwdeBUGAAIoSZNKLKzrcqb.EfgDlYQxDuklXErnyCYTFBkkEVmX(TuKpVMXIgmhMtkXyFOvJmUVbKPocA).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
								nRoThpfGYBkmSnEgOxzMmOTyOnjL = kaPkSBUszZmbbBlNtQVXENsikDFP.spuxbZMpjzXXEeAzzgWchppYZEErA;
								GxfaKwbjakiVyGGaHIqjSVlUYFLE = 0;
								goto IL_0198;
							}
							oOXCHXAlwdeBUGAAIoSZNKLKzrcqb = null;
							ZRWMMGTVqoIDjIrAWgLpfPFejFDc++;
							goto IL_01ec;
							IL_0198:
							if (GxfaKwbjakiVyGGaHIqjSVlUYFLE < nRoThpfGYBkmSnEgOxzMmOTyOnjL)
							{
								ControllerMap controllerMap = kaPkSBUszZmbbBlNtQVXENsikDFP.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(GxfaKwbjakiVyGGaHIqjSVlUYFLE);
								if ((!TmXsLKyMYbdGvatcQNagOTaTMUAyA || controllerMap.enabled) && controllerMap.ContainsAction(uVyMzCOgSvYpnMjjIgsXnYfspVko))
								{
									OYeSGmVVDWTeTueZsrkSpuTBbchW = controllerMap.ElementMapsWithAction(uVyMzCOgSvYpnMjjIgsXnYfspVko, TmXsLKyMYbdGvatcQNagOTaTMUAyA).GetEnumerator();
									qRLtLvOxuqjhDODKFBjPlGfdBrJBA = -3;
									goto IL_016c;
								}
								goto IL_0186;
							}
							kaPkSBUszZmbbBlNtQVXENsikDFP = null;
							TuKpVMXIgmhMtkXyFOvJmUVbKPocA++;
							goto IL_01c2;
							IL_01ec:
							if (ZRWMMGTVqoIDjIrAWgLpfPFejFDc < JwiqfXpKUcdaZiAMaOnTYuMUKEliA)
							{
								oOXCHXAlwdeBUGAAIoSZNKLKzrcqb = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(ZRWMMGTVqoIDjIrAWgLpfPFejFDc);
								pMtLqcVnpYgpoMXzOjiZBCozDqvy = oOXCHXAlwdeBUGAAIoSZNKLKzrcqb.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
								TuKpVMXIgmhMtkXyFOvJmUVbKPocA = 0;
								goto IL_01c2;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void aslkzmjXPIUKbwwMJLbkjMVssTnR()
					{
						qRLtLvOxuqjhDODKFBjPlGfdBrJBA = -1;
						if (OYeSGmVVDWTeTueZsrkSpuTBbchW != null)
						{
							OYeSGmVVDWTeTueZsrkSpuTBbchW.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
					{
						okayzLlwCRoeiyylHCDzvnMpCCJO okayzLlwCRoeiyylHCDzvnMpCCJO2;
						if (qRLtLvOxuqjhDODKFBjPlGfdBrJBA == -2 && UcDcBqKkGJzcRgeYbnRZIPjoegZIb == Environment.CurrentManagedThreadId)
						{
							qRLtLvOxuqjhDODKFBjPlGfdBrJBA = 0;
							okayzLlwCRoeiyylHCDzvnMpCCJO2 = this;
						}
						else
						{
							okayzLlwCRoeiyylHCDzvnMpCCJO2 = new okayzLlwCRoeiyylHCDzvnMpCCJO(0);
							okayzLlwCRoeiyylHCDzvnMpCCJO2.qsjAhQEEzvLHzufNmbHrqCYfNsiP = qsjAhQEEzvLHzufNmbHrqCYfNsiP;
						}
						okayzLlwCRoeiyylHCDzvnMpCCJO2.uVyMzCOgSvYpnMjjIgsXnYfspVko = PEvbVdApKPJfMIhyHoqpmMxkUDnkA;
						okayzLlwCRoeiyylHCDzvnMpCCJO2.TmXsLKyMYbdGvatcQNagOTaTMUAyA = WBJmqArDNBXSvaecFKCxupngEhCS;
						return okayzLlwCRoeiyylHCDzvnMpCCJO2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
					}
				}

				private sealed class LZvJquRtIrioJsfORfvxGoMBKoNB : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
				{
					private int QbumkvRicfSgheDSwweDsoqiwokA;

					private ActionElementMap pvcTkWmMRbDjsOEbgnlMTdIfKeLq;

					private int doolmbkaWeeDmMtCSVVjoREZBPRq;

					private IControllerElementTarget qLZAMsGJNMnDIlfdglHqCPpEVnzFB;

					public IControllerElementTarget RreMBcFqyHWGonZvkNDhLzUOYuxC;

					public MapHelper EizHEAdbFxFQWiOBxOiutfvAmlVSA;

					private bool cJHHcvthlssHjkRDjNSFOpACNKbk;

					public bool rXjhwyElZnJDoJFHyksVavgqTbbK;

					private bool uXgXYsnCvriUVELwDZDBIkVFRMFG;

					public bool JckDxfFWsYOcyBmFfsVZPjesUkogA;

					private int BLmtBlGlQmaciCpLQhuYMiPagEsB;

					public int vwvuUPcxsFjFpgtxxotJdQwUqcUg;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol FDvGOHaXSWYxeWZEvkjtLiLlNLAX;

					private int cUmMzmVTjjAGwHSxjfOQQIdSNhUm;

					private int DZdvALFogFpekKAGWLUdNUabzmCG;

					private IList<ControllerMap> JXroTOmnWYjkGVrAGUbXPvdGTgth;

					private int GdhoHhtLEfXVlbphAGwVbbVGlahVA;

					private int iqGyIdpSsIbxIxhksJEiNLuEMAOI;

					private TempListPool.TList<ActionElementMap> IFfBHDMGFFEOvgrGWmIAUHSIhRpQ;

					private List<ActionElementMap>.Enumerator qfrvuBMhpNcvpkwhBiyzqqrFrVJI;

					ActionElementMap IEnumerator<ActionElementMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return pvcTkWmMRbDjsOEbgnlMTdIfKeLq;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return pvcTkWmMRbDjsOEbgnlMTdIfKeLq;
						}
					}

					[DebuggerHidden]
					public LZvJquRtIrioJsfORfvxGoMBKoNB(int P_0)
					{
						QbumkvRicfSgheDSwweDsoqiwokA = P_0;
						doolmbkaWeeDmMtCSVVjoREZBPRq = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int qbumkvRicfSgheDSwweDsoqiwokA = QbumkvRicfSgheDSwweDsoqiwokA;
						if ((uint)(qbumkvRicfSgheDSwweDsoqiwokA - -4) <= 1u || qbumkvRicfSgheDSwweDsoqiwokA == 1)
						{
							try
							{
								if (qbumkvRicfSgheDSwweDsoqiwokA == -4 || qbumkvRicfSgheDSwweDsoqiwokA == 1)
								{
									try
									{
									}
									finally
									{
										gfkbhwgBIkMYTSOSpavIjOQUDJaoA();
									}
								}
							}
							finally
							{
								frfPbjMCaJfiXaBcSOqvnafrDDBM();
							}
						}
						FDvGOHaXSWYxeWZEvkjtLiLlNLAX = null;
						JXroTOmnWYjkGVrAGUbXPvdGTgth = null;
						IFfBHDMGFFEOvgrGWmIAUHSIhRpQ = null;
						qfrvuBMhpNcvpkwhBiyzqqrFrVJI = default(List<ActionElementMap>.Enumerator);
						QbumkvRicfSgheDSwweDsoqiwokA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int qbumkvRicfSgheDSwweDsoqiwokA = QbumkvRicfSgheDSwweDsoqiwokA;
							MapHelper eizHEAdbFxFQWiOBxOiutfvAmlVSA = EizHEAdbFxFQWiOBxOiutfvAmlVSA;
							if (qbumkvRicfSgheDSwweDsoqiwokA != 0)
							{
								if (qbumkvRicfSgheDSwweDsoqiwokA != 1)
								{
									return false;
								}
								QbumkvRicfSgheDSwweDsoqiwokA = -4;
								goto IL_017c;
							}
							QbumkvRicfSgheDSwweDsoqiwokA = -1;
							if (qLZAMsGJNMnDIlfdglHqCPpEVnzFB == null)
							{
								return false;
							}
							Controller controller = qLZAMsGJNMnDIlfdglHqCPpEVnzFB.controller;
							if (controller == null)
							{
								return false;
							}
							FDvGOHaXSWYxeWZEvkjtLiLlNLAX = eizHEAdbFxFQWiOBxOiutfvAmlVSA.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controller.type);
							cUmMzmVTjjAGwHSxjfOQQIdSNhUm = FDvGOHaXSWYxeWZEvkjtLiLlNLAX.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
							DZdvALFogFpekKAGWLUdNUabzmCG = 0;
							goto IL_01e4;
							IL_017c:
							if (qfrvuBMhpNcvpkwhBiyzqqrFrVJI.MoveNext())
							{
								ActionElementMap current = qfrvuBMhpNcvpkwhBiyzqqrFrVJI.Current;
								pvcTkWmMRbDjsOEbgnlMTdIfKeLq = current;
								QbumkvRicfSgheDSwweDsoqiwokA = 1;
								return true;
							}
							gfkbhwgBIkMYTSOSpavIjOQUDJaoA();
							qfrvuBMhpNcvpkwhBiyzqqrFrVJI = default(List<ActionElementMap>.Enumerator);
							frfPbjMCaJfiXaBcSOqvnafrDDBM();
							IFfBHDMGFFEOvgrGWmIAUHSIhRpQ = null;
							goto IL_01a8;
							IL_01e4:
							if (DZdvALFogFpekKAGWLUdNUabzmCG < cUmMzmVTjjAGwHSxjfOQQIdSNhUm)
							{
								vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = FDvGOHaXSWYxeWZEvkjtLiLlNLAX.EfgDlYQxDuklXErnyCYTFBkkEVmX(DZdvALFogFpekKAGWLUdNUabzmCG).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
								_ = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA;
								JXroTOmnWYjkGVrAGUbXPvdGTgth = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.WLYuOXdjmnqxqMGLXCcRexyymPBD;
								GdhoHhtLEfXVlbphAGwVbbVGlahVA = JXroTOmnWYjkGVrAGUbXPvdGTgth.Count;
								iqGyIdpSsIbxIxhksJEiNLuEMAOI = 0;
								goto IL_01ba;
							}
							return false;
							IL_01ba:
							if (iqGyIdpSsIbxIxhksJEiNLuEMAOI < GdhoHhtLEfXVlbphAGwVbbVGlahVA)
							{
								ControllerMap controllerMap = JXroTOmnWYjkGVrAGUbXPvdGTgth[iqGyIdpSsIbxIxhksJEiNLuEMAOI];
								if (!cJHHcvthlssHjkRDjNSFOpACNKbk || controllerMap.enabled)
								{
									IFfBHDMGFFEOvgrGWmIAUHSIhRpQ = TempListPool.GetTList<ActionElementMap>();
									QbumkvRicfSgheDSwweDsoqiwokA = -3;
									List<ActionElementMap> list = IFfBHDMGFFEOvgrGWmIAUHSIhRpQ.list;
									controllerMap.yxkdMnawZniDZXFmujYxcNWEtVSFA(qLZAMsGJNMnDIlfdglHqCPpEVnzFB, uXgXYsnCvriUVELwDZDBIkVFRMFG, BLmtBlGlQmaciCpLQhuYMiPagEsB, cJHHcvthlssHjkRDjNSFOpACNKbk, list, true, out var _);
									qfrvuBMhpNcvpkwhBiyzqqrFrVJI = list.GetEnumerator();
									QbumkvRicfSgheDSwweDsoqiwokA = -4;
									goto IL_017c;
								}
								goto IL_01a8;
							}
							JXroTOmnWYjkGVrAGUbXPvdGTgth = null;
							DZdvALFogFpekKAGWLUdNUabzmCG++;
							goto IL_01e4;
							IL_01a8:
							iqGyIdpSsIbxIxhksJEiNLuEMAOI++;
							goto IL_01ba;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void frfPbjMCaJfiXaBcSOqvnafrDDBM()
					{
						QbumkvRicfSgheDSwweDsoqiwokA = -1;
						if (IFfBHDMGFFEOvgrGWmIAUHSIhRpQ != null)
						{
							((IDisposable)IFfBHDMGFFEOvgrGWmIAUHSIhRpQ).Dispose();
						}
					}

					private void gfkbhwgBIkMYTSOSpavIjOQUDJaoA()
					{
						QbumkvRicfSgheDSwweDsoqiwokA = -3;
						((IDisposable)qfrvuBMhpNcvpkwhBiyzqqrFrVJI/*cast due to .constrained prefix*/).Dispose();
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
					{
						LZvJquRtIrioJsfORfvxGoMBKoNB lZvJquRtIrioJsfORfvxGoMBKoNB;
						if (QbumkvRicfSgheDSwweDsoqiwokA == -2 && doolmbkaWeeDmMtCSVVjoREZBPRq == Environment.CurrentManagedThreadId)
						{
							QbumkvRicfSgheDSwweDsoqiwokA = 0;
							lZvJquRtIrioJsfORfvxGoMBKoNB = this;
						}
						else
						{
							lZvJquRtIrioJsfORfvxGoMBKoNB = new LZvJquRtIrioJsfORfvxGoMBKoNB(0);
							lZvJquRtIrioJsfORfvxGoMBKoNB.EizHEAdbFxFQWiOBxOiutfvAmlVSA = EizHEAdbFxFQWiOBxOiutfvAmlVSA;
						}
						lZvJquRtIrioJsfORfvxGoMBKoNB.qLZAMsGJNMnDIlfdglHqCPpEVnzFB = RreMBcFqyHWGonZvkNDhLzUOYuxC;
						lZvJquRtIrioJsfORfvxGoMBKoNB.uXgXYsnCvriUVELwDZDBIkVFRMFG = JckDxfFWsYOcyBmFfsVZPjesUkogA;
						lZvJquRtIrioJsfORfvxGoMBKoNB.BLmtBlGlQmaciCpLQhuYMiPagEsB = vwvuUPcxsFjFpgtxxotJdQwUqcUg;
						lZvJquRtIrioJsfORfvxGoMBKoNB.cJHHcvthlssHjkRDjNSFOpACNKbk = rXjhwyElZnJDoJFHyksVavgqTbbK;
						return lZvJquRtIrioJsfORfvxGoMBKoNB;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
					}
				}

				private sealed class WzEAgduqTLDKfjHVZSjFtsKwyODc : IEnumerable<ControllerMap>, IEnumerable, IEnumerator<ControllerMap>, IEnumerator, IDisposable
				{
					private int pMzrDmUIivqLjAkniuFInDirWBdO;

					private ControllerMap dZZAASbFuSXAulIhnZluGYIgQzSt;

					private int kQClABjiIaAlvKEPgEqHTRhjFCmT;

					public MapHelper GdHETqZfakHrEiGyHwpjSvCfRQrIA;

					private int FkKYuFHpwCdtgxQhsYAiGlWxyLjB;

					private int sSFvXbAgzXMMPrwykNYSTXaNNEoV;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol wznLjPhieIchZHxjyUjlkPcqkeVhb;

					private int wMrzZAiHWjkQnPaYxjMwkzcohcHu;

					private int iZGInGTtfxydHvnWYDcBhPEyhXpc;

					private vHszkbCJdDAIcILHhpVCxcZlIBxlA mWWEIyCEVRXUhtqHBkCCdAAcqRee;

					private int YiFjHxfMYMyPkThfnktZRAuoFuMdA;

					private int uElLDjtvwnnSFMGUDQzdEvbmKuzc;

					ControllerMap IEnumerator<ControllerMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return dZZAASbFuSXAulIhnZluGYIgQzSt;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return dZZAASbFuSXAulIhnZluGYIgQzSt;
						}
					}

					[DebuggerHidden]
					public WzEAgduqTLDKfjHVZSjFtsKwyODc(int P_0)
					{
						pMzrDmUIivqLjAkniuFInDirWBdO = P_0;
						kQClABjiIaAlvKEPgEqHTRhjFCmT = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						wznLjPhieIchZHxjyUjlkPcqkeVhb = null;
						mWWEIyCEVRXUhtqHBkCCdAAcqRee = null;
						pMzrDmUIivqLjAkniuFInDirWBdO = -2;
					}

					private bool MoveNext()
					{
						int num = pMzrDmUIivqLjAkniuFInDirWBdO;
						MapHelper gdHETqZfakHrEiGyHwpjSvCfRQrIA = GdHETqZfakHrEiGyHwpjSvCfRQrIA;
						if (num != 0)
						{
							if (num != 1)
							{
								return false;
							}
							pMzrDmUIivqLjAkniuFInDirWBdO = -1;
							uElLDjtvwnnSFMGUDQzdEvbmKuzc++;
							goto IL_0104;
						}
						pMzrDmUIivqLjAkniuFInDirWBdO = -1;
						if (ReInput._id != gdHETqZfakHrEiGyHwpjSvCfRQrIA.MUfpEiqygGgMiNQDuSWClpiuBuic)
						{
							ReInput.CheckInitialized(gdHETqZfakHrEiGyHwpjSvCfRQrIA.MUfpEiqygGgMiNQDuSWClpiuBuic);
							return false;
						}
						FkKYuFHpwCdtgxQhsYAiGlWxyLjB = gdHETqZfakHrEiGyHwpjSvCfRQrIA.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
						sSFvXbAgzXMMPrwykNYSTXaNNEoV = 0;
						goto IL_0151;
						IL_0104:
						if (uElLDjtvwnnSFMGUDQzdEvbmKuzc < YiFjHxfMYMyPkThfnktZRAuoFuMdA)
						{
							dZZAASbFuSXAulIhnZluGYIgQzSt = mWWEIyCEVRXUhtqHBkCCdAAcqRee.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(uElLDjtvwnnSFMGUDQzdEvbmKuzc);
							pMzrDmUIivqLjAkniuFInDirWBdO = 1;
							return true;
						}
						mWWEIyCEVRXUhtqHBkCCdAAcqRee = null;
						iZGInGTtfxydHvnWYDcBhPEyhXpc++;
						goto IL_0129;
						IL_0129:
						if (iZGInGTtfxydHvnWYDcBhPEyhXpc < wMrzZAiHWjkQnPaYxjMwkzcohcHu)
						{
							mWWEIyCEVRXUhtqHBkCCdAAcqRee = wznLjPhieIchZHxjyUjlkPcqkeVhb.EfgDlYQxDuklXErnyCYTFBkkEVmX(iZGInGTtfxydHvnWYDcBhPEyhXpc).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
							YiFjHxfMYMyPkThfnktZRAuoFuMdA = mWWEIyCEVRXUhtqHBkCCdAAcqRee.spuxbZMpjzXXEeAzzgWchppYZEErA;
							uElLDjtvwnnSFMGUDQzdEvbmKuzc = 0;
							goto IL_0104;
						}
						wznLjPhieIchZHxjyUjlkPcqkeVhb = null;
						sSFvXbAgzXMMPrwykNYSTXaNNEoV++;
						goto IL_0151;
						IL_0151:
						if (sSFvXbAgzXMMPrwykNYSTXaNNEoV < FkKYuFHpwCdtgxQhsYAiGlWxyLjB)
						{
							wznLjPhieIchZHxjyUjlkPcqkeVhb = gdHETqZfakHrEiGyHwpjSvCfRQrIA.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(sSFvXbAgzXMMPrwykNYSTXaNNEoV);
							wMrzZAiHWjkQnPaYxjMwkzcohcHu = wznLjPhieIchZHxjyUjlkPcqkeVhb.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
							iZGInGTtfxydHvnWYDcBhPEyhXpc = 0;
							goto IL_0129;
						}
						return false;
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
					IEnumerator<ControllerMap> IEnumerable<ControllerMap>.GetEnumerator()
					{
						WzEAgduqTLDKfjHVZSjFtsKwyODc wzEAgduqTLDKfjHVZSjFtsKwyODc;
						if (pMzrDmUIivqLjAkniuFInDirWBdO == -2 && kQClABjiIaAlvKEPgEqHTRhjFCmT == Environment.CurrentManagedThreadId)
						{
							pMzrDmUIivqLjAkniuFInDirWBdO = 0;
							wzEAgduqTLDKfjHVZSjFtsKwyODc = this;
						}
						else
						{
							wzEAgduqTLDKfjHVZSjFtsKwyODc = new WzEAgduqTLDKfjHVZSjFtsKwyODc(0);
							wzEAgduqTLDKfjHVZSjFtsKwyODc.GdHETqZfakHrEiGyHwpjSvCfRQrIA = GdHETqZfakHrEiGyHwpjSvCfRQrIA;
						}
						return wzEAgduqTLDKfjHVZSjFtsKwyODc;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerMap>)this).GetEnumerator();
					}
				}

				private sealed class HKBbRTEaiQMSxzCxSZiWKMEyaBHAb<_0001> : IEnumerable<_0001>, IEnumerable, IEnumerator<_0001>, IEnumerator, IDisposable where _0001 : ControllerMap
				{
					private int sjfWybyNPthUsVSJFtNlTosKatfD;

					private _0001 mwbzlJdGMsMtVTihnjAgqIJqfIgV;

					private int zrBdhvTZgCgybVKQuVdTUWZXHRmT;

					public MapHelper kWRMzerdnHOrNZuQUaYqqolHZYqg;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol PCpnnHSomLCpFtucTYQsKYySgSWI;

					private int UHFjameIBoTeULZNjLaqGUljDQho;

					private int JTZSAMvzZffvIuErRcOPQGbdBXmr;

					private vHszkbCJdDAIcILHhpVCxcZlIBxlA ywRtflJvRgzDQxBuhkClzDEmsmNj;

					private int jrlBkvctbDQcZovFkRocPHpkaiKH;

					private int iheMlaqBvksQpCfovaFFQKuLJJFx;

					private int ohaDVyUpVQBTNPGFHcREFdwXpHRzA;

					private int kIielDhbLxLxDVPaAsQWaBzQNeWHA;

					_0001 IEnumerator<_0001>.Current
					{
						[DebuggerHidden]
						get
						{
							return mwbzlJdGMsMtVTihnjAgqIJqfIgV;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return mwbzlJdGMsMtVTihnjAgqIJqfIgV;
						}
					}

					[DebuggerHidden]
					public HKBbRTEaiQMSxzCxSZiWKMEyaBHAb(int P_0)
					{
						sjfWybyNPthUsVSJFtNlTosKatfD = P_0;
						zrBdhvTZgCgybVKQuVdTUWZXHRmT = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						PCpnnHSomLCpFtucTYQsKYySgSWI = null;
						ywRtflJvRgzDQxBuhkClzDEmsmNj = null;
						sjfWybyNPthUsVSJFtNlTosKatfD = -2;
					}

					private bool MoveNext()
					{
						int num = sjfWybyNPthUsVSJFtNlTosKatfD;
						MapHelper mapHelper = kWRMzerdnHOrNZuQUaYqqolHZYqg;
						switch (num)
						{
						default:
							return false;
						case 0:
						{
							sjfWybyNPthUsVSJFtNlTosKatfD = -1;
							if (ReInput._id != mapHelper.MUfpEiqygGgMiNQDuSWClpiuBuic)
							{
								ReInput.CheckInitialized(mapHelper.MUfpEiqygGgMiNQDuSWClpiuBuic);
								return false;
							}
							if (cVDyIiOsEfJNYzVuZSmuEXqylgT.QcWlAvzQGvfVRlAokvBMpnQELmEj<_0001>(out var controllerType))
							{
								PCpnnHSomLCpFtucTYQsKYySgSWI = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
								UHFjameIBoTeULZNjLaqGUljDQho = PCpnnHSomLCpFtucTYQsKYySgSWI.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
								JTZSAMvzZffvIuErRcOPQGbdBXmr = 0;
								goto IL_011b;
							}
							UHFjameIBoTeULZNjLaqGUljDQho = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
							JTZSAMvzZffvIuErRcOPQGbdBXmr = 0;
							goto IL_0264;
						}
						case 1:
							sjfWybyNPthUsVSJFtNlTosKatfD = -1;
							iheMlaqBvksQpCfovaFFQKuLJJFx++;
							goto IL_00f6;
						case 2:
							{
								sjfWybyNPthUsVSJFtNlTosKatfD = -1;
								goto IL_0207;
							}
							IL_0207:
							kIielDhbLxLxDVPaAsQWaBzQNeWHA++;
							goto IL_0217;
							IL_0264:
							if (JTZSAMvzZffvIuErRcOPQGbdBXmr >= UHFjameIBoTeULZNjLaqGUljDQho)
							{
								break;
							}
							PCpnnHSomLCpFtucTYQsKYySgSWI = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(JTZSAMvzZffvIuErRcOPQGbdBXmr);
							jrlBkvctbDQcZovFkRocPHpkaiKH = PCpnnHSomLCpFtucTYQsKYySgSWI.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
							iheMlaqBvksQpCfovaFFQKuLJJFx = 0;
							goto IL_023c;
							IL_011b:
							if (JTZSAMvzZffvIuErRcOPQGbdBXmr < UHFjameIBoTeULZNjLaqGUljDQho)
							{
								ywRtflJvRgzDQxBuhkClzDEmsmNj = PCpnnHSomLCpFtucTYQsKYySgSWI.EfgDlYQxDuklXErnyCYTFBkkEVmX(JTZSAMvzZffvIuErRcOPQGbdBXmr).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
								jrlBkvctbDQcZovFkRocPHpkaiKH = ywRtflJvRgzDQxBuhkClzDEmsmNj.spuxbZMpjzXXEeAzzgWchppYZEErA;
								iheMlaqBvksQpCfovaFFQKuLJJFx = 0;
								goto IL_00f6;
							}
							PCpnnHSomLCpFtucTYQsKYySgSWI = null;
							break;
							IL_0217:
							if (kIielDhbLxLxDVPaAsQWaBzQNeWHA < ohaDVyUpVQBTNPGFHcREFdwXpHRzA)
							{
								if (ywRtflJvRgzDQxBuhkClzDEmsmNj.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(kIielDhbLxLxDVPaAsQWaBzQNeWHA) is _0001 val)
								{
									mwbzlJdGMsMtVTihnjAgqIJqfIgV = val;
									sjfWybyNPthUsVSJFtNlTosKatfD = 2;
									return true;
								}
								goto IL_0207;
							}
							ywRtflJvRgzDQxBuhkClzDEmsmNj = null;
							iheMlaqBvksQpCfovaFFQKuLJJFx++;
							goto IL_023c;
							IL_023c:
							if (iheMlaqBvksQpCfovaFFQKuLJJFx < jrlBkvctbDQcZovFkRocPHpkaiKH)
							{
								ywRtflJvRgzDQxBuhkClzDEmsmNj = PCpnnHSomLCpFtucTYQsKYySgSWI.EfgDlYQxDuklXErnyCYTFBkkEVmX(iheMlaqBvksQpCfovaFFQKuLJJFx).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
								ohaDVyUpVQBTNPGFHcREFdwXpHRzA = ywRtflJvRgzDQxBuhkClzDEmsmNj.spuxbZMpjzXXEeAzzgWchppYZEErA;
								kIielDhbLxLxDVPaAsQWaBzQNeWHA = 0;
								goto IL_0217;
							}
							PCpnnHSomLCpFtucTYQsKYySgSWI = null;
							JTZSAMvzZffvIuErRcOPQGbdBXmr++;
							goto IL_0264;
							IL_00f6:
							if (iheMlaqBvksQpCfovaFFQKuLJJFx < jrlBkvctbDQcZovFkRocPHpkaiKH)
							{
								mwbzlJdGMsMtVTihnjAgqIJqfIgV = (_0001)ywRtflJvRgzDQxBuhkClzDEmsmNj.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(iheMlaqBvksQpCfovaFFQKuLJJFx);
								sjfWybyNPthUsVSJFtNlTosKatfD = 1;
								return true;
							}
							ywRtflJvRgzDQxBuhkClzDEmsmNj = null;
							JTZSAMvzZffvIuErRcOPQGbdBXmr++;
							goto IL_011b;
						}
						return false;
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
					IEnumerator<_0001> IEnumerable<_0001>.GetEnumerator()
					{
						HKBbRTEaiQMSxzCxSZiWKMEyaBHAb<_0001> hKBbRTEaiQMSxzCxSZiWKMEyaBHAb;
						if (sjfWybyNPthUsVSJFtNlTosKatfD == -2 && zrBdhvTZgCgybVKQuVdTUWZXHRmT == Environment.CurrentManagedThreadId)
						{
							sjfWybyNPthUsVSJFtNlTosKatfD = 0;
							hKBbRTEaiQMSxzCxSZiWKMEyaBHAb = this;
						}
						else
						{
							hKBbRTEaiQMSxzCxSZiWKMEyaBHAb = new HKBbRTEaiQMSxzCxSZiWKMEyaBHAb<_0001>(0);
							hKBbRTEaiQMSxzCxSZiWKMEyaBHAb.kWRMzerdnHOrNZuQUaYqqolHZYqg = kWRMzerdnHOrNZuQUaYqqolHZYqg;
						}
						return hKBbRTEaiQMSxzCxSZiWKMEyaBHAb;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<_0001>)this).GetEnumerator();
					}
				}

				private sealed class dSPTUGUeyCilpdlfGPsQxKTXBeecA : IEnumerable<ControllerMap>, IEnumerable, IEnumerator<ControllerMap>, IEnumerator, IDisposable
				{
					private int TGvyEZskMFxupLUfHbDcEYypjvpX;

					private ControllerMap SFAVTpYiGcqmKDqgIzDyahWsJMxP;

					private int IrSnwOFaGVuIlxTRlxdcMSwvdsdk;

					public MapHelper pGTnkqNgYNyNspVHWIAXdJpvIhHx;

					private ControllerType nVitdzTMMPVuWLqMlVJmUuFBlwsA;

					public ControllerType ubLgMabYnfwAdVAtrxEAbtLjEVje;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol kxpWdLnGHmVXAoiMvcGdUQssGDKcA;

					private int pPwDaCPxDeRQmfYNuUIIIZQdiCXe;

					private int cEDyxaeXIAedVdgpYLxBIqRrsPYX;

					private vHszkbCJdDAIcILHhpVCxcZlIBxlA XMyqgMYuCzKAAVFXjuDFtLvkHhoV;

					private int NlVgDleuOXcvwFBZcoCXSwGHSitJ;

					private int ohHiCkVcZFbMnhXwpRxsIuIpxGdJA;

					ControllerMap IEnumerator<ControllerMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return SFAVTpYiGcqmKDqgIzDyahWsJMxP;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return SFAVTpYiGcqmKDqgIzDyahWsJMxP;
						}
					}

					[DebuggerHidden]
					public dSPTUGUeyCilpdlfGPsQxKTXBeecA(int P_0)
					{
						TGvyEZskMFxupLUfHbDcEYypjvpX = P_0;
						IrSnwOFaGVuIlxTRlxdcMSwvdsdk = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						kxpWdLnGHmVXAoiMvcGdUQssGDKcA = null;
						XMyqgMYuCzKAAVFXjuDFtLvkHhoV = null;
						TGvyEZskMFxupLUfHbDcEYypjvpX = -2;
					}

					private bool MoveNext()
					{
						int tGvyEZskMFxupLUfHbDcEYypjvpX = TGvyEZskMFxupLUfHbDcEYypjvpX;
						MapHelper mapHelper = pGTnkqNgYNyNspVHWIAXdJpvIhHx;
						if (tGvyEZskMFxupLUfHbDcEYypjvpX != 0)
						{
							if (tGvyEZskMFxupLUfHbDcEYypjvpX != 1)
							{
								return false;
							}
							TGvyEZskMFxupLUfHbDcEYypjvpX = -1;
							ohHiCkVcZFbMnhXwpRxsIuIpxGdJA++;
							goto IL_00e2;
						}
						TGvyEZskMFxupLUfHbDcEYypjvpX = -1;
						if (ReInput._id != mapHelper.MUfpEiqygGgMiNQDuSWClpiuBuic)
						{
							ReInput.CheckInitialized(mapHelper.MUfpEiqygGgMiNQDuSWClpiuBuic);
							return false;
						}
						kxpWdLnGHmVXAoiMvcGdUQssGDKcA = mapHelper.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(nVitdzTMMPVuWLqMlVJmUuFBlwsA);
						pPwDaCPxDeRQmfYNuUIIIZQdiCXe = kxpWdLnGHmVXAoiMvcGdUQssGDKcA.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						cEDyxaeXIAedVdgpYLxBIqRrsPYX = 0;
						goto IL_0107;
						IL_00e2:
						if (ohHiCkVcZFbMnhXwpRxsIuIpxGdJA < NlVgDleuOXcvwFBZcoCXSwGHSitJ)
						{
							SFAVTpYiGcqmKDqgIzDyahWsJMxP = XMyqgMYuCzKAAVFXjuDFtLvkHhoV.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(ohHiCkVcZFbMnhXwpRxsIuIpxGdJA);
							TGvyEZskMFxupLUfHbDcEYypjvpX = 1;
							return true;
						}
						XMyqgMYuCzKAAVFXjuDFtLvkHhoV = null;
						cEDyxaeXIAedVdgpYLxBIqRrsPYX++;
						goto IL_0107;
						IL_0107:
						if (cEDyxaeXIAedVdgpYLxBIqRrsPYX < pPwDaCPxDeRQmfYNuUIIIZQdiCXe)
						{
							XMyqgMYuCzKAAVFXjuDFtLvkHhoV = kxpWdLnGHmVXAoiMvcGdUQssGDKcA.EfgDlYQxDuklXErnyCYTFBkkEVmX(cEDyxaeXIAedVdgpYLxBIqRrsPYX).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
							NlVgDleuOXcvwFBZcoCXSwGHSitJ = XMyqgMYuCzKAAVFXjuDFtLvkHhoV.spuxbZMpjzXXEeAzzgWchppYZEErA;
							ohHiCkVcZFbMnhXwpRxsIuIpxGdJA = 0;
							goto IL_00e2;
						}
						return false;
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
					IEnumerator<ControllerMap> IEnumerable<ControllerMap>.GetEnumerator()
					{
						dSPTUGUeyCilpdlfGPsQxKTXBeecA dSPTUGUeyCilpdlfGPsQxKTXBeecA2;
						if (TGvyEZskMFxupLUfHbDcEYypjvpX == -2 && IrSnwOFaGVuIlxTRlxdcMSwvdsdk == Environment.CurrentManagedThreadId)
						{
							TGvyEZskMFxupLUfHbDcEYypjvpX = 0;
							dSPTUGUeyCilpdlfGPsQxKTXBeecA2 = this;
						}
						else
						{
							dSPTUGUeyCilpdlfGPsQxKTXBeecA2 = new dSPTUGUeyCilpdlfGPsQxKTXBeecA(0);
							dSPTUGUeyCilpdlfGPsQxKTXBeecA2.pGTnkqNgYNyNspVHWIAXdJpvIhHx = pGTnkqNgYNyNspVHWIAXdJpvIhHx;
						}
						dSPTUGUeyCilpdlfGPsQxKTXBeecA2.nVitdzTMMPVuWLqMlVJmUuFBlwsA = ubLgMabYnfwAdVAtrxEAbtLjEVje;
						return dSPTUGUeyCilpdlfGPsQxKTXBeecA2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerMap>)this).GetEnumerator();
					}
				}

				private sealed class cEsMIurEJZBzEnqsJXUWzNZSUuSK : IEnumerable<ControllerMap>, IEnumerable, IEnumerator<ControllerMap>, IEnumerator, IDisposable
				{
					private int QRxgCJjxVVrLuXTjnlkPPAnkAXVH;

					private ControllerMap DqYLJiLJLGAGCZsgSaCAaLdMcBSqA;

					private int QyAQCIeHNcfkLqSqQImnxFQLYrZN;

					public MapHelper DqEvtlIAbTZaHBrIhPceEhsfwGZA;

					private int rcIgPDVMiCgyydfOMOsyYzilWfnNA;

					public int BaEaBKBtzxGmACzYEOgkKDjeHMDjd;

					private int GujFCIRlVnuupcGMTsoikbAolbcU;

					private int syqHzYIlXArLWsUOoLHkCAWwIUdK;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol flCbUeMpyefjnGwjQQvQEmflCUQOA;

					private int CJRCxiVqyvsJgkHYhBUYSjjoiGXM;

					private int rChFcBDHMgHeDXTOpBRqdHUJtcpOB;

					private vHszkbCJdDAIcILHhpVCxcZlIBxlA FZmEvfHatVUNMgAQVjRDhAUaSVtwb;

					private int LYVrDoyaqZGpqJiaJyGVfOAMEtUY;

					private int uZvMEryFZCOoGVoKTmtwfeDmWJQt;

					ControllerMap IEnumerator<ControllerMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return DqYLJiLJLGAGCZsgSaCAaLdMcBSqA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return DqYLJiLJLGAGCZsgSaCAaLdMcBSqA;
						}
					}

					[DebuggerHidden]
					public cEsMIurEJZBzEnqsJXUWzNZSUuSK(int P_0)
					{
						QRxgCJjxVVrLuXTjnlkPPAnkAXVH = P_0;
						QyAQCIeHNcfkLqSqQImnxFQLYrZN = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						flCbUeMpyefjnGwjQQvQEmflCUQOA = null;
						FZmEvfHatVUNMgAQVjRDhAUaSVtwb = null;
						QRxgCJjxVVrLuXTjnlkPPAnkAXVH = -2;
					}

					private bool MoveNext()
					{
						int qRxgCJjxVVrLuXTjnlkPPAnkAXVH = QRxgCJjxVVrLuXTjnlkPPAnkAXVH;
						MapHelper dqEvtlIAbTZaHBrIhPceEhsfwGZA = DqEvtlIAbTZaHBrIhPceEhsfwGZA;
						if (qRxgCJjxVVrLuXTjnlkPPAnkAXVH != 0)
						{
							if (qRxgCJjxVVrLuXTjnlkPPAnkAXVH != 1)
							{
								return false;
							}
							QRxgCJjxVVrLuXTjnlkPPAnkAXVH = -1;
							goto IL_0104;
						}
						QRxgCJjxVVrLuXTjnlkPPAnkAXVH = -1;
						if (ReInput._id != dqEvtlIAbTZaHBrIhPceEhsfwGZA.MUfpEiqygGgMiNQDuSWClpiuBuic)
						{
							ReInput.CheckInitialized(dqEvtlIAbTZaHBrIhPceEhsfwGZA.MUfpEiqygGgMiNQDuSWClpiuBuic);
							return false;
						}
						GujFCIRlVnuupcGMTsoikbAolbcU = dqEvtlIAbTZaHBrIhPceEhsfwGZA.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
						syqHzYIlXArLWsUOoLHkCAWwIUdK = 0;
						goto IL_0161;
						IL_0104:
						uZvMEryFZCOoGVoKTmtwfeDmWJQt++;
						goto IL_0114;
						IL_0161:
						if (syqHzYIlXArLWsUOoLHkCAWwIUdK < GujFCIRlVnuupcGMTsoikbAolbcU)
						{
							flCbUeMpyefjnGwjQQvQEmflCUQOA = dqEvtlIAbTZaHBrIhPceEhsfwGZA.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(syqHzYIlXArLWsUOoLHkCAWwIUdK);
							CJRCxiVqyvsJgkHYhBUYSjjoiGXM = flCbUeMpyefjnGwjQQvQEmflCUQOA.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
							rChFcBDHMgHeDXTOpBRqdHUJtcpOB = 0;
							goto IL_0139;
						}
						return false;
						IL_0114:
						if (uZvMEryFZCOoGVoKTmtwfeDmWJQt < LYVrDoyaqZGpqJiaJyGVfOAMEtUY)
						{
							ControllerMap controllerMap = FZmEvfHatVUNMgAQVjRDhAUaSVtwb.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(uZvMEryFZCOoGVoKTmtwfeDmWJQt);
							if (controllerMap.categoryId == rcIgPDVMiCgyydfOMOsyYzilWfnNA)
							{
								DqYLJiLJLGAGCZsgSaCAaLdMcBSqA = controllerMap;
								QRxgCJjxVVrLuXTjnlkPPAnkAXVH = 1;
								return true;
							}
							goto IL_0104;
						}
						FZmEvfHatVUNMgAQVjRDhAUaSVtwb = null;
						rChFcBDHMgHeDXTOpBRqdHUJtcpOB++;
						goto IL_0139;
						IL_0139:
						if (rChFcBDHMgHeDXTOpBRqdHUJtcpOB < CJRCxiVqyvsJgkHYhBUYSjjoiGXM)
						{
							FZmEvfHatVUNMgAQVjRDhAUaSVtwb = flCbUeMpyefjnGwjQQvQEmflCUQOA.EfgDlYQxDuklXErnyCYTFBkkEVmX(rChFcBDHMgHeDXTOpBRqdHUJtcpOB).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
							LYVrDoyaqZGpqJiaJyGVfOAMEtUY = FZmEvfHatVUNMgAQVjRDhAUaSVtwb.spuxbZMpjzXXEeAzzgWchppYZEErA;
							uZvMEryFZCOoGVoKTmtwfeDmWJQt = 0;
							goto IL_0114;
						}
						flCbUeMpyefjnGwjQQvQEmflCUQOA = null;
						syqHzYIlXArLWsUOoLHkCAWwIUdK++;
						goto IL_0161;
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
					IEnumerator<ControllerMap> IEnumerable<ControllerMap>.GetEnumerator()
					{
						cEsMIurEJZBzEnqsJXUWzNZSUuSK cEsMIurEJZBzEnqsJXUWzNZSUuSK2;
						if (QRxgCJjxVVrLuXTjnlkPPAnkAXVH == -2 && QyAQCIeHNcfkLqSqQImnxFQLYrZN == Environment.CurrentManagedThreadId)
						{
							QRxgCJjxVVrLuXTjnlkPPAnkAXVH = 0;
							cEsMIurEJZBzEnqsJXUWzNZSUuSK2 = this;
						}
						else
						{
							cEsMIurEJZBzEnqsJXUWzNZSUuSK2 = new cEsMIurEJZBzEnqsJXUWzNZSUuSK(0);
							cEsMIurEJZBzEnqsJXUWzNZSUuSK2.DqEvtlIAbTZaHBrIhPceEhsfwGZA = DqEvtlIAbTZaHBrIhPceEhsfwGZA;
						}
						cEsMIurEJZBzEnqsJXUWzNZSUuSK2.rcIgPDVMiCgyydfOMOsyYzilWfnNA = BaEaBKBtzxGmACzYEOgkKDjeHMDjd;
						return cEsMIurEJZBzEnqsJXUWzNZSUuSK2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerMap>)this).GetEnumerator();
					}
				}

				private sealed class usJVhynQQlHUjKXIMokMgwKzycNC<_0001> : IEnumerable<_0001>, IEnumerable, IEnumerator<_0001>, IEnumerator, IDisposable where _0001 : ControllerMap
				{
					private int UffiDoHCKmFORkosnUFiAUioxNw;

					private _0001 wgnhueMdsOteOQvevUyTZRmmFiqj;

					private int BeCKVICBBNqcFefliSOIUnrfvurC;

					public MapHelper HdqRNgadWGTfNvIofxqBboGNUIbf;

					private int QoSFhqeaKFmKFzsKwJIRfQNepPLq;

					public int mEDFGMclzJWyIOAtsviMpMXrAFOO;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol ekbcfQJdnsvBuWaaCRCQOddxrEEEA;

					private int eiQOAjgXzttIJSoYlyHeXFGcDeLH;

					private int qmuXWbScIxNbytcxkCzKVrPoARtp;

					private vHszkbCJdDAIcILHhpVCxcZlIBxlA UNthnTNnRCIHMDvaZHSBiKZaXiMA;

					private int HCGEWGvkftCyVlVNwXBlYvbMvNWI;

					private int uxpZRgIyFcRZsLihRpQHoSckDLct;

					private int bcTAfBclmbNXqcslRbxRKDpwrezm;

					private int afkXzAPecoCYrmxJUnrFcafbTFKV;

					_0001 IEnumerator<_0001>.Current
					{
						[DebuggerHidden]
						get
						{
							return wgnhueMdsOteOQvevUyTZRmmFiqj;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return wgnhueMdsOteOQvevUyTZRmmFiqj;
						}
					}

					[DebuggerHidden]
					public usJVhynQQlHUjKXIMokMgwKzycNC(int P_0)
					{
						UffiDoHCKmFORkosnUFiAUioxNw = P_0;
						BeCKVICBBNqcFefliSOIUnrfvurC = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						ekbcfQJdnsvBuWaaCRCQOddxrEEEA = null;
						UNthnTNnRCIHMDvaZHSBiKZaXiMA = null;
						UffiDoHCKmFORkosnUFiAUioxNw = -2;
					}

					private bool MoveNext()
					{
						int uffiDoHCKmFORkosnUFiAUioxNw = UffiDoHCKmFORkosnUFiAUioxNw;
						MapHelper hdqRNgadWGTfNvIofxqBboGNUIbf = HdqRNgadWGTfNvIofxqBboGNUIbf;
						switch (uffiDoHCKmFORkosnUFiAUioxNw)
						{
						default:
							return false;
						case 0:
						{
							UffiDoHCKmFORkosnUFiAUioxNw = -1;
							if (ReInput._id != hdqRNgadWGTfNvIofxqBboGNUIbf.MUfpEiqygGgMiNQDuSWClpiuBuic)
							{
								ReInput.CheckInitialized(hdqRNgadWGTfNvIofxqBboGNUIbf.MUfpEiqygGgMiNQDuSWClpiuBuic);
								return false;
							}
							if (cVDyIiOsEfJNYzVuZSmuEXqylgT.QcWlAvzQGvfVRlAokvBMpnQELmEj<_0001>(out var _))
							{
								ekbcfQJdnsvBuWaaCRCQOddxrEEEA = hdqRNgadWGTfNvIofxqBboGNUIbf.DtTYEnWeRlrCRbGTGYnarXrLwuZO<_0001>();
								eiQOAjgXzttIJSoYlyHeXFGcDeLH = ekbcfQJdnsvBuWaaCRCQOddxrEEEA.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
								qmuXWbScIxNbytcxkCzKVrPoARtp = 0;
								goto IL_0124;
							}
							eiQOAjgXzttIJSoYlyHeXFGcDeLH = hdqRNgadWGTfNvIofxqBboGNUIbf.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
							qmuXWbScIxNbytcxkCzKVrPoARtp = 0;
							goto IL_0287;
						}
						case 1:
							UffiDoHCKmFORkosnUFiAUioxNw = -1;
							goto IL_00eb;
						case 2:
							{
								UffiDoHCKmFORkosnUFiAUioxNw = -1;
								goto IL_0224;
							}
							IL_0224:
							afkXzAPecoCYrmxJUnrFcafbTFKV++;
							goto IL_0236;
							IL_00eb:
							uxpZRgIyFcRZsLihRpQHoSckDLct++;
							goto IL_00fd;
							IL_0124:
							if (qmuXWbScIxNbytcxkCzKVrPoARtp < eiQOAjgXzttIJSoYlyHeXFGcDeLH)
							{
								UNthnTNnRCIHMDvaZHSBiKZaXiMA = ekbcfQJdnsvBuWaaCRCQOddxrEEEA.EfgDlYQxDuklXErnyCYTFBkkEVmX(qmuXWbScIxNbytcxkCzKVrPoARtp).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
								HCGEWGvkftCyVlVNwXBlYvbMvNWI = UNthnTNnRCIHMDvaZHSBiKZaXiMA.spuxbZMpjzXXEeAzzgWchppYZEErA;
								uxpZRgIyFcRZsLihRpQHoSckDLct = 0;
								goto IL_00fd;
							}
							ekbcfQJdnsvBuWaaCRCQOddxrEEEA = null;
							break;
							IL_0287:
							if (qmuXWbScIxNbytcxkCzKVrPoARtp >= eiQOAjgXzttIJSoYlyHeXFGcDeLH)
							{
								break;
							}
							ekbcfQJdnsvBuWaaCRCQOddxrEEEA = hdqRNgadWGTfNvIofxqBboGNUIbf.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(qmuXWbScIxNbytcxkCzKVrPoARtp);
							HCGEWGvkftCyVlVNwXBlYvbMvNWI = ekbcfQJdnsvBuWaaCRCQOddxrEEEA.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
							uxpZRgIyFcRZsLihRpQHoSckDLct = 0;
							goto IL_025d;
							IL_0236:
							if (afkXzAPecoCYrmxJUnrFcafbTFKV < bcTAfBclmbNXqcslRbxRKDpwrezm)
							{
								if (UNthnTNnRCIHMDvaZHSBiKZaXiMA.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(afkXzAPecoCYrmxJUnrFcafbTFKV) is _0001 val && val.categoryId == QoSFhqeaKFmKFzsKwJIRfQNepPLq)
								{
									wgnhueMdsOteOQvevUyTZRmmFiqj = val;
									UffiDoHCKmFORkosnUFiAUioxNw = 2;
									return true;
								}
								goto IL_0224;
							}
							UNthnTNnRCIHMDvaZHSBiKZaXiMA = null;
							uxpZRgIyFcRZsLihRpQHoSckDLct++;
							goto IL_025d;
							IL_00fd:
							if (uxpZRgIyFcRZsLihRpQHoSckDLct < HCGEWGvkftCyVlVNwXBlYvbMvNWI)
							{
								ControllerMap controllerMap = UNthnTNnRCIHMDvaZHSBiKZaXiMA.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(uxpZRgIyFcRZsLihRpQHoSckDLct);
								if (controllerMap.categoryId == QoSFhqeaKFmKFzsKwJIRfQNepPLq)
								{
									wgnhueMdsOteOQvevUyTZRmmFiqj = (_0001)controllerMap;
									UffiDoHCKmFORkosnUFiAUioxNw = 1;
									return true;
								}
								goto IL_00eb;
							}
							UNthnTNnRCIHMDvaZHSBiKZaXiMA = null;
							qmuXWbScIxNbytcxkCzKVrPoARtp++;
							goto IL_0124;
							IL_025d:
							if (uxpZRgIyFcRZsLihRpQHoSckDLct < HCGEWGvkftCyVlVNwXBlYvbMvNWI)
							{
								UNthnTNnRCIHMDvaZHSBiKZaXiMA = ekbcfQJdnsvBuWaaCRCQOddxrEEEA.EfgDlYQxDuklXErnyCYTFBkkEVmX(uxpZRgIyFcRZsLihRpQHoSckDLct).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
								bcTAfBclmbNXqcslRbxRKDpwrezm = UNthnTNnRCIHMDvaZHSBiKZaXiMA.spuxbZMpjzXXEeAzzgWchppYZEErA;
								afkXzAPecoCYrmxJUnrFcafbTFKV = 0;
								goto IL_0236;
							}
							ekbcfQJdnsvBuWaaCRCQOddxrEEEA = null;
							qmuXWbScIxNbytcxkCzKVrPoARtp++;
							goto IL_0287;
						}
						return false;
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
					IEnumerator<_0001> IEnumerable<_0001>.GetEnumerator()
					{
						usJVhynQQlHUjKXIMokMgwKzycNC<_0001> usJVhynQQlHUjKXIMokMgwKzycNC2;
						if (UffiDoHCKmFORkosnUFiAUioxNw == -2 && BeCKVICBBNqcFefliSOIUnrfvurC == Environment.CurrentManagedThreadId)
						{
							UffiDoHCKmFORkosnUFiAUioxNw = 0;
							usJVhynQQlHUjKXIMokMgwKzycNC2 = this;
						}
						else
						{
							usJVhynQQlHUjKXIMokMgwKzycNC2 = new usJVhynQQlHUjKXIMokMgwKzycNC<_0001>(0);
							usJVhynQQlHUjKXIMokMgwKzycNC2.HdqRNgadWGTfNvIofxqBboGNUIbf = HdqRNgadWGTfNvIofxqBboGNUIbf;
						}
						usJVhynQQlHUjKXIMokMgwKzycNC2.QoSFhqeaKFmKFzsKwJIRfQNepPLq = mEDFGMclzJWyIOAtsviMpMXrAFOO;
						return usJVhynQQlHUjKXIMokMgwKzycNC2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<_0001>)this).GetEnumerator();
					}
				}

				private sealed class ShUoKetzrHBqxYpybfbcjKqImXNY : IEnumerable<ControllerMap>, IEnumerable, IEnumerator<ControllerMap>, IEnumerator, IDisposable
				{
					private int yMvfrfDHyMDLOokYbDnnNortMVOj;

					private ControllerMap pKrgcaHXQvIYdjysRtgqsQVbMMvWA;

					private int HBioAGzEXrxgeQWeiSwTyufxuiWJ;

					public MapHelper DKYOPTOGIenmxgcRnluicjlHvcfg;

					private ControllerType liIVpdJpdySZQSSTbJZeeQGglEVw;

					public ControllerType bIOMbVmtKCxukCzZGEohNbmZXaqG;

					private int tmmEkKltEbkKPCmhyGoozckEdgJo;

					public int uceOZDOFOCADFAgcbJQxpDbwkKSgb;

					private rJRHfxObWEyZQOmmYgoxgmGnxuol URRhblSGgZGvoBqXtWuDFOXQmuUIA;

					private int cfiCPHGFkpOfKrpgsldzYRGOXGFaA;

					private int kwYtiwaeLSYmNpvKUEsWmRfimMcd;

					private vHszkbCJdDAIcILHhpVCxcZlIBxlA EfApDQZYKmLNiLeTaMMHyzvkvyNd;

					private int ETxZdTBXydarbAqBoqSrMrdUTUpA;

					private int FIVwBhqVWbkUdqAKdHhJtOBjrdrX;

					ControllerMap IEnumerator<ControllerMap>.Current
					{
						[DebuggerHidden]
						get
						{
							return pKrgcaHXQvIYdjysRtgqsQVbMMvWA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return pKrgcaHXQvIYdjysRtgqsQVbMMvWA;
						}
					}

					[DebuggerHidden]
					public ShUoKetzrHBqxYpybfbcjKqImXNY(int P_0)
					{
						yMvfrfDHyMDLOokYbDnnNortMVOj = P_0;
						HBioAGzEXrxgeQWeiSwTyufxuiWJ = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						URRhblSGgZGvoBqXtWuDFOXQmuUIA = null;
						EfApDQZYKmLNiLeTaMMHyzvkvyNd = null;
						yMvfrfDHyMDLOokYbDnnNortMVOj = -2;
					}

					private bool MoveNext()
					{
						int num = yMvfrfDHyMDLOokYbDnnNortMVOj;
						MapHelper dKYOPTOGIenmxgcRnluicjlHvcfg = DKYOPTOGIenmxgcRnluicjlHvcfg;
						if (num != 0)
						{
							if (num != 1)
							{
								return false;
							}
							yMvfrfDHyMDLOokYbDnnNortMVOj = -1;
							goto IL_00e2;
						}
						yMvfrfDHyMDLOokYbDnnNortMVOj = -1;
						if (ReInput._id != dKYOPTOGIenmxgcRnluicjlHvcfg.MUfpEiqygGgMiNQDuSWClpiuBuic)
						{
							ReInput.CheckInitialized(dKYOPTOGIenmxgcRnluicjlHvcfg.MUfpEiqygGgMiNQDuSWClpiuBuic);
							return false;
						}
						URRhblSGgZGvoBqXtWuDFOXQmuUIA = dKYOPTOGIenmxgcRnluicjlHvcfg.MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(liIVpdJpdySZQSSTbJZeeQGglEVw);
						cfiCPHGFkpOfKrpgsldzYRGOXGFaA = URRhblSGgZGvoBqXtWuDFOXQmuUIA.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						kwYtiwaeLSYmNpvKUEsWmRfimMcd = 0;
						goto IL_0117;
						IL_00f2:
						if (FIVwBhqVWbkUdqAKdHhJtOBjrdrX < ETxZdTBXydarbAqBoqSrMrdUTUpA)
						{
							ControllerMap controllerMap = EfApDQZYKmLNiLeTaMMHyzvkvyNd.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(FIVwBhqVWbkUdqAKdHhJtOBjrdrX);
							if (controllerMap.categoryId == tmmEkKltEbkKPCmhyGoozckEdgJo)
							{
								pKrgcaHXQvIYdjysRtgqsQVbMMvWA = controllerMap;
								yMvfrfDHyMDLOokYbDnnNortMVOj = 1;
								return true;
							}
							goto IL_00e2;
						}
						EfApDQZYKmLNiLeTaMMHyzvkvyNd = null;
						kwYtiwaeLSYmNpvKUEsWmRfimMcd++;
						goto IL_0117;
						IL_00e2:
						FIVwBhqVWbkUdqAKdHhJtOBjrdrX++;
						goto IL_00f2;
						IL_0117:
						if (kwYtiwaeLSYmNpvKUEsWmRfimMcd < cfiCPHGFkpOfKrpgsldzYRGOXGFaA)
						{
							EfApDQZYKmLNiLeTaMMHyzvkvyNd = URRhblSGgZGvoBqXtWuDFOXQmuUIA.EfgDlYQxDuklXErnyCYTFBkkEVmX(kwYtiwaeLSYmNpvKUEsWmRfimMcd).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
							ETxZdTBXydarbAqBoqSrMrdUTUpA = EfApDQZYKmLNiLeTaMMHyzvkvyNd.spuxbZMpjzXXEeAzzgWchppYZEErA;
							FIVwBhqVWbkUdqAKdHhJtOBjrdrX = 0;
							goto IL_00f2;
						}
						return false;
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
					IEnumerator<ControllerMap> IEnumerable<ControllerMap>.GetEnumerator()
					{
						ShUoKetzrHBqxYpybfbcjKqImXNY shUoKetzrHBqxYpybfbcjKqImXNY;
						if (yMvfrfDHyMDLOokYbDnnNortMVOj == -2 && HBioAGzEXrxgeQWeiSwTyufxuiWJ == Environment.CurrentManagedThreadId)
						{
							yMvfrfDHyMDLOokYbDnnNortMVOj = 0;
							shUoKetzrHBqxYpybfbcjKqImXNY = this;
						}
						else
						{
							shUoKetzrHBqxYpybfbcjKqImXNY = new ShUoKetzrHBqxYpybfbcjKqImXNY(0);
							shUoKetzrHBqxYpybfbcjKqImXNY.DKYOPTOGIenmxgcRnluicjlHvcfg = DKYOPTOGIenmxgcRnluicjlHvcfg;
						}
						shUoKetzrHBqxYpybfbcjKqImXNY.tmmEkKltEbkKPCmhyGoozckEdgJo = uceOZDOFOCADFAgcbJQxpDbwkKSgb;
						shUoKetzrHBqxYpybfbcjKqImXNY.liIVpdJpdySZQSSTbJZeeQGglEVw = bIOMbVmtKCxukCzZGEohNbmZXaqG;
						return shUoKetzrHBqxYpybfbcjKqImXNY;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerMap>)this).GetEnumerator();
					}
				}

				private readonly EhUOIcAWtPzjNpBYLAHyNaaERzaE bRjrQqMfyTYOXVFtOrygKGRGMxNN;

				private Player hIUdxPiJkzzwMniBaGspAmVWYBpDA;

				private ControllerHelper MNHGNJfFQqpihvuAqNaSqIrXWcffA;

				private readonly ControllerMapEnabler BzoEVmkFzWoLiXDbvANzqcaxGILS;

				private readonly ControllerMapLayoutManager XBGmqYuRMPcbxGurrHRZtTaSFlQO;

				private readonly int MUfpEiqygGgMiNQDuSWClpiuBuic;

				public ControllerMapLayoutManager layoutManager => XBGmqYuRMPcbxGurrHRZtTaSFlQO;

				public ControllerMapEnabler mapEnabler => BzoEVmkFzWoLiXDbvANzqcaxGILS;

				public IList<InputBehavior> InputBehaviors
				{
					get
					{
						if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
						{
							ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
							return EmptyObjects<InputBehavior>.EmptyReadOnlyIListT;
						}
						return hIUdxPiJkzzwMniBaGspAmVWYBpDA.XCFDVqZUbyGILGzarkLNUnbiMkmM.UkGZIfhaCReBibQxxKNQcxuisNEb(hIUdxPiJkzzwMniBaGspAmVWYBpDA.mgTogZEAHwpJMhbsccjZDcKdOLwp);
					}
				}

				internal MapHelper(Player P_0, ControllerHelper P_1, EhUOIcAWtPzjNpBYLAHyNaaERzaE P_2, ControllerMapLayoutManager.ICvEqTkajGQZLXkOyPTomcjaeCWs P_3, ControllerMapEnabler.CyXCJRXKNwVfGTMPVLIRUynNmCKI P_4)
				{
					MUfpEiqygGgMiNQDuSWClpiuBuic = ReInput.id;
					hIUdxPiJkzzwMniBaGspAmVWYBpDA = P_0;
					MNHGNJfFQqpihvuAqNaSqIrXWcffA = P_1;
					bRjrQqMfyTYOXVFtOrygKGRGMxNN = P_2;
					BzoEVmkFzWoLiXDbvANzqcaxGILS = new ControllerMapEnabler(P_0, P_4);
					XBGmqYuRMPcbxGurrHRZtTaSFlQO = new ControllerMapLayoutManager(P_0, P_3);
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.cwMWXWydqGuwHwMYdesaMfzxTQMK += BzoEVmkFzWoLiXDbvANzqcaxGILS.Apply;
				}

				public void LoadMap<T>(int controllerId, int categoryId, int layoutId) where T : ControllerMap
				{
					fDEMTDNJGxNdaXonbzHfJRnmUfBA<T>(controllerId, categoryId, layoutId, BoolOption.Default);
				}

				public void LoadMap<T>(int controllerId, string categoryName, string layoutName) where T : ControllerMap
				{
					lbVdEsUoGYKVeHSSSEzwEaaDKfHhb<T>(controllerId, categoryName, layoutName, BoolOption.Default);
				}

				public void LoadMap(ControllerType controllerType, int controllerId, int categoryId, int layoutId)
				{
					uGUxIYoqjuFNSRfBYpRKEzUbBeer(controllerType, controllerId, categoryId, layoutId, BoolOption.Default);
				}

				public void LoadMap(ControllerType controllerType, int controllerId, string categoryName, string layoutName)
				{
					RAcFvdBFxVZCBPRoJBjTtgdIQzsX(controllerType, controllerId, categoryName, layoutName, BoolOption.Default);
				}

				public void LoadMap<T>(int controllerId, int categoryId, int layoutId, bool startEnabled) where T : ControllerMap
				{
					fDEMTDNJGxNdaXonbzHfJRnmUfBA<T>(controllerId, categoryId, layoutId, startEnabled ? BoolOption.True : BoolOption.False);
				}

				public void LoadMap<T>(int controllerId, string categoryName, string layoutName, bool startEnabled) where T : ControllerMap
				{
					lbVdEsUoGYKVeHSSSEzwEaaDKfHhb<T>(controllerId, categoryName, layoutName, startEnabled ? BoolOption.True : BoolOption.False);
				}

				public void LoadMap(ControllerType controllerType, int controllerId, int categoryId, int layoutId, bool startEnabled)
				{
					uGUxIYoqjuFNSRfBYpRKEzUbBeer(controllerType, controllerId, categoryId, layoutId, startEnabled ? BoolOption.True : BoolOption.False);
				}

				public void LoadMap(ControllerType controllerType, int controllerId, string categoryName, string layoutName, bool startEnabled)
				{
					RAcFvdBFxVZCBPRoJBjTtgdIQzsX(controllerType, controllerId, categoryName, layoutName, startEnabled ? BoolOption.True : BoolOption.False);
				}

				private void fDEMTDNJGxNdaXonbzHfJRnmUfBA<_0001>(int P_0, int P_1, int P_2, BoolOption P_3) where _0001 : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						CrgZgEQBnsBvSiIGZqpYlKdxIxUj(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<_0001>(), P_0, P_1, P_2, P_3);
					}
				}

				private void lbVdEsUoGYKVeHSSSEzwEaaDKfHhb<_0001>(int P_0, string P_1, string P_2, BoolOption P_3) where _0001 : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						JNxeEmFtrgQqskRAfoSbnbXngnlDb(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<_0001>(), P_0, P_1, P_2, P_3);
					}
				}

				private void uGUxIYoqjuFNSRfBYpRKEzUbBeer(ControllerType P_0, int P_1, int P_2, int P_3, BoolOption P_4)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						CrgZgEQBnsBvSiIGZqpYlKdxIxUj(P_0, P_1, P_2, P_3, P_4);
					}
				}

				private void RAcFvdBFxVZCBPRoJBjTtgdIQzsX(ControllerType P_0, int P_1, string P_2, string P_3, BoolOption P_4)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						JNxeEmFtrgQqskRAfoSbnbXngnlDb(P_0, P_1, P_2, P_3, P_4);
					}
				}

				[IteratorStateMachine(typeof(WzEAgduqTLDKfjHVZSjFtsKwyODc))]
				public IEnumerable<ControllerMap> GetAllMaps()
				{
					return new WzEAgduqTLDKfjHVZSjFtsKwyODc(-2)
					{
						GdHETqZfakHrEiGyHwpjSvCfRQrIA = this
					};
				}

				public int GetAllMaps(List<ControllerMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (results == null)
					{
						throw new ArgumentNullException("results");
					}
					results.Clear();
					int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i);
						int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						for (int j = 0; j < num; j++)
						{
							rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(j).UEDbvZCpORwBphRIdGLFIEwJLiiEb.nsMbfTaPMxQilFsNrvmdwnPkAAXPA(results, true);
						}
					}
					return results.Count;
				}

				[IteratorStateMachine(typeof(HKBbRTEaiQMSxzCxSZiWKMEyaBHAb))]
				public IEnumerable<T> GetAllMaps<T>() where T : ControllerMap
				{
					return new HKBbRTEaiQMSxzCxSZiWKMEyaBHAb<T>(-2)
					{
						kWRMzerdnHOrNZuQUaYqqolHZYqg = this
					};
				}

				public int GetAllMaps<T>(List<T> results) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (results == null)
					{
						throw new ArgumentNullException("results");
					}
					results.Clear();
					if (cVDyIiOsEfJNYzVuZSmuEXqylgT.QcWlAvzQGvfVRlAokvBMpnQELmEj<T>(out var controllerType))
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
						int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						for (int i = 0; i < num; i++)
						{
							rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.yJMjLAllVdaPdGYYMTKyjJEIiOpR(results, true);
						}
					}
					else
					{
						int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
						for (int j = 0; j < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; j++)
						{
							rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol3 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(j);
							int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol3.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
							for (int k = 0; k < num2; k++)
							{
								rJRHfxObWEyZQOmmYgoxgmGnxuol3.EfgDlYQxDuklXErnyCYTFBkkEVmX(k).UEDbvZCpORwBphRIdGLFIEwJLiiEb.yJMjLAllVdaPdGYYMTKyjJEIiOpR(results, true);
							}
						}
					}
					return results.Count;
				}

				[IteratorStateMachine(typeof(dSPTUGUeyCilpdlfGPsQxKTXBeecA))]
				public IEnumerable<ControllerMap> GetAllMaps(ControllerType controllerType)
				{
					return new dSPTUGUeyCilpdlfGPsQxKTXBeecA(-2)
					{
						pGTnkqNgYNyNspVHWIAXdJpvIhHx = this,
						ubLgMabYnfwAdVAtrxEAbtLjEVje = controllerType
					};
				}

				public int GetAllMaps(ControllerType controllerType, List<ControllerMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (results == null)
					{
						throw new ArgumentNullException("results");
					}
					results.Clear();
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
					for (int i = 0; i < num; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.nsMbfTaPMxQilFsNrvmdwnPkAAXPA(results, true);
					}
					return results.Count;
				}

				public IEnumerable<ControllerMap> GetAllMapsInCategory(string categoryName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return new List<ControllerMap>();
					}
					return GetAllMapsInCategory(mapCategoryId);
				}

				[IteratorStateMachine(typeof(cEsMIurEJZBzEnqsJXUWzNZSUuSK))]
				public IEnumerable<ControllerMap> GetAllMapsInCategory(int categoryId)
				{
					return new cEsMIurEJZBzEnqsJXUWzNZSUuSK(-2)
					{
						DqEvtlIAbTZaHBrIhPceEhsfwGZA = this,
						BaEaBKBtzxGmACzYEOgkKDjeHMDjd = categoryId
					};
				}

				public IEnumerable<T> GetAllMapsInCategory<T>(string categoryName) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<T>.EmptyReadOnlyIListT;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return EmptyObjects<T>.EmptyReadOnlyIListT;
					}
					return GetAllMapsInCategory<T>(mapCategoryId);
				}

				[IteratorStateMachine(typeof(usJVhynQQlHUjKXIMokMgwKzycNC))]
				public IEnumerable<T> GetAllMapsInCategory<T>(int categoryId) where T : ControllerMap
				{
					return new usJVhynQQlHUjKXIMokMgwKzycNC<T>(-2)
					{
						HdqRNgadWGTfNvIofxqBboGNUIbf = this,
						mEDFGMclzJWyIOAtsviMpMXrAFOO = categoryId
					};
				}

				public IEnumerable<ControllerMap> GetAllMapsInCategory(string categoryName, ControllerType controllerType)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return new List<ControllerMap>();
					}
					return GetAllMapsInCategory(mapCategoryId, controllerType);
				}

				[IteratorStateMachine(typeof(ShUoKetzrHBqxYpybfbcjKqImXNY))]
				public IEnumerable<ControllerMap> GetAllMapsInCategory(int categoryId, ControllerType controllerType)
				{
					return new ShUoKetzrHBqxYpybfbcjKqImXNY(-2)
					{
						DKYOPTOGIenmxgcRnluicjlHvcfg = this,
						uceOZDOFOCADFAgcbJQxpDbwkKSgb = categoryId,
						bIOMbVmtKCxukCzZGEohNbmZXaqG = controllerType
					};
				}

				public int GetAllMapsInCategory(string categoryName, List<ControllerMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					return GetAllMapsInCategory(mapCategoryId, results);
				}

				public int GetAllMapsInCategory(int categoryId, List<ControllerMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (results == null)
					{
						throw new ArgumentNullException("results");
					}
					results.Clear();
					if (ReInput.mapping.GetMapCategory(categoryId) == null)
					{
						return 0;
					}
					int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i);
						int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						for (int j = 0; j < num; j++)
						{
							rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(j).UEDbvZCpORwBphRIdGLFIEwJLiiEb.nmtgykGGQmbTkMLvRzaknpIpupeMA(categoryId, results, true);
						}
					}
					return results.Count;
				}

				public int GetAllMapsInCategory<T>(string categoryName, List<T> results) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					return GetAllMapsInCategory(mapCategoryId, results);
				}

				public int GetAllMapsInCategory<T>(int categoryId, List<T> results) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (results == null)
					{
						throw new ArgumentNullException("results");
					}
					results.Clear();
					if (ReInput.mapping.GetMapCategory(categoryId) == null)
					{
						return 0;
					}
					if (cVDyIiOsEfJNYzVuZSmuEXqylgT.QcWlAvzQGvfVRlAokvBMpnQELmEj<T>(out var controllerType))
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
						int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						for (int i = 0; i < num; i++)
						{
							rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.SdKKnuHihxdoLtHGZLxXiMEpbfhj(categoryId, results, true);
						}
					}
					else
					{
						int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
						for (int j = 0; j < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; j++)
						{
							rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol3 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(j);
							int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol3.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
							for (int k = 0; k < num2; k++)
							{
								rJRHfxObWEyZQOmmYgoxgmGnxuol3.EfgDlYQxDuklXErnyCYTFBkkEVmX(k).UEDbvZCpORwBphRIdGLFIEwJLiiEb.SdKKnuHihxdoLtHGZLxXiMEpbfhj(categoryId, results, true);
							}
						}
					}
					return results.Count;
				}

				public int GetAllMapsInCategory(string categoryName, ControllerType controllerType, List<ControllerMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					return GetAllMapsInCategory(mapCategoryId, controllerType, results);
				}

				public int GetAllMapsInCategory(int categoryId, ControllerType controllerType, List<ControllerMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (results == null)
					{
						throw new ArgumentNullException("results");
					}
					results.Clear();
					if (ReInput.mapping.GetMapCategory(categoryId) == null)
					{
						return 0;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
					for (int i = 0; i < num; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.nmtgykGGQmbTkMLvRzaknpIpupeMA(categoryId, results, true);
					}
					return results.Count;
				}

				public IList<T> GetMaps<T>(int controllerId) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<T>.EmptyReadOnlyIListT;
					}
					return dMaatCfuRlkAbWjMsVoZOuiINcOh<T>(controllerId);
				}

				public IList<ControllerMap> GetMaps(ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					return XnHjpyvQzbZqAAKeLfiOTiCoWFVk(controllerType, controllerId);
				}

				public IList<ControllerMap> GetMaps(Controller controller)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					if (controller == null)
					{
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					return GetMaps(controller.type, controller.id);
				}

				public IEnumerable<ControllerMap> GetMapsInCategory(ControllerType controllerType, int controllerId, int categoryId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					if (controllerId < 0 || categoryId < 0)
					{
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					if (ReInput.mapping.GetMapCategory(categoryId) == null)
					{
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					return rRcAfuNJBsgvtVQYMgcaAAUfQdZJA(controllerType, controllerId, categoryId);
				}

				public IEnumerable<ControllerMap> GetMapsInCategory(ControllerType controllerType, int controllerId, string categoryName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					return GetMapsInCategory(controllerType, controllerId, mapCategoryId);
				}

				public IEnumerable<ControllerMap> GetMapsInCategory(Controller controller, int categoryId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					if (controller == null)
					{
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					return GetMapsInCategory(controller.type, controller.id, categoryId);
				}

				public IEnumerable<ControllerMap> GetMapsInCategory(Controller controller, string categoryName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					if (controller == null)
					{
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					return GetMapsInCategory(controller.type, controller.id, mapCategoryId);
				}

				public int GetMapsInCategory(ControllerType controllerType, int controllerId, int categoryId, List<ControllerMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					ListTools.TryClear(results);
					if (results == null)
					{
						throw new ArgumentNullException("results");
					}
					if (controllerId < 0 || categoryId < 0)
					{
						return 0;
					}
					if (ReInput.mapping.GetMapCategory(categoryId) == null)
					{
						return 0;
					}
					return MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType).eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(controllerId)?.UEDbvZCpORwBphRIdGLFIEwJLiiEb.nmtgykGGQmbTkMLvRzaknpIpupeMA(categoryId, results, false) ?? 0;
				}

				public int GetMapsInCategory(ControllerType controllerType, int controllerId, string categoryName, List<ControllerMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					ListTools.TryClear(results);
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					return GetMapsInCategory(controllerType, controllerId, mapCategoryId, results);
				}

				public int GetMapsInCategory(Controller controller, int categoryId, List<ControllerMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					ListTools.TryClear(results);
					if (controller == null)
					{
						return 0;
					}
					return GetMapsInCategory(controller.type, controller.id, categoryId, results);
				}

				public int GetMapsInCategory(Controller controller, string categoryName, List<ControllerMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					ListTools.TryClear(results);
					if (controller == null)
					{
						return 0;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					return GetMapsInCategory(controller.type, controller.id, mapCategoryId, results);
				}

				public IEnumerable<T> GetMapsInCategory<T>(int controllerId, int categoryId) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<T>.EmptyReadOnlyIListT;
					}
					return EuuNGZRSZapFoCLBQiDZuQiKoDTR<T>(controllerId, categoryId);
				}

				public IEnumerable<T> GetMapsInCategory<T>(int controllerId, string categoryName) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<T>.EmptyReadOnlyIListT;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return EmptyObjects<T>.EmptyReadOnlyIListT;
					}
					return GetMapsInCategory<T>(controllerId, mapCategoryId);
				}

				public int GetMapsInCategory<T>(int controllerId, int categoryId, List<T> results) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (results == null)
					{
						throw new ArgumentNullException("results");
					}
					results.Clear();
					if (ReInput.mapping.GetMapCategory(categoryId) == null)
					{
						return 0;
					}
					azzemdvQpsbVWyBpuJprUkaWgEfEA azzemdvQpsbVWyBpuJprUkaWgEfEA2 = DtTYEnWeRlrCRbGTGYnarXrLwuZO<T>().eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(controllerId);
					if (azzemdvQpsbVWyBpuJprUkaWgEfEA2 == null)
					{
						return 0;
					}
					azzemdvQpsbVWyBpuJprUkaWgEfEA2.UEDbvZCpORwBphRIdGLFIEwJLiiEb.SdKKnuHihxdoLtHGZLxXiMEpbfhj(categoryId, results, true);
					return results.Count;
				}

				public int GetMapsInCategory<T>(int controllerId, string categoryName, List<T> results) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					ListTools.TryClear(results);
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					return GetMapsInCategory(controllerId, mapCategoryId, results);
				}

				public T GetMap<T>(int controllerId, int mapId) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (mapId < 0)
					{
						return null;
					}
					return (T)YKTVpqEFpLQdHdolBulaVqDyFLaA(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, mapId);
				}

				public T GetMap<T>(int controllerId, int categoryId, int layoutId) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (categoryId < 0 || layoutId < 0)
					{
						return null;
					}
					return (T)NujCAmEoYXGYxJUyMGLRfuIAJRwGb(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, categoryId, layoutId);
				}

				public T GetMap<T>(int controllerId, string categoryName, string layoutName) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					return (T)wBhHRmKGdrAKeeLguDRoAykGfDeA(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, categoryName, layoutName);
				}

				public ControllerMap GetMap(int mapId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (mapId < 0)
					{
						return null;
					}
					int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i);
						int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						for (int j = 0; j < num; j++)
						{
							ControllerMap controllerMap = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(j).UEDbvZCpORwBphRIdGLFIEwJLiiEb.kFWDBYhsObxbfnNZhRbLDvMmwCaq(mapId);
							if (controllerMap != null)
							{
								return controllerMap;
							}
						}
					}
					return null;
				}

				public ControllerMap GetMap(ControllerType controllerType, int controllerId, int mapId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (mapId < 0)
					{
						return null;
					}
					return YKTVpqEFpLQdHdolBulaVqDyFLaA(controllerType, controllerId, mapId);
				}

				public ControllerMap GetMap(ControllerType controllerType, int controllerId, int categoryId, int layoutId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (categoryId < 0 || layoutId < 0)
					{
						return null;
					}
					return NujCAmEoYXGYxJUyMGLRfuIAJRwGb(controllerType, controllerId, categoryId, layoutId);
				}

				public ControllerMap GetMap(ControllerType controllerType, int controllerId, string categoryName, string layoutName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					return wBhHRmKGdrAKeeLguDRoAykGfDeA(controllerType, controllerId, categoryName, layoutName);
				}

				public ControllerMap GetMap(Controller controller, int mapId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (controller == null)
					{
						return null;
					}
					return GetMap(controller.type, controller.id, mapId);
				}

				public ControllerMap GetMap(Controller controller, int categoryId, int layoutId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (controller == null)
					{
						return null;
					}
					return GetMap(controller.type, controller.id, categoryId, layoutId);
				}

				public ControllerMap GetMap(Controller controller, string categoryName, string layoutName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (controller == null)
					{
						return null;
					}
					return GetMap(controller.type, controller.id, categoryName, layoutName);
				}

				public T GetFirstMapInCategory<T>(int controllerId, string categoryName) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return null;
					}
					return GetFirstMapInCategory<T>(controllerId, mapCategoryId);
				}

				public T GetFirstMapInCategory<T>(int controllerId, int categoryId) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (categoryId < 0)
					{
						return null;
					}
					return (T)QSfOEAPjfqkNbtnmbYDefjqgzntE(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, categoryId);
				}

				public ControllerMap GetFirstMapInCategory(ControllerType controllerType, int controllerId, string categoryName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return null;
					}
					return GetFirstMapInCategory(controllerType, controllerId, mapCategoryId);
				}

				public ControllerMap GetFirstMapInCategory(ControllerType controllerType, int controllerId, int categoryId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (categoryId < 0)
					{
						return null;
					}
					return QSfOEAPjfqkNbtnmbYDefjqgzntE(controllerType, controllerId, categoryId);
				}

				public ControllerMap GetFirstMapInCategory(Controller controller, string categoryName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (controller == null)
					{
						return null;
					}
					return GetFirstMapInCategory(controller.type, controller.id, categoryName);
				}

				public ControllerMap GetFirstMapInCategory(Controller controller, int categoryId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (controller == null)
					{
						return null;
					}
					return GetFirstMapInCategory(controller.type, controller.id, categoryId);
				}

				public void AddMap<T>(int controllerId, ControllerMap map) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						IVStlYfQmoamAgebBlgGsChGRtSbA(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, map, BoolOption.Default);
					}
				}

				public void AddMap(Controller controller, ControllerMap map)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						DLGfgqBcXWhBlpiKNrbryajeUSgc(controller, map, BoolOption.Default);
					}
				}

				public void AddMap(ControllerType controllerType, int controllerId, ControllerMap map)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						IVStlYfQmoamAgebBlgGsChGRtSbA(controllerType, controllerId, map, BoolOption.Default);
					}
				}

				public void AddMap<T>(int controllerId, ControllerMap map, bool startEnabled) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						IVStlYfQmoamAgebBlgGsChGRtSbA(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, map, startEnabled ? BoolOption.True : BoolOption.False);
					}
				}

				public void AddMap(Controller controller, ControllerMap map, bool startEnabled)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						DLGfgqBcXWhBlpiKNrbryajeUSgc(controller, map, startEnabled ? BoolOption.True : BoolOption.False);
					}
				}

				public void AddMap(ControllerType controllerType, int controllerId, ControllerMap map, bool startEnabled)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						IVStlYfQmoamAgebBlgGsChGRtSbA(controllerType, controllerId, map, startEnabled ? BoolOption.True : BoolOption.False);
					}
				}

				public bool AddMapFromXml<T>(int controllerId, string xmlString) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return false;
					}
					return AlwEHyJFJsloIaMuDLeZemNLLLalB(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, xmlString);
				}

				public bool AddMapFromXml(ControllerType controllerType, int controllerId, string xmlString)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return false;
					}
					return AlwEHyJFJsloIaMuDLeZemNLLLalB(controllerType, controllerId, xmlString);
				}

				public int AddMapsFromXml<T>(int controllerId, List<string> xmlStrings) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (xmlStrings == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < xmlStrings.Count; i++)
					{
						if (AddMapFromXml<T>(controllerId, xmlStrings[i]))
						{
							num++;
						}
					}
					return num;
				}

				public int AddMapsFromXml(ControllerType controllerType, int controllerId, List<string> xmlStrings)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (xmlStrings == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < xmlStrings.Count; i++)
					{
						if (AddMapFromXml(controllerType, controllerId, xmlStrings[i]))
						{
							num++;
						}
					}
					return num;
				}

				public bool AddMapFromJson<T>(int controllerId, string jsonString) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return false;
					}
					return DfyCcjSqFpBGtCUaefWhtBtspXTY(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, jsonString);
				}

				public bool AddMapFromJson(ControllerType controllerType, int controllerId, string jsonString)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return false;
					}
					return DfyCcjSqFpBGtCUaefWhtBtspXTY(controllerType, controllerId, jsonString);
				}

				public int AddMapsFromJson<T>(int controllerId, List<string> jsonStrings) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (jsonStrings == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < jsonStrings.Count; i++)
					{
						if (AddMapFromJson<T>(controllerId, jsonStrings[i]))
						{
							num++;
						}
					}
					return num;
				}

				public int AddMapsFromJson(ControllerType controllerType, int controllerId, List<string> jsonStrings)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (jsonStrings == null)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < jsonStrings.Count; i++)
					{
						if (AddMapFromJson(controllerType, controllerId, jsonStrings[i]))
						{
							num++;
						}
					}
					return num;
				}

				public void AddEmptyMap<T>(int controllerId, int categoryId, int layoutId) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						ZcchzppkAZBRQPJLQCANJsaeeKs(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, categoryId, layoutId);
					}
				}

				public void AddEmptyMap<T>(int controllerId, string categoryName, string layoutName) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						PmIFtQxmWzwQeuuQObnryEqmXnQQ(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, categoryName, layoutName);
					}
				}

				public void AddEmptyMap(ControllerType controllerType, int controllerId, int categoryId, int layoutId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						ZcchzppkAZBRQPJLQCANJsaeeKs(controllerType, controllerId, categoryId, layoutId);
					}
				}

				public void AddEmptyMap(ControllerType controllerType, int controllerId, string categoryName, string layoutName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					int layoutId = ReInput.mapping.GetLayoutId(controllerType, layoutName);
					if (mapCategoryId >= 0 && layoutId >= 0)
					{
						AddEmptyMap(controllerType, controllerId, mapCategoryId, layoutId);
					}
				}

				public void RemoveMap<T>(int controllerId, int mapId) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else if (mapId >= 0)
					{
						sIibcxfpDPMvqHqkylBNFbTfmdRUb(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, mapId);
					}
				}

				public void RemoveMap<T>(int controllerId, int categoryId, int layoutId) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else if (categoryId >= 0 && layoutId >= 0)
					{
						iRQWSPNjDNhyxSvurfHMFwJLwHoKA(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, categoryId, layoutId);
					}
				}

				public void RemoveMap<T>(int controllerId, string categoryName, string layoutName) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						UXdEsPladuxedDZbBWAGicbzDjWd(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, categoryName, layoutName);
					}
				}

				public void RemoveMap(ControllerType controllerType, int controllerId, int mapId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else if (mapId >= 0)
					{
						sIibcxfpDPMvqHqkylBNFbTfmdRUb(controllerType, controllerId, mapId);
					}
				}

				public void RemoveMap(ControllerType controllerType, int controllerId, int categoryId, int layoutId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else if (categoryId >= 0 && layoutId >= 0)
					{
						iRQWSPNjDNhyxSvurfHMFwJLwHoKA(controllerType, controllerId, categoryId, layoutId);
					}
				}

				public void RemoveMap(ControllerType controllerType, int controllerId, string categoryName, string layoutName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						UXdEsPladuxedDZbBWAGicbzDjWd(controllerType, controllerId, categoryName, layoutName);
					}
				}

				public void ClearMaps<T>(bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						ClearMaps(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), userAssignableOnly);
					}
				}

				public void ClearMaps(ControllerType controllerType, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.RVvzAxNOJdJwZHgPoiQvzdFRDTIk(userAssignableOnly);
					}
				}

				public void ClearMapsInCategory<T>(int categoryId, bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						ClearMapsInCategory(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), categoryId, userAssignableOnly);
					}
				}

				public void ClearMapsInCategory<T>(string categoryName, bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId >= 0)
					{
						ClearMapsInCategory<T>(mapCategoryId, userAssignableOnly);
					}
				}

				public void ClearMapsInCategory<T>(int categoryId, int layoutId, bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						ClearMapsInCategory(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), categoryId, layoutId, userAssignableOnly);
					}
				}

				public void ClearMapsInCategory<T>(string categoryName, string layoutName, bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId >= 0)
					{
						int layoutId = ReInput.mapping.GetLayoutId(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), layoutName);
						if (layoutId >= 0)
						{
							ClearMapsInCategory<T>(mapCategoryId, layoutId, userAssignableOnly);
						}
					}
				}

				public void ClearMapsInCategory(int categoryId, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.vdLtqhDGdbvtgdsibpfWVEbufiSz(i));
						for (int j = 0; j < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; j++)
						{
							rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(j).UEDbvZCpORwBphRIdGLFIEwJLiiEb.UGvbOuZFJGsCKeIAshrCKKpGLEVQA(categoryId, userAssignableOnly);
						}
					}
				}

				public void ClearMapsInCategory(string categoryName, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId >= 0)
					{
						ClearMapsInCategory(mapCategoryId, userAssignableOnly);
					}
				}

				public void ClearMapsInCategory(ControllerType controllerType, int categoryId, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.UGvbOuZFJGsCKeIAshrCKKpGLEVQA(categoryId, userAssignableOnly);
					}
				}

				public void ClearMapsInCategory(ControllerType controllerType, string categoryName, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId >= 0)
					{
						ClearMapsInCategory(controllerType, mapCategoryId, userAssignableOnly);
					}
				}

				public void ClearMapsInCategory(ControllerType controllerType, int categoryId, int layoutId, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					InputCategory mapCategory = ReInput.mapping.GetMapCategory(categoryId);
					if (mapCategory != null && (!userAssignableOnly || mapCategory.userAssignable))
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
						for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
						{
							rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.YlNZjFktSgaNDlvILASXXSiGNSDL(categoryId, layoutId);
						}
					}
				}

				public void ClearMapsInCategory(ControllerType controllerType, string categoryName, string layoutName, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId >= 0)
					{
						int layoutId = ReInput.mapping.GetLayoutId(controllerType, layoutName);
						if (layoutId >= 0)
						{
							ClearMapsInCategory(controllerType, mapCategoryId, layoutId, userAssignableOnly);
						}
					}
				}

				public void ClearMapsInLayout<T>(int layoutId, bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						ClearMapsInLayout(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), layoutId, userAssignableOnly);
					}
				}

				public void ClearMapsInLayout<T>(string layoutName, bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int layoutId = ReInput.mapping.GetLayoutId(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), layoutName);
					if (layoutId >= 0)
					{
						ClearMapsInLayout<T>(layoutId, userAssignableOnly);
					}
				}

				public void ClearMapsInLayout(ControllerType controllerType, int layoutId, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.vILBLDBAohbLDhkieiDTGEMISbHQd(layoutId, userAssignableOnly);
					}
				}

				public void ClearMapsInLayout(ControllerType controllerType, string layoutName, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int layoutId = ReInput.mapping.GetLayoutId(controllerType, layoutName);
					if (layoutId >= 0)
					{
						ClearMapsInLayout(controllerType, layoutId, userAssignableOnly);
					}
				}

				public void ClearMapsForController<T>(int controllerId, bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						ClearMapsForController(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, userAssignableOnly);
					}
				}

				public void ClearMapsForController<T>(int controllerId, int categoryId, bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						ClearMapsForController(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, categoryId, userAssignableOnly);
					}
				}

				public void ClearMapsForController<T>(int controllerId, string categoryName, bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId >= 0)
					{
						ClearMapsForController<T>(controllerId, mapCategoryId, userAssignableOnly);
					}
				}

				public void ClearMapsForController(ControllerType controllerType, int controllerId, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(controllerId);
					if (num >= 0)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.RVvzAxNOJdJwZHgPoiQvzdFRDTIk(userAssignableOnly);
					}
				}

				public void ClearMapsForController(ControllerType controllerType, int controllerId, int categoryId, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(controllerId);
					if (num >= 0)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.UGvbOuZFJGsCKeIAshrCKKpGLEVQA(categoryId, userAssignableOnly);
					}
				}

				public void ClearMapsForController(ControllerType controllerType, int controllerId, string categoryName, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId >= 0)
					{
						ClearMapsForController(controllerType, controllerId, mapCategoryId, userAssignableOnly);
					}
				}

				public void ClearMapsForControllerInLayout<T>(int controllerId, int layoutId, bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
					}
					else
					{
						ClearMapsForControllerInLayout(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), controllerId, layoutId, userAssignableOnly);
					}
				}

				public void ClearMapsForControllerInLayout<T>(int controllerId, string layoutName, bool userAssignableOnly) where T : ControllerMap
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int layoutId = ReInput.mapping.GetLayoutId(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<T>(), layoutName);
					if (layoutId >= 0)
					{
						ClearMapsForControllerInLayout<T>(controllerId, layoutId, userAssignableOnly);
					}
				}

				public void ClearMapsForControllerInLayout(ControllerType controllerType, int controllerId, int layoutId, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(controllerId);
					if (num >= 0)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.vILBLDBAohbLDhkieiDTGEMISbHQd(layoutId, userAssignableOnly);
					}
				}

				public void ClearMapsForControllerInLayout(ControllerType controllerType, int controllerId, string layoutName, bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					int layoutId = ReInput.mapping.GetLayoutId(controllerType, layoutName);
					if (layoutId >= 0)
					{
						ClearMapsForControllerInLayout(controllerType, controllerId, layoutId, userAssignableOnly);
					}
				}

				public void ClearAllMaps(bool userAssignableOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					for (int i = 0; i < MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						ClearMaps(MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.vdLtqhDGdbvtgdsibpfWVEbufiSz(i), userAssignableOnly);
					}
				}

				public ActionElementMap GetFirstButtonMapWithAction(ControllerType controllerType, int controllerId, int actionId, bool skipDisabledMaps)
				{
					return GetFirstButtonMapWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstButtonMapWithAction(ControllerType controllerType, int controllerId, string actionName, bool skipDisabledMaps)
				{
					return GetFirstButtonMapWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionName, skipDisabledMaps);
				}

				public ActionElementMap GetFirstButtonMapWithAction(Controller controller, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (controller == null)
					{
						return null;
					}
					return KkBMUbgOayfRMPdJjxkYizGjbmPp(controller.type, controller.id, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstButtonMapWithAction(Controller controller, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetFirstButtonMapWithAction(controller, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstButtonMapWithAction(ControllerType controllerType, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					return GGcybEtQYclffaLHiFcVwAEagDGBA(controllerType, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstButtonMapWithAction(ControllerType controllerType, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetFirstButtonMapWithAction(controllerType, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstButtonMapWithAction(int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (actionId < 0)
					{
						return null;
					}
					for (int i = 0; i < MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						ActionElementMap actionElementMap = GGcybEtQYclffaLHiFcVwAEagDGBA(MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.vdLtqhDGdbvtgdsibpfWVEbufiSz(i), actionId, skipDisabledMaps);
						if (actionElementMap != null)
						{
							return actionElementMap;
						}
					}
					return null;
				}

				public ActionElementMap GetFirstButtonMapWithAction(string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetFirstButtonMapWithAction(actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ButtonMapsWithAction(ControllerType controllerType, int controllerId, int actionId, bool skipDisabledMaps)
				{
					return ButtonMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ButtonMapsWithAction(ControllerType controllerType, int controllerId, string actionName, bool skipDisabledMaps)
				{
					return ButtonMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionName, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ButtonMapsWithAction(Controller controller, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					if (controller == null)
					{
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					return ffTnnTcFRMihxlAqJIKibTKpOMl(controller.type, controller.id, actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ButtonMapsWithAction(Controller controller, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return ButtonMapsWithAction(controller, actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ButtonMapsWithAction(ControllerType controllerType, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					return PaFzQOudJtjLHlGDCINSApIEGqahb(controllerType, actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ButtonMapsWithAction(ControllerType controllerType, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return ButtonMapsWithAction(controllerType, actionId, skipDisabledMaps);
				}

				[IteratorStateMachine(typeof(WveEXWkrtuRurBcnNjfjwuimhjPIA))]
				public IEnumerable<ActionElementMap> ButtonMapsWithAction(int actionId, bool skipDisabledMaps)
				{
					return new WveEXWkrtuRurBcnNjfjwuimhjPIA(-2)
					{
						PwAwMPagJLLSzcRnOxdywAzGBKyL = this,
						LfMPccdFQitFrXNTMezSeKmGEjQmA = actionId,
						fOtBzkmZLTRfYwXgoPunxeSdsUOL = skipDisabledMaps
					};
				}

				public IEnumerable<ActionElementMap> ButtonMapsWithAction(string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return ButtonMapsWithAction(actionId, skipDisabledMaps);
				}

				public int GetButtonMapsWithAction(ControllerType controllerType, int controllerId, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					return GetButtonMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionId, skipDisabledMaps, results);
				}

				public int GetButtonMapsWithAction(ControllerType controllerType, int controllerId, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					return GetButtonMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionName, skipDisabledMaps, results);
				}

				public int GetButtonMapsWithAction(Controller controller, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (results == null)
					{
						throw new ArgumentNullException("results");
					}
					if (controller == null)
					{
						results.Clear();
						return 0;
					}
					return HTpCtkjGjOUtkBMsNhxSfYzKiYen(controller.type, controller.id, actionId, skipDisabledMaps, results, false);
				}

				public int GetButtonMapsWithAction(Controller controller, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetButtonMapsWithAction(controller, actionId, skipDisabledMaps, results);
				}

				public int GetButtonMapsWithAction(ControllerType controllerType, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					return cJYeMFDNrVbuwyIazXdSJnxSNpQMA(controllerType, actionId, skipDisabledMaps, results, false);
				}

				public int GetButtonMapsWithAction(ControllerType controllerType, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetButtonMapsWithAction(controllerType, actionId, skipDisabledMaps, results);
				}

				public int GetButtonMapsWithAction(int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					return BKXMvXggBcGPasRKnfVKgUTBQtNt(actionId, skipDisabledMaps, results, false);
				}

				public int GetButtonMapsWithAction(string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetButtonMapsWithAction(actionId, skipDisabledMaps, results);
				}

				public ActionElementMap GetFirstAxisMapWithAction(ControllerType controllerType, int controllerId, int actionId, bool skipDisabledMaps)
				{
					return GetFirstAxisMapWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstAxisMapWithAction(ControllerType controllerType, int controllerId, string actionName, bool skipDisabledMaps)
				{
					return GetFirstAxisMapWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionName, skipDisabledMaps);
				}

				public ActionElementMap GetFirstAxisMapWithAction(Controller controller, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (controller == null)
					{
						return null;
					}
					return ktBmgLshdeREURiLWDPuoqTSGMtbA(controller.type, controller.id, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstAxisMapWithAction(Controller controller, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetFirstAxisMapWithAction(controller, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstAxisMapWithAction(ControllerType controllerType, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					return mjWGynWchpQRVlNUDPaQweSXlmvG(controllerType, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstAxisMapWithAction(ControllerType controllerType, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetFirstAxisMapWithAction(controllerType, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstAxisMapWithAction(int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (actionId < 0)
					{
						return null;
					}
					for (int i = 0; i < MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						ActionElementMap actionElementMap = mjWGynWchpQRVlNUDPaQweSXlmvG(MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.vdLtqhDGdbvtgdsibpfWVEbufiSz(i), actionId, skipDisabledMaps);
						if (actionElementMap != null)
						{
							return actionElementMap;
						}
					}
					return null;
				}

				public ActionElementMap GetFirstAxisMapWithAction(string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetFirstAxisMapWithAction(actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> AxisMapsWithAction(ControllerType controllerType, int controllerId, int actionId, bool skipDisabledMaps)
				{
					return AxisMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> AxisMapsWithAction(ControllerType controllerType, int controllerId, string actionName, bool skipDisabledMaps)
				{
					return AxisMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionName, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> AxisMapsWithAction(Controller controller, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					if (controller == null)
					{
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					return bUjZanronCDHyAfgedxeATYBSDufb(controller.type, controller.id, actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> AxisMapsWithAction(Controller controller, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return AxisMapsWithAction(controller, actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> AxisMapsWithAction(ControllerType controllerType, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					return BBeecfKPrFwWgmOAPqVlwNPeFNAjb(controllerType, actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> AxisMapsWithAction(ControllerType controllerType, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return AxisMapsWithAction(controllerType, actionId, skipDisabledMaps);
				}

				[IteratorStateMachine(typeof(ZGCZROVqXgqRpPRQPVRwGoxetJzx))]
				public IEnumerable<ActionElementMap> AxisMapsWithAction(int actionId, bool skipDisabledMaps)
				{
					return new ZGCZROVqXgqRpPRQPVRwGoxetJzx(-2)
					{
						JTaMnchNBVBgBBoHrSsuKHTeBiRH = this,
						rVGnozjdgIPMXJNxRGegaEQqEHYh = actionId,
						NZfirYICFAjEfUHeInKsSZTFVeZL = skipDisabledMaps
					};
				}

				public IEnumerable<ActionElementMap> AxisMapsWithAction(string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return AxisMapsWithAction(actionId, skipDisabledMaps);
				}

				public int GetAxisMapsWithAction(ControllerType controllerType, int controllerId, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					return GetAxisMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionId, skipDisabledMaps, results);
				}

				public int GetAxisMapsWithAction(ControllerType controllerType, int controllerId, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					return GetAxisMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionName, skipDisabledMaps, results);
				}

				public int GetAxisMapsWithAction(Controller controller, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (controller == null)
					{
						return 0;
					}
					return bWNDcIPHubRAKUbEkUkGMRlDNqJj(controller.type, controller.id, actionId, skipDisabledMaps, results, false);
				}

				public int GetAxisMapsWithAction(Controller controller, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetAxisMapsWithAction(controller, actionId, skipDisabledMaps, results);
				}

				public int GetAxisMapsWithAction(ControllerType controllerType, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (results == null)
					{
						throw new ArgumentNullException("results");
					}
					return AHyCEXYuWIoSPjdmKlWLiKxrCCzM(controllerType, actionId, skipDisabledMaps, results, false);
				}

				public int GetAxisMapsWithAction(ControllerType controllerType, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetAxisMapsWithAction(controllerType, actionId, skipDisabledMaps, results);
				}

				public int GetAxisMapsWithAction(int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					return JlwOzOMHRrLCBjfVIoUdOHKfJjfp(actionId, skipDisabledMaps, results, false);
				}

				public int GetAxisMapsWithAction(string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetAxisMapsWithAction(actionId, skipDisabledMaps, results);
				}

				public ActionElementMap GetFirstElementMapWithAction(ControllerType controllerType, int controllerId, int actionId, bool skipDisabledMaps)
				{
					return GetFirstElementMapWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstElementMapWithAction(ControllerType controllerType, int controllerId, string actionName, bool skipDisabledMaps)
				{
					return GetFirstElementMapWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionName, skipDisabledMaps);
				}

				public ActionElementMap GetFirstElementMapWithAction(Controller controller, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (controller == null)
					{
						return null;
					}
					return QqKgRkGcuHqbQjRRGCwtTVEAMIaIb(controller.type, controller.id, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstElementMapWithAction(Controller controller, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetFirstElementMapWithAction(controller, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstElementMapWithAction(ControllerType controllerType, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					return SOwgzlZRvTOVWlfZYRlumDZSUJMC(controllerType, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstElementMapWithAction(ControllerType controllerType, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetFirstElementMapWithAction(controllerType, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstElementMapWithAction(int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					if (actionId < 0)
					{
						return null;
					}
					for (int i = 0; i < MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						ActionElementMap actionElementMap = SOwgzlZRvTOVWlfZYRlumDZSUJMC(MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.vdLtqhDGdbvtgdsibpfWVEbufiSz(i), actionId, skipDisabledMaps);
						if (actionElementMap != null)
						{
							return actionElementMap;
						}
					}
					return null;
				}

				public ActionElementMap GetFirstElementMapWithAction(string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetFirstElementMapWithAction(actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ElementMapsWithAction(ControllerType controllerType, int controllerId, int actionId, bool skipDisabledMaps)
				{
					return ElementMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ElementMapsWithAction(ControllerType controllerType, int controllerId, string actionName, bool skipDisabledMaps)
				{
					return ElementMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionName, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ElementMapsWithAction(Controller controller, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					if (controller == null)
					{
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					return lUdjRFCAnBNKRdrPGMsZphmYbNJJ(controller.type, controller.id, actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ElementMapsWithAction(Controller controller, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return ElementMapsWithAction(controller, actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ElementMapsWithAction(ControllerType controllerType, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					return dxqvTowISvqfGOltXHnnveNMzcvk(controllerType, actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ElementMapsWithAction(ControllerType controllerType, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return ElementMapsWithAction(controllerType, actionId, skipDisabledMaps);
				}

				[IteratorStateMachine(typeof(okayzLlwCRoeiyylHCDzvnMpCCJO))]
				public IEnumerable<ActionElementMap> ElementMapsWithAction(int actionId, bool skipDisabledMaps)
				{
					return new okayzLlwCRoeiyylHCDzvnMpCCJO(-2)
					{
						qsjAhQEEzvLHzufNmbHrqCYfNsiP = this,
						PEvbVdApKPJfMIhyHoqpmMxkUDnkA = actionId,
						WBJmqArDNBXSvaecFKCxupngEhCS = skipDisabledMaps
					};
				}

				public IEnumerable<ActionElementMap> ElementMapsWithAction(string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return ElementMapsWithAction(actionId, skipDisabledMaps);
				}

				public int GetElementMapsWithAction(ControllerType controllerType, int controllerId, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					return GetElementMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionId, skipDisabledMaps, results);
				}

				public int GetElementMapsWithAction(ControllerType controllerType, int controllerId, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					return GetElementMapsWithAction(ReInput.controllers.GetController(controllerType, controllerId), actionName, skipDisabledMaps, results);
				}

				public int GetElementMapsWithAction(Controller controller, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (controller == null)
					{
						return 0;
					}
					return nDTInocIKxxokWjnWgWNzEnjcvTbb(controller.type, controller.id, actionId, skipDisabledMaps, results, false);
				}

				public int GetElementMapsWithAction(Controller controller, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetElementMapsWithAction(controller, actionId, skipDisabledMaps, results);
				}

				public int GetElementMapsWithAction(ControllerType controllerType, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					return OdRdceERZVZsGGbtlLdVbSOUZRAGA(controllerType, actionId, skipDisabledMaps, results, false);
				}

				public int GetElementMapsWithAction(ControllerType controllerType, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetElementMapsWithAction(controllerType, actionId, skipDisabledMaps, results);
				}

				public int GetElementMapsWithAction(int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					return IamsQkJumiKpKMYPjnsRgiLPxZIG(actionId, skipDisabledMaps, results, false);
				}

				public int GetElementMapsWithAction(string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetElementMapsWithAction(actionId, skipDisabledMaps, results);
				}

				public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(ControllerElementTarget elementTarget, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
					IEnumerable<ActionElementMap> result = ElementMapsWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, skipDisabledMaps);
					SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
					return result;
				}

				public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(IControllerElementTarget elementTarget, bool skipDisabledMaps)
				{
					return rsGSVRQBPvMlvLnSdWKCtZdfnMak(elementTarget, false, -1, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(ControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
					IEnumerable<ActionElementMap> result = ElementMapsWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, actionId, skipDisabledMaps);
					SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
					return result;
				}

				public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(ControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return ElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(IControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
				{
					return rsGSVRQBPvMlvLnSdWKCtZdfnMak(elementTarget, true, actionId, skipDisabledMaps);
				}

				public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(IControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
				{
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return ElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstElementMapWithElementTarget(ControllerElementTarget elementTarget, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
					ActionElementMap firstElementMapWithElementTarget = GetFirstElementMapWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, skipDisabledMaps);
					SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
					return firstElementMapWithElementTarget;
				}

				public ActionElementMap GetFirstElementMapWithElementTarget(IControllerElementTarget elementTarget, bool skipDisabledMaps)
				{
					return GyreroAVwPqScvaRmilmMlloNNMBb(elementTarget, false, -1, skipDisabledMaps);
				}

				public ActionElementMap GetFirstElementMapWithElementTarget(ControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
					ActionElementMap firstElementMapWithElementTarget = GetFirstElementMapWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, actionId, skipDisabledMaps);
					SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
					return firstElementMapWithElementTarget;
				}

				public ActionElementMap GetFirstElementMapWithElementTarget(ControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetFirstElementMapWithElementTarget(elementTarget, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstElementMapWithElementTarget(IControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
				{
					return GyreroAVwPqScvaRmilmMlloNNMBb(elementTarget, true, actionId, skipDisabledMaps);
				}

				public ActionElementMap GetFirstElementMapWithElementTarget(IControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetFirstElementMapWithElementTarget(elementTarget, actionId, skipDisabledMaps);
				}

				public int GetElementMapsWithElementTarget(ControllerElementTarget elementTarget, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
					int elementMapsWithElementTarget = GetElementMapsWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, skipDisabledMaps, results);
					SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
					return elementMapsWithElementTarget;
				}

				public int GetElementMapsWithElementTarget(IControllerElementTarget elementTarget, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					return yeNkzsJqACgKPDpNkpCqQVNFoxEO(elementTarget, false, -1, skipDisabledMaps, results, false);
				}

				public int GetElementMapsWithElementTarget(ControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
					int elementMapsWithElementTarget = GetElementMapsWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, actionId, skipDisabledMaps, results);
					SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
					return elementMapsWithElementTarget;
				}

				public int GetElementMapsWithElementTarget(ControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps, results);
				}

				public int GetElementMapsWithElementTarget(IControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					return yeNkzsJqACgKPDpNkpCqQVNFoxEO(elementTarget, true, actionId, skipDisabledMaps, results, false);
				}

				public int GetElementMapsWithElementTarget(IControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
					return GetElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps, results);
				}

				public T[] GetMapSaveData<T>(int controllerId, bool userAssignableMapsOnly) where T : ControllerMapSaveData
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<T>.array;
					}
					return oNPthjcGJqIwOcylgCSnIMAfeZVB<T>(controllerId, userAssignableMapsOnly);
				}

				public ControllerMapSaveData[] GetMapSaveData(ControllerType controllerType, int controllerId, bool userAssignableMapsOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ControllerMapSaveData>.array;
					}
					return EtNiIjtACRRUvtWGEeVmWDwIIhdh(controllerType, controllerId, userAssignableMapsOnly);
				}

				public T[] GetAllMapSaveData<T>(bool userAssignableMapsOnly) where T : ControllerMapSaveData
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<T>.array;
					}
					return dywRzXOdDCKiRQkFNfvxaguKVVAqA<T>(userAssignableMapsOnly);
				}

				public ControllerMapSaveData[] GetAllMapSaveData(ControllerType controllerType, bool userAssignableMapsOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ControllerMapSaveData>.array;
					}
					return FttGgLfBxjhLCmfLSzKtNerWdhakA(controllerType, userAssignableMapsOnly);
				}

				public ControllerMapSaveData[] GetAllMapSaveData(bool userAssignableMapsOnly)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return EmptyObjects<ControllerMapSaveData>.array;
					}
					ControllerMapSaveData[] array = null;
					for (int i = 0; i < MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						ArrayTools.Combine(ref array, FttGgLfBxjhLCmfLSzKtNerWdhakA(MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.vdLtqhDGdbvtgdsibpfWVEbufiSz(i), userAssignableMapsOnly));
					}
					return array;
				}

				public int SetAllMapsEnabled(bool state)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int num = 0;
					int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i);
						int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						for (int j = 0; j < num2; j++)
						{
							num += rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(j).UEDbvZCpORwBphRIdGLFIEwJLiiEb.AgceHycNrWILbLqntHHIwhQbhwheA(state);
						}
					}
					return num;
				}

				public int SetAllMapsEnabled(bool state, ControllerType controllerType)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int num = 0;
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
					for (int i = 0; i < num2; i++)
					{
						num += rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.AgceHycNrWILbLqntHHIwhQbhwheA(state);
					}
					return num;
				}

				public int SetAllMapsEnabled(bool state, Controller controller)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (controller == null)
					{
						return 0;
					}
					return SetAllMapsEnabled(state, controller.type, controller.id);
				}

				public int SetAllMapsEnabled(bool state, ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					return MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType).eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(controllerId)?.UEDbvZCpORwBphRIdGLFIEwJLiiEb.AgceHycNrWILbLqntHHIwhQbhwheA(state) ?? 0;
				}

				public int SetMapsEnabled(bool state, int categoryId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (categoryId < 0)
					{
						return 0;
					}
					int num = 0;
					int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i);
						int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						for (int j = 0; j < num2; j++)
						{
							num += rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(j).UEDbvZCpORwBphRIdGLFIEwJLiiEb.kripAsExXcnzlsPYuGUmxvCKMDFH(state, categoryId);
						}
					}
					return num;
				}

				public int SetMapsEnabled(bool state, string categoryName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					return SetMapsEnabled(state, mapCategoryId);
				}

				public int SetMapsEnabled(bool state, string categoryName, string layoutName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					int num = 0;
					int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						ControllerType controllerType = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.vdLtqhDGdbvtgdsibpfWVEbufiSz(i);
						int layoutId = ReInput.mapping.GetLayoutId(controllerType, layoutName);
						if (layoutId >= 0)
						{
							num += SetMapsEnabled(state, controllerType, mapCategoryId, layoutId);
						}
					}
					return num;
				}

				public int SetMapsEnabled(bool state, ControllerType controllerType, int categoryId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (categoryId < 0)
					{
						return 0;
					}
					int num = 0;
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
					for (int i = 0; i < num2; i++)
					{
						num += rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.kripAsExXcnzlsPYuGUmxvCKMDFH(state, categoryId);
					}
					return num;
				}

				public int SetMapsEnabled(bool state, ControllerType controllerType, string categoryName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					return SetMapsEnabled(state, controllerType, mapCategoryId);
				}

				public int SetMapsEnabled(bool state, ControllerType controllerType, int categoryId, int layoutId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (categoryId < 0 || layoutId < 0)
					{
						return 0;
					}
					int num = 0;
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
					for (int i = 0; i < num2; i++)
					{
						num += rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.RwkQRjaBRLzhckikKLFhWyfImVqc(state, categoryId, layoutId);
					}
					return num;
				}

				public int SetMapsEnabled(bool state, ControllerType controllerType, string categoryName, string layoutName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					int layoutId = ReInput.mapping.GetLayoutId(controllerType, layoutName);
					if (layoutId < 0)
					{
						return 0;
					}
					return SetMapsEnabled(state, controllerType, mapCategoryId, layoutId);
				}

				public int SetMapsEnabled(bool state, Controller controller, int categoryId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (controller == null)
					{
						return 0;
					}
					if (categoryId < 0)
					{
						return 0;
					}
					return MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controller.type).eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(controller.id)?.UEDbvZCpORwBphRIdGLFIEwJLiiEb.kripAsExXcnzlsPYuGUmxvCKMDFH(state, categoryId) ?? 0;
				}

				public int SetMapsEnabled(bool state, Controller controller, int categoryId, int layoutId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (controller == null)
					{
						return 0;
					}
					if (categoryId < 0)
					{
						return 0;
					}
					if (layoutId < 0)
					{
						return 0;
					}
					return MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controller.type).eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(controller.id)?.UEDbvZCpORwBphRIdGLFIEwJLiiEb.RwkQRjaBRLzhckikKLFhWyfImVqc(state, categoryId, layoutId) ?? 0;
				}

				public int SetMapsEnabled(bool state, Controller controller, string categoryName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (controller == null)
					{
						return 0;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					return SetMapsEnabled(state, controller, mapCategoryId);
				}

				public int SetMapsEnabled(bool state, Controller controller, string categoryName, string layoutName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return 0;
					}
					if (controller == null)
					{
						return 0;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					int layoutId = ReInput.mapping.GetLayoutId(controller.type, layoutName);
					if (layoutId < 0)
					{
						return 0;
					}
					return SetMapsEnabled(state, controller, mapCategoryId, layoutId);
				}

				public void LoadDefaultMaps(ControllerType controllerType)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return;
					}
					switch (controllerType)
					{
					case ControllerType.Joystick:
						KxZNirFPYcOTRMeVnbxxDFAixHJu(false);
						break;
					case ControllerType.Keyboard:
						EBXSWvlskcRzsmNKjtNWCyEjCeir(false);
						break;
					case ControllerType.Mouse:
						ICgkACWSyaPEaoDmKtVxijJGGzNB(false);
						break;
					case ControllerType.Custom:
						yTaelNIrpkDnwkhUKTnuKnklksCU(false);
						break;
					default:
						throw new NotImplementedException();
					}
				}

				public bool ContainsMapInCategory(InputMapCategory category)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return false;
					}
					if (category == null)
					{
						return false;
					}
					return ContainsMapInCategory(category.id);
				}

				public bool ContainsMapInCategory(int categoryId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return false;
					}
					if (categoryId < 0)
					{
						return false;
					}
					int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i);
						int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						for (int j = 0; j < num; j++)
						{
							if (rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(j).UEDbvZCpORwBphRIdGLFIEwJLiiEb.PspLwXWXOtXRpHbkhtLkntzOcOLf(categoryId))
							{
								return true;
							}
						}
					}
					return false;
				}

				public bool ContainsMapInCategory(string categoryName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return false;
					}
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(categoryName);
					if (mapCategoryId < 0)
					{
						return false;
					}
					return ContainsMapInCategory(mapCategoryId);
				}

				public bool ContainsMapInCategory(ControllerType controllerType, int categoryId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return false;
					}
					if (categoryId < 0)
					{
						return false;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
					for (int i = 0; i < num; i++)
					{
						if (rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.PspLwXWXOtXRpHbkhtLkntzOcOLf(categoryId))
						{
							return true;
						}
					}
					return false;
				}

				public InputBehavior GetInputBehavior(int behaviorId)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					return hIUdxPiJkzzwMniBaGspAmVWYBpDA.XCFDVqZUbyGILGzarkLNUnbiMkmM.AUVupqHUEheIGgrZDnacEevMQjxL(hIUdxPiJkzzwMniBaGspAmVWYBpDA.mgTogZEAHwpJMhbsccjZDcKdOLwp, behaviorId);
				}

				public InputBehavior GetInputBehavior(string behaviorName)
				{
					if (ReInput._id != MUfpEiqygGgMiNQDuSWClpiuBuic)
					{
						ReInput.CheckInitialized(MUfpEiqygGgMiNQDuSWClpiuBuic);
						return null;
					}
					return hIUdxPiJkzzwMniBaGspAmVWYBpDA.XCFDVqZUbyGILGzarkLNUnbiMkmM.LAjEtOLXxuEGCJYVpCdAEIAbisdo(hIUdxPiJkzzwMniBaGspAmVWYBpDA.mgTogZEAHwpJMhbsccjZDcKdOLwp, behaviorName);
				}

				internal void HLmEzKlOOuHieSRjqefvIcNqaOxSA()
				{
					BzoEVmkFzWoLiXDbvANzqcaxGILS.LoadDefaults();
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.LoadDefaults();
				}

				internal void KxZNirFPYcOTRMeVnbxxDFAixHJu(bool P_0)
				{
					if (bRjrQqMfyTYOXVFtOrygKGRGMxNN.WfOMuKhdFAWPmdPMsfPYoEJHpgSm == null)
					{
						return;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Joystick);
					MNHGNJfFQqpihvuAqNaSqIrXWcffA.BPoUszNGyThVhLHpFIamgLjstajIA.GLaWLnEHInVwyAWnURcXgeouPfWv();
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
					for (int i = 0; i < num; i++)
					{
						fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap>.afpFMKpOFhEzEBdDrNcunaaeluicA afpFMKpOFhEzEBdDrNcunaaeluicA = (fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap>.afpFMKpOFhEzEBdDrNcunaaeluicA)rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i);
						bool[] array = null;
						if (!P_0)
						{
							int num2 = afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA.GeYULXtFpYgsaXqXZIDwegpjOMgW();
							array = new bool[num2];
							for (int j = 0; j < num2; j++)
							{
								array[j] = afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(j).enabled;
							}
						}
						afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA.hKznxpKmAWxdhUTLnxskYwIoMXgF(false);
						for (int k = 0; k < bRjrQqMfyTYOXVFtOrygKGRGMxNN.WfOMuKhdFAWPmdPMsfPYoEJHpgSm.Length; k++)
						{
							rrwLpwEWzemefRGFhQpFgSrTBdfs(afpFMKpOFhEzEBdDrNcunaaeluicA.TFffYazGHMOVnLpOXAoTjAaUfvCi, afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA, bRjrQqMfyTYOXVFtOrygKGRGMxNN.WfOMuKhdFAWPmdPMsfPYoEJHpgSm[k], P_0);
						}
						if (!P_0)
						{
							int num3 = MathTools.Min(array.Length, afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA.GeYULXtFpYgsaXqXZIDwegpjOMgW());
							for (int l = 0; l < num3; l++)
							{
								afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(l).enabled = array[l];
							}
						}
					}
					bool loadFromUserDataStore = XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore;
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore = false;
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.Apply();
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore = loadFromUserDataStore;
				}

				internal void EBXSWvlskcRzsmNKjtNWCyEjCeir(bool P_0)
				{
					if (bRjrQqMfyTYOXVFtOrygKGRGMxNN.KWAdYfrBWZULRfxrgVzCcgLXFDsEA == null)
					{
						return;
					}
					vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Keyboard).eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(0).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
					bool[] array = null;
					if (!P_0)
					{
						int num = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA;
						array = new bool[num];
						for (int i = 0; i < num; i++)
						{
							array[i] = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(i).enabled;
						}
					}
					vHszkbCJdDAIcILHhpVCxcZlIBxlA2.RVvzAxNOJdJwZHgPoiQvzdFRDTIk(false);
					for (int j = 0; j < bRjrQqMfyTYOXVFtOrygKGRGMxNN.KWAdYfrBWZULRfxrgVzCcgLXFDsEA.Length; j++)
					{
						IRueBbHgtviLAypteudQcHzlDhLM rueBbHgtviLAypteudQcHzlDhLM = bRjrQqMfyTYOXVFtOrygKGRGMxNN.KWAdYfrBWZULRfxrgVzCcgLXFDsEA[j];
						if (rueBbHgtviLAypteudQcHzlDhLM.nymHpUWXblyIOKufKQmXCblorxCK >= 0 && rueBbHgtviLAypteudQcHzlDhLM.WSacpfHPFQOnHXwHCXdQJsbZiXXu >= 0)
						{
							KeyboardMap keyboardMap = ReInput.UserData.FindKeyboardMap_Game(ReInput.controllers.Keyboard, rueBbHgtviLAypteudQcHzlDhLM.nymHpUWXblyIOKufKQmXCblorxCK, rueBbHgtviLAypteudQcHzlDhLM.WSacpfHPFQOnHXwHCXdQJsbZiXXu);
							if (P_0)
							{
								keyboardMap.enabled = rueBbHgtviLAypteudQcHzlDhLM.HYjCrrWWbbZGtCHOVmKfIqcCVfmS;
							}
							IVStlYfQmoamAgebBlgGsChGRtSbA(ControllerType.Keyboard, 0, keyboardMap, BoolOption.Default);
						}
					}
					if (!P_0)
					{
						int num2 = MathTools.Min(array.Length, vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA);
						for (int k = 0; k < num2; k++)
						{
							vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(k).enabled = array[k];
						}
					}
					bool loadFromUserDataStore = XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore;
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore = false;
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.Apply();
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore = loadFromUserDataStore;
				}

				internal void ICgkACWSyaPEaoDmKtVxijJGGzNB(bool P_0)
				{
					if (bRjrQqMfyTYOXVFtOrygKGRGMxNN.MwjISTjkofCBNJhBrzwvddLRYPql == null)
					{
						return;
					}
					vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Mouse).eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(0).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
					bool[] array = null;
					if (!P_0)
					{
						int num = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA;
						array = new bool[num];
						for (int i = 0; i < num; i++)
						{
							array[i] = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(i).enabled;
						}
					}
					vHszkbCJdDAIcILHhpVCxcZlIBxlA2.RVvzAxNOJdJwZHgPoiQvzdFRDTIk(false);
					for (int j = 0; j < bRjrQqMfyTYOXVFtOrygKGRGMxNN.MwjISTjkofCBNJhBrzwvddLRYPql.Length; j++)
					{
						IRueBbHgtviLAypteudQcHzlDhLM rueBbHgtviLAypteudQcHzlDhLM = bRjrQqMfyTYOXVFtOrygKGRGMxNN.MwjISTjkofCBNJhBrzwvddLRYPql[j];
						if (rueBbHgtviLAypteudQcHzlDhLM.nymHpUWXblyIOKufKQmXCblorxCK >= 0 && rueBbHgtviLAypteudQcHzlDhLM.WSacpfHPFQOnHXwHCXdQJsbZiXXu >= 0)
						{
							MouseMap mouseMap = ReInput.UserData.FindMouseMap_Game(ReInput.controllers.Mouse, rueBbHgtviLAypteudQcHzlDhLM.nymHpUWXblyIOKufKQmXCblorxCK, rueBbHgtviLAypteudQcHzlDhLM.WSacpfHPFQOnHXwHCXdQJsbZiXXu);
							if (P_0)
							{
								mouseMap.enabled = rueBbHgtviLAypteudQcHzlDhLM.HYjCrrWWbbZGtCHOVmKfIqcCVfmS;
							}
							IVStlYfQmoamAgebBlgGsChGRtSbA(ControllerType.Mouse, 0, mouseMap, BoolOption.Default);
						}
					}
					if (!P_0)
					{
						int num2 = MathTools.Min(array.Length, vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA);
						for (int k = 0; k < num2; k++)
						{
							vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(k).enabled = array[k];
						}
					}
					bool loadFromUserDataStore = XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore;
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore = false;
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.Apply();
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore = loadFromUserDataStore;
				}

				internal void yTaelNIrpkDnwkhUKTnuKnklksCU(bool P_0)
				{
					if (bRjrQqMfyTYOXVFtOrygKGRGMxNN.kDEWppmJyfCmZISBJfVlRNXsOAej == null)
					{
						return;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Custom);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
					for (int i = 0; i < num; i++)
					{
						fLcZuTpMOwYPWmGZCMQAZMEBzxNc<CustomController, CustomControllerMap>.afpFMKpOFhEzEBdDrNcunaaeluicA afpFMKpOFhEzEBdDrNcunaaeluicA = (fLcZuTpMOwYPWmGZCMQAZMEBzxNc<CustomController, CustomControllerMap>.afpFMKpOFhEzEBdDrNcunaaeluicA)rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i);
						bool[] array = null;
						if (!P_0)
						{
							int num2 = afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA.GeYULXtFpYgsaXqXZIDwegpjOMgW();
							array = new bool[num2];
							for (int j = 0; j < num2; j++)
							{
								array[j] = afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(j).enabled;
							}
						}
						afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA.hKznxpKmAWxdhUTLnxskYwIoMXgF(false);
						for (int k = 0; k < bRjrQqMfyTYOXVFtOrygKGRGMxNN.kDEWppmJyfCmZISBJfVlRNXsOAej.Length; k++)
						{
							EWAuBozihwFwLGiHiWyXamDtpjJA(afpFMKpOFhEzEBdDrNcunaaeluicA.TFffYazGHMOVnLpOXAoTjAaUfvCi, afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA, bRjrQqMfyTYOXVFtOrygKGRGMxNN.kDEWppmJyfCmZISBJfVlRNXsOAej[k], P_0);
						}
						if (!P_0)
						{
							int num3 = MathTools.Min(array.Length, afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA.GeYULXtFpYgsaXqXZIDwegpjOMgW());
							for (int l = 0; l < num3; l++)
							{
								afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(l).enabled = array[l];
							}
						}
					}
					bool loadFromUserDataStore = XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore;
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore = false;
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.Apply();
					XBGmqYuRMPcbxGurrHRZtTaSFlQO.loadFromUserDataStore = loadFromUserDataStore;
				}

				private rJRHfxObWEyZQOmmYgoxgmGnxuol DtTYEnWeRlrCRbGTGYnarXrLwuZO<_0001>() where _0001 : ControllerMap
				{
					return MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(cVDyIiOsEfJNYzVuZSmuEXqylgT.LWdtBZGEpBWBpzaRzUKwIyqvnKvs<_0001>());
				}

				internal global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<JoystickMap> NCEGKHAjrDLxSPYDJQXymFdrQMxeA(Joystick P_0, bool P_1)
				{
					if (P_0 == null || bRjrQqMfyTYOXVFtOrygKGRGMxNN.WfOMuKhdFAWPmdPMsfPYoEJHpgSm == null)
					{
						return null;
					}
					global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<JoystickMap> jXfmwiBdreTpaOqXNtMEJcOCaZSP = new global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<JoystickMap>(P_0.id);
					for (int i = 0; i < bRjrQqMfyTYOXVFtOrygKGRGMxNN.WfOMuKhdFAWPmdPMsfPYoEJHpgSm.Length; i++)
					{
						rrwLpwEWzemefRGFhQpFgSrTBdfs(P_0, jXfmwiBdreTpaOqXNtMEJcOCaZSP, bRjrQqMfyTYOXVFtOrygKGRGMxNN.WfOMuKhdFAWPmdPMsfPYoEJHpgSm[i], P_1);
					}
					if (jXfmwiBdreTpaOqXNtMEJcOCaZSP.GeYULXtFpYgsaXqXZIDwegpjOMgW() == 0)
					{
						return null;
					}
					return jXfmwiBdreTpaOqXNtMEJcOCaZSP;
				}

				private void rrwLpwEWzemefRGFhQpFgSrTBdfs(Joystick P_0, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<JoystickMap> P_1, IRueBbHgtviLAypteudQcHzlDhLM P_2, bool P_3)
				{
					if (P_0 != null && P_2 != null && P_2.nymHpUWXblyIOKufKQmXCblorxCK >= 0 && P_2.WSacpfHPFQOnHXwHCXdQJsbZiXXu >= 0)
					{
						JoystickMap joystickMap = ReInput.UserData.vAHiNYrtViGADjHOxDfcaDShypcZb(P_0, P_2.nymHpUWXblyIOKufKQmXCblorxCK, P_2.WSacpfHPFQOnHXwHCXdQJsbZiXXu);
						NnhFHdcvDdZZhDqoCXXywvEaJxwS(P_0, joystickMap);
						BoolOption boolOption = BoolOption.Default;
						if (P_3)
						{
							boolOption = (P_2.HYjCrrWWbbZGtCHOVmKfIqcCVfmS ? BoolOption.True : BoolOption.False);
						}
						P_1.ykUEjrcRColDXoSlfWCQVezcPybhb(joystickMap, boolOption);
					}
				}

				internal global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<CustomControllerMap> sqzurMWwILfVhUIUjcHbCfYAeYcYA(CustomController P_0, bool P_1)
				{
					if (P_0 == null || bRjrQqMfyTYOXVFtOrygKGRGMxNN.kDEWppmJyfCmZISBJfVlRNXsOAej == null)
					{
						return null;
					}
					global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<CustomControllerMap> jXfmwiBdreTpaOqXNtMEJcOCaZSP = new global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<CustomControllerMap>(P_0.id);
					for (int i = 0; i < bRjrQqMfyTYOXVFtOrygKGRGMxNN.kDEWppmJyfCmZISBJfVlRNXsOAej.Length; i++)
					{
						EWAuBozihwFwLGiHiWyXamDtpjJA(P_0, jXfmwiBdreTpaOqXNtMEJcOCaZSP, bRjrQqMfyTYOXVFtOrygKGRGMxNN.kDEWppmJyfCmZISBJfVlRNXsOAej[i], P_1);
					}
					if (jXfmwiBdreTpaOqXNtMEJcOCaZSP.GeYULXtFpYgsaXqXZIDwegpjOMgW() == 0)
					{
						return null;
					}
					return jXfmwiBdreTpaOqXNtMEJcOCaZSP;
				}

				private void EWAuBozihwFwLGiHiWyXamDtpjJA(CustomController P_0, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<CustomControllerMap> P_1, IRueBbHgtviLAypteudQcHzlDhLM P_2, bool P_3)
				{
					if (P_0 != null && P_2 != null && P_2.nymHpUWXblyIOKufKQmXCblorxCK >= 0 && P_2.WSacpfHPFQOnHXwHCXdQJsbZiXXu >= 0)
					{
						CustomControllerMap customControllerMap = ReInput.UserData.MNKkjYicKUjWIvlytoqoHPNWWAfD(P_2.nymHpUWXblyIOKufKQmXCblorxCK, P_0.sourceControllerId, P_2.WSacpfHPFQOnHXwHCXdQJsbZiXXu);
						NnhFHdcvDdZZhDqoCXXywvEaJxwS(P_0, customControllerMap);
						BoolOption boolOption = BoolOption.Default;
						if (P_3)
						{
							boolOption = (P_2.HYjCrrWWbbZGtCHOVmKfIqcCVfmS ? BoolOption.True : BoolOption.False);
						}
						P_1.ykUEjrcRColDXoSlfWCQVezcPybhb(customControllerMap, boolOption);
					}
				}

				internal void NnhFHdcvDdZZhDqoCXXywvEaJxwS(Controller P_0, ControllerMap P_1)
				{
					if (P_0 != null && P_1 != null)
					{
						P_1.playerId = hIUdxPiJkzzwMniBaGspAmVWYBpDA.mgTogZEAHwpJMhbsccjZDcKdOLwp;
						P_0.FGOWYFxDhAlAiuIOIJEWlURoAViy(P_1);
					}
				}

				private IList<_0001> dMaatCfuRlkAbWjMsVoZOuiINcOh<_0001>(int P_0) where _0001 : ControllerMap
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = DtTYEnWeRlrCRbGTGYnarXrLwuZO<_0001>();
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_0);
					if (num < 0)
					{
						return EmptyObjects<_0001>.EmptyReadOnlyIListT;
					}
					return rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.HlfkqnKSqjcPLaWjYfBdVuecIjhS<_0001>();
				}

				private IList<_0001> ZKjySEpaacHITqtXbkcVKEsEMcaw<_0001>(Controller P_0) where _0001 : ControllerMap
				{
					return DtTYEnWeRlrCRbGTGYnarXrLwuZO<_0001>().XRkBbJBsJBaziaGmeTwVOAtSsQbXb(P_0)?.UEDbvZCpORwBphRIdGLFIEwJLiiEb.HlfkqnKSqjcPLaWjYfBdVuecIjhS<_0001>();
				}

				private IList<ControllerMap> XnHjpyvQzbZqAAKeLfiOTiCoWFVk(ControllerType P_0, int P_1)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return EmptyObjects<ControllerMap>.EmptyReadOnlyIListT;
					}
					return rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
				}

				private IList<ControllerMap> ZKjySEpaacHITqtXbkcVKEsEMcaw(Controller P_0)
				{
					return XnHjpyvQzbZqAAKeLfiOTiCoWFVk(P_0.type, P_0.id);
				}

				private void WIYoDggnULOEOJlnRTUnZVNTQRoJ(ControllerType P_0, int P_1, int P_2, int P_3)
				{
					CrgZgEQBnsBvSiIGZqpYlKdxIxUj(P_0, P_1, P_2, P_3, BoolOption.Default);
				}

				private void jgImdPyaQDuGlgChkbUwFzhxTMXX(Controller P_0, int P_1, int P_2)
				{
					RVxeHiEDLcxKUkJlvUMxldqvONIjb(P_0, P_1, P_2, BoolOption.Default);
				}

				private void dyOuSXPzRCEAsWIwpjaoGxwXZDjk(ControllerType P_0, int P_1, string P_2, string P_3)
				{
					JNxeEmFtrgQqskRAfoSbnbXngnlDb(P_0, P_1, P_2, P_3, BoolOption.Default);
				}

				private void JrJKBKeaKqJdheBPButuRgsuEEcM(Controller P_0, string P_1, string P_2)
				{
					TCbtAbodGqBsnTjjZrlapCtrJYFR(P_0, P_1, P_2, BoolOption.Default);
				}

				private void CrgZgEQBnsBvSiIGZqpYlKdxIxUj(ControllerType P_0, int P_1, int P_2, int P_3, BoolOption P_4)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num >= 0)
					{
						Controller controller = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).HwlnGjcjkEjHQFCbiyeLWdkAyzlm;
						ControllerMap controllerMap = ReInput.UserData.mlHpGZlwlGuuSPQRGehERBHqzRhy(controller, P_2, P_3);
						IVStlYfQmoamAgebBlgGsChGRtSbA(controller.type, controller.id, controllerMap, P_4);
					}
				}

				private void RVxeHiEDLcxKUkJlvUMxldqvONIjb(Controller P_0, int P_1, int P_2, BoolOption P_3)
				{
					CrgZgEQBnsBvSiIGZqpYlKdxIxUj(P_0.type, P_0.id, P_1, P_2, P_3);
				}

				private void JNxeEmFtrgQqskRAfoSbnbXngnlDb(ControllerType P_0, int P_1, string P_2, string P_3, BoolOption P_4)
				{
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(P_2);
					int layoutId = ReInput.mapping.GetLayoutId(P_0, P_3);
					if (mapCategoryId >= 0 && layoutId >= 0)
					{
						CrgZgEQBnsBvSiIGZqpYlKdxIxUj(P_0, P_1, mapCategoryId, layoutId, P_4);
					}
				}

				private void TCbtAbodGqBsnTjjZrlapCtrJYFR(Controller P_0, string P_1, string P_2, BoolOption P_3)
				{
					JNxeEmFtrgQqskRAfoSbnbXngnlDb(P_0.type, P_0.id, P_1, P_2, P_3);
				}

				private void DLGfgqBcXWhBlpiKNrbryajeUSgc(Controller P_0, ControllerMap P_1, BoolOption P_2)
				{
					if (P_0 != null && P_1 != null)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0.type);
						int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_0.id);
						if (num >= 0)
						{
							NnhFHdcvDdZZhDqoCXXywvEaJxwS(P_0, P_1);
							rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.mgScwhewbsnFFUycAEbitSGhYcjw(P_1, P_2);
							BzoEVmkFzWoLiXDbvANzqcaxGILS.Apply();
						}
					}
				}

				private void IVStlYfQmoamAgebBlgGsChGRtSbA(ControllerType P_0, int P_1, ControllerMap P_2, BoolOption P_3)
				{
					Controller controller = ReInput.controllers.GetController(P_0, P_1);
					if (controller != null)
					{
						DLGfgqBcXWhBlpiKNrbryajeUSgc(controller, P_2, P_3);
					}
				}

				private bool AlwEHyJFJsloIaMuDLeZemNLLLalB(ControllerType P_0, int P_1, string P_2)
				{
					if (P_2 == null || P_2 == string.Empty)
					{
						return false;
					}
					if (MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0).faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1) < 0)
					{
						return false;
					}
					ControllerMap controllerMap = ControllerMap.tTAynXDpgjHudMCdDyGjoeUtdBDX(P_0);
					try
					{
						ControllerMap.RAmMePHwhbbjmrfLAYKtBaJPbccQ();
						if (!controllerMap.COZPtosByiWFIIYpuMiNeiToydZW(P_2))
						{
							return false;
						}
					}
					finally
					{
						ControllerMap.oeOZZgeXJicFbaxfdmvQlNMqgCjfA();
					}
					IVStlYfQmoamAgebBlgGsChGRtSbA(P_0, P_1, controllerMap, BoolOption.Default);
					return true;
				}

				private int awdsiYqnWFQokxiLyoPfacjSwgwr(ControllerType P_0, int P_1, List<string> P_2)
				{
					if (P_2 == null || P_2.Count == 0)
					{
						return 0;
					}
					if (MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0).faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1) < 0)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < P_2.Count; i++)
					{
						if (AlwEHyJFJsloIaMuDLeZemNLLLalB(P_0, P_1, P_2[i]))
						{
							num++;
						}
					}
					return num;
				}

				private bool DfyCcjSqFpBGtCUaefWhtBtspXTY(ControllerType P_0, int P_1, string P_2)
				{
					if (P_2 == null || P_2 == string.Empty)
					{
						return false;
					}
					if (MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0).faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1) < 0)
					{
						return false;
					}
					ControllerMap controllerMap = ControllerMap.tTAynXDpgjHudMCdDyGjoeUtdBDX(P_0);
					try
					{
						ControllerMap.RAmMePHwhbbjmrfLAYKtBaJPbccQ();
						if (!controllerMap.fwCoBahZERRKwolRQILikHUYDTVq(P_2))
						{
							return false;
						}
					}
					finally
					{
						ControllerMap.oeOZZgeXJicFbaxfdmvQlNMqgCjfA();
					}
					IVStlYfQmoamAgebBlgGsChGRtSbA(P_0, P_1, controllerMap, BoolOption.Default);
					return true;
				}

				private int oefTrcDpwzcVXZXULSUCnoTACJvc(ControllerType P_0, int P_1, List<string> P_2)
				{
					if (P_2 == null || P_2.Count == 0)
					{
						return 0;
					}
					if (MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0).faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1) < 0)
					{
						return 0;
					}
					int num = 0;
					for (int i = 0; i < P_2.Count; i++)
					{
						if (DfyCcjSqFpBGtCUaefWhtBtspXTY(P_0, P_1, P_2[i]))
						{
							num++;
						}
					}
					return num;
				}

				private void ZcchzppkAZBRQPJLQCANJsaeeKs(ControllerType P_0, int P_1, int P_2, int P_3)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num >= 0)
					{
						Controller controller = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).HwlnGjcjkEjHQFCbiyeLWdkAyzlm;
						ControllerMap controllerMap = ControllerMap.MLvDPLunynqMBBBXdVjBBnmzSBEl(controller, P_2, P_3);
						IVStlYfQmoamAgebBlgGsChGRtSbA(controller.type, controller.id, controllerMap, BoolOption.Default);
					}
				}

				private void bWsIsRLiXaYSnjLleSMuEINPhfTCA(Controller P_0, int P_1, int P_2)
				{
					ZcchzppkAZBRQPJLQCANJsaeeKs(P_0.type, P_0.id, P_1, P_2);
				}

				private void PmIFtQxmWzwQeuuQObnryEqmXnQQ(ControllerType P_0, int P_1, string P_2, string P_3)
				{
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(P_2);
					int layoutId = ReInput.mapping.GetLayoutId(P_0, P_3);
					if (mapCategoryId >= 0 && layoutId >= 0)
					{
						ZcchzppkAZBRQPJLQCANJsaeeKs(P_0, P_1, mapCategoryId, layoutId);
					}
				}

				private void LfkMaWxjnhDOorWgHuyCVHLAXOjh(Controller P_0, string P_1, string P_2)
				{
					PmIFtQxmWzwQeuuQObnryEqmXnQQ(P_0.type, P_0.id, P_1, P_2);
				}

				private void sIibcxfpDPMvqHqkylBNFbTfmdRUb(ControllerType P_0, int P_1, int P_2)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num >= 0)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.JSHmeeXLyIGjUmEtNIqcTCGDbgZP(P_2);
					}
				}

				private void SjpJUJdJCAghScgDqTPsdpreUAIy(Controller P_0, int P_1)
				{
					sIibcxfpDPMvqHqkylBNFbTfmdRUb(P_0.type, P_0.id, P_1);
				}

				private void BBYfWRRkxZIXUswsspQtYALbMbZF(ControllerType P_0, int P_1, ControllerMap P_2)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num >= 0)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.PwiSezlPtSmefrXLzAWJKHDvZanD(P_2);
					}
				}

				private void dWcWaAykxwqjHXFbSRPzNGpqsFZw(Controller P_0, ControllerMap P_1)
				{
					sIibcxfpDPMvqHqkylBNFbTfmdRUb(P_0.type, P_0.id, P_1.id);
				}

				private void iRQWSPNjDNhyxSvurfHMFwJLwHoKA(ControllerType P_0, int P_1, int P_2, int P_3)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num >= 0)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.YlNZjFktSgaNDlvILASXXSiGNSDL(P_2, P_3);
					}
				}

				private void grmkKnMbSfceSuGcBsXYOCxEeUCV(Controller P_0, int P_1, int P_2)
				{
					iRQWSPNjDNhyxSvurfHMFwJLwHoKA(P_0.type, P_0.id, P_1, P_2);
				}

				private void UXdEsPladuxedDZbBWAGicbzDjWd(ControllerType P_0, int P_1, string P_2, string P_3)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num >= 0)
					{
						int mapCategoryId = ReInput.mapping.GetMapCategoryId(P_2);
						int layoutId = ReInput.mapping.GetLayoutId(P_0, P_3);
						if (mapCategoryId >= 0 && layoutId >= 0)
						{
							rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.YlNZjFktSgaNDlvILASXXSiGNSDL(mapCategoryId, layoutId);
						}
					}
				}

				private void wbUNOoNBuYSGfcEgLoIptKnDVGyg(Controller P_0, string P_1, string P_2)
				{
					UXdEsPladuxedDZbBWAGicbzDjWd(P_0.type, P_0.id, P_1, P_2);
				}

				private ControllerMap YKTVpqEFpLQdHdolBulaVqDyFLaA(ControllerType P_0, int P_1, int P_2)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return null;
					}
					return rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.kFWDBYhsObxbfnNZhRbLDvMmwCaq(P_2);
				}

				private ControllerMap xaOUtuozBlHBrKiYTflQHvOPAExGb(Controller P_0, int P_1)
				{
					return YKTVpqEFpLQdHdolBulaVqDyFLaA(P_0.type, P_0.id, P_1);
				}

				private ControllerMap NujCAmEoYXGYxJUyMGLRfuIAJRwGb(ControllerType P_0, int P_1, int P_2, int P_3)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return null;
					}
					return rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.uPKExlWfhhUSlkRQhpRMUAdVNYjG(P_2, P_3);
				}

				private ControllerMap UnQGrgyLjmaZiuvoHjVICSFbxrCBA(Controller P_0, int P_1, int P_2)
				{
					return NujCAmEoYXGYxJUyMGLRfuIAJRwGb(P_0.type, P_0.id, P_1, P_2);
				}

				private ControllerMap wBhHRmKGdrAKeeLguDRoAykGfDeA(ControllerType P_0, int P_1, string P_2, string P_3)
				{
					int mapCategoryId = ReInput.mapping.GetMapCategoryId(P_2);
					int layoutId = ReInput.mapping.GetLayoutId(P_0, P_3);
					if (mapCategoryId < 0 || layoutId < 0)
					{
						return null;
					}
					return NujCAmEoYXGYxJUyMGLRfuIAJRwGb(P_0, P_1, mapCategoryId, layoutId);
				}

				private ControllerMap JkIEzWPnsvEDvslcuCSqrQmstKgq(Controller P_0, string P_1, string P_2)
				{
					return wBhHRmKGdrAKeeLguDRoAykGfDeA(P_0.type, P_0.id, P_1, P_2);
				}

				private ControllerMap QSfOEAPjfqkNbtnmbYDefjqgzntE(ControllerType P_0, int P_1, int P_2)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return null;
					}
					return rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.bxkeVpiKsOiXKgaPOpdplUrJgEHk(P_2);
				}

				private ControllerMap TaNbdRkSdATOQBfdgZMhhNMaBLtU(Controller P_0, int P_1)
				{
					return QSfOEAPjfqkNbtnmbYDefjqgzntE(P_0.type, P_0.id, P_1);
				}

				private ControllerMap cPdTJbRlfONgNVfmpIDcJLhsHFSkA(ControllerType P_0, int P_1, string P_2)
				{
					int mapCategoryId = ReInput.UserData.GetMapCategoryId(P_2);
					if (mapCategoryId < 0)
					{
						return null;
					}
					return QSfOEAPjfqkNbtnmbYDefjqgzntE(P_0, P_1, mapCategoryId);
				}

				private ControllerMap hbHAzvFtHWfJfNakAFUqrlXipnfwA(Controller P_0, string P_1)
				{
					return cPdTJbRlfONgNVfmpIDcJLhsHFSkA(P_0.type, P_0.id, P_1);
				}

				private ControllerMap[] SCMrMhpHbdZTTHEMQnvamTROLMai(ControllerType P_0)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = 0;
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						num += rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.spuxbZMpjzXXEeAzzgWchppYZEErA;
					}
					ControllerMap[] array = new ControllerMap[num];
					num = 0;
					for (int j = 0; j < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; j++)
					{
						vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(j).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
						for (int k = 0; k < vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA; k++)
						{
							array[num] = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(k);
							num++;
						}
					}
					return array;
				}

				private ControllerMapSaveData[] EtNiIjtACRRUvtWGEeVmWDwIIhdh(ControllerType P_0, int P_1, bool P_2)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return null;
					}
					List<ControllerMapSaveData> list = new List<ControllerMapSaveData>();
					vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
					for (int i = 0; i < vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA; i++)
					{
						ControllerMap controllerMap = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(i);
						if (P_2)
						{
							InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(controllerMap.categoryId);
							if (mapCategory != null && !mapCategory.userAssignable)
							{
								continue;
							}
						}
						Controller controller = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).HwlnGjcjkEjHQFCbiyeLWdkAyzlm;
						list.Add(ControllerMapSaveData.UYgWdNkOFTIarlgJWXwfzYsPRwaR(controller, controllerMap));
					}
					return list.ToArray();
				}

				private _0001[] oNPthjcGJqIwOcylgCSnIMAfeZVB<_0001>(int P_0, bool P_1) where _0001 : ControllerMapSaveData
				{
					ControllerType controllerType = cVDyIiOsEfJNYzVuZSmuEXqylgT.prvTbtTHjRSdpHuUmwMZRSRbpBij<_0001>();
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_0);
					if (num < 0)
					{
						return null;
					}
					List<_0001> list = new List<_0001>();
					vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
					for (int i = 0; i < vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA; i++)
					{
						ControllerMap controllerMap = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(i);
						if (P_1)
						{
							InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(controllerMap.categoryId);
							if (mapCategory != null && !mapCategory.userAssignable)
							{
								continue;
							}
						}
						Controller controller = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).HwlnGjcjkEjHQFCbiyeLWdkAyzlm;
						list.Add(ControllerMapSaveData.UYgWdNkOFTIarlgJWXwfzYsPRwaR<_0001>(controller, controllerMap));
					}
					return list.ToArray();
				}

				private ControllerMapSaveData[] FttGgLfBxjhLCmfLSzKtNerWdhakA(ControllerType P_0, bool P_1)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					List<ControllerMapSaveData> list = new List<ControllerMapSaveData>();
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
						for (int j = 0; j < vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA; j++)
						{
							ControllerMap controllerMap = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(j);
							if (P_1)
							{
								InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(controllerMap.categoryId);
								if (mapCategory != null && !mapCategory.userAssignable)
								{
									continue;
								}
							}
							Controller controller = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).HwlnGjcjkEjHQFCbiyeLWdkAyzlm;
							list.Add(ControllerMapSaveData.UYgWdNkOFTIarlgJWXwfzYsPRwaR(controller, controllerMap));
						}
					}
					return list.ToArray();
				}

				private _0001[] dywRzXOdDCKiRQkFNfvxaguKVVAqA<_0001>(bool P_0) where _0001 : ControllerMapSaveData
				{
					ControllerType controllerType = cVDyIiOsEfJNYzVuZSmuEXqylgT.prvTbtTHjRSdpHuUmwMZRSRbpBij<_0001>();
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType);
					List<_0001> list = new List<_0001>();
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
						for (int j = 0; j < vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA; j++)
						{
							ControllerMap controllerMap = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(j);
							if (P_0)
							{
								InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(controllerMap.categoryId);
								if (mapCategory != null && !mapCategory.userAssignable)
								{
									continue;
								}
							}
							Controller controller = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).HwlnGjcjkEjHQFCbiyeLWdkAyzlm;
							list.Add(ControllerMapSaveData.UYgWdNkOFTIarlgJWXwfzYsPRwaR<_0001>(controller, controllerMap));
						}
					}
					return list.ToArray();
				}

				private int qDJmSkyavJtMjRSQkmWTUiOwNmfT(ControllerType P_0, int P_1, int P_2, List<ControllerMap> P_3)
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return 0;
					}
					return rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.nmtgykGGQmbTkMLvRzaknpIpupeMA(P_2, P_3, false);
				}

				private int lAojaUjZEPlsrimQNWySruiExDSe(Controller P_0, int P_1, List<ControllerMap> P_2)
				{
					return qDJmSkyavJtMjRSQkmWTUiOwNmfT(P_0.type, P_0.id, P_1, P_2);
				}

				private int hGYfEccaOVBGowRhRLFDkBFroKOy(ControllerType P_0, int P_1, string P_2, List<ControllerMap> P_3)
				{
					int mapCategoryId = ReInput.UserData.GetMapCategoryId(P_2);
					if (mapCategoryId < 0)
					{
						return 0;
					}
					return qDJmSkyavJtMjRSQkmWTUiOwNmfT(P_0, P_1, mapCategoryId, P_3);
				}

				private int QWBKUpYfdwLGlFmDbcGIhcCcHBlCb(Controller P_0, string P_1, List<ControllerMap> P_2)
				{
					return hGYfEccaOVBGowRhRLFDkBFroKOy(P_0.type, P_0.id, P_1, P_2);
				}

				[IteratorStateMachine(typeof(NXqgpGhehClIXdrGgInUICKZbVMtA))]
				private IEnumerable<ControllerMap> rRcAfuNJBsgvtVQYMgcaAAUfQdZJA(ControllerType P_0, int P_1, int P_2)
				{
					return new NXqgpGhehClIXdrGgInUICKZbVMtA(-2)
					{
						GmmdVkQGZVGlwzrUoIbmEiqRcqgfA = this,
						idzFzvVrZSrNMVCGcwRxMoCdWgXG = P_0,
						eePGklArUlluWtozTEdyouDufCwlA = P_1,
						cgJZhJswBclBeuevcPiQgcLyBRxHA = P_2
					};
				}

				[IteratorStateMachine(typeof(cgOEWFBxxXyUDtUbVEMLCdlpJkszA))]
				private IEnumerable<_0001> EuuNGZRSZapFoCLBQiDZuQiKoDTR<_0001>(int P_0, int P_1) where _0001 : ControllerMap
				{
					return new cgOEWFBxxXyUDtUbVEMLCdlpJkszA<_0001>(-2)
					{
						fUglqonmBZHvsErINdRrTqawGKoX = this,
						CmjQnJyQJlISMZdqdEdSZVMyPbCQ = P_0,
						FfVcoMOJYPbColKSMsnRJznSMTFr = P_1
					};
				}

				private ActionElementMap GGcybEtQYclffaLHiFcVwAEagDGBA(ControllerType P_0, int P_1, bool P_2)
				{
					if (P_1 < 0)
					{
						return null;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
						for (int j = 0; j < list.Count; j++)
						{
							if ((!P_2 || list[j].enabled) && list[j].ContainsAction(P_1))
							{
								ActionElementMap firstButtonMapWithAction = list[j].GetFirstButtonMapWithAction(P_1, P_2);
								if (firstButtonMapWithAction != null)
								{
									return firstButtonMapWithAction;
								}
							}
						}
					}
					return null;
				}

				private ActionElementMap pnePAwzqKThaprpykyJLgltJKDLw(ControllerType P_0, string P_1, bool P_2)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_1);
					return GGcybEtQYclffaLHiFcVwAEagDGBA(P_0, num, P_2);
				}

				[IteratorStateMachine(typeof(SkFpOgciCsVWqHnXmXTnAlBRGqhL))]
				private IEnumerable<ActionElementMap> PaFzQOudJtjLHlGDCINSApIEGqahb(ControllerType P_0, int P_1, bool P_2)
				{
					return new SkFpOgciCsVWqHnXmXTnAlBRGqhL(-2)
					{
						nTmTcZMXlTpCNhdQyeEjCrqErfhU = this,
						yMZgMLEpiVfQLovhAHmzpfvlzLTb = P_0,
						BaydhJTiRuuDNLmlANptKMwLUmeS = P_1,
						rGJFpbfUOhuPhWdiqLfurpqkfhzP = P_2
					};
				}

				private IEnumerable<ActionElementMap> XNmOHWlwnIveItQTxJtokhXWIoHv(ControllerType P_0, string P_1, bool P_2)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_1);
					return PaFzQOudJtjLHlGDCINSApIEGqahb(P_0, num, P_2);
				}

				private ActionElementMap mjWGynWchpQRVlNUDPaQweSXlmvG(ControllerType P_0, int P_1, bool P_2)
				{
					if (P_1 < 0)
					{
						return null;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
						for (int j = 0; j < list.Count; j++)
						{
							if (!(list[j] is ControllerMapWithAxes))
							{
								return null;
							}
							if ((!P_2 || list[j].enabled) && list[j].ContainsAction(P_1))
							{
								ActionElementMap firstAxisMapWithAction = (list[j] as ControllerMapWithAxes).GetFirstAxisMapWithAction(P_1, P_2);
								if (firstAxisMapWithAction != null)
								{
									return firstAxisMapWithAction;
								}
							}
						}
					}
					return null;
				}

				private ActionElementMap PhEsMSGulqOFaXHqdIbPsptQKdsp(ControllerType P_0, string P_1, bool P_2)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_1);
					return mjWGynWchpQRVlNUDPaQweSXlmvG(P_0, num, P_2);
				}

				[IteratorStateMachine(typeof(JldUPVCDlMTiKCYQUUvYxvRSwomI))]
				private IEnumerable<ActionElementMap> BBeecfKPrFwWgmOAPqVlwNPeFNAjb(ControllerType P_0, int P_1, bool P_2)
				{
					return new JldUPVCDlMTiKCYQUUvYxvRSwomI(-2)
					{
						YwfPjnPQVSQeWXAJrfpaMPbtGWjU = this,
						gpUJaCNkvsLZTXmVbeIrmgSaGtQz = P_0,
						vqZvdSXeZrOcPXXNTckmlevfRnRW = P_1,
						RIvfwGrWINsWpfqRCxxVyaHsCsZBA = P_2
					};
				}

				private IEnumerable<ActionElementMap> HEKpODHhwwgIiqBoClBVmZilbfSW(ControllerType P_0, string P_1, bool P_2)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_1);
					return BBeecfKPrFwWgmOAPqVlwNPeFNAjb(P_0, num, P_2);
				}

				private ActionElementMap SOwgzlZRvTOVWlfZYRlumDZSUJMC(ControllerType P_0, int P_1, bool P_2)
				{
					if (P_1 < 0)
					{
						return null;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
						for (int j = 0; j < list.Count; j++)
						{
							if ((!P_2 || list[j].enabled) && list[j].ContainsAction(P_1))
							{
								ActionElementMap firstElementMapWithAction = list[j].GetFirstElementMapWithAction(P_1, P_2);
								if (firstElementMapWithAction != null)
								{
									return firstElementMapWithAction;
								}
							}
						}
					}
					return null;
				}

				private ActionElementMap QASjtacnrxOgJZWnJtWfbYUdIevp(ControllerType P_0, string P_1, bool P_2)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_1);
					return SOwgzlZRvTOVWlfZYRlumDZSUJMC(P_0, num, P_2);
				}

				[IteratorStateMachine(typeof(dgXTotYzdjpgFKeKxZpeeZuXRKUF))]
				private IEnumerable<ActionElementMap> dxqvTowISvqfGOltXHnnveNMzcvk(ControllerType P_0, int P_1, bool P_2)
				{
					return new dgXTotYzdjpgFKeKxZpeeZuXRKUF(-2)
					{
						jUcbobiPbeJSdGSHbwCNGnupGvyy = this,
						HBBwQjBwOvhwnqhsoSKUasjqGnoW = P_0,
						srSVfBYYtXWggITDxfkhzIYpOzdb = P_1,
						mnaJSFrpkieQGDaIMqZjbpAcJLhIB = P_2
					};
				}

				private IEnumerable<ActionElementMap> SUnzhCPLAgdZpqylQCYTDRWNOhsL(ControllerType P_0, string P_1, bool P_2)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_1);
					return dxqvTowISvqfGOltXHnnveNMzcvk(P_0, num, P_2);
				}

				private int BKXMvXggBcGPasRKnfVKgUTBQtNt(int P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
				{
					if (P_2 == null)
					{
						throw new ArgumentNullException("results");
					}
					if (!P_3)
					{
						P_2.Clear();
					}
					if (P_0 < 0)
					{
						return 0;
					}
					int num = 0;
					int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i);
						int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						for (int j = 0; j < num2; j++)
						{
							vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(j).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
							int num3 = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA;
							for (int k = 0; k < num3; k++)
							{
								ControllerMap controllerMap = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(k);
								if ((!P_1 || controllerMap.enabled) && controllerMap.ContainsAction(P_0))
								{
									num += controllerMap.iQchYbBsHSgyRzunLuzvVgWwAaeh(P_0, P_1, P_2, true);
								}
							}
						}
					}
					return num;
				}

				private int JlwOzOMHRrLCBjfVIoUdOHKfJjfp(int P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
				{
					if (P_2 == null)
					{
						throw new ArgumentNullException("results");
					}
					if (!P_3)
					{
						P_2.Clear();
					}
					if (P_0 < 0)
					{
						return 0;
					}
					int num = 0;
					int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i);
						int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						for (int j = 0; j < num2; j++)
						{
							vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(j).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
							int num3 = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA;
							for (int k = 0; k < num3; k++)
							{
								if (vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(k) is ControllerMapWithAxes controllerMapWithAxes && (!P_1 || controllerMapWithAxes.enabled) && controllerMapWithAxes.ContainsAction(P_0))
								{
									num += controllerMapWithAxes.DYmUwpxHrWBxsHRjasXByyALHrNM(P_0, P_1, P_2, true);
								}
							}
						}
					}
					return num;
				}

				private int IamsQkJumiKpKMYPjnsRgiLPxZIG(int P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
				{
					if (P_2 == null)
					{
						throw new ArgumentNullException("results");
					}
					if (!P_3)
					{
						P_2.Clear();
					}
					if (P_0 < 0)
					{
						return 0;
					}
					int num = 0;
					int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
					for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
					{
						rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i);
						int num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
						for (int j = 0; j < num2; j++)
						{
							vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(j).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
							int num3 = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA;
							for (int k = 0; k < num3; k++)
							{
								ControllerMap controllerMap = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(k);
								if ((!P_1 || controllerMap.enabled) && controllerMap.ContainsAction(P_0))
								{
									num += controllerMap.StwzmqpEamBsxgECgGOOyjBfNurP(P_0, P_1, P_2, true);
								}
							}
						}
					}
					return num;
				}

				private int cJYeMFDNrVbuwyIazXdSJnxSNpQMA(ControllerType P_0, int P_1, bool P_2, List<ActionElementMap> P_3, bool P_4)
				{
					if (P_3 == null)
					{
						throw new ArgumentNullException("results");
					}
					if (!P_4)
					{
						P_3.Clear();
					}
					if (P_1 < 0)
					{
						return 0;
					}
					int num = 0;
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
						for (int j = 0; j < list.Count; j++)
						{
							if ((!P_2 || list[j].enabled) && list[j].ContainsAction(P_1))
							{
								num += list[j].iQchYbBsHSgyRzunLuzvVgWwAaeh(P_1, P_2, P_3, true);
							}
						}
					}
					return num;
				}

				private int GfZGJCblxGzOQVPvtYIPXMTscqTBA(ControllerType P_0, string P_1, bool P_2, List<ActionElementMap> P_3, bool P_4)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_1);
					return cJYeMFDNrVbuwyIazXdSJnxSNpQMA(P_0, num, P_2, P_3, P_4);
				}

				private int AHyCEXYuWIoSPjdmKlWLiKxrCCzM(ControllerType P_0, int P_1, bool P_2, List<ActionElementMap> P_3, bool P_4)
				{
					if (P_3 == null)
					{
						throw new ArgumentNullException("results");
					}
					if (!P_4)
					{
						P_3.Clear();
					}
					if (P_1 < 0)
					{
						return 0;
					}
					int num = 0;
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
						for (int j = 0; j < list.Count; j++)
						{
							if (!(list[j] is ControllerMapWithAxes))
							{
								return P_3.Count;
							}
							if ((!P_2 || list[j].enabled) && list[j].ContainsAction(P_1))
							{
								num += (list[j] as ControllerMapWithAxes).DYmUwpxHrWBxsHRjasXByyALHrNM(P_1, P_2, P_3, true);
							}
						}
					}
					return num;
				}

				private int OyMisaScivjvddfHHfcNvasqDreZ(ControllerType P_0, string P_1, bool P_2, List<ActionElementMap> P_3, bool P_4)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_1);
					return AHyCEXYuWIoSPjdmKlWLiKxrCCzM(P_0, num, P_2, P_3, P_4);
				}

				private int OdRdceERZVZsGGbtlLdVbSOUZRAGA(ControllerType P_0, int P_1, bool P_2, List<ActionElementMap> P_3, bool P_4)
				{
					if (P_3 == null)
					{
						throw new ArgumentNullException("results");
					}
					if (!P_4)
					{
						P_3.Clear();
					}
					if (P_1 < 0)
					{
						return 0;
					}
					int num = 0;
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					for (int i = 0; i < rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib; i++)
					{
						IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
						for (int j = 0; j < list.Count; j++)
						{
							if ((!P_2 || list[j].enabled) && list[j].ContainsAction(P_1))
							{
								num += list[j].StwzmqpEamBsxgECgGOOyjBfNurP(P_1, P_2, P_3, true);
							}
						}
					}
					return num;
				}

				private int AgvWzVcbXnpCVQtSYNBzBrngjKyQ(ControllerType P_0, string P_1, bool P_2, List<ActionElementMap> P_3, bool P_4)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_1);
					return OdRdceERZVZsGGbtlLdVbSOUZRAGA(P_0, num, P_2, P_3, P_4);
				}

				private ActionElementMap KkBMUbgOayfRMPdJjxkYizGjbmPp(ControllerType P_0, int P_1, int P_2, bool P_3)
				{
					if (P_2 < 0)
					{
						return null;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return null;
					}
					IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
					for (int i = 0; i < list.Count; i++)
					{
						if ((!P_3 || list[i].enabled) && list[i].ContainsAction(P_2))
						{
							ActionElementMap firstButtonMapWithAction = list[i].GetFirstButtonMapWithAction(P_2, P_3);
							if (firstButtonMapWithAction != null)
							{
								return firstButtonMapWithAction;
							}
						}
					}
					return null;
				}

				private ActionElementMap SmBfPiTWfMMtgJxWEmuEQyNGhDyI(ControllerType P_0, int P_1, string P_2, bool P_3)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_2);
					return KkBMUbgOayfRMPdJjxkYizGjbmPp(P_0, P_1, num, P_3);
				}

				[IteratorStateMachine(typeof(zkEKkSbmgisgTJXFlpIZQOAjWTyB))]
				private IEnumerable<ActionElementMap> ffTnnTcFRMihxlAqJIKibTKpOMl(ControllerType P_0, int P_1, int P_2, bool P_3)
				{
					return new zkEKkSbmgisgTJXFlpIZQOAjWTyB(-2)
					{
						kwjlrkdFDqDomvpeFROBdKwUzvcV = this,
						rAcRQDAbzBltJGCbvYGAZSfqBQFl = P_0,
						ijusRhQqZvsIPVlPfYjZVOEGAVFHA = P_1,
						RTxfqScQuvMSbASoolhqwhpdDdXx = P_2,
						fEjjfzXJHwvPpsILNPGijhekNdgx = P_3
					};
				}

				private IEnumerable<ActionElementMap> AOdvmTZJXLXeGCcTgLIrcHqYlPls(ControllerType P_0, int P_1, string P_2, bool P_3)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_2);
					return ffTnnTcFRMihxlAqJIKibTKpOMl(P_0, P_1, num, P_3);
				}

				private ActionElementMap ktBmgLshdeREURiLWDPuoqTSGMtbA(ControllerType P_0, int P_1, int P_2, bool P_3)
				{
					if (P_2 < 0)
					{
						return null;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return null;
					}
					IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
					for (int i = 0; i < list.Count; i++)
					{
						if (!(list[i] is ControllerMapWithAxes))
						{
							return null;
						}
						if ((!P_3 || list[i].enabled) && list[i].ContainsAction(P_2))
						{
							ActionElementMap firstAxisMapWithAction = (list[i] as ControllerMapWithAxes).GetFirstAxisMapWithAction(P_2, P_3);
							if (firstAxisMapWithAction != null)
							{
								return firstAxisMapWithAction;
							}
						}
					}
					return null;
				}

				private ActionElementMap myVIYBaGDHQaWdyhyPfeOiJlXOIt(ControllerType P_0, int P_1, string P_2, bool P_3)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_2);
					return ktBmgLshdeREURiLWDPuoqTSGMtbA(P_0, P_1, num, P_3);
				}

				[IteratorStateMachine(typeof(BLKXTkwDeHSSxKfvFemOBIooEvqUA))]
				private IEnumerable<ActionElementMap> bUjZanronCDHyAfgedxeATYBSDufb(ControllerType P_0, int P_1, int P_2, bool P_3)
				{
					return new BLKXTkwDeHSSxKfvFemOBIooEvqUA(-2)
					{
						iejmNJcBCmEZCeXhgnlQPJlWBHJmA = this,
						WwmPFmZgsWHdeqrEbwDTshIHBiMB = P_0,
						xmZwZBhQTJnZSJivDenYOcXkGhWd = P_1,
						TrbalfsvoLxkTCeaZlQDDLdAelJU = P_2,
						OqZODjaQPScYKpbuXdJNWHnWRbln = P_3
					};
				}

				private IEnumerable<ActionElementMap> bGDVlZhqBLXlHoFlCjFfIgQJvfrJA(ControllerType P_0, int P_1, string P_2, bool P_3)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_2);
					return bUjZanronCDHyAfgedxeATYBSDufb(P_0, P_1, num, P_3);
				}

				private ActionElementMap QqKgRkGcuHqbQjRRGCwtTVEAMIaIb(ControllerType P_0, int P_1, int P_2, bool P_3)
				{
					if (P_2 < 0)
					{
						return null;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return null;
					}
					IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
					for (int i = 0; i < list.Count; i++)
					{
						if ((!P_3 || list[i].enabled) && list[i].ContainsAction(P_2))
						{
							ActionElementMap firstElementMapWithAction = list[i].GetFirstElementMapWithAction(P_2, P_3);
							if (firstElementMapWithAction != null)
							{
								return firstElementMapWithAction;
							}
						}
					}
					return null;
				}

				private ActionElementMap oyReUjWOQeeTKRMScyhKuVslMwKs(ControllerType P_0, int P_1, string P_2, bool P_3)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_2);
					return QqKgRkGcuHqbQjRRGCwtTVEAMIaIb(P_0, P_1, num, P_3);
				}

				[IteratorStateMachine(typeof(OGCeldLwcwvXcFXOhXYIHpXJQDaj))]
				private IEnumerable<ActionElementMap> lUdjRFCAnBNKRdrPGMsZphmYbNJJ(ControllerType P_0, int P_1, int P_2, bool P_3)
				{
					return new OGCeldLwcwvXcFXOhXYIHpXJQDaj(-2)
					{
						rgzqAfLUTwfVDDqYlSzucuJOakqg = this,
						FGwMoBPueckqLRPzPbMzhTKuSjER = P_0,
						kslDCAgzYYTdwoPWlBUlvncSwWjX = P_1,
						NBGKhrMrjmBXBtuEcPuyseySctgb = P_2,
						gVXQISOmJseLkfxptiHechnnNMYp = P_3
					};
				}

				private IEnumerable<ActionElementMap> wcrotgsaKlZbgsUKZStIYSVwjpdc(ControllerType P_0, int P_1, string P_2, bool P_3)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_2);
					return lUdjRFCAnBNKRdrPGMsZphmYbNJJ(P_0, P_1, num, P_3);
				}

				private int HTpCtkjGjOUtkBMsNhxSfYzKiYen(ControllerType P_0, int P_1, int P_2, bool P_3, List<ActionElementMap> P_4, bool P_5)
				{
					if (P_4 == null)
					{
						throw new ArgumentNullException("results");
					}
					if (!P_5)
					{
						P_4.Clear();
					}
					if (P_2 < 0)
					{
						return 0;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return 0;
					}
					int num2 = 0;
					IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerMap controllerMap = list[i];
						if ((!P_3 || controllerMap.enabled) && controllerMap.ContainsAction(P_2))
						{
							num2 += controllerMap.iQchYbBsHSgyRzunLuzvVgWwAaeh(P_2, P_3, P_4, true);
						}
					}
					return num2;
				}

				private int AwRBQZPLTnfAUeOMXHmakcihUAWnA(ControllerType P_0, int P_1, string P_2, bool P_3, List<ActionElementMap> P_4, bool P_5)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_2);
					return HTpCtkjGjOUtkBMsNhxSfYzKiYen(P_0, P_1, num, P_3, P_4, P_5);
				}

				private int bWNDcIPHubRAKUbEkUkGMRlDNqJj(ControllerType P_0, int P_1, int P_2, bool P_3, List<ActionElementMap> P_4, bool P_5)
				{
					if (P_4 == null)
					{
						throw new ArgumentNullException("results");
					}
					if (!P_5)
					{
						P_4.Clear();
					}
					if (P_2 < 0)
					{
						return 0;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return 0;
					}
					int num2 = 0;
					IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
					for (int i = 0; i < list.Count; i++)
					{
						ControllerMapWithAxes controllerMapWithAxes = list[i] as ControllerMapWithAxes;
						if (list == null)
						{
							return num2;
						}
						if ((!P_3 || controllerMapWithAxes.enabled) && controllerMapWithAxes.ContainsAction(P_2))
						{
							num2 += controllerMapWithAxes.DYmUwpxHrWBxsHRjasXByyALHrNM(P_2, P_3, P_4, true);
						}
					}
					return num2;
				}

				private int frgDJhqJBiddUkDMLCqiVwXZWkSuA(ControllerType P_0, int P_1, string P_2, bool P_3, List<ActionElementMap> P_4, bool P_5)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_2);
					return bWNDcIPHubRAKUbEkUkGMRlDNqJj(P_0, P_1, num, P_3, P_4, P_5);
				}

				private int nDTInocIKxxokWjnWgWNzEnjcvTbb(ControllerType P_0, int P_1, int P_2, bool P_3, List<ActionElementMap> P_4, bool P_5)
				{
					if (P_4 == null)
					{
						throw new ArgumentNullException("results");
					}
					if (!P_5)
					{
						P_4.Clear();
					}
					if (P_2 < 0)
					{
						return 0;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.faRlVmfoiqiQrJTzbjHrcoaesFpg(P_1);
					if (num < 0)
					{
						return 0;
					}
					int num2 = 0;
					IList<ControllerMap> list = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).UEDbvZCpORwBphRIdGLFIEwJLiiEb.WLYuOXdjmnqxqMGLXCcRexyymPBD;
					for (int i = 0; i < list.Count; i++)
					{
						if ((!P_3 || list[i].enabled) && list[i].ContainsAction(P_2))
						{
							num2 += list[i].StwzmqpEamBsxgECgGOOyjBfNurP(P_2, P_3, P_4, true);
						}
					}
					return num2;
				}

				private int vNxWBTeTjYOCGnSwaXXZKxpxVtcC(ControllerType P_0, int P_1, string P_2, bool P_3, List<ActionElementMap> P_4, bool P_5)
				{
					int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(P_2);
					return nDTInocIKxxokWjnWgWNzEnjcvTbb(P_0, P_1, num, P_3, P_4, P_5);
				}

				private ActionElementMap GyreroAVwPqScvaRmilmMlloNNMBb(IControllerElementTarget P_0, bool P_1, int P_2, bool P_3)
				{
					if (P_0 == null)
					{
						return null;
					}
					Controller controller = P_0.controller;
					if (controller == null)
					{
						return null;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controller.type);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
					for (int i = 0; i < num; i++)
					{
						vHszkbCJdDAIcILHhpVCxcZlIBxlA obj = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
						_ = obj.spuxbZMpjzXXEeAzzgWchppYZEErA;
						IList<ControllerMap> list = obj.WLYuOXdjmnqxqMGLXCcRexyymPBD;
						int count = list.Count;
						for (int j = 0; j < count; j++)
						{
							ControllerMap controllerMap = list[j];
							if (!P_3 || controllerMap.enabled)
							{
								bool flag;
								ActionElementMap actionElementMap = controllerMap.BtSVfeTrzQORMCWyDKlguEcdjZpHA(P_0, P_1, P_2, P_3, out flag);
								if (actionElementMap != null)
								{
									return actionElementMap;
								}
							}
						}
					}
					return null;
				}

				[IteratorStateMachine(typeof(LZvJquRtIrioJsfORfvxGoMBKoNB))]
				private IEnumerable<ActionElementMap> rsGSVRQBPvMlvLnSdWKCtZdfnMak(IControllerElementTarget P_0, bool P_1, int P_2, bool P_3)
				{
					return new LZvJquRtIrioJsfORfvxGoMBKoNB(-2)
					{
						EizHEAdbFxFQWiOBxOiutfvAmlVSA = this,
						RreMBcFqyHWGonZvkNDhLzUOYuxC = P_0,
						JckDxfFWsYOcyBmFfsVZPjesUkogA = P_1,
						vwvuUPcxsFjFpgtxxotJdQwUqcUg = P_2,
						rXjhwyElZnJDoJFHyksVavgqTbbK = P_3
					};
				}

				private int yeNkzsJqACgKPDpNkpCqQVNFoxEO(IControllerElementTarget P_0, bool P_1, int P_2, bool P_3, List<ActionElementMap> P_4, bool P_5)
				{
					if (P_4 == null)
					{
						throw new ArgumentNullException("results");
					}
					if (!P_5)
					{
						P_4.Clear();
					}
					if (P_0 == null)
					{
						return 0;
					}
					Controller controller = P_0.controller;
					if (controller == null)
					{
						return 0;
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = MNHGNJfFQqpihvuAqNaSqIrXWcffA.eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controller.type);
					int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						vHszkbCJdDAIcILHhpVCxcZlIBxlA obj = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
						_ = obj.spuxbZMpjzXXEeAzzgWchppYZEErA;
						IList<ControllerMap> list = obj.WLYuOXdjmnqxqMGLXCcRexyymPBD;
						int count = list.Count;
						for (int j = 0; j < count; j++)
						{
							ControllerMap controllerMap = list[j];
							if (!P_3 || controllerMap.enabled)
							{
								num2 += controllerMap.yxkdMnawZniDZXFmujYxcNWEtVSFA(P_0, P_1, P_2, P_3, P_4, P_5, out var _);
							}
						}
					}
					return num2;
				}
			}

			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public sealed class PollingHelper : CodeHelper
			{
				private sealed class IpYaLajIsurpWKzEmtrjdKMcSKPP : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int NfYfUtsGJhmhdRAUalduzjYmepIs;

					private ControllerPollingInfo xHvJGsNhVpaPGrUepDcjNxXhDXCt;

					private int jGDJdZwKHdoLfIDevMnDfzShotEv;

					public PollingHelper kvtDJbCdydPVGJzPierTgDMHoSClB;

					private IList<CustomController> IejtFRWgiYIWhdKlQcIGYTChYDDl;

					private int uqCZKscaoCfGhDOsEMjTjBTwYXRgA;

					private int HVwQoldTjytBNRpPwOVnMTMZDMTb;

					private IEnumerator<ControllerPollingInfo> hDfCxkcEPEfjzjRBaspORSoMojwEA;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return xHvJGsNhVpaPGrUepDcjNxXhDXCt;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return xHvJGsNhVpaPGrUepDcjNxXhDXCt;
						}
					}

					[DebuggerHidden]
					public IpYaLajIsurpWKzEmtrjdKMcSKPP(int P_0)
					{
						NfYfUtsGJhmhdRAUalduzjYmepIs = P_0;
						jGDJdZwKHdoLfIDevMnDfzShotEv = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int nfYfUtsGJhmhdRAUalduzjYmepIs = NfYfUtsGJhmhdRAUalduzjYmepIs;
						if (nfYfUtsGJhmhdRAUalduzjYmepIs == -3 || nfYfUtsGJhmhdRAUalduzjYmepIs == 1)
						{
							try
							{
							}
							finally
							{
								KNBbkgkPxGbZlbLgCVERdzVVvgPgc();
							}
						}
						IejtFRWgiYIWhdKlQcIGYTChYDDl = null;
						hDfCxkcEPEfjzjRBaspORSoMojwEA = null;
						NfYfUtsGJhmhdRAUalduzjYmepIs = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int nfYfUtsGJhmhdRAUalduzjYmepIs = NfYfUtsGJhmhdRAUalduzjYmepIs;
							PollingHelper pollingHelper = kvtDJbCdydPVGJzPierTgDMHoSClB;
							if (nfYfUtsGJhmhdRAUalduzjYmepIs != 0)
							{
								if (nfYfUtsGJhmhdRAUalduzjYmepIs != 1)
								{
									return false;
								}
								NfYfUtsGJhmhdRAUalduzjYmepIs = -3;
								goto IL_00c5;
							}
							NfYfUtsGJhmhdRAUalduzjYmepIs = -1;
							IejtFRWgiYIWhdKlQcIGYTChYDDl = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
							uqCZKscaoCfGhDOsEMjTjBTwYXRgA = IejtFRWgiYIWhdKlQcIGYTChYDDl.Count;
							HVwQoldTjytBNRpPwOVnMTMZDMTb = 0;
							goto IL_00f1;
							IL_00c5:
							if (hDfCxkcEPEfjzjRBaspORSoMojwEA.MoveNext())
							{
								ControllerPollingInfo current = hDfCxkcEPEfjzjRBaspORSoMojwEA.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								xHvJGsNhVpaPGrUepDcjNxXhDXCt = controllerPollingInfo;
								NfYfUtsGJhmhdRAUalduzjYmepIs = 1;
								return true;
							}
							KNBbkgkPxGbZlbLgCVERdzVVvgPgc();
							hDfCxkcEPEfjzjRBaspORSoMojwEA = null;
							HVwQoldTjytBNRpPwOVnMTMZDMTb++;
							goto IL_00f1;
							IL_00f1:
							if (HVwQoldTjytBNRpPwOVnMTMZDMTb < uqCZKscaoCfGhDOsEMjTjBTwYXRgA)
							{
								hDfCxkcEPEfjzjRBaspORSoMojwEA = IejtFRWgiYIWhdKlQcIGYTChYDDl[HVwQoldTjytBNRpPwOVnMTMZDMTb].PollForAllAxes().GetEnumerator();
								NfYfUtsGJhmhdRAUalduzjYmepIs = -3;
								goto IL_00c5;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void KNBbkgkPxGbZlbLgCVERdzVVvgPgc()
					{
						NfYfUtsGJhmhdRAUalduzjYmepIs = -1;
						if (hDfCxkcEPEfjzjRBaspORSoMojwEA != null)
						{
							hDfCxkcEPEfjzjRBaspORSoMojwEA.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						IpYaLajIsurpWKzEmtrjdKMcSKPP ipYaLajIsurpWKzEmtrjdKMcSKPP;
						if (NfYfUtsGJhmhdRAUalduzjYmepIs == -2 && jGDJdZwKHdoLfIDevMnDfzShotEv == Environment.CurrentManagedThreadId)
						{
							NfYfUtsGJhmhdRAUalduzjYmepIs = 0;
							ipYaLajIsurpWKzEmtrjdKMcSKPP = this;
						}
						else
						{
							ipYaLajIsurpWKzEmtrjdKMcSKPP = new IpYaLajIsurpWKzEmtrjdKMcSKPP(0);
							ipYaLajIsurpWKzEmtrjdKMcSKPP.kvtDJbCdydPVGJzPierTgDMHoSClB = kvtDJbCdydPVGJzPierTgDMHoSClB;
						}
						return ipYaLajIsurpWKzEmtrjdKMcSKPP;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class cLlOxdgNaIkRwiQzffEpqJlfnlFH : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int UdFVIWWEVqzwHFDvWOijXGtZtICX;

					private ControllerPollingInfo LJaCdAhAVaQNuDkjEieasvbBZIgyc;

					private int XSaddXoVtOySUgtsYhxFQufaMaRQ;

					public PollingHelper gAINsDSOzOphRmsKbzcmdDrAhwnI;

					private IList<CustomController> OETZGZDRHHFkMCjKSaRlFGtVlURuA;

					private int sCgVdItdSSWAIHLnBtXUIaEkRsQk;

					private int GplnUuAcTcYJWATKGhiSHOuPIjYv;

					private IEnumerator<ControllerPollingInfo> zZZDokDQPpKlYZJaDpGJAWSacyOqA;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return LJaCdAhAVaQNuDkjEieasvbBZIgyc;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return LJaCdAhAVaQNuDkjEieasvbBZIgyc;
						}
					}

					[DebuggerHidden]
					public cLlOxdgNaIkRwiQzffEpqJlfnlFH(int P_0)
					{
						UdFVIWWEVqzwHFDvWOijXGtZtICX = P_0;
						XSaddXoVtOySUgtsYhxFQufaMaRQ = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int udFVIWWEVqzwHFDvWOijXGtZtICX = UdFVIWWEVqzwHFDvWOijXGtZtICX;
						if (udFVIWWEVqzwHFDvWOijXGtZtICX == -3 || udFVIWWEVqzwHFDvWOijXGtZtICX == 1)
						{
							try
							{
							}
							finally
							{
								OTuGPOnoSHnTwdYNkjypxZmaDcYg();
							}
						}
						OETZGZDRHHFkMCjKSaRlFGtVlURuA = null;
						zZZDokDQPpKlYZJaDpGJAWSacyOqA = null;
						UdFVIWWEVqzwHFDvWOijXGtZtICX = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int udFVIWWEVqzwHFDvWOijXGtZtICX = UdFVIWWEVqzwHFDvWOijXGtZtICX;
							PollingHelper pollingHelper = gAINsDSOzOphRmsKbzcmdDrAhwnI;
							if (udFVIWWEVqzwHFDvWOijXGtZtICX != 0)
							{
								if (udFVIWWEVqzwHFDvWOijXGtZtICX != 1)
								{
									return false;
								}
								UdFVIWWEVqzwHFDvWOijXGtZtICX = -3;
								goto IL_00c5;
							}
							UdFVIWWEVqzwHFDvWOijXGtZtICX = -1;
							OETZGZDRHHFkMCjKSaRlFGtVlURuA = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
							sCgVdItdSSWAIHLnBtXUIaEkRsQk = OETZGZDRHHFkMCjKSaRlFGtVlURuA.Count;
							GplnUuAcTcYJWATKGhiSHOuPIjYv = 0;
							goto IL_00f1;
							IL_00c5:
							if (zZZDokDQPpKlYZJaDpGJAWSacyOqA.MoveNext())
							{
								ControllerPollingInfo current = zZZDokDQPpKlYZJaDpGJAWSacyOqA.Current;
								ControllerPollingInfo lJaCdAhAVaQNuDkjEieasvbBZIgyc = new ControllerPollingInfo(current);
								lJaCdAhAVaQNuDkjEieasvbBZIgyc.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								LJaCdAhAVaQNuDkjEieasvbBZIgyc = lJaCdAhAVaQNuDkjEieasvbBZIgyc;
								UdFVIWWEVqzwHFDvWOijXGtZtICX = 1;
								return true;
							}
							OTuGPOnoSHnTwdYNkjypxZmaDcYg();
							zZZDokDQPpKlYZJaDpGJAWSacyOqA = null;
							GplnUuAcTcYJWATKGhiSHOuPIjYv++;
							goto IL_00f1;
							IL_00f1:
							if (GplnUuAcTcYJWATKGhiSHOuPIjYv < sCgVdItdSSWAIHLnBtXUIaEkRsQk)
							{
								zZZDokDQPpKlYZJaDpGJAWSacyOqA = OETZGZDRHHFkMCjKSaRlFGtVlURuA[GplnUuAcTcYJWATKGhiSHOuPIjYv].PollForAllButtons().GetEnumerator();
								UdFVIWWEVqzwHFDvWOijXGtZtICX = -3;
								goto IL_00c5;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void OTuGPOnoSHnTwdYNkjypxZmaDcYg()
					{
						UdFVIWWEVqzwHFDvWOijXGtZtICX = -1;
						if (zZZDokDQPpKlYZJaDpGJAWSacyOqA != null)
						{
							zZZDokDQPpKlYZJaDpGJAWSacyOqA.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						cLlOxdgNaIkRwiQzffEpqJlfnlFH cLlOxdgNaIkRwiQzffEpqJlfnlFH2;
						if (UdFVIWWEVqzwHFDvWOijXGtZtICX == -2 && XSaddXoVtOySUgtsYhxFQufaMaRQ == Environment.CurrentManagedThreadId)
						{
							UdFVIWWEVqzwHFDvWOijXGtZtICX = 0;
							cLlOxdgNaIkRwiQzffEpqJlfnlFH2 = this;
						}
						else
						{
							cLlOxdgNaIkRwiQzffEpqJlfnlFH2 = new cLlOxdgNaIkRwiQzffEpqJlfnlFH(0);
							cLlOxdgNaIkRwiQzffEpqJlfnlFH2.gAINsDSOzOphRmsKbzcmdDrAhwnI = gAINsDSOzOphRmsKbzcmdDrAhwnI;
						}
						return cLlOxdgNaIkRwiQzffEpqJlfnlFH2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class lzLoAJPHWFuoNQSQcEYDpbXkHZAt : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int wmMpdtmDdZuYAcLwcgOtVufXRqlf;

					private ControllerPollingInfo xTMZsSqNowjGyvTiqqyXzpGYrIMM;

					private int hxuNPYdNpgwHhtksMPxljrXyniCy;

					public PollingHelper jlNpgwPihJXTWDoqQBsctHMRibnG;

					private IList<CustomController> SNuHSrrjowgIpfqcGnxztquVSGLO;

					private int oPbdNFthPHvtnsYTdUVfTDFjddvHA;

					private int WTRNYIWtptpOskdFWZVhkfPOdNWcA;

					private IEnumerator<ControllerPollingInfo> lObMiDFnnwKbhykvKdFkyzmEhmao;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return xTMZsSqNowjGyvTiqqyXzpGYrIMM;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return xTMZsSqNowjGyvTiqqyXzpGYrIMM;
						}
					}

					[DebuggerHidden]
					public lzLoAJPHWFuoNQSQcEYDpbXkHZAt(int P_0)
					{
						wmMpdtmDdZuYAcLwcgOtVufXRqlf = P_0;
						hxuNPYdNpgwHhtksMPxljrXyniCy = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = wmMpdtmDdZuYAcLwcgOtVufXRqlf;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								sLwWuCITwIRSPunkOlbFNsZSYCmL();
							}
						}
						SNuHSrrjowgIpfqcGnxztquVSGLO = null;
						lObMiDFnnwKbhykvKdFkyzmEhmao = null;
						wmMpdtmDdZuYAcLwcgOtVufXRqlf = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = wmMpdtmDdZuYAcLwcgOtVufXRqlf;
							PollingHelper pollingHelper = jlNpgwPihJXTWDoqQBsctHMRibnG;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								wmMpdtmDdZuYAcLwcgOtVufXRqlf = -3;
								goto IL_00c5;
							}
							wmMpdtmDdZuYAcLwcgOtVufXRqlf = -1;
							SNuHSrrjowgIpfqcGnxztquVSGLO = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
							oPbdNFthPHvtnsYTdUVfTDFjddvHA = SNuHSrrjowgIpfqcGnxztquVSGLO.Count;
							WTRNYIWtptpOskdFWZVhkfPOdNWcA = 0;
							goto IL_00f1;
							IL_00c5:
							if (lObMiDFnnwKbhykvKdFkyzmEhmao.MoveNext())
							{
								ControllerPollingInfo current = lObMiDFnnwKbhykvKdFkyzmEhmao.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								xTMZsSqNowjGyvTiqqyXzpGYrIMM = controllerPollingInfo;
								wmMpdtmDdZuYAcLwcgOtVufXRqlf = 1;
								return true;
							}
							sLwWuCITwIRSPunkOlbFNsZSYCmL();
							lObMiDFnnwKbhykvKdFkyzmEhmao = null;
							WTRNYIWtptpOskdFWZVhkfPOdNWcA++;
							goto IL_00f1;
							IL_00f1:
							if (WTRNYIWtptpOskdFWZVhkfPOdNWcA < oPbdNFthPHvtnsYTdUVfTDFjddvHA)
							{
								lObMiDFnnwKbhykvKdFkyzmEhmao = SNuHSrrjowgIpfqcGnxztquVSGLO[WTRNYIWtptpOskdFWZVhkfPOdNWcA].PollForAllButtonsDown().GetEnumerator();
								wmMpdtmDdZuYAcLwcgOtVufXRqlf = -3;
								goto IL_00c5;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void sLwWuCITwIRSPunkOlbFNsZSYCmL()
					{
						wmMpdtmDdZuYAcLwcgOtVufXRqlf = -1;
						if (lObMiDFnnwKbhykvKdFkyzmEhmao != null)
						{
							lObMiDFnnwKbhykvKdFkyzmEhmao.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						lzLoAJPHWFuoNQSQcEYDpbXkHZAt lzLoAJPHWFuoNQSQcEYDpbXkHZAt2;
						if (wmMpdtmDdZuYAcLwcgOtVufXRqlf == -2 && hxuNPYdNpgwHhtksMPxljrXyniCy == Environment.CurrentManagedThreadId)
						{
							wmMpdtmDdZuYAcLwcgOtVufXRqlf = 0;
							lzLoAJPHWFuoNQSQcEYDpbXkHZAt2 = this;
						}
						else
						{
							lzLoAJPHWFuoNQSQcEYDpbXkHZAt2 = new lzLoAJPHWFuoNQSQcEYDpbXkHZAt(0);
							lzLoAJPHWFuoNQSQcEYDpbXkHZAt2.jlNpgwPihJXTWDoqQBsctHMRibnG = jlNpgwPihJXTWDoqQBsctHMRibnG;
						}
						return lzLoAJPHWFuoNQSQcEYDpbXkHZAt2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class AMzxBfXhbGSdEValZAXIKLuGnThV : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int NjMAamfdoNEgTjHEGOYtGddoAXJcb;

					private ControllerPollingInfo eWBAwBxxRcEqmzTsyRbpMHWLmsdP;

					private int MRzKnAymmsmIxdhucfEeKjOqYEyT;

					public PollingHelper wLxcgaEqFTfwnFwSeyKEqtSOsawqA;

					private IList<CustomController> jrxOtmwVwvjZIviEcrWeNxOVDqzn;

					private int JKWIgwsUkPqaVqUPMmxjIipHosnX;

					private int SmQITvzRskMhzasyqofxQZHlQpAw;

					private IEnumerator<ControllerPollingInfo> NPauxgDfKtNzcEOeeMEWSjBRwCtH;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return eWBAwBxxRcEqmzTsyRbpMHWLmsdP;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return eWBAwBxxRcEqmzTsyRbpMHWLmsdP;
						}
					}

					[DebuggerHidden]
					public AMzxBfXhbGSdEValZAXIKLuGnThV(int P_0)
					{
						NjMAamfdoNEgTjHEGOYtGddoAXJcb = P_0;
						MRzKnAymmsmIxdhucfEeKjOqYEyT = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int njMAamfdoNEgTjHEGOYtGddoAXJcb = NjMAamfdoNEgTjHEGOYtGddoAXJcb;
						if (njMAamfdoNEgTjHEGOYtGddoAXJcb == -3 || njMAamfdoNEgTjHEGOYtGddoAXJcb == 1)
						{
							try
							{
							}
							finally
							{
								OwTFacqDqkpvUsQYcbArDyWFULNW();
							}
						}
						jrxOtmwVwvjZIviEcrWeNxOVDqzn = null;
						NPauxgDfKtNzcEOeeMEWSjBRwCtH = null;
						NjMAamfdoNEgTjHEGOYtGddoAXJcb = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int njMAamfdoNEgTjHEGOYtGddoAXJcb = NjMAamfdoNEgTjHEGOYtGddoAXJcb;
							PollingHelper pollingHelper = wLxcgaEqFTfwnFwSeyKEqtSOsawqA;
							if (njMAamfdoNEgTjHEGOYtGddoAXJcb != 0)
							{
								if (njMAamfdoNEgTjHEGOYtGddoAXJcb != 1)
								{
									return false;
								}
								NjMAamfdoNEgTjHEGOYtGddoAXJcb = -3;
								goto IL_00c5;
							}
							NjMAamfdoNEgTjHEGOYtGddoAXJcb = -1;
							jrxOtmwVwvjZIviEcrWeNxOVDqzn = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
							JKWIgwsUkPqaVqUPMmxjIipHosnX = jrxOtmwVwvjZIviEcrWeNxOVDqzn.Count;
							SmQITvzRskMhzasyqofxQZHlQpAw = 0;
							goto IL_00f1;
							IL_00c5:
							if (NPauxgDfKtNzcEOeeMEWSjBRwCtH.MoveNext())
							{
								ControllerPollingInfo current = NPauxgDfKtNzcEOeeMEWSjBRwCtH.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								eWBAwBxxRcEqmzTsyRbpMHWLmsdP = controllerPollingInfo;
								NjMAamfdoNEgTjHEGOYtGddoAXJcb = 1;
								return true;
							}
							OwTFacqDqkpvUsQYcbArDyWFULNW();
							NPauxgDfKtNzcEOeeMEWSjBRwCtH = null;
							SmQITvzRskMhzasyqofxQZHlQpAw++;
							goto IL_00f1;
							IL_00f1:
							if (SmQITvzRskMhzasyqofxQZHlQpAw < JKWIgwsUkPqaVqUPMmxjIipHosnX)
							{
								NPauxgDfKtNzcEOeeMEWSjBRwCtH = jrxOtmwVwvjZIviEcrWeNxOVDqzn[SmQITvzRskMhzasyqofxQZHlQpAw].PollForAllElements().GetEnumerator();
								NjMAamfdoNEgTjHEGOYtGddoAXJcb = -3;
								goto IL_00c5;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void OwTFacqDqkpvUsQYcbArDyWFULNW()
					{
						NjMAamfdoNEgTjHEGOYtGddoAXJcb = -1;
						if (NPauxgDfKtNzcEOeeMEWSjBRwCtH != null)
						{
							NPauxgDfKtNzcEOeeMEWSjBRwCtH.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						AMzxBfXhbGSdEValZAXIKLuGnThV aMzxBfXhbGSdEValZAXIKLuGnThV;
						if (NjMAamfdoNEgTjHEGOYtGddoAXJcb == -2 && MRzKnAymmsmIxdhucfEeKjOqYEyT == Environment.CurrentManagedThreadId)
						{
							NjMAamfdoNEgTjHEGOYtGddoAXJcb = 0;
							aMzxBfXhbGSdEValZAXIKLuGnThV = this;
						}
						else
						{
							aMzxBfXhbGSdEValZAXIKLuGnThV = new AMzxBfXhbGSdEValZAXIKLuGnThV(0);
							aMzxBfXhbGSdEValZAXIKLuGnThV.wLxcgaEqFTfwnFwSeyKEqtSOsawqA = wLxcgaEqFTfwnFwSeyKEqtSOsawqA;
						}
						return aMzxBfXhbGSdEValZAXIKLuGnThV;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class JbBOoruIKCtMnrMeXnpijjZGUQOH : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int LDMuSJcZnBfyGDFRceVXYudoxNtl;

					private ControllerPollingInfo nLEHBxHZorzTGEebqlccRUKjCpDD;

					private int YwEgikUGKoiXNVDfTOlhllwlNPOR;

					public PollingHelper qKIJuOJunTPwnkuJjjSARfJsrfkI;

					private IList<CustomController> ywfFNvJZZmNHhMhoCqHqGdHCkyikA;

					private int MNuzzDALhSKOuLvFMSBUTleggkeX;

					private int vLnegmjdDMAGzgRDLRDcuCWXhyVc;

					private IEnumerator<ControllerPollingInfo> KeWahRwxTliDaNsgLSmeHoeUXAxf;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return nLEHBxHZorzTGEebqlccRUKjCpDD;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return nLEHBxHZorzTGEebqlccRUKjCpDD;
						}
					}

					[DebuggerHidden]
					public JbBOoruIKCtMnrMeXnpijjZGUQOH(int P_0)
					{
						LDMuSJcZnBfyGDFRceVXYudoxNtl = P_0;
						YwEgikUGKoiXNVDfTOlhllwlNPOR = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int lDMuSJcZnBfyGDFRceVXYudoxNtl = LDMuSJcZnBfyGDFRceVXYudoxNtl;
						if (lDMuSJcZnBfyGDFRceVXYudoxNtl == -3 || lDMuSJcZnBfyGDFRceVXYudoxNtl == 1)
						{
							try
							{
							}
							finally
							{
								UlLQpnApjsezOGGBJgyWAjjCgxpsB();
							}
						}
						ywfFNvJZZmNHhMhoCqHqGdHCkyikA = null;
						KeWahRwxTliDaNsgLSmeHoeUXAxf = null;
						LDMuSJcZnBfyGDFRceVXYudoxNtl = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int lDMuSJcZnBfyGDFRceVXYudoxNtl = LDMuSJcZnBfyGDFRceVXYudoxNtl;
							PollingHelper pollingHelper = qKIJuOJunTPwnkuJjjSARfJsrfkI;
							if (lDMuSJcZnBfyGDFRceVXYudoxNtl != 0)
							{
								if (lDMuSJcZnBfyGDFRceVXYudoxNtl != 1)
								{
									return false;
								}
								LDMuSJcZnBfyGDFRceVXYudoxNtl = -3;
								goto IL_00c5;
							}
							LDMuSJcZnBfyGDFRceVXYudoxNtl = -1;
							ywfFNvJZZmNHhMhoCqHqGdHCkyikA = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
							MNuzzDALhSKOuLvFMSBUTleggkeX = ywfFNvJZZmNHhMhoCqHqGdHCkyikA.Count;
							vLnegmjdDMAGzgRDLRDcuCWXhyVc = 0;
							goto IL_00f1;
							IL_00c5:
							if (KeWahRwxTliDaNsgLSmeHoeUXAxf.MoveNext())
							{
								ControllerPollingInfo current = KeWahRwxTliDaNsgLSmeHoeUXAxf.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								nLEHBxHZorzTGEebqlccRUKjCpDD = controllerPollingInfo;
								LDMuSJcZnBfyGDFRceVXYudoxNtl = 1;
								return true;
							}
							UlLQpnApjsezOGGBJgyWAjjCgxpsB();
							KeWahRwxTliDaNsgLSmeHoeUXAxf = null;
							vLnegmjdDMAGzgRDLRDcuCWXhyVc++;
							goto IL_00f1;
							IL_00f1:
							if (vLnegmjdDMAGzgRDLRDcuCWXhyVc < MNuzzDALhSKOuLvFMSBUTleggkeX)
							{
								KeWahRwxTliDaNsgLSmeHoeUXAxf = ywfFNvJZZmNHhMhoCqHqGdHCkyikA[vLnegmjdDMAGzgRDLRDcuCWXhyVc].PollForAllElementsDown().GetEnumerator();
								LDMuSJcZnBfyGDFRceVXYudoxNtl = -3;
								goto IL_00c5;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void UlLQpnApjsezOGGBJgyWAjjCgxpsB()
					{
						LDMuSJcZnBfyGDFRceVXYudoxNtl = -1;
						if (KeWahRwxTliDaNsgLSmeHoeUXAxf != null)
						{
							KeWahRwxTliDaNsgLSmeHoeUXAxf.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						JbBOoruIKCtMnrMeXnpijjZGUQOH jbBOoruIKCtMnrMeXnpijjZGUQOH;
						if (LDMuSJcZnBfyGDFRceVXYudoxNtl == -2 && YwEgikUGKoiXNVDfTOlhllwlNPOR == Environment.CurrentManagedThreadId)
						{
							LDMuSJcZnBfyGDFRceVXYudoxNtl = 0;
							jbBOoruIKCtMnrMeXnpijjZGUQOH = this;
						}
						else
						{
							jbBOoruIKCtMnrMeXnpijjZGUQOH = new JbBOoruIKCtMnrMeXnpijjZGUQOH(0);
							jbBOoruIKCtMnrMeXnpijjZGUQOH.qKIJuOJunTPwnkuJjjSARfJsrfkI = qKIJuOJunTPwnkuJjjSARfJsrfkI;
						}
						return jbBOoruIKCtMnrMeXnpijjZGUQOH;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class NApakeYkuNhsppTkhIZwICKhuSry : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int FwqeiEGtESszHGPfWooWkajmnkZxA;

					private ControllerPollingInfo yQiAQwazlfDsnYDeqyylEXAvepKwA;

					private int gsJIPlFyZCsYVkKYcePJGZthrsKJ;

					public PollingHelper mXupIgPOGIqbGSmjhALOAOJMiqOG;

					private IList<Joystick> VsSMlQPYgCWZgYCLGRMxbFWCOdgy;

					private int RXhRcvcEAMCugJobyQzHRwpTMdeI;

					private int ceVYtBikJVeMXNnyVMufNIIHkKWm;

					private IEnumerator<ControllerPollingInfo> DZbaxIDARgvWPknHyyTQiPaWGybAb;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return yQiAQwazlfDsnYDeqyylEXAvepKwA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return yQiAQwazlfDsnYDeqyylEXAvepKwA;
						}
					}

					[DebuggerHidden]
					public NApakeYkuNhsppTkhIZwICKhuSry(int P_0)
					{
						FwqeiEGtESszHGPfWooWkajmnkZxA = P_0;
						gsJIPlFyZCsYVkKYcePJGZthrsKJ = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int fwqeiEGtESszHGPfWooWkajmnkZxA = FwqeiEGtESszHGPfWooWkajmnkZxA;
						if (fwqeiEGtESszHGPfWooWkajmnkZxA == -3 || fwqeiEGtESszHGPfWooWkajmnkZxA == 1)
						{
							try
							{
							}
							finally
							{
								GFDjYXbKUSBqyRzAScuuDcSZFhlab();
							}
						}
						VsSMlQPYgCWZgYCLGRMxbFWCOdgy = null;
						DZbaxIDARgvWPknHyyTQiPaWGybAb = null;
						FwqeiEGtESszHGPfWooWkajmnkZxA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int fwqeiEGtESszHGPfWooWkajmnkZxA = FwqeiEGtESszHGPfWooWkajmnkZxA;
							PollingHelper pollingHelper = mXupIgPOGIqbGSmjhALOAOJMiqOG;
							if (fwqeiEGtESszHGPfWooWkajmnkZxA != 0)
							{
								if (fwqeiEGtESszHGPfWooWkajmnkZxA != 1)
								{
									return false;
								}
								FwqeiEGtESszHGPfWooWkajmnkZxA = -3;
								goto IL_00c5;
							}
							FwqeiEGtESszHGPfWooWkajmnkZxA = -1;
							VsSMlQPYgCWZgYCLGRMxbFWCOdgy = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
							RXhRcvcEAMCugJobyQzHRwpTMdeI = VsSMlQPYgCWZgYCLGRMxbFWCOdgy.Count;
							ceVYtBikJVeMXNnyVMufNIIHkKWm = 0;
							goto IL_00f1;
							IL_00c5:
							if (DZbaxIDARgvWPknHyyTQiPaWGybAb.MoveNext())
							{
								ControllerPollingInfo current = DZbaxIDARgvWPknHyyTQiPaWGybAb.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								yQiAQwazlfDsnYDeqyylEXAvepKwA = controllerPollingInfo;
								FwqeiEGtESszHGPfWooWkajmnkZxA = 1;
								return true;
							}
							GFDjYXbKUSBqyRzAScuuDcSZFhlab();
							DZbaxIDARgvWPknHyyTQiPaWGybAb = null;
							ceVYtBikJVeMXNnyVMufNIIHkKWm++;
							goto IL_00f1;
							IL_00f1:
							if (ceVYtBikJVeMXNnyVMufNIIHkKWm < RXhRcvcEAMCugJobyQzHRwpTMdeI)
							{
								DZbaxIDARgvWPknHyyTQiPaWGybAb = VsSMlQPYgCWZgYCLGRMxbFWCOdgy[ceVYtBikJVeMXNnyVMufNIIHkKWm].PollForAllAxes().GetEnumerator();
								FwqeiEGtESszHGPfWooWkajmnkZxA = -3;
								goto IL_00c5;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void GFDjYXbKUSBqyRzAScuuDcSZFhlab()
					{
						FwqeiEGtESszHGPfWooWkajmnkZxA = -1;
						if (DZbaxIDARgvWPknHyyTQiPaWGybAb != null)
						{
							DZbaxIDARgvWPknHyyTQiPaWGybAb.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						NApakeYkuNhsppTkhIZwICKhuSry nApakeYkuNhsppTkhIZwICKhuSry;
						if (FwqeiEGtESszHGPfWooWkajmnkZxA == -2 && gsJIPlFyZCsYVkKYcePJGZthrsKJ == Environment.CurrentManagedThreadId)
						{
							FwqeiEGtESszHGPfWooWkajmnkZxA = 0;
							nApakeYkuNhsppTkhIZwICKhuSry = this;
						}
						else
						{
							nApakeYkuNhsppTkhIZwICKhuSry = new NApakeYkuNhsppTkhIZwICKhuSry(0);
							nApakeYkuNhsppTkhIZwICKhuSry.mXupIgPOGIqbGSmjhALOAOJMiqOG = mXupIgPOGIqbGSmjhALOAOJMiqOG;
						}
						return nApakeYkuNhsppTkhIZwICKhuSry;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class dLfvWuPauaQBidWxKvAQKzodNKYA : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int pPRkCULOYuiEKVxdrmWfQsKsWvQn;

					private ControllerPollingInfo ktWcWXHfVKGjpPiEWSZhTcdfibhM;

					private int nEzenjBjZfCOsvRxeQCNIaKztIFZ;

					public PollingHelper UdKfDejjHDnniYTdsIOVUpJIPydr;

					private IList<Joystick> UqVyCGcHLtWnIqUUSKPrmBAmFRBKA;

					private int nyDnmPoEVrpAdgFQTIGhZdUOJjRN;

					private int rKUamchewTWrGPpplBFbaJXysQoo;

					private IEnumerator<ControllerPollingInfo> bzUKFyJCLcMGYHMDYLEhsScozlGB;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return ktWcWXHfVKGjpPiEWSZhTcdfibhM;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return ktWcWXHfVKGjpPiEWSZhTcdfibhM;
						}
					}

					[DebuggerHidden]
					public dLfvWuPauaQBidWxKvAQKzodNKYA(int P_0)
					{
						pPRkCULOYuiEKVxdrmWfQsKsWvQn = P_0;
						nEzenjBjZfCOsvRxeQCNIaKztIFZ = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = pPRkCULOYuiEKVxdrmWfQsKsWvQn;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								SmnXovavTKgbfOlImqhOFoKyEmSi();
							}
						}
						UqVyCGcHLtWnIqUUSKPrmBAmFRBKA = null;
						bzUKFyJCLcMGYHMDYLEhsScozlGB = null;
						pPRkCULOYuiEKVxdrmWfQsKsWvQn = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = pPRkCULOYuiEKVxdrmWfQsKsWvQn;
							PollingHelper udKfDejjHDnniYTdsIOVUpJIPydr = UdKfDejjHDnniYTdsIOVUpJIPydr;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								pPRkCULOYuiEKVxdrmWfQsKsWvQn = -3;
								goto IL_00c5;
							}
							pPRkCULOYuiEKVxdrmWfQsKsWvQn = -1;
							UqVyCGcHLtWnIqUUSKPrmBAmFRBKA = udKfDejjHDnniYTdsIOVUpJIPydr.UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
							nyDnmPoEVrpAdgFQTIGhZdUOJjRN = UqVyCGcHLtWnIqUUSKPrmBAmFRBKA.Count;
							rKUamchewTWrGPpplBFbaJXysQoo = 0;
							goto IL_00f1;
							IL_00c5:
							if (bzUKFyJCLcMGYHMDYLEhsScozlGB.MoveNext())
							{
								ControllerPollingInfo current = bzUKFyJCLcMGYHMDYLEhsScozlGB.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = udKfDejjHDnniYTdsIOVUpJIPydr.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								ktWcWXHfVKGjpPiEWSZhTcdfibhM = controllerPollingInfo;
								pPRkCULOYuiEKVxdrmWfQsKsWvQn = 1;
								return true;
							}
							SmnXovavTKgbfOlImqhOFoKyEmSi();
							bzUKFyJCLcMGYHMDYLEhsScozlGB = null;
							rKUamchewTWrGPpplBFbaJXysQoo++;
							goto IL_00f1;
							IL_00f1:
							if (rKUamchewTWrGPpplBFbaJXysQoo < nyDnmPoEVrpAdgFQTIGhZdUOJjRN)
							{
								bzUKFyJCLcMGYHMDYLEhsScozlGB = UqVyCGcHLtWnIqUUSKPrmBAmFRBKA[rKUamchewTWrGPpplBFbaJXysQoo].PollForAllButtons().GetEnumerator();
								pPRkCULOYuiEKVxdrmWfQsKsWvQn = -3;
								goto IL_00c5;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void SmnXovavTKgbfOlImqhOFoKyEmSi()
					{
						pPRkCULOYuiEKVxdrmWfQsKsWvQn = -1;
						if (bzUKFyJCLcMGYHMDYLEhsScozlGB != null)
						{
							bzUKFyJCLcMGYHMDYLEhsScozlGB.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						dLfvWuPauaQBidWxKvAQKzodNKYA dLfvWuPauaQBidWxKvAQKzodNKYA2;
						if (pPRkCULOYuiEKVxdrmWfQsKsWvQn == -2 && nEzenjBjZfCOsvRxeQCNIaKztIFZ == Environment.CurrentManagedThreadId)
						{
							pPRkCULOYuiEKVxdrmWfQsKsWvQn = 0;
							dLfvWuPauaQBidWxKvAQKzodNKYA2 = this;
						}
						else
						{
							dLfvWuPauaQBidWxKvAQKzodNKYA2 = new dLfvWuPauaQBidWxKvAQKzodNKYA(0);
							dLfvWuPauaQBidWxKvAQKzodNKYA2.UdKfDejjHDnniYTdsIOVUpJIPydr = UdKfDejjHDnniYTdsIOVUpJIPydr;
						}
						return dLfvWuPauaQBidWxKvAQKzodNKYA2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class ArLzvAnOXbMsoVjRGdCfEajCJYLp : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int iZutCmuxcvWQOqLdhspUjvEhwyhG;

					private ControllerPollingInfo ZTDDPtxfsetihWmAdKRQAALRLPqF;

					private int GOcvfcqIeClNnyhLbTryDNWkgkwIA;

					public PollingHelper UyWRAXuhPoAJBIAhLgVprYeoUZwG;

					private IList<Joystick> iIaPMAOogBhZEjoWTHzMJdhjuaKg;

					private int fcQiCoGsOwdOCZqKZBXNciogYAiQb;

					private int IScuqnJmJWhjJmQtUelOkGqbztGsA;

					private IEnumerator<ControllerPollingInfo> vaQJyqVNpIFkENLLvpulRPdCeKYl;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return ZTDDPtxfsetihWmAdKRQAALRLPqF;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return ZTDDPtxfsetihWmAdKRQAALRLPqF;
						}
					}

					[DebuggerHidden]
					public ArLzvAnOXbMsoVjRGdCfEajCJYLp(int P_0)
					{
						iZutCmuxcvWQOqLdhspUjvEhwyhG = P_0;
						GOcvfcqIeClNnyhLbTryDNWkgkwIA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = iZutCmuxcvWQOqLdhspUjvEhwyhG;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								yWzkynoCTADwmDfCicBVJMnKfGRB();
							}
						}
						iIaPMAOogBhZEjoWTHzMJdhjuaKg = null;
						vaQJyqVNpIFkENLLvpulRPdCeKYl = null;
						iZutCmuxcvWQOqLdhspUjvEhwyhG = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = iZutCmuxcvWQOqLdhspUjvEhwyhG;
							PollingHelper uyWRAXuhPoAJBIAhLgVprYeoUZwG = UyWRAXuhPoAJBIAhLgVprYeoUZwG;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								iZutCmuxcvWQOqLdhspUjvEhwyhG = -3;
								goto IL_00c5;
							}
							iZutCmuxcvWQOqLdhspUjvEhwyhG = -1;
							iIaPMAOogBhZEjoWTHzMJdhjuaKg = uyWRAXuhPoAJBIAhLgVprYeoUZwG.UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
							fcQiCoGsOwdOCZqKZBXNciogYAiQb = iIaPMAOogBhZEjoWTHzMJdhjuaKg.Count;
							IScuqnJmJWhjJmQtUelOkGqbztGsA = 0;
							goto IL_00f1;
							IL_00c5:
							if (vaQJyqVNpIFkENLLvpulRPdCeKYl.MoveNext())
							{
								ControllerPollingInfo current = vaQJyqVNpIFkENLLvpulRPdCeKYl.Current;
								ControllerPollingInfo zTDDPtxfsetihWmAdKRQAALRLPqF = new ControllerPollingInfo(current);
								zTDDPtxfsetihWmAdKRQAALRLPqF.playerId = uyWRAXuhPoAJBIAhLgVprYeoUZwG.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								ZTDDPtxfsetihWmAdKRQAALRLPqF = zTDDPtxfsetihWmAdKRQAALRLPqF;
								iZutCmuxcvWQOqLdhspUjvEhwyhG = 1;
								return true;
							}
							yWzkynoCTADwmDfCicBVJMnKfGRB();
							vaQJyqVNpIFkENLLvpulRPdCeKYl = null;
							IScuqnJmJWhjJmQtUelOkGqbztGsA++;
							goto IL_00f1;
							IL_00f1:
							if (IScuqnJmJWhjJmQtUelOkGqbztGsA < fcQiCoGsOwdOCZqKZBXNciogYAiQb)
							{
								vaQJyqVNpIFkENLLvpulRPdCeKYl = iIaPMAOogBhZEjoWTHzMJdhjuaKg[IScuqnJmJWhjJmQtUelOkGqbztGsA].PollForAllButtonsDown().GetEnumerator();
								iZutCmuxcvWQOqLdhspUjvEhwyhG = -3;
								goto IL_00c5;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void yWzkynoCTADwmDfCicBVJMnKfGRB()
					{
						iZutCmuxcvWQOqLdhspUjvEhwyhG = -1;
						if (vaQJyqVNpIFkENLLvpulRPdCeKYl != null)
						{
							vaQJyqVNpIFkENLLvpulRPdCeKYl.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						ArLzvAnOXbMsoVjRGdCfEajCJYLp arLzvAnOXbMsoVjRGdCfEajCJYLp;
						if (iZutCmuxcvWQOqLdhspUjvEhwyhG == -2 && GOcvfcqIeClNnyhLbTryDNWkgkwIA == Environment.CurrentManagedThreadId)
						{
							iZutCmuxcvWQOqLdhspUjvEhwyhG = 0;
							arLzvAnOXbMsoVjRGdCfEajCJYLp = this;
						}
						else
						{
							arLzvAnOXbMsoVjRGdCfEajCJYLp = new ArLzvAnOXbMsoVjRGdCfEajCJYLp(0);
							arLzvAnOXbMsoVjRGdCfEajCJYLp.UyWRAXuhPoAJBIAhLgVprYeoUZwG = UyWRAXuhPoAJBIAhLgVprYeoUZwG;
						}
						return arLzvAnOXbMsoVjRGdCfEajCJYLp;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class yrKMDKHUtkemgcFUipRjnmLxjTiz : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int hgiRNiSsSEpLlscHBJCiiaykqHydA;

					private ControllerPollingInfo gojWmkmPoaXJxZMZOrWJYohrddbR;

					private int lRqcHfbridiJBoLwNCurVpadMwcG;

					public PollingHelper wpeozKJkQFSCNYqmWpWKPJFraTCl;

					private IList<Joystick> xuWApFNHkzJRndApZWAXlQWkGdmW;

					private int VNXfQiWocWMWWEHsOgtEqrZhdNVs;

					private int WfcKQFyvkMBOnhLRvuskFEJGkkGR;

					private IEnumerator<ControllerPollingInfo> SbmdbaDncqJKEpcrVrIbmJnhYAQCA;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return gojWmkmPoaXJxZMZOrWJYohrddbR;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return gojWmkmPoaXJxZMZOrWJYohrddbR;
						}
					}

					[DebuggerHidden]
					public yrKMDKHUtkemgcFUipRjnmLxjTiz(int P_0)
					{
						hgiRNiSsSEpLlscHBJCiiaykqHydA = P_0;
						lRqcHfbridiJBoLwNCurVpadMwcG = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = hgiRNiSsSEpLlscHBJCiiaykqHydA;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								nuQjffQNWPCzsEpJbAVJEBAHoHNsb();
							}
						}
						xuWApFNHkzJRndApZWAXlQWkGdmW = null;
						SbmdbaDncqJKEpcrVrIbmJnhYAQCA = null;
						hgiRNiSsSEpLlscHBJCiiaykqHydA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = hgiRNiSsSEpLlscHBJCiiaykqHydA;
							PollingHelper pollingHelper = wpeozKJkQFSCNYqmWpWKPJFraTCl;
							if (num != 0)
							{
								if (num != 1)
								{
									return false;
								}
								hgiRNiSsSEpLlscHBJCiiaykqHydA = -3;
								goto IL_00c5;
							}
							hgiRNiSsSEpLlscHBJCiiaykqHydA = -1;
							xuWApFNHkzJRndApZWAXlQWkGdmW = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
							VNXfQiWocWMWWEHsOgtEqrZhdNVs = xuWApFNHkzJRndApZWAXlQWkGdmW.Count;
							WfcKQFyvkMBOnhLRvuskFEJGkkGR = 0;
							goto IL_00f1;
							IL_00c5:
							if (SbmdbaDncqJKEpcrVrIbmJnhYAQCA.MoveNext())
							{
								ControllerPollingInfo current = SbmdbaDncqJKEpcrVrIbmJnhYAQCA.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								gojWmkmPoaXJxZMZOrWJYohrddbR = controllerPollingInfo;
								hgiRNiSsSEpLlscHBJCiiaykqHydA = 1;
								return true;
							}
							nuQjffQNWPCzsEpJbAVJEBAHoHNsb();
							SbmdbaDncqJKEpcrVrIbmJnhYAQCA = null;
							WfcKQFyvkMBOnhLRvuskFEJGkkGR++;
							goto IL_00f1;
							IL_00f1:
							if (WfcKQFyvkMBOnhLRvuskFEJGkkGR < VNXfQiWocWMWWEHsOgtEqrZhdNVs)
							{
								SbmdbaDncqJKEpcrVrIbmJnhYAQCA = xuWApFNHkzJRndApZWAXlQWkGdmW[WfcKQFyvkMBOnhLRvuskFEJGkkGR].PollForAllElements().GetEnumerator();
								hgiRNiSsSEpLlscHBJCiiaykqHydA = -3;
								goto IL_00c5;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void nuQjffQNWPCzsEpJbAVJEBAHoHNsb()
					{
						hgiRNiSsSEpLlscHBJCiiaykqHydA = -1;
						if (SbmdbaDncqJKEpcrVrIbmJnhYAQCA != null)
						{
							SbmdbaDncqJKEpcrVrIbmJnhYAQCA.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						yrKMDKHUtkemgcFUipRjnmLxjTiz yrKMDKHUtkemgcFUipRjnmLxjTiz2;
						if (hgiRNiSsSEpLlscHBJCiiaykqHydA == -2 && lRqcHfbridiJBoLwNCurVpadMwcG == Environment.CurrentManagedThreadId)
						{
							hgiRNiSsSEpLlscHBJCiiaykqHydA = 0;
							yrKMDKHUtkemgcFUipRjnmLxjTiz2 = this;
						}
						else
						{
							yrKMDKHUtkemgcFUipRjnmLxjTiz2 = new yrKMDKHUtkemgcFUipRjnmLxjTiz(0);
							yrKMDKHUtkemgcFUipRjnmLxjTiz2.wpeozKJkQFSCNYqmWpWKPJFraTCl = wpeozKJkQFSCNYqmWpWKPJFraTCl;
						}
						return yrKMDKHUtkemgcFUipRjnmLxjTiz2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class AAyLCgOkORcfxPNTXEnqvhUIGegw : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int RgcdvHUmqcNPYbAKNfgiibtVALEpA;

					private ControllerPollingInfo zWXOWWLeYmAgBZXoPcdyxAnbiCMhA;

					private int aaXlLxBSfdMgIYRcbsMPbUhhwMsq;

					public PollingHelper KSQGPhWpXKBqHAalOXNFrvQYjdXjA;

					private IList<Joystick> YrDkaBhzdrNKEXauIeCUkuLJxQuTA;

					private int pAUKmnwgbgyqEsMuNHhqJPlrJQGd;

					private int cydXXnyoeMtZiLDWRUOaeSktSIwg;

					private IEnumerator<ControllerPollingInfo> PJCUQeZgnxrckmcrclApNTCOEVPw;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return zWXOWWLeYmAgBZXoPcdyxAnbiCMhA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return zWXOWWLeYmAgBZXoPcdyxAnbiCMhA;
						}
					}

					[DebuggerHidden]
					public AAyLCgOkORcfxPNTXEnqvhUIGegw(int P_0)
					{
						RgcdvHUmqcNPYbAKNfgiibtVALEpA = P_0;
						aaXlLxBSfdMgIYRcbsMPbUhhwMsq = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int rgcdvHUmqcNPYbAKNfgiibtVALEpA = RgcdvHUmqcNPYbAKNfgiibtVALEpA;
						if (rgcdvHUmqcNPYbAKNfgiibtVALEpA == -3 || rgcdvHUmqcNPYbAKNfgiibtVALEpA == 1)
						{
							try
							{
							}
							finally
							{
								VUjEkWBImNMWTeJXbHWMGtEZTzpvB();
							}
						}
						YrDkaBhzdrNKEXauIeCUkuLJxQuTA = null;
						PJCUQeZgnxrckmcrclApNTCOEVPw = null;
						RgcdvHUmqcNPYbAKNfgiibtVALEpA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int rgcdvHUmqcNPYbAKNfgiibtVALEpA = RgcdvHUmqcNPYbAKNfgiibtVALEpA;
							PollingHelper kSQGPhWpXKBqHAalOXNFrvQYjdXjA = KSQGPhWpXKBqHAalOXNFrvQYjdXjA;
							if (rgcdvHUmqcNPYbAKNfgiibtVALEpA != 0)
							{
								if (rgcdvHUmqcNPYbAKNfgiibtVALEpA != 1)
								{
									return false;
								}
								RgcdvHUmqcNPYbAKNfgiibtVALEpA = -3;
								goto IL_00c5;
							}
							RgcdvHUmqcNPYbAKNfgiibtVALEpA = -1;
							YrDkaBhzdrNKEXauIeCUkuLJxQuTA = kSQGPhWpXKBqHAalOXNFrvQYjdXjA.UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
							pAUKmnwgbgyqEsMuNHhqJPlrJQGd = YrDkaBhzdrNKEXauIeCUkuLJxQuTA.Count;
							cydXXnyoeMtZiLDWRUOaeSktSIwg = 0;
							goto IL_00f1;
							IL_00c5:
							if (PJCUQeZgnxrckmcrclApNTCOEVPw.MoveNext())
							{
								ControllerPollingInfo current = PJCUQeZgnxrckmcrclApNTCOEVPw.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = kSQGPhWpXKBqHAalOXNFrvQYjdXjA.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								zWXOWWLeYmAgBZXoPcdyxAnbiCMhA = controllerPollingInfo;
								RgcdvHUmqcNPYbAKNfgiibtVALEpA = 1;
								return true;
							}
							VUjEkWBImNMWTeJXbHWMGtEZTzpvB();
							PJCUQeZgnxrckmcrclApNTCOEVPw = null;
							cydXXnyoeMtZiLDWRUOaeSktSIwg++;
							goto IL_00f1;
							IL_00f1:
							if (cydXXnyoeMtZiLDWRUOaeSktSIwg < pAUKmnwgbgyqEsMuNHhqJPlrJQGd)
							{
								PJCUQeZgnxrckmcrclApNTCOEVPw = YrDkaBhzdrNKEXauIeCUkuLJxQuTA[cydXXnyoeMtZiLDWRUOaeSktSIwg].PollForAllElementsDown().GetEnumerator();
								RgcdvHUmqcNPYbAKNfgiibtVALEpA = -3;
								goto IL_00c5;
							}
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void VUjEkWBImNMWTeJXbHWMGtEZTzpvB()
					{
						RgcdvHUmqcNPYbAKNfgiibtVALEpA = -1;
						if (PJCUQeZgnxrckmcrclApNTCOEVPw != null)
						{
							PJCUQeZgnxrckmcrclApNTCOEVPw.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						AAyLCgOkORcfxPNTXEnqvhUIGegw aAyLCgOkORcfxPNTXEnqvhUIGegw;
						if (RgcdvHUmqcNPYbAKNfgiibtVALEpA == -2 && aaXlLxBSfdMgIYRcbsMPbUhhwMsq == Environment.CurrentManagedThreadId)
						{
							RgcdvHUmqcNPYbAKNfgiibtVALEpA = 0;
							aAyLCgOkORcfxPNTXEnqvhUIGegw = this;
						}
						else
						{
							aAyLCgOkORcfxPNTXEnqvhUIGegw = new AAyLCgOkORcfxPNTXEnqvhUIGegw(0);
							aAyLCgOkORcfxPNTXEnqvhUIGegw.KSQGPhWpXKBqHAalOXNFrvQYjdXjA = KSQGPhWpXKBqHAalOXNFrvQYjdXjA;
						}
						return aAyLCgOkORcfxPNTXEnqvhUIGegw;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class hlssEsOtapQNYkTovDnbubyuAEon : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int HNlMvXzCpeCLDgsUBqlEasOsGQoDA;

					private ControllerPollingInfo yFpHknKlFQjXvwlwdpbzDpvxtKXbb;

					private int kOQUlUMtPPaecMbREaPcodiyTEwR;

					private int ROHfdpfFdtJpMvxaCCMPUOXakePI;

					public int DFhwikCXHDQlliPyRcbUIBvJnhrn;

					public PollingHelper XXZBSOUSTboxhPYhDWWRiQeQBXyf;

					private IEnumerator<ControllerPollingInfo> rZTUirBitWAMwVtufdsojNFhBLMm;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return yFpHknKlFQjXvwlwdpbzDpvxtKXbb;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return yFpHknKlFQjXvwlwdpbzDpvxtKXbb;
						}
					}

					[DebuggerHidden]
					public hlssEsOtapQNYkTovDnbubyuAEon(int P_0)
					{
						HNlMvXzCpeCLDgsUBqlEasOsGQoDA = P_0;
						kOQUlUMtPPaecMbREaPcodiyTEwR = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int hNlMvXzCpeCLDgsUBqlEasOsGQoDA = HNlMvXzCpeCLDgsUBqlEasOsGQoDA;
						if (hNlMvXzCpeCLDgsUBqlEasOsGQoDA == -3 || hNlMvXzCpeCLDgsUBqlEasOsGQoDA == 1)
						{
							try
							{
							}
							finally
							{
								XoxQTlhFQrJzmXBzFVSBdtEeHnNX();
							}
						}
						rZTUirBitWAMwVtufdsojNFhBLMm = null;
						HNlMvXzCpeCLDgsUBqlEasOsGQoDA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int hNlMvXzCpeCLDgsUBqlEasOsGQoDA = HNlMvXzCpeCLDgsUBqlEasOsGQoDA;
							PollingHelper xXZBSOUSTboxhPYhDWWRiQeQBXyf = XXZBSOUSTboxhPYhDWWRiQeQBXyf;
							switch (hNlMvXzCpeCLDgsUBqlEasOsGQoDA)
							{
							default:
								return false;
							case 0:
							{
								HNlMvXzCpeCLDgsUBqlEasOsGQoDA = -1;
								if (ROHfdpfFdtJpMvxaCCMPUOXakePI < 0)
								{
									return false;
								}
								CustomController customController = xXZBSOUSTboxhPYhDWWRiQeQBXyf.UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.mwluYWpfujxinFejkWlvebBLHySV(ROHfdpfFdtJpMvxaCCMPUOXakePI);
								if (customController == null)
								{
									return false;
								}
								rZTUirBitWAMwVtufdsojNFhBLMm = customController.PollForAllAxes().GetEnumerator();
								HNlMvXzCpeCLDgsUBqlEasOsGQoDA = -3;
								break;
							}
							case 1:
								HNlMvXzCpeCLDgsUBqlEasOsGQoDA = -3;
								break;
							}
							if (rZTUirBitWAMwVtufdsojNFhBLMm.MoveNext())
							{
								ControllerPollingInfo current = rZTUirBitWAMwVtufdsojNFhBLMm.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = xXZBSOUSTboxhPYhDWWRiQeQBXyf.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								yFpHknKlFQjXvwlwdpbzDpvxtKXbb = controllerPollingInfo;
								HNlMvXzCpeCLDgsUBqlEasOsGQoDA = 1;
								return true;
							}
							XoxQTlhFQrJzmXBzFVSBdtEeHnNX();
							rZTUirBitWAMwVtufdsojNFhBLMm = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void XoxQTlhFQrJzmXBzFVSBdtEeHnNX()
					{
						HNlMvXzCpeCLDgsUBqlEasOsGQoDA = -1;
						if (rZTUirBitWAMwVtufdsojNFhBLMm != null)
						{
							rZTUirBitWAMwVtufdsojNFhBLMm.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						hlssEsOtapQNYkTovDnbubyuAEon hlssEsOtapQNYkTovDnbubyuAEon2;
						if (HNlMvXzCpeCLDgsUBqlEasOsGQoDA == -2 && kOQUlUMtPPaecMbREaPcodiyTEwR == Environment.CurrentManagedThreadId)
						{
							HNlMvXzCpeCLDgsUBqlEasOsGQoDA = 0;
							hlssEsOtapQNYkTovDnbubyuAEon2 = this;
						}
						else
						{
							hlssEsOtapQNYkTovDnbubyuAEon2 = new hlssEsOtapQNYkTovDnbubyuAEon(0);
							hlssEsOtapQNYkTovDnbubyuAEon2.XXZBSOUSTboxhPYhDWWRiQeQBXyf = XXZBSOUSTboxhPYhDWWRiQeQBXyf;
						}
						hlssEsOtapQNYkTovDnbubyuAEon2.ROHfdpfFdtJpMvxaCCMPUOXakePI = DFhwikCXHDQlliPyRcbUIBvJnhrn;
						return hlssEsOtapQNYkTovDnbubyuAEon2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class VRmWZjZQtAiwowbgUPMYExVhiney : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int DzoUVahaahmTvJPkcmIpCwbGwMYG;

					private ControllerPollingInfo cDAgfAWDhLdXOlXQkegMEYJxNkmAA;

					private int WFXlFmCsKxonVUoPjCeRafBCrtdWA;

					private int jgwTXBPfMsxNZBlKieOyfIHBVQmsA;

					public int hfHMZWGfiChXDGSaRbAurlWYnsdK;

					public PollingHelper vFGDbFAQFBXbmSSxbHldHdSEAxByA;

					private IEnumerator<ControllerPollingInfo> dceYvGZbVfPhycvouxTECcjtAApB;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return cDAgfAWDhLdXOlXQkegMEYJxNkmAA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return cDAgfAWDhLdXOlXQkegMEYJxNkmAA;
						}
					}

					[DebuggerHidden]
					public VRmWZjZQtAiwowbgUPMYExVhiney(int P_0)
					{
						DzoUVahaahmTvJPkcmIpCwbGwMYG = P_0;
						WFXlFmCsKxonVUoPjCeRafBCrtdWA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int dzoUVahaahmTvJPkcmIpCwbGwMYG = DzoUVahaahmTvJPkcmIpCwbGwMYG;
						if (dzoUVahaahmTvJPkcmIpCwbGwMYG == -3 || dzoUVahaahmTvJPkcmIpCwbGwMYG == 1)
						{
							try
							{
							}
							finally
							{
								UjLskOIIORpAVXTnYtObOAlsDBFbA();
							}
						}
						dceYvGZbVfPhycvouxTECcjtAApB = null;
						DzoUVahaahmTvJPkcmIpCwbGwMYG = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int dzoUVahaahmTvJPkcmIpCwbGwMYG = DzoUVahaahmTvJPkcmIpCwbGwMYG;
							PollingHelper pollingHelper = vFGDbFAQFBXbmSSxbHldHdSEAxByA;
							switch (dzoUVahaahmTvJPkcmIpCwbGwMYG)
							{
							default:
								return false;
							case 0:
							{
								DzoUVahaahmTvJPkcmIpCwbGwMYG = -1;
								if (jgwTXBPfMsxNZBlKieOyfIHBVQmsA < 0)
								{
									return false;
								}
								CustomController customController = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.mwluYWpfujxinFejkWlvebBLHySV(jgwTXBPfMsxNZBlKieOyfIHBVQmsA);
								if (customController == null)
								{
									return false;
								}
								dceYvGZbVfPhycvouxTECcjtAApB = customController.PollForAllButtons().GetEnumerator();
								DzoUVahaahmTvJPkcmIpCwbGwMYG = -3;
								break;
							}
							case 1:
								DzoUVahaahmTvJPkcmIpCwbGwMYG = -3;
								break;
							}
							if (dceYvGZbVfPhycvouxTECcjtAApB.MoveNext())
							{
								ControllerPollingInfo current = dceYvGZbVfPhycvouxTECcjtAApB.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								cDAgfAWDhLdXOlXQkegMEYJxNkmAA = controllerPollingInfo;
								DzoUVahaahmTvJPkcmIpCwbGwMYG = 1;
								return true;
							}
							UjLskOIIORpAVXTnYtObOAlsDBFbA();
							dceYvGZbVfPhycvouxTECcjtAApB = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void UjLskOIIORpAVXTnYtObOAlsDBFbA()
					{
						DzoUVahaahmTvJPkcmIpCwbGwMYG = -1;
						if (dceYvGZbVfPhycvouxTECcjtAApB != null)
						{
							dceYvGZbVfPhycvouxTECcjtAApB.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						VRmWZjZQtAiwowbgUPMYExVhiney vRmWZjZQtAiwowbgUPMYExVhiney;
						if (DzoUVahaahmTvJPkcmIpCwbGwMYG == -2 && WFXlFmCsKxonVUoPjCeRafBCrtdWA == Environment.CurrentManagedThreadId)
						{
							DzoUVahaahmTvJPkcmIpCwbGwMYG = 0;
							vRmWZjZQtAiwowbgUPMYExVhiney = this;
						}
						else
						{
							vRmWZjZQtAiwowbgUPMYExVhiney = new VRmWZjZQtAiwowbgUPMYExVhiney(0);
							vRmWZjZQtAiwowbgUPMYExVhiney.vFGDbFAQFBXbmSSxbHldHdSEAxByA = vFGDbFAQFBXbmSSxbHldHdSEAxByA;
						}
						vRmWZjZQtAiwowbgUPMYExVhiney.jgwTXBPfMsxNZBlKieOyfIHBVQmsA = hfHMZWGfiChXDGSaRbAurlWYnsdK;
						return vRmWZjZQtAiwowbgUPMYExVhiney;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class idLnPXiltXdnsMCDpeqkMwzmjEyn : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int UYTqgfsGBHouVOKJsdaZvDRiVNlF;

					private ControllerPollingInfo QljPvxYYkoraKcbnsssKjyLZhXvN;

					private int yNpdvBKmyjWoTMuFpHuwyuESvAdI;

					private int lRBEpCaqVFbtSHoexfsvBtNDbUxhb;

					public int iXkyUMIPysBbIiUOVjrHyPuqaude;

					public PollingHelper mHrRNNaglWHEZHSTdEohaJcJnhuyA;

					private IEnumerator<ControllerPollingInfo> TiXoRxVdBBTERqeKsUnwRmwgstuT;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return QljPvxYYkoraKcbnsssKjyLZhXvN;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return QljPvxYYkoraKcbnsssKjyLZhXvN;
						}
					}

					[DebuggerHidden]
					public idLnPXiltXdnsMCDpeqkMwzmjEyn(int P_0)
					{
						UYTqgfsGBHouVOKJsdaZvDRiVNlF = P_0;
						yNpdvBKmyjWoTMuFpHuwyuESvAdI = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int uYTqgfsGBHouVOKJsdaZvDRiVNlF = UYTqgfsGBHouVOKJsdaZvDRiVNlF;
						if (uYTqgfsGBHouVOKJsdaZvDRiVNlF == -3 || uYTqgfsGBHouVOKJsdaZvDRiVNlF == 1)
						{
							try
							{
							}
							finally
							{
								dTvqiwBMPDIJZqjWkyTUVgtMkZwb();
							}
						}
						TiXoRxVdBBTERqeKsUnwRmwgstuT = null;
						UYTqgfsGBHouVOKJsdaZvDRiVNlF = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int uYTqgfsGBHouVOKJsdaZvDRiVNlF = UYTqgfsGBHouVOKJsdaZvDRiVNlF;
							PollingHelper pollingHelper = mHrRNNaglWHEZHSTdEohaJcJnhuyA;
							switch (uYTqgfsGBHouVOKJsdaZvDRiVNlF)
							{
							default:
								return false;
							case 0:
							{
								UYTqgfsGBHouVOKJsdaZvDRiVNlF = -1;
								if (lRBEpCaqVFbtSHoexfsvBtNDbUxhb < 0)
								{
									return false;
								}
								CustomController customController = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.mwluYWpfujxinFejkWlvebBLHySV(lRBEpCaqVFbtSHoexfsvBtNDbUxhb);
								if (customController == null)
								{
									return false;
								}
								TiXoRxVdBBTERqeKsUnwRmwgstuT = customController.PollForAllButtonsDown().GetEnumerator();
								UYTqgfsGBHouVOKJsdaZvDRiVNlF = -3;
								break;
							}
							case 1:
								UYTqgfsGBHouVOKJsdaZvDRiVNlF = -3;
								break;
							}
							if (TiXoRxVdBBTERqeKsUnwRmwgstuT.MoveNext())
							{
								ControllerPollingInfo current = TiXoRxVdBBTERqeKsUnwRmwgstuT.Current;
								ControllerPollingInfo qljPvxYYkoraKcbnsssKjyLZhXvN = new ControllerPollingInfo(current);
								qljPvxYYkoraKcbnsssKjyLZhXvN.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								QljPvxYYkoraKcbnsssKjyLZhXvN = qljPvxYYkoraKcbnsssKjyLZhXvN;
								UYTqgfsGBHouVOKJsdaZvDRiVNlF = 1;
								return true;
							}
							dTvqiwBMPDIJZqjWkyTUVgtMkZwb();
							TiXoRxVdBBTERqeKsUnwRmwgstuT = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void dTvqiwBMPDIJZqjWkyTUVgtMkZwb()
					{
						UYTqgfsGBHouVOKJsdaZvDRiVNlF = -1;
						if (TiXoRxVdBBTERqeKsUnwRmwgstuT != null)
						{
							TiXoRxVdBBTERqeKsUnwRmwgstuT.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						idLnPXiltXdnsMCDpeqkMwzmjEyn idLnPXiltXdnsMCDpeqkMwzmjEyn2;
						if (UYTqgfsGBHouVOKJsdaZvDRiVNlF == -2 && yNpdvBKmyjWoTMuFpHuwyuESvAdI == Environment.CurrentManagedThreadId)
						{
							UYTqgfsGBHouVOKJsdaZvDRiVNlF = 0;
							idLnPXiltXdnsMCDpeqkMwzmjEyn2 = this;
						}
						else
						{
							idLnPXiltXdnsMCDpeqkMwzmjEyn2 = new idLnPXiltXdnsMCDpeqkMwzmjEyn(0);
							idLnPXiltXdnsMCDpeqkMwzmjEyn2.mHrRNNaglWHEZHSTdEohaJcJnhuyA = mHrRNNaglWHEZHSTdEohaJcJnhuyA;
						}
						idLnPXiltXdnsMCDpeqkMwzmjEyn2.lRBEpCaqVFbtSHoexfsvBtNDbUxhb = iXkyUMIPysBbIiUOVjrHyPuqaude;
						return idLnPXiltXdnsMCDpeqkMwzmjEyn2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class chKfOcEMyrvdxdGuRvbupfWzwkOcA : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int FPAYpxZkJGVnnqnXmMITOefwCdKGA;

					private ControllerPollingInfo lodVtjNxkkngELsstWzkcdXGbQrBA;

					private int JmJNpSgGjWsiSvscDqqZHBsWUMob;

					private int IgawUzdnyCDUFEODdlmOniZVAJhT;

					public int CXhMEbBkQZxQIxOIfucyCeLnjJVAA;

					public PollingHelper qImIjlJlBPFaUZtXsdateSKqUidX;

					private IEnumerator<ControllerPollingInfo> JDLGiCLzQthPcdpJpzrRwAdQjyTg;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return lodVtjNxkkngELsstWzkcdXGbQrBA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return lodVtjNxkkngELsstWzkcdXGbQrBA;
						}
					}

					[DebuggerHidden]
					public chKfOcEMyrvdxdGuRvbupfWzwkOcA(int P_0)
					{
						FPAYpxZkJGVnnqnXmMITOefwCdKGA = P_0;
						JmJNpSgGjWsiSvscDqqZHBsWUMob = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int fPAYpxZkJGVnnqnXmMITOefwCdKGA = FPAYpxZkJGVnnqnXmMITOefwCdKGA;
						if (fPAYpxZkJGVnnqnXmMITOefwCdKGA == -3 || fPAYpxZkJGVnnqnXmMITOefwCdKGA == 1)
						{
							try
							{
							}
							finally
							{
								COBpLJAZkavVjvrrmtkozVnAkoJf();
							}
						}
						JDLGiCLzQthPcdpJpzrRwAdQjyTg = null;
						FPAYpxZkJGVnnqnXmMITOefwCdKGA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int fPAYpxZkJGVnnqnXmMITOefwCdKGA = FPAYpxZkJGVnnqnXmMITOefwCdKGA;
							PollingHelper pollingHelper = qImIjlJlBPFaUZtXsdateSKqUidX;
							switch (fPAYpxZkJGVnnqnXmMITOefwCdKGA)
							{
							default:
								return false;
							case 0:
							{
								FPAYpxZkJGVnnqnXmMITOefwCdKGA = -1;
								if (IgawUzdnyCDUFEODdlmOniZVAJhT < 0)
								{
									return false;
								}
								CustomController customController = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.mwluYWpfujxinFejkWlvebBLHySV(IgawUzdnyCDUFEODdlmOniZVAJhT);
								if (customController == null)
								{
									return false;
								}
								JDLGiCLzQthPcdpJpzrRwAdQjyTg = customController.PollForAllElements().GetEnumerator();
								FPAYpxZkJGVnnqnXmMITOefwCdKGA = -3;
								break;
							}
							case 1:
								FPAYpxZkJGVnnqnXmMITOefwCdKGA = -3;
								break;
							}
							if (JDLGiCLzQthPcdpJpzrRwAdQjyTg.MoveNext())
							{
								ControllerPollingInfo current = JDLGiCLzQthPcdpJpzrRwAdQjyTg.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								lodVtjNxkkngELsstWzkcdXGbQrBA = controllerPollingInfo;
								FPAYpxZkJGVnnqnXmMITOefwCdKGA = 1;
								return true;
							}
							COBpLJAZkavVjvrrmtkozVnAkoJf();
							JDLGiCLzQthPcdpJpzrRwAdQjyTg = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void COBpLJAZkavVjvrrmtkozVnAkoJf()
					{
						FPAYpxZkJGVnnqnXmMITOefwCdKGA = -1;
						if (JDLGiCLzQthPcdpJpzrRwAdQjyTg != null)
						{
							JDLGiCLzQthPcdpJpzrRwAdQjyTg.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						chKfOcEMyrvdxdGuRvbupfWzwkOcA chKfOcEMyrvdxdGuRvbupfWzwkOcA2;
						if (FPAYpxZkJGVnnqnXmMITOefwCdKGA == -2 && JmJNpSgGjWsiSvscDqqZHBsWUMob == Environment.CurrentManagedThreadId)
						{
							FPAYpxZkJGVnnqnXmMITOefwCdKGA = 0;
							chKfOcEMyrvdxdGuRvbupfWzwkOcA2 = this;
						}
						else
						{
							chKfOcEMyrvdxdGuRvbupfWzwkOcA2 = new chKfOcEMyrvdxdGuRvbupfWzwkOcA(0);
							chKfOcEMyrvdxdGuRvbupfWzwkOcA2.qImIjlJlBPFaUZtXsdateSKqUidX = qImIjlJlBPFaUZtXsdateSKqUidX;
						}
						chKfOcEMyrvdxdGuRvbupfWzwkOcA2.IgawUzdnyCDUFEODdlmOniZVAJhT = CXhMEbBkQZxQIxOIfucyCeLnjJVAA;
						return chKfOcEMyrvdxdGuRvbupfWzwkOcA2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class biBUPueSYATWVFKExBnFJfJsfShK : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int dqwcVAwkMFiEJsgKyjPZbGpzpDSo;

					private ControllerPollingInfo ItvwcntFbFemzFYfXcWRcvMedwgxb;

					private int GqAzONINNCsalDhcudZYiKcCnppK;

					private int ZvMtLYrJZRdLOKYvxVzZtZxzJzod;

					public int tjFLaytIcENhPvGfzUqQHBQJQJKi;

					public PollingHelper yKmOVOgbTjtWsItOCRPdzuJWMmKD;

					private IEnumerator<ControllerPollingInfo> AcIZyYMpgmbhCJnOXMaoTzYJgiaw;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return ItvwcntFbFemzFYfXcWRcvMedwgxb;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return ItvwcntFbFemzFYfXcWRcvMedwgxb;
						}
					}

					[DebuggerHidden]
					public biBUPueSYATWVFKExBnFJfJsfShK(int P_0)
					{
						dqwcVAwkMFiEJsgKyjPZbGpzpDSo = P_0;
						GqAzONINNCsalDhcudZYiKcCnppK = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = dqwcVAwkMFiEJsgKyjPZbGpzpDSo;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								iFokZHwJPlMKcWgIHfanLugoZLAG();
							}
						}
						AcIZyYMpgmbhCJnOXMaoTzYJgiaw = null;
						dqwcVAwkMFiEJsgKyjPZbGpzpDSo = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = dqwcVAwkMFiEJsgKyjPZbGpzpDSo;
							PollingHelper pollingHelper = yKmOVOgbTjtWsItOCRPdzuJWMmKD;
							switch (num)
							{
							default:
								return false;
							case 0:
							{
								dqwcVAwkMFiEJsgKyjPZbGpzpDSo = -1;
								if (ZvMtLYrJZRdLOKYvxVzZtZxzJzod < 0)
								{
									return false;
								}
								CustomController customController = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.mwluYWpfujxinFejkWlvebBLHySV(ZvMtLYrJZRdLOKYvxVzZtZxzJzod);
								if (customController == null)
								{
									return false;
								}
								AcIZyYMpgmbhCJnOXMaoTzYJgiaw = customController.PollForAllElementsDown().GetEnumerator();
								dqwcVAwkMFiEJsgKyjPZbGpzpDSo = -3;
								break;
							}
							case 1:
								dqwcVAwkMFiEJsgKyjPZbGpzpDSo = -3;
								break;
							}
							if (AcIZyYMpgmbhCJnOXMaoTzYJgiaw.MoveNext())
							{
								ControllerPollingInfo current = AcIZyYMpgmbhCJnOXMaoTzYJgiaw.Current;
								ControllerPollingInfo itvwcntFbFemzFYfXcWRcvMedwgxb = new ControllerPollingInfo(current);
								itvwcntFbFemzFYfXcWRcvMedwgxb.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								ItvwcntFbFemzFYfXcWRcvMedwgxb = itvwcntFbFemzFYfXcWRcvMedwgxb;
								dqwcVAwkMFiEJsgKyjPZbGpzpDSo = 1;
								return true;
							}
							iFokZHwJPlMKcWgIHfanLugoZLAG();
							AcIZyYMpgmbhCJnOXMaoTzYJgiaw = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void iFokZHwJPlMKcWgIHfanLugoZLAG()
					{
						dqwcVAwkMFiEJsgKyjPZbGpzpDSo = -1;
						if (AcIZyYMpgmbhCJnOXMaoTzYJgiaw != null)
						{
							AcIZyYMpgmbhCJnOXMaoTzYJgiaw.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						biBUPueSYATWVFKExBnFJfJsfShK biBUPueSYATWVFKExBnFJfJsfShK2;
						if (dqwcVAwkMFiEJsgKyjPZbGpzpDSo == -2 && GqAzONINNCsalDhcudZYiKcCnppK == Environment.CurrentManagedThreadId)
						{
							dqwcVAwkMFiEJsgKyjPZbGpzpDSo = 0;
							biBUPueSYATWVFKExBnFJfJsfShK2 = this;
						}
						else
						{
							biBUPueSYATWVFKExBnFJfJsfShK2 = new biBUPueSYATWVFKExBnFJfJsfShK(0);
							biBUPueSYATWVFKExBnFJfJsfShK2.yKmOVOgbTjtWsItOCRPdzuJWMmKD = yKmOVOgbTjtWsItOCRPdzuJWMmKD;
						}
						biBUPueSYATWVFKExBnFJfJsfShK2.ZvMtLYrJZRdLOKYvxVzZtZxzJzod = tjFLaytIcENhPvGfzUqQHBQJQJKi;
						return biBUPueSYATWVFKExBnFJfJsfShK2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class JIcKVkZMRYtdSzcruKtDnwbbmWMP : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int XvWesIoqFSbIikiYOImWKlsUPuhfA;

					private ControllerPollingInfo VyHEXHCMTNmbvTCnRPfzAHjsnKxrA;

					private int SQlyAlUCYLIFfgBHpAGZDCwdTXHiA;

					private int sRbAqOpLplNGUoaySrllXPBopgFt;

					public int rrEiFKftpjeHUbWvQVzBkORAwnFH;

					public PollingHelper obtnImNfCVbDGYqBkgSWVGqdNNtQ;

					private IEnumerator<ControllerPollingInfo> EKyKLiseXSGjlxyHkadwWSwoOIIu;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return VyHEXHCMTNmbvTCnRPfzAHjsnKxrA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return VyHEXHCMTNmbvTCnRPfzAHjsnKxrA;
						}
					}

					[DebuggerHidden]
					public JIcKVkZMRYtdSzcruKtDnwbbmWMP(int P_0)
					{
						XvWesIoqFSbIikiYOImWKlsUPuhfA = P_0;
						SQlyAlUCYLIFfgBHpAGZDCwdTXHiA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int xvWesIoqFSbIikiYOImWKlsUPuhfA = XvWesIoqFSbIikiYOImWKlsUPuhfA;
						if (xvWesIoqFSbIikiYOImWKlsUPuhfA == -3 || xvWesIoqFSbIikiYOImWKlsUPuhfA == 1)
						{
							try
							{
							}
							finally
							{
								nVOpClCIEMMizhnpTyVOqRBYdIHK();
							}
						}
						EKyKLiseXSGjlxyHkadwWSwoOIIu = null;
						XvWesIoqFSbIikiYOImWKlsUPuhfA = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int xvWesIoqFSbIikiYOImWKlsUPuhfA = XvWesIoqFSbIikiYOImWKlsUPuhfA;
							PollingHelper pollingHelper = obtnImNfCVbDGYqBkgSWVGqdNNtQ;
							switch (xvWesIoqFSbIikiYOImWKlsUPuhfA)
							{
							default:
								return false;
							case 0:
							{
								XvWesIoqFSbIikiYOImWKlsUPuhfA = -1;
								if (sRbAqOpLplNGUoaySrllXPBopgFt < 0)
								{
									return false;
								}
								Joystick joystick = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.mwluYWpfujxinFejkWlvebBLHySV(sRbAqOpLplNGUoaySrllXPBopgFt);
								if (joystick == null)
								{
									return false;
								}
								EKyKLiseXSGjlxyHkadwWSwoOIIu = joystick.PollForAllAxes().GetEnumerator();
								XvWesIoqFSbIikiYOImWKlsUPuhfA = -3;
								break;
							}
							case 1:
								XvWesIoqFSbIikiYOImWKlsUPuhfA = -3;
								break;
							}
							if (EKyKLiseXSGjlxyHkadwWSwoOIIu.MoveNext())
							{
								ControllerPollingInfo current = EKyKLiseXSGjlxyHkadwWSwoOIIu.Current;
								ControllerPollingInfo vyHEXHCMTNmbvTCnRPfzAHjsnKxrA = new ControllerPollingInfo(current);
								vyHEXHCMTNmbvTCnRPfzAHjsnKxrA.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								VyHEXHCMTNmbvTCnRPfzAHjsnKxrA = vyHEXHCMTNmbvTCnRPfzAHjsnKxrA;
								XvWesIoqFSbIikiYOImWKlsUPuhfA = 1;
								return true;
							}
							nVOpClCIEMMizhnpTyVOqRBYdIHK();
							EKyKLiseXSGjlxyHkadwWSwoOIIu = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void nVOpClCIEMMizhnpTyVOqRBYdIHK()
					{
						XvWesIoqFSbIikiYOImWKlsUPuhfA = -1;
						if (EKyKLiseXSGjlxyHkadwWSwoOIIu != null)
						{
							EKyKLiseXSGjlxyHkadwWSwoOIIu.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						JIcKVkZMRYtdSzcruKtDnwbbmWMP jIcKVkZMRYtdSzcruKtDnwbbmWMP;
						if (XvWesIoqFSbIikiYOImWKlsUPuhfA == -2 && SQlyAlUCYLIFfgBHpAGZDCwdTXHiA == Environment.CurrentManagedThreadId)
						{
							XvWesIoqFSbIikiYOImWKlsUPuhfA = 0;
							jIcKVkZMRYtdSzcruKtDnwbbmWMP = this;
						}
						else
						{
							jIcKVkZMRYtdSzcruKtDnwbbmWMP = new JIcKVkZMRYtdSzcruKtDnwbbmWMP(0);
							jIcKVkZMRYtdSzcruKtDnwbbmWMP.obtnImNfCVbDGYqBkgSWVGqdNNtQ = obtnImNfCVbDGYqBkgSWVGqdNNtQ;
						}
						jIcKVkZMRYtdSzcruKtDnwbbmWMP.sRbAqOpLplNGUoaySrllXPBopgFt = rrEiFKftpjeHUbWvQVzBkORAwnFH;
						return jIcKVkZMRYtdSzcruKtDnwbbmWMP;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class OkqNUkhZGRAqVmEYpIUTYTwuPKBP : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int LudmazKqRtNUIZiSnSdkMlPdbGRD;

					private ControllerPollingInfo OilEBduuOAIEeBkoCffyOxAORbNGA;

					private int MSFQGAIUnHhgAQuhUatJKwbomEcc;

					private int rFuXKGYZzqoMEwZlzdcdPNfnRxZN;

					public int txaRKpPPXnlblxWKuxkdCgkCWQTB;

					public PollingHelper TeUryaLixnvhLpOrBAXzKlATzxwh;

					private IEnumerator<ControllerPollingInfo> IzjVgbYsMxhEyXPzdkleKVYwbbXk;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return OilEBduuOAIEeBkoCffyOxAORbNGA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return OilEBduuOAIEeBkoCffyOxAORbNGA;
						}
					}

					[DebuggerHidden]
					public OkqNUkhZGRAqVmEYpIUTYTwuPKBP(int P_0)
					{
						LudmazKqRtNUIZiSnSdkMlPdbGRD = P_0;
						MSFQGAIUnHhgAQuhUatJKwbomEcc = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int ludmazKqRtNUIZiSnSdkMlPdbGRD = LudmazKqRtNUIZiSnSdkMlPdbGRD;
						if (ludmazKqRtNUIZiSnSdkMlPdbGRD == -3 || ludmazKqRtNUIZiSnSdkMlPdbGRD == 1)
						{
							try
							{
							}
							finally
							{
								CzyNMmJjYjipOhDNbyJkTrwXOcvO();
							}
						}
						IzjVgbYsMxhEyXPzdkleKVYwbbXk = null;
						LudmazKqRtNUIZiSnSdkMlPdbGRD = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int ludmazKqRtNUIZiSnSdkMlPdbGRD = LudmazKqRtNUIZiSnSdkMlPdbGRD;
							PollingHelper teUryaLixnvhLpOrBAXzKlATzxwh = TeUryaLixnvhLpOrBAXzKlATzxwh;
							switch (ludmazKqRtNUIZiSnSdkMlPdbGRD)
							{
							default:
								return false;
							case 0:
							{
								LudmazKqRtNUIZiSnSdkMlPdbGRD = -1;
								if (rFuXKGYZzqoMEwZlzdcdPNfnRxZN < 0)
								{
									return false;
								}
								Joystick joystick = teUryaLixnvhLpOrBAXzKlATzxwh.UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.mwluYWpfujxinFejkWlvebBLHySV(rFuXKGYZzqoMEwZlzdcdPNfnRxZN);
								if (joystick == null)
								{
									return false;
								}
								IzjVgbYsMxhEyXPzdkleKVYwbbXk = joystick.PollForAllButtons().GetEnumerator();
								LudmazKqRtNUIZiSnSdkMlPdbGRD = -3;
								break;
							}
							case 1:
								LudmazKqRtNUIZiSnSdkMlPdbGRD = -3;
								break;
							}
							if (IzjVgbYsMxhEyXPzdkleKVYwbbXk.MoveNext())
							{
								ControllerPollingInfo current = IzjVgbYsMxhEyXPzdkleKVYwbbXk.Current;
								ControllerPollingInfo oilEBduuOAIEeBkoCffyOxAORbNGA = new ControllerPollingInfo(current);
								oilEBduuOAIEeBkoCffyOxAORbNGA.playerId = teUryaLixnvhLpOrBAXzKlATzxwh.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								OilEBduuOAIEeBkoCffyOxAORbNGA = oilEBduuOAIEeBkoCffyOxAORbNGA;
								LudmazKqRtNUIZiSnSdkMlPdbGRD = 1;
								return true;
							}
							CzyNMmJjYjipOhDNbyJkTrwXOcvO();
							IzjVgbYsMxhEyXPzdkleKVYwbbXk = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void CzyNMmJjYjipOhDNbyJkTrwXOcvO()
					{
						LudmazKqRtNUIZiSnSdkMlPdbGRD = -1;
						if (IzjVgbYsMxhEyXPzdkleKVYwbbXk != null)
						{
							IzjVgbYsMxhEyXPzdkleKVYwbbXk.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						OkqNUkhZGRAqVmEYpIUTYTwuPKBP okqNUkhZGRAqVmEYpIUTYTwuPKBP;
						if (LudmazKqRtNUIZiSnSdkMlPdbGRD == -2 && MSFQGAIUnHhgAQuhUatJKwbomEcc == Environment.CurrentManagedThreadId)
						{
							LudmazKqRtNUIZiSnSdkMlPdbGRD = 0;
							okqNUkhZGRAqVmEYpIUTYTwuPKBP = this;
						}
						else
						{
							okqNUkhZGRAqVmEYpIUTYTwuPKBP = new OkqNUkhZGRAqVmEYpIUTYTwuPKBP(0);
							okqNUkhZGRAqVmEYpIUTYTwuPKBP.TeUryaLixnvhLpOrBAXzKlATzxwh = TeUryaLixnvhLpOrBAXzKlATzxwh;
						}
						okqNUkhZGRAqVmEYpIUTYTwuPKBP.rFuXKGYZzqoMEwZlzdcdPNfnRxZN = txaRKpPPXnlblxWKuxkdCgkCWQTB;
						return okqNUkhZGRAqVmEYpIUTYTwuPKBP;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class hzKZjuRHPmbQzHelfwzXkxLayIGb : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int eRVvmnqCsqhMBswgyyrYJZKicAag;

					private ControllerPollingInfo ILnNglEAyXKewRhecPvAquXZROgU;

					private int jnAKqIMooiiCAmWcyHAkqaIJNBko;

					private int jNHIuRxiqXHZwuzXHesZMGoyrLwG;

					public int RZYxMnmiGZBYDRxHtBuOqJpahqSAA;

					public PollingHelper ZwrHEHkELhRljVGVyybRfIzNfkgGA;

					private IEnumerator<ControllerPollingInfo> PjIGLlKvZgzLWbMlHNSjCeGKsZXs;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return ILnNglEAyXKewRhecPvAquXZROgU;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return ILnNglEAyXKewRhecPvAquXZROgU;
						}
					}

					[DebuggerHidden]
					public hzKZjuRHPmbQzHelfwzXkxLayIGb(int P_0)
					{
						eRVvmnqCsqhMBswgyyrYJZKicAag = P_0;
						jnAKqIMooiiCAmWcyHAkqaIJNBko = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = eRVvmnqCsqhMBswgyyrYJZKicAag;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								GUBfoJwPdJcPXqBpsYceNwpTdBQp();
							}
						}
						PjIGLlKvZgzLWbMlHNSjCeGKsZXs = null;
						eRVvmnqCsqhMBswgyyrYJZKicAag = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = eRVvmnqCsqhMBswgyyrYJZKicAag;
							PollingHelper zwrHEHkELhRljVGVyybRfIzNfkgGA = ZwrHEHkELhRljVGVyybRfIzNfkgGA;
							switch (num)
							{
							default:
								return false;
							case 0:
							{
								eRVvmnqCsqhMBswgyyrYJZKicAag = -1;
								if (jNHIuRxiqXHZwuzXHesZMGoyrLwG < 0)
								{
									return false;
								}
								Joystick joystick = zwrHEHkELhRljVGVyybRfIzNfkgGA.UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.mwluYWpfujxinFejkWlvebBLHySV(jNHIuRxiqXHZwuzXHesZMGoyrLwG);
								if (joystick == null)
								{
									return false;
								}
								PjIGLlKvZgzLWbMlHNSjCeGKsZXs = joystick.PollForAllButtonsDown().GetEnumerator();
								eRVvmnqCsqhMBswgyyrYJZKicAag = -3;
								break;
							}
							case 1:
								eRVvmnqCsqhMBswgyyrYJZKicAag = -3;
								break;
							}
							if (PjIGLlKvZgzLWbMlHNSjCeGKsZXs.MoveNext())
							{
								ControllerPollingInfo current = PjIGLlKvZgzLWbMlHNSjCeGKsZXs.Current;
								ControllerPollingInfo iLnNglEAyXKewRhecPvAquXZROgU = new ControllerPollingInfo(current);
								iLnNglEAyXKewRhecPvAquXZROgU.playerId = zwrHEHkELhRljVGVyybRfIzNfkgGA.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								ILnNglEAyXKewRhecPvAquXZROgU = iLnNglEAyXKewRhecPvAquXZROgU;
								eRVvmnqCsqhMBswgyyrYJZKicAag = 1;
								return true;
							}
							GUBfoJwPdJcPXqBpsYceNwpTdBQp();
							PjIGLlKvZgzLWbMlHNSjCeGKsZXs = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void GUBfoJwPdJcPXqBpsYceNwpTdBQp()
					{
						eRVvmnqCsqhMBswgyyrYJZKicAag = -1;
						if (PjIGLlKvZgzLWbMlHNSjCeGKsZXs != null)
						{
							PjIGLlKvZgzLWbMlHNSjCeGKsZXs.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						hzKZjuRHPmbQzHelfwzXkxLayIGb hzKZjuRHPmbQzHelfwzXkxLayIGb2;
						if (eRVvmnqCsqhMBswgyyrYJZKicAag == -2 && jnAKqIMooiiCAmWcyHAkqaIJNBko == Environment.CurrentManagedThreadId)
						{
							eRVvmnqCsqhMBswgyyrYJZKicAag = 0;
							hzKZjuRHPmbQzHelfwzXkxLayIGb2 = this;
						}
						else
						{
							hzKZjuRHPmbQzHelfwzXkxLayIGb2 = new hzKZjuRHPmbQzHelfwzXkxLayIGb(0);
							hzKZjuRHPmbQzHelfwzXkxLayIGb2.ZwrHEHkELhRljVGVyybRfIzNfkgGA = ZwrHEHkELhRljVGVyybRfIzNfkgGA;
						}
						hzKZjuRHPmbQzHelfwzXkxLayIGb2.jNHIuRxiqXHZwuzXHesZMGoyrLwG = RZYxMnmiGZBYDRxHtBuOqJpahqSAA;
						return hzKZjuRHPmbQzHelfwzXkxLayIGb2;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class DfAxFJWPyfreEqOSjRdaNkqoOPrR : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int oydDiDcgABJooQDuQGSoGQSinFAQB;

					private ControllerPollingInfo gIRgGIipkPYvHjWnZIpnEByTLLgzA;

					private int GsrqBZKdeCFJrAUIDfOmiSgpWrmrA;

					private int LvSPLDklOexnVvUjrcDecBJSGzWL;

					public int NuAceYoQzcwAAwGhFNyMPvPveezF;

					public PollingHelper earJQJIlhFHnqwPrLTJkexNmllPw;

					private IEnumerator<ControllerPollingInfo> wWNtTeOvKBGSZJiCQvXGiOiMCaCr;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return gIRgGIipkPYvHjWnZIpnEByTLLgzA;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return gIRgGIipkPYvHjWnZIpnEByTLLgzA;
						}
					}

					[DebuggerHidden]
					public DfAxFJWPyfreEqOSjRdaNkqoOPrR(int P_0)
					{
						oydDiDcgABJooQDuQGSoGQSinFAQB = P_0;
						GsrqBZKdeCFJrAUIDfOmiSgpWrmrA = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = oydDiDcgABJooQDuQGSoGQSinFAQB;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								fneLRcZGXhXVLRNzWXvDmvcXeZnb();
							}
						}
						wWNtTeOvKBGSZJiCQvXGiOiMCaCr = null;
						oydDiDcgABJooQDuQGSoGQSinFAQB = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = oydDiDcgABJooQDuQGSoGQSinFAQB;
							PollingHelper pollingHelper = earJQJIlhFHnqwPrLTJkexNmllPw;
							switch (num)
							{
							default:
								return false;
							case 0:
							{
								oydDiDcgABJooQDuQGSoGQSinFAQB = -1;
								if (LvSPLDklOexnVvUjrcDecBJSGzWL < 0)
								{
									return false;
								}
								Joystick joystick = pollingHelper.UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.mwluYWpfujxinFejkWlvebBLHySV(LvSPLDklOexnVvUjrcDecBJSGzWL);
								if (joystick == null)
								{
									return false;
								}
								wWNtTeOvKBGSZJiCQvXGiOiMCaCr = joystick.PollForAllElements().GetEnumerator();
								oydDiDcgABJooQDuQGSoGQSinFAQB = -3;
								break;
							}
							case 1:
								oydDiDcgABJooQDuQGSoGQSinFAQB = -3;
								break;
							}
							if (wWNtTeOvKBGSZJiCQvXGiOiMCaCr.MoveNext())
							{
								ControllerPollingInfo current = wWNtTeOvKBGSZJiCQvXGiOiMCaCr.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = pollingHelper.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								gIRgGIipkPYvHjWnZIpnEByTLLgzA = controllerPollingInfo;
								oydDiDcgABJooQDuQGSoGQSinFAQB = 1;
								return true;
							}
							fneLRcZGXhXVLRNzWXvDmvcXeZnb();
							wWNtTeOvKBGSZJiCQvXGiOiMCaCr = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void fneLRcZGXhXVLRNzWXvDmvcXeZnb()
					{
						oydDiDcgABJooQDuQGSoGQSinFAQB = -1;
						if (wWNtTeOvKBGSZJiCQvXGiOiMCaCr != null)
						{
							wWNtTeOvKBGSZJiCQvXGiOiMCaCr.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						DfAxFJWPyfreEqOSjRdaNkqoOPrR dfAxFJWPyfreEqOSjRdaNkqoOPrR;
						if (oydDiDcgABJooQDuQGSoGQSinFAQB == -2 && GsrqBZKdeCFJrAUIDfOmiSgpWrmrA == Environment.CurrentManagedThreadId)
						{
							oydDiDcgABJooQDuQGSoGQSinFAQB = 0;
							dfAxFJWPyfreEqOSjRdaNkqoOPrR = this;
						}
						else
						{
							dfAxFJWPyfreEqOSjRdaNkqoOPrR = new DfAxFJWPyfreEqOSjRdaNkqoOPrR(0);
							dfAxFJWPyfreEqOSjRdaNkqoOPrR.earJQJIlhFHnqwPrLTJkexNmllPw = earJQJIlhFHnqwPrLTJkexNmllPw;
						}
						dfAxFJWPyfreEqOSjRdaNkqoOPrR.LvSPLDklOexnVvUjrcDecBJSGzWL = NuAceYoQzcwAAwGhFNyMPvPveezF;
						return dfAxFJWPyfreEqOSjRdaNkqoOPrR;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private sealed class UAUbKEBpojXMDntarigocsPzzWDrA : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
				{
					private int dsptCDaHjZbltsvTXvhjDAMDsQwE;

					private ControllerPollingInfo tGYfovbcqZpYujkPxAkMAJVJhETX;

					private int dGmGxJQAyitzECqLaKFeRAdVvNxK;

					private int jsJqNKvCiejMoaBNvIjnmeZRGPqV;

					public int gCiokynAOMyoBdfQScPofXCgQItA;

					public PollingHelper KKoCGHSoeOnZdgnmttVSiDdtyCYn;

					private IEnumerator<ControllerPollingInfo> dRdTvSQpkngcFjPYoVMOuBuJOlehb;

					ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
					{
						[DebuggerHidden]
						get
						{
							return tGYfovbcqZpYujkPxAkMAJVJhETX;
						}
					}

					object IEnumerator.Current
					{
						[DebuggerHidden]
						get
						{
							return tGYfovbcqZpYujkPxAkMAJVJhETX;
						}
					}

					[DebuggerHidden]
					public UAUbKEBpojXMDntarigocsPzzWDrA(int P_0)
					{
						dsptCDaHjZbltsvTXvhjDAMDsQwE = P_0;
						dGmGxJQAyitzECqLaKFeRAdVvNxK = Environment.CurrentManagedThreadId;
					}

					[DebuggerHidden]
					void IDisposable.Dispose()
					{
						int num = dsptCDaHjZbltsvTXvhjDAMDsQwE;
						if (num == -3 || num == 1)
						{
							try
							{
							}
							finally
							{
								QjlUotsAjNbdKHumbEFcxFhVnBmSA();
							}
						}
						dRdTvSQpkngcFjPYoVMOuBuJOlehb = null;
						dsptCDaHjZbltsvTXvhjDAMDsQwE = -2;
					}

					private bool MoveNext()
					{
						try
						{
							int num = dsptCDaHjZbltsvTXvhjDAMDsQwE;
							PollingHelper kKoCGHSoeOnZdgnmttVSiDdtyCYn = KKoCGHSoeOnZdgnmttVSiDdtyCYn;
							switch (num)
							{
							default:
								return false;
							case 0:
							{
								dsptCDaHjZbltsvTXvhjDAMDsQwE = -1;
								if (jsJqNKvCiejMoaBNvIjnmeZRGPqV < 0)
								{
									return false;
								}
								Joystick joystick = kKoCGHSoeOnZdgnmttVSiDdtyCYn.UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.mwluYWpfujxinFejkWlvebBLHySV(jsJqNKvCiejMoaBNvIjnmeZRGPqV);
								if (joystick == null)
								{
									return false;
								}
								dRdTvSQpkngcFjPYoVMOuBuJOlehb = joystick.PollForAllElementsDown().GetEnumerator();
								dsptCDaHjZbltsvTXvhjDAMDsQwE = -3;
								break;
							}
							case 1:
								dsptCDaHjZbltsvTXvhjDAMDsQwE = -3;
								break;
							}
							if (dRdTvSQpkngcFjPYoVMOuBuJOlehb.MoveNext())
							{
								ControllerPollingInfo current = dRdTvSQpkngcFjPYoVMOuBuJOlehb.Current;
								ControllerPollingInfo controllerPollingInfo = new ControllerPollingInfo(current);
								controllerPollingInfo.playerId = kKoCGHSoeOnZdgnmttVSiDdtyCYn.GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
								tGYfovbcqZpYujkPxAkMAJVJhETX = controllerPollingInfo;
								dsptCDaHjZbltsvTXvhjDAMDsQwE = 1;
								return true;
							}
							QjlUotsAjNbdKHumbEFcxFhVnBmSA();
							dRdTvSQpkngcFjPYoVMOuBuJOlehb = null;
							return false;
						}
						catch
						{
							//try-fault
							((IDisposable)this).Dispose();
							throw;
						}
					}

					bool IEnumerator.MoveNext()
					{
						//ILSpy generated this explicit interface implementation from .override directive in MoveNext
						return this.MoveNext();
					}

					private void QjlUotsAjNbdKHumbEFcxFhVnBmSA()
					{
						dsptCDaHjZbltsvTXvhjDAMDsQwE = -1;
						if (dRdTvSQpkngcFjPYoVMOuBuJOlehb != null)
						{
							dRdTvSQpkngcFjPYoVMOuBuJOlehb.Dispose();
						}
					}

					[DebuggerHidden]
					void IEnumerator.Reset()
					{
						throw new NotSupportedException();
					}

					[DebuggerHidden]
					IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
					{
						UAUbKEBpojXMDntarigocsPzzWDrA uAUbKEBpojXMDntarigocsPzzWDrA;
						if (dsptCDaHjZbltsvTXvhjDAMDsQwE == -2 && dGmGxJQAyitzECqLaKFeRAdVvNxK == Environment.CurrentManagedThreadId)
						{
							dsptCDaHjZbltsvTXvhjDAMDsQwE = 0;
							uAUbKEBpojXMDntarigocsPzzWDrA = this;
						}
						else
						{
							uAUbKEBpojXMDntarigocsPzzWDrA = new UAUbKEBpojXMDntarigocsPzzWDrA(0);
							uAUbKEBpojXMDntarigocsPzzWDrA.KKoCGHSoeOnZdgnmttVSiDdtyCYn = KKoCGHSoeOnZdgnmttVSiDdtyCYn;
						}
						uAUbKEBpojXMDntarigocsPzzWDrA.jsJqNKvCiejMoaBNvIjnmeZRGPqV = gCiokynAOMyoBdfQScPofXCgQItA;
						return uAUbKEBpojXMDntarigocsPzzWDrA;
					}

					[DebuggerHidden]
					IEnumerator IEnumerable.GetEnumerator()
					{
						return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
					}
				}

				private readonly Player GALayqYyGJvICjxphFBNPTmSSaQL;

				private readonly ControllerHelper UiVtMiGqkCeljvdgeLyYPGbOFuWE;

				private readonly int HXDbdfBejnLtEUTWDlzRtNqHMysz;

				internal PollingHelper(Player P_0, ControllerHelper P_1)
				{
					HXDbdfBejnLtEUTWDlzRtNqHMysz = ReInput.id;
					GALayqYyGJvICjxphFBNPTmSSaQL = P_0;
					UiVtMiGqkCeljvdgeLyYPGbOFuWE = P_1;
				}

				public ControllerPollingInfo PollControllerForFirstElement(ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Keyboard => KwQUKtXoiLJKOoUMfbOpBuRGRuBQ(), 
						ControllerType.Joystick => pMurKafRskcAULBGgAejyCxhgcb(controllerId), 
						ControllerType.Mouse => eqORiwivwAlYyAgsNQfUEuBwmgPT(), 
						ControllerType.Custom => CJaCdzsfEfayiXDpQTFMevIefeZc(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstElementDown(ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Keyboard => crYmYMJpOdlTtRpQYUbIyvlByCXE(), 
						ControllerType.Joystick => adYLQCCRJUlpaPqwnwwKvlKYDwGf(controllerId), 
						ControllerType.Mouse => JfXxUUimIdjSBmUlVLaNgDZknpEM(), 
						ControllerType.Custom => AFjyVmsgXVzOTNehuXWOfxMEFebQ(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstButton(ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Keyboard => KwQUKtXoiLJKOoUMfbOpBuRGRuBQ(), 
						ControllerType.Joystick => spQfLELKjaHZkhJeqWZtrIWYRPYP(controllerId), 
						ControllerType.Mouse => XXLpaoeskMstxQmDJHHpCdOaUsmK(), 
						ControllerType.Custom => sehbQsZuQABZjDjxFKQULctyxyTM(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstButtonDown(ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Keyboard => crYmYMJpOdlTtRpQYUbIyvlByCXE(), 
						ControllerType.Joystick => uteBBbCfGnPxvEbtiOlnxDnUeOqUA(controllerId), 
						ControllerType.Mouse => OmMlvvXIsRyJMmYOaZbRMiTcbjIc(), 
						ControllerType.Custom => bfsidlfPeRlfaeRcKBikFNJqBZRR(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollControllerForFirstAxis(ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Keyboard => ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ(), 
						ControllerType.Joystick => GVHqGmbkAjMsEMGNCoIfwHFqZPCf(controllerId), 
						ControllerType.Mouse => YjWHdCggbuqpwAZexohiavlASUol(), 
						ControllerType.Custom => BhdxnneYDtrVYJmgMgUSyQyvlPAj(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollControllerForAllElements(ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Keyboard => LiaggUJdGqEbnUHaJirapWtQcAyb(), 
						ControllerType.Joystick => ldhakhiwkMgwMlnqdvpNgTfLoxbGA(controllerId), 
						ControllerType.Mouse => ACNnjLPzvUeteXmGAbOIHaulDfqT(), 
						ControllerType.Custom => YBehOOjfnwLGRDIJeqnkdjeKPivU(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollControllerForAllElementsDown(ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Keyboard => yGrUJoQHFeAPvJOWNOkZKfuJgZAhA(), 
						ControllerType.Joystick => PFrWmQpWSIpRALcBmIprjYnwRykU(controllerId), 
						ControllerType.Mouse => KtFNijZXnbblguDXYfNzvMlMggHg(), 
						ControllerType.Custom => GVAKNATrLGPzKRGpGCPKCbVtwijv(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollControllerForAllButtons(ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Keyboard => LiaggUJdGqEbnUHaJirapWtQcAyb(), 
						ControllerType.Joystick => YveXhwlHhMtKQVEAAHYptuMODUCQ(controllerId), 
						ControllerType.Mouse => RvDXsrqGMosaXCbgXOjqkXTiSasn(), 
						ControllerType.Custom => JoBwqGZbenhjVMFAqSMnUhMsGbNjA(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollControllerForAllButtonsDown(ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Keyboard => yGrUJoQHFeAPvJOWNOkZKfuJgZAhA(), 
						ControllerType.Joystick => bESljPDPsNfEadhAuZSwZGXmNLOr(controllerId), 
						ControllerType.Mouse => yQtDsCFBizEmBJfFeZaXeGikdYXdA(), 
						ControllerType.Custom => IbgyANcOmMnPkMJUJqcWJWEHMlNe(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollControllerForAllAxes(ControllerType controllerType, int controllerId)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Keyboard => new List<ControllerPollingInfo>(), 
						ControllerType.Joystick => VHKCEixymMsTQAgkFhtSnKiEknuG(controllerId), 
						ControllerType.Mouse => twlkFDQqehthpevClZThKzhObYxD(), 
						ControllerType.Custom => cAeKNyqGBNhIeAHFOPcFsDRBuUcOA(controllerId), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstElement(ControllerType controllerType)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Keyboard => KwQUKtXoiLJKOoUMfbOpBuRGRuBQ(), 
						ControllerType.Joystick => eOVLBPuiAZNlarxbZiECuOjlABVP(), 
						ControllerType.Mouse => eqORiwivwAlYyAgsNQfUEuBwmgPT(), 
						ControllerType.Custom => YKIuKDjUjnBgHRrFbWSrkCIVUjKQ(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstButton(ControllerType controllerType)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Keyboard => KwQUKtXoiLJKOoUMfbOpBuRGRuBQ(), 
						ControllerType.Joystick => oBwARNIxsQBhizkEYoWdDLMCLlKiB(), 
						ControllerType.Mouse => XXLpaoeskMstxQmDJHHpCdOaUsmK(), 
						ControllerType.Custom => jYQptacoiQAfZXzkQaNTgQBjocdD(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstButtonDown(ControllerType controllerType)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Keyboard => crYmYMJpOdlTtRpQYUbIyvlByCXE(), 
						ControllerType.Joystick => ZLgZrVfOlGOEuWOKcvnARbCODYoAA(), 
						ControllerType.Mouse => OmMlvvXIsRyJMmYOaZbRMiTcbjIc(), 
						ControllerType.Custom => EvsTOtRCqAYwmXUtkhyzuCKsgZIh(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public ControllerPollingInfo PollAllControllersOfTypeForFirstAxis(ControllerType controllerType)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return controllerType switch
					{
						ControllerType.Keyboard => ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ(), 
						ControllerType.Joystick => HmjiJsVFldikVuUoEhsiLzZhTcux(), 
						ControllerType.Mouse => YjWHdCggbuqpwAZexohiavlASUol(), 
						ControllerType.Custom => JRomsCcQZvgYFlSaDFVXAWnkcbCAb(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollAllControllersOfTypeForAllElements(ControllerType controllerType)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Keyboard => LiaggUJdGqEbnUHaJirapWtQcAyb(), 
						ControllerType.Joystick => OEKzxfOKnKcJbVhBBKsahkFSfDWz(), 
						ControllerType.Mouse => ACNnjLPzvUeteXmGAbOIHaulDfqT(), 
						ControllerType.Custom => EWbPkosTGNhoQZXysYqYIlIclYOg(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollAllControllersOfTypeForAllElementsDown(ControllerType controllerType)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Keyboard => yGrUJoQHFeAPvJOWNOkZKfuJgZAhA(), 
						ControllerType.Joystick => qLectMaqNJsDBSwLgHwJoZFtTmAnA(), 
						ControllerType.Mouse => KtFNijZXnbblguDXYfNzvMlMggHg(), 
						ControllerType.Custom => MAAHwRDqIigEogZjtyTHhKWvgsmmA(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollAllControllersOfTypeForAllButtons(ControllerType controllerType)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Keyboard => LiaggUJdGqEbnUHaJirapWtQcAyb(), 
						ControllerType.Joystick => CUnXsfdrcAYYRCzieVdpdRxIdSei(), 
						ControllerType.Mouse => RvDXsrqGMosaXCbgXOjqkXTiSasn(), 
						ControllerType.Custom => jAucBqmCMCSarunbpPbPicLAMnCY(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollAllControllersOfTypeForAllButtonsDown(ControllerType controllerType)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Keyboard => yGrUJoQHFeAPvJOWNOkZKfuJgZAhA(), 
						ControllerType.Joystick => orgkYHxjaHqQsHXNDLGKEWCMndgW(), 
						ControllerType.Mouse => yQtDsCFBizEmBJfFeZaXeGikdYXdA(), 
						ControllerType.Custom => MVGdZhKLEGbybEcYRAmJxLaWSZUxA(), 
						_ => throw new NotImplementedException(), 
					};
				}

				public IEnumerable<ControllerPollingInfo> PollAllControllersOfTypeForAllAxes(ControllerType controllerType)
				{
					if (ReInput._id != HXDbdfBejnLtEUTWDlzRtNqHMysz)
					{
						ReInput.CheckInitialized(HXDbdfBejnLtEUTWDlzRtNqHMysz);
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return controllerType switch
					{
						ControllerType.Keyboard => new List<ControllerPollingInfo>(), 
						ControllerType.Joystick => dYPahPGUATUYGzIrSafvXFjzwEIP(), 
						ControllerType.Mouse => twlkFDQqehthpevClZThKzhObYxD(), 
						ControllerType.Custom => CJxAFjtgewaXTKpUDytpwRIYoASd(), 
						_ => throw new NotImplementedException(), 
					};
				}

				private ControllerPollingInfo pMurKafRskcAULBGgAejyCxhgcb(int P_0)
				{
					if (P_0 < 0)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					Joystick joystick = UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.mwluYWpfujxinFejkWlvebBLHySV(P_0);
					if (joystick == null)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = joystick.PollForFirstElement();
					if (result.success)
					{
						result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
					}
					return result;
				}

				private ControllerPollingInfo adYLQCCRJUlpaPqwnwwKvlKYDwGf(int P_0)
				{
					if (P_0 < 0)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					Joystick joystick = UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.mwluYWpfujxinFejkWlvebBLHySV(P_0);
					if (joystick == null)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = joystick.PollForFirstElementDown();
					if (result.success)
					{
						result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
					}
					return result;
				}

				private ControllerPollingInfo spQfLELKjaHZkhJeqWZtrIWYRPYP(int P_0)
				{
					if (P_0 < 0)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					Joystick joystick = UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.mwluYWpfujxinFejkWlvebBLHySV(P_0);
					if (joystick == null)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = joystick.PollForFirstButton();
					if (result.success)
					{
						result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
					}
					return result;
				}

				private ControllerPollingInfo uteBBbCfGnPxvEbtiOlnxDnUeOqUA(int P_0)
				{
					if (P_0 < 0)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					Joystick joystick = UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.mwluYWpfujxinFejkWlvebBLHySV(P_0);
					if (joystick == null)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = joystick.PollForFirstButtonDown();
					if (result.success)
					{
						result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
					}
					return result;
				}

				private ControllerPollingInfo GVHqGmbkAjMsEMGNCoIfwHFqZPCf(int P_0)
				{
					if (P_0 < 0)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					Joystick joystick = UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.mwluYWpfujxinFejkWlvebBLHySV(P_0);
					if (joystick == null)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = joystick.PollForFirstAxis();
					if (result.success)
					{
						result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
					}
					return result;
				}

				[IteratorStateMachine(typeof(DfAxFJWPyfreEqOSjRdaNkqoOPrR))]
				private IEnumerable<ControllerPollingInfo> ldhakhiwkMgwMlnqdvpNgTfLoxbGA(int P_0)
				{
					return new DfAxFJWPyfreEqOSjRdaNkqoOPrR(-2)
					{
						earJQJIlhFHnqwPrLTJkexNmllPw = this,
						NuAceYoQzcwAAwGhFNyMPvPveezF = P_0
					};
				}

				[IteratorStateMachine(typeof(UAUbKEBpojXMDntarigocsPzzWDrA))]
				private IEnumerable<ControllerPollingInfo> PFrWmQpWSIpRALcBmIprjYnwRykU(int P_0)
				{
					return new UAUbKEBpojXMDntarigocsPzzWDrA(-2)
					{
						KKoCGHSoeOnZdgnmttVSiDdtyCYn = this,
						gCiokynAOMyoBdfQScPofXCgQItA = P_0
					};
				}

				[IteratorStateMachine(typeof(OkqNUkhZGRAqVmEYpIUTYTwuPKBP))]
				private IEnumerable<ControllerPollingInfo> YveXhwlHhMtKQVEAAHYptuMODUCQ(int P_0)
				{
					return new OkqNUkhZGRAqVmEYpIUTYTwuPKBP(-2)
					{
						TeUryaLixnvhLpOrBAXzKlATzxwh = this,
						txaRKpPPXnlblxWKuxkdCgkCWQTB = P_0
					};
				}

				[IteratorStateMachine(typeof(hzKZjuRHPmbQzHelfwzXkxLayIGb))]
				private IEnumerable<ControllerPollingInfo> bESljPDPsNfEadhAuZSwZGXmNLOr(int P_0)
				{
					return new hzKZjuRHPmbQzHelfwzXkxLayIGb(-2)
					{
						ZwrHEHkELhRljVGVyybRfIzNfkgGA = this,
						RZYxMnmiGZBYDRxHtBuOqJpahqSAA = P_0
					};
				}

				[IteratorStateMachine(typeof(JIcKVkZMRYtdSzcruKtDnwbbmWMP))]
				private IEnumerable<ControllerPollingInfo> VHKCEixymMsTQAgkFhtSnKiEknuG(int P_0)
				{
					return new JIcKVkZMRYtdSzcruKtDnwbbmWMP(-2)
					{
						obtnImNfCVbDGYqBkgSWVGqdNNtQ = this,
						rrEiFKftpjeHUbWvQVzBkORAwnFH = P_0
					};
				}

				private ControllerPollingInfo eOVLBPuiAZNlarxbZiECuOjlABVP()
				{
					IList<Joystick> list = UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElement();
						if (result.success)
						{
							result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo NrHThWkPdVHvCfwuYdxLcsvFYOxJB()
				{
					IList<Joystick> list = UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElementDown();
						if (result.success)
						{
							result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo oBwARNIxsQBhizkEYoWdDLMCLlKiB()
				{
					IList<Joystick> list = UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButton();
						if (result.success)
						{
							result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo ZLgZrVfOlGOEuWOKcvnARbCODYoAA()
				{
					IList<Joystick> list = UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButtonDown();
						if (result.success)
						{
							result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo HmjiJsVFldikVuUoEhsiLzZhTcux()
				{
					IList<Joystick> list = UiVtMiGqkCeljvdgeLyYPGbOFuWE.RUnKPUrOyOObdOUTXiiMIJUsdTqaA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstAxis();
						if (result.success)
						{
							result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				[IteratorStateMachine(typeof(yrKMDKHUtkemgcFUipRjnmLxjTiz))]
				private IEnumerable<ControllerPollingInfo> OEKzxfOKnKcJbVhBBKsahkFSfDWz()
				{
					return new yrKMDKHUtkemgcFUipRjnmLxjTiz(-2)
					{
						wpeozKJkQFSCNYqmWpWKPJFraTCl = this
					};
				}

				[IteratorStateMachine(typeof(AAyLCgOkORcfxPNTXEnqvhUIGegw))]
				private IEnumerable<ControllerPollingInfo> qLectMaqNJsDBSwLgHwJoZFtTmAnA()
				{
					return new AAyLCgOkORcfxPNTXEnqvhUIGegw(-2)
					{
						KSQGPhWpXKBqHAalOXNFrvQYjdXjA = this
					};
				}

				[IteratorStateMachine(typeof(dLfvWuPauaQBidWxKvAQKzodNKYA))]
				private IEnumerable<ControllerPollingInfo> CUnXsfdrcAYYRCzieVdpdRxIdSei()
				{
					return new dLfvWuPauaQBidWxKvAQKzodNKYA(-2)
					{
						UdKfDejjHDnniYTdsIOVUpJIPydr = this
					};
				}

				[IteratorStateMachine(typeof(ArLzvAnOXbMsoVjRGdCfEajCJYLp))]
				private IEnumerable<ControllerPollingInfo> orgkYHxjaHqQsHXNDLGKEWCMndgW()
				{
					return new ArLzvAnOXbMsoVjRGdCfEajCJYLp(-2)
					{
						UyWRAXuhPoAJBIAhLgVprYeoUZwG = this
					};
				}

				[IteratorStateMachine(typeof(NApakeYkuNhsppTkhIZwICKhuSry))]
				private IEnumerable<ControllerPollingInfo> dYPahPGUATUYGzIrSafvXFjzwEIP()
				{
					return new NApakeYkuNhsppTkhIZwICKhuSry(-2)
					{
						mXupIgPOGIqbGSmjhALOAOJMiqOG = this
					};
				}

				private ControllerPollingInfo KwQUKtXoiLJKOoUMfbOpBuRGRuBQ()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.bypclrNzWVojCWiWuiLJxTlmQIGc)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Keyboard.PollForFirstKey();
				}

				private ControllerPollingInfo crYmYMJpOdlTtRpQYUbIyvlByCXE()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.bypclrNzWVojCWiWuiLJxTlmQIGc)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Keyboard.PollForFirstKeyDown();
				}

				private IEnumerable<ControllerPollingInfo> LiaggUJdGqEbnUHaJirapWtQcAyb()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.bypclrNzWVojCWiWuiLJxTlmQIGc)
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Keyboard.PollForAllKeys();
				}

				private IEnumerable<ControllerPollingInfo> yGrUJoQHFeAPvJOWNOkZKfuJgZAhA()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.bypclrNzWVojCWiWuiLJxTlmQIGc)
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Keyboard.PollForAllKeysDown();
				}

				private ControllerPollingInfo eqORiwivwAlYyAgsNQfUEuBwmgPT()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.ZZMYZUrdobtGhqoVQTwAeRfvVdOo)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Mouse.PollForFirstElement();
				}

				private ControllerPollingInfo JfXxUUimIdjSBmUlVLaNgDZknpEM()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.ZZMYZUrdobtGhqoVQTwAeRfvVdOo)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Mouse.PollForFirstElementDown();
				}

				private ControllerPollingInfo XXLpaoeskMstxQmDJHHpCdOaUsmK()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.ZZMYZUrdobtGhqoVQTwAeRfvVdOo)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Mouse.PollForFirstButton();
				}

				private ControllerPollingInfo OmMlvvXIsRyJMmYOaZbRMiTcbjIc()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.ZZMYZUrdobtGhqoVQTwAeRfvVdOo)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Mouse.PollForFirstButtonDown();
				}

				private ControllerPollingInfo YjWHdCggbuqpwAZexohiavlASUol()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.ZZMYZUrdobtGhqoVQTwAeRfvVdOo)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Mouse.PollForFirstAxis();
				}

				private IEnumerable<ControllerPollingInfo> ACNnjLPzvUeteXmGAbOIHaulDfqT()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.ZZMYZUrdobtGhqoVQTwAeRfvVdOo)
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Mouse.PollForAllElements();
				}

				private IEnumerable<ControllerPollingInfo> KtFNijZXnbblguDXYfNzvMlMggHg()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.ZZMYZUrdobtGhqoVQTwAeRfvVdOo)
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Mouse.PollForAllElementsDown();
				}

				private IEnumerable<ControllerPollingInfo> RvDXsrqGMosaXCbgXOjqkXTiSasn()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.ZZMYZUrdobtGhqoVQTwAeRfvVdOo)
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Mouse.PollForAllButtons();
				}

				private IEnumerable<ControllerPollingInfo> yQtDsCFBizEmBJfFeZaXeGikdYXdA()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.ZZMYZUrdobtGhqoVQTwAeRfvVdOo)
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Mouse.PollForAllButtonsDown();
				}

				private IEnumerable<ControllerPollingInfo> twlkFDQqehthpevClZThKzhObYxD()
				{
					if (!UiVtMiGqkCeljvdgeLyYPGbOFuWE.ZZMYZUrdobtGhqoVQTwAeRfvVdOo)
					{
						return EmptyObjects<ControllerPollingInfo>.EmptyReadOnlyIListT;
					}
					return UiVtMiGqkCeljvdgeLyYPGbOFuWE.Mouse.PollForAllAxes();
				}

				private ControllerPollingInfo CJaCdzsfEfayiXDpQTFMevIefeZc(int P_0)
				{
					if (P_0 < 0)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					CustomController customController = UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.mwluYWpfujxinFejkWlvebBLHySV(P_0);
					if (customController == null)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = customController.PollForFirstElement();
					if (result.success)
					{
						result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
					}
					return result;
				}

				private ControllerPollingInfo AFjyVmsgXVzOTNehuXWOfxMEFebQ(int P_0)
				{
					if (P_0 < 0)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					CustomController customController = UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.mwluYWpfujxinFejkWlvebBLHySV(P_0);
					if (customController == null)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = customController.PollForFirstElementDown();
					if (result.success)
					{
						result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
					}
					return result;
				}

				private ControllerPollingInfo sehbQsZuQABZjDjxFKQULctyxyTM(int P_0)
				{
					if (P_0 < 0)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					CustomController customController = UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.mwluYWpfujxinFejkWlvebBLHySV(P_0);
					if (customController == null)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = customController.PollForFirstButton();
					if (result.success)
					{
						result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
					}
					return result;
				}

				private ControllerPollingInfo bfsidlfPeRlfaeRcKBikFNJqBZRR(int P_0)
				{
					if (P_0 < 0)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					CustomController customController = UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.mwluYWpfujxinFejkWlvebBLHySV(P_0);
					if (customController == null)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = customController.PollForFirstButtonDown();
					if (result.success)
					{
						result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
					}
					return result;
				}

				private ControllerPollingInfo BhdxnneYDtrVYJmgMgUSyQyvlPAj(int P_0)
				{
					if (P_0 < 0)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					CustomController customController = UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.mwluYWpfujxinFejkWlvebBLHySV(P_0);
					if (customController == null)
					{
						return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
					}
					ControllerPollingInfo result = customController.PollForFirstAxis();
					if (result.success)
					{
						result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
					}
					return result;
				}

				[IteratorStateMachine(typeof(chKfOcEMyrvdxdGuRvbupfWzwkOcA))]
				private IEnumerable<ControllerPollingInfo> YBehOOjfnwLGRDIJeqnkdjeKPivU(int P_0)
				{
					return new chKfOcEMyrvdxdGuRvbupfWzwkOcA(-2)
					{
						qImIjlJlBPFaUZtXsdateSKqUidX = this,
						CXhMEbBkQZxQIxOIfucyCeLnjJVAA = P_0
					};
				}

				[IteratorStateMachine(typeof(biBUPueSYATWVFKExBnFJfJsfShK))]
				private IEnumerable<ControllerPollingInfo> GVAKNATrLGPzKRGpGCPKCbVtwijv(int P_0)
				{
					return new biBUPueSYATWVFKExBnFJfJsfShK(-2)
					{
						yKmOVOgbTjtWsItOCRPdzuJWMmKD = this,
						tjFLaytIcENhPvGfzUqQHBQJQJKi = P_0
					};
				}

				[IteratorStateMachine(typeof(VRmWZjZQtAiwowbgUPMYExVhiney))]
				private IEnumerable<ControllerPollingInfo> JoBwqGZbenhjVMFAqSMnUhMsGbNjA(int P_0)
				{
					return new VRmWZjZQtAiwowbgUPMYExVhiney(-2)
					{
						vFGDbFAQFBXbmSSxbHldHdSEAxByA = this,
						hfHMZWGfiChXDGSaRbAurlWYnsdK = P_0
					};
				}

				[IteratorStateMachine(typeof(idLnPXiltXdnsMCDpeqkMwzmjEyn))]
				private IEnumerable<ControllerPollingInfo> IbgyANcOmMnPkMJUJqcWJWEHMlNe(int P_0)
				{
					return new idLnPXiltXdnsMCDpeqkMwzmjEyn(-2)
					{
						mHrRNNaglWHEZHSTdEohaJcJnhuyA = this,
						iXkyUMIPysBbIiUOVjrHyPuqaude = P_0
					};
				}

				[IteratorStateMachine(typeof(hlssEsOtapQNYkTovDnbubyuAEon))]
				private IEnumerable<ControllerPollingInfo> cAeKNyqGBNhIeAHFOPcFsDRBuUcOA(int P_0)
				{
					return new hlssEsOtapQNYkTovDnbubyuAEon(-2)
					{
						XXZBSOUSTboxhPYhDWWRiQeQBXyf = this,
						DFhwikCXHDQlliPyRcbUIBvJnhrn = P_0
					};
				}

				private ControllerPollingInfo YKIuKDjUjnBgHRrFbWSrkCIVUjKQ()
				{
					IList<CustomController> list = UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElement();
						if (result.success)
						{
							result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo gQjZtrFVDOWgdEgyGSyZboKSWBUy()
				{
					IList<CustomController> list = UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstElementDown();
						if (result.success)
						{
							result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo jYQptacoiQAfZXzkQaNTgQBjocdD()
				{
					IList<CustomController> list = UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButton();
						if (result.success)
						{
							result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo EvsTOtRCqAYwmXUtkhyzuCKsgZIh()
				{
					IList<CustomController> list = UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstButtonDown();
						if (result.success)
						{
							result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				private ControllerPollingInfo JRomsCcQZvgYFlSaDFVXAWnkcbCAb()
				{
					IList<CustomController> list = UiVtMiGqkCeljvdgeLyYPGbOFuWE.oXYdIKJxZGxUDTqfqmRNjceSdVhbA.ZDrrszgPTGReCxYGDqgUGfMuezBB;
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ControllerPollingInfo result = list[i].PollForFirstAxis();
						if (result.success)
						{
							result.playerId = GALayqYyGJvICjxphFBNPTmSSaQL.mgTogZEAHwpJMhbsccjZDcKdOLwp;
							return result;
						}
					}
					return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
				}

				[IteratorStateMachine(typeof(AMzxBfXhbGSdEValZAXIKLuGnThV))]
				private IEnumerable<ControllerPollingInfo> EWbPkosTGNhoQZXysYqYIlIclYOg()
				{
					return new AMzxBfXhbGSdEValZAXIKLuGnThV(-2)
					{
						wLxcgaEqFTfwnFwSeyKEqtSOsawqA = this
					};
				}

				[IteratorStateMachine(typeof(JbBOoruIKCtMnrMeXnpijjZGUQOH))]
				private IEnumerable<ControllerPollingInfo> MAAHwRDqIigEogZjtyTHhKWvgsmmA()
				{
					return new JbBOoruIKCtMnrMeXnpijjZGUQOH(-2)
					{
						qKIJuOJunTPwnkuJjjSARfJsrfkI = this
					};
				}

				[IteratorStateMachine(typeof(cLlOxdgNaIkRwiQzffEpqJlfnlFH))]
				private IEnumerable<ControllerPollingInfo> jAucBqmCMCSarunbpPbPicLAMnCY()
				{
					return new cLlOxdgNaIkRwiQzffEpqJlfnlFH(-2)
					{
						gAINsDSOzOphRmsKbzcmdDrAhwnI = this
					};
				}

				[IteratorStateMachine(typeof(lzLoAJPHWFuoNQSQcEYDpbXkHZAt))]
				private IEnumerable<ControllerPollingInfo> MVGdZhKLEGbybEcYRAmJxLaWSZUxA()
				{
					return new lzLoAJPHWFuoNQSQcEYDpbXkHZAt(-2)
					{
						jlNpgwPihJXTWDoqQBsctHMRibnG = this
					};
				}

				[IteratorStateMachine(typeof(IpYaLajIsurpWKzEmtrjdKMcSKPP))]
				private IEnumerable<ControllerPollingInfo> CJxAFjtgewaXTKpUDytpwRIYoASd()
				{
					return new IpYaLajIsurpWKzEmtrjdKMcSKPP(-2)
					{
						kvtDJbCdydPVGJzPierTgDMHoSClB = this
					};
				}
			}

			[Serializable]
			private sealed class qxNFcKBOVQmGYvUJlDYMARQAwsBBc
			{
				public static readonly qxNFcKBOVQmGYvUJlDYMARQAwsBBc _003C_003E9 = new qxNFcKBOVQmGYvUJlDYMARQAwsBBc();

				public static Action<Exception> _003C_003E9__23_0;

				public static Action<Exception> _003C_003E9__23_1;

				internal void wJBUIiZyGGcpupkTFyTitJCnlGUm(Exception P_0)
				{
					ReInput.HandleCallbackException("Player.ControllerHelper.ControllerAddedEvent", P_0);
				}

				internal void aLQfTGnftjEBsywkMMfqSzvSSPdy(Exception P_0)
				{
					ReInput.HandleCallbackException("Player.ControllerHelper.ControllerRemovedEvent", P_0);
				}
			}

			private sealed class oEOkRYxBxygOTEXOQApKPeCiXOsc : IEnumerable<Controller>, IEnumerable, IEnumerator<Controller>, IEnumerator, IDisposable
			{
				private int dBjCjGRUOPZMBOGTWAJpTcKELXBl;

				private Controller dTiwFbRHZLTKmVcpKuqndiLlXBfB;

				private int CCLFcvJyzhjnTmuecIcKcnrSuxoK;

				public ControllerHelper BHyBXVajkXfXcEWBgkqVdvCwUQZpB;

				private int QUatfGlIJcSQguFsLPFTzgtQRUQq;

				private IList<Joystick> GewIUEwSgdTLZbzzDJcPrefmgmKM;

				private int ZlJVBrLCAgiJUbHcNdJtfXvHecPPb;

				private IList<CustomController> JOtdzhHurOSQOvNLzEwesKmHNDQVA;

				private int DkAbcUARXpkvsANpfJSSPRalGRJyB;

				Controller IEnumerator<Controller>.Current
				{
					[DebuggerHidden]
					get
					{
						return dTiwFbRHZLTKmVcpKuqndiLlXBfB;
					}
				}

				object IEnumerator.Current
				{
					[DebuggerHidden]
					get
					{
						return dTiwFbRHZLTKmVcpKuqndiLlXBfB;
					}
				}

				[DebuggerHidden]
				public oEOkRYxBxygOTEXOQApKPeCiXOsc(int P_0)
				{
					dBjCjGRUOPZMBOGTWAJpTcKELXBl = P_0;
					CCLFcvJyzhjnTmuecIcKcnrSuxoK = Environment.CurrentManagedThreadId;
				}

				[DebuggerHidden]
				void IDisposable.Dispose()
				{
					GewIUEwSgdTLZbzzDJcPrefmgmKM = null;
					JOtdzhHurOSQOvNLzEwesKmHNDQVA = null;
					dBjCjGRUOPZMBOGTWAJpTcKELXBl = -2;
				}

				private bool MoveNext()
				{
					int num = dBjCjGRUOPZMBOGTWAJpTcKELXBl;
					ControllerHelper bHyBXVajkXfXcEWBgkqVdvCwUQZpB = BHyBXVajkXfXcEWBgkqVdvCwUQZpB;
					switch (num)
					{
					default:
						return false;
					case 0:
						dBjCjGRUOPZMBOGTWAJpTcKELXBl = -1;
						if (ReInput._id != bHyBXVajkXfXcEWBgkqVdvCwUQZpB.ONlkEECFwBoatxTxPkvzaElXELVj)
						{
							ReInput.CheckInitialized(bHyBXVajkXfXcEWBgkqVdvCwUQZpB.ONlkEECFwBoatxTxPkvzaElXELVj);
							return false;
						}
						if (bHyBXVajkXfXcEWBgkqVdvCwUQZpB.ZZMYZUrdobtGhqoVQTwAeRfvVdOo)
						{
							dTiwFbRHZLTKmVcpKuqndiLlXBfB = bHyBXVajkXfXcEWBgkqVdvCwUQZpB.Mouse;
							dBjCjGRUOPZMBOGTWAJpTcKELXBl = 1;
							return true;
						}
						goto IL_0070;
					case 1:
						dBjCjGRUOPZMBOGTWAJpTcKELXBl = -1;
						goto IL_0070;
					case 2:
						dBjCjGRUOPZMBOGTWAJpTcKELXBl = -1;
						goto IL_0094;
					case 3:
						dBjCjGRUOPZMBOGTWAJpTcKELXBl = -1;
						DkAbcUARXpkvsANpfJSSPRalGRJyB++;
						goto IL_00ec;
					case 4:
						{
							dBjCjGRUOPZMBOGTWAJpTcKELXBl = -1;
							DkAbcUARXpkvsANpfJSSPRalGRJyB++;
							break;
						}
						IL_0094:
						QUatfGlIJcSQguFsLPFTzgtQRUQq = bHyBXVajkXfXcEWBgkqVdvCwUQZpB.joystickCount;
						GewIUEwSgdTLZbzzDJcPrefmgmKM = bHyBXVajkXfXcEWBgkqVdvCwUQZpB.Joysticks;
						DkAbcUARXpkvsANpfJSSPRalGRJyB = 0;
						goto IL_00ec;
						IL_00ec:
						if (DkAbcUARXpkvsANpfJSSPRalGRJyB < QUatfGlIJcSQguFsLPFTzgtQRUQq)
						{
							dTiwFbRHZLTKmVcpKuqndiLlXBfB = GewIUEwSgdTLZbzzDJcPrefmgmKM[DkAbcUARXpkvsANpfJSSPRalGRJyB];
							dBjCjGRUOPZMBOGTWAJpTcKELXBl = 3;
							return true;
						}
						ZlJVBrLCAgiJUbHcNdJtfXvHecPPb = bHyBXVajkXfXcEWBgkqVdvCwUQZpB.customControllerCount;
						JOtdzhHurOSQOvNLzEwesKmHNDQVA = bHyBXVajkXfXcEWBgkqVdvCwUQZpB.CustomControllers;
						DkAbcUARXpkvsANpfJSSPRalGRJyB = 0;
						break;
						IL_0070:
						if (bHyBXVajkXfXcEWBgkqVdvCwUQZpB.bypclrNzWVojCWiWuiLJxTlmQIGc)
						{
							dTiwFbRHZLTKmVcpKuqndiLlXBfB = bHyBXVajkXfXcEWBgkqVdvCwUQZpB.Keyboard;
							dBjCjGRUOPZMBOGTWAJpTcKELXBl = 2;
							return true;
						}
						goto IL_0094;
					}
					if (DkAbcUARXpkvsANpfJSSPRalGRJyB < ZlJVBrLCAgiJUbHcNdJtfXvHecPPb)
					{
						dTiwFbRHZLTKmVcpKuqndiLlXBfB = JOtdzhHurOSQOvNLzEwesKmHNDQVA[DkAbcUARXpkvsANpfJSSPRalGRJyB];
						dBjCjGRUOPZMBOGTWAJpTcKELXBl = 4;
						return true;
					}
					return false;
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
				IEnumerator<Controller> IEnumerable<Controller>.GetEnumerator()
				{
					oEOkRYxBxygOTEXOQApKPeCiXOsc oEOkRYxBxygOTEXOQApKPeCiXOsc2;
					if (dBjCjGRUOPZMBOGTWAJpTcKELXBl == -2 && CCLFcvJyzhjnTmuecIcKcnrSuxoK == Environment.CurrentManagedThreadId)
					{
						dBjCjGRUOPZMBOGTWAJpTcKELXBl = 0;
						oEOkRYxBxygOTEXOQApKPeCiXOsc2 = this;
					}
					else
					{
						oEOkRYxBxygOTEXOQApKPeCiXOsc2 = new oEOkRYxBxygOTEXOQApKPeCiXOsc(0);
						oEOkRYxBxygOTEXOQApKPeCiXOsc2.BHyBXVajkXfXcEWBgkqVdvCwUQZpB = BHyBXVajkXfXcEWBgkqVdvCwUQZpB;
					}
					return oEOkRYxBxygOTEXOQApKPeCiXOsc2;
				}

				[DebuggerHidden]
				IEnumerator IEnumerable.GetEnumerator()
				{
					return ((IEnumerable<Controller>)this).GetEnumerator();
				}
			}

			private readonly fYKvcKBvQvLCetjdAAIphvYYuqYMA eEwnNlLDiORMxybSsBzKXNQcRftb;

			private bool ZZMYZUrdobtGhqoVQTwAeRfvVdOo;

			private bool bypclrNzWVojCWiWuiLJxTlmQIGc;

			private bool NPiBjWntpeMkvyslGLYavDjufdjQ;

			private double eOjOcRBqCLyTUnhCqNJrkmqgmEQI;

			private double oPejKzwBWjYIKLHAVBIiBgHdVrGkA;

			private SafeAction<ControllerAssignmentChangedEventArgs> xPdXfIowddCmEpMNTgeWpXfQdRAkA = new SafeAction<ControllerAssignmentChangedEventArgs>(qxNFcKBOVQmGYvUJlDYMARQAwsBBc._003C_003E9.wJBUIiZyGGcpupkTFyTitJCnlGUm);

			private SafeAction<ControllerAssignmentChangedEventArgs> ZCjNXYGhnYWcKmFuARKEnDeAQTOw = new SafeAction<ControllerAssignmentChangedEventArgs>(qxNFcKBOVQmGYvUJlDYMARQAwsBBc._003C_003E9.aLQfTGnftjEBsywkMMfqSzvSSPdy);

			private readonly ioztArpRhJBBpDKdzHOLwnTeIxai BPoUszNGyThVhLHpFIamgLjstajIA;

			private readonly Player VkOpbxhhZlsWxLzmWsTDhMjvNRzf;

			private readonly BfSuWOtYIJOEShfeXemgQlZkXemn eSQWbpaTmOIOgkwAIhmweKyhNgfL;

			private readonly int ONlkEECFwBoatxTxPkvzaElXELVj;

			public readonly MapHelper maps;

			public readonly ConflictCheckingHelper conflictChecking;

			public readonly PollingHelper polling;

			private fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap> RUnKPUrOyOObdOUTXiiMIJUsdTqaA => (fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap>)eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Joystick);

			private global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<KeyboardMap> YjmFIPDNzImOLouWSUVvaLZLKYPIA => (global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<KeyboardMap>)eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Keyboard).eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(0).UEDbvZCpORwBphRIdGLFIEwJLiiEb;

			private global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<MouseMap> zUjTgtTczFEofMiBtuFpiPLDHbokA => (global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<MouseMap>)eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Mouse).eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(0).UEDbvZCpORwBphRIdGLFIEwJLiiEb;

			private fLcZuTpMOwYPWmGZCMQAZMEBzxNc<CustomController, CustomControllerMap> oXYdIKJxZGxUDTqfqmRNjceSdVhbA => (fLcZuTpMOwYPWmGZCMQAZMEBzxNc<CustomController, CustomControllerMap>)eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Custom);

			public bool hasMouse
			{
				get
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
						return false;
					}
					return ZZMYZUrdobtGhqoVQTwAeRfvVdOo;
				}
				set
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					}
					else
					{
						if (ZZMYZUrdobtGhqoVQTwAeRfvVdOo == value)
						{
							return;
						}
						ZZMYZUrdobtGhqoVQTwAeRfvVdOo = value;
						if (value)
						{
							eSQWbpaTmOIOgkwAIhmweKyhNgfL.twQjFuHMXyfPWoSHuycASCLyVoAM(Mouse);
						}
						else
						{
							eSQWbpaTmOIOgkwAIhmweKyhNgfL.YPbLVCMIvIHWRQTVZLgUrWLBevvq(Mouse);
						}
						if (value)
						{
							maps.layoutManager.Apply();
							if (xPdXfIowddCmEpMNTgeWpXfQdRAkA.Count > 0)
							{
								xPdXfIowddCmEpMNTgeWpXfQdRAkA.Invoke(new ControllerAssignmentChangedEventArgs(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.id, ReInput.controllers.Mouse.id, ControllerType.Mouse, value));
							}
						}
						else if (ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Count > 0)
						{
							ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Invoke(new ControllerAssignmentChangedEventArgs(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.id, ReInput.controllers.Mouse.id, ControllerType.Mouse, value));
						}
					}
				}
			}

			public bool hasKeyboard
			{
				get
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
						return false;
					}
					return bypclrNzWVojCWiWuiLJxTlmQIGc;
				}
				set
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					}
					else
					{
						if (bypclrNzWVojCWiWuiLJxTlmQIGc == value)
						{
							return;
						}
						bypclrNzWVojCWiWuiLJxTlmQIGc = value;
						if (value)
						{
							eSQWbpaTmOIOgkwAIhmweKyhNgfL.twQjFuHMXyfPWoSHuycASCLyVoAM(Keyboard);
						}
						else
						{
							eSQWbpaTmOIOgkwAIhmweKyhNgfL.YPbLVCMIvIHWRQTVZLgUrWLBevvq(Keyboard);
						}
						if (value)
						{
							maps.layoutManager.Apply();
							if (xPdXfIowddCmEpMNTgeWpXfQdRAkA.Count > 0)
							{
								xPdXfIowddCmEpMNTgeWpXfQdRAkA.Invoke(new ControllerAssignmentChangedEventArgs(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.id, ReInput.controllers.Keyboard.id, ControllerType.Keyboard, value));
							}
						}
						else if (ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Count > 0)
						{
							ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Invoke(new ControllerAssignmentChangedEventArgs(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.id, ReInput.controllers.Keyboard.id, ControllerType.Keyboard, value));
						}
					}
				}
			}

			public bool excludeFromControllerAutoAssignment
			{
				get
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
						return false;
					}
					return NPiBjWntpeMkvyslGLYavDjufdjQ;
				}
				set
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					}
					else
					{
						NPiBjWntpeMkvyslGLYavDjufdjQ = value;
					}
				}
			}

			public Keyboard Keyboard
			{
				get
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
						return null;
					}
					return ReInput.controllers.Keyboard;
				}
			}

			public Mouse Mouse
			{
				get
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
						return null;
					}
					return ReInput.controllers.Mouse;
				}
			}

			public int joystickCount
			{
				get
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
						return 0;
					}
					return eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Joystick).hdHAWgwPJNkiyeCCaiyVDbScMIAib;
				}
			}

			public IList<Joystick> Joysticks
			{
				get
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
						return EmptyObjects<Joystick>.EmptyReadOnlyIListT;
					}
					return (eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Joystick) as fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap>).ZDrrszgPTGReCxYGDqgUGfMuezBB;
				}
			}

			public int customControllerCount
			{
				get
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
						return 0;
					}
					return eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Custom).hdHAWgwPJNkiyeCCaiyVDbScMIAib;
				}
			}

			public IList<CustomController> CustomControllers
			{
				get
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
						return EmptyObjects<CustomController>.EmptyReadOnlyIListT;
					}
					return (eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Custom) as fLcZuTpMOwYPWmGZCMQAZMEBzxNc<CustomController, CustomControllerMap>).ZDrrszgPTGReCxYGDqgUGfMuezBB;
				}
			}

			public IEnumerable<Controller> Controllers
			{
				[IteratorStateMachine(typeof(oEOkRYxBxygOTEXOQApKPeCiXOsc))]
				get
				{
					return new oEOkRYxBxygOTEXOQApKPeCiXOsc(-2)
					{
						BHyBXVajkXfXcEWBgkqVdvCwUQZpB = this
					};
				}
			}

			public event Action<ControllerAssignmentChangedEventArgs> ControllerAddedEvent
			{
				add
				{
					xPdXfIowddCmEpMNTgeWpXfQdRAkA.AddDelegate(value);
				}
				remove
				{
					xPdXfIowddCmEpMNTgeWpXfQdRAkA.RemoveDelegate(value);
				}
			}

			public event Action<ControllerAssignmentChangedEventArgs> ControllerRemovedEvent
			{
				add
				{
					ZCjNXYGhnYWcKmFuARKEnDeAQTOw.AddDelegate(value);
				}
				remove
				{
					ZCjNXYGhnYWcKmFuARKEnDeAQTOw.RemoveDelegate(value);
				}
			}

			internal ControllerHelper(Player P_0, EhUOIcAWtPzjNpBYLAHyNaaERzaE P_1, ControllerMapLayoutManager.ICvEqTkajGQZLXkOyPTomcjaeCWs P_2, ControllerMapEnabler.CyXCJRXKNwVfGTMPVLIRUynNmCKI P_3)
			{
				ONlkEECFwBoatxTxPkvzaElXELVj = ReInput.id;
				VkOpbxhhZlsWxLzmWsTDhMjvNRzf = P_0;
				maps = new MapHelper(P_0, this, P_1, P_2, P_3);
				polling = new PollingHelper(P_0, this);
				conflictChecking = new ConflictCheckingHelper(P_0, this);
				eEwnNlLDiORMxybSsBzKXNQcRftb = new fYKvcKBvQvLCetjdAAIphvYYuqYMA(4);
				eEwnNlLDiORMxybSsBzKXNQcRftb.WSAzvniOAzEKalJWPqwJODoejxSCA(0, ControllerType.Joystick, new fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap>());
				eEwnNlLDiORMxybSsBzKXNQcRftb.WSAzvniOAzEKalJWPqwJODoejxSCA(1, ControllerType.Keyboard, new fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Keyboard, KeyboardMap>());
				eEwnNlLDiORMxybSsBzKXNQcRftb.WSAzvniOAzEKalJWPqwJODoejxSCA(2, ControllerType.Mouse, new fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Mouse, MouseMap>());
				eEwnNlLDiORMxybSsBzKXNQcRftb.WSAzvniOAzEKalJWPqwJODoejxSCA(3, ControllerType.Custom, new fLcZuTpMOwYPWmGZCMQAZMEBzxNc<CustomController, CustomControllerMap>());
				BPoUszNGyThVhLHpFIamgLjstajIA = new ioztArpRhJBBpDKdzHOLwnTeIxai(P_0);
				eSQWbpaTmOIOgkwAIhmweKyhNgfL = new BfSuWOtYIJOEShfeXemgQlZkXemn(UnityTools.externalTools.GetControllerTemplateTypes(), UnityTools.externalTools.GetControllerTemplateInterfaceTypes());
			}

			public T GetController<T>(int controllerId) where T : Controller
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return null;
				}
				return (T)eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(cVDyIiOsEfJNYzVuZSmuEXqylgT.eaYIahKSyFNptVWNBbCwNVCCqDxVA<T>()).tRzYsIDkUnycgWhiaBoHxEoQFNVe(controllerId);
			}

			public Controller GetController(ControllerType controllerType, int controllerId)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return null;
				}
				return eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType).tRzYsIDkUnycgWhiaBoHxEoQFNVe(controllerId);
			}

			public T GetControllerWithTag<T>(string tag) where T : Controller
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return null;
				}
				return (T)eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(cVDyIiOsEfJNYzVuZSmuEXqylgT.eaYIahKSyFNptVWNBbCwNVCCqDxVA<T>()).akiTudPECybTenNzjQJPXgKQxnjf(tag);
			}

			public Controller GetControllerWithTag(ControllerType controllerType, string tag)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return null;
				}
				return eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(controllerType).akiTudPECybTenNzjQJPXgKQxnjf(tag);
			}

			public void AddController<T>(int controllerId, bool removeFromOtherPlayers) where T : Controller
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return;
				}
				Type typeFromHandle = typeof(T);
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Joystick)))
				{
					NLFjCdyYjKnSnaPQYaCnBtWGxaoc(controllerId, removeFromOtherPlayers);
					return;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Keyboard)))
				{
					AddController(ControllerType.Keyboard, controllerId, removeFromOtherPlayers);
					return;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Mouse)))
				{
					AddController(ControllerType.Mouse, controllerId, removeFromOtherPlayers);
					return;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(CustomController)))
				{
					eWcVyKtszZtWFjBgiwHnUsshGBbc(controllerId, removeFromOtherPlayers);
					return;
				}
				throw new NotImplementedException();
			}

			public void AddController(Controller controller, bool removeFromOtherPlayers)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
				}
				else if (controller != null)
				{
					switch (controller.type)
					{
					case ControllerType.Joystick:
						hhmqdFNEziZipQHXNKUvCsrJCuwl(controller as Joystick, removeFromOtherPlayers);
						break;
					case ControllerType.Keyboard:
						AddController(controller.type, controller.id, removeFromOtherPlayers);
						break;
					case ControllerType.Mouse:
						AddController(controller.type, controller.id, removeFromOtherPlayers);
						break;
					case ControllerType.Custom:
						kEAbMXgUFCzMuOyhJNeDWOPVrEoGA(controller as CustomController, removeFromOtherPlayers);
						break;
					default:
						throw new NotImplementedException();
					}
				}
			}

			public void AddController(ControllerType controllerType, int controllerId, bool removeFromOtherPlayers)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return;
				}
				switch (controllerType)
				{
				case ControllerType.Joystick:
					hhmqdFNEziZipQHXNKUvCsrJCuwl(ReInput.controllers.GetController(controllerType, controllerId) as Joystick, removeFromOtherPlayers);
					break;
				case ControllerType.Keyboard:
					if (removeFromOtherPlayers)
					{
						ReInput.controllers.RemoveControllerFromAllPlayers(controllerType, controllerId);
					}
					hasKeyboard = true;
					break;
				case ControllerType.Mouse:
					if (removeFromOtherPlayers)
					{
						ReInput.controllers.RemoveControllerFromAllPlayers(controllerType, controllerId);
					}
					hasMouse = true;
					break;
				case ControllerType.Custom:
					kEAbMXgUFCzMuOyhJNeDWOPVrEoGA(ReInput.controllers.GetController(controllerType, controllerId) as CustomController, removeFromOtherPlayers);
					break;
				default:
					throw new NotImplementedException();
				}
			}

			public void RemoveController<T>(int controllerId) where T : Controller
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return;
				}
				Type typeFromHandle = typeof(T);
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Joystick)))
				{
					vYDeVSBaxYpinEXvmsBZJpzePelt(controllerId);
					return;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Keyboard)))
				{
					RemoveController(ControllerType.Keyboard, 0);
					return;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Mouse)))
				{
					RemoveController(ControllerType.Mouse, 0);
					return;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(CustomController)))
				{
					PQWjEvkFgnoxCCDAvPBdmhizDesm(controllerId);
					return;
				}
				throw new NotImplementedException();
			}

			public void RemoveController(ControllerType controllerType, int controllerId)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return;
				}
				switch (controllerType)
				{
				case ControllerType.Joystick:
					vYDeVSBaxYpinEXvmsBZJpzePelt(controllerId);
					break;
				case ControllerType.Keyboard:
					hasKeyboard = false;
					break;
				case ControllerType.Mouse:
					hasMouse = false;
					break;
				case ControllerType.Custom:
					PQWjEvkFgnoxCCDAvPBdmhizDesm(controllerId);
					break;
				default:
					throw new NotImplementedException();
				}
			}

			public void RemoveController(Controller controller)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
				}
				else if (controller != null)
				{
					switch (controller.type)
					{
					case ControllerType.Joystick:
						KMPMiMoEebbjbaespAkXfArEmWZbb(controller as Joystick);
						break;
					case ControllerType.Keyboard:
						hasKeyboard = false;
						break;
					case ControllerType.Mouse:
						hasMouse = false;
						break;
					case ControllerType.Custom:
						olyogBEWAdfNkvaMUpyfhXXcrdsH(controller as CustomController);
						break;
					default:
						throw new NotImplementedException();
					}
				}
			}

			public bool ContainsController<T>(int controllerId) where T : Controller
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return false;
				}
				Type typeFromHandle = typeof(T);
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Joystick)))
				{
					return ContainsController(ControllerType.Joystick, controllerId);
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Keyboard)))
				{
					return bypclrNzWVojCWiWuiLJxTlmQIGc;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Mouse)))
				{
					return ZZMYZUrdobtGhqoVQTwAeRfvVdOo;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(CustomController)))
				{
					return ContainsController(ControllerType.Custom, controllerId);
				}
				throw new NotImplementedException();
			}

			public bool ContainsController(ControllerType controllerType, int controllerId)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return false;
				}
				return controllerType switch
				{
					ControllerType.Joystick => eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Joystick).YPWLlgPgmfXEmjcHdghesHfKasTy(controllerId), 
					ControllerType.Keyboard => bypclrNzWVojCWiWuiLJxTlmQIGc, 
					ControllerType.Mouse => ZZMYZUrdobtGhqoVQTwAeRfvVdOo, 
					ControllerType.Custom => eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Custom).YPWLlgPgmfXEmjcHdghesHfKasTy(controllerId), 
					_ => throw new NotImplementedException(), 
				};
			}

			public bool ContainsController(Controller controller)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return false;
				}
				if (controller == null)
				{
					return false;
				}
				return ContainsController(controller.type, controller.id);
			}

			public void ClearControllersOfType<T>() where T : Controller
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return;
				}
				Type typeFromHandle = typeof(T);
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Joystick)))
				{
					PzRHtKWecKuYcWSNrsegbAMZmHXp();
					return;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Keyboard)))
				{
					hasKeyboard = false;
					return;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Mouse)))
				{
					hasMouse = false;
					return;
				}
				if (ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(CustomController)))
				{
					ecVpLEAxFuwJpnXHFmdHBGfgpdjr();
					return;
				}
				if ((object)typeFromHandle == typeof(Controller))
				{
					ClearAllControllers();
					return;
				}
				throw new NotImplementedException();
			}

			public void ClearControllersOfType(ControllerType controllerType)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return;
				}
				switch (controllerType)
				{
				case ControllerType.Joystick:
					PzRHtKWecKuYcWSNrsegbAMZmHXp();
					break;
				case ControllerType.Keyboard:
					hasKeyboard = false;
					break;
				case ControllerType.Mouse:
					hasMouse = false;
					break;
				case ControllerType.Custom:
					ecVpLEAxFuwJpnXHFmdHBGfgpdjr();
					break;
				default:
					throw new NotImplementedException();
				}
			}

			public void ClearAllControllers()
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return;
				}
				PzRHtKWecKuYcWSNrsegbAMZmHXp();
				ecVpLEAxFuwJpnXHFmdHBGfgpdjr();
				hasMouse = false;
				hasKeyboard = false;
			}

			public Controller GetLastActiveController()
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return null;
				}
				Controller result = null;
				double num = 0.0;
				lyKfjHMKSMYEutKjUTtrlIjEdLSBA(ControllerType.Joystick, ref result, ref num);
				if (ZZMYZUrdobtGhqoVQTwAeRfvVdOo && eOjOcRBqCLyTUnhCqNJrkmqgmEQI > num)
				{
					result = Mouse;
					num = eOjOcRBqCLyTUnhCqNJrkmqgmEQI;
				}
				if (bypclrNzWVojCWiWuiLJxTlmQIGc && oPejKzwBWjYIKLHAVBIiBgHdVrGkA > num)
				{
					result = Keyboard;
					num = oPejKzwBWjYIKLHAVBIiBgHdVrGkA;
				}
				lyKfjHMKSMYEutKjUTtrlIjEdLSBA(ControllerType.Custom, ref result, ref num);
				return result;
			}

			public Controller GetLastActiveController(ControllerType controllerType)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return null;
				}
				Controller result = null;
				double num = 0.0;
				switch (controllerType)
				{
				case ControllerType.Joystick:
				case ControllerType.Custom:
					lyKfjHMKSMYEutKjUTtrlIjEdLSBA(controllerType, ref result, ref num);
					break;
				case ControllerType.Keyboard:
					if (bypclrNzWVojCWiWuiLJxTlmQIGc && oPejKzwBWjYIKLHAVBIiBgHdVrGkA > 0.0)
					{
						result = Keyboard;
					}
					break;
				case ControllerType.Mouse:
					if (ZZMYZUrdobtGhqoVQTwAeRfvVdOo && eOjOcRBqCLyTUnhCqNJrkmqgmEQI > 0.0)
					{
						result = Mouse;
					}
					break;
				default:
					throw new NotImplementedException();
				}
				return result;
			}

			private void lyKfjHMKSMYEutKjUTtrlIjEdLSBA(ControllerType P_0, ref Controller P_1, ref double P_2)
			{
				rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
				int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib;
				for (int i = 0; i < num; i++)
				{
					double num2 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).OvEOwhMasvJnaipypySCYOXubbwy;
					if (!(num2 <= P_2))
					{
						P_1 = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(i).HwlnGjcjkEjHQFCbiyeLWdkAyzlm;
						P_2 = num2;
					}
				}
			}

			public Controller GetLastActiveController<T>() where T : Controller
			{
				return GetLastActiveController(cVDyIiOsEfJNYzVuZSmuEXqylgT.eaYIahKSyFNptVWNBbCwNVCCqDxVA<T>());
			}

			public void AddLastActiveControllerChangedDelegate(PlayerActiveControllerChangedDelegate callback)
			{
				if (ReInput.isReady)
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					}
					else
					{
						VkOpbxhhZlsWxLzmWsTDhMjvNRzf.XCFDVqZUbyGILGzarkLNUnbiMkmM.SdpLFiihptMZfHxZkgvTbdRFrLMb(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, callback);
					}
				}
			}

			public void AddLastActiveControllerChangedDelegate(PlayerActiveControllerChangedDelegate callback, ControllerType controllerType)
			{
				if (ReInput.isReady)
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					}
					else
					{
						VkOpbxhhZlsWxLzmWsTDhMjvNRzf.XCFDVqZUbyGILGzarkLNUnbiMkmM.xxYxXFLnjpWLAKLMCqCzPDwcTCDG(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, controllerType);
					}
				}
			}

			public void RemoveLastActiveControllerChangedDelegate(PlayerActiveControllerChangedDelegate callback)
			{
				if (ReInput.isReady)
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					}
					else
					{
						VkOpbxhhZlsWxLzmWsTDhMjvNRzf.XCFDVqZUbyGILGzarkLNUnbiMkmM.DHseRPCIsZLFIZMymJMAlhDdgliT(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, callback);
					}
				}
			}

			public void RemoveLastActiveControllerChangedDelegate(PlayerActiveControllerChangedDelegate callback, ControllerType controllerType)
			{
				if (ReInput.isReady)
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					}
					else
					{
						VkOpbxhhZlsWxLzmWsTDhMjvNRzf.XCFDVqZUbyGILGzarkLNUnbiMkmM.PunOxnffJqmmGYdePHvHNeDGfcMCA(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, controllerType);
					}
				}
			}

			public void ClearLastActiveControllerChangedDelegates()
			{
				if (ReInput.isReady)
				{
					if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
					{
						ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					}
					else
					{
						VkOpbxhhZlsWxLzmWsTDhMjvNRzf.XCFDVqZUbyGILGzarkLNUnbiMkmM.YFkjSeFvBxUzGvlWAZFkKErUZjVl(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp);
					}
				}
			}

			public Controller GetFirstControllerWithTemplate(Guid templateTypeGuid)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return null;
				}
				int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
				for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
				{
					Controller controller = hnqtBQKjoqwHqzNfTYLISOIaClZg(eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i).CmnNBXSIdbihnrUVSoDPbcGWAEaJA, Controller.LGCbTsfsKjXUihgBjSRSfMnFskaqB, templateTypeGuid);
					if (controller != null)
					{
						return controller;
					}
				}
				return null;
			}

			public Controller GetFirstControllerWithTemplate(Type templateType)
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return null;
				}
				int qmWCUQsQKLAGFDPAlkhDMwpKjAPm = eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm;
				for (int i = 0; i < qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
				{
					Controller controller = hnqtBQKjoqwHqzNfTYLISOIaClZg(eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i).CmnNBXSIdbihnrUVSoDPbcGWAEaJA, Controller.uZGcBlGhyPRcywtCOzvauDzbiJPSA, templateType);
					if (controller != null)
					{
						return controller;
					}
				}
				return null;
			}

			public Controller GetFirstControllerWithTemplate<T>() where T : class
			{
				return GetFirstControllerWithTemplate(typeof(T));
			}

			public IList<TInterface> GetControllerTemplates<TInterface>() where TInterface : IControllerTemplate
			{
				if (ReInput._id != ONlkEECFwBoatxTxPkvzaElXELVj)
				{
					ReInput.CheckInitialized(ONlkEECFwBoatxTxPkvzaElXELVj);
					return EmptyObjects<TInterface>.EmptyReadOnlyIListT;
				}
				return eSQWbpaTmOIOgkwAIhmweKyhNgfL.VJzsOuBgNaRhPOlQAJASDbsknCBn<TInterface>();
			}

			private Controller hnqtBQKjoqwHqzNfTYLISOIaClZg<_0001>(ControllerType P_0, Func<Controller, _0001, bool> P_1, _0001 P_2)
			{
				switch (P_0)
				{
				case ControllerType.Joystick:
				{
					int num2 = joystickCount;
					IList<Joystick> joysticks = Joysticks;
					for (int j = 0; j < num2; j++)
					{
						if (P_1(joysticks[j], P_2))
						{
							return joysticks[j];
						}
					}
					return null;
				}
				case ControllerType.Keyboard:
					if (bypclrNzWVojCWiWuiLJxTlmQIGc && P_1(Keyboard, P_2))
					{
						return Keyboard;
					}
					return null;
				case ControllerType.Mouse:
					if (ZZMYZUrdobtGhqoVQTwAeRfvVdOo && P_1(Mouse, P_2))
					{
						return Mouse;
					}
					return null;
				case ControllerType.Custom:
				{
					int num = customControllerCount;
					IList<CustomController> customControllers = CustomControllers;
					for (int i = 0; i < num; i++)
					{
						if (P_1(customControllers[i], P_2))
						{
							return customControllers[i];
						}
					}
					return null;
				}
				default:
					throw new NotImplementedException();
				}
			}

			internal void SSidKDjCaEnvidWxZZKyckkfwiPNA()
			{
				for (int i = 0; i < eEwnNlLDiORMxybSsBzKXNQcRftb.qmWCUQsQKLAGFDPAlkhDMwpKjAPm; i++)
				{
					eEwnNlLDiORMxybSsBzKXNQcRftb.kImmYmzaCiBQvBGIMTsyajaFoeGI(i).qDXuYglJNcJClRMmUAumTMMTvwVf();
				}
				eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Keyboard).OectVWAHFVqDzTrMmfXfIKlAoVeY(new fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Keyboard, KeyboardMap>.afpFMKpOFhEzEBdDrNcunaaeluicA(ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn.TVvLxBfEgOqnloHdRFcagvmpmnZT, new global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<KeyboardMap>(0)));
				eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Mouse).OectVWAHFVqDzTrMmfXfIKlAoVeY(new fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Mouse, MouseMap>.afpFMKpOFhEzEBdDrNcunaaeluicA(ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn.RVEYLKIoOydxyctXJKWZflgnfQyi, new global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<MouseMap>(0)));
				BPoUszNGyThVhLHpFIamgLjstajIA.CzSpYeUOHMbjQobFyCDEdLrecRHNA();
				oPejKzwBWjYIKLHAVBIiBgHdVrGkA = 0.0;
				eOjOcRBqCLyTUnhCqNJrkmqgmEQI = 0.0;
				maps.HLmEzKlOOuHieSRjqefvIcNqaOxSA();
			}

			internal double srsDPIieElYApGDgafIUhFdfynUi(int P_0)
			{
				return BPoUszNGyThVhLHpFIamgLjstajIA.ZUUEyZycxAvvEXwRwslIAfpPWAmv(P_0)?.cdTQrUFQrmxXmTDABSVQNGgmgVzV ?? (-1.0);
			}

			internal void hhmqdFNEziZipQHXNKUvCsrJCuwl(Joystick P_0, bool P_1)
			{
				if (P_0 == null)
				{
					return;
				}
				rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Joystick);
				if (rJRHfxObWEyZQOmmYgoxgmGnxuol2.YPWLlgPgmfXEmjcHdghesHfKasTy(P_0.id))
				{
					return;
				}
				if (P_1)
				{
					ReInput.controllers.RemoveJoystickFromAllPlayers(P_0);
				}
				ioztArpRhJBBpDKdzHOLwnTeIxai.AWuExMeXcRcFvejhKnEIzzotTfRNA aWuExMeXcRcFvejhKnEIzzotTfRNA = BPoUszNGyThVhLHpFIamgLjstajIA.ZUUEyZycxAvvEXwRwslIAfpPWAmv(P_0.id);
				fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap>.afpFMKpOFhEzEBdDrNcunaaeluicA afpFMKpOFhEzEBdDrNcunaaeluicA;
				if (aWuExMeXcRcFvejhKnEIzzotTfRNA != null && aWuExMeXcRcFvejhKnEIzzotTfRNA.NixxSePeJrbvXWAFUfJZYdxHrNWF != null)
				{
					afpFMKpOFhEzEBdDrNcunaaeluicA = new fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap>.afpFMKpOFhEzEBdDrNcunaaeluicA(P_0, aWuExMeXcRcFvejhKnEIzzotTfRNA.NixxSePeJrbvXWAFUfJZYdxHrNWF);
				}
				else
				{
					global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<JoystickMap> jXfmwiBdreTpaOqXNtMEJcOCaZSP = maps.NCEGKHAjrDLxSPYDJQXymFdrQMxeA(P_0, true);
					if (jXfmwiBdreTpaOqXNtMEJcOCaZSP == null)
					{
						jXfmwiBdreTpaOqXNtMEJcOCaZSP = new global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<JoystickMap>(P_0.id);
					}
					afpFMKpOFhEzEBdDrNcunaaeluicA = new fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap>.afpFMKpOFhEzEBdDrNcunaaeluicA(P_0, jXfmwiBdreTpaOqXNtMEJcOCaZSP);
				}
				rJRHfxObWEyZQOmmYgoxgmGnxuol2.OectVWAHFVqDzTrMmfXfIKlAoVeY(afpFMKpOFhEzEBdDrNcunaaeluicA);
				BPoUszNGyThVhLHpFIamgLjstajIA.GrkffHPRitRYlQBnXbCOZrtaeRHj(afpFMKpOFhEzEBdDrNcunaaeluicA);
				eSQWbpaTmOIOgkwAIhmweKyhNgfL.twQjFuHMXyfPWoSHuycASCLyVoAM(P_0);
				maps.layoutManager.Apply();
				if (xPdXfIowddCmEpMNTgeWpXfQdRAkA.Count > 0)
				{
					xPdXfIowddCmEpMNTgeWpXfQdRAkA.Invoke(new ControllerAssignmentChangedEventArgs(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.id, P_0.id, ControllerType.Joystick, true));
				}
			}

			internal void NLFjCdyYjKnSnaPQYaCnBtWGxaoc(int P_0, bool P_1)
			{
				Joystick joystick = ReInput.controllers.GetJoystick(P_0);
				if (joystick != null)
				{
					hhmqdFNEziZipQHXNKUvCsrJCuwl(joystick, P_1);
				}
			}

			internal void vYDeVSBaxYpinEXvmsBZJpzePelt(int P_0)
			{
				rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Joystick);
				if (rJRHfxObWEyZQOmmYgoxgmGnxuol2.YPWLlgPgmfXEmjcHdghesHfKasTy(P_0))
				{
					if (rJRHfxObWEyZQOmmYgoxgmGnxuol2.eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(P_0) is fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap>.afpFMKpOFhEzEBdDrNcunaaeluicA afpFMKpOFhEzEBdDrNcunaaeluicA)
					{
						BPoUszNGyThVhLHpFIamgLjstajIA.GrkffHPRitRYlQBnXbCOZrtaeRHj(afpFMKpOFhEzEBdDrNcunaaeluicA);
					}
					rJRHfxObWEyZQOmmYgoxgmGnxuol2.tLuHYEZoYWwWCNCBqjbBnZSeRHNd(P_0);
					Joystick joystick = ReInput.controllers.GetJoystick(P_0);
					eSQWbpaTmOIOgkwAIhmweKyhNgfL.YPbLVCMIvIHWRQTVZLgUrWLBevvq(joystick);
					if (ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Count > 0)
					{
						ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Invoke(new ControllerAssignmentChangedEventArgs(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.id, joystick.id, ControllerType.Joystick, false));
					}
				}
			}

			internal void KMPMiMoEebbjbaespAkXfArEmWZbb(Joystick P_0)
			{
				if (P_0 != null)
				{
					vYDeVSBaxYpinEXvmsBZJpzePelt(P_0.id);
				}
			}

			internal void PzRHtKWecKuYcWSNrsegbAMZmHXp()
			{
				rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Joystick);
				for (int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib - 1; num >= 0; num--)
				{
					BPoUszNGyThVhLHpFIamgLjstajIA.GrkffHPRitRYlQBnXbCOZrtaeRHj(rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num) as fLcZuTpMOwYPWmGZCMQAZMEBzxNc<Joystick, JoystickMap>.afpFMKpOFhEzEBdDrNcunaaeluicA);
					eSQWbpaTmOIOgkwAIhmweKyhNgfL.YPbLVCMIvIHWRQTVZLgUrWLBevvq(rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).HwlnGjcjkEjHQFCbiyeLWdkAyzlm);
					int id = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).HwlnGjcjkEjHQFCbiyeLWdkAyzlm.id;
					rJRHfxObWEyZQOmmYgoxgmGnxuol2.OyWpEnvgZWsPMJPfaFYuTBQZiZpn(num);
					if (ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Count > 0)
					{
						ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Invoke(new ControllerAssignmentChangedEventArgs(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.id, id, ControllerType.Joystick, false));
					}
				}
				rJRHfxObWEyZQOmmYgoxgmGnxuol2.qDXuYglJNcJClRMmUAumTMMTvwVf();
			}

			internal void kEAbMXgUFCzMuOyhJNeDWOPVrEoGA(CustomController P_0, bool P_1)
			{
				if (P_0 == null)
				{
					return;
				}
				rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Custom);
				if (!rJRHfxObWEyZQOmmYgoxgmGnxuol2.YPWLlgPgmfXEmjcHdghesHfKasTy(P_0.id))
				{
					if (P_1)
					{
						ReInput.controllers.RemoveCustomControllerFromAllPlayers(P_0);
					}
					global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<CustomControllerMap> jXfmwiBdreTpaOqXNtMEJcOCaZSP = maps.sqzurMWwILfVhUIUjcHbCfYAeYcYA(P_0, true);
					if (jXfmwiBdreTpaOqXNtMEJcOCaZSP == null)
					{
						jXfmwiBdreTpaOqXNtMEJcOCaZSP = new global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<CustomControllerMap>(P_0.id);
					}
					fLcZuTpMOwYPWmGZCMQAZMEBzxNc<CustomController, CustomControllerMap>.afpFMKpOFhEzEBdDrNcunaaeluicA afpFMKpOFhEzEBdDrNcunaaeluicA = new fLcZuTpMOwYPWmGZCMQAZMEBzxNc<CustomController, CustomControllerMap>.afpFMKpOFhEzEBdDrNcunaaeluicA(P_0, jXfmwiBdreTpaOqXNtMEJcOCaZSP);
					rJRHfxObWEyZQOmmYgoxgmGnxuol2.OectVWAHFVqDzTrMmfXfIKlAoVeY(afpFMKpOFhEzEBdDrNcunaaeluicA);
					eSQWbpaTmOIOgkwAIhmweKyhNgfL.twQjFuHMXyfPWoSHuycASCLyVoAM(P_0);
					maps.layoutManager.Apply();
					if (xPdXfIowddCmEpMNTgeWpXfQdRAkA.Count > 0)
					{
						xPdXfIowddCmEpMNTgeWpXfQdRAkA.Invoke(new ControllerAssignmentChangedEventArgs(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.id, P_0.id, ControllerType.Custom, true));
					}
				}
			}

			internal void eWcVyKtszZtWFjBgiwHnUsshGBbc(int P_0, bool P_1)
			{
				CustomController customController = ReInput.controllers.GetCustomController(P_0);
				if (customController != null)
				{
					kEAbMXgUFCzMuOyhJNeDWOPVrEoGA(customController, P_1);
				}
			}

			internal void PQWjEvkFgnoxCCDAvPBdmhizDesm(int P_0)
			{
				rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Custom);
				if (rJRHfxObWEyZQOmmYgoxgmGnxuol2.YPWLlgPgmfXEmjcHdghesHfKasTy(P_0))
				{
					rJRHfxObWEyZQOmmYgoxgmGnxuol2.eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(P_0);
					rJRHfxObWEyZQOmmYgoxgmGnxuol2.tLuHYEZoYWwWCNCBqjbBnZSeRHNd(P_0);
					CustomController customController = ReInput.controllers.GetCustomController(P_0);
					eSQWbpaTmOIOgkwAIhmweKyhNgfL.YPbLVCMIvIHWRQTVZLgUrWLBevvq(customController);
					if (ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Count > 0)
					{
						ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Invoke(new ControllerAssignmentChangedEventArgs(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.id, customController.id, ControllerType.Custom, false));
					}
				}
			}

			internal void olyogBEWAdfNkvaMUpyfhXXcrdsH(CustomController P_0)
			{
				if (P_0 != null)
				{
					PQWjEvkFgnoxCCDAvPBdmhizDesm(P_0.id);
				}
			}

			internal void ecVpLEAxFuwJpnXHFmdHBGfgpdjr()
			{
				rJRHfxObWEyZQOmmYgoxgmGnxuol rJRHfxObWEyZQOmmYgoxgmGnxuol2 = eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Custom);
				for (int num = rJRHfxObWEyZQOmmYgoxgmGnxuol2.hdHAWgwPJNkiyeCCaiyVDbScMIAib - 1; num >= 0; num--)
				{
					eSQWbpaTmOIOgkwAIhmweKyhNgfL.YPbLVCMIvIHWRQTVZLgUrWLBevvq(rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).HwlnGjcjkEjHQFCbiyeLWdkAyzlm);
					int id = rJRHfxObWEyZQOmmYgoxgmGnxuol2.EfgDlYQxDuklXErnyCYTFBkkEVmX(num).HwlnGjcjkEjHQFCbiyeLWdkAyzlm.id;
					rJRHfxObWEyZQOmmYgoxgmGnxuol2.OyWpEnvgZWsPMJPfaFYuTBQZiZpn(num);
					if (ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Count > 0)
					{
						ZCjNXYGhnYWcKmFuARKEnDeAQTOw.Invoke(new ControllerAssignmentChangedEventArgs(VkOpbxhhZlsWxLzmWsTDhMjvNRzf.id, id, ControllerType.Custom, false));
					}
				}
				rJRHfxObWEyZQOmmYgoxgmGnxuol2.qDXuYglJNcJClRMmUAumTMMTvwVf();
			}

			internal CustomController uCYyTbonGpeYijGrkXvihYKJNYQP(int P_0)
			{
				CustomController customController = VkOpbxhhZlsWxLzmWsTDhMjvNRzf.XCFDVqZUbyGILGzarkLNUnbiMkmM.WWEUAIfEGDhZVmEWMiuGLFXZCtBI(P_0);
				if (customController == null)
				{
					return null;
				}
				kEAbMXgUFCzMuOyhJNeDWOPVrEoGA(customController, false);
				return customController;
			}

			internal void paEuTLolRmWivldZSfhDYGfdyUrP(Action<bool, int, int> P_0)
			{
				TTRayEdrzseBNtFdqTzXmIbUSDBy<Joystick, JoystickMap>(ControllerType.Joystick, P_0);
			}

			internal void KfulNBNpspCMeYlkHDTukWbvxvIf(Keyboard P_0, ixyiAxzBBALGbgKSZMZuYYGBvaTW P_1, Action<bool, int, int> P_2)
			{
				if (!bypclrNzWVojCWiWuiLJxTlmQIGc || !P_0.enabled)
				{
					return;
				}
				DiHMazxWcrHjKAtwYQbMtWMSQcFJ njNjryiutAUEterrCvmdMFtwSYGc = kBOilrfmQspwwsLlQucgVePHzaAKA.NjNjryiutAUEterrCvmdMFtwSYGc;
				bool flag = false;
				vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Keyboard).eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(0).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
				int num = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA;
				KeyCombinationOverrideMode keyCombinationOverrideMode = ReInput.configVars.keyCombinationOverrideMode;
				bool flag2 = keyCombinationOverrideMode == KeyCombinationOverrideMode.None;
				ixyiAxzBBALGbgKSZMZuYYGBvaTW.PVgbInnSkREPcFeCSmhRojPiLzqbb pVgbInnSkREPcFeCSmhRojPiLzqbb = ((keyCombinationOverrideMode == KeyCombinationOverrideMode.Overlap) ? ixyiAxzBBALGbgKSZMZuYYGBvaTW.PVgbInnSkREPcFeCSmhRojPiLzqbb.OverlapModifiers : ixyiAxzBBALGbgKSZMZuYYGBvaTW.PVgbInnSkREPcFeCSmhRojPiLzqbb.Normal);
				SJMiMZJZkwuyvjnzAVoAtfGYlqFC.qBGoeVnKVifhULDhTYEtKsmyCkrB qBGoeVnKVifhULDhTYEtKsmyCkrB = new SJMiMZJZkwuyvjnzAVoAtfGYlqFC.qBGoeVnKVifhULDhTYEtKsmyCkrB
				{
					ERNnDUTmwxeiuEOOuKXIzmfWhxXbb = ReInput.configVars.generateKeyEventsOnKeyCombinationOverride
				};
				for (int i = 0; i < num; i++)
				{
					KeyboardMap keyboardMap = (KeyboardMap)vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(i);
					if (!keyboardMap.enabled)
					{
						continue;
					}
					AList<ActionElementMap> aList = keyboardMap.LrIuPDSCedgUWTQghItEJWRAaExrA;
					int count = aList._count;
					for (int j = 0; j < count; j++)
					{
						ActionElementMap actionElementMap = aList._items[j];
						if (!actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
						{
							continue;
						}
						int actionId = actionElementMap._actionId;
						KeyboardKeyCode keyboardKeyCode = actionElementMap._keyboardKeyCode;
						ModifierKeyFlags modifierKeyFlags = actionElementMap.modifierKeyFlags;
						bool flag3 = false;
						bool flag4;
						if (modifierKeyFlags != ModifierKeyFlags.None)
						{
							ButtonStateFlags buttonStateFlags = (P_0.zkOdMTnuSDBkqLNykibsCvZeIHhV(keyboardKeyCode, modifierKeyFlags) ? ButtonStateFlags.On : ButtonStateFlags.Off);
							flag4 = buttonStateFlags != ButtonStateFlags.Off;
							if (!flag4)
							{
								SJMiMZJZkwuyvjnzAVoAtfGYlqFC sJMiMZJZkwuyvjnzAVoAtfGYlqFC = SJMiMZJZkwuyvjnzAVoAtfGYlqFC.ftJHJOVhirgnAfpyAYwegOIzPfjTA(actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu);
								if (sJMiMZJZkwuyvjnzAVoAtfGYlqFC != null && sJMiMZJZkwuyvjnzAVoAtfGYlqFC.jbgfcTRHsxwRThLPghNULhDTfYHB(true) != ButtonStateFlags.Off)
								{
									flag4 = true;
								}
							}
						}
						else
						{
							ButtonStateFlags buttonStateFlags = P_0.faGHnbatZKmYeuwMysYwtGBKNqobb(actionElementMap.uxemeTqImFAncCLpTkOkfOWaWKUK);
							flag4 = buttonStateFlags != ButtonStateFlags.Off;
						}
						if (flag4 && !flag2 && !P_1.AsvqUKNLdGsyPmVglwTYjGlSiFUQ(keyboardKeyCode, modifierKeyFlags, pVgbInnSkREPcFeCSmhRojPiLzqbb, out flag3))
						{
							P_1.fKYEriIowNmHTgwXmFnaoYmvAnyNA(keyboardKeyCode, modifierKeyFlags);
						}
					}
				}
				for (int k = 0; k < num; k++)
				{
					KeyboardMap keyboardMap = (KeyboardMap)vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(k);
					if (!keyboardMap.enabled)
					{
						continue;
					}
					AList<ActionElementMap> aList = keyboardMap.LrIuPDSCedgUWTQghItEJWRAaExrA;
					int count = aList._count;
					for (int j = 0; j < count; j++)
					{
						ActionElementMap actionElementMap = aList._items[j];
						if (!actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
						{
							continue;
						}
						int actionId = actionElementMap._actionId;
						KeyboardKeyCode keyboardKeyCode = actionElementMap._keyboardKeyCode;
						ModifierKeyFlags modifierKeyFlags = actionElementMap.modifierKeyFlags;
						bool flag5 = false;
						bool flag3 = false;
						ButtonStateFlags buttonStateFlags;
						bool flag4;
						if (modifierKeyFlags != ModifierKeyFlags.None)
						{
							buttonStateFlags = (P_0.zkOdMTnuSDBkqLNykibsCvZeIHhV(keyboardKeyCode, modifierKeyFlags) ? ButtonStateFlags.On : ButtonStateFlags.Off);
							flag4 = buttonStateFlags != ButtonStateFlags.Off;
							if (!flag4)
							{
								SJMiMZJZkwuyvjnzAVoAtfGYlqFC sJMiMZJZkwuyvjnzAVoAtfGYlqFC = SJMiMZJZkwuyvjnzAVoAtfGYlqFC.ftJHJOVhirgnAfpyAYwegOIzPfjTA(actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu);
								if (sJMiMZJZkwuyvjnzAVoAtfGYlqFC != null && sJMiMZJZkwuyvjnzAVoAtfGYlqFC.jbgfcTRHsxwRThLPghNULhDTfYHB(true) != ButtonStateFlags.Off)
								{
									flag4 = true;
								}
							}
						}
						else
						{
							buttonStateFlags = P_0.faGHnbatZKmYeuwMysYwtGBKNqobb(actionElementMap.uxemeTqImFAncCLpTkOkfOWaWKUK);
							flag4 = buttonStateFlags != ButtonStateFlags.Off;
						}
						if (flag4)
						{
							if (!flag2)
							{
								flag5 = P_1.AsvqUKNLdGsyPmVglwTYjGlSiFUQ(keyboardKeyCode, modifierKeyFlags, pVgbInnSkREPcFeCSmhRojPiLzqbb, out flag3);
							}
							if (flag3 || modifierKeyFlags != ModifierKeyFlags.None)
							{
								qBGoeVnKVifhULDhTYEtKsmyCkrB.BTTSOiPbusxOBSfmzcMRKHiSiSzQ = flag5;
								SJMiMZJZkwuyvjnzAVoAtfGYlqFC sJMiMZJZkwuyvjnzAVoAtfGYlqFC = SJMiMZJZkwuyvjnzAVoAtfGYlqFC.DFXjNULgOQrQHhNvRWZzvbhTKOA(actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu, qBGoeVnKVifhULDhTYEtKsmyCkrB);
								if (keyCombinationOverrideMode == KeyCombinationOverrideMode.Pause)
								{
									sJMiMZJZkwuyvjnzAVoAtfGYlqFC.qOLLFCdasZlBISSMjqpXBOjybSRdA = flag5;
								}
								else if (flag5)
								{
									sJMiMZJZkwuyvjnzAVoAtfGYlqFC.qOLLFCdasZlBISSMjqpXBOjybSRdA = true;
								}
								sJMiMZJZkwuyvjnzAVoAtfGYlqFC.LXTCaFDkmlzkVLNnHNcpjJnNkhlDb(ReInput.currentUpdateLoop, buttonStateFlags, true);
								buttonStateFlags = sJMiMZJZkwuyvjnzAVoAtfGYlqFC.jbgfcTRHsxwRThLPghNULhDTfYHB(true);
							}
						}
						if (buttonStateFlags != ButtonStateFlags.Off)
						{
							DCnwqoJdfwdlvbWRxgxZiOCgzoju(P_0, keyboardMap, actionElementMap, njNjryiutAUEterrCvmdMFtwSYGc, buttonStateFlags);
							P_2(arg1: true, VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId);
							flag = true;
							continue;
						}
						if (njNjryiutAUEterrCvmdMFtwSYGc.qYHCHUUoOfAWcggXJJyAvBfgOuQzA != 0f)
						{
							njNjryiutAUEterrCvmdMFtwSYGc.qYHCHUUoOfAWcggXJJyAvBfgOuQzA = 0f;
						}
						if (njNjryiutAUEterrCvmdMFtwSYGc.qDDMBBwlnkhbJkDUeoEGHQzUFYqab != ButtonStateFlags.Off)
						{
							njNjryiutAUEterrCvmdMFtwSYGc.qDDMBBwlnkhbJkDUeoEGHQzUFYqab = ButtonStateFlags.Off;
						}
						P_2(arg1: false, VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId);
					}
				}
				if (flag)
				{
					oPejKzwBWjYIKLHAVBIiBgHdVrGkA = ReInput.unscaledTime;
				}
			}

			private static void DCnwqoJdfwdlvbWRxgxZiOCgzoju(Keyboard P_0, ControllerMap P_1, ActionElementMap P_2, DiHMazxWcrHjKAtwYQbMtWMSQcFJ P_3, ButtonStateFlags P_4)
			{
				float num = (((P_4 & ButtonStateFlags.On) != ButtonStateFlags.Off) ? 1f : 0f);
				if (num != 0f && P_2._axisContribution == Pole.Negative)
				{
					num *= -1f;
				}
				P_3.qYHCHUUoOfAWcggXJJyAvBfgOuQzA = num;
				P_3.qDDMBBwlnkhbJkDUeoEGHQzUFYqab = P_4;
				P_3.dETBOxiMUQgZgPWxRvIkMzjzDSSO = P_0;
				P_3.JDGBgXPVMDVUqFoYrFTwdkmFwdty = ControllerType.Keyboard;
				P_3.bUScxzgFvNwAwsaDaAgXaHxyciQHA = ControllerElementType.Button;
				P_3.lJuSuVYfkbBfxQxkTnPCOPsqeqmN = P_2;
				P_3.msrNRqwsDjCYqTbjaFOmiKuFHiNl = P_1;
				if (P_3.zdzDmjIWBKsoyhDwpGsHUxPSXTUh)
				{
					P_3.zdzDmjIWBKsoyhDwpGsHUxPSXTUh = false;
				}
				if (P_3.PWuJoHkuXhJbNSKrnlqVhyYROSlc)
				{
					P_3.PWuJoHkuXhJbNSKrnlqVhyYROSlc = false;
				}
			}

			internal void OswiFrhrsUTajCLxSjQkuHVIeaPg(Mouse P_0, Action<bool, int, int> P_1)
			{
				if (!ZZMYZUrdobtGhqoVQTwAeRfvVdOo || !P_0.enabled)
				{
					return;
				}
				vHszkbCJdDAIcILHhpVCxcZlIBxlA vHszkbCJdDAIcILHhpVCxcZlIBxlA2 = eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(ControllerType.Mouse).eRwYtMwCUAzTPLpEsDUVDDEmgeZRA(0).UEDbvZCpORwBphRIdGLFIEwJLiiEb;
				DiHMazxWcrHjKAtwYQbMtWMSQcFJ njNjryiutAUEterrCvmdMFtwSYGc = kBOilrfmQspwwsLlQucgVePHzaAKA.NjNjryiutAUEterrCvmdMFtwSYGc;
				bool flag = false;
				int num = vHszkbCJdDAIcILHhpVCxcZlIBxlA2.spuxbZMpjzXXEeAzzgWchppYZEErA;
				for (int i = 0; i < num; i++)
				{
					MouseMap mouseMap = (MouseMap)vHszkbCJdDAIcILHhpVCxcZlIBxlA2.atsKcfQrzLEbpHgPTmFdSKsaiGvVA(i);
					if (!mouseMap.enabled)
					{
						continue;
					}
					AList<ActionElementMap> aList = mouseMap.bluHTIHZzVOouyueNmhnQDqXANlH;
					if (aList != null)
					{
						int count = aList._count;
						for (int j = 0; j < count; j++)
						{
							ActionElementMap actionElementMap = aList._items[j];
							if (!actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA || actionElementMap._elementType != ControllerElementType.Axis)
							{
								continue;
							}
							int actionId = actionElementMap._actionId;
							if (!P_0.xYieDEgzHjpAALzGGAlrgVIEtdXHB(actionElementMap, actionId, true, false, out var num2))
							{
								continue;
							}
							if (num2 == 0f)
							{
								P_0.xYieDEgzHjpAALzGGAlrgVIEtdXHB(actionElementMap, actionId, true, true, out var num3);
								if (num3 == 0f)
								{
									P_1(arg1: false, VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId);
									continue;
								}
							}
							njNjryiutAUEterrCvmdMFtwSYGc.qYHCHUUoOfAWcggXJJyAvBfgOuQzA = num2;
							njNjryiutAUEterrCvmdMFtwSYGc.dETBOxiMUQgZgPWxRvIkMzjzDSSO = P_0;
							njNjryiutAUEterrCvmdMFtwSYGc.JDGBgXPVMDVUqFoYrFTwdkmFwdty = ControllerType.Mouse;
							njNjryiutAUEterrCvmdMFtwSYGc.bUScxzgFvNwAwsaDaAgXaHxyciQHA = ControllerElementType.Axis;
							njNjryiutAUEterrCvmdMFtwSYGc.lJuSuVYfkbBfxQxkTnPCOPsqeqmN = actionElementMap;
							njNjryiutAUEterrCvmdMFtwSYGc.msrNRqwsDjCYqTbjaFOmiKuFHiNl = mouseMap;
							if (njNjryiutAUEterrCvmdMFtwSYGc.PWuJoHkuXhJbNSKrnlqVhyYROSlc)
							{
								njNjryiutAUEterrCvmdMFtwSYGc.PWuJoHkuXhJbNSKrnlqVhyYROSlc = false;
							}
							if (njNjryiutAUEterrCvmdMFtwSYGc.rMZgpEQqPgUbVxUUelignejLxGDg != AxisCoordinateMode.Relative)
							{
								njNjryiutAUEterrCvmdMFtwSYGc.rMZgpEQqPgUbVxUUelignejLxGDg = AxisCoordinateMode.Relative;
							}
							P_1(arg1: true, VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId);
							flag = true;
						}
					}
					AList<ActionElementMap> aList2 = mouseMap.LrIuPDSCedgUWTQghItEJWRAaExrA;
					if (aList2 == null)
					{
						continue;
					}
					int count2 = aList2._count;
					for (int k = 0; k < count2; k++)
					{
						ActionElementMap actionElementMap2 = aList2._items[k];
						if (!actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA || actionElementMap2._elementType != ControllerElementType.Button)
						{
							continue;
						}
						int actionId2 = actionElementMap2._actionId;
						if (!P_0.BLbebkNyKNYgVESpoLfsIwKqgdIj(actionElementMap2, actionId2, out var qYHCHUUoOfAWcggXJJyAvBfgOuQzA, out njNjryiutAUEterrCvmdMFtwSYGc.zdzDmjIWBKsoyhDwpGsHUxPSXTUh))
						{
							continue;
						}
						ButtonStateFlags buttonStateFlags = P_0.faGHnbatZKmYeuwMysYwtGBKNqobb(actionElementMap2.uxemeTqImFAncCLpTkOkfOWaWKUK);
						if (buttonStateFlags == ButtonStateFlags.Off)
						{
							P_1(arg1: false, VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId2);
							continue;
						}
						njNjryiutAUEterrCvmdMFtwSYGc.qYHCHUUoOfAWcggXJJyAvBfgOuQzA = qYHCHUUoOfAWcggXJJyAvBfgOuQzA;
						njNjryiutAUEterrCvmdMFtwSYGc.qDDMBBwlnkhbJkDUeoEGHQzUFYqab = buttonStateFlags;
						njNjryiutAUEterrCvmdMFtwSYGc.dETBOxiMUQgZgPWxRvIkMzjzDSSO = P_0;
						njNjryiutAUEterrCvmdMFtwSYGc.JDGBgXPVMDVUqFoYrFTwdkmFwdty = ControllerType.Mouse;
						njNjryiutAUEterrCvmdMFtwSYGc.bUScxzgFvNwAwsaDaAgXaHxyciQHA = ControllerElementType.Button;
						njNjryiutAUEterrCvmdMFtwSYGc.lJuSuVYfkbBfxQxkTnPCOPsqeqmN = actionElementMap2;
						njNjryiutAUEterrCvmdMFtwSYGc.msrNRqwsDjCYqTbjaFOmiKuFHiNl = mouseMap;
						if (njNjryiutAUEterrCvmdMFtwSYGc.zdzDmjIWBKsoyhDwpGsHUxPSXTUh)
						{
							njNjryiutAUEterrCvmdMFtwSYGc.zdzDmjIWBKsoyhDwpGsHUxPSXTUh = false;
						}
						if (njNjryiutAUEterrCvmdMFtwSYGc.PWuJoHkuXhJbNSKrnlqVhyYROSlc)
						{
							njNjryiutAUEterrCvmdMFtwSYGc.PWuJoHkuXhJbNSKrnlqVhyYROSlc = false;
						}
						P_1(arg1: true, VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId2);
						flag = true;
					}
				}
				if (flag)
				{
					eOjOcRBqCLyTUnhCqNJrkmqgmEQI = ReInput.unscaledTime;
				}
			}

			internal void YWgkmvLKQVcbyGVwGnLXrRWLJmVxA(Action<bool, int, int> P_0)
			{
				TTRayEdrzseBNtFdqTzXmIbUSDBy<CustomController, CustomControllerMap>(ControllerType.Custom, P_0);
			}

			private void TTRayEdrzseBNtFdqTzXmIbUSDBy<_0001, _0002>(ControllerType P_0, Action<bool, int, int> P_1) where _0001 : ControllerWithAxes where _0002 : ControllerMapWithAxes
			{
				fLcZuTpMOwYPWmGZCMQAZMEBzxNc<_0001, _0002> fLcZuTpMOwYPWmGZCMQAZMEBzxNc2 = (fLcZuTpMOwYPWmGZCMQAZMEBzxNc<_0001, _0002>)eEwnNlLDiORMxybSsBzKXNQcRftb.kFBeiGjxShwPyQqgGeeBVAenBxYfA(P_0);
				DiHMazxWcrHjKAtwYQbMtWMSQcFJ njNjryiutAUEterrCvmdMFtwSYGc = kBOilrfmQspwwsLlQucgVePHzaAKA.NjNjryiutAUEterrCvmdMFtwSYGc;
				int num = fLcZuTpMOwYPWmGZCMQAZMEBzxNc2.RArIgKvfketfmjSIMPRkYKyyEMJJA();
				for (int i = 0; i < num; i++)
				{
					fLcZuTpMOwYPWmGZCMQAZMEBzxNc<_0001, _0002>.afpFMKpOFhEzEBdDrNcunaaeluicA afpFMKpOFhEzEBdDrNcunaaeluicA = fLcZuTpMOwYPWmGZCMQAZMEBzxNc2.aDdPXHhAYJFMppzvBBrJXYrRwLSs(i);
					_0001 tFffYazGHMOVnLpOXAoTjAaUfvCi = afpFMKpOFhEzEBdDrNcunaaeluicA.TFffYazGHMOVnLpOXAoTjAaUfvCi;
					if (!tFffYazGHMOVnLpOXAoTjAaUfvCi.enabled)
					{
						continue;
					}
					global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0002> ggWdqFddlhBZqeFBwCaEiHGCVWTqA = afpFMKpOFhEzEBdDrNcunaaeluicA.GgWdqFddlhBZqeFBwCaEiHGCVWTqA;
					bool flag = false;
					int num2 = ggWdqFddlhBZqeFBwCaEiHGCVWTqA.GeYULXtFpYgsaXqXZIDwegpjOMgW();
					for (int j = 0; j < num2; j++)
					{
						_0002 val = ggWdqFddlhBZqeFBwCaEiHGCVWTqA.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(j);
						if (!val.enabled)
						{
							continue;
						}
						AList<ActionElementMap> aList = val.bluHTIHZzVOouyueNmhnQDqXANlH;
						if (aList != null)
						{
							int count = aList._count;
							for (int k = 0; k < count; k++)
							{
								ActionElementMap actionElementMap = aList._items[k];
								if (!actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA || actionElementMap._elementType != ControllerElementType.Axis)
								{
									continue;
								}
								int actionId = actionElementMap._actionId;
								if (!tFffYazGHMOVnLpOXAoTjAaUfvCi.xYieDEgzHjpAALzGGAlrgVIEtdXHB(actionElementMap, actionId, false, false, out var num3))
								{
									continue;
								}
								if (num3 == 0f)
								{
									tFffYazGHMOVnLpOXAoTjAaUfvCi.xYieDEgzHjpAALzGGAlrgVIEtdXHB(actionElementMap, actionId, false, true, out var num4);
									if (num4 == 0f)
									{
										P_1(arg1: false, VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId);
										continue;
									}
								}
								njNjryiutAUEterrCvmdMFtwSYGc.qYHCHUUoOfAWcggXJJyAvBfgOuQzA = num3;
								njNjryiutAUEterrCvmdMFtwSYGc.dETBOxiMUQgZgPWxRvIkMzjzDSSO = tFffYazGHMOVnLpOXAoTjAaUfvCi;
								njNjryiutAUEterrCvmdMFtwSYGc.JDGBgXPVMDVUqFoYrFTwdkmFwdty = P_0;
								njNjryiutAUEterrCvmdMFtwSYGc.bUScxzgFvNwAwsaDaAgXaHxyciQHA = ControllerElementType.Axis;
								njNjryiutAUEterrCvmdMFtwSYGc.lJuSuVYfkbBfxQxkTnPCOPsqeqmN = actionElementMap;
								njNjryiutAUEterrCvmdMFtwSYGc.msrNRqwsDjCYqTbjaFOmiKuFHiNl = val;
								njNjryiutAUEterrCvmdMFtwSYGc.PWuJoHkuXhJbNSKrnlqVhyYROSlc = tFffYazGHMOVnLpOXAoTjAaUfvCi.calibrationMap.Axes[actionElementMap.uxemeTqImFAncCLpTkOkfOWaWKUK].applyRangeCalibration;
								njNjryiutAUEterrCvmdMFtwSYGc.rMZgpEQqPgUbVxUUelignejLxGDg = tFffYazGHMOVnLpOXAoTjAaUfvCi.Axes[actionElementMap.elementIndex].dyBDSFNRWhEclaVHduPpTilGwQgN?._dataFormat ?? AxisCoordinateMode.Absolute;
								P_1(arg1: true, VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId);
								flag = true;
							}
						}
						AList<ActionElementMap> aList2 = val.LrIuPDSCedgUWTQghItEJWRAaExrA;
						if (aList2 != null)
						{
							int count2 = aList2._count;
							for (int l = 0; l < count2; l++)
							{
								ActionElementMap actionElementMap2 = aList2._items[l];
								if (!actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA || actionElementMap2._elementType != ControllerElementType.Button)
								{
									continue;
								}
								int actionId2 = actionElementMap2._actionId;
								float qYHCHUUoOfAWcggXJJyAvBfgOuQzA = 0f;
								int uxemeTqImFAncCLpTkOkfOWaWKUK = actionElementMap2.uxemeTqImFAncCLpTkOkfOWaWKUK;
								if (!bVtnWlCxnIUiDWxgLwUREKyLaMmf(tFffYazGHMOVnLpOXAoTjAaUfvCi, i, uxemeTqImFAncCLpTkOkfOWaWKUK, actionElementMap2, ggWdqFddlhBZqeFBwCaEiHGCVWTqA, actionId2, ref qYHCHUUoOfAWcggXJJyAvBfgOuQzA) && !tFffYazGHMOVnLpOXAoTjAaUfvCi.BLbebkNyKNYgVESpoLfsIwKqgdIj(actionElementMap2, actionId2, out qYHCHUUoOfAWcggXJJyAvBfgOuQzA, out njNjryiutAUEterrCvmdMFtwSYGc.zdzDmjIWBKsoyhDwpGsHUxPSXTUh))
								{
									continue;
								}
								ButtonStateFlags buttonStateFlags = tFffYazGHMOVnLpOXAoTjAaUfvCi.faGHnbatZKmYeuwMysYwtGBKNqobb(actionElementMap2.uxemeTqImFAncCLpTkOkfOWaWKUK);
								if (buttonStateFlags == ButtonStateFlags.Off)
								{
									P_1(arg1: false, VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId2);
									continue;
								}
								njNjryiutAUEterrCvmdMFtwSYGc.qYHCHUUoOfAWcggXJJyAvBfgOuQzA = qYHCHUUoOfAWcggXJJyAvBfgOuQzA;
								njNjryiutAUEterrCvmdMFtwSYGc.qDDMBBwlnkhbJkDUeoEGHQzUFYqab = buttonStateFlags;
								njNjryiutAUEterrCvmdMFtwSYGc.dETBOxiMUQgZgPWxRvIkMzjzDSSO = tFffYazGHMOVnLpOXAoTjAaUfvCi;
								njNjryiutAUEterrCvmdMFtwSYGc.JDGBgXPVMDVUqFoYrFTwdkmFwdty = P_0;
								njNjryiutAUEterrCvmdMFtwSYGc.bUScxzgFvNwAwsaDaAgXaHxyciQHA = ControllerElementType.Button;
								njNjryiutAUEterrCvmdMFtwSYGc.lJuSuVYfkbBfxQxkTnPCOPsqeqmN = actionElementMap2;
								njNjryiutAUEterrCvmdMFtwSYGc.msrNRqwsDjCYqTbjaFOmiKuFHiNl = val;
								if (njNjryiutAUEterrCvmdMFtwSYGc.PWuJoHkuXhJbNSKrnlqVhyYROSlc)
								{
									njNjryiutAUEterrCvmdMFtwSYGc.PWuJoHkuXhJbNSKrnlqVhyYROSlc = false;
								}
								P_1(arg1: true, VkOpbxhhZlsWxLzmWsTDhMjvNRzf.mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId2);
								flag = true;
							}
						}
						if (flag)
						{
							afpFMKpOFhEzEBdDrNcunaaeluicA.eEiBJvFIRTCjCLeGfjCifhpeLHnhA();
						}
					}
				}
			}

			private bool bVtnWlCxnIUiDWxgLwUREKyLaMmf<_0001>(ControllerWithAxes P_0, int P_1, int P_2, ActionElementMap P_3, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_4, int P_5, ref float P_6) where _0001 : ControllerMapWithAxes
			{
				if (!P_0.yZwGORAVRJPjNCmxxWIIoQgNomuqA.IsUnknownHatCardinal(P_2))
				{
					return false;
				}
				UnknownControllerHat.HatButtons unknownHatButtons = P_0.yZwGORAVRJPjNCmxxWIIoQgNomuqA.GetUnknownHatButtons(P_2);
				if (BCEFHtSqMrERYkpkHvXBOhWPmrag(unknownHatButtons, P_1, P_4))
				{
					unknownHatButtons.GetNeighbors(P_2, out var neighbor, out var neighbor2);
					if (P_0.GetButton(neighbor) || P_0.GetButton(neighbor2))
					{
						if (!P_0.zIVcpTEOiCkgBkmaYtBIcWKMePct(P_3, P_5, true, out P_6))
						{
							return false;
						}
						return true;
					}
				}
				return false;
			}

			private bool BCEFHtSqMrERYkpkHvXBOhWPmrag<_0001>(UnknownControllerHat.HatButtons P_0, int P_1, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_2) where _0001 : ControllerMapWithAxes
			{
				if (P_0 == null)
				{
					return false;
				}
				if (ReInput.configVars.force4WayHats)
				{
					return true;
				}
				if (dLPwCAJBpytSahizBJTdpAjUOFBl(P_0, P_1, P_2))
				{
					return false;
				}
				return true;
			}

			private bool dLPwCAJBpytSahizBJTdpAjUOFBl<_0001>(UnknownControllerHat.HatButtons P_0, int P_1, global::JXfmwiBdreTpaOqXNtMEJcOCaZSP<_0001> P_2) where _0001 : ControllerMapWithAxes
			{
				if (P_2 == null)
				{
					return false;
				}
				int num = P_2.GeYULXtFpYgsaXqXZIDwegpjOMgW();
				for (int i = 0; i < num; i++)
				{
					IList<ActionElementMap> buttonMaps = P_2.LXAtNOeXAkcDEyEDCFMJuBFLDZpT(i).ButtonMaps;
					if (buttonMaps == null)
					{
						continue;
					}
					int count = buttonMaps.Count;
					for (int j = 0; j < count; j++)
					{
						int uxemeTqImFAncCLpTkOkfOWaWKUK = buttonMaps[j].uxemeTqImFAncCLpTkOkfOWaWKUK;
						if (buttonMaps[j]._actionId >= 0 && P_0.IsCorner(uxemeTqImFAncCLpTkOkfOWaWKUK))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		private const string ipxaYblCWLZUrqQmfvInlkjPdOFEA = "player";

		private readonly NXTDUDBeDljECgTdktAoWNvFngijA XCFDVqZUbyGILGzarkLNUnbiMkmM;

		private bool vRMKMEVvXMhqEIXMwTOvIUjVSzti;

		private int mgTogZEAHwpJMhbsccjZDcKdOLwp;

		private string EmJDjMgHHrjVJdEKYBfcCITXjWKbA;

		private string lwkbUxawkZkntQyxjreqCdYVcpkF;

		private readonly string XwtwkRbBZoglybRpCVfQPPaNEEzrA;

		private bool JBrvNMrORFXbQfwylqrttGlaPHEj;

		private readonly int QFUtqbyrbpfoAROuRGDvtLrHaIwBA;

		private readonly uDMHJlZGqPMWeMVCxldavZLGKMck MNuVjsDsOmRWmdHwMtnGpgnmzLEP;

		private int XrImNHNZTMqTStPAbrfokCUwVGKn;

		public readonly ControllerHelper controllers;

		public int id
		{
			get
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
					return -1;
				}
				return mgTogZEAHwpJMhbsccjZDcKdOLwp;
			}
			internal set
			{
				mgTogZEAHwpJMhbsccjZDcKdOLwp = num;
			}
		}

		public string name
		{
			get
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
					return string.Empty;
				}
				return EmJDjMgHHrjVJdEKYBfcCITXjWKbA;
			}
			internal set
			{
				EmJDjMgHHrjVJdEKYBfcCITXjWKbA = emJDjMgHHrjVJdEKYBfcCITXjWKbA;
			}
		}

		public string descriptiveName
		{
			get
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
					return string.Empty;
				}
				if (!LocalizationManager.isEnabled)
				{
					return lwkbUxawkZkntQyxjreqCdYVcpkF;
				}
				return MNuVjsDsOmRWmdHwMtnGpgnmzLEP.MpfwJMTclVnnxEuHhBPCmlxJadkBA;
			}
			internal set
			{
				nonLocalizedDescriptiveName = text;
			}
		}

		public bool isPlaying
		{
			get
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
					return false;
				}
				return JBrvNMrORFXbQfwylqrttGlaPHEj;
			}
			set
			{
				JBrvNMrORFXbQfwylqrttGlaPHEj = value;
			}
		}

		internal string nonLocalizedDescriptiveName
		{
			get
			{
				return lwkbUxawkZkntQyxjreqCdYVcpkF;
			}
			set
			{
				lwkbUxawkZkntQyxjreqCdYVcpkF = value;
				MNuVjsDsOmRWmdHwMtnGpgnmzLEP.YSgvMmquHVoFhixWnSsVWmcflge();
			}
		}

		string bguKJVtsagJfXPpJQeurpzlOLIYd.keyCategory => "player";

		string bguKJVtsagJfXPpJQeurpzlOLIYd.scriptingName => EmJDjMgHHrjVJdEKYBfcCITXjWKbA;

		string bguKJVtsagJfXPpJQeurpzlOLIYd.nonLocalizedDescriptiveName
		{
			get
			{
				return lwkbUxawkZkntQyxjreqCdYVcpkF;
			}
			set
			{
				lwkbUxawkZkntQyxjreqCdYVcpkF = value;
			}
		}

		string bguKJVtsagJfXPpJQeurpzlOLIYd.key => XwtwkRbBZoglybRpCVfQPPaNEEzrA;

		int bguKJVtsagJfXPpJQeurpzlOLIYd.autoGeneratedValueFlags
		{
			get
			{
				return XrImNHNZTMqTStPAbrfokCUwVGKn;
			}
			set
			{
				XrImNHNZTMqTStPAbrfokCUwVGKn = value;
			}
		}

		internal Player(bool P_0, int P_1, string P_2, string P_3, string P_4, EhUOIcAWtPzjNpBYLAHyNaaERzaE P_5, ControllerMapLayoutManager.ICvEqTkajGQZLXkOyPTomcjaeCWs P_6, ControllerMapEnabler.CyXCJRXKNwVfGTMPVLIRUynNmCKI P_7)
		{
			vRMKMEVvXMhqEIXMwTOvIUjVSzti = P_0;
			mgTogZEAHwpJMhbsccjZDcKdOLwp = P_1;
			EmJDjMgHHrjVJdEKYBfcCITXjWKbA = P_2;
			lwkbUxawkZkntQyxjreqCdYVcpkF = P_3;
			XwtwkRbBZoglybRpCVfQPPaNEEzrA = P_4;
			QFUtqbyrbpfoAROuRGDvtLrHaIwBA = ReInput.id;
			MNuVjsDsOmRWmdHwMtnGpgnmzLEP = uDMHJlZGqPMWeMVCxldavZLGKMck.KoYuvhvHwZHOiMEpyUHHifMSYsLO(this);
			controllers = new ControllerHelper(this, P_5, P_6, P_7);
			XCFDVqZUbyGILGzarkLNUnbiMkmM = ReInput.FoarDfUMCtoVFquEtrllUhEjZUUn;
			GOGfMeanvsKaJIAqUutWylvnOgKzA();
		}

		public PlayerSaveData GetSaveData(bool userAssignableMapsOnly)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return default(PlayerSaveData);
			}
			return new PlayerSaveData(controllers.maps.GetAllMapSaveData<JoystickMapSaveData>(userAssignableMapsOnly), controllers.maps.GetAllMapSaveData<KeyboardMapSaveData>(userAssignableMapsOnly), controllers.maps.GetAllMapSaveData<MouseMapSaveData>(userAssignableMapsOnly), controllers.maps.GetAllMapSaveData<CustomControllerMapSaveData>(userAssignableMapsOnly), ReInput.mapping.GetInputBehaviors(mgTogZEAHwpJMhbsccjZDcKdOLwp));
		}

		public bool GetButton(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.BRgJAcEyxDdRJtEmfVewtjEoYNqt() ?? false;
		}

		public bool GetButton(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.BRgJAcEyxDdRJtEmfVewtjEoYNqt() ?? false;
		}

		public bool GetButtonDown(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.ulNzhMMaXtAXOxcBFAnWyCDHeqoN() ?? false;
		}

		public bool GetButtonDown(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.ulNzhMMaXtAXOxcBFAnWyCDHeqoN() ?? false;
		}

		public bool GetButtonUp(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.aOLFHKiGReYtLuVqzyyHLHbfKQYab() ?? false;
		}

		public bool GetButtonUp(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.aOLFHKiGReYtLuVqzyyHLHbfKQYab() ?? false;
		}

		public bool GetButtonPrev(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.ooKSDMuVHpVciXXBbIhumspcSpRM() ?? false;
		}

		public bool GetButtonPrev(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.ooKSDMuVHpVciXXBbIhumspcSpRM() ?? false;
		}

		public bool GetButtonSinglePressHold(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.ZbWxWKzrywKLPNgJCRBudWaWVnQo() ?? false;
		}

		public bool GetButtonSinglePressHold(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.ZbWxWKzrywKLPNgJCRBudWaWVnQo() ?? false;
		}

		public bool GetButtonSinglePressDown(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.DzHscFSLxdVERLjLMZVUWfQUTaSF() ?? false;
		}

		public bool GetButtonSinglePressDown(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.DzHscFSLxdVERLjLMZVUWfQUTaSF() ?? false;
		}

		public bool GetButtonSinglePressUp(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.LZPEdseAeYHhrEFJMyIMBhfZCwvn() ?? false;
		}

		public bool GetButtonSinglePressUp(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.LZPEdseAeYHhrEFJMyIMBhfZCwvn() ?? false;
		}

		public bool GetButtonDoublePressHold(string actionName, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.eSRbSlRIiNAqSZHieyTWJIkiWXff(speed) ?? false;
		}

		public bool GetButtonDoublePressHold(int actionId, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.eSRbSlRIiNAqSZHieyTWJIkiWXff(speed) ?? false;
		}

		public bool GetButtonDoublePressHold(string actionName)
		{
			return GetButtonDoublePressHold(actionName, 0f);
		}

		public bool GetButtonDoublePressHold(int actionId)
		{
			return GetButtonDoublePressHold(actionId, 0f);
		}

		public bool GetButtonDoublePressDown(string actionName, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.tchZIqxqMcltzqTiihcJYKwuaAei(speed) ?? false;
		}

		public bool GetButtonDoublePressDown(int actionId, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.tchZIqxqMcltzqTiihcJYKwuaAei(speed) ?? false;
		}

		public bool GetButtonDoublePressDown(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return GetButtonDoublePressDown(actionName, 0f);
		}

		public bool GetButtonDoublePressDown(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return GetButtonDoublePressDown(actionId, 0f);
		}

		public bool GetButtonDoublePressUp(string actionName, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.VeFehoBAikgOZbeXFHZCDKEIPZGKC(speed) ?? false;
		}

		public bool GetButtonDoublePressUp(int actionId, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.VeFehoBAikgOZbeXFHZCDKEIPZGKC(speed) ?? false;
		}

		public bool GetButtonDoublePressUp(string actionName)
		{
			return GetButtonDoublePressUp(actionName, 0f);
		}

		public bool GetButtonDoublePressUp(int actionId)
		{
			return GetButtonDoublePressUp(actionId, 0f);
		}

		public bool GetButtonTimedPress(string actionName, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.jTXtRIsJnPihrseaUxpjqKnCOoPC(time, 0f) ?? false;
		}

		public bool GetButtonTimedPress(int actionId, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.jTXtRIsJnPihrseaUxpjqKnCOoPC(time, 0f) ?? false;
		}

		public bool GetButtonTimedPress(string actionName, float time, float expireIn)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.jTXtRIsJnPihrseaUxpjqKnCOoPC(time, expireIn) ?? false;
		}

		public bool GetButtonTimedPress(int actionId, float time, float expireIn)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.jTXtRIsJnPihrseaUxpjqKnCOoPC(time, expireIn) ?? false;
		}

		public bool GetButtonTimedPressDown(string actionName, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.kBBCZOnptttxaQSaLliSLAyVIslL(time) ?? false;
		}

		public bool GetButtonTimedPressDown(int actionId, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.kBBCZOnptttxaQSaLliSLAyVIslL(time) ?? false;
		}

		public bool GetButtonTimedPressUp(string actionName, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.RCOCLdDZYLOfkVVZPPcNmYplMPqF(time, 0f) ?? false;
		}

		public bool GetButtonTimedPressUp(int actionId, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.RCOCLdDZYLOfkVVZPPcNmYplMPqF(time, 0f) ?? false;
		}

		public bool GetButtonTimedPressUp(string actionName, float time, float expireIn)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.RCOCLdDZYLOfkVVZPPcNmYplMPqF(time, expireIn) ?? false;
		}

		public bool GetButtonTimedPressUp(int actionId, float time, float expireIn)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.RCOCLdDZYLOfkVVZPPcNmYplMPqF(time, expireIn) ?? false;
		}

		public bool GetButtonShortPress(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.FIsgvKBIeQboRaWNaSyVaektTMzzA() ?? false;
		}

		public bool GetButtonShortPress(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.FIsgvKBIeQboRaWNaSyVaektTMzzA() ?? false;
		}

		public bool GetButtonShortPressDown(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.JNUXlbFKdcnOumpOjdJBlERkDRZCA() ?? false;
		}

		public bool GetButtonShortPressDown(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.JNUXlbFKdcnOumpOjdJBlERkDRZCA() ?? false;
		}

		public bool GetButtonShortPressUp(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.lekaYDUvsTYCOiLPOfTfzDVLfDuc() ?? false;
		}

		public bool GetButtonShortPressUp(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.lekaYDUvsTYCOiLPOfTfzDVLfDuc() ?? false;
		}

		public bool GetButtonLongPress(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.UsNLWxwEABEtvLvDfRUDvXVPnCdx() ?? false;
		}

		public bool GetButtonLongPress(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.UsNLWxwEABEtvLvDfRUDvXVPnCdx() ?? false;
		}

		public bool GetButtonLongPressDown(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.gOpFTmMabCeWBnCmcrsVKBMyxMQF() ?? false;
		}

		public bool GetButtonLongPressDown(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.gOpFTmMabCeWBnCmcrsVKBMyxMQF() ?? false;
		}

		public bool GetButtonLongPressUp(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.jWCwyVMAyGNFusPDQDolSrygEocX() ?? false;
		}

		public bool GetButtonLongPressUp(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.jWCwyVMAyGNFusPDQDolSrygEocX() ?? false;
		}

		public bool GetButtonRepeating(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.luHCfXeDrbtNBOhpwjjGImBAgXznA() ?? false;
		}

		public bool GetButtonRepeating(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.luHCfXeDrbtNBOhpwjjGImBAgXznA() ?? false;
		}

		public bool GetAnyButton()
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.dOWihLBVqbhasnWxUedOOgLvhhVq(mgTogZEAHwpJMhbsccjZDcKdOLwp);
		}

		public bool GetAnyButtonDown()
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.dvuVTLKANvgVhLQedePpWnZXhoEH(mgTogZEAHwpJMhbsccjZDcKdOLwp);
		}

		public bool GetAnyButtonUp()
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.mcYMPIxhRtIeYNfHJLoaJqOkHYyh(mgTogZEAHwpJMhbsccjZDcKdOLwp);
		}

		public bool GetAnyButtonPrev()
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.axUBQajEqZhmhvhoGZWqojMdEKLG(mgTogZEAHwpJMhbsccjZDcKdOLwp);
		}

		public double GetButtonTimePressed(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.eQYIpHgMDFhqqRZhPeGNYPzxXVnc() ?? 0.0;
		}

		public double GetButtonTimePressed(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.eQYIpHgMDFhqqRZhPeGNYPzxXVnc() ?? 0.0;
		}

		public double GetButtonTimeUnpressed(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.DEkfVfoTkufJnSXSodctvAYanAfg() ?? 0.0;
		}

		public double GetButtonTimeUnpressed(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.DEkfVfoTkufJnSXSodctvAYanAfg() ?? 0.0;
		}

		public bool GetNegativeButton(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.VRiBjNoimKLqMJcOigBnTGHHrHub() ?? false;
		}

		public bool GetNegativeButton(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.VRiBjNoimKLqMJcOigBnTGHHrHub() ?? false;
		}

		public bool GetNegativeButtonDown(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.VYSikZzuXqPelkspEaGEPYtFZADJ() ?? false;
		}

		public bool GetNegativeButtonDown(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.VYSikZzuXqPelkspEaGEPYtFZADJ() ?? false;
		}

		public bool GetNegativeButtonUp(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.KOICBttxgehAkePueGwrcVfAlxWCb() ?? false;
		}

		public bool GetNegativeButtonUp(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.KOICBttxgehAkePueGwrcVfAlxWCb() ?? false;
		}

		public bool GetNegativeButtonPrev(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.tnXlVtxyaxIfLqFVXAJLDtiGTaUf() ?? false;
		}

		public bool GetNegativeButtonPrev(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.tnXlVtxyaxIfLqFVXAJLDtiGTaUf() ?? false;
		}

		public bool GetNegativeButtonSinglePressHold(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.ofLgjxbAiLFczqfdeXZQnDffqjaAb() ?? false;
		}

		public bool GetNegativeButtonSinglePressHold(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.ofLgjxbAiLFczqfdeXZQnDffqjaAb() ?? false;
		}

		public bool GetNegativeButtonSinglePressDown(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.WuHTXaSrrQTzEyeAyWMAMFzZWqGR() ?? false;
		}

		public bool GetNegativeButtonSinglePressDown(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.WuHTXaSrrQTzEyeAyWMAMFzZWqGR() ?? false;
		}

		public bool GetNegativeButtonSinglePressUp(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.SyxgdBgGAcWsYgijIiZkjXGkreFJA() ?? false;
		}

		public bool GetNegativeButtonSinglePressUp(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.SyxgdBgGAcWsYgijIiZkjXGkreFJA() ?? false;
		}

		public bool GetNegativeButtonDoublePressHold(string actionName, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.VosydDNNmgXAqxlxaeIqKRirlQgdA(speed) ?? false;
		}

		public bool GetNegativeButtonDoublePressHold(int actionId, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.VosydDNNmgXAqxlxaeIqKRirlQgdA(speed) ?? false;
		}

		public bool GetNegativeButtonDoublePressHold(string actionName)
		{
			return GetNegativeButtonDoublePressHold(actionName, 0f);
		}

		public bool GetNegativeButtonDoublePressHold(int actionId)
		{
			return GetNegativeButtonDoublePressHold(actionId, 0f);
		}

		public bool GetNegativeButtonDoublePressDown(string actionName, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.ZnwZnJXiZrPcerRlvjBWaMetofsY(speed) ?? false;
		}

		public bool GetNegativeButtonDoublePressDown(int actionId, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.ZnwZnJXiZrPcerRlvjBWaMetofsY(speed) ?? false;
		}

		public bool GetNegativeButtonDoublePressDown(string actionName)
		{
			return GetNegativeButtonDoublePressDown(actionName, 0f);
		}

		public bool GetNegativeButtonDoublePressDown(int actionId)
		{
			return GetNegativeButtonDoublePressDown(actionId, 0f);
		}

		public bool GetNegativeButtonDoublePressUp(string actionName, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.xehengRSBDHzLfImHDBStcbEAmlIb(speed) ?? false;
		}

		public bool GetNegativeButtonDoublePressUp(int actionId, float speed)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.xehengRSBDHzLfImHDBStcbEAmlIb(speed) ?? false;
		}

		public bool GetNegativeButtonDoublePressUp(string actionName)
		{
			return GetNegativeButtonDoublePressUp(actionName, 0f);
		}

		public bool GetNegativeButtonDoublePressUp(int actionId)
		{
			return GetNegativeButtonDoublePressUp(actionId, 0f);
		}

		public bool GetNegativeButtonTimedPress(string actionName, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.DpGjKMVCONaAXZmoEjjceWwJoSVj(time, 0f) ?? false;
		}

		public bool GetNegativeButtonTimedPress(int actionId, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.DpGjKMVCONaAXZmoEjjceWwJoSVj(time, 0f) ?? false;
		}

		public bool GetNegativeButtonTimedPress(string actionName, float time, float expireIn)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.DpGjKMVCONaAXZmoEjjceWwJoSVj(time, expireIn) ?? false;
		}

		public bool GetNegativeButtonTimedPress(int actionId, float time, float expireIn)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.DpGjKMVCONaAXZmoEjjceWwJoSVj(time, expireIn) ?? false;
		}

		public bool GetNegativeButtonTimedPressDown(string actionName, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.kEjagsDgZituqfuLKPjZGOHbrQSIA(time) ?? false;
		}

		public bool GetNegativeButtonTimedPressDown(int actionId, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.kEjagsDgZituqfuLKPjZGOHbrQSIA(time) ?? false;
		}

		public bool GetNegativeButtonTimedPressUp(string actionName, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.LVqGPOKOkUJddoPzWyZInGbDACPXA(time, 0f) ?? false;
		}

		public bool GetNegativeButtonTimedPressUp(int actionId, float time)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.LVqGPOKOkUJddoPzWyZInGbDACPXA(time, 0f) ?? false;
		}

		public bool GetNegativeButtonTimedPressUp(string actionName, float time, float expireIn)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.LVqGPOKOkUJddoPzWyZInGbDACPXA(time, expireIn) ?? false;
		}

		public bool GetNegativeButtonTimedPressUp(int actionId, float time, float expireIn)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.LVqGPOKOkUJddoPzWyZInGbDACPXA(time, expireIn) ?? false;
		}

		public bool GetNegativeButtonShortPress(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.oMTJrgKTbTdWHoFmJuSgxgShPrm() ?? false;
		}

		public bool GetNegativeButtonShortPress(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.oMTJrgKTbTdWHoFmJuSgxgShPrm() ?? false;
		}

		public bool GetNegativeButtonShortPressDown(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.kslciTTidvoMyIZgtSTdFtuZsUoP() ?? false;
		}

		public bool GetNegativeButtonShortPressDown(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.kslciTTidvoMyIZgtSTdFtuZsUoP() ?? false;
		}

		public bool GetNegativeButtonShortPressUp(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.LDMsGESyNgOlMulQqSikAlFjYnii() ?? false;
		}

		public bool GetNegativeButtonShortPressUp(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.LDMsGESyNgOlMulQqSikAlFjYnii() ?? false;
		}

		public bool GetNegativeButtonLongPress(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.QnupUpitKImkVGNVIPtpBxzUcDXf() ?? false;
		}

		public bool GetNegativeButtonLongPress(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.QnupUpitKImkVGNVIPtpBxzUcDXf() ?? false;
		}

		public bool GetNegativeButtonLongPressDown(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.shokBLMVDXLmddfYxVRvxaLiVrib() ?? false;
		}

		public bool GetNegativeButtonLongPressDown(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.shokBLMVDXLmddfYxVRvxaLiVrib() ?? false;
		}

		public bool GetNegativeButtonLongPressUp(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.BMXFPjDOFTDTTfHBGMkTmqmNPqEX() ?? false;
		}

		public bool GetNegativeButtonLongPressUp(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.BMXFPjDOFTDTTfHBGMkTmqmNPqEX() ?? false;
		}

		public bool GetNegativeButtonRepeating(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.uakFyBehSWuTSJarHLAsQDuocrmd() ?? false;
		}

		public bool GetNegativeButtonRepeating(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.uakFyBehSWuTSJarHLAsQDuocrmd() ?? false;
		}

		public bool GetAnyNegativeButton()
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.wQLEuWyfJafvNqoWDFzeSidJbbMW(mgTogZEAHwpJMhbsccjZDcKdOLwp);
		}

		public bool GetAnyNegativeButtonDown()
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.ndmftXNSUwiLKVuQmztrMrsknIAj(mgTogZEAHwpJMhbsccjZDcKdOLwp);
		}

		public bool GetAnyNegativeButtonUp()
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.vOGOUBImYulJQCmtkfSujuqdRRfGA(mgTogZEAHwpJMhbsccjZDcKdOLwp);
		}

		public bool GetAnyNegativeButtonPrev()
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.SfZAmeeXiCsbCpACbjdIjcditpuv(mgTogZEAHwpJMhbsccjZDcKdOLwp);
		}

		public double GetNegativeButtonTimePressed(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.uKZGxIdxHqFkqZipNXnmUkllIwSn() ?? 0.0;
		}

		public double GetNegativeButtonTimePressed(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.uKZGxIdxHqFkqZipNXnmUkllIwSn() ?? 0.0;
		}

		public double GetNegativeButtonTimeUnpressed(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.OlOkRsACaykIHFyuncCFeVAojcvN() ?? 0.0;
		}

		public double GetNegativeButtonTimeUnpressed(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.OlOkRsACaykIHFyuncCFeVAojcvN() ?? 0.0;
		}

		public float GetAxis(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.RvRTgrdjWTZRmjoKbsOcGREaiXDi() ?? 0f;
		}

		public float GetAxis(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.RvRTgrdjWTZRmjoKbsOcGREaiXDi() ?? 0f;
		}

		public float GetAxisRaw(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.IWvqBQApuMVSUeHFsYcOZGosazGeA() ?? 0f;
		}

		public float GetAxisRaw(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.IWvqBQApuMVSUeHFsYcOZGosazGeA() ?? 0f;
		}

		public float GetAxisPrev(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.lywVsHFwvRHDyAOYaAHUvBjHQYpD() ?? 0f;
		}

		public float GetAxisPrev(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.lywVsHFwvRHDyAOYaAHUvBjHQYpD() ?? 0f;
		}

		public float GetAxisRawPrev(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.RfXcmkCBFuBWUxLdultQzbgNZpmN() ?? 0f;
		}

		public float GetAxisRawPrev(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.RfXcmkCBFuBWUxLdultQzbgNZpmN() ?? 0f;
		}

		public float GetAxisDelta(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.BACwvsUbbeKukgnoPhsIIDwRnnqEA() ?? 0f;
		}

		public float GetAxisDelta(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.BACwvsUbbeKukgnoPhsIIDwRnnqEA() ?? 0f;
		}

		public float GetAxisRawDelta(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.EcPdvFDjVKZgYaUueGlcfyniybkMA() ?? 0f;
		}

		public float GetAxisRawDelta(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.EcPdvFDjVKZgYaUueGlcfyniybkMA() ?? 0f;
		}

		public Vector2 GetAxis2D(string xAxisActionName, string yAxisActionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return Vector2.zero;
			}
			Vector2 result = default(Vector2);
			kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, xAxisActionName, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.x = kBOilrfmQspwwsLlQucgVePHzaAKA2.RvRTgrdjWTZRmjoKbsOcGREaiXDi();
			}
			kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, yAxisActionName, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.y = kBOilrfmQspwwsLlQucgVePHzaAKA2.RvRTgrdjWTZRmjoKbsOcGREaiXDi();
			}
			return result;
		}

		public Vector2 GetAxis2D(int xAxisActionId, int yAxisActionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return Vector2.zero;
			}
			Vector2 result = default(Vector2);
			kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, xAxisActionId, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.x = kBOilrfmQspwwsLlQucgVePHzaAKA2.RvRTgrdjWTZRmjoKbsOcGREaiXDi();
			}
			kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, yAxisActionId, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.y = kBOilrfmQspwwsLlQucgVePHzaAKA2.RvRTgrdjWTZRmjoKbsOcGREaiXDi();
			}
			return result;
		}

		public Vector2 GetAxis2DPrev(string xAxisActionName, string yAxisActionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return Vector2.zero;
			}
			Vector2 result = default(Vector2);
			kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, xAxisActionName, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.x = kBOilrfmQspwwsLlQucgVePHzaAKA2.lywVsHFwvRHDyAOYaAHUvBjHQYpD();
			}
			kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, yAxisActionName, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.y = kBOilrfmQspwwsLlQucgVePHzaAKA2.lywVsHFwvRHDyAOYaAHUvBjHQYpD();
			}
			return result;
		}

		public Vector2 GetAxis2DPrev(int xAxisActionId, int yAxisActionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return Vector2.zero;
			}
			Vector2 result = default(Vector2);
			kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, xAxisActionId, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.x = kBOilrfmQspwwsLlQucgVePHzaAKA2.lywVsHFwvRHDyAOYaAHUvBjHQYpD();
			}
			kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, yAxisActionId, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.y = kBOilrfmQspwwsLlQucgVePHzaAKA2.lywVsHFwvRHDyAOYaAHUvBjHQYpD();
			}
			return result;
		}

		public Vector2 GetAxis2DRaw(string xAxisActionName, string yAxisActionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return Vector2.zero;
			}
			Vector2 result = default(Vector2);
			kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, xAxisActionName, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.x = kBOilrfmQspwwsLlQucgVePHzaAKA2.IWvqBQApuMVSUeHFsYcOZGosazGeA();
			}
			kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, yAxisActionName, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.y = kBOilrfmQspwwsLlQucgVePHzaAKA2.IWvqBQApuMVSUeHFsYcOZGosazGeA();
			}
			return result;
		}

		public Vector2 GetAxis2DRaw(int xAxisActionId, int yAxisActionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return Vector2.zero;
			}
			Vector2 result = default(Vector2);
			kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, xAxisActionId, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.x = kBOilrfmQspwwsLlQucgVePHzaAKA2.IWvqBQApuMVSUeHFsYcOZGosazGeA();
			}
			kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, yAxisActionId, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.y = kBOilrfmQspwwsLlQucgVePHzaAKA2.IWvqBQApuMVSUeHFsYcOZGosazGeA();
			}
			return result;
		}

		public Vector2 GetAxis2DRawPrev(string xAxisActionName, string yAxisActionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return Vector2.zero;
			}
			Vector2 result = default(Vector2);
			kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, xAxisActionName, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.x = kBOilrfmQspwwsLlQucgVePHzaAKA2.RfXcmkCBFuBWUxLdultQzbgNZpmN();
			}
			kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, yAxisActionName, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.y = kBOilrfmQspwwsLlQucgVePHzaAKA2.RfXcmkCBFuBWUxLdultQzbgNZpmN();
			}
			return result;
		}

		public Vector2 GetAxis2DRawPrev(int xAxisActionId, int yAxisActionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return Vector2.zero;
			}
			Vector2 result = default(Vector2);
			kBOilrfmQspwwsLlQucgVePHzaAKA kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, xAxisActionId, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.x = kBOilrfmQspwwsLlQucgVePHzaAKA2.RfXcmkCBFuBWUxLdultQzbgNZpmN();
			}
			kBOilrfmQspwwsLlQucgVePHzaAKA2 = XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, yAxisActionId, true);
			if (kBOilrfmQspwwsLlQucgVePHzaAKA2 != null)
			{
				result.y = kBOilrfmQspwwsLlQucgVePHzaAKA2.RfXcmkCBFuBWUxLdultQzbgNZpmN();
			}
			return result;
		}

		public double GetAxisTimeActive(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.SrhbCmEKNSsPfzfldsYwwlVRjIPw() ?? 0.0;
		}

		public double GetAxisTimeActive(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.SrhbCmEKNSsPfzfldsYwwlVRjIPw() ?? 0.0;
		}

		public double GetAxisTimeInactive(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.YRlmdmsBYvPRMgsSPGzadiYsBgND() ?? 0.0;
		}

		public double GetAxisTimeInactive(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.YRlmdmsBYvPRMgsSPGzadiYsBgND() ?? 0.0;
		}

		public double GetAxisRawTimeActive(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.LIGFjSCuufpsUrrlkvEjDRcJaftQA() ?? 0.0;
		}

		public double GetAxisRawTimeActive(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.LIGFjSCuufpsUrrlkvEjDRcJaftQA() ?? 0.0;
		}

		public double GetAxisRawTimeInactive(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.ymaDYEBuDUjLBukOIukrtcbLmuCS() ?? 0.0;
		}

		public double GetAxisRawTimeInactive(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0.0;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.ymaDYEBuDUjLBukOIukrtcbLmuCS() ?? 0.0;
		}

		public AxisCoordinateMode GetAxisCoordinateMode(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return AxisCoordinateMode.Absolute;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.SWVAbFKGBmrkqcRaBxMpGpAuomJu() ?? AxisCoordinateMode.Absolute;
		}

		public AxisCoordinateMode GetAxisCoordinateMode(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return AxisCoordinateMode.Absolute;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.SWVAbFKGBmrkqcRaBxMpGpAuomJu() ?? AxisCoordinateMode.Absolute;
		}

		public AxisCoordinateMode GetAxisRawCoordinateMode(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return AxisCoordinateMode.Absolute;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.mNVutZbAuNuIbUVTXQAfvbUOtHuT() ?? AxisCoordinateMode.Absolute;
		}

		public AxisCoordinateMode GetAxisRawCoordinateMode(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return AxisCoordinateMode.Absolute;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.mNVutZbAuNuIbUVTXQAfvbUOtHuT() ?? AxisCoordinateMode.Absolute;
		}

		public AxisCoordinateMode GetAxisCoordinateModePrev(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return AxisCoordinateMode.Absolute;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.MsrrdOZikLJjhWrbptjAYjOtAZQR() ?? AxisCoordinateMode.Absolute;
		}

		public AxisCoordinateMode GetAxisCoordinateModePrev(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return AxisCoordinateMode.Absolute;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.MsrrdOZikLJjhWrbptjAYjOtAZQR() ?? AxisCoordinateMode.Absolute;
		}

		public AxisCoordinateMode GetAxisRawCoordinateModePrev(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return AxisCoordinateMode.Absolute;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.aFCUOxEQfRrVLIItkuCjbBMJxSIX() ?? AxisCoordinateMode.Absolute;
		}

		public AxisCoordinateMode GetAxisRawCoordinateModePrev(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return AxisCoordinateMode.Absolute;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.aFCUOxEQfRrVLIItkuCjbBMJxSIX() ?? AxisCoordinateMode.Absolute;
		}

		public IList<InputActionSourceData> GetCurrentInputSources(string actionName)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return EmptyObjects<InputActionSourceData>.EmptyReadOnlyIListT;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.hlzGvsjjwSlGrgcCDkJICfVVspVf();
		}

		public IList<InputActionSourceData> GetCurrentInputSources(int actionId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return EmptyObjects<InputActionSourceData>.EmptyReadOnlyIListT;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.hlzGvsjjwSlGrgcCDkJICfVVspVf();
		}

		public bool IsCurrentInputSource(string actionName, ControllerType controllerType)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.eGhJyWdvqDhIKIDlJfQLHrEKWHdq(controllerType) ?? false;
		}

		public bool IsCurrentInputSource(int actionId, ControllerType controllerType)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.eGhJyWdvqDhIKIDlJfQLHrEKWHdq(controllerType) ?? false;
		}

		public bool IsCurrentInputSource(string actionName, ControllerType controllerType, int controllerId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.iiraDRzsYkgmogHACIatHsoAhRUKc(controllerType, controllerId) ?? false;
		}

		public bool IsCurrentInputSource(int actionId, ControllerType controllerType, int controllerId)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.iiraDRzsYkgmogHACIatHsoAhRUKc(controllerType, controllerId) ?? false;
		}

		public bool IsCurrentInputSource(string actionName, Controller controller)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.hofxhLckICmzdddUmWnGeMyCMRVm(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionName, true)?.xHIjoijXjTBHMhIexUzlnrIsLruq(controller) ?? false;
		}

		public bool IsCurrentInputSource(int actionId, Controller controller)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return false;
			}
			return XCFDVqZUbyGILGzarkLNUnbiMkmM.FfgVHeyzXYOBalgpoIeyNyHpAHaO(mgTogZEAHwpJMhbsccjZDcKdOLwp, actionId, true)?.xHIjoijXjTBHMhIexUzlnrIsLruq(controller) ?? false;
		}

		public void AddInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.nvJlMegypofMmlmFYEsDwGUnCemY(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, updateLoop);
				}
			}
		}

		public void AddInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, int actionId)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.PHXuIjmHwdTPbdhoeqfeVmTeizJG(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, updateLoop, actionId);
				}
			}
		}

		public void AddInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, string actionName)
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return;
			}
			int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			if (num >= 0)
			{
				AddInputEventDelegate(callback, updateLoop, num);
			}
		}

		public void AddInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, InputActionEventType eventType)
		{
			AddInputEventDelegate(callback, updateLoop, eventType, (object[])null);
		}

		public void AddInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, InputActionEventType eventType, int actionId)
		{
			AddInputEventDelegate(callback, updateLoop, eventType, actionId, null);
		}

		public void AddInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, InputActionEventType eventType, string actionName)
		{
			AddInputEventDelegate(callback, updateLoop, eventType, actionName, null);
		}

		public void AddInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, InputActionEventType eventType, object[] arguments)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.NnaeZEblCldBQiyWCRhsFaLrGwU(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, updateLoop, eventType, arguments);
				}
			}
		}

		public void AddInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, InputActionEventType eventType, int actionId, object[] arguments)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.IKejPHmuKzHkCOQkYgfnOGyUgnJT(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, updateLoop, eventType, actionId, arguments);
				}
			}
		}

		public void AddInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, InputActionEventType eventType, string actionName, object[] arguments)
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return;
			}
			int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName, true);
			if (num >= 0)
			{
				AddInputEventDelegate(callback, updateLoop, eventType, num, arguments);
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.FKgiYbNkJHIQITZiFQdKUgsHcBhm(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback);
				}
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback, int actionId)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.LVMWrgHmqKvKmNKZQcFyajfLWXQw(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, actionId);
				}
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback, string actionName)
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return;
			}
			int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			if (num >= 0)
			{
				RemoveInputEventDelegate(callback, num);
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.rDCAMuHlglAoxLFUgqBGJvyKdzJJ(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, updateLoop);
				}
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback, InputActionEventType eventType)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.iUjCLpCkpwAMkxFUDUoNeajknXMy(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, eventType);
				}
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, int actionId)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.OuADhETpQnfqbOCDmUNwkNkuJGPP(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, updateLoop, actionId);
				}
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, string actionName)
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return;
			}
			int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			if (num >= 0)
			{
				RemoveInputEventDelegate(callback, updateLoop, num);
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback, InputActionEventType eventType, int actionId)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.yUbxkEEdrvcYADVfImSCiWfsAaDDA(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, eventType, actionId);
				}
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback, InputActionEventType eventType, string actionName)
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return;
			}
			int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			if (num >= 0)
			{
				RemoveInputEventDelegate(callback, eventType, num);
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, InputActionEventType eventType)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.cRMczdFKdAtFYIphgLBMqeufjlfsA(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, updateLoop, eventType);
				}
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, InputActionEventType eventType, int actionId)
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.mlVvArjbyBthhbBWEPpwDyRkhLHy(mgTogZEAHwpJMhbsccjZDcKdOLwp, callback, updateLoop, eventType, actionId);
				}
			}
		}

		public void RemoveInputEventDelegate(Action<InputActionEventData> callback, UpdateLoopType updateLoop, InputActionEventType eventType, string actionName)
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return;
			}
			int num = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			if (num >= 0)
			{
				RemoveInputEventDelegate(callback, updateLoop, eventType, num);
			}
		}

		public void ClearInputEventDelegates()
		{
			if (ReInput.isReady)
			{
				if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
				{
					ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				}
				else
				{
					XCFDVqZUbyGILGzarkLNUnbiMkmM.ARQGcvGEsYwHGQbeFaZtLOjBluWW(mgTogZEAHwpJMhbsccjZDcKdOLwp);
				}
			}
		}

		public void SetVibration(int motorIndex, float motorLevel)
		{
			SetVibration(motorIndex, motorLevel, 0f, stopOtherMotors: false);
		}

		public void SetVibration(int motorIndex, float motorLevel, float duration)
		{
			SetVibration(motorIndex, motorLevel, duration, stopOtherMotors: false);
		}

		public void SetVibration(int motorIndex, float motorLevel, bool stopOtherMotors)
		{
			SetVibration(motorIndex, motorLevel, 0f, stopOtherMotors);
		}

		public void SetVibration(int motorIndex, float motorLevel, float duration, bool stopOtherMotors)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return;
			}
			IList<Joystick> joysticks = controllers.Joysticks;
			int count = joysticks.Count;
			for (int i = 0; i < count; i++)
			{
				Joystick joystick = joysticks[i];
				if (joystick.supportsVibration)
				{
					joystick.SetVibration(motorIndex, motorLevel, duration, stopOtherMotors);
				}
			}
		}

		public float GetVibration(int motorIndex)
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return 0f;
			}
			IList<Joystick> joysticks = controllers.Joysticks;
			int count = joysticks.Count;
			float num = 0f;
			for (int i = 0; i < count; i++)
			{
				Joystick joystick = joysticks[i];
				if (joystick.supportsVibration)
				{
					num = MathTools.Max(joystick.GetVibration(motorIndex), num);
				}
			}
			return num;
		}

		public void StopVibration()
		{
			if (ReInput._id != QFUtqbyrbpfoAROuRGDvtLrHaIwBA)
			{
				ReInput.CheckInitialized(QFUtqbyrbpfoAROuRGDvtLrHaIwBA);
				return;
			}
			IList<Joystick> joysticks = controllers.Joysticks;
			int count = joysticks.Count;
			for (int i = 0; i < count; i++)
			{
				Joystick joystick = joysticks[i];
				if (joystick.supportsVibration)
				{
					joystick.StopVibration();
				}
			}
		}

		internal void ouNeOnoWDJCHwaMdiiQbWzaMoNZFA()
		{
			GOGfMeanvsKaJIAqUutWylvnOgKzA();
		}

		private void GOGfMeanvsKaJIAqUutWylvnOgKzA()
		{
			controllers.SSidKDjCaEnvidWxZZKyckkfwiPNA();
			JBrvNMrORFXbQfwylqrttGlaPHEj = false;
		}
	}
}
