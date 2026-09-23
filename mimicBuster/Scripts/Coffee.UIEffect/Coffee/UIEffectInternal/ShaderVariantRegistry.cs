using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIEffectInternal
{
	[Serializable]
	public sealed class ShaderVariantRegistry
	{
		[Serializable]
		internal class StringPair : IEquatable<StringPair>
		{
			public string key;

			public string value;

			public bool Equals(StringPair other)
			{
				if (other != null && key == other.key)
				{
					return value == other.value;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj is StringPair other)
				{
					return Equals(other);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (((key != null) ? key.GetHashCode() : 0) * 397) ^ ((value != null) ? value.GetHashCode() : 0);
			}
		}

		private Dictionary<int, string> _cachedOptionalShaders = new Dictionary<int, string>();

		private Dictionary<string, Shader> _shaderByName;

		[SerializeField]
		private List<StringPair> m_OptionalShaders = new List<StringPair>();

		[SerializeField]
		internal ShaderVariantCollection m_Asset;

		[SerializeField]
		private List<Shader> m_RegisteredShaders = new List<Shader>();

		public Func<string, bool> onShaderRequested;

		public ShaderVariantCollection shaderVariantCollection => m_Asset;

		public void InitializeShaderLookup()
		{
			int count = m_RegisteredShaders.Count;
			if (count == 0)
			{
				return;
			}
			if (_shaderByName == null)
			{
				_shaderByName = new Dictionary<string, Shader>(count);
			}
			else
			{
				_shaderByName.Clear();
			}
			for (int i = 0; i < count; i++)
			{
				Shader shader = m_RegisteredShaders[i];
				if (shader != null)
				{
					_shaderByName[shader.name] = shader;
				}
			}
		}

		public Shader FindShaderByName(string name)
		{
			if (_shaderByName != null && _shaderByName.TryGetValue(name, out var value) && value != null)
			{
				return value;
			}
			return Shader.Find(name);
		}

		public Shader FindOptionalShader(Shader shader, string requiredName, string format, string defaultOptionalShaderName)
		{
			if (shader == null)
			{
				return null;
			}
			int hashCode = shader.GetHashCode();
			if (_cachedOptionalShaders.TryGetValue(hashCode, out var value))
			{
				return FindShaderByName(value);
			}
			string name = shader.name;
			if (name.Contains(requiredName))
			{
				_cachedOptionalShaders[hashCode] = name;
				return shader;
			}
			int count = m_OptionalShaders.Count;
			Shader shader2;
			for (int i = 0; i < count; i++)
			{
				StringPair stringPair = m_OptionalShaders[i];
				if (!(stringPair.key != name))
				{
					shader2 = FindShaderByName(stringPair.value);
					if (!(shader2 == null))
					{
						_cachedOptionalShaders[hashCode] = stringPair.value;
						return shader2;
					}
				}
			}
			value = string.Format(format, name);
			shader2 = FindShaderByName(value);
			if (shader2 != null)
			{
				_cachedOptionalShaders[hashCode] = value;
				return shader2;
			}
			_cachedOptionalShaders[hashCode] = defaultOptionalShaderName;
			return FindShaderByName(defaultOptionalShaderName);
		}
	}
}
