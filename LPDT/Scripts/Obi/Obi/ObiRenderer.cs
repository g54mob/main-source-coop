namespace Obi
{
	public interface ObiRenderer<T> where T : ObiRenderer<T>
	{
		protected RenderSystem<T> CreateRenderSystem(ObiSolver solver);

		bool ValidateRenderer()
		{
			return true;
		}

		void CleanupRenderer()
		{
		}

		protected bool UnregisterRenderer(ObiSolver solver)
		{
			CleanupRenderer();
			RenderSystem<T> renderSystem = solver.GetRenderSystem<T>();
			if (renderSystem != null && renderSystem.RemoveRenderer((T)this))
			{
				if (renderSystem.isEmpty)
				{
					solver.UnregisterRenderSystem(renderSystem);
					renderSystem.Dispose();
				}
				solver.dirtyRendering |= (int)renderSystem.typeEnum;
				return true;
			}
			return false;
		}

		protected bool RegisterRenderer(ObiSolver solver)
		{
			if (ValidateRenderer())
			{
				RenderSystem<T> renderSystem = solver.GetRenderSystem<T>();
				if (renderSystem == null)
				{
					renderSystem = CreateRenderSystem(solver);
					solver.RegisterRenderSystem(renderSystem);
				}
				if (renderSystem != null && renderSystem.AddRenderer((T)this))
				{
					solver.dirtyRendering |= (int)renderSystem.typeEnum;
					return true;
				}
			}
			return false;
		}
	}
}
