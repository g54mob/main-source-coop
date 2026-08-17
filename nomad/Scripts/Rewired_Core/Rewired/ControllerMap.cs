using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired
{
	public abstract class ControllerMap
	{
		private class OpWxcQrhSLqRHNtoglCEeChHsGtf : IComparer<ActionElementMap>
		{
			public static OpWxcQrhSLqRHNtoglCEeChHsGtf TEMNTetkcAtnimYlushySIgqjEef;

			public static OpWxcQrhSLqRHNtoglCEeChHsGtf MIhwAOqhmoaWRsWmUHdjgEsxahKr => TEMNTetkcAtnimYlushySIgqjEef ?? (TEMNTetkcAtnimYlushySIgqjEef = new OpWxcQrhSLqRHNtoglCEeChHsGtf());

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

			int IComparer<ActionElementMap>.Compare(ActionElementMap x, ActionElementMap y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Compare
				return this.Compare(x, y);
			}
		}

		private sealed class ICLKocmWNtDNVLWZmeBmjIrNwBBC : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
		{
			private int vQXErxQQwXuEhTzDhvLnIhiAqMVL;

			private ActionElementMap RHAdRStPOrytEEkxUgANQDHZtpxF;

			private int pJiQdCaQxEzCwKKbZFxvdcAZGkyJA;

			public ControllerMap TzLsiJLhMQBiAVvRVRBqLdqEVeIT;

			private int ShufuRaBPOppGkSvXNSGglHAZxvJA;

			public int tyvoGGHkdsaYpGUnmunQVcpgGMDzA;

			private bool hBtJsrlHApcMsgzWYkuPkbmCWwoTA;

			public bool npWAenJALalYNeTGIUsFWpVjfEMSA;

			private IList<ActionElementMap> VmvVThmolMkxOyGLqUjwxsEJRLAk;

			private int CmMaLjcxIGhTBFSNeBYGXRjqlhLXA;

			private int EKPulFimIUNqbzPwAggXlFfVcOQm;

			ActionElementMap IEnumerator<ActionElementMap>.Current
			{
				[DebuggerHidden]
				get
				{
					return RHAdRStPOrytEEkxUgANQDHZtpxF;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return RHAdRStPOrytEEkxUgANQDHZtpxF;
				}
			}

			[DebuggerHidden]
			public ICLKocmWNtDNVLWZmeBmjIrNwBBC(int P_0)
			{
				vQXErxQQwXuEhTzDhvLnIhiAqMVL = P_0;
				pJiQdCaQxEzCwKKbZFxvdcAZGkyJA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				VmvVThmolMkxOyGLqUjwxsEJRLAk = null;
				vQXErxQQwXuEhTzDhvLnIhiAqMVL = -2;
			}

			private bool MoveNext()
			{
				int num = vQXErxQQwXuEhTzDhvLnIhiAqMVL;
				ControllerMap tzLsiJLhMQBiAVvRVRBqLdqEVeIT = TzLsiJLhMQBiAVvRVRBqLdqEVeIT;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					vQXErxQQwXuEhTzDhvLnIhiAqMVL = -1;
					goto IL_00af;
				}
				vQXErxQQwXuEhTzDhvLnIhiAqMVL = -1;
				if (ReInput._id != tzLsiJLhMQBiAVvRVRBqLdqEVeIT.zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(tzLsiJLhMQBiAVvRVRBqLdqEVeIT.zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return false;
				}
				if (ShufuRaBPOppGkSvXNSGglHAZxvJA < 0)
				{
					return false;
				}
				VmvVThmolMkxOyGLqUjwxsEJRLAk = tzLsiJLhMQBiAVvRVRBqLdqEVeIT.ButtonMaps;
				CmMaLjcxIGhTBFSNeBYGXRjqlhLXA = tzLsiJLhMQBiAVvRVRBqLdqEVeIT.buttonMapCount;
				EKPulFimIUNqbzPwAggXlFfVcOQm = 0;
				goto IL_00bf;
				IL_00bf:
				if (EKPulFimIUNqbzPwAggXlFfVcOQm < CmMaLjcxIGhTBFSNeBYGXRjqlhLXA)
				{
					ActionElementMap actionElementMap = VmvVThmolMkxOyGLqUjwxsEJRLAk[EKPulFimIUNqbzPwAggXlFfVcOQm];
					if (actionElementMap._actionId == ShufuRaBPOppGkSvXNSGglHAZxvJA && (!hBtJsrlHApcMsgzWYkuPkbmCWwoTA || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
					{
						RHAdRStPOrytEEkxUgANQDHZtpxF = actionElementMap;
						vQXErxQQwXuEhTzDhvLnIhiAqMVL = 1;
						return true;
					}
					goto IL_00af;
				}
				return false;
				IL_00af:
				EKPulFimIUNqbzPwAggXlFfVcOQm++;
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
				ICLKocmWNtDNVLWZmeBmjIrNwBBC iCLKocmWNtDNVLWZmeBmjIrNwBBC;
				if (vQXErxQQwXuEhTzDhvLnIhiAqMVL == -2 && pJiQdCaQxEzCwKKbZFxvdcAZGkyJA == Environment.CurrentManagedThreadId)
				{
					vQXErxQQwXuEhTzDhvLnIhiAqMVL = 0;
					iCLKocmWNtDNVLWZmeBmjIrNwBBC = this;
				}
				else
				{
					iCLKocmWNtDNVLWZmeBmjIrNwBBC = new ICLKocmWNtDNVLWZmeBmjIrNwBBC(0);
					iCLKocmWNtDNVLWZmeBmjIrNwBBC.TzLsiJLhMQBiAVvRVRBqLdqEVeIT = TzLsiJLhMQBiAVvRVRBqLdqEVeIT;
				}
				iCLKocmWNtDNVLWZmeBmjIrNwBBC.ShufuRaBPOppGkSvXNSGglHAZxvJA = tyvoGGHkdsaYpGUnmunQVcpgGMDzA;
				iCLKocmWNtDNVLWZmeBmjIrNwBBC.hBtJsrlHApcMsgzWYkuPkbmCWwoTA = npWAenJALalYNeTGIUsFWpVjfEMSA;
				return iCLKocmWNtDNVLWZmeBmjIrNwBBC;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
			}
		}

		private sealed class LwQLeasmNXehdQGxsAwelDDAClpA : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
		{
			private int qHABHHnxafwLxyiSXJYnNpJRBGQv;

			private ElementAssignmentConflictInfo OaRDRcFIoUcJjzkTaOpjSDAapAFxA;

			private int OxDUgzUJdonkBGusssENwqcngpuG;

			public ControllerMap GtWYzRSSMqhLbTJbiqFmheYBBZOJ;

			private ControllerMap rpHgJhdCRYAqLNxyNEMFvmweNkaw;

			public ControllerMap qKjdJfmoQhHTtIPLasZLPDVTWUHi;

			private bool VHKFnBEOXRXiwXXEGTuaDhRAAzbY;

			public bool AkoOwTVolifnznOKQWdZivRUBJgM;

			private IList<ActionElementMap> KIIfmQuzUFbRrszygAywnFnjROhV;

			private int NvBaZvZXgHvakHHzAoBlCuPRuhfA;

			private int WpelfviqWOSDZAvmpuJZcbZrJScH;

			private ActionElementMap MxyPBpZjMorKwbidNCzLFgUjHDrz;

			private int SajNBLkWifbDSnSvmobJSarPVykv;

			ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return OaRDRcFIoUcJjzkTaOpjSDAapAFxA;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return OaRDRcFIoUcJjzkTaOpjSDAapAFxA;
				}
			}

			[DebuggerHidden]
			public LwQLeasmNXehdQGxsAwelDDAClpA(int P_0)
			{
				qHABHHnxafwLxyiSXJYnNpJRBGQv = P_0;
				OxDUgzUJdonkBGusssENwqcngpuG = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				KIIfmQuzUFbRrszygAywnFnjROhV = null;
				MxyPBpZjMorKwbidNCzLFgUjHDrz = null;
				qHABHHnxafwLxyiSXJYnNpJRBGQv = -2;
			}

			private bool MoveNext()
			{
				int num = qHABHHnxafwLxyiSXJYnNpJRBGQv;
				ControllerMap gtWYzRSSMqhLbTJbiqFmheYBBZOJ = GtWYzRSSMqhLbTJbiqFmheYBBZOJ;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					qHABHHnxafwLxyiSXJYnNpJRBGQv = -1;
					goto IL_019c;
				}
				qHABHHnxafwLxyiSXJYnNpJRBGQv = -1;
				if (ReInput._id != gtWYzRSSMqhLbTJbiqFmheYBBZOJ.zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(gtWYzRSSMqhLbTJbiqFmheYBBZOJ.zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return false;
				}
				if (rpHgJhdCRYAqLNxyNEMFvmweNkaw == null || gtWYzRSSMqhLbTJbiqFmheYBBZOJ.VRlwPFoStycupprwoRTWAUXYBQXF == null)
				{
					return false;
				}
				if (VHKFnBEOXRXiwXXEGTuaDhRAAzbY && (!gtWYzRSSMqhLbTJbiqFmheYBBZOJ._enabled || !rpHgJhdCRYAqLNxyNEMFvmweNkaw._enabled))
				{
					return false;
				}
				KIIfmQuzUFbRrszygAywnFnjROhV = rpHgJhdCRYAqLNxyNEMFvmweNkaw.ButtonMaps;
				if (KIIfmQuzUFbRrszygAywnFnjROhV == null)
				{
					return false;
				}
				NvBaZvZXgHvakHHzAoBlCuPRuhfA = KIIfmQuzUFbRrszygAywnFnjROhV.Count;
				WpelfviqWOSDZAvmpuJZcbZrJScH = 0;
				goto IL_01d4;
				IL_01d4:
				if (WpelfviqWOSDZAvmpuJZcbZrJScH < gtWYzRSSMqhLbTJbiqFmheYBBZOJ.VRlwPFoStycupprwoRTWAUXYBQXF.Count)
				{
					MxyPBpZjMorKwbidNCzLFgUjHDrz = gtWYzRSSMqhLbTJbiqFmheYBBZOJ.VRlwPFoStycupprwoRTWAUXYBQXF[WpelfviqWOSDZAvmpuJZcbZrJScH];
					if (!VHKFnBEOXRXiwXXEGTuaDhRAAzbY || MxyPBpZjMorKwbidNCzLFgUjHDrz.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
					{
						SajNBLkWifbDSnSvmobJSarPVykv = 0;
						goto IL_01ac;
					}
					goto IL_01c4;
				}
				return false;
				IL_01ac:
				if (SajNBLkWifbDSnSvmobJSarPVykv < NvBaZvZXgHvakHHzAoBlCuPRuhfA)
				{
					ActionElementMap actionElementMap = KIIfmQuzUFbRrszygAywnFnjROhV[SajNBLkWifbDSnSvmobJSarPVykv];
					if ((!VHKFnBEOXRXiwXXEGTuaDhRAAzbY || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && MxyPBpZjMorKwbidNCzLFgUjHDrz.CheckForAssignmentConflict(actionElementMap))
					{
						OaRDRcFIoUcJjzkTaOpjSDAapAFxA = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(gtWYzRSSMqhLbTJbiqFmheYBBZOJ._categoryId).userAssignable, -1, gtWYzRSSMqhLbTJbiqFmheYBBZOJ._controllerType, gtWYzRSSMqhLbTJbiqFmheYBBZOJ._controllerId, gtWYzRSSMqhLbTJbiqFmheYBBZOJ._id, MxyPBpZjMorKwbidNCzLFgUjHDrz.nJilCjIhFvMUTsTBcUWuYpormNsu, MxyPBpZjMorKwbidNCzLFgUjHDrz._actionId, MxyPBpZjMorKwbidNCzLFgUjHDrz._elementType, MxyPBpZjMorKwbidNCzLFgUjHDrz._elementIdentifierId, MxyPBpZjMorKwbidNCzLFgUjHDrz.keyCode, MxyPBpZjMorKwbidNCzLFgUjHDrz.modifierKeyFlags);
						qHABHHnxafwLxyiSXJYnNpJRBGQv = 1;
						return true;
					}
					goto IL_019c;
				}
				MxyPBpZjMorKwbidNCzLFgUjHDrz = null;
				goto IL_01c4;
				IL_01c4:
				WpelfviqWOSDZAvmpuJZcbZrJScH++;
				goto IL_01d4;
				IL_019c:
				SajNBLkWifbDSnSvmobJSarPVykv++;
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
				LwQLeasmNXehdQGxsAwelDDAClpA lwQLeasmNXehdQGxsAwelDDAClpA;
				if (qHABHHnxafwLxyiSXJYnNpJRBGQv == -2 && OxDUgzUJdonkBGusssENwqcngpuG == Environment.CurrentManagedThreadId)
				{
					qHABHHnxafwLxyiSXJYnNpJRBGQv = 0;
					lwQLeasmNXehdQGxsAwelDDAClpA = this;
				}
				else
				{
					lwQLeasmNXehdQGxsAwelDDAClpA = new LwQLeasmNXehdQGxsAwelDDAClpA(0);
					lwQLeasmNXehdQGxsAwelDDAClpA.GtWYzRSSMqhLbTJbiqFmheYBBZOJ = GtWYzRSSMqhLbTJbiqFmheYBBZOJ;
				}
				lwQLeasmNXehdQGxsAwelDDAClpA.rpHgJhdCRYAqLNxyNEMFvmweNkaw = qKjdJfmoQhHTtIPLasZLPDVTWUHi;
				lwQLeasmNXehdQGxsAwelDDAClpA.VHKFnBEOXRXiwXXEGTuaDhRAAzbY = AkoOwTVolifnznOKQWdZivRUBJgM;
				return lwQLeasmNXehdQGxsAwelDDAClpA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private sealed class OKcoFyVDZgavbnogiSLgraSFdXwp : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
		{
			private int ATIWYCFawogYJgsEbXaycEdyZJiDA;

			private ElementAssignmentConflictInfo azpvqFAwRLWMDronMMBYRjoTRHTu;

			private int JYIDWjnQVBkTQnWKGJtNFEXJiRUA;

			public ControllerMap nAllwnSvcnoFhmIlFzDvWNEnApUX;

			private ActionElementMap KwySqpIvuZCJMprnpeeTHlGivvze;

			public ActionElementMap WPguKVwYihoGFCWKfFShufTxgcEDA;

			private bool qlnQLeuxvrarqzjFMThsDizJlzkq;

			public bool nhVybgNaShDajCdvsMQbCZACAnzcA;

			private int qAAgeABkYDnDMJSPUuOcHalBvsmoA;

			ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return azpvqFAwRLWMDronMMBYRjoTRHTu;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return azpvqFAwRLWMDronMMBYRjoTRHTu;
				}
			}

			[DebuggerHidden]
			public OKcoFyVDZgavbnogiSLgraSFdXwp(int P_0)
			{
				ATIWYCFawogYJgsEbXaycEdyZJiDA = P_0;
				JYIDWjnQVBkTQnWKGJtNFEXJiRUA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				ATIWYCFawogYJgsEbXaycEdyZJiDA = -2;
			}

			private bool MoveNext()
			{
				int aTIWYCFawogYJgsEbXaycEdyZJiDA = ATIWYCFawogYJgsEbXaycEdyZJiDA;
				ControllerMap controllerMap = nAllwnSvcnoFhmIlFzDvWNEnApUX;
				if (aTIWYCFawogYJgsEbXaycEdyZJiDA != 0)
				{
					if (aTIWYCFawogYJgsEbXaycEdyZJiDA != 1)
					{
						return false;
					}
					ATIWYCFawogYJgsEbXaycEdyZJiDA = -1;
					goto IL_0111;
				}
				ATIWYCFawogYJgsEbXaycEdyZJiDA = -1;
				if (ReInput._id != controllerMap.zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(controllerMap.zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return false;
				}
				if (KwySqpIvuZCJMprnpeeTHlGivvze == null || controllerMap.VRlwPFoStycupprwoRTWAUXYBQXF == null)
				{
					return false;
				}
				if (qlnQLeuxvrarqzjFMThsDizJlzkq && (!controllerMap._enabled || !KwySqpIvuZCJMprnpeeTHlGivvze.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					return false;
				}
				qAAgeABkYDnDMJSPUuOcHalBvsmoA = 0;
				goto IL_0121;
				IL_0111:
				qAAgeABkYDnDMJSPUuOcHalBvsmoA++;
				goto IL_0121;
				IL_0121:
				if (qAAgeABkYDnDMJSPUuOcHalBvsmoA < controllerMap.VRlwPFoStycupprwoRTWAUXYBQXF.Count)
				{
					ActionElementMap actionElementMap = controllerMap.VRlwPFoStycupprwoRTWAUXYBQXF[qAAgeABkYDnDMJSPUuOcHalBvsmoA];
					if ((!qlnQLeuxvrarqzjFMThsDizJlzkq || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.CheckForAssignmentConflict(KwySqpIvuZCJMprnpeeTHlGivvze))
					{
						azpvqFAwRLWMDronMMBYRjoTRHTu = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(controllerMap._categoryId).userAssignable, -1, controllerMap._controllerType, controllerMap._controllerId, controllerMap._id, actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu, actionElementMap._actionId, actionElementMap._elementType, actionElementMap._elementIdentifierId, actionElementMap.keyCode, actionElementMap.modifierKeyFlags);
						ATIWYCFawogYJgsEbXaycEdyZJiDA = 1;
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
				OKcoFyVDZgavbnogiSLgraSFdXwp oKcoFyVDZgavbnogiSLgraSFdXwp;
				if (ATIWYCFawogYJgsEbXaycEdyZJiDA == -2 && JYIDWjnQVBkTQnWKGJtNFEXJiRUA == Environment.CurrentManagedThreadId)
				{
					ATIWYCFawogYJgsEbXaycEdyZJiDA = 0;
					oKcoFyVDZgavbnogiSLgraSFdXwp = this;
				}
				else
				{
					oKcoFyVDZgavbnogiSLgraSFdXwp = new OKcoFyVDZgavbnogiSLgraSFdXwp(0);
					oKcoFyVDZgavbnogiSLgraSFdXwp.nAllwnSvcnoFhmIlFzDvWNEnApUX = nAllwnSvcnoFhmIlFzDvWNEnApUX;
				}
				oKcoFyVDZgavbnogiSLgraSFdXwp.KwySqpIvuZCJMprnpeeTHlGivvze = WPguKVwYihoGFCWKfFShufTxgcEDA;
				oKcoFyVDZgavbnogiSLgraSFdXwp.qlnQLeuxvrarqzjFMThsDizJlzkq = nhVybgNaShDajCdvsMQbCZACAnzcA;
				return oKcoFyVDZgavbnogiSLgraSFdXwp;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private sealed class eDIhlLTWSMxqHOHhtoRHPEOOPuqL : IEnumerable<ElementAssignmentConflictInfo>, IEnumerable, IEnumerator<ElementAssignmentConflictInfo>, IEnumerator, IDisposable
		{
			private int hpMVnYNjhrUbBfrmZmasmtlDgNMk;

			private ElementAssignmentConflictInfo gjiXZZvEPLnYgMVpiEPCuWQuElcm;

			private int IkiHyfnXOVnKfBGfIDIvhVLdfyjL;

			public ControllerMap pPVVqsnhRqyUDaNNziPsgqWigGUN;

			private bool HUgbhDXQnfVsimzKRVcoMzzNekjp;

			public bool abZtdrSQNsnemRbkBhQQUgiqWoMm;

			private ElementAssignmentConflictCheck HkhvsyXNaKASyXetSkuXAgpSFJHT;

			public ElementAssignmentConflictCheck fChgKHJgfKvftwJvhlUxPPghmVUaA;

			private ElementAssignment JMTqiZgcRltkMKaWLBQhbMtJDvTlA;

			private int npWrgXaEWDzJZwYqvnEngnuOeMXF;

			ElementAssignmentConflictInfo IEnumerator<ElementAssignmentConflictInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return gjiXZZvEPLnYgMVpiEPCuWQuElcm;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return gjiXZZvEPLnYgMVpiEPCuWQuElcm;
				}
			}

			[DebuggerHidden]
			public eDIhlLTWSMxqHOHhtoRHPEOOPuqL(int P_0)
			{
				hpMVnYNjhrUbBfrmZmasmtlDgNMk = P_0;
				IkiHyfnXOVnKfBGfIDIvhVLdfyjL = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				hpMVnYNjhrUbBfrmZmasmtlDgNMk = -2;
			}

			private bool MoveNext()
			{
				int num = hpMVnYNjhrUbBfrmZmasmtlDgNMk;
				ControllerMap controllerMap = pPVVqsnhRqyUDaNNziPsgqWigGUN;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					hpMVnYNjhrUbBfrmZmasmtlDgNMk = -1;
					goto IL_0123;
				}
				hpMVnYNjhrUbBfrmZmasmtlDgNMk = -1;
				if (ReInput._id != controllerMap.zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(controllerMap.zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return false;
				}
				if (HUgbhDXQnfVsimzKRVcoMzzNekjp && !controllerMap._enabled)
				{
					return false;
				}
				if (controllerMap.VRlwPFoStycupprwoRTWAUXYBQXF == null)
				{
					return false;
				}
				JMTqiZgcRltkMKaWLBQhbMtJDvTlA = HkhvsyXNaKASyXetSkuXAgpSFJHT.ToElementAssignment();
				npWrgXaEWDzJZwYqvnEngnuOeMXF = 0;
				goto IL_0133;
				IL_0133:
				if (npWrgXaEWDzJZwYqvnEngnuOeMXF < controllerMap.VRlwPFoStycupprwoRTWAUXYBQXF.Count)
				{
					ActionElementMap actionElementMap = controllerMap.VRlwPFoStycupprwoRTWAUXYBQXF[npWrgXaEWDzJZwYqvnEngnuOeMXF];
					if ((!HUgbhDXQnfVsimzKRVcoMzzNekjp || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu != HkhvsyXNaKASyXetSkuXAgpSFJHT.elementMapId && actionElementMap.CheckForAssignmentConflict(JMTqiZgcRltkMKaWLBQhbMtJDvTlA))
					{
						gjiXZZvEPLnYgMVpiEPCuWQuElcm = new ElementAssignmentConflictInfo(true, ReInput.mapping.GetMapCategory(controllerMap._categoryId).userAssignable, -1, controllerMap._controllerType, controllerMap._controllerId, controllerMap._id, actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu, actionElementMap._actionId, actionElementMap._elementType, actionElementMap._elementIdentifierId, actionElementMap.keyCode, actionElementMap.modifierKeyFlags);
						hpMVnYNjhrUbBfrmZmasmtlDgNMk = 1;
						return true;
					}
					goto IL_0123;
				}
				return false;
				IL_0123:
				npWrgXaEWDzJZwYqvnEngnuOeMXF++;
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
				eDIhlLTWSMxqHOHhtoRHPEOOPuqL eDIhlLTWSMxqHOHhtoRHPEOOPuqL2;
				if (hpMVnYNjhrUbBfrmZmasmtlDgNMk == -2 && IkiHyfnXOVnKfBGfIDIvhVLdfyjL == Environment.CurrentManagedThreadId)
				{
					hpMVnYNjhrUbBfrmZmasmtlDgNMk = 0;
					eDIhlLTWSMxqHOHhtoRHPEOOPuqL2 = this;
				}
				else
				{
					eDIhlLTWSMxqHOHhtoRHPEOOPuqL2 = new eDIhlLTWSMxqHOHhtoRHPEOOPuqL(0);
					eDIhlLTWSMxqHOHhtoRHPEOOPuqL2.pPVVqsnhRqyUDaNNziPsgqWigGUN = pPVVqsnhRqyUDaNNziPsgqWigGUN;
				}
				eDIhlLTWSMxqHOHhtoRHPEOOPuqL2.HkhvsyXNaKASyXetSkuXAgpSFJHT = fChgKHJgfKvftwJvhlUxPPghmVUaA;
				eDIhlLTWSMxqHOHhtoRHPEOOPuqL2.HUgbhDXQnfVsimzKRVcoMzzNekjp = abZtdrSQNsnemRbkBhQQUgiqWoMm;
				return eDIhlLTWSMxqHOHhtoRHPEOOPuqL2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ElementAssignmentConflictInfo>)this).GetEnumerator();
			}
		}

		private sealed class AZUVOpRozrCjJKrDJBtfgiNEUKURA : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
		{
			private int pFlvUXFmwIIfeNBGRkjGGhawhghP;

			private ActionElementMap VQTYqWxGwFmgJYUTvluzZwSUnWte;

			private int uLmSmiZiTkTEMSXTGNiLTjglLMZC;

			public ControllerMap CrLKRoLjtOpsHQpUKlULtPpjHFvl;

			private int iRHAamHpIgtBhFntaWlAurwokXVIA;

			public int sQPOpZxojasjunlpYcRqptzbbHrF;

			private bool plpGfBhXhKaRwsqMFzQwOtGoPvJtA;

			public bool ERLcPKKIYkqoaveWnXPgKrwXUkTG;

			private IEnumerator<ActionElementMap> NSYBsGIFvSXoKJvKYNpLIknYctSFb;

			ActionElementMap IEnumerator<ActionElementMap>.Current
			{
				[DebuggerHidden]
				get
				{
					return VQTYqWxGwFmgJYUTvluzZwSUnWte;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VQTYqWxGwFmgJYUTvluzZwSUnWte;
				}
			}

			[DebuggerHidden]
			public AZUVOpRozrCjJKrDJBtfgiNEUKURA(int P_0)
			{
				pFlvUXFmwIIfeNBGRkjGGhawhghP = P_0;
				uLmSmiZiTkTEMSXTGNiLTjglLMZC = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int num = pFlvUXFmwIIfeNBGRkjGGhawhghP;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						aoVbOlQNesHmZyozkXIZNjZhsmSC();
					}
				}
				NSYBsGIFvSXoKJvKYNpLIknYctSFb = null;
				pFlvUXFmwIIfeNBGRkjGGhawhghP = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int num = pFlvUXFmwIIfeNBGRkjGGhawhghP;
					ControllerMap crLKRoLjtOpsHQpUKlULtPpjHFvl = CrLKRoLjtOpsHQpUKlULtPpjHFvl;
					switch (num)
					{
					default:
						return false;
					case 0:
						pFlvUXFmwIIfeNBGRkjGGhawhghP = -1;
						if (ReInput._id != crLKRoLjtOpsHQpUKlULtPpjHFvl.zYMtfthQqWFUiFGChqAKAcaBAqFL)
						{
							ReInput.CheckInitialized(crLKRoLjtOpsHQpUKlULtPpjHFvl.zYMtfthQqWFUiFGChqAKAcaBAqFL);
							return false;
						}
						NSYBsGIFvSXoKJvKYNpLIknYctSFb = crLKRoLjtOpsHQpUKlULtPpjHFvl.AllMaps.GetEnumerator();
						pFlvUXFmwIIfeNBGRkjGGhawhghP = -3;
						break;
					case 1:
						pFlvUXFmwIIfeNBGRkjGGhawhghP = -3;
						break;
					}
					while (NSYBsGIFvSXoKJvKYNpLIknYctSFb.MoveNext())
					{
						ActionElementMap current = NSYBsGIFvSXoKJvKYNpLIknYctSFb.Current;
						if (current._actionId == iRHAamHpIgtBhFntaWlAurwokXVIA && (!plpGfBhXhKaRwsqMFzQwOtGoPvJtA || current.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
						{
							VQTYqWxGwFmgJYUTvluzZwSUnWte = current;
							pFlvUXFmwIIfeNBGRkjGGhawhghP = 1;
							return true;
						}
					}
					aoVbOlQNesHmZyozkXIZNjZhsmSC();
					NSYBsGIFvSXoKJvKYNpLIknYctSFb = null;
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

			private void aoVbOlQNesHmZyozkXIZNjZhsmSC()
			{
				pFlvUXFmwIIfeNBGRkjGGhawhghP = -1;
				if (NSYBsGIFvSXoKJvKYNpLIknYctSFb != null)
				{
					NSYBsGIFvSXoKJvKYNpLIknYctSFb.Dispose();
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
				AZUVOpRozrCjJKrDJBtfgiNEUKURA aZUVOpRozrCjJKrDJBtfgiNEUKURA;
				if (pFlvUXFmwIIfeNBGRkjGGhawhghP == -2 && uLmSmiZiTkTEMSXTGNiLTjglLMZC == Environment.CurrentManagedThreadId)
				{
					pFlvUXFmwIIfeNBGRkjGGhawhghP = 0;
					aZUVOpRozrCjJKrDJBtfgiNEUKURA = this;
				}
				else
				{
					aZUVOpRozrCjJKrDJBtfgiNEUKURA = new AZUVOpRozrCjJKrDJBtfgiNEUKURA(0);
					aZUVOpRozrCjJKrDJBtfgiNEUKURA.CrLKRoLjtOpsHQpUKlULtPpjHFvl = CrLKRoLjtOpsHQpUKlULtPpjHFvl;
				}
				aZUVOpRozrCjJKrDJBtfgiNEUKURA.iRHAamHpIgtBhFntaWlAurwokXVIA = sQPOpZxojasjunlpYcRqptzbbHrF;
				aZUVOpRozrCjJKrDJBtfgiNEUKURA.plpGfBhXhKaRwsqMFzQwOtGoPvJtA = ERLcPKKIYkqoaveWnXPgKrwXUkTG;
				return aZUVOpRozrCjJKrDJBtfgiNEUKURA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
			}
		}

		private sealed class UnwTSRMcZaysIQhRSqQCaRguCpQEA : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
		{
			private int UHLrupNRfIpmNVzhpQlCIqgMhPGw;

			private ActionElementMap PakEGXHxYQcJcXOdvAUiaNMEWELUB;

			private int NDahdzvZGlRfTLthjSjYDWOXOejF;

			public ControllerMap WmxyfQRJsQzuojnzmJQcNVFGcdSb;

			private IControllerElementTarget JAAQOaVTLJctpVOaVgdPDxQATTJDA;

			public IControllerElementTarget ajsDidGkXmwJhigjAiliNLWKRplQ;

			private bool sENaEABPKXnxsAGQkTFKSzjaBWdjA;

			public bool hCTbyyBSrKhWGvaiqPZArEenMagTA;

			private TempListPool.TList<ActionElementMap> dkEVPxOPRFfFOYEWKhCEYMRzwNQm;

			private List<ActionElementMap>.Enumerator mKClJkzvdVsrKyUUEIbsLyYkRapF;

			ActionElementMap IEnumerator<ActionElementMap>.Current
			{
				[DebuggerHidden]
				get
				{
					return PakEGXHxYQcJcXOdvAUiaNMEWELUB;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return PakEGXHxYQcJcXOdvAUiaNMEWELUB;
				}
			}

			[DebuggerHidden]
			public UnwTSRMcZaysIQhRSqQCaRguCpQEA(int P_0)
			{
				UHLrupNRfIpmNVzhpQlCIqgMhPGw = P_0;
				NDahdzvZGlRfTLthjSjYDWOXOejF = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int uHLrupNRfIpmNVzhpQlCIqgMhPGw = UHLrupNRfIpmNVzhpQlCIqgMhPGw;
				if ((uint)(uHLrupNRfIpmNVzhpQlCIqgMhPGw - -4) <= 1u || uHLrupNRfIpmNVzhpQlCIqgMhPGw == 1)
				{
					try
					{
						if (uHLrupNRfIpmNVzhpQlCIqgMhPGw == -4 || uHLrupNRfIpmNVzhpQlCIqgMhPGw == 1)
						{
							try
							{
							}
							finally
							{
								rFegkZYIxjIKZxogAKavXGGvxfuK();
							}
						}
					}
					finally
					{
						azyCRCAaEonoEgptwPnOjzHMTgqCA();
					}
				}
				dkEVPxOPRFfFOYEWKhCEYMRzwNQm = null;
				mKClJkzvdVsrKyUUEIbsLyYkRapF = default(List<ActionElementMap>.Enumerator);
				UHLrupNRfIpmNVzhpQlCIqgMhPGw = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int uHLrupNRfIpmNVzhpQlCIqgMhPGw = UHLrupNRfIpmNVzhpQlCIqgMhPGw;
					ControllerMap wmxyfQRJsQzuojnzmJQcNVFGcdSb = WmxyfQRJsQzuojnzmJQcNVFGcdSb;
					switch (uHLrupNRfIpmNVzhpQlCIqgMhPGw)
					{
					default:
						return false;
					case 0:
					{
						UHLrupNRfIpmNVzhpQlCIqgMhPGw = -1;
						if (ReInput._id != wmxyfQRJsQzuojnzmJQcNVFGcdSb.zYMtfthQqWFUiFGChqAKAcaBAqFL)
						{
							ReInput.CheckInitialized(wmxyfQRJsQzuojnzmJQcNVFGcdSb.zYMtfthQqWFUiFGChqAKAcaBAqFL);
							return false;
						}
						dkEVPxOPRFfFOYEWKhCEYMRzwNQm = TempListPool.GetTList<ActionElementMap>();
						UHLrupNRfIpmNVzhpQlCIqgMhPGw = -3;
						List<ActionElementMap> list = dkEVPxOPRFfFOYEWKhCEYMRzwNQm.list;
						wmxyfQRJsQzuojnzmJQcNVFGcdSb.yxkdMnawZniDZXFmujYxcNWEtVSFA(JAAQOaVTLJctpVOaVgdPDxQATTJDA, false, -1, sENaEABPKXnxsAGQkTFKSzjaBWdjA, list, false, out var _);
						mKClJkzvdVsrKyUUEIbsLyYkRapF = list.GetEnumerator();
						UHLrupNRfIpmNVzhpQlCIqgMhPGw = -4;
						break;
					}
					case 1:
						UHLrupNRfIpmNVzhpQlCIqgMhPGw = -4;
						break;
					}
					if (mKClJkzvdVsrKyUUEIbsLyYkRapF.MoveNext())
					{
						ActionElementMap current = mKClJkzvdVsrKyUUEIbsLyYkRapF.Current;
						PakEGXHxYQcJcXOdvAUiaNMEWELUB = current;
						UHLrupNRfIpmNVzhpQlCIqgMhPGw = 1;
						return true;
					}
					rFegkZYIxjIKZxogAKavXGGvxfuK();
					mKClJkzvdVsrKyUUEIbsLyYkRapF = default(List<ActionElementMap>.Enumerator);
					azyCRCAaEonoEgptwPnOjzHMTgqCA();
					dkEVPxOPRFfFOYEWKhCEYMRzwNQm = null;
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

			private void azyCRCAaEonoEgptwPnOjzHMTgqCA()
			{
				UHLrupNRfIpmNVzhpQlCIqgMhPGw = -1;
				if (dkEVPxOPRFfFOYEWKhCEYMRzwNQm != null)
				{
					((IDisposable)dkEVPxOPRFfFOYEWKhCEYMRzwNQm).Dispose();
				}
			}

			private void rFegkZYIxjIKZxogAKavXGGvxfuK()
			{
				UHLrupNRfIpmNVzhpQlCIqgMhPGw = -3;
				((IDisposable)mKClJkzvdVsrKyUUEIbsLyYkRapF/*cast due to .constrained prefix*/).Dispose();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
			{
				UnwTSRMcZaysIQhRSqQCaRguCpQEA unwTSRMcZaysIQhRSqQCaRguCpQEA;
				if (UHLrupNRfIpmNVzhpQlCIqgMhPGw == -2 && NDahdzvZGlRfTLthjSjYDWOXOejF == Environment.CurrentManagedThreadId)
				{
					UHLrupNRfIpmNVzhpQlCIqgMhPGw = 0;
					unwTSRMcZaysIQhRSqQCaRguCpQEA = this;
				}
				else
				{
					unwTSRMcZaysIQhRSqQCaRguCpQEA = new UnwTSRMcZaysIQhRSqQCaRguCpQEA(0);
					unwTSRMcZaysIQhRSqQCaRguCpQEA.WmxyfQRJsQzuojnzmJQcNVFGcdSb = WmxyfQRJsQzuojnzmJQcNVFGcdSb;
				}
				unwTSRMcZaysIQhRSqQCaRguCpQEA.JAAQOaVTLJctpVOaVgdPDxQATTJDA = ajsDidGkXmwJhigjAiliNLWKRplQ;
				unwTSRMcZaysIQhRSqQCaRguCpQEA.sENaEABPKXnxsAGQkTFKSzjaBWdjA = hCTbyyBSrKhWGvaiqPZArEenMagTA;
				return unwTSRMcZaysIQhRSqQCaRguCpQEA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ActionElementMap>)this).GetEnumerator();
			}
		}

		private sealed class sudCxGQfRLbWUfbKOeSDdWnXmrcL : IEnumerable<ActionElementMap>, IEnumerable, IEnumerator<ActionElementMap>, IEnumerator, IDisposable
		{
			private int uisJplVuaYrcqQWqqCxHiBSUoAvE;

			private ActionElementMap rQdHpAlbjoQHlVFzUUhtfqFsByvFA;

			private int ExXExvwqnYmerUISoXXZyPcnMasL;

			public ControllerMap PKTdMLRVScSkyYUCpIwxdbrIEyMEA;

			private IControllerElementTarget NYmXnsjfkOkrgqrYYEGSppgKHBdkA;

			public IControllerElementTarget QKziDKWdbzjJPrvqkRbiJfyMWPtg;

			private int VdftZjTYMXOocuaibQBRzAgUNuML;

			public int HnOzahZSFhRgEONBMbpijjqlFeUJA;

			private bool MxYixCEdYcTUbMHGuuhBjWxzRTLE;

			public bool FTEXMRvOfjEtZBtzyeUdyZAzCnvDb;

			private TempListPool.TList<ActionElementMap> OsMkPqfUcjNlJSAGytWXpfkctSZm;

			private List<ActionElementMap>.Enumerator VcObuiBAoohQYereBpDXfWsOnPAHA;

			ActionElementMap IEnumerator<ActionElementMap>.Current
			{
				[DebuggerHidden]
				get
				{
					return rQdHpAlbjoQHlVFzUUhtfqFsByvFA;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return rQdHpAlbjoQHlVFzUUhtfqFsByvFA;
				}
			}

			[DebuggerHidden]
			public sudCxGQfRLbWUfbKOeSDdWnXmrcL(int P_0)
			{
				uisJplVuaYrcqQWqqCxHiBSUoAvE = P_0;
				ExXExvwqnYmerUISoXXZyPcnMasL = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int num = uisJplVuaYrcqQWqqCxHiBSUoAvE;
				if ((uint)(num - -4) <= 1u || num == 1)
				{
					try
					{
						if (num == -4 || num == 1)
						{
							try
							{
							}
							finally
							{
								sjSiNeCkZoGXcgHfQrCwkIEZnDcVA();
							}
						}
					}
					finally
					{
						hLAHfsZZcHSFeublipMuDVLKCvTU();
					}
				}
				OsMkPqfUcjNlJSAGytWXpfkctSZm = null;
				VcObuiBAoohQYereBpDXfWsOnPAHA = default(List<ActionElementMap>.Enumerator);
				uisJplVuaYrcqQWqqCxHiBSUoAvE = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int num = uisJplVuaYrcqQWqqCxHiBSUoAvE;
					ControllerMap pKTdMLRVScSkyYUCpIwxdbrIEyMEA = PKTdMLRVScSkyYUCpIwxdbrIEyMEA;
					switch (num)
					{
					default:
						return false;
					case 0:
					{
						uisJplVuaYrcqQWqqCxHiBSUoAvE = -1;
						if (ReInput._id != pKTdMLRVScSkyYUCpIwxdbrIEyMEA.zYMtfthQqWFUiFGChqAKAcaBAqFL)
						{
							ReInput.CheckInitialized(pKTdMLRVScSkyYUCpIwxdbrIEyMEA.zYMtfthQqWFUiFGChqAKAcaBAqFL);
							return false;
						}
						OsMkPqfUcjNlJSAGytWXpfkctSZm = TempListPool.GetTList<ActionElementMap>();
						uisJplVuaYrcqQWqqCxHiBSUoAvE = -3;
						List<ActionElementMap> list = OsMkPqfUcjNlJSAGytWXpfkctSZm.list;
						pKTdMLRVScSkyYUCpIwxdbrIEyMEA.yxkdMnawZniDZXFmujYxcNWEtVSFA(NYmXnsjfkOkrgqrYYEGSppgKHBdkA, true, VdftZjTYMXOocuaibQBRzAgUNuML, MxYixCEdYcTUbMHGuuhBjWxzRTLE, list, false, out var _);
						VcObuiBAoohQYereBpDXfWsOnPAHA = list.GetEnumerator();
						uisJplVuaYrcqQWqqCxHiBSUoAvE = -4;
						break;
					}
					case 1:
						uisJplVuaYrcqQWqqCxHiBSUoAvE = -4;
						break;
					}
					if (VcObuiBAoohQYereBpDXfWsOnPAHA.MoveNext())
					{
						ActionElementMap current = VcObuiBAoohQYereBpDXfWsOnPAHA.Current;
						rQdHpAlbjoQHlVFzUUhtfqFsByvFA = current;
						uisJplVuaYrcqQWqqCxHiBSUoAvE = 1;
						return true;
					}
					sjSiNeCkZoGXcgHfQrCwkIEZnDcVA();
					VcObuiBAoohQYereBpDXfWsOnPAHA = default(List<ActionElementMap>.Enumerator);
					hLAHfsZZcHSFeublipMuDVLKCvTU();
					OsMkPqfUcjNlJSAGytWXpfkctSZm = null;
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

			private void hLAHfsZZcHSFeublipMuDVLKCvTU()
			{
				uisJplVuaYrcqQWqqCxHiBSUoAvE = -1;
				if (OsMkPqfUcjNlJSAGytWXpfkctSZm != null)
				{
					((IDisposable)OsMkPqfUcjNlJSAGytWXpfkctSZm).Dispose();
				}
			}

			private void sjSiNeCkZoGXcgHfQrCwkIEZnDcVA()
			{
				uisJplVuaYrcqQWqqCxHiBSUoAvE = -3;
				((IDisposable)VcObuiBAoohQYereBpDXfWsOnPAHA/*cast due to .constrained prefix*/).Dispose();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<ActionElementMap> IEnumerable<ActionElementMap>.GetEnumerator()
			{
				sudCxGQfRLbWUfbKOeSDdWnXmrcL sudCxGQfRLbWUfbKOeSDdWnXmrcL2;
				if (uisJplVuaYrcqQWqqCxHiBSUoAvE == -2 && ExXExvwqnYmerUISoXXZyPcnMasL == Environment.CurrentManagedThreadId)
				{
					uisJplVuaYrcqQWqqCxHiBSUoAvE = 0;
					sudCxGQfRLbWUfbKOeSDdWnXmrcL2 = this;
				}
				else
				{
					sudCxGQfRLbWUfbKOeSDdWnXmrcL2 = new sudCxGQfRLbWUfbKOeSDdWnXmrcL(0);
					sudCxGQfRLbWUfbKOeSDdWnXmrcL2.PKTdMLRVScSkyYUCpIwxdbrIEyMEA = PKTdMLRVScSkyYUCpIwxdbrIEyMEA;
				}
				sudCxGQfRLbWUfbKOeSDdWnXmrcL2.NYmXnsjfkOkrgqrYYEGSppgKHBdkA = QKziDKWdbzjJPrvqkRbiJfyMWPtg;
				sudCxGQfRLbWUfbKOeSDdWnXmrcL2.VdftZjTYMXOocuaibQBRzAgUNuML = HnOzahZSFhRgEONBMbpijjqlFeUJA;
				sudCxGQfRLbWUfbKOeSDdWnXmrcL2.MxYixCEdYcTUbMHGuuhBjWxzRTLE = FTEXMRvOfjEtZBtzyeUdyZAzCnvDb;
				return sudCxGQfRLbWUfbKOeSDdWnXmrcL2;
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

		protected string _name = string.Empty;

		protected Guid _hardwareGuid;

		protected bool _enabled;

		internal readonly int zYMtfthQqWFUiFGChqAKAcaBAqFL;

		private double umIdXBbEcxboeZWinXJoMgkphcbC;

		private readonly AList<ActionElementMap> VRlwPFoStycupprwoRTWAUXYBQXF;

		private readonly ReadOnlyCollection<ActionElementMap> IGjEMyjJsrbtREjTJNUKzlwwcdx;

		private readonly AList<ActionElementMap> ySGqoPAVOxrtlyHbiechAnovEvmd;

		private readonly ReadOnlyCollection<ActionElementMap> xWwcifRuEypWIDdMJNbqeqoiiJPn;

		protected int _playerId = -1;

		protected int _controllerId = -1;

		protected ControllerType _controllerType;

		private static int HdcwGfvsEXNsdagBAZRrztzchBLq;

		private static int IoBmcKKKNFbgvDusUrVIIDEADjEfb;

		private static int QtnCJDrgvfxSSwUiyRjemktqgbGKA
		{
			get
			{
				int hdcwGfvsEXNsdagBAZRrztzchBLq = HdcwGfvsEXNsdagBAZRrztzchBLq;
				if (HdcwGfvsEXNsdagBAZRrztzchBLq == 2147483647)
				{
					HdcwGfvsEXNsdagBAZRrztzchBLq = 0;
					return hdcwGfvsEXNsdagBAZRrztzchBLq;
				}
				HdcwGfvsEXNsdagBAZRrztzchBLq++;
				return hdcwGfvsEXNsdagBAZRrztzchBLq;
			}
		}

		internal static bool mCTnDNAaJjJXoZDRSzjNPwGcXRBm => IoBmcKKKNFbgvDusUrVIIDEADjEfb > 0;

		public int id
		{
			get
			{
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return -1;
				}
				return _id;
			}
		}

		public int sourceMapId
		{
			get
			{
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return null;
				}
				return ReInput.controllers.GetController(_controllerType, _controllerId);
			}
		}

		public ControllerType controllerType
		{
			get
			{
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return ControllerType.Keyboard;
				}
				return _controllerType;
			}
		}

		public Player player
		{
			get
			{
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return null;
				}
				return ReInput.players.GetPlayer(_playerId);
			}
		}

		public int elementMapCount
		{
			get
			{
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return 0;
				}
				return ySGqoPAVOxrtlyHbiechAnovEvmd.Count;
			}
		}

		public int buttonMapCount
		{
			get
			{
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return 0;
				}
				return VRlwPFoStycupprwoRTWAUXYBQXF.Count;
			}
		}

		public IList<ActionElementMap> AllMaps
		{
			get
			{
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
				}
				return xWwcifRuEypWIDdMJNbqeqoiiJPn;
			}
		}

		public IList<ActionElementMap> ElementMaps
		{
			get
			{
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
				}
				return xWwcifRuEypWIDdMJNbqeqoiiJPn;
			}
		}

		public IList<ActionElementMap> ButtonMaps
		{
			get
			{
				if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
				{
					ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
					return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
				}
				return IGjEMyjJsrbtREjTJNUKzlwwcdx;
			}
		}

		public double modifiedTime
		{
			get
			{
				int count = ySGqoPAVOxrtlyHbiechAnovEvmd.Count;
				double num = umIdXBbEcxboeZWinXJoMgkphcbC;
				for (int i = 0; i < count; i++)
				{
					if (ySGqoPAVOxrtlyHbiechAnovEvmd[i] != null && ySGqoPAVOxrtlyHbiechAnovEvmd[i].modifiedTime > num)
					{
						num = ySGqoPAVOxrtlyHbiechAnovEvmd[i].modifiedTime;
					}
				}
				return num;
			}
		}

		public bool isModified
		{
			get
			{
				if (umIdXBbEcxboeZWinXJoMgkphcbC > 0.0)
				{
					return true;
				}
				int count = ySGqoPAVOxrtlyHbiechAnovEvmd.Count;
				for (int i = 0; i < count; i++)
				{
					if (ySGqoPAVOxrtlyHbiechAnovEvmd[i] != null && ySGqoPAVOxrtlyHbiechAnovEvmd[i].isModified)
					{
						return true;
					}
				}
				return false;
			}
			set
			{
				if (value)
				{
					umIdXBbEcxboeZWinXJoMgkphcbC = ReInput.realTime;
					return;
				}
				umIdXBbEcxboeZWinXJoMgkphcbC = 0.0;
				int count = ySGqoPAVOxrtlyHbiechAnovEvmd.Count;
				_ = umIdXBbEcxboeZWinXJoMgkphcbC;
				for (int i = 0; i < count; i++)
				{
					if (ySGqoPAVOxrtlyHbiechAnovEvmd[i] != null)
					{
						ySGqoPAVOxrtlyHbiechAnovEvmd[i].isModified = value;
					}
				}
			}
		}

		internal AList<ActionElementMap> LrIuPDSCedgUWTQghItEJWRAaExrA => VRlwPFoStycupprwoRTWAUXYBQXF;

		public ControllerMap()
		{
			_id = QtnCJDrgvfxSSwUiyRjemktqgbGKA;
			_sourceMapId = -1;
			VRlwPFoStycupprwoRTWAUXYBQXF = new AList<ActionElementMap>();
			IGjEMyjJsrbtREjTJNUKzlwwcdx = new ReadOnlyCollection<ActionElementMap>(VRlwPFoStycupprwoRTWAUXYBQXF);
			ySGqoPAVOxrtlyHbiechAnovEvmd = new AList<ActionElementMap>();
			xWwcifRuEypWIDdMJNbqeqoiiJPn = new ReadOnlyCollection<ActionElementMap>(ySGqoPAVOxrtlyHbiechAnovEvmd);
			zYMtfthQqWFUiFGChqAKAcaBAqFL = ReInput.id;
		}

		public ControllerMap(ControllerMap P_0)
			: this()
		{
			_id = QtnCJDrgvfxSSwUiyRjemktqgbGKA;
			_sourceMapId = P_0._sourceMapId;
			_categoryId = P_0._categoryId;
			_layoutId = P_0._layoutId;
			_name = P_0._name;
			_hardwareGuid = P_0._hardwareGuid;
			_enabled = P_0._enabled;
			_playerId = P_0._playerId;
			_controllerId = P_0._controllerId;
			_controllerType = P_0._controllerType;
			RAmMePHwhbbjmrfLAYKtBaJPbccQ();
			if (P_0.VRlwPFoStycupprwoRTWAUXYBQXF != null)
			{
				int count = P_0.VRlwPFoStycupprwoRTWAUXYBQXF.Count;
				for (int i = 0; i < count; i++)
				{
					PezDvOGcKjNEkBMHZWqCEhajEEXPB(new ActionElementMap(P_0.VRlwPFoStycupprwoRTWAUXYBQXF[i]));
				}
			}
			oeOZZgeXJicFbaxfdmvQlNMqgCjfA();
			umIdXBbEcxboeZWinXJoMgkphcbC = P_0.umIdXBbEcxboeZWinXJoMgkphcbC;
		}

		public bool ContainsAction(string actionName)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			InputAction inputAction = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.NsixmaaSoJxmhDlFhVMoiKUGoKgn(actionName, true);
			if (inputAction == null)
			{
				return false;
			}
			return ContainsAction(inputAction.id);
		}

		public virtual bool ContainsAction(int actionId)
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
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (VRlwPFoStycupprwoRTWAUXYBQXF[i]._actionId == actionId)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsElementIdentifier(int elementIdentifierId)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			AList<ActionElementMap> aList = ySGqoPAVOxrtlyHbiechAnovEvmd;
			for (int i = 0; i < aList.Count; i++)
			{
				if (ySGqoPAVOxrtlyHbiechAnovEvmd[i].elementIdentifierId == elementIdentifierId)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsKeyboardKey(KeyCode keyCode, ModifierKeyFlags modifierKeys)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			AList<ActionElementMap> aList = ySGqoPAVOxrtlyHbiechAnovEvmd;
			for (int i = 0; i < aList.Count; i++)
			{
				if (ySGqoPAVOxrtlyHbiechAnovEvmd[i].keyCode == keyCode && ySGqoPAVOxrtlyHbiechAnovEvmd[i].modifierKeyFlags == modifierKeys)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsElementMap(ActionElementMap elementMap)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			if (elementMap == null)
			{
				return false;
			}
			AList<ActionElementMap> aList = ySGqoPAVOxrtlyHbiechAnovEvmd;
			for (int i = 0; i < aList.Count; i++)
			{
				if (ySGqoPAVOxrtlyHbiechAnovEvmd[i].nJilCjIhFvMUTsTBcUWuYpormNsu == elementMap.id)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsElementMap(int elementMapId)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			AList<ActionElementMap> aList = ySGqoPAVOxrtlyHbiechAnovEvmd;
			for (int i = 0; i < aList.Count; i++)
			{
				if (ySGqoPAVOxrtlyHbiechAnovEvmd[i].nJilCjIhFvMUTsTBcUWuYpormNsu == elementMapId)
				{
					return true;
				}
			}
			return false;
		}

		public bool ReplaceOrCreateElementMap(ElementAssignment elementAssignment)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			ActionElementMap result;
			return ReplaceOrCreateElementMap(elementAssignment, out result);
		}

		public bool ReplaceOrCreateElementMap(ElementAssignment elementAssignment, out ActionElementMap result)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			ActionElementMap result;
			return CreateElementMap(elementAssignment, out result);
		}

		public bool CreateElementMap(ElementAssignment elementAssignment, out ActionElementMap result)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				result = null;
				return false;
			}
			if (_controllerType == ControllerType.Keyboard)
			{
				return CreateElementMap(elementAssignment.actionId, elementAssignment.axisContribution, elementAssignment.keyboardKey, elementAssignment.modifierKeyFlags, out result);
			}
			if (_controllerType == ControllerType.Joystick || _controllerType == ControllerType.Mouse || _controllerType == ControllerType.Custom)
			{
				return CreateElementMap(elementAssignment.actionId, elementAssignment.axisContribution, elementAssignment.elementIdentifierId, cVDyIiOsEfJNYzVuZSmuEXqylgT.qKYDwIHBJWlecbzxaQOcnVEuKZDVb(elementAssignment.type), elementAssignment.axisRange, elementAssignment.invert, out result);
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				result = null;
				return false;
			}
			ActionElementMap actionElementMap = new ActionElementMap(actionId, ControllerElementType.Button, axisContribution, (KeyboardKeyCode)keyCode, modifierKey1, modifierKey2, modifierKey3);
			ReInput.controllers.Keyboard.VnQORoKBKYcDfQniJOyRPalZgtMZ(this, actionElementMap);
			PezDvOGcKjNEkBMHZWqCEhajEEXPB(actionElementMap);
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				result = null;
				return false;
			}
			AgvfXhluxZViVrRcEjHJVwIOpUnB agvfXhluxZViVrRcEjHJVwIOpUnB = AgvfXhluxZViVrRcEjHJVwIOpUnB.BDmZsItjZWREiqJsCZQghFuSRSdY(modifierKeyFlags);
			return CreateElementMap(actionId, axisContribution, keyCode, agvfXhluxZViVrRcEjHJVwIOpUnB.ZIgWhlWHlCupODaLTBErrOtIhMJq, agvfXhluxZViVrRcEjHJVwIOpUnB.sXZLWCudfSiLvhftzVVhdByYMHYB, agvfXhluxZViVrRcEjHJVwIOpUnB.LVOXLQdqhieMhYyKMfbprcWmfUyv, out result);
		}

		public bool CreateElementMap(int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert)
		{
			ActionElementMap result;
			return CreateElementMap(actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert, out result);
		}

		public virtual bool CreateElementMap(int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert, out ActionElementMap result)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				result = null;
				return false;
			}
			if (!YvTmZmawDMlBXCkDXqRSiLnRLllk(elementType))
			{
				result = null;
				return false;
			}
			ActionElementMap actionElementMap = new ActionElementMap(actionId, elementType, elementIdentifierId, axisContribution, axisRange);
			BakeElementMap(actionElementMap);
			PezDvOGcKjNEkBMHZWqCEhajEEXPB(actionElementMap);
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				result = null;
				return false;
			}
			if (_controllerType == ControllerType.Keyboard)
			{
				return ReplaceElementMap(elementAssignment.elementMapId, elementAssignment.actionId, elementAssignment.axisContribution, elementAssignment.keyboardKey, elementAssignment.modifierKeyFlags, out result);
			}
			if (_controllerType == ControllerType.Joystick || _controllerType == ControllerType.Mouse || _controllerType == ControllerType.Custom)
			{
				return ReplaceElementMap(elementAssignment.elementMapId, elementAssignment.actionId, elementAssignment.axisContribution, elementAssignment.elementIdentifierId, cVDyIiOsEfJNYzVuZSmuEXqylgT.qKYDwIHBJWlecbzxaQOcnVEuKZDVb(elementAssignment.type), elementAssignment.axisRange, elementAssignment.invert, out result);
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				result = null;
				return false;
			}
			ActionElementMap elementMap = GetElementMap(elementMapId);
			if (elementMap == null)
			{
				result = null;
				return false;
			}
			if (SqXPkqocZgFeqebhmtpqdTfOwlIM(elementMapId) < 0)
			{
				DeleteElementMap(elementMapId);
				elementMap.elementType = ControllerElementType.Button;
				PezDvOGcKjNEkBMHZWqCEhajEEXPB(elementMap);
			}
			if (SqXPkqocZgFeqebhmtpqdTfOwlIM(elementMapId) < 0)
			{
				result = null;
				return false;
			}
			elementMap.poaYXnSDRTkbjnijNbPqDYgSEAiS();
			elementMap._actionId = actionId;
			elementMap._elementType = ControllerElementType.Button;
			elementMap._axisContribution = axisContribution;
			elementMap._keyboardKeyCode = (KeyboardKeyCode)keyCode;
			elementMap._modifierKey1 = modifierKey1;
			elementMap._modifierKey2 = modifierKey2;
			elementMap._modifierKey3 = modifierKey3;
			elementMap.mcFJwsoNSHHvPNLkGxwFTlHQondm();
			ReInput.controllers.Keyboard.VnQORoKBKYcDfQniJOyRPalZgtMZ(this, elementMap);
			result = elementMap;
			GKClWfkOaAcWgcWSeeqXJvARlRJB();
			return true;
		}

		public bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, KeyCode keyCode, ModifierKeyFlags modifierKeyFlags)
		{
			ActionElementMap result;
			return ReplaceElementMap(elementMapId, actionId, axisContribution, keyCode, modifierKeyFlags, out result);
		}

		public bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, KeyCode keyCode, ModifierKeyFlags modifierKeyFlags, out ActionElementMap result)
		{
			AgvfXhluxZViVrRcEjHJVwIOpUnB agvfXhluxZViVrRcEjHJVwIOpUnB = AgvfXhluxZViVrRcEjHJVwIOpUnB.BDmZsItjZWREiqJsCZQghFuSRSdY(modifierKeyFlags);
			return ReplaceElementMap(elementMapId, actionId, axisContribution, keyCode, agvfXhluxZViVrRcEjHJVwIOpUnB.ZIgWhlWHlCupODaLTBErrOtIhMJq, agvfXhluxZViVrRcEjHJVwIOpUnB.sXZLWCudfSiLvhftzVVhdByYMHYB, agvfXhluxZViVrRcEjHJVwIOpUnB.LVOXLQdqhieMhYyKMfbprcWmfUyv, out result);
		}

		public bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert)
		{
			ActionElementMap result;
			return ReplaceElementMap(elementMapId, actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert, out result);
		}

		public virtual bool ReplaceElementMap(int elementMapId, int actionId, Pole axisContribution, int elementIdentifierId, ControllerElementType elementType, AxisRange axisRange, bool invert, out ActionElementMap result)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				result = null;
				return false;
			}
			if (!YvTmZmawDMlBXCkDXqRSiLnRLllk(elementType))
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
			if (!YvTmZmawDMlBXCkDXqRSiLnRLllk(elementMap._elementType))
			{
				DeleteElementMap(elementMapId);
				elementMap.elementType = ControllerElementType.Button;
				PezDvOGcKjNEkBMHZWqCEhajEEXPB(elementMap);
			}
			if (SqXPkqocZgFeqebhmtpqdTfOwlIM(elementMapId) < 0)
			{
				result = null;
				return false;
			}
			LxwILqboYSTsOrojOayZHEnpMZuBb(elementMap, actionId, axisContribution, elementIdentifierId, elementType, axisRange, invert);
			BakeElementMap(elementMap);
			result = elementMap;
			GKClWfkOaAcWgcWSeeqXJvARlRJB();
			return true;
		}

		public virtual bool DeleteElementMap(int elementMapId)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			int num = SqXPkqocZgFeqebhmtpqdTfOwlIM(elementMapId);
			if (num < 0)
			{
				return false;
			}
			RKPAwAHEdOlsLfKXlHroBMcxRwNtA(elementMapId, num);
			return true;
		}

		public virtual bool DeleteElementMapsWithAction(string actionName)
		{
			return DeleteElementMapsWithAction(ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName));
		}

		public virtual bool DeleteElementMapsWithAction(int actionId)
		{
			return DeleteButtonMapsWithAction(actionId);
		}

		public virtual ActionElementMap GetElementMap(int elementMapId)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			if (elementMapId < 0)
			{
				return null;
			}
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (VRlwPFoStycupprwoRTWAUXYBQXF[i].nJilCjIhFvMUTsTBcUWuYpormNsu == elementMapId)
				{
					return VRlwPFoStycupprwoRTWAUXYBQXF[i];
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				if (!skipDisabledMaps || allMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			results.Clear();
			return knRVlfEXYlRaoRSAktAGsblDVoJP(results, skipDisabledMaps);
		}

		public ActionElementMap[] GetElementMapsWithAction(string actionName)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.array;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetElementMapsWithAction(actionId);
		}

		public ActionElementMap[] GetElementMapsWithAction(int actionId)
		{
			return GetElementMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public ActionElementMap[] GetElementMapsWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.array;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetElementMapsWithAction(actionId, skipDisabledMaps);
		}

		public ActionElementMap[] GetElementMapsWithAction(int actionId, bool skipDisabledMaps)
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
			if (elementMapCount == 0)
			{
				return EmptyObjects<ActionElementMap>.array;
			}
			int num = 0;
			foreach (ActionElementMap allMap in AllMaps)
			{
				if (allMap._actionId == actionId && (!skipDisabledMaps || allMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
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
				if (allMap2._actionId == actionId && (!skipDisabledMaps || allMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					array[num2] = allMap2;
					num2++;
				}
			}
			return array;
		}

		public int GetElementMapsWithAction(string actionName, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetElementMapsWithAction(actionId, results);
		}

		public int GetElementMapsWithAction(int actionId, List<ActionElementMap> results)
		{
			return GetElementMapsWithAction(actionId, skipDisabledMaps: false, results);
		}

		public int GetElementMapsWithAction(string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetElementMapsWithAction(actionId, skipDisabledMaps, results);
		}

		public int GetElementMapsWithAction(int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			return StwzmqpEamBsxgECgGOOyjBfNurP(actionId, skipDisabledMaps, results, false);
		}

		public IEnumerable<ActionElementMap> ElementMapsWithAction(string actionName)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return ElementMapsWithAction(actionId);
		}

		public IEnumerable<ActionElementMap> ElementMapsWithAction(int actionId)
		{
			return ElementMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public IEnumerable<ActionElementMap> ElementMapsWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return ElementMapsWithAction(actionId, skipDisabledMaps);
		}

		[IteratorStateMachine(typeof(AZUVOpRozrCjJKrDJBtfgiNEUKURA))]
		public IEnumerable<ActionElementMap> ElementMapsWithAction(int actionId, bool skipDisabledMaps)
		{
			return new AZUVOpRozrCjJKrDJBtfgiNEUKURA(-2)
			{
				CrLKRoLjtOpsHQpUKlULtPpjHFvl = this,
				sQPOpZxojasjunlpYcRqptzbbHrF = actionId,
				ERLcPKKIYkqoaveWnXPgKrwXUkTG = skipDisabledMaps
			};
		}

		public virtual ActionElementMap GetFirstElementMapWithAction(int actionId)
		{
			return GetFirstElementMapWithAction(actionId, skipDisabledMaps: false);
		}

		public virtual ActionElementMap GetFirstElementMapWithAction(string actionName)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetFirstElementMapWithAction(actionId);
		}

		public virtual ActionElementMap GetFirstElementMapWithAction(int actionId, bool skipDisabledMaps)
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
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (VRlwPFoStycupprwoRTWAUXYBQXF[i]._actionId == actionId && (!skipDisabledMaps || VRlwPFoStycupprwoRTWAUXYBQXF[i].uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					return VRlwPFoStycupprwoRTWAUXYBQXF[i];
				}
			}
			return null;
		}

		public ActionElementMap GetFirstElementMapWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetFirstElementMapWithAction(actionId, skipDisabledMaps);
		}

		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(ControllerElementTarget elementTarget, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
			IEnumerable<ActionElementMap> result = ElementMapsWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, skipDisabledMaps);
			SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
			return result;
		}

		[IteratorStateMachine(typeof(UnwTSRMcZaysIQhRSqQCaRguCpQEA))]
		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(IControllerElementTarget elementTarget, bool skipDisabledMaps)
		{
			return new UnwTSRMcZaysIQhRSqQCaRguCpQEA(-2)
			{
				WmxyfQRJsQzuojnzmJQcNVFGcdSb = this,
				ajsDidGkXmwJhigjAiliNLWKRplQ = elementTarget,
				hCTbyyBSrKhWGvaiqPZArEenMagTA = skipDisabledMaps
			};
		}

		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(ControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
			IEnumerable<ActionElementMap> result = ElementMapsWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, actionId, skipDisabledMaps);
			SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
			return result;
		}

		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(ControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return ElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps);
		}

		[IteratorStateMachine(typeof(sudCxGQfRLbWUfbKOeSDdWnXmrcL))]
		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(IControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
		{
			return new sudCxGQfRLbWUfbKOeSDdWnXmrcL(-2)
			{
				PKTdMLRVScSkyYUCpIwxdbrIEyMEA = this,
				QKziDKWdbzjJPrvqkRbiJfyMWPtg = elementTarget,
				HnOzahZSFhRgEONBMbpijjqlFeUJA = actionId,
				FTEXMRvOfjEtZBtzyeUdyZAzCnvDb = skipDisabledMaps
			};
		}

		public IEnumerable<ActionElementMap> ElementMapsWithElementTarget(IControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return ElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps);
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(ControllerElementTarget elementTarget, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
			ActionElementMap firstElementMapWithElementTarget = GetFirstElementMapWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, skipDisabledMaps);
			SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
			return firstElementMapWithElementTarget;
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(IControllerElementTarget elementTarget, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			bool flag;
			return BtSVfeTrzQORMCWyDKlguEcdjZpHA(elementTarget, false, -1, skipDisabledMaps, out flag);
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(ControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
			ActionElementMap firstElementMapWithElementTarget = GetFirstElementMapWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, actionId, skipDisabledMaps);
			SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
			return firstElementMapWithElementTarget;
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(ControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetFirstElementMapWithElementTarget(elementTarget, actionId, skipDisabledMaps);
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(IControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			bool flag;
			return BtSVfeTrzQORMCWyDKlguEcdjZpHA(elementTarget, true, actionId, skipDisabledMaps, out flag);
		}

		public ActionElementMap GetFirstElementMapWithElementTarget(IControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetFirstElementMapWithElementTarget(elementTarget, actionId, skipDisabledMaps);
		}

		public int GetElementMapsWithElementTarget(ControllerElementTarget elementTarget, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
			int elementMapsWithElementTarget = GetElementMapsWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, skipDisabledMaps, results);
			SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
			return elementMapsWithElementTarget;
		}

		public int GetElementMapsWithElementTarget(IControllerElementTarget elementTarget, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			bool flag;
			return yxkdMnawZniDZXFmujYxcNWEtVSFA(elementTarget, false, -1, skipDisabledMaps, results, false, out flag);
		}

		public int GetElementMapsWithElementTarget(ControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			SzcVmbDpoJahYmnXXukLaOXfCanz szcVmbDpoJahYmnXXukLaOXfCanz = SzcVmbDpoJahYmnXXukLaOXfCanz.iUalUWqSTahvFebVilfnXrVIAQbf(elementTarget);
			int elementMapsWithElementTarget = GetElementMapsWithElementTarget(szcVmbDpoJahYmnXXukLaOXfCanz, actionId, skipDisabledMaps, results);
			SzcVmbDpoJahYmnXXukLaOXfCanz.jzhYiOZYDeArdmkyDczZrxvFgLDbA(szcVmbDpoJahYmnXXukLaOXfCanz);
			return elementMapsWithElementTarget;
		}

		public int GetElementMapsWithElementTarget(ControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps, results);
		}

		public int GetElementMapsWithElementTarget(IControllerElementTarget elementTarget, int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			bool flag;
			return yxkdMnawZniDZXFmujYxcNWEtVSFA(elementTarget, true, actionId, skipDisabledMaps, results, false, out flag);
		}

		public int GetElementMapsWithElementTarget(IControllerElementTarget elementTarget, string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetElementMapsWithElementTarget(elementTarget, actionId, skipDisabledMaps, results);
		}

		public ActionElementMap GetFirstElementMapMatch(Predicate<ActionElementMap> predicate)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			return rhhqnkCVXqczPtVGWeHPIXVpAJyO(predicate, false);
		}

		internal virtual ActionElementMap rhhqnkCVXqczPtVGWeHPIXVpAJyO(Predicate<ActionElementMap> P_0, bool P_1)
		{
			return BMDSNbcMvuAYYrhYbuEatxzZYWgj(P_0, P_1);
		}

		public int GetElementMapMatches(Predicate<ActionElementMap> predicate, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return zRAaXHZhgQekRkSOpZXGmIyScrpxA(predicate, false, results, false);
		}

		internal virtual int zRAaXHZhgQekRkSOpZXGmIyScrpxA(Predicate<ActionElementMap> P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			return GuSVOwnPfcRygWPoumhSOPxdvbKW(P_0, P_1, P_2, P_3);
		}

		public void ForEachElementMapMatch(Predicate<ActionElementMap> predicate, Action<ActionElementMap> actionToPerform)
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
			int count = ySGqoPAVOxrtlyHbiechAnovEvmd.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					ActionElementMap obj = ySGqoPAVOxrtlyHbiechAnovEvmd[i];
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return;
			}
			VRlwPFoStycupprwoRTWAUXYBQXF.Clear();
			ySGqoPAVOxrtlyHbiechAnovEvmd.Clear();
			GKClWfkOaAcWgcWSeeqXJvARlRJB();
		}

		public int SetAllElementMapsEnabled(bool state)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			int num = 0;
			int count = ySGqoPAVOxrtlyHbiechAnovEvmd.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = ySGqoPAVOxrtlyHbiechAnovEvmd[i];
				if (actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA != state)
				{
					actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA = state;
					num++;
				}
			}
			return num;
		}

		public ActionElementMap GetButtonMap(int index)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			if (VRlwPFoStycupprwoRTWAUXYBQXF == null || index < 0 || index >= VRlwPFoStycupprwoRTWAUXYBQXF.Count)
			{
				return null;
			}
			return VRlwPFoStycupprwoRTWAUXYBQXF[index];
		}

		public ActionElementMap[] GetButtonMaps()
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.array;
			}
			return ListTools.ToArray(VRlwPFoStycupprwoRTWAUXYBQXF);
		}

		public ActionElementMap[] GetButtonMaps(bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.array;
			}
			int count = VRlwPFoStycupprwoRTWAUXYBQXF.Count;
			List<ActionElementMap> list = new List<ActionElementMap>(count);
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[i];
				if (!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					list.Add(actionElementMap);
				}
			}
			return list.ToArray();
		}

		public int GetButtonMaps(bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return wKqSQgsLhkpIZlQGixUKHDIBCvXU(skipDisabledMaps, results, false);
		}

		public ActionElementMap[] GetButtonMapsWithAction(string actionName)
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
			return GetButtonMapsWithAction(inputAction.id);
		}

		public ActionElementMap[] GetButtonMapsWithAction(int actionId)
		{
			return GetButtonMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public ActionElementMap[] GetButtonMapsWithAction(string actionName, bool skipDisabledMaps)
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
			return GetButtonMapsWithAction(inputAction.id, skipDisabledMaps);
		}

		public ActionElementMap[] GetButtonMapsWithAction(int actionId, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[i];
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
				ActionElementMap actionElementMap2 = VRlwPFoStycupprwoRTWAUXYBQXF[j];
				if (actionElementMap2._actionId == actionId && (!skipDisabledMaps || actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					array[num3] = actionElementMap2;
					num3++;
				}
			}
			return array;
		}

		public int GetButtonMapsWithAction(string actionName, List<ActionElementMap> results)
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
			return GetButtonMapsWithAction(inputAction.id, results);
		}

		public int GetButtonMapsWithAction(int actionId, List<ActionElementMap> results)
		{
			return GetButtonMapsWithAction(actionId, skipDisabledMaps: false, results);
		}

		public int GetButtonMapsWithAction(string actionName, bool skipDisabledMaps, List<ActionElementMap> results)
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
			return GetButtonMapsWithAction(inputAction.id, skipDisabledMaps, results);
		}

		public int GetButtonMapsWithAction(int actionId, bool skipDisabledMaps, List<ActionElementMap> results)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return iQchYbBsHSgyRzunLuzvVgWwAaeh(actionId, skipDisabledMaps, results, false);
		}

		public IEnumerable<ActionElementMap> ButtonMapsWithAction(int actionId)
		{
			return ButtonMapsWithAction(actionId, skipDisabledMaps: false);
		}

		public IEnumerable<ActionElementMap> ButtonMapsWithAction(string actionName)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return ButtonMapsWithAction(actionId);
		}

		[IteratorStateMachine(typeof(ICLKocmWNtDNVLWZmeBmjIrNwBBC))]
		public IEnumerable<ActionElementMap> ButtonMapsWithAction(int actionId, bool skipDisabledMaps)
		{
			return new ICLKocmWNtDNVLWZmeBmjIrNwBBC(-2)
			{
				TzLsiJLhMQBiAVvRVRBqLdqEVeIT = this,
				tyvoGGHkdsaYpGUnmunQVcpgGMDzA = actionId,
				npWAenJALalYNeTGIUsFWpVjfEMSA = skipDisabledMaps
			};
		}

		public IEnumerable<ActionElementMap> ButtonMapsWithAction(string actionName, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return EmptyObjects<ActionElementMap>.EmptyReadOnlyIListT;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return ButtonMapsWithAction(actionId, skipDisabledMaps);
		}

		public ActionElementMap GetFirstButtonMapWithAction(int actionId)
		{
			return GetFirstButtonMapWithAction(actionId, skipDisabledMaps: false);
		}

		public ActionElementMap GetFirstButtonMapWithAction(string actionName)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetFirstButtonMapWithAction(actionId);
		}

		public ActionElementMap GetFirstButtonMapWithAction(int actionId, bool skipDisabledMaps)
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			int actionId = ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName);
			return GetFirstButtonMapWithAction(actionId, skipDisabledMaps);
		}

		public ActionElementMap GetFirstButtonMapMatch(Predicate<ActionElementMap> predicate)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			return BMDSNbcMvuAYYrhYbuEatxzZYWgj(predicate, false);
		}

		internal ActionElementMap BMDSNbcMvuAYYrhYbuEatxzZYWgj(Predicate<ActionElementMap> P_0, bool P_1)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return GuSVOwnPfcRygWPoumhSOPxdvbKW(predicate, false, results, false);
		}

		internal int GuSVOwnPfcRygWPoumhSOPxdvbKW(Predicate<ActionElementMap> P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
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
			int count = VRlwPFoStycupprwoRTWAUXYBQXF.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					ActionElementMap obj = VRlwPFoStycupprwoRTWAUXYBQXF[i];
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
			return DeleteButtonMapsWithAction(ReInput.stWBKWMOrAnxQyItwkKzVuulIRgF.gtrjEglTtLNqnGEQlKvLXcvkcVLn(actionName));
		}

		public bool DeleteButtonMapsWithAction(int actionId)
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
			int num = buttonMapCount;
			if (num == 0)
			{
				return false;
			}
			bool result = false;
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[num2];
				if (actionElementMap != null && actionElementMap._actionId == actionId)
				{
					RKPAwAHEdOlsLfKXlHroBMcxRwNtA(actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu, num2);
					result = true;
				}
			}
			return result;
		}

		public int SetAllButtonMapsEnabled(bool state)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			int num = 0;
			int count = VRlwPFoStycupprwoRTWAUXYBQXF.Count;
			for (int i = 0; i < count; i++)
			{
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[i];
				if (actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA != state)
				{
					actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA = state;
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
			if (VRlwPFoStycupprwoRTWAUXYBQXF == null)
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
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[i];
				if (skipDisabledMaps && !actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					continue;
				}
				for (int j = 0; j < count; j++)
				{
					ActionElementMap actionElementMap2 = buttonMaps[j];
					if ((!skipDisabledMaps || actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap != actionElementMap2 && actionElementMap.CheckForAssignmentConflict(actionElementMap2))
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual bool DoesElementAssignmentConflict(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			if (actionElementMap == null || VRlwPFoStycupprwoRTWAUXYBQXF == null)
			{
				return false;
			}
			if (skipDisabledMaps && (!_enabled || !actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
			{
				return false;
			}
			for (int i = 0; i < VRlwPFoStycupprwoRTWAUXYBQXF.Count; i++)
			{
				ActionElementMap actionElementMap2 = VRlwPFoStycupprwoRTWAUXYBQXF[i];
				if ((!skipDisabledMaps || actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap2 != actionElementMap && actionElementMap2.CheckForAssignmentConflict(actionElementMap))
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool DoesElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return false;
			}
			if (VRlwPFoStycupprwoRTWAUXYBQXF == null)
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
			for (int i = 0; i < VRlwPFoStycupprwoRTWAUXYBQXF.Count; i++)
			{
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[i];
				if ((!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu != conflictCheck.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
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

		[IteratorStateMachine(typeof(LwQLeasmNXehdQGxsAwelDDAClpA))]
		public virtual IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ControllerMap controllerMap, bool skipDisabledMaps)
		{
			return new LwQLeasmNXehdQGxsAwelDDAClpA(-2)
			{
				GtWYzRSSMqhLbTJbiqFmheYBBZOJ = this,
				qKjdJfmoQhHTtIPLasZLPDVTWUHi = controllerMap,
				AkoOwTVolifnznOKQWdZivRUBJgM = skipDisabledMaps
			};
		}

		[IteratorStateMachine(typeof(OKcoFyVDZgavbnogiSLgraSFdXwp))]
		public virtual IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			return new OKcoFyVDZgavbnogiSLgraSFdXwp(-2)
			{
				nAllwnSvcnoFhmIlFzDvWNEnApUX = this,
				WPguKVwYihoGFCWKfFShufTxgcEDA = actionElementMap,
				nhVybgNaShDajCdvsMQbCZACAnzcA = skipDisabledMaps
			};
		}

		[IteratorStateMachine(typeof(eDIhlLTWSMxqHOHhtoRHPEOOPuqL))]
		public virtual IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			return new eDIhlLTWSMxqHOHhtoRHPEOOPuqL(-2)
			{
				pPVVqsnhRqyUDaNNziPsgqWigGUN = this,
				fChgKHJgfKvftwJvhlUxPPghmVUaA = conflictCheck,
				abZtdrSQNsnemRbkBhQQUgiqWoMm = skipDisabledMaps
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
			if (VRlwPFoStycupprwoRTWAUXYBQXF == null)
			{
				return num;
			}
			IList<ActionElementMap> vRlwPFoStycupprwoRTWAUXYBQXF = controllerMap.VRlwPFoStycupprwoRTWAUXYBQXF;
			if (vRlwPFoStycupprwoRTWAUXYBQXF == null)
			{
				return num;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory != null && !mapCategory.userAssignable)
			{
				return num;
			}
			_ = buttonMapCount;
			int count = vRlwPFoStycupprwoRTWAUXYBQXF.Count;
			for (int num2 = VRlwPFoStycupprwoRTWAUXYBQXF.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[num2];
				if (!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					for (int i = 0; i < count; i++)
					{
						if ((!skipDisabledMaps || vRlwPFoStycupprwoRTWAUXYBQXF[i].uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.CheckForAssignmentConflict(vRlwPFoStycupprwoRTWAUXYBQXF[i]))
						{
							RKPAwAHEdOlsLfKXlHroBMcxRwNtA(actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu, num2);
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			if (actionElementMap == null)
			{
				return 0;
			}
			if (skipDisabledMaps && (!_enabled || !actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
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
			if (VRlwPFoStycupprwoRTWAUXYBQXF == null)
			{
				return num;
			}
			for (int num2 = VRlwPFoStycupprwoRTWAUXYBQXF.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap2 = VRlwPFoStycupprwoRTWAUXYBQXF[num2];
				if ((!skipDisabledMaps || actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap2.CheckForAssignmentConflict(actionElementMap))
				{
					RKPAwAHEdOlsLfKXlHroBMcxRwNtA(actionElementMap2.nJilCjIhFvMUTsTBcUWuYpormNsu, num2);
					num++;
				}
			}
			return num;
		}

		public virtual int RemoveElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			if (skipDisabledMaps && !_enabled)
			{
				return 0;
			}
			if (VRlwPFoStycupprwoRTWAUXYBQXF == null)
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
			for (int num2 = VRlwPFoStycupprwoRTWAUXYBQXF.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[num2];
				if ((!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu != conflictCheck.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
				{
					RKPAwAHEdOlsLfKXlHroBMcxRwNtA(actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu, num2);
					num++;
				}
			}
			return num;
		}

		public int DisableElementAssignmentConflicts(ControllerMap controllerMap)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return uDLYQOQEpEkgIfOMsAsuIdaYXpcq(controllerMap, false, null, false);
		}

		public int DisableElementAssignmentConflicts(ActionElementMap actionElementMap)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return cTyUpZEcqTXlnXWCJGCCWDVCdMkt(actionElementMap, false, null, false);
		}

		public int DisableElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return UlMygnKDTVqoNMDLtBFpFSHRKVIw(conflictCheck, false, null, false);
		}

		public int DisableElementAssignmentConflicts(ControllerMap controllerMap, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return uDLYQOQEpEkgIfOMsAsuIdaYXpcq(controllerMap, skipDisabledMaps, null, false);
		}

		public int DisableElementAssignmentConflicts(ActionElementMap actionElementMap, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return cTyUpZEcqTXlnXWCJGCCWDVCdMkt(actionElementMap, skipDisabledMaps, null, false);
		}

		public int DisableElementAssignmentConflicts(ElementAssignmentConflictCheck conflictCheck, bool skipDisabledMaps)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return 0;
			}
			return UlMygnKDTVqoNMDLtBFpFSHRKVIw(conflictCheck, skipDisabledMaps, null, false);
		}

		internal virtual int uDLYQOQEpEkgIfOMsAsuIdaYXpcq(ControllerMap P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
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
			if (VRlwPFoStycupprwoRTWAUXYBQXF == null)
			{
				return num;
			}
			IList<ActionElementMap> vRlwPFoStycupprwoRTWAUXYBQXF = P_0.VRlwPFoStycupprwoRTWAUXYBQXF;
			if (vRlwPFoStycupprwoRTWAUXYBQXF == null)
			{
				return num;
			}
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(_categoryId);
			if (mapCategory != null && !mapCategory.userAssignable)
			{
				return num;
			}
			int num2 = buttonMapCount;
			int count = vRlwPFoStycupprwoRTWAUXYBQXF.Count;
			for (int i = 0; i < num2; i++)
			{
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[i];
				if (!actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					continue;
				}
				for (int j = 0; j < count; j++)
				{
					ActionElementMap actionElementMap2 = vRlwPFoStycupprwoRTWAUXYBQXF[j];
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

		internal virtual int cTyUpZEcqTXlnXWCJGCCWDVCdMkt(ActionElementMap P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			if (P_2 != null && !P_3)
			{
				P_2.Clear();
			}
			if (P_0 == null)
			{
				return 0;
			}
			if (P_1 && (!_enabled || !P_0.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
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
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[i];
				if (actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA && P_0.CheckForAssignmentConflict(actionElementMap))
				{
					actionElementMap.enabled = false;
					P_2?.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		internal virtual int UlMygnKDTVqoNMDLtBFpFSHRKVIw(ElementAssignmentConflictCheck P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
		{
			if (P_2 != null && !P_3)
			{
				P_2.Clear();
			}
			if (P_1 && !_enabled)
			{
				return 0;
			}
			if (VRlwPFoStycupprwoRTWAUXYBQXF == null)
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
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[i];
				if (actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA && actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu != P_0.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
			if (ySGqoPAVOxrtlyHbiechAnovEvmd == null)
			{
				return num;
			}
			IList<ActionElementMap> list = controllerMap.ySGqoPAVOxrtlyHbiechAnovEvmd;
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
			for (int num2 = ySGqoPAVOxrtlyHbiechAnovEvmd.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = ySGqoPAVOxrtlyHbiechAnovEvmd[num2];
				if (!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					for (int i = 0; i < count; i++)
					{
						if ((!skipDisabledMaps || list[i].uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.CheckForAssignmentConflict(list[i]))
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
			if (skipDisabledMaps && (!_enabled || !actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
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
			if (ySGqoPAVOxrtlyHbiechAnovEvmd == null)
			{
				return num;
			}
			for (int num2 = ySGqoPAVOxrtlyHbiechAnovEvmd.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap2 = ySGqoPAVOxrtlyHbiechAnovEvmd[num2];
				if ((!skipDisabledMaps || actionElementMap2.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap2.CheckForAssignmentConflict(actionElementMap))
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
			if (ySGqoPAVOxrtlyHbiechAnovEvmd == null)
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
			for (int num2 = ySGqoPAVOxrtlyHbiechAnovEvmd.Count - 1; num2 >= 0; num2--)
			{
				ActionElementMap actionElementMap = ySGqoPAVOxrtlyHbiechAnovEvmd[num2];
				if ((!skipDisabledMaps || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA) && actionElementMap.nJilCjIhFvMUTsTBcUWuYpormNsu != conflictCheck.elementMapId && actionElementMap.CheckForAssignmentConflict(elementAssignment))
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
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				array[i] = VRlwPFoStycupprwoRTWAUXYBQXF[i].elementIdentifierName;
			}
			return array;
		}

		public string ToXmlString()
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return string.Empty;
			}
			try
			{
				return hVBdSQOAnAWUAuUHoilAcrLyEhOv().ToXmlString(writeDocumentTag: true);
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to XML. " + ex.Message);
				return string.Empty;
			}
		}

		public string ToJsonString()
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return string.Empty;
			}
			try
			{
				return hVBdSQOAnAWUAuUHoilAcrLyEhOv().ToJsonString();
			}
			catch (Exception ex)
			{
				Logger.LogWarning("Error writing " + GetType().Name + " to JSON. " + ex.Message);
				return string.Empty;
			}
		}

		public ControllerTemplateMap ToControllerTemplateMap(Guid templateTypeGuid)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
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
				KvKIjjJtUTuaYVUulSaPgImHJaaT kvKIjjJtUTuaYVUulSaPgImHJaaT = ReInput.lsQXmwJMQCkfHSQCVwAraUenyajb(templateTypeGuid);
				string text = ((kvKIjjJtUTuaYVUulSaPgImHJaaT != null) ? kvKIjjJtUTuaYVUulSaPgImHJaaT.wXnvrcQpjzGQLpuTKnvzchYGVJMT : templateTypeGuid.ToString());
				Logger.LogError("The Controller does not implement " + text + ".", requiredThreadSafety: true);
				return null;
			}
			return ControllerTemplateMap.etXjiJxFMJGdZePDIixBOnvxkoedb(controllerTemplate, this);
		}

		public ControllerTemplateMap ToControllerTemplateMap<T>() where T : class
		{
			return ToControllerTemplateMap(typeof(T));
		}

		public ControllerTemplateMap ToControllerTemplateMap(Type templateInterfaceType)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			if (templateInterfaceType == null)
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
			return ControllerTemplateMap.etXjiJxFMJGdZePDIixBOnvxkoedb(controllerTemplate, this);
		}

		private ControllerTemplateMap VKXlUxodMWADvCxuJlvzlPITgIQG(IControllerTemplate P_0)
		{
			if (ReInput._id != zYMtfthQqWFUiFGChqAKAcaBAqFL)
			{
				ReInput.CheckInitialized(zYMtfthQqWFUiFGChqAKAcaBAqFL);
				return null;
			}
			if (P_0 == null)
			{
				throw new ArgumentNullException("controllerTemplate");
			}
			return ControllerTemplateMap.etXjiJxFMJGdZePDIixBOnvxkoedb(P_0, this);
		}

		internal virtual bool MJooKrlGDJFRhfXMOcJbjtQgiaYJA(ActionElementMap P_0)
		{
			if (!YvTmZmawDMlBXCkDXqRSiLnRLllk(P_0._elementType))
			{
				return false;
			}
			PezDvOGcKjNEkBMHZWqCEhajEEXPB(P_0);
			return true;
		}

		internal virtual int knRVlfEXYlRaoRSAktAGsblDVoJP(List<ActionElementMap> P_0, bool P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("results");
			}
			int count = P_0.Count;
			int count2 = VRlwPFoStycupprwoRTWAUXYBQXF.Count;
			for (int i = 0; i < count2; i++)
			{
				if (!P_1 || VRlwPFoStycupprwoRTWAUXYBQXF[i].uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					P_0.Add(VRlwPFoStycupprwoRTWAUXYBQXF[i]);
				}
			}
			return P_0.Count - count;
		}

		internal virtual ActionElementMap HYreXCAlSycyyfzLleEmQGUDFSgSA(int P_0, int P_1, ControllerElementType P_2)
		{
			if (!YvTmZmawDMlBXCkDXqRSiLnRLllk(P_2))
			{
				return null;
			}
			int num = rSpTdMBDrCCMwimlFcglFUsolrZGA(P_0, P_1, P_2);
			if (num < 0)
			{
				return null;
			}
			return VRlwPFoStycupprwoRTWAUXYBQXF[num];
		}

		internal virtual int glICmISHwtlFDUzxVTNsrjklgfBc(int P_0, List<ActionElementMap> P_1, bool P_2)
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
			if (VRlwPFoStycupprwoRTWAUXYBQXF == null)
			{
				return 0;
			}
			int num2 = buttonMapCount;
			for (int i = 0; i < num2; i++)
			{
				if (VRlwPFoStycupprwoRTWAUXYBQXF[i]._elementIdentifierId == P_0)
				{
					P_1.Add(VRlwPFoStycupprwoRTWAUXYBQXF[i]);
				}
			}
			return P_1.Count - num;
		}

		internal virtual bool rJOLusvCyaSUHoFtlGPHBGerHdyp(int P_0, int P_1, ControllerElementType P_2)
		{
			if (!YvTmZmawDMlBXCkDXqRSiLnRLllk(P_2))
			{
				return false;
			}
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (VRlwPFoStycupprwoRTWAUXYBQXF[i]._elementIdentifierId == P_0 && VRlwPFoStycupprwoRTWAUXYBQXF[i]._actionId == P_1)
				{
					return true;
				}
			}
			return false;
		}

		internal virtual int rSpTdMBDrCCMwimlFcglFUsolrZGA(int P_0, int P_1, ControllerElementType P_2)
		{
			if (!YvTmZmawDMlBXCkDXqRSiLnRLllk(P_2))
			{
				return -1;
			}
			if (VRlwPFoStycupprwoRTWAUXYBQXF == null)
			{
				return -1;
			}
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (VRlwPFoStycupprwoRTWAUXYBQXF[i]._elementIdentifierId == P_0 && VRlwPFoStycupprwoRTWAUXYBQXF[i]._actionId == P_1)
				{
					return i;
				}
			}
			return -1;
		}

		internal int SqXPkqocZgFeqebhmtpqdTfOwlIM(int P_0)
		{
			if (VRlwPFoStycupprwoRTWAUXYBQXF == null)
			{
				return -1;
			}
			int num = buttonMapCount;
			for (int i = 0; i < num; i++)
			{
				if (VRlwPFoStycupprwoRTWAUXYBQXF[i].nJilCjIhFvMUTsTBcUWuYpormNsu == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		internal int wKqSQgsLhkpIZlQGixUKHDIBCvXU(bool P_0, List<ActionElementMap> P_1, bool P_2)
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
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[i];
				if (!P_0 || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA)
				{
					P_1.Add(actionElementMap);
					num2++;
				}
			}
			return num2;
		}

		internal int iQchYbBsHSgyRzunLuzvVgWwAaeh(int P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
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
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[i];
				if (actionElementMap._actionId == P_0 && (!P_1 || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					P_2.Add(actionElementMap);
					num2++;
				}
			}
			return num2;
		}

		internal virtual int StwzmqpEamBsxgECgGOOyjBfNurP(int P_0, bool P_1, List<ActionElementMap> P_2, bool P_3)
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
				ActionElementMap actionElementMap = VRlwPFoStycupprwoRTWAUXYBQXF[i];
				if (actionElementMap._actionId == P_0 && (!P_1 || actionElementMap.uPyFcaFdRzKajesnqkOUtFvpIRKHA))
				{
					P_2.Add(actionElementMap);
					num++;
				}
			}
			return num;
		}

		internal virtual ActionElementMap BtSVfeTrzQORMCWyDKlguEcdjZpHA(IControllerElementTarget P_0, bool P_1, int P_2, bool P_3, out bool P_4)
		{
			P_4 = false;
			if (P_1 && P_2 < 0)
			{
				P_4 = true;
				return null;
			}
			if (!ATKqmUnEHRvSSUhBOknVpLbPhUtX(P_0))
			{
				P_4 = true;
				return null;
			}
			if (!YvTmZmawDMlBXCkDXqRSiLnRLllk(P_0.elementType))
			{
				return null;
			}
			int num = buttonMapCount;
			_ = P_0.elementIdentifierId;
			for (int i = 0; i < num; i++)
			{
				if ((!P_1 || VRlwPFoStycupprwoRTWAUXYBQXF[i]._actionId == P_2) && (!P_3 || VRlwPFoStycupprwoRTWAUXYBQXF[i].uPyFcaFdRzKajesnqkOUtFvpIRKHA) && VRlwPFoStycupprwoRTWAUXYBQXF[i].IsTarget(P_0))
				{
					return VRlwPFoStycupprwoRTWAUXYBQXF[i];
				}
			}
			return null;
		}

		internal virtual int yxkdMnawZniDZXFmujYxcNWEtVSFA(IControllerElementTarget P_0, bool P_1, int P_2, bool P_3, List<ActionElementMap> P_4, bool P_5, out bool P_6)
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
			if (!ATKqmUnEHRvSSUhBOknVpLbPhUtX(P_0))
			{
				P_6 = true;
				return num;
			}
			if (!YvTmZmawDMlBXCkDXqRSiLnRLllk(P_0.elementType))
			{
				return num;
			}
			int num2 = buttonMapCount;
			_ = P_0.elementIdentifierId;
			for (int i = 0; i < num2; i++)
			{
				if ((!P_1 || VRlwPFoStycupprwoRTWAUXYBQXF[i]._actionId == P_2) && (!P_3 || VRlwPFoStycupprwoRTWAUXYBQXF[i].uPyFcaFdRzKajesnqkOUtFvpIRKHA) && VRlwPFoStycupprwoRTWAUXYBQXF[i].IsTarget(P_0))
				{
					P_4.Add(VRlwPFoStycupprwoRTWAUXYBQXF[i]);
					num++;
				}
			}
			return num;
		}

		internal void ZVdtyfwTyDwVTuIonGIxjptiyrFA(int P_0, ControllerElementType P_1)
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
				HPWPNfmeSDuLnzXMyTkqQVhfoWLd(elementMap);
			}
		}

		internal virtual bool HPWPNfmeSDuLnzXMyTkqQVhfoWLd(ActionElementMap P_0)
		{
			if (P_0 == null)
			{
				return false;
			}
			if (!YvTmZmawDMlBXCkDXqRSiLnRLllk(P_0._elementType))
			{
				return false;
			}
			VRlwPFoStycupprwoRTWAUXYBQXF.Add(P_0);
			RitJKwyVulqhfjIjIbSwgSykCWuwA(P_0);
			return true;
		}

		internal bool ATKqmUnEHRvSSUhBOknVpLbPhUtX(IControllerElementTarget P_0)
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

		internal bool COZPtosByiWFIIYpuMiNeiToydZW(string P_0)
		{
			try
			{
				SgsBMSItxbPvgtEJRxnDZAFORNZj(SerializedObject.FromXml(GetType(), P_0));
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogError("Error creating  " + GetType().Name + "  from XML. " + ex.Message);
				return false;
			}
		}

		internal bool fwCoBahZERRKwolRQILikHUYDTVq(string P_0)
		{
			try
			{
				SgsBMSItxbPvgtEJRxnDZAFORNZj(SerializedObject.FromJson(GetType(), P_0));
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogError("Error creating  " + GetType().Name + "  from JSON. " + ex.Message);
				return false;
			}
		}

		internal void RitJKwyVulqhfjIjIbSwgSykCWuwA(ActionElementMap P_0)
		{
			if (P_0 != null)
			{
				ySGqoPAVOxrtlyHbiechAnovEvmd.Add(P_0);
				ySGqoPAVOxrtlyHbiechAnovEvmd.Sort(OpWxcQrhSLqRHNtoglCEeChHsGtf.MIhwAOqhmoaWRsWmUHdjgEsxahKr);
				GKClWfkOaAcWgcWSeeqXJvARlRJB();
			}
		}

		internal void LdlsrfeCcJddaRXscQXRsHwnwEAl(int P_0)
		{
			int num = KuognzhgcmmLiLzBHpCcAPSqchwU(P_0);
			if (num >= 0)
			{
				ySGqoPAVOxrtlyHbiechAnovEvmd.RemoveAt(num);
				GKClWfkOaAcWgcWSeeqXJvARlRJB();
			}
		}

		internal void lOdUHYjHyCASfHGlVAAWZCLSoutmA(int P_0, ActionElementMap P_1)
		{
			if (P_1 != null)
			{
				int num = KuognzhgcmmLiLzBHpCcAPSqchwU(P_0);
				if (num >= 0)
				{
					ySGqoPAVOxrtlyHbiechAnovEvmd[num] = P_1;
					ySGqoPAVOxrtlyHbiechAnovEvmd.Sort(OpWxcQrhSLqRHNtoglCEeChHsGtf.MIhwAOqhmoaWRsWmUHdjgEsxahKr);
					GKClWfkOaAcWgcWSeeqXJvARlRJB();
				}
			}
		}

		internal static void LxwILqboYSTsOrojOayZHEnpMZuBb(ActionElementMap P_0, int P_1, Pole P_2, int P_3, ControllerElementType P_4, AxisRange P_5, bool P_6)
		{
			P_0.poaYXnSDRTkbjnijNbPqDYgSEAiS();
			P_0._actionId = P_1;
			P_0._elementType = P_4;
			P_0._elementIdentifierId = P_3;
			P_0._axisContribution = P_2;
			P_0._axisRange = P_5;
			if (P_4 == ControllerElementType.Axis)
			{
				P_0._invert = P_6;
			}
			P_0.mcFJwsoNSHHvPNLkGxwFTlHQondm();
		}

		protected void BakeElementMap(ActionElementMap map)
		{
			if (map != null)
			{
				ReInput.controllers.GetController(_controllerType, _controllerId)?.VnQORoKBKYcDfQniJOyRPalZgtMZ(this, map);
			}
		}

		internal virtual bool SgsBMSItxbPvgtEJRxnDZAFORNZj(SerializedObject P_0)
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
						actionElementMap.nynjKwRhGWhMIfJsmDLrvmxlEBrrA(value2);
						if (ActionElementMap.rWwBQUblidRcekuSVdiHTcNfrIlmA(actionElementMap))
						{
							PezDvOGcKjNEkBMHZWqCEhajEEXPB(actionElementMap);
						}
					}
				}
			}
			GKClWfkOaAcWgcWSeeqXJvARlRJB();
			return flag;
		}

		internal virtual void jizQYKJmGXYvAYwHkHUTwVXZgVDE(SerializedObject P_0)
		{
			if (P_0.xmlInfo == null)
			{
				P_0.xmlInfo = new SerializedObject.XmlInfo();
			}
			P_0.Add("dataVersion", 2, SerializedObject.FieldOptions.ExculdeFromXml);
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "dataVersion",
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = 2.ToString()
			});
			if ((object)GetType() == typeof(JoystickMap))
			{
				Joystick joystick = ReInput.controllers.GetJoystick(_controllerId);
				Guid guid = joystick?.hardwareTypeGuid ?? Guid.Empty;
				string vLOpmXQkMsPmsBAJNcXkcKfWoznZ = ((joystick != null) ? SerializationTools.CleanInvalidXmlChars(joystick.hardwareName) : "Unknown");
				P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
				{
					qItGqcAiFsVuEXTeGzrduYLlUPFM = "hardwareGuid",
					vLOpmXQkMsPmsBAJNcXkcKfWoznZ = guid.ToString()
				});
				P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
				{
					qItGqcAiFsVuEXTeGzrduYLlUPFM = "hardwareName",
					vLOpmXQkMsPmsBAJNcXkcKfWoznZ = vLOpmXQkMsPmsBAJNcXkcKfWoznZ
				});
			}
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				XCTmYYtBCCzTuZmlJPszaypSgJdS = "xmlns",
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "xsi",
				GpDRMyFZBJdjlvWjrvVYAzZnhbYW = null,
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = "http://www.w3.org/2001/XMLSchema-instance"
			});
			P_0.xmlInfo.attributes.Add(new SerializedObject.XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo
			{
				XCTmYYtBCCzTuZmlJPszaypSgJdS = "xsi",
				qItGqcAiFsVuEXTeGzrduYLlUPFM = "schemaLocation",
				GpDRMyFZBJdjlvWjrvVYAzZnhbYW = null,
				vLOpmXQkMsPmsBAJNcXkcKfWoznZ = string.Format("{0} {1}{2}{3}{4}{5}", "http://guavaman.com/rewired", "http://guavaman.com/schemas/rewired/", "1.1", "/", GetType().Name, ".xsd")
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
				if (VRlwPFoStycupprwoRTWAUXYBQXF[i] != null)
				{
					list.Add(VRlwPFoStycupprwoRTWAUXYBQXF[i].nXkDBItKfPbLOehFpkJWWFJeYwiu());
				}
			}
		}

		private bool YvTmZmawDMlBXCkDXqRSiLnRLllk(ControllerElementType P_0)
		{
			if (P_0 != ControllerElementType.Button)
			{
				return false;
			}
			return true;
		}

		private void RKPAwAHEdOlsLfKXlHroBMcxRwNtA(int P_0, int P_1)
		{
			LdlsrfeCcJddaRXscQXRsHwnwEAl(P_0);
			if (P_1 >= 0 && P_1 < buttonMapCount)
			{
				VRlwPFoStycupprwoRTWAUXYBQXF.RemoveAt(P_1);
			}
		}

		private void PezDvOGcKjNEkBMHZWqCEhajEEXPB(ActionElementMap P_0)
		{
			if (P_0 != null)
			{
				VRlwPFoStycupprwoRTWAUXYBQXF.Add(P_0);
				RitJKwyVulqhfjIjIbSwgSykCWuwA(P_0);
			}
		}

		private void wqlRbUaqCripqSwUAZoMHOauyNbp(ActionElementMap P_0, int P_1)
		{
			if (P_0 != null && P_1 >= 0 && P_1 < buttonMapCount)
			{
				lOdUHYjHyCASfHGlVAAWZCLSoutmA(VRlwPFoStycupprwoRTWAUXYBQXF[P_1].nJilCjIhFvMUTsTBcUWuYpormNsu, P_0);
				VRlwPFoStycupprwoRTWAUXYBQXF[P_1] = P_0;
			}
		}

		private int KuognzhgcmmLiLzBHpCcAPSqchwU(int P_0)
		{
			if (ySGqoPAVOxrtlyHbiechAnovEvmd == null)
			{
				return -1;
			}
			int count = ySGqoPAVOxrtlyHbiechAnovEvmd.Count;
			for (int i = 0; i < count; i++)
			{
				if (ySGqoPAVOxrtlyHbiechAnovEvmd[i].nJilCjIhFvMUTsTBcUWuYpormNsu == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		private SerializedObject hVBdSQOAnAWUAuUHoilAcrLyEhOv()
		{
			SerializedObject serializedObject = new SerializedObject(GetType(), SerializedObject.ObjectType.Object);
			jizQYKJmGXYvAYwHkHUTwVXZgVDE(serializedObject);
			return serializedObject;
		}

		internal void GKClWfkOaAcWgcWSeeqXJvARlRJB()
		{
			if (!mCTnDNAaJjJXoZDRSzjNPwGcXRBm)
			{
				umIdXBbEcxboeZWinXJoMgkphcbC = ReInput.realTime;
			}
		}

		public static ControllerMap Create(Controller controller, int categoryId, int layoutId)
		{
			return MLvDPLunynqMBBBXdVjBBnmzSBEl(controller, categoryId, layoutId);
		}

		internal static ControllerMap tTAynXDpgjHudMCdDyGjoeUtdBDX(ControllerType P_0)
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

		internal static ControllerMap MLvDPLunynqMBBBXdVjBBnmzSBEl(Controller P_0, int P_1, int P_2)
		{
			if (P_0 == null)
			{
				return null;
			}
			return P_0.type switch
			{
				ControllerType.Keyboard => KeyboardMap.ZjKfUjGyzyNIUfydHfYlzgEqlodt(P_0.hardwareTypeGuid, P_1, P_2), 
				ControllerType.Mouse => MouseMap.JhOhEpSscAngETaHuKgQZGczibSC(P_0.hardwareTypeGuid, P_1, P_2), 
				ControllerType.Joystick => JoystickMap.PfrqCKEIgMPkwkEXazIGyUcXeKcP(P_0.hardwareTypeGuid, P_1, P_2), 
				ControllerType.Custom => CustomControllerMap.WMPviajLMUiWvPCoyygwQMdkXrtE(P_0.hardwareTypeGuid, ((CustomController)P_0).sourceControllerId, P_1, P_2), 
				_ => throw new NotImplementedException(), 
			};
		}

		public static ControllerMap CreateFromXml(ControllerType controllerType, string xmlString)
		{
			if (string.IsNullOrEmpty(xmlString))
			{
				return null;
			}
			ControllerMap controllerMap = tTAynXDpgjHudMCdDyGjoeUtdBDX(controllerType);
			try
			{
				RAmMePHwhbbjmrfLAYKtBaJPbccQ();
				controllerMap.COZPtosByiWFIIYpuMiNeiToydZW(xmlString);
				return controllerMap;
			}
			catch
			{
				return null;
			}
			finally
			{
				oeOZZgeXJicFbaxfdmvQlNMqgCjfA();
			}
		}

		public static ControllerMap CreateFromJson(ControllerType controllerType, string jsonString)
		{
			if (string.IsNullOrEmpty(jsonString))
			{
				return null;
			}
			ControllerMap controllerMap = tTAynXDpgjHudMCdDyGjoeUtdBDX(controllerType);
			try
			{
				RAmMePHwhbbjmrfLAYKtBaJPbccQ();
				controllerMap.fwCoBahZERRKwolRQILikHUYDTVq(jsonString);
				return controllerMap;
			}
			catch
			{
				return null;
			}
			finally
			{
				oeOZZgeXJicFbaxfdmvQlNMqgCjfA();
			}
		}

		internal static void RAmMePHwhbbjmrfLAYKtBaJPbccQ()
		{
			IoBmcKKKNFbgvDusUrVIIDEADjEfb++;
		}

		internal static void oeOZZgeXJicFbaxfdmvQlNMqgCjfA()
		{
			IoBmcKKKNFbgvDusUrVIIDEADjEfb--;
			if (IoBmcKKKNFbgvDusUrVIIDEADjEfb < 0)
			{
				IoBmcKKKNFbgvDusUrVIIDEADjEfb = 0;
				Logger.LogError("Too many calls to disable internal modify mode!");
			}
		}
	}
}
