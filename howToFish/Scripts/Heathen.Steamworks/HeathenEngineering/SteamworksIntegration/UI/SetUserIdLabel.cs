using System.Collections;
using HeathenEngineering.SteamworksIntegration.API;
using TMPro;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	[HelpURL("https://kb.heathen.group/assets/steamworks/unity-engine/ui-components/set-user-id-label")]
	public class SetUserIdLabel : MonoBehaviour, IUserProfile
	{
		private TextMeshProUGUI label;

		[SerializeField]
		[Tooltip("Should the component load the local user's name on Start.\nIf false you must call SetName and provide the ID of the user to load")]
		private bool useLocalUser;

		[SerializeField]
		private bool asHex;

		private UserData currentUser;

		public bool AsHex
		{
			get
			{
				return asHex;
			}
			set
			{
				asHex = value;
				Apply(currentUser);
			}
		}

		public UserData UserData
		{
			get
			{
				return currentUser;
			}
			set
			{
				Apply(value);
			}
		}

		private void Start()
		{
			label = GetComponent<TextMeshProUGUI>();
			StartCoroutine(DelayUpdate());
		}

		private IEnumerator DelayUpdate()
		{
			yield return new WaitUntil(() => App.Initialized);
			if (useLocalUser)
			{
				Apply(UserData.Me);
			}
		}

		public void Apply(UserData user)
		{
			if (label == null)
			{
				label = GetComponent<TextMeshProUGUI>();
			}
			if (!(label == null))
			{
				currentUser = user;
				if (asHex)
				{
					label.text = user.FriendId.ToString("X");
				}
				else
				{
					label.text = user.FriendId.ToString();
				}
			}
		}
	}
}
