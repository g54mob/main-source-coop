using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[RequireComponent(typeof(RawImage))]
	public class SetUserAvatar : MonoBehaviour
	{
		private RawImage image;

		[SerializeField]
		[Tooltip("Should the component load the local user's avatar on Start.\nIf false you must call LoadAvatar and provide the ID of the user to load")]
		private bool useLocalUser;

		public UnityEvent evtLoaded;

		private UserData currentUser;

		public UserData UserData
		{
			get
			{
				return currentUser;
			}
			set
			{
				LoadAvatar(value);
			}
		}

		private void OnEnable()
		{
			Friends.Client.EventPersonaStateChange.AddListener(HandlePersonaStateChange);
		}

		private void OnDisable()
		{
			Friends.Client.EventPersonaStateChange.RemoveListener(HandlePersonaStateChange);
		}

		private void Start()
		{
			image = GetComponent<RawImage>();
			if (useLocalUser)
			{
				if (App.Initialized)
				{
					LoadAvatar(UserData.Me);
				}
				else
				{
					App.evtSteamInitialized.AddListener(HandleSteamInitalized);
				}
			}
		}

		private void HandleSteamInitalized()
		{
			if (useLocalUser)
			{
				LoadAvatar(UserData.Me);
			}
			App.evtSteamInitialized.RemoveListener(HandleSteamInitalized);
		}

		private void HandlePersonaStateChange(PersonaStateChange arg)
		{
			if (!Friends.Client.PersonaChangeHasFlag(arg.Flags, EPersonaChange.k_EPersonaChangeAvatar) || !(arg.SubjectId == currentUser))
			{
				return;
			}
			UserData userData = arg.SubjectId;
			if (userData == currentUser)
			{
				userData.LoadAvatar(delegate(Texture2D t)
				{
					image.texture = t;
					evtLoaded?.Invoke();
				});
			}
		}

		public void LoadAvatar(UserData user)
		{
			user.LoadAvatar(delegate(Texture2D r)
			{
				if (image == null)
				{
					image = GetComponent<RawImage>();
				}
				if (!(image == null))
				{
					currentUser = user;
					image.texture = r;
					evtLoaded?.Invoke();
				}
			});
		}

		public void LoadAvatar(CSteamID user)
		{
			UserData.Get(user).LoadAvatar(delegate(Texture2D r)
			{
				if (image == null)
				{
					image = GetComponent<RawImage>();
				}
				if (!(image == null))
				{
					currentUser = user;
					image.texture = r;
					evtLoaded?.Invoke();
				}
			});
		}

		public void LoadAvatar(ulong user)
		{
			UserData.Get(user).LoadAvatar(delegate(Texture2D r)
			{
				if (image == null)
				{
					image = GetComponent<RawImage>();
				}
				if (!(image == null))
				{
					currentUser = user;
					image.texture = r;
					evtLoaded?.Invoke();
				}
			});
		}
	}
}
