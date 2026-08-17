using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Rewired.Data.Mapping;
using Rewired.Utils;
using UnityEngine;

namespace Rewired
{
	public abstract class ControllerWithAxes : ControllerWithMap
	{
		private sealed class nwJxbgGibhWHtWkcVbrEgPGgCWORA : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerWithAxes TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
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
			public nwJxbgGibhWHtWkcVbrEgPGgCWORA(int P_0)
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
				ControllerWithAxes ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_00a8;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ReInput._id != ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(ttytLoUfsgUyhsklaKccrnoMiiek.QajDFFaomlkLHzaostfWYpGUioys);
					return false;
				}
				ttytLoUfsgUyhsklaKccrnoMiiek.UpdatePollingFrameTracking();
				ttytLoUfsgUyhsklaKccrnoMiiek.gjdBRcmBmbjkZAFXTlhOEtwctLxQb();
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
				goto IL_00ba;
				IL_00ba:
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek._axisCount)
				{
					if (ttytLoUfsgUyhsklaKccrnoMiiek.IsPolledAxisActive(hWZNbLCFLBBWXNZxiKeGtzgrnTsg, out var pole, out var elementIdentifierId))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = new ControllerPollingInfo(true, -1, ttytLoUfsgUyhsklaKccrnoMiiek.id, ttytLoUfsgUyhsklaKccrnoMiiek._name, ttytLoUfsgUyhsklaKccrnoMiiek._type, ControllerElementType.Axis, hWZNbLCFLBBWXNZxiKeGtzgrnTsg, pole, ttytLoUfsgUyhsklaKccrnoMiiek.yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetElementIdentifierName(elementIdentifierId), elementIdentifierId, KeyCode.None);
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_00a8;
				}
				return false;
				IL_00a8:
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
				goto IL_00ba;
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
				nwJxbgGibhWHtWkcVbrEgPGgCWORA nwJxbgGibhWHtWkcVbrEgPGgCWORA2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					nwJxbgGibhWHtWkcVbrEgPGgCWORA2 = this;
				}
				else
				{
					nwJxbgGibhWHtWkcVbrEgPGgCWORA2 = new nwJxbgGibhWHtWkcVbrEgPGgCWORA(0);
					nwJxbgGibhWHtWkcVbrEgPGgCWORA2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return nwJxbgGibhWHtWkcVbrEgPGgCWORA2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		private sealed class fMZGDhviQkvHJfAGWbtBjUMyCgqm : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerWithAxes TtytLoUfsgUyhsklaKccrnoMiiek;

			private IEnumerator<ControllerPollingInfo> hSeONskmQJhVsjVUHGUASYbDGJrU;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
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
			public fMZGDhviQkvHJfAGWbtBjUMyCgqm(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				switch (RxAoyfYzYDsYonLGXsvUgwChukLk)
				{
				case -3:
				case 1:
					try
					{
						break;
					}
					finally
					{
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					}
				case -4:
				case 2:
					try
					{
						break;
					}
					finally
					{
						tMeeFNDmLhzrFkrObkICOsBGeJux();
					}
				case -2:
				case -1:
				case 0:
					break;
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					ControllerWithAxes ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
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
						hSeONskmQJhVsjVUHGUASYbDGJrU = ((Controller)ttytLoUfsgUyhsklaKccrnoMiiek).PollForAllElements().GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_0092;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_0092;
					case 2:
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
							break;
						}
						IL_0092:
						if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
						{
							ControllerPollingInfo current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
							VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
							RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
							return true;
						}
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
						hSeONskmQJhVsjVUHGUASYbDGJrU = null;
						hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.PollForAllAxes().GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
						break;
					}
					if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
					{
						ControllerPollingInfo current2 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
						VqEePGSMyrGKIqkWibsjeHcWPSIx = current2;
						RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
						return true;
					}
					tMeeFNDmLhzrFkrObkICOsBGeJux();
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

			private void tMeeFNDmLhzrFkrObkICOsBGeJux()
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
			IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
			{
				fMZGDhviQkvHJfAGWbtBjUMyCgqm fMZGDhviQkvHJfAGWbtBjUMyCgqm2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					fMZGDhviQkvHJfAGWbtBjUMyCgqm2 = this;
				}
				else
				{
					fMZGDhviQkvHJfAGWbtBjUMyCgqm2 = new fMZGDhviQkvHJfAGWbtBjUMyCgqm(0);
					fMZGDhviQkvHJfAGWbtBjUMyCgqm2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return fMZGDhviQkvHJfAGWbtBjUMyCgqm2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		private sealed class HtWJjmVxkRQhqIhEaWYiykDtfmKE : IEnumerable<ControllerPollingInfo>, IEnumerator<ControllerPollingInfo>, IDisposable, IEnumerable, IEnumerator
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ControllerPollingInfo VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ControllerWithAxes TtytLoUfsgUyhsklaKccrnoMiiek;

			private IEnumerator<ControllerPollingInfo> hSeONskmQJhVsjVUHGUASYbDGJrU;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
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
			public HtWJjmVxkRQhqIhEaWYiykDtfmKE(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				switch (RxAoyfYzYDsYonLGXsvUgwChukLk)
				{
				case -3:
				case 1:
					try
					{
						break;
					}
					finally
					{
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					}
				case -4:
				case 2:
					try
					{
						break;
					}
					finally
					{
						tMeeFNDmLhzrFkrObkICOsBGeJux();
					}
				case -2:
				case -1:
				case 0:
					break;
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					ControllerWithAxes ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
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
						hSeONskmQJhVsjVUHGUASYbDGJrU = ((Controller)ttytLoUfsgUyhsklaKccrnoMiiek).PollForAllElementsDown().GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_0092;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_0092;
					case 2:
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
							break;
						}
						IL_0092:
						if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
						{
							ControllerPollingInfo current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
							VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
							RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
							return true;
						}
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
						hSeONskmQJhVsjVUHGUASYbDGJrU = null;
						hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.PollForAllAxes().GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -4;
						break;
					}
					if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
					{
						ControllerPollingInfo current2 = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
						VqEePGSMyrGKIqkWibsjeHcWPSIx = current2;
						RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
						return true;
					}
					tMeeFNDmLhzrFkrObkICOsBGeJux();
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

			private void tMeeFNDmLhzrFkrObkICOsBGeJux()
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
			IEnumerator<ControllerPollingInfo> IEnumerable<ControllerPollingInfo>.GetEnumerator()
			{
				HtWJjmVxkRQhqIhEaWYiykDtfmKE htWJjmVxkRQhqIhEaWYiykDtfmKE;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					htWJjmVxkRQhqIhEaWYiykDtfmKE = this;
				}
				else
				{
					htWJjmVxkRQhqIhEaWYiykDtfmKE = new HtWJjmVxkRQhqIhEaWYiykDtfmKE(0);
					htWJjmVxkRQhqIhEaWYiykDtfmKE.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return htWJjmVxkRQhqIhEaWYiykDtfmKE;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		protected readonly int _axisCount;

		protected readonly int _axis2DCount;

		protected readonly Axis[] axes;

		protected readonly ReadOnlyCollection<Axis> axes_readOnly;

		protected readonly Axis2D[] axes2D;

		protected readonly ReadOnlyCollection<Axis2D> axes2D_readOnly;

		protected CalibrationMap _calibrationMap;

		private float[] BijdrwGaINHJxnPnVjrnZCtkCPVCb;

		private uint etHUIhDEqrnLEiPJGCMbqVceKELW = uint.MaxValue;

		private Func<int, int> zSSDgSGEFuoVXbqlGAfVNVsHjHaRA;

		public int axisCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				return _axisCount;
			}
		}

		public int axis2DCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				return _axis2DCount;
			}
		}

		public IList<Axis> Axes
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<Axis>.EmptyReadOnlyIListT;
				}
				return axes_readOnly;
			}
		}

		public IList<Axis2D> Axes2D
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<Axis2D>.EmptyReadOnlyIListT;
				}
				return axes2D_readOnly;
			}
		}

		public CalibrationMap calibrationMap
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				return _calibrationMap;
			}
		}

		public IList<ControllerElementIdentifier> AxisElementIdentifiers
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return EmptyObjects<ControllerElementIdentifier>.EmptyReadOnlyIListT;
				}
				return yPbTGFEOQNqKOEHVnHaIUhnHjgYD.axisElementIdentifiers_readOnly;
			}
		}

		internal ControllerWithAxes(int P_0, InputSource P_1, string P_2, string P_3, string P_4, ControllerType P_5, Guid P_6, int P_7, int P_8, bool[] P_9, HardwareControllerMap_Game P_10, Extension P_11, ControllerDataUpdater P_12)
			: base(P_0, P_1, P_2, P_3, P_4, P_5, P_6, P_8, P_9, P_10, P_11, P_12)
		{
			_axisCount = P_7;
			axes = new Axis[P_7];
			for (int i = 0; i < P_7; i++)
			{
				axes[i] = new Axis(this, P_10.axisElementIdentifierIds[i], "Axis " + i, P_10.hwAxisRanges[i], P_10.hwAxisInfo[i]);
				JymOYvJCaUZfrfNvqSliYMwhgCGz(axes[i]);
			}
			axes_readOnly = new ReadOnlyCollection<Axis>(axes);
			_calibrationMap = new CalibrationMap(P_10.hwAxisCalibrationData);
			_axis2DCount = P_10.axis2DCount;
			axes2D = new Axis2D[_axis2DCount];
			for (int j = 0; j < _axis2DCount; j++)
			{
				try
				{
					HardwareJoystickMap.CompoundElement axis2DData = P_10.GetAxis2DData(j);
					if (axis2DData == null)
					{
						Logger.LogError("Error creating Axis2D from hardware map! CompoundElement is null!");
						axes2D[j] = new Axis2D(this, axis2DData.elementIdentifier, "Axis 2D " + j, null, null, 0, 0, null);
						continue;
					}
					int axisIndex = P_10.GetAxisIndex(axis2DData.componentElementIdentifiers[0]);
					int axisIndex2 = P_10.GetAxisIndex(axis2DData.componentElementIdentifiers[1]);
					if (axisIndex < 0 || axisIndex >= _axisCount || axisIndex2 < 0 || axisIndex2 >= _axisCount)
					{
						axes2D[j] = new Axis2D(this, axis2DData.elementIdentifier, "Axis 2D " + j, null, null, 0, 0, null);
					}
					else
					{
						axes2D[j] = new Axis2D(this, axis2DData.elementIdentifier, "Axis 2D " + j, axes[axisIndex], axes[axisIndex2], axisIndex, axisIndex2, _calibrationMap);
					}
				}
				catch
				{
					Logger.LogError("Error creating Axis2D from hardware map! An exception was thrown.");
					axes2D[j] = new Axis2D(this, -1, "Axis 2D " + j, null, null, 0, 0, null);
				}
				finally
				{
					ilFFizeJvQVyRFyOMjSxGFTfdnmy(axes2D[j]);
				}
			}
			axes2D_readOnly = new ReadOnlyCollection<Axis2D>(axes2D);
			CtrjjJxhauzevwWMqlUzXBxtwPCT();
			zSSDgSGEFuoVXbqlGAfVNVsHjHaRA = P_10.GetAxisIndex;
		}

		public override Element GetElementById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			if (yPbTGFEOQNqKOEHVnHaIUhnHjgYD == null)
			{
				return null;
			}
			Element elementById = base.GetElementById(elementIdentifierId);
			if (elementById != null)
			{
				return elementById;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0)
			{
				return null;
			}
			return axes[axisIndex];
		}

		public int GetAxisIndexById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return -1;
			}
			return yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
		}

		public float GetAxis(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0f;
			}
			return axes[index].value;
		}

		public float GetAxisPrev(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0f;
			}
			return axes[index].valuePrev;
		}

		public float GetAxisRaw(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0f;
			}
			return axes[index].valueRaw;
		}

		public float GetAxisRawPrev(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0f;
			}
			return axes[index].valueRawPrev;
		}

		public double GetAxisTimeActive(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0.0;
			}
			return axes[index].timeActive;
		}

		public double GetAxisTimeInactive(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0.0;
			}
			return axes[index].timeInactive;
		}

		public double GetAxisLastTimeActive(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0.0;
			}
			return axes[index].lastTimeActive;
		}

		public double GetAxisLastTimeInactive(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0.0;
			}
			return axes[index].lastTimeInactive;
		}

		public double GetAxisRawTimeActive(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0.0;
			}
			return axes[index].timeActiveRaw;
		}

		public double GetAxisRawTimeInactive(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0.0;
			}
			return axes[index].timeInactiveRaw;
		}

		public double GetAxisRawLastTimeActive(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0.0;
			}
			return axes[index].lastTimeActiveRaw;
		}

		public double GetAxisRawLastTimeInactive(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (index < 0 || index >= _axisCount)
			{
				return 0.0;
			}
			return axes[index].lastTimeInactiveRaw;
		}

		public float GetAxisById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0f;
			}
			return axes[axisIndex].value;
		}

		public float GetAxisPrevById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0f;
			}
			return axes[axisIndex].valuePrev;
		}

		public float GetAxisRawById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0f;
			}
			return axes[axisIndex].valueRaw;
		}

		public float GetAxisRawPrevById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0f;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0f;
			}
			return axes[axisIndex].valueRawPrev;
		}

		public double GetAxisTimeActiveById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].timeActive;
		}

		public double GetAxisTimeInactiveById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].timeInactive;
		}

		public double GetAxisLastTimeActiveById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].lastTimeActive;
		}

		public double GetAxisLastTimeInactiveById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].lastTimeInactive;
		}

		public double GetAxisRawTimeActiveById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].timeActiveRaw;
		}

		public double GetAxisRawTimeInactiveById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].timeInactiveRaw;
		}

		public double GetAxisRawLastTimeActiveById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].lastTimeActiveRaw;
		}

		public double GetAxisRawLastTimeInactiveById(int elementIdentifierId)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			int axisIndex = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].lastTimeInactiveRaw;
		}

		public Vector2 GetAxis2D(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return Vector2.zero;
			}
			if (index < 0 || index >= _axis2DCount)
			{
				return default(Vector2);
			}
			return axes2D[index].value;
		}

		public Vector2 GetAxis2DPrev(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return Vector2.zero;
			}
			if (index < 0 || index >= _axis2DCount)
			{
				return default(Vector2);
			}
			return axes2D[index].valuePrev;
		}

		public Vector2 GetAxis2DRaw(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return Vector2.zero;
			}
			if (index < 0 || index >= _axis2DCount)
			{
				return default(Vector2);
			}
			return axes2D[index].valueRaw;
		}

		public Vector2 GetAxis2DRawPrev(int index)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return Vector2.zero;
			}
			if (index < 0 || index >= _axis2DCount)
			{
				return default(Vector2);
			}
			return axes2D[index].valueRawPrev;
		}

		public override double GetLastTimeActive()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			return GetLastTimeActive(useRawValues: false);
		}

		public override double GetLastTimeActive(bool useRawValues)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			return MathTools.Max(base.GetLastTimeActive(useRawValues), GetLastTimeAnyAxisActive(useRawValues));
		}

		public override double GetLastTimeAnyElementChanged()
		{
			return GetLastTimeAnyElementChanged(useRawValues: false);
		}

		public override double GetLastTimeAnyElementChanged(bool useRawValues)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			return MathTools.Max(base.GetLastTimeAnyElementChanged(useRawValues), GetLastTimeAnyAxisChanged(useRawValues));
		}

		public double GetLastTimeAnyAxisActive()
		{
			return GetLastTimeAnyAxisActive(useRawValues: false);
		}

		public double GetLastTimeAnyAxisActive(bool useRawValues)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (axes == null)
			{
				return 0.0;
			}
			double num = 0.0;
			for (int i = 0; i < axes.Length; i++)
			{
				double num2 = (useRawValues ? axes[i].lastTimeActiveRaw : axes[i].lastTimeActive);
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		public double GetLastTimeAnyAxisChanged()
		{
			return GetLastTimeAnyAxisChanged(useRawValues: false);
		}

		public double GetLastTimeAnyAxisChanged(bool useRawValues)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0.0;
			}
			if (axes == null)
			{
				return 0.0;
			}
			double num = 0.0;
			for (int i = 0; i < axes.Length; i++)
			{
				double num2 = (useRawValues ? axes[i].lastTimeValueChangedRaw : axes[i].lastTimeValueChanged);
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		public override ControllerPollingInfo PollForFirstElement()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
			}
			ControllerPollingInfo result = base.PollForFirstElement();
			if (result.success)
			{
				return result;
			}
			return PollForFirstAxis();
		}

		public override ControllerPollingInfo PollForFirstElementDown()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
			}
			ControllerPollingInfo result = base.PollForFirstElementDown();
			if (result.success)
			{
				return result;
			}
			return PollForFirstAxis();
		}

		public ControllerPollingInfo PollForFirstAxis()
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
			}
			UpdatePollingFrameTracking();
			gjdBRcmBmbjkZAFXTlhOEtwctLxQb();
			for (int i = 0; i < _axisCount; i++)
			{
				if (IsPolledAxisActive(i, out var pole, out var elementIdentifierId))
				{
					return new ControllerPollingInfo(true, -1, id, _name, _type, ControllerElementType.Axis, i, pole, yPbTGFEOQNqKOEHVnHaIUhnHjgYD.GetElementIdentifierName(elementIdentifierId), elementIdentifierId, KeyCode.None);
				}
			}
			return ControllerPollingInfo.TCcbVoDrsLjCcBTyyQNnWzCBaEGA();
		}

		public override IEnumerable<ControllerPollingInfo> PollForAllElements()
		{
			return new fMZGDhviQkvHJfAGWbtBjUMyCgqm(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this
			};
		}

		public override IEnumerable<ControllerPollingInfo> PollForAllElementsDown()
		{
			return new HtWJjmVxkRQhqIhEaWYiykDtfmKE(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this
			};
		}

		public IEnumerable<ControllerPollingInfo> PollForAllAxes()
		{
			return new nwJxbgGibhWHtWkcVbrEgPGgCWORA(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this
			};
		}

		private void gjdBRcmBmbjkZAFXTlhOEtwctLxQb()
		{
			if (BijdrwGaINHJxnPnVjrnZCtkCPVCb == null)
			{
				BijdrwGaINHJxnPnVjrnZCtkCPVCb = new float[_axisCount];
			}
			if (uypLrHiCVLoiDTqTLBgqBIQjfPUGb != etHUIhDEqrnLEiPJGCMbqVceKELW)
			{
				etHUIhDEqrnLEiPJGCMbqVceKELW = uypLrHiCVLoiDTqTLBgqBIQjfPUGb;
				UpdateLoopType currentUpdateLoop = ReInput.currentUpdateLoop;
				for (int i = 0; i < _axisCount; i++)
				{
					BijdrwGaINHJxnPnVjrnZCtkCPVCb[i] = axes[i].xAdhNpJZqNksTfCDJERsuTOMoYVSA(currentUpdateLoop, _calibrationMap.GetAxis(i));
				}
			}
		}

		protected virtual bool IsPolledAxisActive(int index, out Pole pole, out int elementIdentifierId)
		{
			pole = Pole.Positive;
			elementIdentifierId = -1;
			if (axes[index].EKLxpZvxKyeifPSbgLSzhhhiYuui != null)
			{
				if (axes[index].EKLxpZvxKyeifPSbgLSzhhhiYuui._excludeFromPolling)
				{
					return false;
				}
				if (axes[index].EKLxpZvxKyeifPSbgLSzhhhiYuui._dataFormat == AxisCoordinateMode.Relative)
				{
					return false;
				}
			}
			float value = axes[index].xAdhNpJZqNksTfCDJERsuTOMoYVSA(ReInput.currentUpdateLoop, _calibrationMap.GetAxis(index)) - BijdrwGaINHJxnPnVjrnZCtkCPVCb[index];
			if (MathTools.Abs(value) <= axes[index].rtcgmrWiaobuHyScaIvcDhJnPmXHA)
			{
				return false;
			}
			pole = ((!(MathTools.Sign(value) >= 0f)) ? Pole.Negative : Pole.Positive);
			elementIdentifierId = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.axisElementIdentifierIds[index];
			if (elementIdentifierId < 0)
			{
				return false;
			}
			return true;
		}

		public bool ImportCalibrationMapFromXmlString(string xmlString)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			return calibrationMap.ImportXmlString(xmlString);
		}

		public bool ImportCalibrationMapFromJsonString(string jsonString)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return false;
			}
			return calibrationMap.ImportJsonString(jsonString);
		}

		internal override void PwSVyxiNOmkyuuXUHWyuymUyabog(UpdateLoopType P_0)
		{
			base.PwSVyxiNOmkyuuXUHWyuymUyabog(P_0);
			bool flag = ReInput.IsInputAllowed(_type);
			bool flag2 = _type == ControllerType.Joystick || _type == ControllerType.Custom;
			bool flag3 = _type == ControllerType.Joystick && ReInput.checkNeverPressed;
			bool flag4 = _type == ControllerType.Joystick && !BkAqQtJmzNvLobJflzLPUhNYRphu.hasReceivedInput;
			for (int i = 0; i < _axisCount; i++)
			{
				axes[i].OSdaMXbKzaxocQhQUwpRqqjiKWMZ(P_0);
				if (!flag || flag4 || (flag3 && !BkAqQtJmzNvLobJflzLPUhNYRphu.axisHasBeenPressedOSXLinux[i]))
				{
					axes[i].valueRaw = _calibrationMap.GetAxis(i).calibratedZero;
					axes[i].OfBGVYEcqDtxKFrYpGLjfDFRRFgR();
					continue;
				}
				axes[i].valueRaw = BkAqQtJmzNvLobJflzLPUhNYRphu.axisValues[i];
				if (flag2)
				{
					axes[i].dQOebbIzlPxLQZJPccrvmfbbpCPJ(_calibrationMap.GetAxis(i));
				}
				else
				{
					axes[i].dQOebbIzlPxLQZJPccrvmfbbpCPJ();
				}
			}
			for (int j = 0; j < _axis2DCount; j++)
			{
				axes2D[j].SiJfFLPgexWkpZJLIICzdmwvQQeLA();
			}
			for (int k = 0; k < _axisCount; k++)
			{
				axes[k].UtptVpcXhrlwQJVIcqtgoEuszodI();
			}
		}

		internal bool oZNTooZUteIdmEdiqibKSUQVHtqT(ActionElementMap P_0, int P_1, bool P_2, bool P_3, out float P_4)
		{
			P_4 = 0f;
			ControllerElementType elementType = P_0._elementType;
			if (P_1 != P_0._actionId)
			{
				return false;
			}
			int fBOOknsxpcCbVstgmGENdPFIgrbiA = P_0.FBOOknsxpcCbVstgmGENdPFIgrbiA;
			if (fBOOknsxpcCbVstgmGENdPFIgrbiA < 0 || fBOOknsxpcCbVstgmGENdPFIgrbiA >= _axisCount)
			{
				return false;
			}
			float num = ((!P_3) ? (P_2 ? axes[fBOOknsxpcCbVstgmGENdPFIgrbiA].valueRaw : axes[fBOOknsxpcCbVstgmGENdPFIgrbiA].value) : (P_2 ? axes[fBOOknsxpcCbVstgmGENdPFIgrbiA].valueRawPrev : axes[fBOOknsxpcCbVstgmGENdPFIgrbiA].valuePrev));
			if (MathTools.Approximately(num, 0f))
			{
				return true;
			}
			switch (elementType)
			{
			case ControllerElementType.Axis:
			{
				if (P_0._axisRange == AxisRange.Full)
				{
					if (P_0._invert)
					{
						num *= -1f;
					}
					break;
				}
				bool flag = MathTools.Sign(num) > 0f;
				if (flag && P_0._axisRange == AxisRange.Positive)
				{
					num = ((num >= 0f) ? num : 0f);
					if (P_0._axisContribution == Pole.Negative)
					{
						num *= -1f;
					}
				}
				else if (!flag && P_0._axisRange == AxisRange.Negative)
				{
					num = ((num <= 0f) ? num : 0f);
					if (P_0._axisContribution == Pole.Positive)
					{
						num *= -1f;
					}
				}
				else
				{
					num = 0f;
				}
				break;
			}
			case ControllerElementType.Button:
				if (P_0._axisContribution == Pole.Negative)
				{
					num *= -1f;
				}
				break;
			}
			P_4 = num;
			return true;
		}

		internal override void dSUizcQaOGnPQOOBZVgCNLRSewAe(ControllerMap P_0)
		{
			if (P_0 == null)
			{
				return;
			}
			if (!(P_0 is ControllerMapWithAxes controllerMapWithAxes))
			{
				Logger.LogWarning("Map type must inherit from ControllerMapWithAxes!");
				return;
			}
			base.dSUizcQaOGnPQOOBZVgCNLRSewAe(P_0);
			IList<ActionElementMap> axisMaps = controllerMapWithAxes.AxisMaps;
			for (int i = 0; i < axisMaps.Count; i++)
			{
				JpxRPMmkCiJxotCLchUqPLcjlZiQ(P_0, axisMaps[i]);
			}
			for (int num = axisMaps.Count - 1; num >= 0; num--)
			{
				if (axisMaps[num].elementIndex < 0)
				{
					P_0.DeleteElementMap(axisMaps[num].YVGDeWQAhUWKAOSPoxXuizkXaiTI);
				}
			}
		}

		internal override void JpxRPMmkCiJxotCLchUqPLcjlZiQ(ControllerMap P_0, ActionElementMap P_1)
		{
			if (P_1 != null)
			{
				base.JpxRPMmkCiJxotCLchUqPLcjlZiQ(P_0, P_1);
				if (P_1._elementType == ControllerElementType.Axis)
				{
					P_1.nCUqazOObDuxSjUNBaQhabwMxrSG(P_0);
				}
			}
		}

		internal void CtrjjJxhauzevwWMqlUzXBxtwPCT()
		{
			for (int i = 0; i < axisCount; i++)
			{
				switch (axes[i].EKLxpZvxKyeifPSbgLSzhhhiYuui._specialAxisType)
				{
				case SpecialAxisType.None:
					_calibrationMap.Axes[i].calibrationMode = AlternateAxisCalibrationType.Default;
					break;
				case SpecialAxisType.Throttle:
					_calibrationMap.Axes[i].calibrationMode = EnumConverter.ToAlternateAxisCalibrationType(ReInput.configVars.throttleCalibrationMode);
					break;
				default:
					throw new NotImplementedException();
				}
			}
		}

		internal override void SPGTRPyvIslcMdbPTItsewSLRPxx()
		{
			base.SPGTRPyvIslcMdbPTItsewSLRPxx();
			for (int i = 0; i < _axisCount; i++)
			{
				if (axes[i] != null)
				{
					axes[i].Reset();
				}
			}
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private IEnumerable<ControllerPollingInfo> rXAkxTyoyMrxwhMbMPkFGsngVgiq()
		{
			return base.PollForAllElements();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private IEnumerable<ControllerPollingInfo> kIFOMqoSovBmWyQlKpcSxOuXerX()
		{
			return base.PollForAllElementsDown();
		}
	}
}
