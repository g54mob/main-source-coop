using System;
using Den.Tools;
using MapMagic.Core;
using UnityEngine;

namespace MapMagic.Terrains
{
	[ExecuteInEditMode]
	public class DirectTexturesHolder : MonoBehaviour, ISerializationCallbackReceiver
	{
		public Vector2D position;

		public Vector2D size;

		[NonSerialized]
		public DictionaryOrdered<string, Texture2D> textures = new DictionaryOrdered<string, Texture2D>();

		[SerializeField]
		private string[] serNames;

		[SerializeField]
		private Texture2D[] serTextures;

		public Texture2D this[string name]
		{
			get
			{
				if (textures.TryGetValue(name, out var value))
				{
					return value;
				}
				return null;
			}
		}

		public bool ContainsPosition(float x, float z)
		{
			if (x > position.x && x < position.x + size.x && z > position.z)
			{
				return z < position.z + size.z;
			}
			return false;
		}

		public static DirectTexturesHolder FindHolder(float x, float z)
		{
			DirectTexturesHolder[] array = UnityEngine.Object.FindObjectsOfType<DirectTexturesHolder>();
			foreach (DirectTexturesHolder directTexturesHolder in array)
			{
				if (directTexturesHolder.ContainsPosition(x, z))
				{
					return directTexturesHolder;
				}
			}
			return null;
		}

		public static Texture2D FindTexture(string name, float x, float z)
		{
			DirectTexturesHolder directTexturesHolder = FindHolder(x, z);
			if (directTexturesHolder != null)
			{
				return directTexturesHolder[name];
			}
			return null;
		}

		public static Texture2D FindTexture(MapMagicObject mapMagicObject, string name, float x, float z)
		{
			TerrainTile terrainTile = mapMagicObject.tiles.FindByWorldPosition(x, z);
			if (terrainTile == null)
			{
				return null;
			}
			DirectTexturesHolder component = terrainTile.ActiveTerrain.GetComponent<DirectTexturesHolder>();
			if (component == null)
			{
				return null;
			}
			return component[name];
		}

		public void OnBeforeSerialize()
		{
			(serNames, serTextures) = textures.Serialize();
		}

		public void OnAfterDeserialize()
		{
			textures.Deserialize(serNames, serTextures);
		}
	}
}
