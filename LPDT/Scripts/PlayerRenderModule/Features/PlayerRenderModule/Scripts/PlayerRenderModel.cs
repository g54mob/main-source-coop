using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.PlayerRenderModule.Scripts
{
	public class PlayerRenderModel
	{
		private List<Renderer> _customPlayerRenderers = new List<Renderer>();

		private List<Renderer> _basePlayerRenderers = new List<Renderer>();

		public IReadOnlyList<Renderer> CustomPlayerRenderers => _customPlayerRenderers;

		public IReadOnlyList<Renderer> BasePlayerRenderers => _basePlayerRenderers;

		public void InitializeCustomPlayerRenderers(IEnumerable<Renderer> renderers)
		{
			_customPlayerRenderers = renderers.ToList();
		}

		public void InitializeBasePlayerRenderers(IEnumerable<Renderer> renderers)
		{
			_basePlayerRenderers = renderers.ToList();
		}

		public void InitializePlayerRenderers(IEnumerable<Renderer> basePlayerRenderers, IEnumerable<Renderer> customPlayerRenderers)
		{
			InitializeBasePlayerRenderers(basePlayerRenderers);
			InitializeCustomPlayerRenderers(customPlayerRenderers);
		}

		public void AddCustomPlayerRenderer(Renderer renderer)
		{
			_customPlayerRenderers.Add(renderer);
		}

		public void AddBasePlayerRenderer(Renderer renderer)
		{
			_basePlayerRenderers.Add(renderer);
		}

		public void ClearPlayerRenderers()
		{
			_basePlayerRenderers.Clear();
			_customPlayerRenderers.Clear();
		}
	}
}
