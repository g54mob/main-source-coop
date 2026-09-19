using UnityEngine;

namespace INab.BetterFog.Runtime
{
	public interface IGradientTextureForEditor
	{
		void CreateTexture();

		Texture2D GetTexture();

		void LoadExisitingTexture();
	}
}
