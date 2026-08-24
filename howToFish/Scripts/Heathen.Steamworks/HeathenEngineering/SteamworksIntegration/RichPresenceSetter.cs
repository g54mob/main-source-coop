using HeathenEngineering.SteamworksIntegration.API;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	public class RichPresenceSetter : MonoBehaviour
	{
		public bool setOnEnable = true;

		public bool changeWithAppFocus;

		public StringKeyValuePair[] withFocus = new StringKeyValuePair[1]
		{
			new StringKeyValuePair
			{
				key = "steam_display",
				value = "#Status_AtMainMenu"
			}
		};

		public StringKeyValuePair[] withoutFocus;

		private void OnEnable()
		{
			if (App.Initialized)
			{
				if (setOnEnable)
				{
					if (Application.isFocused)
					{
						Set(withFocus);
					}
					else
					{
						Set(withoutFocus);
					}
				}
			}
			else
			{
				App.evtSteamInitialized.AddListener(DelayUpdate);
			}
			Application.focusChanged += Application_focusChanged;
		}

		private void DelayUpdate()
		{
			if (setOnEnable)
			{
				if (Application.isFocused)
				{
					Set(withFocus);
				}
				else
				{
					Set(withoutFocus);
				}
			}
			App.evtSteamInitialized.RemoveListener(DelayUpdate);
		}

		private void OnDisable()
		{
			Application.focusChanged -= Application_focusChanged;
		}

		private void Application_focusChanged(bool focused)
		{
			if (changeWithAppFocus)
			{
				if (focused)
				{
					Set(withFocus);
				}
				else
				{
					Set(withoutFocus);
				}
			}
		}

		public void Set(params StringKeyValuePair[] settings)
		{
			for (int i = 0; i < settings.Length; i++)
			{
				StringKeyValuePair stringKeyValuePair = settings[i];
				Friends.Client.SetRichPresence(stringKeyValuePair.key, stringKeyValuePair.value);
			}
		}

		public void Set(string key, string value)
		{
			Friends.Client.SetRichPresence(key, value);
		}

		public void Clear()
		{
			Friends.Client.ClearRichPresence();
		}
	}
}
