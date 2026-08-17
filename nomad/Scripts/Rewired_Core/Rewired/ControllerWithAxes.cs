using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

namespace Rewired
{
	public abstract class ControllerWithAxes : ControllerWithMap
	{
		private sealed class ydreqPdKdDyeIxcEObVdJCFMPlAZA : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
		{
			private int sjJKCnhBGcPFDkjJwFKJgaDDrjWs;

			private ControllerPollingInfo MCmikIWHCaETqaFdgATEClahiyrMc;

			private int lKVeZNHWodNryOJCuPKizPmmBFAdA;

			public ControllerWithAxes EOPioxwZrsQkTvKGgANfDYrWAQj;

			private int PcHDDXywOpCUQHyoHhMJnNjnKGlQA;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return MCmikIWHCaETqaFdgATEClahiyrMc;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return MCmikIWHCaETqaFdgATEClahiyrMc;
				}
			}

			[DebuggerHidden]
			public ydreqPdKdDyeIxcEObVdJCFMPlAZA(int P_0)
			{
				sjJKCnhBGcPFDkjJwFKJgaDDrjWs = P_0;
				lKVeZNHWodNryOJCuPKizPmmBFAdA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				sjJKCnhBGcPFDkjJwFKJgaDDrjWs = -2;
			}

			private bool MoveNext()
			{
				int num = sjJKCnhBGcPFDkjJwFKJgaDDrjWs;
				ControllerWithAxes eOPioxwZrsQkTvKGgANfDYrWAQj = EOPioxwZrsQkTvKGgANfDYrWAQj;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					sjJKCnhBGcPFDkjJwFKJgaDDrjWs = -1;
					goto IL_00a8;
				}
				sjJKCnhBGcPFDkjJwFKJgaDDrjWs = -1;
				if (ReInput._id != eOPioxwZrsQkTvKGgANfDYrWAQj.SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(eOPioxwZrsQkTvKGgANfDYrWAQj.SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return false;
				}
				eOPioxwZrsQkTvKGgANfDYrWAQj.UpdatePollingFrameTracking();
				eOPioxwZrsQkTvKGgANfDYrWAQj.BdkiwytCQmdyllXvwvrXLfjjfgbhA();
				PcHDDXywOpCUQHyoHhMJnNjnKGlQA = 0;
				goto IL_00ba;
				IL_00ba:
				if (PcHDDXywOpCUQHyoHhMJnNjnKGlQA < eOPioxwZrsQkTvKGgANfDYrWAQj._axisCount)
				{
					if (eOPioxwZrsQkTvKGgANfDYrWAQj.IsPolledAxisActive(PcHDDXywOpCUQHyoHhMJnNjnKGlQA, out var pole, out var elementIdentifierId))
					{
						MCmikIWHCaETqaFdgATEClahiyrMc = new ControllerPollingInfo(true, -1, eOPioxwZrsQkTvKGgANfDYrWAQj.id, eOPioxwZrsQkTvKGgANfDYrWAQj._name, eOPioxwZrsQkTvKGgANfDYrWAQj._type, ControllerElementType.Axis, PcHDDXywOpCUQHyoHhMJnNjnKGlQA, pole, eOPioxwZrsQkTvKGgANfDYrWAQj.UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetElementIdentifierName(elementIdentifierId), elementIdentifierId, KeyCode.None);
						sjJKCnhBGcPFDkjJwFKJgaDDrjWs = 1;
						return true;
					}
					goto IL_00a8;
				}
				return false;
				IL_00a8:
				PcHDDXywOpCUQHyoHhMJnNjnKGlQA++;
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
				ydreqPdKdDyeIxcEObVdJCFMPlAZA ydreqPdKdDyeIxcEObVdJCFMPlAZA2;
				if (sjJKCnhBGcPFDkjJwFKJgaDDrjWs == -2 && lKVeZNHWodNryOJCuPKizPmmBFAdA == Environment.CurrentManagedThreadId)
				{
					sjJKCnhBGcPFDkjJwFKJgaDDrjWs = 0;
					ydreqPdKdDyeIxcEObVdJCFMPlAZA2 = this;
				}
				else
				{
					ydreqPdKdDyeIxcEObVdJCFMPlAZA2 = new ydreqPdKdDyeIxcEObVdJCFMPlAZA(0);
					ydreqPdKdDyeIxcEObVdJCFMPlAZA2.EOPioxwZrsQkTvKGgANfDYrWAQj = EOPioxwZrsQkTvKGgANfDYrWAQj;
				}
				return ydreqPdKdDyeIxcEObVdJCFMPlAZA2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		private sealed class OBBcbNrkPqhumXHyuPcUAuAcnlWF : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
		{
			private int KTGIBQEqazsVliHxnpxBtFUGJxfN;

			private ControllerPollingInfo YHkZyAozSkTshtqcpGnQcMXxnRURA;

			private int iblVgTOQQCBCdvCDDTEmrcjRwtcP;

			public ControllerWithAxes adJFOkmajRMPASGlzIAFWPLVdvKCA;

			private IEnumerator<ControllerPollingInfo> ZbSMOdZuOWcjELemXeZraGzvgTwr;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return YHkZyAozSkTshtqcpGnQcMXxnRURA;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return YHkZyAozSkTshtqcpGnQcMXxnRURA;
				}
			}

