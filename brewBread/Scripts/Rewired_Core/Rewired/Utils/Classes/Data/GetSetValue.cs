using System;
using Rewired.Utils.Interfaces;

namespace Rewired.Utils.Classes.Data
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal sealed class GetSetValue<T> : IGetSetValue<T>, IGetValue<T>, ISetValue<T>
	{
		private Func<T> XwzDgvMWltiflzVyqEhvijYXnoqzA;

		private Action<T> oTYaMbEJOHgWFriwyKVuQJmGgRLMA;

		public Func<T> getValueDelegate
		{
			get
			{
				return XwzDgvMWltiflzVyqEhvijYXnoqzA;
			}
			set
			{
				XwzDgvMWltiflzVyqEhvijYXnoqzA = value;
			}
		}

		public Action<T> setValueDelegate
		{
			get
			{
				return oTYaMbEJOHgWFriwyKVuQJmGgRLMA;
			}
			set
			{
				oTYaMbEJOHgWFriwyKVuQJmGgRLMA = value;
			}
		}

		public GetSetValue(Func<T> P_0, Action<T> P_1)
		{
			XwzDgvMWltiflzVyqEhvijYXnoqzA = P_0;
			oTYaMbEJOHgWFriwyKVuQJmGgRLMA = P_1;
		}

		public T GetValue()
		{
			if (XwzDgvMWltiflzVyqEhvijYXnoqzA == null)
			{
				throw new ArgumentNullException("getValueDelegate");
			}
			return XwzDgvMWltiflzVyqEhvijYXnoqzA();
		}

		public void SetValue(T value)
		{
			if (oTYaMbEJOHgWFriwyKVuQJmGgRLMA == null)
			{
				throw new ArgumentNullException("setValueDelegate");
			}
			oTYaMbEJOHgWFriwyKVuQJmGgRLMA(value);
		}
	}
}
