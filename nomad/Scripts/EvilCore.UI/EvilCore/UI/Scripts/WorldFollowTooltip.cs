using EvilCore.Extensions;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class WorldFollowTooltip : MonoBehaviour
	{
		[Header("Follow")]
		[SerializeField]
		private RectTransform tooltipRoot;

		[SerializeField]
		private Vector2 pivot = new Vector2(0f, 0f);

		[Tooltip("Canvas-space (reference px) offset applied up-right from the resolved anchor.")]
		[SerializeField]
		private Vector2 screenMargin = new Vector2(24f, 24f);

		[Tooltip("Max canvas-px the anchor may sit up-right of the part center. Caps drift on large parts; small parts unaffected.")]
		[SerializeField]
		private Vector2 maxCornerOffset = new Vector2(220f, 150f);

		[SerializeField]
		private bool clampToScreen = true;

		[SerializeField]
		private Vector2 clampPadding = new Vector2(8f, 8f);

		[Header("Stacking / Idle")]
		[Tooltip("Lower value is placed first (top) when several tooltips share the same target object.")]
		[SerializeField]
		private int stackPriority;

		[Tooltip("When true, snaps to homeAnchoredPosition while there is no target (e.g. equipped item HUD).")]
		[SerializeField]
		private bool returnHomeWhenIdle;

		[SerializeField]
		private Vector2 homeAnchoredPosition;

		private GameObject _target;

		private Renderer[] _renderers;

		private Camera _camera;

		private Canvas _canvas;

		private bool _registered;

		public GameObject Target => _target;

		public int StackPriority => stackPriority;

		public bool IsFollowing => _target != null;

		public Vector2 Size
		{
			get
			{
				if (!(tooltipRoot != null))
				{
					return Vector2.zero;
				}
				return tooltipRoot.rect.size;
			}
		}

		private void Awake()
		{
			if (tooltipRoot == null)
			{
				tooltipRoot = base.transform as RectTransform;
			}
			if (tooltipRoot != null)
			{
				tooltipRoot.anchorMin = new Vector2(0.5f, 0.5f);
				tooltipRoot.anchorMax = new Vector2(0.5f, 0.5f);
				tooltipRoot.pivot = pivot;
			}
			_canvas = GetComponentInParent<Canvas>();
		}

		public void SetTarget(GameObject target, Camera cam)
		{
			_target = target;
			_camera = cam;
			_renderers = ((target != null) ? target.GetComponentsInChildren<Renderer>() : null);
			Register();
			SelfPosition();
		}

		public void ClearTarget()
		{
			_target = null;
			_renderers = null;
			Unregister();
			if (returnHomeWhenIdle && tooltipRoot != null)
			{
				tooltipRoot.anchoredPosition = homeAnchoredPosition;
			}
		}

		private void OnDisable()
		{
			Unregister();
		}

		private void Register()
		{
			if (!_registered && !(WorldFollowTooltipCoordinator.Instance == null))
			{
				WorldFollowTooltipCoordinator.Instance.Register(this);
				_registered = true;
			}
		}

		private void Unregister()
		{
			if (_registered)
			{
				if (WorldFollowTooltipCoordinator.Instance != null)
				{
					WorldFollowTooltipCoordinator.Instance.Unregister(this);
				}
				_registered = false;
			}
		}

		private void LateUpdate()
		{
			if (WorldFollowTooltipCoordinator.Instance != null)
			{
				if (_target != null)
				{
					Register();
				}
			}
			else
			{
				SelfPosition();
			}
		}

		private void SelfPosition()
		{
			if (!(_target == null) && TryComputeBaseAnchor(out var canvasPos))
			{
				ApplyAnchoredPosition(clampToScreen ? ClampToCanvas(canvasPos) : canvasPos);
			}
		}

		public bool TryComputeBaseAnchor(out Vector2 canvasPos)
		{
			canvasPos = default(Vector2);
			if (_target == null || tooltipRoot == null || _canvas == null || _camera == null)
			{
				return false;
			}
			if (!TryGetScreenAnchor(out var topRight, out var center))
			{
				return false;
			}
			Vector3 vector = _canvas.ScreenToCanvasPosition(new Vector3(topRight.x, topRight.y, 0f));
			Vector3 vector2 = _canvas.ScreenToCanvasPosition(new Vector3(center.x, center.y, 0f));
			Vector2 vector3 = new Vector2(vector.x - vector2.x, vector.y - vector2.y);
			vector3.x = Mathf.Clamp(vector3.x, 0f - maxCornerOffset.x, maxCornerOffset.x);
			vector3.y = Mathf.Clamp(vector3.y, 0f - maxCornerOffset.y, maxCornerOffset.y);
			canvasPos = new Vector2(vector2.x, vector2.y) + vector3 + screenMargin;
			return true;
		}

		public void ApplyAnchoredPosition(Vector2 pos)
		{
			if (tooltipRoot != null)
			{
				tooltipRoot.anchoredPosition = pos;
			}
		}

		public Vector2 ClampToCanvas(Vector2 pos)
		{
			if (_canvas == null)
			{
				return pos;
			}
			Vector2 sizeDelta = ((RectTransform)_canvas.transform).sizeDelta;
			Vector2 size = Size;
			float num = sizeDelta.x * 0.5f;
			float num2 = sizeDelta.y * 0.5f;
			float num3 = 0f - num + clampPadding.x + pivot.x * size.x;
			float num4 = num - clampPadding.x - (1f - pivot.x) * size.x;
			float num5 = 0f - num2 + clampPadding.y + pivot.y * size.y;
			float num6 = num2 - clampPadding.y - (1f - pivot.y) * size.y;
			if (num4 < num3)
			{
				num4 = num3;
			}
			if (num6 < num5)
			{
				num6 = num5;
			}
			pos.x = Mathf.Clamp(pos.x, num3, num4);
			pos.y = Mathf.Clamp(pos.y, num5, num6);
			return pos;
		}

		private bool TryGetScreenAnchor(out Vector2 topRight, out Vector2 center)
		{
			topRight = default(Vector2);
			center = default(Vector2);
			float nearClipPlane = _camera.nearClipPlane;
			Vector3 vector = _camera.WorldToScreenPoint(_target.transform.position);
			if (vector.z <= nearClipPlane)
			{
				return false;
			}
			center = new Vector2(vector.x, vector.y);
			float num = -1f / 0f;
			float num2 = -1f / 0f;
			bool flag = false;
			if (_renderers != null)
			{
				for (int i = 0; i < _renderers.Length; i++)
				{
					Renderer renderer = _renderers[i];
					if (renderer == null || !renderer.enabled || !renderer.gameObject.activeInHierarchy)
					{
						continue;
					}
					GetCornerSource(renderer, out var local, out var space);
					Vector3 min = local.min;
					Vector3 max = local.max;
					for (int j = 0; j < 2; j++)
					{
						for (int k = 0; k < 2; k++)
						{
							for (int l = 0; l < 2; l++)
							{
								Vector3 vector2 = new Vector3((j == 0) ? min.x : max.x, (k == 0) ? min.y : max.y, (l == 0) ? min.z : max.z);
								Vector3 position = ((space != null) ? space.TransformPoint(vector2) : vector2);
								Vector3 vector3 = _camera.WorldToScreenPoint(position);
								if (!(vector3.z <= nearClipPlane))
								{
									flag = true;
									if (vector3.x > num)
									{
										num = vector3.x;
									}
									if (vector3.y > num2)
									{
										num2 = vector3.y;
									}
								}
							}
						}
					}
				}
			}
			topRight = (flag ? new Vector2(num, num2) : center);
			return true;
		}

		private static void GetCornerSource(Renderer renderer, out Bounds local, out Transform space)
		{
			MeshFilter component;
			if (renderer is SkinnedMeshRenderer skinnedMeshRenderer)
			{
				local = skinnedMeshRenderer.localBounds;
				space = skinnedMeshRenderer.transform;
			}
			else if (renderer.TryGetComponent<MeshFilter>(out component) && component.sharedMesh != null)
			{
				local = component.sharedMesh.bounds;
				space = renderer.transform;
			}
			else
			{
				local = renderer.bounds;
				space = null;
			}
		}
	}
}
