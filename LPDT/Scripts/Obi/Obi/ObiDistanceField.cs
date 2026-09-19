using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[CreateAssetMenu(fileName = "distance field", menuName = "Obi/Distance Field", order = 181)]
	[ExecuteInEditMode]
	public class ObiDistanceField : ScriptableObject
	{
		[SerializeProperty("InputMesh")]
		[SerializeField]
		private Mesh input;

		[HideInInspector]
		[SerializeField]
		private float minNodeSize;

		[HideInInspector]
		[SerializeField]
		private Bounds bounds;

		[HideInInspector]
		public List<DFNode> nodes;

		[Range(1E-07f, 0.1f)]
		public float maxError = 0.01f;

		[Range(1f, 8f)]
		public int maxDepth = 5;

		public bool Initialized => nodes != null;

		public Bounds FieldBounds => bounds;

		public float EffectiveSampleSize => minNodeSize;

		public Mesh InputMesh
		{
			get
			{
				return input;
			}
			set
			{
				if (value != input)
				{
					Reset();
					input = value;
				}
			}
		}

		public void Reset()
		{
			nodes = null;
			if (input != null)
			{
				bounds = input.bounds;
			}
		}

		public IEnumerator Generate()
		{
			Reset();
			if (!(input == null))
			{
				int[] triangles = input.triangles;
				Vector3[] vertices = input.vertices;
				nodes = new List<DFNode>();
				IEnumerator buildingCoroutine = ASDF.Build(maxError, maxDepth, vertices, triangles, nodes);
				while (buildingCoroutine.MoveNext())
				{
					yield return new CoroutineJob.ProgressInfo("Processed nodes: " + nodes.Count, 1f);
				}
				minNodeSize = float.PositiveInfinity;
				for (int i = 0; i < nodes.Count; i++)
				{
					minNodeSize = Mathf.Min(minNodeSize, nodes[i].center[3] * 2f);
				}
				float num = Mathf.Max(bounds.size[0], Mathf.Max(bounds.size[1], bounds.size[2])) + 0.2f;
				bounds.size = new Vector3(num, num, num);
			}
		}

		public Texture3D GetVolumeTexture(int size)
		{
			if (!Initialized)
			{
				return null;
			}
			float num = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
			float num2 = bounds.size.x / (float)size;
			float num3 = bounds.size.y / (float)size;
			float num4 = bounds.size.z / (float)size;
			Texture3D texture3D = new Texture3D(size, size, size, TextureFormat.Alpha8, mipChain: false);
			Color[] array = new Color[size * size * size];
			int num5 = 0;
			Color black = Color.black;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					int num6 = 0;
					while (num6 < size)
					{
						Vector3 position = bounds.min + new Vector3(num2 * (float)num6 + num2 * 0.5f, num3 * (float)j + num3 * 0.5f, num4 * (float)i + num4 * 0.5f);
						float num7 = ASDF.Sample(nodes, position);
						if (num7 >= 0f)
						{
							black.a = num7.Remap(0f, num * 0.1f, 0.5f, 1f);
						}
						else
						{
							black.a = num7.Remap((0f - num) * 0.1f, 0f, 0f, 0.5f);
						}
						array[num5] = black;
						num6++;
						num5++;
					}
				}
			}
			texture3D.SetPixels(array);
			texture3D.Apply();
			return texture3D;
		}
	}
}
