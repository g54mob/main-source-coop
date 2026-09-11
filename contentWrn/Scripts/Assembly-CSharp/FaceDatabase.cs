using UnityEngine;
using Zorro.Core;

[CreateAssetMenu(menuName = "Database/FaceDatabase", fileName = "FaceDatabase")]
public class FaceDatabase : SingletonAsset<FaceDatabase>
{
	public Color[] FaceColors;

	public string[] Faces;

	public static byte GetRandomFaceIndex()
	{
		return (byte)Random.Range(0, SingletonAsset<FaceDatabase>.Instance.Faces.Length);
	}

	public static byte GetRandomColorIndex()
	{
		return (byte)Random.Range(0, SingletonAsset<FaceDatabase>.Instance.FaceColors.Length);
	}

	public static string GetFace(byte index)
	{
		return SingletonAsset<FaceDatabase>.Instance.Faces[index];
	}

	public static Color GetColor(byte index)
	{
		return SingletonAsset<FaceDatabase>.Instance.FaceColors[index];
	}
}
