using System;
using System.Collections.Generic;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Ledge", iconName = "GeneratorIcons/Ledge", disengageable = true, advancedOptions = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Ledge")]
	public class Ledge210 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>, IMultiInlet
	{
		[Val("Level")]
		public float level = 50f;

		[Val("Contour Blur")]
		public float contourBlur = 3f;

		[Val("Height")]
		public float height = 10f;

		[Val("Steep")]
		public float steep = 30f;

		[Val("Top Shoulder", "Advanced")]
		public float topShoulder = 2f;

		[Val("Bottom Shoulder", "Advanced")]
		public float bottomShoulder = 2f;

		[Val("Smooth", "Advanced")]
		public bool smooth = true;

		[Val("Mask", "Inlet")]
		public readonly Inlet<MatrixWorld> heightMaskIn = new Inlet<MatrixWorld>();

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 741);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return heightMaskIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = data.ReadInletProduct(this);
			if (matrixWorld == null)
			{
				return;
			}
			if (!enabled)
			{
				data.StoreProduct(this, matrixWorld);
			}
			else if (stop == null || !stop.stop)
			{
				MatrixWorld intensity = data.ReadInletProduct(heightMaskIn);
				Matrix mask = LedgeMask(matrixWorld, contourBlur);
				MatrixWorld matrixWorld2 = new MatrixWorld(matrixWorld.rect, matrixWorld.worldPos, matrixWorld.worldSize);
				LedgeStep(matrixWorld, mask, intensity, matrixWorld2, level / data.globals.height, height / data.globals.height, steep, bottomShoulder, topShoulder, smooth);
				if (stop == null || !stop.stop)
				{
					data.StoreProduct(this, matrixWorld2);
				}
			}
		}

		public static Matrix LedgeMask(Matrix src, float blur)
		{
			Matrix matrix;
			if (blur > 2f)
			{
				matrix = new Matrix(src);
				MatrixOps.DownsampleBlur(matrix, (int)blur, 1.75f);
			}
			else if (blur > 0.001f)
			{
				matrix = new Matrix(src);
				MatrixOps.GaussianBlur(matrix, blur);
			}
			else
			{
				matrix = src;
			}
			return matrix;
		}

		public static void LedgeStep(Matrix src, Matrix mask, Matrix intensity, Matrix dst, float minFrom, float maxFrom, float minTo, float maxTo, float bottomShoulder, float topShoulder)
		{
			for (int i = 0; i < dst.arr.Length; i++)
			{
				float num = src.arr[i];
				float num2 = mask.arr[i];
				if (num2 < minFrom)
				{
					float num3 = num / minFrom;
					num3 = (num3 / bottomShoulder + (bottomShoulder - 1f) / bottomShoulder) * num3 + num3 * (1f - num3);
					dst.arr[i] = minTo * num3;
					continue;
				}
				if (num2 > maxFrom)
				{
					float num4 = (num - maxFrom) / (1f - maxFrom);
					num4 = num4 / topShoulder * (1f - num4) + num4 * num4;
					dst.arr[i] = maxTo + (1f - maxTo) * num4;
					continue;
				}
				float num5 = (num - minFrom) / (maxFrom - minFrom);
				float num6 = (num5 / topShoulder + (topShoulder - 1f) / topShoulder) * num5 + num5 * (1f - num5);
				float num7 = num5 / bottomShoulder * (1f - num5) + num5 * num5;
				float num8 = 3f * num5 * num5 - 2f * num5 * num5 * num5;
				num5 = num7 * (1f - num8) + num6 * num8;
				dst.arr[i] = minTo + (maxTo - minTo) * num5;
			}
		}

		public static void LedgeStep(Matrix src, Matrix mask, Matrix intensity, Matrix dst, float level, float height, float steep, float bottomShoulder, float topShoulder, bool smooth = true)
		{
			for (int i = 0; i < dst.arr.Length; i++)
			{
				float num = ((intensity != null) ? intensity.arr[i] : 1f);
				float num2 = src.arr[i];
				float num3 = mask.arr[i];
				float num4 = level - 1f / steep * num;
				float num5 = level + 1f / steep * num;
				float num6 = num4 - height * num / 2f * bottomShoulder;
				float num7 = num5 + height * num / 2f * topShoulder;
				if (num3 > num4 && num3 < num5)
				{
					float num8 = (num3 - num4) / (num5 - num4);
					if (smooth)
					{
						num8 = 3f * num8 * num8 - 2f * num8 * num8 * num8;
					}
					dst.arr[i] = (num2 - height * num / 2f) * (1f - num8) + (num2 + height * num / 2f) * num8;
				}
				else if (num3 > num6 && num3 < level)
				{
					float num9 = (num3 - num6) / (num4 - num6);
					if (smooth)
					{
						num9 = 3f * num9 * num9 - 2f * num9 * num9 * num9;
					}
					dst.arr[i] = num2 - height * num / 2f * num9;
				}
				else if (num3 < num7 && num3 > level)
				{
					float num10 = (num7 - num3) / (num7 - num5);
					if (smooth)
					{
						num10 = 3f * num10 * num10 - 2f * num10 * num10 * num10;
					}
					dst.arr[i] = num2 + height * num / 2f * num10;
				}
				else
				{
					dst.arr[i] = num2;
				}
			}
		}
	}
}
