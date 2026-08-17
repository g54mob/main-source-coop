using UnityEngine;

[CreateAssetMenu(fileName = "AudioObject", menuName = "Audio/AudioObject", order = 1)]
public class AudioObjectManager : ScriptableObject
{
	public string audioName;

	public Sound[] variables;
}