			[DebuggerHidden]
			public OBBcbNrkPqhumXHyuPcUAuAcnlWF(int P_0)
			{
				KTGIBQEqazsVliHxnpxBtFUGJxfN = P_0;
				iblVgTOQQCBCdvCDDTEmrcjRwtcP = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				switch (KTGIBQEqazsVliHxnpxBtFUGJxfN)
				{
				case -3:
				case 1:
					try
					{
					}
					finally
					{
						PbSgIHcWadpCOegLgQgyAWGEOwWY();
					}
					break;
				case -4:
				case 2:
					try
					{
					}
					finally
					{
						kNALwFiNYwdOfxhwtetRBUAwobtmA();
					}
					break;
				}
				ZbSMOdZuOWcjELemXeZraGzvgTwr = null;
				KTGIBQEqazsVliHxnpxBtFUGJxfN = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int kTGIBQEqazsVliHxnpxBtFUGJxfN = KTGIBQEqazsVliHxnpxBtFUGJxfN;
					ControllerWithAxes controllerWithAxes = adJFOkmajRMPASGlzIAFWPLVdvKCA;
					switch (kTGIBQEqazsVliHxnpxBtFUGJxfN)
					{
					default:
						return false;
					case 0:
						KTGIBQEqazsVliHxnpxBtFUGJxfN = -1;
						if (ReInput._id != controllerWithAxes.SnkHeIsGgHerWcblJwOwvoQCoGCVA)
						{
							ReInput.CheckInitialized(controllerWithAxes.SnkHeIsGgHerWcblJwOwvoQCoGCVA);
							return false;
						}
						ZbSMOdZuOWcjELemXeZraGzvgTwr = ((Controller)controllerWithAxes).PollForAllElements().GetEnumerator();
						KTGIBQEqazsVliHxnpxBtFUGJxfN = -3;
						goto IL_0092;
					case 1:
						KTGIBQEqazsVliHxnpxBtFUGJxfN = -3;
						goto IL_0092;
					case 2:
						{
							KTGIBQEqazsVliHxnpxBtFUGJxfN = -4;
							break;
						}
						IL_0092:
						if (ZbSMOdZuOWcjELemXeZraGzvgTwr.MoveNext())
						{
							ControllerPollingInfo current = ZbSMOdZuOWcjELemXeZraGzvgTwr.Current;
							YHkZyAozSkTshtqcpGnQcMXxnRURA = current;
							KTGIBQEqazsVliHxnpxBtFUGJxfN = 1;
							return true;
						}
						PbSgIHcWadpCOegLgQgyAWGEOwWY();
						ZbSMOdZuOWcjELemXeZraGzvgTwr = null;
						ZbSMOdZuOWcjELemXeZraGzvgTwr = controllerWithAxes.PollForAllAxes().GetEnumerator();
						KTGIBQEqazsVliHxnpxBtFUGJxfN = -4;
						break;
					}
					if (ZbSMOdZuOWcjELemXeZraGzvgTwr.MoveNext())
					{
						ControllerPollingInfo current2 = ZbSMOdZuOWcjELemXeZraGzvgTwr.Current;
						YHkZyAozSkTshtqcpGnQcMXxnRURA = current2;
						KTGIBQEqazsVliHxnpxBtFUGJxfN = 2;
						return true;
					}
					kNALwFiNYwdOfxhwtetRBUAwobtmA();
					ZbSMOdZuOWcjELemXeZraGzvgTwr = null;
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

			private void PbSgIHcWadpCOegLgQgyAWGEOwWY()
			{
				KTGIBQEqazsVliHxnpxBtFUGJxfN = -1;
				if (ZbSMOdZuOWcjELemXeZraGzvgTwr != null)
				{
					ZbSMOdZuOWcjELemXeZraGzvgTwr.Dispose();
				}
			}

			private void kNALwFiNYwdOfxhwtetRBUAwobtmA()
			{
				KTGIBQEqazsVliHxnpxBtFUGJxfN = -1;
				if (ZbSMOdZuOWcjELemXeZraGzvgTwr != null)
				{
					ZbSMOdZuOWcjELemXeZraGzvgTwr.Dispose();
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
				OBBcbNrkPqhumXHyuPcUAuAcnlWF oBBcbNrkPqhumXHyuPcUAuAcnlWF;
				if (KTGIBQEqazsVliHxnpxBtFUGJxfN == -2 && iblVgTOQQCBCdvCDDTEmrcjRwtcP == Environment.CurrentManagedThreadId)
				{
					KTGIBQEqazsVliHxnpxBtFUGJxfN = 0;
					oBBcbNrkPqhumXHyuPcUAuAcnlWF = this;
				}
				else
				{
					oBBcbNrkPqhumXHyuPcUAuAcnlWF = new OBBcbNrkPqhumXHyuPcUAuAcnlWF(0);
					oBBcbNrkPqhumXHyuPcUAuAcnlWF.adJFOkmajRMPASGlzIAFWPLVdvKCA = adJFOkmajRMPASGlzIAFWPLVdvKCA;
				}
				return oBBcbNrkPqhumXHyuPcUAuAcnlWF;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerPollingInfo>)this).GetEnumerator();
			}
		}

		private sealed class WivtzPbZJxVVDPluQOQeDMTVMtQN : IEnumerable<ControllerPollingInfo>, IEnumerable, IEnumerator<ControllerPollingInfo>, IEnumerator, IDisposable
		{
			private int CcUVuOnMGNfICbVoJgeqchgIRQxc;

			private ControllerPollingInfo lGTcooKMEouQwdfGHpbjAZTJbFGP;

			private int bPgBEXAEunFPcITUACombzDMQqgic;

			public ControllerWithAxes UsOGUlxExbFuIQOYLGyXpmeURYWL;

			private IEnumerator<ControllerPollingInfo> DFgWKjmkCKnQllWJftPuWlBmvZhC;

			ControllerPollingInfo IEnumerator<ControllerPollingInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return lGTcooKMEouQwdfGHpbjAZTJbFGP;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return lGTcooKMEouQwdfGHpbjAZTJbFGP;
				}
			}

			[DebuggerHidden]
			public WivtzPbZJxVVDPluQOQeDMTVMtQN(int P_0)
			{
				CcUVuOnMGNfICbVoJgeqchgIRQxc = P_0;
				bPgBEXAEunFPcITUACombzDMQqgic = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				switch (CcUVuOnMGNfICbVoJgeqchgIRQxc)
				{
				case -3:
				case 1:
					try
					{
					}
					finally
					{
						beLiaJTJSjHMbLaMsJuERyRDLpvh();
					}
					break;
				case -4:
				case 2:
					try
					{
					}
					finally
					{
						aGgIOoEsakhOtrsVPsJuftLyTTGt();
					}
					break;
				}
				DFgWKjmkCKnQllWJftPuWlBmvZhC = null;
				CcUVuOnMGNfICbVoJgeqchgIRQxc = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int ccUVuOnMGNfICbVoJgeqchgIRQxc = CcUVuOnMGNfICbVoJgeqchgIRQxc;
					ControllerWithAxes usOGUlxExbFuIQOYLGyXpmeURYWL = UsOGUlxExbFuIQOYLGyXpmeURYWL;
					switch (ccUVuOnMGNfICbVoJgeqchgIRQxc)
					{
					default:
						return false;
					case 0:
						CcUVuOnMGNfICbVoJgeqchgIRQxc = -1;
						if (ReInput._id != usOGUlxExbFuIQOYLGyXpmeURYWL.SnkHeIsGgHerWcblJwOwvoQCoGCVA)
						{
							ReInput.CheckInitialized(usOGUlxExbFuIQOYLGyXpmeURYWL.SnkHeIsGgHerWcblJwOwvoQCoGCVA);
							return false;
						}
						DFgWKjmkCKnQllWJftPuWlBmvZhC = ((Controller)usOGUlxExbFuIQOYLGyXpmeURYWL).PollForAllElementsDown().GetEnumerator();
						CcUVuOnMGNfICbVoJgeqchgIRQxc = -3;
						goto IL_0092;
					case 1:
						CcUVuOnMGNfICbVoJgeqchgIRQxc = -3;
						goto IL_0092;
					case 2:
						{
							CcUVuOnMGNfICbVoJgeqchgIRQxc = -4;
							break;
						}
						IL_0092:
						if (DFgWKjmkCKnQllWJftPuWlBmvZhC.MoveNext())
						{
							ControllerPollingInfo current = DFgWKjmkCKnQllWJftPuWlBmvZhC.Current;
							lGTcooKMEouQwdfGHpbjAZTJbFGP = current;
							CcUVuOnMGNfICbVoJgeqchgIRQxc = 1;
							return true;
						}
						beLiaJTJSjHMbLaMsJuERyRDLpvh();
						DFgWKjmkCKnQllWJftPuWlBmvZhC = null;
						DFgWKjmkCKnQllWJftPuWlBmvZhC = usOGUlxExbFuIQOYLGyXpmeURYWL.PollForAllAxes().GetEnumerator();
						CcUVuOnMGNfICbVoJgeqchgIRQxc = -4;
						break;
					}
					if (DFgWKjmkCKnQllWJftPuWlBmvZhC.MoveNext())
					{
						ControllerPollingInfo current2 = DFgWKjmkCKnQllWJftPuWlBmvZhC.Current;
						lGTcooKMEouQwdfGHpbjAZTJbFGP = current2;
						CcUVuOnMGNfICbVoJgeqchgIRQxc = 2;
						return true;
					}
					aGgIOoEsakhOtrsVPsJuftLyTTGt();
					DFgWKjmkCKnQllWJftPuWlBmvZhC = null;
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

			private void beLiaJTJSjHMbLaMsJuERyRDLpvh()
			{
				CcUVuOnMGNfICbVoJgeqchgIRQxc = -1;
				if (DFgWKjmkCKnQllWJftPuWlBmvZhC != null)
				{
					DFgWKjmkCKnQllWJftPuWlBmvZhC.Dispose();
				}
			}

			private void aGgIOoEsakhOtrsVPsJuftLyTTGt()
			{
				CcUVuOnMGNfICbVoJgeqchgIRQxc = -1;
				if (DFgWKjmkCKnQllWJftPuWlBmvZhC != null)
				{
					DFgWKjmkCKnQllWJftPuWlBmvZhC.Dispose();
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
				WivtzPbZJxVVDPluQOQeDMTVMtQN wivtzPbZJxVVDPluQOQeDMTVMtQN;
				if (CcUVuOnMGNfICbVoJgeqchgIRQxc == -2 && bPgBEXAEunFPcITUACombzDMQqgic == Environment.CurrentManagedThreadId)
				{
					CcUVuOnMGNfICbVoJgeqchgIRQxc = 0;
					wivtzPbZJxVVDPluQOQeDMTVMtQN = this;
				}
				else
				{
					wivtzPbZJxVVDPluQOQeDMTVMtQN = new WivtzPbZJxVVDPluQOQeDMTVMtQN(0);
					wivtzPbZJxVVDPluQOQeDMTVMtQN.UsOGUlxExbFuIQOYLGyXpmeURYWL = UsOGUlxExbFuIQOYLGyXpmeURYWL;
				}
				return wivtzPbZJxVVDPluQOQeDMTVMtQN;
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

		protected readonly CalibrationMap _calibrationMap;

		private float[] CrHELjrxkmiOYUbbHTIKzDjwYBoj;

		private uint TNnsHhGazbgiOQDLsSvZMTkviipP = 4294967295u;

		private TimerAbs RQkKyPdzrrNJJHyeaOXIGPjnkmMX;

		private float[] lggCQHBYAIFTBlLgTKeHoXrsbxks;

		private Func<int, int> LfZTTXojjhwUFirPQCiwnRbNkXcQ;

		public int axisCount
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return 0;
				}
				return _axisCount;
			}
		}

		public int axis2DCount
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return 0;
				}
				return _axis2DCount;
			}
		}

		public IList<Axis> Axes
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<Axis>.EmptyReadOnlyIListT;
				}
				return axes_readOnly;
			}
		}

		public IList<Axis2D> Axes2D
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<Axis2D>.EmptyReadOnlyIListT;
				}
				return axes2D_readOnly;
			}
		}

		public CalibrationMap calibrationMap
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return null;
				}
				return _calibrationMap;
			}
		}

		public IList<ControllerElementIdentifier> AxisElementIdentifiers
		{
			get
			{
				if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
				{
					ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
					return EmptyObjects<ControllerElementIdentifier>.EmptyReadOnlyIListT;
				}
				return UNRIOyvPojfCPrjRsEYcHBwwkZqS.axisElementIdentifiers_readOnly;
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
				VSJEcXGbudCogDmpJpnIeQxXMTbBB(axes[i]);
			}
			axes_readOnly = new ReadOnlyCollection<Axis>(axes);
			Func<int, int> func = null;
			if (base.extension is IAxisCalibrationIndexMap)
			{
				func = (int num2) => (base.extension is IAxisCalibrationIndexMap axisCalibrationIndexMap) ? axisCalibrationIndexMap.GetMappedAxisIndex(num2) : num2;
			}
			_calibrationMap = new CalibrationMap(P_10.hwAxisCalibrationData, func);
			_axis2DCount = P_10.axis2DCount;
			axes2D = new Axis2D[_axis2DCount];
			for (int num = 0; num < _axis2DCount; num++)
			{
				try
				{
					HardwareJoystickMap.CompoundElement axis2DData = P_10.GetAxis2DData(num);
					if (axis2DData == null)
					{
						Logger.LogError("Error creating Axis2D from hardware map! CompoundElement is null!");
						axes2D[num] = new Axis2D(this, axis2DData.elementIdentifier, "Axis 2D " + num, null, null, 0, 0, null);
						continue;
					}
					int axisIndex = P_10.GetAxisIndex(axis2DData.componentElementIdentifiers[0]);
					int axisIndex2 = P_10.GetAxisIndex(axis2DData.componentElementIdentifiers[1]);
					if (axisIndex < 0 || axisIndex >= _axisCount || axisIndex2 < 0 || axisIndex2 >= _axisCount)
					{
						axes2D[num] = new Axis2D(this, axis2DData.elementIdentifier, "Axis 2D " + num, null, null, 0, 0, null);
					}
					else
					{
						axes2D[num] = new Axis2D(this, axis2DData.elementIdentifier, "Axis 2D " + num, axes[axisIndex], axes[axisIndex2], axisIndex, axisIndex2, _calibrationMap);
					}
				}
				catch
				{
					Logger.LogError("Error creating Axis2D from hardware map! An exception was thrown.");
					axes2D[num] = new Axis2D(this, -1, "Axis 2D " + num, null, null, 0, 0, null);
				}
				finally
				{
					eTsBnmdKizZCCukLuZJWklbrvzdOA(axes2D[num]);
				}
			}
			axes2D_readOnly = new ReadOnlyCollection<Axis2D>(axes2D);
			ktsztMwHvfZzTtmoLTOeIhofqqMF();
			LfZTTXojjhwUFirPQCiwnRbNkXcQ = P_10.GetAxisIndex;
		}

		public override Element GetElementById(int elementIdentifierId)
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
			Element elementById = base.GetElementById(elementIdentifierId);
			if (elementById != null)
			{
				return elementById;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0)
			{
				return null;
			}
			return axes[axisIndex];
		}

		public int GetAxisIndexById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return -1;
			}
			return UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
		}

		public float GetAxis(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0f;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0f;
			}
			return axes[axisIndex].value;
		}

		public float GetAxisPrevById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0f;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0f;
			}
			return axes[axisIndex].valuePrev;
		}

		public float GetAxisRawById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0f;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0f;
			}
			return axes[axisIndex].valueRaw;
		}

		public float GetAxisRawPrevById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0f;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0f;
			}
			return axes[axisIndex].valueRawPrev;
		}

		public double GetAxisTimeActiveById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].timeActive;
		}

		public double GetAxisTimeInactiveById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].timeInactive;
		}

		public double GetAxisLastTimeActiveById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].lastTimeActive;
		}

		public double GetAxisLastTimeInactiveById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].lastTimeInactive;
		}

		public double GetAxisRawTimeActiveById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].timeActiveRaw;
		}

		public double GetAxisRawTimeInactiveById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].timeInactiveRaw;
		}

		public double GetAxisRawLastTimeActiveById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].lastTimeActiveRaw;
		}

		public double GetAxisRawLastTimeInactiveById(int elementIdentifierId)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			int axisIndex = UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetAxisIndex(elementIdentifierId);
			if (axisIndex < 0 || axisIndex >= _axisCount)
			{
				return 0.0;
			}
			return axes[axisIndex].lastTimeInactiveRaw;
		}

		public Vector2 GetAxis2D(int index)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return 0.0;
			}
			return GetLastTimeActive(useRawValues: false);
		}

		public override double GetLastTimeActive(bool useRawValues)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
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
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
			}
			UpdatePollingFrameTracking();
			BdkiwytCQmdyllXvwvrXLfjjfgbhA();
			for (int i = 0; i < _axisCount; i++)
			{
				if (IsPolledAxisActive(i, out var pole, out var elementIdentifierId))
				{
					return new ControllerPollingInfo(true, -1, id, _name, _type, ControllerElementType.Axis, i, pole, UNRIOyvPojfCPrjRsEYcHBwwkZqS.GetElementIdentifierName(elementIdentifierId), elementIdentifierId, KeyCode.None);
				}
			}
			return ControllerPollingInfo.AqzEIPyMjpyXUFHmMVsymezoLmoQ();
		}

		[IteratorStateMachine(typeof(OBBcbNrkPqhumXHyuPcUAuAcnlWF))]
		public override IEnumerable<ControllerPollingInfo> PollForAllElements()
		{
			return new OBBcbNrkPqhumXHyuPcUAuAcnlWF(-2)
			{
				adJFOkmajRMPASGlzIAFWPLVdvKCA = this
			};
		}

		[IteratorStateMachine(typeof(WivtzPbZJxVVDPluQOQeDMTVMtQN))]
		public override IEnumerable<ControllerPollingInfo> PollForAllElementsDown()
		{
			return new WivtzPbZJxVVDPluQOQeDMTVMtQN(-2)
			{
				UsOGUlxExbFuIQOYLGyXpmeURYWL = this
			};
		}

		[IteratorStateMachine(typeof(ydreqPdKdDyeIxcEObVdJCFMPlAZA))]
		public IEnumerable<ControllerPollingInfo> PollForAllAxes()
		{
			return new ydreqPdKdDyeIxcEObVdJCFMPlAZA(-2)
			{
				EOPioxwZrsQkTvKGgANfDYrWAQj = this
			};
		}

		private void BdkiwytCQmdyllXvwvrXLfjjfgbhA()
		{
			if (CrHELjrxkmiOYUbbHTIKzDjwYBoj == null)
			{
				CrHELjrxkmiOYUbbHTIKzDjwYBoj = new float[_axisCount];
			}
			if (WFyUDqcKJyQDAXEmOKARWhCbapCI != TNnsHhGazbgiOQDLsSvZMTkviipP)
			{
				TNnsHhGazbgiOQDLsSvZMTkviipP = WFyUDqcKJyQDAXEmOKARWhCbapCI;
				UpdateLoopType currentUpdateLoop = ReInput.currentUpdateLoop;
				for (int i = 0; i < _axisCount; i++)
				{
					CrHELjrxkmiOYUbbHTIKzDjwYBoj[i] = axes[i].MjUcmHSsSWDyqbnOlMgFROvslKJHA(currentUpdateLoop, _calibrationMap.GetAxis(i));
				}
			}
		}

		protected virtual bool IsPolledAxisActive(int index, out Pole pole, out int elementIdentifierId)
		{
			pole = Pole.Positive;
			elementIdentifierId = -1;
			if (axes[index].excludeFromPolling)
			{
				return false;
			}
			float value;
			switch (axes[index].axisCoordinateMode)
			{
			case AxisCoordinateMode.Absolute:
				value = axes[index].MjUcmHSsSWDyqbnOlMgFROvslKJHA(ReInput.currentUpdateLoop, _calibrationMap.GetAxis(index)) - CrHELjrxkmiOYUbbHTIKzDjwYBoj[index];
				break;
			case AxisCoordinateMode.Relative:
				if (lggCQHBYAIFTBlLgTKeHoXrsbxks == null)
				{
					lggCQHBYAIFTBlLgTKeHoXrsbxks = new float[_axisCount];
				}
				if (RQkKyPdzrrNJJHyeaOXIGPjnkmMX == null)
				{
					RQkKyPdzrrNJJHyeaOXIGPjnkmMX = new TimerAbs(1.0);
				}
				if (RQkKyPdzrrNJJHyeaOXIGPjnkmMX.Update() || !RQkKyPdzrrNJJHyeaOXIGPjnkmMX.running)
				{
					RQkKyPdzrrNJJHyeaOXIGPjnkmMX.Start();
					Array.Clear(lggCQHBYAIFTBlLgTKeHoXrsbxks, 0, lggCQHBYAIFTBlLgTKeHoXrsbxks.Length);
				}
				lggCQHBYAIFTBlLgTKeHoXrsbxks[index] += axes[index].valueRaw;
				value = lggCQHBYAIFTBlLgTKeHoXrsbxks[index];
				break;
			default:
				throw new NotImplementedException();
			}
			if (MathTools.Abs(value) <= axes[index].ZmUbemxBLMMLeVyysMXgQzhrblwr)
			{
				return false;
			}
			pole = ((!(MathTools.Sign(value) >= 0f)) ? Pole.Negative : Pole.Positive);
			elementIdentifierId = UNRIOyvPojfCPrjRsEYcHBwwkZqS.axisElementIdentifierIds[index];
			if (elementIdentifierId < 0)
			{
				return false;
			}
			return true;
		}

		public bool ImportCalibrationMapFromXmlString(string xmlString)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			return calibrationMap.ImportXmlString(xmlString);
		}

		public bool ImportCalibrationMapFromJsonString(string jsonString)
		{
			if (ReInput._id != SnkHeIsGgHerWcblJwOwvoQCoGCVA)
			{
				ReInput.CheckInitialized(SnkHeIsGgHerWcblJwOwvoQCoGCVA);
				return false;
			}
			return calibrationMap.ImportJsonString(jsonString);
		}

		internal virtual void ZHABOdGNpKMPRYvZWajChRBuWCjCA(UpdateLoopType P_0)
		{
			base.PTpLZPTdIGBCXbVzlMCHCqylApVQA(P_0);
			bool flag = ReInput.IsInputAllowed(_type);
			bool flag2 = _type == ControllerType.Joystick || _type == ControllerType.Custom;
			bool flag3 = _type == ControllerType.Joystick && ReInput.checkNeverPressed;
			bool flag4 = _type == ControllerType.Joystick && !yZwGORAVRJPjNCmxxWIIoQgNomuqA.hasReceivedInput;
			for (int i = 0; i < _axisCount; i++)
			{
				axes[i].djDAGcFGahfpCIQyOMeGtBhgCdRjb(P_0);
				if (!flag || flag4 || (flag3 && !yZwGORAVRJPjNCmxxWIIoQgNomuqA.axisHasBeenPressedOSXLinux[i]))
				{
					axes[i].valueRaw = _calibrationMap.GetAxis(i).calibratedZero;
					axes[i].WdjBmJghRNEhxNnKSipACAdKSKGfA();
					continue;
				}
				axes[i].valueRaw = yZwGORAVRJPjNCmxxWIIoQgNomuqA.axisValues[i];
				if (flag2)
				{
					axes[i].VaFdUwCfBYWoHlFEnHpkRJWicfYP(_calibrationMap.GetAxis(i));
				}
				else
				{
					axes[i].CWlYieOkidEnFODOKxOpjHXYAYnbA();
				}
			}
			for (int j = 0; j < _axis2DCount; j++)
			{
				axes2D[j].fabXstqoGnFPFRfQUDZCKnMUIfwB();
			}
			for (int k = 0; k < _axisCount; k++)
			{
				axes[k].htPLiNlTbUoEUTdlgcwYKZkEMZTn();
			}
		}

		internal bool xYieDEgzHjpAALzGGAlrgVIEtdXHB(ActionElementMap P_0, int P_1, bool P_2, bool P_3, out float P_4)
		{
			P_4 = 0f;
			ControllerElementType elementType = P_0._elementType;
			if (P_1 != P_0._actionId)
			{
				return false;
			}
			int uxemeTqImFAncCLpTkOkfOWaWKUK = P_0.uxemeTqImFAncCLpTkOkfOWaWKUK;
			if (uxemeTqImFAncCLpTkOkfOWaWKUK < 0 || uxemeTqImFAncCLpTkOkfOWaWKUK >= _axisCount)
			{
				return false;
			}
			float num = ((!P_3) ? (P_2 ? axes[uxemeTqImFAncCLpTkOkfOWaWKUK].valueRaw : axes[uxemeTqImFAncCLpTkOkfOWaWKUK].value) : (P_2 ? axes[uxemeTqImFAncCLpTkOkfOWaWKUK].valueRawPrev : axes[uxemeTqImFAncCLpTkOkfOWaWKUK].valuePrev));
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

		internal virtual void kurlZUXXbbyPqIulFGEAazomoqZK(ControllerMap P_0)
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
			try
			{
				ControllerMap.RAmMePHwhbbjmrfLAYKtBaJPbccQ();
				base.FGOWYFxDhAlAiuIOIJEWlURoAViy(P_0);
				IList<ActionElementMap> axisMaps = controllerMapWithAxes.AxisMaps;
				for (int i = 0; i < axisMaps.Count; i++)
				{
					VnQORoKBKYcDfQniJOyRPalZgtMZ(P_0, axisMaps[i]);
				}
				for (int num = axisMaps.Count - 1; num >= 0; num--)
				{
					if (axisMaps[num].elementIndex < 0)
					{
						P_0.DeleteElementMap(axisMaps[num].nJilCjIhFvMUTsTBcUWuYpormNsu);
					}
				}
			}
			finally
			{
				ControllerMap.oeOZZgeXJicFbaxfdmvQlNMqgCjfA();
			}
		}

		internal virtual void OZRDWhHxVDCWSWjqzgAkGQNnwRtzA(ControllerMap P_0, ActionElementMap P_1)
		{
			if (P_1 != null)
			{
				base.VnQORoKBKYcDfQniJOyRPalZgtMZ(P_0, P_1);
				if (P_1._elementType == ControllerElementType.Axis)
				{
					P_1.CeDmmMmdwtjVdcVPRFXshDHqgijv(P_0);
				}
			}
		}

		internal void ktsztMwHvfZzTtmoLTOeIhofqqMF()
		{
			for (int i = 0; i < axisCount; i++)
			{
				switch (axes[i].dyBDSFNRWhEclaVHduPpTilGwQgN._specialAxisType)
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

		internal virtual void hlKcVTBoQXLWkqiUgXEameLeIdsf()
		{
			base.bglRweWaaTFfEiIQjwyzpBARhNXC();
			if (RQkKyPdzrrNJJHyeaOXIGPjnkmMX != null)
			{
				RQkKyPdzrrNJJHyeaOXIGPjnkmMX.Clear();
			}
			for (int i = 0; i < _axisCount; i++)
			{
				if (axes[i] != null)
				{
					axes[i].Reset();
				}
			}
		}

		[CompilerGenerated]
		private int ipUgBrTKoOVdbWAsnbbxqADnZtqA(int P_0)
		{
			if (base.extension is IAxisCalibrationIndexMap axisCalibrationIndexMap)
			{
				return axisCalibrationIndexMap.GetMappedAxisIndex(P_0);
			}
			return P_0;
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private IEnumerable<ControllerPollingInfo> PLuhvNLDGbgnrLGCkalYEEwvytiTA()
		{
			return base.PollForAllElements();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private IEnumerable<ControllerPollingInfo> yOUYrhpdeaFGutiRLhCLzJWLMMTw()
		{
			return base.PollForAllElementsDown();
		}
	}
}
