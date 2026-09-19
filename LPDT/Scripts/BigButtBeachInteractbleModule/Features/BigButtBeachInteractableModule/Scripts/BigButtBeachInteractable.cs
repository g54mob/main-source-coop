using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.CameraModelModule;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.UsersStatsModule.Scripts;
using Fusion;
using Global.SerializableDictionary;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(4)]
	public class BigButtBeachInteractable : NetworkBehaviour
	{
		[SerializeField]
		private Transform _centerPoint;

		[SerializeField]
		private Transform _parentSpine;

		[SerializeField]
		private List<SimplePointGrabable> _simplePointGrabable;

		[SerializeField]
		private ButtEventProgressView _progressView;

		[SerializeField]
		private string _statAPIString;

		[SerializeField]
		private int _historyDays = 20;

		[SerializeField]
		private long _maxValue = 1000000L;

		[Range(0f, 1f)]
		[SerializeField]
		private float _startValueRatio = 0.8f;

		[Header("Refresh")]
		[SerializeField]
		private float _refreshIntervalSeconds = 600f;

		[Header("Animation")]
		[Tooltip("How long the counter takes to lerp from the current displayed value to a higher Steam snapshot.")]
		[SerializeField]
		private float _steamLerpDuration = 600f;

		[Tooltip("How long the counter takes to catch up after a local +1 increment (instant feedback).")]
		[SerializeField]
		private float _localGrabLerpDuration = 0.35f;

		[Header("Counter Popup")]
		[SerializeField]
		private List<BigButtBeachCounterPopupSpawnZone> _counterPopupSpawnZones = new List<BigButtBeachCounterPopupSpawnZone>();

		[Header("Grab Click Popup")]
		[SerializeField]
		private Global.SerializableDictionary.SerializableDictionary<Transform, BigButtBeachCounterPopupSpawnZone> _grabClickPopupSpawnZonesByHandle = new Global.SerializableDictionary.SerializableDictionary<Transform, BigButtBeachCounterPopupSpawnZone>();

		[Header("Noise")]
		[Tooltip("Positive burst amount as a fraction of the Steam lerp range. Bursts are capped by Max Noise Lead Seconds.")]
		[Range(0f, 0.5f)]
		[SerializeField]
		private float _noiseAmplitude = 0.06f;

		[Tooltip("How fast the fake progression wobbles (Perlin samples per second). Higher = more frequent bursts.")]
		[SerializeField]
		private float _noiseFrequency = 0.15f;

		[Tooltip("Maximum time the noisy value may run ahead of the linear baseline. Lower values keep bursts small and prevent long pauses after a burst.")]
		[SerializeField]
		private float _maxNoiseLeadSeconds = 0.75f;

		[Header("Milestone")]
		[Tooltip("When the Steam global crosses a million but the computed start value lands below it (e.g. 0.8 * 1_100_000), the displayed start is pulled up to this value instead.")]
		[SerializeField]
		private long _millionMilestoneStartValue = 1004235L;

		[Tooltip("Displayed value at which the fireworks burst plays once.")]
		[SerializeField]
		private long _fireworksTriggerValue = 100000L;

		private MultiplayerModel _multiplayerModel;

		private CameraModel _cameraModel;

		private IUserStatsService _userStatsService;

		private BigButtBeachCounterProgressModel _counterProgressModel;

		private ISavingService _savingService;

		private LineArmsModel _lineArmsModel;

		private BigButtBeachCounterChangedEventClass _counterChangedEventClass;

		private BigButtBeachGrabClickedEventClass _grabClickedEventClass;

		private BigButtBeachExternalGrabEventClass _externalGrabEventClass;

		private readonly Dictionary<SimplePointGrabable, Action<int>> _localGrabHandlers = new Dictionary<SimplePointGrabable, Action<int>>();

		private float _refreshTimer;

		private Vector3 _startParentSpinePosition;

		private Vector3 _startCenterPointPosition;

		private bool _hasStartSpinePosition;

		private long _steamMaxValue;

		private double _displayBaseValue;

		private double _displayTargetValue;

		private float _displayLerpStartTime;

		private float _displayLerpDuration;

		private double _localOffsetBaseValue;

		private double _localOffsetTargetValue;

		private float _localOffsetLerpStartTime;

		private float _localOffsetLerpDuration;

		private long _lastDisplayedValue;

		private long _lastCounterProgressValue;

		private double _steamCurveFloor;

		private float _noiseSeed;

		[WeaverGenerated]
		[DefaultForProperty("CurrentValue", 0, 2)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private long _CurrentValue;

		[WeaverGenerated]
		[DefaultForProperty("CurrentFakeValue", 2, 2)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private long _CurrentFakeValue;

		[Networked]
		[NetworkedWeaved(0, 2)]
		private unsafe long CurrentValue
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BigButtBeachInteractable.CurrentValue. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(long*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BigButtBeachInteractable.CurrentValue. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(long*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 2)]
		private unsafe long CurrentFakeValue
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BigButtBeachInteractable.CurrentFakeValue. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((long*)Ptr)[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BigButtBeachInteractable.CurrentFakeValue. Networked properties can only be accessed when Spawned() has been called.");
				}
				((long*)Ptr)[1] = value;
			}
		}

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel, CameraModel cameraModel, IUserStatsService userStatsService, BigButtBeachCounterProgressModel counterProgressModel, ISavingService savingService, LineArmsModel lineArmsModel, BigButtBeachCounterChangedEventClass counterChangedEventClass, BigButtBeachGrabClickedEventClass grabClickedEventClass, BigButtBeachExternalGrabEventClass externalGrabEventClass)
		{
			_multiplayerModel = multiplayerModel;
			_cameraModel = cameraModel;
			_userStatsService = userStatsService;
			_counterProgressModel = counterProgressModel;
			_savingService = savingService;
			_lineArmsModel = lineArmsModel;
			_counterChangedEventClass = counterChangedEventClass;
			_grabClickedEventClass = grabClickedEventClass;
			_externalGrabEventClass = externalGrabEventClass;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.HasStateAuthority)
			{
				UpdateNetworkedCurrentValue(force: true);
			}
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			_progressView.ActiveCamera = _cameraModel.CameraObject;
			foreach (SimplePointGrabable item in _simplePointGrabable)
			{
				if (!(item == null) && !_localGrabHandlers.ContainsKey(item))
				{
					SimplePointGrabable capturedGrabable = item;
					Action<int> value = delegate(int id)
					{
						HandleLocalGrab(capturedGrabable, id);
					};
					_localGrabHandlers[capturedGrabable] = value;
					capturedGrabable.LocalOnGrab += value;
				}
			}
			_userStatsService.OnGlobalStatsReceived += HandleGlobalStatsReceived;
			_externalGrabEventClass.OnExternalGrab += HandleExternalGrab;
			_noiseSeed = UnityEngine.Random.value * 1000f;
			ResetLocalOffset();
			RestoreProgress();
			_refreshTimer = _refreshIntervalSeconds;
			_lastDisplayedValue = -1L;
			_lastCounterProgressValue = -1L;
			UpdateCounter();
			if (IsCounterAuthority())
			{
				RequestRefresh();
			}
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			SaveProgress();
			foreach (KeyValuePair<SimplePointGrabable, Action<int>> localGrabHandler in _localGrabHandlers)
			{
				if (localGrabHandler.Key != null)
				{
					localGrabHandler.Key.LocalOnGrab -= localGrabHandler.Value;
				}
			}
			_localGrabHandlers.Clear();
			_userStatsService.OnGlobalStatsReceived -= HandleGlobalStatsReceived;
			_externalGrabEventClass.OnExternalGrab -= HandleExternalGrab;
		}

		private void Update()
		{
			if (IsCounterAuthority())
			{
				_refreshTimer -= Time.deltaTime;
				if (_refreshTimer <= 0f)
				{
					_refreshTimer = _refreshIntervalSeconds;
					RequestRefresh();
				}
				UpdateNetworkedCurrentValue();
			}
			UpdateCounter();
		}

		private void LateUpdate()
		{
			UpdateCenterPointPosition();
		}

		private void HandleLocalGrab(SimplePointGrabable simplePointGrabable, int id)
		{
			if (!(_multiplayerModel.NetworkRunner == null))
			{
				int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
				if (id == playerId)
				{
					HandleGrabClicked(simplePointGrabable, id);
					OnLocalGrab();
				}
			}
		}

		private void HandleExternalGrab()
		{
			OnLocalGrab();
		}

		private void HandleGrabClicked(SimplePointGrabable simplePointGrabable, int playerId)
		{
			_grabClickedEventClass?.InvokeGrabClicked(this, simplePointGrabable, playerId);
		}

		private void OnLocalGrab()
		{
			_userStatsService.IncrementUserStat(_statAPIString);
			if (!HasValidNetworkObject() || IsCounterAuthority())
			{
				ApplyAuthoritativeIncrement();
			}
			else
			{
				RpcRequestIncrementCurrentValue();
			}
		}

		private void ApplyAuthoritativeIncrement()
		{
			SetLocalOffsetTarget(GetLocalOffsetValue() + 1.0, _localGrabLerpDuration);
			UpdateNetworkedCurrentValue(force: true);
			SaveProgress();
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 1381359898u)]
		private void RpcRequestIncrementCurrentValue()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1381359898u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BigButtBeachInteractableModule.Scripts.BigButtBeachInteractable::RpcRequestIncrementCurrentValue()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (IsCounterAuthority())
			{
				ApplyAuthoritativeIncrement();
			}
		}

		private bool IsCounterAuthority()
		{
			if (HasValidNetworkObject())
			{
				return base.HasStateAuthority;
			}
			if (!(_multiplayerModel.NetworkRunner == null))
			{
				return _multiplayerModel.NetworkRunner.IsSharedModeMasterClient;
			}
			return true;
		}

		private void HandleGlobalStatsReceived()
		{
			if (IsCounterAuthority() && _userStatsService.TryGetGlobalStat(_statAPIString, out var pData))
			{
				if (pData <= _steamMaxValue)
				{
					UpdateNetworkedCurrentValue(force: true);
				}
				else
				{
					StartNewSteamMaxPipeline(pData);
				}
			}
		}

		private void RequestRefresh()
		{
			_userStatsService.RequestGlobalStats(_historyDays);
		}

		private void RestoreProgress()
		{
			if (!_counterProgressModel.TryGetProgress(_statAPIString, out var currentValue, out var steamMaxValue, out var savedUtcTicks))
			{
				_steamMaxValue = 0L;
				SnapDisplay(0.0);
				return;
			}
			_steamMaxValue = steamMaxValue;
			double offlineAdvancedValue = GetOfflineAdvancedValue(currentValue, steamMaxValue, savedUtcTicks);
			if (offlineAdvancedValue >= (double)steamMaxValue)
			{
				SnapDisplay(offlineAdvancedValue);
				return;
			}
			float remainingLerpDuration = GetRemainingLerpDuration(savedUtcTicks);
			BeginDisplayLerp(offlineAdvancedValue, steamMaxValue, remainingLerpDuration);
		}

		private void StartNewSteamMaxPipeline(long steamMaxValue)
		{
			_steamMaxValue = steamMaxValue;
			double num = Math.Max(GetDisplayedValue(), (float)steamMaxValue * Mathf.Clamp01(_startValueRatio));
			if (steamMaxValue > _maxValue && num < (double)_maxValue)
			{
				num = _millionMilestoneStartValue;
			}
			ResetLocalOffset();
			if (num >= (double)steamMaxValue)
			{
				SnapDisplay(num);
				SaveProgress();
				UpdateNetworkedCurrentValue(force: true);
			}
			else
			{
				BeginDisplayLerp(num, steamMaxValue, _steamLerpDuration);
				SaveProgress();
				UpdateNetworkedCurrentValue(force: true);
			}
		}

		private void SetLocalOffsetTarget(double targetValue, float duration)
		{
			double localOffsetValue = GetLocalOffsetValue();
			if (!(targetValue <= localOffsetValue))
			{
				BeginLocalOffsetLerp(localOffsetValue, targetValue, duration);
			}
		}

		private void BeginDisplayLerp(double startValue, double targetValue, float duration)
		{
			_displayBaseValue = startValue;
			_displayTargetValue = targetValue;
			_displayLerpStartTime = Time.time;
			_displayLerpDuration = Mathf.Max(0f, duration);
			_steamCurveFloor = startValue;
		}

		private void BeginLocalOffsetLerp(double startValue, double targetValue, float duration)
		{
			_localOffsetBaseValue = startValue;
			_localOffsetTargetValue = targetValue;
			_localOffsetLerpStartTime = Time.time;
			_localOffsetLerpDuration = Mathf.Max(0f, duration);
		}

		private double GetDisplayedValue()
		{
			return GetSteamCurveValue() + GetLocalOffsetValue();
		}

		private double GetSteamCurveValue()
		{
			if (_displayLerpDuration <= 0f)
			{
				return _displayTargetValue;
			}
			float num = Time.time - _displayLerpStartTime;
			float num2 = Mathf.Clamp01(num / _displayLerpDuration);
			double num3 = _displayTargetValue - _displayBaseValue;
			double num4 = _displayBaseValue + num3 * (double)num2;
			double num5 = 4.0 * (double)num2 * (double)(1f - num2);
			float b = (Mathf.PerlinNoise(_noiseSeed, num * _noiseFrequency) - 0.5f) * 2f;
			double val = num3 * (double)_noiseAmplitude * num5 * (double)Mathf.Max(0f, b);
			double val2 = num3 / (double)_displayLerpDuration * (double)Mathf.Max(0f, _maxNoiseLeadSeconds);
			double val3 = num4 + Math.Min(val, val2);
			val3 = Math.Min(val3, _displayTargetValue);
			if (val3 < _steamCurveFloor)
			{
				val3 = _steamCurveFloor;
			}
			_steamCurveFloor = val3;
			return val3;
		}

		private double GetLocalOffsetValue()
		{
			if (_localOffsetLerpDuration <= 0f)
			{
				return _localOffsetTargetValue;
			}
			float num = Mathf.Clamp01((Time.time - _localOffsetLerpStartTime) / _localOffsetLerpDuration);
			return _localOffsetBaseValue + (_localOffsetTargetValue - _localOffsetBaseValue) * (double)num;
		}

		private void SnapDisplay(double value)
		{
			BeginDisplayLerp(value, value, 0f);
		}

		private void ResetLocalOffset()
		{
			BeginLocalOffsetLerp(0.0, 0.0, 0f);
		}

		private void UpdateCounter()
		{
			long currentValueForDisplay = GetCurrentValueForDisplay();
			if (currentValueForDisplay != _lastDisplayedValue)
			{
				long lastDisplayedValue = _lastDisplayedValue;
				_lastDisplayedValue = currentValueForDisplay;
				_progressView.SetProgress(currentValueForDisplay, _maxValue);
				_progressView.SetCompletedStatus(currentValueForDisplay >= _maxValue);
				if (_fireworksTriggerValue > 0 && lastDisplayedValue >= 0 && currentValueForDisplay / _fireworksTriggerValue > lastDisplayedValue / _fireworksTriggerValue)
				{
					_progressView.TriggerFireworks();
				}
			}
			UpdateCounterProgressEvent();
		}

		private void UpdateCenterPointPosition()
		{
			if (!(_centerPoint == null) && !(_parentSpine == null))
			{
				if (!_hasStartSpinePosition)
				{
					_startParentSpinePosition = _parentSpine.position;
					_startCenterPointPosition = _centerPoint.position;
					_hasStartSpinePosition = true;
				}
				Vector3 vector = _parentSpine.position - _startParentSpinePosition;
				_centerPoint.position = _startCenterPointPosition + vector;
			}
		}

		private void UpdateCounterProgressEvent()
		{
			long currentFakeValueForCounterEvents = GetCurrentFakeValueForCounterEvents();
			if (_lastCounterProgressValue >= 0 && currentFakeValueForCounterEvents > _lastCounterProgressValue)
			{
				_counterChangedEventClass?.InvokeCounterIncreased(this, _lastCounterProgressValue, currentFakeValueForCounterEvents);
			}
			_lastCounterProgressValue = currentFakeValueForCounterEvents;
		}

		public BigButtBeachCounterPopupSpawnData GetCounterPopupSpawnData(Vector3 fallbackOffset)
		{
			if (_counterPopupSpawnZones == null || _counterPopupSpawnZones.Count == 0)
			{
				return new BigButtBeachCounterPopupSpawnData(base.transform.position + fallbackOffset, Vector3.up);
			}
			int num = UnityEngine.Random.Range(0, _counterPopupSpawnZones.Count);
			for (int i = 0; i < _counterPopupSpawnZones.Count; i++)
			{
				BigButtBeachCounterPopupSpawnZone bigButtBeachCounterPopupSpawnZone = _counterPopupSpawnZones[(num + i) % _counterPopupSpawnZones.Count];
				if (bigButtBeachCounterPopupSpawnZone != null)
				{
					return bigButtBeachCounterPopupSpawnZone.GetSpawnData();
				}
			}
			return new BigButtBeachCounterPopupSpawnData(base.transform.position + fallbackOffset, Vector3.up);
		}

		public bool TryGetGrabClickPopupSpawnData(SimplePointGrabable simplePointGrabable, int playerId, out BigButtBeachCounterPopupSpawnData spawnData)
		{
			Transform nearestGrabHandle = GetNearestGrabHandle(simplePointGrabable, playerId);
			if (nearestGrabHandle != null && _grabClickPopupSpawnZonesByHandle != null && _grabClickPopupSpawnZonesByHandle.TryGetValue(nearestGrabHandle, out var value) && value != null)
			{
				spawnData = value.GetSpawnData();
				return true;
			}
			spawnData = default(BigButtBeachCounterPopupSpawnData);
			return false;
		}

		private Transform GetNearestGrabHandle(SimplePointGrabable simplePointGrabable, int playerId)
		{
			if (simplePointGrabable == null || _lineArmsModel == null)
			{
				return null;
			}
			LineArmControllerBase grabbingLineArm = GetGrabbingLineArm(simplePointGrabable, playerId);
			if (!(grabbingLineArm != null))
			{
				return null;
			}
			return simplePointGrabable.GetNearestHandle(grabbingLineArm.transform.position);
		}

		private LineArmControllerBase GetGrabbingLineArm(SimplePointGrabable simplePointGrabable, int playerId)
		{
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = _lineArmsModel.GetAllLineArmsForPlayer(playerId);
			if (allLineArmsForPlayer == null)
			{
				return null;
			}
			LineArmControllerBase lineArmControllerBase = null;
			foreach (LineArmControllerBase value in allLineArmsForPlayer.Values)
			{
				if (!(value == null) && value.enabled)
				{
					if ((object)lineArmControllerBase == null)
					{
						lineArmControllerBase = value;
					}
					if (IsGrabbing(value, simplePointGrabable))
					{
						return value;
					}
				}
			}
			return lineArmControllerBase;
		}

		private static bool IsGrabbing(LineArmControllerBase lineArm, SimplePointGrabable simplePointGrabable)
		{
			foreach (IPointGrabable currentGrabbable in lineArm.CurrentGrabbables)
			{
				if (currentGrabbable == simplePointGrabable)
				{
					return true;
				}
			}
			return false;
		}

		private void UpdateNetworkedCurrentValue(bool force = false)
		{
			if (HasValidNetworkObject() && base.HasStateAuthority)
			{
				double steamCurveValue = GetSteamCurveValue();
				long num = (long)Math.Max(0.0, steamCurveValue);
				long num2 = (long)Math.Max(0.0, steamCurveValue + GetLocalOffsetValue());
				if (force || num2 != CurrentValue || num != CurrentFakeValue)
				{
					CurrentValue = num2;
					CurrentFakeValue = num;
				}
			}
		}

		private long GetCurrentValueForDisplay()
		{
			if (HasValidNetworkObject())
			{
				return CurrentValue;
			}
			return (long)Math.Max(0.0, GetDisplayedValue());
		}

		private long GetCurrentFakeValueForCounterEvents()
		{
			if (HasValidNetworkObject())
			{
				return CurrentFakeValue;
			}
			return (long)Math.Max(0.0, GetSteamCurveValue());
		}

		private bool HasValidNetworkObject()
		{
			if (base.Object != null)
			{
				return base.Object.IsValid;
			}
			return false;
		}

		private double GetOfflineAdvancedValue(long currentValue, long steamMaxValue, long savedUtcTicks)
		{
			if (steamMaxValue <= currentValue || savedUtcTicks <= 0)
			{
				return currentValue;
			}
			double num = Math.Max(0.0, (DateTime.UtcNow - new DateTime(savedUtcTicks, DateTimeKind.Utc)).TotalSeconds);
			double num2 = Math.Min(1.0, num / Math.Max(1.0, _steamLerpDuration));
			return (double)currentValue + (double)(steamMaxValue - currentValue) * num2;
		}

		private float GetRemainingLerpDuration(long savedUtcTicks)
		{
			if (savedUtcTicks <= 0)
			{
				return _steamLerpDuration;
			}
			double num = Math.Max(0.0, (DateTime.UtcNow - new DateTime(savedUtcTicks, DateTimeKind.Utc)).TotalSeconds);
			return Mathf.Max(0f, _steamLerpDuration - (float)num);
		}

		private void SaveProgress()
		{
			if (_counterProgressModel != null && _savingService != null)
			{
				long currentValueForDisplay = GetCurrentValueForDisplay();
				long steamMaxValue = Math.Max(_steamMaxValue, currentValueForDisplay);
				_counterProgressModel.SetProgress(_statAPIString, currentValueForDisplay, steamMaxValue, DateTime.UtcNow.Ticks);
				_savingService.SaveDataForGroup(SavingGroup.BigButtBeachCounterProgress);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			CurrentValue = _CurrentValue;
			CurrentFakeValue = _CurrentFakeValue;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_CurrentValue = CurrentValue;
			_CurrentFakeValue = CurrentFakeValue;
		}

		[NetworkRpcWeavedInvoker(1381359898u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RpcRequestIncrementCurrentValue_0040Invoker1381359898([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BigButtBeachInteractable)context.TargetBehaviour).RpcRequestIncrementCurrentValue();
		}
	}
}
