using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.MultiplayerSessionServices.Scripts.Networked
{
	[NetworkBehaviourWeaved(31)]
	public class PlayerJoinSourcesNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Sources", 0, 31)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SerializableDictionary<int, JoinSource> _Sources;

		[Networked]
		[Capacity(4)]
		[OnChangedRender("OnSourcesChangedRender")]
		[NetworkedWeaved(0, 31)]
		[NetworkedWeavedDictionary(7, 1, 1, typeof(ElementReaderWriterUnmanaged<int, MetaConstant1>), typeof(ElementReaderWriterUnmanaged<JoinSource, MetaConstant1>))]
		public unsafe NetworkDictionary<int, JoinSource> Sources
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerJoinSourcesNetworkObject.Sources. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkDictionary<int, JoinSource>((int*)((byte*)Ptr + 0), 7, ElementReaderWriterUnmanaged<int, MetaConstant1>.GetInstance(), ElementReaderWriterUnmanaged<JoinSource, MetaConstant1>.GetInstance());
			}
		}

		public event Action<IReadOnlyDictionary<int, JoinSource>> OnNetworkedSourcesChanged;

		public event Action OnAuthoritativeTick;

		public event Action OnDespawned;

		public event Action<int> OnReportSignalReceived;

		[Inject]
		public void InjectDependencies(INetworkedModelInstanceProvider networkedModelInstanceProvider)
		{
			_networkedModelInstanceProvider = networkedModelInstanceProvider;
		}

		public override void Spawned()
		{
			_networkedModelInstanceProvider?.Register(this);
			this.OnNetworkedSourcesChanged?.Invoke(ReadSources());
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_networkedModelInstanceProvider?.Unregister(this);
			this.OnDespawned?.Invoke();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				this.OnAuthoritativeTick?.Invoke();
			}
		}

		public bool TryWriteSources(IReadOnlyDictionary<int, JoinSource> sources)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			List<int> list = null;
			foreach (KeyValuePair<int, JoinSource> source in Sources)
			{
				if (!sources.ContainsKey(source.Key))
				{
					(list ?? (list = new List<int>())).Add(source.Key);
				}
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Sources.Remove(list[i]);
				}
			}
			foreach (KeyValuePair<int, JoinSource> source2 in sources)
			{
				Sources.Set(source2.Key, source2.Value);
			}
			return true;
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 603533366u)]
		public void RpcRaiseReportSignal([RpcPayload(4)] int value)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(603533366u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.MultiplayerSessionServices.Scripts.Networked.PlayerJoinSourcesNetworkObject::RpcRaiseReportSignal(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(value, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			this.OnReportSignalReceived?.Invoke(value);
		}

		private void OnSourcesChangedRender()
		{
			this.OnNetworkedSourcesChanged?.Invoke(ReadSources());
		}

		public Dictionary<int, JoinSource> ReadSources()
		{
			Dictionary<int, JoinSource> dictionary = new Dictionary<int, JoinSource>(Sources.Count);
			foreach (KeyValuePair<int, JoinSource> source in Sources)
			{
				dictionary[source.Key] = source.Value;
			}
			return dictionary;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetworkBehaviourUtils.InitializeNetworkDictionary(Sources, _Sources, "Sources");
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			NetworkBehaviourUtils.CopyFromNetworkDictionary(Sources, ref _Sources);
		}

		[NetworkRpcWeavedInvoker(603533366u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RpcRaiseReportSignal_0040Invoker603533366([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerJoinSourcesNetworkObject)context.TargetBehaviour).RpcRaiseReportSignal(value);
		}
	}
}
