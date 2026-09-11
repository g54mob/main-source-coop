using System;

[Serializable]
public class Emote
{
	public string displayName;

	public string animationName;

	public bool unequip = true;

	public float emoteLength = 2f;

	public float emoteMovementSpeed = 0.1f;

	public bool emoteAllowRotate = true;

	public float emoteBaseScore;

	public float emoteScoreMultiplier = 1.5f;

	public string[] comments;
}
