using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Utils
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal abstract class SafeDelegate : ICloneable
	{
		private static Action<Exception> fpZaZCYQmShOrIvhGSeXexBuYwKtA;

		internal abstract int Count { get; }

		internal abstract Action<Exception> ExceptionHandler { get; set; }

		internal static Action<Exception> S_ExceptionHandler
		{
			get
			{
				return fpZaZCYQmShOrIvhGSeXexBuYwKtA;
			}
			set
			{
				fpZaZCYQmShOrIvhGSeXexBuYwKtA = value;
			}
		}

		internal abstract void RemoveDelegateOrAllDelegatesFromAnObject(object obj);

		internal abstract void Clear();

		public abstract object Clone();
	}
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal abstract class SafeDelegate<T> : SafeDelegate where T : class
	{
		private class ZVmDCTmiBmCfXBJcmZqXYxksQIHN
		{
			public readonly T gybqTscDHxcHontLfEbdWinPeCkKA;

			public readonly object QMmaBzbGhLRYBQhUiAbwUcRQwInF;

			public readonly object EZzJBXXjyieUbeYhLlWfaYRiXMqV;

			public readonly bool VpbfNLnAZtFmPVDaoFArWpYuhYTu;

			public ZVmDCTmiBmCfXBJcmZqXYxksQIHN(T P_0)
			{
				gybqTscDHxcHontLfEbdWinPeCkKA = P_0;
				QMmaBzbGhLRYBQhUiAbwUcRQwInF = ((Delegate)(object)P_0).Target;
				try
				{
					EZzJBXXjyieUbeYhLlWfaYRiXMqV = ReflectionTools.GetMethodInfo((Delegate)(object)P_0);
				}
				catch
				{
					EZzJBXXjyieUbeYhLlWfaYRiXMqV = null;
				}
				VpbfNLnAZtFmPVDaoFArWpYuhYTu = QMmaBzbGhLRYBQhUiAbwUcRQwInF != null && QMmaBzbGhLRYBQhUiAbwUcRQwInF is UnityEngine.Object;
			}

			public ZVmDCTmiBmCfXBJcmZqXYxksQIHN(ZVmDCTmiBmCfXBJcmZqXYxksQIHN P_0)
				: this(MiscTools.Clone((object)P_0.gybqTscDHxcHontLfEbdWinPeCkKA) as T)
			{
			}

			public bool RjoIYnlMCCCHeQDOkNVYsnqUILvO()
			{
				if (QMmaBzbGhLRYBQhUiAbwUcRQwInF != null)
				{
					if (QMmaBzbGhLRYBQhUiAbwUcRQwInF is UnityEngine.Object)
					{
						return (UnityEngine.Object)QMmaBzbGhLRYBQhUiAbwUcRQwInF == null;
					}
					return false;
				}
				return true;
			}
		}

		private Action<Exception> CwbPWAaYPGNxgJdSPyoDBEDDOAjr;

		private readonly List<ZVmDCTmiBmCfXBJcmZqXYxksQIHN> eEjopwMKawDinzISecKicaoZKNHDb;

		private readonly List<ZVmDCTmiBmCfXBJcmZqXYxksQIHN> tehaGGcITLJasAnKFCONSdYEENcb;

		internal override int Count => eEjopwMKawDinzISecKicaoZKNHDb.Count;

		internal override Action<Exception> ExceptionHandler
		{
			get
			{
				return CwbPWAaYPGNxgJdSPyoDBEDDOAjr;
			}
			set
			{
				CwbPWAaYPGNxgJdSPyoDBEDDOAjr = value;
			}
		}

		protected SafeDelegate()
		{
			if (!ReflectionTools.DoesTypeImplement(typeof(T), typeof(Delegate)))
			{
				throw new Exception(typeof(T).Name + " is not a delegate type! SafeDelegate only works with delegate types.");
			}
			eEjopwMKawDinzISecKicaoZKNHDb = new List<ZVmDCTmiBmCfXBJcmZqXYxksQIHN>();
			tehaGGcITLJasAnKFCONSdYEENcb = new List<ZVmDCTmiBmCfXBJcmZqXYxksQIHN>();
			if (CwbPWAaYPGNxgJdSPyoDBEDDOAjr == null)
			{
				CwbPWAaYPGNxgJdSPyoDBEDDOAjr = SafeDelegate.S_ExceptionHandler;
			}
		}

		protected SafeDelegate(Action<Exception> P_0)
			: this()
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("exceptionHandler");
			}
			CwbPWAaYPGNxgJdSPyoDBEDDOAjr = P_0;
		}

		protected SafeDelegate(SafeDelegate<T> P_0)
			: this()
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("source");
			}
			if (P_0.CwbPWAaYPGNxgJdSPyoDBEDDOAjr != null)
			{
				CwbPWAaYPGNxgJdSPyoDBEDDOAjr = P_0.CwbPWAaYPGNxgJdSPyoDBEDDOAjr;
			}
			for (int i = 0; i < P_0.eEjopwMKawDinzISecKicaoZKNHDb.Count; i++)
			{
				eEjopwMKawDinzISecKicaoZKNHDb.Add(new ZVmDCTmiBmCfXBJcmZqXYxksQIHN(P_0.eEjopwMKawDinzISecKicaoZKNHDb[i]));
			}
		}

		public void AddDelegate(T @delegate)
		{
			if (@delegate == null)
			{
				return;
			}
			List<Delegate> list = SuxMhnrCMQkelvYwUQmFQbctxDcQ((Delegate)(object)@delegate);
			if (list == null || list.Count == 0)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				T val = (T)(object)list[i];
				if (!fSBMVaLfQvgqXypKYGeLWMIbARbB(val))
				{
					eEjopwMKawDinzISecKicaoZKNHDb.Add(new ZVmDCTmiBmCfXBJcmZqXYxksQIHN(val));
				}
			}
		}

		public void RemoveDelegate(T @delegate)
		{
			if (@delegate == null)
			{
				return;
			}
			List<Delegate> list = SuxMhnrCMQkelvYwUQmFQbctxDcQ((Delegate)(object)@delegate);
			if (list == null || list.Count == 0)
			{
				return;
			}
			int count = eEjopwMKawDinzISecKicaoZKNHDb.Count;
			for (int i = 0; i < list.Count; i++)
			{
				for (int num = count - 1; num >= 0; num--)
				{
					if (EqualityComparer<T>.Default.Equals(eEjopwMKawDinzISecKicaoZKNHDb[num].gybqTscDHxcHontLfEbdWinPeCkKA, (T)(object)list[i]))
					{
						eEjopwMKawDinzISecKicaoZKNHDb.RemoveAt(num);
					}
				}
			}
		}

		internal override void RemoveDelegateOrAllDelegatesFromAnObject(object obj)
		{
			for (int num = eEjopwMKawDinzISecKicaoZKNHDb.Count - 1; num >= 0; num--)
			{
				Delegate obj2 = SAvGoXPnOxkodIMfCiCaamPAfcjTA(obj, (Delegate)(object)eEjopwMKawDinzISecKicaoZKNHDb[num].gybqTscDHxcHontLfEbdWinPeCkKA);
				if (AFTntPCNEMvTRzWlxTAnXzBwWyGT(obj2) == 0)
				{
					eEjopwMKawDinzISecKicaoZKNHDb.RemoveAt(num);
				}
				else
				{
					eEjopwMKawDinzISecKicaoZKNHDb[num] = new ZVmDCTmiBmCfXBJcmZqXYxksQIHN((T)(object)obj2);
				}
			}
		}

		internal override void Clear()
		{
			eEjopwMKawDinzISecKicaoZKNHDb.Clear();
		}

		protected void Invoke(Action<object, T> invokeCallback)
		{
			if (invokeCallback == null)
			{
				throw new ArgumentNullException("invokeCallback");
			}
			int count = eEjopwMKawDinzISecKicaoZKNHDb.Count;
			if (count == 0)
			{
				return;
			}
			tehaGGcITLJasAnKFCONSdYEENcb.Clear();
			for (int i = 0; i < count; i++)
			{
				tehaGGcITLJasAnKFCONSdYEENcb.Add(eEjopwMKawDinzISecKicaoZKNHDb[i]);
			}
			List<int> list = null;
			for (int j = 0; j < count; j++)
			{
				ZVmDCTmiBmCfXBJcmZqXYxksQIHN zVmDCTmiBmCfXBJcmZqXYxksQIHN = tehaGGcITLJasAnKFCONSdYEENcb[j];
				if (zVmDCTmiBmCfXBJcmZqXYxksQIHN.VpbfNLnAZtFmPVDaoFArWpYuhYTu && zVmDCTmiBmCfXBJcmZqXYxksQIHN.RjoIYnlMCCCHeQDOkNVYsnqUILvO())
				{
					if (list == null)
					{
						list = TempListPool.Get<int>();
					}
					list.Add(j);
					continue;
				}
				try
				{
					invokeCallback(this, zVmDCTmiBmCfXBJcmZqXYxksQIHN.gybqTscDHxcHontLfEbdWinPeCkKA);
				}
				catch (Exception ex)
				{
					if (CwbPWAaYPGNxgJdSPyoDBEDDOAjr != null)
					{
						CwbPWAaYPGNxgJdSPyoDBEDDOAjr(ex);
					}
					else if (ex.InnerException != null)
					{
						Logger.LogError(ex.InnerException, requiredThreadSafety: true);
					}
					if (list == null)
					{
						list = TempListPool.Get<int>();
					}
					list.Add(j);
				}
			}
			if (list != null)
			{
				for (int num = list.Count - 1; num >= 0; num--)
				{
					eEjopwMKawDinzISecKicaoZKNHDb.RemoveAt(list[num]);
				}
				TempListPool.Return(list);
			}
			if (count > 0)
			{
				tehaGGcITLJasAnKFCONSdYEENcb.Clear();
			}
		}

		protected T GetCombinedDelegate()
		{
			if (eEjopwMKawDinzISecKicaoZKNHDb == null)
			{
				return null;
			}
			T val = null;
			for (int i = 0; i < eEjopwMKawDinzISecKicaoZKNHDb.Count; i++)
			{
				T gybqTscDHxcHontLfEbdWinPeCkKA = eEjopwMKawDinzISecKicaoZKNHDb[i].gybqTscDHxcHontLfEbdWinPeCkKA;
				if (val == null)
				{
					val = gybqTscDHxcHontLfEbdWinPeCkKA;
					continue;
				}
				try
				{
					val = (T)(object)Delegate.Combine((Delegate)(object)val, (Delegate)(object)gybqTscDHxcHontLfEbdWinPeCkKA);
				}
				catch
				{
				}
			}
			return val;
		}

		private bool fSBMVaLfQvgqXypKYGeLWMIbARbB(T P_0)
		{
			return lsWdiPZgHHfEfIiesCpnLAlcpBgUA(P_0) >= 0;
		}

		private int lsWdiPZgHHfEfIiesCpnLAlcpBgUA(T P_0)
		{
			int count = eEjopwMKawDinzISecKicaoZKNHDb.Count;
			for (int i = 0; i < count; i++)
			{
				if (EqualityComparer<T>.Default.Equals(eEjopwMKawDinzISecKicaoZKNHDb[i].gybqTscDHxcHontLfEbdWinPeCkKA, P_0))
				{
					return i;
				}
			}
			return -1;
		}

		private static Delegate SAvGoXPnOxkodIMfCiCaamPAfcjTA(object P_0, Delegate P_1)
		{
			if ((object)P_1 == null || P_0 == null)
			{
				return P_1;
			}
			if (P_0 is Delegate)
			{
				return SAvGoXPnOxkodIMfCiCaamPAfcjTA((Delegate)P_0, P_1);
			}
			try
			{
				Delegate[] invocationList = P_1.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					if (invocationList[i].Target == P_0 || ReflectionTools.GetMethodInfo(invocationList[i]) == P_0)
					{
						if ((object)P_1 == null)
						{
							return P_1;
						}
						P_1 = Delegate.RemoveAll(P_1, invocationList[i]);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogError("Exception caught while removing delegates from list (1):\n" + ex);
			}
			return P_1;
		}

		private static Delegate SAvGoXPnOxkodIMfCiCaamPAfcjTA(Delegate P_0, Delegate P_1)
		{
			if ((object)P_0 == null || (object)P_1 == null)
			{
				return P_1;
			}
			if ((object)P_0.GetType() != P_0.GetType())
			{
				return P_1;
			}
			try
			{
				Delegate[] invocationList = P_0.GetInvocationList();
				Delegate[] invocationList2 = P_1.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					object methodInfo = ReflectionTools.GetMethodInfo(invocationList[i]);
					foreach (Delegate obj in invocationList2)
					{
						object methodInfo2 = ReflectionTools.GetMethodInfo(obj);
						if (methodInfo == methodInfo2)
						{
							if ((object)P_1 == null)
							{
								return P_1;
							}
							P_1 = Delegate.RemoveAll(P_1, obj);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogError("Exception caught while removing delegates from list (2):\n" + ex);
			}
			return P_1;
		}

		private static int AFTntPCNEMvTRzWlxTAnXzBwWyGT(Delegate P_0)
		{
			if ((object)P_0 == null)
			{
				return 0;
			}
			Delegate[] invocationList = P_0.GetInvocationList();
			if (invocationList == null)
			{
				return 0;
			}
			return invocationList.Length;
		}

		private static List<Delegate> SuxMhnrCMQkelvYwUQmFQbctxDcQ(Delegate P_0)
		{
			if ((object)P_0 == null)
			{
				return null;
			}
			Delegate[] invocationList = P_0.GetInvocationList();
			if (invocationList == null)
			{
				return null;
			}
			List<Delegate> list = new List<Delegate>(invocationList.Length);
			for (int i = 0; i < invocationList.Length; i++)
			{
				list.Add(invocationList[i]);
			}
			return list;
		}
	}
}
