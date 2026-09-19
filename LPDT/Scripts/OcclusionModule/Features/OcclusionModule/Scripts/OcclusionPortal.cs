using System.Collections.Generic;
using UnityEngine;

namespace Features.OcclusionModule.Scripts
{
	public class OcclusionPortal : MonoBehaviour
	{
		private const float NEIGHBOR_PROBE_DISTANCE = 2f;

		private const float DOOR_SEARCH_RADIUS = 2f;

		private static readonly Vector3[] _gizmoCorners = new Vector3[4];

		[Header("Open detection")]
		[Tooltip("Doorless archway — always traversable, skips door detection.")]
		[SerializeField]
		private bool _alwaysOpen = true;

		[Header("Portal opening")]
		[Tooltip("Doorway width (transform right axis).")]
		[SerializeField]
		private float _portalWidth = 2.2f;

		[Tooltip("Doorway height (transform up axis).")]
		[SerializeField]
		private float _portalHeight = 3f;

		[Header("Door frame")]
		[Tooltip("Meshes toggled with this portal, not its rooms: shown while either adjacent room is visible so the frame doesn't flicker at the boundary. Assign, or reparent under the portal and Collect Frame Renderers.")]
		[SerializeField]
		private MeshRenderer[] _frameRenderers;

		private OcclusionRoom _resolvedA;

		private OcclusionRoom _resolvedB;

		private HingeJoint _resolvedHinge;

		private bool _hadHinge;

		private Transform _leaf;

		private Vector3 _leafLocalWidthDir;

		private bool _isWidthResolved;

		private bool _isFrameApplied;

		private bool _isFrameVisible = true;

		private bool[] _frameRendererStates;

		public OcclusionRoom RoomA => _resolvedA;

		public OcclusionRoom RoomB => _resolvedB;

		public Vector3 PortalCenter => base.transform.position;

		public MeshRenderer[] FrameRenderers => _frameRenderers;

		private void OnEnable()
		{
			OcclusionRegistry.Register(this);
		}

		private void OnDisable()
		{
			OcclusionRegistry.Unregister(this);
		}

		public bool IsOpen(float doorOpenThreshold)
		{
			if (_alwaysOpen)
			{
				return true;
			}
			if (_hadHinge && _resolvedHinge == null)
			{
				return true;
			}
			if (!TryGetOpenness(out var openness))
			{
				return true;
			}
			return openness > doorOpenThreshold;
		}

		private bool TryGetOpenness(out float openness)
		{
			openness = 0f;
			if (!TryResolveWidthDir())
			{
				return false;
			}
			Vector3 vector = _leaf.TransformDirection(_leafLocalWidthDir);
			vector.y = 0f;
			Vector3 forward = base.transform.forward;
			forward.y = 0f;
			if (vector.sqrMagnitude < 0.0001f || forward.sqrMagnitude < 0.0001f)
			{
				return false;
			}
			openness = Mathf.Abs(Vector3.Dot(vector.normalized, forward.normalized));
			return true;
		}

		public void ResolveDoor(HingeJoint[] hinges)
		{
			_resolvedHinge = null;
			_isWidthResolved = false;
			_hadHinge = false;
			if (_alwaysOpen || hinges == null)
			{
				return;
			}
			Vector3 portalCenter = PortalCenter;
			float num = 4f;
			foreach (HingeJoint hingeJoint in hinges)
			{
				if (!(hingeJoint == null) && !(hingeJoint.GetComponentInChildren<MeshRenderer>() == null))
				{
					float sqrMagnitude = (hingeJoint.transform.position - portalCenter).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						_resolvedHinge = hingeJoint;
					}
				}
			}
			_hadHinge = _resolvedHinge != null;
		}

