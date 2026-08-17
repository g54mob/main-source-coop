using UnityEngine;

namespace Enviro
{
	public class EnviroWeatherTypeCreation
	{
		public static EnviroWeatherType CreateMyAsset()
		{
			return ScriptableObject.CreateInstance<EnviroWeatherType>();
		}

		public static GameObject GetAssetPrefab(string name)
		{
			return null;
		}

		public static Cubemap GetAssetCubemap(string name)
		{
			return null;
		}

		public static Texture GetAssetTexture(string name)
		{
			return null;
		}

		public static Gradient CreateGradient()
		{
			Gradient gradient = new Gradient();
			GradientColorKey[] array = new GradientColorKey[2];
			GradientAlphaKey[] array2 = new GradientAlphaKey[2];
			array[0].color = Color.white;
			array[0].time = 0f;
			array[1].color = Color.white;
			array[1].time = 0f;
			array2[0].alpha = 0f;
			array2[0].time = 0f;
			array2[1].alpha = 0f;
			array2[1].time = 1f;
			gradient.SetKeys(array, array2);
			return gradient;
		}

		public static Color GetColor(string hex)
		{
			Color color = default(Color);
			ColorUtility.TryParseHtmlString(hex, out color);
			return color;
		}

		public static Keyframe CreateKey(float value, float time)
		{
			return new Keyframe
			{
				value = value,
				time = time
			};
		}

		public static Keyframe CreateKey(float value, float time, float inTangent, float outTangent)
		{
			return new Keyframe
			{
				value = value,
				time = time,
				inTangent = inTangent,
				outTangent = outTangent
			};
		}
	}
}
