using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.OcclusionModule.Scripts
{
	public class OcclusionRoom : MonoBehaviour
	{
		public const float BACKDROP_HORIZONTAL_EXTENT = 200f;

		private static readonly List<MeshRenderer> _childBuffer = new List<MeshRenderer>();

		private static readonly List<MeshRenderer> _keepBuffer = new List<MeshRenderer>();

		private static readonly List<MeshRenderer> _boundsCollectBuffer = new List<MeshRenderer>();

		private readonly List<Bounds> _boundsBuffer = new List<Bounds>();

		private readonly Dictionary<MeshRenderer, bool> _authoredStates = new Dictionary<MeshRenderer, bool>();

		private MeshRenderer[] _renderers;

		private MeshRenderer[] _boundsRenderers;

		private Bounds[] _rendererWorldBounds;

		private Bounds _rendererBounds;

		private bool _hasBounds;

		private bool _isApplied;

		private int _authoredVisibleCount;

		public bool IsVisible { get; private set; } = true;

		public MeshRenderer[] Renderers => _renderers;

		public Bounds RendererBounds => _rendererBounds;

		public bool HasBounds => _hasBounds;

		public int AuthoredVisibleCount => _authoredVisibleCount;

		private void Awake()
		{
			CaptureAuthoredStates();
		}

		private void OnEnable()
		{
			OcclusionRegistry.Register(this);
		}

		private void OnDisable()
		{
			OcclusionRegistry.Unregister(this);
		}

		public void CaptureAuthoredStates()
		{
			GetComponentsInChildren(includeInactive: true, _childBuffer);
			_authoredStates.Clear();
			_authoredVisibleCount = 0;
			foreach (MeshRenderer item in _childBuffer)
			{
				_authoredStates[item] = item.enabled;
				if (item.enabled)
				{
					_authoredVisibleCount++;
				}
			}
		}

		public bool IsAuthoredVisible(MeshRenderer meshRenderer)
		{
			if (_authoredStates.TryGetValue(meshRenderer, out var value))
			{
				return value;
			}
			value = meshRenderer.enabled;
			_authoredStates[meshRenderer] = value;
			return value;
		}

		public void CollectRenderers()
		{
			GetComponentsInChildren(includeInactive: true, _childBuffer);
			_keepBuffer.Clear();
			_boundsCollectBuffer.Clear();
			foreach (MeshRenderer item in _childBuffer)
			{
				if (IsAuthoredVisible(item) && !(item.GetComponentInParent<OcclusionPortal>() != null))
				{
					_boundsCollectBuffer.Add(item);
					if (item.GetComponentInParent<OcclusionAlwaysVisible>() == null)
					{
						_keepBuffer.Add(item);
					}
				}
			}
			_renderers = _keepBuffer.ToArray();
			_boundsRenderers = _boundsCollectBuffer.ToArray();
			RecalculateBounds();
		}

		private static bool IsBackdrop(MeshRenderer meshRenderer)
		{
			Vector3 size = meshRenderer.bounds.size;
			if (!(size.x > 200f))
			{
				return size.z > 200f;
			}
			return true;
		}

		private void RecalculateBounds()
		{
			_hasBounds = false;
			_boundsBuffer.Clear();
			if (_boundsRenderers == null)
			{
				_rendererWorldBounds = Array.Empty<Bounds>();
				return;
			}
			MeshRenderer[] boundsRenderers = _boundsRenderers;
			foreach (MeshRenderer meshRenderer in boundsRenderers)
			{
				if (!(meshRenderer == null) && meshRenderer.gameObject.activeInHierarchy && !IsBackdrop(meshRenderer))
				{
					Bounds bounds = meshRenderer.bounds;
					_boundsBuffer.Add(bounds);
					if (!_hasBounds)
					{
						_rendererBounds = bounds;
						_hasBounds = true;
					}
					else
					{
						_rendererBounds.Encapsulate(bounds);
					}
				}
			}
			_rendererWorldBounds = _boundsBuffer.ToArray();
		}

		public float SqrDistanceToNearestRenderer(Vector3 point)
		{
			float num = float.MaxValue;
			if (_rendererWorldBounds == null)
			{
				return num;
			}
			Bounds[] rendererWorldBounds = _rendererWorldBounds;
			foreach (Bounds bounds in rendererWorldBounds)
			{
				float num2 = bounds.SqrDistance(point);
				if (num2 < num)
				{
					num = num2;
					if (num <= 0f)
					{
						return 0f;
					}
				}
			}
			return num;
		}

		public void ExcludeRenderers(HashSet<MeshRenderer> exclude)
		{
			if (_renderers == null || exclude == null || exclude.Count == 0)
			{
				return;
			}
			_keepBuffer.Clear();
			MeshRenderer[] renderers = _renderers;
			foreach (MeshRenderer meshRenderer in renderers)
			{
				if (meshRenderer != null && !exclude.Contains(meshRenderer))
				{
					_keepBuffer.Add(meshRenderer);
				}
			}
			_boundsCollectBuffer.Clear();
			renderers = _boundsRenderers;
			foreach (MeshRenderer meshRenderer2 in renderers)
			{
				if (meshRenderer2 != null && !exclude.Contains(meshRenderer2))
				{
					_boundsCollectBuffer.Add(meshRenderer2);
				}
			}
			_renderers = _keepBuffer.ToArray();
			_boundsRenderers = _boundsCollectBuffer.ToArray();
			RecalculateBounds();
		}

		public void SetVisible(bool isVisible)
		{
			if (_isApplied && isVisible == IsVisible)
			{
				return;
			}
			_isApplied = true;
			IsVisible = isVisible;
			if (_renderers == null)
			{
				return;
			}
			MeshRenderer[] renderers = _renderers;
			foreach (MeshRenderer meshRenderer in renderers)
			{
				if (meshRenderer != null)
				{
					meshRenderer.enabled = isVisible && IsAuthoredVisible(meshRenderer);
				}
			}
		}
	}
}
