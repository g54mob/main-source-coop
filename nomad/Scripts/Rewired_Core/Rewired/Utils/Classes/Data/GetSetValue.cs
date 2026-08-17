using System;
using Rewired.Utils.Interfaces;

namespace Rewired.Utils.Classes.Data
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal sealed class GetSetValue<T> : IGetSetValue<T>, IGetValue<T>, ISetValue<T>
	{
		private Func<T> tTkEHXrLADjpKTdWlCTVajPNmVji;

		private Action<T> uGTLwjEAEYMApcGiEhNuBIpazysQA;

		public Func<T> getValueDelegate
		{
			get
			{
				return tTkEHXrLADjpKTdWlCTVajPNmVji;
			}
			set
			{
				tTkEHXrLADjpKTdWlCTVajPNmVji = value;
			}
		}

		public Action<T> setValueDelegate
		{
			get
			{
				return uGTLwjEAEYMApcGiEhNuBIpazysQA;
			}
			set
			{
				uGTLwjEAEYMApcGiEhNuBIpazysQA = value;
			}
		}

		public GetSetValue(Func<T> P_0, Action<T> P_1)
		{
			tTkEHXrLADjpKTdWlCTVajPNmVji = P_0;
			uGTLwjEAEYMApcGiEhNuBIpazysQA = P_1;
		}

		public T GetValue()
		{
			if (tTkEHXrLADjpKTdWlCTVajPNmVji == null)
			{
				throw new ArgumentNullException("getValueDelegate");
			}
			return tTkEHXrLADjpKTdWlCTVajPNmVji();
		}

		T IGetValue<T>.GetValue()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetValue
			return this.GetValue();
		}

		public void SetValue(T value)
		{
			if (uGTLwjEAEYMApcGiEhNuBIpazysQA == null)
			{
				throw new ArgumentNullException("setValueDelegate");
			}
			uGTLwjEAEYMApcGiEhNuBIpazysQA(value);
		}

		void ISetValue<T>.SetValue(T value)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetValue
			this.SetValue(value);
		}
	}
}
