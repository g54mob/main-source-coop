using System;
using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore;
using EvilCore.Audio;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.EvilSave;
using Mirror;
using NomadDrive.Features.Objectives.Networking;
using NomadDrive.Features.Player;
using PrimeTween;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Objectives
{
	public class ObjectivesService : MonoBehaviour, IObjectivesService, ISaveable
	{
		private const int SaveFormatVersion = 2;

		private const int SaveVersionSentinel = -2;

		[SerializeField]
		[Tooltip("Database containing every ObjectiveDefinition available in the project.")]
		private ObjectiveDatabase _database;

		[SerializeField]
		[Tooltip("Seconds to wait between final step checkmark and removing the entry from the panel.")]
		private float _completionFadeDelay = 1f;

		[Header("Audio")]
		[SerializeField]
		[Tooltip("Plays when a new objective is discovered.")]
		private SoundID objectiveDiscoveredSound;

		[SerializeField]
		[Tooltip("Plays when a sub-task (step) is completed.")]
		private SoundID stepCompletedSound;

		[SerializeField]
		[Tooltip("Plays when an objective is fully completed.")]
		private SoundID objectiveCompletedSound;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private ObjectivesPanel _panel;

		[Inject]
		private ObjectiveDiscoveryBanner _discoveryBanner;

		[Inject]
		private IAudioManager _audioManager;

		[Inject]
		private IGameLoadingManager _loadingManager;

		private readonly HashSet<string> _discoveredIds = new HashSet<string>();

		private readonly HashSet<string> _completedIds = new HashSet<string>();

		private readonly Dictionary<string, ObjectiveTrackerState> _activeById = new Dictionary<string, ObjectiveTrackerState>();

		private readonly Dictionary<string, List<ObjectiveTrigger>> _discoveryTriggers = new Dictionary<string, List<ObjectiveTrigger>>();

		private readonly Dictionary<string, List<(ObjectiveStepDefinition step, ObjectiveTrigger trigger)>> _activeStepTriggers = new Dictionary<string, List<(ObjectiveStepDefinition, ObjectiveTrigger)>>();

		private ObjectiveTriggerContext _triggerContext;

		private bool _playerHooked;

		private ObjectivesNetworkSync _networkSync;

		private bool _networkHooked;

		private bool _pendingNetworkSeedFromSave;

		public string SaveId => "objectives";

		public IReadOnlyCollection<string> DiscoveredObjectiveIds => _discoveredIds;

		public IReadOnlyCollection<string> CompletedObjectiveIds => _completedIds;

		public IReadOnlyCollection<ObjectiveTrackerState> ActiveTrackers => _activeById.Values;

		public event Action<ObjectiveDefinition, ObjectiveTrackerState> ObjectiveDiscovered;

		public event Action<ObjectiveDefinition, ObjectiveTrackerState, ObjectiveStepDefinition> StepCompleted;

		public event Action<ObjectiveDefinition, ObjectiveTrackerState, ObjectiveStepDefinition, ObjectiveStepProgress> StepProgressChanged;

		public event Action<ObjectiveDefinition> ObjectiveCompleted;

		private void OnEnable()
		{
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered += OnPlayerRegistered;
				_playerService.OnPlayerCleared += OnPlayerCleared;
				if (_playerService.IsPlayerSpawned)
				{
					OnPlayerRegistered();
				}
			}
		}

		private void OnDisable()
		{
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered -= OnPlayerRegistered;
				_playerService.OnPlayerCleared -= OnPlayerCleared;
			}
			UnsubscribeLoadingGate();
			TeardownAllTriggers();
		}

		public bool IsDiscovered(string objectiveId)
		{
			if (!string.IsNullOrEmpty(objectiveId))
			{
				return _discoveredIds.Contains(objectiveId);
			}
			return false;
		}

		public bool IsCompleted(string objectiveId)
		{
			if (!string.IsNullOrEmpty(objectiveId))
			{
				return _completedIds.Contains(objectiveId);
			}
			return false;
		}

		public bool IsStepCompleted(string objectiveId, string stepId)
		{
			if (_activeById.TryGetValue(objectiveId, out var value))
			{
				return value.IsStepCompleted(stepId);
			}
			return false;
		}

		public void TryDiscoverObjective(string objectiveId)
		{
			TryDiscoverObjective(objectiveId, null);
		}

		public void TryDiscoverObjective(string objectiveId, string autoCompleteStepId)
		{
			Discover(objectiveId, autoCompleteStepId);
		}

		private void OnPlayerRegistered()
		{
			if (!_playerHooked && _playerService != null && !(_database == null))
			{
				_triggerContext = new ObjectiveTriggerContext
				{
					InteractionManager = _playerService.InteractionManager,
					EquipmentManager = _playerService.EquipmentManager,
					PlayerService = _playerService,
					ObjectivesService = this
				};
				_playerHooked = true;
				if (_loadingManager == null || _loadingManager.IsLoadingComplete())
				{
					ActivateOnceLoaded();
					return;
				}
				_loadingManager.OnLoadingComplete.AddListener(ActivateOnceLoaded);
				_loadingManager.OnTimeout.AddListener(ActivateOnceLoaded);
			}
		}

		private void ActivateOnceLoaded()
		{
			UnsubscribeLoadingGate();
			if (_playerHooked)
			{
				TryHookNetworkSync();
				RebuildUIFromState();
				ActivateDiscoveryTriggers();
				ActivateActiveStepTriggers();
			}
		}

		private void UnsubscribeLoadingGate()
		{
			if (_loadingManager != null)
			{
				_loadingManager.OnLoadingComplete.RemoveListener(ActivateOnceLoaded);
				_loadingManager.OnTimeout.RemoveListener(ActivateOnceLoaded);
			}
		}

		private void OnPlayerCleared()
		{
			_playerHooked = false;
			UnsubscribeLoadingGate();
			UnhookNetworkSync();
			TeardownAllTriggers();
			_triggerContext = null;
		}

		private void TryHookNetworkSync()
		{
			if (_networkHooked)
			{
				return;
			}
			if (_networkSync == null)
			{
				_networkSync = UnityEngine.Object.FindFirstObjectByType<ObjectivesNetworkSync>();
			}
			if (_networkSync == null)
			{
				return;
			}
			_networkSync.NetworkObjectiveDiscovered += OnNetworkObjectiveDiscovered;
			_networkSync.NetworkObjectiveCompleted += OnNetworkObjectiveCompleted;
			_networkSync.NetworkStepCounterChanged += OnNetworkStepCounterChanged;
			_networkHooked = true;
			foreach (string discoveredObjectiveId in _networkSync.DiscoveredObjectiveIds)
			{
				ApplyDiscoveryLocally(discoveredObjectiveId, null, silent: true);
			}
			foreach (KeyValuePair<string, ushort> stepCounter in _networkSync.StepCounters)
			{
				if (ObjectivesNetworkSync.TryParseStepKey(stepCounter.Key, out var objectiveId, out var stepId))
				{
					ApplyStepCounterFromNetwork(objectiveId, stepId, stepCounter.Value, silent: true);
				}
			}
			foreach (string completedObjectiveId in _networkSync.CompletedObjectiveIds)
			{
				ApplyObjectiveCompletionLocally(completedObjectiveId, silent: true);
			}
			TrySeedNetworkSyncFromSave();
		}

		private void UnhookNetworkSync()
		{
			if (_networkHooked && !(_networkSync == null))
			{
				_networkSync.NetworkObjectiveDiscovered -= OnNetworkObjectiveDiscovered;
				_networkSync.NetworkObjectiveCompleted -= OnNetworkObjectiveCompleted;
				_networkSync.NetworkStepCounterChanged -= OnNetworkStepCounterChanged;
				_networkHooked = false;
			}
		}

		private bool MustSync(string objectiveId)
		{
			if (_networkSync == null)
			{
				return false;
			}
			ObjectiveDefinition objectiveDefinition = _database?.GetById(objectiveId);
			if (objectiveDefinition != null)
			{
				return objectiveDefinition.MustSync;
			}
			return false;
		}

		private void RebuildUIFromState()
		{
			if (_panel == null)
			{
				return;
			}
			_panel.ClearAll();
			foreach (ObjectiveTrackerState value in _activeById.Values)
			{
				ObjectiveDefinition byId = _database.GetById(value.ObjectiveId);
				if (!(byId == null))
				{
					_panel.AddObjective(byId, value);
				}
			}
		}

		private void ActivateDiscoveryTriggers()
		{
			if (_database == null)
			{
				return;
			}
			foreach (ObjectiveDefinition item in _database.All)
			{
				if (item == null || string.IsNullOrEmpty(item.ObjectiveId) || _discoveredIds.Contains(item.ObjectiveId) || _completedIds.Contains(item.ObjectiveId) || item.DiscoveryTriggers == null || item.DiscoveryTriggers.Count == 0)
				{
					continue;
				}
				string id = item.ObjectiveId;
				List<ObjectiveTrigger> list = new List<ObjectiveTrigger>(item.DiscoveryTriggers.Count);
				foreach (ObjectiveDiscoveryEntry discoveryTrigger in item.DiscoveryTriggers)
				{
					if (discoveryTrigger?.Trigger != null)
					{
						string autoStep = discoveryTrigger.AutoCompletesStepId;
						ObjectiveTrigger trigger = discoveryTrigger.Trigger;
						trigger.Activate(_triggerContext, delegate
						{
							Discover(id, autoStep);
						});
						list.Add(trigger);
					}
				}
				if (list.Count > 0)
				{
					_discoveryTriggers[id] = list;
				}
			}
		}

		private void ActivateActiveStepTriggers()
		{
			foreach (ObjectiveTrackerState value in _activeById.Values)
			{
				ActivateStepTriggersFor(value);
			}
		}

		private void ActivateStepTriggersFor(ObjectiveTrackerState state)
		{
			ObjectiveDefinition objectiveDefinition = _database?.GetById(state.ObjectiveId);
			if (objectiveDefinition == null)
			{
				return;
			}
			if (!_activeStepTriggers.TryGetValue(state.ObjectiveId, out List<(ObjectiveStepDefinition, ObjectiveTrigger)> value))
			{
				value = new List<(ObjectiveStepDefinition, ObjectiveTrigger)>();
				_activeStepTriggers[state.ObjectiveId] = value;
			}
			for (int i = 0; i < objectiveDefinition.Steps.Count; i++)
			{
				ObjectiveStepDefinition objectiveStepDefinition = objectiveDefinition.Steps[i];
				if (objectiveStepDefinition == null || string.IsNullOrEmpty(objectiveStepDefinition.StepId) || state.IsStepCompleted(objectiveStepDefinition.StepId) || objectiveStepDefinition.CompletionTrigger == null || objectiveStepDefinition.CompletionTrigger.IsActive)
				{
					continue;
				}
				string objId = state.ObjectiveId;
				string stepId = objectiveStepDefinition.StepId;
				switch (objectiveStepDefinition.ProgressMode)
				{
				case StepProgressMode.DiscreteCount:
				{
					int target = Mathf.Max(1, objectiveStepDefinition.DiscreteTarget);
					objectiveStepDefinition.CompletionTrigger.Activate(_triggerContext, delegate
					{
						HandleDiscreteIncrement(objId, stepId, target);
					});
					break;
				}
				case StepProgressMode.FloatRatio:
					objectiveStepDefinition.CompletionTrigger.Activate(_triggerContext, delegate
					{
						CompleteStep(objId, stepId);
					}, delegate(float ratio)
					{
						HandleRatioReport(objId, stepId, ratio);
					});
					break;
				default:
					objectiveStepDefinition.CompletionTrigger.Activate(_triggerContext, delegate
					{
						CompleteStep(objId, stepId);
					});
					break;
				}
				value.Add((objectiveStepDefinition, objectiveStepDefinition.CompletionTrigger));
			}
		}

		private void HandleDiscreteIncrement(string objectiveId, string stepId, int target)
		{
			if (MustSync(objectiveId))
			{
				RouteStepEventToNetwork(objectiveId, stepId);
			}
			else
			{
				if (!_activeById.TryGetValue(objectiveId, out var value) || value.IsStepCompleted(stepId))
				{
					return;
				}
				ObjectiveDefinition objectiveDefinition = _database?.GetById(objectiveId);
				if (objectiveDefinition == null)
				{
					return;
				}
				ObjectiveStepDefinition step = objectiveDefinition.GetStep(stepId);
				if (step != null)
				{
					ObjectiveStepProgress orCreate = value.GetOrCreate(stepId);
					orCreate.Count = Mathf.Min(orCreate.Count + 1, target);
					_panel?.UpdateStepProgress(objectiveId, stepId, orCreate, step);
					this.StepProgressChanged?.Invoke(objectiveDefinition, value, step, orCreate);
					if (orCreate.Count >= target)
					{
						CompleteStep(objectiveId, stepId);
					}
				}
			}
		}

		private void RouteStepEventToNetwork(string objectiveId, string stepId)
		{
			if (!(_networkSync == null))
			{
				string text = TryGetSourceIdForStep(objectiveId, stepId);
				if (!string.IsNullOrEmpty(text))
				{
					_networkSync.CmdRequestRegisterSource(objectiveId, stepId, text);
				}
				else
				{
					_networkSync.CmdRequestCompleteStepBoolean(objectiveId, stepId);
				}
			}
		}

		private string TryGetSourceIdForStep(string objectiveId, string stepId)
		{
			if (!_activeStepTriggers.TryGetValue(objectiveId, out List<(ObjectiveStepDefinition, ObjectiveTrigger)> value))
			{
				return null;
			}
			for (int i = 0; i < value.Count; i++)
			{
				if (value[i].Item1 != null && value[i].Item1.StepId == stepId)
				{
					return value[i].Item2?.LastSourceId;
				}
			}
			return null;
		}

		private void HandleRatioReport(string objectiveId, string stepId, float ratio)
		{
			if (!_activeById.TryGetValue(objectiveId, out var value) || value.IsStepCompleted(stepId))
			{
				return;
			}
			ObjectiveDefinition objectiveDefinition = _database?.GetById(objectiveId);
			if (objectiveDefinition == null)
			{
				return;
			}
			ObjectiveStepDefinition step = objectiveDefinition.GetStep(stepId);
			if (step == null)
			{
				return;
			}
			ObjectiveStepProgress orCreate = value.GetOrCreate(stepId);
			float num = Mathf.Clamp01(ratio);
			if (!Mathf.Approximately(orCreate.Ratio, num))
			{
				orCreate.Ratio = num;
				_panel?.UpdateStepProgress(objectiveId, stepId, orCreate, step);
				this.StepProgressChanged?.Invoke(objectiveDefinition, value, step, orCreate);
				if (orCreate.Ratio >= 1f)
				{
					CompleteStep(objectiveId, stepId);
				}
			}
		}

		private void Discover(string objectiveId)
		{
			Discover(objectiveId, null);
		}

		private void Discover(string objectiveId, string autoCompleteStepId)
		{
			if (!string.IsNullOrEmpty(objectiveId) && !_discoveredIds.Contains(objectiveId) && !_completedIds.Contains(objectiveId))
			{
				if (_database?.GetById(objectiveId) == null)
				{
					EvilLogger.LogError("[ObjectivesService] Discover('" + objectiveId + "') failed: ObjectiveDefinition not in database. Either the ObjectiveId is misspelled or the asset isn't listed in ObjectiveDatabase._objectives.", "Discover", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Objectives\\Scripts\\Service\\ObjectivesService.cs", 397);
				}
				else if (MustSync(objectiveId))
				{
					_ = NetworkClient.active;
					_networkSync.CmdRequestDiscover(objectiveId, autoCompleteStepId);
				}
				else
				{
					ApplyDiscoveryLocally(objectiveId, autoCompleteStepId, silent: false);
				}
			}
		}

		private void ApplyDiscoveryLocally(string objectiveId, string autoCompleteStepId, bool silent)
		{
			if (string.IsNullOrEmpty(objectiveId) || _discoveredIds.Contains(objectiveId) || _completedIds.Contains(objectiveId))
			{
				return;
			}
			ObjectiveDefinition objectiveDefinition = _database?.GetById(objectiveId);
			if (objectiveDefinition == null)
			{
				return;
			}
			_discoveredIds.Add(objectiveId);
			if (_discoveryTriggers.TryGetValue(objectiveId, out var value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					value[i]?.Deactivate();
				}
				value.Clear();
				_discoveryTriggers.Remove(objectiveId);
			}
			ObjectiveTrackerState objectiveTrackerState = new ObjectiveTrackerState(objectiveId);
			_activeById[objectiveId] = objectiveTrackerState;
			_panel?.AddObjective(objectiveDefinition, objectiveTrackerState);
			if (!silent)
			{
				_discoveryBanner?.Show(objectiveDefinition);
			}
			ActivateStepTriggersFor(objectiveTrackerState);
			this.ObjectiveDiscovered?.Invoke(objectiveDefinition, objectiveTrackerState);
			if (!silent && IsPanelVisibleForNotification() && IsObjectiveAudibleViaPanel(objectiveId))
			{
				_audioManager?.PlayOneShotUI(objectiveDiscoveredSound);
			}
			if (!string.IsNullOrEmpty(autoCompleteStepId) && objectiveDefinition.GetStep(autoCompleteStepId) != null)
			{
				CompleteStep(objectiveId, autoCompleteStepId);
			}
			if (!objectiveDefinition.MustSync && objectiveDefinition.StepCount == 0)
			{
				ScheduleCompletion(objectiveDefinition);
			}
		}

		private void CompleteStep(string objectiveId, string stepId)
		{
			if (MustSync(objectiveId))
			{
				RouteStepEventToNetwork(objectiveId, stepId);
			}
			else
			{
				ApplyStepCompleteLocally(objectiveId, stepId, silent: false);
			}
		}

		private void ApplyStepCompleteLocally(string objectiveId, string stepId, bool silent)
		{
			if (!_activeById.TryGetValue(objectiveId, out var value) || value.IsStepCompleted(stepId))
			{
				return;
			}
			ObjectiveDefinition objectiveDefinition = _database?.GetById(objectiveId);
			if (objectiveDefinition == null)
			{
				return;
			}
			ObjectiveStepDefinition step = objectiveDefinition.GetStep(stepId);
			if (step == null)
			{
				return;
			}
			value.MarkStepCompleted(stepId);
			if (_activeStepTriggers.TryGetValue(objectiveId, out List<(ObjectiveStepDefinition, ObjectiveTrigger)> value2))
			{
				for (int i = 0; i < value2.Count; i++)
				{
					if (value2[i].Item1 == step)
					{
						value2[i].Item2.Deactivate();
						value2.RemoveAt(i);
						break;
					}
				}
			}
			_panel?.MarkStepComplete(objectiveId, stepId);
			this.StepCompleted?.Invoke(objectiveDefinition, value, step);
			if (!silent && IsPanelVisibleForNotification() && IsObjectiveAudibleViaPanel(objectiveId))
			{
				_audioManager?.PlayOneShotUI(stepCompletedSound);
			}
			if (!objectiveDefinition.MustSync && AreAllStepsCompleted(objectiveDefinition, value))
			{
				ScheduleCompletion(objectiveDefinition);
			}
		}

		private bool AreAllStepsCompleted(ObjectiveDefinition def, ObjectiveTrackerState state)
		{
			if (def.StepCount == 0)
			{
				return true;
			}
			for (int i = 0; i < def.Steps.Count; i++)
			{
				ObjectiveStepDefinition objectiveStepDefinition = def.Steps[i];
				if (objectiveStepDefinition != null && !string.IsNullOrEmpty(objectiveStepDefinition.StepId) && !state.IsStepCompleted(objectiveStepDefinition.StepId))
				{
					return false;
				}
			}
			return true;
		}

		private void ScheduleCompletion(ObjectiveDefinition def)
		{
			string id = def.ObjectiveId;
			if (_completionFadeDelay <= 0f)
			{
				CompleteObjective(id);
				return;
			}
			Tween.Delay(this, _completionFadeDelay, delegate(ObjectivesService target)
			{
				target.CompleteObjective(id);
			});
		}

		private void CompleteObjective(string objectiveId)
		{
			ApplyObjectiveCompletionLocally(objectiveId, silent: false);
		}

		private void ApplyObjectiveCompletionLocally(string objectiveId, bool silent)
		{
			if (_completedIds.Contains(objectiveId))
			{
				return;
			}
			ObjectiveDefinition objectiveDefinition = _database?.GetById(objectiveId);
			_completedIds.Add(objectiveId);
			_activeById.Remove(objectiveId);
			if (_activeStepTriggers.TryGetValue(objectiveId, out List<(ObjectiveStepDefinition, ObjectiveTrigger)> value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					value[i].Item2?.Deactivate();
				}
				value.Clear();
				_activeStepTriggers.Remove(objectiveId);
			}
			_panel?.RemoveObjective(objectiveId);
			if (objectiveDefinition != null)
			{
				this.ObjectiveCompleted?.Invoke(objectiveDefinition);
				if (!silent && IsPanelVisibleForNotification())
				{
					_audioManager?.PlayOneShotUI(objectiveCompletedSound);
				}
			}
		}

		private bool IsPanelVisibleForNotification()
		{
			if (!(_panel == null))
			{
				return !_panel.IsHidden;
			}
			return true;
		}

		private bool IsObjectiveAudibleViaPanel(string objectiveId)
		{
			if (!(_panel == null))
			{
				return _panel.IsObjectiveAudible(objectiveId);
			}
			return true;
		}

		private void OnNetworkObjectiveDiscovered(string objectiveId)
		{
			bool silent = _networkSync != null && _networkSync.IsInitializing;
			ApplyDiscoveryLocally(objectiveId, null, silent);
		}

		private void OnNetworkObjectiveCompleted(string objectiveId)
		{
			bool silent = _networkSync != null && _networkSync.IsInitializing;
			ApplyObjectiveCompletionLocally(objectiveId, silent);
		}

		private void OnNetworkStepCounterChanged(string objectiveId, string stepId, ushort count)
		{
			bool silent = _networkSync != null && _networkSync.IsInitializing;
			ApplyStepCounterFromNetwork(objectiveId, stepId, count, silent);
		}

		private void ApplyStepCounterFromNetwork(string objectiveId, string stepId, ushort count, bool silent)
		{
			if (!_activeById.TryGetValue(objectiveId, out var value))
			{
				return;
			}
			ObjectiveDefinition objectiveDefinition = _database?.GetById(objectiveId);
			if (objectiveDefinition == null)
			{
				return;
			}
			ObjectiveStepDefinition step = objectiveDefinition.GetStep(stepId);
			if (step != null)
			{
				ObjectiveStepProgress orCreate = value.GetOrCreate(stepId);
				int num = ResolveStepTarget(step);
				int num2 = Mathf.Min(count, num);
				bool num3 = orCreate.Count != num2;
				orCreate.Count = num2;
				if (num3)
				{
					_panel?.UpdateStepProgress(objectiveId, stepId, orCreate, step);
					this.StepProgressChanged?.Invoke(objectiveDefinition, value, step, orCreate);
				}
				if (num2 >= num && !value.IsStepCompleted(stepId))
				{
					ApplyStepCompleteLocally(objectiveId, stepId, silent);
				}
			}
		}

		private static int ResolveStepTarget(ObjectiveStepDefinition step)
		{
			if (step.ProgressMode == StepProgressMode.DiscreteCount)
			{
				if (step.DiscreteTarget >= 1)
				{
					return step.DiscreteTarget;
				}
				return 1;
			}
			return 1;
		}

		private void TeardownAllTriggers()
		{
			foreach (KeyValuePair<string, List<ObjectiveTrigger>> discoveryTrigger in _discoveryTriggers)
			{
				List<ObjectiveTrigger> value = discoveryTrigger.Value;
				if (value != null)
				{
					for (int i = 0; i < value.Count; i++)
					{
						value[i]?.Deactivate();
					}
					value.Clear();
				}
			}
			_discoveryTriggers.Clear();
			foreach (KeyValuePair<string, List<(ObjectiveStepDefinition, ObjectiveTrigger)>> activeStepTrigger in _activeStepTriggers)
			{
				List<(ObjectiveStepDefinition, ObjectiveTrigger)> value2 = activeStepTrigger.Value;
				if (value2 != null)
				{
					for (int j = 0; j < value2.Count; j++)
					{
						value2[j].Item2?.Deactivate();
					}
					value2.Clear();
				}
			}
			_activeStepTriggers.Clear();
		}

		public void OnSave(EvilWriter writer)
		{
			writer.Write(-2);
			writer.Write(2);
			writer.Write(_discoveredIds.Count);
			foreach (string discoveredId in _discoveredIds)
			{
				writer.Write(discoveredId);
			}
			writer.Write(_completedIds.Count);
			foreach (string completedId in _completedIds)
			{
				writer.Write(completedId);
			}
			writer.Write(_activeById.Count);
			foreach (ObjectiveTrackerState value in _activeById.Values)
			{
				writer.Write(value.ObjectiveId);
				writer.Write(value.StepProgresses.Count);
				foreach (KeyValuePair<string, ObjectiveStepProgress> stepProgress in value.StepProgresses)
				{
					writer.Write(stepProgress.Key);
					writer.Write(stepProgress.Value.IsCompleted);
					writer.Write(stepProgress.Value.Count);
					writer.Write(stepProgress.Value.Ratio);
				}
			}
		}

		public void OnLoad(EvilReader reader)
		{
			_discoveredIds.Clear();
			_completedIds.Clear();
			_activeById.Clear();
			int num = reader.ReadInt();
			if (num < 0)
			{
				int version = reader.ReadInt();
				LoadVersioned(reader, version);
			}
			else
			{
				LoadLegacyV1(reader, num);
			}
			if (_playerHooked)
			{
				TeardownAllTriggers();
				RebuildUIFromState();
				ActivateDiscoveryTriggers();
				ActivateActiveStepTriggers();
			}
			_pendingNetworkSeedFromSave = true;
			TrySeedNetworkSyncFromSave();
		}

		private void TrySeedNetworkSyncFromSave()
		{
			if (!_pendingNetworkSeedFromSave || _networkSync == null || !NetworkServer.active || _database == null)
			{
				return;
			}
			List<string> list = new List<string>();
			foreach (string discoveredId in _discoveredIds)
			{
				if (MustSync(discoveredId))
				{
					list.Add(discoveredId);
				}
			}
			List<string> list2 = new List<string>();
			foreach (string completedId in _completedIds)
			{
				if (MustSync(completedId))
				{
					list2.Add(completedId);
				}
			}
			Dictionary<string, ushort> dictionary = new Dictionary<string, ushort>();
			foreach (ObjectiveTrackerState value in _activeById.Values)
			{
				if (!MustSync(value.ObjectiveId))
				{
					continue;
				}
				ObjectiveDefinition byId = _database.GetById(value.ObjectiveId);
				if (byId == null)
				{
					continue;
				}
				foreach (KeyValuePair<string, ObjectiveStepProgress> stepProgress in value.StepProgresses)
				{
					ObjectiveStepDefinition step = byId.GetStep(stepProgress.Key);
					if (step != null)
					{
						int num = ResolveStepTarget(step);
						int num2 = (stepProgress.Value.IsCompleted ? num : Mathf.Min(stepProgress.Value.Count, num));
						if (num2 > 0)
						{
							dictionary[ObjectivesNetworkSync.MakeStepKey(value.ObjectiveId, stepProgress.Key)] = (ushort)num2;
						}
					}
				}
			}
			_networkSync.ServerApplyLoadedState(list, list2, dictionary);
			_pendingNetworkSeedFromSave = false;
		}

		private void LoadVersioned(EvilReader reader, int version)
		{
			int num = reader.ReadInt();
			for (int i = 0; i < num; i++)
			{
				_discoveredIds.Add(reader.ReadString());
			}
			int num2 = reader.ReadInt();
			for (int j = 0; j < num2; j++)
			{
				_completedIds.Add(reader.ReadString());
			}
			int num3 = reader.ReadInt();
			for (int k = 0; k < num3; k++)
			{
				string text = reader.ReadString();
				ObjectiveTrackerState objectiveTrackerState = new ObjectiveTrackerState(text);
				int num4 = reader.ReadInt();
				for (int l = 0; l < num4; l++)
				{
					string stepId = reader.ReadString();
					ObjectiveStepProgress orCreate = objectiveTrackerState.GetOrCreate(stepId);
					orCreate.IsCompleted = reader.ReadBool();
					orCreate.Count = reader.ReadInt();
					orCreate.Ratio = reader.ReadFloat();
				}
				_activeById[text] = objectiveTrackerState;
			}
			_ = 2;
		}

		private void LoadLegacyV1(EvilReader reader, int discoveredCount)
		{
			for (int i = 0; i < discoveredCount; i++)
			{
				_discoveredIds.Add(reader.ReadString());
			}
			int num = reader.ReadInt();
			for (int j = 0; j < num; j++)
			{
				_completedIds.Add(reader.ReadString());
			}
			int num2 = reader.ReadInt();
			for (int k = 0; k < num2; k++)
			{
				string text = reader.ReadString();
				ObjectiveTrackerState objectiveTrackerState = new ObjectiveTrackerState(text);
				int num3 = reader.ReadInt();
				for (int l = 0; l < num3; l++)
				{
					objectiveTrackerState.MarkStepCompleted(reader.ReadString());
				}
				_activeById[text] = objectiveTrackerState;
			}
		}
	}
}
