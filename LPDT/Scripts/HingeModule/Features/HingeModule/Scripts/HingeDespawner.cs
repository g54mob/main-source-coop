using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.ObjectDespawnModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.HingeModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class HingeDespawner : NetworkBehaviour, IHingeInitializable
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private HingeJoint _hingeJoint;

		[SerializeField]
		private float _refreshPositionOnDistanceX;

		[SerializeField]
		private float _refreshPositionOnDistanceY;

		[SerializeField]
		private float _refreshPositionOnDistanceZ;

		[SerializeField]
		private bool _isWithBreakRotationX = true;

		[SerializeField]
		private float _breakRotationX;

		[SerializeField]
		private bool _isWithBreakRotationY;

		[SerializeField]
		private float _breakRotationY;

		[SerializeField]
		private bool _isWithBreakRotationZ = true;

		[SerializeField]
		private float _breakRotationZ;

		[SerializeField]
		private float _breakDespawnTime;

		[SerializeField]
		private float _breakForce;

		[SerializeField]
		private float _returnToStartDuration;

		private ObjectDespawnModel _objectDespawnModel;

		private Coroutine _returnToStartRoutine;

		private Quaternion _startRotation;

		private bool _isDespawnProcessStarted;

		private bool _initialized;

		private bool _isReturningToStart;

		private bool _broken;

		private IChestScreamerLidGate _chestScreamerLidGate;

		public event Action OnBreakEvent;

		public override void Spawned()
		{
			_initialized = true;
		}

		[Inject]
		private void InjectDependency(ObjectDespawnModel objectDespawnModel)
		{
			_objectDespawnModel = objectDespawnModel;
		}

		private void Awake()
		{
			_startRotation = base.transform.rotation;
			_chestScreamerLidGate = GetComponent<IChestScreamerLidGate>();
		}

		private void FixedUpdate()
		{
			if (_initialized && base.Object.HasStateAuthority && _hingeJoint != null && !_rigidbody.IsSleeping())
			{
				Vector3 eulerAngles = (base.transform.rotation * Quaternion.Inverse(_startRotation)).eulerAngles;
				float value = NormalizeAngle(eulerAngles.x);
				float value2 = NormalizeAngle(eulerAngles.y);
				float value3 = NormalizeAngle(eulerAngles.z);
				if (_chestScreamerLidGate != null && _chestScreamerLidGate.ShouldSuppressHingeAngleBreak)
				{
					return;
				}
				if ((_isWithBreakRotationX && Math.Abs(value) > _breakRotationX) || (_isWithBreakRotationY && Math.Abs(value2) > _breakRotationY) || (_isWithBreakRotationZ && Math.Abs(value3) > _breakRotationZ))
				{
					OnBreakRPC();
					return;
				}
			}
			if (_initialized && _hingeJoint == null && !_isDespawnProcessStarted)
			{
				StartDespawnProcess();
			}
		}

		private void OnCollisionEnter(Collision other)
		{
			if (_initialized && base.Object == null && base.Object.HasStateAuthority && !(_hingeJoint != null) && other.relativeVelocity.magnitude > _breakForce)
			{
				FastDestroy();
			}
		}

		private void FastDestroy()
		{
			if (base.Object != null && base.Object.IsValid && base.Object.HasStateAuthority)
			{
				_objectDespawnModel.AddObjectToDespawn(new DespawnObjectData(base.Object));
			}
		}

		private float NormalizeAngle(float angle)
		{
			while (angle > 180f)
			{
				angle -= 360f;
			}
			while (angle < -180f)
			{
				angle += 360f;
			}
			return angle;
		}

		private void OnJointBreak(float breakForce)
		{
			if (_initialized)
			{
				OnBreakRPC();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3990620935u)]
		private void OnBreakRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3990620935u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.HingeModule.Scripts.HingeDespawner::OnBreakRPC()", invokeInfo, PlayerRef.None);
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
			if (_initialized)
			{
				OnBreak();
			}
		}

		private void OnBreak()
		{
			if (_initialized && !_broken)
			{
				_broken = true;
				if (_hingeJoint != null)
				{
					UnityEngine.Object.Destroy(_hingeJoint);
				}
				this.OnBreakEvent?.Invoke();
				if (_returnToStartRoutine != null)
				{
					StopCoroutine(_returnToStartRoutine);
				}
				_rigidbody.useGravity = true;
			}
		}

		private void StartDespawnProcess()
		{
			if (!_isDespawnProcessStarted && (_chestScreamerLidGate == null || !_chestScreamerLidGate.ShouldSuppressHingeDespawn))
			{
				_isDespawnProcessStarted = true;
				StartCoroutine(DespawnAfterDelay());
			}
		}

		private IEnumerator DespawnAfterDelay()
		{
			yield return new WaitForSeconds(_breakDespawnTime);
			if (base.Object != null && base.Object.IsValid && base.Object.HasStateAuthority)
			{
				_objectDespawnModel.AddObjectToDespawn(new DespawnObjectData(base.Object));
			}
		}

		public void InitializeHingeJoint(HingeJoint joint)
		{
			_hingeJoint = joint;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(3990620935u)]
		[Preserve]
		[WeaverGenerated]
		protected static void OnBreakRPC_0040Invoker3990620935([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HingeDespawner)context.TargetBehaviour).OnBreakRPC();
		}
	}
}
