using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

namespace NomadDrive.Features.Objectives.Networking
{
	public class ObjectivesNetworkSync : NetworkBehaviour
	{
		private const char StepKeySeparator = '|';

		[SerializeField]
		private ObjectiveDatabase _database;

		public readonly SyncHashSet<string> DiscoveredObjectiveIds = new SyncHashSet<string>();

		public readonly SyncHashSet<string> CompletedObjectiveIds = new SyncHashSet<string>();

		public readonly SyncDictionary<string, ushort> StepCounters = new SyncDictionary<string, ushort>();

		public readonly SyncList<StepSourceRecord> StepSources = new SyncList<StepSourceRecord>();

		private readonly HashSet<string> _serverSourceSet = new HashSet<string>();

		public bool IsInitializing { get; private set; }

		public ObjectiveDatabase Database => _database;

		public event Action<string> NetworkObjectiveDiscovered;

		public event Action<string> NetworkObjectiveCompleted;

		public event Action<string, string, ushort> NetworkStepCounterChanged;

		public static string MakeStepKey(string objectiveId, string stepId)
		{
			if (!string.IsNullOrEmpty(stepId))
			{
				return objectiveId + "|" + stepId;
			}
			return objectiveId;
		}

		public static bool TryParseStepKey(string key, out string objectiveId, out string stepId)
		{
			if (string.IsNullOrEmpty(key))
			{
				objectiveId = null;
				stepId = null;
				return false;
			}
			int num = key.IndexOf('|');
			if (num < 0)
			{
				objectiveId = key;
				stepId = string.Empty;
				return true;
			}
			objectiveId = key.Substring(0, num);
			stepId = key.Substring(num + 1);
			return true;
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			_serverSourceSet.Clear();
			for (int i = 0; i < StepSources.Count; i++)
			{
				_serverSourceSet.Add(SourceKey(StepSources[i]));
			}
		}

