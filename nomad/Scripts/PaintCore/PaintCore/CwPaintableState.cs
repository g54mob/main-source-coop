using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	public class CwPaintableState
	{
		public RenderTexture Texture;

		public List<CwCommand> Commands = new List<CwCommand>();

		private static Stack<CwPaintableState> pool = new Stack<CwPaintableState>();

		public static CwPaintableState Pop()
		{
			if (pool.Count <= 0)
			{
				return new CwPaintableState();
			}
			return pool.Pop();
		}

		public void Write(RenderTexture current)
		{
			Clear();
			Texture = CwCommon.GetRenderTexture(current.descriptor, current);
			CwCommon.Blit(Texture, current);
		}

		public void Write(List<CwCommand> commands)
		{
			Clear();
			Commands.AddRange(commands);
		}

		public void Write(RenderTexture current, List<CwCommand> commands)
		{
			Clear();
			Texture = CwCommon.GetRenderTexture(current.descriptor, current);
			CwCommon.Blit(Texture, current);
			Commands.AddRange(commands);
		}

		private void Clear()
		{
			if (Texture != null)
			{
				CwCommon.ReleaseRenderTexture(Texture);
				Texture = null;
			}
			for (int num = Commands.Count - 1; num >= 0; num--)
			{
				Commands[num].Pool();
			}
			Commands.Clear();
		}

		public void Pool()
		{
			Clear();
			pool.Push(this);
		}
	}
}
