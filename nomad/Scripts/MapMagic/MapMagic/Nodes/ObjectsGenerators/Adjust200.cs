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
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Adjust", iconName = "GeneratorIcons/Adjust", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/object_generators/Adjust")]
	public class Adjust200 : Generator, IMultiInlet, IOutlet<TransitionsList>, IUnit
	{
		public enum Relativeness
		{
			absolute = 0,
			relative = 1
		}

		[Val("Input", "Inlet")]
		public readonly Inlet<TransitionsList> input = new Inlet<TransitionsList>();

		[Val("Intensity", "Inlet")]
		public readonly Inlet<MatrixWorld> intensityIn = new Inlet<MatrixWorld>();

		public bool useRandom;

		public int seed = 12345;

		public float sizeFactor;

		public Relativeness relativeness = Relativeness.relative;

		public Vector2 offsetFront = Vector2.zero;

		public Vector2 offsetRight = Vector2.zero;

		public Vector2 height = Vector2.zero;

		public Vector2 rotation = Vector2.zero;

		public Vector2 scale = Vector2.one;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 17);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return input;
			yield return intensityIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			TransitionsList transitionsList = data.ReadInletProduct(input);
			if (transitionsList == null)
			{
				return;
			}
			if (!enabled)
			{
				data.StoreProduct(this, transitionsList);
				return;
			}
			TransitionsList transitionsList2 = new TransitionsList(transitionsList);
			MatrixWorld intensityMatrix = data.ReadInletProduct(intensityIn);
			Noise rnd = (useRandom ? new Noise(data.random, seed) : null);
			for (int i = 0; i < transitionsList2.count; i++)
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				Adjust(ref transitionsList2.arr[i], intensityMatrix, rnd);
			}
			data.StoreProduct(this, transitionsList2);
		}

		public void Adjust(ref Transition trn, MatrixWorld intensityMatrix, Noise rnd)
		{
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			if (rnd != null)
			{
				num = rnd.Random(trn.hash, 0);
				num = height.x + num * (height.y - height.x);
				num2 = rnd.Random(trn.hash, 1);
				num2 = rotation.x + num2 * (rotation.y - rotation.x);
				num3 = rnd.Random(trn.hash, 2);
				num3 = scale.x + num3 * (scale.y - scale.x);
				num4 = rnd.Random(trn.hash, 3);
				num4 = scale.x + num4 * (offsetFront.y - offsetFront.x);
				num5 = rnd.Random(trn.hash, 3);
				num5 = scale.x + num4 * (offsetRight.y - offsetRight.x);
			}
			else
			{
				num = height.x;
				num2 = rotation.x;
				num3 = scale.x;
				num4 = offsetFront.x;
				num5 = offsetRight.x;
			}
			float num6 = 1f;
			if (intensityMatrix != null)
			{
				num6 = (intensityMatrix.ContainsWorldValue(trn.pos.x, trn.pos.z) ? intensityMatrix.GetWorldValue(trn.pos.x, trn.pos.z) : 0f);
			}
			if (relativeness == Relativeness.relative)
			{
				trn.scale *= num3 * num6;
				num6 = num6 * (1f - sizeFactor) + num6 * trn.scale.x * sizeFactor;
				var (vector2D, vector2D2) = trn.FrontRight2D;
				trn.pos += (Vector3)vector2D * num4 * num6;
				trn.pos += (Vector3)vector2D2 * num5 * num6;
				trn.pos.y += num * num6;
				trn.Yaw += num2 * num6;
			}
			else
			{
				trn.scale = new Vector3(1f, 1f, 1f) * num3 * num6;
				num6 = num6 * (1f - sizeFactor) + num6 * trn.scale.x * sizeFactor;
				var (vector2D3, vector2D4) = trn.FrontRight2D;
				trn.pos += (Vector3)vector2D3 * num4 * num6;
				trn.pos += (Vector3)vector2D4 * num5 * num6;
				trn.pos.y = num * num6;
				trn.Yaw = num2 * num6;
			}
		}
	}
}
