using System;

namespace Rewired.Utils
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class SafePredicate<T> : SafeDelegate<Predicate<T>>
	{
		private T iycFpybfiLRDYdIeXNFDXPzNFHkdA;

		private bool bdnPcfinffkgAFHzAqAgJQKqxXNh;

		private static Action<object, Predicate<T>> eaSmezmUiTddkXzvgCvPVDenrmsG;

		private static Action<object, Predicate<T>> invokeDelegate => GGIVvebCPkEFsDmSRuDeLjmSaIhib;

		public SafePredicate()
		{
		}

		public SafePredicate(Action<Exception> P_0)
			: base(P_0)
		{
		}

		protected SafePredicate(SafePredicate<T> P_0)
			: base((SafeDelegate<Predicate<T>>)P_0)
		{
		}

		public bool Invoke(T arg0)
		{
			iycFpybfiLRDYdIeXNFDXPzNFHkdA = arg0;
			try
			{
				Invoke(invokeDelegate);
				return bdnPcfinffkgAFHzAqAgJQKqxXNh;
			}
			catch
			{
				Logger.LogError("Error invoking SafeDelegate base class.");
				return false;
			}
			finally
			{
				iycFpybfiLRDYdIeXNFDXPzNFHkdA = default(T);
				bdnPcfinffkgAFHzAqAgJQKqxXNh = false;
			}
		}

		public override object Clone()
		{
			return new SafePredicate<T>(this);
		}

		private static void GGIVvebCPkEFsDmSRuDeLjmSaIhib(object P_0, Predicate<T> P_1)
		{
			if (P_1 != null && P_0 is SafePredicate<T> safePredicate)
			{
				safePredicate.bdnPcfinffkgAFHzAqAgJQKqxXNh = P_1(safePredicate.iycFpybfiLRDYdIeXNFDXPzNFHkdA);
			}
		}

		public static SafePredicate<T> operator +(SafePredicate<T> eventList, Predicate<T> predicate)
		{
			if (eventList == null)
			{
				eventList = new SafePredicate<T>();
			}
			eventList.AddDelegate(predicate);
			return eventList;
		}

		public static SafePredicate<T> operator -(SafePredicate<T> eventList, Predicate<T> predicate)
		{
			if (eventList == null)
			{
				return null;
			}
			eventList.RemoveDelegate(predicate);
			return eventList;
		}

		public static implicit operator Predicate<T>(SafePredicate<T> obj)
		{
			return obj?.GetCombinedDelegate();
		}

		public static implicit operator SafePredicate<T>(Predicate<T> obj)
		{
			if (obj == null)
			{
				return null;
			}
			SafePredicate<T> safePredicate = new SafePredicate<T>();
			safePredicate.AddDelegate(obj);
			return safePredicate;
		}
	}
}
