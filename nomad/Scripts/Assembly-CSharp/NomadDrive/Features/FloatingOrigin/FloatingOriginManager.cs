using System;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using NomadDrive.Features.Player;
using NomadDrive.Features.SaveSystem;
using NomadDrive.Features.WorldGeneration;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.FloatingOrigin
{
	[DefaultExecutionOrder(-500)]
	public class FloatingOriginManager : MonoBehaviour
	{
		[SerializeField]
		private FloatingOriginConfig config;

		[Inject]
		private IWorldGenerator _worldGenerator;

		[Inject]
		private IPlayerService _playerService;

		private static readonly HashSet<IFloatingOriginShiftable> Shiftables = new HashSet<IFloatingOriginShiftable>();

		private static readonly List<IFloatingOriginShiftable> ShiftableSnapshot = new List<IFloatingOriginShiftable>();

		private bool _ready;

		private const float ShiftCooldownSeconds = 0.5f;

		private float _shiftCooldownTimer;

		public static FloatingOriginManager Instance { get; private set; }

		public Vector3 TotalShift { get; private set; }

		public Transform WorldContentRoot { get; private set; }

		public Vector3 ServerShift
		{
			get
			{
				if (_worldGenerator == null)
				{
					return TotalShift;
				}
				return _worldGenerator.NetworkOriginShift;
			}
		}

		public event Action<Vector3> OnOriginShifted;

		public static Vector3 ToTrueWorld(Vector3 renderPos)
		{
			if (!(Instance != null))
			{
				return renderPos;
			}
			return renderPos - Instance.TotalShift;
		}

		public static float ToTrueWorldZ(float renderZ)
		{
			if (!(Instance != null))
			{
				return renderZ;
			}
			return renderZ - Instance.TotalShift.z;
		}

		public static Vector3 ToRenderWorld(Vector3 truePos)
		{
			if (!(Instance != null))
			{
				return truePos;
			}
			return truePos + Instance.TotalShift;
		}

		public Vector3 RenderToWorld(Vector3 renderPos)
		{
			return renderPos - TotalShift;
		}

		public Vector3 WorldToRender(Vector3 truePos)
		{
			return truePos + TotalShift;
		}

		public static void RegisterShiftable(IFloatingOriginShiftable s)
		{
			if (s != null)
			{
				Shiftables.Add(s);
			}
		}

		public static void UnregisterShiftable(IFloatingOriginShiftable s)
		{
			if (s != null)
			{
				Shiftables.Remove(s);
			}
		}

		private void Awake()
		{
			if (config == null || !config.enabled)
			{
				_ = config == null;
				base.enabled = false;
				return;
			}
			if (Instance != null && Instance != this)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			Instance = this;
			GameObject gameObject = new GameObject("WorldContentRoot");
			gameObject.transform.SetParent(null);
			gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
			WorldContentRoot = gameObject.transform;
			_ready = true;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
			if (WorldContentRoot != null)
			{
				UnityEngine.Object.Destroy(WorldContentRoot.gameObject);
			}
		}

		private void LateUpdate()
		{
			if (!_ready || GameSaveService.IsRestoringWorld)
			{
				return;
			}
			if (!IsLocalShiftAuthority())
			{
				if (_worldGenerator != null)
				{
					ApplyServerShift(_worldGenerator.NetworkOriginShift);
				}
			}
			else if (_shiftCooldownTimer > 0f)
			{
				_shiftCooldownTimer -= Time.unscaledDeltaTime;
			}
			else
			{
				if (!TryGetFocus(out var focus))
				{
					return;
				}
				float shiftThresholdMeters = config.shiftThresholdMeters;
				if (focus.x * focus.x + focus.z * focus.z < shiftThresholdMeters * shiftThresholdMeters)
				{
					return;
				}
				Vector3 vector = ComputeRebaseDelta(focus);
				if (!(vector == Vector3.zero))
				{
					ExecuteShift(vector);
					_shiftCooldownTimer = 0.5f;
					if (NetworkServer.active)
					{
						_worldGenerator?.ServerBroadcastOriginShift(TotalShift);
					}
				}
			}
		}

		private bool IsLocalShiftAuthority()
		{
			if (!NetworkServer.active && !NetworkClient.active)
			{
				return true;
			}
			if (!NetworkServer.active)
			{
				return false;
			}
			if (NetworkServer.connections.Count > 1)
			{
				return config.enabledInMultiplayer;
			}
			return true;
		}

		public void ApplyServerShift(Vector3 serverShift)
		{
			if (_ready)
			{
				Vector3 vector = serverShift - TotalShift;
				if (!(vector == Vector3.zero))
				{
					ExecuteShift(vector);
				}
			}
		}

		public void ApplyRestoreShift(Vector3 focusTrueWorldPos)
		{
			if (!_ready)
			{
				return;
			}
			Vector3 vector = ComputeRebaseDelta(focusTrueWorldPos) - TotalShift;
			if (!(vector == Vector3.zero))
			{
				ExecuteShift(vector, teleportLocalPlayer: false);
				if (NetworkServer.active)
				{
					_worldGenerator?.ServerBroadcastOriginShift(TotalShift);
				}
			}
		}

		private bool TryGetFocus(out Vector3 focus)
		{
			focus = default(Vector3);
			if (_playerService == null || !_playerService.IsPlayerSpawned)
			{
				return false;
			}
			NomadDrive.Features.Player.Player localPlayer = _playerService.LocalPlayer;
			if (localPlayer == null)
			{
				return false;
			}
			Transform seatedVehicleFocusTransform = localPlayer.SeatedVehicleFocusTransform;
			if (seatedVehicleFocusTransform != null)
			{
				focus = seatedVehicleFocusTransform.position;
				return true;
			}
			Transform downedFocusTransform = localPlayer.DownedFocusTransform;
			if (downedFocusTransform != null)
			{
				focus = downedFocusTransform.position;
				return true;
			}
			focus = localPlayer.transform.position;
			return true;
		}

		private Vector3 ComputeRebaseDelta(Vector3 focus)
		{
			float num = ((_worldGenerator != null) ? _worldGenerator.TileSize : 1024f);
			if (num <= 0f)
			{
				num = 1024f;
			}
			float num2 = Mathf.Round(focus.x / num) * num;
			float num3 = Mathf.Round(focus.z / num) * num;
			return new Vector3(0f - num2, 0f, 0f - num3);
		}

		private void ExecuteShift(Vector3 delta, bool teleportLocalPlayer = true)
		{
			Transform transform = _worldGenerator?.MapMagicRoot;
			if (transform != null)
			{
				transform.position += delta;
			}
			if (WorldContentRoot != null)
			{
				WorldContentRoot.position += delta;
			}
			if (teleportLocalPlayer && _playerService != null && _playerService.IsPlayerSpawned)
			{
				NomadDrive.Features.Player.Player localPlayer = _playerService.LocalPlayer;
				if (localPlayer != null)
				{
					if (localPlayer.IsPlayerSitting())
					{
						localPlayer.ShiftSeatedRootByDelta(delta);
					}
					else
					{
						localPlayer.SetPositionAndRotation(localPlayer.transform.position + delta, localPlayer.transform.eulerAngles, snapToGround: true);
					}
				}
			}
			ShiftableSnapshot.Clear();
			ShiftableSnapshot.AddRange(Shiftables);
			foreach (IFloatingOriginShiftable item in ShiftableSnapshot)
			{
				if (!(item is UnityEngine.Object obj) || !(obj == null))
				{
					try
					{
						item.OnOriginShift(delta);
					}
					catch (Exception arg)
					{
						EvilLogger.LogError($"[FloatingOrigin] Shiftable threw on rebase: {arg}", "ExecuteShift", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\FloatingOrigin\\Scripts\\FloatingOriginManager.cs", 383);
					}
				}
			}
			_worldGenerator?.ApplyOriginShift(delta);
			Physics.SyncTransforms();
			TotalShift += delta;
			this.OnOriginShifted?.Invoke(delta);
		}

		public string DebugInfo()
		{
			Vector3 focus;
			Vector3 vector = (TryGetFocus(out focus) ? focus : Vector3.zero);
			float num = ((_worldGenerator != null) ? _worldGenerator.TileSize : 1024f);
			Vector2Int vector2Int = new Vector2Int(Mathf.FloorToInt(vector.x / num), Mathf.FloorToInt(vector.z / num));
			Vector3 vector2 = ((_worldGenerator != null) ? _worldGenerator.NetworkOriginShift : Vector3.zero);
			return $"[FloatingOrigin] TotalShift={TotalShift} | serverShift={vector2} | focus render={vector} (|XZ|={new Vector2(vector.x, vector.z).magnitude:F0}m) " + $"| focus true={RenderToWorld(vector)} | tileCoord={vector2Int} | threshold={((config != null) ? config.shiftThresholdMeters : 0f)}m " + $"| authority={IsLocalShiftAuthority()} | mpEnabled={config != null && config.enabledInMultiplayer} | shiftables={Shiftables.Count}";
		}

		public void ForceShift()
		{
			if (_ready && TryGetFocus(out var focus))
			{
				Vector3 vector = ComputeRebaseDelta(focus);
				if (vector != Vector3.zero)
				{
					ExecuteShift(vector);
				}
			}
		}
	}
}
