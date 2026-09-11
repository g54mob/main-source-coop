using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
	public SFX_Instance sound;

	public void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			return;
		}
		Player componentInParent = other.GetComponentInParent<Player>();
		if (componentInParent.refs.view.IsMine && !componentInParent.ai)
		{
			Bodypart componentInParent2 = other.GetComponentInParent<Bodypart>();
			if ((bool)componentInParent2 && componentInParent2.bodypartType != BodypartType.Item && componentInParent2.bodypartType == BodypartType.Hip)
			{
				MetaProgressionHandler.CheckIfUnlockedAllHats();
				sound.Play(base.transform.position);
			}
		}
	}
}
