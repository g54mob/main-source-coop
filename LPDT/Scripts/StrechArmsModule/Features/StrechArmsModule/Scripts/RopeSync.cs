using System;
using Fusion;
using Obi;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(2400)]
	public class RopeSync : NetworkBehaviour
	{
		[SerializeField]
		private ObiRope _rope;

		[SerializeField]
		private ObiActor _obiActor;

		[WeaverGenerated]
		[DefaultForProperty("ServerSendStates", 0, 2400)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private RopeState[] _ServerSendStates;

		[Networked]
		[Capacity(200)]
		[NetworkedWeaved(0, 2400)]
		[NetworkedWeavedArray(200, 12, typeof(ElementReaderWriterUnmanaged<RopeState, MetaConstant12>))]
		private unsafe NetworkArray<RopeState> ServerSendStates
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RopeSync.ServerSendStates. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkArray<RopeState>((byte*)Ptr + 0, 200, ElementReaderWriterUnmanaged<RopeState, MetaConstant12>.GetInstance());
			}
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
		}

		private void EndStep(ObiActor actor, float stepTime, float substepTime)
		{
			if (base.Object.HasInputAuthority)
			{
				for (int i = 0; i < _rope.particleCount; i++)
				{
					int particleRuntimeIndex = _rope.GetParticleRuntimeIndex(i);
					NetworkArray<RopeState> serverSendStates = ServerSendStates;
					serverSendStates[i] = new RopeState
					{
						position = _rope.solver.positions[particleRuntimeIndex],
						velocity = _rope.solver.velocities[particleRuntimeIndex],
						externalForces = _rope.solver.externalForces[particleRuntimeIndex],
						externalTorques = _rope.solver.externalTorques[particleRuntimeIndex]
					};
				}
			}
			else
			{
				for (int j = 0; j < _rope.particleCount; j++)
				{
					int particleRuntimeIndex2 = _rope.GetParticleRuntimeIndex(j);
					RopeState ropeState = ServerSendStates[j];
					_rope.solver.positions[particleRuntimeIndex2] = ropeState.position;
					_rope.solver.velocities[particleRuntimeIndex2] = ropeState.velocity;
					_rope.solver.externalForces[particleRuntimeIndex2] = ropeState.externalForces;
					_rope.solver.externalTorques[particleRuntimeIndex2] = ropeState.externalTorques;
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetworkBehaviourUtils.InitializeNetworkArray(ServerSendStates, _ServerSendStates, "ServerSendStates");
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			NetworkBehaviourUtils.CopyFromNetworkArray(ServerSendStates, ref _ServerSendStates);
		}
	}
}
