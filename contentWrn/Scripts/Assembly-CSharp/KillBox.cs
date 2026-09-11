using UnityEngine;

public class KillBox : MonoBehaviour
{
	public SFX_Instance killSound;

	public void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			return;
		}
		Player componentInParent = other.GetComponentInParent<Player>();
		if (componentInParent?.refs?.view == null)
		{
			Debug.LogError(other.gameObject.name + ": Missing Player References");
		}
		else
		{
			if (!componentInParent.refs.view.IsMine || componentInParent.ai)
			{
				return;
			}
			Bodypart componentInParent2 = other.GetComponentInParent<Bodypart>();
			if ((bool)componentInParent2 && componentInParent2.bodypartType != BodypartType.Item)
			{
				if ((bool)killSound)
				{
					killSound.Play(base.transform.position);
				}
				componentInParent.Die();
			}
		}
	}
}
