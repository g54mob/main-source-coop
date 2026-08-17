using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Utils
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal abstract class SafeDelegate : ICloneable
	{
		private static Action<Exception> MAShMhdEbDPdStfeakMFYqMIzgMz;

		internal abstract int Count { get; }

		internal abstract Action<Exception> ExceptionHandler { get; set; }

		internal static Action<Exception> S_ExceptionHandler
		{
			get
			{
				return MAShMhdEbDPdStfeakMFYqMIzgMz;
			}
			set
			{
				MAShMhdEbDPdStfeakMFYqMIzgMz = value;
			}
		}

		internal abstract void RemoveDelegateOrAllDelegatesFromAnObject(object obj);

		internal abstract void Clear();

		public abstract object Clone();
	}
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal abstract class SafeDelegate<T> : SafeDelegate where T : class
	{
		private class pfIpRNQbsGJGGxmZEomBhrzUEAKjA
		{
			public readonly T xedWSzVJwHnnMBdVsUGTmlSXymCn;

			public readonly object hjhfXnYcjdvcDwtVrmCFBbESghTt;

			public readonly object ISLaqVLVfIAvkcDZuUbCmkvfyBJi;

			public readonly bool dKvJTvEAINgMLhROTasIXwsdqvaB;

			public pfIpRNQbsGJGGxmZEomBhrzUEAKjA(T P_0)
			{
				xedWSzVJwHnnMBdVsUGTmlSXymCn = P_0;
				hjhfXnYcjdvcDwtVrmCFBbESghTt = ((Delegate)(object)P_0).Target;
				try
				{
					ISLaqVLVfIAvkcDZuUbCmkvfyBJi = ReflectionTools.GetMethodInfo((Delegate)(object)P_0);
				}
				catch
				{
					ISLaqVLVfIAvkcDZuUbCmkvfyBJi = null;
				}
				dKvJTvEAINgMLhROTasIXwsdqvaB = hjhfXnYcjdvcDwtVrmCFBbESghTt != null && hjhfXnYcjdvcDwtVrmCFBbESghTt is UnityEngine.Object;
			}

			public pfIpRNQbsGJGGxmZEomBhrzUEAKjA(pfIpRNQbsGJGGxmZEomBhrzUEAKjA P_0)
				: this(MiscTools.Clone((object)P_0.xedWSzVJwHnnMBdVsUGTmlSXymCn) as T)
			{
			}

			public bool RsjgfXSpbSjEggZVZcFjrnIFoHWc()
			{
				if (hjhfXnYcjdvcDwtVrmCFBbESghTt != null)
				{
					if (hjhfXnYcjdvcDwtVrmCFBbESghTt is UnityEngine.Object)
					{
						return (UnityEngine.Object)hjhfXnYcjdvcDwtVrmCFBbESghTt == null;
					}
					return false;
				}
				return true;
			}
		}

		private Action<Exception> uOQdIgbpZrLCkzjIUdeMAjaaRNmyB;

		private readonly List<pfIpRNQbsGJGGxmZEomBhrzUEAKjA> FsWMubjDrAlVVXFdhIKrFOEOFZXN;

		private readonly List<pfIpRNQbsGJGGxmZEomBhrzUEAKjA> DceSlvRMRgcqvKUxdnZqakqbDtVeb;

		int SafeDelegate.Count => FsWMubjDrAlVVXFdhIKrFOEOFZXN.Count;

		Action<Exception> SafeDelegate.ExceptionHandler
		{
			get
			{
				return uOQdIgbpZrLCkzjIUdeMAjaaRNmyB;
			}
			set
			{
				uOQdIgbpZrLCkzjIUdeMAjaaRNmyB = value;
			}
		}

		protected SafeDelegate()
		{
			if (!ReflectionTools.DoesTypeImplement(typeof(T), typeof(Delegate)))
			{
				throw new Exception(typeof(T).Name + " is not a delegate type! SafeDelegate only works with delegate types.");
			}
			FsWMubjDrAlVVXFdhIKrFOEOFZXN = new List<pfIpRNQbsGJGGxmZEomBhrzUEAKjA>();
			DceSlvRMRgcqvKUxdnZqakqbDtVeb = new List<pfIpRNQbsGJGGxmZEomBhrzUEAKjA>();
			if (uOQdIgbpZrLCkzjIUdeMAjaaRNmyB == null)
			{
				uOQdIgbpZrLCkzjIUdeMAjaaRNmyB = SafeDelegate.S_ExceptionHandler;
			}
		}

		protected SafeDelegate(Action<Exception> P_0)
			: this()
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("exceptionHandler");
			}
			uOQdIgbpZrLCkzjIUdeMAjaaRNmyB = P_0;
		}

		protected SafeDelegate(SafeDelegate<T> P_0)
			: this()
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("source");
			}
			if (P_0.uOQdIgbpZrLCkzjIUdeMAjaaRNmyB != null)
			{
				uOQdIgbpZrLCkzjIUdeMAjaaRNmyB = P_0.uOQdIgbpZrLCkzjIUdeMAjaaRNmyB;
			}
			for (int i = 0; i < P_0.FsWMubjDrAlVVXFdhIKrFOEOFZXN.Count; i++)
			{
				FsWMubjDrAlVVXFdhIKrFOEOFZXN.Add(new pfIpRNQbsGJGGxmZEomBhrzUEAKjA(P_0.FsWMubjDrAlVVXFdhIKrFOEOFZXN[i]));
			}
		}

		public void AddDelegate(T @delegate)
		{
			if (@delegate == null)
			{
				return;
			}
			List<Delegate> list = zMjzYyPsFUNdcFJutYznmxGDfkOA((Delegate)(object)@delegate);
			if (list == null || list.Count == 0)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				T val = (T)(object)list[i];
				if (!FvWcyJhnAQeSPfVoySPxbkhCJVSRB(val))
				{
					FsWMubjDrAlVVXFdhIKrFOEOFZXN.Add(new pfIpRNQbsGJGGxmZEomBhrzUEAKjA(val));
				}
			}
		}

		public void RemoveDelegate(T @delegate)
		{
			if (@delegate == null)
			{
				return;
			}
			List<Delegate> list = zMjzYyPsFUNdcFJutYznmxGDfkOA((Delegate)(object)@delegate);
			if (list == null || list.Count == 0)
			{
				return;
			}
			int count = FsWMubjDrAlVVXFdhIKrFOEOFZXN.Count;
			for (int i = 0; i < list.Count; i++)
			{
				for (int num = count - 1; num >= 0; num--)
				{
					if (EqualityComparer<T>.Default.Equals(FsWMubjDrAlVVXFdhIKrFOEOFZXN[num].xedWSzVJwHnnMBdVsUGTmlSXymCn, (T)(object)list[i]))
					{
						FsWMubjDrAlVVXFdhIKrFOEOFZXN.RemoveAt(num);
					}
				}
			}
		}

		internal override void RemoveDelegateOrAllDelegatesFromAnObject(object obj)
		{
			for (int num = FsWMubjDrAlVVXFdhIKrFOEOFZXN.Count - 1; num >= 0; num--)
			{
				Delegate obj2 = kjSFQBXMOqJPLkhTVUQkwFZEAqejA(obj, (Delegate)(object)FsWMubjDrAlVVXFdhIKrFOEOFZXN[num].xedWSzVJwHnnMBdVsUGTmlSXymCn);
				if (lDUCuAniJClknACvDADWmojtkFxg(obj2) == 0)
				{
					FsWMubjDrAlVVXFdhIKrFOEOFZXN.RemoveAt(num);
				}
				else
				{
					FsWMubjDrAlVVXFdhIKrFOEOFZXN[num] = new pfIpRNQbsGJGGxmZEomBhrzUEAKjA((T)(object)obj2);
				}
			}
		}

		internal override void Clear()
		{
			FsWMubjDrAlVVXFdhIKrFOEOFZXN.Clear();
		}

		protected void Invoke(Action<object, T> invokeCallback)
		{
			if (invokeCallback == null)
			{
				throw new ArgumentNullException("invokeCallback");
			}
			int count = FsWMubjDrAlVVXFdhIKrFOEOFZXN.Count;
			if (count == 0)
			{
				return;
			}
			DceSlvRMRgcqvKUxdnZqakqbDtVeb.Clear();
			for (int i = 0; i < count; i++)
			{
				DceSlvRMRgcqvKUxdnZqakqbDtVeb.Add(FsWMubjDrAlVVXFdhIKrFOEOFZXN[i]);
			}
			List<int> list = null;
			for (int j = 0; j < count; j++)
			{
				pfIpRNQbsGJGGxmZEomBhrzUEAKjA pfIpRNQbsGJGGxmZEomBhrzUEAKjA2 = DceSlvRMRgcqvKUxdnZqakqbDtVeb[j];
				if (pfIpRNQbsGJGGxmZEomBhrzUEAKjA2.dKvJTvEAINgMLhROTasIXwsdqvaB && pfIpRNQbsGJGGxmZEomBhrzUEAKjA2.RsjgfXSpbSjEggZVZcFjrnIFoHWc())
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
					invokeCallback(this, pfIpRNQbsGJGGxmZEomBhrzUEAKjA2.xedWSzVJwHnnMBdVsUGTmlSXymCn);
				}
				catch (Exception ex)
				{
					if (uOQdIgbpZrLCkzjIUdeMAjaaRNmyB != null)
					{
						uOQdIgbpZrLCkzjIUdeMAjaaRNmyB(ex);
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
					FsWMubjDrAlVVXFdhIKrFOEOFZXN.RemoveAt(list[num]);
				}
				TempListPool.Return(list);
			}
			if (count > 0)
			{
				DceSlvRMRgcqvKUxdnZqakqbDtVeb.Clear();
			}
		}

		protected T GetCombinedDelegate()
		{
			if (FsWMubjDrAlVVXFdhIKrFOEOFZXN == null)
			{
				return null;
			}
			T val = null;
			for (int i = 0; i < FsWMubjDrAlVVXFdhIKrFOEOFZXN.Count; i++)
			{
				T xedWSzVJwHnnMBdVsUGTmlSXymCn = FsWMubjDrAlVVXFdhIKrFOEOFZXN[i].xedWSzVJwHnnMBdVsUGTmlSXymCn;
				if (val == null)
				{
					val = xedWSzVJwHnnMBdVsUGTmlSXymCn;
					continue;
				}
				try
				{
					val = (T)(object)Delegate.Combine((Delegate)(object)val, (Delegate)(object)xedWSzVJwHnnMBdVsUGTmlSXymCn);
				}
				catch
				{
				}
			}
			return val;
		}

		private bool FvWcyJhnAQeSPfVoySPxbkhCJVSRB(T P_0)
		{
			return EmEqKPzMOTOsoDBmxHAidGyuLdAhb(P_0) >= 0;
		}

		private int EmEqKPzMOTOsoDBmxHAidGyuLdAhb(T P_0)
		{
			int count = FsWMubjDrAlVVXFdhIKrFOEOFZXN.Count;
			for (int i = 0; i < count; i++)
			{
				if (EqualityComparer<T>.Default.Equals(FsWMubjDrAlVVXFdhIKrFOEOFZXN[i].xedWSzVJwHnnMBdVsUGTmlSXymCn, P_0))
				{
					return i;
				}
			}
			return -1;
		}

		private static Delegate kjSFQBXMOqJPLkhTVUQkwFZEAqejA(object P_0, Delegate P_1)
		{
			if ((object)P_1 == null || P_0 == null)
			{
				return P_1;
			}
			if (P_0 is Delegate)
			{
				return fiaWiQiwWpnwDenczvzMnMdZkUeR((Delegate)P_0, P_1);
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

		private static Delegate fiaWiQiwWpnwDenczvzMnMdZkUeR(Delegate P_0, Delegate P_1)
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

		private static int lDUCuAniJClknACvDADWmojtkFxg(Delegate P_0)
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

		private static List<Delegate> zMjzYyPsFUNdcFJutYznmxGDfkOA(Delegate P_0)
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
