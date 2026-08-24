using System.Collections;
using HeathenEngineering.SteamworksIntegration.API;
using TMPro;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration.UI
{
	[RequireComponent(typeof(TMP_InputField))]
	[HelpURL("https://kb.heathen.group/assets/steamworks/unity-engine/ui-components/set-user-id-input-field")]
	public class SetUserIdInputField : MonoBehaviour, IUserProfile
	{
		private TMP_InputField label;

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
			label = GetComponent<TMP_InputField>();
			StartCoroutine(DelayUpdate());
		}

		private IEnumerator DelayUpdate()
		{
			yield return new WaitUntil(() => App.Initialized);
			if (useLocalUser)
			{
				UserData id = User.Client.Id;
				Apply(id);
			}
		}

		public void Apply(UserData user)
		{
			if (label == null)
			{
				label = GetComponent<TMP_InputField>();
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
