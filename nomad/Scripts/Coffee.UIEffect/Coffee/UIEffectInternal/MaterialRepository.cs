using System;
using UnityEngine;

namespace Coffee.UIEffectInternal
{
	internal static class MaterialRepository
	{
		private static readonly ObjectRepository<Material> s_Repository = new ObjectRepository<Material>();

		public static int count => s_Repository.count;

		public static bool Valid(Hash128 hash, Material material)
		{
			return s_Repository.Valid(hash, material);
		}

		public static void Get(Hash128 hash, ref Material material, Func<Material> onCreate)
		{
			s_Repository.Get(hash, ref material, onCreate);
		}

		public static void Get(Hash128 hash, ref Material material, string shaderName)
		{
			s_Repository.Get(hash, ref material, (string x) => new Material(Shader.Find(x))
			{
				hideFlags = (HideFlags.DontSave | HideFlags.NotEditable)
			}, shaderName);
		}

		public static void Get(Hash128 hash, ref Material material, string shaderName, string[] keywords)
		{
			s_Repository.Get(hash, ref material, ((string shaderName, string[] keywords) x) => new Material(Shader.Find(x.shaderName))
			{
				hideFlags = (HideFlags.DontSave | HideFlags.NotEditable),
				shaderKeywords = x.keywords
			}, (shaderName, keywords));
		}

		public static void Get<T>(Hash128 hash, ref Material material, Func<T, Material> onCreate, T source)
		{
			s_Repository.Get(hash, ref material, onCreate, source);
		}

		public static void Release(ref Material material)
		{
			s_Repository.Release(ref material);
		}
	}
}
