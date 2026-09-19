using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace Obi
{
	public static class BurstLocalOptimization
	{
		public struct SurfacePoint
		{
			public float4 bary;

			public float4 point;

			public float4 normal;
		}

		public interface IDistanceFunction
		{
			void Evaluate(float4 point, float4 radii, quaternion orientation, ref SurfacePoint projectedPoint);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void GetInterpolatedSimplexData(int simplexStart, int simplexSize, NativeArray<int> simplices, NativeArray<float4> positions, NativeArray<quaternion> orientations, NativeArray<float4> radii, float4 convexBary, out float4 convexPoint, out float4 convexRadii, out quaternion convexOrientation)
		{
			convexPoint = float4.zero;
			convexRadii = float4.zero;
			convexOrientation = new quaternion(0f, 0f, 0f, 0f);
			for (int i = 0; i < simplexSize; i++)
			{
				int index = simplices[simplexStart + i];
				convexPoint += positions[index] * convexBary[i];
				convexRadii += radii[index] * convexBary[i];
				convexOrientation.value += orientations[index].value * convexBary[i];
			}
			convexPoint.w = 0f;
		}

		public static SurfacePoint Optimize<T>(ref T function, NativeArray<float4> positions, NativeArray<quaternion> orientations, NativeArray<float4> radii, NativeArray<int> simplices, int simplexStart, int simplexSize, ref float4 convexBary, out float4 convexPoint, int maxIterations = 16, float tolerance = 0.004f) where T : struct, IDistanceFunction
		{
			SurfacePoint pointInFunction = default(SurfacePoint);
			GetInterpolatedSimplexData(simplexStart, simplexSize, simplices, positions, orientations, radii, convexBary, out convexPoint, out var convexRadii, out var convexOrientation);
			if (simplexSize == 1 || maxIterations < 1)
			{
				function.Evaluate(convexPoint, convexRadii, convexOrientation, ref pointInFunction);
			}
			else if (simplexSize == 2)
			{
				GoldenSearch(ref function, simplexStart, simplexSize, positions, orientations, radii, simplices, ref convexPoint, ref convexRadii, ref convexOrientation, ref convexBary, ref pointInFunction, maxIterations, tolerance * 10f);
			}
			else
			{
				FrankWolfe(ref function, simplexStart, simplexSize, positions, orientations, radii, simplices, ref convexPoint, ref convexRadii, ref convexOrientation, ref convexBary, ref pointInFunction, maxIterations, tolerance);
			}
			return pointInFunction;
		}

		private static void FrankWolfe<T>(ref T function, int simplexStart, int simplexSize, NativeArray<float4> positions, NativeArray<quaternion> orientations, NativeArray<float4> radii, NativeArray<int> simplices, ref float4 convexPoint, ref float4 convexThickness, ref quaternion convexOrientation, ref float4 convexBary, ref SurfacePoint pointInFunction, int maxIterations, float tolerance) where T : struct, IDistanceFunction
		{
			for (int i = 0; i < maxIterations; i++)
			{
				function.Evaluate(convexPoint, convexThickness, convexOrientation, ref pointInFunction);
				int index = 0;
				float num = float.MinValue;
				for (int j = 0; j < simplexSize; j++)
				{
					int index2 = simplices[simplexStart + j];
					float4 y = positions[index2] - convexPoint;
					y.w = 0f;
					y -= pointInFunction.normal * (radii[index2].x - convexThickness.x);
					float num2 = math.dot(-pointInFunction.normal, y);
					if (num2 > num)
					{
						index = j;
						num = num2;
					}
				}
				if (!(num < tolerance))
				{
					float num3 = 0.6f / (float)(i + 2);
					convexBary *= 1f - num3;
					convexBary[index] += num3;
					GetInterpolatedSimplexData(simplexStart, simplexSize, simplices, positions, orientations, radii, convexBary, out convexPoint, out convexThickness, out convexOrientation);
					continue;
				}
				break;
			}
		}

		private static void GoldenSearch<T>(ref T function, int simplexStart, int simplexSize, NativeArray<float4> positions, NativeArray<quaternion> orientations, NativeArray<float4> radii, NativeArray<int> simplices, ref float4 convexPoint, ref float4 convexThickness, ref quaternion convexOrientation, ref float4 convexBary, ref SurfacePoint pointInFunction, int maxIterations, float tolerance) where T : struct, IDistanceFunction
		{
			SurfacePoint projectedPoint = default(SurfacePoint);
			float num = (math.sqrt(5f) + 1f) / 2f;
			float num2 = 0f;
			float num3 = 1f;
			float num4 = num3 - (num3 - num2) / num;
			float num5 = num2 + (num3 - num2) / num;
			for (int i = 0; i < maxIterations; i++)
			{
				if (math.abs(num3 - num2) < tolerance * (math.abs(num4) + math.abs(num5)))
				{
					break;
				}
				GetInterpolatedSimplexData(simplexStart, simplexSize, simplices, positions, orientations, radii, new float4(num4, 1f - num4, 0f, 0f), out convexPoint, out convexThickness, out convexOrientation);
				GetInterpolatedSimplexData(simplexStart, simplexSize, simplices, positions, orientations, radii, new float4(num5, 1f - num5, 0f, 0f), out var convexPoint2, out var convexRadii, out var convexOrientation2);
				function.Evaluate(convexPoint, convexThickness, convexOrientation, ref pointInFunction);
				function.Evaluate(convexPoint2, convexRadii, convexOrientation2, ref projectedPoint);
				float4 y = positions[simplices[simplexStart]] - pointInFunction.point;
				float4 y2 = positions[simplices[simplexStart + 1]] - projectedPoint.point;
				y.w = 0f;
				y2.w = 0f;
				y -= pointInFunction.normal * (radii[simplices[simplexStart]].x - convexThickness.x);
				y2 -= projectedPoint.normal * (radii[simplices[simplexStart + 1]].x - convexRadii.x);
				if (math.dot(-pointInFunction.normal, y) < math.dot(-projectedPoint.normal, y2))
				{
					num3 = num5;
				}
				else
				{
					num2 = num4;
				}
				num4 = num3 - (num3 - num2) / num;
				num5 = num2 + (num3 - num2) / num;
			}
			convexBary.y = 1f - (convexBary.x = (num3 + num2) * 0.5f);
			GetInterpolatedSimplexData(simplexStart, simplexSize, simplices, positions, orientations, radii, convexBary, out convexPoint, out convexThickness, out convexOrientation);
			function.Evaluate(convexPoint, convexThickness, convexOrientation, ref pointInFunction);
		}
	}
}
