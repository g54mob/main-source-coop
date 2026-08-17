using FIMSpace.FProceduralAnimation;
using NomadDrive.Features.Player.Core;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player
{
	public class RemoteLegsAnimatorLOD : MonoBehaviour, IPlayerComponent
	{
		[Header("LOD Distances")]
		[Tooltip("Beyond this distance from the local camera, LegsAnimator is fully disabled on remote clones.")]
		[SerializeField]
		private float lodDistance = 30f;

		[Tooltip("Beyond this distance, the body Animator is disabled too (frozen pose). Keep >= lodDistance so the body keeps animating a bit longer than the legs; set equal to cut both together.")]
		[SerializeField]
		private float animatorLodDistance = 45f;

		[Tooltip("Distance hysteresis (meters) so a clone hovering at a threshold does not flicker on/off.")]
		[SerializeField]
		private float hysteresis = 3f;

		[Header("Visibility")]
		[Tooltip("Bounds expansion (meters) so legs re-enable slightly before the body enters the frustum, hiding the reactivation pop.")]
		[SerializeField]
		private float visibilityMargin = 2f;

		[Tooltip("Seconds between LOD evaluations (throttled, not per-frame).")]
		[SerializeField]
		private float evalInterval = 0.15f;

		[Header("Bounds Refresh (bug-1051)")]
		[Tooltip("Seconds to force per-frame skinned-bounds recompute after a teleport/standup, so a CullCompletely clone re-evaluates visibility at its new position. Off the rest of the time to avoid the permanent per-frame bounds tax.")]
		[SerializeField]
		private float boundsRefreshDuration = 0.4f;

		[Tooltip("A clone moving faster than this (m/s) for one frame is treated as a teleport (respawn, force-attach) and triggers a bounds-refresh pulse. Keep well above any vehicle/locomotion speed so smooth movement never trips it; sit/stand teleports are covered separately via the legs sync signal.")]
		[SerializeField]
		private float teleportSpeedThreshold = 50f;

		[Header("References")]
		[SerializeField]
		private LegsAnimator legsAnimator;

		[SerializeField]
		private Animator bodyAnimator;

		[Inject]
		private IPlayerService _playerService;

		private Player _player;

		private Transform _selfTransform;

		private Camera _camera;

		private SkinnedMeshRenderer[] _bodyRenderers;

		private readonly Plane[] _frustumPlanes = new Plane[6];

		private bool _isRemote;

		private bool _syncedEnabled = true;

		private float _evalTimer;

		private bool _withinLegs = true;

		private bool _withinAnimator = true;

		private bool _inFrustum = true;

		private float _boundsRefreshTimer;

		private bool _boundsForcedOn;

		private Vector3 _lastPosition;

		public int SetupPriority => 40;

		private void Awake()
		{
			_selfTransform = base.transform;
			_player = GetComponentInParent<Player>();
			Transform transform = ((_player != null) ? _player.transform : base.transform);
			if (legsAnimator == null)
			{
				legsAnimator = transform.GetComponentInChildren<LegsAnimator>(includeInactive: true);
			}
			if (bodyAnimator == null)
			{
				bodyAnimator = transform.GetComponentInChildren<Animator>(includeInactive: true);
			}
			_bodyRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
		}

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (isLocalPlayer)
			{
				_isRemote = false;
				base.enabled = false;
				return;
			}
			_isRemote = true;
			base.enabled = true;
			_lastPosition = _selfTransform.position;
			RefreshBounds();
			_syncedEnabled = _player == null || _player.LegsAnimatorSyncedEnabled;
			EvaluateLod(force: true);
		}

		private void Update()
		{
			if (_isRemote)
			{
				UpdateBoundsRefresh();
				_evalTimer -= Time.deltaTime;
				if (!(_evalTimer > 0f))
				{
					_evalTimer = evalInterval;
					EvaluateLod(force: false);
				}
			}
		}

		private void UpdateBoundsRefresh()
		{
			Vector3 position = _selfTransform.position;
			float deltaTime = Time.deltaTime;
			float num = teleportSpeedThreshold * deltaTime;
			if (deltaTime > 0f && (position - _lastPosition).sqrMagnitude > num * num)
			{
				RefreshBounds();
			}
			_lastPosition = position;
			if (_boundsForcedOn)
			{
				_boundsRefreshTimer -= deltaTime;
				if (_boundsRefreshTimer <= 0f)
				{
					SetBoundsAlwaysUpdate(value: false);
				}
			}
		}

		private void RefreshBounds()
		{
			_boundsRefreshTimer = boundsRefreshDuration;
			if (!_boundsForcedOn)
			{
				SetBoundsAlwaysUpdate(value: true);
			}
		}

		private void SetBoundsAlwaysUpdate(bool value)
		{
			_boundsForcedOn = value;
			if (_bodyRenderers == null)
			{
				return;
			}
			for (int i = 0; i < _bodyRenderers.Length; i++)
			{
				if (_bodyRenderers[i] != null)
				{
					_bodyRenderers[i].updateWhenOffscreen = value;
				}
			}
		}

		private void EvaluateLod(bool force)
		{
			Camera camera = ResolveCamera();
			bool flag;
			bool flag2;
			bool flag3;
			if (camera == null)
			{
				flag = (flag2 = (flag3 = true));
			}
			else
			{
				float dist = Vector3.Distance(camera.transform.position, _selfTransform.position);
				flag = WithinHysteresis(_withinLegs, dist, lodDistance);
				flag2 = WithinHysteresis(_withinAnimator, dist, animatorLodDistance);
				flag3 = flag && IsInFrustum(camera);
			}
			if (force || flag != _withinLegs || flag2 != _withinAnimator || flag3 != _inFrustum)
			{
				_withinLegs = flag;
				_withinAnimator = flag2;
				_inFrustum = flag3;
				Apply();
			}
		}

		private bool WithinHysteresis(bool currentlyWithin, float dist, float threshold)
		{
			float num = Mathf.Max(0f, threshold - hysteresis);
			if (!currentlyWithin)
			{
				return dist <= num;
			}
			return dist <= threshold;
		}

		private bool IsInFrustum(Camera cam)
		{
			if (!TryGetBodyBounds(out var bounds))
			{
				return true;
			}
			bounds.Expand(visibilityMargin * 2f);
			GeometryUtility.CalculateFrustumPlanes(cam, _frustumPlanes);
			return GeometryUtility.TestPlanesAABB(_frustumPlanes, bounds);
		}

		private bool TryGetBodyBounds(out Bounds bounds)
		{
			bounds = default(Bounds);
			bool flag = false;
			if (_bodyRenderers == null)
			{
				return false;
			}
			for (int i = 0; i < _bodyRenderers.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = _bodyRenderers[i];
				if (!(skinnedMeshRenderer == null) && skinnedMeshRenderer.enabled && skinnedMeshRenderer.gameObject.activeInHierarchy)
				{
					if (!flag)
					{
						bounds = skinnedMeshRenderer.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(skinnedMeshRenderer.bounds);
					}
				}
			}
			return flag;
		}

		private Camera ResolveCamera()
		{
			if ((bool)_camera)
			{
				return _camera;
			}
			if (_playerService != null && _playerService.TryGetCameraTransform(out var cameraTransform) && cameraTransform != null)
			{
				Camera camera = cameraTransform.GetComponent<Camera>();
				if (camera == null)
				{
					camera = cameraTransform.GetComponentInChildren<Camera>();
				}
				_camera = camera;
			}
			return _camera;
		}

		public void NotifySyncedEnabledChanged(bool syncedEnabled)
		{
			_syncedEnabled = syncedEnabled;
			if (_isRemote)
			{
				RefreshBounds();
				Apply();
			}
		}

		private void Apply()
		{
			if (legsAnimator != null)
			{
				bool flag = _syncedEnabled && _withinLegs && _inFrustum;
				if (legsAnimator.enabled != flag)
				{
					legsAnimator.enabled = flag;
				}
			}
			if (bodyAnimator != null && bodyAnimator.enabled != _withinAnimator)
			{
				bodyAnimator.enabled = _withinAnimator;
			}
		}
	}
}
