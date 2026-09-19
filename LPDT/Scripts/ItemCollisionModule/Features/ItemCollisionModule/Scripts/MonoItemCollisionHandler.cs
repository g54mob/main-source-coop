using System.Collections;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.ItemCollisionModule.Scripts
{
	public class MonoItemCollisionHandler : MonoBehaviour, IItemCollisionHandler, ICollisionDamageSuppressible
	{
		[SerializeField]
		private LayerMask _ignoreCollisionMask;

		[SerializeField]
		private MonoItem _monoItem;

		[SerializeField]
		private float _collisionCooldown = 1f;

		[SerializeField]
		private float _initialCollisionCooldown = 5f;

		private IPointGrabable _grabable;

		private IItemCollisionService _itemCollisionService;

		private float _lastCollisionTime = float.MinValue;

		private bool _isNotCollidable;

		[Inject]
		private void InjectDependencies(IItemCollisionService itemCollisionService)
		{
			_itemCollisionService = itemCollisionService;
		}

		private void Awake()
		{
			_grabable = GetComponent<IPointGrabable>();
			if (_grabable != null)
			{
				_grabable.OnGrabbedPlayersChanged += OnGrabbedPlayersChanged;
			}
			SuppressCollisionDamageFor(_initialCollisionCooldown);
			if (!((Object)(object)_monoItem != null) && TryGetComponent<MonoItem>(out var component))
			{
				_monoItem = component;
			}
		}

		public void SuppressCollisionDamageFor(float seconds)
		{
			ReloadCooldown(Time.time + seconds);
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

		private void OnCollisionEnter(Collision other)
		{
			if ((Object)(object)_monoItem == null || _isNotCollidable || _grabable.IgnoreItemsCollision || !_monoItem.IsSpawned)
			{
				return;
			}
			int layer = other.gameObject.layer;
			if ((_ignoreCollisionMask.value & (1 << layer)) == 0 && CanProcessCollision())
			{
				ReloadCooldown(Time.time);
				float magnitude = other.relativeVelocity.magnitude;
				float itemMass = 1f;
				if (TryGetComponent<Rigidbody>(out var component))
				{
					itemMass = component.mass;
				}
				_itemCollisionService.ProcessIItemCollision(_monoItem, magnitude, itemMass, other);
			}
		}

		private bool CanProcessCollision()
		{
			float time = Time.time;
			if (_collisionCooldown != 0f)
			{
				return time - _lastCollisionTime >= _collisionCooldown;
			}
			return true;
		}

		private void ReloadCooldown(float lastCollisionTime)
		{
			_lastCollisionTime = lastCollisionTime;
		}
	}
}
