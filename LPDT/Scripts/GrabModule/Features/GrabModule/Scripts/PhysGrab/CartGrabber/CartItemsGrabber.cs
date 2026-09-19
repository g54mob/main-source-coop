using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.GrabModule.Scripts.PhysGrab.CartGrabber
{
	[NetworkBehaviourWeaved(0)]
	public class CartItemsGrabber : NetworkBehaviour, ICartItemsContainer
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private SimplePointGrabable _cartGrabable;

		[SerializeField]
		private GrabberType _grabberType;

		[SerializeField]
		private Transform _kinematicPoint;

		[SerializeField]
		private LayerMask _ignoreLayerMask;

		[SerializeField]
		private LayerMask _nothingLayerMask;

		[SerializeField]
		private LayerMask _ignoreCollectableLayerMask;

		[SerializeField]
		private bool _resetPhysicsForItems;

		[SerializeField]
		private int _requasteCountPerTick = 10;

		[SerializeField]
		private EventReference _itemAddedSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private float _audioCooldown = 0.5f;

		[SerializeField]
		private float _cartVelocityBorder = 3f;

		private Dictionary<IPointGrabable, Coroutine> _fallRoutines = new Dictionary<IPointGrabable, Coroutine>();

		private Dictionary<IPointGrabable, Vector3> _localPositions = new Dictionary<IPointGrabable, Vector3>();

		private Dictionary<IPointGrabable, bool> _isGrabbedInvokeTriggered = new Dictionary<IPointGrabable, bool>();

		private readonly Dictionary<IPointGrabable, int> _adderPlayerId = new Dictionary<IPointGrabable, int>();

		private readonly HashSet<IPointGrabable> _items = new HashSet<IPointGrabable>();

		private readonly Dictionary<IPointGrabable, int> _overlapCounts = new Dictionary<IPointGrabable, int>();

		private readonly Dictionary<IPointGrabable, CartItemsGrabber> _childGrabbers = new Dictionary<IPointGrabable, CartItemsGrabber>();

		private readonly Dictionary<IPointGrabable, CartItemsGrabber> _parentGrabbers = new Dictionary<IPointGrabable, CartItemsGrabber>();

		private bool _initialized;

		private bool _releasingKinematicItems;

		private int _requestCounter;

		private float _lastAudioPlayTime;

		private CartItemAddedEventClass _cartItemAddedEventClass;

		private IAudioService _audioService;

		public HashSet<IPointGrabable> Items => _items;

		public IPointGrabable CartGrabbable => _cartGrabable;

		public Dictionary<IPointGrabable, CartItemsGrabber> ChildGrabbers => _childGrabbers;

		public Dictionary<IPointGrabable, CartItemsGrabber> ParentGrabbers => _parentGrabbers;

		public GrabberType GrabberType => _grabberType;

		public IPointGrabable CartGrabable => _cartGrabable;

		Transform ICartItemsContainer.transform => base.transform;

		[Inject]
		public void InjectDependencies(CartItemAddedEventClass cartItemAddedEventClass, IAudioService audioService)
		{
			_cartItemAddedEventClass = cartItemAddedEventClass;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			_initialized = true;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (((1 << other.gameObject.layer) & _ignoreLayerMask.value) != 0)
			{
				return;
			}
			IPointGrabable pointGrabable = FindComponent<IPointGrabable>(other.gameObject);
			if (pointGrabable == null || pointGrabable.IgnoredByCart || _cartGrabable == pointGrabable)
			{
				return;
			}
			_overlapCounts.TryGetValue(pointGrabable, out var value);
			value++;
			_overlapCounts[pointGrabable] = value;
			if (value > 1)
			{
				return;
			}
			CartItemsGrabber cartItemsGrabber = FindComponent<CartItemsGrabber>(pointGrabable.GameObject);
			if (cartItemsGrabber != null)
			{
				if (GrabberType > cartItemsGrabber.GrabberType)
				{
					_childGrabbers.TryAdd(pointGrabable, cartItemsGrabber);
				}
				else
				{
					_parentGrabbers.TryAdd(pointGrabable, cartItemsGrabber);
				}
			}
			pointGrabable.InCart = true;
			pointGrabable.Carts.Add(_rigidbody.gameObject);
			_items.Add(pointGrabable);
			pointGrabable.PhysicsResolutionController?.SetPhysicsResolutionInContainer();
			if (!pointGrabable.WeightResetOnCart)
			{
				return;
			}
			if (pointGrabable.GrabbedByPlayers.Count > 0 || pointGrabable.GrabbedBySomethingCount > 0)
			{
				_adderPlayerId[pointGrabable] = ResolveAdderPlayerId(pointGrabable);
				_isGrabbedInvokeTriggered[pointGrabable] = false;
			}
			else if (pointGrabable.GrabbedByPlayers.Count == 0 && pointGrabable.GrabbedBySomethingCount == 0)
			{
				int num = ResolveAdderPlayerId(pointGrabable);
				_adderPlayerId[pointGrabable] = num;
				PlayAudio(pointGrabable, num);
				_isGrabbedInvokeTriggered[pointGrabable] = true;
				if (pointGrabable.Carts.Count > 0 && pointGrabable.Carts[0] == _rigidbody.gameObject)
				{
					_cartItemAddedEventClass.InvokeOnGrabableAddedInCart(this, pointGrabable, num);
				}
				ApplyCartMass(pointGrabable);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			IPointGrabable pointGrabable = FindComponent<IPointGrabable>(other.gameObject);
			if (pointGrabable == null || pointGrabable.IgnoredByCart || !_overlapCounts.TryGetValue(pointGrabable, out var value))
			{
				return;
			}
			value = Mathf.Max(0, value - 1);
			if (value > 0)
			{
				_overlapCounts[pointGrabable] = value;
				return;
			}
			_overlapCounts.Remove(pointGrabable);
			if (_items.Contains(pointGrabable))
			{
				_childGrabbers.Remove(pointGrabable);
				_parentGrabbers.Remove(pointGrabable);
				_items.Remove(pointGrabable);
				if (pointGrabable.GrabbedByPlayersCount == 0 && pointGrabable.GrabbedBySomethingCount == 0)
				{
					pointGrabable.IsAuthorityRequested = false;
				}
				pointGrabable.PhysicsResolutionController?.RestoreDefaultResolution();
				pointGrabable.Carts.Remove(_rigidbody.gameObject);
				if (_isGrabbedInvokeTriggered.ContainsKey(pointGrabable))
				{
					_isGrabbedInvokeTriggered.Remove(pointGrabable);
				}
				_adderPlayerId.Remove(pointGrabable);
				if (pointGrabable.Carts.Count == 0)
				{
					pointGrabable.InCart = false;
				}
				if (pointGrabable.WeightResetOnCart)
				{
					RestoreOriginalMass(pointGrabable);
				}
			}
		}

		public void FixedUpdate()
		{
			if (!_initialized)
			{
				return;
			}
			if (base.Object.StateAuthority.PlayerId != base.Runner.LocalPlayer.PlayerId)
			{
				ReleaseDroppedItemClaims();
				return;
			}
			for (int i = 0; i < _items.Count; i++)
			{
				IPointGrabable pointGrabable = _items.ElementAt(i);
				if (pointGrabable == null || pointGrabable.NetworkObject == null)
				{
					_items.Remove(pointGrabable);
					i--;
				}
			}
			foreach (IPointGrabable item in _items)
			{
				if (item.GrabbedByPlayersCount > 0 || item.GrabbedBySomethingCount > 0)
				{
					continue;
				}
				if (_rigidbody.linearVelocity.magnitude > _cartVelocityBorder)
				{
					item.Rigidbody.linearVelocity = Vector3.Lerp(item.Rigidbody.linearVelocity, new Vector3(_rigidbody.linearVelocity.x, item.Rigidbody.linearVelocity.y, _rigidbody.linearVelocity.z), 30f * Time.fixedDeltaTime);
				}
				item.Rigidbody.isKinematic = false;
				if (_childGrabbers.TryGetValue(item, out var value))
				{
					if (item.NetworkObject.StateAuthority == base.Object.StateAuthority)
					{
						item.IsAuthorityRequested = true;
					}
					else if (!item.IsAuthorityRequested)
					{
						if (_requestCounter >= _requasteCountPerTick)
						{
							_requestCounter = 0;
							break;
						}
						_requestCounter++;
						item.IsAuthorityRequested = true;
						item.RequestStateAuthorityRPC(base.Object.StateAuthority.PlayerId);
						break;
					}
					continue;
				}
				if (_parentGrabbers.TryGetValue(item, out value))
				{
					break;
				}
				if (item.NetworkObject.StateAuthority == base.Object.StateAuthority)
				{
					item.IsAuthorityRequested = true;
					continue;
				}
				bool flag = false;
				foreach (CartItemsGrabber value2 in _parentGrabbers.Values)
				{
					if (value2.Items.Contains(item))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					continue;
				}
				bool flag2 = false;
				foreach (CartItemsGrabber value3 in _childGrabbers.Values)
				{
					if (value3.Items.Contains(item) && (value3.CartGrabable.GrabbedByPlayers.Count > 0 || value3.CartGrabable.GrabbedBySomethingCount > 0))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && !item.IsAuthorityRequested)
				{
					if (_requestCounter >= _requasteCountPerTick)
					{
						_requestCounter = 0;
						break;
					}
					_requestCounter++;
					item.IsAuthorityRequested = true;
					item.RequestStateAuthorityRPC(base.Object.StateAuthority.PlayerId);
				}
			}
		}

		private void ReleaseDroppedItemClaims()
		{
			foreach (IPointGrabable item in _items)
			{
				if (item != null && !(item.NetworkObject == null) && item.NetworkObject.HasStateAuthority && item.GrabbedByPlayersCount <= 0 && item.GrabbedBySomethingCount <= 0)
				{
					item.IsAuthorityRequested = false;
				}
			}
		}

		private void Update()
		{
			if (!_initialized)
			{
				return;
			}
			foreach (IPointGrabable item in _items)
			{
				if (!item.WeightResetOnCart || item.Rigidbody == null)
				{
					continue;
				}
				if ((item.GrabbedByPlayers.Count > 0 || item.GrabbedBySomethingCount > 0) && !Mathf.Approximately(item.Rigidbody.mass, item.OriginalMass))
				{
					RestoreOriginalMass(item);
				}
				else
				{
					if (item.GrabbedByPlayers.Count != 0 || item.GrabbedBySomethingCount != 0 || !Mathf.Approximately(item.Rigidbody.mass, item.OriginalMass))
					{
						continue;
					}
					int value;
					int adderPlayerId = (_adderPlayerId.TryGetValue(item, out value) ? value : ResolveAdderPlayerId(item));
					if (_isGrabbedInvokeTriggered.ContainsKey(item))
					{
						if (!_isGrabbedInvokeTriggered[item])
						{
							_isGrabbedInvokeTriggered[item] = true;
							if (item.Carts[0] == _rigidbody.gameObject)
							{
								PlayAudio(item, adderPlayerId);
								_cartItemAddedEventClass.InvokeOnGrabableAddedInCart(this, item, adderPlayerId);
							}
						}
					}
					else
					{
						_isGrabbedInvokeTriggered[item] = true;
						if (item.Carts[0] == _rigidbody.gameObject)
						{
							PlayAudio(item, adderPlayerId);
							_cartItemAddedEventClass.InvokeOnGrabableAddedInCart(this, item, adderPlayerId);
						}
					}
					ApplyCartMass(item);
				}
			}
		}

		private void ApplyCartMass(IPointGrabable grabbable)
		{
			grabbable.Rigidbody.mass = grabbable.CartMass;
		}

		private void RestoreOriginalMass(IPointGrabable grabbable)
		{
			grabbable.Rigidbody.mass = grabbable.OriginalMass;
			grabbable.Rigidbody.linearVelocity = Vector3.zero;
			grabbable.Rigidbody.angularVelocity = Vector3.zero;
		}

		private T FindComponent<T>(GameObject other)
		{
			T val = other.GetComponent<T>();
			if (val == null)
			{
				val = other.GetComponentInParent<T>();
			}
			if (val == null)
			{
				val = other.GetComponentInChildren<T>();
			}
			return val;
		}

		private void PlayAudio(IPointGrabable grabbable, int adderPlayerId)
		{
			if (base.Runner.LocalPlayer.PlayerId == adderPlayerId && !(Time.time - _lastAudioPlayTime < _audioCooldown))
			{
				_lastAudioPlayTime = Time.time;
				_audioService.PlayOneShot(_itemAddedSound, _soundSourceBehaviour);
			}
		}

		private int ResolveAdderPlayerId(IPointGrabable grabbable)
		{
			if (grabbable.GrabbedByPlayers.Count > 0)
			{
				return grabbable.GrabbedByPlayers[0];
			}
			return grabbable.NetworkObject.StateAuthority.PlayerId;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
