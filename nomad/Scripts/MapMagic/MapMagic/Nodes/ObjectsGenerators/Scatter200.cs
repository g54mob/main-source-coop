using System;
using Den.Tools;
using Den.Tools.GUI;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Initial", name = "Scatter", iconName = "GeneratorIcons/Scatter", disengageable = true, advancedOptions = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Scatter")]
	public class Scatter200 : Generator, IOutlet<TransitionsList>, IUnit
	{
		[Val("Seed")]
		public int seed = 12345;

		[Val("Density")]
		public float density = 10f;

		[Val("Uniformity")]
		public float uniformity = 0.1f;

		[Val("Relax", "Advanced")]
		public float relax = 0.5f;

		[Val("Add.Margin", "Advanced")]
		public float additionalMargins;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsInitial.cs", 47);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (enabled)
			{
				if (density == 0f)
				{
					data.StoreProduct(this, new TransitionsList());
					return;
				}
				Noise random = new Noise(data.random, seed);
				TransitionsList product = Scatter((Vector3)data.area.full.worldPos - new Vector3(additionalMargins, 0f, additionalMargins), (Vector3)data.area.full.worldSize + new Vector3(additionalMargins * 2f, 0f, additionalMargins * 2f), random);
				data.StoreProduct(this, product);
			}
		}

		public TransitionsList Scatter(Vector3 worldPos, Vector3 worldSize, Noise random)
		{
			float num = 1000f / Mathf.Sqrt(density);
			CoordRect rect = CoordRect.WorldToPixel((Vector2D)worldPos, (Vector2D)worldSize, (Vector2D)num);
			worldPos = new Vector3((float)rect.offset.x * num, worldPos.y, (float)rect.offset.z * num);
			worldSize = new Vector3((float)rect.size.x * num, worldSize.y, (float)rect.size.z * num);
			rect.offset -= 1;
			rect.size += 2;
			worldPos.x -= num;
			worldPos.z -= num;
			worldSize.x += num * 2f;
			worldSize.z += num * 2f;
			PositionMatrix positionMatrix = new PositionMatrix(rect, worldPos, worldSize);
			positionMatrix.Scatter(uniformity, random);
			return positionMatrix.Relaxed(relax).ToTransitionsList();
		}
	}
}
