using System;

namespace RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API
{
	public class SteamModel
	{
		public bool IsSteamInitialized { get; set; }

		public event Action OnInitialized;

		public void InvokeOnInitialized()
		{
			this.OnInitialized?.Invoke();
		}
	}
}
