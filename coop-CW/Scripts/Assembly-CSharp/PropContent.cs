using UnityEngine;

[CreateAssetMenu(fileName = "PropContent", menuName = "ContentEvent/PropContent", order = 0)]
public class PropContent : ScriptableObject
{
	[HideInInspector]
	public bool isArtifact;

	public float contentValue;

	public ushort id;

	public string[] comments;

	public PropContentEvent GetContentEvent()
	{
		return new PropContentEvent
		{
			content = this
		};
	}
}
