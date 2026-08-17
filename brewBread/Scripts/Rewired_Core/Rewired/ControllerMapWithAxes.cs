using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired
{
	public abstract class ControllerMapWithAxes : ControllerMap
	{
		private sealed class VPhATKGpdkvWSXuTTZMhgpnkgDIA : IDisposable, IEnumerable, IEnumerable<ActionElementMap>, IEnumerator, IEnumerator<ActionElementMap>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ActionElementMap VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerMapWithAxes TtytLoUfsgUyhsklaKccrnoMiiek;

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
			public VPhATKGpdkvWSXuTTZMhgpnkgDIA(int P_0)
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
					ControllerMapWithAxes ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
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
						if (tlJxlOEWqQEUYpNGSwfnmndnnqSe < 0)
						{
							return false;
						}
						hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.AxisMaps.GetEnumerator();
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
				VPhATKGpdkvWSXuTTZMhgpnkgDIA vPhATKGpdkvWSXuTTZMhgpnkgDIA;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					vPhATKGpdkvWSXuTTZMhgpnkgDIA = this;
				}
				else
				{
					vPhATKGpdkvWSXuTTZMhgpnkgDIA = new VPhATKGpdkvWSXuTTZMhgpnkgDIA(0);
					vPhATKGpdkvWSXuTTZMhgpnkgDIA.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				vPhATKGpdkvWSXuTTZMhgpnkgDIA.tlJxlOEWqQEUYpNGSwfnmndnnqSe = jGepRyLmTEBLAXIsaiiTHGhibRuCA;
				vPhATKGpdkvWSXuTTZMhgpnkgDIA.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
				return vPhATKGpdkvWSXuTTZMhgpnkgDIA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
			}
		}

		private sealed class XkWmBQqudyLAVZPWUGNOmBnibycE : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerMapWithAxes TtytLoUfsgUyhsklaKccrnoMiiek;

			private ControllerMap yaBNguSowjFppZBFNMqWwGMqKjjx;

			public ControllerMap dAqaBgHEAvkbMVeRDCvJcJYeWvRO;

			private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

			public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

			private IList<ActionElementMap> FxsEOkXUkSAvirAKscEjIknCWwdp;

			private int JAuRFibtNVayLydiGDvmbhSedpqr;

			private IEnumerator<ElementAssignmentConflictInfo> ffMVLALuuURkVdxHSgrNcNQkUFZS;

			private int ytncSRhcFRbRpstpicuZASIlEEjc;

			private ActionElementMap qIXRwrCCaihDrXmxLOznIjkkTjX;

			private int LUpfPSueQCzKPBAEBjjhIwROENlD;

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
			public XkWmBQqudyLAVZPWUGNOmBnibycE(int P_0)
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
					ControllerMapWithAxes ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
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
						if (yaBNguSowjFppZBFNMqWwGMqKjjx == null)
						{
							return false;
						}
						ffMVLALuuURkVdxHSgrNcNQkUFZS = ((ControllerMap)ttytLoUfsgUyhsklaKccrnoMiiek).ElementAssignmentConflicts(yaBNguSowjFppZBFNMqWwGMqKjjx, HIuFjgoQMMeZHtUafJbERKutTjQt).GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_00af;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_00af;
					case 2:
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							goto IL_0232;
						}
						IL_0244:
						if (LUpfPSueQCzKPBAEBjjhIwROENlD < JAuRFibtNVayLydiGDvmbhSedpqr)
						{
							ActionElementMap actionElementMap = FxsEOkXUkSAvirAKscEjIknCWwdp[LUpfPSueQCzKPBAEBjjhIwROENlD];
							if ((!HIuFjgoQMMeZHtUafJbERKutTjQt || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo) && qIXRwrCCaihDrXmxLOznIjkkTjX.CheckForAssignmentConflict(actionElementMap))
							{
								VqEePGSMyrGKIqkWibsjeHcWPSIx = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(ttytLoUfsgUyhsklaKccrnoMiiek._categoryId).userAssignable, -1, ttytLoUfsgUyhsklaKccrnoMiiek._controllerType, ttytLoUfsgUyhsklaKccrnoMiiek._controllerId, ttytLoUfsgUyhsklaKccrnoMiiek._id, qIXRwrCCaihDrXmxLOznIjkkTjX.YVGDeWQAhUWKAOSPoxXuizkXaiTI, qIXRwrCCaihDrXmxLOznIjkkTjX._actionId, qIXRwrCCaihDrXmxLOznIjkkTjX._elementType, qIXRwrCCaihDrXmxLOznIjkkTjX._elementIdentifierId, qIXRwrCCaihDrXmxLOznIjkkTjX.keyCode, qIXRwrCCaihDrXmxLOznIjkkTjX.modifierKeyFlags);
								RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
								return true;
							}
							goto IL_0232;
						}
						qIXRwrCCaihDrXmxLOznIjkkTjX = null;
						goto IL_025c;
						IL_0232:
						LUpfPSueQCzKPBAEBjjhIwROENlD++;
						goto IL_0244;
						IL_026e:
						if (ytncSRhcFRbRpstpicuZASIlEEjc < ttytLoUfsgUyhsklaKccrnoMiiek.JfTobMsTRweaeWanKTfLHPJFqbSS.Count)
						{
							qIXRwrCCaihDrXmxLOznIjkkTjX = ttytLoUfsgUyhsklaKccrnoMiiek.JfTobMsTRweaeWanKTfLHPJFqbSS[ytncSRhcFRbRpstpicuZASIlEEjc];
							if (!HIuFjgoQMMeZHtUafJbERKutTjQt || qIXRwrCCaihDrXmxLOznIjkkTjX.kKFZZElqKQSUFMZnWdEudRvTJGpo)
							{
								LUpfPSueQCzKPBAEBjjhIwROENlD = 0;
								goto IL_0244;
							}
							goto IL_025c;
						}
						return false;
						IL_00af:
						if (ffMVLALuuURkVdxHSgrNcNQkUFZS.MoveNext())
						{
							ElementAssignmentConflictInfo current = ffMVLALuuURkVdxHSgrNcNQkUFZS.Current;
							VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
							RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
							return true;
						}
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
						ffMVLALuuURkVdxHSgrNcNQkUFZS = null;
						if (!(yaBNguSowjFppZBFNMqWwGMqKjjx is ControllerMapWithAxes controllerMapWithAxes))
						{
							return false;
						}
						if (HIuFjgoQMMeZHtUafJbERKutTjQt && (!ttytLoUfsgUyhsklaKccrnoMiiek._enabled || !controllerMapWithAxes._enabled))
						{
							return false;
						}
						FxsEOkXUkSAvirAKscEjIknCWwdp = controllerMapWithAxes.AxisMaps;
						if (FxsEOkXUkSAvirAKscEjIknCWwdp == null)
						{
							return false;
						}
						JAuRFibtNVayLydiGDvmbhSedpqr = FxsEOkXUkSAvirAKscEjIknCWwdp.Count;
						ytncSRhcFRbRpstpicuZASIlEEjc = 0;
						goto IL_026e;
						IL_025c:
						ytncSRhcFRbRpstpicuZASIlEEjc++;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ffMVLALuuURkVdxHSgrNcNQkUFZS != null)
				{
					ffMVLALuuURkVdxHSgrNcNQkUFZS.Dispose();
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
				XkWmBQqudyLAVZPWUGNOmBnibycE xkWmBQqudyLAVZPWUGNOmBnibycE;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					xkWmBQqudyLAVZPWUGNOmBnibycE = this;
				}
				else
				{
					xkWmBQqudyLAVZPWUGNOmBnibycE = new XkWmBQqudyLAVZPWUGNOmBnibycE(0);
					xkWmBQqudyLAVZPWUGNOmBnibycE.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				xkWmBQqudyLAVZPWUGNOmBnibycE.yaBNguSowjFppZBFNMqWwGMqKjjx = dAqaBgHEAvkbMVeRDCvJcJYeWvRO;
				xkWmBQqudyLAVZPWUGNOmBnibycE.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
				return xkWmBQqudyLAVZPWUGNOmBnibycE;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private sealed class CODHjysPShNccvoajGrJuLQyLgXp : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerMapWithAxes TtytLoUfsgUyhsklaKccrnoMiiek;

			private ActionElementMap QSqCmiOFzeNqpmkBXQgKFemLcIUCA;

			public ActionElementMap tpItAifrGrjraEGilAOGvgJvUACH;

			private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

			public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

			private IEnumerator<ElementAssignmentConflictInfo> hSeONskmQJhVsjVUHGUASYbDGJrU;

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
			public CODHjysPShNccvoajGrJuLQyLgXp(int P_0)
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
					ControllerMapWithAxes ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
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
						if (QSqCmiOFzeNqpmkBXQgKFemLcIUCA == null)
						{
							return false;
						}
						hSeONskmQJhVsjVUHGUASYbDGJrU = ((ControllerMap)ttytLoUfsgUyhsklaKccrnoMiiek).ElementAssignmentConflicts(QSqCmiOFzeNqpmkBXQgKFemLcIUCA, HIuFjgoQMMeZHtUafJbERKutTjQt).GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_00ad;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_00ad;
					case 2:
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							goto IL_01a9;
						}
						IL_00ad:
						if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
						{
							ElementAssignmentConflictInfo current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
							VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
							RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
							return true;
						}
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
						hSeONskmQJhVsjVUHGUASYbDGJrU = null;
						if (HIuFjgoQMMeZHtUafJbERKutTjQt && (!ttytLoUfsgUyhsklaKccrnoMiiek._enabled || !QSqCmiOFzeNqpmkBXQgKFemLcIUCA.kKFZZElqKQSUFMZnWdEudRvTJGpo))
						{
							return false;
						}
						if (ttytLoUfsgUyhsklaKccrnoMiiek.JfTobMsTRweaeWanKTfLHPJFqbSS == null)
						{
							return false;
						}
						fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
						goto IL_01bb;
						IL_01bb:
						if (fIMVaffCgsuIJcnrkMmGGKfPwwel < ttytLoUfsgUyhsklaKccrnoMiiek.JfTobMsTRweaeWanKTfLHPJFqbSS.Count)
						{
							ActionElementMap actionElementMap = ttytLoUfsgUyhsklaKccrnoMiiek.JfTobMsTRweaeWanKTfLHPJFqbSS[fIMVaffCgsuIJcnrkMmGGKfPwwel];
							if ((!HIuFjgoQMMeZHtUafJbERKutTjQt || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.CheckForAssignmentConflict(QSqCmiOFzeNqpmkBXQgKFemLcIUCA))
							{
								VqEePGSMyrGKIqkWibsjeHcWPSIx = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(ttytLoUfsgUyhsklaKccrnoMiiek._categoryId).userAssignable, -1, ttytLoUfsgUyhsklaKccrnoMiiek._controllerType, ttytLoUfsgUyhsklaKccrnoMiiek._controllerId, ttytLoUfsgUyhsklaKccrnoMiiek._id, actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI, actionElementMap._actionId, actionElementMap._elementType, actionElementMap._elementIdentifierId, actionElementMap.keyCode, actionElementMap.modifierKeyFlags);
								RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
								return true;
							}
							goto IL_01a9;
						}
						return false;
						IL_01a9:
						fIMVaffCgsuIJcnrkMmGGKfPwwel++;
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
			IEnumerator<ElementAssignmentConflictInfo> IEnumerable<ElementAssignmentConflictInfo>.GetEnumerator()
			{
				CODHjysPShNccvoajGrJuLQyLgXp cODHjysPShNccvoajGrJuLQyLgXp;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					cODHjysPShNccvoajGrJuLQyLgXp = this;
				}
				else
				{
					cODHjysPShNccvoajGrJuLQyLgXp = new CODHjysPShNccvoajGrJuLQyLgXp(0);
					cODHjysPShNccvoajGrJuLQyLgXp.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				cODHjysPShNccvoajGrJuLQyLgXp.QSqCmiOFzeNqpmkBXQgKFemLcIUCA = tpItAifrGrjraEGilAOGvgJvUACH;
				cODHjysPShNccvoajGrJuLQyLgXp.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
				return cODHjysPShNccvoajGrJuLQyLgXp;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private sealed class cLadcmcGlWfjItVAJYHyrETEsSMOA : IDisposable, IEnumerable, IEnumerator, IEnumerable<ElementAssignmentConflictInfo>, IEnumerator<ElementAssignmentConflictInfo>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ElementAssignmentConflictInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerMapWithAxes TtytLoUfsgUyhsklaKccrnoMiiek;

			private ElementAssignmentConflictCheck HHyftJlGnirHuGQIQnIQkaXvgYegA;

			public ElementAssignmentConflictCheck MelcAnrraPFJzeIaADSAOKRmqdwDb;

			private bool HIuFjgoQMMeZHtUafJbERKutTjQt;

			public bool MlWZQPQtLyJnETcXcYBEkbNZbonN;

			private ElementAssignment FnvvcrDffLPIUvRKBIfBFqTiuZyQ;

			private IEnumerator<ElementAssignmentConflictInfo> tIlurAGjswRzPeLpFSGHcEnzPBQq;

			private int FhIFjZbpbSEntgsVWVgqHGbPqlLqA;

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
			public cLadcmcGlWfjItVAJYHyrETEsSMOA(int P_0)
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
					ControllerMapWithAxes ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
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
						tIlurAGjswRzPeLpFSGHcEnzPBQq = ((ControllerMap)ttytLoUfsgUyhsklaKccrnoMiiek).ElementAssignmentConflicts(HHyftJlGnirHuGQIQnIQkaXvgYegA, HIuFjgoQMMeZHtUafJbERKutTjQt).GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_009e;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_009e;
					case 2:
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							goto IL_01b5;
						}
						IL_01c7:
						if (FhIFjZbpbSEntgsVWVgqHGbPqlLqA < ttytLoUfsgUyhsklaKccrnoMiiek.JfTobMsTRweaeWanKTfLHPJFqbSS.Count)
						{
							ActionElementMap actionElementMap = ttytLoUfsgUyhsklaKccrnoMiiek.JfTobMsTRweaeWanKTfLHPJFqbSS[FhIFjZbpbSEntgsVWVgqHGbPqlLqA];
							if ((!HIuFjgoQMMeZHtUafJbERKutTjQt || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI != HHyftJlGnirHuGQIQnIQkaXvgYegA.elementMapId && actionElementMap.CheckForAssignmentConflict(FnvvcrDffLPIUvRKBIfBFqTiuZyQ))
							{
								VqEePGSMyrGKIqkWibsjeHcWPSIx = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(ttytLoUfsgUyhsklaKccrnoMiiek._categoryId).userAssignable, -1, ttytLoUfsgUyhsklaKccrnoMiiek._controllerType, ttytLoUfsgUyhsklaKccrnoMiiek._controllerId, ttytLoUfsgUyhsklaKccrnoMiiek._id, actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI, actionElementMap._actionId, actionElementMap._elementType, actionElementMap._elementIdentifierId, actionElementMap.keyCode, actionElementMap.modifierKeyFlags);
								RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
								return true;
							}
							goto IL_01b5;
						}
						return false;
						IL_009e:
						if (tIlurAGjswRzPeLpFSGHcEnzPBQq.MoveNext())
						{
							ElementAssignmentConflictInfo current = tIlurAGjswRzPeLpFSGHcEnzPBQq.Current;
							VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
							RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
							return true;
						}
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
						tIlurAGjswRzPeLpFSGHcEnzPBQq = null;
						if (HIuFjgoQMMeZHtUafJbERKutTjQt && !ttytLoUfsgUyhsklaKccrnoMiiek._enabled)
						{
							return false;
						}
						if (ttytLoUfsgUyhsklaKccrnoMiiek.JfTobMsTRweaeWanKTfLHPJFqbSS == null)
						{
							return false;
						}
						FnvvcrDffLPIUvRKBIfBFqTiuZyQ = HHyftJlGnirHuGQIQnIQkaXvgYegA.ToElementAssignment();
						FhIFjZbpbSEntgsVWVgqHGbPqlLqA = 0;
						goto IL_01c7;
						IL_01b5:
						FhIFjZbpbSEntgsVWVgqHGbPqlLqA++;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (tIlurAGjswRzPeLpFSGHcEnzPBQq != null)
				{
					tIlurAGjswRzPeLpFSGHcEnzPBQq.Dispose();
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
				cLadcmcGlWfjItVAJYHyrETEsSMOA cLadcmcGlWfjItVAJYHyrETEsSMOA2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					cLadcmcGlWfjItVAJYHyrETEsSMOA2 = this;
				}
				else
				{
					cLadcmcGlWfjItVAJYHyrETEsSMOA2 = new cLadcmcGlWfjItVAJYHyrETEsSMOA(0);
					cLadcmcGlWfjItVAJYHyrETEsSMOA2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				cLadcmcGlWfjItVAJYHyrETEsSMOA2.HHyftJlGnirHuGQIQnIQkaXvgYegA = MelcAnrraPFJzeIaADSAOKRmqdwDb;
				cLadcmcGlWfjItVAJYHyrETEsSMOA2.HIuFjgoQMMeZHtUafJbERKutTjQt = MlWZQPQtLyJnETcXcYBEkbNZbonN;
				return cLadcmcGlWfjItVAJYHyrETEsSMOA2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private readonly IList<ActionElementMap> JfTobMsTRweaeWanKTfLHPJFqbSS;

		private readonly ReadOnlyCollection<ActionElementMap> cpElFHCburCDNacUoAckPJphhLxBA;

		public int axisMapCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
				{
					return 0;
				}
				return JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			}
		}

		public IList<ActionElementMap> AxisMaps
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
				}
				return cpElFHCburCDNacUoAckPJphhLxBA;
			}
		}

		internal AList<ActionElementMap> RJUoFwVagxGQGFUzBAhEbSlaIKJqc => (AList<ActionElementMap>)JfTobMsTRweaeWanKTfLHPJFqbSS;

		public ControllerMapWithAxes()
		{
			JfTobMsTRweaeWanKTfLHPJFqbSS = new AList<ActionElementMap>();
			cpElFHCburCDNacUoAckPJphhLxBA = new ReadOnlyCollection<ActionElementMap>(JfTobMsTRweaeWanKTfLHPJFqbSS);
		}

		public ControllerMapWithAxes(ControllerMapWithAxes P_0)
			: base(P_0)
		{
			JfTobMsTRweaeWanKTfLHPJFqbSS = new AList<ActionElementMap>();
			cpElFHCburCDNacUoAckPJphhLxBA = new ReadOnlyCollection<ActionElementMap>(JfTobMsTRweaeWanKTfLHPJFqbSS);
			if (P_0.JfTobMsTRweaeWanKTfLHPJFqbSS != null)
			{
				int count = P_0.JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
				for (int i = 0; i < count; i++)
				{
					LWNVplDVEUvjebyNsmCzzuhagXgr(new ActionElementMap(P_0.JfTobMsTRweaeWanKTfLHPJFqbSS[i]));
				}
			}
		}

		public override bool ContainsAction(int actionId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (base.ContainsAction(actionId))
			{
				return true;
			}
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
			{
				return false;
			}
			int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			for (int i = 0; i < count; i++)
			{
				if (JfTobMsTRweaeWanKTfLHPJFqbSS[i]._actionId == actionId)
				{
					return true;
				}
			}
			return false;
		}

		public override bool CreateElementMap(int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert, out ActionElementMap result)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				result = null;
				return false;
			}
			if (base.CreateElementMap(actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert, out result))
			{
				return true;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(elementType))
			{
				return false;
			}
			ActionElementMap actionElementMap = new ActionElementMap(actionId, elementType, elementIdentifierId, axisContribution, axisRange, invert);
			BakeElementMap(actionElementMap);
			LWNVplDVEUvjebyNsmCzzuhagXgr(actionElementMap);
			result = actionElementMap;
			return true;
		}

		public override bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert, out ActionElementMap result)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				result = null;
				return false;
			}
			if (base.ReplaceElementMap(elementMapId, actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert, out result))
			{
				return true;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(elementType))
			{
				return false;
			}
			ActionElementMap elementMap = GetElementMap(elementMapId);
			if (elementMap == null)
			{
				return false;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(elementMap._elementType))
			{
				DeleteElementMap(elementMapId);
				elementMap._elementType = ControllerElementType.Axis;
				LWNVplDVEUvjebyNsmCzzuhagXgr(elementMap);
			}
			if (VaEHvuGEYsBvTjMriqJRnBGCqDQwA(elementMapId) < 0)
			{
				return false;
			}
			ControllerMap.OgCdCocQpxWwVEWsnTRcismHoNbJ(elementMap, actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert);
			BakeElementMap(elementMap);
			result = elementMap;
			return true;
		}

		public override bool DeleteElementMap(int elementMapId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			if (base.DeleteElementMap(elementMapId))
			{
				return true;
			}
			int num = VaEHvuGEYsBvTjMriqJRnBGCqDQwA(elementMapId);
			if (num < 0)
			{
				return false;
			}
			phsFXAfmoCzHIOxoXZsBPwVzSlUrA(elementMapId, num);
			return true;
		}

		public override bool DeleteElementMapsWithAction(string actionName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			return DeleteElementMapsWithAction(ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName));
		}

		public override bool DeleteElementMapsWithAction(int actionId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			return base.DeleteElementMapsWithAction(actionId) | DeleteAxisMapsWithAction(actionId);
		}

		public override ActionElementMap GetElementMap(int elementMapId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			ActionElementMap elementMap = base.GetElementMap(elementMapId);
			if (elementMap != null)
			{
				return elementMap;
			}
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
			{
				return null;
			}
			int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			for (int i = 0; i < count; i++)
			{
				if (JfTobMsTRweaeWanKTfLHPJFqbSS[i].YVGDeWQAhUWKAOSPoxXuizkXaiTI == elementMapId)
				{
					return JfTobMsTRweaeWanKTfLHPJFqbSS[i];
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
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
			int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if (actionElementMap._actionId == actionId && (!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					return actionElementMap;
				}
			}
			return null;
		}

		internal override ActionElementMap QUAvRrpyocavBEJosAIunPklcrkdA(Predicate<ActionElementMap> P_0, bool P_1)
		{
			ActionElementMap actionElementMap = base.QUAvRrpyocavBEJosAIunPklcrkdA(P_0, P_1);
			if (actionElementMap != null)
			{
				return actionElementMap;
			}
			return snKhjzOFGiLJViWHbqesjmjwBvCm(P_0, P_1);
		}

		internal override int ygvDdijRuLSzWpDZXtNNZKGfcQWiA(Predicate<ActionElementMap> P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			return base.ygvDdijRuLSzWpDZXtNNZKGfcQWiA(P_0, P_1, P_2, P_3) + cpUrADCzPPOXeUkEjPuCLKnkFohQ(P_0, P_1, P_2, true);
		}

		public override void ClearElementMaps()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return;
			}
			base.ClearElementMaps();
			JfTobMsTRweaeWanKTfLHPJFqbSS.Clear();
		}

		public ActionElementMap GetAxisMap(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null || index < 0 || index >= JfTobMsTRweaeWanKTfLHPJFqbSS.Count)
			{
				return null;
			}
			return JfTobMsTRweaeWanKTfLHPJFqbSS[index];
		}

		public ActionElementMap[] GetAxisMaps()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.array;
			}
			return GetAxisMaps(skipDisabledMaps: false);
		}

		public ActionElementMap[] GetAxisMaps(bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.array;
			}
			if (!skipDisabledMaps)
			{
				return ListTools.ToArray(JfTobMsTRweaeWanKTfLHPJFqbSS);
			}
			int num = axisMapCount;
			List<ActionElementMap> list = new List<ActionElementMap>(num);
			for (int i = 0; i < num; i++)
			{
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if (actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					list.Add(actionElementMap);
				}
			}
			return list.ToArray();
		}

		public int GetAxisMaps(bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return IfOVuETHiJiWHeAyFJNCnrajobhY(skipDisabledMaps, results, false);
		}

		public ActionElementMap[] GetAxisMapsWithAction(string actionName)
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
			return GetAxisMapsWithAction(inputAction.id);
		}

		public ActionElementMap[] GetAxisMapsWithAction(int actionId)
		{
			return GetAxisMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public ActionElementMap[] GetAxisMapsWithAction(string actionName, bool skipDisabledMaps)
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
			return GetAxisMapsWithAction(inputAction.id, skipDisabledMaps);
		}

		public ActionElementMap[] GetAxisMapsWithAction(int actionId, bool skipDisabledMaps)
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
			int num = axisMapCount;
			if (num == 0)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
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
				ActionElementMap actionElementMap2 = JfTobMsTRweaeWanKTfLHPJFqbSS[j];
				if (actionElementMap2._actionId == actionId && (!skipDisabledMaps || actionElementMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					array[num3] = actionElementMap2;
					num3++;
				}
			}
			return array;
		}

		public int GetAxisMapsWithAction(string actionName, List<ActionElementMap> results)
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
			return GetAxisMapsWithAction(inputAction.id, results);
		}

		public int GetAxisMapsWithAction(int actionId, List<ActionElementMap> results)
		{
			return GetAxisMapsWithAction(actionId, skipDisabledMaps: false, results);
		}

		public int GetAxisMapsWithAction(string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
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
			return GetAxisMapsWithAction(inputAction.id, skipDisabledMaps, results);
		}

		public int GetAxisMapsWithAction(int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return HAUmMgxstRAfPIcWpXXvhvjJIhco(actionId, skipDisabledMaps, results, false);
		}

		public IEnumerable<ActionElementMap> AxisMapsWithAction(string actionName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return AxisMapsWithAction(actionId);
		}

		public IEnumerable<ActionElementMap> AxisMapsWithAction(int actionId)
		{
			return AxisMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public IEnumerable<ActionElementMap> AxisMapsWithAction(string actionName, bool skipDisabledMaps)
		{
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return AxisMapsWithAction(actionId, skipDisabledMaps);
		}

		public IEnumerable<ActionElementMap> AxisMapsWithAction(int actionId, bool skipDisabledMaps)
		{
			return new VPhATKGpdkvWSXuTTZMhgpnkgDIA(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				jGepRyLmTEBLAXIsaiiTHGhibRuCA = actionId,
				MlWZQPQtLyJnETcXcYBEkbNZbonN = skipDisabledMaps
			};
		}

		public ActionElementMap GetFirstAxisMapWithAction(int actionId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			return GetFirstAxisMapWithAction(actionId, skipDisabledMaps: false);
		}

		public ActionElementMap GetFirstAxisMapWithAction(string actionName)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetFirstAxisMapWithAction(actionId);
		}

		public ActionElementMap GetFirstAxisMapWithAction(int actionId, bool skipDisabledMaps)
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
			IList<ActionElementMap> axisMaps = AxisMaps;
			int count = axisMaps.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = axisMaps[i];
				if (actionElementMap._actionId == actionId && (!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					return actionElementMap;
				}
			}
			return null;
		}

		public ActionElementMap GetFirstAxisMapWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			int actionId = ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName);
			return GetFirstAxisMapWithAction(actionId, skipDisabledMaps);
		}

		public ActionElementMap GetFirstAxisMapMatch(Predicate<ActionElementMap> predicate)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			return snKhjzOFGiLJViWHbqesjmjwBvCm(predicate, false);
		}

		internal ActionElementMap snKhjzOFGiLJViWHbqesjmjwBvCm(Predicate<ActionElementMap> P_0, bool P_1)
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			return cpUrADCzPPOXeUkEjPuCLKnkFohQ(predicate, false, results, false);
		}

		internal int cpUrADCzPPOXeUkEjPuCLKnkFohQ(Predicate<ActionElementMap> P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
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
			int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					ActionElementMap obj = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
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
			return DeleteAxisMapsWithAction(ReInput.GAwkqrvWlvpikRXvCLOCwZfKeMsC.hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(actionName));
		}

		public bool DeleteAxisMapsWithAction(int actionId)
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
			int num = axisMapCount;
			if (num == 0)
			{
				return false;
			}
			bool result = false;
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				if (JfTobMsTRweaeWanKTfLHPJFqbSS[num2] != null && JfTobMsTRweaeWanKTfLHPJFqbSS[num2]._actionId == actionId)
				{
					phsFXAfmoCzHIOxoXZsBPwVzSlUrA(JfTobMsTRweaeWanKTfLHPJFqbSS[num2].YVGDeWQAhUWKAOSPoxXuizkXaiTI, num2);
					result = true;
				}
			}
			return result;
		}

		public int SetAllAxisMapsEnabled(bool state)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			int num = 0;
			int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if (actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo != state)
				{
					actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo = state;
					num++;
				}
			}
			return num;
		}

		public override bool DoesElementAssignmentConflict(ControllerMap controllerMap, bool skipDisabledMaps)
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
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
			{
				return false;
			}
			IList<ActionElementMap> axisMaps = controllerMapWithAxes.AxisMaps;
			if (axisMaps == null)
			{
				return false;
			}
			int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			int count2 = axisMaps.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if (skipDisabledMaps && !actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					continue;
				}
				for (int j = 0; j < count2; j++)
				{
					ActionElementMap actionElementMap2 = axisMaps[j];
					if ((!skipDisabledMaps || actionElementMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.CheckForAssignmentConflict(actionElementMap2))
					{
						return true;
					}
				}
			}
			return false;
		}

		public override bool DoesElementAssignmentConflict(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
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
			if (skipDisabledMaps && (!_enabled || !actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
			{
				return false;
			}
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
			{
				return false;
			}
			for (int i = 0; i < JfTobMsTRweaeWanKTfLHPJFqbSS.Count; i++)
			{
				ActionElementMap actionElementMap2 = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if ((!skipDisabledMaps || actionElementMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap2.CheckForAssignmentConflict(actionElementMap))
				{
					return true;
				}
			}
			return false;
		}

		public override bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
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
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
			{
				return false;
			}
			ElementAssignment elementAssignment = conflictCheck.ToElementAssignment();
			for (int i = 0; i < JfTobMsTRweaeWanKTfLHPJFqbSS.Count; i++)
			{
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if ((!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI != conflictCheck.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
				{
					return true;
				}
			}
			return false;
		}

		public override IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ControllerMap controllerMap, bool skipDisabledMaps)
		{
			return new XkWmBQqudyLAVZPWUGNOmBnibycE(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				dAqaBgHEAvkbMVeRDCvJcJYeWvRO = controllerMap,
				MlWZQPQtLyJnETcXcYBEkbNZbonN = skipDisabledMaps
			};
		}

		public override IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			return new CODHjysPShNccvoajGrJuLQyLgXp(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				tpItAifrGrjraEGilAOGvgJvUACH = actionElementMap,
				MlWZQPQtLyJnETcXcYBEkbNZbonN = skipDisabledMaps
			};
		}

		public override IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			return new cLadcmcGlWfjItVAJYHyrETEsSMOA(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				MelcAnrraPFJzeIaADSAOKRmqdwDb = conflictCheck,
				MlWZQPQtLyJnETcXcYBEkbNZbonN = skipDisabledMaps
			};
		}

		public override int RemoveElementAssignmentConflicts(ControllerMap controllerMap, bool skipDisabledMaps)
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
			int num = base.RemoveElementAssignmentConflicts(controllerMap, skipDisabledMaps);
			if (!(controllerMap is ControllerMapWithAxes controllerMapWithAxes))
			{
				return num;
			}
			if (skipDisabledMaps && (!_enabled || !controllerMapWithAxes._enabled))
			{
				return num;
			}
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
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
			_ = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			int count = axisMaps.Count;
			for (int num2 = JfTobMsTRweaeWanKTfLHPJFqbSS.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[num2];
				if (!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					for (int i = 0; i < count; i++)
					{
						ActionElementMap actionElementMap2 = axisMaps[i];
						if ((!skipDisabledMaps || actionElementMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.CheckForAssignmentConflict(actionElementMap2))
						{
							phsFXAfmoCzHIOxoXZsBPwVzSlUrA(actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI, num2);
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			if (actionElementMap == null)
			{
				return 0;
			}
			int num = base.RemoveElementAssignmentConflicts(actionElementMap, skipDisabledMaps);
			if (skipDisabledMaps && (!_enabled || !actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
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
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
			{
				return num;
			}
			for (int num2 = JfTobMsTRweaeWanKTfLHPJFqbSS.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap2 = JfTobMsTRweaeWanKTfLHPJFqbSS[num2];
				if ((!skipDisabledMaps || actionElementMap2.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap2.CheckForAssignmentConflict(actionElementMap))
				{
					phsFXAfmoCzHIOxoXZsBPwVzSlUrA(actionElementMap2.YVGDeWQAhUWKAOSPoxXuizkXaiTI, num2);
					num++;
				}
			}
			return num;
		}

		public override int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			int num = base.RemoveElementAssignmentConflicts(conflictCheck, skipDisabledMaps);
			if (skipDisabledMaps && !_enabled)
			{
				return num;
			}
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
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
			for (int num2 = JfTobMsTRweaeWanKTfLHPJFqbSS.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[num2];
				if ((!skipDisabledMaps || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo) && actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI != conflictCheck.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
				{
					phsFXAfmoCzHIOxoXZsBPwVzSlUrA(actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI, num2);
					num++;
				}
			}
			return num;
		}

		internal override int lpZmTcYQMeYBtGMoiEbkUblMviuS(ControllerMap P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			int num = base.lpZmTcYQMeYBtGMoiEbkUblMviuS(P_0, P_1, P_2, P_3);
			if (!(P_0 is ControllerMapWithAxes controllerMapWithAxes))
			{
				return num;
			}
			if (P_1 && (!_enabled || !controllerMapWithAxes._enabled))
			{
				return num;
			}
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
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
			int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			int count2 = axisMaps.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if (!actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					continue;
				}
				for (int j = 0; j < count2; j++)
				{
					ActionElementMap actionElementMap2 = axisMaps[j];
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

		internal override int lpZmTcYQMeYBtGMoiEbkUblMviuS(ActionElementMap P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			int num = base.lpZmTcYQMeYBtGMoiEbkUblMviuS(P_0, P_1, P_2, P_3);
			if (P_0 == null)
			{
				return num;
			}
			if (P_1 && (!_enabled || !P_0.kKFZZElqKQSUFMZnWdEudRvTJGpo))
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
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if (actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo && P_0.CheckForAssignmentConflict(actionElementMap))
				{
					actionElementMap.enabled = false;
					P_2?.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		internal override int lpZmTcYQMeYBtGMoiEbkUblMviuS(ElementAssignmentConflictCheck P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			int num = base.lpZmTcYQMeYBtGMoiEbkUblMviuS(P_0, P_1, P_2, P_3);
			if (P_1 && !_enabled)
			{
				return num;
			}
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
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
			int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if (actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo && actionElementMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI != P_0.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
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
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
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
				array[i] = JfTobMsTRweaeWanKTfLHPJFqbSS[i].elementIdentifierName;
			}
			return array;
		}

		internal override bool lqUvlJMImEtcvtFlVYWmonZgLfZC(ActionElementMap P_0)
		{
			if (base.lqUvlJMImEtcvtFlVYWmonZgLfZC(P_0))
			{
				return true;
			}
			ControllerElementType elementType = P_0._elementType;
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(elementType))
			{
				return false;
			}
			LWNVplDVEUvjebyNsmCzzuhagXgr(P_0);
			return true;
		}

		internal override int SfYThgjCbGGBVaAmuUBhxMEDxCeO(List<ActionElementMap> P_0, bool P_1)
		{
			base.SfYThgjCbGGBVaAmuUBhxMEDxCeO(P_0, P_1);
			int count = P_0.Count;
			int count2 = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			for (int i = 0; i < count2; i++)
			{
				if (!P_1 || JfTobMsTRweaeWanKTfLHPJFqbSS[i].kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					P_0.Add(JfTobMsTRweaeWanKTfLHPJFqbSS[i]);
				}
			}
			return P_0.Count - count;
		}

		internal override ActionElementMap hOxSQVcwNejfCacLSpeCVuEITowP(int P_0, int P_1, ControllerElementType P_2)
		{
			ActionElementMap actionElementMap = base.hOxSQVcwNejfCacLSpeCVuEITowP(P_0, P_1, P_2);
			if (actionElementMap != null)
			{
				return actionElementMap;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_2))
			{
				return null;
			}
			int num = jItBPCmwrRbFUjcSXfytsGHrQKcIA(P_0, P_1, P_2);
			if (num < 0)
			{
				return null;
			}
			if (P_2 == ControllerElementType.Axis)
			{
				return JfTobMsTRweaeWanKTfLHPJFqbSS[num];
			}
			throw new NotImplementedException();
		}

		internal override int ULJHbsZFwKdaRYbNmcqpVsYjydsr(int P_0, List<ActionElementMap> P_1, bool P_2)
		{
			if (P_1 == null)
			{
				throw new ArgumentNullException("results");
			}
			int num = (P_2 ? P_1.Count : 0);
			base.ULJHbsZFwKdaRYbNmcqpVsYjydsr(P_0, P_1, P_2);
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
			{
				return P_1.Count - num;
			}
			int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			for (int i = 0; i < count; i++)
			{
				if (JfTobMsTRweaeWanKTfLHPJFqbSS[i]._elementIdentifierId == P_0)
				{
					P_1.Add(JfTobMsTRweaeWanKTfLHPJFqbSS[i]);
				}
			}
			return P_1.Count - num;
		}

		internal override bool OSxfPrgbMEYNaJmXwwncKGNUvVZj(int P_0, int P_1, ControllerElementType P_2)
		{
			if (base.OSxfPrgbMEYNaJmXwwncKGNUvVZj(P_0, P_1, P_2))
			{
				return true;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_2))
			{
				return false;
			}
			if (P_2 == ControllerElementType.Axis)
			{
				int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
				for (int i = 0; i < count; i++)
				{
					if (JfTobMsTRweaeWanKTfLHPJFqbSS[i]._elementIdentifierId == P_0 && JfTobMsTRweaeWanKTfLHPJFqbSS[i]._actionId == P_1)
					{
						return true;
					}
				}
				return false;
			}
			throw new NotImplementedException();
		}

		internal override int jItBPCmwrRbFUjcSXfytsGHrQKcIA(int P_0, int P_1, ControllerElementType P_2)
		{
			int num = base.jItBPCmwrRbFUjcSXfytsGHrQKcIA(P_0, P_1, P_2);
			if (num >= 0)
			{
				return num;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_2))
			{
				return -1;
			}
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
			{
				return -1;
			}
			if (P_2 == ControllerElementType.Axis)
			{
				int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
				for (int i = 0; i < count; i++)
				{
					if (JfTobMsTRweaeWanKTfLHPJFqbSS[i]._elementIdentifierId == P_0 && JfTobMsTRweaeWanKTfLHPJFqbSS[i]._actionId == P_1)
					{
						return i;
					}
				}
				return -1;
			}
			throw new NotImplementedException();
		}

		internal int VaEHvuGEYsBvTjMriqJRnBGCqDQwA(int P_0)
		{
			if (JfTobMsTRweaeWanKTfLHPJFqbSS == null)
			{
				return -1;
			}
			int count = JfTobMsTRweaeWanKTfLHPJFqbSS.Count;
			for (int i = 0; i < count; i++)
			{
				if (JfTobMsTRweaeWanKTfLHPJFqbSS[i].YVGDeWQAhUWKAOSPoxXuizkXaiTI == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		internal int IfOVuETHiJiWHeAyFJNCnrajobhY(bool P_0, List<ActionElementMap> P_1, bool P_2)
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
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if (!P_0 || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo)
				{
					P_1.Add(actionElementMap);
					num2++;
				}
			}
			return num2;
		}

		internal int HAUmMgxstRAfPIcWpXXvhvjJIhco(int P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
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
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if (actionElementMap._actionId == P_0 && (!P_1 || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					P_2.Add(actionElementMap);
					num2++;
				}
			}
			return num2;
		}

		internal override int wpGLUOiKKYaZujmxopRHHGStkBpW(int P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			int num = base.wpGLUOiKKYaZujmxopRHHGStkBpW(P_0, P_1, P_2, P_3);
			if (P_0 < 0)
			{
				return num;
			}
			int num2 = axisMapCount;
			for (int i = 0; i < num2; i++)
			{
				ActionElementMap actionElementMap = JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				if (actionElementMap._actionId == P_0 && (!P_1 || actionElementMap.kKFZZElqKQSUFMZnWdEudRvTJGpo))
				{
					P_2.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		internal override ActionElementMap EUcSmaxDeldMRvTJKrymSFSbIMkm(IControllerElementTarget P_0, bool P_1, int P_2, bool P_3, out bool P_4)
		{
			ActionElementMap actionElementMap = base.EUcSmaxDeldMRvTJKrymSFSbIMkm(P_0, P_1, P_2, P_3, out P_4);
			if (actionElementMap != null)
			{
				return actionElementMap;
			}
			if (P_4)
			{
				return null;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_0.elementType))
			{
				return null;
			}
			int num = axisMapCount;
			_ = P_0.elementIdentifierId;
			for (int i = 0; i < num; i++)
			{
				if ((!P_1 || JfTobMsTRweaeWanKTfLHPJFqbSS[i]._actionId == P_2) && (!P_3 || JfTobMsTRweaeWanKTfLHPJFqbSS[i].kKFZZElqKQSUFMZnWdEudRvTJGpo) && JfTobMsTRweaeWanKTfLHPJFqbSS[i].IsTarget(P_0))
				{
					return JfTobMsTRweaeWanKTfLHPJFqbSS[i];
				}
			}
			return null;
		}

		internal override int UDJQNhqhtojGfcxRhUYWnYqrkexm(IControllerElementTarget P_0, bool P_1, int P_2, bool P_3, List<ActionElementMap> P_4, bool P_5, out bool P_6)
		{
			int num = base.UDJQNhqhtojGfcxRhUYWnYqrkexm(P_0, P_1, P_2, P_3, P_4, P_5, out P_6);
			if (P_6)
			{
				return num;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_0.elementType))
			{
				return num;
			}
			int num2 = axisMapCount;
			_ = P_0.elementIdentifierId;
			for (int i = 0; i < num2; i++)
			{
				if ((!P_1 || JfTobMsTRweaeWanKTfLHPJFqbSS[i]._actionId == P_2) && (!P_3 || JfTobMsTRweaeWanKTfLHPJFqbSS[i].kKFZZElqKQSUFMZnWdEudRvTJGpo) && JfTobMsTRweaeWanKTfLHPJFqbSS[i].IsTarget(P_0))
				{
					P_4.Add(JfTobMsTRweaeWanKTfLHPJFqbSS[i]);
					num++;
				}
			}
			return num;
		}

		internal override bool ZxwtLymUXBfmZpaPpFNVkvIPfZZDA(ActionElementMap P_0)
		{
			if (base.ZxwtLymUXBfmZpaPpFNVkvIPfZZDA(P_0))
			{
				return true;
			}
			if (P_0 == null)
			{
				return false;
			}
			if (!kTJfWCbzqEHPgeOYseEucoxIAjYkB(P_0._elementType))
			{
				return false;
			}
			JfTobMsTRweaeWanKTfLHPJFqbSS.Add(P_0);
			mHtfYeiNvxekaJaWFKccGvJaIkiD(P_0);
			return true;
		}

		private bool kTJfWCbzqEHPgeOYseEucoxIAjYkB(ControllerElementType P_0)
		{
			if (P_0 != ControllerElementType.Axis)
			{
				return false;
			}
			return true;
		}

		private void phsFXAfmoCzHIOxoXZsBPwVzSlUrA(int P_0, int P_1)
		{
			cmioCUaiaDFNaRGdRlMmfdBtuqvk(P_0);
			if (P_1 >= 0 && P_1 < axisMapCount)
			{
				JfTobMsTRweaeWanKTfLHPJFqbSS.RemoveAt(P_1);
			}
		}

		private void LWNVplDVEUvjebyNsmCzzuhagXgr(ActionElementMap P_0)
		{
			if (P_0 != null)
			{
				JfTobMsTRweaeWanKTfLHPJFqbSS.Add(P_0);
				mHtfYeiNvxekaJaWFKccGvJaIkiD(P_0);
			}
		}

		private void EzDoLkLqqghcqNbibEMadPsCooUrA(ActionElementMap P_0, int P_1)
		{
			if (P_0 != null && P_1 >= 0 && P_1 < axisMapCount)
			{
				lAPTPcDPxSWjnbfFbRfkhyVfqMQV(JfTobMsTRweaeWanKTfLHPJFqbSS[P_1].YVGDeWQAhUWKAOSPoxXuizkXaiTI, P_0);
				JfTobMsTRweaeWanKTfLHPJFqbSS[P_1] = P_0;
			}
		}

		internal override void eqxcMNQhZrhfftsCuDbhlTUMXCQB(SerializedObject P_0)
		{
			base.eqxcMNQhZrhfftsCuDbhlTUMXCQB(P_0);
			int num = axisMapCount;
			List<object> list = new List<object>();
			P_0.Add("axisMaps", list);
			for (int i = 0; i < num; i++)
			{
				if (JfTobMsTRweaeWanKTfLHPJFqbSS[i] != null)
				{
					list.Add(JfTobMsTRweaeWanKTfLHPJFqbSS[i].JxowpgGeLUBIhbyiSIIfFkkGllDuA());
				}
			}
		}

		internal override bool ckPgJNiFQjrLdKfwClUsdsySXFyIb(SerializedObject P_0)
		{
			bool flag = base.ckPgJNiFQjrLdKfwClUsdsySXFyIb(P_0);
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
						actionElementMap.ckPgJNiFQjrLdKfwClUsdsySXFyIb(value2);
						if (ActionElementMap.QCVAmAdeLePlbTxUIzzPjfmaVeTVA(actionElementMap))
						{
							LWNVplDVEUvjebyNsmCzzuhagXgr(actionElementMap);
						}
					}
				}
			}
			return flag;
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private IEnumerable<ElementAssignmentConflictInfo> rXAkxTyoyMrxwhMbMPkFGsngVgiq(ControllerMap P_0, bool P_1)
		{
			return base.ElementAssignmentConflicts(P_0, P_1);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<ElementAssignmentConflictInfo> kIFOMqoSovBmWyQlKpcSxOuXerX(ActionElementMap P_0, bool P_1)
		{
			return base.ElementAssignmentConflicts(P_0, P_1);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<ElementAssignmentConflictInfo> oWAumnJVBpxbooPJBlGpqcbMFted(ElementAssignmentConflictCheck P_0, bool P_1)
		{
			return base.ElementAssignmentConflicts(P_0, P_1);
		}
	}
}