		public void ResolveRooms(OcclusionRoom[] rooms)
		{
			_resolvedA = GetComponentInParent<OcclusionRoom>();
			Vector3 vector = PortalCenter + base.transform.forward * 2f;
			OcclusionRoom occlusionRoom = null;
			float num = float.MaxValue;
			OcclusionRoom occlusionRoom2 = null;
			float num2 = float.MaxValue;
			OcclusionRoom occlusionRoom3 = null;
			float num3 = float.MaxValue;
			GameObject gameObject = ((_resolvedA != null) ? _resolvedA.gameObject : null);
			foreach (OcclusionRoom occlusionRoom4 in rooms)
			{
				if (occlusionRoom4 == null || occlusionRoom4 == _resolvedA || occlusionRoom4.gameObject == gameObject)
				{
					continue;
				}
				if (occlusionRoom4.HasBounds && occlusionRoom4.RendererBounds.Contains(vector))
				{
					Vector3 size = occlusionRoom4.RendererBounds.size;
					float num4 = size.x * size.y * size.z;
					if (num4 < num)
					{
						num = num4;
						occlusionRoom = occlusionRoom4;
					}
					continue;
				}
				float num5 = (occlusionRoom4.HasBounds ? occlusionRoom4.RendererBounds.SqrDistance(vector) : (occlusionRoom4.transform.position - vector).sqrMagnitude);
				if (num5 < num2)
				{
					num2 = num5;
					occlusionRoom2 = occlusionRoom4;
				}
				if (occlusionRoom4.HasBounds && !(vector.y < occlusionRoom4.RendererBounds.min.y) && !(vector.y > occlusionRoom4.RendererBounds.max.y) && num5 < num3)
				{
					num3 = num5;
					occlusionRoom3 = occlusionRoom4;
				}
			}
			if (occlusionRoom != null)
			{
				_resolvedB = occlusionRoom;
			}
			else
			{
				_resolvedB = ((occlusionRoom3 != null) ? occlusionRoom3 : occlusionRoom2);
			}
		}

		public OcclusionRoom GetOther(OcclusionRoom room)
		{
			if (!(room == RoomA))
			{
				if (!(room == RoomB))
				{
					return null;
				}
				return RoomA;
			}
			return RoomB;
		}

		[ContextMenu("Collect Frame Renderers")]
		public void CollectFrameRenderers()
		{
			if (_frameRenderers == null || _frameRenderers.Length == 0)
			{
				MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>(includeInactive: true);
				List<MeshRenderer> list = new List<MeshRenderer>(componentsInChildren.Length);
				MeshRenderer[] array = componentsInChildren;
				foreach (MeshRenderer meshRenderer in array)
				{
					if (meshRenderer.enabled)
					{
						list.Add(meshRenderer);
					}
				}
				_frameRenderers = list.ToArray();
			}
			CaptureFrameRendererStates();
		}

		public void SetFrameVisible(bool isVisible)
		{
			if (_isFrameApplied && isVisible == _isFrameVisible)
			{
				return;
			}
			_isFrameApplied = true;
			_isFrameVisible = isVisible;
			if (_frameRenderers == null)
			{
				return;
			}
			for (int i = 0; i < _frameRenderers.Length; i++)
			{
				MeshRenderer meshRenderer = _frameRenderers[i];
				if (!(meshRenderer == null))
				{
					meshRenderer.enabled = isVisible && (_frameRendererStates == null || _frameRendererStates[i]);
				}
			}
		}

		private void CaptureFrameRendererStates()
		{
			if (_frameRenderers == null)
			{
				_frameRendererStates = null;
				return;
			}
			_frameRendererStates = new bool[_frameRenderers.Length];
			for (int i = 0; i < _frameRenderers.Length; i++)
			{
				_frameRendererStates[i] = _frameRenderers[i] != null && _frameRenderers[i].enabled;
			}
		}

		public void GetPortalCorners(Vector3[] corners)
		{
			Vector3 portalCenter = PortalCenter;
			Vector3 vector = base.transform.right * (_portalWidth * 0.5f);
			Vector3 vector2 = base.transform.up * (_portalHeight * 0.5f);
			corners[0] = portalCenter - vector - vector2;
			corners[1] = portalCenter + vector - vector2;
			corners[2] = portalCenter + vector + vector2;
			corners[3] = portalCenter - vector + vector2;
		}

		private bool TryResolveWidthDir()
		{
			if (_isWidthResolved)
			{
				return _leaf != null;
			}
			Transform transform = ((_resolvedHinge != null) ? _resolvedHinge.transform : null);
			if (transform == null)
			{
				return false;
			}
			Renderer[] componentsInChildren = transform.GetComponentsInChildren<Renderer>();
			if (componentsInChildren.Length == 0)
			{
				return false;
			}
			Bounds bounds = componentsInChildren[0].bounds;
			for (int i = 1; i < componentsInChildren.Length; i++)
			{
				bounds.Encapsulate(componentsInChildren[i].bounds);
			}
			Vector3 vector = bounds.center - transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				return false;
			}
			_leaf = transform;
			_leafLocalWidthDir = transform.InverseTransformDirection(vector.normalized);
			_isWidthResolved = true;
			return true;
		}

		private void OnDrawGizmos()
		{
			GetPortalCorners(_gizmoCorners);
			Gizmos.color = Color.cyan;
			for (int i = 0; i < 4; i++)
			{
				Gizmos.DrawLine(_gizmoCorners[i], _gizmoCorners[(i + 1) % 4]);
			}
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(PortalCenter, PortalCenter + base.transform.forward);
		}
	}
}
