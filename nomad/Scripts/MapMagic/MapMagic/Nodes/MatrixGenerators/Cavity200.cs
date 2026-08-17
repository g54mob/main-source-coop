using System;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Cavity", iconName = "GeneratorIcons/Cavity", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Cavity")]
	public class Cavity200 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>
	{
		public enum CavityType
		{
			Convex = 0,
			Concave = 1,
			Both = 2
		}

		[Val("Type")]
		public CavityType type;

		[Val("Intensity")]
		public float intensity = 3f;

		[Val("Spread")]
		public float spread = 10f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 450);
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
			else
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				MatrixWorld matrixWorld2 = new MatrixWorld(matrixWorld.rect, matrixWorld.worldPos, matrixWorld.worldSize);
				if (stop == null || !stop.stop)
				{
					Cavity(matrixWorld, matrixWorld2, type, intensity, spread, matrixWorld.PixelSize.x, data.area.active.worldSize, stop);
					if (stop == null || !stop.stop)
					{
						data.StoreProduct(this, matrixWorld2);
					}
				}
			}
		}

		public static void Cavity(Matrix src, Matrix dst, CavityType cavityType, float intensity, float spread, float pixelSize, Vector2D worldSize, StopToken stop)
		{
			MatrixOps.Cavity(src, dst);
			dst.Multiply(1f / Mathf.Pow(pixelSize, 0.25f));
			float f = worldSize.x / spread;
			float num = Mathf.Log(src.rect.size.x, 2f);
			num -= Mathf.Log(f, 2f);
			if (stop != null && stop.stop)
			{
				return;
			}
			MatrixOps.OverblurMipped(dst, Mathf.Max(0f, num), 1.5f);
			if (stop != null && stop.stop)
			{
				return;
			}
			dst.Multiply(intensity * 100f);
			switch (cavityType)
			{
			case CavityType.Convex:
				dst.Invert();
				break;
			case CavityType.Both:
				dst.Invert();
				dst.Multiply(0.5f);
				dst.Add(0.5f);
				break;
			}
			dst.Clamp01();
			if ((stop == null || !stop.stop) && num < 0f)
			{
				float num2 = (0f - num) / 4f;
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				float val = dst.Average();
				dst.Fill(val, num2);
			}
		}
	}
}
