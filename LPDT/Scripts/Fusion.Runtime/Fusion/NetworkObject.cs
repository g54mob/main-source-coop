#define TRACE
#define DEBUG
using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fusion
{
	[AddComponentMenu("Fusion/Network Object")]
	[DisallowMultipleComponent]
	[HelpURL("https://doc.photonengine.com/fusion/v2/manual/network-object")]
	[ScriptHelp(Url = "https://doc.photonengine.com/fusion/current/manual/network-object", BackColor = ScriptHeaderBackColor.Orange)]
	public class NetworkObject : Behaviour
	{
		public delegate bool ReplicateToDelegate(NetworkObject networkObject, PlayerRef player);

		public delegate int PriorityLevelDelegate(NetworkObject networkObject, PlayerRef player);

		[NonSerialized]
		internal unsafe int* Ptr;

		[NonSerialized]
		public bool IsResume;

		private NetworkRunner _runner;

		internal NetworkObjectMeta Meta;

		[HideInInspector]
		[SerializeField]
		public uint SortKey;

		[Obsolete("NetworkObject.SendPriority has been removed. Set per-player send priority on the Server with NetworkObject.SetPriority(player, priority) and clear it with NetworkObject.ClearPriority(player).", true)]
		[FormerlySerializedAs("Priority")]
		public int SendPriority = 1;

		[NonSerialized]
		[Obsolete("NetworkObject.SendPriorityCallback has been removed. Set per-player send priority on the Server with NetworkObject.SetPriority(player, priority) and clear it with NetworkObject.ClearPriority(player).", true)]
		public PriorityLevelDelegate SendPriorityCallback;

		[InlineHelp]
		public bool EnableInterpolation = true;

		[InlineHelp]
		public NetworkObjectFlags Flags = NetworkObjectFlags.DestroyWhenStateAuthorityLeaves;

		[NonSerialized]
		internal NetworkObjectRuntimeFlags RuntimeFlags;

		[NonSerialized]
		public NetworkObjectTypeId NetworkTypeId;

		[InlineHelp]
		public NetworkObject[] NestedObjects;

		[InlineHelp]
		public NetworkBehaviour[] NetworkedBehaviours;

		private RenderSource _renderSource;

		public bool ForceRemoteRenderTimeframe = false;

		internal unsafe ref NetworkObjectHeader Header
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref *(NetworkObjectHeader*)Ptr;
			}
		}

		internal unsafe Span<int> RawWords
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (Ptr == null) ? default(Span<int>) : new Span<int>(Ptr, Header.WordCount);
			}
		}

		public NetworkId Id
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return RawWords.IsEmpty ? default(NetworkId) : Header.Id;
			}
		}

		public NetworkRunner Runner
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _runner;
			}
		}

		public Tick LastReceiveTick
		{
			get
			{
				NetworkObjectMeta meta = Meta;
				return (meta != null && meta.HasSnapshots) ? Meta.SnapshotLatest.Tick : default(Tick);
			}
		}

		public bool IsRenderInterpolationReady
		{
			get
			{
				NetworkObjectMeta meta = Meta;
				Simulation simulation = Simulation;
				if (meta == null || simulation == null)
				{
					return false;
				}
				return Runner.RemoteRenderTime >= (float)(int)(meta.InitialTick ?? ((Tick)0)) * simulation.TickDeltaFloat;
			}
		}

		public string Name => Id.ToString() + (BehaviourUtils.IsAlive(this) ? ("(" + base.name + ")") : "");

		internal Simulation Simulation
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return BehaviourUtils.IsAlive(Runner) ? Runner._simulation : null;
			}
		}

		public bool IsValid => BehaviourUtils.IsAlive(Runner) && Runner.Exists(this);

		public bool IsInSimulation
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return RuntimeFlags.Has(NetworkObjectRuntimeFlags.InSimulation);
			}
		}

		internal Span<int> Data
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				Span<int> rawWords = RawWords;
				Span<int> result;
				if (!rawWords.IsEmpty)
				{
					rawWords = RawWords;
					result = rawWords.Slice(20, rawWords.Length - 20);
				}
				else
				{
					result = default(Span<int>);
				}
				return result;
			}
		}

		internal ReadOnlySpan<int> BehaviourChangedTickArray => (Meta != null) ? Meta.BehaviourChangedTickArray : default(Span<int>);

		public unsafe bool HasInputAuthority => Ptr != null && (Simulation?.IsLocalSimulationInputAuthority(ref Header) ?? false);

		public unsafe bool HasStateAuthority
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Ptr != null && (Simulation?.IsLocalSimulationStateAuthority(ref Header) ?? false);
			}
		}

		public bool IsProxy => Simulation != null && !Simulation.IsLocalSimulationInputAuthority(ref Header) && !Simulation.IsLocalSimulationStateAuthority(ref Header);

		public bool IsNested => RuntimeFlags.Has(NetworkObjectRuntimeFlags.IsNested);

		public unsafe NetworkObject NestingRoot
		{
			get
			{
				if (!IsNested || Runner == null || Ptr == null)
				{
					return null;
				}
				return Runner.FindObject(Header.NestingRoot);
			}
		}

		public RenderTimeframe RenderTimeframe
		{
			get
			{
				if (ForceRemoteRenderTimeframe)
				{
					return RenderTimeframe.Remote;
				}
				int result;
				if (!IsInSimulation)
				{
					Simulation simulation = Simulation;
					if (simulation == null || !simulation.IsLocalSimulationStateAuthority(ref Header))
					{
						result = 1;
						goto IL_0036;
					}
				}
				result = 0;
				goto IL_0036;
				IL_0036:
				return (RenderTimeframe)result;
			}
		}

		public RenderSource RenderSource
		{
			get
			{
				return _renderSource;
			}
			set
			{
				_renderSource = value;
			}
		}

		public float RenderTime
		{
			get
			{
				if (!BehaviourUtils.IsAlive(Runner))
				{
					return 0f;
				}
				if (RenderTimeframe == RenderTimeframe.Local)
				{
					return Runner.LocalRenderTime;
				}
				return Runner.RemoteRenderTime;
			}
		}

		public unsafe PlayerRef InputAuthority => (Ptr == null) ? PlayerRef.None : Header.InputAuthority;

		public unsafe PlayerRef StateAuthority
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (Ptr == null) ? PlayerRef.None : Runner.Simulation.GetStateAuthority(Header.StateAuthority);
			}
		}

		public bool IsSpawnable
		{
			get
			{
				return !Flags.IsIgnored();
			}
			set
			{
				Flags = Flags.SetIgnored(!value);
			}
		}

		public NetworkObjectInterestModes ObjectInterest
		{
			get
			{
				return Flags.GetInterestMode();
			}
			set
			{
				Flags = Flags.SetInterestMode(value);
			}
		}

		protected virtual void Awake()
		{
			Assert.Check(RuntimeFlags.HasNot(NetworkObjectRuntimeFlags.HadAwake), "RuntimeFlags.HasNot(NetworkObjectRuntimeFlags.HadAwake)");
			RuntimeFlags |= NetworkObjectRuntimeFlags.HadAwake;
			DebugAwake();
			if (RuntimeFlags.Has(NetworkObjectRuntimeFlags.NotAwakeWhenAttaching) && Id.IsValid)
			{
				if (BehaviourUtils.IsAlive(Runner))
				{
					Runner.AttachActivatedByUser(this);
				}
				else
				{
					InternalLogStreams.LogDebug?.Warn(this, "Expected to be activated while the runner is active");
				}
			}
		}

		public bool SetPriority(PlayerRef player, int priority)
		{
			if (Simulation.Topology == Topologies.Shared)
			{
				bool flag = player == Simulation.LocalPlayer;
				Assert.Check(flag, "In Shared Mode priority can only be set for the local player");
				if (!flag)
				{
					return false;
				}
				return Simulation.SetPriorityOfObjectForLocalPlayerInSharedMode(Id, Math.Max(0, priority));
			}
			bool isServer = Simulation.IsServer;
			Assert.Check(isServer, "In Client/Server priority can only be set on the Server (Host or Dedicated)");
			if (!isServer)
			{
				return false;
			}
			if (player == Simulation.LocalPlayer)
			{
				return true;
			}
			if (!Simulation.TryGetSimulationConnectionForPlayer(player, out var result))
			{
				return false;
			}
			result.GetObjectData(Meta.Id, create: true).PriorityLevel = priority;
			return true;
		}

		public bool ClearPriority(PlayerRef player)
		{
			return SetPriority(player, 1);
		}

		protected virtual void OnDestroy()
		{
			OnDestroyInternal();
			DebugOnDestroy(wasActive: true);
		}

		internal void OnDestroyNeverActive()
		{
			Assert.Check(RuntimeFlags.HasNot(NetworkObjectRuntimeFlags.HadAwake), "Object was not supposed to be activated {0}", LogUtils.GetDump(this));
			Assert.Check(RuntimeFlags.Has(NetworkObjectRuntimeFlags.NotAwakeWhenAttaching), "Expected to have the flag {0}", LogUtils.GetDump(this));
			OnDestroyInternal();
			Assert.Check(RuntimeFlags.HasNot(NetworkObjectRuntimeFlags.Spawned), "Never should have become active");
			DebugOnDestroy(wasActive: false);
		}

		private unsafe void OnDestroyInternal()
		{
			if (BehaviourUtils.IsAlive(this))
			{
				RuntimeFlags |= NetworkObjectRuntimeFlags.IsDestroyed;
				if (BehaviourUtils.IsAlive(Runner))
				{
					bool flag = Ptr != null && Runner.Simulation != null && (HasStateAuthority || (StateAuthority == PlayerRef.None && Runner.Simulation.IsMasterClient));
					Runner.DestroyNetworkObjectInternal(this, (NetworkObjectDestroyFlags)(1 | (flag ? 2 : 0)));
				}
				else if (BehaviourUtils.IsNotNull(Runner) && Id.IsValid)
				{
					InternalLogStreams.LogDebug?.Warn(this, "Runner has been destroyed, but the object has not been despawned.");
				}
				Ptr = null;
			}
		}

		public bool ResetToLatestState()
		{
			if (HasStateAuthority || Runner.IsResimulation)
			{
				return false;
			}
			if (!Meta.HasSnapshots)
			{
				return false;
			}
			Meta.SnapshotLatest.CopyTo(Meta);
			return true;
		}

		internal unsafe void ResetNetworkState()
		{
			MakeUnowned();
			Ptr = default(int*);
			RuntimeFlags &= ~NetworkObjectRuntimeFlags.ClearMask;
		}

		internal unsafe void Defaults()
		{
			Assert.Check(Ptr != null, "Ptr != null");
			Header.InputAuthority = default(PlayerRef);
			Header.StateAuthority = default(PlayerRef);
		}

		public static int GetWordCount(NetworkObject obj)
		{
			if (BehaviourUtils.IsAlive(obj))
			{
				int num = GetBehaviourDataOffset(obj);
				if (obj.NetworkedBehaviours != null)
				{
					for (int i = 0; i < obj.NetworkedBehaviours.Length; i++)
					{
						if (BehaviourUtils.IsAlive(obj.NetworkedBehaviours[i]))
						{
							num += NetworkBehaviourUtils.GetWordCount(obj.NetworkedBehaviours[i]);
							continue;
						}
						throw new Exception("Found missing NetworkBehaviour reference in NetworkBehaviours[] list on " + obj.Name + ". Re-baking of object required. Please check prefab or scene object and make sure NetworkBehaviour list is up to date.");
					}
					num += obj.NetworkedBehaviours.Length;
				}
				return num;
			}
			return 0;
		}

		internal static int GetBehaviourDataOffset(NetworkObjectFlags flags)
		{
			int num = 20;
			if (flags.HasNot(NetworkObjectFlags.HasMainNetworkTRSP))
			{
				num += 14;
			}
			return num;
		}

		internal static int GetBehaviourDataOffset(NetworkObject networkObject)
		{
			int behaviourDataOffset = GetBehaviourDataOffset(networkObject.Flags);
			if (behaviourDataOffset != 20 && networkObject.NetworkedBehaviours != null && networkObject.NetworkedBehaviours.Length != 0 && networkObject.NetworkedBehaviours[0] is NetworkTRSP)
			{
				string text = string.Empty;
				if (networkObject.NetworkedBehaviours.Length != 0)
				{
					text = networkObject.NetworkedBehaviours[0].ToString();
				}
				InternalLogStreams.LogError?.Log($"offset: {behaviourDataOffset}, networkObject.NetworkedBehaviours: {networkObject.NetworkedBehaviours}, networkObject.NetworkedBehaviours[0]: {text}, Flags: {networkObject.Flags:X}");
			}
			Assert.Check(behaviourDataOffset == 20 || networkObject.NetworkedBehaviours == null || networkObject.NetworkedBehaviours.Length == 0 || !(networkObject.NetworkedBehaviours[0] is NetworkTRSP), "offset == NetworkObjectHeader.WORDS || networkObject.NetworkedBehaviours == null || networkObject.NetworkedBehaviours.Length == 0 || networkObject.NetworkedBehaviours[0] is not NetworkTRSP");
			return behaviourDataOffset;
		}

		public int GetLocalAuthorityMask()
		{
			return Simulation?.GetLocalAuthorityMask(ref Header) ?? 0;
		}

		public void AssignInputAuthority(PlayerRef player)
		{
			Assert.Check(BehaviourUtils.IsAlive(Runner), "IsAlive(Runner)");
			Assert.Check(Runner.Exists(this), "Runner.Exists(this)");
			if (!Runner.CanAssignInputAuthority(this, player))
			{
				InternalLogStreams.LogDebug?.Warn(this, $"Unable to assign Input Authority to {player}");
				return;
			}
			PlayerRef inputAuthority = Header.InputAuthority;
			Header.InputAuthority = player;
			if ((Simulation.Config.ReplicationFeatures & NetworkProjectConfig.ReplicationFeatures.InterestManagement) == NetworkProjectConfig.ReplicationFeatures.InterestManagement)
			{
				if (inputAuthority.IsRealPlayer && Simulation.TryGetSimulationConnectionForPlayer(inputAuthority, out var result))
				{
					result.RemoveAlwaysInterested(Meta);
				}
				if (player.IsRealPlayer && Simulation.TryGetSimulationConnectionForPlayer(player, out var result2))
				{
					result2.AddAlwaysInterested(Meta);
				}
			}
		}

		public bool RequestStateAuthority()
		{
			Assert.Check(BehaviourUtils.IsAlive(Runner), "IsAlive(Runner)");
			Assert.Check(Runner.Exists(this), "Runner.Exists(this)");
			if (Runner?.Simulation != null)
			{
				return Runner.Simulation.RequestStateAuthority(Id, wants: true);
			}
			return false;
		}

		public bool ReleaseStateAuthority()
		{
			Assert.Check(BehaviourUtils.IsAlive(Runner), "IsAlive(Runner)");
			Assert.Check(Runner.Exists(this), "Runner.Exists(this)");
			if (Runner?.Simulation != null)
			{
				return Runner.Simulation.RequestStateAuthority(Id, wants: false);
			}
			return false;
		}

		public void RemoveInputAuthority()
		{
			AssignInputAuthority(default(PlayerRef));
		}

		public static implicit operator NetworkId(NetworkObject obj)
		{
			return BehaviourUtils.IsNull(obj) ? default(NetworkId) : obj.Id;
		}

		public bool SetPlayerAlwaysInterested(PlayerRef player, bool alwaysInterested)
		{
			if (!HasStateAuthority)
			{
				return false;
			}
			Runner.Simulation.SetPlayerAlwaysInterested(player, this, alwaysInterested);
			return true;
		}

		public unsafe void CopyStateFrom(NetworkObject source)
		{
			Assert.Check(source.Id.IsValid, "Invalid NetworkId from source NetworkObject");
			Assert.Check(source.Id.Equals(Id), "NetworkObjects must have the same NetworkIds");
			Assert.Check(Ptr != null, "Ptr != null");
			Assert.Check(source.Ptr != null, "source.Ptr != null");
			Assert.Check(Header.Type.Equals(source.Header.Type), "NetworkObjects must be of the same type");
			FusionUnsafe.Copy(Data, source.Data);
			for (int i = 0; i < NestedObjects.Length; i++)
			{
				NestedObjects[i].CopyStateFrom(source.NestedObjects[i]);
			}
		}

		public unsafe void CopyStateFrom(NetworkObjectHeaderPtr source)
		{
			Assert.Check(Ptr != null, "Ptr != null");
			Assert.Check(Header.Type.Equals(source.Ptr->Type), "NetworkObjects must be of the same type");
			FusionUnsafe.Copy(Data, source.Data);
		}

		[Obsolete("Use NetworkWrap(NetworkObject) instead")]
		public static NetworkId NetworkWrap(NetworkRunner runner, NetworkObject obj)
		{
			return NetworkWrap(obj);
		}

		[NetworkSerializeMethod]
		public static NetworkId NetworkWrap(NetworkObject obj)
		{
			if (BehaviourUtils.IsNotAlive(obj))
			{
				return default(NetworkId);
			}
			return obj.Id;
		}

		[NetworkDeserializeMethod]
		public static void NetworkUnwrap(NetworkRunner runner, NetworkId wrapper, ref NetworkObject result)
		{
			if (!wrapper.IsValid)
			{
				result = null;
			}
			else if (!runner.TryFindObject(wrapper, out result))
			{
				runner.InvokeFailedToResolveNetworkObject(wrapper);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void MakeOwned(NetworkRunner runner)
		{
			Assert.Check(_runner == null, "Already owned {0}", _runner);
			_runner = runner;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void MakeUnowned()
		{
			_runner = null;
		}

		private void DebugAwake()
		{
			InternalLogStreams.LogTraceObject?.Log(this, $"Awake ({RuntimeFlags})");
			if (RuntimeFlags.Has(NetworkObjectRuntimeFlags.Spawned))
			{
				InternalLogStreams.LogError?.Log(this, "Spawned before Awake");
			}
		}

		private void DebugOnDestroy(bool wasActive)
		{
			InternalLogStreams.LogTraceObject?.Log(this, $"OnDestroy ({RuntimeFlags})");
			if (wasActive)
			{
				if (RuntimeFlags.Has(NetworkObjectRuntimeFlags.Spawned))
				{
					InternalLogStreams.LogError?.Log(this, "Not despawned before OnDestroy");
				}
			}
			else if (RuntimeFlags.Has(NetworkObjectRuntimeFlags.HadAwake))
			{
				InternalLogStreams.LogError?.Log(this, "Should not have been awoken");
			}
		}

		protected internal override void GetDumpString(StringBuilder builder)
		{
			builder.Append("[");
			builder.Append(base.DebugNameThreadSafe);
			if (Id.IsValid)
			{
				builder.Append(" ");
				builder.Append(Id.ToString());
			}
			int length = builder.Length;
			if (NetworkRunner.TryGetPrettyRunnerName(builder, Runner))
			{
				builder.Insert(length, "@");
			}
			builder.Append("]");
		}
	}
}
