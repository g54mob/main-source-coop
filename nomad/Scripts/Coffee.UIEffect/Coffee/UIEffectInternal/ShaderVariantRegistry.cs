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

		[SerializeField]
		private List<StringPair> m_OptionalShaders = new List<StringPair>();

		[SerializeField]
		internal ShaderVariantCollection m_Asset;

		public Func<string, bool> onShaderRequested;

		public ShaderVariantCollection shaderVariantCollection => m_Asset;

		public Shader FindOptionalShader(Shader shader, string requiredName, string format, string defaultOptionalShaderName)
		{
			if (!shader)
			{
				return null;
			}
			int instanceID = shader.GetInstanceID();
			if (_cachedOptionalShaders.TryGetValue(instanceID, out var value))
			{
				return Shader.Find(value);
			}
			string name = shader.name;
			if (name.Contains(requiredName))
			{
				_cachedOptionalShaders[instanceID] = name;
				return shader;
			}
			int count = m_OptionalShaders.Count;
			Shader shader2;
			for (int i = 0; i < count; i++)
			{
				StringPair stringPair = m_OptionalShaders[i];
				if (!(stringPair.key != name))
				{
					shader2 = Shader.Find(stringPair.value);
					if ((bool)shader2)
					{
						_cachedOptionalShaders[instanceID] = stringPair.value;
						return shader2;
					}
				}
			}
			value = string.Format(format, name);
			shader2 = Shader.Find(value);
			if ((bool)shader2)
			{
				_cachedOptionalShaders[instanceID] = value;
				return shader2;
			}
			_cachedOptionalShaders[instanceID] = defaultOptionalShaderName;
			return Shader.Find(defaultOptionalShaderName);
		}
	}
}