		[Server]
		public void ServerApplyLoadedState(IReadOnlyCollection<string> discoveredIds, IReadOnlyCollection<string> completedIds, IReadOnlyDictionary<string, ushort> stepCounters)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Objectives.Networking.ObjectivesNetworkSync::ServerApplyLoadedState(System.Collections.Generic.IReadOnlyCollection`1<System.String>,System.Collections.Generic.IReadOnlyCollection`1<System.String>,System.Collections.Generic.IReadOnlyDictionary`2<System.String,System.UInt16>)' called when server was not active");
				return;
			}
			if (discoveredIds != null)
			{
				foreach (string discoveredId in discoveredIds)
				{
					if (!string.IsNullOrEmpty(discoveredId))
					{
						DiscoveredObjectiveIds.Add(discoveredId);
					}
				}
			}
			if (stepCounters != null)
			{
				foreach (KeyValuePair<string, ushort> stepCounter in stepCounters)
				{
					StepCounters[stepCounter.Key] = stepCounter.Value;
				}
			}
			if (completedIds == null)
			{
				return;
			}
			foreach (string completedId in completedIds)
			{
				if (!string.IsNullOrEmpty(completedId))
				{
					CompletedObjectiveIds.Add(completedId);
				}
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			IsInitializing = true;
			SyncHashSet<string> discoveredObjectiveIds = DiscoveredObjectiveIds;
			discoveredObjectiveIds.OnChange = (Action<SyncSet<string>.Operation, string>)Delegate.Combine(discoveredObjectiveIds.OnChange, new Action<SyncSet<string>.Operation, string>(HandleDiscoveredChanged));
			SyncHashSet<string> completedObjectiveIds = CompletedObjectiveIds;
			completedObjectiveIds.OnChange = (Action<SyncSet<string>.Operation, string>)Delegate.Combine(completedObjectiveIds.OnChange, new Action<SyncSet<string>.Operation, string>(HandleCompletedChanged));
			SyncDictionary<string, ushort> stepCounters = StepCounters;
			stepCounters.OnChange = (Action<SyncIDictionary<string, ushort>.Operation, string, ushort>)Delegate.Combine(stepCounters.OnChange, new Action<SyncIDictionary<string, ushort>.Operation, string, ushort>(HandleStepCountersChanged));
			ReplayInitialStateAsync().Forget();
		}

		public override void OnStopClient()
		{
			SyncHashSet<string> discoveredObjectiveIds = DiscoveredObjectiveIds;
			discoveredObjectiveIds.OnChange = (Action<SyncSet<string>.Operation, string>)Delegate.Remove(discoveredObjectiveIds.OnChange, new Action<SyncSet<string>.Operation, string>(HandleDiscoveredChanged));
			SyncHashSet<string> completedObjectiveIds = CompletedObjectiveIds;
			completedObjectiveIds.OnChange = (Action<SyncSet<string>.Operation, string>)Delegate.Remove(completedObjectiveIds.OnChange, new Action<SyncSet<string>.Operation, string>(HandleCompletedChanged));
			SyncDictionary<string, ushort> stepCounters = StepCounters;
			stepCounters.OnChange = (Action<SyncIDictionary<string, ushort>.Operation, string, ushort>)Delegate.Remove(stepCounters.OnChange, new Action<SyncIDictionary<string, ushort>.Operation, string, ushort>(HandleStepCountersChanged));
			base.OnStopClient();
		}

		private async UniTaskVoid ReplayInitialStateAsync()
		{
			await UniTask.DelayFrame(2, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			foreach (string discoveredObjectiveId in DiscoveredObjectiveIds)
			{
				this.NetworkObjectiveDiscovered?.Invoke(discoveredObjectiveId);
			}
			foreach (KeyValuePair<string, ushort> stepCounter in StepCounters)
			{
				if (TryParseStepKey(stepCounter.Key, out var objectiveId, out var stepId))
				{
					this.NetworkStepCounterChanged?.Invoke(objectiveId, stepId, stepCounter.Value);
				}
			}
			foreach (string completedObjectiveId in CompletedObjectiveIds)
			{
				this.NetworkObjectiveCompleted?.Invoke(completedObjectiveId);
			}
			IsInitializing = false;
		}

		private void HandleDiscoveredChanged(SyncSet<string>.Operation op, string item)
		{
			if (!IsInitializing && (uint)op == 0u)
			{
				this.NetworkObjectiveDiscovered?.Invoke(item);
			}
		}

		private void HandleCompletedChanged(SyncSet<string>.Operation op, string item)
		{
			if (!IsInitializing && (uint)op == 0u)
			{
				this.NetworkObjectiveCompleted?.Invoke(item);
			}
		}

		private void HandleStepCountersChanged(SyncIDictionary<string, ushort>.Operation op, string key, ushort _)
		{
			if (!IsInitializing && ((uint)op == 0u || (uint)op == 1u) && TryParseStepKey(key, out var objectiveId, out var stepId) && StepCounters.TryGetValue(key, out var value))
			{
				this.NetworkStepCounterChanged?.Invoke(objectiveId, stepId, value);
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdRequestDiscover(string objectiveId, string autoCompleteStepId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(objectiveId);
			writer.WriteString(autoCompleteStepId);
			SendCommandInternal("System.Void NomadDrive.Features.Objectives.Networking.ObjectivesNetworkSync::CmdRequestDiscover(System.String,System.String)", -2124356424, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		private void ServerRequestDiscover(string objectiveId, string autoCompleteStepId)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Objectives.Networking.ObjectivesNetworkSync::ServerRequestDiscover(System.String,System.String)' called when server was not active");
			}
			else
			{
				if (string.IsNullOrEmpty(objectiveId))
				{
					return;
				}
				ObjectiveDefinition objectiveDefinition = ((_database != null) ? _database.GetById(objectiveId) : null);
				if (!(objectiveDefinition == null) && objectiveDefinition.MustSync && !CompletedObjectiveIds.Contains(objectiveId) && DiscoveredObjectiveIds.Add(objectiveId))
				{
					if (!string.IsNullOrEmpty(autoCompleteStepId) && objectiveDefinition.GetStep(autoCompleteStepId) != null)
					{
						ServerForceCompleteStep(objectiveDefinition, autoCompleteStepId);
					}
					CheckObjectiveCompletion(objectiveDefinition);
				}
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdRequestRegisterSource(string objectiveId, string stepId, string sourceId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(objectiveId);
			writer.WriteString(stepId);
			writer.WriteString(sourceId);
			SendCommandInternal("System.Void NomadDrive.Features.Objectives.Networking.ObjectivesNetworkSync::CmdRequestRegisterSource(System.String,System.String,System.String)", -561925591, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		private void ServerRequestRegisterSource(string objectiveId, string stepId, string sourceId)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Objectives.Networking.ObjectivesNetworkSync::ServerRequestRegisterSource(System.String,System.String,System.String)' called when server was not active");
			}
			else
			{
				if (string.IsNullOrEmpty(objectiveId) || string.IsNullOrEmpty(stepId))
				{
					return;
				}
				ObjectiveDefinition objectiveDefinition = ((_database != null) ? _database.GetById(objectiveId) : null);
				if (objectiveDefinition == null || !objectiveDefinition.MustSync)
				{
					return;
				}
				ObjectiveStepDefinition step = objectiveDefinition.GetStep(stepId);
				if (step == null)
				{
					return;
				}
				string sourceId2 = (string.IsNullOrEmpty(sourceId) ? Guid.NewGuid().ToString("N") : sourceId);
				string item = MakeSourceKey(objectiveId, stepId, sourceId2);
				if (_serverSourceSet.Add(item))
				{
					StepSources.Add(new StepSourceRecord
					{
						ObjectiveId = objectiveId,
						StepId = stepId,
						SourceId = sourceId2
					});
					string text = MakeStepKey(objectiveId, stepId);
					ushort value;
					ushort num = (ushort)(StepCounters.TryGetValue(text, out value) ? value : 0);
					ushort num2 = ResolveStepTarget(step);
					ushort num3 = ((num < num2) ? ((ushort)(num + 1)) : num);
					if (num3 != num)
					{
						StepCounters[text] = num3;
					}
					CheckObjectiveCompletion(objectiveDefinition);
				}
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdRequestCompleteStepBoolean(string objectiveId, string stepId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(objectiveId);
			writer.WriteString(stepId);
			SendCommandInternal("System.Void NomadDrive.Features.Objectives.Networking.ObjectivesNetworkSync::CmdRequestCompleteStepBoolean(System.String,System.String)", 2025724644, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		private void ServerForceCompleteStep(ObjectiveDefinition def, string stepId)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Objectives.Networking.ObjectivesNetworkSync::ServerForceCompleteStep(NomadDrive.Features.Objectives.ObjectiveDefinition,System.String)' called when server was not active");
				return;
			}
			string i = MakeStepKey(def.ObjectiveId, stepId);
			ObjectiveStepDefinition step = def.GetStep(stepId);
			ushort value = (ushort)((step == null) ? 1 : ResolveStepTarget(step));
			StepCounters[i] = value;
			string sourceId = Guid.NewGuid().ToString("N");
			string item = MakeSourceKey(def.ObjectiveId, stepId, sourceId);
			if (_serverSourceSet.Add(item))
			{
				StepSources.Add(new StepSourceRecord
				{
					ObjectiveId = def.ObjectiveId,
					StepId = stepId,
					SourceId = sourceId
				});
			}
		}

		[Server]
		private void CheckObjectiveCompletion(ObjectiveDefinition def)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Objectives.Networking.ObjectivesNetworkSync::CheckObjectiveCompletion(NomadDrive.Features.Objectives.ObjectiveDefinition)' called when server was not active");
			}
			else
			{
				if (def == null || CompletedObjectiveIds.Contains(def.ObjectiveId))
				{
					return;
				}
				if (def.StepCount == 0)
				{
					CompletedObjectiveIds.Add(def.ObjectiveId);
					return;
				}
				for (int i = 0; i < def.Steps.Count; i++)
				{
					ObjectiveStepDefinition objectiveStepDefinition = def.Steps[i];
					if (objectiveStepDefinition != null && !string.IsNullOrEmpty(objectiveStepDefinition.StepId))
					{
						ushort num = ResolveStepTarget(objectiveStepDefinition);
						string key = MakeStepKey(def.ObjectiveId, objectiveStepDefinition.StepId);
						if ((StepCounters.TryGetValue(key, out var value) ? value : 0) < num)
						{
							return;
						}
					}
				}
				CompletedObjectiveIds.Add(def.ObjectiveId);
			}
		}

		private static ushort ResolveStepTarget(ObjectiveStepDefinition step)
		{
			if (step.ProgressMode == StepProgressMode.DiscreteCount)
			{
				return (ushort)Mathf.Min((step.DiscreteTarget < 1) ? 1 : step.DiscreteTarget, 65535);
			}
			return 1;
		}

		private static string MakeSourceKey(string objectiveId, string stepId, string sourceId)
		{
			return objectiveId + "|" + stepId + "|" + sourceId;
		}

		private static string SourceKey(StepSourceRecord record)
		{
			return MakeSourceKey(record.ObjectiveId, record.StepId, record.SourceId);
		}

		public ObjectivesNetworkSync()
		{
			InitSyncObject(DiscoveredObjectiveIds);
			InitSyncObject(CompletedObjectiveIds);
			InitSyncObject(StepCounters);
			InitSyncObject(StepSources);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdRequestDiscover__String__String(string objectiveId, string autoCompleteStepId)
		{
			ServerRequestDiscover(objectiveId, autoCompleteStepId);
		}

		protected static void InvokeUserCode_CmdRequestDiscover__String__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestDiscover called on client.");
			}
			else
			{
				((ObjectivesNetworkSync)obj).UserCode_CmdRequestDiscover__String__String(reader.ReadString(), reader.ReadString());
			}
		}

		protected void UserCode_CmdRequestRegisterSource__String__String__String(string objectiveId, string stepId, string sourceId)
		{
			ServerRequestRegisterSource(objectiveId, stepId, sourceId);
		}

		protected static void InvokeUserCode_CmdRequestRegisterSource__String__String__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestRegisterSource called on client.");
			}
			else
			{
				((ObjectivesNetworkSync)obj).UserCode_CmdRequestRegisterSource__String__String__String(reader.ReadString(), reader.ReadString(), reader.ReadString());
			}
		}

		protected void UserCode_CmdRequestCompleteStepBoolean__String__String(string objectiveId, string stepId)
		{
			ServerRequestRegisterSource(objectiveId, stepId, null);
		}

		protected static void InvokeUserCode_CmdRequestCompleteStepBoolean__String__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestCompleteStepBoolean called on client.");
			}
			else
			{
				((ObjectivesNetworkSync)obj).UserCode_CmdRequestCompleteStepBoolean__String__String(reader.ReadString(), reader.ReadString());
			}
		}

		static ObjectivesNetworkSync()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(ObjectivesNetworkSync), "System.Void NomadDrive.Features.Objectives.Networking.ObjectivesNetworkSync::CmdRequestDiscover(System.String,System.String)", InvokeUserCode_CmdRequestDiscover__String__String, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(ObjectivesNetworkSync), "System.Void NomadDrive.Features.Objectives.Networking.ObjectivesNetworkSync::CmdRequestRegisterSource(System.String,System.String,System.String)", InvokeUserCode_CmdRequestRegisterSource__String__String__String, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(ObjectivesNetworkSync), "System.Void NomadDrive.Features.Objectives.Networking.ObjectivesNetworkSync::CmdRequestCompleteStepBoolean(System.String,System.String)", InvokeUserCode_CmdRequestCompleteStepBoolean__String__String, requiresAuthority: false);
		}
	}
}
