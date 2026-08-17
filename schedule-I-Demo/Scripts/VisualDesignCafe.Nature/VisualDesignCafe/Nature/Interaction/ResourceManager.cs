using System.Collections.Generic;
using UnityEngine;

namespace VisualDesignCafe.Nature.Interaction
{
	internal class ResourceManager
	{
		private static List<Object> _objectsToDestroy = new List<Object>();

		private static Dictionary<PrimitiveType, Mesh> _meshLookup = new Dictionary<PrimitiveType, Mesh>();

		public static Mesh GetPrimitiveMesh(PrimitiveType type)
		{
			if (_meshLookup.TryGetValue(type, out var value) && value != null)
			{
				return value;
			}
			GameObject gameObject = GameObject.CreatePrimitive(type);
			gameObject.SetActive(value: false);
			value = gameObject.GetComponent<MeshFilter>().sharedMesh;
			_meshLookup[type] = value;
			Destroy(gameObject);
			return value;
		}

		public static void Destroy(Object obj)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(obj);
			}
			else
			{
				_objectsToDestroy.Add(obj);
			}
		}

		internal static void DestroyPending()
		{
			foreach (Object item in _objectsToDestroy)
			{
				if (item != null)
				{
					Object.DestroyImmediate(item);
				}
			}
			_objectsToDestroy.Clear();
		}
	}
}
