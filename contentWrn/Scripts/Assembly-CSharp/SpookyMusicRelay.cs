using UnityEngine;

public class SpookyMusicRelay : MonoBehaviour
{
	public static SpookyMusicRelay instance;

	private SpookyMusicHandler[] musics;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		musics = GetComponentsInChildren<SpookyMusicHandler>();
	}

	internal void AddDanger(float spookAmount, int jumpScareLevel)
	{
		for (int i = 0; i < musics.Length; i++)
		{
			musics[i].AddDanger(spookAmount, jumpScareLevel);
		}
	}
}
