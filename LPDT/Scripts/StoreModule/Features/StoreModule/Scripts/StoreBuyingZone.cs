using System;
using System.Collections.Generic;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreBuyingZone : MonoBehaviour
	{
		[SerializeField]
		private Transform _targetTransform;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private EventReference _closeSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private List<Transform> _cardPoint;

		[SerializeField]
		private Collider _zoneTriggerCollider;

		[SerializeField]
		private LayerMask _layerMask;

		public Light SuccessLight;

		public Light FailureLight;

		private bool _isClosed;

		private IAudioService _audioService;

		private static readonly int _idleStateHash = Animator.StringToHash("Idle");

		private static readonly int _closeStateHash = Animator.StringToHash("CloseAnimation");

		[field: SerializeField]
		public SoundSourceBehaviour SoundSourceBehaviour { get; private set; }

		public List<StoreCardBehaviour> CardsInZone { get; private set; } = new List<StoreCardBehaviour>();

		public List<Transform> CardPoint => _cardPoint;

		public bool IsClosed => _isClosed;

		public Transform TargetTransform => _targetTransform;

		public event Action<StoreBuyingZone, StoreCardBehaviour> OnCardInStoreZone;

		public event Action<StoreBuyingZone, StoreCardBehaviour> OnCardInStoreZoneExit;

		[Inject]
		public void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		public bool ContainsCard(StoreCardBehaviour storeCardBehaviour)
		{
			if (storeCardBehaviour == null || _zoneTriggerCollider == null)
			{
				return false;
			}
			if (!storeCardBehaviour.TryGetBuyingZoneCheckBounds(out var bounds))
			{
				return false;
			}
			return _zoneTriggerCollider.bounds.Intersects(bounds);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!IsClosed && IsOnWatchedLayer(other) && other.TryGetComponent<StoreCardBehaviour>(out var component))
			{
				NotifyCardEntered(component);
			}
		}

		public void NotifyCardEntered(StoreCardBehaviour storeCard)
		{
			if (!IsClosed)
			{
				this.OnCardInStoreZone?.Invoke(this, storeCard);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!IsClosed && IsOnWatchedLayer(other) && other.TryGetComponent<StoreCardBehaviour>(out var component))
			{
				this.OnCardInStoreZoneExit?.Invoke(this, component);
			}
		}

		private bool IsOnWatchedLayer(Collider other)
		{
			return ((1 << other.gameObject.layer) & _layerMask.value) != 0;
		}

		public void CloseBuyingZone()
		{
			_isClosed = true;
			if (!(_animator == null) && !IsInAnimatorState(_closeStateHash))
			{
				_animator.Play(_closeStateHash, 0, 0f);
			}
		}

		public void OpenBuyingZone()
		{
			_isClosed = false;
			if (!(_animator == null) && !IsInAnimatorState(_idleStateHash))
			{
				_animator.ResetTrigger("Close");
				_animator.Play(_idleStateHash, 0, 0f);
			}
		}

		private bool IsInAnimatorState(int stateHash)
		{
			return _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == stateHash;
		}

		public void OnCloseAnimationEnd()
		{
			_audioService.PlayOneShotAttached(_closeSound, _soundSourceBehaviour);
		}
	}
}
