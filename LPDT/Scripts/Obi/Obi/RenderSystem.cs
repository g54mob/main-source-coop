using System;

namespace Obi
{
	public interface RenderSystem<T> : IRenderSystem where T : ObiRenderer<T>
	{
		RendererSet<T> renderers { get; }

		bool IRenderSystem.isEmpty => renderers.Count == 0;

		Type IRenderSystem.GetRendererType()
		{
			return typeof(T);
		}

		bool AddRenderer(T renderer)
		{
			return renderers.AddRenderer(renderer);
		}

		bool RemoveRenderer(T renderer)
		{
			return renderers.RemoveRenderer(renderer);
		}
	}
}
