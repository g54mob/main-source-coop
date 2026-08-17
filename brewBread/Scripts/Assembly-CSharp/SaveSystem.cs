using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
	private const string SAVEPATH = "/save.scs";

	public static void SaveData(bool single)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = new FileStream(Application.persistentDataPath + "/save.scs", FileMode.Create);
		binaryFormatter.Serialize(fileStream, new SaveData());
		fileStream.Close();
		if (!single)
		{
			PlayerPrefs.SetFloat("BreadX", StaticInstance<Bread>.Instance.PenguinTransform.position.x);
			PlayerPrefs.SetFloat("BreadY", StaticInstance<Bread>.Instance.PenguinTransform.position.y);
			PlayerPrefs.SetFloat("FredX", StaticInstance<Fred>.Instance.PenguinTransform.position.x);
			PlayerPrefs.SetFloat("FredY", StaticInstance<Fred>.Instance.PenguinTransform.position.y);
		}
		else
		{
			PlayerPrefs.SetFloat("GregX", StaticInstance<Bread>.Instance.PenguinTransform.position.x);
			PlayerPrefs.SetFloat("GregY", StaticInstance<Bread>.Instance.PenguinTransform.position.y);
			PlayerPrefs.SetFloat("JeffX", StaticInstance<Fred>.Instance.PenguinTransform.position.x);
			PlayerPrefs.SetFloat("JeffY", StaticInstance<Fred>.Instance.PenguinTransform.position.y);
		}
	}

	public static SaveData LoadData(bool single)
	{
		string path = Application.persistentDataPath + "/save.scs";
		Vector2 vector = default(Vector2);
		if (single)
		{
			vector = new Vector2(PlayerPrefs.GetFloat("GregX"), PlayerPrefs.GetFloat("GregY"));
			StaticInstance<Bread>.Instance.PenguinTransform.position = vector;
			vector = new Vector2(PlayerPrefs.GetFloat("JeffX"), PlayerPrefs.GetFloat("JeffY"));
			StaticInstance<Fred>.Instance.PenguinTransform.position = vector;
		}
		else
		{
			vector = new Vector2(PlayerPrefs.GetFloat("BreadX"), PlayerPrefs.GetFloat("BreadY"));
			StaticInstance<Bread>.Instance.PenguinTransform.position = vector;
			vector = new Vector2(PlayerPrefs.GetFloat("FredX"), PlayerPrefs.GetFloat("FredY"));
			StaticInstance<Fred>.Instance.PenguinTransform.position = vector;
		}
		if (File.Exists(path))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = new FileStream(path, FileMode.Open);
			SaveData result = binaryFormatter.Deserialize(fileStream) as SaveData;
			fileStream.Close();
			return result;
		}
		Debug.Log("Couldn't load Saved Data. No file found with the name: /save.scs");
		return null;
	}

	public static bool FileExist()
	{
		return File.Exists(Application.persistentDataPath + "/save.scs");
	}
}
