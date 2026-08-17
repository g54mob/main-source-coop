using System;
using UnityEngine;

namespace EvilCore.Particles
{
	[Serializable]
	public struct ParticleKey
	{
		[SerializeField]
		private string key;

		public string Key => key;

		public bool IsValid => !string.IsNullOrEmpty(key);

		public ParticleKey(string key)
		{
			this.key = key;
		}

		public static implicit operator string(ParticleKey pk)
		{
			return pk.key;
		}

		public override string ToString()
		{
			return key ?? string.Empty;
		}
	}
}
