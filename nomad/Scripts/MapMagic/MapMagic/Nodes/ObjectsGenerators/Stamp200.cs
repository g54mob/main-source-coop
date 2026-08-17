using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Stamp", iconName = "GeneratorIcons/Stamp", disengageable = true, colorType = typeof(TransitionsList), helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Stamp")]
	public class Stamp200 : Generator, IMultiInlet, IOutlet<MatrixWorld>, IUnit
	{
		public enum BlendType
		{
			Max = 0,
			Add = 1
		}

		[Val("Positions", "Inlet")]
		public readonly Inlet<TransitionsList> positionsIn = new Inlet<TransitionsList>();

		[Val("Stamp", "Inlet")]
		public readonly Inlet<MatrixWorld> stampIn = new Inlet<MatrixWorld>();

		[Val("Size", "Custom")]
		public float size = 1f;

		[Val("Intensity", "Custom")]
		public float intensity = 1f;

		[Val("Hardness", "UseFallof")]
		public float hardness = 0.8f;

		[Val("Size Factor", "Custom")]
		public float sizeFactor = 1f;

		[Val("Intensity Factor", "Custom")]
		public float intensityFactor = 1f;

		public BlendType blendType;

		[Val("Use Rotation", "Custom", isLeft = true)]
		public bool useRotation = true;

		[Val("Use Falloff", "Custom", isLeft = true)]
		public bool useFalloff;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 939);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return positionsIn;
			yield return stampIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			TransitionsList transitionsList = data.ReadInletProduct(positionsIn);
			MatrixWorld matrixWorld = data.ReadInletProduct(stampIn);
			if (transitionsList == null || matrixWorld == null || !enabled)
			{
				return;
			}
			MatrixWorld matrixWorld2 = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
			for (int i = 0; i < transitionsList.count; i++)
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				float num = size / 2f;
				float num2 = num * (1f - sizeFactor) + num * transitionsList.arr[i].scale.y * sizeFactor;
				if (useRotation)
				{
					num2 *= 1.4142135f;
				}
				StampMatrix(matrixWorld2, matrixWorld, data.area.active.rect, transitionsList.arr[i].pos, transitionsList.arr[i].rotation, num2, hardness, intensity * (1f - intensityFactor) + intensity * transitionsList.arr[i].scale.y * intensityFactor, blendType == BlendType.Add, useRotation, useFalloff);
			}
			data.StoreProduct(this, matrixWorld2);
		}

		private static void StampMatrix(MatrixWorld matrix, MatrixWorld stamp, CoordRect stampRect, Vector3 center, Quaternion rotation, float radius, float hardness, float intensity, bool blendAdditive, bool useRotation, bool useFalloff)
		{
			Vector2D center2 = (Vector2D)matrix.WorldToPixelInterpolated(center.x, center.z);
			float num = matrix.WorldDistToPixelInterpolated(radius);
			CoordRect c = new CoordRect(center2, num);
			CoordRect coordRect = CoordRect.Intersected(matrix.rect, c);
			Coord min = coordRect.Min;
			Coord max = coordRect.Max;
			Coord coord = default(Coord);
			Vector2D rotationDirection = new Vector2D(0f, 1f);
			if (useRotation)
			{
				rotationDirection = (Vector2D)(rotation * new Vector3(1f, 0f, 0f));
			}
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					float num2 = 1f * (float)(i - c.offset.x) / (float)(c.size.x - 1);
					float num3 = 1f * (float)(j - c.offset.z) / (float)(c.size.z - 1);
					if (num2 > 0.999f)
					{
						num2 = 0.999f;
					}
					if (num3 > 0.999f)
					{
						num3 = 0.999f;
					}
					float num4 = 1f;
					if (useFalloff)
					{
						coord.x = i;
						coord.z = j;
						num4 = coord.GetInterpolatedFalloff(center2, num / (useRotation ? 1.4142135f : 1f), hardness);
						if (num4 < 1E-05f)
						{
							continue;
						}
					}
					float num5 = 0f;
					if (useRotation)
					{
						num2 = (num2 - 0.5f) * 1.4142135f + 0.5f;
						num3 = (num3 - 0.5f) * 1.4142135f + 0.5f;
						(num2, num3) = GetRelativeRotatedCoords(num2, num3, rotationDirection, new Vector2D(0.5f, 0.5f));
					}
					float fx = num2 * (float)stampRect.size.x + (float)stampRect.offset.x;
					float fz = num3 * (float)stampRect.size.z + (float)stampRect.offset.z;
					num5 = stamp.GetFloored(fx, fz);
					num5 *= intensity;
					int num6 = (j - matrix.rect.offset.z) * matrix.rect.size.x + i - matrix.rect.offset.x;
					if (blendAdditive)
					{
						matrix.arr[num6] += num5 * num4;
					}
					else if (num5 * num4 > matrix.arr[num6])
					{
						matrix.arr[num6] = num5 * num4;
					}
				}
			}
		}

		private static (float, float) GetRelativeRotatedCoords(float sx, float sz, Vector2D rotationDirection, Vector2D rotationPivot)
		{
			float num = sx - rotationPivot.x;
			float num2 = sz - rotationPivot.z;
			Vector2D vector2D = rotationDirection * num;
			Vector2D vector2D2 = new Vector2D(rotationDirection.z, 0f - rotationDirection.x) * num2;
			Vector2D vector2D3 = vector2D + vector2D2;
			vector2D3.x += rotationPivot.x;
			vector2D3.z += rotationPivot.z;
			if (vector2D3.x < 0f)
			{
				vector2D3.x = 0f;
			}
			if (vector2D3.x > 0.999f)
			{
				vector2D3.x = 0.999f;
			}
			if (vector2D3.z < 0f)
			{
				vector2D3.z = 0f;
			}
			if (vector2D3.z > 0.999f)
			{
				vector2D3.z = 0.999f;
			}
			return (vector2D3.x, vector2D3.z);
		}
	}
}
