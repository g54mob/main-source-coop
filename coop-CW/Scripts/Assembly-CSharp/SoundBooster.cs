using UnityEngine;

public class SoundBooster : MonoBehaviour
{
	public float maxRange = 3f;

	public float minRange = 2f;

	private void Start()
	{
		SFX_Player.instance.soundBoosters.Add(this);
	}

	private void OnDestroy()
	{
		SFX_Player.instance.soundBoosters.Remove(this);
	}
}
