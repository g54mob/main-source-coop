using Features.GrabModule.Scripts.PhysGrab;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	public class AnchorItemView : MonoBehaviour
	{
		[Header("Sphere (gameplay)")]
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private GrabObject _grabObject;

		[SerializeField]
		private PhysGrabber _pullGrabber;

		[SerializeField]
		private Transform _backGrabPoint;

		[SerializeField]
		private Transform _restReference;

		[SerializeField]
		private Collider[] _sphereColliders;

		[SerializeField]
		private Renderer[] _sphereRenderers;

		[SerializeField]
		private LayerMask _playerNoImpulseLayers = 128;

		[Header("Pendant (visual + joint)")]
		[SerializeField]
		private Transform _pendantTransform;

		[SerializeField]
		private Rigidbody _pendantRigidbody;

		[SerializeField]
		private GrabObject _pendantGrabObject;

		[SerializeField]
		private Transform _pendantGrabPoint;

		[SerializeField]
		private SpringJoint _returnSpringJoint;

		[SerializeField]
		private float _returnSpringStrength = 18f;

		[SerializeField]
		private float _returnSpringDamper = 2f;

		[SerializeField]
		private float _returnSpringMassScale = 1f;

		[SerializeField]
		private float _settleSpeedThreshold = 1.2f;

		[SerializeField]
		private float _minFlightTimeBeforeSettle = 0.15f;

		[SerializeField]
		private Collider[] _pendantColliders;

		[SerializeField]
		private Collider _pendantRootCollider;

		[Tooltip("excludeLayers on the pendant root collider while the hook is flying outbound.")]
		[SerializeField]
		private LayerMask _pendantThrowExcludeLayers = 128;

		[Tooltip("excludeLayers on all pendant colliders while reeling / returning.")]
		[SerializeField]
		private LayerMask _pendantReelExcludeLayers;

		[SerializeField]
		private Renderer[] _pendantRenderers;

		[SerializeField]
		private Vector3 _pendantRestLocalPosition = new Vector3(0f, -0.5f, 0f);

		[SerializeField]
		private Vector3 _pendantRestLocalEuler;

		[SerializeField]
		[FormerlySerializedAs("_flightWorldEuler")]
		private Vector3 _flightLocalEuler = new Vector3(-90f, 0f, 0f);

		private readonly AnchorThrowSphere _sphere = new AnchorThrowSphere();

		private readonly AnchorPendant _pendant = new AnchorPendant();

		private readonly AnchorFlight _flight = new AnchorFlight();

		private readonly AnchorPlayerLatch _latch = new AnchorPlayerLatch();

		private readonly AnchorSpringReel _reel = new AnchorSpringReel();

		private AnchorEnemyContext _context;

		private bool _thrown;

		private bool _physGrabReeling;

		public bool HasRestReference => _sphere.HasRestReference;

		public float RestWorldY => _sphere.RestWorldY;

		public bool IsAnchorSettled => _flight.IsSettled(_sphere, _pendant);

		public float MinFlightTimeBeforeSettle => _flight.MinFlightTimeBeforeSettle;

		public Vector3 AnchorWorldPosition
		{
			get
			{
				if (!(_pendant.Transform != null))
				{
					return base.transform.position;
				}
				return _pendant.WorldPosition;
			}
		}

		public bool IsLatchedToPlayer => _latch.IsLatched;

		public void TeleportSphereTo(Vector3 worldPosition)
		{
			_sphere.TeleportTo(worldPosition);
		}

		public void TeleportPendantTo(Vector3 worldPosition)
		{
			_pendant.TeleportTo(worldPosition);
		}

		public void PrepareForLaunch()
		{
			_latch.Clear();
			_physGrabReeling = false;
			_flight.Prepare(_sphere, _pendant, ClearPullGrabbers);
			_thrown = true;
			SetThrownVisual(thrown: true);
		}

		public void LaunchWithImpulse(Vector3 target, float flightTime, float upBias, float speedMultiplier, float maxHookDistance)
		{
			_physGrabReeling = false;
			_flight.LaunchWithImpulse(_sphere, _pendant, target, flightTime, upBias, speedMultiplier, maxHookDistance, ClearPullGrabbers);
		}

		public void LatchToPlayer(Transform attachTarget)
		{
			_latch.Latch(attachTarget, _sphere, _pendant, ClearPullGrabbers, ref _physGrabReeling);
			ClearPendantCollision();
		}

		public void ClearPlayerLatch()
		{
			_latch.Clear();
		}

		public void BeginSpringReel()
		{
			_reel.Begin(_sphere, _pendant, _latch, _flight, ref _physGrabReeling);
			if (!_latch.IsLatched)
			{
				ApplyPendantReelCollision();
			}
			else
			{
				ClearPendantCollision();
			}
		}

		public void ReturnToRest()
		{
			_reel.ReturnToRest(_sphere, _pendant, _latch, _flight, ref _thrown, ref _physGrabReeling);
			SetThrownVisual(thrown: false);
		}

		[Inject]
		private void InjectDependencies(AnchorEnemyContext context)
		{
			_context = context;
		}

		private void Awake()
		{
			BindHelpers();
			_sphere.BindHost(base.transform);
			_pendant.Initialize();
			IgnoreSpherePendantCollisions();
			_pendant.SetReturnSpring(0f, 0f, 1f);
			ClearPullGrabbers();
			SetThrownVisual(thrown: false);
			SetPhysicsThrown(thrown: false);
		}

		private void BindHelpers()
		{
			_sphere.Assign(_rigidbody, _grabObject, _restReference, _sphereColliders, _sphereRenderers);
			_pendant.Assign(_pendantTransform, _pendantRigidbody, _pendantGrabObject, _pendantGrabPoint, _returnSpringJoint, _pendantColliders, _pendantRenderers, _pendantRestLocalPosition, _pendantRestLocalEuler, _flightLocalEuler);
			_flight.Assign(_settleSpeedThreshold, _minFlightTimeBeforeSettle);
			_reel.Assign(_pullGrabber, _backGrabPoint, _returnSpringStrength, _returnSpringDamper, _returnSpringMassScale);
		}

		private void Update()
		{
			if (!CanReadNetworkedState())
			{
				return;
			}
			bool isAnchorThrown = _context.IsAnchorThrown;
			if (isAnchorThrown != _thrown)
			{
				_thrown = isAnchorThrown;
				SetThrownVisual(isAnchorThrown);
				if (_context.HasStateAuthority)
				{
					SetPhysicsThrown(isAnchorThrown);
				}
			}
		}

		private void LateUpdate()
		{
			if (CanReadNetworkedState())
			{
				if (_context.HasStateAuthority && _latch.IsLatched)
				{
					_latch.Snap(_sphere, _pendant);
				}
				else if (!_thrown)
				{
					_flight.SnapPairReadyForThrow(_sphere, _pendant, GetReadyThrowDirection());
				}
			}
		}

		private void FixedUpdate()
		{
			if (_thrown && CanReadNetworkedState() && _context.HasStateAuthority && !_latch.IsLatched && !_physGrabReeling)
			{
				_flight.EnforceMaxChainDistance(_sphere, _pendant);
			}
		}

		private Vector3 GetReadyThrowDirection()
		{
			Vector3 vector = (_sphere.HasRestReference ? _restReference.position : base.transform.position);
			if (_context.HasLivePriorityPlayer())
			{
				Vector3 result = _context.PriorityPlayer.NetworkObject.transform.position - vector;
				if (result.sqrMagnitude > 0.0001f)
				{
					return result;
				}
			}
			return base.transform.forward;
		}

		private bool CanReadNetworkedState()
		{
			if (_context != null && _context.Object != null)
			{
				return _context.Object.IsValid;
			}
			return false;
		}

		private void ClearPullGrabbers()
		{
			_reel.ClearPullGrabbers(_sphere, _pendant);
		}

		private void SetPhysicsThrown(bool thrown)
		{
			_flight.SetPhysicsThrown(thrown, _latch.IsLatched, _sphere, _pendant, ClearPullGrabbers, ref _physGrabReeling);
		}

		private void SetThrownVisual(bool thrown)
		{
			AnchorPhysicsUtil.SetRenderersEnabled(_sphere.Renderers, enabled: false);
			AnchorPhysicsUtil.SetRenderersEnabled(_pendant.Renderers, thrown);
			AnchorPhysicsUtil.SetCollidersEnabled(_sphere.Colliders, thrown);
			SetSolidPlayerImpulseExcluded(_sphere.Colliders, thrown);
			if (thrown)
			{
				ApplyPendantThrowCollision();
			}
			else
			{
				ClearPendantCollision();
			}
		}

		private void ApplyPendantThrowCollision()
		{
			AnchorPhysicsUtil.SetCollidersEnabled(_pendantColliders, enabled: false);
			if (_pendantRootCollider != null)
			{
				_pendantRootCollider.enabled = true;
				if (!_pendantRootCollider.isTrigger)
				{
					_pendantRootCollider.excludeLayers = _pendantThrowExcludeLayers;
				}
			}
		}

		private void ApplyPendantReelCollision()
		{
			AnchorPhysicsUtil.SetCollidersEnabled(_pendantColliders, enabled: true);
			SetColliderExcludeLayers(_pendantColliders, _pendantReelExcludeLayers);
			if (_pendantRootCollider != null && !_pendantRootCollider.isTrigger)
			{
				_pendantRootCollider.excludeLayers = _pendantReelExcludeLayers;
			}
		}

		private void ClearPendantCollision()
		{
			AnchorPhysicsUtil.SetCollidersEnabled(_pendantColliders, enabled: false);
			SetColliderExcludeLayers(_pendantColliders, 0);
			if (_pendantRootCollider != null)
			{
				_pendantRootCollider.enabled = false;
				if (!_pendantRootCollider.isTrigger)
				{
					_pendantRootCollider.excludeLayers = 0;
				}
			}
		}

		private void SetSolidPlayerImpulseExcluded(Collider[] colliders, bool excludePlayer)
		{
			SetColliderExcludeLayers(colliders, excludePlayer ? _playerNoImpulseLayers : ((LayerMask)0));
		}

		private static void SetColliderExcludeLayers(Collider[] colliders, LayerMask excludeLayers)
		{
			if (colliders == null)
			{
				return;
			}
			foreach (Collider collider in colliders)
			{
				if (!(collider == null) && !collider.isTrigger)
				{
					collider.excludeLayers = excludeLayers;
				}
			}
		}

		private void IgnoreSpherePendantCollisions()
		{
			Collider[] colliders = _sphere.Colliders;
			Collider[] colliders2 = _pendant.Colliders;
			if (colliders == null || colliders2 == null)
			{
				return;
			}
			foreach (Collider collider in colliders)
			{
				if (collider == null)
				{
					continue;
				}
				foreach (Collider collider2 in colliders2)
				{
					if (collider2 != null)
					{
						Physics.IgnoreCollision(collider, collider2, ignore: true);
					}
				}
			}
		}
	}
}
