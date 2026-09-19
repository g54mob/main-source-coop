using System.Collections.Generic;

namespace Obi
{
	public class ObiRenderSystemStack
	{
		private List<IRenderSystem>[] stack;

		public ObiRenderSystemStack(int tiers)
		{
			stack = new List<IRenderSystem>[tiers];
			for (int i = 0; i < tiers; i++)
			{
				stack[i] = new List<IRenderSystem>();
			}
		}

		public void Setup(int dirtyFlags)
		{
			List<IRenderSystem>[] array = stack;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (IRenderSystem item in array[i])
				{
					if (((uint)dirtyFlags & (uint)item.typeEnum) != 0)
					{
						item.Setup();
					}
				}
			}
		}

		public void Step()
		{
			List<IRenderSystem>[] array = stack;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (IRenderSystem item in array[i])
				{
					item.Step();
				}
			}
		}

		public void Render()
		{
			List<IRenderSystem>[] array = stack;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (IRenderSystem item in array[i])
				{
					item.Render();
				}
			}
		}

		public bool RegisterRenderSystem(IRenderSystem renderSystem)
		{
			if (renderSystem != null && renderSystem.tier >= 0 && renderSystem.tier < stack.Length)
			{
				stack[renderSystem.tier].Add(renderSystem);
				return true;
			}
			return false;
		}

		public bool UnregisterRenderSystem(IRenderSystem renderSystem)
		{
			if (renderSystem != null && renderSystem.tier >= 0 && renderSystem.tier < stack.Length)
			{
				stack[renderSystem.tier].Remove(renderSystem);
				return true;
			}
			return false;
		}

		public RenderSystem<T> GetRenderSystem<T>() where T : ObiRenderer<T>
		{
			List<IRenderSystem>[] array = stack;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (IRenderSystem item in array[i])
				{
					if (item.GetRendererType() == typeof(T))
					{
						return item as RenderSystem<T>;
					}
				}
			}
			return null;
		}

		public IRenderSystem GetRenderSystem(Oni.RenderingSystemType systemType)
		{
			List<IRenderSystem>[] array = stack;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (IRenderSystem item in array[i])
				{
					if (item.typeEnum == systemType)
					{
						return item;
					}
				}
			}
			return null;
		}
	}
}
