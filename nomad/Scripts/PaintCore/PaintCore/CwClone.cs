using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	public abstract class CwClone : MonoBehaviour, IClone
	{
		[NonSerialized]
		public static int MatrixCount;

		[NonSerialized]
		public static int ClonerCount;

		[NonSerialized]
		private static List<Matrix4x4> tempPosMatrices = new List<Matrix4x4>();

		[NonSerialized]
		private static List<Matrix4x4> tempRotMatrices = new List<Matrix4x4>();

		[NonSerialized]
		private static List<Matrix4x4> tempRotMatrices2 = new List<Matrix4x4>();

		[NonSerialized]
		private static List<IClone> tempCloners = new List<IClone>();

		private static LinkedList<CwClone> instances = new LinkedList<CwClone>();

		private LinkedListNode<CwClone> instancesNode;

		public static LinkedList<CwClone> Instances => instances;

		public abstract void Transform(ref Matrix4x4 posMatrix, ref Matrix4x4 rotMatrix, ref Matrix4x4 rotMatrix2);

		public static void BuildCloners(List<IClone> cloners = null)
		{
			tempCloners.Clear();
			tempPosMatrices.Clear();
			tempRotMatrices.Clear();
			tempRotMatrices2.Clear();
			tempPosMatrices.Add(Matrix4x4.identity);
			tempRotMatrices.Add(Matrix4x4.identity);
			tempRotMatrices2.Add(Matrix4x4.identity);
			if (cloners != null)
			{
				for (int i = 0; i < cloners.Count; i++)
				{
					IClone clone = cloners[i];
					if (clone != null)
					{
						tempCloners.Add(clone);
					}
				}
			}
			else
			{
				foreach (CwClone instance in instances)
				{
					tempCloners.Add(instance);
				}
			}
			MatrixCount = 1;
			ClonerCount = tempCloners.Count;
		}

		public static void Clone(CwCommand command, int clonerIndex, int matrixIndex)
		{
			if (matrixIndex == 0)
			{
				MatrixCount = tempPosMatrices.Count;
			}
			Matrix4x4 posMatrix = tempPosMatrices[matrixIndex];
			Matrix4x4 rotMatrix = tempRotMatrices[matrixIndex];
			Matrix4x4 rotMatrix2 = tempRotMatrices2[matrixIndex];
			tempCloners[clonerIndex].Transform(ref posMatrix, ref rotMatrix, ref rotMatrix2);
			tempPosMatrices.Add(posMatrix);
			tempRotMatrices.Add(rotMatrix);
			tempRotMatrices2.Add(rotMatrix2);
			command.Transform(posMatrix, rotMatrix, rotMatrix2);
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode);
			instancesNode = null;
		}
	}
}
