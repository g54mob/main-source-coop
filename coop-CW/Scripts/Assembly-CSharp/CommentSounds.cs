using TMPro;
using UnityEngine;

public class CommentSounds : MonoBehaviour
{
	public GameObject playVideo;

	public Transform commentParent;

	public SFX_Instance[] viewSound;

	public SFX_Instance[] commentSfx;

	private int childCount;

	public TextMeshProUGUI viewCount;

	private bool t;

	private void Update()
	{
		if (childCount != commentParent.childCount)
		{
			for (int i = 0; i < commentSfx.Length; i++)
			{
				commentSfx[i].Play(base.transform.position);
			}
		}
		childCount = commentParent.childCount;
	}
}
