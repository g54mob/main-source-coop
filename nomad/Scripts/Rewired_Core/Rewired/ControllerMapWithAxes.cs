using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired
{
	public abstract class ControllerMapWithAxes : ControllerMap
	{
		private sealed class zclcdDHeEZeYJbrBGjHSuJuLDiQXb : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
		{
			private int RQrgCObLsoVZPWbQJTyqWkjFjERt;

			private ActionElementMap olbzUieBQFuBMgJsuBBaoVFJYZzd;

			private int vSjLvJneIwcxSGkEqiVGGdZFLReqA;

			public ControllerMapWithAxes kcOEeXEWPOGSzBfCNIwQSFDTRYpAA;

			private int HrVbQWbAIfGBBnnHjbWGbPIIFKaQ;

			public int cNoGhiPMhTeipTXRdzvUvlknaYMY;

			private bool zGykiKcvYuvSAdwHQqpzvCxloRmh;

			public bool sMFGKyzvGdCLdQPATltEwfHVQCT;

			private IEnumerator<ActionElementMap> VfYApKjVbafKNHUkOdJmlSxspdQh;

			ActionElementMap IEnumerator<ActionElementMap>.Current
			{
				[DebuggerHidden]
				get
				{
					return olbzUieBQFuBMgJsuBBaoVFJYZzd;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return olbzUieBQFuBMgJsuBBaoVFJYZzd;
				}
			}

			[DebuggerHidden]
			public zclcdDHeEZeYJbrBGjHSuJuLDiQXb(int P_0)
			{
				RQrgCObLsoVZPWbQJTyqWkjFjERt = P_0;
				vSjLvJneIwcxSGkEqiVGGdZFLReqA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rQrgCObLsoVZPWbQJTyqWkjFjERt = RQrgCObLsoVZPWbQJTyqWkjFjERt;
				if (rQrgCObLsoVZPWbQJTyqWkjFjERt == -3 || rQrgCObLsoVZPWbQJTyqWkjFjERt == 1)
				{
					try
					{
					}
					finally
					{
						pBjInvdWiujKnUTxQZPoNsWsAjQKA();
					}
				}
				VfYApKjVbafKNHUkOdJmlSxspdQh = null;
				RQrgCObLsoVZPWbQJTyqWkjFjERt = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int rQrgCObLsoVZPWbQJTyqWkjFjERt = RQrgCObLsoVZPWbQJTyqWkjFjERt;
					ControllerMapWithAxes controllerMapWithAxes = kcOEeXEWPOGSzBfCNIwQSFDTRYpAA;
					switch (rQrgCObLsoVZPWbQJTyqWkjFjERt)
					{
					default:
						return false;
					case 0:
						RQrgCObLsoVZPWbQJTyqWkjFjERt = -1;
						if (ReInput._id != controllerMapWithAxes.zYMtfthQqWFUiFGChqAKAcaBAqFL)
						{
							ReInput.CheckInitialized(controllerMapWithAxes.zYMtfthQqWFUiFGChqAKAcaBAqFL);
							return false;
						}
						if (HrVbQWbAIfGBBnnHjbWGbPIIFKaQ < 0)
						{
							return false;
						}
						VfYApKjVbafKNHUkOdJmlSxspdQh = controllerMapWithAxes.AxisMaps.GetEnumerator();
						RQrgCObLsoVZPWbQJTyqWkjFjERt = -3;
						break;
					case 1:
						RQrgCObLsoVZPWbQJTyqWkjFjERt = -3;
						break;
					}
					while (VfYApKjVbafKNHUkOdJmlSxspdQh.MoveNext())
					{
						ActionElementMap current = VfYApKjVbafKNHUkOdJmlSxspdQh.Current;
						if (current._actionId == HrVbQWbAIfGBBnnHjbWGbPIIFKaQ && (!zGykiKcvYuvSAdwHQqpzvCxloRmh || current.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
						{
							olbzUieBQFuBMgJsuBBaoVFJYZzd = current;
							RQrgCObLsoVZPWbQJTyqWkjFjERt = 1;
							return true;
						}
					}
					pBjInvdWiujKnUTxQZPoNsWsAjQKA();
					VfYApKjVbafKNHUkOdJmlSxspdQh = null;
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

			private void pBjInvdWiujKnUTxQZPoNsWsAjQKA()
			{
				RQrgCObLsoVZPWbQJTyqWkjFjERt = -1;
				if (VfYApKjVbafKNHUkOdJmlSxspdQh != null)
				{
					VfYApKjVbafKNHUkOdJmlSxspdQh.Dispose();
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
				zclcdDHeEZeYJbrBGjHSuJuLDiQXb zclcdDHeEZeYJbrBGjHSuJuLDiQXb2;
				if (RQrgCObLsoVZPWbQJTyqWkjFjERt == -2 && vSjLvJneIwcxSGkEqiVGGdZFLReqA == Environment.CurrentManagedThreadId)
				{
					RQrgCObLsoVZPWbQJTyqWkjFjERt = 0;
					zclcdDHeEZeYJbrBGjHSuJuLDiQXb2 = this;
				}
				else
				{
					zclcdDHeEZeYJbrBGjHSuJuLDiQXb2 = new zclcdDHeEZeYJbrBGjHSuJuLDiQXb(0);
					zclcdDHeEZeYJbrBGjHSuJuLDiQXb2.kcOEeXEWPOGSzBfCNIwQSFDTRYpAA = kcOEeXEWPOGSzBfCNIwQSFDTRYpAA;
				}
				zclcdDHeEZeYJbrBGjHSuJuLDiQXb2.HrVbQWbAIfGBBnnHjbWGbPIIFKaQ = cNoGhiPMhTeipTXRdzvUvlknaYMY;
				zclcdDHeEZeYJbrBGjHSuJuLDiQXb2.zGykiKcvYuvSAdwHQqpzvCxloRmh = sMFGKyzvGdCLdQPATltEwfHVQCT;
				return zclcdDHeEZeYJbrBGjHSuJuLDiQXb2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
			}
		}

		private sealed class fFoEDJHIVGygKDpeaOSAYFQICtxcb : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
		{
			private int vCGgpAhhSjYSqsOPbEIwzNJJOdugA;

			private ElementAssignmentConflictInfo UpeTkirJQpUYhKgaZZlDIyCpGaWE;

			private int aOSCcCkKHqYtrfAiLcagWTLTedtD;

			public ControllerMapWithAxes ttwAyMPVmbefdeAswKBTXfHkqxYwA;

			private ControllerMap NtWiEhoHKNgHbhZufbnaIOIcExKjA;

			public ControllerMap mSGUhsYFTLBuFLfbHlgBQPtGZHeb;

			private bool bZPaUFxkZAYUMcGZxUCRvYhvcKtV;

			public bool IJCuDhQsaeZGbAUGPWnVgfhmcfAt;

			private IList<ActionElementMap> wwHyxOTjfnFfbxOaxDOkyVIdyNsk;

			private int radcUYdEtcbZbcdKYcHZGHkRqjcxA;

			private IEnumerator<ElementAssignmentConflictInfo> zWTItRNjdyNsjUuZLsjIHpbcKUtF;

			private int CzWDITBlLApTpkvyoDcnjmKmLfTzA;

			private ActionElementMap IBOsFyWAHZHrNjSTZVJSbcsNfRbd;

			private int suZfKKAucVKIDnSYxSEHTEmHScAS;

			ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return UpeTkirJQpUYhKgaZZlDIyCpGaWE;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return UpeTkirJQpUYhKgaZZlDIyCpGaWE;
				}
			}

			[DebuggerHidden]
			public fFoEDJHIVGygKDpeaOSAYFQICtxcb(int P_0)
			{
				vCGgpAhhSjYSqsOPbEIwzNJJOdugA = P_0;
				aOSCcCkKHqYtrfAiLcagWTLTedtD = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int num = vCGgpAhhSjYSqsOPbEIwzNJJOdugA;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						mwKQKUDtXTIhqCdujeVXggTFaBZv();
					}
				}
				wwHyxOTjfnFfbxOaxDOkyVIdyNsk = null;
				zWTItRNjdyNsjUuZLsjIHpbcKUtF = null;
				IBOsFyWAHZHrNjSTZVJSbcsNfRbd = null;
				vCGgpAhhSjYSqsOPbEIwzNJJOdugA = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int num = vCGgpAhhSjYSqsOPbEIwzNJJOdugA;
					ControllerMapWithAxes controllerMapWithAxes = ttwAyMPVmbefdeAswKBTXfHkqxYwA;
					switch (num)
					{
					default:
						return false;
					case 0:
						vCGgpAhhSjYSqsOPbEIwzNJJOdugA = -1;
						if (ReInput._id != controllerMapWithAxes.zYMtfthQqWFUiFGChqAKAcaBAqFL)
						{
							ReInput.CheckInitialized(controllerMapWithAxes.zYMtfthQqWFUiFGChqAKAcaBAqFL);
							return false;
						}
						if (NtWiEhoHKNgHbhZufbnaIOIcExKjA == null)
						{
							return false;
						}
						zWTItRNjdyNsjUuZLsjIHpbcKUtF = ((ControllerMap)controllerMapWithAxes).ElementAssignmentConflicts(NtWiEhoHKNgHbhZufbnaIOIcExKjA, bZPaUFxkZAYUMcGZxUCRvYhvcKtV).GetEnumerator();
						vCGgpAhhSjYSqsOPbEIwzNJJOdugA = -3;
						goto IL_00af;
					case 1:
						vCGgpAhhSjYSqsOPbEIwzNJJOdugA = -3;
						goto IL_00af;
					case 2:
						{
							vCGgpAhhSjYSqsOPbEIwzNJJOdugA = -1;
							goto IL_0232;
						}
						IL_0244:
						if (suZfKKAucVKIDnSYxSEHTEmHScAS < radcUYdEtcbZbcdKYcHZGHkRqjcxA)
						{
							ActionElementMap actionElementMap = wwHyxOTjfnFfbxOaxDOkyVIdyNsk[suZfKKAucVKIDnSYxSEHTEmHScAS];
							if ((!bZPaUFxkZAYUMcGZxUCRvYhvcKtV || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && IBOsFyWAHZHrNjSTZVJSbcsNfRbd.CheckForAssignmentConflict(actionElementMap))
							{
								UpeTkirJQpUYhKgaZZlDIyCpGaWE = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(controllerMapWithAxes._categoryId).userAssignable, -1, controllerMapWithAxes._controllerType, controllerMapWithAxes._controllerId, controllerMapWithAxes._id, IBOsFyWAHZHrNjSTZVJSbcsNfRbd.nJilCjIhFvMUTsTBcUWuYpormNsu, IBOsFyWAHZHrNjSTZVJSbcsNfRbd._actionId, IBOsFyWAHZHrNjSTZVJSbcsNfRbd._elementType, IBOsFyWAHZHrNjSTZVJSbcsNfRbd._elementIdentifierId, IBOsFyWAHZHrNjSTZVJSbcsNfRbd.keyCode, IBOsFyWAHZHrNjSTZVJSbcsNfRbd.modifierKeyFlags);
								vCGgpAhhSjYSqsOPbEIwzNJJOdugA = 2;
								return true;
							}
							goto IL_0232;
						}
						IBOsFyWAHZHrNjSTZVJSbcsNfRbd = null;
						goto IL_025c;
						IL_0232:
						suZfKKAucVKIDnSYxSEHTEmHScAS++;
						goto IL_0244;
						IL_026e:
						if (CzWDITBlLApTpkvyoDcnjmKmLfTzA < controllerMapWithAxes.VvlxjdzijrYJqIDtBmqfXnOovQEf.Count)
						{
							IBOsFyWAHZHrNjSTZVJSbcsNfRbd = controllerMapWithAxes.VvlxjdzijrYJqIDtBmqfXnOovQEf[CzWDITBlLApTpkvyoDcnjmKmLfTzA];
							if (!bZPaUFxkZAYUMcGZxUCRvYhvcKtV || IBOsFyWAHZHrNjSTZVJSbcsNfRbd.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
							{
								suZfKKAucVKIDnSYxSEHTEmHScAS = 0;
								goto IL_0244;
							}
							goto IL_025c;
						}
						return false;
						IL_00af:
						if (zWTItRNjdyNsjUuZLsjIHpbcKUtF.MoveNext())
						{
							ElementAssignmentConflictInfo current = zWTItRNjdyNsjUuZLsjIHpbcKUtF.Current;
							UpeTkirJQpUYhKgaZZlDIyCpGaWE = current;
							vCGgpAhhSjYSqsOPbEIwzNJJOdugA = 1;
							return true;
						}
						mwKQKUDtXTIhqCdujeVXggTFaBZv();
						zWTItRNjdyNsjUuZLsjIHpbcKUtF = null;
						if (!(NtWiEhoHKNgHbhZufbnaIOIcExKjA is ControllerMapWithAxes controllerMapWithAxes2))
						{
							return false;
						}
						if (bZPaUFxkZAYUMcGZxUCRvYhvcKtV && (!controllerMapWithAxes._enabled || !controllerMapWithAxes2._enabled))
						{
							return false;
						}
						wwHyxOTjfnFfbxOaxDOkyVIdyNsk = controllerMapWithAxes2.AxisMaps;
						if (wwHyxOTjfnFfbxOaxDOkyVIdyNsk == null)
						{
							return false;
						}
						radcUYdEtcbZbcdKYcHZGHkRqjcxA = wwHyxOTjfnFfbxOaxDOkyVIdyNsk.Count;
						CzWDITBlLApTpkvyoDcnjmKmLfTzA = 0;
						goto IL_026e;
						IL_025c:
						CzWDITBlLApTpkvyoDcnjmKmLfTzA++;
						goto IL_026e;
					}
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

			private void mwKQKUDtXTIhqCdujeVXggTFaBZv()
			{
				vCGgpAhhSjYSqsOPbEIwzNJJOdugA = -1;
				if (zWTItRNjdyNsjUuZLsjIHpbcKUtF != null)
				{
					zWTItRNjdyNsjUuZLsjIHpbcKUtF.Dispose();
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
				fFoEDJHIVGygKDpeaOSAYFQICtxcb fFoEDJHIVGygKDpeaOSAYFQICtxcb2;
				if (vCGgpAhhSjYSqsOPbEIwzNJJOdugA == -2 && aOSCcCkKHqYtrfAiLcagWTLTedtD == Environment.CurrentManagedThreadId)
				{
					vCGgpAhhSjYSqsOPbEIwzNJJOdugA = 0;
					fFoEDJHIVGygKDpeaOSAYFQICtxcb2 = this;
				}
				else
				{
					fFoEDJHIVGygKDpeaOSAYFQICtxcb2 = new fFoEDJHIVGygKDpeaOSAYFQICtxcb(0);
					fFoEDJHIVGygKDpeaOSAYFQICtxcb2.ttwAyMPVmbefdeAswKBTXfHkqxYwA = ttwAyMPVmbefdeAswKBTXfHkqxYwA;
				}
				fFoEDJHIVGygKDpeaOSAYFQICtxcb2.NtWiEhoHKNgHbhZufbnaIOIcExKjA = mSGUhsYFTLBuFLfbHlgBQPtGZHeb;
				fFoEDJHIVGygKDpeaOSAYFQICtxcb2.bZPaUFxkZAYUMcGZxUCRvYhvcKtV = IJCuDhQsaeZGbAUGPWnVgfhmcfAt;
				return fFoEDJHIVGygKDpeaOSAYFQICtxcb2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private sealed class yktQLcWGbZHHhNaTNczNDPVSKkErA : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
		{
			private int JkmUjvdZaYroCEMZiDFKGNoQLMeD;

			private ElementAssignmentConflictInfo zJZIjnuLJNdzZHcYPqHzKrFbkPNH;

			private int gnZBePeBCGhUnhWYCfUBHlIMnLDj;

			public ControllerMapWithAxes QdlxcoiYFqKrWxiLqpmLWdQecfFv;

			private ActionElementMap qwBZmxcmDJqfwmFXUETFnqwUYae;

			public ActionElementMap KamGFeZgqQLSXNpfYvIMVEpgqJfS;

			private bool ZDXDoXpKjRJbHFntslCoESgzLoDF;

			public bool LdMQcJSyrQGmabdPKGsqHwMojhjy;

			private IEnumerator<ElementAssignmentConflictInfo> garcpahrpjBOYPLkoOOvDhXhWjPn;

			private int TBmOzUXBonaaVHFsyfMfTPPtwQL;

			ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return zJZIjnuLJNdzZHcYPqHzKrFbkPNH;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return zJZIjnuLJNdzZHcYPqHzKrFbkPNH;
				}
			}

			[DebuggerHidden]
			public yktQLcWGbZHHhNaTNczNDPVSKkErA(int P_0)
			{
				JkmUjvdZaYroCEMZiDFKGNoQLMeD = P_0;
				gnZBePeBCGhUnhWYCfUBHlIMnLDj = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int jkmUjvdZaYroCEMZiDFKGNoQLMeD = JkmUjvdZaYroCEMZiDFKGNoQLMeD;
				if (jkmUjvdZaYroCEMZiDFKGNoQLMeD == -3 || jkmUjvdZaYroCEMZiDFKGNoQLMeD == 1)
				{
					try
					{
					}
					finally
					{
						iZObeQabMnQJDAlBkDxZCftGhdnR();
					}
				}
				garcpahrpjBOYPLkoOOvDhXhWjPn = null;
				JkmUjvdZaYroCEMZiDFKGNoQLMeD = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int jkmUjvdZaYroCEMZiDFKGNoQLMeD = JkmUjvdZaYroCEMZiDFKGNoQLMeD;
					ControllerMapWithAxes qdlxcoiYFqKrWxiLqpmLWdQecfFv = QdlxcoiYFqKrWxiLqpmLWdQecfFv;
					switch (jkmUjvdZaYroCEMZiDFKGNoQLMeD)
					{
					default:
						return false;
					case 0:
						JkmUjvdZaYroCEMZiDFKGNoQLMeD = -1;
						if (ReInput._id != qdlxcoiYFqKrWxiLqpmLWdQecfFv.zYMtfthQqWFUiFGChqAKAcaBAqFL)
						{
							ReInput.CheckInitialized(qdlxcoiYFqKrWxiLqpmLWdQecfFv.zYMtfthQqWFUiFGChqAKAcaBAqFL);
							return false;
						}
						if (qwBZmxcmDJqfwmFXUETFnqwUYae == null)
						{
							return false;
						}
						garcpahrpjBOYPLkoOOvDhXhWjPn = ((ControllerMap)qdlxcoiYFqKrWxiLqpmLWdQecfFv).ElementAssignmentConflicts(qwBZmxcmDJqfwmFXUETFnqwUYae, ZDXDoXpKjRJbHFntslCoESgzLoDF).GetEnumerator();
						JkmUjvdZaYroCEMZiDFKGNoQLMeD = -3;
						goto IL_00ad;
					case 1:
						JkmUjvdZaYroCEMZiDFKGNoQLMeD = -3;
						goto IL_00ad;
					case 2:
						{
							JkmUjvdZaYroCEMZiDFKGNoQLMeD = -1;
							goto IL_01a9;
						}
						IL_00ad:
						if (garcpahrpjBOYPLkoOOvDhXhWjPn.MoveNext())
						{
							ElementAssignmentConflictInfo current = garcpahrpjBOYPLkoOOvDhXhWjPn.Current;
							zJZIjnuLJNdzZHcYPqHzKrFbkPNH = current;
							JkmUjvdZaYroCEMZiDFKGNoQLMeD = 1;
							return true;
						}
						iZObeQabMnQJDAlBkDxZCftGhdnR();
						garcpahrpjBOYPLkoOOvDhXhWjPn = null;
						if (ZDXDoXpKjRJbHFntslCoESgzLoDF && (!qdlxcoiYFqKrWxiLqpmLWdQecfFv._enabled || !qwBZmxcmDJqfwmFXUETFnqwUYae.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
						{
							return false;
						}
						if (qdlxcoiYFqKrWxiLqpmLWdQecfFv.VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
						{
							return false;
						}
						TBmOzUXBonaaVHFsyfMfTPPtwQL = 0;
						goto IL_01bb;
						IL_01bb:
						if (TBmOzUXBonaaVHFsyfMfTPPtwQL < qdlxcoiYFqKrWxiLqpmLWdQecfFv.VvlxjdzijrYJqIDtBmqfXnOovQEf.Count)
						{
							ActionElementMap actionElementMap = qdlxcoiYFqKrWxiLqpmLWdQecfFv.VvlxjdzijrYJqIDtBmqfXnOovQEf[TBmOzUXBonaaVHFsyfMfTPPtwQL];
							if ((!ZDXDoXpKjRJbHFntslCoESgzLoDF || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.CheckForAssignmentConflict(qwBZmxcmDJqfwmFXUETFnqwUYae))
							{
								zJZIjnuLJNdzZHcYPqHzKrFbkPNH = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(qdlxcoiYFqKrWxiLqpmLWdQecfFv._categoryId).userAssignable, -1, qdlxcoiYFqKrWxiLqpmLWdQecfFv._controllerType, qdlxcoiYFqKrWxiLqpmLWdQecfFv._controllerId, qdlxcoiYFqKrWxiLqpmLWdQecfFv._id, actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu, actionElementMap._actionId, actionElementMap._elementType, actionElementMap._elementIdentifierId, actionElementMap.keyCode, actionElementMap.modifierKeyFlags);
								JkmUjvdZaYroCEMZiDFKGNoQLMeD = 2;
								return true;
							}
							goto IL_01a9;
						}
						return false;
						IL_01a9:
						TBmOzUXBonaaVHFsyfMfTPPtwQL++;
						goto IL_01bb;
					}
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

			private void iZObeQabMnQJDAlBkDxZCftGhdnR()
			{
				JkmUjvdZaYroCEMZiDFKGNoQLMeD = -1;
				if (garcpahrpjBOYPLkoOOvDhXhWjPn != null)
				{
					garcpahrpjBOYPLkoOOvDhXhWjPn.Dispose();
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
				yktQLcWGbZHHhNaTNczNDPVSKkErA yktQLcWGbZHHhNaTNczNDPVSKkErA2;
				if (JkmUjvdZaYroCEMZiDFKGNoQLMeD == -2 && gnZBePeBCGhUnhWYCfUBHlIMnLDj == Environment.CurrentManagedThreadId)
				{
					JkmUjvdZaYroCEMZiDFKGNoQLMeD = 0;
					yktQLcWGbZHHhNaTNczNDPVSKkErA2 = this;
				}
				else
				{
					yktQLcWGbZHHhNaTNczNDPVSKkErA2 = new yktQLcWGbZHHhNaTNczNDPVSKkErA(0);
					yktQLcWGbZHHhNaTNczNDPVSKkErA2.QdlxcoiYFqKrWxiLqpmLWdQecfFv = QdlxcoiYFqKrWxiLqpmLWdQecfFv;
				}
				yktQLcWGbZHHhNaTNczNDPVSKkErA2.qwBZmxcmDJqfwmFXUETFnqwUYae = KamGFeZgqQLSXNpfYvIMVEpgqJfS;
				yktQLcWGbZHHhNaTNczNDPVSKkErA2.ZDXDoXpKjRJbHFntslCoESgzLoDF = LdMQcJSyrQGmabdPKGsqHwMojhjy;
				return yktQLcWGbZHHhNaTNczNDPVSKkErA2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private sealed class GWHIksmCoQJJdEbhJTatwGyWZHF : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
		{
			private int YzkgebFIiHYQsdiauEThTyeBJTge;

			private ElementAssignmentConflictInfo JdJlLOfVsdtQdAXVLIwAToBGDFaC;

			private int EgDUArfHXrLPXINcwVwevyUqrVOP;

			public ControllerMapWithAxes vZcpjESpcjpLIyMuRbgNdPOkyMCR;

			private ElementAssignmentConflictCheck RIeMsphGSyhXWLGfheWsdjTemksk;

			public ElementAssignmentConflictCheck PUqblJXBfDEYNYcSCGVYIKEaeYom;

			private bool zhUbGTDIzMmUCxqgotxSxNhXfbyn;

			public bool nxavMMRjWqPmqRaJQngQkoSnfcSn;

			private ElementAssignment TBZuyPbcxafEtBDnUOYWKHMZtUpV;

			private IEnumerator<ElementAssignmentConflictInfo> XKNBSSPRaGgHNKHvEOTrUMnhpkzYA;

			private int JuqOZnfJrrvmORoysUYhiJsdcYKC;

			ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return JdJlLOfVsdtQdAXVLIwAToBGDFaC;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return JdJlLOfVsdtQdAXVLIwAToBGDFaC;
				}
			}

			[DebuggerHidden]
			public GWHIksmCoQJJdEbhJTatwGyWZHF(int P_0)
			{
				YzkgebFIiHYQsdiauEThTyeBJTge = P_0;
				EgDUArfHXrLPXINcwVwevyUqrVOP = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int yzkgebFIiHYQsdiauEThTyeBJTge = YzkgebFIiHYQsdiauEThTyeBJTge;
				if (yzkgebFIiHYQsdiauEThTyeBJTge == -3 || yzkgebFIiHYQsdiauEThTyeBJTge == 1)
				{
					try
					{
					}
					finally
					{
						KeCJiGNYCzvImGmsfbySFLnHaTAkA();
					}
				}
				XKNBSSPRaGgHNKHvEOTrUMnhpkzYA = null;
				YzkgebFIiHYQsdiauEThTyeBJTge = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int yzkgebFIiHYQsdiauEThTyeBJTge = YzkgebFIiHYQsdiauEThTyeBJTge;
					ControllerMapWithAxes controllerMapWithAxes = vZcpjESpcjpLIyMuRbgNdPOkyMCR;
					switch (yzkgebFIiHYQsdiauEThTyeBJTge)
					{
					default:
						return false;
					case 0:
						YzkgebFIiHYQsdiauEThTyeBJTge = -1;
						if (ReInput._id != controllerMapWithAxes.zYMtfthQqWFUiFGChqAKAcaBAqFL)
						{
							ReInput.CheckInitialized(controllerMapWithAxes.zYMtfthQqWFUiFGChqAKAcaBAqFL);
							return false;
						}
						XKNBSSPRaGgHNKHvEOTrUMnhpkzYA = ((ControllerMap)controllerMapWithAxes).ElementAssignmentConflicts(RIeMsphGSyhXWLGfheWsdjTemksk, zhUbGTDIzMmUCxqgotxSxNhXfbyn).GetEnumerator();
						YzkgebFIiHYQsdiauEThTyeBJTge = -3;
						goto IL_009e;
					case 1:
						YzkgebFIiHYQsdiauEThTyeBJTge = -3;
						goto IL_009e;
					case 2:
						{
							YzkgebFIiHYQsdiauEThTyeBJTge = -1;
							goto IL_01b5;
						}
						IL_01c7:
						if (JuqOZnfJrrvmORoysUYhiJsdcYKC < controllerMapWithAxes.VvlxjdzijrYJqIDtBmqfXnOovQEf.Count)
						{
							ActionElementMap actionElementMap = controllerMapWithAxes.VvlxjdzijrYJqIDtBmqfXnOovQEf[JuqOZnfJrrvmORoysUYhiJsdcYKC];
							if ((!zhUbGTDIzMmUCxqgotxSxNhXfbyn || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu != RIeMsphGSyhXWLGfheWsdjTemksk.elementMapId && actionElementMap.CheckForAssignmentConflict(TBZuyPbcxafEtBDnUOYWKHMZtUpV))
							{
								JdJlLOfVsdtQdAXVLIwAToBGDFaC = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(controllerMapWithAxes._categoryId).userAssignable, -1, controllerMapWithAxes._controllerType, controllerMapWithAxes._controllerId, controllerMapWithAxes._id, actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu, actionElementMap._actionId, actionElementMap._elementType, actionElementMap._elementIdentifierId, actionElementMap.keyCode, actionElementMap.modifierKeyFlags);
								YzkgebFIiHYQsdiauEThTyeBJTge = 2;
								return true;
							}
							goto IL_01b5;
						}
						return false;
						IL_009e:
						if (XKNBSSPRaGgHNKHvEOTrUMnhpkzYA.MoveNext())
						{
							ElementAssignmentConflictInfo current = XKNBSSPRaGgHNKHvEOTrUMnhpkzYA.Current;
							JdJlLOfVsdtQdAXVLIwAToBGDFaC = current;
							YzkgebFIiHYQsdiauEThTyeBJTge = 1;
							return true;
						}
						KeCJiGNYCzvImGmsfbySFLnHaTAkA();
						XKNBSSPRaGgHNKHvEOTrUMnhpkzYA = null;
						if (zhUbGTDIzMmUCxqgotxSxNhXfbyn && !controllerMapWithAxes._enabled)
						{
							return false;
						}
						if (controllerMapWithAxes.VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
						{
							return false;
						}
						TBZuyPbcxafEtBDnUOYWKHMZtUpV = RIeMsphGSyhXWLGfheWsdjTemksk.ToElementAssignment();
						JuqOZnfJrrvmORoysUYhiJsdcYKC = 0;
						goto IL_01c7;
						IL_01b5:
						JuqOZnfJrrvmORoysUYhiJsdcYKC++;
						goto IL_01c7;
					}
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

			private void KeCJiGNYCzvImGmsfbySFLnHaTAkA()
			{
				YzkgebFIiHYQsdiauEThTyeBJTge = -1;
				if (XKNBSSPRaGgHNKHvEOTrUMnhpkzYA != null)
				{
					XKNBSSPRaGgHNKHvEOTrUMnhpkzYA.Dispose();
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
				GWHIksmCoQJJdEbhJTatwGyWZHF gWHIksmCoQJJdEbhJTatwGyWZHF;
				if (YzkgebFIiHYQsdiauEThTyeBJTge == -2 && EgDUArfHXrLPXINcwVwevyUqrVOP == Environment.CurrentManagedThreadId)
				{
					YzkgebFIiHYQsdiauEThTyeBJTge = 0;
					gWHIksmCoQJJdEbhJTatwGyWZHF = this;
				}
				else
				{
					gWHIksmCoQJJdEbhJTatwGyWZHF = new GWHIksmCoQJJdEbhJTatwGyWZHF(0);
					gWHIksmCoQJJdEbhJTatwGyWZHF.vZcpjESpcjpLIyMuRbgNdPOkyMCR = vZcpjESpcjpLIyMuRbgNdPOkyMCR;
				}
				gWHIksmCoQJJdEbhJTatwGyWZHF.RIeMsphGSyhXWLGfheWsdjTemksk = PUqblJXBfDEYNYcSCGVYIKEaeYom;
				gWHIksmCoQJJdEbhJTatwGyWZHF.zhUbGTDIzMmUCxqgotxSxNhXfbyn = nxavMMRjWqPmqRaJQngQkoSnfcSn;
				return gWHIksmCoQJJdEbhJTatwGyWZHF;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private readonly IList<ActionElementMap> VvlxjdzijrYJqIDtBmqfXnOovQEf;

		private readonly ReadOnlyCollection<ActionElementMap> zhzBBIJrOMJQqmZSWseMrfLitpzX;

		public int axisMapCount
		{
			get
			{
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return 0;
				}
				if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
				{
					return 0;
				}
				return VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			}
		}

		public IList<ActionElementMap> AxisMaps
		{
			get
			{
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
				}
				return zhzBBIJrOMJQqmZSWseMrfLitpzX;
			}
		}

		internal AList<ActionElementMap> bluHTIHZzVOouyueNmhnQDqXANlH => (AList<ActionElementMap>)VvlxjdzijrYJqIDtBmqfXnOovQEf;

		public ControllerMapWithAxes()
		{
			VvlxjdzijrYJqIDtBmqfXnOovQEf = new AList<ActionElementMap>();
			zhzBBIJrOMJQqmZSWseMrfLitpzX = new ReadOnlyCollection<ActionElementMap>(VvlxjdzijrYJqIDtBmqfXnOovQEf);
		}

		public ControllerMapWithAxes(ControllerMapWithAxes P_0)
			: base(P_0)
		{
			VvlxjdzijrYJqIDtBmqfXnOovQEf = new AList<ActionElementMap>();
			zhzBBIJrOMJQqmZSWseMrfLitpzX = new ReadOnlyCollection<ActionElementMap>(VvlxjdzijrYJqIDtBmqfXnOovQEf);
			ControllerMap.RAmMePHwhbbjmrfLAYKtBaJPbccQ();
			if (P_0.VvlxjdzijrYJqIDtBmqfXnOovQEf != null)
			{
				int count = P_0.VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
				for (int i = 0; i < count; i++)
				{
					HdHsqRJwBHzLQaHbsrmxFrCfmUzI(new ActionElementMap(P_0.VvlxjdzijrYJqIDtBmqfXnOovQEf[i]));
				}
			}
			ControllerMap.oeOZZgeXJicFbaxfdmvQlNMqgCjfA();
		}

		public override bool ContainsAction(int actionId)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			if (base.ContainsAction(actionId))
			{
				return true;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return false;
			}
			int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			for (int i = 0; i < count; i++)
			{
				if (VvlxjdzijrYJqIDtBmqfXnOovQEf[i]._actionId == actionId)
				{
					return true;
				}
			}
			return false;
		}

		public override bool CreateElementMap(int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert, out ActionElementMap result)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				result = null;
				return false;
			}
			if (base.CreateElementMap(actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert, out result))
			{
				return true;
			}
			if (!SAlkOmnfgRGtGeCoXcZFVoCatRxd(elementType))
			{
				return false;
			}
			ActionElementMap actionElementMap = new ActionElementMap(actionId, elementType, elementIdentifierId, axisContribution, axisRange, invert);
			BakeElementMap(actionElementMap);
			HdHsqRJwBHzLQaHbsrmxFrCfmUzI(actionElementMap);
			result = actionElementMap;
			return true;
		}

		public override bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert, out ActionElementMap result)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				result = null;
				return false;
			}
			if (base.ReplaceElementMap(elementMapId, actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert, out result))
			{
				return true;
			}
			if (!SAlkOmnfgRGtGeCoXcZFVoCatRxd(elementType))
			{
				return false;
			}
			ActionElementMap elementMap = GetElementMap(elementMapId);
			if (elementMap == null)
			{
				return false;
			}
			if (!SAlkOmnfgRGtGeCoXcZFVoCatRxd(elementMap._elementType))
			{
				DeleteElementMap(elementMapId);
				elementMap.elementType = ControllerElementType.Axis;
				HdHsqRJwBHzLQaHbsrmxFrCfmUzI(elementMap);
			}
			if (EfpdPlwpfNDeNXbRAXfUhuXnGKEu(elementMapId) < 0)
			{
				return false;
			}
			ControllerMap.LxwILqboYSTsOrojOayZHEnpMZuBb(elementMap, actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert);
			BakeElementMap(elementMap);
			result = elementMap;
			GKClWfkOaAcWgcWSeeqXJvARlRJB();
			return true;
		}

		public override bool DeleteElementMap(int elementMapId)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			if (base.DeleteElementMap(elementMapId))
			{
				return true;
			}
			int num = EfpdPlwpfNDeNXbRAXfUhuXnGKEu(elementMapId);
			if (num < 0)
			{
				return false;
			}
			xqvjiekhsgrFAluIeQkFOBrPrFPo(elementMapId, num);
			return true;
		}

		public override bool DeleteElementMapsWithAction(string actionName)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			return DeleteElementMapsWithAction(ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName));
		}

		public override bool DeleteElementMapsWithAction(int actionId)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			return base.DeleteElementMapsWithAction(actionId) | DeleteAxisMapsWithAction(actionId);
		}

		public override ActionElementMap GetElementMap(int elementMapId)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			ActionElementMap elementMap = base.GetElementMap(elementMapId);
			if (elementMap != null)
			{
				return elementMap;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return null;
			}
			int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			for (int i = 0; i < count; i++)
			{
				if (VvlxjdzijrYJqIDtBmqfXnOovQEf[i].nJilCjIhFvMUTsTBcUWuYpormNsu == elementMapId)
				{
					return VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				}
			}
			return null;
		}

		public override ActionElementMap GetFirstElementMapWithAction(int actionId)
		{
			return GetFirstElementMapWithAction(actionId, skipDisabledMaps: false);
		}

		public override ActionElementMap GetFirstElementMapWithAction(int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			if (actionId < 0)
			{
				return null;
			}
			ActionElementMap firstElementMapWithAction = base.GetFirstElementMapWithAction(actionId, skipDisabledMaps);
			if (firstElementMapWithAction != null)
			{
				return firstElementMapWithAction;
			}
			int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if (actionElementMap._actionId == actionId && (!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					return actionElementMap;
				}
			}
			return null;
		}

		internal virtual ActionElementMap EUNIpyjAYsNPWZGZkQvCfktWAKoi(Predicate<ActionElementMap> P_0, bool P_1)
		{
			ActionElementMap actionElementMap = base.rhhqnkCVXqczPtVGWeHPIXVpAJyO(P_0, P_1);
			if (actionElementMap != null)
			{
				return actionElementMap;
			}
			return oVFUhFRCqqhYcktZGFQuzurZKZGf(P_0, P_1);
		}

		internal virtual int FytgwWMOogiuNzPAUngrffElLsSv(Predicate<ActionElementMap> P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			return base.zRAaXHZhgQekRkSOpZXGmIyScrpxA(P_0, P_1, P_2, P_3) + hqiNCQnmiSqTvdKgojCCLTGiSDzT(P_0, P_1, P_2, true);
		}

		public override void ClearElementMaps()
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return;
			}
			base.ClearElementMaps();
			VvlxjdzijrYJqIDtBmqfXnOovQEf.Clear();
		}

		public ActionElementMap GetAxisMap(int index)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null || index < 0 || index >= VvlxjdzijrYJqIDtBmqfXnOovQEf.Count)
			{
				return null;
			}
			return VvlxjdzijrYJqIDtBmqfXnOovQEf[index];
		}

		public ActionElementMap[] GetAxisMaps()
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.array;
			}
			return GetAxisMaps(skipDisabledMaps: false);
		}

		public ActionElementMap[] GetAxisMaps(bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.array;
			}
			if (!skipDisabledMaps)
			{
				return ListTools.ToArray(VvlxjdzijrYJqIDtBmqfXnOovQEf);
			}
			int num = axisMapCount;
			List<ActionElementMap> list = new List<ActionElementMap>(num);
			for (int i = 0; i < num; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if (actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					list.Add(actionElementMap);
				}
			}
			return list.ToArray();
		}

		public int GetAxisMaps(bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return qHAZUXgepQfunBPVbIhQZiqRBLwOA(skipDisabledMaps, results, false);
		}

		public ActionElementMap[] GetAxisMapsWithAction(string actionName)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.array;
			}
			InputAction inputAction = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.NsixmaaSoJxmhDlFhVMoiKUGoKgn(actionName, true);
			if (inputAction == null)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			return GetAxisMapsWithAction(inputAction.id);
		}

		public ActionElementMap[] GetAxisMapsWithAction(int actionId)
		{
			return GetAxisMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public ActionElementMap[] GetAxisMapsWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.array;
			}
			InputAction inputAction = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.NsixmaaSoJxmhDlFhVMoiKUGoKgn(actionName, true);
			if (inputAction == null)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			return GetAxisMapsWithAction(inputAction.id, skipDisabledMaps);
		}

		public ActionElementMap[] GetAxisMapsWithAction(int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.array;
			}
			if (actionId < 0)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			int num = axisMapCount;
			if (num == 0)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if (actionElementMap._actionId == actionId && (!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					num2++;
				}
			}
			if (num2 == 0)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			ActionElementMap[] array = new ActionElementMap[num2];
			int num3 = 0;
			for (int j = 0; j < num; j++)
			{
				ActionElementMap actionElementMap2 = VvlxjdzijrYJqIDtBmqfXnOovQEf[j];
				if (actionElementMap2._actionId == actionId && (!skipDisabledMaps || actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					array[num3] = actionElementMap2;
					num3++;
				}
			}
			return array;
		}

		public int GetAxisMapsWithAction(string actionName, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			InputAction inputAction = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.NsixmaaSoJxmhDlFhVMoiKUGoKgn(actionName, true);
			if (inputAction == null)
			{
				ListTools.TryClear(results);
				return 0;
			}
			return GetAxisMapsWithAction(inputAction.id, results);
		}

		public int GetAxisMapsWithAction(int actionId, List<ActionElementMap> results)
		{
			return GetAxisMapsWithAction(actionId, skipDisabledMaps: false, results);
		}

		public int GetAxisMapsWithAction(string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			InputAction inputAction = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.NsixmaaSoJxmhDlFhVMoiKUGoKgn(actionName, true);
			if (inputAction == null)
			{
				ListTools.TryClear(results);
				return 0;
			}
			return GetAxisMapsWithAction(inputAction.id, skipDisabledMaps, results);
		}

		public int GetAxisMapsWithAction(int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return DYmUwpxHrWBxsHRjasXByyALHrNM(actionId, skipDisabledMaps, results, false);
		}

		public IEnumerable<ActionElementMap> AxisMapsWithAction(string actionName)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return AxisMapsWithAction(actionId);
		}

		public IEnumerable<ActionElementMap> AxisMapsWithAction(int actionId)
		{
			return AxisMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public IEnumerable<ActionElementMap> AxisMapsWithAction(string actionName, bool skipDisabledMaps)
		{
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return AxisMapsWithAction(actionId, skipDisabledMaps);
		}

		[IteratorStateMachine(typeof(zclcdDHeEZeYJbrBGjHSuJuLDiQXb))]
		public IEnumerable<ActionElementMap> AxisMapsWithAction(int actionId, bool skipDisabledMaps)
		{
			return new zclcdDHeEZeYJbrBGjHSuJuLDiQXb(-2)
			{
				kcOEeXEWPOGSzBfCNIwQSFDTRYpAA = this,
				cNoGhiPMhTeipTXRdzvUvlknaYMY = actionId,
				sMFGKyzvGdCLdQPATltEwfHVQCT = skipDisabledMaps
			};
		}

		public ActionElementMap GetFirstAxisMapWithAction(int actionId)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			return GetFirstAxisMapWithAction(actionId, skipDisabledMaps: false);
		}

		public ActionElementMap GetFirstAxisMapWithAction(string actionName)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetFirstAxisMapWithAction(actionId);
		}

		public ActionElementMap GetFirstAxisMapWithAction(int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			if (actionId < 0)
			{
				return null;
			}
			IList<ActionElementMap> axisMaps = AxisMaps;
			int count = axisMaps.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = axisMaps[i];
				if (actionElementMap._actionId == actionId && (!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					return actionElementMap;
				}
			}
			return null;
		}

		public ActionElementMap GetFirstAxisMapWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetFirstAxisMapWithAction(actionId, skipDisabledMaps);
		}

		public ActionElementMap GetFirstAxisMapMatch(Predicate<ActionElementMap> predicate)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			return oVFUhFRCqqhYcktZGFQuzurZKZGf(predicate, false);
		}

		internal ActionElementMap oVFUhFRCqqhYcktZGFQuzurZKZGf(Predicate<ActionElementMap> P_0, bool P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("predicate");
			}
			IList<ActionElementMap> axisMaps = AxisMaps;
			int num = axisMapCount;
			try
			{
				for (int i = 0; i < num; i++)
				{
					ActionElementMap actionElementMap = axisMaps[i];
					if ((!P_1 || actionElementMap.enabled) && P_0(actionElementMap))
					{
						return actionElementMap;
					}
				}
			}
			catch (Exception exception)
			{
				ReInput.HandleCallbackException("ControllerMap.GetFirstAxisMapMatch", exception);
			}
			return null;
		}

		public int GetAxisMapMatches(Predicate<ActionElementMap> predicate, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return hqiNCQnmiSqTvdKgojCCLTGiSDzT(predicate, false, results, false);
		}

		internal int hqiNCQnmiSqTvdKgojCCLTGiSDzT(Predicate<ActionElementMap> P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("predicate");
			}
			if (P_2 == null)
			{
				throw new ArgumentNullException("results");
			}
			int num = 0;
			if (!P_3)
			{
				P_2.Clear();
			}
			else
			{
				num = P_2.Count;
			}
			IList<ActionElementMap> axisMaps = AxisMaps;
			int num2 = axisMapCount;
			try
			{
				for (int i = 0; i < num2; i++)
				{
					ActionElementMap actionElementMap = axisMaps[i];
					if ((!P_1 || actionElementMap.enabled) && P_0(actionElementMap))
					{
						P_2.Add(actionElementMap);
					}
				}
			}
			catch (Exception exception)
			{
				ReInput.HandleCallbackException("ControllerMap.GetAxisMapMatches", exception);
			}
			return P_2.Count - num;
		}

		public void ForEachAxisMapMatch(Predicate<ActionElementMap> predicate, Action<ActionElementMap> actionToPerform)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return;
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			if (actionToPerform == null)
			{
				throw new ArgumentNullException("actionToPerform");
			}
			int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					ActionElementMap obj = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
					if (predicate(obj))
					{
						actionToPerform(obj);
					}
				}
			}
			catch (Exception exception)
			{
				ReInput.HandleCallbackException("ControllerMap.ForEachAxisMapMatch", exception);
			}
		}

		public bool DeleteAxisMapsWithAction(string actionName)
		{
			return DeleteAxisMapsWithAction(ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName));
		}

		public bool DeleteAxisMapsWithAction(int actionId)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			if (actionId < 0)
			{
				return false;
			}
			int num = axisMapCount;
			if (num == 0)
			{
				return false;
			}
			bool result = false;
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				if (VvlxjdzijrYJqIDtBmqfXnOovQEf[num2] != null && VvlxjdzijrYJqIDtBmqfXnOovQEf[num2]._actionId == actionId)
				{
					xqvjiekhsgrFAluIeQkFOBrPrFPo(VvlxjdzijrYJqIDtBmqfXnOovQEf[num2].nJilCjIhFvMUTsTBcUWuYpormNsu, num2);
					result = true;
				}
			}
			return result;
		}

		public int SetAllAxisMapsEnabled(bool state)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			int num = 0;
			int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if (actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA != state)
				{
					actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA = state;
					num++;
				}
			}
			return num;
		}

		public override bool DoesElementAssignmentConflict(ControllerMap controllerMap, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			if (controllerMap == null)
			{
				return false;
			}
			if (base.DoesElementAssignmentConflict(controllerMap, skipDisabledMaps))
			{
				return true;
			}
			if (!(controllerMap is ControllerMapWithAxes controllerMapWithAxes))
			{
				return false;
			}
			if (skipDisabledMaps && (!_enabled || !controllerMapWithAxes._enabled))
			{
				return false;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return false;
			}
			IList<ActionElementMap> axisMaps = controllerMapWithAxes.AxisMaps;
			if (axisMaps == null)
			{
				return false;
			}
			int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			int count2 = axisMaps.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if (skipDisabledMaps && !actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					continue;
				}
				for (int j = 0; j < count2; j++)
				{
					ActionElementMap actionElementMap2 = axisMaps[j];
					if ((!skipDisabledMaps || actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.CheckForAssignmentConflict(actionElementMap2))
					{
						return true;
					}
				}
			}
			return false;
		}

		public override bool DoesElementAssignmentConflict(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			if (actionElementMap == null)
			{
				return false;
			}
			if (base.DoesElementAssignmentConflict(actionElementMap, skipDisabledMaps))
			{
				return true;
			}
			if (skipDisabledMaps && (!_enabled || !actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
			{
				return false;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return false;
			}
			for (int i = 0; i < VvlxjdzijrYJqIDtBmqfXnOovQEf.Count; i++)
			{
				ActionElementMap actionElementMap2 = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if ((!skipDisabledMaps || actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap2.CheckForAssignmentConflict(actionElementMap))
				{
					return true;
				}
			}
			return false;
		}

		public override bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			if (base.DoesElementAssignmentConflict(conflictCheck, skipDisabledMaps))
			{
				return true;
			}
			if (skipDisabledMaps && !_enabled)
			{
				return false;
			}
			if (conflictCheck.elementAssignmentType != ElementAssignmentType.FullAxis && conflictCheck.elementAssignmentType != ElementAssignmentType.SplitAxis)
			{
				return false;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return false;
			}
			ElementAssignment elementAssignment = conflictCheck.ToElementAssignment();
			for (int i = 0; i < VvlxjdzijrYJqIDtBmqfXnOovQEf.Count; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if ((!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu != conflictCheck.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
				{
					return true;
				}
			}
			return false;
		}

		[IteratorStateMachine(typeof(fFoEDJHIVGygKDpeaOSAYFQICtxcb))]
		public override IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ControllerMap controllerMap, bool skipDisabledMaps)
		{
			return new fFoEDJHIVGygKDpeaOSAYFQICtxcb(-2)
			{
				ttwAyMPVmbefdeAswKBTXfHkqxYwA = this,
				mSGUhsYFTLBuFLfbHlgBQPtGZHeb = controllerMap,
				IJCuDhQsaeZGbAUGPWnVgfhmcfAt = skipDisabledMaps
			};
		}

		[IteratorStateMachine(typeof(yktQLcWGbZHHhNaTNczNDPVSKkErA))]
		public override IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			return new yktQLcWGbZHHhNaTNczNDPVSKkErA(-2)
			{
				QdlxcoiYFqKrWxiLqpmLWdQecfFv = this,
				KamGFeZgqQLSXNpfYvIMVEpgqJfS = actionElementMap,
				LdMQcJSyrQGmabdPKGsqHwMojhjy = skipDisabledMaps
			};
		}

		[IteratorStateMachine(typeof(GWHIksmCoQJJdEbhJTatwGyWZHF))]
		public override IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			return new GWHIksmCoQJJdEbhJTatwGyWZHF(-2)
			{
				vZcpjESpcjpLIyMuRbgNdPOkyMCR = this,
				PUqblJXBfDEYNYcSCGVYIKEaeYom = conflictCheck,
				nxavMMRjWqPmqRaJQngQkoSnfcSn = skipDisabledMaps
			};
		}

		public override int RemoveElementAssignmentConflicts(ControllerMap controllerMap, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			if (controllerMap == null)
			{
				return 0;
			}
			int num = base.RemoveElementAssignmentConflicts(controllerMap, skipDisabledMaps);
			if (!(controllerMap is ControllerMapWithAxes controllerMapWithAxes))
			{
				return num;
			}
			if (skipDisabledMaps && (!_enabled || !controllerMapWithAxes._enabled))
			{
				return num;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return num;
			}
			IList<ActionElementMap> axisMaps = controllerMapWithAxes.AxisMaps;
			if (axisMaps == null)
			{
				return num;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory != null && !mapCategory.userAssignable)
			{
				return num;
			}
			_ = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			int count = axisMaps.Count;
			for (int num2 = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[num2];
				if (!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					for (int i = 0; i < count; i++)
					{
						ActionElementMap actionElementMap2 = axisMaps[i];
						if ((!skipDisabledMaps || actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.CheckForAssignmentConflict(actionElementMap2))
						{
							xqvjiekhsgrFAluIeQkFOBrPrFPo(actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu, num2);
							num++;
							break;
						}
					}
				}
			}
			return num;
		}

		public override int RemoveElementAssignmentConflicts(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			if (actionElementMap == null)
			{
				return 0;
			}
			int num = base.RemoveElementAssignmentConflicts(actionElementMap, skipDisabledMaps);
			if (skipDisabledMaps && (!_enabled || !actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
			{
				return num;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory == null)
			{
				return num;
			}
			if (!mapCategory.userAssignable)
			{
				return num;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return num;
			}
			for (int num2 = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap2 = VvlxjdzijrYJqIDtBmqfXnOovQEf[num2];
				if ((!skipDisabledMaps || actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap2.CheckForAssignmentConflict(actionElementMap))
				{
					xqvjiekhsgrFAluIeQkFOBrPrFPo(actionElementMap2.nJilCjIhFvMUTsTBcUWuYpormNsu, num2);
					num++;
				}
			}
			return num;
		}

		public override int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			int num = base.RemoveElementAssignmentConflicts(conflictCheck, skipDisabledMaps);
			if (skipDisabledMaps && !_enabled)
			{
				return num;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return num;
			}
			if (conflictCheck.elementAssignmentType != ElementAssignmentType.FullAxis && conflictCheck.elementAssignmentType != ElementAssignmentType.SplitAxis)
			{
				return num;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory == null)
			{
				return num;
			}
			if (!mapCategory.userAssignable)
			{
				return num;
			}
			ElementAssignment elementAssignment = conflictCheck.ToElementAssignment();
			for (int num2 = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[num2];
				if ((!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu != conflictCheck.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
				{
					xqvjiekhsgrFAluIeQkFOBrPrFPo(actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu, num2);
					num++;
				}
			}
			return num;
		}

		internal virtual int DsoyHjGLutfswEARjdKUQHQGAatab(ControllerMap P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			int num = base.uDLYQOQEpEkgIfOMsAsuIdaYXpcq(P_0, P_1, P_2, P_3);
			if (!(P_0 is ControllerMapWithAxes controllerMapWithAxes))
			{
				return num;
			}
			if (P_1 && (!_enabled || !controllerMapWithAxes._enabled))
			{
				return num;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return num;
			}
			IList<ActionElementMap> axisMaps = controllerMapWithAxes.AxisMaps;
			if (axisMaps == null)
			{
				return num;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory != null && !mapCategory.userAssignable)
			{
				return num;
			}
			int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			int count2 = axisMaps.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if (!actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					continue;
				}
				for (int j = 0; j < count2; j++)
				{
					ActionElementMap actionElementMap2 = axisMaps[j];
					if ((!P_1 || actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.CheckForAssignmentConflict(actionElementMap2))
					{
						actionElementMap.enabled = false;
						P_2?.Add(actionElementMap);
						num++;
						break;
					}
				}
			}
			return num;
		}

		internal virtual int lzijZADyFIgSfnJBKivmFqPqZxmJ(ActionElementMap P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			int num = base.cTyUpZEcqTXlnXWCJGCCWDVCdMkt(P_0, P_1, P_2, P_3);
			if (P_0 == null)
			{
				return num;
			}
			if (P_1 && (!_enabled || !P_0.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
			{
				return num;
			}
			if (P_0.elementIdentifierId < 0)
			{
				return num;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory == null)
			{
				return num;
			}
			if (!mapCategory.userAssignable)
			{
				return num;
			}
			int num2 = axisMapCount;
			for (int i = 0; i < num2; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if (actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA && P_0.CheckForAssignmentConflict(actionElementMap))
				{
					actionElementMap.enabled = false;
					P_2?.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		internal virtual int GWjjUZjTGnfOdhinQBitkVVeeefb(ElementAssignmentConflictCheck P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			int num = base.UlMygnKDTVqoNMDLtBFpFSHRKVIw(P_0, P_1, P_2, P_3);
			if (P_1 && !_enabled)
			{
				return num;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return num;
			}
			if (P_0.elementAssignmentType != ElementAssignmentType.FullAxis && P_0.elementAssignmentType != ElementAssignmentType.SplitAxis)
			{
				return num;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory == null)
			{
				return num;
			}
			if (!mapCategory.userAssignable)
			{
				return num;
			}
			ElementAssignment elementAssignment = P_0.ToElementAssignment();
			int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if (actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA && actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu != P_0.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
				{
					actionElementMap.enabled = false;
					P_2?.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		public string[] GetAxisNames()
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<string>.array;
			}
			int num = axisMapCount;
			if (num == 0)
			{
				return null;
			}
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = VvlxjdzijrYJqIDtBmqfXnOovQEf[i].elementIdentifierName;
			}
			return array;
		}

		internal virtual bool pgRcILHtIcQbnaTTiAYuyYsGCzWV(ActionElementMap P_0)
		{
			if (base.MJooKrlGDJFRhfXMOcJbjtQgiaYJA(P_0))
			{
				return true;
			}
			ControllerElementType elementType = P_0._elementType;
			if (!SAlkOmnfgRGtGeCoXcZFVoCatRxd(elementType))
			{
				return false;
			}
			HdHsqRJwBHzLQaHbsrmxFrCfmUzI(P_0);
			return true;
		}

		internal virtual int HhPkHFopcAeUcCzLniVeqSDKlCryA(List<ActionElementMap> P_0, bool P_1)
		{
			base.knRVlfEXYlRaoRSAktAGsblDVoJP(P_0, P_1);
			int count = P_0.Count;
			int count2 = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			for (int i = 0; i < count2; i++)
			{
				if (!P_1 || VvlxjdzijrYJqIDtBmqfXnOovQEf[i].uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					P_0.Add(VvlxjdzijrYJqIDtBmqfXnOovQEf[i]);
				}
			}
			return P_0.Count - count;
		}

		internal virtual ActionElementMap zHXuTrpvBNcaLxyxjQjTeDPIqTBF(int P_0, int P_1, ControllerElementType P_2)
		{
			ActionElementMap actionElementMap = base.HYreXCAlSycyyfzLleEmQGUDFSgSA(P_0, P_1, P_2);
			if (actionElementMap != null)
			{
				return actionElementMap;
			}
			if (!SAlkOmnfgRGtGeCoXcZFVoCatRxd(P_2))
			{
				return null;
			}
			int num = rSpTdMBDrCCMwimlFcglFUsolrZGA(P_0, P_1, P_2);
			if (num < 0)
			{
				return null;
			}
			if (P_2 == ControllerElementType.Axis)
			{
				return VvlxjdzijrYJqIDtBmqfXnOovQEf[num];
			}
			throw new NotImplementedException();
		}

		internal virtual int rYlOtiJOReWrBPvDxKSClUYGbViF(int P_0, List<ActionElementMap> P_1, bool P_2)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("results");
			}
			int num = (P_2 ? P_1.Count : 0);
			base.glICmISHwtlFDUzxVTNsrjklgfBc(P_0, P_1, P_2);
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return P_1.Count - num;
			}
			int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			for (int i = 0; i < count; i++)
			{
				if (VvlxjdzijrYJqIDtBmqfXnOovQEf[i]._elementIdentifierId == P_0)
				{
					P_1.Add(VvlxjdzijrYJqIDtBmqfXnOovQEf[i]);
				}
			}
			return P_1.Count - num;
		}

		internal virtual bool hEAAkihUOVHrnCqzDRdENwWhFjKI(int P_0, int P_1, ControllerElementType P_2)
		{
			if (base.rJOLusvCyaSUHoFtlGPHBGerHdyp(P_0, P_1, P_2))
			{
				return true;
			}
			if (!SAlkOmnfgRGtGeCoXcZFVoCatRxd(P_2))
			{
				return false;
			}
			if (P_2 == ControllerElementType.Axis)
			{
				int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
				for (int i = 0; i < count; i++)
				{
					if (VvlxjdzijrYJqIDtBmqfXnOovQEf[i]._elementIdentifierId == P_0 && VvlxjdzijrYJqIDtBmqfXnOovQEf[i]._actionId == P_1)
					{
						return true;
					}
				}
				return false;
			}
			throw new NotImplementedException();
		}

		internal virtual int fTIcivMxzdOCTxudBMVbJxETewkm(int P_0, int P_1, ControllerElementType P_2)
		{
			int num = base.rSpTdMBDrCCMwimlFcglFUsolrZGA(P_0, P_1, P_2);
			if (num >= 0)
			{
				return num;
			}
			if (!SAlkOmnfgRGtGeCoXcZFVoCatRxd(P_2))
			{
				return -1;
			}
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return -1;
			}
			if (P_2 == ControllerElementType.Axis)
			{
				int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
				for (int i = 0; i < count; i++)
				{
					if (VvlxjdzijrYJqIDtBmqfXnOovQEf[i]._elementIdentifierId == P_0 && VvlxjdzijrYJqIDtBmqfXnOovQEf[i]._actionId == P_1)
					{
						return i;
					}
				}
				return -1;
			}
			throw new NotImplementedException();
		}

		internal int EfpdPlwpfNDeNXbRAXfUhuXnGKEu(int P_0)
		{
			if (VvlxjdzijrYJqIDtBmqfXnOovQEf == null)
			{
				return -1;
			}
			int count = VvlxjdzijrYJqIDtBmqfXnOovQEf.Count;
			for (int i = 0; i < count; i++)
			{
				if (VvlxjdzijrYJqIDtBmqfXnOovQEf[i].nJilCjIhFvMUTsTBcUWuYpormNsu == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		internal int qHAZUXgepQfunBPVbIhQZiqRBLwOA(bool P_0, List<ActionElementMap> P_1, bool P_2)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("results");
			}
			if (!P_2)
			{
				P_1.Clear();
			}
			int num = axisMapCount;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if (!P_0 || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					P_1.Add(actionElementMap);
					num2++;
				}
			}
			return num2;
		}

		internal int DYmUwpxHrWBxsHRjasXByyALHrNM(int P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
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
			int num = axisMapCount;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if (actionElementMap._actionId == P_0 && (!P_1 || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					P_2.Add(actionElementMap);
					num2++;
				}
			}
			return num2;
		}

		internal virtual int GhWhazOWmxGdLzEzQSzdXaAAVbUg(int P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			int num = base.StwzmqpEamBsxgECgGOOyjBfNurP(P_0, P_1, P_2, P_3);
			if (P_0 < 0)
			{
				return num;
			}
			int num2 = axisMapCount;
			for (int i = 0; i < num2; i++)
			{
				ActionElementMap actionElementMap = VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				if (actionElementMap._actionId == P_0 && (!P_1 || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					P_2.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		internal virtual ActionElementMap hihDPLiXUHAsYernGTAtjSmZvuUsA(IControllerElementTarget P_0, bool P_1, int P_2, bool P_3, out bool P_4)
		{
			ActionElementMap actionElementMap = base.BtSVfeTrzQORMCWyDKlguEcdjZpHA(P_0, P_1, P_2, P_3, out P_4);
			if (actionElementMap != null)
			{
				return actionElementMap;
			}
			if (P_4)
			{
				return null;
			}
			if (!SAlkOmnfgRGtGeCoXcZFVoCatRxd(P_0.elementType))
			{
				return null;
			}
			int num = axisMapCount;
			_ = P_0.elementIdentifierId;
			for (int i = 0; i < num; i++)
			{
				if ((!P_1 || VvlxjdzijrYJqIDtBmqfXnOovQEf[i]._actionId == P_2) && (!P_3 || VvlxjdzijrYJqIDtBmqfXnOovQEf[i].uPyFcaFdRzKajesnqkOUtFvpIRKHA) && VvlxjdzijrYJqIDtBmqfXnOovQEf[i].IsTarget(P_0))
				{
					return VvlxjdzijrYJqIDtBmqfXnOovQEf[i];
				}
			}
			return null;
		}

		internal virtual int lNazZEOICkglACsovIkpHyfhVwES(IControllerElementTarget P_0, bool P_1, int P_2, bool P_3, List<ActionElementMap> P_4, bool P_5, out bool P_6)
		{
			int num = base.yxkdMnawZniDZXFmujYxcNWEtVSFA(P_0, P_1, P_2, P_3, P_4, P_5, out P_6);
			if (P_6)
			{
				return num;
			}
			if (!SAlkOmnfgRGtGeCoXcZFVoCatRxd(P_0.elementType))
			{
				return num;
			}
			int num2 = axisMapCount;
			_ = P_0.elementIdentifierId;
			for (int i = 0; i < num2; i++)
			{
				if ((!P_1 || VvlxjdzijrYJqIDtBmqfXnOovQEf[i]._actionId == P_2) && (!P_3 || VvlxjdzijrYJqIDtBmqfXnOovQEf[i].uPyFcaFdRzKajesnqkOUtFvpIRKHA) && VvlxjdzijrYJqIDtBmqfXnOovQEf[i].IsTarget(P_0))
				{
					P_4.Add(VvlxjdzijrYJqIDtBmqfXnOovQEf[i]);
					num++;
				}
			}
			return num;
		}

		internal virtual bool kKCFJXBnqnfxrQfwktVRhziBnKKQ(ActionElementMap P_0)
		{
			if (base.HPWPNfmeSDuLnzXMyTkqQVhfoWLd(P_0))
			{
				return true;
			}
			if (P_0 == null)
			{
				return false;
			}
			if (!SAlkOmnfgRGtGeCoXcZFVoCatRxd(P_0._elementType))
			{
				return false;
			}
			VvlxjdzijrYJqIDtBmqfXnOovQEf.Add(P_0);
			RitJKwyVulqhfjIjIbSwgSykCWuwA(P_0);
			return true;
		}

		private bool SAlkOmnfgRGtGeCoXcZFVoCatRxd(ControllerElementType P_0)
		{
			if (P_0 != ControllerElementType.Axis)
			{
				return false;
			}
			return true;
		}

		private void xqvjiekhsgrFAluIeQkFOBrPrFPo(int P_0, int P_1)
		{
			LdlsrfeCcJddaRXscQXRsHwnwEAl(P_0);
			if (P_1 >= 0 && P_1 < axisMapCount)
			{
				VvlxjdzijrYJqIDtBmqfXnOovQEf.RemoveAt(P_1);
			}
		}

		private void HdHsqRJwBHzLQaHbsrmxFrCfmUzI(ActionElementMap P_0)
		{
			if (P_0 != null)
			{
				VvlxjdzijrYJqIDtBmqfXnOovQEf.Add(P_0);
				RitJKwyVulqhfjIjIbSwgSykCWuwA(P_0);
			}
		}

		private void BhyDyOeHlixjrNDQlrsgYBWnzMhO(ActionElementMap P_0, int P_1)
		{
			if (P_0 != null && P_1 >= 0 && P_1 < axisMapCount)
			{
				lOdUHYjHyCASfHGlVAAWZCLSoutmA(VvlxjdzijrYJqIDtBmqfXnOovQEf[P_1].nJilCjIhFvMUTsTBcUWuYpormNsu, P_0);
				VvlxjdzijrYJqIDtBmqfXnOovQEf[P_1] = P_0;
			}
		}

		internal virtual void EkdZJcuhhWxaDZHjFCZNuRZzOaoC(SerializedObject P_0)
		{
			base.jizQYKJmGXYvAYwHkHUTwVXZgVDE(P_0);
			int num = axisMapCount;
			List<object> list = new List<object>();
			P_0.Add("axisMaps", list);
			for (int i = 0; i < num; i++)
			{
				if (VvlxjdzijrYJqIDtBmqfXnOovQEf[i] != null)
				{
					list.Add(VvlxjdzijrYJqIDtBmqfXnOovQEf[i].nXkDBItKfPbLOehFpkJWWFJeYwiu());
				}
			}
		}

		internal virtual bool KPrdZUVSHFUEHItHysAUAJnxaIKq(SerializedObject P_0)
		{
			bool flag = base.SgsBMSItxbPvgtEJRxnDZAFORNZj(P_0);
			if (!flag)
			{
				ClearElementMaps();
				flag = true;
			}
			SerializedObject value = null;
			if (P_0.TryGetDeserializedValueByRef("axisMaps", ref value) && value != null)
			{
				for (int i = 0; i < value.count; i++)
				{
					if (value.TryGetDeserializedValue<SerializedObject>(i, out var value2) || value2 == null)
					{
						ActionElementMap actionElementMap = new ActionElementMap();
						actionElementMap.nynjKwRhGWhMIfJsmDLrvmxlEBrrA(value2);
						if (ActionElementMap.rWwBQUblidRcekuSVdiHTcNfrIlmA(actionElementMap))
						{
							HdHsqRJwBHzLQaHbsrmxFrCfmUzI(actionElementMap);
						}
					}
				}
			}
			GKClWfkOaAcWgcWSeeqXJvARlRJB();
			return flag;
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private IEnumerable<ElementAssignmentConflictInfo> drrAiBJqaydOuDppvwKUsMjwKTMf(ControllerMap P_0, bool P_1)
		{
			return base.ElementAssignmentConflicts(P_0, P_1);
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private IEnumerable<ElementAssignmentConflictInfo> WsgperugNTQGoolHPkYQwOlgDSCD(ActionElementMap P_0, bool P_1)
		{
			return base.ElementAssignmentConflicts(P_0, P_1);
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private IEnumerable<ElementAssignmentConflictInfo> GvtbqonDnfNjRWmuWcRTzMqgUcAd(ElementAssignmentConflictCheck P_0, bool P_1)
		{
			return base.ElementAssignmentConflicts(P_0, P_1);
		}
	}
}
