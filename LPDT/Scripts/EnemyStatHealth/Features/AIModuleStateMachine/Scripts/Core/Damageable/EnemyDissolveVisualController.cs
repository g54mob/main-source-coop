using System;
using System.Collections.Generic;
using Obi;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core.Damageable
{
	public sealed class EnemyDissolveVisualController : IDisposable
	{
		private sealed class MaterialState
		{
			public Material Material { get; }

			public Color AuthoredEdgeColor { get; }

			public int ReferenceCount { get; set; }

			public MaterialState(Material material)
			{
				Material = material;
				AuthoredEdgeColor = material.GetColor(_dissolveEdgeColor);
			}
		}

		private static readonly int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");

		private static readonly int _dissolveEdgeColor = Shader.PropertyToID("_DissolveEdgeColor");

		private static readonly Dictionary<Renderer, MaterialState> _rendererStates = new Dictionary<Renderer, MaterialState>();

		private static readonly Dictionary<ObiRopeExtrudedRenderer, MaterialState> _obiRopeRendererStates = new Dictionary<ObiRopeExtrudedRenderer, MaterialState>();

		private readonly IList<Renderer> _renderers;

		private readonly IList<ObiRopeExtrudedRenderer> _obiRopeRenderers;

		private readonly List<Renderer> _dynamicRenderers = new List<Renderer>();

		private readonly List<ObiRopeExtrudedRenderer> _dynamicObiRopeRenderers = new List<ObiRopeExtrudedRenderer>();

		private readonly Dictionary<Renderer, MaterialState> _trackedRendererStates = new Dictionary<Renderer, MaterialState>();

		private readonly Dictionary<ObiRopeExtrudedRenderer, MaterialState> _trackedObiRopeRendererStates = new Dictionary<ObiRopeExtrudedRenderer, MaterialState>();

		private readonly List<Renderer> _staleRenderers = new List<Renderer>();

		private readonly List<ObiRopeExtrudedRenderer> _staleObiRopeRenderers = new List<ObiRopeExtrudedRenderer>();

		private bool _materialsInstanced;

		public EnemyDissolveVisualController(IList<Renderer> renderers, IList<ObiRopeExtrudedRenderer> obiRopeRenderers)
		{
			_renderers = renderers;
			_obiRopeRenderers = obiRopeRenderers;
		}

		public void Dispose()
		{
			foreach (Renderer key in _trackedRendererStates.Keys)
			{
				ReleaseRenderer(key);
			}
			foreach (ObiRopeExtrudedRenderer key2 in _trackedObiRopeRendererStates.Keys)
			{
				ReleaseObiRopeRenderer(key2);
			}
			_trackedRendererStates.Clear();
			_trackedObiRopeRendererStates.Clear();
			_materialsInstanced = false;
		}

		public void AddRenderers(IEnumerable<Renderer> renderers)
		{
			foreach (Renderer renderer in renderers)
			{
				if (!_dynamicRenderers.Contains(renderer))
				{
					_dynamicRenderers.Add(renderer);
				}
			}
			RefreshMaterialsIfInstanced();
		}

		public void RemoveRenderers(IEnumerable<Renderer> renderers)
		{
			foreach (Renderer renderer in renderers)
			{
				_dynamicRenderers.Remove(renderer);
			}
			RefreshMaterialsIfInstanced();
		}

		public void AddObiRopeRenderers(IEnumerable<ObiRopeExtrudedRenderer> obiRopeRenderers)
		{
			foreach (ObiRopeExtrudedRenderer obiRopeRenderer in obiRopeRenderers)
			{
				if (!_dynamicObiRopeRenderers.Contains(obiRopeRenderer))
				{
					_dynamicObiRopeRenderers.Add(obiRopeRenderer);
				}
			}
			RefreshMaterialsIfInstanced();
		}

		public void RemoveObiRopeRenderers(IEnumerable<ObiRopeExtrudedRenderer> obiRopeRenderers)
		{
			foreach (ObiRopeExtrudedRenderer obiRopeRenderer in obiRopeRenderers)
			{
				_dynamicObiRopeRenderers.Remove(obiRopeRenderer);
			}
			RefreshMaterialsIfInstanced();
		}

		public void RefreshMaterials()
		{
			_materialsInstanced = true;
			RefreshRenderers();
			RefreshObiRopeRenderers();
		}

		private void RefreshMaterialsIfInstanced()
		{
			if (_materialsInstanced)
			{
				RefreshMaterials();
			}
		}

		public void SetDissolveAmount(float amount)
		{
			foreach (MaterialState value in _trackedRendererStates.Values)
			{
				value.Material.SetFloat(_dissolveAmount, amount);
			}
			foreach (MaterialState value2 in _trackedObiRopeRendererStates.Values)
			{
				value2.Material.SetFloat(_dissolveAmount, amount);
			}
		}

		public void SetDissolveEdgeColor(Color color)
		{
			foreach (MaterialState value in _trackedRendererStates.Values)
			{
				value.Material.SetColor(_dissolveEdgeColor, color);
			}
			foreach (MaterialState value2 in _trackedObiRopeRendererStates.Values)
			{
				value2.Material.SetColor(_dissolveEdgeColor, color);
			}
		}

		public void RestoreAuthoredEdgeColors()
		{
			foreach (MaterialState value in _trackedRendererStates.Values)
			{
				value.Material.SetColor(_dissolveEdgeColor, value.AuthoredEdgeColor);
			}
			foreach (MaterialState value2 in _trackedObiRopeRendererStates.Values)
			{
				value2.Material.SetColor(_dissolveEdgeColor, value2.AuthoredEdgeColor);
			}
		}

		private void RefreshRenderers()
		{
			_staleRenderers.Clear();
			foreach (KeyValuePair<Renderer, MaterialState> trackedRendererState in _trackedRendererStates)
			{
				Renderer key = trackedRendererState.Key;
				bool num = IsConfiguredRenderer(key);
				bool flag = key == null || key.sharedMaterial != trackedRendererState.Value.Material;
				if (!num || flag)
				{
					_staleRenderers.Add(key);
				}
			}
			foreach (Renderer staleRenderer in _staleRenderers)
			{
				_trackedRendererStates.Remove(staleRenderer);
				ReleaseRenderer(staleRenderer);
			}
			foreach (Renderer renderer in _renderers)
			{
				TrackRenderer(renderer);
			}
			foreach (Renderer dynamicRenderer in _dynamicRenderers)
			{
				TrackRenderer(dynamicRenderer);
			}
		}

		private void RefreshObiRopeRenderers()
		{
			_staleObiRopeRenderers.Clear();
			foreach (KeyValuePair<ObiRopeExtrudedRenderer, MaterialState> trackedObiRopeRendererState in _trackedObiRopeRendererStates)
			{
				ObiRopeExtrudedRenderer key = trackedObiRopeRendererState.Key;
				bool num = IsConfiguredObiRopeRenderer(key);
				bool flag = key == null || key.material != trackedObiRopeRendererState.Value.Material;
				if (!num || flag)
				{
					_staleObiRopeRenderers.Add(key);
				}
			}
			foreach (ObiRopeExtrudedRenderer staleObiRopeRenderer in _staleObiRopeRenderers)
			{
				_trackedObiRopeRendererStates.Remove(staleObiRopeRenderer);
				ReleaseObiRopeRenderer(staleObiRopeRenderer);
			}
			foreach (ObiRopeExtrudedRenderer obiRopeRenderer in _obiRopeRenderers)
			{
				TrackObiRopeRenderer(obiRopeRenderer);
			}
			foreach (ObiRopeExtrudedRenderer dynamicObiRopeRenderer in _dynamicObiRopeRenderers)
			{
				TrackObiRopeRenderer(dynamicObiRopeRenderer);
			}
		}

		private void TrackRenderer(Renderer renderer)
		{
			if (!(renderer == null) && !_trackedRendererStates.ContainsKey(renderer) && IsDissolveMaterial(renderer.sharedMaterial))
			{
				MaterialState value = AcquireRenderer(renderer);
				_trackedRendererStates[renderer] = value;
			}
		}

		private void TrackObiRopeRenderer(ObiRopeExtrudedRenderer renderer)
		{
			if (!(renderer == null) && !_trackedObiRopeRendererStates.ContainsKey(renderer) && IsDissolveMaterial(renderer.material))
			{
				MaterialState value = AcquireObiRopeRenderer(renderer);
				_trackedObiRopeRendererStates[renderer] = value;
			}
		}

		private bool IsConfiguredRenderer(Renderer renderer)
		{
			if (renderer != null)
			{
				if (!_renderers.Contains(renderer))
				{
					return _dynamicRenderers.Contains(renderer);
				}
				return true;
			}
			return false;
		}

		private bool IsConfiguredObiRopeRenderer(ObiRopeExtrudedRenderer renderer)
		{
			if (renderer != null)
			{
				if (!_obiRopeRenderers.Contains(renderer))
				{
					return _dynamicObiRopeRenderers.Contains(renderer);
				}
				return true;
			}
			return false;
		}

		private static bool IsDissolveMaterial(Material material)
		{
			if (material != null && material.HasProperty(_dissolveAmount))
			{
				return material.HasProperty(_dissolveEdgeColor);
			}
			return false;
		}

		private static MaterialState AcquireRenderer(Renderer renderer)
		{
			if (!_rendererStates.TryGetValue(renderer, out var value) || renderer.sharedMaterial != value.Material)
			{
				value = new MaterialState(renderer.material);
				_rendererStates[renderer] = value;
			}
			value.ReferenceCount++;
			return value;
		}

		private static MaterialState AcquireObiRopeRenderer(ObiRopeExtrudedRenderer renderer)
		{
			if (!_obiRopeRendererStates.TryGetValue(renderer, out var value) || renderer.material != value.Material)
			{
				value = new MaterialState(renderer.material = new Material(renderer.material));
				_obiRopeRendererStates[renderer] = value;
			}
			value.ReferenceCount++;
			return value;
		}

		private static void ReleaseRenderer(Renderer renderer)
		{
			if ((object)renderer != null && _rendererStates.TryGetValue(renderer, out var value))
			{
				value.ReferenceCount--;
				if (value.ReferenceCount <= 0)
				{
					_rendererStates.Remove(renderer);
					UnityEngine.Object.Destroy(value.Material);
				}
			}
		}

		private static void ReleaseObiRopeRenderer(ObiRopeExtrudedRenderer renderer)
		{
			if ((object)renderer != null && _obiRopeRendererStates.TryGetValue(renderer, out var value))
			{
				value.ReferenceCount--;
				if (value.ReferenceCount <= 0)
				{
					_obiRopeRendererStates.Remove(renderer);
					UnityEngine.Object.Destroy(value.Material);
				}
			}
		}
	}
}
