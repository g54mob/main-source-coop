using System;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.UI
{
	[AddComponentMenu("NomadDrive/UI/External Link Button")]
	public class ExternalLinkButton : MonoBehaviour
	{
		[Header("Link")]
		[Tooltip("Full URL to open. For the Steam store page use e.g. https://store.steampowered.com/app/<appId>/")]
		[SerializeField]
		private string url = "https://store.steampowered.com/";

		[Tooltip("Optional Steam App ID. When > 0 and Steam is running, opens the native store overlay for this app instead of a web overlay.")]
		[SerializeField]
		private uint steamAppId;

		[Header("Behaviour")]
		[Tooltip("Open inside the Steam overlay browser when Steam is available. Disable to always use the system browser.")]
		[SerializeField]
		private bool preferSteamOverlay = true;

		[Tooltip("Automatically subscribe to a Button on this GameObject. Leave off if you assign Open() manually in the Inspector.")]
		[SerializeField]
		private bool autoBindButton;

		private Button _button;

		private void Awake()
		{
			if (autoBindButton)
			{
				_button = GetComponent<Button>();
				if (_button != null)
				{
					_button.onClick.AddListener(Open);
				}
			}
		}

		private void OnDestroy()
		{
			if (_button != null)
			{
				_button.onClick.RemoveListener(Open);
			}
		}

		[ContextMenu("Open Link")]
		public void Open()
		{
			OpenUrl(url);
		}

		public void OpenUrl(string targetUrl)
		{
			if (!string.IsNullOrWhiteSpace(targetUrl) && (!preferSteamOverlay || !TryOpenInSteamOverlay(targetUrl)))
			{
				Application.OpenURL(targetUrl);
			}
		}

		private bool TryOpenInSteamOverlay(string targetUrl)
		{
			try
			{
				if (!SteamClient.IsValid)
				{
					return false;
				}
				if (steamAppId != 0)
				{
					SteamFriends.OpenStoreOverlay(new AppId
					{
						Value = steamAppId
					});
					return true;
				}
				SteamFriends.OpenWebOverlay(targetUrl);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
