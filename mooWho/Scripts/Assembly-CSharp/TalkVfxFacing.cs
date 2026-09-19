using UnityEngine;

public class TalkVfxFacing : MonoBehaviour
{
	public Transform facingReference;

	public Quaternion localCorrection = Quaternion.identity;

	private void LateUpdate()
	{
		if (!(facingReference == null))
		{
			base.transform.rotation = facingReference.rotation * localCorrection;
		}
	}
}
