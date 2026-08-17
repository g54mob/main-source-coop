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
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Forest", iconName = "GeneratorIcons/Forest", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Forest")]
	public class Forest200 : Generator, IMultiInlet, IOutlet<TransitionsList>, IUnit
	{
		[Val("Seedlings", "Inlet")]
		public readonly Inlet<TransitionsList> seedlingsIn = new Inlet<TransitionsList>();

		[Val("Other Trees", "Inlet")]
		public readonly Inlet<TransitionsList> otherTreesIn = new Inlet<TransitionsList>();

		[Val("Soil", "Inlet")]
		public readonly Inlet<MatrixWorld> soilIn = new Inlet<MatrixWorld>();

		[Val("Years")]
		public int years = 100;

		[Val("Density")]
		public float density = 10000f;

		[Val("Fecundity")]
		public float fecundity = 0.5f;

		[Val("Seed Dist")]
		public float seedDist = 15f;

		[Val("Reproductive Age")]
		public float reproductiveAge = 10f;

		[Val("Survival Rate")]
		public float survivalRate = 0.95f;

		[Val("Life Age")]
		public float lifeAge = 100f;

		[Val("Size Is Age")]
		public bool sizeIsLife = true;

		[Val("Seed")]
		public int seed = 12345;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 1076);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return seedlingsIn;
			yield return otherTreesIn;
			yield return soilIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			TransitionsList transitionsList = data.ReadInletProduct(seedlingsIn);
			if (transitionsList != null)
			{
				if (!enabled)
				{
					data.StoreProduct(this, transitionsList);
					return;
				}
				TransitionsList otherTrees = data.ReadInletProduct(otherTreesIn);
				MatrixWorld soil = data.ReadInletProduct(soilIn);
				Noise random = new Noise(data.random, seed);
				TransitionsList product = Forest(transitionsList, otherTrees, soil, (Vector3)data.area.full.worldPos, (Vector3)data.area.full.worldSize, random, stop);
				data.StoreProduct(this, product);
			}
		}

		public TransitionsList Forest(TransitionsList seedlings, TransitionsList otherTrees, MatrixWorld soil, Vector3 worldPos, Vector3 worldSize, Noise random, StopToken stop = null)
		{
			float num = 1000f / Mathf.Sqrt(density);
			CoordRect rect = CoordRect.WorldToPixel((Vector2D)worldPos, (Vector2D)worldSize, (Vector2D)num);
			worldPos = new Vector3((float)rect.offset.x * num, worldPos.y, (float)rect.offset.z * num);
			worldSize = new Vector3((float)rect.size.x * num, worldSize.y, (float)rect.size.z * num);
			PositionMatrix positionMatrix = new PositionMatrix(rect, worldPos, worldSize);
			positionMatrix.Scatter(0f, random, 0f);
			positionMatrix = positionMatrix.Relaxed();
			PositionMatrix positionMatrix2 = new PositionMatrix(rect, worldPos, worldSize);
			if (otherTrees != null)
			{
				positionMatrix2.AddTransitionsList(otherTrees, 1f);
			}
			for (int i = 0; i < years; i++)
			{
				if (i == 0)
				{
					TransitionsToMatrix(positionMatrix, seedlings, sizeIsLife);
				}
				Coord min = positionMatrix.rect.Min;
				Coord max = positionMatrix.rect.Max;
				for (int j = min.x; j < max.x; j++)
				{
					if (stop != null && stop.stop)
					{
						return null;
					}
					for (int k = min.z; k < max.z; k++)
					{
						float height = positionMatrix.GetHeight(j, k);
						if (height < 0.5f)
						{
							continue;
						}
						int z = (int)height - 1;
						positionMatrix.SetHeight(j, k, height += 1f);
						float num2 = survivalRate;
						if (soil != null)
						{
							Vector3 vector = positionMatrix[j, k];
							num2 = (soil.ContainsWorldValue(vector.x, vector.z) ? (num2 * soil.GetWorldValue(vector.x, vector.z)) : 0f);
						}
						if (height > lifeAge || random.Random(j, k, z, 0) > num2)
						{
							positionMatrix.SetHeight(j, k, 0f);
						}
						if (height > reproductiveAge && random.Random(j, k, z, 1) < fecundity)
						{
							float f = random.Random(j, k, z, 2) * 6.283f;
							float num3 = random.Random(j, k, z, 3) * seedDist / positionMatrix.cellSize + 1f;
							int x = (int)((float)j + Mathf.Sin(f) * num3);
							int z2 = (int)((float)k + Mathf.Cos(f) * num3);
							if (positionMatrix.rect.Contains(x, z2) && positionMatrix.GetHeight(x, z2) < 0.5f && positionMatrix2.GetHeight(x, z2) < 0.01f)
							{
								positionMatrix.SetHeight(x, z2, 1f);
							}
						}
					}
				}
			}
			TransitionsList transitionsList = new TransitionsList();
			MatrixToTransitions(positionMatrix, transitionsList, sizeIsLife);
			return transitionsList;
		}

		private void TransitionsToMatrix(PositionMatrix matrix, TransitionsList trns, bool scaleIsAge)
		{
			for (int i = 0; i < trns.count; i++)
			{
				if (!(trns.arr[i].pos.x < matrix.worldPos.x) && !(trns.arr[i].pos.x > matrix.worldPos.x + matrix.worldSize.x) && !(trns.arr[i].pos.z < matrix.worldPos.z) && !(trns.arr[i].pos.z > matrix.worldPos.z + matrix.worldSize.z))
				{
					float num = (scaleIsAge ? trns.arr[i].scale.y : (reproductiveAge + 1f));
					Coord coord = matrix.GetCoord(trns.arr[i].pos);
					float height = matrix.GetHeight(coord.x, coord.z);
					if (num > height)
					{
						matrix.SetPosition(new Vector3(trns.arr[i].pos.x, num, trns.arr[i].pos.z));
					}
				}
			}
		}

		private static void MatrixToTransitions(PositionMatrix matrix, TransitionsList trns, bool scaleIsAge)
		{
			Coord min = matrix.rect.Min;
			Coord max = matrix.rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					Vector3 vector = matrix[i, j];
					if (!(vector.y < 1.5f))
					{
						Transition trs = new Transition(vector.x, vector.z);
						if (scaleIsAge)
						{
							trs.scale.x = vector.y;
							trs.scale.y = vector.y;
							trs.scale.z = vector.y;
						}
						trs.hash = i * 2000 + j;
						trns.Add(trs);
					}
				}
			}
		}
	}
}
