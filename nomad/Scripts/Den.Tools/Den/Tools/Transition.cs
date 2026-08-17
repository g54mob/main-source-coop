using System;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public struct Transition
	{
		public Vector3 pos;

		public Quaternion rotation;

		public Vector3 scale;

		public float terrainHeight;

		public int id;

		public int hash;

		public int num;

		[NonSerialized]
		public Transform instance;

		public float Yaw
		{
			get
			{
				return Mathf.Acos(rotation.w) / ((float)Math.PI / 180f) * 2f;
			}
			set
			{
				float f = value / 2f * ((float)Math.PI / 180f);
				rotation.w = Mathf.Cos(f);
				rotation.z = 0f;
				rotation.y = Mathf.Sin(f);
				rotation.x = 0f;
			}
		}

		public (Vector2D, Vector2D) FrontRight2D
		{
			get
			{
				float yaw = Yaw;
				Vector2D item = new Vector2D(Mathf.Sin(yaw * ((float)Math.PI / 180f)), Mathf.Cos(yaw * ((float)Math.PI / 180f)));
				return new ValueTuple<Vector2D, Vector2D>(item2: new Vector2D(item.z, 0f - item.x), item1: item);
			}
		}

		public Transition(float x, float z)
		{
			pos = new Vector3(x, 0f, z);
			rotation = new Quaternion(0f, 0f, 0f, 1f);
			scale = new Vector3(1f, 1f, 1f);
			terrainHeight = 0f;
			id = 0;
			hash = 0;
			num = 0;
			instance = null;
		}

		public Transition(float x, float y, float z)
		{
			pos = new Vector3(x, y, z);
			rotation = new Quaternion(0f, 0f, 0f, 1f);
			scale = new Vector3(1f, 1f, 1f);
			terrainHeight = 0f;
			id = 0;
			hash = 0;
			num = 0;
			instance = null;
		}

		public Transition(Transform transform)
		{
			pos = transform.position;
			rotation = transform.rotation;
			scale = transform.localScale;
			terrainHeight = 0f;
			id = 0;
			hash = 0;
			num = 0;
			instance = transform;
		}

		public Transition(TreeInstance tree, Vector3 terrainPos, Vector3 terrainSize, Transform prototypePrefab = null)
		{
			pos = new Vector3(tree.position.x * terrainSize.x, tree.position.y * terrainSize.y, tree.position.z * terrainSize.z) + terrainPos;
			rotation = Quaternion.Euler(0f, tree.rotation, 0f);
			scale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
			terrainHeight = 0f;
			id = 0;
			hash = 0;
			num = tree.prototypeIndex;
			instance = prototypePrefab;
		}
	}
}
