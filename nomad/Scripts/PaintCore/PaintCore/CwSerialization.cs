using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	public static class CwSerialization
	{
		public static Dictionary<int, Material> HashToMaterial = new Dictionary<int, Material>();

		public static Dictionary<Material, int> MaterialToHash = new Dictionary<Material, int>();

		public static Dictionary<CwHash, CwModel> HashToModel = new Dictionary<CwHash, CwModel>();

		public static Dictionary<CwModel, CwHash> ModelToHash = new Dictionary<CwModel, CwHash>();

		public static Dictionary<CwHash, Texture> HashToTexture = new Dictionary<CwHash, Texture>();

		public static Dictionary<Texture, CwHash> TextureToHash = new Dictionary<Texture, CwHash>();

		public static Dictionary<CwHash, CwPaintableTexture> HashToPaintableTexture = new Dictionary<CwHash, CwPaintableTexture>();

		public static Dictionary<CwPaintableTexture, CwHash> PaintableTextureToHash = new Dictionary<CwPaintableTexture, CwHash>();

		public static void TryRegister(CwPaintableTexture paintableTexture, CwHash hash)
		{
			TryRegister(paintableTexture, hash, HashToPaintableTexture, PaintableTextureToHash);
		}

		public static void TryRegister(CwModel model, CwHash hash)
		{
			TryRegister(model, model.Hash, HashToModel, ModelToHash);
		}

		public static void TryRegister(Texture texture, CwHash hash)
		{
			TryRegister(texture, hash, HashToTexture, TextureToHash);
		}

		public static void TryRegister<T>(T obj, CwHash hash, Dictionary<CwHash, T> hashToObj, Dictionary<T, CwHash> objToHash) where T : UnityEngine.Object
		{
			if (objToHash.TryGetValue(obj, out var value))
			{
				if ((int)value == (int)hash)
				{
					return;
				}
				objToHash.Remove(obj);
				hashToObj.Remove(value);
			}
			if ((int)hash != (int)default(CwHash))
			{
				objToHash.Add(obj, hash);
				hashToObj.Add(hash, obj);
			}
		}

		public static int TryRegister(Material material)
		{
			int stableStringHash = GetStableStringHash(material.name);
			if (HashToMaterial.ContainsKey(stableStringHash))
			{
				throw new Exception("You're trying to register the " + material?.ToString() + " Material, but you've already registered the " + HashToMaterial[stableStringHash]?.ToString() + " Material with the same hash.");
			}
			MaterialToHash.Add(material, stableStringHash);
			HashToMaterial.Add(stableStringHash, material);
			return stableStringHash;
		}

		private static int GetStableStringHash(string s)
		{
			int num = 23;
			foreach (char c in s)
			{
				num = num * 31 + c;
			}
			return num;
		}
	}
}
