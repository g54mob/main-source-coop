using System;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Terrace", iconName = "GeneratorIcons/Terrace", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Terrace")]
	public class Terrace200 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>
	{
		[Val("Seed")]
		public int seed = 12345;

		[Val("Num")]
		public int num = 10;

		[Val("Uniformity")]
		public float uniformity = 0.5f;

		[Val("Steepness")]
		public float steepness = 0.5f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 648);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = data.ReadInletProduct(this);
			if (matrixWorld == null || num <= 1)
			{
				return;
			}
			if (!enabled)
			{
				data.StoreProduct(this, matrixWorld);
				return;
			}
			MatrixWorld matrixWorld2 = new MatrixWorld(matrixWorld);
			float[] terraces = TerraceLevels(new Noise(data.random, seed), num, uniformity);
			if (stop == null || !stop.stop)
			{
				matrixWorld2.Terrace(terraces, steepness);
				data.StoreProduct(this, matrixWorld2);
			}
		}

		public static float[] TerraceLevels(Noise random, int num, float uniformity)
		{
			float[] array = new float[num];
			float num2 = 1f / (float)(num - 1);
			for (int i = 1; i < num; i++)
			{
				array[i] = array[i - 1] + num2;
			}
			for (int j = 0; j < 10; j++)
			{
				for (int k = 1; k < num - 1; k++)
				{
					float num3 = random.Random(j);
					num3 = array[k - 1] + num3 * (array[k + 1] - array[k - 1]);
					array[k] = array[k] * uniformity + num3 * (1f - uniformity);
				}
			}
			return array;
		}
	}
}
