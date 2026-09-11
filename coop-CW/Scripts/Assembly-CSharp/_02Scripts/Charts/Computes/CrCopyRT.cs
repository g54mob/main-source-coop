using UnityEngine;
using pworld.Scripts.Extensions;

namespace _02Scripts.Charts.Computes
{
	public static class CrCopyRT
	{
		public static ComputeShader csCopyRt;

		public static void CopyRenderTexture(RenderTexture source, RenderTexture target)
		{
			if (!(source == null))
			{
				if (source.width != target.width && source.height != target.height)
				{
					Debug.LogError("source and target must have same resolution");
				}
				int num = csCopyRt.FindKernel("CopyRt");
				csCopyRt.SetTexture(num, "source", source);
				csCopyRt.SetTexture(num, "target", target);
				csCopyRt.PDispatch(num, "threadGroups", source.width, source.height);
			}
		}
	}
}
