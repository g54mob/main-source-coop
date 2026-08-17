using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
	public void DeleteSaves()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
}
