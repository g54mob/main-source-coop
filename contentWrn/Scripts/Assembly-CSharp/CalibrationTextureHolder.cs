using UnityEngine;
using Zorro.Core;

[CreateAssetMenu(fileName = "CalibrationTextureHolder", menuName = "Zorro/CalibrationTextureHolder")]
public class CalibrationTextureHolder : SingletonAsset<CalibrationTextureHolder>
{
	public Texture2D[] calibrationTextures;

	public static Texture2D GetTexture(BrightnessSetting brightnessSetting)
	{
		int num = Mathf.RoundToInt(Mathf.InverseLerp(-1f, 1f, brightnessSetting.Value) * (float)(SingletonAsset<CalibrationTextureHolder>.Instance.calibrationTextures.Length - 1));
		return SingletonAsset<CalibrationTextureHolder>.Instance.calibrationTextures[num];
	}
}
