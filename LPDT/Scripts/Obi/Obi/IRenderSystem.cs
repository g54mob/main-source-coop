using System;

namespace Obi
{
	public interface IRenderSystem
	{
		uint tier => 1u;

		Oni.RenderingSystemType typeEnum { get; }

		bool isEmpty { get; }

		void Setup();

		void Step();

		void Render();

		void Dispose();

		Type GetRendererType();
	}
}
