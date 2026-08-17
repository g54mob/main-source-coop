using UnityEngine;

namespace Enviro
{
	public class EnviroQualityCreation
	{
		public static EnviroQuality CreateMyAsset()
		{
			return ScriptableObject.CreateInstance<EnviroQuality>();
		}
	}
}
