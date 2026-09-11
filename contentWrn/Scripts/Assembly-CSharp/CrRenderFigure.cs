using DefaultNamespace.Petter.TitleCard;
using Unity.Collections;
using UnityEngine;
using Zorro.Core;
using pworld.Scripts.Extensions;

public class CrRenderFigure
{
	public static ComputeShader csRenderFigure;

	private const string KERNEL_NAME = "RenderFigure";

	private const string POINTS_NAME = "points";

	private const string POINTS_COUNT_NAME = "numPoints";

	private const string RT_NAME = "renderTexture";

	private const string RADIUS_NAME = "radius";

	private const string RESOLUTION_NAME = "resolution";

	private const string FILL_COLOR_NAME = "color";

	private const string THREAD_GROUP_NAME = "threadGroups";

	public static void DrawFigure(RenderTexture rt, Figure figure)
	{
		if (!(rt == null))
		{
			int num = csRenderFigure.FindKernel("RenderFigure");
			csRenderFigure.SetTexture(num, "renderTexture", rt);
			NativeArray<Vector2> me = figure.GetPoints().ToArray().ToNativeArray(Allocator.Temp);
			ComputeBuffer computeBuffer = me.SetAndCreateComputeBuffer(csRenderFigure, num, "points");
			csRenderFigure.SetInt("numPoints", figure.points.Count);
			csRenderFigure.SetFloat("radius", figure.radius);
			Vector2 vector = rt.PGetSize();
			csRenderFigure.SetVector("resolution", rt.PGetSize());
			csRenderFigure.SetVector("color", figure.color);
			csRenderFigure.PDispatch(num, "threadGroups", vector.x, vector.y);
			computeBuffer.Dispose();
			me.Dispose();
		}
	}
}
