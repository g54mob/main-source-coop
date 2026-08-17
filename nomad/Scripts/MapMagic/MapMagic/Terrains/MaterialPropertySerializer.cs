using System;
using Den.Tools;
using UnityEngine;

namespace MapMagic.Terrains
{
	[ExecuteInEditMode]
	public class MaterialPropertySerializer : MonoBehaviour
	{
		[SerializeField]
		private Terrain terrain;

		[SerializeField]
		private string[] names = new string[0];

		[SerializeField]
		private Texture2D[] textures = new Texture2D[0];

		[NonSerialized]
		private MaterialPropertyBlock matProps;

		public bool updateVisibility;

		public void Start()
		{
			if (!(terrain == null))
			{
				matProps = new MaterialPropertyBlock();
				for (int i = 0; i < textures.Length; i++)
				{
					matProps.SetTexture(names[i], textures[i]);
				}
				terrain.SetSplatMaterialPropertyBlock(matProps);
			}
		}

		public void OnDrawGizmos()
		{
			if (updateVisibility)
			{
				if (terrain.enabled)
				{
					terrain.enabled = false;
					return;
				}
				terrain.enabled = true;
				updateVisibility = false;
			}
		}

		public void SetTexture(string name, Texture2D texture)
		{
			if (terrain == null)
			{
				terrain = GetComponent<Terrain>();
			}
			if (matProps == null)
			{
				matProps = new MaterialPropertyBlock();
			}
			matProps.SetTexture(name, texture);
			int num = names.Find(name);
			if (num < 0)
			{
				ArrayTools.Add(ref names, name);
				ArrayTools.Add(ref textures, texture);
			}
			else
			{
				textures[num] = texture;
			}
		}

		public Texture2D GetTexture(string name)
		{
			if (name == null)
			{
				return null;
			}
			if (terrain == null)
			{
				terrain = GetComponent<Terrain>();
			}
			if (matProps == null)
			{
				matProps = new MaterialPropertyBlock();
			}
			int num = names.Find(name);
			if (num < 0)
			{
				return null;
			}
			return textures[num];
		}

		public void Apply()
		{
			terrain.SetSplatMaterialPropertyBlock(matProps);
		}
	}
}
