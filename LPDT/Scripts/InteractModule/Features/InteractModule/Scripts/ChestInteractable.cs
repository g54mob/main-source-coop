using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AnimationModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.InteractModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class ChestInteractable : InteractableBase
	{
		[SerializeField]
		private Animator _chestAnimator;

		[SerializeField]
		private OnAnimationEndedFunctionReactor _onAnimationEndedFunctionReactor;

		[SerializeField]
		private Rigidbody _rb;

		[SerializeField]
		private float _flipDuration = 0.6f;

		[SerializeField]
		private AnimationCurve _liftCurve;

		[SerializeField]
		private float _liftHeight = 1f;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsOpen", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsOpen = true;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsFlipping", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsFlipping;

		private bool _isPendingInteraction;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe bool IsOpen
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChestInteractable.IsOpen. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChestInteractable.IsOpen. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe bool IsFlipping
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChestInteractable.IsFlipping. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChestInteractable.IsFlipping. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		public override void Interact()
		{
			base.Interact();
			if (base.Object.HasStateAuthority)
			{
				ProcessInteraction();
				return;
			}
			_isPendingInteraction = true;
			base.Object.RequestStateAuthority();
		}

		public override void StateAuthorityChanged()
		{
			ClearLocalPendingInteractionUnlessAuthority(ref _isPendingInteraction);
			base.StateAuthorityChanged();
			if (base.HasStateAuthority && _isPendingInteraction)
			{
				ProcessInteraction();
			}
		}

		private void ProcessInteraction()
		{
			_isPendingInteraction = false;
			if (!IsFlipping)
			{
				if (Vector3.Dot(base.transform.up, Vector3.up) < 0.95f)
				{
					StartCoroutine(FlipRoutine());
				}
				else
				{
					OpenChestRpc();
				}
			}
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			_onAnimationEndedFunctionReactor.OnAnimationEnded += OnAnimationEnded;
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_onAnimationEndedFunctionReactor.OnAnimationEnded -= OnAnimationEnded;
		}

		private void OnAnimationEnded()
		{
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 804515876u)]
		private void OpenChestRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(804515876u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.InteractModule.Scripts.ChestInteractable::OpenChestRpc()", invokeInfo, PlayerRef.None);
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
			_chestAnimator.SetTrigger("Open");
		}

		private Quaternion CalculateUprightRotation()
		{
			Vector3 forward = base.transform.forward;
			forward = Vector3.ProjectOnPlane(forward, Vector3.up);
			if (forward.sqrMagnitude < 0.001f)
			{
				forward = base.transform.right;
			}
			return Quaternion.LookRotation(forward.normalized, Vector3.up);
		}

		private IEnumerator FlipRoutine()
		{
			IsFlipping = true;
			_rb.isKinematic = true;
			Vector3 startPos = base.transform.position;
			float groundY = startPos.y;
			Quaternion startRot = base.transform.rotation;
			Quaternion targetRot = CalculateUprightRotation();
			float time = 0f;
			while (time < _flipDuration)
			{
				float num = time / _flipDuration;
				float num2 = _liftCurve.Evaluate(num) * _liftHeight;
				Vector3 position = new Vector3(startPos.x, groundY + num2, startPos.z);
				_rb.MovePosition(position);
				_rb.MoveRotation(Quaternion.Slerp(startRot, targetRot, num));
				time += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			base.transform.position = new Vector3(startPos.x, groundY, startPos.z);
			base.transform.rotation = targetRot;
			_rb.isKinematic = false;
			_rb.linearVelocity = Vector3.zero;
			IsFlipping = false;
			OpenChestRpc();
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			IsOpen = _IsOpen;
			IsFlipping = _IsFlipping;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_IsOpen = IsOpen;
			_IsFlipping = IsFlipping;
		}

		[NetworkRpcWeavedInvoker(804515876u)]
		[Preserve]
		[WeaverGenerated]
		protected static void OpenChestRpc_0040Invoker804515876([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ChestInteractable)context.TargetBehaviour).OpenChestRpc();
		}
	}
}
