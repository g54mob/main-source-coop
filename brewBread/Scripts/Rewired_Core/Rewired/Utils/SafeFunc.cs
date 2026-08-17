using System;

namespace Rewired.Utils
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class SafeFunc<T, TResult> : SafeDelegate<Func<T, TResult>>
	{
		private T iycFpybfiLRDYdIeXNFDXPzNFHkdA;

		private TResult bdnPcfinffkgAFHzAqAgJQKqxXNh;

		private static Action<object, Func<T, TResult>> eaSmezmUiTddkXzvgCvPVDenrmsG;

		private static Action<object, Func<T, TResult>> invokeDelegate => GGIVvebCPkEFsDmSRuDeLjmSaIhib;

		public SafeFunc()
		{
		}

		public SafeFunc(Action<Exception> P_0)
			: base(P_0)
		{
		}

		protected SafeFunc(SafeFunc<T, TResult> P_0)
			: base((SafeDelegate<Func<T, TResult>>)P_0)
		{
		}

		public TResult Invoke(T arg0)
		{
			iycFpybfiLRDYdIeXNFDXPzNFHkdA = arg0;
			try
			{
				Invoke(invokeDelegate);
				return bdnPcfinffkgAFHzAqAgJQKqxXNh;
			}
			catch
			{
				Logger.LogError("Error invoking SafeFunc base class.");
				return default(TResult);
			}
			finally
			{
				iycFpybfiLRDYdIeXNFDXPzNFHkdA = default(T);
				bdnPcfinffkgAFHzAqAgJQKqxXNh = default(TResult);
			}
		}

		public override object Clone()
		{
			return new SafeFunc<T, TResult>(this);
		}

		private static void GGIVvebCPkEFsDmSRuDeLjmSaIhib(object P_0, Func<T, TResult> P_1)
		{
			if (P_1 != null && P_0 is SafeFunc<T, TResult> safeFunc)
			{
				safeFunc.bdnPcfinffkgAFHzAqAgJQKqxXNh = P_1(safeFunc.iycFpybfiLRDYdIeXNFDXPzNFHkdA);
			}
		}

		public static SafeFunc<T, TResult> operator +(SafeFunc<T, TResult> eventList, Func<T, TResult> func)
		{
			if (eventList == null)
			{
				eventList = new SafeFunc<T, TResult>();
			}
			eventList.AddDelegate(func);
			return eventList;
		}

		public static SafeFunc<T, TResult> operator -(SafeFunc<T, TResult> eventList, Func<T, TResult> func)
		{
			if (eventList == null)
			{
				return null;
			}
			eventList.RemoveDelegate(func);
			return eventList;
		}

		public static implicit operator Func<T, TResult>(SafeFunc<T, TResult> obj)
		{
			return obj?.GetCombinedDelegate();
		}

		public static implicit operator SafeFunc<T, TResult>(Func<T, TResult> obj)
		{
			if (obj == null)
			{
				return null;
			}
			SafeFunc<T, TResult> safeFunc = new SafeFunc<T, TResult>();
			safeFunc.AddDelegate(obj);
			return safeFunc;
		}
	}
}
