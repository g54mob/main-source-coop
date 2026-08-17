using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Player.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace NomadDrive.Features.Player.Cosmetics
{
	public class PlayerCosmeticManager : NetworkBehaviour, IPlayerComponent
	{
		[Header("Configuration")]
		[SerializeField]
		private Transform cosmeticRoot;

		[SerializeField]
		private CosmeticSet[] availableSets;

		[Header("Default Setup")]
		[SerializeField]
		private string defaultFeetId;

		[SerializeField]
		private string defaultLowerBodyId;

		[SerializeField]
		private string defaultUpperBodyId;

		[SerializeField]
		private string defaultHeadId;

		[SerializeField]
		private List<string> defaultAccessoryIds = new List<string>();

		[SyncVar(hook = "OnFeetChanged")]
		private string _activeFeetId;

		[SyncVar(hook = "OnLowerBodyChanged")]
		private string _activeLowerBodyId;

		[SyncVar(hook = "OnUpperBodyChanged")]
		private string _activeUpperBodyId;

		[SyncVar(hook = "OnHeadChanged")]
		private string _activeHeadId;

		private readonly SyncList<string> _activeAccessoryIds = new SyncList<string>();

		private Dictionary<string, PlayerCosmetic> _cosmeticRegistry = new Dictionary<string, PlayerCosmetic>();

		private Dictionary<CosmeticCategory, List<PlayerCosmetic>> _cosmeticsByCategory = new Dictionary<CosmeticCategory, List<PlayerCosmetic>>();

		private bool _isInitialized;

		private bool _isLocalPlayer;

		private bool _isThirdPersonView;

		private static int _setAssignmentCounter;

		public Action<string, string> _Mirror_SyncVarHookDelegate__activeFeetId;

		public Action<string, string> _Mirror_SyncVarHookDelegate__activeLowerBodyId;

		public Action<string, string> _Mirror_SyncVarHookDelegate__activeUpperBodyId;

		public Action<string, string> _Mirror_SyncVarHookDelegate__activeHeadId;

		public int SetupPriority => 35;

		public Dictionary<string, PlayerCosmetic> CosmeticRegistry => _cosmeticRegistry;

		public Dictionary<CosmeticCategory, List<PlayerCosmetic>> CosmeticsByCategory => _cosmeticsByCategory;

		public string DefaultFeetId => defaultFeetId;

		public string DefaultLowerBodyId => defaultLowerBodyId;

		public string DefaultUpperBodyId => defaultUpperBodyId;

		public string DefaultHeadId => defaultHeadId;

		public List<string> DefaultAccessoryIds => defaultAccessoryIds;

		public string Network_activeFeetId
		{
			get
			{
				return _activeFeetId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _activeFeetId, 1uL, _Mirror_SyncVarHookDelegate__activeFeetId);
			}
		}

		public string Network_activeLowerBodyId
		{
			get
			{
				return _activeLowerBodyId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _activeLowerBodyId, 2uL, _Mirror_SyncVarHookDelegate__activeLowerBodyId);
			}
		}

		public string Network_activeUpperBodyId
		{
			get
			{
				return _activeUpperBodyId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _activeUpperBodyId, 4uL, _Mirror_SyncVarHookDelegate__activeUpperBodyId);
			}
		}

		public string Network_activeHeadId
		{
			get
			{
				return _activeHeadId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _activeHeadId, 8uL, _Mirror_SyncVarHookDelegate__activeHeadId);
			}
		}

		private void Awake()
		{
			ScanCosmetics();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			_isInitialized = true;
			if (availableSets != null && availableSets.Length != 0)
			{
				int num = _setAssignmentCounter % availableSets.Length;
				_setAssignmentCounter++;
				ApplySetOnServer(availableSets[num]);
			}
			else
			{
				AssignDefaults();
			}
			ApplyFullState();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			SyncList<string> activeAccessoryIds = _activeAccessoryIds;
			activeAccessoryIds.Callback = (Action<SyncList<string>.Operation, int, string, string>)Delegate.Combine(activeAccessoryIds.Callback, new Action<SyncList<string>.Operation, int, string, string>(OnAccessoryListChanged));
			if (!base.isServer)
			{
				_isInitialized = true;
				ApplyFullState();
			}
		}

		private void OnDestroy()
		{
			SyncList<string> activeAccessoryIds = _activeAccessoryIds;
			activeAccessoryIds.Callback = (Action<SyncList<string>.Operation, int, string, string>)Delegate.Remove(activeAccessoryIds.Callback, new Action<SyncList<string>.Operation, int, string, string>(OnAccessoryListChanged));
		}

		public void SetupForPlayer(bool isLocalPlayer)
		{
			_isLocalPlayer = isLocalPlayer;
			if (isLocalPlayer)
			{
				ApplyLocalVisibilityToAllActive();
			}
			base.enabled = false;
		}

		private void ScanCosmetics()
		{
			PlayerCosmetic[] componentsInChildren = ((cosmeticRoot != null) ? cosmeticRoot : base.transform).GetComponentsInChildren<PlayerCosmetic>(includeInactive: true);
			_cosmeticRegistry = new Dictionary<string, PlayerCosmetic>();
			_cosmeticsByCategory = new Dictionary<CosmeticCategory, List<PlayerCosmetic>>();
			PlayerCosmetic[] array = componentsInChildren;
			foreach (PlayerCosmetic playerCosmetic in array)
			{
				if (!string.IsNullOrEmpty(playerCosmetic.CosmeticId) && _cosmeticRegistry.TryAdd(playerCosmetic.CosmeticId, playerCosmetic))
				{
					if (!_cosmeticsByCategory.TryGetValue(playerCosmetic.Category, out var value))
					{
						value = new List<PlayerCosmetic>();
						_cosmeticsByCategory[playerCosmetic.Category] = value;
					}
					value.Add(playerCosmetic);
					playerCosmetic.SetActive(active: false);
				}
			}
		}

		private void ApplyFullState()
		{
			foreach (PlayerCosmetic value in _cosmeticRegistry.Values)
			{
				value.SetActive(active: false);
			}
			ActivateCosmetic(_activeFeetId);
			ActivateCosmetic(_activeLowerBodyId);
			ActivateCosmetic(_activeUpperBodyId);
			if (!string.IsNullOrEmpty(_activeHeadId))
			{
				ActivateCosmetic(_activeHeadId);
			}
			foreach (string activeAccessoryId in _activeAccessoryIds)
			{
				ActivateCosmetic(activeAccessoryId);
			}
		}

		private void ActivateCosmetic(string cosmeticId)
		{
			if (!string.IsNullOrEmpty(cosmeticId) && _cosmeticRegistry.TryGetValue(cosmeticId, out var value))
			{
				value.SetActive(active: true);
				ApplyLocalVisibility(value);
			}
		}

		private void DeactivateCosmetic(string cosmeticId)
		{
			if (!string.IsNullOrEmpty(cosmeticId) && _cosmeticRegistry.TryGetValue(cosmeticId, out var value))
			{
				value.SetActive(active: false);
			}
		}

		private void DeactivateAllInCategory(CosmeticCategory category)
		{
			if (!_cosmeticsByCategory.TryGetValue(category, out var value))
			{
				return;
			}
			foreach (PlayerCosmetic item in value)
			{
				item.SetActive(active: false);
			}
		}

		private void ApplyLocalVisibility(PlayerCosmetic cosmetic)
		{
			if (!_isLocalPlayer || !cosmetic.HideForLocalPlayer || _isThirdPersonView)
			{
				return;
			}
			Renderer[] componentsInChildren = cosmetic.GetComponentsInChildren<Renderer>(includeInactive: true);
			foreach (Renderer renderer in componentsInChildren)
			{
				if (renderer != null)
				{
					renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
				}
			}
		}

		public void ApplyLocalVisibilityToAllActive()
		{
			if (!_isLocalPlayer)
			{
				return;
			}
			foreach (PlayerCosmetic value in _cosmeticRegistry.Values)
			{
				if (value.gameObject.activeSelf && value.HideForLocalPlayer)
				{
					ApplyLocalVisibility(value);
				}
			}
		}

		private void AssignDefaults()
		{
			Network_activeFeetId = ((!string.IsNullOrEmpty(defaultFeetId) && _cosmeticRegistry.ContainsKey(defaultFeetId)) ? defaultFeetId : GetFirstCosmeticId(CosmeticCategory.Feet));
			Network_activeLowerBodyId = ((!string.IsNullOrEmpty(defaultLowerBodyId) && _cosmeticRegistry.ContainsKey(defaultLowerBodyId)) ? defaultLowerBodyId : GetFirstCosmeticId(CosmeticCategory.LowerBody));
			Network_activeUpperBodyId = ((!string.IsNullOrEmpty(defaultUpperBodyId) && _cosmeticRegistry.ContainsKey(defaultUpperBodyId)) ? defaultUpperBodyId : GetFirstCosmeticId(CosmeticCategory.UpperBody));
			Network_activeHeadId = ((!string.IsNullOrEmpty(defaultHeadId) && _cosmeticRegistry.ContainsKey(defaultHeadId)) ? defaultHeadId : "");
			_activeAccessoryIds.Clear();
			foreach (string defaultAccessoryId in defaultAccessoryIds)
			{
				if (!string.IsNullOrEmpty(defaultAccessoryId) && _cosmeticRegistry.ContainsKey(defaultAccessoryId))
				{
					_activeAccessoryIds.Add(defaultAccessoryId);
				}
			}
			if (string.IsNullOrEmpty(_activeFeetId))
			{
				EvilLogger.LogError("[PlayerCosmeticManager] No cosmetics found for required category: Feet", "AssignDefaults", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Cosmetics\\PlayerCosmeticManager.cs", 235);
			}
			if (string.IsNullOrEmpty(_activeLowerBodyId))
			{
				EvilLogger.LogError("[PlayerCosmeticManager] No cosmetics found for required category: LowerBody", "AssignDefaults", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Cosmetics\\PlayerCosmeticManager.cs", 237);
			}
			if (string.IsNullOrEmpty(_activeUpperBodyId))
			{
				EvilLogger.LogError("[PlayerCosmeticManager] No cosmetics found for required category: UpperBody", "AssignDefaults", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Cosmetics\\PlayerCosmeticManager.cs", 239);
			}
		}

		private string GetFirstCosmeticId(CosmeticCategory category)
		{
			if (_cosmeticsByCategory.TryGetValue(category, out var value) && value.Count > 0)
			{
				return value[0].CosmeticId;
			}
			return "";
		}

		private void ApplySetOnServer(CosmeticSet set)
		{
			if (!base.isServer)
			{
				return;
			}
			if (!string.IsNullOrEmpty(set.FeetCosmeticId))
			{
				Network_activeFeetId = set.FeetCosmeticId;
			}
			else
			{
				Network_activeFeetId = GetFirstCosmeticId(CosmeticCategory.Feet);
			}
			if (!string.IsNullOrEmpty(set.LowerBodyCosmeticId))
			{
				Network_activeLowerBodyId = set.LowerBodyCosmeticId;
			}
			else
			{
				Network_activeLowerBodyId = GetFirstCosmeticId(CosmeticCategory.LowerBody);
			}
			if (!string.IsNullOrEmpty(set.UpperBodyCosmeticId))
			{
				Network_activeUpperBodyId = set.UpperBodyCosmeticId;
			}
			else
			{
				Network_activeUpperBodyId = GetFirstCosmeticId(CosmeticCategory.UpperBody);
			}
			Network_activeHeadId = set.HeadCosmeticId ?? "";
			_activeAccessoryIds.Clear();
			if (set.AccessoryCosmeticIds == null)
			{
				return;
			}
			foreach (string accessoryCosmeticId in set.AccessoryCosmeticIds)
			{
				if (!string.IsNullOrEmpty(accessoryCosmeticId))
				{
					_activeAccessoryIds.Add(accessoryCosmeticId);
				}
			}
		}

		private void OnFeetChanged(string oldId, string newId)
		{
			if (_isInitialized)
			{
				DeactivateCosmetic(oldId);
				ActivateCosmetic(newId);
			}
		}

		private void OnLowerBodyChanged(string oldId, string newId)
		{
			if (_isInitialized)
			{
				DeactivateCosmetic(oldId);
				ActivateCosmetic(newId);
			}
		}

		private void OnUpperBodyChanged(string oldId, string newId)
		{
			if (_isInitialized)
			{
				DeactivateCosmetic(oldId);
				ActivateCosmetic(newId);
			}
		}

		private void OnHeadChanged(string oldId, string newId)
		{
			if (_isInitialized)
			{
				DeactivateCosmetic(oldId);
				if (!string.IsNullOrEmpty(newId))
				{
					ActivateCosmetic(newId);
				}
			}
		}

		private void OnAccessoryListChanged(SyncList<string>.Operation op, int index, string oldItem, string newItem)
		{
			if (_isInitialized)
			{
				switch (op)
				{
				case SyncList<string>.Operation.OP_ADD:
					ActivateCosmetic(newItem);
					break;
				case SyncList<string>.Operation.OP_REMOVEAT:
					DeactivateCosmetic(oldItem);
					break;
				case SyncList<string>.Operation.OP_CLEAR:
					DeactivateAllInCategory(CosmeticCategory.Accessory);
					break;
				case SyncList<string>.Operation.OP_SET:
				case SyncList<string>.Operation.OP_INSERT:
					break;
				}
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSetCosmetic(byte categoryByte, string cosmeticId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, categoryByte);
			writer.WriteString(cosmeticId);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Cosmetics.PlayerCosmeticManager::CmdSetCosmetic(System.Byte,System.String)", -1799967169, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdAddAccessory(string cosmeticId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(cosmeticId);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Cosmetics.PlayerCosmeticManager::CmdAddAccessory(System.String)", 542487482, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdRemoveAccessory(string cosmeticId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(cosmeticId);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Cosmetics.PlayerCosmeticManager::CmdRemoveAccessory(System.String)", -113564647, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdApplySet(string feetId, string lowerBodyId, string upperBodyId, string headId, string[] accessoryIds)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(feetId);
			writer.WriteString(lowerBodyId);
			writer.WriteString(upperBodyId);
			writer.WriteString(headId);
			GeneratedNetworkCode._Write_System_002EString_005B_005D(writer, accessoryIds);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Cosmetics.PlayerCosmeticManager::CmdApplySet(System.String,System.String,System.String,System.String,System.String[])", -1848837817, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private bool ValidateCosmetic(string cosmeticId, CosmeticCategory expectedCategory)
		{
			if (!_cosmeticRegistry.TryGetValue(cosmeticId, out var value))
			{
				return false;
			}
			if (value.Category != expectedCategory)
			{
				return false;
			}
			return true;
		}

		public void ApplySet(CosmeticSet set)
		{
			if (set == null)
			{
				return;
			}
			if (base.isServer)
			{
				ApplySetOnServer(set);
				ApplyFullState();
				return;
			}
			string[] array;
			if (set.AccessoryCosmeticIds != null && set.AccessoryCosmeticIds.Count > 0)
			{
				array = new string[set.AccessoryCosmeticIds.Count];
				for (int i = 0; i < set.AccessoryCosmeticIds.Count; i++)
				{
					array[i] = set.AccessoryCosmeticIds[i];
				}
			}
			else
			{
				array = Array.Empty<string>();
			}
			CmdApplySet(set.FeetCosmeticId, set.LowerBodyCosmeticId, set.UpperBodyCosmeticId, set.HeadCosmeticId ?? "", array);
		}

		public void SetCosmetic(CosmeticCategory category, string cosmeticId)
		{
			if (category != CosmeticCategory.Accessory)
			{
				CmdSetCosmetic((byte)category, cosmeticId);
			}
		}

		public void AddAccessory(string cosmeticId)
		{
			CmdAddAccessory(cosmeticId);
		}

		public void RemoveAccessory(string cosmeticId)
		{
			CmdRemoveAccessory(cosmeticId);
		}

		public string GetActiveCosmetic(CosmeticCategory category)
		{
			return category switch
			{
				CosmeticCategory.Feet => _activeFeetId, 
				CosmeticCategory.LowerBody => _activeLowerBodyId, 
				CosmeticCategory.UpperBody => _activeUpperBodyId, 
				CosmeticCategory.Head => _activeHeadId, 
				_ => null, 
			};
		}

		public IReadOnlyList<string> GetActiveAccessories()
		{
			return _activeAccessoryIds;
		}

		public IReadOnlyList<PlayerCosmetic> GetAvailableCosmetics(CosmeticCategory category)
		{
			if (_cosmeticsByCategory.TryGetValue(category, out var value))
			{
				return value;
			}
			return Array.Empty<PlayerCosmetic>();
		}

		public CosmeticSet[] GetAvailableSets()
		{
			return availableSets ?? Array.Empty<CosmeticSet>();
		}

		public void SetThirdPersonView(bool isThirdPerson)
		{
			_isThirdPersonView = isThirdPerson;
		}

		public void RestoreLocalVisibility()
		{
			if (!_isLocalPlayer)
			{
				return;
			}
			foreach (PlayerCosmetic value in _cosmeticRegistry.Values)
			{
				if (!value.gameObject.activeSelf || !value.HideForLocalPlayer)
				{
					continue;
				}
				Renderer[] componentsInChildren = value.GetComponentsInChildren<Renderer>(includeInactive: true);
				foreach (Renderer renderer in componentsInChildren)
				{
					if (renderer != null)
					{
						renderer.shadowCastingMode = ShadowCastingMode.On;
					}
				}
			}
		}

		public void EditorScanCosmetics()
		{
			ScanCosmetics();
		}

		public void EditorRescanAndRestore()
		{
			ScanCosmetics();
			if (Application.isPlaying)
			{
				ApplyFullState();
			}
		}

		public void EditorSetDefault(CosmeticCategory category, string cosmeticId)
		{
			switch (category)
			{
			case CosmeticCategory.Feet:
				defaultFeetId = cosmeticId;
				break;
			case CosmeticCategory.LowerBody:
				defaultLowerBodyId = cosmeticId;
				break;
			case CosmeticCategory.UpperBody:
				defaultUpperBodyId = cosmeticId;
				break;
			case CosmeticCategory.Head:
				defaultHeadId = cosmeticId ?? "";
				break;
			}
		}

		public void EditorToggleAccessory(string cosmeticId)
		{
			if (defaultAccessoryIds.Contains(cosmeticId))
			{
				defaultAccessoryIds.Remove(cosmeticId);
			}
			else
			{
				defaultAccessoryIds.Add(cosmeticId);
			}
		}

		public void EditorApplyPreview()
		{
			foreach (PlayerCosmetic value in _cosmeticRegistry.Values)
			{
				value.SetActive(active: false);
			}
			if (!string.IsNullOrEmpty(defaultFeetId) && _cosmeticRegistry.ContainsKey(defaultFeetId))
			{
				_cosmeticRegistry[defaultFeetId].SetActive(active: true);
			}
			if (!string.IsNullOrEmpty(defaultLowerBodyId) && _cosmeticRegistry.ContainsKey(defaultLowerBodyId))
			{
				_cosmeticRegistry[defaultLowerBodyId].SetActive(active: true);
			}
			if (!string.IsNullOrEmpty(defaultUpperBodyId) && _cosmeticRegistry.ContainsKey(defaultUpperBodyId))
			{
				_cosmeticRegistry[defaultUpperBodyId].SetActive(active: true);
			}
			if (!string.IsNullOrEmpty(defaultHeadId) && _cosmeticRegistry.ContainsKey(defaultHeadId))
			{
				_cosmeticRegistry[defaultHeadId].SetActive(active: true);
			}
			foreach (string defaultAccessoryId in defaultAccessoryIds)
			{
				if (!string.IsNullOrEmpty(defaultAccessoryId) && _cosmeticRegistry.ContainsKey(defaultAccessoryId))
				{
					_cosmeticRegistry[defaultAccessoryId].SetActive(active: true);
				}
			}
		}

		public string EditorGetDefault(CosmeticCategory category)
		{
			return category switch
			{
				CosmeticCategory.Feet => defaultFeetId, 
				CosmeticCategory.LowerBody => defaultLowerBodyId, 
				CosmeticCategory.UpperBody => defaultUpperBodyId, 
				CosmeticCategory.Head => defaultHeadId, 
				_ => null, 
			};
		}

		public PlayerCosmeticManager()
		{
			InitSyncObject(_activeAccessoryIds);
			_Mirror_SyncVarHookDelegate__activeFeetId = OnFeetChanged;
			_Mirror_SyncVarHookDelegate__activeLowerBodyId = OnLowerBodyChanged;
			_Mirror_SyncVarHookDelegate__activeUpperBodyId = OnUpperBodyChanged;
			_Mirror_SyncVarHookDelegate__activeHeadId = OnHeadChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetCosmetic__Byte__String(byte categoryByte, string cosmeticId)
		{
			CosmeticCategory cosmeticCategory = (CosmeticCategory)categoryByte;
			if (cosmeticCategory != CosmeticCategory.Accessory && ((cosmeticCategory != CosmeticCategory.Feet && cosmeticCategory != CosmeticCategory.LowerBody && cosmeticCategory != CosmeticCategory.UpperBody) || !string.IsNullOrEmpty(cosmeticId)) && (string.IsNullOrEmpty(cosmeticId) || ValidateCosmetic(cosmeticId, cosmeticCategory)))
			{
				switch (cosmeticCategory)
				{
				case CosmeticCategory.Feet:
					Network_activeFeetId = cosmeticId;
					break;
				case CosmeticCategory.LowerBody:
					Network_activeLowerBodyId = cosmeticId;
					break;
				case CosmeticCategory.UpperBody:
					Network_activeUpperBodyId = cosmeticId;
					break;
				case CosmeticCategory.Head:
					Network_activeHeadId = cosmeticId ?? "";
					break;
				}
			}
		}

		protected static void InvokeUserCode_CmdSetCosmetic__Byte__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetCosmetic called on client.");
			}
			else
			{
				((PlayerCosmeticManager)obj).UserCode_CmdSetCosmetic__Byte__String(NetworkReaderExtensions.ReadByte(reader), reader.ReadString());
			}
		}

		protected void UserCode_CmdAddAccessory__String(string cosmeticId)
		{
			if (!string.IsNullOrEmpty(cosmeticId) && !_activeAccessoryIds.Contains(cosmeticId) && ValidateCosmetic(cosmeticId, CosmeticCategory.Accessory))
			{
				_activeAccessoryIds.Add(cosmeticId);
			}
		}

		protected static void InvokeUserCode_CmdAddAccessory__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdAddAccessory called on client.");
			}
			else
			{
				((PlayerCosmeticManager)obj).UserCode_CmdAddAccessory__String(reader.ReadString());
			}
		}

		protected void UserCode_CmdRemoveAccessory__String(string cosmeticId)
		{
			if (!string.IsNullOrEmpty(cosmeticId))
			{
				_activeAccessoryIds.Remove(cosmeticId);
			}
		}

		protected static void InvokeUserCode_CmdRemoveAccessory__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRemoveAccessory called on client.");
			}
			else
			{
				((PlayerCosmeticManager)obj).UserCode_CmdRemoveAccessory__String(reader.ReadString());
			}
		}

		protected void UserCode_CmdApplySet__String__String__String__String__String_005B_005D(string feetId, string lowerBodyId, string upperBodyId, string headId, string[] accessoryIds)
		{
			if (string.IsNullOrEmpty(feetId) || string.IsNullOrEmpty(lowerBodyId) || string.IsNullOrEmpty(upperBodyId))
			{
				return;
			}
			Network_activeFeetId = feetId;
			Network_activeLowerBodyId = lowerBodyId;
			Network_activeUpperBodyId = upperBodyId;
			Network_activeHeadId = headId ?? "";
			_activeAccessoryIds.Clear();
			if (accessoryIds == null)
			{
				return;
			}
			foreach (string text in accessoryIds)
			{
				if (!string.IsNullOrEmpty(text))
				{
					_activeAccessoryIds.Add(text);
				}
			}
		}

		protected static void InvokeUserCode_CmdApplySet__String__String__String__String__String_005B_005D(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdApplySet called on client.");
			}
			else
			{
				((PlayerCosmeticManager)obj).UserCode_CmdApplySet__String__String__String__String__String_005B_005D(reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadString(), GeneratedNetworkCode._Read_System_002EString_005B_005D(reader));
			}
		}

		static PlayerCosmeticManager()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerCosmeticManager), "System.Void NomadDrive.Features.Player.Cosmetics.PlayerCosmeticManager::CmdSetCosmetic(System.Byte,System.String)", InvokeUserCode_CmdSetCosmetic__Byte__String, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerCosmeticManager), "System.Void NomadDrive.Features.Player.Cosmetics.PlayerCosmeticManager::CmdAddAccessory(System.String)", InvokeUserCode_CmdAddAccessory__String, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerCosmeticManager), "System.Void NomadDrive.Features.Player.Cosmetics.PlayerCosmeticManager::CmdRemoveAccessory(System.String)", InvokeUserCode_CmdRemoveAccessory__String, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerCosmeticManager), "System.Void NomadDrive.Features.Player.Cosmetics.PlayerCosmeticManager::CmdApplySet(System.String,System.String,System.String,System.String,System.String[])", InvokeUserCode_CmdApplySet__String__String__String__String__String_005B_005D, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteString(_activeFeetId);
				writer.WriteString(_activeLowerBodyId);
				writer.WriteString(_activeUpperBodyId);
				writer.WriteString(_activeHeadId);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteString(_activeFeetId);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteString(_activeLowerBodyId);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteString(_activeUpperBodyId);
			}
			if ((syncVarDirtyBits & 8L) != 0L)
			{
				writer.WriteString(_activeHeadId);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _activeFeetId, _Mirror_SyncVarHookDelegate__activeFeetId, reader.ReadString());
				GeneratedSyncVarDeserialize(ref _activeLowerBodyId, _Mirror_SyncVarHookDelegate__activeLowerBodyId, reader.ReadString());
				GeneratedSyncVarDeserialize(ref _activeUpperBodyId, _Mirror_SyncVarHookDelegate__activeUpperBodyId, reader.ReadString());
				GeneratedSyncVarDeserialize(ref _activeHeadId, _Mirror_SyncVarHookDelegate__activeHeadId, reader.ReadString());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _activeFeetId, _Mirror_SyncVarHookDelegate__activeFeetId, reader.ReadString());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _activeLowerBodyId, _Mirror_SyncVarHookDelegate__activeLowerBodyId, reader.ReadString());
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _activeUpperBodyId, _Mirror_SyncVarHookDelegate__activeUpperBodyId, reader.ReadString());
			}
			if ((num & 8L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _activeHeadId, _Mirror_SyncVarHookDelegate__activeHeadId, reader.ReadString());
			}
		}
	}
}
