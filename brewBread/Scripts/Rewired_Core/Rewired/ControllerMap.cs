using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using Rewired.Data.Mapping;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired
{
	public abstract class ControllerMap
	{
		private class eKoFSWJFUhkTGBpYHAfEqNRdvkyzA : IComparer<ActionElementMap>
		{
			public static eKoFSWJFUhkTGBpYHAfEqNRdvkyzA sKlAxiBmvyBxAjrDeOltNvBiEdkP;

			public static eKoFSWJFUhkTGBpYHAfEqNRdvkyzA vhJlHqbDISDRgELdbflITCiWLdGrA => sKlAxiBmvyBxAjrDeOltNvBiEdkP ?? (sKlAxiBmvyBxAjrDeOltNvBiEdkP = new eKoFSWJFUhkTGBpYHAfEqNRdvkyzA());

			public int Compare(ActionElementMap x, ActionElementMap y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return 0;
					}
					return -1;
				}
				if (y == null)
				{
					return 1;
				}
				if (x._elementType == y._elementType)
				{
					return x.id.CompareTo(y.id);
				}
				if (x._elementType switch
				{
					ControllerElementType.Button => 0, 
					ControllerElementType.Axis => 1, 
					ControllerElementType.CompoundElement => 2, 
					_ => throw new NotImplementedException(), 
				} <= y._elementType switch
				{
					ControllerElementType.Button => 0, 
					ControllerElementType.Axis => 1, 
					ControllerElementType.CompoundElement => 2, 
					_ => throw new NotImplementedException(), 
				})
				{
					return -1;
				}
				return 1;
			}
		}

		private sealed class YrwLYGbpuIucBWiJclEEPbvXAsnG : IDisposable, IEnumerable, IEnumerable<ActionElementMap>, IEnumerator, IEnumerator<ActionElementMap>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ActionElementMap VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerMap TtytLoUfsgUyhsklaKccrnoMiiek;

			private int tlJxlOEWqQEUYpNGSwfnmndnnqSe;

			public int jGepRyLmTEBLAXIsaiiTHGhibRuCA;

			private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

			public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

			private IList<ActionElementMap> PPqUAboePWIsEoABjwZIVMwKBLTd;

			private int ZglSMeLzNpGmkzdqcFxseuuObhPA;

			private int FhIFjZbpbSEntgsVWVgqHGbPqlLqA;

			ActionElementMap IEnumerator<ActionElementMap>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public YrwLYGbpuIucBWiJclEEPbvXAsnG(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				ControllerMap ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_00af;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				if (tlJxlOEWqQEUYpNGSwfnmndnnqSe < 0)
				{
					return false;
				}
				PPqUAboePWIsEoABjwZIVMwKBLTd = ttytLoUfsgUyhsklaKccrnoMiiek.ButtonMaps;
				ZglSMeLzNpGmkzdqcFxseuuObhPA = ttytLoUfsgUyhsklaKccrnoMiiek.buttonMapCount;
				FhIFjZbpbSEntgsVWVgqHGbPqlLqA = 0;
				goto IL_00bf;
				IL_00bf:
				if (FhIFjZbpbSEntgsVWVgqHGbPqlLqA < ZglSMeLzNpGmkzdqcFxseuuObhPA)
				{
					ActionElementMap actionElementMap = PPqUAboePWIsEoABjwZIVMwKBLTd[FhIFjZbpbSEntgsVWVgqHGbPqlLqA];
					if (actionElementMap._actionId == tlJxlOEWqQEUYpNGSwfnmndnnqSe && (!HIuFjgoQMMeZHtUafJbERKutTjQt || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = actionElementMap;
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_00af;
				}
				return false;
				IL_00af:
				FhIFjZbpbSEntgsVWVgqHGbPqlLqA++;
				goto IL_00bf;
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
			IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
			{
				YrwLYGbpuIucBWiJclEEPbvXAsnG yrwLYGbpuIucBWiJclEEPbvXAsnG;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					yrwLYGbpuIucBWiJclEEPbvXAsnG = this;
				}
				else
				{
					yrwLYGbpuIucBWiJclEEPbvXAsnG = new YrwLYGbpuIucBWiJclEEPbvXAsnG(0);
					yrwLYGbpuIucBWiJclEEPbvXAsnG.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				yrwLYGbpuIucBWiJclEEPbvXAsnG.tlJxlOEWqQEUYpNGSwfnmndnnqSe = jGepRyLmTEBLAXIsaiiTHGhibRuCA;
				yrwLYGbpuIucBWiJclEEPbvXAsnG.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
				return yrwLYGbpuIucBWiJclEEPbvXAsnG;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
			}
		}

		private sealed class vymVLZALnkRCMreGHdTXWHcOlxCb : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerMap TtytLoUfsgUyhsklaKccrnoMiiek;

			private ControllerMap yaBNguSowjFppZBFNMqWwGMqKjjx;

			public ControllerMap dAqaBgHEAvkbMVeRDCvJcJYeWvRO;

			private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

			public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

			private IList<ActionElementMap> FxsEOkXUkSAvirAKscEjIknCWwdp;

			private int JAuRFibtNVayLydiGDvmbhSedpqr;

			private int FhIFjZbpbSEntgsVWVgqHGbPqlLqA;

			private ActionElementMap gFGgIiwJnwiqwTwXsCEfJSaTaNXIA;

			private int PnYTDMmQdsBQPLcZJgWiBIStgHec;

			ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public vymVLZALnkRCMreGHdTXWHcOlxCb(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				ControllerMap ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_019c;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				if (yaBNguSowjFppZBFNMqWwGMqKjjx == null || ttytLoUfsgUyhsklaKccrnoMiiek.RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
				{
					return false;
				}
				if (HIuFjgoQMMeZHtUafJbERKutTjQt && (!ttytLoUfsgUyhsklaKccrnoMiiek._enabled || !yaBNguSowjFppZBFNMqWwGMqKjjx._enabled))
				{
					return false;
				}
				FxsEOkXUkSAvirAKscEjIknCWwdp = yaBNguSowjFppZBFNMqWwGMqKjjx.ButtonMaps;
				if (FxsEOkXUkSAvirAKscEjIknCWwdp == null)
				{
					return false;
				}
				JAuRFibtNVayLydiGDvmbhSedpqr = FxsEOkXUkSAvirAKscEjIknCWwdp.Count;
				FhIFjZbpbSEntgsVWVgqHGbPqlLqA = 0;
				goto IL_01d4;
				IL_01d4:
				if (FhIFjZbpbSEntgsVWVgqHGbPqlLqA < ttytLoUfsgUyhsklaKccrnoMiiek.RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count)
				{
					gFGgIiwJnwiqwTwXsCEfJSaTaNXIA = ttytLoUfsgUyhsklaKccrnoMiiek.RlEWvWhBtSYLCZgEJyoUhzhqfrnW[FhIFjZbpbSEntgsVWVgqHGbPqlLqA];
					if (!HIuFjgoQMMeZHtUafJbERKutTjQt || gFGgIiwJnwiqwTwXsCEfJSaTaNXIA.kKFZZElqKQSUFMZnWdEudRvTJGpo)
					{
						PnYTDMmQdsBQPLcZJgWiBIStgHec = 0;
						goto IL_01ac;
					}
					goto IL_01c4;
				}
				return false;
				IL_01ac:
				if (PnYTDMmQdsBQPLcZJgWiBIStgHec < JAuRFibtNVayLydiGDvmbhSedpqr)
				{
					ActionElementMap actionElementMap = FxsEOkXUkSAvirAKscEjIknCWwdp[PnYTDMmQdsBQPLcZJgWiBIStgHec];
					if ((!HIuFjgoQMMeZHtUafJbERKutTjQt || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo) && gFGgIiwJnwiqwTwXsCEfJSaTaNXIA.CheckForAssignmentConflict(actionElementMap))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(ttytLoUfsgUyhsklaKccrnoMiiek._categoryId).userAssignable, -1, ttytLoUfsgUyhsklaKccrnoMiiek._controllerType, ttytLoUfsgUyhsklaKccrnoMiiek._controllerId, ttytLoUfsgUyhsklaKccrnoMiiek._id, gFGgIiwJnwiqwTwXsCEfJSaTaNXIA.YVGDeWQAhUWKAOSPoxXuizkXaiTI, gFGgIiwJnwiqwTwXsCEfJSaTaNXIA._actionId, gFGgIiwJnwiqwTwXsCEfJSaTaNXIA._elementType, gFGgIiwJnwiqwTwXsCEfJSaTaNXIA._elementIdentifierId, gFGgIiwJnwiqwTwXsCEfJSaTaNXIA.keyCode, gFGgIiwJnwiqwTwXsCEfJSaTaNXIA.modifierKeyFlags);
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_019c;
				}
				gFGgIiwJnwiqwTwXsCEfJSaTaNXIA = null;
				goto IL_01c4;
				IL_01c4:
				FhIFjZbpbSEntgsVWVgqHGbPqlLqA++;
				goto IL_01d4;
				IL_019c:
				PnYTDMmQdsBQPLcZJgWiBIStgHec++;
				goto IL_01ac;
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
			IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
			{
				vymVLZALnkRCMreGHdTXWHcOlxCb vymVLZALnkRCMreGHdTXWHcOlxCb2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					vymVLZALnkRCMreGHdTXWHcOlxCb2 = this;
				}
				else
				{
					vymVLZALnkRCMreGHdTXWHcOlxCb2 = new vymVLZALnkRCMreGHdTXWHcOlxCb(0);
					vymVLZALnkRCMreGHdTXWHcOlxCb2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				vymVLZALnkRCMreGHdTXWHcOlxCb2.yaBNguSowjFppZBFNMqWwGMqKjjx = dAqaBgHEAvkbMVeRDCvJcJYeWvRO;
				vymVLZALnkRCMreGHdTXWHcOlxCb2.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
				return vymVLZALnkRCMreGHdTXWHcOlxCb2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private sealed class QcZlaGNElskgKdZhVjlJpFmmjzXn : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerMap TtytLoUfsgUyhsklaKccrnoMiiek;

			private ActionElementMap QSqCmiOFzeNqpmkBXQgKFemLcIUCA;

			public ActionElementMap tpItAifrGrjraEGilAOGvgJvUACH;

			private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

			public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public QcZlaGNElskgKdZhVjlJpFmmjzXn(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				ControllerMap ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_0111;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				if (QSqCmiOFzeNqpmkBXQgKFemLcIUCA == null || ttytLoUfsgUyhsklaKccrnoMiiek.RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
				{
					return false;
				}
				if (HIuFjgoQMMeZHtUafJbERKutTjQt && (!ttytLoUfsgUyhsklaKccrnoMiiek._enabled || !QSqCmiOFzeNqpmkBXQgKFemLcIUCA.kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					return false;
				}
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
				goto IL_0121;
				IL_0111:
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
				goto IL_0121;
				IL_0121:
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek.RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count)
				{
					ActionElementMap actionElementMap = ttytLoUfsgUyhsklaKccrnoMiiek.RlEWvWhBtSYLCZgEJyoUhzhqfrnW[hWZNbLCFLBBWXNZxiKeGtzgrnTsg];
					if ((!HIuFjgoQMMeZHtUafJbERKutTjQt || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.CheckForAssignmentConflict(QSqCmiOFzeNqpmkBXQgKFemLcIUCA))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(ttytLoUfsgUyhsklaKccrnoMiiek._categoryId).userAssignable, -1, ttytLoUfsgUyhsklaKccrnoMiiek._controllerType, ttytLoUfsgUyhsklaKccrnoMiiek._controllerId, ttytLoUfsgUyhsklaKccrnoMiiek._id, actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI, actionElementMap._actionId, actionElementMap._elementType, actionElementMap._elementIdentifierId, actionElementMap.keyCode, actionElementMap.modifierKeyFlags);
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_0111;
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
			IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
			{
				QcZlaGNElskgKdZhVjlJpFmmjzXn qcZlaGNElskgKdZhVjlJpFmmjzXn;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					qcZlaGNElskgKdZhVjlJpFmmjzXn = this;
				}
				else
				{
					qcZlaGNElskgKdZhVjlJpFmmjzXn = new QcZlaGNElskgKdZhVjlJpFmmjzXn(0);
					qcZlaGNElskgKdZhVjlJpFmmjzXn.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				qcZlaGNElskgKdZhVjlJpFmmjzXn.QSqCmiOFzeNqpmkBXQgKFemLcIUCA = tpItAifrGrjraEGilAOGvgJvUACH;
				qcZlaGNElskgKdZhVjlJpFmmjzXn.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
				return qcZlaGNElskgKdZhVjlJpFmmjzXn;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private sealed class urttbYsbfbHyZJcctshGYDrCqTuC : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerMap TtytLoUfsgUyhsklaKccrnoMiiek;

			private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

			public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

			private ElementAssignmentConflictCheck HHyftJlGnirHuGQIQnIQkaXvgYegA;

			public ElementAssignmentConflictCheck MelcAnrraPFJzeIaADSAOKRmqdwDb;

			private ElementAssignment FnvvcrDffLPIUvRKBIfBFqTiuZyQ;

			private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

			ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public urttbYsbfbHyZJcctshGYDrCqTuC(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				ControllerMap ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_0123;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				if (HIuFjgoQMMeZHtUafJbERKutTjQt && !ttytLoUfsgUyhsklaKccrnoMiiek._enabled)
				{
					return false;
				}
				if (ttytLoUfsgUyhsklaKccrnoMiiek.RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
				{
					return false;
				}
				FnvvcrDffLPIUvRKBIfBFqTiuZyQ = HHyftJlGnirHuGQIQnIQkaXvgYegA.ToElementAssignment();
				fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
				goto IL_0133;
				IL_0133:
				if (fIMVaffCgsuIJcnrkMmGGKfPwwel < ttytLoUfsgUyhsklaKccrnoMiiek.RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count)
				{
					ActionElementMap actionElementMap = ttytLoUfsgUyhsklaKccrnoMiiek.RlEWvWhBtSYLCZgEJyoUhzhqfrnW[fIMVaffCgsuIJcnrkMmGGKfPwwel];
					if ((!HIuFjgoQMMeZHtUafJbERKutTjQt || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI != HHyftJlGnirHuGQIQnIQkaXvgYegA.elementMapId && actionElementMap.CheckForAssignmentConflict(FnvvcrDffLPIUvRKBIfBFqTiuZyQ))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(ttytLoUfsgUyhsklaKccrnoMiiek._categoryId).userAssignable, -1, ttytLoUfsgUyhsklaKccrnoMiiek._controllerType, ttytLoUfsgUyhsklaKccrnoMiiek._controllerId, ttytLoUfsgUyhsklaKccrnoMiiek._id, actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI, actionElementMap._actionId, actionElementMap._elementType, actionElementMap._elementIdentifierId, actionElementMap.keyCode, actionElementMap.modifierKeyFlags);
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_0123;
				}
				return false;
				IL_0123:
				fIMVaffCgsuIJcnrkMmGGKfPwwel++;
				goto IL_0133;
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
			IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
			{
				urttbYsbfbHyZJcctshGYDrCqTuC urttbYsbfbHyZJcctshGYDrCqTuC2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					urttbYsbfbHyZJcctshGYDrCqTuC2 = this;
				}
				else
				{
					urttbYsbfbHyZJcctshGYDrCqTuC2 = new urttbYsbfbHyZJcctshGYDrCqTuC(0);
					urttbYsbfbHyZJcctshGYDrCqTuC2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				urttbYsbfbHyZJcctshGYDrCqTuC2.HHyftJlGnirHuGQIQnIQkaXvgYegA = MelcAnrraPFJzeIaADSAOKRmqdwDb;
				urttbYsbfbHyZJcctshGYDrCqTuC2.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
				return urttbYsbfbHyZJcctshGYDrCqTuC2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private sealed class qpvmsznkvLtGvYPkturyJZjRuTDG : IDisposable, IEnumerable, IEnumerable<ActionElementMap>, IEnumerator, IEnumerator<ActionElementMap>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ActionElementMap VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerMap TtytLoUfsgUyhsklaKccrnoMiiek;

			private int tlJxlOEWqQEUYpNGSwfnmndnnqSe;

			public int jGepRyLmTEBLAXIsaiiTHGhibRuCA;

			private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

			public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

			private IEnumerator<ActionElementMap> hSeONskmQJhVsjVUHGUASYbDGJrU;

			ActionElementMap IEnumerator<ActionElementMap>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public qpvmsznkvLtGvYPkturyJZjRuTDG(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
				{
					try
					{
					}
					finally
					{
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					ControllerMap ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
						{
							ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
							return false;
						}
						hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.AllMaps.GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						break;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						break;
					}
					while (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
					{
						ActionElementMap current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
						if (current._actionId == tlJxlOEWqQEUYpNGSwfnmndnnqSe && (!HIuFjgoQMMeZHtUafJbERKutTjQt || current.kKFZZElqKQSUFMZnWdEudRvTJGpo))
						{
							VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
							RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
							return true;
						}
					}
					uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					hSeONskmQJhVsjVUHGUASYbDGJrU = null;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
				{
					hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
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
				qpvmsznkvLtGvYPkturyJZjRuTDG qpvmsznkvLtGvYPkturyJZjRuTDG2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					qpvmsznkvLtGvYPkturyJZjRuTDG2 = this;
				}
				else
				{
					qpvmsznkvLtGvYPkturyJZjRuTDG2 = new qpvmsznkvLtGvYPkturyJZjRuTDG(0);
					qpvmsznkvLtGvYPkturyJZjRuTDG2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				qpvmsznkvLtGvYPkturyJZjRuTDG2.tlJxlOEWqQEUYpNGSwfnmndnnqSe = jGepRyLmTEBLAXIsaiiTHGhibRuCA;
				qpvmsznkvLtGvYPkturyJZjRuTDG2.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
				return qpvmsznkvLtGvYPkturyJZjRuTDG2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
			}
		}

		private sealed class CMoMjmYFLphertVfhBPQEvmLtddkA : IDisposable, IEnumerable, IEnumerable<ActionElementMap>, IEnumerator, IEnumerator<ActionElementMap>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ActionElementMap VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerMap TtytLoUfsgUyhsklaKccrnoMiiek;

			private IControllerElementTarget yxNBOzLVvOroyVAopqSLpBzOHENH;

			public IControllerElementTarget AitdhRwEAPeSrJKDrWtrsIVhEmQHb;

			private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

			public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

			private TempListPool.TList<ActionElementMap> hDhRaceBnPIWicvxrmnwTGLAzqRC;

			private List<ActionElementMap>.Enumerator tIlurAGjswRzPeLpFSGHcEnzPBQq;

			ActionElementMap IEnumerator<ActionElementMap>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public CMoMjmYFLphertVfhBPQEvmLtddkA(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				if ((uint)(rxAoyfYzYDsYonLGXsvUgwChukLk - -4) > 1u && rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
				{
					return;
				}
				try
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != -4 && rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return;
					}
					try
					{
					}
					finally
					{
						tMeeFNDmLhzrFkrObkICOsBGeJux();
					}
				}
				finally
				{
					uvbwjgAeXsWySvtbvsLpuFVuAgJB();
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					ControllerMap ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
						{
							ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
							return false;
						}
						hDhRaceBnPIWicvxrmnwTGLAzqRC = TempListPool.GetTList<ActionElementMap>();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						List<ActionElementMap> list = hDhRaceBnPIWicvxrmnwTGLAzqRC.list;
						ttytLoUfsgUyhsklaKccrnoMiiek.UDJQNhqhtojGfcxRhUYWnYqrkexm(yxNBOzLVvOroyVAopqSLpBzOHENH, false, -1, HIuFjgoQMMeZHtUafJbERKutTjQt, list, false, out var _);
						tIlurAGjswRzPeLpFSGHcEnzPBQq = list.GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
						break;
					}
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
						break;
					}
					if (tIlurAGjswRzPeLpFSGHcEnzPBQq.MoveNext())
					{
						ActionElementMap current = tIlurAGjswRzPeLpFSGHcEnzPBQq.Current;
						VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					tMeeFNDmLhzrFkrObkICOsBGeJux();
					tIlurAGjswRzPeLpFSGHcEnzPBQq = default(List<ActionElementMap>.Enumerator);
					uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					hDhRaceBnPIWicvxrmnwTGLAzqRC = null;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (hDhRaceBnPIWicvxrmnwTGLAzqRC != null)
				{
					((IDisposable)hDhRaceBnPIWicvxrmnwTGLAzqRC).Dispose();
				}
			}

			private void tMeeFNDmLhzrFkrObkICOsBGeJux()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
				((IDisposable)tIlurAGjswRzPeLpFSGHcEnzPBQq/*cast due to .constrained prefix*/).Dispose();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
			{
				CMoMjmYFLphertVfhBPQEvmLtddkA cMoMjmYFLphertVfhBPQEvmLtddkA;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					cMoMjmYFLphertVfhBPQEvmLtddkA = this;
				}
				else
				{
					cMoMjmYFLphertVfhBPQEvmLtddkA = new CMoMjmYFLphertVfhBPQEvmLtddkA(0);
					cMoMjmYFLphertVfhBPQEvmLtddkA.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				cMoMjmYFLphertVfhBPQEvmLtddkA.yxNBOzLVvOroyVAopqSLpBzOHENH = AitdhRwEAPeSrJKDrWtrsIVhEmQHb;
				cMoMjmYFLphertVfhBPQEvmLtddkA.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
				return cMoMjmYFLphertVfhBPQEvmLtddkA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
			}
		}

		private sealed class OiOwxbbeGcJppxpXrbcgzAXfGnGeA : IDisposable, IEnumerable, IEnumerable<ActionElementMap>, IEnumerator, IEnumerator<ActionElementMap>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ActionElementMap VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerMap TtytLoUfsgUyhsklaKccrnoMiiek;

			private IControllerElementTarget yxNBOzLVvOroyVAopqSLpBzOHENH;

			public IControllerElementTarget AitdhRwEAPeSrJKDrWtrsIVhEmQHb;

			private int tlJxlOEWqQEUYpNGSwfnmndnnqSe;

			public int jGepRyLmTEBLAXIsaiiTHGhibRuCA;

			private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

			public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

			private TempListPool.TList<ActionElementMap> hDhRaceBnPIWicvxrmnwTGLAzqRC;

			private List<ActionElementMap>.Enumerator tIlurAGjswRzPeLpFSGHcEnzPBQq;

			ActionElementMap IEnumerator<ActionElementMap>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public OiOwxbbeGcJppxpXrbcgzAXfGnGeA(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				if ((uint)(rxAoyfYzYDsYonLGXsvUgwChukLk - -4) > 1u && rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
				{
					return;
				}
				try
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != -4 && rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return;
					}
					try
					{
					}
					finally
					{
						tMeeFNDmLhzrFkrObkICOsBGeJux();
					}
				}
				finally
				{
					uvbwjgAeXsWySvtbvsLpuFVuAgJB();
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					ControllerMap ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
						{
							ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
							return false;
						}
						hDhRaceBnPIWicvxrmnwTGLAzqRC = TempListPool.GetTList<ActionElementMap>();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						List<ActionElementMap> list = hDhRaceBnPIWicvxrmnwTGLAzqRC.list;
						ttytLoUfsgUyhsklaKccrnoMiiek.UDJQNhqhtojGfcxRhUYWnYqrkexm(yxNBOzLVvOroyVAopqSLpBzOHENH, true, tlJxlOEWqQEUYpNGSwfnmndnnqSe, HIuFjgoQMMeZHtUafJbERKutTjQt, list, false, out var _);
						tIlurAGjswRzPeLpFSGHcEnzPBQq = list.GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
						break;
					}
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
						break;
					}
					if (tIlurAGjswRzPeLpFSGHcEnzPBQq.MoveNext())
					{
						ActionElementMap current = tIlurAGjswRzPeLpFSGHcEnzPBQq.Current;
						VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					tMeeFNDmLhzrFkrObkICOsBGeJux();
					tIlurAGjswRzPeLpFSGHcEnzPBQq = default(List<ActionElementMap>.Enumerator);
					uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					hDhRaceBnPIWicvxrmnwTGLAzqRC = null;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (hDhRaceBnPIWicvxrmnwTGLAzqRC != null)
				{
					((IDisposable)hDhRaceBnPIWicvxrmnwTGLAzqRC).Dispose();
				}
			}

			private void tMeeFNDmLhzrFkrObkICOsBGeJux()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
				((IDisposable)tIlurAGjswRzPeLpFSGHcEnzPBQq/*cast due to .constrained prefix*/).Dispose();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
			{
				OiOwxbbeGcJppxpXrbcgzAXfGnGeA oiOwxbbeGcJppxpXrbcgzAXfGnGeA;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					oiOwxbbeGcJppxpXrbcgzAXfGnGeA = this;
				}
				else
				{
					oiOwxbbeGcJppxpXrbcgzAXfGnGeA = new OiOwxbbeGcJppxpXrbcgzAXfGnGeA(0);
					oiOwxbbeGcJppxpXrbcgzAXfGnGeA.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				oiOwxbbeGcJppxpXrbcgzAXfGnGeA.yxNBOzLVvOroyVAopqSLpBzOHENH = AitdhRwEAPeSrJKDrWtrsIVhEmQHb;
				oiOwxbbeGcJppxpXrbcgzAXfGnGeA.tlJxlOEWqQEUYpNGSwfnmndnnqSe = jGepRyLmTEBLAXIsaiiTHGhibRuCA;
				oiOwxbbeGcJppxpXrbcgzAXfGnGeA.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
				return oiOwxbbeGcJppxpXrbcgzAXfGnGeA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
			}
		}

		protected int _id;

		protected int _sourceMapId;

		protected int _categoryId;

		protected int _layoutId;

		protected string _name;

		protected Guid _hardwareGuid;

		protected bool _enabled;

		internal readonly int QajDFFaomlkLHzaostfWYpGUioys;

		private readonly AList<ActionElementMap> RlEWvWhBtSYLCZgEJyoUhzhqfrnW;

		private readonly ReadOnlyCollection<ActionElementMap> PFxPkDcFqItqjJkeJsOlItdTVYKK;

		private readonly AList<ActionElementMap> goUXNJtKtwGJDoSIRfidQYIVOBlD;

		private readonly ReadOnlyCollection<ActionElementMap> yoKfWhSGpjMHgopreUxyudOtPHHh;

		protected int _playerId = -1;

		protected int _controllerId = -1;

		protected ControllerType _controllerType;

		private static int TMzSxjjyapuKOmNiKYPcOmUjpTlD;

		private static int JYOCoLrZDRYoCWqKYeukLdGKhAkD
		{
			get
			{
				int tMzSxjjyapuKOmNiKYPcOmUjpTlD = TMzSxjjyapuKOmNiKYPcOmUjpTlD;
				if (TMzSxjjyapuKOmNiKYPcOmUjpTlD == int.MaxValue)
				{
					TMzSxjjyapuKOmNiKYPcOmUjpTlD = 0;
					return tMzSxjjyapuKOmNiKYPcOmUjpTlD;
				}
				TMzSxjjyapuKOmNiKYPcOmUjpTlD++;
				return tMzSxjjyapuKOmNiKYPcOmUjpTlD;
			}
		}

		public int id
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1;
				}
				return _id;
			}
		}

		public int sourceMapId
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1;
				}
				return _sourceMapId;
			}
			internal set
			{
				_sourceMapId = num;
			}
		}

		public int categoryId
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1;
				}
				return _categoryId;
			}
			internal set
			{
				_categoryId = num;
			}
		}

		public int layoutId
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1;
				}
				return _layoutId;
			}
			internal set
			{
				_layoutId = num;
			}
		}

		public string name
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return string.Empty;
				}
				return _name;
			}
			internal set
			{
				_name = text;
			}
		}

		public Guid hardwareGuid
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Guid.Empty;
				}
				return _hardwareGuid;
			}
			internal set
			{
				_hardwareGuid = guid;
			}
		}

		public bool enabled
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				return _enabled;
			}
			set
			{
				_enabled = value;
			}
		}

		public int playerId
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1;
				}
				return _playerId;
			}
			internal set
			{
				_playerId = num;
			}
		}

		public int controllerId
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return -1;
				}
				return _controllerId;
			}
			internal set
			{
				_controllerId = num;
			}
		}

		public Controller controller
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				return ReInput.controllers.GetController(_controllerType, _controllerId);
			}
		}

		public ControllerType controllerType
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return ControllerType.Keyboard;
				}
				return _controllerType;
			}
		}

		public Player player
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				return ReInput.players.GetPlayer(_playerId);
			}
		}

		public int elementMapCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				return goUXNJtKtwGJDoSIRfidQYIVOBlD.Count;
			}
		}

		public int buttonMapCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				return RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count;
			}
		}

		public IList<ActionElementMap> AllMaps
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
				}
				return yoKfWhSGpjMHgopreUxyudOtPHHh;
			}
		}

		public IList<ActionElementMap> ButtonMaps
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
				}
				return PFxPkDcFqItqjJkeJsOlItdTVYKK;
			}
		}

		internal AList<ActionElementMap> cFGJUwYhcfUZYvBnprhEpVaWEPom => RlEWvWhBtSYLCZgEJyoUhzhqfrnW;

		public ControllerMap()
		{
			_id = JYOCoLrZDRYoCWqKYeukLdGKhAkD;
			_sourceMapId = -1;
			RlEWvWhBtSYLCZgEJyoUhzhqfrnW = new AList<ActionElementMap>();
			PFxPkDcFqItqjJkeJsOlItdTVYKK = new ReadOnlyCollection<ActionElementMap>(RlEWvWhBtSYLCZgEJyoUhzhqfrnW);
			goUXNJtKtwGJDoSIRfidQYIVOBlD = new AList<ActionElementMap>();
			yoKfWhSGpjMHgopreUxyudOtPHHh = new ReadOnlyCollection<ActionElementMap>(goUXNJtKtwGJDoSIRfidQYIVOBlD);
			QajDFFaomlkLHzaostfWYpGUioys = ReInput.id;
		}

		public ControllerMap(ControllerMap P_0)
			: this()
		{
			_id = JYOCoLrZDRYoCWqKYeukLdGKhAkD;
			_sourceMapId = P_0._sourceMapId;
			_categoryId = P_0._categoryId;
			_layoutId = P_0._layoutId;
			_name = P_0._name;
			_hardwareGuid = P_0._hardwareGuid;
			_enabled = P_0._enabled;
			_playerId = P_0._playerId;
			_controllerId = P_0._controllerId;
			_controllerType = P_0._controllerType;
			if (P_0.RlEWvWhBtSYLCZgEJyoUhzhqfrnW != null)
			{
				int count = P_0.RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count;
				for (int i = 0; i < count; i++)
				{
					WyzBpHpqqzkYVfNdqvKJfqBDGXMO(new ActionElementMap(P_0.RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]));
				}
			}
		}

		public bool ContainsAction(string actionName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			InputAction inputAction = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.cJlaptDDgHYEZdKZMGkPWYdKckQLA(actionName, true);
			if (inputAction == null)
			{
				return false;
			}
			return ContainsAction(inputAction.id);
		}

		public virtual bool ContainsAction(int actionId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (actionId < 0)
			{
				return false;
			}
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]._actionId == actionId)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsElementIdentifier(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			AList<ActionElementMap> aList = goUXNJtKtwGJDoSIRfidQYIVOBlD;
			for (int i = 0; i < aList.Count; i++)
			{
				if (goUXNJtKtwGJDoSIRfidQYIVOBlD[i].elementIdentifierId == elementIdentifierId)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsKeyboardKey(KeyCode keyCode, ModifierKeyFlags modifierKeys)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			AList<ActionElementMap> aList = goUXNJtKtwGJDoSIRfidQYIVOBlD;
			for (int i = 0; i < aList.Count; i++)
			{
				if (goUXNJtKtwGJDoSIRfidQYIVOBlD[i].keyCode == keyCode && goUXNJtKtwGJDoSIRfidQYIVOBlD[i].modifierKeyFlags == modifierKeys)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsElementMap(ActionElementMap elementMap)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (elementMap == null)
			{
				return false;
			}
			AList<ActionElementMap> aList = goUXNJtKtwGJDoSIRfidQYIVOBlD;
			for (int i = 0; i < aList.Count; i++)
			{
				if (goUXNJtKtwGJDoSIRfidQYIVOBlD[i].YVGDeWQAhUWKAOSPoxXuizkXaiTI == elementMap.id)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsElementMap(int elementMapId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			AList<ActionElementMap> aList = goUXNJtKtwGJDoSIRfidQYIVOBlD;
			for (int i = 0; i < aList.Count; i++)
			{
				if (goUXNJtKtwGJDoSIRfidQYIVOBlD[i].YVGDeWQAhUWKAOSPoxXuizkXaiTI == elementMapId)
				{
					return true;
				}
			}
			return false;
		}

		public bool ReplaceOrCreateElementMap(ElementAssignment elementAssignment)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			ActionElementMap result;
			return ReplaceOrCreateElementMap(elementAssignment, out result);
		}

		public bool ReplaceOrCreateElementMap(ElementAssignment elementAssignment, out ActionElementMap result)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				result = null;
				return false;
			}
			if (GetElementMap(elementAssignment.elementMapId) == null)
			{
				return CreateElementMap(elementAssignment, out result);
			}
			return ReplaceElementMap(elementAssignment, out result);
		}

		public bool CreateElementMap(ElementAssignment elementAssignment)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			ActionElementMap result;
			return CreateElementMap(elementAssignment, out result);
		}

		public bool CreateElementMap(ElementAssignment elementAssignment, out ActionElementMap result)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				result = null;
				return false;
			}
			if (_controllerType == ControllerType.Keyboard)
			{
				return CreateElementMap(elementAssignment.actionId, elementAssignment.axisContribution, elementAssignment.keyboardKey, elementAssignment.modifierKeyFlags, out result);
			}
			if (_controllerType == ControllerType.Joystick || _controllerType == ControllerType.Mouse || _controllerType == ControllerType.Custom)
			{
				return CreateElementMap(elementAssignment.actionId, elementAssignment.axisContribution, elementAssignment.elementIdentifierId, OzpbBOHoTkskKeBaOXWaTXQCmjjBA.jabYOprZCpTijtFrYngdXNagQtNF(elementAssignment.type), elementAssignment.axisRange, elementAssignment.invert, out result);
			}
			throw new NotImplementedException();
		}

		public bool CreateElementMap(int actionId, Pole axisContribution, KeyCode keyCode, ModifierKey modifierKey1, ModifierKey modifierKey2, ModifierKey modifierKey3)
		{
			ActionElementMap result;
			return CreateElementMap(actionId, axisContribution, keyCode, modifierKey1, modifierKey2, modifierKey3, out result);
		}

		public bool CreateElementMap(int actionId, Pole axisContribution, KeyCode keyCode, ModifierKey modifierKey1, ModifierKey modifierKey2, ModifierKey modifierKey3, out ActionElementMap result)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				result = null;
				return false;
			}
			ActionElementMap actionElementMap = new ActionElementMap(actionId, ControllerElementType.Button, axisContribution, (KeyboardKeyCode)keyCode, modifierKey1, modifierKey2, modifierKey3);
			ReInput.controllers.Keyboard.JpxRPMmkCiJxotCLchUqPLcjlZiQ(this, actionElementMap);
			WyzBpHpqqzkYVfNdqvKJfqBDGXMO(actionElementMap);
			result = actionElementMap;
			return true;
		}

		public bool CreateElementMap(int actionId, Pole axisContribution, KeyCode keyCode, ModifierKeyFlags modifierKeyFlags)
		{
			ActionElementMap result;
			return CreateElementMap(actionId, axisContribution, keyCode, modifierKeyFlags, out result);
		}

		public bool CreateElementMap(int actionId, Pole axisContribution, KeyCode keyCode, ModifierKeyFlags modifierKeyFlags, out ActionElementMap result)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				result = null;
				return false;
			}
			sIZBQTbRJzPyjCTmkapJVcfyXvVBb sIZBQTbRJzPyjCTmkapJVcfyXvVBb2 = sIZBQTbRJzPyjCTmkapJVcfyXvVBb.IsSgiyIILrCIJkNwCUzKaexlZlhV(modifierKeyFlags);
			return CreateElementMap(actionId, axisContribution, keyCode, sIZBQTbRJzPyjCTmkapJVcfyXvVBb2.uTQDUHBOiryPBqHLLXEKKjbRBBus, sIZBQTbRJzPyjCTmkapJVcfyXvVBb2.BpuVLSKErRUyEXnNoenFvstYfATV, sIZBQTbRJzPyjCTmkapJVcfyXvVBb2.pmQKuWfkHtqDXVkQqroayukudkkH, out result);
		}

		public bool CreateElementMap(int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert)
		{
			ActionElementMap result;
			return CreateElementMap(actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert, out result);
		}

		public virtual bool CreateElementMap(int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert, out ActionElementMap result)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				result = null;
				return false;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(elementType))
			{
				result = null;
				return false;
			}
			ActionElementMap actionElementMap = new ActionElementMap(actionId, elementType, elementIdentifierId, axisContribution, axisRange);
			BakeElementMap(actionElementMap);
			WyzBpHpqqzkYVfNdqvKJfqBDGXMO(actionElementMap);
			result = actionElementMap;
			return true;
		}

		public bool ReplaceElementMap(ElementAssignment elementAssignment)
		{
			ActionElementMap result;
			return ReplaceElementMap(elementAssignment, out result);
		}

		public bool ReplaceElementMap(ElementAssignment elementAssignment, out ActionElementMap result)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				result = null;
				return false;
			}
			if (_controllerType == ControllerType.Keyboard)
			{
				return ReplaceElementMap(elementAssignment.elementMapId, elementAssignment.actionId, elementAssignment.axisContribution, elementAssignment.keyboardKey, elementAssignment.modifierKeyFlags, out result);
			}
			if (_controllerType == ControllerType.Joystick || _controllerType == ControllerType.Mouse || _controllerType == ControllerType.Custom)
			{
				return ReplaceElementMap(elementAssignment.elementMapId, elementAssignment.actionId, elementAssignment.axisContribution, elementAssignment.elementIdentifierId, OzpbBOHoTkskKeBaOXWaTXQCmjjBA.jabYOprZCpTijtFrYngdXNagQtNF(elementAssignment.type), elementAssignment.axisRange, elementAssignment.invert, out result);
			}
			throw new NotImplementedException();
		}

		public bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, KeyCode keyCode, ModifierKey modifierKey1, ModifierKey modifierKey2, ModifierKey modifierKey3)
		{
			ActionElementMap result;
			return ReplaceElementMap(elementMapId, actionId, axisContribution, keyCode, modifierKey1, modifierKey2, modifierKey3, out result);
		}

		public bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, KeyCode keyCode, ModifierKey modifierKey1, ModifierKey modifierKey2, ModifierKey modifierKey3, out ActionElementMap result)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				result = null;
				return false;
			}
			ActionElementMap elementMap = GetElementMap(elementMapId);
			if (elementMap == null)
			{
				result = null;
				return false;
			}
			if (tJMxlFgPSSwZhsmmUcyYlWrSITXL(elementMapId) < 0)
			{
				DeleteElementMap(elementMapId);
				elementMap._elementType = ControllerElementType.Button;
				WyzBpHpqqzkYVfNdqvKJfqBDGXMO(elementMap);
			}
			if (tJMxlFgPSSwZhsmmUcyYlWrSITXL(elementMapId) < 0)
			{
				result = null;
				return false;
			}
			elementMap.SPGTRPyvIslcMdbPTItsewSLRPxx();
			elementMap._actionId = actionId;
			elementMap._elementType = ControllerElementType.Button;
			elementMap._axisContribution = axisContribution;
			elementMap._keyboardKeyCode = (KeyboardKeyCode)keyCode;
			elementMap._modifierKey1 = modifierKey1;
			elementMap._modifierKey2 = modifierKey2;
			elementMap._modifierKey3 = modifierKey3;
			ReInput.controllers.Keyboard.JpxRPMmkCiJxotCLchUqPLcjlZiQ(this, elementMap);
			result = elementMap;
			return true;
		}

		public bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, KeyCode keyCode, ModifierKeyFlags modifierKeyFlags)
		{
			ActionElementMap result;
			return ReplaceElementMap(elementMapId, actionId, axisContribution, keyCode, modifierKeyFlags, out result);
		}

		public bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, KeyCode keyCode, ModifierKeyFlags modifierKeyFlags, out ActionElementMap result)
		{
			sIZBQTbRJzPyjCTmkapJVcfyXvVBb sIZBQTbRJzPyjCTmkapJVcfyXvVBb2 = sIZBQTbRJzPyjCTmkapJVcfyXvVBb.IsSgiyIILrCIJkNwCUzKaexlZlhV(modifierKeyFlags);
			return ReplaceElementMap(elementMapId, actionId, axisContribution, keyCode, sIZBQTbRJzPyjCTmkapJVcfyXvVBb2.uTQDUHBOiryPBqHLLXEKKjbRBBus, sIZBQTbRJzPyjCTmkapJVcfyXvVBb2.BpuVLSKErRUyEXnNoenFvstYfATV, sIZBQTbRJzPyjCTmkapJVcfyXvVBb2.pmQKuWfkHtqDXVkQqroayukudkkH, out result);
		}

		public bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert)
		{
			ActionElementMap result;
			return ReplaceElementMap(elementMapId, actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert, out result);
		}

		public virtual bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert, out ActionElementMap result)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				result = null;
				return false;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(elementType))
			{
				result = null;
				return false;
			}
			ActionElementMap elementMap = GetElementMap(elementMapId);
			if (elementMap == null)
			{
				result = null;
				return false;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(elementMap._elementType))
			{
				DeleteElementMap(elementMapId);
				elementMap._elementType = ControllerElementType.Button;
				WyzBpHpqqzkYVfNdqvKJfqBDGXMO(elementMap);
			}
			if (tJMxlFgPSSwZhsmmUcyYlWrSITXL(elementMapId) < 0)
			{
				result = null;
				return false;
			}
			OgCdCocQpxWwVEWsnTRcismHoNbJ(elementMap, actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert);
			BakeElementMap(elementMap);
			result = elementMap;
			return true;
		}

		public virtual bool DeleteElementMap(int elementMapId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			int num = tJMxlFgPSSwZhsmmUcyYlWrSITXL(elementMapId);
			if (num < 0)
			{
				return false;
			}
			fGLHXijIihCDOAHYbiLOljyRdDgy(elementMapId, num);
			return true;
		}

		public virtual bool DeleteElementMapsWithAction(string actionName)
		{
			return DeleteElementMapsWithAction(ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName));
		}

		public virtual bool DeleteElementMapsWithAction(int actionId)
		{
			return DeleteButtonMapsWithAction(actionId);
		}

		public virtual ActionElementMap GetElementMap(int elementMapId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (elementMapId < 0)
			{
				return null;
			}
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i].YVGDeWQAhUWKAOSPoxXuizkXaiTI == elementMapId)
				{
					return RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				}
			}
			return null;
		}

		public ActionElementMap[] GetElementMaps()
		{
			return GetElementMaps(skipDisabledMaps: false);
		}

		public ActionElementMap[] GetElementMaps(bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.array;
			}
			int num = elementMapCount;
			if (num == 0)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			List<ActionElementMap> list = new List<ActionElementMap>(num);
			foreach (ActionElementMap allMap in AllMaps)
			{
				if (!skipDisabledMaps || allMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					list.Add(allMap);
				}
			}
			return list.ToArray();
		}

		public int GetElementMaps(List<ActionElementMap> results)
		{
			return GetElementMaps(skipDisabledMaps: false, results);
		}

		public int GetElementMaps(bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			results.Clear();
			return SfYThgjCbGGBVaAmuUBhxMEDxCeO(results, skipDisabledMaps);
		}

		public ActionElementMap[] GetElementMapsWithAction(string actionName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.array;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetElementMapsWithAction(actionId);
		}

		public ActionElementMap[] GetElementMapsWithAction(int actionId)
		{
			return GetElementMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public ActionElementMap[] GetElementMapsWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.array;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetElementMapsWithAction(actionId, skipDisabledMaps);
		}

		public ActionElementMap[] GetElementMapsWithAction(int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.array;
			}
			if (actionId < 0)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			if (elementMapCount == 0)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			int num = 0;
			foreach (ActionElementMap allMap in AllMaps)
			{
				if (allMap._actionId == actionId && (!skipDisabledMaps || allMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			ActionElementMap[] array = new ActionElementMap[num];
			int num2 = 0;
			foreach (ActionElementMap allMap2 in AllMaps)
			{
				if (allMap2._actionId == actionId && (!skipDisabledMaps || allMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					array[num2] = allMap2;
					num2++;
				}
			}
			return array;
		}

		public int GetElementMapsWithAction(string actionName, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetElementMapsWithAction(actionId, results);
		}

		public int GetElementMapsWithAction(int actionId, List<ActionElementMap> results)
		{
			return GetElementMapsWithAction(actionId, skipDisabledMaps: false, results);
		}

		public int GetElementMapsWithAction(string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetElementMapsWithAction(actionId, skipDisabledMaps, results);
		}

		public int GetElementMapsWithAction(int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			return wpGLUOiKKYaZujmxopRHHGStkBpW(actionId, skipDisabledMaps, results, false);
		}

		public IEnumerable<ActionElementMap> ElementMapsWithAction(string actionName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return ElementMapsWithAction(actionId);
		}

		public IEnumerable<ActionElementMap> ElementMapsWithAction(int actionId)
		{
			return ElementMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public IEnumerable<ActionElementMap> ElementMapsWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return ElementMapsWithAction(actionId, skipDisabledMaps);
		}

		public IEnumerable<ActionElementMap> ElementMapsWithAction(int actionId, bool skipDisabledMaps)
		{
			return new qpvmsznkvLtGvYPkturyJZjRuTDG(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				jGepRyLmTEBLAXIsaiiTHGhibRuCA = actionId,
				MlWZQPQtLyJnETcXcYBEkbNZbonN = skipDisabledMaps
			};
		}

		public virtual ActionElementMap GetFirstElementMapWithAction(int actionId)
		{
			return GetFirstElementMapWithAction(actionId, skipDisabledMaps: false);
		}

		public virtual ActionElementMap GetFirstElementMapWithAction(string actionName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetFirstElementMapWithAction(actionId);
		}

		public virtual ActionElementMap GetFirstElementMapWithAction(int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (actionId < 0)
			{
				return null;
			}
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]._actionId == actionId && (!skipDisabledMaps || RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i].kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					return RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				}
			}
			return null;
		}

		public ActionElementMap GetFirstElementMapWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetFirstElementMapWithAction(actionId, skipDisabledMaps);
		}

		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(ControllerElementTarget elementTarget, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			cyWFKpvAJpSlVUbktJsNPZKHVsyU cyWFKpvAJpSlVUbktJsNPZKHVsyU2 = cyWFKpvAJpSlVUbktJsNPZKHVsyU.LVEUXoDfRdrEgyPvOtkseSGcUXvL(elementTarget);
			IEnumerable<ActionElementMap> result = ElementMapsWithElementTarget(cyWFKpvAJpSlVUbktJsNPZKHVsyU2, skipDisabledMaps);
			cyWFKpvAJpSlVUbktJsNPZKHVsyU.GAJoHEqKKFTdYPaPYTwarurQvQhB(cyWFKpvAJpSlVUbktJsNPZKHVsyU2);
			return result;
		}

		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(IControllerElementTarget elementTarget, bool skipDisabledMaps)
		{
			return new CMoMjmYFLphertVfhBPQEvmLtddkA(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				AitdhRwEAPeSrJKDrWtrsIVhEmQHb = elementTarget,
				MlWZQPQtLyJnETcXcYBEkbNZbonN = skipDisabledMaps
			};
		}

		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(ControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			cyWFKpvAJpSlVUbktJsNPZKHVsyU cyWFKpvAJpSlVUbktJsNPZKHVsyU2 = cyWFKpvAJpSlVUbktJsNPZKHVsyU.LVEUXoDfRdrEgyPvOtkseSGcUXvL(elementTarget);
			IEnumerable<ActionElementMap> result = ElementMapsWithElementTarget(cyWFKpvAJpSlVUbktJsNPZKHVsyU2, actionId, skipDisabledMaps);
			cyWFKpvAJpSlVUbktJsNPZKHVsyU.GAJoHEqKKFTdYPaPYTwarurQvQhB(cyWFKpvAJpSlVUbktJsNPZKHVsyU2);
			return result;
		}

		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(ControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return ElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps);
		}

		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(IControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
		{
			return new OiOwxbbeGcJppxpXrbcgzAXfGnGeA(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				AitdhRwEAPeSrJKDrWtrsIVhEmQHb = elementTarget,
				jGepRyLmTEBLAXIsaiiTHGhibRuCA = actionId,
				MlWZQPQtLyJnETcXcYBEkbNZbonN = skipDisabledMaps
			};
		}

		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(IControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return ElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps);
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(ControllerElementTarget elementTarget, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			cyWFKpvAJpSlVUbktJsNPZKHVsyU cyWFKpvAJpSlVUbktJsNPZKHVsyU2 = cyWFKpvAJpSlVUbktJsNPZKHVsyU.LVEUXoDfRdrEgyPvOtkseSGcUXvL(elementTarget);
			ActionElementMap firstElementMapWithElementTarget = GetFirstElementMapWithElementTarget(cyWFKpvAJpSlVUbktJsNPZKHVsyU2, skipDisabledMaps);
			cyWFKpvAJpSlVUbktJsNPZKHVsyU.GAJoHEqKKFTdYPaPYTwarurQvQhB(cyWFKpvAJpSlVUbktJsNPZKHVsyU2);
			return firstElementMapWithElementTarget;
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(IControllerElementTarget elementTarget, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			bool flag;
			return EUcSmaxDeldMRvTJKrymSFSbIMkm(elementTarget, false, -1, skipDisabledMaps, out flag);
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(ControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			cyWFKpvAJpSlVUbktJsNPZKHVsyU cyWFKpvAJpSlVUbktJsNPZKHVsyU2 = cyWFKpvAJpSlVUbktJsNPZKHVsyU.LVEUXoDfRdrEgyPvOtkseSGcUXvL(elementTarget);
			ActionElementMap firstElementMapWithElementTarget = GetFirstElementMapWithElementTarget(cyWFKpvAJpSlVUbktJsNPZKHVsyU2, actionId, skipDisabledMaps);
			cyWFKpvAJpSlVUbktJsNPZKHVsyU.GAJoHEqKKFTdYPaPYTwarurQvQhB(cyWFKpvAJpSlVUbktJsNPZKHVsyU2);
			return firstElementMapWithElementTarget;
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(ControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetFirstElementMapWithElementTarget(elementTarget, actionId, skipDisabledMaps);
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(IControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			bool flag;
			return EUcSmaxDeldMRvTJKrymSFSbIMkm(elementTarget, true, actionId, skipDisabledMaps, out flag);
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(IControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetFirstElementMapWithElementTarget(elementTarget, actionId, skipDisabledMaps);
		}

		public int GetElementMapsWithElementTarget(ControllerElementTarget elementTarget, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			cyWFKpvAJpSlVUbktJsNPZKHVsyU cyWFKpvAJpSlVUbktJsNPZKHVsyU2 = cyWFKpvAJpSlVUbktJsNPZKHVsyU.LVEUXoDfRdrEgyPvOtkseSGcUXvL(elementTarget);
			int elementMapsWithElementTarget = GetElementMapsWithElementTarget(cyWFKpvAJpSlVUbktJsNPZKHVsyU2, skipDisabledMaps, results);
			cyWFKpvAJpSlVUbktJsNPZKHVsyU.GAJoHEqKKFTdYPaPYTwarurQvQhB(cyWFKpvAJpSlVUbktJsNPZKHVsyU2);
			return elementMapsWithElementTarget;
		}

		public int GetElementMapsWithElementTarget(IControllerElementTarget elementTarget, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			bool flag;
			return UDJQNhqhtojGfcxRhUYWnYqrkexm(elementTarget, false, -1, skipDisabledMaps, results, false, out flag);
		}

		public int GetElementMapsWithElementTarget(ControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			cyWFKpvAJpSlVUbktJsNPZKHVsyU cyWFKpvAJpSlVUbktJsNPZKHVsyU2 = cyWFKpvAJpSlVUbktJsNPZKHVsyU.LVEUXoDfRdrEgyPvOtkseSGcUXvL(elementTarget);
			int elementMapsWithElementTarget = GetElementMapsWithElementTarget(cyWFKpvAJpSlVUbktJsNPZKHVsyU2, actionId, skipDisabledMaps, results);
			cyWFKpvAJpSlVUbktJsNPZKHVsyU.GAJoHEqKKFTdYPaPYTwarurQvQhB(cyWFKpvAJpSlVUbktJsNPZKHVsyU2);
			return elementMapsWithElementTarget;
		}

		public int GetElementMapsWithElementTarget(ControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps, results);
		}

		public int GetElementMapsWithElementTarget(IControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			bool flag;
			return UDJQNhqhtojGfcxRhUYWnYqrkexm(elementTarget, true, actionId, skipDisabledMaps, results, false, out flag);
		}

		public int GetElementMapsWithElementTarget(IControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps, results);
		}

		public ActionElementMap GetFirstElementMapMatch(Predicate<ActionElementMap> predicate)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			return QUAvRrpyocavBEJosAIunPklcrkdA(predicate, false);
		}

		internal virtual ActionElementMap QUAvRrpyocavBEJosAIunPklcrkdA(Predicate<ActionElementMap> P_0, bool P_1)
		{
			return NlOdkJIfjvNbDEZkwSIosbBIquIP(P_0, P_1);
		}

		public int GetElementMapMatches(Predicate<ActionElementMap> predicate, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return ygvDdijRuLSzWpDZXtNNZKGfcQWiA(predicate, false, results, false);
		}

		internal virtual int ygvDdijRuLSzWpDZXtNNZKGfcQWiA(Predicate<ActionElementMap> P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			return rEwjYTvKvfXuntEkILnonDEYmxOq(P_0, P_1, P_2, P_3);
		}

		public void ForEachElementMapMatch(Predicate<ActionElementMap> predicate, Action<ActionElementMap> actionToPerform)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
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
			int count = goUXNJtKtwGJDoSIRfidQYIVOBlD.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					ActionElementMap obj = goUXNJtKtwGJDoSIRfidQYIVOBlD[i];
					if (predicate(obj))
					{
						actionToPerform(obj);
					}
				}
			}
			catch (Exception exception)
			{
				ReInput.HandleCallbackException("ControllerMap.ForEachElementMapMatch", exception);
			}
		}

		public virtual void ClearElementMaps()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return;
			}
			RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Clear();
			goUXNJtKtwGJDoSIRfidQYIVOBlD.Clear();
		}

		public int SetAllElementMapsEnabled(bool state)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			int num = 0;
			int count = goUXNJtKtwGJDoSIRfidQYIVOBlD.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = goUXNJtKtwGJDoSIRfidQYIVOBlD[i];
				if (actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo != state)
				{
					actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo = state;
					num++;
				}
			}
			return num;
		}

		public ActionElementMap GetButtonMap(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null || index < 0 || index >= RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count)
			{
				return null;
			}
			return RlEWvWhBtSYLCZgEJyoUhzhqfrnW[index];
		}

		public ActionElementMap[] GetButtonMaps()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.array;
			}
			return ListTools.ToArray(RlEWvWhBtSYLCZgEJyoUhzhqfrnW);
		}

		public ActionElementMap[] GetButtonMaps(bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.array;
			}
			int count = RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count;
			List<ActionElementMap> list = new List<ActionElementMap>(count);
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if (!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					list.Add(actionElementMap);
				}
			}
			return list.ToArray();
		}

		public int GetButtonMaps(bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return JxIhYzXNPIGZFEzAOAZDHnQnqhdl(skipDisabledMaps, results, false);
		}

		public ActionElementMap[] GetButtonMapsWithAction(string actionName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.array;
			}
			InputAction inputAction = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.cJlaptDDgHYEZdKZMGkPWYdKckQLA(actionName, true);
			if (inputAction == null)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			return GetButtonMapsWithAction(inputAction.id);
		}

		public ActionElementMap[] GetButtonMapsWithAction(int actionId)
		{
			return GetButtonMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public ActionElementMap[] GetButtonMapsWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.array;
			}
			InputAction inputAction = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.cJlaptDDgHYEZdKZMGkPWYdKckQLA(actionName, true);
			if (inputAction == null)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			return GetButtonMapsWithAction(inputAction.id, skipDisabledMaps);
		}

		public ActionElementMap[] GetButtonMapsWithAction(int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.array;
			}
			int num = buttonMapCount;
			if (num == 0)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if (actionElementMap._actionId == actionId && (!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
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
				ActionElementMap actionElementMap2 = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[j];
				if (actionElementMap2._actionId == actionId && (!skipDisabledMaps || actionElementMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					array[num3] = actionElementMap2;
					num3++;
				}
			}
			return array;
		}

		public int GetButtonMapsWithAction(string actionName, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			InputAction inputAction = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.cJlaptDDgHYEZdKZMGkPWYdKckQLA(actionName, true);
			if (inputAction == null)
			{
				ListTools.TryClear(results);
				return 0;
			}
			return GetButtonMapsWithAction(inputAction.id, results);
		}

		public int GetButtonMapsWithAction(int actionId, List<ActionElementMap> results)
		{
			return GetButtonMapsWithAction(actionId, skipDisabledMaps: false, results);
		}

		public int GetButtonMapsWithAction(string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			InputAction inputAction = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.cJlaptDDgHYEZdKZMGkPWYdKckQLA(actionName, true);
			if (inputAction == null)
			{
				ListTools.TryClear(results);
				return 0;
			}
			return GetButtonMapsWithAction(inputAction.id, skipDisabledMaps, results);
		}

		public int GetButtonMapsWithAction(int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return RccFssUlGIivCgGSJMuSdBNiEhGLA(actionId, skipDisabledMaps, results, false);
		}

		public IEnumerable<ActionElementMap> ButtonMapsWithAction(int actionId)
		{
			return ButtonMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public IEnumerable<ActionElementMap> ButtonMapsWithAction(string actionName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return ButtonMapsWithAction(actionId);
		}

		public IEnumerable<ActionElementMap> ButtonMapsWithAction(int actionId, bool skipDisabledMaps)
		{
			return new YrwLYGbpuIucBWiJclEEPbvXAsnG(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				jGepRyLmTEBLAXIsaiiTHGhibRuCA = actionId,
				MlWZQPQtLyJnETcXcYBEkbNZbonN = skipDisabledMaps
			};
		}

		public IEnumerable<ActionElementMap> ButtonMapsWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return ButtonMapsWithAction(actionId, skipDisabledMaps);
		}

		public ActionElementMap GetFirstButtonMapWithAction(int actionId)
		{
			return GetFirstButtonMapWithAction(actionId, skipDisabledMaps: false);
		}

		public ActionElementMap GetFirstButtonMapWithAction(string actionName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetFirstButtonMapWithAction(actionId);
		}

		public ActionElementMap GetFirstButtonMapWithAction(int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (actionId < 0)
			{
				return null;
			}
			IList<ActionElementMap> buttonMaps = ButtonMaps;
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				ActionElementMap actionElementMap = buttonMaps[i];
				if (actionElementMap._actionId == actionId && (!skipDisabledMaps || actionElementMap.enabled))
				{
					return actionElementMap;
				}
			}
			return null;
		}

		public ActionElementMap GetFirstButtonMapWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetFirstButtonMapWithAction(actionId, skipDisabledMaps);
		}

		public ActionElementMap GetFirstButtonMapMatch(Predicate<ActionElementMap> predicate)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			return NlOdkJIfjvNbDEZkwSIosbBIquIP(predicate, false);
		}

		internal ActionElementMap NlOdkJIfjvNbDEZkwSIosbBIquIP(Predicate<ActionElementMap> P_0, bool P_1)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (P_0 == null)
			{
				throw new ArgumentNullException("predicate");
			}
			IList<ActionElementMap> buttonMaps = ButtonMaps;
			int num = buttonMapCount;
			try
			{
				for (int i = 0; i < num; i++)
				{
					ActionElementMap actionElementMap = buttonMaps[i];
					if ((!P_1 || actionElementMap.enabled) && P_0(actionElementMap))
					{
						return actionElementMap;
					}
				}
			}
			catch (Exception exception)
			{
				ReInput.HandleCallbackException("ControllerMap.GetFirstButtonMapMatch", exception);
			}
			return null;
		}

		public int GetButtonMapMatches(Predicate<ActionElementMap> predicate, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return rEwjYTvKvfXuntEkILnonDEYmxOq(predicate, false, results, false);
		}

		internal int rEwjYTvKvfXuntEkILnonDEYmxOq(Predicate<ActionElementMap> P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
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
			IList<ActionElementMap> buttonMaps = ButtonMaps;
			int num2 = buttonMapCount;
			try
			{
				for (int i = 0; i < num2; i++)
				{
					ActionElementMap actionElementMap = buttonMaps[i];
					if ((!P_1 || actionElementMap.enabled) && P_0(actionElementMap))
					{
						P_2.Add(actionElementMap);
					}
				}
			}
			catch (Exception exception)
			{
				ReInput.HandleCallbackException("ControllerMap.GetButtonMapMatches", exception);
			}
			return P_2.Count - num;
		}

		public void ForEachButtonMapMatch(Predicate<ActionElementMap> predicate, Action<ActionElementMap> actionToPerform)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
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
			int count = RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					ActionElementMap obj = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
					if (predicate(obj))
					{
						actionToPerform(obj);
					}
				}
			}
			catch (Exception exception)
			{
				ReInput.HandleCallbackException("ControllerMap.GetButtonMapMatches", exception);
			}
		}

		public bool DeleteButtonMapsWithAction(string actionName)
		{
			return DeleteButtonMapsWithAction(ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName));
		}

		public bool DeleteButtonMapsWithAction(int actionId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (actionId < 0)
			{
				return false;
			}
			int num = buttonMapCount;
			if (num == 0)
			{
				return false;
			}
			bool result = false;
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[num2];
				if (actionElementMap != null && actionElementMap._actionId == actionId)
				{
					fGLHXijIihCDOAHYbiLOljyRdDgy(actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI, num2);
					result = true;
				}
			}
			return result;
		}

		public int SetAllButtonMapsEnabled(bool state)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			int num = 0;
			int count = RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if (actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo != state)
				{
					actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo = state;
					num++;
				}
			}
			return num;
		}

		public bool DoesElementAssignmentConflict(ControllerMap controllerMap)
		{
			return DoesElementAssignmentConflict(controllerMap, skipDisabledMaps: false);
		}

		public bool DoesElementAssignmentConflict(ActionElementMap actionElementMap)
		{
			return DoesElementAssignmentConflict(actionElementMap, skipDisabledMaps: false);
		}

		public bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck)
		{
			return DoesElementAssignmentConflict(conflictCheck, skipDisabledMaps: false);
		}

		public virtual bool DoesElementAssignmentConflict(ControllerMap controllerMap, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (controllerMap == null)
			{
				return false;
			}
			if (skipDisabledMaps && (!_enabled || !controllerMap._enabled))
			{
				return false;
			}
			if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return false;
			}
			IList<ActionElementMap> buttonMaps = controllerMap.ButtonMaps;
			if (buttonMaps == null)
			{
				return false;
			}
			int num = buttonMapCount;
			int count = buttonMaps.Count;
			for (int i = 0; i < num; i++)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if (skipDisabledMaps && !actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					continue;
				}
				for (int j = 0; j < count; j++)
				{
					ActionElementMap actionElementMap2 = buttonMaps[j];
					if ((!skipDisabledMaps || actionElementMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap != actionElementMap2 && actionElementMap.CheckForAssignmentConflict(actionElementMap2))
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual bool DoesElementAssignmentConflict(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (actionElementMap == null || RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return false;
			}
			if (skipDisabledMaps && (!_enabled || !actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
			{
				return false;
			}
			for (int i = 0; i < RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count; i++)
			{
				ActionElementMap actionElementMap2 = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if ((!skipDisabledMaps || actionElementMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap2 != actionElementMap && actionElementMap2.CheckForAssignmentConflict(actionElementMap))
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return false;
			}
			if (skipDisabledMaps && !_enabled)
			{
				return false;
			}
			if (conflictCheck.elementAssignmentType != ElementAssignmentType.Button && conflictCheck.elementAssignmentType != ElementAssignmentType.KeyboardKey)
			{
				return false;
			}
			ElementAssignment elementAssignment = conflictCheck.ToElementAssignment();
			for (int i = 0; i < RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count; i++)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if ((!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI != conflictCheck.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ControllerMap controllerMap)
		{
			return ElementAssignmentConflicts(controllerMap, skipDisabledMaps: false);
		}

		public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ActionElementMap actionElementMap)
		{
			return ElementAssignmentConflicts(actionElementMap, skipDisabledMaps: false);
		}

		public IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck)
		{
			return ElementAssignmentConflicts(conflictCheck, skipDisabledMaps: false);
		}

		public virtual IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ControllerMap controllerMap, bool skipDisabledMaps)
		{
			return new vymVLZALnkRCMreGHdTXWHcOlxCb(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				dAqaBgHEAvkbMVeRDCvJcJYeWvRO = controllerMap,
				MlWZQPQtLyJnETcXcYBEkbNZbonN = skipDisabledMaps
			};
		}

		public virtual IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			return new QcZlaGNElskgKdZhVjlJpFmmjzXn(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				tpItAifrGrjraEGilAOGvgJvUACH = actionElementMap,
				MlWZQPQtLyJnETcXcYBEkbNZbonN = skipDisabledMaps
			};
		}

		public virtual IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			return new urttbYsbfbHyZJcctshGYDrCqTuC(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				MelcAnrraPFJzeIaADSAOKRmqdwDb = conflictCheck,
				MlWZQPQtLyJnETcXcYBEkbNZbonN = skipDisabledMaps
			};
		}

		public int RemoveElementAssignmentConflicts(ControllerMap controllerMap)
		{
			return RemoveElementAssignmentConflicts(controllerMap, skipDisabledMaps: false);
		}

		public int RemoveElementAssignmentConflicts(ActionElementMap actionElementMap)
		{
			return RemoveElementAssignmentConflicts(actionElementMap, skipDisabledMaps: false);
		}

		public int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck)
		{
			return RemoveElementAssignmentConflicts(conflictCheck, skipDisabledMaps: false);
		}

		public virtual int RemoveElementAssignmentConflicts(ControllerMap controllerMap, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			if (controllerMap == null)
			{
				return 0;
			}
			if (skipDisabledMaps && (!_enabled || !controllerMap._enabled))
			{
				return 0;
			}
			int num = 0;
			if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return num;
			}
			IList<ActionElementMap> rlEWvWhBtSYLCZgEJyoUhzhqfrnW = controllerMap.RlEWvWhBtSYLCZgEJyoUhzhqfrnW;
			if (rlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return num;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory != null && !mapCategory.userAssignable)
			{
				return num;
			}
			_ = buttonMapCount;
			int count = rlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count;
			for (int num2 = RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[num2];
				if (!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					for (int i = 0; i < count; i++)
					{
						if ((!skipDisabledMaps || rlEWvWhBtSYLCZgEJyoUhzhqfrnW[i].kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.CheckForAssignmentConflict(rlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]))
						{
							fGLHXijIihCDOAHYbiLOljyRdDgy(actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI, num2);
							num++;
							break;
						}
					}
				}
			}
			return num;
		}

		public virtual int RemoveElementAssignmentConflicts(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			if (actionElementMap == null)
			{
				return 0;
			}
			if (skipDisabledMaps && (!_enabled || !actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
			{
				return 0;
			}
			int num = 0;
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory == null)
			{
				return num;
			}
			if (!mapCategory.userAssignable)
			{
				return num;
			}
			if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return num;
			}
			for (int num2 = RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap2 = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[num2];
				if ((!skipDisabledMaps || actionElementMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap2.CheckForAssignmentConflict(actionElementMap))
				{
					fGLHXijIihCDOAHYbiLOljyRdDgy(actionElementMap2.YVGDeWQAhUWKAOSPoxXuizkXaiTI, num2);
					num++;
				}
			}
			return num;
		}

		public virtual int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			if (skipDisabledMaps && !_enabled)
			{
				return 0;
			}
			if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return 0;
			}
			if (conflictCheck.elementAssignmentType != ElementAssignmentType.Button && conflictCheck.elementAssignmentType != ElementAssignmentType.KeyboardKey)
			{
				return 0;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory == null)
			{
				return 0;
			}
			if (!mapCategory.userAssignable)
			{
				return 0;
			}
			ElementAssignment elementAssignment = conflictCheck.ToElementAssignment();
			int num = 0;
			for (int num2 = RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[num2];
				if ((!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI != conflictCheck.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
				{
					fGLHXijIihCDOAHYbiLOljyRdDgy(actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI, num2);
					num++;
				}
			}
			return num;
		}

		public int DisableElementAssignmentConflicts(ControllerMap controllerMap)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return lpZmTcYQMeYBtGMoiEbkUblMviuS(controllerMap, false, null, false);
		}

		public int DisableElementAssignmentConflicts(ActionElementMap actionElementMap)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return lpZmTcYQMeYBtGMoiEbkUblMviuS(actionElementMap, false, null, false);
		}

		public int DisableElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return lpZmTcYQMeYBtGMoiEbkUblMviuS(conflictCheck, false, null, false);
		}

		public int DisableElementAssignmentConflicts(ControllerMap controllerMap, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return lpZmTcYQMeYBtGMoiEbkUblMviuS(controllerMap, skipDisabledMaps, null, false);
		}

		public int DisableElementAssignmentConflicts(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return lpZmTcYQMeYBtGMoiEbkUblMviuS(actionElementMap, skipDisabledMaps, null, false);
		}

		public int DisableElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return lpZmTcYQMeYBtGMoiEbkUblMviuS(conflictCheck, skipDisabledMaps, null, false);
		}

		internal virtual int lpZmTcYQMeYBtGMoiEbkUblMviuS(ControllerMap P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			if (P_2 != null && !P_3)
			{
				P_2.Clear();
			}
			if (P_0 == null)
			{
				return 0;
			}
			if (P_1 && (!_enabled || !P_0._enabled))
			{
				return 0;
			}
			int num = 0;
			if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return num;
			}
			IList<ActionElementMap> rlEWvWhBtSYLCZgEJyoUhzhqfrnW = P_0.RlEWvWhBtSYLCZgEJyoUhzhqfrnW;
			if (rlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return num;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory != null && !mapCategory.userAssignable)
			{
				return num;
			}
			int num2 = buttonMapCount;
			int count = rlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count;
			for (int i = 0; i < num2; i++)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if (!actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					continue;
				}
				for (int j = 0; j < count; j++)
				{
					ActionElementMap actionElementMap2 = rlEWvWhBtSYLCZgEJyoUhzhqfrnW[j];
					if ((!P_1 || actionElementMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.CheckForAssignmentConflict(actionElementMap2))
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

		internal virtual int lpZmTcYQMeYBtGMoiEbkUblMviuS(ActionElementMap P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			if (P_2 != null && !P_3)
			{
				P_2.Clear();
			}
			if (P_0 == null)
			{
				return 0;
			}
			if (P_1 && (!_enabled || !P_0.kKFZZElqKQSUFMZnWdEudRvTJGpo))
			{
				return 0;
			}
			int num = 0;
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
			int num2 = buttonMapCount;
			for (int i = 0; i < num2; i++)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if (actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo && P_0.CheckForAssignmentConflict(actionElementMap))
				{
					actionElementMap.enabled = false;
					P_2?.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		internal virtual int lpZmTcYQMeYBtGMoiEbkUblMviuS(ElementAssignmentConflictCheck P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			if (P_2 != null && !P_3)
			{
				P_2.Clear();
			}
			if (P_1 && !_enabled)
			{
				return 0;
			}
			if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return 0;
			}
			if (P_0.elementAssignmentType != ElementAssignmentType.Button && P_0.elementAssignmentType != ElementAssignmentType.KeyboardKey)
			{
				return 0;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory == null)
			{
				return 0;
			}
			if (!mapCategory.userAssignable)
			{
				return 0;
			}
			ElementAssignment elementAssignment = P_0.ToElementAssignment();
			int num = 0;
			int num2 = buttonMapCount;
			for (int i = 0; i < num2; i++)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if (actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo && actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI != P_0.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
				{
					actionElementMap.enabled = false;
					P_2?.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		public int ForEachElementAssignmentConflict(ControllerMap controllerMap, Action<ActionElementMap> actionToPerform)
		{
			return ForEachElementAssignmentConflict(controllerMap, actionToPerform, skipDisabledMaps: false);
		}

		public int ForEachElementAssignmentConflict(ActionElementMap actionElementMap, Action<ActionElementMap> actionToPerform)
		{
			return ForEachElementAssignmentConflict(actionElementMap, actionToPerform, skipDisabledMaps: false);
		}

		public int ForEachElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, Action<ActionElementMap> actionToPerform)
		{
			return ForEachElementAssignmentConflict(conflictCheck, actionToPerform, skipDisabledMaps: false);
		}

		public int ForEachElementAssignmentConflict(ControllerMap controllerMap, Action<ActionElementMap> actionToPerform, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			if (actionToPerform == null)
			{
				throw new ArgumentNullException("actionToPerform");
			}
			if (controllerMap == null)
			{
				return 0;
			}
			if (skipDisabledMaps && (!_enabled || !controllerMap._enabled))
			{
				return 0;
			}
			int num = 0;
			if (goUXNJtKtwGJDoSIRfidQYIVOBlD == null)
			{
				return num;
			}
			IList<ActionElementMap> list = controllerMap.goUXNJtKtwGJDoSIRfidQYIVOBlD;
			if (list == null)
			{
				return num;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory != null && !mapCategory.userAssignable)
			{
				return num;
			}
			int count = list.Count;
			for (int num2 = goUXNJtKtwGJDoSIRfidQYIVOBlD.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = goUXNJtKtwGJDoSIRfidQYIVOBlD[num2];
				if (!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					for (int i = 0; i < count; i++)
					{
						if ((!skipDisabledMaps || list[i].kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.CheckForAssignmentConflict(list[i]))
						{
							try
							{
								actionToPerform(actionElementMap);
							}
							catch (Exception exception)
							{
								ReInput.HandleCallbackException("ControllerMap.ForEachElementAssignmentConflict", exception);
								return num;
							}
							num++;
							break;
						}
					}
				}
			}
			return num;
		}

		public int ForEachElementAssignmentConflict(ActionElementMap actionElementMap, Action<ActionElementMap> actionToPerform, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			if (actionToPerform == null)
			{
				throw new ArgumentNullException("actionToPerform");
			}
			if (actionElementMap == null)
			{
				return 0;
			}
			if (skipDisabledMaps && (!_enabled || !actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
			{
				return 0;
			}
			int num = 0;
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory == null)
			{
				return num;
			}
			if (!mapCategory.userAssignable)
			{
				return num;
			}
			if (goUXNJtKtwGJDoSIRfidQYIVOBlD == null)
			{
				return num;
			}
			for (int num2 = goUXNJtKtwGJDoSIRfidQYIVOBlD.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap2 = goUXNJtKtwGJDoSIRfidQYIVOBlD[num2];
				if ((!skipDisabledMaps || actionElementMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap2.CheckForAssignmentConflict(actionElementMap))
				{
					try
					{
						actionToPerform(actionElementMap2);
					}
					catch (Exception exception)
					{
						ReInput.HandleCallbackException("ControllerMap.ForEachElementAssignmentConflict", exception);
						return num;
					}
					num++;
				}
			}
			return num;
		}

		public int ForEachElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, Action<ActionElementMap> actionToPerform, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			if (actionToPerform == null)
			{
				throw new ArgumentNullException("actionToPerform");
			}
			if (skipDisabledMaps && !_enabled)
			{
				return 0;
			}
			if (goUXNJtKtwGJDoSIRfidQYIVOBlD == null)
			{
				return 0;
			}
			if (conflictCheck.elementAssignmentType != ElementAssignmentType.Button && conflictCheck.elementAssignmentType != ElementAssignmentType.KeyboardKey)
			{
				return 0;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory == null)
			{
				return 0;
			}
			if (!mapCategory.userAssignable)
			{
				return 0;
			}
			ElementAssignment elementAssignment = conflictCheck.ToElementAssignment();
			int num = 0;
			for (int num2 = goUXNJtKtwGJDoSIRfidQYIVOBlD.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = goUXNJtKtwGJDoSIRfidQYIVOBlD[num2];
				if ((!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI != conflictCheck.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
				{
					try
					{
						actionToPerform(actionElementMap);
					}
					catch (Exception exception)
					{
						ReInput.HandleCallbackException("ControllerMap.ForEachElementAssignmentConflict", exception);
						return num;
					}
					num++;
				}
			}
			return num;
		}

		public string[] GetButtonNames()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<string>.array;
			}
			int num = buttonMapCount;
			if (num == 0)
			{
				return new string[0];
			}
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i].elementIdentifierName;
			}
			return array;
		}

		public string ToXmlString()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return string.Empty;
			}
			try
			{
				return JxowpgGeLUBIhbyiSIIfFkkGllDuA().ToXmlString(writeDocumentTag: true);
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to XML. " + ex.Message);
				return string.Empty;
			}
		}

		public string ToJsonString()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return string.Empty;
			}
			try
			{
				return JxowpgGeLUBIhbyiSIIfFkkGllDuA().ToJsonString();
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to JSON. " + ex.Message);
				return string.Empty;
			}
		}

		public ControllerTemplateMap ToControllerTemplateMap(Guid templateTypeGuid)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (controller == null)
			{
				Logger.LogError("The Controller Map is not associated with a Controller. This method can only be used with a Controller Map that is associated with a Controller.", requiredThreadSafety: true);
				return null;
			}
			IControllerTemplate controllerTemplate = controller.GetTemplate(templateTypeGuid) ?? (controller.GetTemplate(templateTypeGuid) as ControllerTemplate);
			if (controllerTemplate == null)
			{
				HardwareJoystickTemplateMap hardwareJoystickTemplateMap = ReInput.rfsEypEpDSCojIvJWQqUMoXAhWHO(templateTypeGuid);
				string text = ((hardwareJoystickTemplateMap != null) ? hardwareJoystickTemplateMap.ClassName : templateTypeGuid.ToString());
				Logger.LogError("The Controller does not implement " + text + ".", requiredThreadSafety: true);
				return null;
			}
			return ControllerTemplateMap.FSIRgQmHZEfgkCSfmcoeDxGPUtlh(controllerTemplate, this);
		}

		public ControllerTemplateMap ToControllerTemplateMap<T>() where T : class
		{
			return ToControllerTemplateMap(typeof(T));
		}

		public ControllerTemplateMap ToControllerTemplateMap(Type templateInterfaceType)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if ((object)templateInterfaceType == null)
			{
				throw new ArgumentNullException("templateInterfaceType");
			}
			if (controller == null)
			{
				Logger.LogError("The Controller Map is not associated with a Controller. This method can only be used with a Controller Map that is associated with a Controller.", requiredThreadSafety: true);
				return null;
			}
			IControllerTemplate controllerTemplate = controller.GetTemplate(templateInterfaceType) ?? (controller.GetTemplate(templateInterfaceType) as ControllerTemplate);
			if (controllerTemplate == null)
			{
				Logger.LogError("The Controller does not implement " + templateInterfaceType.Name + ".", requiredThreadSafety: true);
				return null;
			}
			return ControllerTemplateMap.FSIRgQmHZEfgkCSfmcoeDxGPUtlh(controllerTemplate, this);
		}

		private ControllerTemplateMap nLmdNYRQIpjhrFVmXAfEyjhKKdgyA(IControllerTemplate P_0)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (P_0 == null)
			{
				throw new ArgumentNullException("controllerTemplate");
			}
			return ControllerTemplateMap.FSIRgQmHZEfgkCSfmcoeDxGPUtlh(P_0, this);
		}

		internal virtual bool lqUvlJMImEtcvtFlVYWmonZgLfZC(ActionElementMap P_0)
		{
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_0._elementType))
			{
				return false;
			}
			WyzBpHpqqzkYVfNdqvKJfqBDGXMO(P_0);
			return true;
		}

		internal virtual int SfYThgjCbGGBVaAmuUBhxMEDxCeO(List<ActionElementMap> P_0, bool P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("results");
			}
			int count = P_0.Count;
			int count2 = RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Count;
			for (int i = 0; i < count2; i++)
			{
				if (!P_1 || RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i].kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					P_0.Add(RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]);
				}
			}
			return P_0.Count - count;
		}

		internal virtual ActionElementMap hOxSQVcwNejfCacLSpeCVuEITowP(int P_0, int P_1, ControllerElementType P_2)
		{
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_2))
			{
				return null;
			}
			int num = jItBPCmwrRbFUjcSXfytsGHrQKcIA(P_0, P_1, P_2);
			if (num < 0)
			{
				return null;
			}
			return RlEWvWhBtSYLCZgEJyoUhzhqfrnW[num];
		}

		internal virtual int ULJHbsZFwKdaRYbNmcqpVsYjydsr(int P_0, List<ActionElementMap> P_1, bool P_2)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("results");
			}
			int num = 0;
			if (!P_2)
			{
				P_1.Clear();
			}
			else
			{
				num = P_1.Count;
			}
			if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return 0;
			}
			int num2 = buttonMapCount;
			for (int i = 0; i < num2; i++)
			{
				if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]._elementIdentifierId == P_0)
				{
					P_1.Add(RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]);
				}
			}
			return P_1.Count - num;
		}

		internal virtual bool OSxfPrgbMEYNaJmXwwncKGNUvVZj(int P_0, int P_1, ControllerElementType P_2)
		{
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_2))
			{
				return false;
			}
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]._elementIdentifierId == P_0 && RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]._actionId == P_1)
				{
					return true;
				}
			}
			return false;
		}

		internal virtual int jItBPCmwrRbFUjcSXfytsGHrQKcIA(int P_0, int P_1, ControllerElementType P_2)
		{
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_2))
			{
				return -1;
			}
			if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return -1;
			}
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]._elementIdentifierId == P_0 && RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]._actionId == P_1)
				{
					return i;
				}
			}
			return -1;
		}

		internal int tJMxlFgPSSwZhsmmUcyYlWrSITXL(int P_0)
		{
			if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW == null)
			{
				return -1;
			}
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i].YVGDeWQAhUWKAOSPoxXuizkXaiTI == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		internal int JxIhYzXNPIGZFEzAOAZDHnQnqhdl(bool P_0, List<ActionElementMap> P_1, bool P_2)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("results");
			}
			if (!P_2)
			{
				P_1.Clear();
			}
			int num = buttonMapCount;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if (!P_0 || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					P_1.Add(actionElementMap);
					num2++;
				}
			}
			return num2;
		}

		internal int RccFssUlGIivCgGSJMuSdBNiEhGLA(int P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			if (P_2 == null)
			{
				throw new ArgumentNullException("results");
			}
			if (!P_3)
			{
				P_2.Clear();
			}
			int num = buttonMapCount;
			if (num == 0)
			{
				return 0;
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if (actionElementMap._actionId == P_0 && (!P_1 || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					P_2.Add(actionElementMap);
					num2++;
				}
			}
			return num2;
		}

		internal virtual int wpGLUOiKKYaZujmxopRHHGStkBpW(int P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
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
			int num2 = buttonMapCount;
			for (int i = 0; i < num2; i++)
			{
				ActionElementMap actionElementMap = RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				if (actionElementMap._actionId == P_0 && (!P_1 || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					P_2.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		internal virtual ActionElementMap EUcSmaxDeldMRvTJKrymSFSbIMkm(IControllerElementTarget P_0, bool P_1, int P_2, bool P_3, out bool P_4)
		{
			P_4 = false;
			if (P_1 && P_2 < 0)
			{
				P_4 = true;
				return null;
			}
			if (!yCOuaObPHCTFSJWZalIFHqzWIxiT(P_0))
			{
				P_4 = true;
				return null;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_0.elementType))
			{
				return null;
			}
			int num = buttonMapCount;
			_ = P_0.elementIdentifierId;
			for (int i = 0; i < num; i++)
			{
				if ((!P_1 || RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]._actionId == P_2) && (!P_3 || RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i].kKFZZElqKQSUFMZnWdEudRvTJGpo) && RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i].IsTarget(P_0))
				{
					return RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i];
				}
			}
			return null;
		}

		internal virtual int UDJQNhqhtojGfcxRhUYWnYqrkexm(IControllerElementTarget P_0, bool P_1, int P_2, bool P_3, List<ActionElementMap> P_4, bool P_5, out bool P_6)
		{
			if (P_4 == null)
			{
				throw new ArgumentNullException("results");
			}
			int num = 0;
			if (!P_5)
			{
				P_4.Clear();
			}
			P_6 = false;
			if (P_1 && P_2 < 0)
			{
				P_6 = true;
				return num;
			}
			if (!yCOuaObPHCTFSJWZalIFHqzWIxiT(P_0))
			{
				P_6 = true;
				return num;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_0.elementType))
			{
				return num;
			}
			int num2 = buttonMapCount;
			_ = P_0.elementIdentifierId;
			for (int i = 0; i < num2; i++)
			{
				if ((!P_1 || RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]._actionId == P_2) && (!P_3 || RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i].kKFZZElqKQSUFMZnWdEudRvTJGpo) && RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i].IsTarget(P_0))
				{
					P_4.Add(RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i]);
					num++;
				}
			}
			return num;
		}

		internal void XcTGcHaOmkURzedtWJpvJneMEfqxA(int P_0, ControllerElementType P_1)
		{
			ActionElementMap elementMap = GetElementMap(P_0);
			if (elementMap != null && elementMap._elementType != P_1)
			{
				elementMap._elementType = P_1;
				if (P_1 == ControllerElementType.Button)
				{
					elementMap._axisRange = AxisRange.Full;
					elementMap._invert = false;
				}
				DeleteElementMap(P_0);
				ZxwtLymUXBfmZpaPpFNVkvIPfZZDA(elementMap);
			}
		}

		internal virtual bool ZxwtLymUXBfmZpaPpFNVkvIPfZZDA(ActionElementMap P_0)
		{
			if (P_0 == null)
			{
				return false;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_0._elementType))
			{
				return false;
			}
			RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Add(P_0);
			mHtfYeiNvxekaJaWFKccGvJaIkiD(P_0);
			return true;
		}

		internal bool yCOuaObPHCTFSJWZalIFHqzWIxiT(IControllerElementTarget P_0)
		{
			if (P_0 == null)
			{
				return false;
			}
			Controller controller = P_0.controller;
			if (controller == null || controller.type != _controllerType || controller.id != _controllerId)
			{
				return false;
			}
			return true;
		}

		internal bool PORfKBCFZnGGqUkVPSzMjLBhaSoKA(string P_0)
		{
			try
			{
				ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject.FromXml(GetType(), P_0));
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogError("Error creating  " + GetType().Name + "  from XML. " + ex.Message);
				return false;
			}
		}

		internal bool BnBhHUJyVcVYPmJYOVmdkJUPHIIK(string P_0)
		{
			try
			{
				ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject.FromJson(GetType(), P_0));
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogError("Error creating  " + GetType().Name + "  from JSON. " + ex.Message);
				return false;
			}
		}

		internal void mHtfYeiNvxekaJaWFKccGvJaIkiD(ActionElementMap P_0)
		{
			if (P_0 != null)
			{
				goUXNJtKtwGJDoSIRfidQYIVOBlD.Add(P_0);
				goUXNJtKtwGJDoSIRfidQYIVOBlD.Sort(eKoFSWJFUhkTGBpYHAfEqNRdvkyzA.vhJlHqbDISDRgELdbflITCiWLdGrA);
			}
		}

		internal void cmioCUaiaDFNaRGdRlMmfdBtuqvk(int P_0)
		{
			int num = ejJQAWluhtdimTLCBSOKVGHZYvGT(P_0);
			if (num >= 0)
			{
				goUXNJtKtwGJDoSIRfidQYIVOBlD.RemoveAt(num);
			}
		}

		internal void lAPTPcDPxSWjnbfFbRfkhyVfqMQV(int P_0, ActionElementMap P_1)
		{
			if (P_1 != null)
			{
				int num = ejJQAWluhtdimTLCBSOKVGHZYvGT(P_0);
				if (num >= 0)
				{
					goUXNJtKtwGJDoSIRfidQYIVOBlD[num] = P_1;
					goUXNJtKtwGJDoSIRfidQYIVOBlD.Sort(eKoFSWJFUhkTGBpYHAfEqNRdvkyzA.vhJlHqbDISDRgELdbflITCiWLdGrA);
				}
			}
		}

		internal static void OgCdCocQpxWwVEWsnTRcismHoNbJ(ActionElementMap P_0, int P_1, Pole P_2, int P_3, ControllerElementType P_4, AxisRange P_5, bool P_6)
		{
			P_0.SPGTRPyvIslcMdbPTItsewSLRPxx();
			P_0._actionId = P_1;
			P_0._elementType = P_4;
			P_0._elementIdentifierId = P_3;
			P_0._axisContribution = P_2;
			P_0._axisRange = P_5;
			if (P_4 == ControllerElementType.Axis)
			{
				P_0._invert = P_6;
			}
		}

		protected void BakeElementMap(ActionElementMap map)
		{
			if (map != null)
			{
				ReInput.controllers.GetController(_controllerType, _controllerId)?.JpxRPMmkCiJxotCLchUqPLcjlZiQ(this, map);
			}
		}

		internal virtual bool ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject P_0)
		{
			bool flag = false;
			_sourceMapId = -1;
			_categoryId = -1;
			_layoutId = -1;
			_name = string.Empty;
			_hardwareGuid = Guid.Empty;
			_enabled = true;
			P_0.TryGetDeserializedValueByRef("sourceMapId", ref _sourceMapId);
			P_0.TryGetDeserializedValueByRef("categoryId", ref _categoryId);
			P_0.TryGetDeserializedValueByRef("layoutId", ref _layoutId);
			P_0.TryGetDeserializedValueByRef("name", ref _name);
			P_0.TryGetDeserializedValueByRef("hardwareGuid", ref _hardwareGuid);
			P_0.TryGetDeserializedValueByRef("enabled", ref _enabled);
			if (!flag)
			{
				ClearElementMaps();
				flag = true;
			}
			SerializedObject value = null;
			if (P_0.TryGetDeserializedValueByRef("buttonMaps", ref value) && value != null)
			{
				for (int i = 0; i < value.count; i++)
				{
					if (value.TryGetDeserializedValue<SerializedObject>(i, out var value2) || value2 == null)
					{
						ActionElementMap actionElementMap = new ActionElementMap();
						actionElementMap.ckPgJNiFQjrLdKfwClUsdsySXFyIb(value2);
						if (ActionElementMap.QCVAmAdeLePlbTxUIzzPjfmaVeTVA(actionElementMap))
						{
							WyzBpHpqqzkYVfNdqvKJfqBDGXMO(actionElementMap);
						}
					}
				}
			}
			return flag;
		}

		internal virtual void eqxcMNQhZrhfftsCuDbhlTUMXCQB(SerializedObject P_0)
		{
			if (P_0.xmlInfo == null)
			{
				P_0.xmlInfo = new SerializedObject.XmlInfo();
			}
			P_0.Add("dataVersion", 2, SerializedObject.FieldOptions.ExculdeFromXml);
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "dataVersion",
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = 2.ToString()
			});
			if ((object)GetType() == typeof(JoystickMap))
			{
				Joystick joystick = ReInput.controllers.GetJoystick(_controllerId);
				Guid guid = joystick?.hardwareTypeGuid ?? Guid.Empty;
				string yYOUbwIbBcyPAQvjeAEXVsjzLAln = ((joystick != null) ? SerializationTools.CleanInvalidXmlChars(joystick.hardwareName) : "Unknown");
				P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
				{
					OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "hardwareGuid",
					yYOUbwIbBcyPAQvjeAEXVsjzLAln = guid.ToString()
				});
				P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
				{
					OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "hardwareName",
					yYOUbwIbBcyPAQvjeAEXVsjzLAln = yYOUbwIbBcyPAQvjeAEXVsjzLAln
				});
			}
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				yCwJMdebpVsoAZwRHxyMbDxHYvvT = "xmlns",
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "xsi",
				ZVXfpRlSPjKWRYaEYlEnqADXJiAs = null,
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = "http://www.w3.org/2001/XMLSchema-instance"
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.nIytNPQltHNThjHAgjtVciMQQyqsA
			{
				yCwJMdebpVsoAZwRHxyMbDxHYvvT = "xsi",
				OvVRJOklVDAaDOOGrXwIFuDdpwVJ = "schemaLocation",
				ZVXfpRlSPjKWRYaEYlEnqADXJiAs = null,
				yYOUbwIbBcyPAQvjeAEXVsjzLAln = string.Format("{0} {1}{2}{3}{4}{5}", "http://guavaman.com/rewired", "http://guavaman.com/schemas/rewired/", "1.1", "/", GetType().Name, ".xsd")
			});
			P_0.Add("sourceMapId", _sourceMapId);
			P_0.Add("categoryId", _categoryId);
			P_0.Add("layoutId", _layoutId);
			P_0.Add("name", _name);
			P_0.Add("hardwareGuid", _hardwareGuid);
			P_0.Add("enabled", _enabled);
			int num = buttonMapCount;
			List<object> list = new List<object>();
			P_0.Add("buttonMaps", list);
			for (int i = 0; i < num; i++)
			{
				if (RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i] != null)
				{
					list.Add(RlEWvWhBtSYLCZgEJyoUhzhqfrnW[i].JxowpgGeLUBIhbyiSIIfFkkGllDuA());
				}
			}
		}

		private bool kTJfWCbzqEHPgeOYseEucoxIAjYkB(ControllerElementType P_0)
		{
			if (P_0 != ControllerElementType.Button)
			{
				return false;
			}
			return true;
		}

		private void fGLHXijIihCDOAHYbiLOljyRdDgy(int P_0, int P_1)
		{
			cmioCUaiaDFNaRGdRlMmfdBtuqvk(P_0);
			if (P_1 >= 0 && P_1 < buttonMapCount)
			{
				RlEWvWhBtSYLCZgEJyoUhzhqfrnW.RemoveAt(P_1);
			}
		}

		private void WyzBpHpqqzkYVfNdqvKJfqBDGXMO(ActionElementMap P_0)
		{
			if (P_0 != null)
			{
				RlEWvWhBtSYLCZgEJyoUhzhqfrnW.Add(P_0);
				mHtfYeiNvxekaJaWFKccGvJaIkiD(P_0);
			}
		}

		private void WPLdldHFDQtgOkYRMaKlCNKkHVilB(ActionElementMap P_0, int P_1)
		{
			if (P_0 != null && P_1 >= 0 && P_1 < buttonMapCount)
			{
				lAPTPcDPxSWjnbfFbRfkhyVfqMQV(RlEWvWhBtSYLCZgEJyoUhzhqfrnW[P_1].YVGDeWQAhUWKAOSPoxXuizkXaiTI, P_0);
				RlEWvWhBtSYLCZgEJyoUhzhqfrnW[P_1] = P_0;
			}
		}

		private int ejJQAWluhtdimTLCBSOKVGHZYvGT(int P_0)
		{
			if (goUXNJtKtwGJDoSIRfidQYIVOBlD == null)
			{
				return -1;
			}
			int count = goUXNJtKtwGJDoSIRfidQYIVOBlD.Count;
			for (int i = 0; i < count; i++)
			{
				if (goUXNJtKtwGJDoSIRfidQYIVOBlD[i].YVGDeWQAhUWKAOSPoxXuizkXaiTI == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		private SerializedObject JxowpgGeLUBIhbyiSIIfFkkGllDuA()
		{
			SerializedObject serializedObject = new SerializedObject(GetType(), SerializedObject.ObjectType.Object);
			eqxcMNQhZrhfftsCuDbhlTUMXCQB(serializedObject);
			return serializedObject;
		}

		internal static ControllerMap lOzmglNwrCkddSvkHTjSvLufiURJ(ControllerType P_0)
		{
			return P_0 switch
			{
				ControllerType.Keyboard => new KeyboardMap(), 
				ControllerType.Mouse => new MouseMap(), 
				ControllerType.Joystick => new JoystickMap(), 
				ControllerType.Custom => new CustomControllerMap(), 
				_ => throw new NotImplementedException(), 
			};
		}

		internal static ControllerMap JGdwEkTXDISoEvRBhOWRtTDetjzA(Controller P_0, int P_1, int P_2)
		{
			if (P_0 == null)
			{
				return null;
			}
			return P_0.type switch
			{
				ControllerType.Keyboard => KeyboardMap.JGdwEkTXDISoEvRBhOWRtTDetjzA(P_0.hardwareTypeGuid, P_1, P_2), 
				ControllerType.Mouse => MouseMap.JGdwEkTXDISoEvRBhOWRtTDetjzA(P_0.hardwareTypeGuid, P_1, P_2), 
				ControllerType.Joystick => JoystickMap.JGdwEkTXDISoEvRBhOWRtTDetjzA(P_0.hardwareTypeGuid, P_1, P_2), 
				ControllerType.Custom => CustomControllerMap.JGdwEkTXDISoEvRBhOWRtTDetjzA(P_0.hardwareTypeGuid, ((CustomController)P_0).sourceControllerId, P_1, P_2), 
				_ => throw new NotImplementedException(), 
			};
		}

		public static ControllerMap CreateFromXml(ControllerType controllerType, string xmlString)
		{
			if (string.IsNullOrEmpty(xmlString))
			{
				return null;
			}
			ControllerMap controllerMap = lOzmglNwrCkddSvkHTjSvLufiURJ(controllerType);
			try
			{
				controllerMap.PORfKBCFZnGGqUkVPSzMjLBhaSoKA(xmlString);
				return controllerMap;
			}
			catch
			{
				return null;
			}
		}

		public static ControllerMap CreateFromJson(ControllerType controllerType, string jsonString)
		{
			if (string.IsNullOrEmpty(jsonString))
			{
				return null;
			}
			ControllerMap controllerMap = lOzmglNwrCkddSvkHTjSvLufiURJ(controllerType);
			try
			{
				controllerMap.BnBhHUJyVcVYPmJYOVmdkJUPHIIK(jsonString);
				return controllerMap;
			}
			catch
			{
				return null;
			}
		}
	}
}
