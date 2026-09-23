using System;
using Mimicraft.Dev;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	[RequireComponent(typeof(NetworkObject))]
	public class LobbySettingsSync : NetworkBehaviour
	{
		[SerializeField]
		private LobbyInfoView infoView;

		public static LobbySettingsData? PendingSettings;

		private readonly NetworkVariable<LobbySettingsData> settings = new NetworkVariable<LobbySettingsData>();

		private readonly NetworkVariable<bool> cheats = new NetworkVariable<bool>(value: false);

		public static LobbySettingsSync Instance { get; private set; }

		public bool CheatsEnabled => cheats.Value;

		public LobbySettingsData CurrentSettings => settings.Value;

		public event Action<LobbySettingsData> SettingsChanged;

		public void ServerSetCheats(bool value)
		{
			if (base.IsServer)
			{
				cheats.Value = value;
			}
		}

		public void SetInfoView(LobbyInfoView infoView)
		{
			this.infoView = infoView;
		}

		public bool ServerApplySettings(LobbySettingsData proposed)
		{
			if (!base.IsServer)
			{
				return false;
			}
			LobbySettingsData value = settings.Value;
			proposed.ModeId = value.ModeId;
			proposed.MapId = value.MapId;
			proposed.HasPassword = value.HasPassword;
			if (proposed.Equals(value))
			{
				return true;
			}
			settings.Value = proposed;
			return true;
		}

		public override void OnNetworkSpawn()
		{
			Instance = this;
			NetworkVariable<LobbySettingsData> networkVariable = settings;
			networkVariable.OnValueChanged = (NetworkVariable<LobbySettingsData>.OnValueChangedDelegate)Delegate.Combine(networkVariable.OnValueChanged, new NetworkVariable<LobbySettingsData>.OnValueChangedDelegate(OnSettingsChanged));
			NetworkVariable<bool> networkVariable2 = cheats;
			networkVariable2.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Combine(networkVariable2.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(OnCheatsChanged));
			if (base.IsServer && PendingSettings.HasValue)
			{
				settings.Value = PendingSettings.Value;
				PendingSettings = null;
			}
			else if (base.IsServer)
			{
				Debug.LogWarning("[Oturum] LobbySettingsSync sunucuda devralinacak ayar bulamadi - mod ve harita varsayilanda kalacak.");
			}
			OnSettingsChanged(default(LobbySettingsData), settings.Value);
		}

		public override void OnNetworkDespawn()
		{
			if (Instance == this)
			{
				Instance = null;
			}
			NetworkVariable<LobbySettingsData> networkVariable = settings;
			networkVariable.OnValueChanged = (NetworkVariable<LobbySettingsData>.OnValueChangedDelegate)Delegate.Remove(networkVariable.OnValueChanged, new NetworkVariable<LobbySettingsData>.OnValueChangedDelegate(OnSettingsChanged));
			NetworkVariable<bool> networkVariable2 = cheats;
			networkVariable2.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Remove(networkVariable2.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(OnCheatsChanged));
		}

		private void OnCheatsChanged(bool previous, bool current)
		{
			string text = $"sv_cheats is now {(current ? 1 : 0)}.";
			DevConsole.Log(text);
			Debug.Log("[Session] " + text);
		}

		private void OnSettingsChanged(LobbySettingsData previous, LobbySettingsData current)
		{
			if (infoView != null)
			{
				infoView.SetSettings(current);
			}
			if (base.IsServer)
			{
				SteamLobbyData.Republish(current);
			}
			this.SettingsChanged?.Invoke(current);
		}

		protected override void __initializeVariables()
		{
			if (settings == null)
			{
				throw new Exception("LobbySettingsSync.settings cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			settings.Initialize(this);
			__nameNetworkVariable(settings, "settings");
			NetworkVariableFields.Add(settings);
			if (cheats == null)
			{
				throw new Exception("LobbySettingsSync.cheats cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			cheats.Initialize(this);
			__nameNetworkVariable(cheats, "cheats");
			NetworkVariableFields.Add(cheats);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			base.__initializeRpcs();
		}

		protected internal override string __getTypeName()
		{
			return "LobbySettingsSync";
		}
	}
}
