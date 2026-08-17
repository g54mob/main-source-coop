using System;
using System.Runtime.InteropServices;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Initial", name = "Noise", iconName = "GeneratorIcons/Noise", disengageable = true, codeFile = "MatrixInitial", codeLine = 36, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Noise")]
	public class Noise200 : Generator, IOutlet<MatrixWorld>, IUnit
	{
		public enum Type
		{
			Unity = 0,
			Linear = 1,
			Perlin = 2,
			Simplex = 3
		}

		[Val("Type")]
		public Type type = Type.Perlin;

		[Val("Seed")]
		public int seed = 12345;

		[Val("Intensity")]
		public float intensity = 1f;

		[Val("Size")]
		public float size = 200f;

		[Val("Detail")]
		public float detail = 0.5f;

		[Val("Turbulence")]
		public float turbulence;

		[Val("Offset")]
		public Vector2D offset = new Vector2D(0f, 0f);

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixInitial.cs", 39);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
			Noise noise = new Noise(data.random, seed);
			if (type != Type.Unity)
			{
				GeneratorNoise200(matrixWorld, noise, stop, (int)type, intensity, size, detail, turbulence, offset.x, offset.z, matrixWorld.worldPos.x, matrixWorld.worldPos.z, matrixWorld.worldSize.x, matrixWorld.worldSize.z);
			}
			else
			{
				Noise(matrixWorld, noise, stop, (int)type, intensity, size, detail, turbulence, offset);
			}
			data.StoreProduct(this, matrixWorld);
		}

		[DllImport("NativePlugins", CallingConvention = CallingConvention.Cdecl)]
		private static extern void GeneratorNoise200(Matrix matrix, Noise noise, StopToken stop, int type, float intensity, float size, float detail, float turbulence, float offsetX, float offsetZ, float worldRectPosX, float worldRectPosZ, float worldRectSizeX, float worldRectSizeZ);

		private static void Noise(MatrixWorld matrix, Noise noise, StopToken stop, int type, float intensity, float size, float detail, float turbulence, Vector2D offset)
		{
			int iterations = (int)Mathf.Log(size, 2f) + 1;
			Coord min = matrix.rect.Min;
			Coord max = matrix.rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					Vector2D vector2D = new Vector2D((float)(i - matrix.rect.offset.x) / (float)(matrix.rect.size.x - 1), (float)(j - matrix.rect.offset.z) / (float)(matrix.rect.size.z - 1));
					Vector2D vector2D2 = new Vector2D(vector2D.x * matrix.worldSize.x + matrix.worldPos.x, vector2D.z * matrix.worldSize.z + matrix.worldPos.z);
					float num = noise.Fractal(vector2D2.x + offset.x, vector2D2.z + offset.z, size, iterations, detail, turbulence, type);
					num *= intensity;
					if (num < 0f)
					{
						num = 0f;
					}
					if (num > 1f)
					{
						num = 1f;
					}
					matrix[i, j] += num;
				}
				if (stop != null && stop.stop)
				{
					break;
				}
			}
		}
	}
}
