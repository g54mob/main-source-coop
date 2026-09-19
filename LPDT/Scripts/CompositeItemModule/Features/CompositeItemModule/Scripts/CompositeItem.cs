using System;
using System.Collections;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.CompositeItemModule.Scripts
{
	public class CompositeItem : MonoBehaviour, IDisplayedCurrencyOverride
	{
		[SerializeField]
		private CompositeItemGroup _group;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private float _collisionCooldown = 1f;

		[SerializeField]
		private float _initialCollisionCooldown = 5f;

		private IPointGrabable _grabable;

		private float _nextCollisionTime;

		private bool _isNotCollidable;

		private bool _isBroken;

		public CompositeItemGroup Group => _group;

		public Rigidbody Rigidbody => _rigidbody;

		public bool IsBroken => _isBroken;

		public object DisplayedCurrencyGroupKey
		{
			get
			{
				if (!(_group != null))
				{
					return null;
				}
				return _group.ClusterKeyOf(this);
			}
		}

		public event Action OnBroken;

		private void Awake()
		{
			if (_group == null)
			{
				_group = GetComponentInParent<CompositeItemGroup>();
			}
			if (_rigidbody == null)
			{
				_rigidbody = GetComponent<Rigidbody>();
			}
			_nextCollisionTime = Time.time + _initialCollisionCooldown;
			_grabable = GetComponent<IPointGrabable>();
			if (_grabable != null)
			{
				_grabable.OnGrabbedPlayersChanged += OnGrabbedPlayersChanged;
			}
		}

		private void OnDestroy()
		{
			if (_grabable != null)
			{
				_grabable.OnGrabbedPlayersChanged -= OnGrabbedPlayersChanged;
			}
		}

		private void OnGrabbedPlayersChanged()
		{
			_grabable.OnGrabbedPlayersChanged -= OnGrabbedPlayersChanged;
			StartCoroutine(NoCollisionCoroutine());
		}

		private IEnumerator NoCollisionCoroutine()
		{
			float timer = 0f;
			_isNotCollidable = true;
			while (timer < _collisionCooldown)
			{
				timer += Time.deltaTime;
				yield return null;
			}
			_isNotCollidable = false;
		}

		public bool TryGetDisplayedCurrency(out int currency)
		{
			currency = 0;
			if (_group != null)
			{
				return _group.TryGetClusterCurrency(this, out currency);
			}
			return false;
		}

		public void NotifyBroken()
		{
			if (!_isBroken)
			{
				_isBroken = true;
				this.OnBroken?.Invoke();
			}
		}

		private void OnCollisionEnter(Collision other)
		{
			if (!(_group == null) && !_isNotCollidable && !(Time.time < _nextCollisionTime) && !IsSameGroup(other))
			{
				_nextCollisionTime = Time.time + _collisionCooldown;
				_group.ReportImpact(this, other.relativeVelocity.magnitude);
			}
		}

		private bool IsSameGroup(Collision other)
		{
			if (other.rigidbody == null)
			{
				return false;
			}
			if (other.rigidbody.TryGetComponent<CompositeItem>(out var component))
			{
				return component._group == _group;
			}
			return false;
		}
	}
}
