using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Core;
using UnityEngine;

namespace MapMagic.Terrains
{
	[ExecuteInEditMode]
	public class DirectMatricesHolder : MonoBehaviour, ISerializationCallbackReceiver
	{
		[NonSerialized]
		public DictionaryOrdered<string, MatrixWorld> maps = new DictionaryOrdered<string, MatrixWorld>();

		[SerializeField]
		private string[] serNames;

		[SerializeField]
		private MatrixWorld[] serMaps;

		private MatrixWorld this[string name]
		{
			get
			{
				if (maps.TryGetValue(name, out var value))
				{
					return value;
				}
				throw new Exception("Matrix with the name '" + name + "' is not found in dictionary");
			}
		}

		private static void CheckPosition(MatrixWorld matrix, float x, float z)
		{
			if (!matrix.ContainsWorldValue(x, z))
			{
				throw new Exception($"Position {x},{z} is out of matrix bounds");
			}
		}

		public bool ContainsPosition(float x, float z)
		{
			if (maps.Count == 0)
			{
				return false;
			}
			return maps[0].ContainsWorldValue(x, z);
		}

		public bool ContainsPosition(Vector3 pos)
		{
			if (maps.Count == 0)
			{
				return false;
			}
			return maps[0].ContainsWorldValue(pos.x, pos.z);
		}

		public float ValueAtPosition(string name, float x, float z)
		{
			return this[name].GetWorldValue(x, z);
		}

		public float ValueAtPosition(string name, Vector3 pos)
		{
			return this[name].GetWorldValue(pos.x, pos.z);
		}

		public float ValueAtPosition(int num, float x, float z)
		{
			return maps[num].GetWorldValue(x, z);
		}

		public float ValueAtPosition(int num, Vector3 pos)
		{
			return maps[num].GetWorldValue(pos.x, pos.z);
		}

		public float ValueAtPositionInterpolated(string name, float x, float z)
		{
			return this[name].GetWorldInterpolatedValue(x, z);
		}

		public float ValueAtPositionInterpolated(string name, Vector3 pos)
		{
			return this[name].GetWorldInterpolatedValue(pos.x, pos.z);
		}

		public float ValueAtPositionInterpolated(int num, float x, float z)
		{
			return maps[num].GetWorldInterpolatedValue(x, z);
		}

		public float ValueAtPositionInterpolated(int num, Vector3 pos)
		{
			return maps[num].GetWorldInterpolatedValue(pos.x, pos.z);
		}

		public Dictionary<string, float> AllValuesAtPosition(float x, float z)
		{
			Dictionary<string, float> dictionary = new Dictionary<string, float>();
			foreach (KeyValuePair<string, MatrixWorld> map in maps)
			{
				MatrixWorld value = map.Value;
				CheckPosition(value, x, z);
				dictionary.Add(map.Key, value.GetWorldValue(x, z));
			}
			return dictionary;
		}

		public static DirectMatricesHolder FindHolder(float x, float z)
		{
			DirectMatricesHolder[] array = UnityEngine.Object.FindObjectsOfType<DirectMatricesHolder>();
			foreach (DirectMatricesHolder directMatricesHolder in array)
			{
				if (directMatricesHolder.ContainsPosition(x, z))
				{
					return directMatricesHolder;
				}
			}
			return null;
		}

		public static float FindValueAtPosition(string name, float x, float z)
		{
			DirectMatricesHolder directMatricesHolder = FindHolder(x, z);
			if (directMatricesHolder != null)
			{
				return directMatricesHolder.ValueAtPosition(name, x, z);
			}
			return 0f;
		}

		public static float FindValueAtPosition(MapMagicObject mapMagicObject, string name, float x, float z)
		{
			TerrainTile terrainTile = mapMagicObject.tiles.FindByWorldPosition(x, z);
			if (terrainTile == null)
			{
				return 0f;
			}
			DirectMatricesHolder component = terrainTile.ActiveTerrain.GetComponent<DirectMatricesHolder>();
			if (component == null)
			{
				return 0f;
			}
			return component.ValueAtPosition(name, x, z);
		}

		public void OnBeforeSerialize()
		{
			(serNames, serMaps) = maps.Serialize();
		}

		public void OnAfterDeserialize()
		{
			maps.Deserialize(serNames, serMaps);
		}
	}
}
