using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.StoreModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class CardsJumper : NetworkBehaviour
	{
		[SerializeField]
		private AnimationCurve _jumpCurve;

		[SerializeField]
		private Transform _targetTransform;

		[SerializeField]
		private LayerMask _layerMask;

		[SerializeField]
		private EventReference _soundReference;

		[SerializeField]
		private float _jumpDuration = 0.5f;

		[SerializeField]
		private float _jumpHeight = 2f;

		[SerializeField]
		private float _targetYOffset = 0.5f;

		[SerializeField]
		private float _targetRadius = 1.5f;

		private readonly List<StoreCardBehaviour> _cardsInJump = new List<StoreCardBehaviour>();

		private IAudioService _audioService;

		private Dictionary<StoreCardBehaviour, Coroutine> _cardJumpRoutines = new Dictionary<StoreCardBehaviour, Coroutine>();

		public event Action<StoreCardBehaviour> OnCardJumpFinished;

		[Inject]
		public void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		public void JumpCardToTarget(StoreCardBehaviour storeCard, Transform target, float radius)
		{
			ProcessCardJump(storeCard, target, canJumpFromAnywhere: true, radius);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (((1 << other.gameObject.layer) & _layerMask.value) != 0 && other.TryGetComponent<StoreCardBehaviour>(out var component) && !_cardsInJump.Contains(component))
			{
				ProcessCardJump(component, _targetTransform, canJumpFromAnywhere: false, _targetRadius);
			}
		}

		private void ProcessCardJump(StoreCardBehaviour storeCard, Transform target, bool canJumpFromAnywhere, float distributionRadius)
		{
			if (!storeCard.Object.HasStateAuthority || (_cardsInJump.Contains(storeCard) && !canJumpFromAnywhere))
			{
				return;
			}
			if (_cardsInJump.Contains(storeCard))
			{
				if (!canJumpFromAnywhere)
				{
					return;
				}
				StopCoroutine(_cardJumpRoutines[storeCard]);
				_cardsInJump.Remove(storeCard);
			}
			_cardsInJump.Add(storeCard);
			PlaySoundRpc(storeCard.SoundSourceBehaviour.SoundSourceTransform.position, storeCard.SoundSourceBehaviour.ID);
			_cardJumpRoutines[storeCard] = StartCoroutine(CardJump(storeCard, target, distributionRadius));
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3681104216u)]
		private void PlaySoundRpc([RpcPayload(12)] Vector3 position, [RpcPayload(4)] int id)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3681104216u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StoreModule.Scripts.CardsJumper::PlaySoundRpc(UnityEngine.Vector3,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(position, 12);
						writer.Write(id, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_audioService.PlayOneShot(_soundReference, new GenericSoundSource(position, id));
		}

		private IEnumerator CardJump(StoreCardBehaviour storeCard, Transform target, float distributionRadius)
		{
			Rigidbody rb = storeCard.Rigidbody;
			Vector3 startPosition = rb.position;
			Vector3 position = target.position;
			Vector2 vector = UnityEngine.Random.insideUnitCircle * distributionRadius;
			Vector3 targetPosition = new Vector3(position.x + vector.x, position.y, position.z + vector.y);
			float elapsed = 0f;
			while (elapsed < _jumpDuration)
			{
				elapsed += Time.fixedDeltaTime;
				float num = Mathf.Clamp01(elapsed / _jumpDuration);
				Vector3 position2 = Vector3.Lerp(startPosition, targetPosition, num);
				float num2 = Mathf.Lerp(startPosition.y, targetPosition.y, num);
				float num3 = _jumpCurve.Evaluate(num) * _jumpHeight;
				position2.y = num2 + num3;
				rb.MovePosition(position2);
				yield return new WaitForFixedUpdate();
			}
			rb.linearVelocity = Vector3.zero;
			_cardsInJump.Remove(storeCard);
			this.OnCardJumpFinished?.Invoke(storeCard);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(3681104216u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlaySoundRpc_0040Invoker3681104216([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out Vector3 value, 12);
			payloadReader.Read(out int value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CardsJumper)context.TargetBehaviour).PlaySoundRpc(value, value2);
		}
	}
}
